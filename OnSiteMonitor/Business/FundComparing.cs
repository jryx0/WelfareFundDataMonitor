//#define 加密

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

using System.Text.RegularExpressions;
using System.Windows.Forms;
using OnSiteFundComparer.Models;


namespace OnSiteFundComparer.Business
{
    class FundComparing
    {
        private log4net.ILog log;
        //private string MainDB = Application.StartupPath + "\\" +
        //Properties.Settings.Default.MainDBFile;

        private string ResultExcelDir = GlobalEnviroment.ResultOutputDir; //excel file    
        private string InputDBDir = GlobalEnviroment.InputDBDir;
        private string InputExcelDir = GlobalEnviroment.InputExceltDir;

        private DAL.MySqlite MainSqliteDB = null;
        private DAL.MySqlite ImportSqliteDB = null;
        private DAL.MySqlite ResultSqliteDB = null;
        private string ComparingInfo;

        public FileTranser.MTOM.ClassLibrary.WebServicesHelp theWebService;
        public bool IsDataChecking = false;

        public void InitEnviroment()
        {
            theWebService = GlobalEnviroment.theWebService;
        }

        public FundComparing()
        {
            log = GlobalEnviroment.log;
            GlobalEnviroment.MakeSureDirectory(InputDBDir);
            GlobalEnviroment.MakeSureDirectory(ResultExcelDir);
        }

        internal bool Start(Models.Task _t)
        {

            log.Info("-创建任务:");
            Models.Task task = _t;
            TimeSpan ts = DateTime.Now - new DateTime(2015, 1, 1);
            ComparingInfo = ts.TotalMilliseconds.ToString();
            task.DBInfo = ComparingInfo;//ts.TotalMilliseconds.ToString();
            task.UserName = GlobalEnviroment.LoginedUser.Name;


            MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile);
            ImportSqliteDB = new DAL.MySqlite(InputDBDir + ComparingInfo + ".db"); //中间文件file db
            ResultSqliteDB = new DAL.MySqlite(InputDBDir + "result." + ComparingInfo + ".db"); //结果文件file db

            ImportSqliteDB.ExecuteNonQuery("attach database '" + InputDBDir + "result." + ComparingInfo + ".db' as result");

#if 加密
            ImportSqliteDB.sqlitePassword = "OnSiteMonitor.com";
#endif
            // ImportSqliteDB = new DAL.MySqlite(":memory:");


            if (!IsDataChecking && !IsChecked())
            {
                if (MessageBox.Show("需要首先对数据进行校验， 请先进行数据校验！", "数据校验", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    IsDataChecking = true;
                }
                else return false;
            }

            if (IsDataChecking)
            {//进行数据校验                
                return ImportCompDataCheck(); ;
            }


            if (!CreateTask(task))
                return false;
            log.Info("-任务创建成功!");

            Business.DataMgr dmgr = new DataMgr();
            var complist = dmgr.GetChildDataItemList(3);
            var sourcelist = dmgr.GetChildAllList(2);
            var formatslist = dmgr.GetAllDataFormatList();

            log.Info("-创建输入数据库.");
            if (!CreateInputDB(task, complist))
                return false;

            if (!CreateInputDB(task, sourcelist))
                return false;
            log.Info("输入数据库创建成功!");

            log.Info("开始导入比对数据Excel数据文件");
            ImportCompExcel(complist);
            //数据库清理
            if (!CleanUpData())
            {
                return false;
            }

            log.Info("开始导入源数据Excel数据文件");
            ImportSourceExcelEx(sourcelist, formatslist);

            log.Info("-输入数据库创建成功!");
            log.Info("----------------------------------------------------------\r\n\r\n");

            log.Info("==============开始对数据进行预处理===============");
            PreProcessData(complist);
            PreProcessData(sourcelist);
            log.Info("==============数据预处理完成===============\r\n");



            log.Info("********************************************************************\r\n");
            log.Info("开始比对...");
            ResultExcelDir += "\\比对结果(" + System.DateTime.Now.ToString("yyyy年MM月dd日-HH点mm分ss秒") + ")\\";
            GlobalEnviroment.MakeSureDirectory(ResultExcelDir);
            GetResult(sourcelist);
            GetReport();

            if (!GlobalEnviroment.LocalVersion)
            {
                log.Info("--------------开始上传比对结果--------------------------");
                UI.UploadResult ur = new UI.UploadResult();
                ur.FilePath = InputDBDir + "result." + ComparingInfo + ".db";
                ur.task = _t;
                theWebService = GlobalEnviroment.theWebService;
                ur.ws = theWebService;
                if (ur.ShowDialog() != DialogResult.OK)
                {
                    log.Error("------------上传数据失败--------------");
                    log.Error("退出比对！");
                    return false;
                }

                log.Info("-------------- 上传成功-----------------");
            }

            try
            {
                SaveReulstFile(sourcelist);
                SaveSummaryFile(sourcelist);
            }
            catch(Exception ex)
            {
                log.Info("错误:" + ex.Message);
                return false;
            }

            if (!UpdateTask(_t)) return false;

            MainSqliteDB.CloseConnection();
            ImportSqliteDB.CloseConnection();
            ResultSqliteDB.CloseConnection();

            MainSqliteDB = null;
            ImportSqliteDB = null;
            ResultSqliteDB = null;


            log.Info("*****************************************************");
            log.Info("*                      比对结束                       *");
            log.Info("******************************************************");

            return true;
        }

        private void PreProcessData(List<DataItem> datalist)
        {
            ImportSqliteDB.BeginTran();
            foreach (DataItem di in datalist)
            {
                if (di.col1.Length != 0)
                    try
                    {
                        var colSql = di.col1.Split(';');
                        foreach(string sql in colSql)
                        {
                            ImportSqliteDB.ExecuteDataset(sql);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info("$$$$$$$$$$$预处理数据错误:" + di.DataFullName + "\r\n");
                        log.Info("col1 = " + di.col1 + "\r\n");
                        log.Info("$$$$$$$$$$$异常信息:\r\n" + ex.Message + "\r\n");

                        ImportSqliteDB.RollBack();
                        return;
                    }

            }
            ImportSqliteDB.Commit();
        }

        private bool IsChecked()
        {
            bool bRet = false;
            try
            {
                var o = MainSqliteDB.ExecuteScalar("select settingvalue from Setting where  settingname = 'ImportDataChecking'");

                int status = 0;
                int.TryParse(o.ToString(), out status);
                if (status == 1)
                    bRet = true;
            }
            catch (Exception ex)
            {

            }

            return bRet;
        }
        private bool ImportCompDataCheck()
        {

            log.Info("开始进行数据校验...");
            TimeSpan ts = DateTime.Now - new DateTime(2015, 1, 1);
            ComparingInfo = ts.TotalMilliseconds.ToString();

            MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile);
            ImportSqliteDB = new DAL.MySqlite(InputDBDir + "Check." + ComparingInfo + ".db"); //file db

            MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile);
            ImportSqliteDB = new DAL.MySqlite(InputDBDir + "Check." + ComparingInfo + ".db"); //file db



            Business.DataMgr dmgr = new DataMgr();
            var complist = dmgr.GetChildDataItemList(3);
            var formatslist = dmgr.GetAllDataFormatList();


            if (!CreateInputDB(new Task(), complist))
                return false;

            log.Info("导入比对数据Excel数据文件");
            ImportCompExcel(complist);
            CleanUpData();


            ResultExcelDir += "\\数据校验结果(" + System.DateTime.Now.ToString("yyyy年MM月dd日-HH点mm分ss秒") + ")\\";
            GlobalEnviroment.MakeSureDirectory(ResultExcelDir);

            try
            {
                var aims = new Service.DataCheckService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCheckAllRules();
                foreach (var a in aims.Where(x => x.Type == 1))
                {//校验财政供养人员身份证号/领导干部身份证号/村干部身份证号/家属身份证号
                    if (a == null || a.CheckSql == "")
                        continue;
                    try
                    {//清理数据库
                        ImportSqliteDB.ExecuteNonQuery("create table  " + a.CheckName + " as " + a.CheckSql);
                        log.Info("完成:" + a.CheckName + "，成功获取校验结果!");
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message + " aim:" + a.CheckSql);
                    }

                    foreach (var b in aims.Where(x => x.Type == 1))
                    {
                        var ds = ImportSqliteDB.ExecuteDataset("select * from " + a.CheckName + " order by 单位");

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                            SaveToExcelFile(ResultExcelDir, a.CheckName + "结果", ds.Tables[0]);
                    }

                    //update checking status
                    MainSqliteDB.ExecuteNonQuery("Update Setting set settingvalue = 1 where settingname = 'ImportDataChecking'");
                }
            }
            catch (Exception ex)
            {
                log.Error("比对数据校验错误：" + ex.Message);
            }

            log.Info("\r\n\r\n结果文件保存位置：");
            log.Info(ResultExcelDir);
            log.Info("\r\n\r\n");

            log.Info("*****************************************************");
            log.Info("*                      数据校验完成                    *");
            log.Info("******************************************************");

            return true;
        }

        private void SaveReulstFile(List<DataItem> sourcelist)
        { 
            var aims = new Service.CompareAimService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCompareAim();

            aims = aims.OrderBy(x => x.seq).ToList();
            int FileSeq = 1;
            foreach (var a in aims)
            {
                DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
                if (di == null)
                    continue;

                var o = ImportSqliteDB.ExecuteScalar(@"select count(*) from sqlite_master where type = 'table' and name = '" + a.TableName + "'");
                if (o != null)
                {
                    int i = 0;
                    int.TryParse(o.ToString(), out i);
                    if (i == 0)
                        continue;
                }
                //明细

                int total = 0;
                o = ImportSqliteDB.ExecuteScalar("Select count() from[" + a.TableName + "]");
                int.TryParse(o.ToString(), out total);

                for(int j = 0; j <= total % 60000; j ++)
                { 
                    var dt = GetDataTable(ImportSqliteDB, "Select * from [" + a.TableName + "] limit " + j * 60000 + " , 60000");
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        string SavePath = ResultExcelDir + di.DataShortName + "\\明细数据\\";
                        SaveToExcelFile(SavePath, String.Format("{0:00}", FileSeq) + a.AimName + "- 明细数据" + j, dt);                    

                        log.Info("生成文件 --- " + a.AimName + "- 明细数据.xls,成功！");
                        dt.Clear();
                    }
                    else
                    {
                        break;
                    }
                }


                FileSeq++; 
            }
        }

        /*
        private void SaveReulstFile(List<DataItem> sourcelist)
        {
            //DataMgr dm = new DataMgr();
            //List<CollisionAim> aims = dm.GetAllAim();
            var aims = new Service.CompareAimService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCompareAim();

            aims = aims.OrderBy(x => x.seq).ToList();
            /////////////////////////
            DataTable resultDT = new DataTable();

            resultDT.Columns.Add("序号");
            resultDT.Columns.Add("大类");
            resultDT.Columns.Add("小类");
            resultDT.Columns.Add("明细条数");
            resultDT.Columns.Add("按人汇总数");
            ////////////////////////          
            int FileSeq = 1;
            foreach (var a in aims)
            {
                DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
                if (di == null)
                    continue;

                /////////////////////////
                DataRow dr = resultDT.NewRow();
                dr[1] = di.DataShortName;
                dr[2] = a.AimName;
                /////////////////////////

                var o = ImportSqliteDB.ExecuteScalar(@"select count(*) from sqlite_master where type = 'table' and name = '" + a.TableName + "'");
                if (o != null)
                {
                    int i = 0;
                    int.TryParse(o.ToString(), out i);
                    if (i == 0)
                        continue;
                }

                //明细
                var dt = GetDataTable(ImportSqliteDB, "Select * from [" + a.TableName + "]");
                if (dt != null && dt.Rows.Count != 0)
                {
                    string SavePath = ResultExcelDir + di.DataShortName + "\\明细数据\\";
                    SaveToExcelFile(SavePath, String.Format("{0:00}", FileSeq) + a.AimName + "- 明细数据", dt);

                    dr[3] = dt.Rows.Count;

                    log.Info("生成文件 --- " + a.AimName + "- 明细数据.xls,成功！");
                    dt.Clear();
                }
                else dr[3] = 0;


                //汇总
                dt = GetDataTable(ImportSqliteDB, a.Rules2);
                if (dt != null && dt.Rows.Count != 0)
                {
                    string SavePath = ResultExcelDir + di.DataShortName + "\\按人分项汇总\\";
                    SaveToExcelFile(SavePath, String.Format("{0:00}", FileSeq) + a.AimName + "- 按人分项", dt);

                    dr[4] = dt.Rows.Count;

                    log.Info("生成文件 --- " + a.AimName + "- 按人分项.xls,成功！");
                    dt.Clear();
                }
                else dr[4] = 0;

                FileSeq++;

                ////按人统计
                //dt = GetDataTable(ResultSqliteDB, a.Rules2);
                //if (dt != null && dt.Rows.Count != 0)
                //{
                //    string SavePath = ResultDir + di.DataShortName + "\\按人统计\\";
                //    SaveToExcelFile(SavePath, a.AimName, dt);
                //}
                //dt.Clear();

                resultDT.Rows.Add(dr);
            }
           // SaveToExcelFile(ResultExcelDir + "\\", "比对结果统计", resultDT);
        }
        */
        private void SaveSummaryFile(List<DataItem> sourcelist)
        {         

            ImportSqliteDB.AttchDatabase(MainSqliteDB, "config");
            string summarySql = @"SELECT '' as 序号, b.Datashortname 项目名称,
                                        a.总数 总数,
                                        a.问题数 问题数,
                                        a.输入错误 输入错误
                                    FROM vw_Clues_Summary a
                                        JOIN config.dataitem b ON a.table1 = b.RowID
                                    ORDER BY b.seq";
            string ItemSql = @"SELECT '' as 序号, Region 乡镇街道, ID 身份证号, Name 姓名, addr 地址, type 线索类型, dateRange 领取时间, comment 备注
                                  FROM  Clue_report
                                 WHERE table1 = @table1 order by Region, addr, ID";


            log.Info("-------------------开始汇总------------ ");
            string regionName = Properties.Settings.Default.CurentRegionName;
           // Business.SaveToExcelXml reportFile = new Business.SaveToExcelXml();
            var summaryDT = ImportSqliteDB.ExecuteDataset(summarySql);
            if (summaryDT != null && summaryDT.Tables[0].Rows.Count != 0)
            {
                 SaveToExcelFile(ResultExcelDir ,  regionName + "惠民政策监督检查问题线索总表", summaryDT.Tables[0]);

                //reportFile.AddSummarySheet(regionName + "惠民政策监督检查问题线索统计表", summaryDT.Tables[0]);
                //reportFile.SaveExcelXml(ResultExcelDir + regionName + "惠民政策监督检查问题线索总表.xls");
            }

            foreach (var di in sourcelist)
            {
                var dt = GetDataTable(ImportSqliteDB, ItemSql.Replace("@table1", di.RowID.ToString()));
                if (dt != null && dt.Rows.Count != 0)
                {
                    var newdt = SreeenDataTable(dt, "线索类型 LIKE '%不一致%' AND 线索类型 NOT LIKE '%+%'", "乡镇街道, 地址, 身份证号");
                    if (newdt.Rows != null || newdt.Rows.Count != 0)
                        SaveToExcelFile(ResultExcelDir + di.DataShortName + "\\", regionName + "数据录入问题", newdt);

                    newdt = SreeenDataTable(dt, "线索类型 not LIKE '%不一致%' or 线索类型 LIKE '%+%'", "乡镇街道, 地址, 身份证号");
                    SaveToExcelFile(ResultExcelDir + di.DataShortName + "\\", regionName + di.DataShortName + "问题线索-按人分项统计", newdt);

                    log.Info("生成文件 --- " + di.DataFullName + "- 按人分项统计.xls,成功！");


                    dt.Clear();
                } 
            } 
        }

        private void GetResult(List<DataItem> sourcelist)
        {
            //DataMgr dm = new DataMgr();
            // List<CollisionAim> aims = dm.GetAllAim();
            var aims = new Service.CompareAimService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCompareAim();
            foreach (var a in aims)
            {
                if (a == null || a.Rules == "")
                    continue;

                DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
                if (di == null)
                    continue;

                try
                {//结果 -->新数据库中
                    ImportSqliteDB.ExecuteNonQuery("create table  " + a.TableName + " as " + a.Rules);
                    log.Info("成功获取 " + a.AimName + " 的比对结果!");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message + "AimName:" + a.AimName + " aim:" + a.Rules);
                }
            }


            try
            {
                ResultSqliteDB.ExecuteNonQuery(@"CREATE TABLE Clue_report 
                                                    (RowID          INTEGER       PRIMARY KEY AUTOINCREMENT,
                                                     ID             VARCHAR (20),
                                                     Name           VARCHAR (20),
                                                     Addr           VARCHAR (100),
                                                     Region         VARCHAR (50),
                                                     Type           VARCHAR (500),                                                    
                                                     DateRange      VARCHAR (200),
                                                     Comment         VARCHAR (500),  
                                                     Table1         INTERGER,  
                                                     InputError      INTERGER DEFAULT (0),                                                 
                                                     IsConfirmed     INTERGER DEFAULT (0),
                                                     IsClueTrue     INTERGER,
                                                     IsCompliance    INTERGER,
                                                     IsCP           INTERGER,
                                                     Fact           VARCHAR (500),
                                                     IllegalMoney   real,
                                                     CheckDate      DATETIME,
                                                     CheckByName1   VARCHAR (10),
                                                     CheckByName2   VARCHAR (10),
                                                     ReCheckFact    VARCHAR(100),
                                                     ReCheckType    INTERGER,
                                                     ReCheckByName1 VARCHAR (10),
                                                     IsUploaded     INTERGER DEFAULT (0))");
                ResultSqliteDB.ExecuteNonQuery(@"CREATE INDEX Clue_report_ID ON Clue_report (ID ASC)");

                ResultSqliteDB.ExecuteNonQuery(@"CREATE TABLE report 
                                                    (
                                                     ID             VARCHAR (20),
                                                     Name           VARCHAR (20),
                                                     Addr           VARCHAR (100),
                                                     Region         VARCHAR (50),
                                                     Type           VARCHAR (50),
                                                     Amount         real,
                                                     DateRange      VARCHAR (200),
                                                     Comment  VARCHAR (200),
                                                     Table1         INTERGER,
                                                     Table2         INTERGER
                                                     )");
                ResultSqliteDB.ExecuteNonQuery(@"CREATE INDEX Report_ID ON report (ID ASC)");
            }
            catch (Exception ex)
            {
            }

            foreach (var a in aims)
            {
                if (a == null || a.Rules == "")
                    continue;

                DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
                if (di == null)
                    continue;

                log.Info("正在按人汇总比对结果!");
                try
                {//按人汇总 -->新数据库中                   
                    ImportSqliteDB.ExecuteNonQuery("insert into result.report(Type, Region, ID, Name, Addr, DateRange , Comment, Table1, Table2)"
                        + a.Rules3.Replace("@injection", ", " + a.t1 + ", " + (a.t2 * 100 + a.t3).ToString()));
                    log.Info("按人汇总" + a.AimName + "比对结果!");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message + "AimName:" + a.AimName + " aim:" + a.Rules3);
                }
            }
        }
        private void GetReport()
        {
            String vw = @"CREATE VIEW vw_Clues_Summary AS
                                    SELECT a.table1,
                                           a.总数,
                                           ifnull(b.输入错误, 0) 输入错误,
                                           c.问题数
                                      FROM
                                           (SELECT count() 总数,
                                                   table1
                                              FROM Clue_report
                                             GROUP BY table1) a
                                           LEFT JOIN
                                           (SELECT count() 输入错误,
                                                   table1
                                              FROM Clue_report
                                             WHERE type LIKE '%不一致%' AND 
                                                   type NOT LIKE '%+%'
                                             GROUP BY table1) b ON a.table1 = b.table1
                                           left JOIN
                                           (SELECT count() 问题数,
                                                   table1
                                              FROM Clue_report
                                             WHERE (type NOT LIKE '%不一致%' OR 
                                                    type LIKE '%+%') 
                                             GROUP BY table1) c ON a.table1 = c.table1";

            string TableSql = @"INSERT INTO Clue_report(Region,ID, Name, Addr, Type,DateRange,Comment,Table1) 
                                     SELECT group_concat(DISTINCT region) region,
                                            ID,
                                            group_concat(DISTINCT Name) Name,
                                            group_concat(DISTINCT addr) addr,
                                            replace(group_concat(DISTINCT type), ',', '+') type,
                                            group_concat(dateRange, '/') dateRange,
                                            group_concat(comment, '/') comment,
                                            table1
                                        FROM report
                                        GROUP BY id,  table1
                                        ORDER BY region,type,id";
            string InputError = @"Update Clue_Report Set InputError = 1 where type LIKE '%不一致%' AND 
                                                   type NOT LIKE '%+%'";

            try
            {
                ResultSqliteDB.ExecuteNonQuery(TableSql);
                ResultSqliteDB.ExecuteNonQuery(InputError);

                ResultSqliteDB.ExecuteNonQuery(vw);
            }
            catch (Exception ex)
            {

            }

        }

        private DataTable GetDataTable(DAL.MySqlite db, String Sql)
        {
            DataTable dt = null;
            if (Sql == "")
                return dt;

            try
            {
                DataSet ds = db.ExecuteDataset(Sql);

                if (ds != null)
                    dt = ds.Tables[0];
                //log.Info("成功获取 "+ aim.AimName + " 的比对结果!");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + Sql);
            }

            return dt;
        }

        //private void DBtoExcle(List<DataItem> sourcelist)
        //{
        //    DataMgr dm = new DataMgr();
        //    List<CollisionAim> aims = dm.GetAllAim();

        //    DataTable resultDT = new DataTable();
        //    resultDT.Columns.Add("序号");
        //    resultDT.Columns.Add("大类");
        //    resultDT.Columns.Add("小类");
        //    resultDT.Columns.Add("按人汇总数");
        //    resultDT.Columns.Add("明细条数");

        //    foreach (var a in aims)
        //    {
        //        DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
        //        if (di == null)
        //            continue;

        //        DataRow dr = resultDT.NewRow();
        //        dr[1] = di.DataShortName;
        //        dr[2] = a.AimName;

        //        //DataTable dt = DataCollision(a.Rules);
        //        //if (dt == null || dt.Rows.Count == 0)
        //        //{
        //        //    dr[3] = 0;
        //        //    //continue;
        //        //}
        //        //else
        //        //{
        //        //    dr[3] = dt.Rows.Count;
        //        //    string ResultFileDir = ResultDir + di.DataShortName + "\\明细数据\\";
        //        //    SaveToExcelFile(ResultFileDir, a.AimName, dt);
        //        //    dt.Clear();

        //        //    log.Info("成功写入文件(明细数据):" + a.AimName + ".xls");
        //        //}

        //        //dt = DataCollision(a.Rules2);
        //        //if (dt == null || dt.Rows.Count == 0)
        //        //{
        //        //    dr[4] = 0;
        //        //    //continue;
        //        //}
        //        //else
        //        //{
        //        //    dr[4] = dt.Rows.Count;
        //        //    string ResultFileDir = ResultDir + di.DataShortName + "\\按人汇总\\";
        //        //    SaveToExcelFile(ResultFileDir, a.AimName, dt);
        //        //    dt.Clear();

        //        //    log.Info("成功写入文件(按人汇总):" + a.AimName + ".xls");
        //        //}

        //        resultDT.Rows.Add(dr);
        //    }

        //    SaveToExcelFile(ResultExcelDir + "\\", "比对结果汇总", resultDT);
        //}

        internal bool CreateTask(Models.Task _t)
        {
            bool bRet = false;

            string Sql = @"insert into Task(TaskName, CreateDate, TaskComment, Region, DBInfo, UserName) 
                                     values(@TaskName, @CreateDate, @TaskComment, @Region, @DBInfo, @UserName)";

            try
            {
                MainSqliteDB.ExecuteNonQuery(CommandType.Text, Sql, Models.Task.getParam(_t));
                var o = MainSqliteDB.ExecuteScalar("SELECT last_insert_rowid()");

                _t.RowID = int.Parse(o.ToString());

                bRet = true;
                log.Info("创建任务成功!");
            }
            catch (Exception ex)
            {
                log.Error("创建任务失败");
                log.Error(ex.Message);
            }

            return bRet;
        }
        internal bool UpdateTask(Models.Task _t)
        {
            bool bRet = false;

            string Sql = @"update Task set status = 10 where rowid = @rowid";

            try
            {
                MainSqliteDB.ExecuteNonQuery(CommandType.Text, Sql,
                    new System.Data.SQLite.SQLiteParameter[] {
                        new System.Data.SQLite.SQLiteParameter("@rowid", _t.RowID)}
                    );

                bRet = true;
                //log.Info("任务成功!");
            }
            catch (Exception ex)
            {
                log.Error("更新任务失败");
                log.Error(ex.Message);
            }

            return bRet;
        }
        internal int GetTaskNum()
        {
            int nRet = 0;

            string Sql = @"select count() from  Task where  tstatus = 10";

            try
            {
                var o = MainSqliteDB.ExecuteScalar(Sql);
                int.TryParse(o.ToString(), out nRet); ;
                //log.Info("任务成功!");
            }
            catch (Exception ex)
            {
                log.Error("更新任务失败");
                log.Error(ex.Message);
            }

            return nRet;
        }

        internal bool CreateInputDB(Models.Task _t, List<Models.DataItem> complist)
        {
            bool bRet = false;

            //DAL.MySqlite _sqliteInput = new DAL.MySqlite();
            //_sqliteInput.sqliteConnectionString = ImportDB;
            try
            {
                ImportSqliteDB.BeginTran();
                foreach (Models.DataItem di in complist)
                {
                    if (di.dbTable.Length == 0)
                        continue;

                    string CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTable);
                    ImportSqliteDB.ExecuteNonQuery(CreateTable);

                    if(di.col1.Length != 0)
                    {
                        CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTablePre);
                        ImportSqliteDB.ExecuteNonQuery(CreateTable);
                    }

                }

                // _sqliteInput.ExecuteNonQuery();
                ImportSqliteDB.Commit();

                bRet = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ImportSqliteDB.RollBack();
            }

            //_sqliteInput.CloseConnection();

            return bRet;
        }
        internal bool ImportCompExcel(List<Models.DataItem> complist)
        {
            DataMgr dmg = new DataMgr();
            bool bRet = false;
            int totalData = 0;

            foreach (Models.DataItem di in complist)
            {
                string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
                //log.Info("导入:" + di.DataFullName + ", 路径:" + dir);

                int totalItemData = 0;
                DirectoryInfo folder = new DirectoryInfo(dir);
                try
                {

                    var filelist = folder.GetFiles("*.xls");
                    foreach (FileInfo file in filelist)
                    {
                        //log.Info("读取文件:" +　file.Name);
                        int readlines = ReadXLSToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                        // ReadXLSToDBWithFormat(file.FullName, di, dmg.GetDataFormatList(di));
                    }
                    if (filelist.Count() != 0)
                        log.Info("共导入: " + filelist.Count() + " 个.xls文件");

                    filelist = folder.GetFiles("*.csv");
                    foreach (FileInfo file in filelist)
                    {
                        //log.Info("读取文件:" + file.Name);
                        int readlines = ReadCSVToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                    }
                    if (filelist.Count() != 0)
                        log.Info("共导入: " + filelist.Count() + " .csv个文件");

                    bRet = true;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    bRet = false;
                }

                // log.Info(di.DataFullName + " 共有: " + totalItemData.ToString() + "条数据插入数据库");
                totalData += totalItemData;
            }
            return bRet;
        }
        public bool ImportSourceExcelEx(List<Models.DataItem> sourcelist, List<Models.DataFormat> formats)
        {
            DataMgr dmg = new DataMgr();
            bool bRet = false;
            int totalData = 0;

            foreach (Models.DataItem di in sourcelist)
            {
                string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";

                log.Info("读入" + di.DataFullName + "的数据文件");
                // log.Info("路径:" + dir);
                int totalItemData = 0;
                DirectoryInfo folder = new DirectoryInfo(dir);
                try
                {


                    var filelist = folder.GetFiles("*.xls");
                    foreach (FileInfo file in filelist)
                    {
                        //log.Info("读取文件:" +　file.Name);
                        int readlines = ReadXLSToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                        // ReadXLSToDBWithFormat(file.FullName, di, dmg.GetDataFormatList(di));
                    }
                    if (filelist.Count() != 0)
                        log.Info("共导入: " + filelist.Count() + " 个.xls文件");

                    filelist = folder.GetFiles("*.csv");
                    foreach (FileInfo file in filelist)
                    {
                        //log.Info("读取文件:" + file.Name);
                        int readlines = ReadCSVToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                    }
                    if (filelist.Count() != 0)
                        log.Info("共导入: " + filelist.Count() + " .csv个文件");

                    bRet = true;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    bRet = false;
                }
                totalData += totalItemData;
            }
            return bRet;
        }

        internal bool CleanUpData()
        {
            bool bRet = true;
            log.Info("清除重复数据");

            var aims = new Service.DataCheckService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCheckAllRules();
            foreach (var a in aims.Where(x => x.Type == 10))
            {
                if (a == null || a.CheckSql == "")
                    continue;
                try
                {//清理数据库
                    var Sqls = a.CheckSql.Split(';');
                    foreach (string s in Sqls)
                        ImportSqliteDB.ExecuteNonQuery(s);

                    log.Info("成功完成：" + a.CheckName + "!");
                    bRet = true;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message + " 清库:" + a.CheckSql);
                    bRet = false;

                }


            }
            return bRet;
        }

        public int ReadXLSToDBWithFormatEx(string FileName, Models.DataItem dataItem, List<Models.DataFormat> dataFormat)
        {
            System.Collections.IEnumerator rows = GlobalEnviroment.getExcelFileRows(FileName);
            if (rows == null)
            {
                log.Error("文件" + Path.GetFileName(FileName) + "无法打开");
                return -1;
            }

            if (dataItem == null || dataFormat == null || dataFormat.Count == 0)
            {
                log.Error("未找到文件格式!");
                return -1;
            }

            //double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0;


            ImportSqliteDB.BeginTran();
            int readlines = 0;
            while (rows.MoveNext())
            {
                //the first format is line number
                if (readlines++ < dataFormat[0].colNumber - 1)
                    continue;

                if (FileName.IndexOf("2014.10") != -1)
                {
                    if (readlines == 1957)
                    {

                    }
                }//debug     

                string Lines = "";
                IRow row = (HSSFRow)rows.Current;
                String Sql = "";
                try
                {
                    bool isAllNull = true;
                    int maxcol = 50;
                    if (row.LastCellNum < maxcol)
                        maxcol = row.LastCellNum;

                    for (int i = 0; i < maxcol; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell == null)
                            Lines += "&";
                        else if (cell.CellType == CellType.Blank)
                            Lines += "&";
                        else
                        {
                            //System.DateTime sdt1 = System.DateTime.Now;
                            Lines += GlobalEnviroment.GetExcelValue(cell) + "&";
                            //System.DateTime edt1 = System.DateTime.Now;
                            //t1 += (edt1 - sdt1).TotalMilliseconds;
                            isAllNull = false;
                        }
                    }

                    if (Lines.Length == 0 || isAllNull)
                    {
                        Lines = "";
                        continue;
                    }

                    List<SQLiteParameter> pList = new List<SQLiteParameter>();
                    string[] cols = Lines.Split('&');

                    //System.DateTime sdt2 = System.DateTime.Now;
                    Sql = GenerateInsertSqlWithFormat(cols, dataFormat, pList);
                    //System.DateTime edt2 = System.DateTime.Now;
                    // t2 += (edt2 - sdt2).TotalMilliseconds;
                    var para = Sql.Split('-');

                    //add Item data name  如：城市低保/农村低保等
                    if (para.Length == 2)
                    {
                        para[1] += "@ItemType,";
                        para[0] += "ItemType";

                        pList.Add(new SQLiteParameter("@ItemType", dataItem.DataShortName));
                    }


                    //System.DateTime sdt3 = System.DateTime.Now;
                    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0], para[1]);
                    ImportSqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                    //System.DateTime edt3 = System.DateTime.Now;
                    //t3 += (edt3 - sdt3).TotalMilliseconds;

                   

                    pList.Clear();
                }
                catch (Exception ex)
                {
                    ImportSqliteDB.RollBack();
                    log.Info("插入数据库失败, Sql = " + Sql);
                    log.Info("错误信息:" + ex.Message);
                    log.Info("文件" + FileName + "\r\n行号:" + readlines.ToString());
                    return -1;
                }
            }
            ImportSqliteDB.Commit();

            




            readlines = readlines - dataFormat[0].colNumber;
            log.Info("文件:<" + Path.GetFileName(FileName) + ">, 写入数据库成功, 共 " + readlines.ToString() + " 条记录!");
           
             ImportSqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null") ;

            return readlines < 0 ? readlines : 0;
        }
        public int ReadCSVToDBWithFormatEx(string FileName, Models.DataItem dataItem, List<Models.DataFormat> dataFormat)
        {
            StreamReader mysr;
            try
            {
                mysr = new StreamReader(FileName, System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                log.Error("读取CSV文件错误:" + FileName);
                log.Error(ex.Message);
                return -1;
            }

            ImportSqliteDB.BeginTran();
            String Lines = "";
            int readlines = 0;
            while ((Lines = mysr.ReadLine()) != null)
            {
                if (readlines++ < dataFormat[0].colNumber - 1)
                    continue;
                string Sql = "";
                try
                {
                    List<SQLiteParameter> pList = new List<SQLiteParameter>();
                    string[] cols = Lines.Split(',');

                    Sql = GenerateInsertSqlWithFormat(cols, dataFormat, pList);
                    var para = Sql.Split('-');


                    //add Item data name  如：城市低保/农村低保等
                    if (para.Length == 2)
                    {
                        para[1] += "@ItemType,";
                        para[0] += "ItemType";

                        pList.Add(new SQLiteParameter("@ItemType", dataItem.DataShortName));
                    }

                    //if (dataItem.parentItem != null && dataItem.parentItem.DataType != Models.FundItemTypes.SourceItems)
                    //{//has comment
                    //    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0] + "Type,", para[1] + "@Type");
                    //    pList.Add(new SQLiteParameter("@Type", dataItem.DataShortName));
                    //}
                    //else
                    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0], para[1]);

                    ImportSqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                    pList.Clear();
                }
                catch (Exception ex)
                {
                    ImportSqliteDB.RollBack();
                    log.Info("插入数据库失败, Sql = " + Sql);
                    log.Info("错误信息:" + ex.Message);
                    return -1;
                }

            }
            ImportSqliteDB.Commit();　

            readlines = readlines - dataFormat[0].colNumber;
            log.Info("文件" + Path.GetFileName(FileName) + "写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            ImportSqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null") ;
            return readlines;
        }

        public string GenerateInsertSqlWithFormat(string[] cols, List<Models.DataFormat> formats, List<SQLiteParameter> para)
        {
            string sqlColName = "";
            string sqlValues = "";
            int MaxCellNum = cols.Length;

            for (int i = 1; i < formats.Count; i++)
            {
                if (MaxCellNum < formats[i].colNumber)
                    continue;
                int index = formats[i].colNumber - 1;

                try
                {
                    string value = cols[index];
                    if (value != null)
                        value = value.Trim(' ');
                    object tmp;
                    System.DateTime sdt1 = System.DateTime.Now;
                    switch (formats[i].colName)
                    {
                        case "身份证号":
                            tmp = GlobalEnviroment.GetFullIDEx(value);
                            sqlColName += "ID,";
                            sqlValues += "@ID,";
                            para.Add(new SQLiteParameter("@ID", tmp));

                            tmp = value;
                            break;
                        case "相关人身份证号":
                            tmp = GlobalEnviroment.GetFullIDEx(value);
                            sqlColName += "sRelateID,";
                            sqlValues += "@sRelateID,";
                            para.Add(new SQLiteParameter("@sRelateID", tmp));

                            tmp = value;
                            break;
                        case "日期":
                            //tmp = value.Split(' ')[0];
                            tmp = GlobalEnviroment.tryParingDateTime(value/*.Split(' ')[0]*/);
                            sqlColName += "sDataDate,";
                            sqlValues += "@sDataDate,";
                            para.Add(new SQLiteParameter("@sDataDate", ((DateTime)tmp).ToString("yyyy-MM-dd")));

                            tmp = value;
                            break;
                        case "日期1":
                            tmp = GlobalEnviroment.tryParingDateTime(value).ToString("yyyy-MM-dd");
                            break;
                        case "姓名":
                        case "相关人姓名":
                            Regex reg = new Regex(@"[^\u4e00-\u9fa5]+");
                            tmp = reg.Replace(value, "");
                            //tmp = value.Replace(" ", ""); 
                            break;
                        case "金额":
                        case "面积":
                            double d = 0.0;
                            double.TryParse(value, out d);
                            tmp = d;//value;

                            if (value == "未兑现")
                                tmp = 0.0;
                            break;
                        default:
                            tmp = value;
                            break;
                    }

                    if (tmp.ToString().Length == 0)
                        tmp = DBNull.Value;

                    sqlColName += formats[i].colCode + ",";
                    sqlValues += "@" + formats[i].colCode + ",";
                    para.Add(new SQLiteParameter("@" + formats[i].colCode, tmp));
                }
                catch (Exception ex)
                {
                    throw new Exception("第" + index.ToString() + "列. "
                                   + "\r\n sqlColName=" + sqlColName + "\r\n SqlValues = " + sqlValues + "\r\n" +
                                   ex.Message);
                }
            }

            return sqlColName + "-" + sqlValues;
        }

        public void SaveToExcelFile(String Dir, String FileName, DataTable dt)
        {
            GlobalEnviroment.MakeSureDirectory(Dir);

            string ResultFile = Dir + FileName + ".xls";
            using (MemoryStream ms = GlobalEnviroment.DataTableToExcel(dt, FileName))
            {
                using (FileStream fs = new FileStream(ResultFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataRow转换为DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strWhere">筛选的条件</param>
        /// <returns></returns>
        public DataTable SreeenDataTable(DataTable dt, string strWhere, string order)
        {
            if (dt.Rows.Count <= 0) return dt;        //当数据为空时返回
            DataTable dtNew = dt.Clone();         //复制数据源的表结构
            DataRow[] dr = dt.Select(strWhere, order);  //strWhere条件筛选出需要的数据！
            for (int i = 0; i < dr.Length; i++)
            {
                dtNew.Rows.Add(dr[i].ItemArray);  // 将DataRow添加到DataTable中
            }
            return dtNew;
        }

    }
}
