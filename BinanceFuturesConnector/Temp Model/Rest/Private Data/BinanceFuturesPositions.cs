using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesPositions
    {
        [JsonProperty(PropertyName = "entryPrice")]
        public string EntryPrice { get; set; }

        [JsonProperty(PropertyName = "marginType")]
        public string MarginType { get; set; }

        [JsonProperty(PropertyName = "isAutoAddMargin")]
        public string IsAutoAddMargin { get; set; }

        [JsonProperty(PropertyName = "isolatedMargin")]
        public string IsolatedMargin { get; set; }

        [JsonProperty(PropertyName = "leverage")]
        public string Leverage { get; set; }

        [JsonProperty(PropertyName = "liquidationPrice")]
        public string LiquidationPrice { get; set; }

        [JsonProperty(PropertyName = "markPrice")]
        public string MarkPrice { get; set; }

        [JsonProperty(PropertyName = "maxNotionalValue")]
        public string MaxNotionalValue { get; set; }

        [JsonProperty(PropertyName = "positionAmt")]
        public string PositionAmt { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "unRealizedProfit")]
        public string UnRealizedProfit { get; set; }

    }
    public class BinanceFuturesAccountPositions
    {
        [JsonProperty(PropertyName = "isolated")]
        public string Isolated { get; set; }

        [JsonProperty(PropertyName = "leverage")]
        public string Leverage { get; set; }

        [JsonProperty(PropertyName = "initialMargin")]
        public string InitialMargin { get; set; }

        [JsonProperty(PropertyName = "maintMargin")]
        public string MaintMargin { get; set; }

        [JsonProperty(PropertyName = "openOrderInitialMargin")]
        public string OpenOrderInitialMargin { get; set; }

        [JsonProperty(PropertyName = "positionInitialMargin")]
        public string PositionInitialMargin { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "unrealizedProfit")]
        public string UnrealizedProfit { get; set; }

    }


}
