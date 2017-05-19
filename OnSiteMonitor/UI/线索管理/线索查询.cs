using OnSiteFundComparer.UI.线索管理;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 线索查询 : Form
    {
        private string dbFile = "";
        private int PageIndex = 1;
        //private int PageSize = 10;
        //private int MaxPageIndex = 0;
        //private int MinPageIndex = 0;
        
        public 线索查询()
        {
            InitializeComponent();
            lock(this)
            { 
            init();
            }
        }

        void init()
        {
            Business.DataMgr dm = new Business.DataMgr();
            var sList = dm.GetChildDataItemList(2);

            var di = new Models.DataItem();
            di.DataShortName = "请选择";
            di.RowID = 0;
            sList.Insert(0, di);

            cbItem.DataSource = sList;
            cbItem.DisplayMember = "DataShortName";
            cbItem.ValueMember = "RowID";//"dbTable";//             
            cbItem.SelectedIndex = 0;

            cbDataStatus.SelectedIndex = 1;
            cbCheckStatus.SelectedIndex = 0;

            GetDefaultData();

            lbInfo.Text = "";
        }      
        public bool GetDefaultData()
        {
            DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                结果数据库管理.SyncReportData(configDB);
                var ds = configDB.ExecuteDataset(@"SELECT rowid,
                                                               CASE TStatus 
                                                               WHEN 0 THEN taskname || '-' || createdate 
                                                               WHEN 1 THEN '上报库-' || taskname || '-' || createdate END AS name,
                                                               dbInfo
                                                          FROM task
                                                          Where TStatus = 1 and Status = 20 and UserName = '"+GlobalEnviroment.LoginedUser.Name+"' ORDER BY TStatus DESC");

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("请设置正式上报数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; 
                }

                dbFile = GlobalEnviroment.InputDBDir + "result." + ds.Tables[0].Rows[0][2].ToString() + ".db";
                if (!File.Exists(dbFile))
                {
                    dbFile = "";
                    MessageBox.Show("未找到上报数据：" + dbFile + "，可能被删除！请重新比对！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                cbReport.DataSource = ds.Tables[0];
                cbReport.DisplayMember = "name";
                cbReport.ValueMember = "rowid";                

                cbReport.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("上报数据异常");
                return false;
            }

            return true;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
           
            dataGridView1.DataSource = null;
            

            PageIndex = 1;
            GoPage(PageIndex);
            //if (dbFile.Length == 0)
            //    return;

            //String Sql = @"SELECT Clue_Report.rowid,
            //                       case when isuploaded = 1 then '已上传' else '未上传' end as 上传状态,
            //                       ID AS 身份证号,
            //                       Name AS 姓名,
            //                       Region AS 乡镇,
            //                       config.DataItem.DataShortName AS 项目名称,
            //                       Type AS 线索类型,                                    
            //                       CASE WHEN isconfirmed = 0 THEN '未核查' ELSE '已核查' END AS 线索状态,
            //                       CASE WHEN isconfirmed = 0 THEN '' ELSE CASE WHEN isclueTrue = 1 THEN '查实' ELSE '查否' END END AS 核查状态,
            //                       CASE WHEN isconfirmed = 0 THEN '' ELSE CASE WHEN IsCP = 1 THEN '党员' ELSE '群众' END END AS 政治面貌
            //                  FROM Clue_Report
            //                       JOIN Config.DataItem ON Clue_Report.table1 = Config.DataItem.Rowid
            //                 WHERE 1 = 1";

            //String CountSql = @"Select count() from Clue_Report where 1 = 1";

            //DAL.MySqlite result = new DAL.MySqlite(dbFile);
            //DAL.MySqlite config = new DAL.MySqlite(GlobalEnviroment.MainDBFile);

            //try
            //{
            //    string clasue = "";

            //    result.AttchDatabase(config, "Config");

            //    if (cbCheckStatus.SelectedIndex == 0)
            //        switch (cbDataStatus.SelectedIndex)
            //        {
            //            case 1:
            //                clasue += " and IsConfirmed = 0 ";
            //                break;
            //            case 2:
            //                clasue += " and IsConfirmed = 1  ";
            //                break;
            //        }
            //    else if (cbCheckStatus.SelectedIndex == 1)
            //        clasue += " and isclueTrue = 1";
            //    else clasue += " and isclueTrue = 0";

            //    int Itemtype = int.Parse(cbItem.SelectedValue.ToString());
            //    if (Itemtype != 0)
            //        clasue += " and Table1 = @Itemtype";                

            //    switch (cbUploaded.SelectedIndex)
            //    {
            //        case 1:
            //            clasue += " and isuploaded = 0 ";
            //            break;
            //        case 2:
            //            clasue += " and isuploaded = 1  ";
            //            break;
            //        case 3:
            //            clasue += " and (isuploaded <> 1 or isuploaded <> 0) ";
            //            break;
            //    }               

            //    if (tbID.Text.Length != 0)
            //        clasue += " and ID like @ID ";

            //    Sql += clasue + " ORDER BY Region,config.DataItem.DataShortName,ID LIMIT 500";
            //    CountSql += clasue;

            //    String condition =   tbID.Text + "%";
            //    var count = result.ExecuteScalar(CommandType.Text, CountSql, new System.Data.SQLite.SQLiteParameter[]
            //                                            {
            //                                                new System.Data.SQLite.SQLiteParameter("@Itemtype", Itemtype),
            //                                                new System.Data.SQLite.SQLiteParameter("@ID", condition)
            //                                            });

            //    var ds = result.ExecuteDataset(CommandType.Text, Sql, new System.Data.SQLite.SQLiteParameter[]
            //                                            {
            //                                                new System.Data.SQLite.SQLiteParameter("@Itemtype", Itemtype),                                                         
            //                                                new System.Data.SQLite.SQLiteParameter("@ID", condition)
            //                                            });
            //    if(ds != null)
            //    { 
            //        dataGridView1.DataSource = ds.Tables[0];
            //        dataGridView1.Columns[0].Visible = false;

            //        lbInfo.Text = "总记录数：" + count.ToString() + "， 显示数：" + ds.Tables[0].Rows.Count.ToString() + "（最多显示500条）";
            //    }
            //}
            //catch (Exception ex )
            //{

            //}
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
              
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dataGridView1.Rows[e.RowIndex].Cells[0];

            int rowid = 0;
            int.TryParse(cell.Value.ToString(), out rowid);

            if (rowid == 0)
                return;

            线索管理.线索核查结果录入 dlg = new 线索管理.线索核查结果录入(dbFile);

            dlg.RowID = rowid;
            dlg.ID = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                btnQuery_Click(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

           // updateFormat(); //fomat error , 户籍反了

            OpenFileDialog of = new OpenFileDialog();

            of.Filter = "Excel文件(*.xls)|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
                Cursor _preCursor = this.Cursor;
                                
                ImportExcelCluesPO xlsCluePO = new ImportExcelCluesPO(dbFile, of.FileName);
                xlsCluePO.StartNumber = 3;

                xlsCluePO.resultDB = new DAL.MySqlite(dbFile, GlobalEnviroment.isCryt);
                xlsCluePO.resultDB.AttchDatabase(Properties.Settings.Default.MainDBFile, "Config");

                xlsCluePO.MainDB = new DAL.MySqlite(Properties.Settings.Default.MainDBFile, GlobalEnviroment.isCryt);
                xlsCluePO.MainDB.AttchDatabase(xlsCluePO.resultDB, "result");

                


                Erik.Utilities.Lib.StartProgressiveOperation(xlsCluePO, this);
                if (xlsCluePO.resultDT == null )
                {
                    MessageBox.Show("未发现核查数据");
                    return;
                }

                UI.显示列表 dlg = new 显示列表();
                dlg.DataSource = xlsCluePO.resultDT;
                dlg.DataSource2 = xlsCluePO.UnSuccessDT;
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    var list = from dr in xlsCluePO.resultDT.AsEnumerable()
                               select new WFM.JW.HB.Models.Clues
                               {
                                   RowID = (Int32)dr.Field<Int64>("RowID"),
                                   Region = dr.Field<String>("乡镇街道"),
                                   Table1 = (Int32)dr.Field<Int64>("Table1"),
                                   ID = dr.Field<String>("身份证号"),
                                   Name = dr.Field<String>("姓名"),
                                   Addr = dr.Field<String>("详细地址"),

                                   Type = dr.Field<String>("线索类型"),
                                   IsClueTrue = dr.Field<String>("是否属实") == "不属实" ? 0 : 1,
                                   IsCP = dr.Field<String>("是否党员") == "否" ? 0 : 1,
                                   Fact = dr.Field<String>("核查说明"),
                                 //  IllegalMoney = (float)dr.Field<Double>("违规资金"),
                                   CheckDate = GlobalEnviroment.tryParingDateTime(dr.Field<String>("核查日期")),
                                   CheckByName1 = dr.Field<String>("核查人1")
                               };

                    UpdateLocalClues(dbFile, list.ToList(), this);

                    xlsCluePO.Clear();
                    this.Cursor = Cursors.Arrow;
                }

                this.Cursor = _preCursor;
            }
        }

        private void updateFormat()
        {
            
            if (Properties.Settings.Default.IsFirst == "1")
                return;
            Properties.Settings.Default.IsFirst = "1";
            Properties.Settings.Default.Save();

            DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            String updateSql1 = @"Update dataFormat set col = 3 where colcode='RelateID'";
            String updateSql2 = @"Update dataFormat set col = 4 where colcode='RelateName'";

            configDB.ExecuteNonQuery(updateSql1);
            configDB.ExecuteNonQuery(updateSql2);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if(GlobalEnviroment.LocalVersion)
            {
                MessageBox.Show("本地版本，无法上传！");
                return;
            }


            DAL.MySqlite result = new DAL.MySqlite(dbFile, GlobalEnviroment.isCryt);
            String SqlSelect = @"SELECT RowID, ID,   Addr, Region,  
                               IsClueTrue,   IsCP, Fact, IllegalMoney,CheckDate,
                               CheckByName1, CheckByName2, ReCheckFact, ReCheckType, ReCheckByName1,isconfirmed,isuploaded,table1
                          FROM Clue_Report 
                          where isconfirmed = 1 and isuploaded <> 1 limit 1000";

       //     String SqlUpdate = @"Update Clue_Report set isuploaded = @isuploaded where rowid = @rowid";

            this.Cursor = Cursors.AppStarting;
            try
            {
                //get clue to list              
                var ds = result.ExecuteDataset(SqlSelect);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                List<WFM.JW.HB.Models.Clues> cList = new List<WFM.JW.HB.Models.Clues>();

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    WFM.JW.HB.Models.Clues c = new WFM.JW.HB.Models.Clues();

                    int tmpi = 0;
                    int.TryParse(dr[0].ToString(), out tmpi);
                    c.RowID = tmpi;

                    c.ID = dr[1].ToString();
                    c.Addr = dr[2].ToString(); 
                    c.Region = dr[3].ToString();

                    tmpi = 0;
                    int.TryParse(dr[4].ToString(), out tmpi);
                    c.IsClueTrue = tmpi;

                    tmpi = 0;
                    int.TryParse(dr[5].ToString(), out tmpi);
                    c.IsCP = tmpi;
                    

                    c.Fact = dr[6].ToString(); 

                    float tmpf = 0.0f;
                    float.TryParse(dr[7].ToString(), out tmpf);
                    c.IllegalMoney = tmpf;


                    if (dr[8].ToString() == "")
                        c.CheckDate = System.DateTime.Now;
                    else
                    {
                        DateTime dt;
                        System.DateTime.TryParse(dr[8].ToString(), out dt);
                        c.CheckDate = dt;
                    }

                    c.CheckByName1 = dr[9].ToString() ;
                    c.CheckByName2 = dr[10].ToString() ;

                    c.ReCheckFact = dr[11].ToString();

                    tmpi = 0;
                    int.TryParse(dr[12].ToString(), out tmpi);
                    c.ReCheckType = tmpi;

                    c.ReCheckByName1 = dr[13].ToString();

                    int t = 0;
                    int.TryParse(dr[14].ToString(), out t);
                    c.Table1 = t;

                    cList.Add(c);
                }
                if(cList.Count == 0)
                {
                    MessageBox.Show("未发现数据！");
                    return;
                }

                //线索管理.UploadCluePO uploader = new 线索管理.UploadCluePO(cList);
                //Erik.Utilities.Lib.StartProgressiveOperation(uploader, this);

                //int success = 0;
                //foreach(var c in uploader.returnList)
                //{//update local db
                //    if (c.IsConfirmed == 1) success++;
                //    string sql = SqlUpdate.Replace("@rowid", c.RowID.ToString());
                //    sql = sql.Replace("@isuploaded", c.IsConfirmed.ToString());
                //    result.ExecuteNonQuery(sql);
                //}
                var success = UpdateRemoteClues(dbFile, cList, this);

                btnQuery_Click(null, null);
                MessageBox.Show("上传数据：" + cList.Count + ", 成功：" + success);

            }
            catch(Exception ex )
            {
                MessageBox.Show("上传线索失败：" + ex.Message);
            }

            this.Cursor = Cursors.Arrow;
        }

        public static int UpdateRemoteClues(String _dbFile, IEnumerable<WFM.JW.HB.Models.Clues> _clues, IWin32Window owner)
        {
            String SqlUpdate = @"Update Clue_Report set isuploaded = @isuploaded where rowid = @rowid";

            线索管理.UploadCluePO uploader = new 线索管理.UploadCluePO(_clues.ToList());
            Erik.Utilities.Lib.StartProgressiveOperation(uploader, owner);

            DAL.MySqlite result = new DAL.MySqlite(_dbFile, GlobalEnviroment.isCryt);
            int success = 0;
            try
            {
                result.BeginTran();
                foreach (var c in uploader.returnList)
                {//update local db
                    
                    if (c.IsConfirmed == 1) success++;
                    string sql = SqlUpdate.Replace("@rowid", c.RowID.ToString());
                    sql = sql.Replace("@isuploaded", c.IsConfirmed.ToString());
                    result.ExecuteNonQuery(sql);
                }
                result.Commit();
            }
            catch (Exception ex)
            {
                result.RollBack();
            }
            return success;
        }

        public static int UpdateLocalClues(String _dbFile, IEnumerable<WFM.JW.HB.Models.Clues> _clues, IWin32Window owner)
        {
            #region UpdateSql
            String sql = @"UPDATE Clue_report  SET 
                                Addr = @Addr,
                                Region = @Region,
                                IsConfirmed = 1,
                                IsUploaded = 0,
                                IsClueTrue = @IsClueTrue,                                
                                IsCP = @IsCP,
                                Fact = @Fact,
                                IllegalMoney = @IllegalMoney,
                                CheckDate = @CheckDate,
                                CheckByName1 = @CheckByName1,
                                CheckByName2 = @CheckByName2,
                                ReCheckFact = @ReCheckFact,
                                ReCheckType = @ReCheckType,
                                ReCheckByName1 = @ReCheckByName1
                              Where RowID = @RowID ";
            #endregion

            DAL.MySqlite result = new DAL.MySqlite(_dbFile, GlobalEnviroment.isCryt);
            int success = 0;
            try
            {
                foreach (var c in _clues)
                {
                    System.Data.SQLite.SQLiteParameter[] sqliteParams = new System.Data.SQLite.SQLiteParameter[]
                                          {
                                                new System.Data.SQLite.SQLiteParameter("@RowID", c.RowID),
                                                new System.Data.SQLite.SQLiteParameter("@Addr", c.Addr),
                                                new System.Data.SQLite.SQLiteParameter("@Region", c.Region),
                                                new System.Data.SQLite.SQLiteParameter("@IsClueTrue", c.IsClueTrue),
                                                new System.Data.SQLite.SQLiteParameter("@IsCP", c.IsCP),
                                                new System.Data.SQLite.SQLiteParameter("@Fact", c.Fact),
                                                new System.Data.SQLite.SQLiteParameter("@IllegalMoney", c.IllegalMoney),
                                                new System.Data.SQLite.SQLiteParameter("@CheckDate", c.CheckDate),
                                                new System.Data.SQLite.SQLiteParameter("@CheckByName1", c.CheckByName1),
                                                new System.Data.SQLite.SQLiteParameter("@CheckByName2", c.CheckByName2),
                                                new System.Data.SQLite.SQLiteParameter("@ReCheckFact", c.ReCheckFact),
                                                new System.Data.SQLite.SQLiteParameter("@ReCheckType", c.ReCheckType),
                                                new System.Data.SQLite.SQLiteParameter("@ReCheckByName1", c.ReCheckByName1)
                                          };

                    result.ExecuteNonQuery(CommandType.Text, sql, sqliteParams);
                    success++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新核查问题失败：" + ex.Message);
            }
            return success;
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (PageIndex > 1)
                PageIndex--;
            else PageIndex = 1;

            GoPage(PageIndex);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            GoPage(++PageIndex);
        }

        private void GoPage(int index)
        {
            int _size = 15;

            if (dbFile.Length == 0)
            {
                MessageBox.Show("数据库错误！");
                return;
            }

            String Sql = @"SELECT Clue_Report.rowid,
                                   case when isuploaded = 1 then '已上传' else '未上传' end as 上传状态,
                                   ID AS 身份证号,
                                   Name AS 姓名,
                                   Region AS 乡镇,
                                   config.DataItem.DataShortName AS 项目名称,
                                   Type AS 线索类型,                                    
                                   CASE WHEN isconfirmed = 0 THEN '未核查' ELSE '已核查' END AS 线索状态,
                                   CASE WHEN isconfirmed = 0 THEN '' ELSE CASE WHEN isclueTrue = 1 THEN '查实' ELSE '查否' END END AS 核查状态,
                                   CASE WHEN isconfirmed = 0 THEN '' ELSE CASE WHEN IsCP = 1 THEN '党员' ELSE '群众' END END AS 政治面貌
                              FROM Clue_Report
                                   JOIN Config.DataItem ON Clue_Report.table1 = Config.DataItem.Rowid
                             WHERE 1 = 1";

            String CountSql = @"Select count() from Clue_Report where 1 = 1";

            DAL.MySqlite result = new DAL.MySqlite(dbFile, GlobalEnviroment.isCryt);
            DAL.MySqlite config = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                string clasue = "";

                result.AttchDatabase(config, "Config");

                if (cbCheckStatus.SelectedIndex == 0)
                    switch (cbDataStatus.SelectedIndex)
                    {
                        case 1:
                            clasue += " and IsConfirmed = 0 ";
                            break;
                        case 2:
                            clasue += " and IsConfirmed = 1  ";
                            break;
                    }
                else if (cbCheckStatus.SelectedIndex == 1)
                    clasue += " and isclueTrue = 1";
                else clasue += " and isclueTrue = 0";

                int Itemtype = int.Parse(cbItem.SelectedValue.ToString());
                if (Itemtype != 0)
                    clasue += " and Table1 = @Itemtype";

                switch (cbUploaded.SelectedIndex)
                {
                    case 1:
                        clasue += " and isuploaded = 0 ";
                        break;
                    case 2:
                        clasue += " and isuploaded = 1  ";
                        break;
                    case 3:
                        clasue += " and (isuploaded <> 1 or isuploaded <> 0) ";
                        break;
                }

                if (tbID.Text.Length != 0)
                    clasue += " and ID like @ID ";

                Sql += clasue + String.Format(" ORDER BY Region,config.DataItem.DataShortName,ID LIMIT {0} offset {0}*{1}", _size,  index - 1);
                CountSql += clasue;

                String condition = tbID.Text + "%";
                var count = result.ExecuteScalar(CommandType.Text, CountSql, new System.Data.SQLite.SQLiteParameter[]
                                                        {
                                                            new System.Data.SQLite.SQLiteParameter("@Itemtype", Itemtype),
                                                            new System.Data.SQLite.SQLiteParameter("@ID", condition)
                                                        });


                int total = 0;
                int.TryParse(count.ToString(), out total);

                int totalpage = total / _size;
                if (total % _size != 0)
                    totalpage++;

                if (index > totalpage)
                {
                    PageIndex--;
                    return;
                }                  
               

                var ds = result.ExecuteDataset(CommandType.Text, Sql, new System.Data.SQLite.SQLiteParameter[]
                                                        {
                                                            new System.Data.SQLite.SQLiteParameter("@Itemtype", Itemtype),
                                                            new System.Data.SQLite.SQLiteParameter("@ID", condition)
                                                        });
                if (ds != null)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[0].Visible = false;

                    lbInfo.Text = "" + count.ToString() + "条记录   第 " + index + "页，总" + totalpage + " 页";

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
