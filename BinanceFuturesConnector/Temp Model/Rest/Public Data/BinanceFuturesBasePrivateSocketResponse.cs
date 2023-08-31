using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesBasePrivateSocketResponse
    {
        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventTime { get; set; }

        [JsonProperty("T")]
        public string Transaction { get; set; }
    }
}
