using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.QuickCompare.UI
{
    public partial class 规则管理 : Form
    {
        protected List<Models.CompareRule> compareRule;
        public 规则管理()
        {
            InitializeComponent();            

            if (GlobalEnviroment.LoginedUser.Name!= "admin")
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button5.Visible = false;
                btnTmp.Visible = false;
            }
        }

        private void Init(int index = -1)
        {
            Services.CompareRuleProxy crp = new Services.CompareRuleProxy();

            try
            {
                compareRule = crp.GetAll();
                if (compareRule == null) return;

                var displayList = from cr in compareRule
                                  where index < (int)Models.CompareRulesTypes.Compare ||  index > (int)Models.CompareRulesTypes.Preprocess ?
                                           true : cr.RuleType == (Models.CompareRulesTypes)index
                                  orderby cr.ParentItem.ParentItem.Seq, cr.ParentItem.Seq, cr.Seq
                                  select new
                                  {
                                      RowID = cr.RowID,
                                      规则名称 = cr.RuleName,
                                      规则描述 = cr.RuleDesc,
                                      规则类型 = cr.RuleType == Models.CompareRulesTypes.Compare ? "比对规则" : (cr.RuleType == Models.CompareRulesTypes.Preprocess ? "预处理规则" : "校验规则"),
                                  };

                this.dataGridView1.DataSource = displayList.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        protected override void OnLoad(EventArgs e)
        {
            //Init();
            cbRuleType.SelectedIndex = 0;

            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {//new
            ShowDetail(-1);
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {//modify
            if (e.RowIndex < 0)
                return;
 
            ShowDetail((int)this.dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            
        }
        private void button2_Click(object sender, EventArgs e)
        {//modify
            if (this.dataGridView1.SelectedRows.Count == 0)
                return;
            
            ShowDetail((int)this.dataGridView1.SelectedRows[0].Cells[0].Value);
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
                    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
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

        private void ShowDetail(int rowid)
        {
            if (GlobalEnviroment.LoginedUser.Name != "admin")
                return;
            

            规则设计 dlg = new 规则设计(compareRule.Find(x => x.RowID == rowid));
             
            dlg.ShowDialog();

            ReSetGridView();
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
                    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Init(cbRuleType.SelectedIndex);
        }

        private void btnTmp_Click(object sender, EventArgs e)
        {
            //new 规则模板().ShowDialog();
        }
    }
}

