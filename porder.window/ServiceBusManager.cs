using Microsoft.ServiceBus;
using porder.configuration;
using porder.service;
using porder.service.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
namespace porder.window
{
    public class ServiceBusManager
    {
        private ConfigurationManager cfgMgr;
        private EndpointConfig endpointConfig;
        private ServiceHost sh;
        private ServiceEndpoint endpoint;
        private bool started = false;

        public ServiceBusManager()
        {
            cfgMgr = new ConfigurationManager();
        }

        private void StartBus()
        {
            if (started) return;
            
            endpointConfig = cfgMgr.GetEndpointConfig(Username, Password);
            if (endpointConfig.Error)
                throw new ApplicationException(endpointConfig.ErrorMessage);
            sh = new ServiceHost(typeof(OrderService));
            endpoint = sh.AddServiceEndpoint(
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
            started = true;
        }
        private void StopBus()
        {
            sh.Close();
            instance = null;
        }
        private void TestBus() 
        {
            cfgMgr.FetchHomePage();
        }
        
        public static void Start() 
        {
            if (instance == null)
            {
                instance = new ServiceBusManager();
            }
            instance.StartBus();
        }
        public static void Stop()
        {
            instance.StopBus();
        }
        public static void Test()
        {
            if (instance == null)
                instance = new ServiceBusManager();
            instance.TestBus();
        }

        private static ServiceBusManager instance;
        public static string Username { get; set; }
        public static string Password { get; set; }
    }
}