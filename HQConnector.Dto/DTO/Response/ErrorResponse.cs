using HQConnector.Dto.DTO.Response.Enums;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response
{
    public abstract class ErrorResult : IErrorResult
    {
        public int Code { get; set; }

        public ResultType ErrorType { get; set; }

        public string Message { get; set; }

        public bool IsSuccess => ErrorType == ResultType.Success;
    }
}
