
using HQConnector.Dto.DTO.Enums.Commission;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Commission.Model
{
    public class CommissionRateModel
    {
        public ComissionChargingType CommisionChargingType { get; }

        public object CurrentComissionRate { get; }

        public CommissionRateModel(ComissionChargingType comissionChargingType, object currentComissionRate) 
        {
            CommisionChargingType = comissionChargingType;
            CurrentComissionRate = currentComissionRate;
        }
    }
}
