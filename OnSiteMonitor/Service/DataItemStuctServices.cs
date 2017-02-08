using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;


namespace OnSiteFundComparer.Service
{
    class DataItemStuctServices: IDisposable
    {
        private List<Models.DataItem> _dataItems;
        private DAL.MySqlite _sqliteDB;
        public DataItemStuctServices(string connStr)
        {
            _dataItems = new List<Models.DataItem>();
            _sqliteDB = new DAL.MySqlite();

            _sqliteDB.sqliteConnectionString = connStr;
        }

        public List<Models.DataItem>  GetDisplayDataItems( )
        {
            // return _sqliteDB.ExecuteToList<Models.DataItem>(Models.DataItem.SelectSql());

            List<Models.DataItem> fiList = new List<Models.DataItem>();


            SQLiteDataReader reader = _sqliteDB.ExecuteReader(Models.DataItem.SelectSql());
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //reader.FieldCount
                    Models.DataItem fi = new Models.DataItem();

                    fi.RowID = Convert.ToInt32(reader.GetValue(0));
                    fi.ParentID = Convert.ToInt32(reader.GetValue(1));


                    Models.FundItemTypes ft = new Models.FundItemTypes();
                    Enum.TryParse<Models.FundItemTypes>(reader.GetValue(2).ToString(), out ft);
                    fi.DataType = ft;

                    fi.DataShortName = reader.GetValue(3).ToString();
                    fi.DataFullName = reader.GetValue(4).ToString();
                    fi.DataLink = Convert.ToInt32(reader.GetValue(5));
                    fi.Datapath = reader.GetValue(6).ToString();
                    fi.Status = (bool)reader.GetValue(7);
                    fi.Seq = Convert.ToInt32(reader.GetValue(8));
                    //fi.CreateDate = reader.GetValue(9).ToString();
                    fi.dbTable = reader.GetValue(10).ToString();
                    fi.people = reader.GetValue(11).ToString();
                    fi.dbTablePre = reader.GetValue(12).ToString();

                    fiList.Add(fi);
                }
            }

            BuildDataItemStruct(fiList);


            return fiList;
        }


        public List<Models.DataItem> GetDataItems()
        {
            // return _sqliteDB.ExecuteToList<Models.DataItem>(Models.DataItem.SelectSql());

            List<Models.DataItem> fiList = new List<Models.DataItem>();


            SQLiteDataReader reader = _sqliteDB.ExecuteReader(Models.DataItem.SelectSqlAll());
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //reader.FieldCount
                    Models.DataItem fi = new Models.DataItem();

                    fi.RowID = Convert.ToInt32(reader.GetValue(0));
                    fi.ParentID = Convert.ToInt32(reader.GetValue(1));


                    Models.FundItemTypes ft = new Models.FundItemTypes();
                    Enum.TryParse<Models.FundItemTypes>(reader.GetValue(2).ToString(), out ft);
                    fi.DataType = ft;

                    fi.DataShortName = reader.GetValue(3).ToString();
                    fi.DataFullName = reader.GetValue(4).ToString();
                    fi.DataLink = Convert.ToInt32(reader.GetValue(5));
                    fi.Datapath = reader.GetValue(6).ToString();
                    fi.Status = (bool)reader.GetValue(7);
                    fi.Seq = Convert.ToInt32(reader.GetValue(8));
                    //fi.CreateDate = reader.GetValue(9).ToString();
                    fi.dbTable = reader.GetValue(10).ToString();
                    fi.people = reader.GetValue(11).ToString();

                    fiList.Add(fi);
                }
            }

            BuildDataItemStruct(fiList);


            return fiList;
        }

        private void BuildDataItemStruct(List<Models.DataItem> list)
        {
            if (list == null) return;
            
            foreach(Models.DataItem di in list)
            {
                di.parentItem = list.Find(x => x.RowID == di.ParentID);
            }
        }
        public void  Close()
        {
            _sqliteDB.CloseConnection();
        }

        public  DataSet GetDataLabel()
        {
            String sql = @"Select RowID, ParentID, Label, Col1, Seq from DataLabel";

            return _sqliteDB.ExecuteDataset(sql);
        }

        public DataSet GetDataLabel(int seq)
        {
            String sql = @"Select RowID, ParentID, Label, Col1, Seq from DataLabel where seq = " + seq.ToString();

            return _sqliteDB.ExecuteDataset(sql);
        }

        internal void UpdateLabels(List<string> labels)
        {
            String sql = @"delete from DataLabel";
            String insertSql = "insert into DataLabel(label) values(@label)";
            try
            {
                _sqliteDB.BeginTran();
                _sqliteDB.ExecuteNonQuery(sql);

                foreach(string l in labels)
                {
                    _sqliteDB.ExecuteNonQuery(CommandType.Text, insertSql, new SQLiteParameter("@label", l));

                }

                _sqliteDB.Commit();
            }
            catch (Exception ex)
            {
                _sqliteDB.RollBack();

            }
        }

        public Models.DataItem GetDataItem(Models.DataItem di)
        {            
            var list = GetDataItems();

            return list.Find(x => x.RowID == di.RowID);
        }


        public String ModifyDataItem(Models.DataItem di)
        {
            string strRet ="";
            try
            {
                _sqliteDB.ExecuteNonQuery(CommandType.Text, Models.DataItem.ModifySql(), Models.DataItem.getParam(di));
            }
            catch (Exception ex)
            {
                strRet = "项目" + di.DataFullName + "更新失败！" + ex.Message;
            }

            return strRet;
        }

        public String InsertDataItem(Models.DataItem di)
        {
            string strRet = "";
            try
            {
                string[] sqls = new string[] {
                    @"select rowid from dataitem where datashortname = '" + di.DataShortName +"'", "项目简称重复！",
                    @"select rowid from dataitem where datafullname = '" + di.DataFullName +"'", "项目名称重复！",
                    @"select rowid from dataitem where datapath = '" + di.Datapath +"'", "存储路径重复！",
                    @"select rowid from dataitem where dbtable = '" + di.dbTable +"'", "存储表名重复！"
                };

                for (int i = 0; i < sqls.Length; i = i + 2)
                {
                    var o = _sqliteDB.ExecuteScalar(sqls[i]);

                    if (o != null)
                    { 
                        strRet = "项目" + di.DataFullName + sqls[i +1];
                        return strRet;
                    }
                }
                _sqliteDB.ExecuteNonQuery(CommandType.Text, Models.DataItem.InsertSql(), Models.DataItem.getParam(di));
            }
            catch (Exception ex)
            {
                strRet = "项目" + di.DataFullName + "插入失败！" + ex.Message;
            }

            return strRet;
        }

        public String DelDataItem(Models.DataItem di)
        {
            string strRet = "项目" + di.DataFullName;
            try
            {                
                _sqliteDB.ExecuteNonQuery(@"Delete from dataitem where rowid = " + di.RowID.ToString());
                strRet = "";
            }
            catch (Exception ex)
            {
                strRet += "删除失败!(" + ex.Message + ")";
            }

            return strRet;
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
