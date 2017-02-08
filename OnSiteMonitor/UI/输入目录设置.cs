using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 输入目录设置 : Form
    {
        private string CurrentDir = Properties.Settings.Default.WorkDir;
        public 输入目录设置()
        {
            InitializeComponent();

            if(!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                Properties.Settings.Default.WorkDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }
            
            this.textBox1.Text = Properties.Settings.Default.WorkDir;

            if(GlobalEnviroment.LoginedUser.Name == "admin")
            {
                button1.Visible = true;
                button3.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdr = new FolderBrowserDialog();
            if(fdr.ShowDialog() == DialogResult.OK)
            {
                CurrentDir = fdr.SelectedPath;               
                this.textBox1.Text = CurrentDir;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.WorkDir = CurrentDir;

            //try
            //{
            //    RegistryKey key = Registry.CurrentUser;
            //    RegistryKey src = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("hubeijw", true);
            //    if (src == null)
            //    {
            //        src = key.CreateSubKey("software\\hubeijw"); // WelfareMonitorWorkDir");
            //        src.SetValue("WelfareMonitorWorkDir", CurrentDir);
            //    }
            //    else
            //    {
            //        src.SetValue("WelfareMonitorWorkDir", CurrentDir);
            //    }
            //}
            //catch (Exception ex)
            //{
              
            //}

            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
