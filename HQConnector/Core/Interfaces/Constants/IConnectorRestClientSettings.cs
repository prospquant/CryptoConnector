using System;
using System.Collections.Generic;
using System.Text;
using HQConnector.Dto.DTO.Enums.TimeSpan;

namespace HQConnector.Core.Interfaces.Constants
{
    public interface IConnectorRestClientSettings
    {

        #region Public data

   
        int MaxCountSymbols { get; set; }     
        int MaxCountTrades { get; set; }      
        int MaxCountCandles { get; set; }
        TimestampType TimestampType { get; }

        #endregion

        #region Private data
          
        int MaxCountOrders { get; set; }
        int MaxCountBalances { get; set; }
        int MaxCountMyTrades { get; set; }

        #endregion



    }
}
