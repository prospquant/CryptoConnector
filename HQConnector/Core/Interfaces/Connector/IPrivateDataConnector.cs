using HQConnector.Core.Interfaces.Constants;
using HQConnector.Core.Interfaces.Credentials;
using HQConnector.Core.Interfaces.Rest;
using HQConnector.Core.Interfaces.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Core.Interfaces.Connector
{
   public interface IPrivateDataConnector : IConnectorSettings,IRestPrivateData, ISocketPrivateData
    {
        IConnectorCredentials Credentials { get; set; }
    }
}
