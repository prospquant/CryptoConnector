using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using HQConnector.Core.Enums;
using HQConnector.Core.Interfaces.RateLimiter;
using HQConnector.Dto.DTO.Response;
using HQConnector.Dto.DTO.Response.Error;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Core.RateLimit
{
    public class RateLimiterPerEndpoint : IRateLimiter
    {
        internal Dictionary<string, RateLimitObject> history = new Dictionary<string, RateLimitObject>();

        private readonly int limitPerEndpoint;
        private readonly TimeSpan perTimePeriod;
        private readonly object historyLock = new object();

        /// <summary>
        /// Create a new RateLimiterPerEndpoint. This rate limiter limits the amount of requests per time period to a certain limit, counts the request per endpoint.
        /// </summary>
        /// <param name="limitPerEndpoint">The amount to limit to</param>
        /// <param name="perTimePeriod">The time period over which the limit counts</param>
        public RateLimiterPerEndpoint(int limitPerEndpoint, TimeSpan perTimePeriod)
        {
            this.limitPerEndpoint = limitPerEndpoint;
            this.perTimePeriod = perTimePeriod;
        }

        public IExchangeResultResponse<double> LimitRequest(string url, RateLimitingBehaviour limitBehaviour)
        {
            int waitTime;
            RateLimitObject rlo;
            lock (historyLock)
            {
                if (history.ContainsKey(url))
                    rlo = history[url];
                else
                {
                    rlo = new RateLimitObject();
                    history.Add(url, rlo);
                }
            }

            var sw = Stopwatch.StartNew();
            lock (rlo.LockObject)
            {
                sw.Stop();
                waitTime = rlo.GetWaitTime(DateTime.UtcNow, limitPerEndpoint, perTimePeriod);
                if (waitTime != 0)
                {
                    if (limitBehaviour == RateLimitingBehaviour.Fail)
                        return new MessageExchangeResponse<double>(waitTime, new RateLimitError($"endpoint limit of {limitPerEndpoint} reached on endpoint " + url));

                    Thread.Sleep(Convert.ToInt32(waitTime));
                    waitTime += (int)sw.ElapsedMilliseconds;
                }

                rlo.Add(DateTime.UtcNow);
            }

            return new MessageExchangeResponse<double>(waitTime, null);
        }

       
    }
}
