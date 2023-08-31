using BinanceFuturesConnector.Temp_Model;
using HQConnector.Core.Classes.Sender_and_Socket.SocketClient;
using HQConnector.Core.Interfaces.Credentials;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSocket4Net;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesSocketClient : SocketClient
    {
        BinanceFuturesPrivateSocketClient privateSocket;
        public BinanceFuturesSocketClient(string url, IConnectorCredentials connectorCredentials) : base(url, connectorCredentials)
        {
            privateSocket = new BinanceFuturesPrivateSocketClient(url, connectorCredentials);
            privateSocket.MessageReceived += OnMessageReceived;
        }

        public BinanceFuturesSocketClient(string url) : base(url)
        {

        }
       protected override void SendAuthToSocket()
        {
            privateSocket.SubscribeAuth();
        }

        public override void SubscribeAuth(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubsribe = false)
        {

            
            privateSocket.SubscribeAuth(endpoints, parameters, ifUnsubsribe);
          
            //if (CurrentAuthSubscriptions.Count() == 0)
            //{
            //    privateSocket.Dispose();
            //}
        }
        protected override Subscription CreateSubscription(string endpoint, Dictionary<string, object> parameters)
        {
            return new BinanceFuturesSubscription(endpoint, parameters, IdCounter(parameters));
        }

        private int IdCounter(IDictionary<string,object> parameters)
        {
            try
            {
                if (this.CurrentSubscriptions.Any(p => p.Parameters.First().Value.ToString() == parameters.First().Value.ToString()))
                {
                    return Convert.ToInt32(this.CurrentSubscriptions.First(p => p.Parameters.First().Value.ToString() == parameters.First().Value.ToString()).Id);
                }
            }
            catch (Exception ex)
            {

            }
            return this.CurrentSubscriptions.Count() + 1;
        }
        protected override void OnMessageReceived(object sender, EventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        
    }
}