using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HQConnector.Core.Classes.Credentials;
using HQConnector.Core.Interfaces.BuilderParameters;
using HQConnector.Core.Interfaces.Connector;
using HQConnector.Core.Interfaces.Connectors.ConnectorEndpoint;
using HQConnector.Core.Interfaces.Constants;
using HQConnector.Core.Interfaces.Converters;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.Rest;
using HQConnector.Core.Interfaces.Socket;
using HQConnector.Dto.DTO.Account;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;
using HQConnector.Dto.DTO.Symbol;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;
using WebSocket4Net;

namespace HQConnector.Core.Classes.Connector
{
    public abstract class AbstractConnector : IMarketDataConnector,IPrivateDataConnector
    {
        #region ctor

        protected AbstractConnector(string connectorName, Exchange exchange)
        {
            Credentials = new ConnectorCredentials(connectorName, exchange);
        }
        
        protected AbstractConnector(string connectorName, Exchange exchange, string apiKey, string secretKey ) : this(connectorName, exchange)
        {
            Credentials.ApiKey = apiKey;
            Credentials.ApiSecretKey = secretKey;
        }

        protected AbstractConnector(string connectorName, Exchange exchange, string apiKey, string secretKey, string passphrase) : this(connectorName, exchange,apiKey, secretKey)
        {
            Credentials.Passphrase = passphrase;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Фьючерсы или нет
        /// </summary>
        [DefaultValue(false)]
        public bool IsFutures { get; set; }

        /// <summary>
        /// Маржиналка или нет
        /// </summary>
        [DefaultValue(false)]
        public bool IsMargin { get; set; }


        [DefaultValue(ExchangeMode.Trade)]
        public ExchangeMode ExchangeMode { get; set; }

        [DefaultValue(false)]
        public bool IsSupportStopOrders { get; set; }

        public bool IsSocketConnected
        {
            get
            {
                return (SocketClient.SocketState == WebSocketState.Open) ||
                    SocketClient.CurrentSubscriptions.Count == 0;
            }
        }

        public IConnectorCredentials Credentials { get; set; }

        public IConnectorRestClientSettings RestSettings { get; set; }

        protected IRestClient RestClient { get; set; }

        protected IConnectorEndpointsAndMethods EndpointsAndMethods { get; set; }

        protected IBuilderParameters BuilderParameters { get; set; }

        protected IConverterToBaseModel Converter { get; set; }

        protected ContentType ContentTypeForSender { get; set; }

        protected ISocketClient SocketClient { get; set; }

        public ConcurrentBag<object> ReceivedItems { get; set; }

        #endregion

        #region [REST] Abstract and Virtual Methods

        #region Rest Public Data

        /// <summary>
        /// Get server time
        /// </summary>
        /// <returns></returns>
        public abstract Task<long> GetServerTimeAsync();
        protected async Task<long> GetServerTimeAsync<TTempModel>()
        {
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetServerTimeRestEndpoint, EndpointsAndMethods.ServerTimeHttpMethods, null, Signed.No, ContentTypeForSender);
            return Converter.ConvertServerTime(res.Data);
        }

        /// <summary>
        /// Get exchange symbols
        /// </summary>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetSymbolsAsync(int? maxcount);
        protected  async Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetSymbolsAsync<TTempModel>(int? maxcount)
        {
            var listSymbols = new List<Symbol>();
            IExchangeResultResponse<IEnumerable<Symbol>> symbolsResponse;
            var countsymbols = maxcount != null && maxcount >= RestSettings.MaxCountSymbols ? RestSettings.MaxCountSymbols : maxcount != null ? maxcount : RestSettings.MaxCountSymbols;
            var param = BuilderParameters.CreateParamsGetSymbols(countsymbols);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetSymbolsRestEndpoint, EndpointsAndMethods.SymbolsHttpMethods, param, Signed.No, ContentTypeForSender);
            symbolsResponse = Converter.ConverterSymbols(res);
            if (symbolsResponse != null && symbolsResponse.ErrorResult.IsSuccess)
            {
                listSymbols.AddRange(symbolsResponse.Data);
                   
            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Symbol>>(null, symbolsResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Symbol>>(listSymbols, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get active exchange symbols
        /// </summary>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetActiveSymbolsAsync();
        protected async Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetActiveSymbolsAsync<TTempModel>()
        {
            var listSymbols = new List<Symbol>();
            IExchangeResultResponse<IEnumerable<Symbol>> activesymbolsResponse;

            var param = BuilderParameters.CreateParamsGetActiveSymbols();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetActiveSymbolsRestEndpoint, EndpointsAndMethods.ActiveSymbolsHttpMethods, param, Signed.No, ContentTypeForSender);
            activesymbolsResponse = Converter.ConverterActiveSymbols(res);
            if (activesymbolsResponse != null && activesymbolsResponse.ErrorResult.IsSuccess)
            {
                listSymbols.AddRange(activesymbolsResponse.Data);

            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Symbol>>(null, activesymbolsResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Symbol>>(listSymbols, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get ticker 
        /// </summary>
        /// <param name="pair">Exchange symbolcode</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<Ticker>> GetTickerAsync(string pair);
        protected async Task<IExchangeResultResponse<Ticker>> GetTickerAsync<TTempModel>(string pair)
        {
            var param = BuilderParameters.CreateParamsGetTicker(pair);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetTickerRestEndpoint(pair), EndpointsAndMethods.TickerHttpMethods, param, Signed.No, ContentTypeForSender);
            return Converter.ConverterTicker(res, pair);
        }

        /// <summary>
        ///Get orderbook
        /// </summary>
        /// <param name="pair">Exchange symbolcode</param>
        /// <param name="depth">Orderbook depth</param>
        /// <param name="merge">Order combination(usually unuse)</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<OrderBook>> GetOrderBookAsync(string pair, int depth, int merge = 0);
        protected async Task<IExchangeResultResponse<OrderBook>> GetOrderBookAsync<TTempModel>(string pair, int depth, int merge = 0)
        {
            var param = BuilderParameters.CreateParamsGetOrderBook(pair, depth, merge);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.OrderBookRestEndpoint(pair, depth, merge),
                EndpointsAndMethods.OrderBookHttpMethods, param, Signed.No, ContentTypeForSender);
            return Converter.ConverterOrderBook(res, pair, depth, merge);
        }

        /// <summary>
        ///Get Candles(Klines)
        /// </summary>
        /// <param name="pair">>Exchange symbolcode</param>
        /// <param name="periodInSec">interval in seconds</param>
        /// <param name="fromDate">Start from date</param>
        /// <param name="toDate">End time</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<IEnumerable<Candle>>> GetCandleSeriesAsync(string pair, int periodInSec, long? fromDate, long? toDate = null, int? count = null);
        protected async Task<IExchangeResultResponse<IEnumerable<Candle>>> GetCandleSeriesAsync<TTempModel>(string pair, int periodInSec, long? fromDate, long? toDate, int? count)
        {
           
            var countCandles = count != null && count >= RestSettings.MaxCountCandles ? RestSettings.MaxCountCandles  : count;
            List<Candle> listCandles = new List<Candle>();
            IExchangeResultResponse<IEnumerable<Candle>> candlesResponse;
            
                var param = BuilderParameters.CreateParamsGetCandleSeries(pair, periodInSec, fromDate, toDate, countCandles);
                var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetCandlesRestEndpoint(pair, periodInSec, fromDate, toDate, countCandles),
                    EndpointsAndMethods.CandlesHttpMethods, param, Signed.No, ContentTypeForSender);
                candlesResponse = Converter.ConverterCandleSeries(res, pair);
                if (candlesResponse.ErrorResult.IsSuccess)
                {
                    listCandles.AddRange(candlesResponse.Data);
                  
                }
                else
                {
                    return new MessageExchangeResponse<IEnumerable<Candle>>(null, candlesResponse.ErrorResult, Credentials.Exchange);
                }

       
            return new MessageExchangeResponse<IEnumerable<Candle>>(listCandles, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get exchange trades flow
        /// </summary>
        /// <param name="pair">Exchange symbolcode</param>
        /// <param name="maxCount">Max count</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<IEnumerable<Trade>>> GetTradesAsync(string pair, int? maxCount);
        protected async Task<IExchangeResultResponse<IEnumerable<Trade>>> GetTradesAsync<TTempModel>(string pair, int? maxCount)
        {
            
            var countTrades = maxCount != null && maxCount >= RestSettings.MaxCountTrades ? RestSettings.MaxCountTrades : maxCount;
            List<Trade> listTrades = new List<Trade>();
            IExchangeResultResponse<IEnumerable<Trade>> tradesResponse;
           
                var param = BuilderParameters.CreateParamsGetTrades(pair, countTrades);
                var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetTradesRestEndpoint(pair, countTrades),
                    EndpointsAndMethods.TradesHttpMethods, param, Signed.No, ContentTypeForSender);
                tradesResponse = Converter.ConverterTrades(res, pair);
                if (tradesResponse != null && tradesResponse.ErrorResult.IsSuccess)
                {
                    listTrades.AddRange(tradesResponse.Data);                  
                }
                else
                {
                    return new MessageExchangeResponse<IEnumerable<Trade>>(null, tradesResponse.ErrorResult, Credentials.Exchange);
                }
            return new MessageExchangeResponse<IEnumerable<Trade>>(listTrades, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get history trades from exchange
        /// </summary>
        /// <param name="pair">Exchange symbolcode</param>
        /// <param name="maxCount">Max count</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<IEnumerable<Trade>>> GetHistoryTradesAsync(string pair, int? maxCount, string fromId);
        protected async Task<IExchangeResultResponse<IEnumerable<Trade>>> GetHistoryTradesAsync<TTempModel>(string pair, int? maxCount, string fromId)
        {

            var countTrades = maxCount != null && maxCount >= RestSettings.MaxCountTrades ? RestSettings.MaxCountTrades : maxCount;
            List<Trade> listTrades = new List<Trade>();
            IExchangeResultResponse<IEnumerable<Trade>> tradesResponse;

            var param = BuilderParameters.CreateParamsGetHistoryTrades(pair, countTrades,fromId);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetHistoryTradesRestEndpoint(pair, countTrades,fromId),
                EndpointsAndMethods.HistoryTradesHttpMethods, param, Signed.No, ContentTypeForSender);
            tradesResponse = Converter.ConverterTrades(res, pair);
            if (tradesResponse != null && tradesResponse.ErrorResult.IsSuccess)
            {
                listTrades.AddRange(tradesResponse.Data);
            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Trade>>(null, tradesResponse.ErrorResult, Credentials.Exchange);
            }
            return new MessageExchangeResponse<IEnumerable<Trade>>(listTrades, new SuccessResponse(), Credentials.Exchange);
        }
       
        #endregion

        #region Rest Private Data

        /// <summary>
        /// Get user balance
        /// </summary>
        public abstract Task<IExchangeResultResponse<IEnumerable<Balance>>> GetBalanceAsync();
        protected async Task<IExchangeResultResponse<IEnumerable<Balance>>> GetBalanceAsync<TTempModel>()
        {
            List<Balance> listBalance = new List<Balance>();
            IExchangeResultResponse<IEnumerable<Balance>> balanceResponse;
           
            var param = await BuilderParameters.CreateParamsGetBalance();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.BalancesRestEndpoint(), EndpointsAndMethods.BalancesHttpMethods, param, Signed.Yes, ContentTypeForSender);
            balanceResponse = Converter.ConverterBalances(res);
            if (balanceResponse != null && balanceResponse.ErrorResult.IsSuccess)
            {
                listBalance.AddRange(balanceResponse.Data);
                    
            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Balance>>(null, balanceResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Balance>>(listBalance, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get account positions
        /// </summary>
        public abstract Task<IExchangeResultResponse<IEnumerable<Position>>> GetPositionsAsync();
        protected async Task<IExchangeResultResponse<IEnumerable<Position>>> GetPositionsAsync<TTempModel>()
        {
            if (!IsFutures)
            {
                return new MessageExchangeResponse<IEnumerable<Position>>(null, new UnknownError("Exchange don`t have features"));               
            }
            List<Position> listPositions = new List<Position>();
            IExchangeResultResponse<IEnumerable<Position>> positionResponse;
            var param = await BuilderParameters.CreateParamsGetPositions();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetPositionsRestEndpoint(), EndpointsAndMethods.PositionsHttpMethods,
                param, Signed.Yes, ContentTypeForSender);
            positionResponse = Converter.ConverterPositions(res);
            if (positionResponse != null && positionResponse.ErrorResult.IsSuccess)
            {
                listPositions.AddRange(positionResponse.Data);
            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Position>>(null, positionResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Position>>(listPositions, new SuccessResponse(), Credentials.Exchange);
        }

        /// <summary>
        /// Get MyTrades
        /// </summary>
        public abstract Task<IExchangeResultResponse<IEnumerable<MyTrade>>> GetMyTradesAsync(string pair, long? startTime, long? endTime, long? fromId, int? maxCount);
        protected async Task<IExchangeResultResponse<IEnumerable<MyTrade>>> GetMyTradesAsync<TTempModel>(string pair, long? startTime, long? endTime, long? fromId, int? maxCount)
        {
            var countMyTrades = maxCount != null && maxCount >= RestSettings.MaxCountMyTrades ? RestSettings.MaxCountMyTrades : maxCount;
            List<MyTrade> listMyTrades = new List<MyTrade>();
            IExchangeResultResponse<IEnumerable<MyTrade>> myTradesResponse;

            var param =await  BuilderParameters.CreateParamsGetMyTrades(pair,startTime,  endTime, fromId, countMyTrades);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetMyTradesRestEndpoint(pair, startTime, endTime, fromId, countMyTrades), EndpointsAndMethods.MyTradesHttpMethods, param, Signed.Yes, ContentTypeForSender);
            myTradesResponse = Converter.ConverterMyTrades(res);
            if (myTradesResponse != null && myTradesResponse.ErrorResult.IsSuccess)
            {
                listMyTrades.AddRange(myTradesResponse.Data);

            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<MyTrade>>(null, myTradesResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<MyTrade>>(listMyTrades, new SuccessResponse(), Credentials.Exchange);
        }

       
        /// <summary>
        /// Get order info
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<Order>> GetOrderInfoAsync(Order order);
        protected async Task<IExchangeResultResponse<Order>> GetOrderInfoAsync<TTempModel>(Order order)
        {
            var param = await BuilderParameters.CreateParamsGetOrderInfo(order);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetOrderInfoRestEndpoint(order), EndpointsAndMethods.GetOrderInfoHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterGetOrderInfo(res, order);
         
        }

        /// <summary>
        /// Get user orders
        /// </summary>        
        public abstract Task<IExchangeResultResponse<IEnumerable<Order>>> GetOrdersAsync();
        protected async Task<IExchangeResultResponse<IEnumerable<Order>>> GetOrdersAsync<TTempModel>()
        {
            List<Order> listOrders = new List<Order>();
            IExchangeResultResponse<IEnumerable<Order>> ordersResponse;
          
            var param = await BuilderParameters.CreateParamsGetOrders();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetOrdersRestEndpoint(), EndpointsAndMethods.GetOrdersHttpMethods, param, Signed.Yes, ContentTypeForSender);
            ordersResponse = Converter.ConverterGetOrders(res);
            if (ordersResponse != null && ordersResponse.ErrorResult.IsSuccess)
            {
                listOrders.AddRange(ordersResponse.Data);
                 
            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Order>>(null, ordersResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Order>>(listOrders, new SuccessResponse(), Credentials.Exchange);

        }

        /// <summary>
        /// Get active user orders
        /// </summary>        
        public abstract Task<IExchangeResultResponse<IEnumerable<Order>>> GetActiveOrdersAsync();
        protected async Task<IExchangeResultResponse<IEnumerable<Order>>> GetActiveOrdersAsync<TTempModel>()
        {
            List<Order> listOrders = new List<Order>();
            IExchangeResultResponse<IEnumerable<Order>> ActiveOrdersResponse;

            var param = await BuilderParameters.CreateParamsGetActiveOrders();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.GetActiveOrdersRestEndpoint(), EndpointsAndMethods.GetActiveOrdersHttpMethods, param, Signed.Yes, ContentTypeForSender);
            ActiveOrdersResponse = Converter.ConverterGetActiveOrders(res);
            if (ActiveOrdersResponse != null && ActiveOrdersResponse.ErrorResult.IsSuccess)
            {
                listOrders.AddRange(ActiveOrdersResponse.Data);

            }
            else
            {
                return new MessageExchangeResponse<IEnumerable<Order>>(null, ActiveOrdersResponse.ErrorResult, Credentials.Exchange);
            }

            return new MessageExchangeResponse<IEnumerable<Order>>(listOrders, new SuccessResponse(), Credentials.Exchange);

        }
        /// <summary>
        /// Place order
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<Order>> PlaceOrderAsync(Order order);
        protected async Task<IExchangeResultResponse<Order>> PlaceOrderAsync<TTempModel>(Order order)
        {
            var param = await BuilderParameters.CreateParamsPlaceOrder(order);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.PlaceOrderRestEndpoint(order), EndpointsAndMethods.PlaceOrderHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterPlaceOrderResult(res, order);
        }

        /// <summary>
        /// Cancel otrder
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<Order>> CancelOrderAsync(Order order);
        protected async Task<IExchangeResultResponse<Order>> CancelOrderAsync<TTempModel>(Order order)
        {
            var param =await BuilderParameters.CreateParamsCancelOrder(order);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelOrderRestEndpoint(order), EndpointsAndMethods.CancelOrderHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelOrderResult(res, order);
        }

        /// <summary>
        ///Cancel all active orders
        /// </summary>
        public abstract Task<IExchangeResultResponse<bool>> CancelAllOrdersAsync();
        protected async Task<IExchangeResultResponse<bool>> CancelAllOrdersAsync<TTempModel>()
        {
            var param = await BuilderParameters.CreateParamsCancelAllOrders();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelAllOrdersRestEndpoint(), EndpointsAndMethods.CancelAllOrdersHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelAllOrdersResult(res);
        }

        /// <summary>
        ///Cancel all limit active orders
        /// </summary>
        public abstract Task<IExchangeResultResponse<bool>> CancelAllLimitOrdersAsync(string pair);
        protected async Task<IExchangeResultResponse<bool>> CancelAllLimitOrdersAsync<TTempModel>(string pair)
        {
            var param = await BuilderParameters.CreateParamsCancelAllLimitOrders(pair);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelAllLimitOrdersRestEndpoint(pair), EndpointsAndMethods.CancelAllLimitOrdersHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelAllLimitOrdersResult(res);
        }
        /// <summary>
        ///Cancel all active stop orders
        /// </summary>
        public abstract Task<IExchangeResultResponse<bool>> CancelAllStopOrdersAsync(string pair);
        protected async Task<IExchangeResultResponse<bool>> CancelAllStopOrdersAsync<TTempModel>(string pair)
        {
            var param = await BuilderParameters.CreateParamsCancelAllStopOrders(pair);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelAllStopOrdersRestEndpoint(pair), EndpointsAndMethods.CancelAllStopOrdersHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelAllStopOrdersResult(res);
        }


        /// <summary>
        ///Cancel all active orders by symbol
        /// </summary>
        /// <param name="pair">Exchange symbol code</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<bool>> CancelAllOrdersBySymbolAsync(string pair);
        protected async Task<IExchangeResultResponse<bool>> CancelAllOrdersBySymbolAsync<TTempModel>(string pair)
        {
            var param = await BuilderParameters.CreateParamsCancelAllOrdersBySymbol(pair);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelAllOrdersBySymbolRestEndpoint(pair), EndpointsAndMethods.CancelAllOrdersBySymbolHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelAllOrdersBySymbolResult(res);
        }

        /// <summary>
        ///Cancel all active orders by side
        /// </summary>
        /// <param name="side">Order side</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<bool>> CancelAllOrdersBySideAsync(string pair,Sides side);
        protected async Task<IExchangeResultResponse<bool>> CancelAllOrdersBySideAsync<TTempModel>(string pair,Sides side)
        {
            var param = await BuilderParameters.CreateParamsCancelAllOrdersBySide(pair,side);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.CancelAllOrdersBySideRestEndpoint(pair,side), EndpointsAndMethods.CancelAllOrdersBySideHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterCancelAllOrdersBySideResult(res);
        }
        /// <summary>
        /// Change open order
        /// </summary>
        /// <param name="order">Order for change</param>
        /// <param name="price">new price if exist</param>
        /// <param name="amount">new amount if exist</param>
        /// <param name="stopprice">new stop price if exist</param>
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<Order>> ChangeOrderAsync(Order order,decimal? price,decimal? amount,decimal? stopprice);
        protected async Task<IExchangeResultResponse<Order>> ChangeOrderAsync<TTempModel>(Order order, decimal? price, decimal? amount,decimal? stopprice)
        {
            var param = await BuilderParameters.CreateParamsChangeOrder(order,price,amount,stopprice);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.ChangeOrderRestEndpoint(order,price,amount,stopprice), EndpointsAndMethods.ChangeOrderHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterChangeOrderResult(res, order);
        }

        /// <summary>
        ///Close position 
        /// </summary>   
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<bool>> ClosePositionAsync(string pair, Sides? side);
        protected async Task<IExchangeResultResponse<bool>> ClosePositionAsync<TTempModel>(string pair, Sides? side)
        {
            var param = await BuilderParameters.CreateParamsClosePosition(pair, side);
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.ClosePositionRestEndpoint(pair, side), EndpointsAndMethods.ClosePositionHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterClosePositionResult(res);
        }

        /// <summary>
        ///Close positions
        /// </summary>   
        /// <returns></returns>
        public abstract Task<IExchangeResultResponse<bool>> ClosePositionsAsync();
        protected async Task<IExchangeResultResponse<bool>> ClosePositionsAsync<TTempModel>()
        {
            var param = await BuilderParameters.CreateParamsClosePositions();
            var res = await RestClient.SendRequestAsync<TTempModel>(EndpointsAndMethods.ClosePositionsRestEndpoint(), EndpointsAndMethods.ClosePositionsHttpMethods, param, Signed.Yes, ContentTypeForSender);
            return Converter.ConverterClosePositionsResult(res);
        }
        #endregion
        #endregion

        #region Socket

        #region Events
        // Public Data

        public event Action<IEnumerable<Trade>> NewTrades;
        public event Action<IEnumerable<Trade>> NewSellTrades;
        public event Action<IEnumerable<Trade>> NewBuyTrades;
        public event Action<OrderBook> UpdateOrderBook;
        public event Action<IEnumerable<Candle>> CandleSeriesProcessing;
        public event Action<Ticker> UpdateTicker;

        // Private Data
        public event Action<IEnumerable<Balance>> UpdateBalances;
        public event Action<IEnumerable<Position>> UpdatePositions;
        public event Action<IEnumerable<Order>> UpdateOrders;
        public event Action<IEnumerable<MyTrade>> UpdateMyTrades;
        public event Action<AccountUpdate> UpdateAccount;
        #endregion
        public virtual void ReconnectSocket()
        {
            SocketClient.Reconnect();
        }
    
        public void OnMessageReceived(object sender, EventArgs e)
        {
            try
            { 
                var result = Converter.ConvertSubscriptoionResponce((e as MessageReceivedEventArgs).Message);
                if (result == null || !result.ErrorResult.IsSuccess)
                {
                    return;
                }

                SendUpdate(new MessageExchangeResponse<object>(result.Data, result.ErrorResult, result.Exchange));
            }
            catch (Exception ex)
            {

            }
          
        }

        public void OnMessageReceived(object sender, MessageExchangeResponse<object> e)
        {
            if (e != null)
            SendUpdate(e);
        }

        private void SendUpdate(MessageExchangeResponse<object> update)
        {
            if (update.Data is IEnumerable<Trade> trades)
            {
                NewTrades?.Invoke(trades);
                NewSellTrades?.Invoke(trades.Where(t => t.Side == Sides.Sell));
                NewBuyTrades?.Invoke(trades.Where(t => t.Side == Sides.Buy));
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is OrderBook book)
            {
                UpdateOrderBook?.Invoke(book);
                ReceivedItems.Add(update.Data);
            }
            else if (update.Data is IEnumerable<Candle> candles)
            {
                CandleSeriesProcessing?.Invoke(candles);
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is Ticker ticker)
            {
                UpdateTicker?.Invoke(ticker);
                ReceivedItems.Add(update.Data);
            }
            else if (update.Data is IEnumerable<Position> positions)
            {
                UpdatePositions?.Invoke(positions);
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is IEnumerable<Order> orders)
            {
                UpdateOrders?.Invoke(orders);
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is IEnumerable<Balance> balances)
            {
                UpdateBalances?.Invoke(balances);
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is IEnumerable<MyTrade> myTrades)
            {
                UpdateMyTrades?.Invoke(myTrades);
                foreach (var res in update.Data as IEnumerable<object>)
                {
                    ReceivedItems.Add(res);
                }
            }
            else if (update.Data is OrderWithTradeUpdate orrTradeUpdate)
            {
                if (orrTradeUpdate.MyTrades is IEnumerable<object>)
                {
                    foreach (var res in orrTradeUpdate.MyTrades as IEnumerable<object>)
                    {
                        ReceivedItems.Add(res);
                    }
                }
                if (orrTradeUpdate.MyTrades != null)
                UpdateMyTrades?.Invoke(orrTradeUpdate.MyTrades);
                if (orrTradeUpdate.Orders is IEnumerable<object>)
                {
                    foreach (var res in orrTradeUpdate.Orders as IEnumerable<object>)
                    {
                        ReceivedItems.Add(res);
                    }
                }
                if (orrTradeUpdate.Orders != null)
                    UpdateOrders?.Invoke(orrTradeUpdate.Orders);
            }
            else if (update.Data is AccountUpdate accUpdate)
            {
                UpdateAccount?.Invoke(accUpdate);
               
                //if (accUpdate.Balances is IEnumerable<object>)
                //{
                //    foreach (var res in accUpdate.Balances as IEnumerable<object>)
                //    {
                //        ReceivedItems.Add(res);
                //    }
                //}
                //UpdateBalances?.Invoke(accUpdate.Balances);
                //if (accUpdate.Positions is IEnumerable<object>)
                //{
                //    foreach (var res in accUpdate.Positions as IEnumerable<object>)
                //    {
                //        ReceivedItems.Add(res);
                //    }
                //}
                //UpdatePositions?.Invoke(accUpdate.Positions);
            }

        }

        #region Public Socket Data
        public virtual void SubscribeToTrades(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToTradesParams(pair);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.TradeSocketEndpoint };
            SocketClient.Subscribe(endpoints, param);
        }

        public virtual void UnsubscribeToTrades(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToTradesParams(pair);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.TradeSocketUnsubscribeEndpoint };
            SocketClient.Subscribe(endpoints, param, true);
        }

        public virtual void SubscribeToOrderBook(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToOrderBookParams(pair);
            List<string> endpoints = new List<string>()
            {
                EndpointsAndMethods.OrderBookSocketEndpoint
            };
            SocketClient.Subscribe(endpoints, param);
        }

        public virtual void UnsubscribeToOrderBook(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToOrderBookParams(pair);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.OrderBookSocketUnsubscribeEndpoint };
            SocketClient.Subscribe(endpoints, param, true);
        }

        public virtual void SubscribeToCandles(string pair, int periodInSec)
        {
            var param = BuilderParameters.CreateSubscriptionToCandlesParams(pair, periodInSec);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.CandlesSocketEndpoint };
            SocketClient.Subscribe(endpoints, param);
        }

        public virtual void UnsubscribeToCandles(string pair, int periodInSec)
        {
            var param = BuilderParameters.CreateSubscriptionToCandlesParams(pair, periodInSec);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.CandlesSocketUnsubscribeEndpoint };
            SocketClient.Subscribe(endpoints, param, true);
        }

        public virtual void SubscribeToTicker(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToTickerParams(pair);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.TickerSocketEndpoint };
            SocketClient.Subscribe(endpoints, param);
        }


        public virtual void UnsubscribeToTicker(string pair)
        {
            var param = BuilderParameters.CreateSubscriptionToTickerParams(pair);
            List<string> endpoints = new List<string>() { EndpointsAndMethods.TickerSocketUnsubscribeEndpoint };
            SocketClient.Subscribe(endpoints, param, true);
        }
        #endregion

        #region Private Socket Data
        public virtual void SubscribeToPosition()
        {
            var param = BuilderParameters.CreateSubscriptionToPositionParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.PositionSocketEndpoint };
            SocketClient.SubscribeAuth(endpoints, param);
        }


        public virtual void UnsubscribeToPosition()
        {
            var param = BuilderParameters.CreateSubscriptionToPositionParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.PositionSocketUnsubscribeEndpoint };
            SocketClient.SubscribeAuth(endpoints, param, true);
        }

        public virtual void SubscribeToOrders()
        {
            var param = BuilderParameters.CreateSubscriptionToOrdersParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.OrdersSocketEndpoint };
            SocketClient.SubscribeAuth(endpoints, param);
        }


        public virtual void UnsubscribeToOrders()
        {
            var param = BuilderParameters.CreateSubscriptionToOrdersParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.OrdersSocketUnsubscribeEndpoint };
            SocketClient.SubscribeAuth(endpoints, param, true);
        }

        public virtual void SubscribeToBalances()
        {
            var param = BuilderParameters.CreateSubscriptionToBalancesParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.BalancesSocketEndpoint };
            SocketClient.SubscribeAuth(endpoints, param);
        }


        public virtual void UnsubscribeToBalances()
        {
            var param = BuilderParameters.CreateSubscriptionToBalancesParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.BalancesSocketUnsubscribeEndpoint };
            SocketClient.SubscribeAuth(endpoints, param, true);
        }

        public virtual void SubscribeToMyTrades()
        {
            var param = BuilderParameters.CreateSubscriptionToMyTradesParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.MyTradesSocketEndpoint };
            SocketClient.SubscribeAuth(endpoints, param);
        }


        public virtual void UnsubscribeToMyTrades()
        {
            var param = BuilderParameters.CreateSubscriptionToMyTradesParams();
            List<string> endpoints = new List<string>() { EndpointsAndMethods.MyTradesSocketUnsubscribeEndpoint };
            SocketClient.SubscribeAuth(endpoints, param, true);
        }



        #endregion

        #endregion
    }
}
