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
    public partial class 日期标签管理 : Form
    {
        public List<string> Labels { get; internal set; }
        public List<string> newLabels { get; internal set; }

        public 日期标签管理()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            initGridView();

            base.OnLoad(e);
        }

        private void initGridView()
        {
            this.dataGridView1.Columns.Add("Name", "标签");
            this.dataGridView1.Columns[0].Width = 300;
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[0].ReadOnly = true;

            if (Labels != null)
                foreach (string l in Labels)
                {
                    int index = this.dataGridView1.Rows.Add(1);
                    this.dataGridView1.Rows[index].Cells[0].Value = l;
                }

        }



        private void btnAddLabel_Click(object sender, EventArgs e)
        {
            bool isExist = false;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (tbLabel.Text == r.Cells[0].ToString())
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist)
                MessageBox.Show("日期已存在,请修改日期");
            else
            {
                int index = this.dataGridView1.Rows.Add(1);
                this.dataGridView1.Rows[index].Cells[0].Value = this.tbLabel.Text;
            }

        }

        private void btnDelabel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除标签将会删除标签下的所有文件!", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
                return;

            foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
            {
                this.dataGridView1.Rows.Remove(r);
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            newLabels = new List<string>();
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                string l = r.Cells[0].Value.ToString();
                newLabels.Add(l);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
