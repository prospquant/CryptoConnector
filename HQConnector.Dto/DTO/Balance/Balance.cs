using System.Text;

namespace HQConnector.Dto.DTO.Balance
{
    public class Balance : ViewModelBase
    {
        private string _exchangeAccount;
        public string ExchangeAccount
        {
            get => _exchangeAccount;
            set => Set(ref _exchangeAccount, value);
        }

        private string _symbolCurrency;
        public string SymbolCurrency
        {
            get => _symbolCurrency;
            set => Set(ref _symbolCurrency, value);
        }

        private decimal _freeAmount;
        public decimal FreeAmount
        {
            get => _freeAmount;
            set => Set(ref _freeAmount, value);
        }

        private decimal _blockedAmount;
        public decimal BlockedAmount {
            get => _blockedAmount;
            set => Set(ref _blockedAmount, value);

        }

        private decimal _walletBalance;
        public decimal WalletBalance
        {
            get => _walletBalance;
            set => Set(ref _walletBalance, value);

        }

        private decimal _marginBalance;
        public decimal MarginBalance
        {
            get => _marginBalance;
            set => Set(ref _marginBalance, value);

        }

        private decimal _availBalance;
        public decimal AvailBalance
        {
            get => _availBalance;
            set => Set(ref _availBalance, value);

        }

       

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exchange: {Exchange}");           
            sb.AppendLine($"Currency: {SymbolCurrency}");
            sb.AppendLine($"Free: {FreeAmount}");
            sb.AppendLine($"Blocked: {BlockedAmount}");
            sb.AppendLine($"Blocked: {WalletBalance}");
            return sb.ToString();
        }
    }
}
