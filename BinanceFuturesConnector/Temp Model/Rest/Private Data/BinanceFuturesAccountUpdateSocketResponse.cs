using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesAccountUpdateSocketResponse : BinanceFuturesBasePrivateSocketResponse
    {
        [JsonProperty("a")]
        public BinanceFuturesAccountUpdateCore Data { get; set; }

    }

    public class BinanceFuturesAccountUpdateCore
    {
        [JsonProperty("B")]
        public IEnumerable<BinanceFuturesBalanceSocket> Balances { get; set; }

        [JsonProperty("P")]
        public IEnumerable<BinanceFuturesPositionSocket> Positions { get; set; }
    }
}
