using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
   public class BinanceFuturesAggregateTrade
    {
        [JsonProperty("a")]
        public string AggregatetradeId { get; set; }

        [JsonProperty("p")]
        public string Price { get; set; }

        [JsonProperty("q")]
        public string Quantity { get; set; }

        [JsonProperty("f")]
        public string FirsttradeId { get; set; }

        [JsonProperty("l")]
        public string LasttradeId { get; set; }

        [JsonProperty("T")]
        public string Timestamp { get; set; }

        [JsonProperty("m")]
        public string IsBuyerMaker { get; set; }


    }
}
