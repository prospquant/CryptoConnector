using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response
{
    public class MessageExchangeResponse<TData> : DetailExchangeResponse<TData>
    {
        public MessageExchangeResponse(TData data, IErrorResult error, Exchange exchange) : base(data, error, exchange)
        {
        }

        public MessageExchangeResponse(TData data, IErrorResult error) : base(data, error)
        {
        }
    }

    
}
