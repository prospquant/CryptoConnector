using HQConnector.Dto.DTO.Enums.Position;
using System.Diagnostics;
using System.Text;
using System;

namespace HQConnector.Dto.DTO.Position
{
    public class Position : ViewModelBase
    {
        #region Properties
        private decimal _size;
        public decimal Size
        {
            get => _size;
            set => Set(ref _size, value);
        }

        private PositionSide _positionSide;

        public PositionSide PositionSide
        {
            get => _positionSide;
            set => Set(ref _positionSide, value);
        }

        private decimal _averagePrice;
        public decimal AveragePrice
        {
            get => _averagePrice;
            set => Set(ref _averagePrice, value);
        }

        private decimal _margin;
        public decimal Margin
        {
            get => _margin;
            set => Set(ref _margin, value);
        }

        private decimal _pnl;
        public decimal PnL
        {
            get => _pnl;
            set => Set(ref _pnl, value);
        }

        private decimal _liquidationPrice;
        public decimal LiquidationPrice
        {
            get => _liquidationPrice;
            set => Set(ref _liquidationPrice, value);
        }

        private bool? _isOpen;
        public bool? IsOpen
        {
            get => _isOpen;
            set => Set(ref _isOpen, value);
        }


        #endregion

        #region ctor

        public Position(string connectorName)
        {
            Account = connectorName;
        }
        public Position()
        {
            
        }
        #endregion

        #region Methods

        public void UpdateValues(Position position)
        {
            if (position.PnL != PnL)
            {
                PnL = position.PnL;
            }

            if (Margin != position.Margin)
            {
                Margin = position.Margin;
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");
            sb.AppendLine($"PositionSide: {PositionSide}");
            sb.AppendLine($"AveragePrice: {AveragePrice.ToString()}");
            sb.AppendLine($"Size: {Size}");
            return sb.ToString();
        }
        #endregion
    }
}
