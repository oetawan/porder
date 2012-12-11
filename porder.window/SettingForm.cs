using porder.configuration;
using porder.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace porder.window
{
    public partial class SettingForm : Form
    {
        private bool requireRestart;

        public SettingForm()
        {
            InitializeComponent();
        }

        public SettingForm(bool requireRestart)
            : this()
        {
            this.requireRestart = requireRestart;
        }

        public static bool NotConfigured()
        {
            System.Configuration.ConnectionStringSettings cnSetting = System.Configuration.ConfigurationManager.ConnectionStrings["Order"];
            List<string> cnStrings = cnSetting.ConnectionString.Split(';').ToList();

            string host = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];
            string server = cnStrings.Find(s => s.Split('=')[0].ToLower() == "data source").Split('=')[1];
            string database = cnStrings.Find(s => s.Split('=')[0].ToLower() == "initial catalog").Split('=')[1];
            string userid = cnStrings.Find(s => s.Split('=')[0].ToLower() == "user id").Split('=')[1];
            string password = cnStrings.Find(s => s.Split('=')[0].ToLower() == "password").Split('=')[1];

            return host.Trim() == string.Empty ||
                   server.Trim() == string.Empty ||
                   database.Trim() == string.Empty ||
                   userid.Trim() == string.Empty ||
                   password.Trim() == string.Empty;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            ReadConfig();   
        }

        private void ReadConfig()
        {
            System.Configuration.ConnectionStringSettings cnSetting = System.Configuration.ConfigurationManager.ConnectionStrings["Order"];
            List<string> cnStrings = cnSetting.ConnectionString.Split(';').ToList();

            this.txtHost.Text = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];
            this.txtServer.Text = cnStrings.Find(s => s.Split('=')[0].ToLower() == "data source").Split('=')[1];
            this.txtDatabase.Text = cnStrings.Find(s => s.Split('=')[0].ToLower() == "initial catalog").Split('=')[1];
            this.txtUsername.Text = cnStrings.Find(s => s.Split('=')[0].ToLower() == "user id").Split('=')[1];
            this.txtPassword.Text = cnStrings.Find(s => s.Split('=')[0].ToLower() == "password").Split('=')[1];
        }

        private void SaveConfig()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

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
                                sqlBuilder.DataSource = txtServer.Text;
                                sqlBuilder.InitialCatalog = txtDatabase.Text;
                                sqlBuilder.IntegratedSecurity = false;
                                sqlBuilder.UserID = txtUsername.Text;
                                sqlBuilder.Password = txtPassword.Text;

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
                                childNode.Attributes["value"].Value = txtHost.Text;
                            }
                        }
                    }
                }
                xmlDocument.Save(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                System.Configuration.ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("Saved.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (requireRestart)
                {
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Test()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OrderDbContext dbContext = new OrderDbContext();
                dbContext.Database.Connection.Open();
                dbContext.Database.Connection.Close();
                
                ServiceBusManager.Test();

                MessageBox.Show("Test OK", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Test();
        }
    }
}