
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQConnector.Dto.DTO.Commission.Model
{
    public class CommissionRateGrid:SortedDictionary<decimal,CommissionAdditionalCondition>
    {
        public decimal LookupCurrentRate(object condition)
        {
            var currentAdditionalCondition = ReturnCurrentAdditionalCondition(condition);
            if (currentAdditionalCondition != null)
            {
                return (decimal)this.FirstOrDefault(p => p.Value == currentAdditionalCondition).Value.AdditionalConditionValue;
            }
            else 
            {
                if (this.Keys != null && this.Keys.Count() != 0) 
                {
                    return (decimal)this.First().Value.AdditionalConditionValue;
                }            
            }

            return 0;
        }

        public CommissionAdditionalCondition ReturnCurrentAdditionalCondition(object condition)
        {
            return Values.FirstOrDefault(p => p.AdditionalCondition.Equals(condition));
        }

      
    }
}
