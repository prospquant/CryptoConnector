using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Private_Data
{
    public class BinanceFuturesBalanceInfo
    {
        [JsonProperty(PropertyName = "assets")]
        public List<BinanceFuturesBalance> Balances { get; set; }

        [JsonProperty(PropertyName = "canDeposit")]
        public string CanDeposit { get; set; }

        [JsonProperty(PropertyName = "canTrade")]
        public string CanTrade { get; set; }

        [JsonProperty(PropertyName = "canWithdraw")]
        public string CanWithdraw { get; set; }

        [JsonProperty(PropertyName = "feeTier")]
        public string FeeTier { get; set; }

        [JsonProperty(PropertyName = "maxWithdrawAmount")]
        public string MaxWithdrawAmount { get; set; }

        [JsonProperty(PropertyName = "positions")]
        public List<BinanceFuturesAccountPositions> Positions { get; set; }

        [JsonProperty(PropertyName = "totalInitialMargin")]
        public string TotalInitialMargin { get; set; }

        [JsonProperty(PropertyName = "totalMaintMargin")]
        public string TotalMaintMargin { get; set; }

        [JsonProperty(PropertyName = "totalMarginBalance")]
        public string TotalMarginBalance { get; set; }

        [JsonProperty(PropertyName = "totalOpenOrderInitialMargin")]
        public string TotalOpenOrderInitialMargin { get; set; }

        [JsonProperty(PropertyName = "totalPositionInitialMargin")]
        public string TotalPositionInitialMargin { get; set; }

        [JsonProperty(PropertyName = "totalUnrealizedProfit")]
        public string TotalUnrealizedProfit { get; set; }

        [JsonProperty(PropertyName = "totalWalletBalance")]
        public string TotalWalletBalance { get; set; }

        [JsonProperty(PropertyName = "updateTime")]
        public string UpdateTime { get; set; }
    }

    public class BinanceFuturesBalance 
    {
        [JsonProperty(PropertyName = "asset")]
        public string Asset { get; set; }

        [JsonProperty(PropertyName = "initialMargin")]
        public string InitialMargin { get; set; }

        [JsonProperty(PropertyName = "maintMargin")]
        public string MaintMargin { get; set; }

        [JsonProperty(PropertyName = "marginBalance")]
        public string MarginBalance { get; set; }

        [JsonProperty(PropertyName = "maxWithdrawAmount")]
        public string MaxWithdrawAmount { get; set; }

        [JsonProperty(PropertyName = "openOrderInitialMargin")]
        public string OpenOrderInitialMargin { get; set; }

        [JsonProperty(PropertyName = "positionInitialMargin")]
        public string PositionInitialMargin { get; set; }

        [JsonProperty(PropertyName = "unrealizedProfit")]
        public string UnrealizedProfit { get; set; }

        [JsonProperty(PropertyName = "walletBalance")]
        public string WalletBalance { get; set; }
    }
}
