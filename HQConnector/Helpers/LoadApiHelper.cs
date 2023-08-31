using HQConnector.Dto.DTO.Enums.Exchange;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using HQConnector.Core.Classes.Credentials;

namespace HQConnector.Helpers
{
    public static class LoadApiHelper
    {
        public static  List<ConnectorCredentials> LoadApiFromFile(string filepath,char[] lineseparator)
        {
            var credentials = new List<ConnectorCredentials>();
            try
            {

                using (var file = new System.IO.StreamReader(filepath))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var lineElements = line.Split(lineseparator);
                        if (lineElements.Length != 0)
                        {
                            credentials.Add(new ConnectorCredentials(lineElements[0],(Exchange)Enum.Parse(typeof(Exchange), lineElements[1]), lineElements[2], lineElements[3], lineElements[4]));
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return credentials;
        }
    }
}
