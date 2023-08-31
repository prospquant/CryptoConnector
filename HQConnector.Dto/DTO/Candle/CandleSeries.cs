using HQConnector.Dto.DTO.Enums.Candle;
using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Candle
{
    [Serializable]
    public class CandleSeries
    {
        public CandleType CandleType { get; set; }

        public decimal CandleTimeFrame { get; set; }

        public decimal CandleParametr { get; set; }
        public CandleSource CandleSource { get; set; }

        public string SymbolCode { get; set; }

        public Exchange Exchange { get; set; }

        public bool IsCandleWithTail { get; set; }
        public CandleSeries(CandleType candleType, decimal candleTimeFrame,decimal candleParametr, CandleSource candleSource, string symbolCode, Exchange exchange)
        {
            CandleType = candleType;
            CandleTimeFrame = candleTimeFrame;
            CandleParametr = candleParametr;
            CandleSource = candleSource;
            SymbolCode = symbolCode;
            Exchange = exchange;
        }

        public CandleSeries(CandleType candleType, decimal candleTimeFrame, decimal candleParametr, CandleSource candleSource, string symbolCode, Exchange exchange,bool isCandleWithTail)
        {
            CandleType = candleType;
            CandleTimeFrame = candleTimeFrame;
            CandleParametr = candleParametr;
            CandleSource = candleSource;
            SymbolCode = symbolCode;
            Exchange = exchange;
            IsCandleWithTail = isCandleWithTail;
        }

        public CandleSeries()
        {
        }

        public override string ToString()
        {
            return CandleType.ToString("G") + "_" + Exchange.ToString("G") + "_" + SymbolCode + "_" + CandleTimeFrame + "_" + CandleParametr;
            
        }
    }
}
