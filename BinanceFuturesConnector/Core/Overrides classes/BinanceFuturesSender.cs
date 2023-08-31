using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BinanceFuturesConnector.Temp_Model.Rest;

using HQConnector.Core.Classes.Sender_and_Socket.RestClient;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.RateLimiter;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;
using Newtonsoft.Json;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
   public class BinanceFuturesSender : RestSender
    {        
    
        #region ctor

        public BinanceFuturesSender(string baseUrl, IConnectorCredentials credentials, IRateLimiter limiter) : base(baseUrl, credentials, limiter)
        {
        }

        public BinanceFuturesSender(string baseUrl, IConnectorCredentials credentials, int limitRequestCount, TimeSpan timeLimit) : base(baseUrl, credentials, limitRequestCount, timeLimit)
        {
        }

        public BinanceFuturesSender(string baseUrl, IConnectorCredentials credentials, int limitRequestCount, int secondsLimit) : base(baseUrl, credentials, limitRequestCount, secondsLimit)
        {
        }


        #endregion

        #region Methods


        protected override async Task<IErrorResult> CreateErrorResponseAsync(HttpResponseMessage response)
        {
            IErrorResult errorResponse;
            var result = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<BinanceFuturesErrorResponse>(result);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    errorResponse = new UnauthorizedError(errorMessage.Message);
                    break;

                case HttpStatusCode.BadRequest:
                    errorResponse = new ArgumentError(errorMessage.Message);
                    break;
                case HttpStatusCode.NotFound:
                    errorResponse = new NotFoundError();
                    break;
                default:
                    errorResponse = new UnknownError("Unknown error");
                    break;
            }

            return errorResponse;
        }

        protected override HttpRequestMessage CreateRequest(string endpoint, HttpMethods httpMethod,
            Dictionary<string, object> param = null, Signed signed = Signed.No, ContentType contentType = ContentType.Query)
        {
            var typeMethods = GetMethods(httpMethod);
            var paramData = "";
            string signature = null;
            if (signed == Signed.Yes)
            {
                signature = IsSignedSettings(ref param);
            }
            paramData = BuildQueryData(param);
            if (!string.IsNullOrEmpty(signature))
            {
                paramData += $"&signature={signature}";
            }

            var finalEndpoint = BaseUrl + endpoint;
            if (!string.IsNullOrEmpty(paramData) && typeMethods == HttpMethod.Get)
            {
                finalEndpoint += $"?{paramData}";
            }
            var request = new HttpRequestMessage(typeMethods, new Uri(finalEndpoint));
            if (signed == Signed.Yes)
            {
                request.Headers.Add("X-MBX-APIKEY", Credentials.ApiKey);
            }
            if (typeMethods != HttpMethod.Get)
            {
                request.Content = new StringContent(paramData, Encoding.UTF8, "application/x-www-form-urlencoded");
            }

            return request;
        }

        #endregion

        #region Sign

        private string IsSignedSettings(ref Dictionary<string, object> newData)
        {
            newData = newData.OrderBy(k => k.Key).ToDictionary(process => process.Key, process => process.Value);
            var querySortDataWitnApiKey = BuildQueryData(newData);

            var signatureBytes = HmacSignatureByte(Encoding.UTF8.GetBytes(Credentials.ApiSecretKey), Encoding.UTF8.GetBytes(querySortDataWitnApiKey));
            var signatureString = ByteArrayToString(signatureBytes);
            return signatureString;
        }

        #endregion


    }
}
