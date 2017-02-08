using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class 省纪委大数据本地工具 : Form
    {
        public 省纪委大数据本地工具()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CrytLocalDB cdb = new CrytLocalDB();

            cdb.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestDataGenerator tdg = new TestDataGenerator();
            tdg.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Sql工具窗口().ShowDialog();
        }
    }
}
