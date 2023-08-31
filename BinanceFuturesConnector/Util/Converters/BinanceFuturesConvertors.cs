

using HQConnector.Dto.DTO.Enums.Orders;

namespace BinanceFuturesConnector.Util.Converters
{
   public static class BinanceFuturesConvertors
    {
        public static int GetDepthSize(int depthSize)
        {
            int depth;
            if (depthSize <= 5)
            {
                depth = 5;
            }
            else if (depthSize <= 10)
            {
                depth = 10;
            }
            else if (depthSize <= 20)
            {
                depth = 20;
            }
            else if (depthSize <= 50)
            {
                depth = 50;
            }
            else if (depthSize <= 100)
            {
                depth = 100;
            }
            else if (depthSize <= 500)
            {
                depth = 500;
            }
            else
            {
                depth = 1000;
            }

            return depth;
        }

        public static string GetCandleInterval(int periodInSec)
        {
            if (periodInSec <= 60)
                return "1m";
            else if (periodInSec <= 180)
                return "3m";
            else if (periodInSec <= 300)
                return "5m";
            else if (periodInSec <= 900)
                return "15m";
            else if (periodInSec <= 1800)
                return "30m";
            else if (periodInSec <= 3600)
                return "1h";
            else if (periodInSec <= 2 * 3600)
                return "2h";
            else if (periodInSec <= 4 * 3600)
                return "4h";
            else if (periodInSec <= 6 * 3600)
                return "6h";
            else if (periodInSec <= 8 * 3600)
                return "8h";
            else if (periodInSec <= 12 * 3600)
                return "12h";
            else if (periodInSec <= 24 * 3600)
                return "1d";
            else if (periodInSec <= 72 * 3600)
                return "3d";
            else if (periodInSec <= 7 * 24 * 3600)
                return "1w";
            else if (periodInSec <= 30 * 24 * 3600)
                return "1M";
            return "1m";
        }

        public static string ReturnBinanceFuturesOrderType(OrderType ordertype)
        {
            string type = "";
            switch (ordertype)
            {
                case OrderType.Limit:
                    {
                        type = "LIMIT";
                        break;
                    }
                case OrderType.Market:
                    {
                        type = "MARKET";
                        break;
                    }
                case OrderType.StopLimit:
                    {
                        type = "STOP";
                        break;
                    }
                case OrderType.StopMarket:
                    {
                        type = "STOP_MARKET";
                        break;
                    }
                case OrderType.TakeProfitLimit:
                    {
                        type = "TAKE_RPOFIT";
                        break;
                    }
                case OrderType.TakeProfitMarket:
                    {
                        type = "TAKE_RPOFIT_MARKET";
                        break;
                    }
            }

            return type;
        }

        public static OrderType ReturnOrderType(string ordertype)
        {
            var type = OrderType.None;
            if (ordertype == "LIMIT")
            {
                type = OrderType.Limit;
            }
            else if (ordertype == "MARKET")
            {
                type = OrderType.Market;
            }
            else if (ordertype == "STOP")
            {
                type = OrderType.StopLimit;
            }
            else if (ordertype == "STOP_MARKET")
            {
                type = OrderType.StopMarket;
            }
            else if (ordertype == "TAKE_PROFIT")
            {
                type = OrderType.TakeProfitLimit;
            }
            else if (ordertype == "TAKE_PROFIT_MARKET")
            {
                type = OrderType.TakeProfitMarket;
            }
            return type;
        }
        public static OrderState ReturnOrderState(string status)
        {
            var state = OrderState.NONE;
            if (status == "NEW")
            {
                state = OrderState.OPEN;
            }
            else if (status == "PARTIALLY_FILLED")
            {
                state = OrderState.PARTIALLYFILLED;
            }
            else if (status == "FILLED")
            {
                state = OrderState.FILLED;
            }
            else if (status == "CANCELED")
            {
                state = OrderState.CANCELED;
            }
            else if (status == "REJECTED")
            {
                state = OrderState.REJECTED;
            }
            else if (status == "EXPIRED")
            {
                state = OrderState.TRIGGERED;
            }
            else if (status == "REPLACED")
            {
                state = OrderState.NONE;
            }
            else if (status == "STOPPED")
            {
                state = OrderState.NONE;
            }
            else if (status == "NEW_INSURANCE")
            {
                state = OrderState.NONE;
            }
            else if (status == "NEW_ADL")
            {
                state = OrderState.NONE;
            }            

            return state;
        }

        
    }
}
