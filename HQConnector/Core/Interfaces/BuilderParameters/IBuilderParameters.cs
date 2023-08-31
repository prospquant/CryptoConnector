using System.Collections.Generic;
using System.Threading.Tasks;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Order;

namespace HQConnector.Core.Interfaces.BuilderParameters
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBuilderParameters
    {
        /// <summary>
        /// Разница между локальным временем и временем биржи
        /// </summary>
      

        #region Rest Public Data
        Dictionary<string, object> CreateParamsGetServerTime();
        Dictionary<string, object> CreateParamsGetSymbols(int? maxcount);

        Dictionary<string, object> CreateParamsGetActiveSymbols();

        Dictionary<string, object> CreateParamsGetOrderBook(string pair, int depth, int merge = 0);
        Dictionary<string, object> CreateParamsGetTicker(string pair);

     
        Dictionary<string, object> CreateParamsGetCandleSeries(string pair, int periodInSec, long? fromDate, long? toDate, int? count);

        Dictionary<string, object> CreateParamsGetTrades(string pair, int? maxCount);

        Dictionary<string, object> CreateParamsGetHistoryTrades(string pair, int? maxCount,string fromId);

        #endregion

        #region Private Rest Data

        Task<Dictionary<string, object>> CreateParamsGetBalance();
        Task<Dictionary<string, object>> CreateParamsGetPositions();
        Task<Dictionary<string, object>> CreateParamsGetMyTrades(string pair, long? startTime, long? endTime, long? fromId, int? maxCount);

        Task<Dictionary<string, object>> CreateParamsGetOrderInfo(Order order);
        Task<Dictionary<string, object>> CreateParamsGetOrders();
        Task<Dictionary<string, object>> CreateParamsGetActiveOrders();
        Task<Dictionary<string, object>> CreateParamsPlaceOrder(Order order);
        Task<Dictionary<string, object>> CreateParamsCancelOrder(Order order);
        Task<Dictionary<string, object>> CreateParamsCancelAllOrders();
        Task<Dictionary<string, object>> CreateParamsCancelAllLimitOrders(string pair);
        Task<Dictionary<string, object>> CreateParamsCancelAllStopOrders(string pair);
        Task<Dictionary<string, object>> CreateParamsCancelAllOrdersBySymbol(string pair);
        Task<Dictionary<string, object>> CreateParamsCancelAllOrdersBySide(string pair,Sides side);
        Task<Dictionary<string, object>> CreateParamsChangeOrder(Order order,decimal? price,decimal? amount,decimal? stopprice);
        Task<Dictionary<string, object>> CreateParamsClosePosition(string pair, Sides? side);
        Task<Dictionary<string, object>> CreateParamsClosePositions();


        #endregion

        Dictionary<string, object> CreateSubscriptionToTickerParams(string pair);

        Dictionary<string, object> CreateSubscriptionToOrderBookParams(string pair);

        Dictionary<string, object> CreateSubscriptionToCandlesParams(string pair, int periodInSec);

        Dictionary<string, object> CreateSubscriptionToTradesParams(string pair);



        Dictionary<string, object> CreateSubscriptionToPositionParams();
        Dictionary<string, object> CreateSubscriptionToOrdersParams();
        Dictionary<string, object> CreateSubscriptionToBalancesParams();
        Dictionary<string, object> CreateSubscriptionToMyTradesParams();



    }
}