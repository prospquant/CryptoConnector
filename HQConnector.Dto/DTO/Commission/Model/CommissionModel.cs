using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Commission.Model
{
    public class CommissionModel
    {
        public Exchange CurrentExchange { get; }

        public CommissionRateModel CurrentComissionRate { get; }

        public decimal CurrentComissionValue { get; set; }

        public string CommissionAsset { get;}

        public CommissionModel(Exchange exchange, CommissionRateModel comissionRate, decimal commissionValue, string commissionAsset)
        {
           
            CurrentExchange = exchange;
            if (comissionRate != null)
            {
                CurrentComissionRate = comissionRate;
            }
            else 
            {
                CurrentComissionRate = CommissionHelper.ReturnCommissionRateByExchange(exchange);

            }
            if (commissionValue != 0)
            {
                CurrentComissionValue = commissionValue;
            }
            if (!String.IsNullOrEmpty(commissionAsset))
            {
                CommissionAsset = commissionAsset;
            }
        }
      

    }
}
