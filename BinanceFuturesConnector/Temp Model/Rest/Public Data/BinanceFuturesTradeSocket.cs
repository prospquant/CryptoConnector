using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesTradeSocket
    {
        public string stream { get; set; }

        public BinanceFuturesTradeSocketData data { get; set; }
    }
    public class BinanceFuturesTradeSocketData : BinanceFuturesBasePublicSocketResponse
    {
      
        [JsonProperty("a")]
        public string Id { get; set; }

        [JsonProperty("p")]
        public string Price { get; set; }

        [JsonProperty("q")]
        public string Quantity { get; set; }

        [JsonProperty("f")]
        public string FirstTradeId { get; set; }

        [JsonProperty("l")]
        public string LastTradeId { get; set; }

        [JsonProperty("T")]
        public string TradeTime { get; set; }

        [JsonProperty("m")]
        public string IsBuy { get; set; }


    }
}
