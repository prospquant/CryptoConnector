using System.Linq;
using HQConnector.Dto.DTO.Any_Collection;
using HQConnector.Dto.DTO.Enums.Exchange;


namespace HQConnector.Dto.DTO.OrderBook
{
    public class OrderBook 
    {
        #region Properties
       

        public Exchange Exchange { get; set; }

        public string Pair { get; set; }

        public int Depth { get; set; }

        public bool HaveAnyItem => Bids.Any() || Asks.Any();

        public OrderCollection<Quote> Bids { get; set; }

        public OrderCollection<Quote> Asks { get; set; }

        #endregion

        #region ctor

        public OrderBook()
        {
            Bids = new OrderCollection<Quote>();
            Asks = new OrderCollection<Quote>();
        }


        public OrderBook(string pair, int depth) : this()
        {
            Pair = pair;
            Depth = depth;
        }


        #endregion

    }
}
