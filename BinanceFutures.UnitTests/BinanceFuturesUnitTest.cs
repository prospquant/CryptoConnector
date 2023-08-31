using BinanceFuturesConnector.Core;
using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Candle;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.OrderBook;
using HQConnector.Dto.DTO.Position;
using HQConnector.Dto.DTO.Response.Enums;
using HQConnector.Dto.DTO.Ticker;
using HQConnector.Dto.DTO.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BinanceFutures.UnitTests
{
    public class BinanceFuturesUnitTest
    {
        //todo ключи не рабочие
        private string Username = "test";
        private string ApiKey = "";
        private string SecretKey = "";

        private string ApiKey2 = "";
        private string SecretKey2 = "";
        private string Pair = "BTCUSDT";
        private int Count = 100;
        private int counter = 0;

        private BinanceFuturesClientConnector Connector;

        public BinanceFuturesUnitTest()
        {
            Connector = new BinanceFuturesClientConnector(Username, ApiKey2, SecretKey2);
        }

        #region Public data

        [Fact]
        public async Task GetServerTime()
        {
            var time = await Connector.GetServerTimeAsync();
            var b = 0;
        }

        [Fact]
        public async Task GetSymbols()
        {
            var symbols = await Connector.GetSymbolsAsync(null);
            Assert.NotNull(symbols);
            Assert.True(symbols.ErrorResult.IsSuccess);
            Assert.NotNull(symbols.Data);
        }

        [Fact]
        public async Task GetActiveSymbols()
        {
            var symbols = await Connector.GetActiveSymbolsAsync();
            Assert.NotNull(symbols);
            Assert.True(symbols.ErrorResult.IsSuccess);
            Assert.NotNull(symbols.Data);
        }

        [Fact]
        public async Task GetTicker()
        {
            var ticker = await Connector.GetTickerAsync(Pair);
            Assert.NotNull(ticker);
            Assert.True(ticker.ErrorResult.IsSuccess);
            Assert.NotNull(ticker.Data);
        }

        [Fact]
        public async Task GetTickerPrice()
        {
            var tickerprice = await Connector.GetTickerPriceAsync(Pair);
            Assert.NotNull(tickerprice);
            Assert.True(tickerprice.ErrorResult.IsSuccess);
            Assert.NotNull(tickerprice.Data);
        }

        [Fact]
        public async Task GetOrderBook()
        {
            var orderBook = await Connector.GetOrderBookAsync(Pair, Count);
            Assert.NotNull(orderBook);
            Assert.True(orderBook.ErrorResult.IsSuccess);
            Assert.NotNull(orderBook.Data);
            Assert.Equal(Count, orderBook.Data.Asks.Count);
            Assert.Equal(Count, orderBook.Data.Bids.Count);
            
        }
        
        [Fact]
        public async Task GetCandles()
        {
            var candles = await Connector.GetCandleSeriesAsync(Pair, 5, null);
        
            Assert.NotNull(candles);
            Assert.True(candles.ErrorResult.IsSuccess);
            Assert.NotNull(candles.Data);
        }

        [Fact]
        public async Task GetTrades()
        {
            var trades = await Connector.GetTradesAsync(Pair, null);            
            Assert.NotNull(trades);
            Assert.True(trades.ErrorResult.IsSuccess);
            Assert.NotNull(trades.Data);
        }

        [Fact]
        public async Task GetHistoryTrades()
        {
            var trades = await Connector.GetHistoryTradesAsync(Pair,1000,"0");
            Assert.NotNull(trades);
            Assert.True(trades.ErrorResult.IsSuccess);
            Assert.NotNull(trades.Data);
        }

        [Fact]
        public async Task GetAggregateTrades()
        {
            var trades = await Connector.GetAggregateTradesAsync(Pair,  null, null,null,10000);
            Assert.NotNull(trades);
            Assert.True(trades.ErrorResult.IsSuccess);
            Assert.NotNull(trades.Data);
        }


        #endregion

        #region Rest Private Data
        [Fact]
        public async void GetLeverage()
        {
            var leverage = await Connector.SetLeverageAsync("BTCUSDT", 50);
            Assert.NotNull(leverage);
            Assert.True(leverage.ErrorResult.IsSuccess);
            Assert.NotNull(leverage.Data);
        }

        [Fact]
        public async void GetBalance()
        {
            var balances = await Connector.GetBalanceAsync();
            Assert.NotNull(balances);
            Assert.True(balances.ErrorResult.IsSuccess);
            Assert.NotNull(balances.Data);
        }

        [Fact]
        public async void GetPositions()
        {
            var positions = await Connector.GetPositionsAsync();
            Assert.NotNull(positions);
            Assert.True(positions.ErrorResult.IsSuccess);
            Assert.NotNull(positions.Data);
            var a = positions.Data.Where(p => p.Size != 0).ToList();
            var b = 0;
        }

        [Fact]
        public async void GetMyTrades()
        {
            var myTrades = await Connector.GetMyTradesAsync("BTCUSDT", null, null, null,null);
            Assert.NotNull(myTrades);
            Assert.True(myTrades.ErrorResult.IsSuccess);
            Assert.NotNull(myTrades.Data);
        }

        [Fact]
        public async void PlaceAndCancelOrder()
        {
            var order = new Order(Connector.Credentials.Exchange, Connector.Credentials.ConnectorName, "BTCUSDT"
                , Sides.Buy,OrderType.Market, 0, 0.05M,0,null);

            //var order = new Order
            //{
            //    Exchange = Connector.Credentials.Exchange,
            //    Pair = Pair,
            //    Side = Sides.Buy,
            //    Type = OrderType.Limit,
            //    Amount = 0.001m,
            //    Price = 9400
            //};
            var idOrder = await Connector.PlaceOrderAsync(order);
            Assert.Equal(ResultType.Success, idOrder.ErrorResult.ErrorType);
            order.Id = idOrder.Data.Id;
            var res = await Connector.CancelOrderAsync(order);
            Assert.Equal(ResultType.Success, res.ErrorResult.ErrorType);

        }

        [Fact]
        public async void PlaceAndCancelOrder1()
        {
            var order = new Order(Connector.Credentials.Exchange, Connector.Credentials.ConnectorName, "YFIUSDT"
                , Sides.Buy, OrderType.Market, 0, 0.05M, 0, null);

            //var order = new Order
            //{
            //    Exchange = Connector.Credentials.Exchange,
            //    Pair = Pair,
            //    Side = Sides.Buy,
            //    Type = OrderType.Limit,
            //    Amount = 0.001m,
            //    Price = 9400
            //};
            var idOrder = await Connector.PlaceOrderAsync(order);
            Assert.Equal(ResultType.Success, idOrder.ErrorResult.ErrorType);
            order.Id = idOrder.Data.Id;
            var res = await Connector.CancelOrderAsync(order);
            Assert.Equal(ResultType.Success, res.ErrorResult.ErrorType);

        }

        [Fact]
        public async void PlaceOrder()
        {
            var order = new Order(Connector.Credentials.Exchange, Connector.Credentials.ConnectorName, Pair
                , Sides.Buy, OrderType.StopMarket, 6410, 0.001m, default, null);

            
            var idOrder = await Connector.PlaceOrderAsync(order);
            Assert.Equal(ResultType.Success, idOrder.ErrorResult.ErrorType);
           

        }

        [Fact]
        public async void GetOrderInfo()
        {
            var order = new Order(Connector.Credentials.Exchange, Connector.Credentials.ConnectorName, Pair, "2772307633", null);
            var orderinfo = await Connector.GetOrderInfoAsync(order);
            Assert.NotNull(orderinfo);
            Assert.True(orderinfo.ErrorResult.IsSuccess);
            Assert.NotNull(orderinfo.Data);
        }

        [Fact]
        public async void GetOrders()
        {
            var orders = await Connector.GetOrdersAsync();      
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);
            Assert.NotNull(orders.Data);
        }

        [Fact]
        public async void GetActiveOrders()
        {
            var orders = await Connector.GetActiveOrdersAsync();
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);
            Assert.NotNull(orders.Data);
        }

        [Fact]
        public async void CancelOrder()
        {
            var order = await Connector.GetActiveOrdersAsync();
            Assert.NotNull(order.Data);
            if (order != null && order.Data != null && order.Data.FirstOrDefault() != null)
            {
                var res = await Connector.CancelOrderAsync(order.Data.FirstOrDefault());
                Assert.Equal(ResultType.Success, res.ErrorResult.ErrorType);
            }

        }
        [Fact]
        public async void CancelAllOrdersBySymbol()
        {
            var orders = await Connector.CancelAllOrdersBySymbolAsync(Pair);
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void CancelAllOrdersBySide()
        {
            var orders = await Connector.CancelAllOrdersBySideAsync(null,Sides.Buy);
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void CancelAllOrders()
        {
            var orders = await Connector.CancelAllOrdersAsync();
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void CancelAllLimitOrders()
        {
            var orders = await Connector.CancelAllLimitOrdersAsync(Pair);
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void CancelAllStopOrders()
        {
            var orders = await Connector.CancelAllStopOrdersAsync(Pair);
            Assert.NotNull(orders);
            Assert.True(orders.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void ChangeOrder()
        {
            var order = new Order(Connector.Credentials.Exchange, Connector.Credentials.ConnectorName, Pair
               , Sides.Buy, OrderType.Limit, 6410, 0.001m, default, null);

          
            var idOrder = await Connector.PlaceOrderAsync(order);
            var changeorder = await Connector.ChangeOrderAsync(idOrder.Data,6520, 0.002m, null);
            Assert.NotNull(changeorder);
            Assert.True(changeorder.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void ClosePosition()
        {
            var position = await Connector.ClosePositionAsync("ETHUSDT",Sides.Buy);
            Assert.NotNull(position);
            Assert.True(position.ErrorResult.IsSuccess);

        }

        [Fact]
        public async void ClosePositions()
        {
            var position = await Connector.ClosePositionsAsync();
            Assert.NotNull(position);
            Assert.True(position.ErrorResult.IsSuccess);

        }
        #endregion

        #region Public Socket Data
        [Fact]
        public async Task TestTradeSubscription()
        {
            
            Connector.NewTrades += SocketTrades;
            Connector.SubscribeToTrades(Pair);
            await Task.Delay(100000);
         
        }

        private void SocketTrades(IEnumerable<Trade> obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestTradeUnSubscribe()
        {

            Connector.NewTrades += SocketTrades2;
            Connector.SubscribeToTrades(Pair);
            await Task.Delay(100000);

        }

        private void SocketTrades2(IEnumerable<Trade> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 10)
            {
                Connector.UnsubscribeToTrades(Pair);
            }
        }
        [Fact]
        public async Task TestTickerSubscription()
        {

            Connector.UpdateTicker += UpdateTicker;
            Connector.SubscribeToTicker(Pair);
            await Task.Delay(10000);

        }

        private void UpdateTicker(Ticker obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestTickerUnSubscribe()
        {

            Connector.UpdateTicker += UpdateTicker2;
            Connector.SubscribeToTicker(Pair);
            await Task.Delay(100000);

        }

        private void UpdateTicker2(Ticker obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 10)
            {
                Connector.UnsubscribeToTicker(Pair);
            }
        }

        [Fact]
        public async Task TestCandleSubscription()
        {

            Connector.CandleSeriesProcessing += NewCandles;
            Connector.SubscribeToCandles(Pair,55);
            await Task.Delay(100000);

        }

        private void NewCandles(IEnumerable<Candle> obj)
        {
            Assert.NotNull(obj);
            if (obj.First().IsClosed == true)
            {
                var a = 0;
            }
        }

        [Fact]
        public async Task TestCandleUnSubscribe()
        {

            Connector.CandleSeriesProcessing += NewCandles2;
            Connector.SubscribeToCandles(Pair, 56);
            await Task.Delay(100000);

        }

        private void NewCandles2(IEnumerable<Candle> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 10)
            {
                Connector.UnsubscribeToCandles(Pair,56);
            }
        }

        [Fact]
        public async Task TestOrderBookSubscription()
        {

            Connector.UpdateOrderBook += OrderBookUpdate;
            Connector.SubscribeToOrderBook(Pair);
            await Task.Delay(10000);

        }
        private void OrderBookUpdate(OrderBook obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestOrderBookUnSubscribe()
        {

            Connector.UpdateOrderBook += OrderBookUpdate2;
            Connector.SubscribeToOrderBook(Pair);
            await Task.Delay(100000);

        }
        private void OrderBookUpdate2(OrderBook obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 1000)
            {
                Connector.UnsubscribeToOrderBook(Pair);
            }
        }
        #endregion

        #region Private Socket Data
        [Fact]
        public async Task TestBalanceSubscription()
        {

            Connector.UpdateBalances += BalanceUpdate;
            Connector.SubscribeToBalances();
            await Task.Delay(10000000);

        }

        private void BalanceUpdate(IEnumerable<Balance> obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestBalanceUnsubscribe()
        {

            Connector.UpdateBalances += BalanceUpdate2;
            Connector.SubscribeToBalances();
            await Task.Delay(10000000);

        }

        private void BalanceUpdate2(IEnumerable<Balance> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 20)
            {
                Connector.UnsubscribeToBalances();
            }
        }

        [Fact]
        public async Task TestPositionsSubscription()
        {

            Connector.UpdatePositions += PositionsUpdate;
            Connector.SubscribeToPosition();
            await Task.Delay(10000000);

        }

        private void PositionsUpdate(IEnumerable<Position> obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestPositionsUnsubscribe()
        {

            Connector.UpdatePositions += PositionsUpdate2;
            Connector.SubscribeToPosition();
            await Task.Delay(10000000);

        }

        private void PositionsUpdate2(IEnumerable<Position> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 10)
            {
                Connector.UnsubscribeToPosition();
            }
        }

        [Fact]
        public async Task TestOrderSubscription()
        {

            Connector.UpdateOrders += OrderUpdate;
            Connector.SubscribeToOrders();
            await Task.Delay(1000000000);

        }

        private void OrderUpdate(IEnumerable<Order> obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestOrderUnsubscribe()
        {

            Connector.UpdateOrders += OrderUpdate2;
            Connector.SubscribeToOrders();
            await Task.Delay(10000000);

        }

        private void OrderUpdate2(IEnumerable<Order> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 6)
            {
                Connector.UnsubscribeToOrders();
            }
        }

        [Fact]
        public async Task TestMyTradeSubscription()
        {

            Connector.UpdateMyTrades += MyTradeUpdate;
            Connector.SubscribeToMyTrades();
            await Task.Delay(10000000);

        }

        private void MyTradeUpdate(IEnumerable<MyTrade> obj)
        {
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TestMyTradeUnsubscribe()
        {

            Connector.UpdateMyTrades += MyTradeUpdate2;
            Connector.SubscribeToMyTrades();
            await Task.Delay(10000000);

        }

        private void MyTradeUpdate2(IEnumerable<MyTrade> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 4)
            {
                Connector.UnsubscribeToMyTrades();
            }
        }
        #endregion

        #region Socket Reconnect Test

        [Fact]
        public async Task PublicDataReconnectSingle()
        {

            Connector.NewTrades += SocketTradesReconnect;
            Connector.SubscribeToTrades(Pair.ToLower());
            await Task.Delay(60000);
            Connector.SubscribeToTrades(Pair.ToLower());
            await Task.Delay(30000);
        }

        private void SocketTradesReconnect(IEnumerable<Trade> obj)
        {
            Assert.NotNull(obj);
            counter++;
            if (counter == 100)
            {
                Connector.UnsubscribeToTrades(Pair.ToLower());
            }
        }
        #endregion
    }
}
