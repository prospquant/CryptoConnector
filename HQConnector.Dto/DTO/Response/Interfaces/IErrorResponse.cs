using HQConnector.Dto.DTO.Response.Enums;

namespace HQConnector.Dto.DTO.Response.Interfaces
{
    public interface IErrorResult
    {
        int Code { get; set; }

        ResultType ErrorType { get; set; }

        string Message { get; set; }

        bool IsSuccess { get; }

    }
}