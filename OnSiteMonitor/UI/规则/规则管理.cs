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
    public partial class 规则管理 : Form
    {
        public string configDB;
        public string ImportDB;
        public string resultDB;

        public 规则管理()
        {
            InitializeComponent();
            Init();

            if (GlobalEnviroment.LoginedUser.Name!= "admin")
            {

                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button5.Visible = false;
            }

        }

        private void Init(int index = -1)
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);

            try
            {

                var ds = configDB.ExecuteDataset(@"
                                                SELECT CompareAim.RowID,   
                                                    DataItem.DataShortName as 项目类型,   
                                                    CompareAim.AimName as 规则名称,  CompareAim.AimDesc as 规则说明,
                                                    CompareAim.TableName as 表名,
                                                    CompareAim.t1,
                                                    CompareAim.t2,
                                                    CompareAim.t3,
                                                    CompareAim.tmp,
                                                    RulesTmp.TmpName AS 模板
                                                FROM CompareAim,
                                                    DataItem,
                                                    RulesTmp
                                                WHERE CompareAim.status = 1 AND 
                                                    CompareAim.SourceID = DataItem.RowID AND 
                                                    RulesTmp.RowID = CompareAim.tmp
                                                ORDER BY DataItem.parentid,
                                                    DataItem.seq,
                                                    CompareAim.Seq");

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


                    this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    this.dataGridView1.Columns[4].Visible = true;
                    this.dataGridView1.Columns[5].Visible = false;
                    this.dataGridView1.Columns[6].Visible = false;
                    this.dataGridView1.Columns[7].Visible = false;
                    this.dataGridView1.Columns[8].Visible = false;
                    this.dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch(Exception ex)
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {//new
            ShowDetail("-1");
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {//modify
            if (e.RowIndex < 0)
                return;

            string rowid = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            ShowDetail(rowid);
            
        }
        private void button2_Click(object sender, EventArgs e)
        {//modify
            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            string rowid = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            ShowDetail(rowid);
        }

        private void button3_Click(object sender, EventArgs e)
        {//delete           

            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            var _selectedRows = this.dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认删除 " + _selectedRows  + " 条规则吗？",
                "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
 
            string rowids = "";
            foreach (DataGridViewRow _selectedRow in this.dataGridView1.SelectedRows)
            {
                rowids = rowids + _selectedRow.Cells[0].Value.ToString() + ",";
            }

            if (rowids.Length != 0)
            {
                rowids = rowids.Substring(0, rowids.Length - 1);
                try
                {
                    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
                    configDB.ExecuteNonQuery("delete from CompareAim where rowid in (" + rowids + ")");

                    ReSetGridView();

                    MessageBox.Show("成功删除 " + _selectedRows + " 条规则");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除错误：" + ex.Message);
                }
            }


            //string rowid = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //try
            //{
            //    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            //    configDB.ExecuteNonQuery("delete from CompareAim where rowid = " + rowid);


            //    ReSetGridView();
            //    //int index = dataGridView1.FirstDisplayedScrollingRowIndex;
            //    //int cIndex = -1;
            //    //if (this.dataGridView1.CurrentCell != null)
            //    //    cIndex = this.dataGridView1.CurrentCell.RowIndex;

            //    //Init();

            //    //if (cIndex > 0 && cIndex < this.dataGridView1.Rows.Count)
            //    //    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[cIndex].Cells[1];
            //    //if (index > -1)
            //    //    dataGridView1.FirstDisplayedScrollingRowIndex = index;
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void ShowDetail(string rowid)
        {
            if (GlobalEnviroment.LoginedUser.Name != "admin")
                return;

            UI.规则 dlg = new 规则();
            int.TryParse(rowid, out dlg.CurrentRuleID);
            dlg.ShowDialog();

            ReSetGridView();
            //int index = dataGridView1.FirstDisplayedScrollingRowIndex;
            //int cIndex = -1;
            //if (this.dataGridView1.CurrentCell != null)
            //    cIndex = this.dataGridView1.CurrentCell.RowIndex;

            //Init();

            //if (cIndex > 0 && cIndex < this.dataGridView1.Rows.Count)
            //    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[cIndex].Cells[1];
            //if (index > -1)
            //    dataGridView1.FirstDisplayedScrollingRowIndex = index;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                                                                                    e.RowBounds.Location.Y,
                                                                                    dataGridView1.RowHeadersWidth - 4,
                                                                                    e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Test.生成测试库 tf = new Test.生成测试库();

            //tf.ShowDialog();

            new Test.测试().ShowDialog();


            //Test.TestForm1 tf = new Test.TestForm1();

            //if(tf.ShowDialog() == DialogResult.OK)
            //{
            //    this.resultDB = tf.resultDB;
            //    this.configDB = tf.configDB;
            //    this.ImportDB = tf.ImportDB;
            //}
        }

        private void ReSetGridView()
        {
            int index = dataGridView1.FirstDisplayedScrollingRowIndex;
            int cIndex = -1;
            if (this.dataGridView1.CurrentCell != null)
                cIndex = this.dataGridView1.CurrentCell.RowIndex;

            Init();

            if (cIndex > 0 && cIndex < this.dataGridView1.Rows.Count)
                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[cIndex].Cells[1];
            if (index > -1)
                dataGridView1.FirstDisplayedScrollingRowIndex = index;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            var _selectedRows = this.dataGridView1.SelectedRows.Count;
            if (MessageBox.Show("确认停用 " + _selectedRows + " 条规则吗？",
                "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            string rowids = "";
            foreach (DataGridViewRow _selectedRow in this.dataGridView1.SelectedRows)
            {
                rowids = rowids + _selectedRow.Cells[0].Value.ToString() + ",";
            }

            if (rowids.Length != 0)
            {
                rowids = rowids.Substring(0, rowids.Length - 1);
                try
                {
                    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
                    configDB.ExecuteNonQuery("update CompareAim set status = 0  where rowid in (" + rowids + ")");

                    ReSetGridView();

                    MessageBox.Show("成功停用 " + _selectedRows + " 条规则");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("停用出错：" + ex.Message);
                }
            }
        }
    }
}

