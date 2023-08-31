namespace HQConnector.Dto.DTO.Response.Enums
{
    public enum ResultType
    {
        Success = 777,

        InvalidAPIKeys = 1,
        OldTimestamp = 2,

        AmountLessMinAmount = 3,
        PriceCantMinStep = 4,

        NotAvailBalance = 5,
        NotCurrenyInstrument = 6,

        ServerBusy = 7,
        InstrumentStopped = 8,

        CantConnectToServer = 9,

        DesirializeError = 10,

        InvalidParameter = 11,

        InvalidSignature = 12,

        RateLimitedError = 13,

        WebError = 14,

        TimeOutError = 15,

        HttpRequestError = 16,

        InvalidOperationError = 17,


        ChangeOrderError = 123,

        NotFoundPositionResponse = 222,
        OtherError = 99999
    }
}