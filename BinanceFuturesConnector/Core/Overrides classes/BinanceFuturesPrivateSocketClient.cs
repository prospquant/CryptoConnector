using HQConnector.Core.Classes.Sender_and_Socket.SocketClient;
using HQConnector.Core.Interfaces.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesPrivateSocketClient : SocketClient
    {
        AuthHelper helper;
        public BinanceFuturesPrivateSocketClient(string url, IConnectorCredentials connectorCredentials) : base(url, connectorCredentials)
        {
            helper = new AuthHelper(url, connectorCredentials);
        }

        protected override async void SendAuthToSocket()
        {
            if (AuthToSocket == false)
            {
                await helper.GetAuthToken();
                var url = "wss://fstream.binance.com/ws/" + helper.ListenKey;
                this.Reconnect(url);
            }
        }

        public override void SubscribeAuth(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubsribe = false)
        {
          
            if (AuthToSocket == false)
            {
                SendAuthToSocket();
                AuthToSocket = true;              

            }
            foreach (var endpoint in endpoints)
            {
                Subscription sub = CreateSubscription(endpoint, parameters);

                if (ifUnsubsribe && CurrentAuthSubscriptions.Any(s => s.Id == sub.Id))
                {
                    CurrentAuthSubscriptions.Remove(CurrentAuthSubscriptions.First(s => s.Id == sub.Id));
                    if (CurrentAuthSubscriptions.Count() == 0)
                    {
                        this.Dispose();
                    }
                }
                else
                {
                    if (!CurrentAuthSubscriptions.Any(s => s.Id == sub.Id))
                        CurrentAuthSubscriptions.Add(sub);
                }
            }
        }

        public  void Reconnect(string url)
        {
          
            try
            {
                CloseSocket();
                Socket = new WebSocket(url);
                BaseUrl = url;
                Socket.Opened += OnSocketOpened;
                Socket.Error += OnSocketError;
                Socket.Closed += OnSocketClosed;
                Socket.MessageReceived += OnMessageReceived;
                Socket.DataReceived += OnDataReceived;
                Socket.Open();
                while (Socket.State != WebSocketState.Open)
                {

                }
               
            }
            catch (Exception ex)
            {

            }
        
        }

        public override void Reconnect()
        {
            try
            {
              
                CloseSocket();

                Socket.Open();
                while (Socket.State != WebSocketState.Open)
                {

                }

            }
            catch (Exception ex)
            {

            }
          
        }
        protected override void OnMessageReceived(object sender, EventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        public async override void Dispose()
        {
            CloseSocket();            
            await helper.Close();
            Socket.Dispose();
        }
    }
}
