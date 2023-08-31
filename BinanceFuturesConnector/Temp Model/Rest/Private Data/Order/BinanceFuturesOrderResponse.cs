using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data.Order
{
   public class BinanceFuturesOrderResponse
    {
        [JsonProperty(PropertyName = "avgPrice")]
        public string AvgPrice { get; set; }

        [JsonProperty(PropertyName = "clientOrderId")]
        public string ClientOrderId { get; set; }

        [JsonProperty(PropertyName = "cumQuote")]
        public string CumQuote { get; set; }

        [JsonProperty(PropertyName = "executedQty")]
        public string ExecutedQty { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "origQty")]
        public string OrigQty { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "reduceOnly")]
        public string ReduceOnly { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "stopPrice")]
        public string StopPrice { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "timeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "updateTime")]
        public string UpdateTime { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "workingType")]
        public string WorkingType { get; set; }
    }
}
