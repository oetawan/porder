using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public static bool ShowSettingAtStartup
        {
            get 
            {
                return System.Configuration.ConfigurationManager.AppSettings["ShowSettingAtStartup"] == "true";
            }
        }

        public static void SaveConfig(string server, string database, string userid, string pwd, string hostUrl)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNode parentNode = xmlDocument.DocumentElement;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == "connectionStrings")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "add" && childNode.Attributes["name"].Value == "Order")
                        {
                            string sqlConnectionString = childNode.Attributes["connectionString"].Value;
                            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(sqlConnectionString);
                            sqlBuilder.DataSource = server;
                            sqlBuilder.InitialCatalog = database;
                            sqlBuilder.IntegratedSecurity = false;
                            sqlBuilder.UserID = userid;
                            sqlBuilder.Password = pwd;

                            //Change any other attributes using the sqlBuilder object
                            childNode.Attributes["connectionString"].Value = sqlBuilder.ConnectionString;
                        }
                    }
                }
                else if (node.Name == "appSettings")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "add" && childNode.Attributes["key"].Value == "HostUrl")
                        {
                            childNode.Attributes["value"].Value = hostUrl;
                        }
                    }
                }
            }
            xmlDocument.Save(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
        public static void DoNotShowSettingFormAtStartup()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNodeList nodes = xmlDocument.GetElementsByTagName("appSettings");
            if (nodes.Count == 0) return;

            foreach (XmlNode childNode in nodes[0].ChildNodes)
            {
                if (childNode.Name == "add" && childNode.Attributes["key"].Value == "ShowSettingAtStartup")
                {
                    childNode.Attributes["value"].Value = "false";
                }
            }
            xmlDocument.Save(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
    }
}