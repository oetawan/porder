using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace porder.window
{
    public partial class MainForm : Form
    {
        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;

        public MainForm()
        {
            InitializeComponent();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("&Restore", OnRestore);
            trayMenu.MenuItems.Add("E&xit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Service Host";
            trayIcon.Icon = new Icon("1355324326_Cloud.ico", 32, 32);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnRestore(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ServiceBusManager.Stop();
            }
            catch (Exception ex) 
            {
                e.Cancel = true;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StopServiceBus()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ServiceBusManager.Stop();
                stopImg.Visible = true;
                startImg.Visible = false;
                lblStatus.Text = "Stoped";
                btnStop.Visible = false;
                btnStart.Visible = true;
                lblStatus.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void StartServiceBus()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ServiceBusManager.Start();
                stopImg.Visible = false;
                startImg.Visible = true;
                lblStatus.Text = "Started";
                btnStop.Visible = true;
                btnStart.Visible = false;
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServiceBus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServiceBus();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            new SettingForm().ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            HideMainForm();
        }

        private void HideMainForm()
        {
            Visible = false;
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            HideMainForm();
        }
    }
}