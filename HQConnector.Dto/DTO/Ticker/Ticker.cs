using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Text;

namespace HQConnector.Dto.DTO.Ticker
{
    public class Ticker
    {
        public Exchange Exchange { get; set; }
        public string Pair { get; set; }

        public DateTimeOffset Date { get; set; }

        public decimal Bid { get; set; }

        public decimal Ask { get; set; }

        public decimal Last { get; set; }

        public decimal Volume { get; set; }

        public decimal OpenInterest { get; set; }

        public decimal Volume24h { get; set; }

        public decimal MaxPrice24h { get; set; }

        public decimal MinPrice24h { get; set; }

        public decimal LastTrade { get; set; }

        public decimal PercentChange { get; set; }

        public decimal PriceStep { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");
            sb.AppendLine($"Date: {Date.DateTime}");            
            sb.AppendLine($"Last: {Last}");            
            return sb.ToString();
        }
    }
}
