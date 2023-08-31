
using HQConnector.Dto.DTO.Response.Enums;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response.Error
{
    public abstract class ErrorResponse : IErrorResult
    {
        public int Code { get; set; }

        public ResultType ErrorType { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        protected ErrorResponse(ResultType err, string message)
        {
            ErrorType = err;
            Code = (int)err;
            Message = message;
            if (ErrorType == ResultType.Success)
            {
                IsSuccess = true;
            }
           
        }

        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }

    #region Errors

    public class CantConnectError : ErrorResponse
    {
        public CantConnectError() : base(ResultType.CantConnectToServer, "Can't connect to the server") { }
    }

    public class UnauthorizedError : ErrorResponse
    {
        public UnauthorizedError() : base(ResultType.InvalidAPIKeys, "Invalid API or Secret key") { }

        public UnauthorizedError(string message) : base(ResultType.InvalidAPIKeys, message) { }
    }

    public class ServerError : ErrorResponse
    {
        public ServerError(string message) : base(ResultType.CantConnectToServer, "Server error: " + message) { }
    }

    public class SuccessResponse : ErrorResponse
    {
        public SuccessResponse() : base(ResultType.Success, "Success") { }
    }

    public class CurrencyInstrumentError : ErrorResponse
    {
        public CurrencyInstrumentError(string pair) : base(ResultType.NotCurrenyInstrument, $"Not Currency in this exhange, Pair: {pair}") { }

    }

    public class WebError : ErrorResponse
    {
        public WebError(string message) : base(ResultType.WebError, "Web error: " + message) { }
    }

    public class DeserializeError : ErrorResponse
    {
        public DeserializeError(string message) : base(ResultType.DesirializeError, "Error deserializing data: " + message) { }
    }

    public class UnknownError : ErrorResponse
    {
        public UnknownError(string message) : base(ResultType.OtherError, "Unknown error occured - " + message) { }
    }

    public class ArgumentError : ErrorResponse
    {
        public ArgumentError(string message) : base(ResultType.InvalidParameter, "Invalid parameter: " + message) { }
    }

    public class NotFoundError : ErrorResponse
    {
        public NotFoundError(string message = null) : base(ResultType.InvalidParameter, $"Not Found : {message}") { }
    }

    public class RateLimitError : ErrorResponse
    {
        public RateLimitError(string message) : base(ResultType.RateLimitedError, "Rate limit exceeded: " + message) { }
    }

    public class TimeoutError : ErrorResponse
    {
        public TimeoutError(string message = null) : base(ResultType.TimeOutError, "TimeOut: " + message) { }
    }

    public class HttpRequestError : ErrorResponse
    {
        public HttpRequestError(string message = null) : base(ResultType.HttpRequestError, "HttpRequestError: " + message) { }
    }

    public class InvalidOperationError : ErrorResponse
    {
        public InvalidOperationError(string message = null) : base(ResultType.InvalidOperationError, "InvalidOperationError: " + message) { }
    }


    #region Частный случай ошибки
    public class CancelAllOrdersError : ErrorResponse
    {
        public CancelAllOrdersError() : base(ResultType.OtherError, "Can`t cancel orders") { }
    }
    public class ChangeOrderError : ErrorResponse
    {
        public ChangeOrderError(string message) : base(ResultType.ChangeOrderError, "Can`t change order : " + message) { }
    }

    public class NotFoundPositionResponse : ErrorResponse
    {
        public NotFoundPositionResponse(string message) : base(ResultType.ChangeOrderError, "Can`t change order : " + message) { }
    }

    public class ClosePositionError : ErrorResponse
    {
        public ClosePositionError() : base(ResultType.OtherError, "Can`t close position") { }
        public ClosePositionError(string message) : base(ResultType.OtherError, "Can`t close position: " + message) { }
    }

    public class ClosePositionsError : ErrorResponse
    {
        public ClosePositionsError() : base(ResultType.OtherError, "Can`t close position") { }
        public ClosePositionsError(string message) : base(ResultType.OtherError, "Can`t close positions: " + message) { }
    }
    #endregion




    #endregion


}
