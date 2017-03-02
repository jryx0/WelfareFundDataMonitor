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

        private void button4_Click(object sender, EventArgs e)
        {
 

        }

 


        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}
