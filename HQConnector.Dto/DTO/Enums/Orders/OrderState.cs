using System;
using System.ComponentModel;

namespace HQConnector.Dto.DTO.Enums.Orders
{
    [Flags]
    public enum OrderState : int
    {
        [Description("None")]
        NONE = 0,
        [Description("Open")]
        OPEN = 1,
        [Description("Submitted")]
        SUBMITTED = 2,
        [Description("Partially Filled")]
        PARTIALLYFILLED = 3,
        [Description("Filled")]
        FILLED = 5,
        [Description("Partially Cancelled")]
        PARTIALLYCANCELLED = 9,
        [Description("Cancelled")]
        CANCELED = 6,
        [Description("Rejected")]
        REJECTED = 7,
        [Description("Expired")]
        EXPIRED = 8,
        [Description("New")]
        NEW = 9,
        [Description("Not registered")]
        NOTREGISTERED = 10,
        [Description("Triggered")]
        TRIGGERED = 11

    }
}