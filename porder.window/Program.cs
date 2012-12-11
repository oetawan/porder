﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace porder.window
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (SettingForm.NotConfigured())
            {
                SettingForm settingForm = new SettingForm(true);
                settingForm.ShowDialog();
                settingForm.Dispose();
            }

            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                MainForm mainForm = new MainForm();
                Application.Run(mainForm);
            }
        }
    }
}