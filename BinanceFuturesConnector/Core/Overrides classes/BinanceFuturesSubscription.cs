using HQConnector.Core.Classes.Sender_and_Socket.SocketClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesConnector.Core.Overrides_classes
{
    public class BinanceFuturesSubscription : Subscription
    {
        public BinanceFuturesSubscription(string method, Dictionary<string, object> parameters,int id) : base(method, parameters)
        {
            Id = id.ToString();
        }
        public override string ToString()
        {
            var subRequest = new BinanceFuturesSocketRequest
            {
                Operation = Method.ToUpper(),
                Parameters = new List<object>
                {
                    Parameters["params"]
                },
                Id = Convert.ToInt32(Id)
            };
            var strJson = JsonConvert.SerializeObject(subRequest);
            return strJson;
        }

        public class BinanceFuturesSocketRequest
        {
            [JsonProperty(PropertyName = "method")]
            public string Operation { get; set; }

            [JsonProperty(PropertyName = "params")]
            public List<object> Parameters { get; set; }

            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
        }
    }
}
