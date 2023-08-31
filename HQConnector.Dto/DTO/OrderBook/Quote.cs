using System;
using System.Collections;

namespace HQConnector.Dto.DTO.OrderBook
{
    public class Quote: IEqualityComparer
    {
        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public decimal Total => Price * Amount;

        public bool IsOrdered { get; set; }

        public DateTime Date { get; set; }

        public Quote()
        {
            IsOrdered = true;
        }

        public override string ToString()
        {
            return $"Price: {Price}, Amount: {Amount}";
        }

		public new bool Equals(object x, object y)
		{
			if (x is Quote && y is Quote)
			{
				var q1 = x as Quote;
				var q2 = y as Quote;
				return q1.Price == q2.Price && q1.Amount == q2.Amount;
			}
			else
				return false;
		}

		public int GetHashCode(object obj)
		{
			return Price.GetHashCode();
		}
	}
}
