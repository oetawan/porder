using System;
using System.Linq;
using System.ServiceModel;
using porder.service.contract;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using porder.configuration;

namespace porder.service.host
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                ConfigurationManager cfgMgr = new ConfigurationManager();
                EndpointConfig endpointConfig = cfgMgr.GetEndpointConfig(username, password);

                if (endpointConfig.Error)
                    throw new ApplicationException(endpointConfig.ErrorMessage);
                
                ServiceHost sh = new ServiceHost(typeof(OrderService));
                ServiceEndpoint endpoint = sh.AddServiceEndpoint(
                    typeof(IOrderService),
                    new NetTcpRelayBinding(),
                    ServiceBusEnvironment.CreateServiceUri("sb", endpointConfig.ServiceBusNamespace, "order"));
                
                endpoint.Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(
                        endpointConfig.Issuer, 
                        endpointConfig.SecretKey)
                });
                sh.Open();

                Console.WriteLine("Press ENTER to close");
                Console.ReadLine();

                sh.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}