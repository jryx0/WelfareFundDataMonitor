using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 规则模板添加 : Form
    {
        public string tmpName = "";
        public string type = "2";
        public 规则模板添加()
        {
            InitializeComponent();
            textBox2.Text = "2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tmpName = textBox1.Text;
            textBox2.Text = type;
            this.DialogResult = DialogResult.OK;
        }
    }
}
