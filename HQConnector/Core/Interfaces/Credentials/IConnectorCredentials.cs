using System;
using System.Collections.Generic;
using System.Text;
using HQConnector.Dto.DTO.Enums.Exchange;

namespace HQConnector.Core.Interfaces.Credentials
{
    public interface IConnectorCredentials
    {
        Exchange Exchange { get; set; }
        string ConnectorName { get; set; }

        string ApiKey { get; set; }

        string ApiSecretKey { get; set; }

        string Passphrase { get; set; }

     

        
    }
}
