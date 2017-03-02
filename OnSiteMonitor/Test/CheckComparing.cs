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

namespace OnSiteFundComparer.Test
{      
    public class TestResult
    {
        public String RowID;
        public String ResultNumber;
        public String Time;
    }


    class TestComparing
    {
        public bool CompareDirectly = false;
        public bool IsNeedCompare = true;
        public List<CompareAim> compareAims = new List<CompareAim>();
        public List<TestResult> testResults = new List<TestResult>();

        private string InputDB = GlobalEnviroment.InputDBDir;
        private string InputExcelDir = GlobalEnviroment.InputExceltDir;

        private void InitDB()
        {
             
            if (!CompareDirectly)
            {
                InputDB = NewTestDB();
            }
            else
            {
                InputDB = GetTestDB();
            }

            
        }

        public string NewTestDB()
        {
            TimeSpan ts = DateTime.Now - new DateTime(2015, 1, 1);
            CompareDirectly = false;

            return GlobalEnviroment.InputDBDir +  "test." + ts.TotalMilliseconds.ToString() + ".db";
        }

        public string GetTestDB()
        {
            String dbFileInfo = "";
            DirectoryInfo TheFolder = new DirectoryInfo(GlobalEnviroment.InputDBDir);
            if (TheFolder.GetFiles().Count() != 0)
            {
                var filename = TheFolder.GetFiles()
                    .Where(x => ((x.FullName.IndexOf("result") == -1) && (x.FullName.IndexOf("Check") == -1)))
                    .OrderByDescending(n => n.LastWriteTime).First();

                if (filename != null)
                {
                    dbFileInfo = filename.FullName;
                }
                else dbFileInfo = NewTestDB();
            }
            return dbFileInfo;
        }


        private DAL.MySqlite MainSqliteDB = null;
        private DAL.MySqlite ImportSqliteDB = null;

        public delegate void reportProgress(int n, object info);
        public reportProgress log; 
        


        public TestComparing()
        {
            GlobalEnviroment.MakeSureDirectory(InputDB);
            
                InitDB();
        }

        private void Log(String Info)
        {
            log?.Invoke(0, Info);
        }
        private void Log(int progress, String Info)
        {
            log?.Invoke(progress, Info);
        }

        private void ReportProgress(int progress)
        {
            log(progress, "");
        }

        internal string Start ()
        {
            MainSqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            ImportSqliteDB = new DAL.MySqlite(InputDB, GlobalEnviroment.isCryt);

            Business.DataMgr dmgr = new Business.DataMgr();
            var complist = dmgr.GetChildDataItemList(3);
            var sourcelist = dmgr.GetChildAllList(2);
            var formatslist = dmgr.GetAllDataFormatList();

            if (!CompareDirectly)
            {
                Log("-创建输入数据库.");
                if (!CreateInputDB(complist))
                    return "";

                if (!CreateInputDB(sourcelist))
                    return "";

                Log("输入数据库创建成功!");
                Log(5,"开始导入比对数据Excel数据文件");
                if (!ImportCompExcel(complist))
                    return "";

                Log(10, "开始调整清理比对数据数据");
                if (!CleanUpData())
                {
                    Log("调整清理比对数据数据出错！");
                    return "";
                }


                Log(15, "开始导入源数据Excel数据文件");
                if (!ImportSourceExcelEx(sourcelist, formatslist, compareAims))
                    return "";
                

                Log(20, "-输入数据库创建成功!");
                Log("----------------------------------------------------------\r\n\r\n");

                Log("==============开始对数据进行预处理===============");
                PreProcessData(complist);
                PreProcessData(sourcelist);
                Log(25, "==============数据预处理完成==============");
            }

            if (IsNeedCompare)
                GetResult(sourcelist, compareAims);
            else Log(100, "完成导入");
            



            MainSqliteDB.CloseConnection();
            ImportSqliteDB.CloseConnection();
           

            MainSqliteDB = null;
            ImportSqliteDB = null;

            return InputDB;
            
        }

        private void GetResult(List<DataItem> sourcelist, List<CompareAim> Aims)
        {
            //DataMgr dm = new DataMgr();
            // List<CollisionAim> aims = dm.GetAllAim();
            var aims = Aims;// new Service.CompareAimService(OnSiteFundComparer.GlobalEnviroment.MainDBFile).GetCompareAim();
            if (aims == null) return;

            int TotalAims = aims.Count();
            int i = 0;
            foreach (var a in aims)
            {
                
                if (a == null || a.Rules == "")
                    continue;

                DataItem di = sourcelist.Find(x => x.RowID == a.SourceID);
                if (di == null)
                    continue;

                TestResult tr = new TestResult();

                
                try
                {//结果 -->新数据库中
                    DateTime Start = DateTime.Now;
                    ImportSqliteDB.ExecuteNonQuery("create table  " + a.TableName + " as " + a.Rules);
                    var o = ImportSqliteDB.ExecuteScalar(" select count() from (" + a.Rules2 + ")");

                    TimeSpan ts = DateTime.Now - Start;

                    tr.RowID = a.RowID.ToString();
                    tr.ResultNumber = o.ToString();
                    tr.Time = ts.TotalMilliseconds.ToString();

                    testResults.Add(tr);

                    var p =  (25 + (i++*80 / TotalAims));
                    Log(p, "成功获取 " + a.AimName + " 的比对结果!");
                    // log.Info("成功获取 " + a.AimName + " 的比对结果!");
                }
                catch (Exception ex)
                {
                    Log(100, ex.Message + "AimName:" + a.AimName + " aim:" + a.Rules);
                }
            }

             
            
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
                        foreach (string sql in colSql)
                        {
                            ImportSqliteDB.ExecuteDataset(sql);
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Info("$$$$$$$$$$$预处理数据错误:" + di.DataFullName + "\r\n");
                        //log.Info("col1 = " + di.col1 + "\r\n");
                       // log.Info("$$$$$$$$$$$异常信息:\r\n" + ex.Message + "\r\n");

                        ImportSqliteDB.RollBack();
                        return;
                    }

            }
            ImportSqliteDB.Commit();
        }

        internal bool CreateInputDB(List<Models.DataItem> complist)
        {
            bool bRet = false;

            try
            {
                ImportSqliteDB.BeginTran();
                foreach (Models.DataItem di in complist)
                {
                    if (di.dbTable.Length == 0)
                        continue;

                    string CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTable);
                    ImportSqliteDB.ExecuteNonQuery(CreateTable);

                    if (di.col1.Length != 0)
                    {
                        CreateTable = Properties.Settings.Default.ReferTableSql.Replace("refertable", di.dbTablePre);
                        ImportSqliteDB.ExecuteNonQuery(CreateTable);
                    }

                }


                ImportSqliteDB.Commit();

                bRet = true;
            }
            catch (Exception ex)
            {
                log(100, ex.Message);
                ImportSqliteDB.RollBack();
            }
            return bRet;
        }


        internal bool ImportCompExcel(List<Models.DataItem> complist)
        {
            Business.DataMgr dmg = new Business.DataMgr();
            bool bRet = false;
            int totalData = 0;

            foreach (Models.DataItem di in complist)
            {
                string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
                Log("导入:" + di.DataFullName + ", 路径:" + dir);

                if (!Directory.Exists(dir))
                    continue;

                int totalItemData = 0;
                DirectoryInfo folder = new DirectoryInfo(dir);
                
                try
                {
                    var filelist = folder.GetFiles("*.xls");
                    foreach (FileInfo file in filelist)
                    {
                        Log("读取文件:" +　file.Name);
                        int readlines = ReadXLSToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                        // ReadXLSToDBWithFormat(file.FullName, di, dmg.GetDataFormatList(di));
                    }
                    if (filelist.Count() != 0)
                        Log("共导入: " + filelist.Count() + " 个.xls文件");

                    filelist = folder.GetFiles("*.csv");
                    foreach (FileInfo file in filelist)
                    {
                        Log("读取文件:" + file.Name);
                        int readlines = ReadCSVToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                    }
                    if (filelist.Count() != 0)
                        Log("共导入: " + filelist.Count() + " .csv个文件");

                    bRet = true;
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    bRet = false;
                }

                Log(di.DataFullName + " 共有: " + totalItemData.ToString() + "条数据插入数据库");
                totalData += totalItemData;
            }
            return bRet;
        }
        public bool ImportSourceExcelEx(List<Models.DataItem> sourcelist, List<Models.DataFormat> formats, List<CompareAim> aList)
        {
            Business.DataMgr dmg = new Business.DataMgr();
            bool bRet = false;
            int totalData = 0;

            //var ss = from s in sourcelist join  a in aList 
            //         on s.RowID equals a.SourceID
            //         select s;

            foreach (Models.DataItem di in sourcelist)
            {
                string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
                if (!Directory.Exists(dir))
                    continue;

                Log("读入" + di.DataFullName + "的数据文件");
                Log("路径:" + dir);
                int totalItemData = 0;
                DirectoryInfo folder = new DirectoryInfo(dir);
                try
                {


                    var filelist = folder.GetFiles("*.xls");
                    foreach (FileInfo file in filelist)
                    {
                        Log("读取文件:" +　file.Name);
                        int readlines = ReadXLSToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                        // ReadXLSToDBWithFormat(file.FullName, di, dmg.GetDataFormatList(di));
                    }
                    if (filelist.Count() != 0)
                        Log("共导入: " + filelist.Count() + " 个.xls文件");

                    filelist = folder.GetFiles("*.csv");
                    foreach (FileInfo file in filelist)
                    {
                        Log("读取文件:" + file.Name);
                        int readlines = ReadCSVToDBWithFormatEx(file.FullName, di, dmg.GetDataFormatList(di));
                        if (readlines > 0)
                            totalItemData += readlines;
                    }
                    if (filelist.Count() != 0)
                        Log("共导入: " + filelist.Count() + " .csv个文件");

                    bRet = true;
                }
                catch (Exception ex)
                {
                   // log.Error(ex.Message);
                    bRet = false;
                }
                totalData += totalItemData;
            }
            return bRet;
        }

        internal bool CleanUpData()
        {
            bool bRet = true;
     //       log.Info("清除重复数据");

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

           //         log.Info("成功完成：" + a.CheckName + "!");
                    bRet = true;
                }
                catch (Exception ex)
                {
         //           log.Error(ex.Message + " 清库:" + a.CheckSql);
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
             //   log.Error("文件" + Path.GetFileName(FileName) + "无法打开");
                return -1;
            }

            if (dataItem == null || dataFormat == null || dataFormat.Count == 0)
            {
            //    log.Error("未找到文件格式!");
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
                  //  log.Info("插入数据库失败, Sql = " + Sql);
                  //  log.Info("错误信息:" + ex.Message);
                  //  log.Info("文件" + FileName + "\r\n行号:" + readlines.ToString());
                    return -1;
                }
            }
            ImportSqliteDB.Commit();






            readlines = readlines - dataFormat[0].colNumber;
            //log.Info("文件:<" + Path.GetFileName(FileName) + ">, 写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            ImportSqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null");

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
              //  log.Error("读取CSV文件错误:" + FileName);
              //  log.Error(ex.Message);
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
                //    log.Info("插入数据库失败, Sql = " + Sql);
                 //   log.Info("错误信息:" + ex.Message);
                    return -1;
                }

            }
            ImportSqliteDB.Commit();

            readlines = readlines - dataFormat[0].colNumber;
          //  log.Info("文件" + Path.GetFileName(FileName) + "写入数据库成功, 共 " + readlines.ToString() + " 条记录!");

            ImportSqliteDB.ExecuteNonQuery(@"delete from " + dataItem.dbTable + @" where length(id)=0 and name is null");
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

  
}
