using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDynamics1
{
    public class ConnectionFactory
    {
        public static IOrganizationService GetCrmService()
        {
            string connectionString = "AuthType=OAuth;" +
              "Username=admin@tccgrupo05.onmicrosoft.com;" +
              "Password=P@ssw0rd;" +
              "Url=https://dynamics2g05tcc.crm2.dynamics.com/;" +
              "AppId=4ecf7d92-db21-41cb-90ca-e4e46da8496e;" +
              "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            return crmServiceClient.OrganizationWebProxyClient;

        }
    }
}
