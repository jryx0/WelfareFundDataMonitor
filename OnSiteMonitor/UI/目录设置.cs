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
    public partial class 目录设置 : Form
    {
        public 目录设置()
        {
            InitializeComponent();

            if (!Directory.Exists(Properties.Settings.Default.ResultDir))
            {
                Properties.Settings.Default.ResultDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }

            this.textBox2.Text = Properties.Settings.Default.ResultDir;

            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                Properties.Settings.Default.WorkDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }

            this.textBox1.Text = Properties.Settings.Default.WorkDir;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new UI.输入目录设置().ShowDialog();

            this.textBox1.Text = Properties.Settings.Default.WorkDir;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            new UI.输出目录设置().ShowDialog();
            this.textBox2.Text = Properties.Settings.Default.ResultDir;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            OnSiteFundComparer.GlobalEnviroment.InitEnviroment();
            this.DialogResult = DialogResult.OK;
        }
    }
}
