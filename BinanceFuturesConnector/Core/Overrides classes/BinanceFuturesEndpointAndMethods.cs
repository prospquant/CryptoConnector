using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Connectors.ConnectorEndpoint;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Order;


namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesEndpointAndMethods : IConnectorEndpointsAndMethods
    {
        
        public string BaseUrlRestEndpoint => "https://fapi.binance.com";

        #region Rest Public Data

        public string GetServerTimeRestEndpoint => "/fapi/v1/time";

        public HttpMethods ServerTimeHttpMethods => HttpMethods.GET;


        public string GetSymbolsRestEndpoint => "/fapi/v1/exchangeInfo";

        public HttpMethods SymbolsHttpMethods => HttpMethods.GET;

        public string GetActiveSymbolsRestEndpoint => "/fapi/v1/exchangeInfo";

        public HttpMethods ActiveSymbolsHttpMethods => HttpMethods.GET;

        public string OrderBookRestEndpoint(string pair, int depth, int merge)
        {
            return "/fapi/v1/depth";
        }
        public HttpMethods OrderBookHttpMethods => HttpMethods.GET;

        public string GetTickerPriceRestEndpoint(string pair)
        {
            return "/fapi/v1/ticker/price";
        }
        public HttpMethods TickerPriceHttpMethods => HttpMethods.GET;

        public string GetTickerRestEndpoint(string pair)
        {
            return "/fapi/v1/ticker/24hr";
        }
        public HttpMethods TickerHttpMethods => HttpMethods.GET;

        public string GetCandlesRestEndpoint(string pair, int periodInSec, long? fromDate, long? toDate = null, int? count = null)
        {
            return "/fapi/v1/klines";
        }

        public HttpMethods CandlesHttpMethods => HttpMethods.GET;

        public string GetTradesRestEndpoint(string pair, int? maxCount)
        {
            return "/fapi/v1/trades";
        }

        public HttpMethods TradesHttpMethods => HttpMethods.GET;

        public string GetHistoryTradesRestEndpoint(string pair, int? maxCount,string fromId)
        {
            return "/fapi/v1/historicalTrades";
        }

        public HttpMethods HistoryTradesHttpMethods => HttpMethods.GET;

        public string GetAggregateTradesRestEndpoint(string pair, long? fromId, long? startTime, long? endTime, int? maxCount)
        {
            return "/fapi/v1/aggTrades";
        }

        public HttpMethods AggregateTradesHttpMethods => HttpMethods.GET;
        #endregion

        #region Rest Private Data
        public string SetLeverageRestEndpoint()
        {
            return "/fapi/v1/leverage";
        }
        public HttpMethods SetLeverageHttpMethods => HttpMethods.POST;
        public string BalancesRestEndpoint()
        {
            return "/fapi/v1/account";
        }

        public HttpMethods BalancesHttpMethods => HttpMethods.GET;

      
        public string GetPositionsRestEndpoint()
        {
            return "/fapi/v1/positionRisk";
        }

        public HttpMethods PositionsHttpMethods => HttpMethods.GET;

        public string GetMyTradesRestEndpoint(string pair, long? startTime, long? endTime, long? fromId, int? maxCount)
        {
            return "/fapi/v1/userTrades";
        }

        public HttpMethods MyTradesHttpMethods => HttpMethods.GET;

        public string GetOrderInfoRestEndpoint(Order order)
        {
            return "/fapi/v1/order";
        }
        public HttpMethods GetOrderInfoHttpMethods => HttpMethods.GET;

        public string GetOrdersRestEndpoint()
        {
            return "/fapi/v1/allOrders";
        }
        public HttpMethods GetOrdersHttpMethods => HttpMethods.GET;

        public string GetActiveOrdersRestEndpoint()
        {
            return "/fapi/v1/openOrders";
        }
        public HttpMethods GetActiveOrdersHttpMethods => HttpMethods.GET;

        public string PlaceOrderRestEndpoint(Order order)
        {
            return "/fapi/v1/order";
        }

        public HttpMethods PlaceOrderHttpMethods => HttpMethods.POST;

        public string CancelOrderRestEndpoint(Order order)
        {
            return "/fapi/v1/order";
        }

        public HttpMethods CancelOrderHttpMethods => HttpMethods.DELETE;

        public string CancelAllOrdersRestEndpoint()
        {
            return null;
        }
        public HttpMethods CancelAllOrdersHttpMethods => HttpMethods.DELETE;

        public string CancelAllLimitOrdersRestEndpoint(string pair)
        {
            return null;
        }
        public HttpMethods CancelAllLimitOrdersHttpMethods => HttpMethods.DELETE;
        public string CancelAllStopOrdersRestEndpoint(string pair)
        {
            return null;
        }
        public HttpMethods CancelAllStopOrdersHttpMethods => HttpMethods.DELETE;

        public string CancelAllOrdersBySymbolRestEndpoint(string pair)
        {
            return "/fapi/v1/allOpenOrders";
        }
        public HttpMethods CancelAllOrdersBySymbolHttpMethods => HttpMethods.DELETE;

        public string CancelAllOrdersBySideRestEndpoint(string pair,Sides side)
        {
            return null;
        }
        public HttpMethods CancelAllOrdersBySideHttpMethods => HttpMethods.DELETE;

        public string ChangeOrderRestEndpoint(Order order,decimal? price,decimal? amount,decimal? stopprice)
        {
            return null;
        }

        public HttpMethods ChangeOrderHttpMethods { get; set; }

        public string ClosePositionRestEndpoint(string pair, Sides? side)
        {
            return null;
        }
        public HttpMethods ClosePositionHttpMethods => HttpMethods.DELETE;

        public string ClosePositionsRestEndpoint()
        {
            return null;
        }
        public HttpMethods ClosePositionsHttpMethods => HttpMethods.DELETE;
        #endregion

        public string BaseUrlSocketEndpoint => "wss://fstream3.binance.com/stream";

        #region Public Socket Streams
        public string OrderBookSocketEndpoint => "subscribe";
        public string OrderBookSocketUnsubscribeEndpoint => "unsubscribe";

        public string TickerSocketEndpoint => "subscribe";

        public string TickerSocketUnsubscribeEndpoint => "unsubscribe";

        public string TradeSocketEndpoint => "subscribe";

        public string TradeSocketUnsubscribeEndpoint => "unsubscribe";

        public string CandlesSocketEndpoint => "subscribe";

        public string CandlesSocketUnsubscribeEndpoint => "unsubscribe";

        #endregion

        #region Private Socket Streams
      
        public string BalancesSocketEndpoint => "subscribe";

        public string BalancesSocketUnsubscribeEndpoint => "unsubscribe";      

        public string PositionSocketEndpoint => "subscribe";

        public string PositionSocketUnsubscribeEndpoint => "unsubscribe";

        public string OrdersSocketEndpoint => "subscribe";

        public string OrdersSocketUnsubscribeEndpoint => "unsubscribe";

        public string MyTradesSocketEndpoint => "subscribe";

        public string MyTradesSocketUnsubscribeEndpoint => "unsubscribe";

        #endregion
    }
}
