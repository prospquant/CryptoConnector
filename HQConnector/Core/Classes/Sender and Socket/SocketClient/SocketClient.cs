using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.Socket;
using HQConnector.Dto;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocket4Net;

namespace HQConnector.Core.Classes.Sender_and_Socket.SocketClient
{
    public abstract class SocketClient : PropertyChangedBase, ISocketClient, IDisposable
    {
        public bool AuthToSocket { get; set; } = false;

        public EventHandler MessageReceived { get; set; }

        public IConnectorCredentials Credentials { get; set; }

        public string BaseUrl { get; set; }
        protected WebSocket Socket { get; set; }

        public WebSocketState SocketState => Socket.State;

        public List<Subscription> CurrentSubscriptions { get; set; }

        public List<Subscription> CurrentAuthSubscriptions { get; set; }

        public bool isReconnecting { get; set; } = false;

        protected SocketClient(string baseUrl, IConnectorCredentials connectorCredentials) : this(baseUrl)
        {
            Credentials = connectorCredentials;

        }

        protected SocketClient(string baseUrl)
        {
            try
            {
                BaseUrl = baseUrl;
                Socket = new WebSocket(BaseUrl);
                Socket.Opened += OnSocketOpened;
                Socket.Error += OnSocketError;
                Socket.Closed += OnSocketClosed;
                Socket.MessageReceived += OnMessageReceived;
                Socket.DataReceived += OnDataReceived;
                CurrentSubscriptions = new List<Subscription>();
                CurrentAuthSubscriptions = new List<Subscription>();
            }
            catch (Exception ex)
            {
               
            }

        }

		protected virtual void OnDataReceived(object sender, DataReceivedEventArgs e)
		{
			return;
		}

		protected virtual void OnMessageReceived(object sender, EventArgs e)
		{
			CurrentSubscriptions = new List<Subscription>();
            CurrentAuthSubscriptions = new List<Subscription>();

        }

		protected SocketClient()
		{

		}

		protected virtual void OnSocketClosed(object sender, EventArgs e)
		{
            try
            {
                if (CurrentSubscriptions != null && CurrentSubscriptions.Count() != 0 || CurrentAuthSubscriptions != null && CurrentAuthSubscriptions.Count() != 0)
                {
                    Reconnect();
                }
            }
            catch (Exception ex)
            {

            }
		}

		protected void OnSocketError(object sender, ErrorEventArgs e)
		{
			return;
		}

		protected virtual void OnSocketOpened(object sender, EventArgs e)
		{
		}
		
		protected virtual void Send(Subscription subscription)
		{
           
            Socket.Send(subscription.ToString());
		}

      
        protected virtual void CloseSocket()
        {
            try
            {
                if (Socket.State == WebSocketState.Open)
                {
                    Socket.Close();
                    while (Socket.State == WebSocketState.Open)
                    {

                    }

                    AuthToSocket = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public virtual void Reconnect()
        {
            try
            {
               
                CloseSocket();

                Socket.Open();
                while (Socket.State != WebSocketState.Open)
                {

                }

                if (CurrentAuthSubscriptions != null && CurrentAuthSubscriptions.Count != 0)
                {
                    SendAuthToSocket();
                    foreach (var subscription in CurrentAuthSubscriptions)
                    {

                        Send(subscription);
                    }
                }

                foreach (var subscription in CurrentSubscriptions)
                {

                    Send(subscription);
                }
               
            }
            catch (Exception ex)
            {

            }
           
        }

        
        protected abstract void SendAuthToSocket();

        public  virtual  async  Task Subscribe(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubsribe = false)
        {
            if (Socket.State != WebSocketState.Open)
            {
                Reconnect();
            }
            foreach (var endpoint in endpoints)
            {
                Subscription sub = CreateSubscription(endpoint, parameters);




                if (ifUnsubsribe && CurrentSubscriptions.Any(s => s.Id == sub.Id))
                {
                    CurrentSubscriptions.Remove(CurrentSubscriptions.First(s => s.Id == sub.Id));
                }
                else
                {
                    CurrentSubscriptions.Add(sub);
                }
                Send(sub);


            }
        }

       

        public virtual void SubscribeAuth(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubsribe = false)
        {
            if (Socket.State != WebSocketState.Open)
            {
                Reconnect();
            }
            if (AuthToSocket == false)
            {
                AuthToSocket = true;
                
                SendAuthToSocket();
              
            }

            
            foreach (var endpoint in endpoints)
            {
                Subscription sub = CreateSubscription(endpoint, parameters);

                if (ifUnsubsribe && CurrentAuthSubscriptions.Any(s => s.Id == sub.Id))
                {
                    CurrentAuthSubscriptions.Remove(CurrentAuthSubscriptions.First(s => s.Id == sub.Id));
                }
                else
                {
                    CurrentAuthSubscriptions.Add(sub);
                }
                Send(sub);
            }
        }

        protected virtual Subscription CreateSubscription(string endpoint, Dictionary<string, object> parameters)
        {
            return new Subscription(endpoint, parameters);
        }
        
        public virtual void Dispose()
        {
            CurrentSubscriptions.Clear();
            CurrentAuthSubscriptions.Clear();
            CloseSocket();          
            Socket.Dispose();
        }
    }
}
