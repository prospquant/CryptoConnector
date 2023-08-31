using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections;

namespace HQConnector.Dto.DTO.Candle
{
    public class Candle : IEqualityComparer
	{

        public Exchange Exchange { get; set; }
        public string Pair { get; set; }
        public DateTimeOffset OpenTime { get; set; }

        public DateTimeOffset CloseTime { get; set; }

        public bool IsClosed { get; set; } = false;
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }       
        public decimal TotalVolume { get; set; }        
        public bool IsBullCandle { get; set; }

		public new bool Equals(object x, object y)
		{
			if (x is Candle && y is Candle)
			{
				var o1 = x as Candle;
				var o2 = y as Candle;
				return o1.ClosePrice == o2.ClosePrice
					&& o1.HighPrice == o2.HighPrice
					&& o1.LowPrice == o2.LowPrice
					&& o1.OpenPrice == o2.OpenPrice
					&& o1.OpenTime == o2.OpenTime
					&& o1.Pair == o2.Pair					
					&& o1.TotalVolume == o2.TotalVolume
                    && o1.Exchange == o2.Exchange;
			}
			else return false;
		}

		public int GetHashCode(object obj)
		{
            return -1;
		}

		public override string ToString()
        {
            return
                $"Exchange: {Exchange}\r\nPair: {Pair}\r\nDate {OpenTime.DateTime}\r\nOpen: {OpenPrice}\r\nHigh: {HighPrice}\r\nLow: {LowPrice}\r\nClose: {ClosePrice}";
        }
    }
}
