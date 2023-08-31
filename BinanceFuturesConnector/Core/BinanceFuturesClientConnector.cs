using BinanceFuturesConnector.Core.Overrides_classes;
using BinanceFuturesConnector.Temp_Model.Rest;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data.Order;
using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using HQConnector.Core.Classes.Connector;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Enums.Position;
using HQConnector.Dto.DTO.Enums.Sender;
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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesConnector.Core
{
   public class BinanceFuturesClientConnector : AbstractConnector
    {
        #region Properties and ctor

         public BinanceFuturesClientConnector(string connectorName) : base(connectorName, Exchange.BinanceFutures)
        {
            Init();
        }

        public BinanceFuturesClientConnector(string connectorName, string apiKey, string secretKey) : base(connectorName, Exchange.BinanceFutures, apiKey, secretKey)
        {
            Init();
        }


        AccountUpdater accountUpdater;

        public int BalanceSocketUpdateInterval { get; set; } = 10;

        public int PositionSocketUpdateInterval { get; set; } = 3;

        private void Init()
        {
            IsFutures = true;
            ContentTypeForSender = ContentType.Query;
            EndpointsAndMethods = new BinanceFuturesEndpointAndMethods();
            RestClient = new BinanceFuturesSender(EndpointsAndMethods.BaseUrlRestEndpoint, Credentials, 150, 300);
            Converter = new BinanceFuturesConverterToBaseModel(Credentials);
            BuilderParameters = new BinanceFuturesBuilderParams(RestClient as BinanceFuturesSender);
            SocketClient = new BinanceFuturesSocketClient(EndpointsAndMethods.BaseUrlSocketEndpoint, Credentials);
            SocketClient.MessageReceived += OnMessageReceived;
            RestSettings = new BinanceFuturesRestClientSettings();
            ReceivedItems = new ConcurrentBag<object>();
            accountUpdater = new AccountUpdater(Credentials, RestClient as BinanceFuturesSender);
            accountUpdater.AcccountUpdated += OnMessageReceived;
         
           

        }

        



        #endregion

        #region Rest Public data

        public override Task<long> GetServerTimeAsync()
        {
            return GetServerTimeAsync<BinanceFuturesServerTime>();
         
        }

        public override Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetSymbolsAsync(int? maxcount)
        {
            return GetSymbolsAsync<BinanceFuturesSymbolResponse>(maxcount);
        }

        public override Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetActiveSymbolsAsync()
        {
            return GetActiveSymbolsAsync<BinanceFuturesSymbolResponse>();
        }
        public override Task<IExchangeResultResponse<Ticker>> GetTickerAsync(string pair)
        {

            return GetTickerAsync<BinanceFuturesTicker>(pair);
        }

        public async Task<IExchangeResultResponse<TickerPrice>> GetTickerPriceAsync(string pair)
        {
            var param = ((BinanceFuturesBuilderParams)BuilderParameters).CreateParamsGetTickerPrice(pair);          
            var res = await RestClient.SendRequestAsync<BinanceFuturesTickerPrice>(new BinanceFuturesEndpointAndMethods().GetTickerPriceRestEndpoint(pair), new BinanceFuturesEndpointAndMethods().TickerPriceHttpMethods, param, Signed.No, ContentTypeForSender);
            return new BinanceFuturesConverterToBaseModel(Credentials).ConverterTickerPrice(res, pair);

        }

        public override Task<IExchangeResultResponse<OrderBook>> GetOrderBookAsync(string pair, int depth, int merge = 0)
        {
            return GetOrderBookAsync<BinanceFuturesOrderBook>(pair, depth, merge);
        }

        public override Task<IExchangeResultResponse<IEnumerable<Candle>>> GetCandleSeriesAsync(string pair, int depth, long? fromDate, long? toDate = null, int? count = null)
        {
            return GetCandleSeriesAsync<object[][]>(pair, depth, fromDate, toDate, count);
        }

        public override Task<IExchangeResultResponse<IEnumerable<Trade>>> GetTradesAsync(string pair, int? maxCount)
        {
            return GetTradesAsync<IEnumerable<BinanceFuturesTrade>>(pair, maxCount);
        }


        public override async Task<IExchangeResultResponse<IEnumerable<Trade>>> GetHistoryTradesAsync(string pair, int? maxCount,string fromId)
        {
            var countTrades = maxCount != null && maxCount >= new BinanceFuturesRestClientSettings().MaxCountHistoryTrades ? new BinanceFuturesRestClientSettings().MaxCountHistoryTrades : maxCount;
            List<Trade> listTrades = new List<Trade>();
            IExchangeResultResponse<IEnumerable<Trade>> tradesResponse;

            var param = ((BinanceFuturesBuilderParams)BuilderParameters).CreateParamsGetHistoryTrades(pair, countTrades, fromId);
            var res = await RestClient.SendRequestAsync<IEnumerable<BinanceFuturesTrade>>(new BinanceFuturesEndpointAndMethods().GetHistoryTradesRestEndpoint(pair, countTrades, fromId),
                new BinanceFuturesEndpointAndMethods().HistoryTradesHttpMethods, param, Signed.Yes, ContentTypeForSender);
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

        public async Task<IExchangeResultResponse<IEnumerable<Trade>>> GetAggregateTradesAsync(string pair, long? fromId, long? startTime, long? endTime, int? maxCount)
        {
            var countTrades = maxCount != null && maxCount >= new BinanceFuturesRestClientSettings().MaxCountAggregateTrades ? new BinanceFuturesRestClientSettings().MaxCountAggregateTrades : maxCount;
            List<Trade> listTrades = new List<Trade>();
            IExchangeResultResponse<IEnumerable<Trade>> tradesResponse;

            var param = ((BinanceFuturesBuilderParams)BuilderParameters).CreateParamsGetAggregateTrades(pair, fromId, startTime, endTime, countTrades);
            var res = await RestClient.SendRequestAsync<IEnumerable<BinanceFuturesAggregateTrade>>(new BinanceFuturesEndpointAndMethods().GetAggregateTradesRestEndpoint(pair, fromId, startTime, endTime, countTrades),
                new BinanceFuturesEndpointAndMethods().AggregateTradesHttpMethods, param, Signed.No, ContentTypeForSender);
            tradesResponse = new BinanceFuturesConverterToBaseModel(Credentials).ConverterAggregateTrades(res, pair);
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

        #region Rest private data
        public async Task<IExchangeResultResponse<Leverage>> SetLeverageAsync(string pair,int leverage)
        {
           
            IExchangeResultResponse<Leverage> LeverageResponse;

            var param = await ((BinanceFuturesBuilderParams)BuilderParameters).CreateParamsSetLeverage(pair, leverage);
            var res = await RestClient.SendRequestAsync<BinanceFuturesSetLeverage>(new BinanceFuturesEndpointAndMethods().SetLeverageRestEndpoint(),
                new BinanceFuturesEndpointAndMethods().SetLeverageHttpMethods, param, Signed.Yes, ContentTypeForSender);
            LeverageResponse = new BinanceFuturesConverterToBaseModel(Credentials).ConverterSetLeverage(res);
            if (LeverageResponse != null && LeverageResponse.ErrorResult.IsSuccess)
            {
                return new MessageExchangeResponse<Leverage>(LeverageResponse.Data, new SuccessResponse(), Credentials.Exchange);
            }
            else
            {
                return new MessageExchangeResponse<Leverage>(null, LeverageResponse.ErrorResult, Credentials.Exchange);
            }
           
        }
        public override Task<IExchangeResultResponse<IEnumerable<Balance>>> GetBalanceAsync()
        {
            return GetBalanceAsync<BinanceFuturesBalanceInfo>();
        }

        public override Task<IExchangeResultResponse<IEnumerable<Position>>> GetPositionsAsync()
        {
            return GetPositionsAsync<IEnumerable<BinanceFuturesPositions>>();
        }

        public override Task<IExchangeResultResponse<IEnumerable<MyTrade>>> GetMyTradesAsync(string pair, long? startTime, long? endTime, long? fromId, int? maxCount)
        {
            return GetMyTradesAsync<IEnumerable<BinanceFuturesMyTrade>>(pair, startTime, endTime, fromId, maxCount);

        }

        public override Task<IExchangeResultResponse<Order>> GetOrderInfoAsync(Order order)
        {
            return GetOrderInfoAsync<BinanceFuturesOrderResponse>(order);
        }

        public override Task<IExchangeResultResponse<IEnumerable<Order>>> GetOrdersAsync()
        {
            return GetOrdersAsync<IEnumerable<BinanceFuturesOrderResponse>>();
        }

        public override Task<IExchangeResultResponse<IEnumerable<Order>>> GetActiveOrdersAsync()
        {
            return GetActiveOrdersAsync<IEnumerable<BinanceFuturesOrderResponse>>();
        }

        public override Task<IExchangeResultResponse<Order>> PlaceOrderAsync(Order order)
        {
            return PlaceOrderAsync<BinanceFuturesOrderResponse>(order);
        }

        public override Task<IExchangeResultResponse<Order>> CancelOrderAsync(Order order)
        {
            return CancelOrderAsync<BinanceFuturesOrderResponse>(order);
        }
             
        public override Task<IExchangeResultResponse<bool>> CancelAllOrdersBySymbolAsync(string pair)
        {
            return CancelAllOrdersBySymbolAsync<BinanceFuturesErrorResponse>(pair);
        }

        public override async Task<IExchangeResultResponse<bool>> CancelAllOrdersBySideAsync(string pair,Sides side)
        {
            var orders = await GetActiveOrdersAsync();
            if (orders != null && orders.ErrorResult.IsSuccess && orders.Data != null)
            {
                if (String.IsNullOrEmpty(pair))
                {
                    var ordersforcancel = orders.Data.Where(p => p.Side == side).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }                        
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
                else
                {
                    var ordersforcancel = orders.Data.Where(p => p.Side == side && p.Pair == pair).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }                      
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
            }
            return new MessageExchangeResponse<bool>(false,new CancelAllOrdersError(), Credentials.Exchange);
        }

        public override async Task<IExchangeResultResponse<bool>> CancelAllOrdersAsync()
        {
            var orders = await GetActiveOrdersAsync();
            if (orders != null && orders.ErrorResult.IsSuccess && orders.Data != null)
            {
                var ordersforcancelsymbols = orders.Data.Select(p => p.Pair).ToList();
                if (ordersforcancelsymbols != null && ordersforcancelsymbols.Count() != 0)
                {
                    foreach (var ordersymbol in ordersforcancelsymbols)
                    {
                        var responce = await CancelAllOrdersBySymbolAsync(ordersymbol);
                        if (responce == null || !responce.ErrorResult.IsSuccess)
                        {
                            return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                        }
                    }                    
                }
                return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
            }
            else
            {
                return new MessageExchangeResponse<bool>(false, orders.ErrorResult, Credentials.Exchange);
            }
           
        }

        public override async Task<IExchangeResultResponse<bool>> CancelAllLimitOrdersAsync(string pair)
        {
            var orders = await GetActiveOrdersAsync();
            if (orders != null && orders.ErrorResult.IsSuccess && orders.Data != null)
            {
                if (String.IsNullOrEmpty(pair))
                {
                    var ordersforcancel = orders.Data.Where(p => p.Type == OrderType.Limit).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
                else
                {
                    var ordersforcancel = orders.Data.Where(p => p.Type == OrderType.Limit && p.Pair == pair).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
            }
            return new MessageExchangeResponse<bool>(false, new CancelAllOrdersError(), Credentials.Exchange);
        }
        public override async Task<IExchangeResultResponse<bool>> CancelAllStopOrdersAsync(string pair)
        {
            var orders = await GetActiveOrdersAsync();
            if (orders != null && orders.ErrorResult.IsSuccess && orders.Data != null)
            {
                if (String.IsNullOrEmpty(pair))
                {
                    var ordersforcancel = orders.Data.Where(p => p.Type != OrderType.Limit).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }                        
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
                else
                {
                    var ordersforcancel = orders.Data.Where(p => p.Type != OrderType.Limit && p.Pair == pair).ToList();
                    if (ordersforcancel != null && ordersforcancel.Count() != 0)
                    {
                        foreach (var order in ordersforcancel)
                        {
                            var responce = await CancelOrderAsync(order);
                            if (responce == null || !responce.ErrorResult.IsSuccess)
                            {
                                return new MessageExchangeResponse<bool>(false, responce.ErrorResult, Credentials.Exchange);
                            }
                        }                     
                    }
                    return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                }
            }
            return new MessageExchangeResponse<bool>(false, new CancelAllOrdersError(), Credentials.Exchange);
        }
        public override async Task<IExchangeResultResponse<Order>> ChangeOrderAsync(Order order,decimal? price,decimal? amount,decimal? stopprice)
        {
            var cancelOrder = await CancelOrderAsync(order);
            if (cancelOrder != null && cancelOrder.ErrorResult.IsSuccess)
            {
                var newprice = order.Price;
                if (price.HasValue)
                {
                    newprice = price ?? 0;
                }
                var newamount = order.Amount;
                if (amount.HasValue)
                {
                    newamount = amount ?? 0;
                }
                var newstopprice = order.StopPrice;
                if (stopprice.HasValue)
                {
                    newstopprice = stopprice ?? 0;
                }
                var neword = new Order(order.Exchange, order.Account, order.Pair, order.Side, order.Type, newprice, newamount, newstopprice, order.StopWorkingType);
                var orderNewId = await PlaceOrderAsync(neword);

                if (orderNewId != null && orderNewId.ErrorResult.IsSuccess)
                {
                     return new MessageExchangeResponse<Order>(orderNewId.Data, new SuccessResponse(), Credentials.Exchange);
                }
                else
                {
                    return new MessageExchangeResponse<Order>(null, new ChangeOrderError("Old order Cancel, but new don`t placed"), Credentials.Exchange);
                }
            }


            return new MessageExchangeResponse<Order>(null, new ChangeOrderError("Can`t change order"), Credentials.Exchange);
        }

        public async Task<IExchangeResultResponse<bool>> ClosePositionAsync(string pair, PositionSide side,decimal sizeValue)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Try close position on " + pair);
            if (!String.IsNullOrEmpty(pair))
            {
                var positions = await GetPositionsAsync();
                if (positions != null && positions.ErrorResult.IsSuccess && positions.Data != null)
                {
                    sb.AppendLine("Position list taken ");
                    if (side == PositionSide.LONG)
                    {
                        var positionforclose = positions.Data.Where(p => p.Pair == pair && p.Size > 0).ToList();
                        if (positionforclose.Count() != 0)
                        {
                            foreach (var position in positionforclose)
                            {
                                sb.AppendLine("Position for close: " + position.ToString());
                                var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, Sides.Sell, OrderType.Market,true, 0, Math.Abs(sizeValue), 0, null);
                                var responce = await PlaceOrderAsync(order);
                               
                                if (responce == null)
                                {
                                    sb.AppendLine("responce == null");
                                    return new MessageExchangeResponse<bool>(false, new ClosePositionError(sb.ToString()), Credentials.Exchange);
                                }
                                if (!responce.ErrorResult.IsSuccess)
                                {
                                    sb.AppendLine(responce.ErrorResult.Code.ToString());
                                    sb.AppendLine(responce.ErrorResult.Message.ToString());
                                    sb.AppendLine(responce.ErrorResult.ErrorType.ToString("G"));
                                    sb.AppendLine(responce.ErrorResult.ToString());
                               
                                    return new MessageExchangeResponse<bool>(false, new ClosePositionError(sb.ToString()), Credentials.Exchange);
                                }
                            }                                
                        }
                        return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                    }
                    else if (side == PositionSide.SHORT)
                    {
                        var positionforclose = positions.Data.Where(p => p.Pair == pair && p.Size < 0).ToList();
                        if (positionforclose.Count() != 0)
                        {
                            foreach (var position in positionforclose)
                            {
                                sb.AppendLine("Position for close: " + position.ToString());
                                var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, Sides.Buy, OrderType.Market,true, 0, Math.Abs(sizeValue), 0, null);
                               
                                var responce = await PlaceOrderAsync(order);
                               
                                if (responce == null)
                                {
                                    sb.AppendLine("responce == null");
                                    return new MessageExchangeResponse<bool>(false, new ClosePositionError(sb.ToString()), Credentials.Exchange);
                                }
                                if (!responce.ErrorResult.IsSuccess)
                                {
                                    sb.AppendLine(responce.ErrorResult.Code.ToString());
                                    sb.AppendLine(responce.ErrorResult.Message.ToString());
                                    sb.AppendLine(responce.ErrorResult.ErrorType.ToString("G"));
                                    sb.AppendLine(responce.ErrorResult.ToString());

                                    return new MessageExchangeResponse<bool>(false, new ClosePositionError(sb.ToString()), Credentials.Exchange);
                                }
                            }                                
                        }
                        return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                    }                        
                    
                   
                }
            }

            sb.AppendLine("return from end of function");
            return new MessageExchangeResponse<bool>(false, new ClosePositionError(sb.ToString()), Credentials.Exchange);
        }

        public override async Task<IExchangeResultResponse<bool>> ClosePositionAsync(string pair, Sides? side)
        {
            if (!String.IsNullOrEmpty(pair))
            {
                var positions = await GetPositionsAsync();
                if (positions != null && positions.ErrorResult.IsSuccess && positions.Data != null)
                {
                    if (side.HasValue)
                    {
                        if (side == Sides.Buy)
                        {
                            var positionforclose = positions.Data.Where(p => p.Pair == pair && p.Size > 0).ToList();
                            if (positionforclose.Count() != 0)
                            {
                                foreach (var position in positionforclose)
                                {
                                    var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, Sides.Sell, OrderType.Market,true, 0, Math.Abs(position.Size), 0, null);
                                    var responce = await PlaceOrderAsync(order);
                                    if (responce == null)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(), Credentials.Exchange);
                                    }
                                    if (!responce.ErrorResult.IsSuccess)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(responce.ErrorResult.ToString()), Credentials.Exchange);
                                    }

                                   
                                }
                            }
                            return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                        }
                        else
                        {
                            var positionforclose = positions.Data.Where(p => p.Pair == pair && p.Size < 0).ToList();
                            if (positionforclose.Count() != 0)
                            {
                                foreach (var position in positionforclose)
                                {
                                    var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, Sides.Buy, OrderType.Market,true, 0, Math.Abs(position.Size), 0, null);
                                    var responce = await PlaceOrderAsync(order);
                                    if (responce == null)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(), Credentials.Exchange);
                                    }

                                    if (!responce.ErrorResult.IsSuccess)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(responce.ErrorResult.ToString()), Credentials.Exchange);
                                    }
                                }
                            }
                            return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                        }
                    }
                    else
                    {
                        var positionforclose = positions.Data.Where(p => p.Pair == pair).ToList();
                        if (positionforclose.Count() != 0)
                        {
                            foreach (var position in positionforclose)
                            {
                                if (position.Size != 0)
                                {
                                    var positionside = position.Size > 0 ? Sides.Sell : Sides.Buy;
                                    var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, positionside, OrderType.Market,true, 0, Math.Abs(position.Size), 0, null);
                                    var responce = await PlaceOrderAsync(order);
                                    if (responce == null)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(), Credentials.Exchange);
                                    }

                                    if (!responce.ErrorResult.IsSuccess)
                                    {
                                        return new MessageExchangeResponse<bool>(false, new ClosePositionError(responce.ErrorResult.ToString()), Credentials.Exchange);
                                    }
                                }
                            }
                        }
                        return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);
                    }
                }
            }
            return new MessageExchangeResponse<bool>(false, new ClosePositionError(), Credentials.Exchange);
        }

        public override async Task<IExchangeResultResponse<bool>> ClosePositionsAsync()
        {            
            var positions = await GetPositionsAsync();
            if (positions != null && positions.ErrorResult.IsSuccess && positions.Data != null)
            {
               
                foreach (var position in positions.Data)
                {
                    if (position.Size != 0)
                    {
                        var positionside = position.Size > 0 ? Sides.Sell : Sides.Buy;
                        var order = new Order(Credentials.Exchange, Credentials.ConnectorName, position.Pair, positionside, OrderType.Market,true, 0, Math.Abs(position.Size), 0, null);
                        var responce = await PlaceOrderAsync(order);
                        if (responce == null)
                        {
                            return new MessageExchangeResponse<bool>(false, new ClosePositionsError(), Credentials.Exchange);
                        }
                        if (!responce.ErrorResult.IsSuccess)
                        {
                            return new MessageExchangeResponse<bool>(false, new ClosePositionsError(responce.ErrorResult.ToString()), Credentials.Exchange);
                        }

                    }
                }
                return new MessageExchangeResponse<bool>(true, new SuccessResponse(), Credentials.Exchange);                

            }
            return new MessageExchangeResponse<bool>(false, new ClosePositionsError(), Credentials.Exchange);
        }
        #endregion

        #region Socket Private Data

        public async override void SubscribeToPosition()
        {
            try
            {
                var positons = await GetPositionsAsync();

                 await accountUpdater.Start("positions", positons.Data, PositionSocketUpdateInterval);
           
                UpdateAccount += mes => accountUpdater.AccountUpdate(mes);
                
                SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.PositionSocketEndpoint }, BuilderParameters.CreateSubscriptionToPositionParams());
            }
            catch (Exception ex)
            {

            }

        }

        public async override void UnsubscribeToPosition()
        {
            try
            {
                await accountUpdater.Stop("positions");
                if (!accountUpdater.IsPositionUpdate && !accountUpdater.IsBalanceUpdate)
                {
                    UpdateAccount -= mes => accountUpdater.AccountUpdate(mes);
                    SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.PositionSocketUnsubscribeEndpoint }, BuilderParameters.CreateSubscriptionToPositionParams(),true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async override void SubscribeToBalances()
        {
            try
            {
                var balances = await GetBalanceAsync();
                 await accountUpdater.Start("balances", balances.Data, BalanceSocketUpdateInterval);

                UpdateAccount += mes => accountUpdater.AccountUpdate(mes);
              
                    SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.BalancesSocketEndpoint }, BuilderParameters.CreateSubscriptionToBalancesParams());
            }
            catch (Exception ex)
            {

            }

        }

        public async override void UnsubscribeToBalances()
        {
            try
            {
                await accountUpdater.Stop("balances");
                if (!accountUpdater.IsPositionUpdate && !accountUpdater.IsBalanceUpdate)
                {
                    UpdateAccount -= mes => accountUpdater.AccountUpdate(mes);
                    SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.BalancesSocketUnsubscribeEndpoint }, BuilderParameters.CreateSubscriptionToBalancesParams(),false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async override void SubscribeToOrders()
        {
            try
            {
                
                    SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.OrdersSocketEndpoint }, BuilderParameters.CreateSubscriptionToOrdersParams(),false);
                
            }
            catch (Exception ex)
            {

            }
        }

        public async override void UnsubscribeToOrders()
        {
            try
            {
                SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.OrdersSocketUnsubscribeEndpoint }, BuilderParameters.CreateSubscriptionToOrdersParams(),true);
            }
            catch (Exception ex)
            {

            }
        }

        public async override void SubscribeToMyTrades()
        {
            try
            {
                
                    SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.MyTradesSocketEndpoint }, BuilderParameters.CreateSubscriptionToMyTradesParams());
            }
            catch (Exception ex)
            {

            }
        }

        public async override void UnsubscribeToMyTrades()
        {
            try
            {
                SocketClient.SubscribeAuth(new List<string> { EndpointsAndMethods.MyTradesSocketUnsubscribeEndpoint }, BuilderParameters.CreateSubscriptionToMyTradesParams(),true);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}
