using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 显示列表 : Form
    {
        public string TitleName;

        public DataTable DataSource;
        public DataTable DataSource2;
        public 显示列表()
        {
            InitializeComponent();           
        }

        protected override void OnLoad(EventArgs e)            
        {
            //showFile(fileName);
            showData();
            base.OnLoad(e);
        }


        

        private void showData()
        {
            int unread = 0;
            if (DataSource == null)
                return;

            if (DataSource2 != null)
                unread = DataSource2.Rows.Count;

            label1.Text = "总数：" + (DataSource.Rows.Count + unread) + "正确读入数据: " + DataSource.Rows.Count + ", 错误数据: " + unread;


            this.dataGridView1.DataSource = DataSource;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].Visible = false;
            this.dataGridView1.Columns[2].Visible = false;

            this.dataGridView2.DataSource = DataSource2;
            this.dataGridView2.Columns[0].Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView2.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView2.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView2.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void ExportXLS_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("要导出 " + tabControl1.SelectedTab.Text + "吗？", "导出数据", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DataSource2 == null && DataSource2.Rows.Count == 0) {
                    MessageBox.Show("没有数据！");
                    return;
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "xls files   (*.xls)|*.xls";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                 

                string ResultFile = saveFileDialog1.FileName;
                using (MemoryStream ms = GlobalEnviroment.DataTableToExcel(DataSource2, tabControl1.SelectedTab.Text))
                {
                    using (FileStream fs = new FileStream(ResultFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
        }
    }
}
