using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesTrade
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "qty")]
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "quoteQty")]
        public decimal QuoteQty { get; set; }

        [JsonProperty(PropertyName = "time")]
        public long Time { get; set; }

        [JsonProperty(PropertyName = "isBuyerMaker")]
        public bool IsBuyerMaker { get; set; }
            
    }
}
