using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Service
{
    public class CompareAimService : IDisposable
    {
        private DAL.MySqlite _sqliteDB;

        public CompareAimService(string connStr)
        {
            _sqliteDB = new DAL.MySqlite(connStr);
        }
        
        public List<Models.DataItem> GetChildDataItemList()
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(
                OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            var fundList = diss.GetDisplayDataItems();
            diss.Close();

            return fundList;
        }

        public DataSet GetCompareAimsDS()
        {
            DataSet ds = null;
            String Sql = @"SELECT CompareAim.rowid,
                                   CompareAim.sourceid,
                                   CompareAim.aimname,
                                   CompareAim.aimdesc,
                                   CompareAim.tablename,
                                   RulesTmp.Rules,
                                   RulesTmp.Rule2,
                                   RulesTmp.Rule3,
                                   CompareAim.t1,
                                   CompareAim.t2,
                                   CompareAim.t3,
                                   CompareAim.conditions,
                                   CompareAim.seq,
                                   RulesTmp.Type
                              FROM CompareAim,
                                   RulesTmp,
                                   DataItem
                             WHERE CompareAim.tmp = RulesTmp.RowID AND 
                                   CompareAim.status = 1 AND 
                                   DataItem.status = 1 AND 
                                   DataItem.RowID = CompareAim.SourceID
                             ORDER BY CompareAim.seq";


            try
            {
                ds = _sqliteDB.ExecuteDataset(Sql);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return ds;
        }


        public List<Models.CompareAim> GetCompareAllAim()
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(
          OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            var fundList = diss.GetDisplayDataItems();

            DataSet ds = GetCompareAimsDS();
            List<Models.CompareAim> cList = new List<Models.CompareAim>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Models.CompareAim cl = new Models.CompareAim();

                cl.RowID = int.Parse(dr[0].ToString());
                cl.SourceID = int.Parse(dr[1].ToString());

                cl.AimName = dr[2].ToString();
                cl.AimDesc = dr[3].ToString();
                cl.TableName = dr[4].ToString();

                cl.Rules = dr[5].ToString();
                cl.Rules2 = dr[6].ToString();
                cl.Rules3 = dr[7].ToString();

                cl.t1 = int.Parse(dr[8].ToString());
                cl.t2 = int.Parse(dr[9].ToString());
                cl.t3 = int.Parse(dr[10].ToString());

                //int para = 0;
                //int.TryParse(dr[11].ToString(), out para);
                String para = dr[11].ToString();

                //int para = int.Parse(dr[11].ToString());

                var di1 = fundList.Find(x => x.RowID == cl.t1);
                var di2 = fundList.Find(x => x.RowID == cl.t2);
                var di3 = fundList.Find(x => x.RowID == cl.t3);

                cl.Rules = ReplaceAll(cl.Rules, cl.AimName, cl.TableName, para, di1, di2, di3);
                cl.Rules2 = ReplaceAll(cl.Rules2, cl.AimName, cl.TableName, para, di1, di2, di3);
                cl.Rules3 = ReplaceAll(cl.Rules3, cl.AimName, cl.TableName, para, di1, di2, di3);

                cl.seq = int.Parse(dr[12].ToString());
                cl.Type = int.Parse(dr[13].ToString());
                cList.Add(cl);
            }

            return cList;
        }
        /// <summary>
        /// 校验aim
        /// </summary>
        /// <returns></returns>
        public List<Models.CompareAim> GetDataCheckAim()
        {
            var aList = GetCompareAllAim();
            return aList.Where(x => x.Type > 1000).ToList();
        }
        /// <summary>
        /// 重复数据删除aim
        /// </summary>
        /// <returns></returns>
        public List<Models.CompareAim> GetDataAim()
        {
            var aList = GetCompareAllAim();
            return aList.Where(x => x.Type > 2000).ToList();
        }
        /// <summary>
        /// 比对aim
        /// </summary>
        /// <returns></returns>
        public List<Models.CompareAim> GetCompareAim()
        {
            var aList = GetCompareAllAim();
            return aList.Where(x => x.Type < 1000).ToList();
        }

        private string ReplaceAll(string rule, string type, string tablename, string para, Models.DataItem di1, Models.DataItem di2, Models.DataItem di3)
        //private string ReplaceAll(string rule, string type, string tablename, int para, Models.DataItem di1, Models.DataItem di2, Models.DataItem di3)
        {
            rule = rule.Replace("@table1", di1.dbTable);
            rule = rule.Replace("@tablepre1", di1.dbTablePre);
            rule = rule.Replace("@table2", di2.dbTable);
            rule = rule.Replace("@tablepre2", di2.dbTablePre);

            rule = rule.Replace("@aimtype", type);
            rule = rule.Replace("@tablename", tablename);


            rule = rule.Replace("@t1p", di1.people);
            rule = rule.Replace("@t1s", di1.DataShortName);
            rule = rule.Replace("@t1f", di1.DataFullName);

            rule = rule.Replace("@t2p", di2.people);
            rule = rule.Replace("@t2s", di2.DataShortName);
            rule = rule.Replace("@t2f", di2.DataFullName);

            if (di3 != null)
            {
                rule = rule.Replace("@table3", di3.dbTable);
                rule = rule.Replace("@tablepre3", di3.dbTablePre);

                rule = rule.Replace("@t3p", di3.people);
                rule = rule.Replace("@t3s", di3.DataShortName);
                rule = rule.Replace("@t3f", di3.DataFullName);
            }

            rule = rule.Replace("@para", para.ToString());


            //rule = rule.Replace("@table1", di1.dbTable);
            //rule = rule.Replace("@table2", di2.dbTable);
            //rule = rule.Replace("@table3", di3.dbTable);
            //rule = rule.Replace("@aimtype", type);
            //rule = rule.Replace("@tablename", tablename);




            //rule = rule.Replace("@t1p", di1.people);
            //rule = rule.Replace("@t1s", di1.DataShortName);
            //rule = rule.Replace("@t1f", di1.DataFullName);

            //rule = rule.Replace("@t2p", di2.people);
            //rule = rule.Replace("@t2s", di2.DataShortName);
            //rule = rule.Replace("@t2f", di2.DataFullName);


            //rule = rule.Replace("@t3p", di3.people);
            //rule = rule.Replace("@t3s", di3.DataShortName);
            //rule = rule.Replace("@t3f", di3.DataFullName);

            //rule = rule.Replace("@para", para.ToString());


            return rule;
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _sqliteDB.CloseConnection();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }





        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DataItemStuctServices() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
