
using HQConnector.Dto.DTO.Commission.Model;
using HQConnector.Dto.DTO.Enums.Commission;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.MyTrade;
using HQConnector.Dto.DTO.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQConnector.Dto.DTO.Commission
{
    public static class CommissionHelper
    {
        public static decimal GetCommissionFromTrades(IEnumerable<MyTrade> trades)
        {
            if (trades != null && trades.Count() != 0 && trades.All(p=> p.Commission != null))
            {
                return trades.Sum(p => p.Commission.CurrentComissionValue);
            }
            return 0;
        }

        public static IEnumerable<Tuple<string,decimal>> GetCommissionFromTradesByAsset(IEnumerable<MyTrade> trades)
        {
            var result = new List<Tuple<string, decimal>>();
            if (trades != null && trades.Count() != 0)
            {
                var comissionassets = trades.Select(p => p.Pair).ToList();
                foreach (var asset in comissionassets)
                {
                    result.Add(new Tuple<string, decimal>(asset, trades.Where(p => p.Pair == asset).ToList().Sum(m => m.Commission.CurrentComissionValue)));
                }
            }
            return result;
        }

        public static CommissionRateModel ReturnCommissionRateByExchange(Exchange exchange)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return ExchangeCommisionRates.BinanceDefaultCommissionRate();
                case Exchange.BinanceFutures:
                    return ExchangeCommisionRates.BinanceFuturesDefaultCommissionRate();
                case Exchange.BitMEX:
                    return ExchangeCommisionRates.BitMEXDefaultCommissionRate();
            }

            return null;
        }

        public static CommissionRateGrid MakeCommissionRateGridByCondition(IEnumerable<CommissionAdditionalCondition> conditonsWithValues)
        {
            var comgrid = new CommissionRateGrid();
            if (conditonsWithValues != null && conditonsWithValues.Count() != 0) 
            {
                for(int i = 0;i < conditonsWithValues.Count();i++) 
                {
                    try
                    {
                        var item = conditonsWithValues.ToList()[i];
                        comgrid.Add(i, new CommissionAdditionalCondition(item.AdditionalCondition, item.AdditionalConditionValue));
                    }
                    catch (Exception ex) 
                    {
                    
                    }
                }
            }
          

            return comgrid;
        }

        public static decimal CalculateCommissionOnCommissionRate(CommissionRateModel currentrates,MyTrade trade, object condition) 
        {
            decimal currentcommission = 0;
            if (currentrates != null)
            {
                try
                {
                    if (currentrates.CommisionChargingType == ComissionChargingType.Percentage)
                    {
                        currentcommission = (trade.Price * trade.Amount) * ((decimal)currentrates.CurrentComissionRate / 100);
                    }
                    if (currentrates.CommisionChargingType == ComissionChargingType.PercentageGrid)
                    {
                        var currentgrid = (CommissionRateGrid)currentrates.CurrentComissionRate;

                        currentcommission = (trade.Price * trade.Amount) * (currentgrid.LookupCurrentRate(condition) / 100);
                    }
                }
                catch (Exception ex) 
                {
                
                }
            }
            return currentcommission;
        }

        public static decimal CalculateCommissionOnCommissionRate(Exchange exchange, MyTrade trade, object condition)
        {

            decimal currentcommission = 0;
            var currentrates = ReturnCommissionRateByExchange(exchange);
            if (currentrates != null)
            {
                try
                {
                    if (currentrates.CommisionChargingType == ComissionChargingType.Percentage)
                    {
                        currentcommission = (trade.Price * trade.Amount) * ((decimal)currentrates.CurrentComissionRate / 100);
                    }
                    if (currentrates.CommisionChargingType == ComissionChargingType.PercentageGrid)
                    {
                        var currentgrid = (CommissionRateGrid)currentrates.CurrentComissionRate;

                        currentcommission = (trade.Price * trade.Amount) * (currentgrid.LookupCurrentRate(condition) / 100);
                    }
                }
                catch (Exception ex) 
                {
                
                }
            }
            return currentcommission;
        }

        public static decimal CalculateCommissionOnCommissionRate(CommissionRateModel currentrates, decimal price, decimal amount, object condition)
        {
            decimal currentcommission = 0;
            if (currentrates != null)
            {
                try
                {
                    if (currentrates.CommisionChargingType == ComissionChargingType.Percentage)
                    {
                        currentcommission = (price * amount) * ((decimal)currentrates.CurrentComissionRate / 100);
                    }
                    if (currentrates.CommisionChargingType == ComissionChargingType.PercentageGrid)
                    {
                        var currentgrid = (CommissionRateGrid)currentrates.CurrentComissionRate;

                        currentcommission = (price * amount) * (currentgrid.LookupCurrentRate(condition) / 100);
                    }
                }
                catch (Exception ex) 
                {
                
                }
            }
            return currentcommission;
        }

        public static decimal CalculateCommissionOnCommissionRate(Exchange exchange, decimal price, decimal amount, object condition)
        {
            decimal currentcommission = 0;
            var currentrates = ReturnCommissionRateByExchange(exchange);
            if (currentrates != null)
            {
                try
                {

                    if (currentrates.CommisionChargingType == ComissionChargingType.Percentage)
                    {
                        currentcommission = (price * amount) * ((decimal)currentrates.CurrentComissionRate / 100);
                    }
                    if (currentrates.CommisionChargingType == ComissionChargingType.PercentageGrid)
                    {
                        var currentgrid = (CommissionRateGrid)currentrates.CurrentComissionRate;

                        currentcommission = (price * amount) * (currentgrid.LookupCurrentRate(condition) / 100);
                    }
                }
                catch (Exception ex) 
                {
                
                }
            }
            return currentcommission;
        }
    }
}
