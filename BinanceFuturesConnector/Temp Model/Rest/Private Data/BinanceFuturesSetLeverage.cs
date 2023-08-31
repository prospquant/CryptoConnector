using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesSetLeverage
    {
        [JsonProperty(PropertyName = "leverage")]
        public int Leverage { get; set; }

        [JsonProperty(PropertyName = "maxNotionalValue")]
        public string MaxNotionalValue { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

       
    }
}
