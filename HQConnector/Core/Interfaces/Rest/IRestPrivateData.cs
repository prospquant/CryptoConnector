using System.Collections.Generic;
using System.Threading.Tasks;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Core.Interfaces.Rest
{
    public interface IRestPrivateData
    {

        Task<IExchangeResultResponse<IEnumerable<Balance>>> GetBalanceAsync();
        Task<IExchangeResultResponse<IEnumerable<Position>>> GetPositionsAsync();
        Task<IExchangeResultResponse<Order>> GetOrderInfoAsync(Order order);
        Task<IExchangeResultResponse<IEnumerable<Order>>> GetOrdersAsync();

        Task<IExchangeResultResponse<Order>> PlaceOrderAsync(Order order);
        Task<IExchangeResultResponse<Order>> CancelOrderAsync(Order order);
        Task<IExchangeResultResponse<bool>> CancelAllOrdersAsync();
        Task<IExchangeResultResponse<bool>> CancelAllLimitOrdersAsync(string pair);
        Task<IExchangeResultResponse<bool>> CancelAllStopOrdersAsync(string pair);
        Task<IExchangeResultResponse<bool>> CancelAllOrdersBySymbolAsync(string pair);
        Task<IExchangeResultResponse<bool>> CancelAllOrdersBySideAsync(string pair, Sides side);
        Task<IExchangeResultResponse<Order>> ChangeOrderAsync(Order order, decimal? price, decimal? amount, decimal? stopprice);
        Task<IExchangeResultResponse<bool>> ClosePositionAsync(string pair, Sides? side);
        Task<IExchangeResultResponse<bool>> ClosePositionsAsync();
    }
}