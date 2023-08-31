using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesMyTrade
    {

        [JsonProperty(PropertyName = "buyer")]
        public string Buyer { get; set; }

        [JsonProperty(PropertyName = "commission")]
        public string Commission { get; set; }

        [JsonProperty(PropertyName = "commissionAsset")]
        public string CommissionAsset { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "maker")]
        public string Maker { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "qty")]
        public string Qty { get; set; }

        [JsonProperty(PropertyName = "quoteQty")]
        public string QuoteQty { get; set; }

        [JsonProperty(PropertyName = "realizedPnl")]
        public string RealizedPnl { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
    }
}
