
using HQConnector.Dto.DTO.Commission.Model;
using HQConnector.Dto.DTO.Enums.Commission;
using HQConnector.Dto.DTO.Enums.MyTrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Commission
{
    public static class ExchangeCommisionRates
    {
        public static CommissionRateModel BinanceDefaultCommissionRate() 
        {
            
            return new CommissionRateModel(ComissionChargingType.Percentage, 0.1m); 
        }

        public static CommissionRateModel BinanceFuturesDefaultCommissionRate()
        {
            var comgrid = new CommissionRateGrid();
            comgrid.Add(1, new CommissionAdditionalCondition(TakerOrMaker.Maker, 0.02m));
            comgrid.Add(-1, new CommissionAdditionalCondition(TakerOrMaker.Taker, 0.04m));

            return new CommissionRateModel(ComissionChargingType.PercentageGrid, comgrid);
        }

        public static CommissionRateModel BitMEXDefaultCommissionRate()
        {
            var comgrid = new CommissionRateGrid();
            comgrid.Add(1, new CommissionAdditionalCondition(TakerOrMaker.Maker, -0.01m));
            comgrid.Add(-1, new CommissionAdditionalCondition(TakerOrMaker.Taker, 0.05m));

            return new CommissionRateModel(ComissionChargingType.PercentageGrid, comgrid);
        }

    }
}
