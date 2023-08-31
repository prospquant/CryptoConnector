using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Account
{
    public class AccountUpdate
    {
        public IEnumerable<HQConnector.Dto.DTO.Balance.Balance> Balances { get; set; }
        public IEnumerable<HQConnector.Dto.DTO.Position.Position> Positions { get; set; }
    }
}
