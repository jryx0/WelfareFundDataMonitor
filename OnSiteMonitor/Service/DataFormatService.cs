using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Service
{
    class DataFormatService:IDisposable
    {
        private DAL.MySqlite _sqliteDB; 

        public DataFormatService()
        {
            _sqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
        }

        public DataFormatService(string connStr)
        {
            _sqliteDB  = new DAL.MySqlite(connStr, GlobalEnviroment.isCryt);
        }

        public DataFormatService(DAL.MySqlite _sqlite)
        {
            if (_sqliteDB == null || _sqliteDB.IsDBClose())
                _sqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            else
                _sqliteDB = _sqlite;
        }
        
        public void SaveDataFormat(List<Models.DataFormat> ff, int ParentID)
        {
            string sql = @"insert into DataFormat(ParentID, ColName, Col, Comment,Seq, ColCode, DisplayName) values
                                                  (@ParentID, @ColName, @Col, @Comment, @Seq, @ColCode, @DisplayName)";
            try
            {
                _sqliteDB.BeginTran();

                _sqliteDB.ExecuteNonQuery("delete from DataFormat where ParentID = " + ParentID.ToString());
                foreach (Models.DataFormat f in ff)
                {
                    f.ParentID = ParentID;

                    _sqliteDB.ExecuteNonQuery(System.Data.CommandType.Text, sql,
                        new System.Data.SQLite.SQLiteParameter[] {
                                new System.Data.SQLite.SQLiteParameter("@ParentID", f.ParentID),
                                new System.Data.SQLite.SQLiteParameter("@ColName", f.colName),
                                new System.Data.SQLite.SQLiteParameter("@Col", f.colNumber),
                                new System.Data.SQLite.SQLiteParameter("@Comment", f.Comment),
                                new System.Data.SQLite.SQLiteParameter("@Seq", f.Seq),
                                 new System.Data.SQLite.SQLiteParameter("@ColCode", f.colCode),
                                 new System.Data.SQLite.SQLiteParameter("@DisplayName", f.DisplayName)
                            }
                        );
                }
               _sqliteDB.Commit();

            }
            catch(Exception ex)
            {
               _sqliteDB.RollBack();
            }


        }

        internal DataSet GetDataFormat(int rowID, int Seq = 0)
        {
            string sql1 = @"SELECT   RowID, ParentID, ColName as 名称, Col as 值, Seq , colCode, DisplayName
                             FROM      DataFormat
                           WHERE ParentID = " + rowID.ToString() + " and seq >= 0 Order by Seq";


            string sql2 = @"SELECT   RowID, ParentID, ColName as 名称, Col as 值, Comment, Seq , colCode,DisplayName
                             FROM      DataFormat
                           WHERE ParentID = " + rowID.ToString() + " and seq = -1 Order by Seq";

            if (Seq == 0)
                return _sqliteDB.ExecuteDataset(sql1);
            else return _sqliteDB.ExecuteDataset(sql2);

        }


        internal DataSet GetAllDataFormat()
        {
            string sql1 = @"SELECT   RowID, ParentID, ColName as 名称, Col as 值, Seq , colCode,DisplayName
                             FROM      DataFormat Order by Seq";
            return _sqliteDB.ExecuteDataset(sql1);
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
        // ~DataFormatService() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
