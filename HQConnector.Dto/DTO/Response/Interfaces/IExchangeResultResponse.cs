using HQConnector.Dto.DTO.Enums.Exchange;

namespace HQConnector.Dto.DTO.Response.Interfaces
{
    public interface IExchangeResultResponse<TDataResponse>:IResultResponse<TDataResponse>
    {
        Exchange Exchange { get; set; }

    }
}
