using BinanceFuturesConnector.Core.Overrides_classes;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data;
using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using HQConnector.Core.Classes.Credentials;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Dto.DTO.Account;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebSocket4Net;

namespace BinanceFuturesConnector.Core
{
    
    public class AccountUpdater 
    {
        protected class PositionPriceUpdate
        {
            public string Symbol { get; set; }

            public decimal Price { get; set; }

            public PositionPriceUpdate(string symbol, decimal price)
            {
                Symbol = symbol;
                Price = price;
            }
        }


        #region Ctor
        IConnectorCredentials ConnectorCredentials;

        public event EventHandler<MessageExchangeResponse<object>> AcccountUpdated;

        public bool IsPositionUpdate { get; set; }

        public bool IsBalanceUpdate { get; set; }

      

        int positionInterval { get; set; }

        int balanceInterval { get; set; }

        List<Position> _positions = new List<Position>();

        List<Balance> _balances = new List<Balance>();       

        static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        static SemaphoreSlim _semaphore2 = new SemaphoreSlim(1);

        BinanceFuturesSocketClient markpricedata;

        BinanceFuturesSender restsender;

        System.Timers.Timer timer;

        public AccountUpdater(IConnectorCredentials connectorCredentials, BinanceFuturesSender sender)
        {
            ConnectorCredentials = connectorCredentials;
            markpricedata = new BinanceFuturesSocketClient("wss://fstream.binance.com/stream");
            markpricedata.MessageReceived += OnMessageReceived;
            restsender = sender;
        }

        #endregion

        public async Task Start(string eventName, IEnumerable<object> data, int updateInterval)
        {
            if (eventName == "positions")
            {
                if (IsPositionUpdate == false)
                {
                    IsPositionUpdate = true;
                    positionInterval = updateInterval;
                    if (data != null)
                    {
                        var currentpositions = data as List<Position>;
                        _positions.AddRange(currentpositions.Where(p => p.Size != 0));
                        foreach (var position in _positions)
                        {
                            if (positionInterval == 1)
                            {
                                await markpricedata.Subscribe(new List<string>() { "SUBSCRIBE" }, new Dictionary<string, object>
                                {
                                    ["params"] = $"{position.Pair.ToLower()}@markPrice" + "@" + positionInterval + "s"
                                });
                            }
                            else
                            {
                               await  markpricedata.Subscribe(new List<string>() { "SUBSCRIBE" }, new Dictionary<string, object>
                                {
                                    ["params"] = $"{position.Pair.ToLower()}@markPrice"
                                });
                            }
                        }
                        if (_positions != null && _positions.Count != 0) PositionUpdate(null, null);
                    }
                }

            }
            if (eventName == "balances")
            {
                if (IsBalanceUpdate == false)
                {
                    IsBalanceUpdate = true;
                    balanceInterval = updateInterval;
                    timer = new System.Timers.Timer();
                    timer.Interval = new TimeSpan(00, 00, balanceInterval).TotalMilliseconds;
                    timer.Elapsed += BalanceTimerUpdate;
                    timer.AutoReset = true;
                    timer.Enabled = true;                   
                    await BalanceUpdate(data as List<Balance>, false);
                }
            }
        }

        public async Task Stop(string eventName)
        {
            if (eventName == "positions")
            {

                IsPositionUpdate = false;
                markpricedata.Dispose();
                _positions.Clear();
            }
            if (eventName == "balances")
            {
                if (IsBalanceUpdate)
                {
                    IsBalanceUpdate = false;
                    timer.Enabled = false;
                    _balances.Clear();
                }
            }
         
        }

        private async Task<long> CalcServerTime()
        {
            try
            {

                var time = await restsender.SendRequestAsync<BinanceFuturesServerTime>("/fapi/v1/time", HttpMethods.GET,
                    new Dictionary<string, object>
                    {

                    }, Signed.No, ContentType.Query);
                if (time == null)
                {
                    var restsend = new BinanceFuturesSender("https://api3.binance.com/", new ConnectorCredentials("", HQConnector.Dto.DTO.Enums.Exchange.Exchange.BinanceFutures), 150, 300);
                    time = await restsend.SendRequestAsync<BinanceFuturesServerTime>("api/v3/time", HttpMethods.GET,
                  new Dictionary<string, object>
                  {

                  }, Signed.No, ContentType.Query);
                }
                return time?.Data.ServerTime ?? 0;
            }
            catch (Exception ex)
            {

            }

            return 0;
        }
        private async void BalanceTimerUpdate(object sender, ElapsedEventArgs e)
        {
            try
            {
                var time = await CalcServerTime();
                if (time != 0)
                {
                    var balanceresponse = await restsender.SendRequestAsync<BinanceFuturesBalanceInfo>("/fapi/v1/account", HttpMethods.GET, new Dictionary<string, object>
                    {
                        ["timestamp"] = time,
                        ["recvWindow"] = 30000
                    }, Signed.Yes, ContentType.Query);

                    if (balanceresponse != null && balanceresponse.ErrorResult.IsSuccess)
                    {

                        var dictionaryBalance = new List<Balance>();

                        foreach (var balance in balanceresponse.Data.Balances)
                        {
                            var balanceBase = new Balance
                            {

                                Exchange = ConnectorCredentials.Exchange,
                                Account = ConnectorCredentials.ConnectorName,
                                SymbolCurrency = balance.Asset,
                                WalletBalance = Convert.ToDecimal(balance.WalletBalance, CultureInfo.InvariantCulture),
                                MarginBalance = Convert.ToDecimal(balance.MarginBalance, CultureInfo.InvariantCulture),
                                AvailBalance = Convert.ToDecimal(balance.MaxWithdrawAmount, CultureInfo.InvariantCulture),

                            };
                            dictionaryBalance.Add(balanceBase);
                        }

                        if (dictionaryBalance != null && dictionaryBalance.Count != 0)
                        {
                            await BalanceUpdate(dictionaryBalance, false);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task BalanceUpdate(List<Balance> balances,bool updateneed)
        {
            _semaphore2.Wait();
            if (updateneed)
            {
                foreach (var balance in balances)
                {
                    _balances.First(p => p.SymbolCurrency == balance.SymbolCurrency).WalletBalance = balance.WalletBalance;
                    _balances.First(p => p.SymbolCurrency == balance.SymbolCurrency).AvailBalance  = balance.AvailBalance;
                }
                if (_balances != null && balances.Count != 0)
                    AcccountUpdated?.Invoke(this, new MessageExchangeResponse<object>(_balances, new SuccessResponse(), ConnectorCredentials.Exchange));
            }
            else
            {
                try
                {
                    _balances = balances;
                }
                catch (Exception ex)
                {

                }
                if (_balances != null && balances.Count != 0)
              AcccountUpdated?.Invoke(this, new MessageExchangeResponse<object>(_balances, new SuccessResponse(), ConnectorCredentials.Exchange));
            }
            _semaphore2.Release();
        }
       
        private async Task PositionUpdate(List<Position> positions, PositionPriceUpdate posprice)
        {
            _semaphore.Wait();
            if (positions != null)
            {
                foreach (var position in positions)
                {
                    try
                    {
                        if (_positions.Any(p => p.Pair == position.Pair && p.PositionSide == position.PositionSide))
                        {
                          
                            _positions.First(p => p.Pair == position.Pair && p.PositionSide == position.PositionSide).Size = position.Size;
                            _positions.First(p => p.Pair == position.Pair && p.PositionSide == position.PositionSide).AveragePrice = position.AveragePrice;
                            _positions.First(p => p.Pair == position.Pair && p.PositionSide == position.PositionSide).PnL = position.PnL;
                            if (position.Size == 0)
                            {
                                if (positionInterval == 1)
                                {
                                    await markpricedata.Subscribe(new List<string>() { "UNSUBSCRIBE" }, new Dictionary<string, object>
                                    {
                                        ["params"] = $"{position.Pair.ToLower()}@markPrice" + "@" + positionInterval + "s"
                                    }, true);
                                }
                                else
                                {
                                    await markpricedata.Subscribe(new List<string>() { "UNSUBSCRIBE" }, new Dictionary<string, object>
                                    {
                                        ["params"] = $"{position.Pair.ToLower()}@markPrice"
                                    }, true);
                                }

                            }

                        }
                        else
                        {
                            _positions.Add(position);
                            if (position.Size != 0)
                            {
                                if (positionInterval == 1)
                                {
                                    await markpricedata.Subscribe(new List<string>() { "SUBSCRIBE" }, new Dictionary<string, object>
                                    {
                                        ["params"] = $"{position.Pair.ToLower()}@markPrice" + "@" + positionInterval + "s"
                                    });
                                    
                                }
                                else
                                {
                                    await markpricedata.Subscribe(new List<string>() { "SUBSCRIBE" }, new Dictionary<string, object>
                                    {
                                        ["params"] = $"{position.Pair.ToLower()}@markPrice"
                                    });
                                    
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            if (posprice != null && !String.IsNullOrEmpty(posprice.Symbol) && _positions.Any(p => p.Pair == posprice.Symbol))
            {             
                
                foreach (var pos in _positions.Where(p => p.Pair == posprice.Symbol).ToList())
                {
                    if (pos.Size != 0)
                    {
                        try
                        {
                            _positions.First(p => p.Pair == posprice.Symbol && p.PositionSide == pos.PositionSide).PnL = UpdatePnl(posprice);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                
             
            }

            if (_positions.Count != 0)
            {
                AcccountUpdated?.Invoke(this, new MessageExchangeResponse<object>(_positions, new SuccessResponse(), ConnectorCredentials.Exchange));
            }
            _positions.RemoveAll(p => p.Size == 0);
            _semaphore.Release();
        }

      
        private decimal UpdatePnl(PositionPriceUpdate posprice)
        {
            decimal pnl = 0;
            try
            {
                var openprice = _positions.First(p => p.Pair == posprice.Symbol).AveragePrice;
                var size = _positions.First(p => p.Pair == posprice.Symbol).Size;
                decimal delta = openprice - posprice.Price;
                if (size > 0)
                {
                    delta = posprice.Price - openprice;
                }
                pnl = Math.Abs(size) * delta;

            }
            catch (Exception ex)
            {

            }
            return pnl;
        }

        #region Socket 

        public async Task AccountUpdate(AccountUpdate update)
        {
            if (IsPositionUpdate)
            {
                await PositionUpdate(update.Positions.ToList(), null);
            }
            if (IsBalanceUpdate)
            {
                await BalanceUpdate(update.Balances.ToList(), true);
            }
        }

        private void OnMessageReceived(object sender, EventArgs e)
        {
            try
            {
                var markPrice = JsonConvert.DeserializeObject<BinanceFuturesMarkPrice>((e as MessageReceivedEventArgs).Message); ;
                PositionUpdate(null, new PositionPriceUpdate(markPrice.Symbol, Convert.ToDecimal(markPrice.MarkPrice, CultureInfo.InvariantCulture)));
            }
            catch (Exception ex)
            {

            }
        }


        #endregion


    }
}
