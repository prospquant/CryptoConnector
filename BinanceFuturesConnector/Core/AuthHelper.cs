using BinanceFuturesConnector.Core.Overrides_classes;
using BinanceFuturesConnector.Temp_Model.Rest.Private_Data;
using BinanceFuturesConnector.Temp_Model.Rest.Public_Data;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Dto.DTO.Enums.Sender;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Timers;

namespace BinanceFuturesConnector.Core
{
   public class AuthHelper
    {
        string _url { get; set; }

        IConnectorCredentials _connectorCredentials;

        BinanceFuturesSender restClient;

        public string ListenKey { get; set; }

        public bool IsConnected { get; private set; }

        Timer timer;
        public AuthHelper(string url, IConnectorCredentials connectorCredentials)
        {
            _url = url;
            _connectorCredentials = connectorCredentials;
            restClient = new BinanceFuturesSender("https://fapi.binance.com", connectorCredentials, 150,300);
            timer = new Timer();
            timer.Interval = new TimeSpan(00, 30, 00).TotalMilliseconds;
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (IsConnected == true)
            await KeepAlive();
        }

        private async Task<long> CalcServerTime()
        {

            var time = await restClient.SendRequestAsync<BinanceFuturesServerTime>("/fapi/v1/time", HttpMethods.GET, new Dictionary<string, object>
            {

            }, Signed.No, ContentType.Query);
            return time?.Data.ServerTime ?? 0;
        }
        public async Task<IExchangeResultResponse<string>> GetAuthToken()
        {
            IErrorResult errorResponse;
            try
            {
                var res = await restClient.SendRequestAsync<AuthResponce>("/fapi/v1/listenKey",
                  HttpMethods.POST, new Dictionary<string, object>
                  {
                      ["timestamp"] = await CalcServerTime()

                  }, Signed.Yes, ContentType.Query);
                if (res != null && res.ErrorResult.IsSuccess && !String.IsNullOrEmpty(res.Data.ListenKey))
                {
                    ListenKey = res.Data.ListenKey;
                    IsConnected = true;
                }
                else
                {
                    IsConnected = false;
                }
                return new MessageExchangeResponse<string>(ListenKey, res.ErrorResult, _connectorCredentials.Exchange);

            }
            catch (JsonSerializationException exception)
            {
                errorResponse = new DeserializeError($"Can`t Deserialize this response: {exception.Message}.");
            }
            catch (Exception exception)
            {
                errorResponse = new UnknownError($"UnknownError: {exception.Message}");
            }
            return new MessageExchangeResponse<string>(null, errorResponse, _connectorCredentials.Exchange);
        }

        private async Task<IExchangeResultResponse<bool>> KeepAlive()
        {
           try
            {
                var res = await restClient.SendRequestAsync<AuthResponce>("/fapi/v1/listenKey",
                  HttpMethods.PUT, new Dictionary<string, object>
                  {
                      ["timestamp"] = await CalcServerTime()

                  }, Signed.Yes, ContentType.Query);
                if (res == null || !res.ErrorResult.IsSuccess)
                {
                    return new MessageExchangeResponse<bool>(false, res.ErrorResult, _connectorCredentials.Exchange);
                }
            }
            catch (Exception ex)
            {
                return new MessageExchangeResponse<bool>(false, new UnknownError($"UnknownError: {ex.Message}"), _connectorCredentials.Exchange);
            }
             return new MessageExchangeResponse<bool>(true, new SuccessResponse(), _connectorCredentials.Exchange);

        }

        public async Task Close()
        {
            try
            {
                var res = await restClient.SendRequestAsync<AuthResponce>("/fapi/v1/listenKey",
                  HttpMethods.DELETE, new Dictionary<string, object>
                  {
                      ["timestamp"] = await CalcServerTime()

                  }, Signed.Yes, ContentType.Query);
            }
            catch (Exception ex) { }
            ListenKey = null;
            IsConnected = false;
            timer.Enabled = false;
        }

    }
}
