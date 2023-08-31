using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Ticker
{
   public class TickerPrice
   {
        public string Pair { get; set; }
     
        public decimal Price { get; set; }

        public DateTimeOffset Date { get; set; }

        public Exchange Exchange { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");
            sb.AppendLine($"Date: {Date.DateTime}");
            sb.AppendLine($"Price: {Price}");
           
            return sb.ToString();
        }
    }
}

