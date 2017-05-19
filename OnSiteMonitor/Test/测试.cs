using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OnSiteFundComparer.Test
{
    public partial class 测试 : Form
    {
        List<int> ModifyRows = new List<int>();
        String TestDBFile = GlobalEnviroment.InputDBDir;

        List<Models.CompareAim> aimList = new List<Models.CompareAim>();
        List<Models.CompareAim> TestaimList = new List<Models.CompareAim>();
        List<Models.DataItem> diList = new List<Models.DataItem>();

        bool ShowAllItem = true;
        public 测试()
        {
            InitializeComponent();             
        }

        protected override void OnLoad(EventArgs e)
        {
            Init();


            
            base.OnLoad(e);
        }

        private void Init()
        {
            // 第一步:初始化
            this.backgroundWorker1.WorkerReportsProgress = true; // 显示进度
            this.backgroundWorker1.WorkerSupportsCancellation = true; // 支持取消
             


            InitGridView();            
             

            using (Service.CompareAimService cas = new Service.CompareAimService(GlobalEnviroment.MainDBFile))
            {
                aimList = cas.GetCompareAim();
            }

            using (Service.DataItemStuctServices dis = new Service.DataItemStuctServices(GlobalEnviroment.MainDBFile))
            {
                diList = dis.GetDataItems();
            }

            SetTestPassAll();
        }
   

        private void InitGridView()
        {
            DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                var ds = configDB.ExecuteDataset(@"
                                                SELECT CompareAim.RowID,
                                                       DataItem.DataShortName AS 项目类型,
                                                       CompareAim.AimName AS 规则名称,
                                                       CompareAimTest.TheoryResult 理论数量,
                                                       CompareAimTest.TestResult 测试数量,
                                                       CompareAimTest.TestTime 测试时间,
                                                       RulesTmp.Rowid 模板 
                                                  FROM CompareAim left join CompareAimTest on CompareAim.RowID = CompareAimTest.AimID,
                                                       DataItem,
                                                       RulesTmp
                                                 WHERE CompareAim.status = 1 AND 
                                                       CompareAim.SourceID = DataItem.RowID AND 
                                                       RulesTmp.RowID = CompareAim.tmp and CompareAim.Type = 0
                                                 ORDER BY DataItem.parentid,
                                                          DataItem.seq,
                                                          CompareAim.Seq");

                if (ds == null)
                    return;



                int index = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = false;
                    this.dataGridView1.Rows[index].Cells[1].Value = dr[0].ToString(); //Rowid
                    this.dataGridView1.Rows[index].Cells[2].Value = dr[1].ToString(); //DataItem
                    this.dataGridView1.Rows[index].Cells[3].Value = dr[2].ToString();  //RuleName


                    this.dataGridView1.Rows[index].Cells[4].Value = dr[3].ToString();  //DesignResult
                    this.dataGridView1.Rows[index].Cells[5].Value = dr[4].ToString();  // TestResult

                    //SetTestPass(index);                   

                    this.dataGridView1.Rows[index].Cells[7].Value = dr[5].ToString();
                    this.dataGridView1.Rows[index].Cells[8].Value = dr[6].ToString();
                }

                
            }
            catch
            {

            }
        }

  

        //private void InitGirdView(int index = -1)
        //{
        //    DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);

        //    try
        //    {

        //        var ds = configDB.ExecuteDataset(@"
        //                                        SELECT CompareAim.RowID,
        //                                               DataItem.DataShortName AS 项目类型,
        //                                               CompareAim.AimName AS 规则名称,
        //                                               CompareAimTest.TheoryResult 理论数量,
        //                                               '' 测试数量,
        //                                               '' 测试时间
        //                                          FROM CompareAim left join CompareAimTest on CompareAim.RowID = CompareAimTest.AimID,
        //                                               DataItem,
        //                                               RulesTmp
        //                                         WHERE CompareAim.status = 1 AND 
        //                                               CompareAim.SourceID = DataItem.RowID AND 
        //                                               RulesTmp.RowID = CompareAim.tmp
        //                                         ORDER BY DataItem.parentid,
        //                                                  DataItem.seq,
        //                                                  CompareAim.Seq");

        //        this.dataGridView1.DataSource = ds.Tables[0];

        //        if (index > 0 && index < this.dataGridView1.Rows.Count)
        //        {

        //            this.dataGridView1.ClearSelection();
        //            this.dataGridView1.Rows[index].Selected = true;
        //            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];
        //        }

        //        if (this.dataGridView1.Columns.Count > 2)
        //        {
        //            this.dataGridView1.Columns[0].Visible = false;


        //            this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //            this.dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
        //            this.dataGridView1.Columns[2].Visible = true;
        //            this.dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
        //            this.dataGridView1.Columns[2].Width = 250;

        //            this.dataGridView1.Columns[3].Visible = true;
        //            this.dataGridView1.Columns[3].ReadOnly = false;

        //            this.dataGridView1.Columns[4].Visible = true;
        //            this.dataGridView1.Columns[5].Visible = true;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}




        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)r.Cells[0];
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (flag == true)     //查找被选择的数据行 
                {
                    int rowid = Convert.ToInt32(r.Cells[1].Value.ToString());
                    var aim = aimList.Find(x => x.RowID == rowid);

                    TestaimList.Add(aim);
                }                 
            }

            TestComparing tc = new TestComparing();

            tc.compareAims = TestaimList;
            tc.log += backgroundWorker1.ReportProgress;
            tc.CompareDirectly = false;
             
            tc.Start();

            foreach (var r in tc.testResults)
            {
                var aim = (from DataGridViewRow row in dataGridView1.Rows
                          where row.Cells[1].Value.ToString() == r.RowID
                          select row);
                if(aim != null && aim.Count() != 0)
                {
                    var a = aim.First();

                    a.Cells[5].Value = r.ResultNumber;
                    a.Cells[7].Value = r.Time;

                    int id = Convert.ToInt32(r.RowID);
                    ModifyRows.Add(a.Index);
                }
            }

            SetTestPassAll();
             
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Length != 0)
                tbDPath.AppendText(e.UserState.ToString() + "\r\n");
            if (e.ProgressPercentage != 0)
                this.progressBar1.Value = e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();


            btnStop.Enabled = false;
            btnStart.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (this.backgroundWorker1.CancellationPending)
                return;

            if (!IsDBCorrect())
                return;

            btnStop.Enabled = true;
            btnStart.Enabled = false;

            tbDPath.Text = "";

            TestaimList.Clear();
            this.progressBar1.Value = 0;
            this.dataGridView1.EndEdit();

            this.backgroundWorker1.RunWorkerAsync();
        }

        private bool IsDBCorrect()
        {
            bool bRet = false;
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                var ds = configDB.ExecuteDataset(@"select tbl_name, sql from sqlite_master
                                    where type = 'table' and tbl_name not like '%sqlite%'");

                bRet = true;
            }
            catch (Exception ex)
            {

            }


            return bRet;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            this.progressBar1.Value = 100;

            SetTestPassAll();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Test.生成测试库 tf = new Test.生成测试库();

            tf.ShowDialog();

            tbDPath.Text = tf.InputDBDir;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {                
                SetTestPass(e.RowIndex);
                ModifyRows.Add(e.RowIndex);
            }
        }

        private void SetTestPass(int rowIndex)
        {
            if (dataGridView1.Rows[rowIndex].Cells[4] == null || dataGridView1.Rows[rowIndex].Cells[5] == null)
                return;

            SetTestPass(rowIndex, dataGridView1.Rows[rowIndex].Cells[4].Value.ToString()
                    , dataGridView1.Rows[rowIndex].Cells[5].Value.ToString());
        }
        private void SetTestPass(int index, string dResult, string tResult)
        {
            if (dResult.Length != 0)
            {
                this.dataGridView1.Rows[index].Cells[6].Value = "未通过";
                if (tResult.Length != 0)
                {
                    int DesignResult = 0;
                    int TestResult = 0;
                    int.TryParse(dResult, out DesignResult);
                    int.TryParse(tResult, out TestResult);

                    if (DesignResult == TestResult)
                        this.dataGridView1.Rows[index].Cells[6].Value = "通过";
                }
            }

        }

        private void SetTestPassAll()
        {
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                SetTestPass(r.Index);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            String Sql = @"Insert or replace into CompareAimTest(aimid, TheoryResult, TestResult, TestTime)
                    values(@aimid, '@TheoryResult', '@TestResult', '@TestTime')";

            configDB.BeginTran();
            try
            {
                foreach (var rowid in ModifyRows)
                {
                    var aimid = dataGridView1.Rows[rowid].Cells[1].Value.ToString();
                    var TheoryResult = dataGridView1.Rows[rowid].Cells[4].Value.ToString();
                    var TestResult = dataGridView1.Rows[rowid].Cells[5].Value.ToString();
                    var TestTime = dataGridView1.Rows[rowid].Cells[7].Value.ToString();


                    var _sql = Sql.Replace("@aimid", aimid);
                    _sql = _sql.Replace("@TheoryResult", TheoryResult);
                    _sql = _sql.Replace("@TestResult", TestResult);
                    _sql = _sql.Replace("@TestTime", TestTime);

                    configDB.ExecuteNonQuery(_sql);
                }
                configDB.Commit();
            }
            catch (Exception ex)
            {
                configDB.RollBack();
            }


            base.OnClosing(e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                if (r.Cells[6].Value != null)
                {
                    if (r.Cells[6].Value.ToString() == "通过")
                        if (ShowAllItem)
                            r.Visible = !ShowAllItem;
                        else r.Visible = !ShowAllItem;


                }
            }

            if (ShowAllItem)
                button2.Text = "显示所有";
            else
                button2.Text = "隐藏通过项目";

            ShowAllItem = !ShowAllItem;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                r.Cells[4].Value = null;
                r.Cells[5].Value = null;
                r.Cells[6].Value = null;
                r.Cells[7].Value = null;
            }


            DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            configDB.ExecuteNonQuery("delete from CompareAimTest");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r1 in this.dataGridView1.Rows)
                foreach (DataGridViewRow r2 in this.dataGridView1.Rows)
                {
                    if (r1.Cells[8].Value.ToString() == r2.Cells[8].Value.ToString())
                        if (r1.Cells[6].Value != null && r1.Cells[6].Value.ToString() == "通过")
                        {
                            r2.Visible = false;
                        }
                }
        }
    }
}
