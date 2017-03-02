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
    public partial class 结果数据库管理 : Form
    {
        public 结果数据库管理()
        {
            InitializeComponent();
              
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Cursor = Cursors.WaitCursor;
            init();
            this.Cursor = Cursors.Arrow;
        }


        void init()
        {
            DAL.MySqlite config = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                SyncReportData(config);
                string sql = @"SELECT task.RowID,
                                       task.status,
                                       task.Region as 地区,
                                       task.TaskName AS 比对名称,
                                       CASE task.TStatus When 0 Then '否'
                                                         When 1 Then '是' END as 是否上报,
                                       CASE task.Status + task.TStatus WHEN 0 THEN '未上传' 
                                                        WHEN 1 THEN '正在上传...' 
                                                        WHEN 5 THEN '数据库错误请重传' 
                                                        WHEN 10 THEN '已上传,非上报数据' 
                                                        WHEN 11 THEN '等待后台处理...'
                                                        WHEN 16 THEN '正在解压数据'
                                                        WHEN 17 THEN '解压数据出错'
                                                        WHEN 21 THEN '上报成功' END AS 上报状态,
                                       datetime(task.CreateDate) AS 时间,
                                       task.TaskComment AS 比对说明
                                  FROM Task Where status >=10 and Username = '@Username'
                                 ";
               
                sql = sql.Replace("@Username", GlobalEnviroment.LoginedUser.Name);
                var ds = config.ExecuteDataset(sql);
                if(ds != null && ds.Tables[0] != null)
                { 
                    this.dataGridView1.DataSource = ds.Tables[0];
                    this.dataGridView1.Columns[0].Visible = false;
                    this.dataGridView1.Columns[1].Visible = false;                    
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[6].Width = 400;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("访问数据出错!" + ex.Message, "错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(GlobalEnviroment.LocalVersion)
            {
                MessageBox.Show("本地版本，无法设置上报数据库！");
                return;
            }

            if (this.dataGridView1.SelectedRows == null && this.dataGridView1.SelectedRows.Count == 0)
                return;
            this.Cursor = Cursors.AppStarting;
            DAL.MySqlite config = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                var t = config.ExecuteScalar("Select TStatus  from task where Rowid = " +
                  this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                if (t == null) return;

                if (t.ToString() == "1")
                    if (MessageBox.Show("要取消上报数据吗？", "取消上报", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                var o = config.ExecuteScalar("Select DBInfo  from task where Rowid = " +
                    this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                if (o == null) return;


                foreach (DataGridViewRow dr in this.dataGridView1.Rows)
                {
                    string s = dr.Cells[5].Value.ToString();
                    if (s.IndexOf("等待后台处理...") != -1 || s.IndexOf("正在解压数据") != -1)
                    {
                        MessageBox.Show("数据处理中,请稍后操作！");
                        this.Cursor = Cursors.Arrow;
                        return;

                    }
                }


                var str = GlobalEnviroment.theWebService.SetDefaultData("result." + o.ToString() + ".db.bak.gz");
                if (str.Length == 0)
                { 
                    SyncReportData(config);
                    init();
                }
                else MessageBox.Show(str);

            }
            catch (Exception ex)
            {

                MessageBox.Show("数据库错误！");

            }

            this.Cursor = Cursors.Arrow;
        }

        public static bool SyncReportData(DAL.MySqlite config)
        {
            try
            {
                var ds = config.ExecuteDataset("Select RowID, DBInfo from task where username = '" + GlobalEnviroment.LoginedUser.Name + "'");

                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    String dbInfo = "result." + r[1].ToString() + ".db.bak.gz";
                    var value = GlobalEnviroment.theWebService.GetDataStatus(dbInfo);

                    System.Threading.Thread.Sleep(100);
                    if (value.Length == 0)
                        continue;

                    var s = value.Split(';');
                    if (s.Length == 2)
                        config.ExecuteNonQuery(@"update task set status = " + s[0] + ", TStatus = " + s[1] + " Where rowid = " + r[0].ToString());                     
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= this.dataGridView1.Rows.Count || e.RowIndex < 0)
            //    return;

            //var value = this.dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

            //SetBtnStatus(value);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var dataStatus = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            SetBtnStatus(dataStatus);
        }

        private void SetBtnStatus(string dataStatus)
        {
            switch (dataStatus)
            {
                case "正在上传...":
                    btnSetDefault.Enabled = false;
                    break;
                case "数据库错误请重传":
                    btnSetDefault.Visible = false;                     
                    break;
                case "已上传,非上报数据":
                    btnSetDefault.Enabled = true;
                    btnSetDefault.Text = "设为正式上报数据";
                    break;
                case "等待后台处理...":
                case "正在解压数据":
                    btnSetDefault.Enabled = false;
                    btnSetDefault.Text = "设为正式上报数据";
                    break;                
                case "解压数据出错":
                case "上报成功":
                    btnSetDefault.Enabled = true;
                    btnSetDefault.Text = "取消上报";
                    break;
            }
        }
    }
}
