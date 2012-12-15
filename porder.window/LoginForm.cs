using Microsoft.ServiceBus;
using porder.configuration;
using porder.service;
using porder.service.contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace porder.window
{
    public partial class LoginForm : Form
    {
        string errorMessage = string.Empty;
        bool validUsername = false;
        bool validPassword = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!IsValid())
                {
                    throw new ApplicationException(errorMessage);
                }

                ServiceBusManager.Username = txtUsername.Text;
                ServiceBusManager.Password = txtPassword.Text;
                ServiceBusManager.Start();

                configuration.ConfigurationManager.DoNotShowSettingFormAtStartup();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool IsValid()
        {
            errorMessage = string.Empty;

            if (txtUsername.Text.Trim() == string.Empty) {
                errorMessage = "Username required";
                validUsername = false;
            }
            else { 
                validUsername = true; 
            }

            if (txtPassword.Text.Trim() == string.Empty)
            {
                errorMessage += "\r\nPassword required, please enter your password";
                validPassword = false;
            }
            else if (txtPassword.Text.Length < 6)
            {
                errorMessage += "\r\nMinimum password length is 6 characters";
                validPassword = false;
            }
            else
            {
                validPassword = true;
            }

            return validPassword && validUsername;
        }
    }
}