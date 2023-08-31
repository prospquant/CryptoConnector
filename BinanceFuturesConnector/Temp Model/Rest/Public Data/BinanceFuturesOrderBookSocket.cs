using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesOrderBookSocket
    {
        public string stream { get; set; }

        public BinanceFuturesOrderBookSocketData data { get; set; }
    }
    public class BinanceFuturesOrderBookSocketData: BinanceFuturesBasePublicSocketResponse
    {
        [JsonProperty("U")]
        public long U { get; set; }

        [JsonProperty("u")]
        public long BinanceFuturesOrderBookSocketU { get; set; }

        [JsonProperty("b")]
        public string[][] Bids { get; set; }

        [JsonProperty("a")]
        public string[][] Asks { get; set; }
    }
}
