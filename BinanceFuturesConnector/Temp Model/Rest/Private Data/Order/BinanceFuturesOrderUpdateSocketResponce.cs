using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data.Order
{
    public class BinanceFuturesOrderUpdateSocketResponce : BinanceFuturesBasePrivateSocketResponse
    {
        [JsonProperty("o")]
        public BinanceFuturesSocketOrder BinanceFuturesSocketOrderResponse { get; set; }


    }

    public class BinanceFuturesSocketOrder
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("c")]
        public string ClientOrderId { get; set; }

        [JsonProperty("S")]
        public string Side { get; set; }

        [JsonProperty("o")]
        public string OrderType { get; set; }

        [JsonProperty("f")]
        public string TimeInForce { get; set; }

        [JsonProperty("q")]
        public string OriginalQuantity { get; set; }

        [JsonProperty("p")]
        public string Price { get; set; }

        [JsonProperty("ap")]
        public string AveragePrice { get; set; }

        [JsonProperty("sp")]
        public string StopPrice { get; set; }

        [JsonProperty("x")]
        public string ExecutionType { get; set; }

        [JsonProperty("X")]
        public string OrderStatus { get; set; }

        [JsonProperty("i")]
        public string OrderId { get; set; }

        [JsonProperty("l")]
        public string OrderLastFilledQuantity { get; set; }

        [JsonProperty("z")]
        public string OrderFilledAccumulatedQuantity { get; set; }

        [JsonProperty("L")]
        public string LastFilledPrice { get; set; }

        [JsonProperty("N")]
        public string CommissionAsset { get; set; }

        [JsonProperty("n")]
        public string Commission { get; set; }

        [JsonProperty("T")]
        public string OrderTradeTime { get; set; }

        [JsonProperty("t")]
        public string TradeId { get; set; }

        [JsonProperty("b")]
        public string BidsNotional { get; set; }

        [JsonProperty("a")]
        public string AskNotional { get; set; }

        [JsonProperty("m")]
        public string IsMaker { get; set; }

        [JsonProperty("R")]
        public string IsReduceOnly { get; set; }

        [JsonProperty("wt")]
        public string StopPriceType { get; set; }
    }
}
