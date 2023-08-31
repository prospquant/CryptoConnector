using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.RateLimiter;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Core.Interfaces.Rest
{
    public interface IRestClient
    {
        HttpClient Client { get; }

        string BaseUrl { get; set; }

        IConnectorCredentials Credentials { get; }

        IRateLimiter RateLimiter { get; set; }


        Task<IExchangeResultResponse<TDataResponse>> SendRequestAsync<TDataResponse>
            (string endpoint, HttpMethods httpMethod, Dictionary<string, object> param = null, Signed signed = Signed.No, ContentType contentType = ContentType.Query);

       
    }
}