using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OnSiteFundComparer.DAL
{
    public class MySqlite  
    {
        public String sqliteConnectionString { get; set; }
        public String sqlitePassword { get; set; }

        private SQLiteConnection sqliteConn = new SQLiteConnection();

        private SQLiteConnectionStringBuilder sqliteConnstrBuilder =
            new SQLiteConnectionStringBuilder();

        private SQLiteTransaction sqliteTransaction = null;

        private bool isInit = false;
        private bool isTrans = false;

        #region Sqlite Init
        public MySqlite()
        {
            sqliteConnectionString = "";
            isInit = false;
            isTrans = false;
            sqlitePassword = "dfjwhb2014";
        }

        public MySqlite(String connStr)
        {
            sqliteConnectionString = connStr;
            isInit = false;
            isTrans = false;
            sqlitePassword = "dfjwhb2014";
        }

        public MySqlite(String connStr, string password)
        {
            sqliteConnectionString = connStr;
            isInit = false;
            isTrans = false;
            sqlitePassword = password;
        }


        private void InitSqliteDB()
        {
            lock (this)
            {
                //sqliteConnstrBuilder.DataSource = sqliteConnectionString;
                //sqliteConn.ConnectionString = sqliteConnstrBuilder.ToString();

                if (sqliteConn.State == System.Data.ConnectionState.Closed)
                {
                    sqliteConnstrBuilder.DataSource = sqliteConnectionString;
                    sqliteConnstrBuilder.Version = 3;
                    sqliteConnstrBuilder.Password = sqlitePassword;

                    sqliteConn.ConnectionString = sqliteConnstrBuilder.ToString();
                    
                    sqliteConn.Open();


                    

                    isInit = true;
                }
            }
        }
        #endregion

        #region trans
        public void BeginTran()
        {
            lock (this)
            {
                if (!isTrans)
                {
                    if (sqliteConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqliteConnstrBuilder.DataSource = sqliteConnectionString;
                        sqliteConn.ConnectionString = sqliteConnstrBuilder.ToString();
                        sqliteConn.SetPassword(sqlitePassword);
                        sqliteConn.Open();
                    }
                    sqliteTransaction = sqliteConn.BeginTransaction();
                    isTrans = true;
                }
            }
        }
        public void Commit()
        {
            lock (this)
            {
                if (isTrans && sqliteTransaction != null)
                {
                    sqliteTransaction.Commit();
                    sqliteTransaction.Dispose();
                    isTrans = false;
                }
            }
        }

        public void RollBack()
        {
            lock (this)
            {
                if (isTrans && sqliteTransaction != null)
                {
                    sqliteTransaction.Rollback();
                    sqliteTransaction.Dispose();
                    isTrans = false;
                }
            }
        }
        #endregion

        #region Close DataBase Connection
        public void CloseConnection()
        {
            lock (this)
            {
                if (sqliteConn != null)
                {
                    if (sqliteConn.State == ConnectionState.Open)
                    {                        
                        sqliteConn.Close();
                    }
                    
                    sqliteConn.Dispose();
                    sqliteConn.Dispose();

                    isInit = false;
                }
            }
        }
        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText);
        }
        public int ExecuteNonQuery(CommandType commType, string commandText)
        {
            try
            {
                this.InitSqliteDB();
                if (this.isTrans)
                    return SqlHelperDB.ExecuteNonQuery(this.sqliteTransaction, commType, commandText);
                else
                    return SqlHelperDB.ExecuteNonQuery(this.sqliteConn, commType, commandText);
            }
            catch (Exception e)
            {
                //this.m_haserror = true;
                //Log.instance().AsynWriteLog("ExecuteNonQuery Error:" + e.Message, Log.LOGLEVEL.Error);
                throw new System.Exception(e.Message + ";\r\n" + commandText, e.InnerException);
            }
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitSqliteDB();
                if (this.isTrans)
                    return SqlHelperDB.ExecuteNonQuery(this.sqliteTransaction, commandType, commandText, commandParameters);
                else
                    return SqlHelperDB.ExecuteNonQuery(this.sqliteConn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                //this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
                //Log.instance().AsynWriteLog("ExecuteNonQuery Error:" + e.Message, Log.LOGLEVEL.Error);
            }
        }

        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataset(string commandText)
        {
            return this.ExecuteDataset(CommandType.Text, commandText);
        }

        public DataSet ExecuteDataset(string commandText, string sTableName)
        {
            try
            {
                this.InitSqliteDB();
                return SqlHelperDB.ExecuteDataset(this.sqliteConn, CommandType.Text, commandText, sTableName, (IDbDataParameter[])null);
            }
            catch (Exception e)
            {
                // this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            try
            {
                this.InitSqliteDB();
                return SqlHelperDB.ExecuteDataset(this.sqliteConn, commandType, commandText);
            }
            catch (Exception e)
            {
                // this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        public DataSet ExecuteDataset(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitSqliteDB();
                return SqlHelperDB.ExecuteDataset(this.sqliteConn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                // this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteDataSet

        #region ExecuteScalar
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(CommandType.Text, commandText);
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            try
            {
                this.InitSqliteDB();

                return SqlHelperDB.ExecuteScalar(this.sqliteConn, commandType, commandText);
            }
            catch (Exception e)
            {
                //this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitSqliteDB();
                return SqlHelperDB.ExecuteScalar(this.sqliteConn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                //this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public object ExecuteScalar(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitSqliteDB();
                return SqlHelperDB.ExecuteScalar(this.sqliteConn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                //this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteScalar	

        #region ExecuteReader
        public SQLiteDataReader ExecuteReader(string commandText)
        {
            return this.ExecuteReader(CommandType.Text, commandText);
        }

        public SQLiteDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            try
            {
                this.InitSqliteDB();
               
                    return SqlHelperDB.ExecuteReader(this.sqliteConn, commandType, commandText);
            }
            catch (Exception e)
            {
              //  this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public SQLiteDataReader ExecuteReader(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitSqliteDB();
               
                    return SqlHelperDB.ExecuteReader(this.sqliteConn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
              //  this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public SQLiteDataReader ExecuteReader(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitSqliteDB();
                //return Data.SqlHelperDB.ExecuteReader(this.m_transaction,spName,parameterValues);

                return SqlHelperDB.ExecuteReader(this.sqliteConn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                //  this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteReader

        #region ExcuteMapping

        /// <summary>  
        /// 判断SqlDataReader是否存在某列  
        /// </summary>  
        /// <param name="dr">SqlDataReader</param>  
        /// <param name="columnName">列名</param>  
        /// <returns></returns>  
        private bool readerExists(SQLiteDataReader dr, string columnName)
        {

            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";

            return (dr.GetSchemaTable().DefaultView.Count > 0);

        }

        ///<summary>  
        ///利用反射和泛型将SqlDataReader转换成List模型  
        ///</summary>  
        ///<param name="sql">查询sql语句</param>  
        ///<returns></returns>  
        public List<T> ExecuteToList<T>(string sql) where T : new()

        {
            List<T> list;

            Type type = typeof(T);
            string tempName = string.Empty;

            using (SQLiteDataReader reader = ExecuteReader(sql))
            {
                if (reader.HasRows)
                {
                    list = new List<T>();
                    while (reader.Read())
                    {
                        T t = new T();
                        PropertyInfo[] propertys = t.GetType().GetProperties();

                        foreach (PropertyInfo pi in propertys)
                        {
                            tempName = pi.Name;
                            if (readerExists(reader, tempName))
                            {
                                if (!pi.CanWrite)
                                {
                                    continue;
                                }
                                var value = reader[tempName];

                                if (value != DBNull.Value)
                                {


                                    //if (!pi.PropertyType.IsGenericType)
                                    //{
                                    //    //非泛型
                                    //    pi.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, property.PropertyType), null);
                                    //}
                                    //else
                                    //{
                                    //    //泛型Nullable<>
                                    //    Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                                    //    if (genericTypeDefinition == typeof(Nullable<>))
                                    //    {
                                    //        pi.SetValue(t, string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)), null);
                                    //    }
                                    //}

                                    if (pi.PropertyType.IsEnum)
                                    {
                                        object enumName = Enum.ToObject(pi.PropertyType, value);
                                        pi.SetValue(t, enumName, null);
                                    }
                                    else
                                        pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);//类型转换。


                                    // pi.SetValue(t, value, null);
                                }
                            }
                        }
                        list.Add(t);
                    }
                    return list;
                }
            }
            return null;
        }
        #endregion

        public string AttchDatabase(String attrDBFile, string newDB)
        {
            if (attrDBFile == null)
                return "";

            string strRet = "ATTACH DATABASE '@dbfile' AS @newdb";

            strRet = strRet.Replace("@dbfile", attrDBFile);
            strRet = strRet.Replace("@newdb", newDB);
            try
            {
                this.ExecuteNonQuery(strRet);
            }
            catch (Exception ex)
            {
                return "";
            }
            return newDB;
        }

        public string AttchDatabase(MySqlite attrDB, string newDB)
        {

            return AttchDatabase(attrDB.sqliteConnectionString, newDB);
            //if (attrDB == null)
            //    return "";

            //string strRet = "ATTACH DATABASE '@dbfile' AS @newdb";

            //strRet = strRet.Replace("@dbfile", attrDB.sqliteConnectionString);
            //strRet = strRet.Replace("@newdb", newDB);
            //try
            //{
            //    this.ExecuteNonQuery(strRet);
            //}
            //catch(Exception ex)
            //{
            //    return "";
            //}
            //return newDB;
        }
        public void DetchDatabase( string AliasName)
        {             
            string strRet = "DETACH DATABASE '" +AliasName +"'";

            try
            {
                this.ExecuteNonQuery(strRet);
            }
            catch (Exception ex)
            {
                 
            }
        }

        //#region IDisposable Support
        //private bool disposedValue = false; // 要检测冗余调用

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // TODO: 释放托管状态(托管对象)。
        //            if (sqliteTransaction != null)
        //                sqliteTransaction.Dispose();
        //            if (sqliteConn != null)
        //                sqliteConn.Dispose();

        //        }

        //        // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
        //        // TODO: 将大型字段设置为 null。

        //        disposedValue = true;
        //    }
        //}

        //// TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        //// ~MySqlite() {
        ////   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        ////   Dispose(false);
        //// }

        //// 添加此代码以正确实现可处置模式。
        //void IDisposable.Dispose()
        //{
        //    // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //    Dispose(true);
        //    // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
        //    // GC.SuppressFinalize(this);
        //}
        //#endregion


    }
    /// <summary>
    /// SQLHelper 的摘要说明。
    /// </summary>
    public sealed class SqlHelperDB
    {
        #region private utility methods & constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelper()".
        private SqlHelperDB() { }
        

        /// <summary>
        /// This method is used to attach array of IDbDataParameter s to a IDbCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">an array of IDbDataParameter s tho be added to command</param>
        private static void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
        {
            foreach (IDbDataParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// This method assigns an array of values to an array of IDbDataParameter s.
        /// </summary>
        /// <param name="commandParameters">array of IDbDataParameter s to be assigned values</param>
        /// <param name="parameterValues">array of objects holding the values to be assigned</param>
        private static void AssignParameterValues(IDbDataParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                //do nothing if we get no data
                return;
            }

            // we must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            //iterate through the IDbDataParameter s, assigning the values from the corresponding position in the 
            //value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command.
        /// </summary>
        /// <param name="command">the IDbCommand to be prepared</param>
        /// <param name="connection">a valid IDbConnection , on which to execute this command</param>
        /// <param name="transaction">a valid IDbTransaction , or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of IDbDataParameter s to be associated with the command or 'null' if no parameters are required</param>
        private static void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //associate the connection with the command
            command.Connection = connection;

            //set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            //if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //set the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }


        #endregion private utility methods & constructors

        #region ExecuteNonQuery



        /// <summary>
        /// Execute a IDbCommand (that returns no resultset and takes no parameters) against the provided IDbConnection . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteNonQuery(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns no resultset) against the specified IDbConnection  
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution

            IDbCommand cmd = new SQLiteCommand();

            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //finally, execute the command.
            int retval = cmd.ExecuteNonQuery();

            //cmd.Parameters.CopyTo(commandParameters,0);      // Dobbin Added

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return retval;
        }



        /// <summary>
        /// Execute a IDbCommand (that returns no resultset and takes no parameters) against the provided IDbTransaction . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteNonQuery(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns no resultset) against the specified IDbTransaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution

            IDbCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //finally, execute the command.
            int retval = cmd.ExecuteNonQuery();

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return retval;
        }



        #endregion ExecuteNonQuery

        #region ExecuteDataSet



        /// <summary>
        /// Execute a IDbCommand (that returns a resultset and takes no parameters) against the provided IDbConnection . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteDataset(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a resultset) against the specified IDbConnection  
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution


            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            // detach the IDbDataParameter s from the command object, so they can be used again.			
            cmd.Parameters.Clear();

            //return the dataset
            return ds;
        }


        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, string sTableName, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution


            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds, sTableName);

            // detach the IDbDataParameter s from the command object, so they can be used again.			
            cmd.Parameters.Clear();

            //return the dataset
            return ds;
        }


        /// <summary>
        /// Execute a IDbCommand (that returns a resultset and takes no parameters) against the provided IDbTransaction . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteDataset(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a resultset) against the specified IDbTransaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a dataset containing the resultset generated by the command</returns>
        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();

            //return the dataset
            return ds;
        }


        #endregion ExecuteDataSet

        #region ExecuteReader

        /// <summary>
        /// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        /// we can set the appropriate CommandBehavior when calling ExecuteReader()
        /// </summary>
        private enum ConnectionOwnership
        {
            /// <summary>Connection is owned and managed by SqlHelper</summary>
            Internal,
            /// <summary>Connection is owned and managed by the caller</summary>
            External
        }

        /// <summary>
        /// Create and prepare a IDbCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// </summary>
        /// <remarks>
        /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        /// 
        /// If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="connection">a valid IDbConnection , on which to execute this command</param>
        /// <param name="transaction">a valid IDbTransaction , or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of IDbDataParameter s to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
        /// <returns>DataReader containing the results of the command</returns>
        private static SQLiteDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, ConnectionOwnership connectionOwnership)
        {
            //create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            //create a reader
            SQLiteDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == ConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();

            return dr;
        }


        /// <summary>
        /// Execute a IDbCommand (that returns a resultset and takes no parameters) against the provided IDbConnection . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SQLiteDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a SQLiteDataReader containing the resultset generated by the command</returns>
        public static SQLiteDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteReader(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a resultset) against the specified IDbConnection  
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SQLiteDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a SQLiteDataReader containing the resultset generated by the command</returns>
        public static SQLiteDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (IDbTransaction)null, commandType, commandText, commandParameters, ConnectionOwnership.External);
        }



        /// <summary>
        /// Execute a IDbCommand (that returns a resultset and takes no parameters) against the provided IDbTransaction . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SQLiteDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a SQLiteDataReader containing the resultset generated by the command</returns>
        public static SQLiteDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteReader(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a resultset) against the specified IDbTransaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///   SQLiteDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a SQLiteDataReader containing the resultset generated by the command</returns>
        public static SQLiteDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, ConnectionOwnership.External);
        }



        #endregion ExecuteReader

        #region ExecuteScalar



        /// <summary>
        /// Execute a IDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided IDbConnection . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteScalar(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a 1x1 resultset) against the specified IDbConnection  
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return retval;

        }



        /// <summary>
        /// Execute a IDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided IDbTransaction . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            //pass through the call providing null for the set of IDbDataParameter s
            return ExecuteScalar(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// Execute a IDbCommand (that returns a 1x1 resultset) against the specified IDbTransaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            SQLiteCommand cmd = new SQLiteCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // detach the IDbDataParameter s from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion ExecuteScalar	

    }
}
