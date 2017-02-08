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
    public partial class 输出目录设置 : Form
    {
        private string CurrentDir = Properties.Settings.Default.ResultDir;
        public 输出目录设置()
        {
            InitializeComponent();

            if(!Directory.Exists(Properties.Settings.Default.ResultDir))
            {
                Properties.Settings.Default.ResultDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }
            
            this.textBox1.Text = Properties.Settings.Default.ResultDir;
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
            Properties.Settings.Default.ResultDir = CurrentDir;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
