using HQConnector.Core.Enums;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Core.Interfaces.RateLimiter
{
    public interface IRateLimiter
    {
        IExchangeResultResponse<double> LimitRequest(string url, RateLimitingBehaviour limitBehaviour);

    }
}