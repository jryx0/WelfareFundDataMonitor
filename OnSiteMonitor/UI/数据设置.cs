using OnSiteFundComparer.Models;
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
    

    public partial class 数据设置 : Form
    {
        private DataSetting data;
        public 数据设置()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            data = GetDataSetting();

            tbArgDate.Text = data.ArgDate.ToString();






        }

        private DataSetting GetDataSetting()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSetting();
            this.DialogResult = DialogResult.OK;
        }

        private void SaveSetting()
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.DialogResult = DialogResult.Cancel;
        }
    }
}
