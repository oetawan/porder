using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace porder.configuration
{
    public class EndpointConfig
    {
        public string ServiceBusNamespace { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}