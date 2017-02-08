//#define 试点

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
    public partial class 比对项目 : Form
    {
        private string testDBFile = "";

        public 比对项目()
        {
            InitializeComponent();
            initGridView();
#if 试点
            button1.Visible = false;
            button2.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
#endif

        }

        void initGridView(int index = -1)
        {
            Business.DataMgr dm = new Business.DataMgr();

            var ds = dm.GetAllAimDataSet();
            if (ds == null)
                return;

            this.dataGridView1.DataSource = ds.Tables[0];

            if (index > 0 && index < this.dataGridView1.Rows.Count)
            {
                
                this.dataGridView1.ClearSelection();
                this.dataGridView1.Rows[index].Selected = true;
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];
            }

            if (this.dataGridView1.Columns.Count > 2)
            {
                this.dataGridView1.Columns[0].Visible = false;
                

                this.dataGridView1.Columns[1].Visible = false;
                this.dataGridView1.Columns[2].Width = 250;
                this.dataGridView1.Columns[3].Width = 450;

                this.dataGridView1.Columns[5].Visible = false;
                this.dataGridView1.Columns[8].Visible = false;
                this.dataGridView1.Columns[7].Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            string rowid = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            ShowDetail(rowid);
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
#if 试点

#else
            if (e.RowIndex < 0)
                return;

            string rowid = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            ShowDetail(rowid);
#endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
            比对项目设置 dlg = new 比对项目设置();            

            dlg.ShowDialog();
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.DataSource = null;

            initGridView();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            string rowid = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            Business.DataMgr dm = new Business.DataMgr();
            var ca = dm.GetAimbyID(rowid);
            if (ca != null)
            {
               if( MessageBox.Show("确认删除吗?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    dm.DeleteCollisionAim(ca);

                    this.dataGridView1.Columns.Clear();
                    this.dataGridView1.DataSource = null;

                    initGridView();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(ofd.ShowDialog()== DialogResult.OK)
            {
                testDBFile = ofd.FileName;
            }
        }

        private void ShowDetail(string id)
        {
            Business.DataMgr dm = new Business.DataMgr();
            var ca = dm.GetAimbyID(id);

            if (ca != null)
            {
                比对项目设置 dlg = new 比对项目设置();
                dlg.collisionAim = ca;
                dlg.testDBFile = testDBFile;

                dlg.ShowDialog();

                int index = 0;
                if (this.dataGridView1.SelectedRows.Count >= 1)
                    index = this.dataGridView1.CurrentCell.RowIndex;

                this.dataGridView1.Columns.Clear();
                this.dataGridView1.DataSource = null;

                initGridView(index);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           Test.TestForm1 tf = new Test.TestForm1();

            tf.ShowDialog();
        }
    }
}
