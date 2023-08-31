using Newtonsoft.Json;

namespace BinanceFuturesConnector.Temp_Model.Rest.Public_Data
{
    public class BinanceFuturesSymbolResponse
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }

        [JsonProperty("rateLimits")]
        public BinanceFuturesRateLimit[] RateLimits { get; set; }

        [JsonProperty("exchangeFilters")]
        public object[] ExchangeFilters { get; set; }

        [JsonProperty("symbols")]
        public BinanceFuturesSymbol[] Symbols { get; set; }

        public partial class BinanceFuturesRateLimit
        {
            [JsonProperty("rateLimitType")]
            public string RateLimitType { get; set; }

            [JsonProperty("interval")]
            public string Interval { get; set; }

            [JsonProperty("intervalNum")]
            public decimal IntervalNum { get; set; }

            [JsonProperty("limit")]
            public decimal Limit { get; set; }
        }

        public partial class BinanceFuturesSymbol
        {
            [JsonProperty("symbol")]
            public string SymbolSymbol { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("maintMarginPercent")]
            public decimal MaintMarginPercent { get; set; }

            [JsonProperty("pricePrecision")]
            public decimal PricePrecision { get; set; }

            [JsonProperty("quantityPrecision")]
            public decimal QuantityPrecision { get; set; }

            [JsonProperty("requiredMarginPercent")]
            public decimal RequiredMarginPercent { get; set; }

            [JsonProperty("orderTypes")]
            public string[] OrderTypes { get; set; }

            [JsonProperty("timeInForce")]
            public string[] TimeInForce { get; set; }

            [JsonProperty("filters")]
            public BinanceFututresFilter[] Filters { get; set; }
        }

        public partial class BinanceFututresFilter
        {
            [JsonProperty("filterType")]
            public string FilterType { get; set; }


            [JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
            public string MaxPrice { get; set; }


            [JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
            public string MinPrice { get; set; }                      

            [JsonProperty("tickSize", NullValueHandling = NullValueHandling.Ignore)]
            public string TickSize { get; set; }


            [JsonProperty("maxQty", NullValueHandling = NullValueHandling.Ignore)]
            public string MaxQty { get; set; }

            [JsonProperty("minQty", NullValueHandling = NullValueHandling.Ignore)]
            public string MinQty { get; set; }

            [JsonProperty("stepSize", NullValueHandling = NullValueHandling.Ignore)]
            public string StepSize { get; set; }


            [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
            public long? Limit { get; set; }



            [JsonProperty("multiplierUp", NullValueHandling = NullValueHandling.Ignore)]
            public string MultiplierUp { get; set; }

            [JsonProperty("multiplierDown", NullValueHandling = NullValueHandling.Ignore)]
            public string MultiplierDown { get; set; }

            [JsonProperty("multiplierDecimal", NullValueHandling = NullValueHandling.Ignore)]
            public string MultiplierDecimal { get; set; }
        

        }
    }
}