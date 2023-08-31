using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
   public  class BinanceFuturesTicker
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "priceChange")]
        public decimal PriceChange { get; set; }

        [JsonProperty(PropertyName = "priceChangePercent")]
        public decimal PriceChangePercent { get; set; }

        [JsonProperty(PropertyName = "weightedAvgPrice")]
        public decimal WeightedAvgPrice { get; set; }

        [JsonProperty(PropertyName = "prevClosePrice")]
        public decimal PrevClosePrice { get; set; }

        [JsonProperty(PropertyName = "lastPrice")]
        public decimal LastPrice { get; set; }

        [JsonProperty(PropertyName = "lastQty")]
        public decimal LastQty { get; set; }

        [JsonProperty(PropertyName = "openPrice")]
        public decimal OpenPrice { get; set; }

        [JsonProperty(PropertyName = "highPrice")]
        public decimal HighPrice { get; set; }

        [JsonProperty(PropertyName = "lowPrice")]
        public decimal LowPrice { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public decimal Volume { get; set; }

        [JsonProperty(PropertyName = "quoteVolume")]
        public decimal QuoteVolume { get; set; }

        [JsonProperty(PropertyName = "openTime")]
        public long OpenTime { get; set; }

        [JsonProperty(PropertyName = "closeTime")]
        public long CloseTime { get; set; }

        [JsonProperty(PropertyName = "firstId")]
        public long FirstId { get; set; }

        [JsonProperty(PropertyName = "lastId")]
        public long LastId { get; set; }

        [JsonProperty(PropertyName = "count")]
        public long Count { get; set; }
    }
}

