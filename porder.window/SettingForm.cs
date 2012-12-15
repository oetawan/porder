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

                configuration.ConfigurationManager.SaveConfig(txtServer.Text, txtDatabase.Text, txtUsername.Text, txtPassword.Text, txtHost.Text);

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