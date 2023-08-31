using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesOrderBook
    {
        [JsonProperty(PropertyName = "lastUpdateId")]
        public long LastUpdateId { get; set; }

        [JsonProperty(PropertyName = "bids")]
        public List<List<object>> Bids { get; set; }

        [JsonProperty(PropertyName = "asks")]
        public List<List<object>> Asks { get; set; }
    }
}
