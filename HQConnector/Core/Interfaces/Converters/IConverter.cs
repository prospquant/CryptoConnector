using System.Collections.Generic;
using System.ComponentModel;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response.Interfaces;
using HQConnector.Dto.DTO.Symbol;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;

namespace HQConnector.Core.Interfaces.Converters
{
    public interface IConverterToBaseModel
    {
        #region Prop

        IConnectorCredentials Credentials { get; }

        #endregion

        #region Rest Public Data

        long ConvertServerTime<TTempModel>(TTempModel serverTime);

        IExchangeResultResponse<IEnumerable<Symbol>> ConverterSymbols<TTempModel>(TTempModel symbols);

        IExchangeResultResponse<IEnumerable<Symbol>> ConverterActiveSymbols<TTempModel>(TTempModel symbols);

        IExchangeResultResponse<IEnumerable<Trade>> ConverterTrades<TTempModel>(TTempModel trades, string pair);

        IExchangeResultResponse<Ticker> ConverterTicker<TTempModel>(TTempModel ticker, string pair);

        IExchangeResultResponse<OrderBook> ConverterOrderBook<TTempModel>(TTempModel orderBook, string pair, int depth, int merge);

        IExchangeResultResponse<IEnumerable<Candle>> ConverterCandleSeries<TTempModel>(TTempModel candles, string pair);

        #endregion

        #region Rest Private Data
        
        IExchangeResultResponse<IEnumerable<Balance>> ConverterBalances<TTempModel>(TTempModel balances);

        IExchangeResultResponse<IEnumerable<Position>> ConverterPositions<TTempModel>(TTempModel positions);

        IExchangeResultResponse<IEnumerable<MyTrade>> ConverterMyTrades<TTempModel>(TTempModel myTrades);

        IExchangeResultResponse<Order> ConverterGetOrderInfo<TTempModel>(TTempModel OrderinfoResult, Order order);

        IExchangeResultResponse<IEnumerable<Order>> ConverterGetOrders<TTempModel>(TTempModel allOrdersResult);

        IExchangeResultResponse<IEnumerable<Order>> ConverterGetActiveOrders<TTempModel>(TTempModel activeOrdersResult);
        
        IExchangeResultResponse<Order> ConverterPlaceOrderResult<TTempModel>(TTempModel placeOrderResult, Order order);
        
        IExchangeResultResponse<Order> ConverterCancelOrderResult<TTempModel>(TTempModel cancelOrderResult, Order order);

        IExchangeResultResponse<bool> ConverterCancelAllOrdersResult<TTempModel>(TTempModel cancelAllOrdersResult);

        IExchangeResultResponse<bool> ConverterCancelAllLimitOrdersResult<TTempModel>(TTempModel cancelAllLimitOrdersResult);        

        IExchangeResultResponse<bool> ConverterCancelAllStopOrdersResult<TTempModel>(TTempModel cancelAllOrdersResult);
        
        IExchangeResultResponse<bool> ConverterCancelAllOrdersBySymbolResult<TTempModel>(TTempModel cancelAllOrdersResult);

        IExchangeResultResponse<bool> ConverterCancelAllOrdersBySideResult<TTempModel>(TTempModel cancelAllOrdersResult);

        IExchangeResultResponse<Order> ConverterChangeOrderResult<TTempModel>(TTempModel changeOrderResult, Order order);

        IExchangeResultResponse<bool> ConverterClosePositionResult<TTempModel>(TTempModel closePositionResult);

        IExchangeResultResponse<bool> ConverterClosePositionsResult<TTempModel>(TTempModel closePositionsResult);
        #endregion

        #region Socket

        IExchangeResultResponse<object> ConvertSubscriptoionResponce(string message);

        #endregion
    }
}