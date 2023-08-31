using System;
using System.ComponentModel;

namespace HQConnector.Dto.DTO.Enums.Orders
{
    [Flags]
    public enum OrderType : int
    {
        [Description("None")]
        None = 0,
        [Description("Limit")]
        Limit = 1,
        [Description("Market")]
        Market = 2,
        [Description("Stop Market")]
        StopMarket = 3,
        [Description("Stop Limit")]
        StopLimit = 4,
        [Description("TakeProfit Market")]
        TakeProfitMarket = 5,
        [Description("TakeProfit Limit")]
        TakeProfitLimit = 6,
        [Description("Ioc")]
        Ioc = 7,
        [Description("Limit Maker")]
        LimitMaker = 8,
        [Description("Virtual")]
        Virtual = 9
    }
}