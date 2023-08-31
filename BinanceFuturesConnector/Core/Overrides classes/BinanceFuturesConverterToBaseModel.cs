using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BinanceFuturesConnector.Temp_Model.Rest;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data.Order;
using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using BinanceFuturesConnector.Util.Converters;
using HQConnector.Core.Interfaces.Converters;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Dto.DTO.Account;
using HQConnector.Dto.DTO.Any_Collection;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.Commission;
using HQConnector.Dto.DTO.Commission.Model;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.MyTrade;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Enums.Position;
using HQConnector.Dto.DTO.Enums.Symbol;
using HQConnector.Dto.DTO.Leverage;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;
using HQConnector.Dto.DTO.Symbol;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;
using Newtonsoft.Json;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
   public class BinanceFuturesConverterToBaseModel : IConverterToBaseModel
    {
        #region Prop and ctor
        public IConnectorCredentials Credentials { get; }

        public BinanceFuturesConverterToBaseModel(IConnectorCredentials credentials)
        {
            Credentials = credentials;
            IdOrderBook = new Dictionary<object, decimal>();
        }

        #endregion

        #region Rest Public Data
        public long ConvertServerTime<TTempModel>(TTempModel serverTime)
        {
            var time = serverTime as BinanceFuturesServerTime;
            return time?.ServerTime ?? 0;
        }

        public IExchangeResultResponse<IEnumerable<Symbol>> ConverterSymbols<TTempModel>(TTempModel symbols)
        {
            var symbolsResponse = symbols as IExchangeResultResponse<BinanceFuturesSymbolResponse>;
            var symbolList = new List<Symbol>();
            if (symbolsResponse != null && symbolsResponse.ErrorResult.IsSuccess)
            {
                foreach (var item in symbolsResponse.Data.Symbols)
                {
                    var LotFilter = item.Filters.FirstOrDefault(x => x.FilterType == "LOT_SIZE");
                    var PriceFilter = item.Filters.FirstOrDefault(x => x.FilterType == "PRICE_FILTER");
                    symbolList.Add(new Symbol(item.SymbolSymbol, Credentials.Exchange)
                    {                       
                        SymbolType =SymbolType.Futures,
                        IsMargin = false,
                        IsActive = (item.Status == "TRADING") ? true : false,
                        MinSize = Convert.ToDecimal(LotFilter.MinQty, CultureInfo.InvariantCulture),
                        MaxSize = Convert.ToDecimal(LotFilter.MaxQty, CultureInfo.InvariantCulture),
                        MinPrice = Convert.ToDecimal(PriceFilter.MinPrice, CultureInfo.InvariantCulture),
                        MaxPrice = Convert.ToDecimal(PriceFilter.MaxPrice, CultureInfo.InvariantCulture),
                        PriceStep = Convert.ToDecimal(PriceFilter.TickSize, CultureInfo.InvariantCulture),
                        PricePrecission = Convert.ToDecimal(item.PricePrecision,CultureInfo.InvariantCulture),
                        LotPrecission = Convert.ToDecimal(item.QuantityPrecision, CultureInfo.InvariantCulture)
                    });
                }
            }
            return new MessageExchangeResponse<IEnumerable<Symbol>>(symbolList.OrderBy(x => x.SymbolCode).ToArray(),
                symbolsResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Symbol>> ConverterActiveSymbols<TTempModel>(TTempModel symbols)
        {
            var symbolsResponse = symbols as IExchangeResultResponse<BinanceFuturesSymbolResponse>;
            var symbolList = new List<Symbol>();
            if (symbolsResponse != null && symbolsResponse.ErrorResult.IsSuccess)
            {
                foreach (var item in symbolsResponse.Data.Symbols)
                {
                    var LotFilter = item.Filters.FirstOrDefault(x => x.FilterType == "LOT_SIZE");
                    var PriceFilter = item.Filters.FirstOrDefault(x => x.FilterType == "PRICE_FILTER");
                    symbolList.Add(new Symbol(item.SymbolSymbol, Credentials.Exchange)
                    {
                        SymbolType = SymbolType.Futures,
                        IsMargin = false,
                        IsActive = (item.Status == "TRADING") ? true : false,
                        MinSize = Convert.ToDecimal(LotFilter.MinQty, CultureInfo.InvariantCulture),
                        MaxSize = Convert.ToDecimal(LotFilter.MaxQty, CultureInfo.InvariantCulture),
                        MinPrice = Convert.ToDecimal(PriceFilter.MinPrice, CultureInfo.InvariantCulture),
                        MaxPrice = Convert.ToDecimal(PriceFilter.MaxPrice, CultureInfo.InvariantCulture),
                        PriceStep = Convert.ToDecimal(PriceFilter.TickSize, CultureInfo.InvariantCulture),
                        PricePrecission = Convert.ToDecimal(item.PricePrecision, CultureInfo.InvariantCulture)
                    });
                }
            }
            return new MessageExchangeResponse<IEnumerable<Symbol>>(symbolList.Where(p=> p.IsActive == true).OrderBy(x => x.SymbolCode).ToArray(),
                symbolsResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<Ticker> ConverterTicker<TTempModel>(TTempModel ticker, string pair)
        {
            var tickerResponse = ticker as IExchangeResultResponse<BinanceFuturesTicker>;
            var tickerbase = new Ticker();
            if (tickerResponse != null && tickerResponse.ErrorResult.IsSuccess)
            {
                tickerbase = new Ticker
                {
                    Date = DateTimeOffset.Now,
                    Pair = pair,
                    Exchange = Credentials.Exchange,
                    Last = Convert.ToDecimal(tickerResponse.Data.LastPrice, CultureInfo.InvariantCulture),
                    MaxPrice24h = Convert.ToDecimal(tickerResponse.Data.HighPrice, CultureInfo.InvariantCulture),
                    MinPrice24h = Convert.ToDecimal(tickerResponse.Data.LowPrice, CultureInfo.InvariantCulture),
                    Volume24h = Convert.ToDecimal(tickerResponse.Data.Volume, CultureInfo.InvariantCulture),
                    PercentChange = Convert.ToDecimal(tickerResponse.Data.PriceChangePercent, CultureInfo.InvariantCulture),
                };
            }
            return new MessageExchangeResponse<Ticker>(tickerbase, tickerResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<TickerPrice> ConverterTickerPrice<TTempModel>(TTempModel tickerprice, string pair)
        {
            var tickerResponse = tickerprice as IExchangeResultResponse<BinanceFuturesTickerPrice>;
            var tickerbase = new TickerPrice();
            if (tickerResponse != null && tickerResponse.ErrorResult.IsSuccess)
            {
                tickerbase = new TickerPrice
                {
                    Date = DateTimeOffset.Now,
                    Exchange = Credentials.Exchange,
                    Pair = tickerResponse.Data.Symbol,
                    Price = Convert.ToDecimal(tickerResponse.Data.Price, CultureInfo.InvariantCulture)
                };
            }
            return new MessageExchangeResponse<TickerPrice>(tickerbase, tickerResponse.ErrorResult, Credentials.Exchange);
        }

        private readonly char[] trimChars = new char[] { '0' };
        public IExchangeResultResponse<OrderBook> ConverterOrderBook<TTempModel>(TTempModel orderBook, string pair, int depth, int merge)
        {
            var orderBookTemp = orderBook as IExchangeResultResponse<BinanceFuturesOrderBook>;
            var orderBookBase = new OrderBook() { Exchange = Credentials.Exchange };
            if (orderBookTemp != null && orderBookTemp.ErrorResult.IsSuccess)
            {
                var collectionAsk = new OrderCollection<Quote>();

                foreach (var ask in orderBookTemp.Data.Asks)
                {
                    var orderPrice = new Quote
                    {
                        Price = Convert.ToDecimal(ask[0].ToString().TrimEnd(trimChars), CultureInfo.InvariantCulture),
                        Amount = Convert.ToDecimal(ask[1].ToString().TrimEnd(trimChars), CultureInfo.InvariantCulture)
                    };
                    collectionAsk.Add(orderPrice);
                }

                var collectionBid = new OrderCollection<Quote>();
                foreach (var bid in orderBookTemp.Data.Bids)
                {
                    var orderPrice = new Quote
                    {
                        Price = Convert.ToDecimal(bid[0].ToString().TrimEnd(trimChars), CultureInfo.InvariantCulture),
                        Amount = Convert.ToDecimal(bid[1].ToString().TrimEnd(trimChars), CultureInfo.InvariantCulture)
                    };
                    collectionBid.Add(orderPrice);
                }

                orderBookBase.Bids = collectionBid;
                orderBookBase.Asks = collectionAsk;
                orderBookBase.Pair = pair;
                orderBookBase.Depth = depth;
            }

            return new MessageExchangeResponse<OrderBook>(orderBookBase, orderBookTemp.ErrorResult, Credentials.Exchange);
        }

       

        public IExchangeResultResponse<IEnumerable<Candle>> ConverterCandleSeries<TTempModel>(TTempModel candles, string pair)
        {
            var candlesData = candles as IExchangeResultResponse<object[][]>;
            var candleList = new List<Candle>();
            if (candlesData != null && candlesData.ErrorResult.IsSuccess)
            {
                for (var i = 0; i < candlesData.Data.Length; i++)
                {
                    var oneCandle = candlesData.Data[i];
                    candleList.Add(new Candle
                    {
                        Exchange = Credentials.Exchange,
                        Pair = pair,                        
                        OpenTime = DateTimeOffset.FromUnixTimeMilliseconds((long)oneCandle[0]),
                        CloseTime = DateTimeOffset.FromUnixTimeMilliseconds((long)oneCandle[6]),
                        OpenPrice = Convert.ToDecimal(oneCandle[1].ToString(), CultureInfo.InvariantCulture),
                        HighPrice = Convert.ToDecimal(oneCandle[2].ToString(), CultureInfo.InvariantCulture),
                        LowPrice = Convert.ToDecimal(oneCandle[3].ToString(), CultureInfo.InvariantCulture),
                        ClosePrice = Convert.ToDecimal(oneCandle[4].ToString(), CultureInfo.InvariantCulture),
                        TotalVolume = Convert.ToDecimal(oneCandle[5].ToString(), CultureInfo.InvariantCulture),
                        
                        
                    });
                }
            }
            return new MessageExchangeResponse<IEnumerable<Candle>>(candleList, candlesData.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Trade>> ConverterTrades<TTempModel>(TTempModel trades, string pair)
        {
            var tradesTemp = trades as IExchangeResultResponse<IEnumerable<BinanceFuturesTrade>>;
            var listTrade = new List<Trade>();
            if (tradesTemp != null && tradesTemp.ErrorResult.IsSuccess)
            {
                foreach (var tradeBinance in tradesTemp.Data)
                {
                    var exTrade = new Trade
                    {
                        Id = tradeBinance.Id.ToString(),
                        Exchange = Credentials.Exchange,
                        Pair = pair,
                        Amount = Convert.ToDecimal(tradeBinance.Quantity, CultureInfo.InvariantCulture),
                        Price = Convert.ToDecimal(tradeBinance.Price, CultureInfo.InvariantCulture),
                        Side = tradeBinance.IsBuyerMaker ? Sides.Buy : Sides.Sell,
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(tradeBinance.Time),
                    };
                    listTrade.Add(exTrade);
                }
            }

            return new MessageExchangeResponse<IEnumerable<Trade>>(listTrade, tradesTemp.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Trade>> ConverterAggregateTrades<TTempModel>(TTempModel trades, string pair)
        {
            var tradesTemp = trades as IExchangeResultResponse<IEnumerable<BinanceFuturesAggregateTrade>>;
            var listTrade = new List<Trade>();
            if (tradesTemp != null && tradesTemp.ErrorResult.IsSuccess)
            {
                foreach (var tradeBinance in tradesTemp.Data)
                {
                    var exTrade = new Trade
                    {
                        Id = tradeBinance.AggregatetradeId,
                        Exchange = Credentials.Exchange,
                        Pair = pair,
                        Amount = Convert.ToDecimal(tradeBinance.Quantity, CultureInfo.InvariantCulture),
                        Price = Convert.ToDecimal(tradeBinance.Price, CultureInfo.InvariantCulture),
                        Side = Convert.ToBoolean(tradeBinance.IsBuyerMaker) ? Sides.Buy : Sides.Sell,
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(tradeBinance.Timestamp))
                    };
                    listTrade.Add(exTrade);
                }
            }

            return new MessageExchangeResponse<IEnumerable<Trade>>(listTrade, tradesTemp.ErrorResult, Credentials.Exchange);
        }


        #endregion

        #region Rest Private Data

        public IExchangeResultResponse<Leverage> ConverterSetLeverage<TTempModel>(TTempModel setLeverageInfo)
        {

            var setLeverageInfoReponse = setLeverageInfo as IExchangeResultResponse<BinanceFuturesSetLeverage>;
            var leverageValue = new Leverage();
            if (setLeverageInfoReponse != null && setLeverageInfoReponse.ErrorResult.IsSuccess)
            {
                try
                {
                    leverageValue = new Leverage
                    {

                        LeverageValue = setLeverageInfoReponse.Data.Leverage

                    };

                }
                catch (Exception ex)
                {

                }
            }

            return new MessageExchangeResponse<Leverage>(leverageValue, setLeverageInfoReponse.ErrorResult, Credentials.Exchange);
        }
        public IExchangeResultResponse<IEnumerable<Balance>> ConverterBalances<TTempModel>(TTempModel balances)
        {
            
            var balancesReponse = balances  as IExchangeResultResponse<BinanceFuturesBalanceInfo>;
            var dictionaryBalance = new List<Balance>();           
            if (balancesReponse != null && balancesReponse.ErrorResult.IsSuccess)
            {
                foreach (var balance in balancesReponse.Data.Balances)
                {
                    var balanceBase = new Balance
                    {
                      
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = balance.Asset,
                        SymbolCurrency = balance.Asset,
                        WalletBalance = Convert.ToDecimal(balance.WalletBalance, CultureInfo.InvariantCulture),
                        MarginBalance = Convert.ToDecimal(balance.MarginBalance, CultureInfo.InvariantCulture),
                        AvailBalance = Convert.ToDecimal(balance.MaxWithdrawAmount, CultureInfo.InvariantCulture),

                    };
                    dictionaryBalance.Add(balanceBase);
                }
            }

            return new MessageExchangeResponse<IEnumerable<Balance>>(dictionaryBalance, balancesReponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Position>> ConverterPositions<TTempModel>(TTempModel positions)
        {
            var positionsReponse = positions as IExchangeResultResponse<IEnumerable<BinanceFuturesPositions>>;
            var dictionaryPositions = new List<Position>();
            if (positionsReponse != null && positionsReponse.ErrorResult.IsSuccess)
            {
                foreach (var position in positionsReponse.Data)
                {
                    var positionBase = new Position()
                    {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        IsOpen = Convert.ToDecimal(position.PositionAmt, CultureInfo.InvariantCulture) != 0 ? true : false,
                        Pair = position.Symbol,
                        Size = Convert.ToDecimal(position.PositionAmt, CultureInfo.InvariantCulture),
                        AveragePrice = Convert.ToDecimal(position.EntryPrice, CultureInfo.InvariantCulture),
                        Margin = Convert.ToDecimal(position.IsolatedMargin, CultureInfo.InvariantCulture),
                        PnL = Convert.ToDecimal(position.UnRealizedProfit, CultureInfo.InvariantCulture)
                    };
                    dictionaryPositions.Add(positionBase);
                }
            }

            return new MessageExchangeResponse<IEnumerable<Position>>(dictionaryPositions, positionsReponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<MyTrade>> ConverterMyTrades<TTempModel>(TTempModel myTrades)
        {
            var mytradesReponse = myTrades as IExchangeResultResponse<IEnumerable<BinanceFuturesMyTrade>>;
            var dictionaryMyTrades = new List<MyTrade>();
            if (mytradesReponse != null && mytradesReponse.ErrorResult.IsSuccess)
            {
                foreach (var myTrade in mytradesReponse.Data)
                {
                    var takerOrMaker = TakerOrMaker.Taker;
                    if (string.IsNullOrEmpty(myTrade.Maker) == false)
                    {
                        if (Convert.ToBoolean(myTrade.Maker) == true)
                        {
                            takerOrMaker = TakerOrMaker.Maker;
                        }

                    }
                    var myTradeBase = new MyTrade()
                    {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = myTrade.Symbol,
                        Id = myTrade.Id,
                        OrderId = myTrade.OrderId,
                        Price = Convert.ToDecimal(myTrade.Price, CultureInfo.InvariantCulture),
                        Amount = Convert.ToDecimal(myTrade.Qty, CultureInfo.InvariantCulture),
                        Side = myTrade.Side.ToLower() == "sell" ? Sides.Sell : Sides.Buy,
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(myTrade.Time)),
                        CommissionTakerOrMaker = takerOrMaker,
                        Commission = new CommissionModel(Credentials.Exchange, CommissionHelper.ReturnCommissionRateByExchange(Credentials.Exchange), Convert.ToDecimal(myTrade.Commission,
                            CultureInfo.InvariantCulture), myTrade.CommissionAsset)

                    };
                    dictionaryMyTrades.Add(myTradeBase);
                }
            }

            return new MessageExchangeResponse<IEnumerable<MyTrade>>(dictionaryMyTrades, mytradesReponse.ErrorResult, Credentials.Exchange);
        }
        public IExchangeResultResponse<Order> ConverterGetOrderInfo<TTempModel>(TTempModel orderInfoResult, Order order)
        {
            var orderInfoResponse = orderInfoResult as IExchangeResultResponse<BinanceFuturesOrderResponse>;

            if (orderInfoResponse != null && orderInfoResponse.ErrorResult.IsSuccess)
            {
                try
                {   
                    order.Account = Credentials.ConnectorName;
                    order.Exchange = Credentials.Exchange;
                    order.Pair = orderInfoResponse.Data.Symbol;
                    order.Id = orderInfoResponse.Data.OrderId;
                    order.ClId = orderInfoResponse.Data.ClientOrderId;
                    order.Amount = Convert.ToDecimal(orderInfoResponse.Data.OrigQty, CultureInfo.InvariantCulture);
                    order.AmountFilled = Convert.ToDecimal(orderInfoResponse.Data.ExecutedQty, CultureInfo.InvariantCulture);
                    order.AveragePrice = Convert.ToDecimal(orderInfoResponse.Data.AvgPrice, CultureInfo.InvariantCulture);
                    order.Price = Convert.ToDecimal(orderInfoResponse.Data.Price, CultureInfo.InvariantCulture);
                    order.StopPrice = Convert.ToDecimal(orderInfoResponse.Data.StopPrice, CultureInfo.InvariantCulture);
                    order.StopWorkingType = orderInfoResponse.Data.WorkingType;
                    order.Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(orderInfoResponse.Data.Time));
                    order.Status = BinanceFuturesConvertors.ReturnOrderState(orderInfoResponse.Data.Status);
                    order.Side = orderInfoResponse.Data.Side.ToLower() == "sell" ? Sides.Sell : Sides.Buy;
                    order.Type = BinanceFuturesConvertors.ReturnOrderType(orderInfoResponse.Data.Type);
                   
                }
                catch (Exception ex)
                {

                }
            }
            return new MessageExchangeResponse<Order>(order, orderInfoResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Order>> ConverterGetOrders<TTempModel>(TTempModel allOrdersResult)
        {
            var ordersResponse = allOrdersResult as IExchangeResultResponse<IEnumerable<BinanceFuturesOrderResponse>>;
            var orders = new List<Order>();
            if (ordersResponse != null && ordersResponse.ErrorResult.IsSuccess)
            {
                foreach (var order in ordersResponse.Data)
                {
                    try
                    {
                        var orderBase = new Order()
                    {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = order.Symbol,
                        Id = order.OrderId,
                        ClId = order.ClientOrderId,
                        Amount = Convert.ToDecimal(order.OrigQty, CultureInfo.InvariantCulture),
                        AmountFilled = Convert.ToDecimal(order.ExecutedQty, CultureInfo.InvariantCulture),
                        Price = Convert.ToDecimal(order.Price, CultureInfo.InvariantCulture),
                        AveragePrice = Convert.ToDecimal(order.AvgPrice, CultureInfo.InvariantCulture),
                        StopPrice = Convert.ToDecimal(order.StopPrice, CultureInfo.InvariantCulture),
                        StopWorkingType = order.WorkingType
                    };
                   
                       orderBase.Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.Time));
                       orderBase.Status = BinanceFuturesConvertors.ReturnOrderState(order.Status);
                       orderBase.Side = order.Side.ToLower() == "sell" ? Sides.Sell : Sides.Buy;
                       orderBase.Type = BinanceFuturesConvertors.ReturnOrderType(order.Type);

                       orders.Add(orderBase);
                    }
                    catch (Exception ex)
                    {

                    }
              
                  
                }
            }
            return new MessageExchangeResponse<IEnumerable<Order>>(orders, ordersResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<IEnumerable<Order>> ConverterGetActiveOrders<TTempModel>(TTempModel activeOrdersResult)
        {
            var activeOrdersResponse = activeOrdersResult as IExchangeResultResponse<IEnumerable<BinanceFuturesOrderResponse>>;
            var orders = new List<Order>();
            if (activeOrdersResponse != null && activeOrdersResponse.ErrorResult.IsSuccess)
            {
                foreach (var order in activeOrdersResponse.Data)
                {
                    try
                    {
                        var orderBase = new Order()
                       {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = order.Symbol,
                        Id = order.OrderId,
                        ClId = order.ClientOrderId,
                        Amount = Convert.ToDecimal(order.OrigQty, CultureInfo.InvariantCulture),
                        AmountFilled = Convert.ToDecimal(order.ExecutedQty, CultureInfo.InvariantCulture),
                        Price = Convert.ToDecimal(order.Price, CultureInfo.InvariantCulture),
                        AveragePrice = Convert.ToDecimal(order.AvgPrice, CultureInfo.InvariantCulture),
                        StopPrice = Convert.ToDecimal(order.StopPrice, CultureInfo.InvariantCulture),
                        StopWorkingType = order.WorkingType
                        };
                    
                        orderBase.Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.Time));
                        orderBase.Status = BinanceFuturesConvertors.ReturnOrderState(order.Status);
                        orderBase.Side = order.Side.ToLower() == "sell" ? Sides.Sell : Sides.Buy;
                        orderBase.Type = BinanceFuturesConvertors.ReturnOrderType(order.Type);
                        orders.Add(orderBase);
                    }
                    catch (Exception ex)
                    {

                    }

                   
                }
            }
            return new MessageExchangeResponse<IEnumerable<Order>>(orders, activeOrdersResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<Order> ConverterPlaceOrderResult<TTempModel>(TTempModel placeOrderResult, Order order)
        {
            
            var placeOrderResponse = placeOrderResult as IExchangeResultResponse<BinanceFuturesOrderResponse>;
           
            if (placeOrderResponse != null && placeOrderResponse.ErrorResult.IsSuccess)
            {
               
                try
                {
                    order.Id = placeOrderResponse.Data.OrderId.ToString();
                    order.Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(placeOrderResponse.Data.UpdateTime));
                    order.Status = BinanceFuturesConvertors.ReturnOrderState(placeOrderResponse.Data.Status);
                   
                }
                catch (Exception ex)
                {

                }
            }
            return new MessageExchangeResponse<Order>(order, placeOrderResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<Order> ConverterCancelOrderResult<TTempModel>(TTempModel cancelOrderResult, Order order)
        {
            var cancelOrderResponse = cancelOrderResult as IExchangeResultResponse<BinanceFuturesOrderResponse>;
            if (cancelOrderResponse != null && cancelOrderResponse.ErrorResult.IsSuccess)
            {
               
                try
                {
                    order.Id = cancelOrderResponse.Data.OrderId.ToString();
                    order.Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(cancelOrderResponse.Data.UpdateTime));
                    order.Status = BinanceFuturesConvertors.ReturnOrderState(cancelOrderResponse.Data.Status);
                }
                catch (Exception ex)
                {

                }
            }
            return new MessageExchangeResponse<Order>(order, cancelOrderResponse.ErrorResult, Credentials.Exchange);
        }


      
        public IExchangeResultResponse<bool> ConverterCancelAllOrdersResult<TTempModel>(TTempModel cancelAllOrdersResult)
        {
            return null;
        }
        public IExchangeResultResponse<bool> ConverterCancelAllLimitOrdersResult<TTempModel>(TTempModel cancelAllOrdersResult)
        {
            return null;
        }
        public IExchangeResultResponse<bool> ConverterCancelAllStopOrdersResult<TTempModel>(TTempModel cancelAllOrdersResult)
        {
            return null;
        }
        public IExchangeResultResponse<bool> ConverterCancelAllOrdersBySymbolResult<TTempModel>(TTempModel cancelAllOrdersResult)
        {
            var cancelAllOrdersResponse = cancelAllOrdersResult as IExchangeResultResponse<BinanceFuturesErrorResponse>;
            if (cancelAllOrdersResponse != null && cancelAllOrdersResponse.ErrorResult.IsSuccess)
            {
                return new MessageExchangeResponse<bool>(true, cancelAllOrdersResponse.ErrorResult, Credentials.Exchange);
            }
            return new MessageExchangeResponse<bool>(false, cancelAllOrdersResponse.ErrorResult, Credentials.Exchange);
        }

        public IExchangeResultResponse<bool> ConverterCancelAllOrdersBySideResult<TTempModel>(TTempModel cancelAllOrdersResult)
        {
            return null;
        }
      
        public IExchangeResultResponse<Order> ConverterChangeOrderResult<TTempModel>(TTempModel changeOrderResult, Order order)
        {
            return null;
        }

        public IExchangeResultResponse<bool> ConverterClosePositionResult<TTempModel>(TTempModel closePositionResult)
        {
            return null;
        }

        public IExchangeResultResponse<bool> ConverterClosePositionsResult<TTempModel>(TTempModel closePositionsResult)
        {
            return null;
        }
        #endregion


        public IExchangeResultResponse<object> ConvertSubscriptoionResponce(string message)
        {

            if (message.Contains(@"aggTrade"))
                return ParseAggregateTrades(message);
            else if (message.Contains(@"24hrTicker"))
                return ParseTicker(message);
            else if (message.Contains(@"kline"))
                return ParseCandles(message);
            else if (message.Contains(@"depthUpdate"))
                return ParseOrderBook(message);
            else if (message.Contains(@"ACCOUNT_UPDATE"))
                return AccountUpade(message);
            else if (message.Contains(@"ORDER_TRADE_UPDATE"))
                return ParseOrderTradeUpdate(message);
            return null;
        }

     

        #region Socket Public Data
        public IExchangeResultResponse<object> ParseAggregateTrades(string message)
        {
          
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var tradesTemp = JsonConvert.DeserializeObject<BinanceFuturesTradeSocket>(message);
                    errorResponse = new SuccessResponse();
                    var listTrade = new List<Trade>();
                    if (tradesTemp != null)
                    {

                        var exTrade = new Trade
                        {
                            Id = tradesTemp.data.Id,
                            Exchange = Credentials.Exchange,
                            Pair = tradesTemp.data.Symbol,
                            Amount = Convert.ToDecimal(tradesTemp.data.Quantity, CultureInfo.InvariantCulture),
                            Price = Convert.ToDecimal(tradesTemp.data.Price, CultureInfo.InvariantCulture),
                            Side = Convert.ToBoolean(tradesTemp.data.IsBuy) ? Sides.Buy : Sides.Sell,
                            Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(tradesTemp.data.TradeTime))
                        };
                        listTrade.Add(exTrade);

                    }
                    return new MessageExchangeResponse<object>(listTrade, errorResponse, Credentials.Exchange);
                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }


            return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange); 
        }

        public IExchangeResultResponse<object> ParseTicker(string message)
        {
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var tickerTemp = JsonConvert.DeserializeObject<BinanceFuturesTickerSocket>(message);
                    errorResponse = new SuccessResponse();
                    if (tickerTemp != null)
                    {

                        var ticker = new Ticker
                        {
                            Exchange = Credentials.Exchange,
                            Pair = tickerTemp.Data.Symbol,
                            Date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(tickerTemp.Data.EventTime)),
                            Last = Convert.ToDecimal(tickerTemp.Data.LastPrice, CultureInfo.InvariantCulture),
                            MaxPrice24h = Convert.ToDecimal(tickerTemp.Data.HighPrice, CultureInfo.InvariantCulture),
                            MinPrice24h = Convert.ToDecimal(tickerTemp.Data.LowPrice, CultureInfo.InvariantCulture),
                            Volume24h = Convert.ToDecimal(tickerTemp.Data.TotalTradeVolume, CultureInfo.InvariantCulture),
                            PercentChange = Convert.ToDecimal(tickerTemp.Data.PriceChangePercent, CultureInfo.InvariantCulture),
                        };
                        return new MessageExchangeResponse<object>(ticker, errorResponse, Credentials.Exchange);

                    }

                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }


            return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange);
        }

        public IExchangeResultResponse<object> ParseCandles(string message)
        {
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var candlesTemp = JsonConvert.DeserializeObject<BinanceFuturesCandleSocket>(message);
                    errorResponse = new SuccessResponse();
                    var candlelist = new List<Candle>();
                    if (candlesTemp != null && candlesTemp.Data.Candles != null)
                    {
                        var candle = new Candle
                        {
                            Exchange = Credentials.Exchange,
                            Pair = candlesTemp.Data.Candles.Symbol,
                            OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(candlesTemp.Data.Candles.StartTime)),
                            OpenPrice = Convert.ToDecimal(candlesTemp.Data.Candles.OpenPrice, CultureInfo.InvariantCulture),
                            HighPrice = Convert.ToDecimal(candlesTemp.Data.Candles.HighPrice, CultureInfo.InvariantCulture),
                            LowPrice = Convert.ToDecimal(candlesTemp.Data.Candles.LowPrice, CultureInfo.InvariantCulture),
                            ClosePrice = Convert.ToDecimal(candlesTemp.Data.Candles.ClosePrice, CultureInfo.InvariantCulture),
                            TotalVolume = Convert.ToDecimal(candlesTemp.Data.Candles.Volume, CultureInfo.InvariantCulture),                           
                            IsClosed = candlesTemp.Data.Candles.X
                        };
                        candlelist.Add(candle);
                    }
                    return new MessageExchangeResponse<object>(candlelist, errorResponse, Credentials.Exchange);
                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }


            return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange);
        }

        private Dictionary<object, decimal> IdOrderBook { get; set; }
        public IExchangeResultResponse<object> ParseOrderBook(string message)
        {
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var orderbook = JsonConvert.DeserializeObject<BinanceFuturesOrderBookSocket>(message);
                    errorResponse = new SuccessResponse();
                    var orderBookBase = new OrderBook(orderbook.data.Symbol, 0);
                    var asks = new List<Quote>();
                    foreach (var orderBookBinanceFuturesSocketItem in orderbook.data.Asks)
                    {
                        var quote = new Quote
                        {
                            Amount = Convert.ToDecimal(orderBookBinanceFuturesSocketItem[1], CultureInfo.InvariantCulture)
                        };
                        if (!IdOrderBook.ContainsKey(orderbook.data.BinanceFuturesOrderBookSocketU))
                        {
                            IdOrderBook.Add(orderbook.data.BinanceFuturesOrderBookSocketU,
                                Convert.ToDecimal(orderBookBinanceFuturesSocketItem[0], CultureInfo.InvariantCulture));
                        }


                        quote.Price = IdOrderBook[orderbook.data.BinanceFuturesOrderBookSocketU];
                        asks.Add(quote);

                    }

                    var bids = new List<Quote>();
                    foreach (var orderBookBinanceFuturesSocketItem in orderbook.data.Bids)
                    {
                        var quote = new Quote
                        {
                            Amount = Convert.ToDecimal(orderBookBinanceFuturesSocketItem[1], CultureInfo.InvariantCulture)
                        };
                        if (!IdOrderBook.ContainsKey(orderbook.data.BinanceFuturesOrderBookSocketU))
                        {
                            IdOrderBook.Add(orderbook.data.BinanceFuturesOrderBookSocketU,
                                Convert.ToDecimal(orderBookBinanceFuturesSocketItem[0], CultureInfo.InvariantCulture));
                        }

                        quote.Price = IdOrderBook[orderbook.data.BinanceFuturesOrderBookSocketU];
                        bids.Add(quote);
                    }
                    orderBookBase.Asks = new OrderCollection<Quote>(asks);
                    orderBookBase.Bids = new OrderCollection<Quote>(bids);
                    orderBookBase.Exchange = Exchange.BinanceFutures;
                    orderBookBase.Pair = orderbook.data.Symbol;
                    return new MessageExchangeResponse<object>(orderBookBase, errorResponse, Credentials.Exchange);
                
                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }


            return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange);
        }

        #endregion

        #region Socket Private Data
        public IExchangeResultResponse<object> AccountUpade(string message)
        {
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var balanceTemp = JsonConvert.DeserializeObject<BinanceFuturesAccountUpdateSocketResponse>(message);
                    errorResponse = new SuccessResponse();
                    var balances = ConverterSocketBalances(balanceTemp.Data.Balances);
                    var positions = ConverterSocketPositions(balanceTemp.Data.Positions);
                    var accupdate = new AccountUpdate();
                    if (balances != null && balances.Count() >= 1)
                    {
                        accupdate.Balances = balances.ToList();
                    }
                    if (positions != null && positions.Count() >= 1)
                    {
                        accupdate.Positions = positions.ToList();
                    }
                    return new MessageExchangeResponse<object>(accupdate, errorResponse, Credentials.Exchange);


                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }

             return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange);
        }

        public IEnumerable<Balance> ConverterSocketBalances(IEnumerable<BinanceFuturesBalanceSocket> balances)
        {
            var listBalances = new List<Balance>();
            try
            {


                if (balances != null && balances.Count() >= 1)
                {
                    foreach (var balance in balances)
                    {

                        var balancetemp = new Balance
                        {

                            Exchange = Credentials.Exchange,
                            Account = Credentials.ConnectorName,
                            SymbolCurrency = balance.Symbol,
                            WalletBalance = Convert.ToDecimal(balance.WalletBalance, CultureInfo.InvariantCulture),
                            AvailBalance = Convert.ToDecimal(balance.CrossWalletBalance, CultureInfo.InvariantCulture)
                        };
                        listBalances.Add(balancetemp);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return listBalances;
        }

        public IEnumerable<Position> ConverterSocketPositions(IEnumerable<BinanceFuturesPositionSocket> positions)
        {
            var listPositions = new List<Position>();
            try
            {
                
                if (positions != null && positions.Count() >= 1)
                {
                    foreach (var position in positions)
                    {
                        var positionBase = new Position()
                        {
                            Exchange = Credentials.Exchange,
                            Account = Credentials.ConnectorName,
                            IsOpen = Convert.ToDecimal(position.Amount, CultureInfo.InvariantCulture) != 0 ? true : false,
                            Pair = position.Symbol,
                            PositionSide = (PositionSide)Enum.Parse(typeof(PositionSide), position.PositionSide),
                            Size = Convert.ToDecimal(position.Amount, CultureInfo.InvariantCulture),
                            AveragePrice = Convert.ToDecimal(position.Price, CultureInfo.InvariantCulture),
                            Margin = Convert.ToDecimal(position.IsolatedMargin, CultureInfo.InvariantCulture),
                            PnL = Convert.ToDecimal(position.Pnl, CultureInfo.InvariantCulture),

                        };
                        listPositions.Add(positionBase);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return listPositions;
        }

        private IExchangeResultResponse<object> ParseOrderTradeUpdate(string message)
        {
            IErrorResult errorResponse;
            if (!String.IsNullOrEmpty(message))
            {
                try
                {
                    var orderTradeTemp = JsonConvert.DeserializeObject<BinanceFuturesOrderUpdateSocketResponce>(message);
                    errorResponse = new SuccessResponse();
                    var order = ConverterSocketOrder(orderTradeTemp);
                    var myTrades = ConverterSocketTrade(orderTradeTemp);
                    var ordertradeupdate = new OrderWithTradeUpdate();
                    if (order != null && order.Count() >= 1)
                    {
                        ordertradeupdate.Orders = order.ToList();
                    }
                    if (myTrades != null && myTrades.Count() >= 1)
                    {
                        ordertradeupdate.MyTrades = myTrades.ToList();
                    }
                    return new MessageExchangeResponse<object>(ordertradeupdate, errorResponse, Credentials.Exchange);


                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {message}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {message}");
                }
            }
            else
            {
                errorResponse = new UnknownError(message);
            }

            return new MessageExchangeResponse<object>(null, errorResponse, Credentials.Exchange);
        }

        public IEnumerable<Order> ConverterSocketOrder(BinanceFuturesOrderUpdateSocketResponce order)
        {
            var listOrder = new List<Order>();
            try
            {


                if (order != null)
                {

                    var orderBase = new Order()
                    {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = order.BinanceFuturesSocketOrderResponse.Symbol,
                        Id = order.BinanceFuturesSocketOrderResponse.OrderId,
                        ClId = order.BinanceFuturesSocketOrderResponse.ClientOrderId,
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(
                            Convert.ToInt64(order.BinanceFuturesSocketOrderResponse.OrderTradeTime)),
                        Side = order.BinanceFuturesSocketOrderResponse.Side.ToLower() == "sell"
                            ? Sides.Sell
                            : Sides.Buy,
                        Type = BinanceFuturesConvertors.ReturnOrderType(order.BinanceFuturesSocketOrderResponse
                            .OrderType),
                        Status = BinanceFuturesConvertors.ReturnOrderState(order.BinanceFuturesSocketOrderResponse
                            .OrderStatus),
                        Price = Convert.ToDecimal(order.BinanceFuturesSocketOrderResponse.Price,
                            CultureInfo.InvariantCulture),
                        StopPrice = Convert.ToDecimal(order.BinanceFuturesSocketOrderResponse.StopPrice,
                            CultureInfo.InvariantCulture),
                        Amount = Convert.ToDecimal(order.BinanceFuturesSocketOrderResponse.OriginalQuantity,
                            CultureInfo.InvariantCulture),
                        AmountFilled =
                            Convert.ToDecimal(order.BinanceFuturesSocketOrderResponse.OrderFilledAccumulatedQuantity,
                                CultureInfo.InvariantCulture),
                        AveragePrice = Convert.ToDecimal(order.BinanceFuturesSocketOrderResponse.AveragePrice,
                            CultureInfo.InvariantCulture),
                        StopWorkingType = order.BinanceFuturesSocketOrderResponse.StopPriceType,
                        ReduceOnly= Convert.ToBoolean(order.BinanceFuturesSocketOrderResponse.IsReduceOnly)
                    };

                    listOrder.Add(orderBase);

                }
            }
            catch (Exception ex)
            {

            }

            return listOrder;
        }

        public IEnumerable<MyTrade> ConverterSocketTrade(BinanceFuturesOrderUpdateSocketResponce myTrade)
        {
            var listMyTrade = new List<MyTrade>();
            try
            {


                if (myTrade != null && !String.IsNullOrEmpty(myTrade.BinanceFuturesSocketOrderResponse.TradeId))
                {
                    var tradeId = myTrade.BinanceFuturesSocketOrderResponse.TradeId;
                    if (Convert.ToInt64(tradeId) == 0) return null;
                    var takerOrMaker = TakerOrMaker.Taker;
                    if (string.IsNullOrEmpty(myTrade.BinanceFuturesSocketOrderResponse.IsMaker) == false)
                    {
                        if (Convert.ToBoolean(myTrade.BinanceFuturesSocketOrderResponse.IsMaker) == true)
                        {
                            takerOrMaker = TakerOrMaker.Maker;
                        }

                    }

                    var myTradeBase = new MyTrade()
                    {
                        Exchange = Credentials.Exchange,
                        Account = Credentials.ConnectorName,
                        Pair = myTrade.BinanceFuturesSocketOrderResponse.Symbol,
                        Id = myTrade.BinanceFuturesSocketOrderResponse.TradeId,
                        OrderId = myTrade.BinanceFuturesSocketOrderResponse.OrderId,
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(
                            Convert.ToInt64(myTrade.BinanceFuturesSocketOrderResponse.OrderTradeTime)),
                        Side = myTrade.BinanceFuturesSocketOrderResponse.Side.ToLower() == "sell"
                            ? Sides.Sell
                            : Sides.Buy,
                        Price = Convert.ToDecimal(myTrade.BinanceFuturesSocketOrderResponse.LastFilledPrice,
                            CultureInfo.InvariantCulture),
                        Amount = Convert.ToDecimal(myTrade.BinanceFuturesSocketOrderResponse.OrderLastFilledQuantity,
                            CultureInfo.InvariantCulture),
                        CommissionTakerOrMaker = takerOrMaker,
                        Commission = new CommissionModel(Credentials.Exchange,CommissionHelper.ReturnCommissionRateByExchange(Credentials.Exchange), Convert.ToDecimal(myTrade.BinanceFuturesSocketOrderResponse.Commission,
                            CultureInfo.InvariantCulture), myTrade.BinanceFuturesSocketOrderResponse.CommissionAsset)
                    };
                    listMyTrade.Add(myTradeBase);

                }
            }
            catch (Exception ex)
            {

            }

            return listMyTrade;
        }
        #endregion
    }
}
