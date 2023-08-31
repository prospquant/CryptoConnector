using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
   public class BinanceFuturesServerTime
    {
        [JsonProperty(PropertyName = "serverTime")]
        public long ServerTime { get; set; }
    }
}
