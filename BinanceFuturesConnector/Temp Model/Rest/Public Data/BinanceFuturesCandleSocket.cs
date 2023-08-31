using Newtonsoft.Json;


namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesCandleSocket
    {
        public string stream { get; set; }

        public BinanceFuturesCandleSocketData Data { get; set; }
    }
    public class BinanceFuturesCandleSocketData : BinanceFuturesBasePublicSocketResponse
    {
        [JsonProperty("k")]
        public CandleBinanceFutures Candles { get; set; }
    }

    public class CandleBinanceFutures
    {
        [JsonProperty("t")]
        public long StartTime { get; set; }

        [JsonProperty("T")]
        public long CloseTime { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("i")]
        public string Interval { get; set; }

        [JsonProperty("f")]
        public long FirstTradeID { get; set; }

        [JsonProperty("L")]
        public long LastTradeID { get; set; }

        [JsonProperty("o")]
        public string OpenPrice { get; set; }

        [JsonProperty("c")]
        public string ClosePrice { get; set; }

        [JsonProperty("h")]
        public string HighPrice { get; set; }

        [JsonProperty("l")]
        public string LowPrice { get; set; }

        [JsonProperty("v")]
        public string Volume { get; set; }

        [JsonProperty("n")]
        public long N { get; set; }

        [JsonProperty("x")]
        public bool X { get; set; }

        [JsonProperty("q")]
        public string QuoteVolume { get; set; }

        [JsonProperty("V")]
        public string V { get; set; }

        [JsonProperty("Q")]
        public string Q { get; set; }

        [JsonProperty("B")]
        public string B { get; set; }
    }
}


