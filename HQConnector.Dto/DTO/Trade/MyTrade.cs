using HQConnector.Dto.DTO.Commission.Model;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Enums.MyTrade;
using HQConnector.Dto.DTO.Enums.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Trade
{
    public class MyTrade : ViewModelBase
    {
        #region Properties
        public string Id { get; set; }

        public string OrderId { get; set; }

        public DateTimeOffset Time { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public Sides Side { get; set; }

        public TakerOrMaker CommissionTakerOrMaker { get; set; }
        public CommissionModel Commission { get; set; } 

  
        public decimal BaseCurrencyStepOrCost { get; set; } = 1;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"OrderId: {OrderId}");
            sb.AppendLine($"Exchange: {Exchange}");
            sb.AppendLine($"Pair: {Pair}");
            sb.AppendLine($"Time: {Time}");
            sb.AppendLine($"Side: {Side}");
            sb.AppendLine($"Price: {Price}");
            sb.AppendLine($"Amount: {Amount}");
            sb.AppendLine($"CommissionTakerOrMaker: {CommissionTakerOrMaker}");
            if (Commission != null)
            {
                sb.AppendLine($"CommissionAmount: {Commission.CurrentComissionValue}");
                sb.AppendLine($"CommissionAsset: {Commission.CommissionAsset}");
            }
            sb.AppendLine($"BaseCurrencyStepOrCost: {BaseCurrencyStepOrCost}");
            return sb.ToString();
        }
        #endregion
    }
}
