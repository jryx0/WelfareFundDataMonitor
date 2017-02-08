using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace WFMRegionCluesReport
{

    public class CluesReport
    {
        public String Username { set; get; }
        public String RegionName { set; get; }

        public String ParentName { set; get; }

        public int TotalClues { set; get; }
        public int InputError { set; get; }
        public int Clues { set; get; }

        public DateTime UploadDate { set; get; }
        public string TaskName { set; get; }

        public int Status { set; get; }
    }

    public class CluesRegion
    {
        public string ParentName { get; set; }
        public string RegionName { get; set; }
        public string UserName { get; set; }
        public string RegionCode { get; set; }
        public string Confirmed { get; set; }
        public string IsTrue { get; set; }

    }

    public class ExecSqlPO : Erik.Utilities.Bases.BaseProgressiveOperation
    {
        public DataSet ResultDS = null;
        IEnumerable<CluesRegion> regionList;
        DAL.MyDataBase dbServer = null;
        String _sql;
        public ExecSqlPO(DAL.MyDataBase sqlserver, IEnumerable<CluesRegion> rList, String Sql)
        {
            if (rList == null || rList.ToList().Count == 0 || Sql == null || sqlserver == null)
                throw new Exception("ExecSqlPO ， rList == null || rList.Count == 0 || Sql == null||dbServer == null");

            regionList = rList.ToList();
            dbServer = sqlserver;
            _sql = Sql.ToLower().Trim();

            _totalSteps = regionList.Count();
            _currentStep = 0;

            MainTitle = "执行Sql";
        }

        public override void Start()
        {
            _currentStep = 0;
            OnOperationStart(EventArgs.Empty);

            foreach (var r in regionList)
            {
                _currentStep++;
                OnOperationStart(EventArgs.Empty);



                String tablename = "ClueData_" + r.RegionCode;
                if (!TableExist(dbServer, tablename))
                    continue;

                if (_sql.StartsWith("select"))
                try
                {        
                    String newSql = _sql.Insert(_sql.IndexOf("select ") + 7, " '"+r.ParentName+r.RegionName + "' as regionName, ");

                    if (ResultDS == null)
                        ResultDS = dbServer.ExecuteDataset(newSql.Replace("@table", tablename));
                    else
                    {
                        var ds = dbServer.ExecuteDataset(newSql.Replace("@table", tablename));
                        object[] obj = new object[ds.Tables[0].Columns.Count];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i].ItemArray.CopyTo(obj, 0);
                            ResultDS.Tables[0].Rows.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    ResultDS = null;
                    OnOperationEnd(EventArgs.Empty);
                    break;
                }
                else
                    try
                    {
                        dbServer.ExecuteNonQuery(_sql);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        ResultDS = null;
                        OnOperationEnd(EventArgs.Empty);
                        break;
                    }
            }
            OnOperationEnd(EventArgs.Empty);
        }

        private bool TableExist(DAL.MyDataBase sqlserver, string tableName)
        {
            String Sql = @"select * from sys.tables t join sys.schemas s on (t.schema_id = s.schema_id) 
              where s.name = 'dbo' and t.name = '" + tableName + "'";

            bool bRet = false;
            try
            {
                var o = sqlserver.ExecuteScalar(Sql);
                //drop table
                bRet = (o != null);
            }
            catch
            {

            }
            return bRet;

        }
    }


    public class ReadUploadFilePO : Erik.Utilities.Bases.BaseProgressiveOperation
    {
        public List<CluesReport> crList = new List<CluesReport>();

        List<WFM.JW.HB.Models.WFMUploadTask> tList;
        public ReadUploadFilePO(List<WFM.JW.HB.Models.WFMUploadTask> l)
        {
            if (l == null)
                throw new Exception("ReadUploadFilePO ， List<WFM.JW.HB.Models.WFMUploadTask == null");
            tList = l;

            _totalSteps = l.Count();
            _currentStep = 0;

            MainTitle = "读入上传文件";
        }

        public override void Start()
        {
            _currentStep = 0;
            OnOperationStart(EventArgs.Empty);


            foreach (var t in tList)
            {
                OnOperationProgress(EventArgs.Empty);
                if (!File.Exists(t.FilePath + "\\" + t.FileName))
                    continue;

                var dbFile = DepressData(t.FilePath, t.FileName);
                using (
                DAL.MySqlite result = new DAL.MySqlite(dbFile))
                {
                    try
                    {
                        var ds = result.ExecuteDataset(@"SELECT count() AS totalclues,
                                                               sum(inputerror) AS inputerror,
                                                               sum(CASE WHEN inputerror = 1 THEN 0 ELSE 1 END) AS clues
                                                          FROM clue_report");

                        if (ds == null || ds.Tables[0].Rows.Count == 0)
                            continue;

                        var crl = from dr in ds.Tables[0].AsEnumerable()
                                  select new CluesReport
                                  {
                                      TotalClues = (int)dr.Field<Int64>("totalclues"),
                                      InputError = (int)dr.Field<Int64>("inputerror"),
                                      Clues = (int)dr.Field<Int64>("clues")
                                  };

                        CluesReport cr = crl.First();

                        cr.Username = t.UserName;

                        cr.RegionName = t.RegionName;
                        cr.UploadDate = t.CreateTime;
                        cr.TaskName = t.ClientTaskName;
                        cr.ParentName = t.ParentName;
                        cr.Status = t.Status;

                        crList.Add(cr);

                    }
                    catch (Exception ex)
                    {
                    }

                    result.CloseConnection();
                }
                _currentStep++;
                File.Delete(dbFile);
            }
            OnOperationEnd(EventArgs.Empty);
        }


        private string DepressData(string path, string compressfile)
        {
            //if (!File.Exists(compressfile)) throw new Exception(compressfile + "*文件" + compressfile + "不存在！");

            string outputFileName = "temp\\" + compressfile.Substring(0, compressfile.Length - 7);
            try
            {
                if (File.Exists(outputFileName))
                    File.Delete(outputFileName);


                FileStream inputStream = new FileStream(path + "\\" + compressfile, FileMode.Open, FileAccess.Read);
                FileStream outputStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);

                int bufferSize = 8192;
                int bytesRead = 0;
                byte[] buffer = new byte[bufferSize];
                GZipStream decompressionStream =
                    new GZipStream(inputStream, CompressionMode.Decompress);
                // 把压缩了的数据通过GZipStream解压缩后再读出来
                // 读出来的数据就存在数组里
                while ((bytesRead = decompressionStream.Read(buffer, 0, bufferSize)) > 0)
                {
                    // 把解压后的数据写入到输出数据流
                    outputStream.Write(buffer, 0, bytesRead);
                }
                decompressionStream.Close();

                inputStream.Close();
                outputStream.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(compressfile + "*解压文件" + outputFileName + "失败！" + ex.Message);
            }


            return outputFileName;
        }
    }
}
