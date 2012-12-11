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
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;

            
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnRestore(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
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
    }
}