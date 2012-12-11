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
        static bool fetched = false;

        public void FetchHomePage()
        {
            if (fetched) return;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["HostUrl"]);
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                fetched = false;
                throw new ApplicationException(string.Format("{0} ({1})", httpResponse.StatusDescription, httpResponse.StatusCode));
            }
            else
            {
                fetched = true;
            }
        }

        public EndpointConfig GetEndpointConfig(string username, string password)
        {
            FetchHomePage();

            EndpointConfig endpointConfig = null;

            
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["HostUrl"] + "/Customer/Login");
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                CustomerLoginModel login = new CustomerLoginModel { Username = username, Password = password };
                var loginjson = Newtonsoft.Json.JsonConvert.SerializeObject(login);
                
                streamWriter.Write(loginjson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
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