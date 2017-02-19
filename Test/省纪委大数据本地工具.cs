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
            DAL.MySqlite testDB = new DAL.MySqlite("d:\\tools\\test.db");

            long tick = DateTime.Now.Ticks;
            Random r = new Random((int)(tick & tick & 0xffffffffL) | (int)(tick >> 32));

            String Sql = "insert into tb values('@p', @n, '@p@n') ";
            try
            {
                testDB.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS tb (name varchar(10),val int,memo varchar(20))");

                testDB.BeginTran();

                for(int i = 0; i < 100; i ++)
                {
                    var sql1 = Sql.Replace("@p", GetRandomString(3, false, true, true, false, "") );

                    for (int j = 0; j < 1000; j++)
                    {
                        var sql2 = sql1.Replace("@n", r.Next().ToString());
                        testDB.ExecuteNonQuery(sql2);
                    }
                    
                }

                testDB.Commit();


            }catch(Exception ex)
            {

            }


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
