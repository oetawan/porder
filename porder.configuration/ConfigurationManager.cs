using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace porder.configuration
{
    public class ConfigurationManager
    {
        public EndpointConfig GetEndpointConfig(string username, string password)
        {
            EndpointConfig endpointConfig = null;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["ConfigurationUrl"]);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                CustomerLoginModel login = new CustomerLoginModel { Username = username, Password = password };
                var loginjson = Newtonsoft.Json.JsonConvert.SerializeObject(login);
                
                streamWriter.Write(loginjson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    endpointConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<EndpointConfig>(result);
                }
            }

            return endpointConfig;
        }
    }
}