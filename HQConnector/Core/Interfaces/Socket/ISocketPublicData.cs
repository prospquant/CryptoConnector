using System;
using System.Collections.Generic;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;

namespace HQConnector.Core.Interfaces.Socket
{
    public interface ISocketPublicData
    {
       

        event Action<IEnumerable<Candle>> CandleSeriesProcessing;
        event Action<OrderBook> UpdateOrderBook;
        event Action<IEnumerable<Trade>> NewBuyTrades;
        event Action<IEnumerable<Trade>> NewSellTrades;
        event Action<IEnumerable<Trade>> NewTrades;
        event Action<Ticker> UpdateTicker;

        
        void OnMessageReceived(object sender, EventArgs e);
        void ReconnectSocket();



        void SubscribeToCandles(string pair, int periodInSec);
        void SubscribeToOrderBook(string pair);
        void SubscribeToTrades(string pair);
        void SubscribeToTicker(string pair);

        void UnsubscribeToCandles(string pair, int periodInSec);
        void UnsubscribeToOrderBook(string pair);
        void UnsubscribeToTicker(string pair);
        void UnsubscribeToTrades(string pair);

    }
}