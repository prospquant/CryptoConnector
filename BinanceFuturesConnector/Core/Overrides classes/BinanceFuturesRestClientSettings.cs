using System;
using System.Collections.Generic;
using System.Text;
using HQConnector.Core.Interfaces.Constants;
using HQConnector.Dto.DTO.Enums.TimeSpan;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesRestClientSettings : IConnectorRestClientSettings
    {
        #region properties

        
        public int MaxCountSymbols { get; set; }

        public int MaxCountTrades { get; set; }

        public int MaxCountHistoryTrades { get; set; }

        public int MaxCountAggregateTrades { get; set; }
        
       

        public int MaxCountCandles { get; set; }

        public TimestampType TimestampType { get; set; }

        public int MaxCountOrders { get; set; }
        public int MaxCountBalances { get; set; }
        public int MaxCountMyTrades { get; set; }
        
        #endregion

        #region ctor

        public BinanceFuturesRestClientSettings()
        {
           
            MaxCountSymbols = 500;           
            MaxCountTrades = 1000;
            MaxCountHistoryTrades = 1000;
            MaxCountAggregateTrades = 1000;           
            MaxCountCandles = 1500;          
            MaxCountOrders = 1000;         
            MaxCountBalances = 100;
            MaxCountMyTrades = 1000;

            TimestampType = TimestampType.Seconds;
        }

        #endregion
    }
}
