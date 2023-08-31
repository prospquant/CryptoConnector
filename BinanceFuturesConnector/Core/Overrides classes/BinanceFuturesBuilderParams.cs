using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using BinanceFuturesConnector.Util.Converters;
using HQConnector.Core.Classes.Credentials;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.BuilderParameters;
using HQConnector.Dto.DTO.Enums.Orders;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Order;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesBuilderParams : IBuilderParameters
    {
        #region Ctor
        BinanceFuturesSender restsender;
        public BinanceFuturesBuilderParams(BinanceFuturesSender sender)
        {
            restsender = sender;
        }
        private async Task<long> CalcServerTime()
        {
            try
            {

                var time = await restsender.SendRequestAsync<BinanceFuturesServerTime>("/fapi/v1/time", HttpMethods.GET,
                    new Dictionary<string, object>
                    {

                    }, Signed.No, ContentType.Query);
                if (time == null)
                {
                    var restsend = new BinanceFuturesSender("https://api3.binance.com/", new ConnectorCredentials("", HQConnector.Dto.DTO.Enums.Exchange.Exchange.BinanceFutures), 150, 300);
                    time = await restsend.SendRequestAsync<BinanceFuturesServerTime>("api/v3/time", HttpMethods.GET,
                  new Dictionary<string, object>
                  {

                  }, Signed.No, ContentType.Query);
                }
                return time?.Data.ServerTime ?? 0;
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        
        #endregion

        #region Rest Public Data
        public Dictionary<string, object> CreateParamsGetServerTime()
        {
            return null;
        }

        public Dictionary<string, object> CreateParamsGetSymbols(int? maxcount)
        {
            if (maxcount.HasValue)
            {
                var param = new Dictionary<string, object>
                {
                    ["count"] = maxcount
                };
                return param;
            }
            return null;
        }

        public Dictionary<string, object> CreateParamsGetActiveSymbols()
        {
            return null;
        }
        public Dictionary<string, object> CreateParamsGetTicker(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair
            };
            return param;
        }

        public Dictionary<string, object> CreateParamsGetTickerPrice(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair
            };
            return param;
        }
        public Dictionary<string, object> CreateParamsGetOrderBook(string pair, int depth, int merge = 0)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair,
                ["limit"] = BinanceFuturesConvertors.GetDepthSize(depth)
            };
            return param;
        }

        public Dictionary<string, object> CreateParamsGetCandleSeries(string pair, int periodInSec, long? fromDate, long? toDate, int? count)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair,
                ["interval"] = BinanceFuturesConvertors.GetCandleInterval(periodInSec)
            };
            if (fromDate.HasValue)
                param.Add("startTime", fromDate);
            if (toDate.HasValue)
                param.Add("endTime", toDate);
            if (count.HasValue)
                param.Add("limit", count);
            return param;
        }
        public Dictionary<string, object> CreateParamsGetTrades(string pair, int? maxCount)
        {
          
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair              
            };
            if (maxCount.HasValue)
                param.Add("limit", maxCount);
            return param;
        }

        public Dictionary<string, object> CreateParamsGetHistoryTrades(string pair,int? maxCount,string fromId)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair
              
            };
            if (maxCount.HasValue)
                param.Add("limit", maxCount);
            if (!String.IsNullOrEmpty(fromId))
                param.Add("fromId", fromId);
            return param;
        }

        public Dictionary<string, object> CreateParamsGetAggregateTrades(string pair, long? fromId, long? startTime, long? endTime, int? maxCount)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair,
               
            };
            if (fromId.HasValue)
                param.Add("fromId", fromId);
            if (startTime.HasValue)
                param.Add("startTime", startTime);
            if (endTime.HasValue)
                param.Add("endTime", endTime);
            if (maxCount.HasValue)
                param.Add("limit", maxCount);
            return param;
        }

        #endregion

        #region Rest Private Data
        public async Task<Dictionary<string, object>> CreateParamsSetLeverage(string pair,int leverage)
        {

            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair,
                ["leverage"] = leverage,
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsGetBalance()
        {

            var param = new Dictionary<string, object>
            {
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            return param;
        }

      
        public async Task<Dictionary<string, object>> CreateParamsGetPositions()
        {
            var param = new Dictionary<string, object>
            {
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsGetMyTrades(string pair,long? startTime,long? endTime,long? fromId,int? maxCount)
        {
            var param = new Dictionary<string, object>
            {
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            if (!String.IsNullOrEmpty(pair))
                param.Add("symbol", pair);
            if (startTime.HasValue)
                param.Add("startTime", startTime);
            if (endTime.HasValue)
                param.Add("endTime", endTime);
            if (fromId.HasValue)
                param.Add("fromId", fromId);
            if (maxCount.HasValue)
                param.Add("limit", maxCount);
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsGetOrderInfo(Order order)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = order.Pair,
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            if (!String.IsNullOrEmpty(order.Id))
                param.Add("orderId", order.Id);
            if (!String.IsNullOrEmpty(order.ClId))
                param.Add("origClientOrderId", order.ClId);
            
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsGetOrders()
        {
            var param = new Dictionary<string, object>
            {
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            return param;
        }
        public async Task<Dictionary<string, object>> CreateParamsGetActiveOrders()
        {
            var param = new Dictionary<string, object>
            {
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsPlaceOrder(Order order)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = order.Pair,
                ["side"] = order.Side == Sides.Buy ? "BUY" : "SELL",
                ["type"] =  BinanceFuturesConvertors.ReturnBinanceFuturesOrderType(order.Type), 
                ["quantity"] = order.Amount.ToString(CultureInfo.InvariantCulture),
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
            if (order.Type == OrderType.Limit || order.Type == OrderType.LimitMaker)
            {
                param.Add("timeInForce", "GTC");
            }
            if (order.Type == OrderType.StopLimit || order.Type == OrderType.StopMarket || order.Type == OrderType.TakeProfitLimit || order.Type == OrderType.TakeProfitMarket)
            {
                param.Add("priceProtect", "false");
            }
            if (order.ReduceOnly == true)
            {
                param.Add("reduceOnly", "true");
            }
            if (!String.IsNullOrEmpty(order.Price.ToString()) && order.Price != 0)
                param.Add("price", order.Price.ToString(CultureInfo.InvariantCulture));
            if (!String.IsNullOrEmpty(order.StopPrice.ToString()) && order.StopPrice != 0)
                param.Add("stopPrice", order.StopPrice.ToString(CultureInfo.InvariantCulture));
            if (!String.IsNullOrEmpty(order.StopWorkingType))
                param.Add("workingType", order.StopWorkingType);
          
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsCancelOrder(Order order)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = order.Pair,
                ["orderId"] = order.Id,
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };

            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsCancelAllOrders()
        {
            return null;
        }

        public async Task<Dictionary<string, object>> CreateParamsCancelAllLimitOrders(string pair)
        {
            return null;
        }
        public async Task<Dictionary<string, object>> CreateParamsCancelAllStopOrders(string pair)
        {
            return null;
        }
        public async Task<Dictionary<string, object>> CreateParamsCancelAllOrdersBySymbol(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["symbol"] = pair,
                ["timestamp"] = await CalcServerTime(),
                ["recvWindow"] = 30000
            };
         
            return param;
        }

        public async Task<Dictionary<string, object>> CreateParamsCancelAllOrdersBySide(string pair,Sides side)
        {
            return null;
        }

        public async Task<Dictionary<string, object>> CreateParamsChangeOrder(Order order,decimal? price,decimal? amount,decimal? stopprice)
        {
            return null;
        }

        public async Task<Dictionary<string, object>> CreateParamsClosePosition(string pair, Sides? side)
        {
            return null;
        }

        public async Task<Dictionary<string, object>> CreateParamsClosePositions()
        {
            return null;
        }
        #endregion

        #region Public Socket
        public Dictionary<string, object> CreateSubscriptionToTickerParams(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = $"{pair.ToLower()}@ticker"
            };
            return param;
        }
        public Dictionary<string, object> CreateSubscriptionToOrderBookParams(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = $"{pair.ToLower()}@depth@0ms"
            };
            return param;
        }

        public Dictionary<string, object> CreateSubscriptionToCandlesParams(string pair, int periodInSec)
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = $"{pair.ToLower()}@kline_{BinanceFuturesConvertors.GetCandleInterval(periodInSec)}"
              
            };
            return param;
        }

        public Dictionary<string, object> CreateSubscriptionToTradesParams(string pair)
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = $"{pair.ToLower()}@aggTrade"
            };
            return param;
        }
        #endregion

        #region Socket Private Data
        public Dictionary<string, object> CreateSubscriptionToPositionParams()
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = "positions"
            };
            return param;
        }

        public Dictionary<string, object> CreateSubscriptionToOrdersParams()
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = "orders"
            };
            return param;
        }

        public Dictionary<string, object> CreateSubscriptionToMyTradesParams()
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = "mytrades"
            };
            return param;
        }

        public Dictionary<string, object> CreateSubscriptionToBalancesParams()
        {
            var param = new Dictionary<string, object>
            {
                ["params"] = "balances"
            };
            return param;
        }

       
        #endregion

      
    }
}
