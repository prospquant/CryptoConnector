using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Commission.Model
{
    public class CommissionAdditionalCondition : IComparable<CommissionAdditionalCondition>
    {
        public object AdditionalCondition { get;}

        public object AdditionalConditionValue { get;  set; }

        public CommissionAdditionalCondition(object additionalCondition, object additionalConditionValue) 
        {
            AdditionalCondition = additionalCondition;
            AdditionalConditionValue = additionalConditionValue;
        }

        public int CompareTo(CommissionAdditionalCondition other)
        {
            if (other != null && AdditionalCondition.Equals(other))
            {
                return 0;
            }

            return -1;
        }
    }
}
