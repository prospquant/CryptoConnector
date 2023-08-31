using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Response.Interfaces;
using HQConnector.Dto.DTO.Symbol;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;

namespace HQConnector.Core.Interfaces.Rest
{
    public interface IRestPublicData
    {
        Task<long> GetServerTimeAsync();
        Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetSymbolsAsync(int? maxcount);

        Task<IExchangeResultResponse<IEnumerable<Symbol>>> GetActiveSymbolsAsync();
        Task<IExchangeResultResponse<Ticker>> GetTickerAsync(string pair);
        Task<IExchangeResultResponse<IEnumerable<Trade>>> GetTradesAsync(string pair, int? maxCount);
        Task<IExchangeResultResponse<IEnumerable<Trade>>> GetHistoryTradesAsync(string pair, int? maxCount,string fromId);
        Task<IExchangeResultResponse<IEnumerable<Candle>>> GetCandleSeriesAsync(string pair, int periodInSec, long? fromDate, long? toDate = null, int? count = null);
        Task<IExchangeResultResponse<OrderBook>> GetOrderBookAsync(string pair, int depth, int merge = 0);

    }
}
