using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BinanceFuturesConnector.Temp_Model
{
    public class BinanceFuturesAuthenticationRequest
    {
        private readonly string _apiKey;
        private readonly string _authSig;
        private readonly long _authNonce;
        private readonly string _authPayload;

        public BinanceFuturesAuthenticationRequest(string apiKey, string apiSecret)
        {

            _apiKey = apiKey;

            _authNonce = CreateAuthNonce();
            _authPayload = CreateAuthPayload(_authNonce);

            _authSig = CreateSignature(apiSecret, _authPayload);
        }

        [JsonProperty(PropertyName = "op")]
        public string Operation => "authKeyExpires";

        [JsonProperty(PropertyName = "args")]
        public object[] Args => new object[]
        {
            _apiKey,
            _authNonce,
            _authSig
        };

        private string CreateSignature(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(message);


            string ByteToString(byte[] buff)
            {
                var builder = new StringBuilder();

                for (var i = 0; i < buff.Length; i++)
                {
                    builder.Append(buff[i].ToString("X2")); // hex format
                }
                return builder.ToString();
            }

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ByteToString(hashmessage).ToLower();
            }
        }

        private long CreateAuthNonce(long? time = null)
        {
            
            var timeSafe = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600;
            return timeSafe;
        }

        private string CreateAuthPayload(long nonce)
        {
            return "/fapi/v1/listenKey" + nonce;
        }
    }
}
