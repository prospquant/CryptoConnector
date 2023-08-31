using HQConnector.Core.Enums;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Order;

namespace HQConnector.Core.Interfaces.Connectors.ConnectorEndpoint
{
    public interface IConnectorEndpointsAndMethods
    {
        #region Rest Public Data

        string BaseUrlRestEndpoint { get; }

        string GetServerTimeRestEndpoint { get;   }
        HttpMethods ServerTimeHttpMethods { get;   }

        string GetSymbolsRestEndpoint { get; }
        HttpMethods SymbolsHttpMethods { get; }

        string GetActiveSymbolsRestEndpoint { get; }
        HttpMethods ActiveSymbolsHttpMethods { get; }

        string OrderBookRestEndpoint(string pair, int depth, int merge);
        HttpMethods OrderBookHttpMethods { get;   }

       
        string GetTickerRestEndpoint(string pair);
        HttpMethods TickerHttpMethods { get; }

        string GetCandlesRestEndpoint(string pair, int periodInSec, long? fromDate, long? toDate = null, int? count = null);
        HttpMethods CandlesHttpMethods { get;   }

        string GetTradesRestEndpoint(string pair, int? maxCount);
        HttpMethods TradesHttpMethods { get; }

        string GetHistoryTradesRestEndpoint(string pair, int? maxCount,string fromId);
        HttpMethods HistoryTradesHttpMethods { get; }



        #endregion

        #region Rest Private Data

        string BalancesRestEndpoint();
        HttpMethods BalancesHttpMethods { get; }

        string GetPositionsRestEndpoint();
        HttpMethods PositionsHttpMethods { get; }

        string GetMyTradesRestEndpoint(string pair, long? startTime, long? endTime, long? fromId, int? maxCount);
        HttpMethods MyTradesHttpMethods { get; }

        string GetOrderInfoRestEndpoint(Order order);
        HttpMethods GetOrderInfoHttpMethods { get; }

        string GetOrdersRestEndpoint();
        HttpMethods GetOrdersHttpMethods { get; }

        string GetActiveOrdersRestEndpoint();
        HttpMethods GetActiveOrdersHttpMethods { get; }
        
        string PlaceOrderRestEndpoint(Order order);
        HttpMethods PlaceOrderHttpMethods { get;   }        

        string CancelOrderRestEndpoint(Order order);
        HttpMethods CancelOrderHttpMethods { get;   }    

        string CancelAllOrdersRestEndpoint();
        HttpMethods CancelAllOrdersHttpMethods { get;   }
        string CancelAllLimitOrdersRestEndpoint(string pair);
        HttpMethods CancelAllLimitOrdersHttpMethods { get; }
        
        string CancelAllStopOrdersRestEndpoint(string pair);
        HttpMethods CancelAllStopOrdersHttpMethods { get; }
        
        string CancelAllOrdersBySymbolRestEndpoint(string pair);
        HttpMethods CancelAllOrdersBySymbolHttpMethods { get; }

        string CancelAllOrdersBySideRestEndpoint(string pair,Sides side);
        HttpMethods CancelAllOrdersBySideHttpMethods { get; }

        string ChangeOrderRestEndpoint(Order order,decimal? price,decimal? amount,decimal? stopprice);
        HttpMethods ChangeOrderHttpMethods { get; }

        string ClosePositionRestEndpoint(string pair, Sides? side);
        HttpMethods ClosePositionHttpMethods { get; }

        string ClosePositionsRestEndpoint();
        HttpMethods ClosePositionsHttpMethods { get; }
        #endregion

        #region Socket

        #region Public 

        string BaseUrlSocketEndpoint { get; }

        string OrderBookSocketEndpoint { get; }
        string OrderBookSocketUnsubscribeEndpoint { get; }

        string TradeSocketEndpoint { get; }
        string TradeSocketUnsubscribeEndpoint { get; }

        string CandlesSocketEndpoint { get; }
        string CandlesSocketUnsubscribeEndpoint { get; }

        string TickerSocketEndpoint { get; }
        string TickerSocketUnsubscribeEndpoint { get; }


        #endregion

        #region Private

        string PositionSocketEndpoint { get; }
        string PositionSocketUnsubscribeEndpoint { get; }

        string OrdersSocketEndpoint { get; }
        string OrdersSocketUnsubscribeEndpoint { get; }

        string BalancesSocketEndpoint { get; }
        string BalancesSocketUnsubscribeEndpoint { get; }

        string MyTradesSocketEndpoint { get; }
        string MyTradesSocketUnsubscribeEndpoint { get; }

        #endregion

        #endregion

    }
}