using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response
{
    public abstract class DetailExchangeResponse<TData> : IExchangeResultResponse<TData>
    {
        public TData Data { get; set; }

        public IErrorResult ErrorResult { get; set; }

        public Exchange Exchange { get; set; }

        protected DetailExchangeResponse(TData data, IErrorResult error)
        {
            Data = data;
            ErrorResult = error;
        }

        protected DetailExchangeResponse(TData data, IErrorResult error, Exchange exchange) : this(data, error)
        {
            Exchange = exchange;
        }
    }
}
