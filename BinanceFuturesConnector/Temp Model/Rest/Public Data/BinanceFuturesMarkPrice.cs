using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesMarkPrice : BinanceFuturesBasePublicSocketResponse
    {
        [JsonProperty(PropertyName = "p")]
        public string MarkPrice { get; set; }

        [JsonProperty(PropertyName = "r")]
        public string FundingRate { get; set; }

        [JsonProperty(PropertyName = "T")]
        public string NextFundingTime { get; set; }
    }
}
