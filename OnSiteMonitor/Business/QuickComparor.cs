using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using OnSiteFundComparer.Models;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;



/// <summary>
/// 多线程比对
/// 根据比对规则导入数据、根据项目合并输出结果
/// </summary>

namespace OnSiteFundComparer.Business
{
    public enum CompareEventEnum
    {
        Step = 0,
        Progress = 1,
        Info = 3
    }    
    public class CompareEventArgs : EventArgs
    {
        public CompareEventEnum ArgType { get; set; }

        public int Progress { set; get; }
        public int MaxProgress { set; get; }
        public String ProgressMessage { set; get; }

        

        /// <summary>
        /// 0 debug, 1 Info, 2 Warnning, 3 Error
        /// </summary>
        public int Level { set; get; }
    }
    public delegate void CompareEnventHandler(int progress, object args);

    public class CompareEnvirment
    {
        //导入文件目录
        //public static  string inputExeclFileDir = Properties.Settings.Default.WorkDir;
        //private static string ResultExcelDir = GlobalEnviroment.ResultOutputDir; //excel file    
        //private static string InputDBDir = GlobalEnviroment.InputDBDir;
        //private static string InputExcelDir = GlobalEnviroment.InputExceltDir;

        //导入数据库目录
        //public String ImportDataDir = GlobalEnviroment.InputDBDir;

        #region 日志处理
        public event CompareEnventHandler CompareInfo;
        private void NotifyCompareInfo(int InfoType, object args)
        {
            CompareInfo?.Invoke(InfoType, args);
        }

        internal void NotifyLogInfo(CompareEventArgs cea)
        {
            NotifyCompareInfo(cea.Progress, CaculateProgress(cea));
        }

        private CompareEventArgs CaculateProgress(CompareEventArgs Args)
        {
            if (Args.Progress > Args.MaxProgress)
                Args.Progress = 100;
            else if (Args.Progress < 0)
                Args.Progress = 0;
            else
                Args.Progress = (int)(Args.Progress * 100.0 / Args.MaxProgress);

            return Args;
        }
        #endregion

        #region 数据库定义
        public DAL.MySqlite MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile, GlobalEnviroment.isCryt);

        //中间数据库
        public DAL.MySqlite ImportSqliteDB = null;
        public String ImportSqliteDBPath;
        //结果数据库
        public DAL.MySqlite ResultSqliteDB = null;
        #endregion

        private string _TimeStampe()
        {
            TimeSpan ts = DateTime.Now - new DateTime(2015, 1, 1);
            return ts.TotalMilliseconds.ToString();
        }
        public string CompareStampe;
        public FileTranser.MTOM.ClassLibrary.WebServicesHelp theWebService
        {
            get
            {
               return GlobalEnviroment.theWebService;
            }
        }
        public CompareEnvirment()
        {
            CompareStampe = _TimeStampe();

            MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile, GlobalEnviroment.isCryt);

            ImportSqliteDBPath = GlobalEnviroment.InputDBDir + CompareStampe + "\\";

            GlobalEnviroment.MakeSureDirectory(GlobalEnviroment.InputDBDir);
            GlobalEnviroment.MakeSureDirectory(GlobalEnviroment.ResultOutputDir);
            GlobalEnviroment.MakeSureDirectory(ImportSqliteDBPath);
        }
    }

    public class DataImporter
    {
        int DataItemIndex = 0;
        int MaxImportNumber = 0;
        CompareEnvirment _Env;  
               
        public DataImporter(CompareEnvirment env)
        {
            _Env = env;
        }

        public bool ImportData(List<DataItem> diList, List<DataFormat> format)
        {
            MaxImportNumber = diList.Count;

            DateTime dt = DateTime.Now;
            Parallel.ForEach(diList,
              new ParallelOptions() { MaxDegreeOfParallelism = GlobalEnviroment.MaxThreadNum + 1 },
              di =>
              {
                  ImportData(di, format.Where(x => x.ParentID == di.RowID && x.Seq >= 0).ToList());
              });
            LogInfo("数据导入全部完成！");

            var t = DateTime.Now - dt;
            LogInfo("总耗时:" + t.TotalSeconds.ToString());

            return false;
        }

        private void ImportData(Models.DataItem di, List<DataFormat> diFormat)
        {
            int TotalFiles = 0;
            int TotalItemData = 0;

            string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";

            DataItemIndex++;
            LogInfo("正在导入:" + di.DataFullName);
            LogDebug("路径：" + dir);

            if (!Directory.Exists(dir))
            {
                LogError(di.DataFullName + "导入失败！路径(" + dir + ")不存在.");
                return;
            }

            DAL.MySqlite _sqliteDB = new DAL.MySqlite(_Env.ImportSqliteDBPath + di.dbTable + ".db", GlobalEnviroment.isCryt);
            LogDebug("数据库:" + _sqliteDB.sqliteConnectionString);
            try
            {

                DirectoryInfo folder = new DirectoryInfo(dir);

                if (!CreateInputDB(_sqliteDB, di))
                {
                    LogError("创建输入数据库" + di.dbTable + "失败!");
                    return;
                }

                var filelist = folder.GetFiles("*.xls");
                foreach (FileInfo file in filelist)
                {
                    LogDebug("读取文件:" + file.Name);
                    int readlines = ReadXLSToDBWithFormatEx(_sqliteDB, file.FullName, di, diFormat);
                    if (readlines > 0)
                        TotalItemData += readlines;
                }
                TotalFiles = filelist.Count();

                filelist = folder.GetFiles("*.csv");
                foreach (FileInfo file in filelist)
                {
                    LogDebug("读取文件:" + file.Name);
                    int readlines = ReadCSVToDBWithFormatEx(_sqliteDB, file.FullName, di, diFormat);
                    if (readlines > 0)
                        TotalItemData += readlines;
                }

                TotalFiles += filelist.Count();

                if (TotalFiles != 0)
                    LogDebug(di.DataFullName + "导入完成， 共导入: " + TotalFiles + " 个文件, 总行数:" + TotalItemData);


                var o =_sqliteDB.ExecuteScalar("Select count() from " + di.dbTable);
                di.TotalNumbers = int.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                _sqliteDB.CloseConnection();
                return;
            }

            
            _sqliteDB.CloseConnection();
        }
        
        private void log(int level, string logMessage)
        {
            CompareEventArgs cea = new CompareEventArgs();
            cea.ArgType = CompareEventEnum.Progress;
            cea.MaxProgress = MaxImportNumber;
            cea.Progress = DataItemIndex;
            cea.ProgressMessage = logMessage;
            cea.Level = level;

            _Env.NotifyLogInfo(cea);
        }

        private void LogDebug(string logMessage)
        {
            log(0, logMessage);
        }
        private void LogInfo(string logMessage)
        {
            log(1, logMessage);
        }
        private void LogWarnning(string logMessage)
        {
            log(2, logMessage);
        }
        private void LogError(string logMessage)
        {
            log(3, logMessage);
        }

        #region 日志Helper

        #endregion

        #region 导入数据库处理
        private bool CreateInputDB(DAL.MySqlite sqliteDB, Models.DataItem di)
        {
            if (di == null || di.dbTable.Length == 0)
                return false;

            string CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTable);

            sqliteDB.ExecuteNonQuery(CreateTable);

            if (di.col1 != null && di.col1.Length != 0)
            {
                CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTablePre);
                sqliteDB.ExecuteNonQuery(CreateTable);
            }
            
            return true;
        }
        private bool CreateInputDB(DAL.MySqlite sqliteDB, List<Models.DataItem> dilist)
        {
            bool bRet = false;

            //DAL.MySqlite _sqliteInput = new DAL.MySqlite();
            //_sqliteInput.sqliteConnectionString = ImportDB;
            try
            {
                sqliteDB.BeginTran();
                foreach (Models.DataItem di in dilist)
                {

                    CreateInputDB(sqliteDB, di);
                    //if (di.dbTable.Length == 0)
                    //    continue;

                    //string CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTable);
                    //sqliteDB.ExecuteNonQuery(CreateTable);

                    //if (di.col1 != null && di.col1.Length != 0)
                    //{
                    //    CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTablePre);
                    //    sqliteDB.ExecuteNonQuery(CreateTable);
                    //}

                }

                // _sqliteInput.ExecuteNonQuery();
                sqliteDB.Commit();

                bRet = true;
            }
            catch (Exception ex)
            {
                // log.Error(ex.Message);
                sqliteDB.RollBack();
            }

            //_sqliteInput.CloseConnection();

            return bRet;
        }

        private int ReadXLSToDBWithFormatEx(DAL.MySqlite SqliteDB, string FileName, Models.DataItem dataItem, List<Models.DataFormat> dataFormat)
        {
            System.Collections.IEnumerator rows = GlobalEnviroment.getExcelFileRows(FileName);
            if (rows == null)
            {
                LogError("文件" + Path.GetFileName(FileName) + "无法打开");
                return -1;
            }

            if (dataItem == null || dataFormat == null || dataFormat.Count == 0)
            {
                LogError("未找到文件格式!");
                return -1;
            }

            SqliteDB.BeginTran();
            int readlines = 0;
            while (rows.MoveNext())
            {
                //the first format is line number
                if (readlines++ < dataFormat[0].colNumber - 1)
                    continue;

                if (FileName.IndexOf("2014.10") != -1)
                {//debug
                    if (readlines == 1957)
                    {

                    }
                }     

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
                            Lines += GlobalEnviroment.GetExcelValue(cell) + "&";                            
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
                    Sql = GenerateInsertSqlWithFormat(cols, dataFormat, pList);

                    //add Item data name  如：城市低保/农村低保等
                    var para = Sql.Split('-');
                    if (para.Length == 2)
                    {
                        para[1] += "@ItemType,";
                        para[0] += "ItemType";

                        pList.Add(new SQLiteParameter("@ItemType", dataItem.DataShortName));
                    }
                    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0], para[1]);
                    SqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                   
                    pList.Clear();
                }
                catch (Exception ex)
                {
                    SqliteDB.RollBack();
                    LogError("插入数据库失败, Sql = " + Sql);
                    LogError("错误信息:" + ex.Message);
                    LogError("文件" + FileName + "\r\n行号:" + readlines.ToString());
                    return -1;
                }
            }
            SqliteDB.Commit();
            readlines = readlines - dataFormat[0].colNumber;
            LogInfo("文件:<" + Path.GetFileName(FileName) + ">, 写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            SqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null");
            return readlines < 0 ? readlines : 0;
        }
        private int ReadCSVToDBWithFormatEx(DAL.MySqlite SqliteDB, string FileName, Models.DataItem dataItem, List<Models.DataFormat> dataFormat)
        {
            StreamReader mysr;
            try
            {
                mysr = new StreamReader(FileName, System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                LogError("读取CSV文件错误:" + FileName);
                LogError(ex.Message);
                return -1;
            }

            SqliteDB.BeginTran();
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

                    
                    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0], para[1]);

                    SqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                    pList.Clear();
                }
                catch (Exception ex)
                {
                    SqliteDB.RollBack();
                    LogError("插入数据库失败, Sql = " + Sql);
                    LogError("错误信息:" + ex.Message);
                    LogError("错误行数:" + readlines);
                    return -1;
                }

            }
            SqliteDB.Commit();

            readlines = readlines - dataFormat[0].colNumber;
            LogInfo("文件" + Path.GetFileName(FileName) + "写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            SqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null");
            return readlines < 0 ? readlines : 0;
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
        #endregion
    }

    public class DataCompare
    {
        private CompareEnvirment _Env;
        private List<Models.DataItem> _DataItemList;
        public DataCompare(CompareEnvirment env, List<Models.DataItem> diList)
        {
            _Env = env;
            _DataItemList = diList;
        }

        internal void PreProcessData(List<CompareAim> aims)
        {
            


        }

        public void StartCompare(List<CompareAim> aims)
        {
            var parallelAims = GroupAimForParallel(aims);

            
        }

        public void CompareByAim()
        {

        }

        public void CompareByAim(CompareAim aim)
        {
            try
            {//结果 -->新数据库中
                _Env.ImportSqliteDB.ExecuteNonQuery("create table  " + aim.TableName + " as " + GetParalleAim(aim));

                //log.Info("成功获取 " + a.AimName + " 的比对结果!");
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + "AimName:" + a.AimName + " aim:" + a.Rules);
            }
        }

        private string GetParalleAim(CompareAim aim)
        {
            String Sql = aim.Rules;

            var di = _DataItemList.FirstOrDefault(x => x.RowID == aim.t1);
            if (di != null)
                Sql = Sql.Replace(di.dbTable, di.dbTable + "." + di.dbTable);

            di = _DataItemList.FirstOrDefault(x => x.RowID == aim.t2);
            if (di != null)
                Sql = Sql.Replace(di.dbTable, di.dbTable + "." + di.dbTable);

            di = _DataItemList.FirstOrDefault(x => x.RowID == aim.t3);
            if (di != null)
                Sql = Sql.Replace(di.dbTable, di.dbTable + "." + di.dbTable);

            return Sql;
        }

        private List<List<CompareAim>> GroupAimForParallel(List<CompareAim> aims)
        {
            List<List<CompareAim>> groupAims = new List<List<CompareAim>>();
            
            foreach( var a in aims)
            {
                bool grouped = false;
                foreach(var ga in groupAims)
                {
                    if (ga.Where(x => IsSourceEqual(x, a)) != null)
                    {
                        ga.Add(a);
                        grouped = true;
                        break;
                    }
                }
                if(!grouped)
                {
                    List<CompareAim> ca = new List<CompareAim>();
                    ca.Add(a);
                    groupAims.Add(ca);
                }
            }

            return groupAims;
        }

        private bool IsSourceEqual(CompareAim aim1, CompareAim aim2)
        {
            List<int> di = new List<int>();

            if (aim1.t1 != 0)
                if (aim1.t1 == aim2.t1 || aim1.t1 == aim2.t2 || aim1.t1 == aim2.t2)
                    return true;

            if (aim1.t2 != 0)
                if (aim1.t2 == aim2.t1 || aim1.t2 == aim2.t2 || aim1.t2 == aim2.t2)
                    return true;

            if (aim1.t3 != 0)
                if (aim1.t3 == aim2.t1 || aim1.t3 == aim2.t2 || aim1.t3 == aim2.t2)
                    return true;

            return false;
        }
    }

    public class ReportGenerator
    {
        CompareEnvirment _Env;
        public ReportGenerator(CompareEnvirment env)
        {
            _Env = env;
        }


        public bool GetReportfile()
        {
             

            return false;
        }
    }
    
    public class QuickComparer
    {
        CompareEnvirment Env = new CompareEnvirment();
        public event CompareEnventHandler CompareInfo;
        int CurrentStep = 0;


        public void QuickStart(Models.RulesTypes tmpType)
        {            
            Env.CompareInfo += CompareInfo;

            if (tmpType == RulesTypes.Compare)
                LogInfo("成功完成数据校验!");
            else LogInfo("成功完成比对!");

            Business.DataMgr dmg = new DataMgr(Env.MainSqliteDB);
            var aims = dmg.GetCompareAllAim(tmpType);
            var list = dmg.GetDataItemByAim(aims);
            var format = dmg.GetDataFormatList();

            CurrentStep++;
            LogStep("导入数据>>>>>>>>>>>>>>>");
            DataImporter dImp = new DataImporter(Env);
            dImp.ImportData(list, format);
            //var str = String.Join("\r\n", list.Select(x => x.DataShortName + "," + x.TotalNumbers));
            LogStep("导入数据完成");           

            CurrentStep++;
            LogStep("数据预处理>>>>>>>>>>>>>>>");
            DataCompare dcp = new DataCompare(Env, list);
            dcp.PreProcessData(aims);
            LogStep("预处理完成");


            CurrentStep++;
            LogStep("正在比对>>>>>>>>>>>>>>>");


            LogStep("比对完成");


            CurrentStep++;
            LogStep("正在生成比对结果>>>>>>>>>>>>>>>");            
            ReportGenerator rg = new ReportGenerator(Env);
            rg.GetReportfile();


            if (tmpType == RulesTypes.Compare)
                LogInfo("成功完成数据校验!");
            else LogInfo("成功完成比对!"); 
        }


        protected void LogInfo(String logMessage)
        {
            CompareEventArgs cea = new CompareEventArgs();
            cea.ArgType = CompareEventEnum.Info;
            cea.Progress = 0;
            cea.ProgressMessage = logMessage;

            Env.NotifyLogInfo(cea);
        }
        protected void LogStep(String logMessage)
        {
            CompareEventArgs cea = new CompareEventArgs();
            cea.ArgType = CompareEventEnum.Step;
            cea.Progress = CurrentStep;
            cea.ProgressMessage = logMessage;
            cea.MaxProgress = 6;
            cea.Level = 1;

            Env.NotifyLogInfo(cea);
        }
    }
}
