using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Test.DAL;

namespace Test
{
    public partial class Sql工具窗口 : Form
    {
        private int PageIndex = 1;
        public Sql工具窗口()
        {
            InitializeComponent();
        }

        public void SetDB(String DBFile, String DBName)
        {
            if (!File.Exists(DBFile))
                return;

            TreeNode tn = new TreeNode();
            tn.Text = DBName + "("+ Path.GetFileName(DBFile)+")";
            tn.Tag = DBFile;            

            MySqlite db = new MySqlite(DBFile, "dfjwhb2014");
            try
            {
                var ds = db.ExecuteDataset(@"select * from sqlite_master where type='table' or type='view'");
                
                foreach(var dr in ds.Tables[0].AsEnumerable())
                {
                    TreeNode tbTN = new TreeNode();
                    tbTN.Text = dr.Field<String>("tbl_name");
                    tbTN.Tag = DBFile;

                    tn.Nodes.Add(tbTN);
                }
                treeView1.Nodes.Add(tn);
            }
            catch(Exception ex)
            {

            }


        }

        private void treeView1_Leave(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //让选中项背景色呈现红色
                treeView1.SelectedNode.BackColor = Color.Red;
                //前景色为白色
                treeView1.SelectedNode.ForeColor = Color.White;
            }
        }
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView1.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView1.SelectedNode.ForeColor = Color.Black;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag　==　null)
            {
                MessageBox.Show("请选择数据库名称！");
                return;
            }


            GoPage(1);
            //MySqlite _sqlite = new MySqlite((string)treeView1.SelectedNode.Tag, "dfjwhb2014");

            //try
            //{
            //   var ds = _sqlite.ExecuteDataset(tbSql.Text + " limit 100");

            //    dataGridView1.DataSource = null;
            //    dataGridView1.DataSource = ds.Tables[0];

            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestForm1 tf = new TestForm1();

            tf.ShowDialog();

            SetDB(tf.configDB, "配置数据库");
            SetDB(tf.resultDB, "结果数据库");
            SetDB(tf.ImportDB, "导入数据库");

        }


        void GoPage(int index)
        {
            int _size = 15;
            MySqlite _sqlite = new MySqlite((string)treeView1.SelectedNode.Tag, "dfjwhb2014");

            try
            {


                String Sql = tbSql.Text.Trim();
                if (tbSql.SelectedText.Length != 0)
                    Sql = tbSql.SelectedText.Trim();

                 

                if (Sql.ToUpper().StartsWith("SELECT"))
                {
                    Sql = Sql + String.Format(" limit {0} Offset {0}*{1}", _size, index - 1);

                    var ds = _sqlite.ExecuteDataset(Sql);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = ds.Tables[0];
                }
                else
                    _sqlite.ExecuteNonQuery(Sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (PageIndex > 1)
                PageIndex--;
            else PageIndex = 1;

            GoPage(PageIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GoPage(++PageIndex);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
