using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class CrytLocalDB : Form
    {
        #region
        String sql = @"insert into newDB.@table(@cols) select @cols from @table";
        String password = "dfjwhb2014";
        #endregion

        enum DBType
        {
            Cryt,
            UnCryt,
        }

        public CrytLocalDB()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbDBDir.Text = ofd.FileName;

                if (TestDBType(ofd.FileName) == DBType.Cryt)
                {
                    button2.Enabled = false;
                    button4.Enabled = true;
                }
                else
                {
                    button2.Enabled = true;
                    button4.Enabled = false;
                }
             //   tbNewDB.Text = ofd.FileName;// + ".cryt";
            }
        }

        private DBType TestDBType(string fileName)
        {
            DBType bRet = DBType.UnCryt;

            DAL.MySqlite sourceDB = new DAL.MySqlite();
            sourceDB.sqliteConnectionString = fileName;           

            try
            {
                var ds = sourceDB.ExecuteDataset(@"select tbl_name, sql from sqlite_master
                                    where type = 'table' and tbl_name not like '%sqlite%'");
            }
            catch (Exception ex)
            {
                bRet = DBType.Cryt;
            }

            return bRet;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Cursor = Cursors.WaitCursor;

            List<String> tables = new List<string>();


            DAL.MySqlite sourceDB = new DAL.MySqlite();
            sourceDB.sqliteConnectionString = tbDBDir.Text;

            DAL.MySqlite tragetDB = new DAL.MySqlite();

            var newDB = tbDBDir.Text.Replace(".cryt", "");
            newDB = newDB.Replace(".Uncryt", "");

            tragetDB.sqliteConnectionString = newDB + ".cryt";
            tragetDB.sqlitePassword = "dfjwhb2014";

            try
            {
                var ds = sourceDB.ExecuteDataset(@"select tbl_name, sql from sqlite_master
                                    where type = 'table' and tbl_name not like '%sqlite%'");

                sourceDB.AttchDatabase(tragetDB, "newDB");

                if (ds == null)
                    return;

                var list = from r in ds.Tables[0].AsEnumerable()
                           select new
                           {
                               tablename = r.Field<String>("tbl_name"),
                               Sql = r.Field<String>("sql"),
                           };


                foreach(var l in list)
                {
                    String cols = GetCols(l.Sql);

                    var CreateSql = sql.Replace("@table", l.tablename);
                    CreateSql = CreateSql.Replace("@cols", cols);

                    tragetDB.ExecuteNonQuery(l.Sql);
                    sourceDB.ExecuteNonQuery(CreateSql);

                }

                MessageBox.Show(
                    "成功生成加密数据库：" + System.IO.Path.GetFileName(tragetDB.sqliteConnectionString)
                    );
                tbNewDB.Text = tragetDB.sqliteConnectionString;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Cursor = Cursors.Arrow;
        }

        private string GetCols(string sql)
        {
            if (sql == null)
                return "";

            int startIndex = sql.IndexOf("(");
            int endIndex = sql.LastIndexOf(")");

            var colums= sql.Substring(startIndex + 1);
            var Fields = colums.Split(',');

            string cols = "";
            foreach(var _f in Fields)
            {               
                var fs = _f.Trim().Split(' ');

                if (fs[0] == "UNIQUE")
                    continue;
                //f0  name
                //f1  type




                cols += fs[0] + ",";
            }



            return cols.Trim(',');
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            List<String> tables = new List<string>();

            DAL.MySqlite sourceDB = new DAL.MySqlite();
            sourceDB.sqliteConnectionString = tbDBDir.Text;
            sourceDB.sqlitePassword = "dfjwhb2014";

            DAL.MySqlite tragetDB = new DAL.MySqlite();

            var newDB = tbDBDir.Text.Replace(".cryt", "");
            newDB = newDB.Replace(".Uncryt", "");

            tragetDB.sqliteConnectionString = newDB + ".Uncryt"; ;


            try
            {
                var ds = sourceDB.ExecuteDataset(@"select tbl_name, sql from sqlite_master
                                    where type = 'table' and tbl_name not like '%sqlite%'");

                tragetDB.AttchDatabase(sourceDB, "DB");
                String newsql = @"insert into @table(@cols) select @cols from DB.@table";
                // sourceDB.ExecuteNonQuery("ATTACH DATABASE '" + tbNewDB.Text + "' as newDB");

                if (ds == null)
                    return;

                var list = from r in ds.Tables[0].AsEnumerable()
                           select new
                           {
                               tablename = r.Field<String>("tbl_name"),
                               Sql = r.Field<String>("sql"),
                           };

                foreach (var l in list)
                {
                    String cols = GetCols(l.Sql);

                    var CreateSql = newsql.Replace("@table", l.tablename);
                    CreateSql = CreateSql.Replace("@cols", cols);

                    tragetDB.ExecuteNonQuery(l.Sql);
                    tragetDB.ExecuteNonQuery(CreateSql);
                }

                MessageBox.Show(
                    "成功生成脱密数据库：" + System.IO.Path.GetFileName(tragetDB.sqliteConnectionString)
                    );

                tbNewDB.Text = tragetDB.sqliteConnectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            sourceDB.CloseConnection();
            tragetDB.CloseConnection();

            this.Cursor = Cursors.Arrow;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
          
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                tbNewDB.Text = sfd.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(tbDBDir.Text))
            {
                var dir = System.IO.Path.GetDirectoryName(tbDBDir.Text);
                if (System.IO.Directory.Exists(dir))
                    Process.Start(dir);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var dir = tbNewDB.Text.Substring(0, tbNewDB.Text.LastIndexOf("\\"));

            if (System.IO.Directory.Exists(dir))
                Process.Start(dir);

        }
    }
}
