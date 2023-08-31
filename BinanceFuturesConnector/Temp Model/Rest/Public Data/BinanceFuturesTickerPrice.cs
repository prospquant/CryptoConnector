using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesTickerPrice
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }
    }
}
