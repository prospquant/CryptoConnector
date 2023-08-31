using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{ 
    public class BinanceFuturesTickerSocket
    {
    public string stream { get; set; }

    public BinanceFuturesTickerSocketData Data { get; set; }
    }
public class BinanceFuturesTickerSocketData : BinanceFuturesBasePublicSocketResponse
{
        [JsonProperty("p")]
        public string PriceChange { get; set; }

        [JsonProperty("P")]
        public string PriceChangePercent { get; set; }

        [JsonProperty("w")]
        public string WeightedAveragePrice { get; set; }

        [JsonProperty("c")]
        public string LastPrice { get; set; }
       
        [JsonProperty("Q")]
        public string LastQuantity { get; set; }
         

        [JsonProperty("o")]
        public string OpenPrice { get; set; }

        [JsonProperty("h")]
        public string HighPrice { get; set; }

        [JsonProperty("l")]
        public string LowPrice { get; set; }

        [JsonProperty("v")]
        public string TotalTradeVolume { get; set; }

        [JsonProperty("q")]
        public string TotalTradeQuoteVolume { get; set; }

        [JsonProperty("O")]
        public long O { get; set; }

        [JsonProperty("C")]
        public long C { get; set; }

        [JsonProperty("F")]
        public long FirstTradeId { get; set; }

        [JsonProperty("L")]
        public long LastTradeId { get; set; }

        [JsonProperty("n")]
        public long NumberOfTrades { get; set; }
    }
}
