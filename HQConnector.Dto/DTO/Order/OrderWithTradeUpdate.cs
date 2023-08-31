using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Order
{
   public class OrderWithTradeUpdate
    {
       public IEnumerable<Order> Orders { get; set; }

       public IEnumerable<HQConnector.Dto.DTO.Trade.MyTrade> MyTrades { get; set; }
    }
}
