using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class AuthResponce
    {
        [JsonProperty(PropertyName = "listenKey")]
        public string ListenKey { get; set; }
    }
}
