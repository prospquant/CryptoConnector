using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Core.Interfaces.Constants
{
    public interface IConnectorSettings
    {
        ExchangeMode ExchangeMode { get; set; }
        bool IsFutures { get; set; }
        bool IsMargin { get; set; }
        bool IsSocketConnected { get; }
        bool IsSupportStopOrders { get; set; }

        IConnectorRestClientSettings RestSettings { get; set; }
        ConcurrentBag<object> ReceivedItems { get; set; }
    }
}
