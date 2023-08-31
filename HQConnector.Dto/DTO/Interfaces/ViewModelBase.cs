using HQConnector.Dto.DTO.Enums.Exchange;

namespace HQConnector.Dto
{
    public class ViewModelBase : PropertyChangedBase
    {
        #region Common fields

        private Exchange _exchange;
        public Exchange Exchange
        {
            get => _exchange;
            set => Set(ref _exchange, value);
        }

        private string _account;
        public string Account
        {
            get => _account;
            set => Set(ref _account, value);
        }

        private string _pair;
        public string Pair
        {
            get => _pair;
            set => Set(ref _pair, value);
        }

        #endregion
    }
}
