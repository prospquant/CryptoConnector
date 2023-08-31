using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesPositionSocket
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("pa")]
        public string Amount { get; set; }

        [JsonProperty("ep")]
        public string Price { get; set; }

        [JsonProperty("cr")]
        public string Comission { get; set; }

        [JsonProperty("up")]
        public string Pnl { get; set; }

        [JsonProperty("mt")]
        public string MarginType { get; set; }

        [JsonProperty("iw")]
        public string IsolatedMargin { get; set; }

        [JsonProperty("ps")]
        public string PositionSide { get; set; }
    }
}
