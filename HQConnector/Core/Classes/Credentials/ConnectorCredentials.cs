using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Dto.DTO.Enums.Exchange;

namespace HQConnector.Core.Classes.Credentials
{
    public class ConnectorCredentials : IConnectorCredentials
    {
        #region Properties
        public Exchange Exchange { get; set; }
        public string ConnectorName { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecretKey { get; set; }

        public string Passphrase { get; set; }

       
        #endregion

        #region ctor
        public ConnectorCredentials(string connectorName)
        {
            ConnectorName = connectorName;
           
        }
        public ConnectorCredentials(string connectorName, Exchange exchange) : this(connectorName)
        {
            Exchange = exchange;
        }
        
        public ConnectorCredentials(string connectorName, Exchange exchange, string apiKey, string secretApiKey) : this(connectorName, exchange)
        {
            ApiKey = apiKey;
            ApiSecretKey = secretApiKey;
        }

        public ConnectorCredentials(string connectorName, Exchange exchange, string apiKey, string secretApiKey, string passphrase) : this(connectorName, exchange, apiKey, secretApiKey)
        {
            Passphrase = passphrase;
        }

        #endregion

    }
}
