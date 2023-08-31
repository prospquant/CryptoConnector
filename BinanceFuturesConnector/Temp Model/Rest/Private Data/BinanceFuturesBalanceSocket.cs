using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesBalanceSocket
    {
        [JsonProperty("a")]
        public string Symbol { get; set; }

        [JsonProperty("wb")]
        public string WalletBalance { get; set; }

        [JsonProperty("cw")]
        public string CrossWalletBalance { get; set; }

    }
}
