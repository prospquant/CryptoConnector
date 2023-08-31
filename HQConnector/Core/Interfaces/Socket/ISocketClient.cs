using HQConnector.Core.Classes.Sender_and_Socket.SocketClient;
using HQConnector.Core.Interfaces.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WebSocket4Net;

namespace HQConnector.Core.Interfaces.Socket
{
    public interface ISocketClient : IDisposable
    {
        string BaseUrl { get; set; }

        [DefaultValue(false)]
        bool AuthToSocket { get; set; }

        EventHandler MessageReceived { get; set; }

        IConnectorCredentials Credentials { get; set; }

        Task Subscribe(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubscribe = false);

        void SubscribeAuth(List<string> endpoints = null, Dictionary<string, object> parameters = null, bool ifUnsubscribe = false);

        void Reconnect();

        WebSocketState SocketState { get; }

        List<Subscription> CurrentSubscriptions { get; set; }
    }
}