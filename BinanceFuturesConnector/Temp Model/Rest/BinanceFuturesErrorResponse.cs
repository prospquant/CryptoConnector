using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest
{
    public class BinanceFuturesErrorResponse
    {

        [JsonProperty(PropertyName = "code")]
        public int? Code { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
    }
}
