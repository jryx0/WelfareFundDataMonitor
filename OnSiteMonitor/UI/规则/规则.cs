using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnSiteFundComparer.Models;

namespace OnSiteFundComparer.UI
{
    public partial class 规则 : Form
    {
        
        internal string testDBFile="";
        public int CurrentRuleID = -1;
        public int CurrentTmpType = 0;

        DataTable dtTmp;

        public 规则()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            initTableComb();
            //initTmpComb();

            InitTmp(-1);

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                var ds = configDB.ExecuteDataset(@"SELECT CompareAim.RowID,
                                                           CompareAim.SourceID,
                                                           CompareAim.AimName,
                                                           CompareAim.AimDesc,
                                                           CompareAim.TableName,
                                                           CompareAim.t1,
                                                           CompareAim.t2,
                                                           CompareAim.t3,
                                                           CompareAim.tmp,
                                                           CompareAim.conditions,
                                                           RulesTmp.TmpType,
                                                           CompareAim.Seq
                                                      FROM CompareAim
                                                           JOIN
                                                           RulesTmp ON compareAim.tmp = RulesTmp.RowID
                                                     WHERE CompareAim.status = 1  and CompareAim.RowID = "
                                               + CurrentRuleID.ToString());

                if (ds != null && ds.Tables[0] != null)
                {
                    this.tbAim.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.tbDesc.Text = ds.Tables[0].Rows[0][3].ToString();
                    this.tbTableName.Text = ds.Tables[0].Rows[0][4].ToString();
                    this.tbPara.Text = ds.Tables[0].Rows[0][9].ToString();

                    this.cbTB1.SelectedValue = int.Parse(ds.Tables[0].Rows[0][5].ToString());
                    this.cbTB2.SelectedValue = int.Parse(ds.Tables[0].Rows[0][6].ToString());
                    this.cbTB3.SelectedValue = int.Parse(ds.Tables[0].Rows[0][7].ToString());
                    this.cbTemplate.SelectedValue = int.Parse(ds.Tables[0].Rows[0][8].ToString());

                    //if (ds.Tables[0].Rows[0][10].ToString() == "0")
                    //    this.lbTmpType.Text = "规则类型:比对规则";
                    //else if (ds.Tables[0].Rows[0][10].ToString() == "1")
                    //    this.lbTmpType.Text = "规则类型:校验规则";
                    //else if (ds.Tables[0].Rows[0][10].ToString() == "2")
                    //    this.lbTmpType.Text = "规则类型:预处理规则";
                    //else this.lbTmpType.Text = "";

                    this.tbSeq.Text = ds.Tables[0].Rows[0][11].ToString();

                    //this.tbR1.Text = ds.Tables[0].Rows[0][2].ToString();
                    //this.tbR2.Text = ds.Tables[0].Rows[0][3].ToString();
                    //this.tbR3.Text.Text = ds.Tables[0].Rows[0][4].ToString();
                }

            }
            catch (Exception ex)
            {

            }



            base.OnLoad(e);
        }

        public void InitCombox(ComboBox cb, List<DataItem> sList)
        {
            if (sList == null)
                return;            

            cb.DataSource = sList;
            cb.DisplayMember = "DataFullName";
            cb.ValueMember = "RowID";//"dbTable";// 
            cb.SelectedIndex = 0;
        }

        private void initTableComb()
        {
            var di = new DataItem();
            di.DataFullName = "请选择";
            di.RowID = 0;

            Business.DataMgr dm = new Business.DataMgr();

            List<DataItem> l1 = dm.GetChildDataItemList(2);
            l1.Insert(0, di);
            l1.AddRange(dm.GetChildDataItemList(3));
            InitCombox(this.cbTB1, l1);

            List<DataItem> l2 = new List<DataItem>();
            l2.AddRange(l1);
            l2.AddRange(dm.GetChildDataItemList(-1));
            InitCombox(this.cbTB2, l2);

            List<DataItem> l3 = new List<DataItem>();
            l3.AddRange(l2);             
            InitCombox(this.cbTB3, l3);
        }


        //private void initTmpComb()
        //{
        //    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
        //    try
        //    {
        //        var ds = configDB.ExecuteDataset("SELECT rowid, tmpname,ruletype,tmptype FROM RulesTmp where status = 1  order by seq");
                               

        //        dtTmp = ds.Tables[0]; 

        //        this.cbTemplate.DataSource = ds.Tables[0];
        //        this.cbTemplate.DisplayMember = "tmpname";
        //        this.cbTemplate.ValueMember = "rowid";

        //        this.cbTemplate.SelectedIndex = -1;
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}


        private void btnSave_Click(object sender, EventArgs e)
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                var o = configDB.ExecuteScalar("Select rowid from CompareAim where rowid = " + CurrentRuleID.ToString());

                if (o == null) SaveNew();
                else SaveUpdate();
            }
            catch (Exception ex)
            {
            } 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; 
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();            
            //if (ofd.ShowDialog() == DialogResult.Cancel)
            //    return;

            //testDBFile = ofd.FileName;

            if (!System.IO.File.Exists(testDBFile))
            {
                MessageBox.Show("测试数据库不存在");
                return;
            }

            string testSql = tbR1.Text;

            switch(tbRule2.SelectedIndex)
            {
                case 0:
                    testSql = tbR1.Text;
                    break;
                case 1:
                    testSql = tbR2.Text;
                    break;
                case 2:
                    testSql = tbR3.Text;
                    break;
            }

            DAL.MySqlite sqliteDB = new DAL.MySqlite(testDBFile, GlobalEnviroment.isCryt);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataSet ds = sqliteDB.ExecuteDataset(testSql);
                this.Cursor = Cursors.Arrow;

                显示列表 ret = new 显示列表();
                ret.DataSource = ds.Tables[0];
                ret.TitleName = tbAim.Text;
                ret.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTmp_Click(object sender, EventArgs e)
        {
            new 规则模板().ShowDialog();
        }

        private void cbTB1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTB1.SelectedItem == null ||
                this.cbTB2.SelectedItem == null ||
                this.cbTB3.SelectedItem == null ||
                this.cbTemplate.SelectedItem == null
                )
                return;

            DataItem di1 = (DataItem)this.cbTB1.SelectedItem;
            DataItem di2 = (DataItem)this.cbTB2.SelectedItem;
            DataItem di3 = (DataItem)this.cbTB3.SelectedItem;


            string type = ((DataRowView)this.cbTemplate.SelectedItem).Row.ItemArray[2].ToString();

            if (type == null)
                return;

            if (type == "3") //源比比模型
                tbTableName.Text = di3.people + di2.people + di1.people + di1.DataShortName;
            else if (type == "2") //源比模型
                tbTableName.Text = di2.people + di1.people + di1.DataShortName;
            else if (type == "1")
                tbTableName.Text = di1.DataShortName+ di2.people;
            else if (type == "4")
                tbTableName.Text = di3.people + di2.people + di1.people + di1.DataShortName ;
            else if (type == "5")//源源模型
                tbTableName.Text = di2.people + di2.DataShortName + "又"+di1.people + di1.DataShortName;
            else if (type == "6")//源源时间-2
                tbTableName.Text = di2.people + di2.DataShortName + "又" + di1.people + di1.DataShortName;
            else if(type =="7")
                tbTableName.Text = di3.people +  di2.DataShortName  + di1.DataShortName;
            //    tbAim.Text = "";//di2.people + di1.people + di1.DataShortName;
            //tbTableName.Text = tbAim.Text;


            string tmp = this.cbTemplate.SelectedValue.ToString();

            ////get tmp
            string tmpSql = @"SELECT rowid, rules,
                                       rule2,
                                       rule3,
                                       Comments
                                  FROM RulesTmp
                                 WHERE status = 1 AND 
                                       rowid = @id ";
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                var ds = configDB.ExecuteDataset(tmpSql.Replace("@id", tmp));

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    string r1 = ds.Tables[0].Rows[0][1].ToString();
                    tbR1.Text = ReplaceAll(r1, di1, di2, di3);

                    string r2 = ds.Tables[0].Rows[0][2].ToString();
                    tbR2.Text = ReplaceAll(r2, di1, di2, di3);


                    string r3 = ds.Tables[0].Rows[0][3].ToString();
                    tbR3.Text = ReplaceAll(r3, di1, di2, di3);

                    string r4 = ds.Tables[0].Rows[0][4].ToString();
                    tbPreSql.Text = ReplaceAll(r4, di1, di2, di3);
                }
            }
            catch (Exception ex)
            {

            }
        }


        private string ReplaceAll(string rule, DataItem di1, DataItem di2, DataItem di3)
        {

            rule = rule.Replace("@table1", di1.dbTable);
            rule = rule.Replace("@tablepre1", di1.dbTablePre);
            rule = rule.Replace("@table2", di2.dbTable);
            rule = rule.Replace("@tablepre2", di2.dbTablePre);
            rule = rule.Replace("@table3", di3.dbTable);
            rule = rule.Replace("@tablepre3", di3.dbTablePre);

            rule = rule.Replace("@aimtype", tbAim.Text);
            rule = rule.Replace("@tablename", tbTableName.Text);

            rule = rule.Replace("@t1p", di1.people);
            rule = rule.Replace("@t1s", di1.DataShortName);
            rule = rule.Replace("@t1f", di1.DataFullName);

            rule = rule.Replace("@t2p", di2.people);
            rule = rule.Replace("@t2s", di2.DataShortName);
            rule = rule.Replace("@t2f", di2.DataFullName);


            rule = rule.Replace("@t3p", di3.people);
            rule = rule.Replace("@t3s", di3.DataShortName);
            rule = rule.Replace("@t3f", di3.DataFullName);

            rule = rule.Replace("@para", tbPara.Text);


            return rule;
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            SaveNew();          
        }
        private void SaveNew()
        {
            if (this.cbTB1.SelectedItem == null || this.cbTB2.SelectedItem == null ||
              this.cbTB3.SelectedItem == null || this.cbTemplate.SelectedItem == null)
                return;

            DataItem di1 = (DataItem)this.cbTB1.SelectedItem;
            DataItem di2 = (DataItem)this.cbTB2.SelectedItem;
            DataItem di3 = (DataItem)this.cbTB3.SelectedItem;

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            string insertSql = @"INSERT INTO CompareAim (SourceID,
                                   AimName, AimDesc,  TableName, t1,  t2,  t3,  tmp, seq, conditions) 
                                VALUES (@SourceID, @AimName,  @AimDesc,  @TableName,  @t1, @t2,  @t3, @tmp, @seq, @conditions)";

            try
            { 
                this.CurrentRuleID = configDB.ExecuteNonQuery(CommandType.Text, insertSql,
                    new System.Data.SQLite.SQLiteParameter[]
                                                {
                                                        new System.Data.SQLite.SQLiteParameter("@SourceID", di1.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@AimName", tbAim.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@AimDesc", tbDesc.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@TableName", tbTableName.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@t1", di1.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@t2", di2.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@t3", di3.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@seq", di1.Seq * 10000 + di2.Seq),
                                                        new System.Data.SQLite.SQLiteParameter("@tmp", cbTemplate.SelectedValue.ToString()),
                                                        new System.Data.SQLite.SQLiteParameter("@conditions", tbPara.Text)
                                                });


                var o = configDB.ExecuteScalar("select last_insert_rowid()");
                if (o != null)
                    CurrentRuleID = int.Parse(o.ToString());
                 
                MessageBox.Show("新增规则保存成功！");
                ClearRule();

            }
            catch (Exception ex)
            {
                MessageBox.Show("新增规则保存失败！" + ex.Message);
            }
        }
        private void SaveUpdate()
        {
            if (this.cbTB1.SelectedItem == null || this.cbTB2.SelectedItem == null ||
                this.cbTB3.SelectedItem == null || this.cbTemplate.SelectedItem == null)
                return;

            DataItem di1 = (DataItem)this.cbTB1.SelectedItem;
            DataItem di2 = (DataItem)this.cbTB2.SelectedItem;
            DataItem di3 = (DataItem)this.cbTB3.SelectedItem;

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);             
            string updateSql = @"UPDATE CompareAim    SET
                                    SourceID = @SourceID,    AimName = @AimName,
                                    AimDesc = @AimDesc,     TableName = @TableName,
                                    t1 = @t1,     t2 = @t2,                   t3 = @t3,
                                    tmp = @tmp, conditions = @conditions, seq = @seq
                                 WHERE Rowid = @Rowid";
            try
            {
                int seq = 0;
                int.TryParse(tbSeq.Text, out seq);

                var o = configDB.ExecuteScalar("Select rowid from CompareAim where rowid = " + CurrentRuleID.ToString());

                if (o != null)
                {
                    configDB.ExecuteNonQuery(CommandType.Text, updateSql,
                       new System.Data.SQLite.SQLiteParameter[]
                                                   {
                                                        new System.Data.SQLite.SQLiteParameter("@SourceID", di1.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@AimName", tbAim.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@AimDesc", tbDesc.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@TableName", tbTableName.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@t1", di1.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@t2", di2.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@t3", di3.RowID),
                                                        new System.Data.SQLite.SQLiteParameter("@tmp", cbTemplate.SelectedValue.ToString()),
                                                        new System.Data.SQLite.SQLiteParameter("@Rowid", this.CurrentRuleID ),
                                                        new System.Data.SQLite.SQLiteParameter("@conditions", tbPara.Text),
                                                        new System.Data.SQLite.SQLiteParameter("@seq", seq)
                                                   });
                }
                
                MessageBox.Show("修改规则保存成功！");
                ClearRule();
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改规则保存失败！" + ex.Message);
                //this.DialogResult = DialogResult.Cancel;
            }
        }
        private void ClearRule()
        {
            //this.cbTB1.SelectedIndex = 0;
            //this.cbTB2.SelectedIndex = 0;
            //this.cbTB3.SelectedIndex = 0;
            this.cbTemplate.SelectedIndex = -1;

            //this.tbTableName.Text = "";
            //this.tbAim.Text = "";
            //this.tbDesc.Text = "";
            this.tbR1.Text = "";
            this.tbR2.Text = "";
            this.tbR3.Text = "";

           // this.lbTmpType.Text = "";


        }
        private void tbAim_TextChanged(object sender, EventArgs e)
        {

            //tbR1.Text = tbR1.Text.Replace("@aimtype", tbAim.Text);
            //tbR2.Text = tbR2.Text.Replace("@aimtype", tbAim.Text);
            //tbR3.Text = tbR3.Text.Replace("@aimtype", tbAim.Text);

            //cbTB1_SelectedIndexChanged(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var p = this.PointToScreen(new Point(button2.Location.X, button2.Location.Y + button2.Height));

            contextMenuStrip1.Show(p);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            button2.Text = tsm.Text;

          
            InitTmp(tsm.MergeIndex);
        }

        private void InitTmp(int index)
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                String sql = @"SELECT rowid, tmpname,ruletype,tmptype FROM RulesTmp where status = 1  @para order by seq";
                if (index > -1 && index < 3)
                    sql = sql.Replace("@para", "and tmptype = " + index);
                else sql = sql.Replace("@para", "");

                var ds = configDB.ExecuteDataset(sql);


                dtTmp = ds.Tables[0];


                
                object rowid = this.cbTemplate.SelectedValue;

                this.cbTemplate.DataSource = ds.Tables[0];
                this.cbTemplate.DisplayMember = "tmpname";
                this.cbTemplate.ValueMember = "rowid";

                if (rowid == null)
                    this.cbTemplate.SelectedIndex = -1;
                else
                this.cbTemplate.SelectedValue = rowid;
            }
            catch (Exception ex)
            {

            }
        }
        
    }
}
