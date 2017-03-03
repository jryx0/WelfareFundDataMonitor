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

namespace OnSiteFundComparer.Business
{
    public class CompareEnvirment
    {
        //导入文件目录
        //public static  string inputExeclFileDir = Properties.Settings.Default.WorkDir;
        //private static string ResultExcelDir = GlobalEnviroment.ResultOutputDir; //excel file    
        //private static string InputDBDir = GlobalEnviroment.InputDBDir;
        //private static string InputExcelDir = GlobalEnviroment.InputExceltDir;

        //导入数据库目录
        //public String ImportDataDir = GlobalEnviroment.InputDBDir;

        private string _TimeStampe()
        {             
                TimeSpan ts = DateTime.Now - new DateTime(2015, 1, 1);
                return ts.TotalMilliseconds.ToString();
        }

        public string CompareStampe;

        public log4net.ILog Log
        {
            get
            {
                return GlobalEnviroment.log;
            }
        }

        //配置数据库
        public DAL.MySqlite MainSqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" + Properties.Settings.Default.MainDBFile, GlobalEnviroment.isCryt);

        //中间数据库
        public DAL.MySqlite ImportSqliteDB = null;
        public String ImportSqliteDBPath;
        //结果数据库
        public DAL.MySqlite ResultSqliteDB = null;

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
        CompareEnvirment _Env;
         
        public DataImporter(CompareEnvirment env)
        {
            _Env = env;
        }

        public bool ImportData(List<DataItem> diList, List<DataFormat> format)
        {   
            Parallel.ForEach(diList,
              new ParallelOptions() { MaxDegreeOfParallelism =  GlobalEnviroment.MaxThreadNum },
              di => {
                  ImportData(di, format.Where(x => x.ParentID == di.RowID && x.Seq >= 0).ToList());
              });
            return false;
        }

        private void ImportData(Models.DataItem di, List<DataFormat> diFormat)
        {
            bool bRet = false;
            string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
            //log.Info("导入:" + di.DataFullName + ", 路径:" + dir);
            if (!Directory.Exists(dir))
                return;

            DAL.MySqlite _sqliteDB = new DAL.MySqlite(_Env.ImportSqliteDBPath + di.dbTable + ".db", GlobalEnviroment.isCryt);

           

            int totalItemData = 0;
            DirectoryInfo folder = new DirectoryInfo(dir);
            try
            {
                if (!CreateInputDB(_sqliteDB, di))
                    return;

                var filelist = folder.GetFiles("*.xls");
                foreach (FileInfo file in filelist)
                {
                    //log.Info("读取文件:" +　file.Name);
                    int readlines = ReadXLSToDBWithFormatEx(_sqliteDB, file.FullName, di, diFormat);
                    if (readlines > 0)
                        totalItemData += readlines;
                    // ReadXLSToDBWithFormat(file.FullName, di, dmg.GetDataFormatList(di));
                }

                //if (filelist.Count() != 0)
                //    log.Info("共导入: " + filelist.Count() + " 个.xls文件");

                filelist = folder.GetFiles("*.csv");
                foreach (FileInfo file in filelist)
                {
                    //log.Info("读取文件:" + file.Name);
                    int readlines = ReadCSVToDBWithFormatEx(_sqliteDB, file.FullName, di, diFormat);
                    if (readlines > 0)
                        totalItemData += readlines;
                }
                //if (filelist.Count() != 0)
                //    log.Info("共导入: " + filelist.Count() + " .csv个文件");

                bRet = true;
            }
            catch (Exception ex)
            {
                // log.Error(ex.Message);
                _sqliteDB.CloseConnection();
                bRet = false;
                return;
            }

            _sqliteDB.CloseConnection();

        }

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
             //   log.Error("文件" + Path.GetFileName(FileName) + "无法打开");
                return -1;
            }

            if (dataItem == null || dataFormat == null || dataFormat.Count == 0)
            {
              //  log.Error("未找到文件格式!");
                return -1;
            }

            //double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0;


            SqliteDB.BeginTran();
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
                    SqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                    //System.DateTime edt3 = System.DateTime.Now;
                    //t3 += (edt3 - sdt3).TotalMilliseconds;



                    pList.Clear();
                }
                catch (Exception ex)
                {
                    SqliteDB.RollBack();
                   // log.Info("插入数据库失败, Sql = " + Sql);
                   // log.Info("错误信息:" + ex.Message);
                   // log.Info("文件" + FileName + "\r\n行号:" + readlines.ToString());
                    return -1;
                }
            }
            SqliteDB.Commit();






            readlines = readlines - dataFormat[0].colNumber;
            //  log.Info("文件:<" + Path.GetFileName(FileName) + ">, 写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

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
               // log.Error("读取CSV文件错误:" + FileName);
               // log.Error(ex.Message);
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

                    //if (dataItem.parentItem != null && dataItem.parentItem.DataType != Models.FundItemTypes.SourceItems)
                    //{//has comment
                    //    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0] + "Type,", para[1] + "@Type");
                    //    pList.Add(new SQLiteParameter("@Type", dataItem.DataShortName));
                    //}
                    //else
                    Sql = GlobalEnviroment.GetInsertSql(dataItem.dbTable, para[0], para[1]);

                    SqliteDB.ExecuteNonQuery(CommandType.Text, Sql, pList.ToArray());
                    pList.Clear();
                }
                catch (Exception ex)
                {
                    SqliteDB.RollBack();
                   // log.Info("插入数据库失败, Sql = " + Sql);
                   // log.Info("错误信息:" + ex.Message);
                    return -1;
                }

            }
            SqliteDB.Commit();

            readlines = readlines - dataFormat[0].colNumber;
            //log.Info("文件" + Path.GetFileName(FileName) + "写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            SqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null");
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
    }

    public class DataAnalyer
    {

        CompareEnvirment _Env;
        public DataAnalyer(CompareEnvirment env)
        {
            _Env = env;
        }

        public bool Analyer()
        {
            

            return false;
        }

        public bool Analyer(List<Models.CompareAim> aims)
        {


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
        public delegate void Log(object target, int progress, string Message);  

        public void Comparer(Models.RulesTypes tmpType)
        {
            CompareEnvirment Env = new CompareEnvirment();

            Business.DataMgr dmg = new DataMgr(Env.MainSqliteDB);
            var aims = dmg.GetCompareAllAim(tmpType);
            var list = dmg.GetDataItemByAim(aims);
            var format = dmg.GetDataFormatList();

            DataImporter dImp = new DataImporter(Env);
            dImp.ImportData(list, format);


            //DataAnalyer dA = new DataAnalyer(Env);
            //dA.Analyer(list);

            ReportGenerator rg = new ReportGenerator(Env);
            rg.GetReportfile();
        }

        
    }
}
