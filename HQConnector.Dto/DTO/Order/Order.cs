using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.Orders;
using System;
using System.Text;

namespace HQConnector.Dto.DTO.Order
{
    public class Order : ViewModelBase
    {
        private string _id;
        public string Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private string _clid;
        public string ClId
        {
            get => _clid;
            set => Set(ref _clid, value);
        }
        private DateTimeOffset _time;
        public DateTimeOffset Time
        {
            get => _time;
            set => Set(ref _time, value);
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => Set(ref _price, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        private Sides _side;
        public Sides Side
        {
            get => _side;
            set => Set(ref _side, value);
        }

        private decimal _stopPrice;
        public decimal StopPrice
        {
            get => _stopPrice;
            set => Set(ref _stopPrice, value);
        }

        private string _stopWorkingType;

        public string StopWorkingType
        {
            get => _stopWorkingType;
            set => Set(ref _stopWorkingType, value);
        }

        private decimal _amountFilled;
        public decimal AmountFilled
        {
            get => _amountFilled;
            set => Set(ref _amountFilled, value);
        }

        private decimal _averagePrice;
        public decimal AveragePrice
        {
            get => _averagePrice;
            set => Set(ref _averagePrice, value);
        }

        public decimal TotalVolume => Price * Amount;

        private bool _reduceOnly;
        public bool ReduceOnly
        {
            get => _reduceOnly;
            set => Set(ref _reduceOnly, value);
        }
        public bool IsOpen => Status == OrderState.OPEN || Status == OrderState.PARTIALLYFILLED;

        public bool IsRegistered => Status != OrderState.NOTREGISTERED;

        private OrderType _type;
        public OrderType Type
        {
            get => _type;
            set => Set(ref _type, value);
        }

      
        private OrderState _orderState;
        public OrderState Status
        {
            get => _orderState;
            set => Set(ref _orderState, value);
        }

        public Order()
        {

        }
        public Order(Exchange exchange,string account,string symbol, Sides side,OrderType type, decimal price,decimal amount,decimal stopprice,string stopworkingtype)
        {
            Exchange = exchange;
            Account = account;
            Pair = symbol;
            Side = side;
            Type = type;
            Price = price;
            Amount = amount;            
            StopPrice = stopprice;
            StopWorkingType = stopworkingtype;

        }

        public Order(Exchange exchange, string account, string symbol, Sides side, OrderType type, bool reduceOnly,decimal price, decimal amount, decimal stopprice, string stopworkingtype)
        {
            Exchange = exchange;
            Account = account;
            Pair = symbol;
            Side = side;
            Type = type;
            ReduceOnly = reduceOnly;
            Price = price;
            Amount = amount;
            StopPrice = stopprice;
            StopWorkingType = stopworkingtype;

        }
        public Order(Exchange exchange,string account,string symbol,string id,string clid)
        {
            Exchange = exchange;
            Account = account;
            Pair = symbol;
            Id = id;
            ClId = clid;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");          
            sb.AppendLine($"Price: {Price}");
            sb.AppendLine($"Amount: {Amount}");
            sb.AppendLine($"Type: {Type}");           
            sb.AppendLine($"Side: {Side.ToString()}");
            sb.AppendLine($"Status: {Status.ToString()}");
            sb.AppendLine($"StopPrice: {StopPrice.ToString()}");
            sb.AppendLine($"AveragePrice: {AveragePrice.ToString()}");
            return sb.ToString();
        }
    }
}
