using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.RateLimiter;
using HQConnector.Core.Interfaces.Rest;
using HQConnector.Core.RateLimit;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;
using Newtonsoft.Json;

namespace HQConnector.Core.Classes.Sender_and_Socket.RestClient
{
    public abstract class RestSender : IRestClient
    {
        #region Properties

        public IConnectorCredentials Credentials { get; set; }

        public IRateLimiter RateLimiter { get; set; }

        public HttpClient Client { get; set; }

        public string BaseUrl { get; set; }

        #endregion

        #region ctor

        private RestSender()
        {
            Client = new HttpClient();
        }

        private RestSender(string baseUrl) : this()
        {
            BaseUrl = baseUrl;
            Client.BaseAddress = new Uri(BaseUrl);
        }

        private RestSender(string baseUrl, IConnectorCredentials credentials) : this(baseUrl)
        {
            Credentials = credentials;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">Базовый url</param>
        /// <param name="credentials">Ключи</param>
        /// <param name="limiter">Лимитер (Rate limit)</param>
        protected RestSender(string baseUrl, IConnectorCredentials credentials, IRateLimiter limiter) : this(baseUrl, credentials)
        {
            RateLimiter = limiter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">Базовый url</param>
        /// <param name="credentials">Ключи</param>
        /// <param name="limitRequestCount">Максимальное Кол-во запросов в промежуток времени </param>
        /// <param name="timeLimit">Промежуток времени</param>
        protected RestSender(string baseUrl, IConnectorCredentials credentials, int limitRequestCount, TimeSpan timeLimit) : this(baseUrl, credentials)
        {
            RateLimiter = new RateLimiterTotal(limitRequestCount, timeLimit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">Базовый url</param>
        /// <param name="credentials">Ключи</param>
        /// <param name="limitRequestCount">Максимальное Кол-во запросов в промежуток времени </param>
        /// <param name="seconds">Промежуток времени в секундах</param>
        protected RestSender(string baseUrl, IConnectorCredentials credentials, int limitRequestCount, int seconds) : this(baseUrl, credentials)
        {
            RateLimiter = new RateLimiterTotal(limitRequestCount, new TimeSpan(0, 0, seconds));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <typeparam name="TDataResponse"></typeparam>
        /// <param name="endpoint">Endpoint </param>
        /// <param name="httpMethod">тип http метода</param>
        /// <param name="param">Набор параметров</param>
        /// <param name="signed">Требуется подпись или нет</param>
        /// <param name="contentType">Тип контента для body request</param>
        /// <returns></returns>
        public virtual async Task<IExchangeResultResponse<TDataResponse>> SendRequestAsync<TDataResponse>(string endpoint,
            HttpMethods httpMethod, Dictionary<string, object> param = null,
            Signed signed = Signed.No, ContentType contentType = ContentType.Query)
        {
            var request = CreateRequest(endpoint, httpMethod, param, signed, contentType);
            WaitLimit("total");
            return await ClientSendRequest<TDataResponse>(request);
        }

        /// <summary>
        /// Формирование результата ошибки. Реализация у каждой биржи своя
        /// </summary>
        /// <param name="response">Response от биржи</param>
        /// <returns>Ответ-ошибка от биржи</returns>
        protected abstract Task<IErrorResult> CreateErrorResponseAsync(HttpResponseMessage response);

        /// <summary>
        /// Формирование самого request-запроса. Требуется сделать реализацию для каждой биржи
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="httpMethod">Тип Http метода</param>
        /// <param name="param">набор параметров</param>
        /// <param name="signed">Нужна подпись или нет</param>
        /// <param name="contentType">Тип контента для body в request</param>
        /// <returns>Request</returns>
        protected abstract HttpRequestMessage CreateRequest(string endpoint, HttpMethods httpMethod, Dictionary<string, object> param = null, Signed signed = Signed.No, ContentType contentType = ContentType.Query);

        /// <summary>
        /// Формирование результата
        /// </summary>
        /// <typeparam name="TDataResponse">Временная модель данных, в которую нужно дисериализовать ответ</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual async Task<IExchangeResultResponse<TDataResponse>> CreateResponseAsync<TDataResponse>(HttpResponseMessage response)
        {
            var serializeResponse = default(TDataResponse);
            IErrorResult errorResponse;
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                try
                {
                    serializeResponse = JsonConvert.DeserializeObject<TDataResponse>(result);
                    errorResponse = new SuccessResponse();
                }
                catch (JsonSerializationException exception)
                {
                    errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}. String response: {result}");
                }
                catch (Exception exception)
                {
                    errorResponse = new UnknownError($"UnknownError: {exception.Message}, String response: {result}");
                }
            }
            else
            {
                errorResponse = await CreateErrorResponseAsync(response);
            }

            return new MessageExchangeResponse<TDataResponse>(serializeResponse, errorResponse, Credentials.Exchange);
        }


        protected async Task<IExchangeResultResponse<TDataResponse>> ClientSendRequest<TDataResponse>(HttpRequestMessage request)
        {
            IErrorResult errorResponse;

            try
            {
                var responseMessage = await Client.SendAsync(request);
                return await CreateResponseAsync<TDataResponse>(responseMessage);
            }
            catch (TimeoutException e)
            {
                errorResponse =  new TimeoutError(e.Message);
            }
            catch (HttpRequestException e)
            {
                errorResponse = new HttpRequestError(e.Message);
            }
            catch (InvalidOperationException e)
            {
                errorResponse = new InvalidOperationError(e.Message);
            }
            catch (Exception e)
            {
                errorResponse = new UnknownError(e.Message);
            }

            var messageResponse =
                new MessageExchangeResponse<TDataResponse>(default(TDataResponse), errorResponse, Credentials.Exchange);

            return messageResponse;
        }


        #region Util methods

        /// <summary>
        /// Метод для проверки лимита отосланных нами запросов по ресту
        /// </summary>
        /// <param name="keyLimit">Ключ по лимитеру, default - "total" </param>
        protected virtual void WaitLimit(string keyLimit)
        {
            RateLimiter.LimitRequest(keyLimit, RateLimitingBehaviour.Wait);
        }

        /// <summary>
        /// Формирование query строки по словарю
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string BuildQueryData(Dictionary<string, object> param)
        {
            if (param == null)
                return "";

            var b = new StringBuilder();
            foreach (var item in param)
                b.Append($"&{item.Key}={item.Value}");

            try
            {
                return b.ToString().Substring(1);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Формирование json строки по словарю
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string BuildJSON(Dictionary<string, object> param)
        {
            if (param == null)
                return "";

            var entries = new List<string>();
            foreach (var item in param)
                entries.Add($"\"{item.Key}\":\"{item.Value}\"");

            return "{" + string.Join(",", entries) + "}";
        }

        protected byte[] HmacSignatureByte(byte[] keyByte, byte[] messageBytes)
        {
            using (var hash = new HMACSHA256(keyByte))
            {
                return hash.ComputeHash(messageBytes);
            }
        }

        protected string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        protected HttpMethod GetMethods(HttpMethods httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethods.GET:
                    return HttpMethod.Get;
                case HttpMethods.POST:
                    return HttpMethod.Post;
                case HttpMethods.DELETE:
                    return HttpMethod.Delete;
                case HttpMethods.PUT:
                    return HttpMethod.Put;
                default:
                    throw new Exception();
            }
        }

        #endregion

        #endregion


    }
}
