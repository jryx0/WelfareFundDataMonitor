using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public abstract class CompareServices<T> : IDisposable where T :class
    {
        protected DAL.MySqlite MainSqliteDB;
        #region Init
        public CompareServices()
        {
            MainSqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
        }

        public CompareServices(string connstr)
        {
            MainSqliteDB = new DAL.MySqlite(connstr, GlobalEnviroment.isCryt); ;
        }

        public CompareServices(DAL.MySqlite _sqlite)
        {
            if (MainSqliteDB == null || MainSqliteDB.IsDBClose())
                MainSqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            else
                MainSqliteDB = _sqlite;
        }
        #endregion

        protected abstract T Mapor(SQLiteDataReader reader);
        protected abstract String SelectSql();

        public virtual DataSet GetDataSet()
        {
            return MainSqliteDB.ExecuteDataset(SelectSql());
        }
        public virtual List<T> GetAll()
        {
            List<T> sList = new List<T>();
            SQLiteDataReader reader = MainSqliteDB.ExecuteReader(SelectSql());
            if (reader.HasRows)
                while (reader.Read())
                {
                    sList.Add(Mapor(reader));
                }

            return sList;

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
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~CompareServices() {
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
