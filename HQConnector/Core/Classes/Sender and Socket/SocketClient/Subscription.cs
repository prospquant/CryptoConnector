using System;
using System.Collections.Generic;

namespace HQConnector.Core.Classes.Sender_and_Socket.SocketClient
{
    public enum SubscriptionStatus
    {
        New = 1,
        Successful = 2,
        Fail = 3
    }
    public class Subscription
    {
        public string Method { get; set; }
        public Dictionary<string, object> Parameters { get; set;}
        public SubscriptionStatus Status { get; set; }
        public string Id { get; set; }

        public IDisposable handler { get; set; }

        public Subscription(string method, Dictionary<string, object> parameters)
        {
            Method = method;
            Parameters = new Dictionary<string, object>(parameters);
            Status = SubscriptionStatus.New;
            Id = parameters.ContainsKey("id") ? parameters["id"].ToString() : String.Empty;
        }

        public override string ToString()
        {
            string prm = "\"params\":{";
            int counter = 0;
            foreach (var p in Parameters)
            {
                if (counter != 0) prm += ", ";
                prm += $"\"{p.Key}\":\"{p.Value}\"";
                counter++;
            }
            return "{" + $"\"method\":\"{Method}\"," + prm + "}}";
        }
    }
}