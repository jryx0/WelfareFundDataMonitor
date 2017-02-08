using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Test
{
    public partial class Sql工具窗口 : Form
    {
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

            OnSiteFundComparer.DAL.MySqlite db = new OnSiteFundComparer.DAL.MySqlite(DBFile);
            try
            {
                var ds = db.ExecuteDataset(@"select * from sqlite_master where type='table'");
                
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



            OnSiteFundComparer.DAL.MySqlite _sqlite = new OnSiteFundComparer.DAL.MySqlite((string)treeView1.SelectedNode.Tag);

            try
            {
               var ds = _sqlite.ExecuteDataset(tbSql.Text);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ds.Tables[0];

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
