using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.Symbol;
using System.ComponentModel;

namespace HQConnector.Dto.DTO.Symbol
{
    public class Symbol
    {
       

        public Exchange Exchange { get; set; }

        public string SymbolCode { get; set; }

        public SymbolType SymbolType { get; set; }

        public bool IsMargin { get; set; }
        public bool IsActive { get; set; }

        public decimal? MaxSize { get; set; }

        public decimal? MinSize { get; set; }

        public decimal? MaxPrice { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? PriceStep { get; set; }

        public decimal? PricePrecission { get; set; }

        public decimal? LotPrecission { get; set; }

        [DefaultValue(1)]
        public decimal MarginValue { get; set; }

        public Symbol(string symbolCode, Exchange exchange)
        {
            SymbolCode = symbolCode;
            Exchange = exchange;
        }

        public override string ToString()
        {
            return SymbolCode;
        }
    }
}
