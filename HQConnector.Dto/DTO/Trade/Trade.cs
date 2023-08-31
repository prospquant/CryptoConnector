using System;
using System.Text;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.Orders;


namespace HQConnector.Dto.DTO.Trade
{
    public class Trade 
    {
        public string Id { get; set; }

        public Exchange Exchange { get; set; }

        public string Pair { get; set; }
        public DateTimeOffset Time { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public Sides Side { get; set; }
                
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");
            sb.AppendLine($"Time: {Time}");
            sb.AppendLine($"Side: {Side}");
            sb.AppendLine($"Price: {Price}");
            sb.AppendLine($"Amount: {Amount}");                
          
            return sb.ToString();
        }
    }
}
