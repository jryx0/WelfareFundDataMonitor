using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    /// <summary>
    /// IDE 的摘要说明。
    /// </summary>
    public class DataBase
    {
        public bool m_haserror = false;
        public bool AccessVisitControl = true;

        private System.Threading.ManualResetEvent mEvent = new ManualResetEvent(true);
        public DataBase()
        {

        }

        #region DataBase Connection string
        private string m_DBConnectionString = "";
        public string DBConnectionString
        {
            set { this.m_DBConnectionString = value; }
            get { return this.m_DBConnectionString; }
        }
        #endregion

        #region Data Base Connection Object,Transaction
        private System.Data.OleDb.OleDbTransaction m_transaction;
        private System.Data.OleDb.OleDbConnection m_conn;
        private bool m_NeedTran = false;
        public bool NeedTran
        {
            get { return this.m_NeedTran; }
            set { this.m_NeedTran = value; }
        }
        #endregion

        #region Init Data Base Connection
        private void InitConnection()
        {
            if (m_conn == null)
            {
                m_conn = new System.Data.OleDb.OleDbConnection(this.m_DBConnectionString);
                //m_conn=new System.Data.OleDb.OracleConnection(Configure.DBConnection);
                m_conn.ConnectionString = this.m_DBConnectionString;
                m_conn.Open();
                if (this.m_NeedTran)
                {
                    this.m_transaction = m_conn.BeginTransaction();

                }

            }
            else
            {
                if (m_conn.State == ConnectionState.Closed)
                {
                    //zr修改
                    m_conn.ConnectionString = this.m_DBConnectionString;
                    m_conn.Open();
                }
            }
        }

        #endregion

        #region Close Data Base Connection
        public void CloseConnection()
        {

            if (m_conn != null)
            {

                if (m_conn.State == ConnectionState.Open)
                {
                    m_conn.Close();
                }
                m_conn.Dispose();
                m_conn.Dispose();
            }
        }
        #endregion

        #region Rollback 
        public void RollBack()
        {
            if (this.m_transaction != null)
            {
                this.m_transaction.Rollback();
            }
            this.m_NeedTran = false;
        }
        #endregion

        #region Commit 
        public void Commit()
        {
            if (this.m_transaction != null)
            {
                this.m_transaction.Commit();
            }
            this.m_NeedTran = false;
        }
        #endregion

        #region Begin Tran
        public void BeginTran()
        {
            this.m_NeedTran = true;

            if (m_conn == null)
            {
                m_conn = new System.Data.OleDb.OleDbConnection(this.m_DBConnectionString);
                m_conn.ConnectionString = this.m_DBConnectionString;
                m_conn.Open();
            }
            else
            {
                if (m_conn.State == ConnectionState.Closed)
                {
                    //hlh: 2005.08.29 修改
                    m_conn.ConnectionString = this.m_DBConnectionString;
                    m_conn.Open();
                }
            }
            this.m_transaction = m_conn.BeginTransaction();
        }
        #endregion

        #region  Synchronization DB operation for ACCESS
        public void Enter()
        {
            Monitor.Enter(this);
            return;
        }

        public void Exit()
        {
            Monitor.Exit(this);
            return;
        }
        #endregion

        #region 公用的操作数据库的方法

        #region ExecuteNonQuery
        //Use for access
        public int ExecuteNonQuery(string commandText, bool bExclude)
        {
            int nRet = -1;

            if (bExclude)
            {
                lock (m_DBConnectionString)
                {
                    //Monitor.Enter(m_DBConnectionString);
                    nRet = ExecuteNonQuery(commandText);
                    //Monitor.Exit(m_DBConnectionString);
                }
            }
            else nRet = ExecuteNonQuery(commandText);

            return nRet;
        }

        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteNonQuery(this.m_transaction, commandType, commandText);
                else
                    return SqlHelperDB.ExecuteNonQuery(this.m_conn, commandType, commandText);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                //Log.instance().AsynWriteLog("ExecuteNonQuery Error:" + e.Message, Log.LOGLEVEL.Error);
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteNonQuery(this.m_transaction, commandType, commandText, commandParameters);
                else
                    return SqlHelperDB.ExecuteNonQuery(this.m_conn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
                //Log.instance().AsynWriteLog("ExecuteNonQuery Error:" + e.Message, Log.LOGLEVEL.Error);

            }
        }

        public int ExecuteNonQuery(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitConnection();
                //return Data.SqlHelperDB.ExecuteNonQuery(this.m_transaction,spName,parameterValues);
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteNonQuery(this.m_transaction, CommandType.StoredProcedure, spName, parameterValues);
                else
                    return SqlHelperDB.ExecuteNonQuery(this.m_conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        #endregion ExecuteNonQuery

        #region ExecuteDataSet
        public DataSet ExecuteDataset(string commandText)
        {
            return this.ExecuteDataset(CommandType.Text, commandText);
        }

        public DataSet ExecuteDataset(string commandText, string sTableName)
        {
            try
            {
                this.InitConnection();
                return SqlHelperDB.ExecuteDataset(this.m_conn, CommandType.Text, commandText, sTableName, (IDbDataParameter[])null);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }



        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteDataset(this.m_transaction, commandType, commandText);
                else
                    return SqlHelperDB.ExecuteDataset(this.m_conn, commandType, commandText);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public DataSet ExecuteDataset(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteDataset(this.m_transaction, commandType, commandText, commandParameters);
                else
                    return SqlHelperDB.ExecuteDataset(this.m_conn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public DataSet ExecuteDataset(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitConnection();
                //return Data.SqlHelperDB.ExecuteDataset(this.m_transaction,spName,parameterValues);
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteDataset(this.m_transaction, CommandType.StoredProcedure, spName, parameterValues);
                else
                    return SqlHelperDB.ExecuteDataset(this.m_conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteDataSet

        #region ExecuteReader
        public System.Data.OleDb.OleDbDataReader ExecuteReader(string commandText)
        {
            return this.ExecuteReader(CommandType.Text, commandText);
        }

        public System.Data.OleDb.OleDbDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteReader(this.m_transaction, commandType, commandText);
                else
                    return SqlHelperDB.ExecuteReader(this.m_conn, commandType, commandText);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public System.Data.OleDb.OleDbDataReader ExecuteReader(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteReader(this.m_transaction, commandType, commandText, commandParameters);
                else
                    return SqlHelperDB.ExecuteReader(this.m_conn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public System.Data.OleDb.OleDbDataReader ExecuteReader(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitConnection();
                //return Data.SqlHelperDB.ExecuteReader(this.m_transaction,spName,parameterValues);
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteReader(this.m_transaction, CommandType.StoredProcedure, spName, parameterValues);
                else
                    return SqlHelperDB.ExecuteReader(this.m_conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteReader

        #region ExecuteScalar
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(CommandType.Text, commandText);
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteScalar(this.m_transaction, commandType, commandText);
                else
                    return SqlHelperDB.ExecuteScalar(this.m_conn, commandType, commandText);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                this.InitConnection();
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteScalar(this.m_transaction, commandType, commandText, commandParameters);
                else
                    return SqlHelperDB.ExecuteScalar(this.m_conn, commandType, commandText, commandParameters);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }

        public object ExecuteScalar(string spName, params IDbDataParameter[] parameterValues)
        {
            try
            {
                this.InitConnection();
                //return Data.SqlHelperDB.ExecuteScalar(this.m_transaction,spName,parameterValues);
                if (this.m_NeedTran)
                    return SqlHelperDB.ExecuteScalar(this.m_transaction, CommandType.StoredProcedure, spName, parameterValues);
                else
                    return SqlHelperDB.ExecuteScalar(this.m_conn, CommandType.StoredProcedure, spName, parameterValues);
            }
            catch (Exception e)
            {
                this.m_haserror = true;
                throw new System.Exception(e.Message, e.InnerException);
            }
        }
        #endregion ExecuteScalar	




        #endregion


        private OleDbCommand GetOraComm(string procname, string[] prams, string[] values)
        {
            OleDbCommand comm = new OleDbCommand(procname, m_conn);
            comm.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i < prams.Length; i++)
            {
                comm.Parameters.Add(prams[i], values[i]);
            }
            comm.Parameters.Add(new OleDbParameter("RururnValue", OleDbType.VarChar, 50, ParameterDirection.Output, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return comm;
        }

        public string Proc(string procName, string[] prams, string[] values)
        {
            //用途：    （1）执行代输入、输出参数的存储过程，以完成插入、修改、删除操作,并返回需要的结果字符串；
            //          （2）执行代输入、输出参数的存储过程，以获得需要的结果字符串；

            //制作人：  柏亮
            this.InitConnection();
            OleDbCommand comm = GetOraComm(procName, prams, values);
            //m_conn.Open();
            comm.ExecuteNonQuery();
            m_conn.Close();
            string AA = comm.Parameters["RururnValue"].Value.ToString();
            return AA;
        }



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

            IDbCommand cmd = new System.Data.OleDb.OleDbCommand();

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

            IDbCommand cmd = new System.Data.OleDb.OleDbCommand();
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


            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(cmd);
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


            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
            PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(cmd);
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
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            //create the DataAdapter & DataSet
            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(cmd);
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
        /// <returns>System.Data.OleDb.OleDbDataReader containing the results of the command</returns>
        private static System.Data.OleDb.OleDbDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, ConnectionOwnership connectionOwnership)
        {
            //create a command and prepare it for execution
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            //create a reader
            System.Data.OleDb.OleDbDataReader dr;

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
        ///  System.Data.OleDb.OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a System.Data.OleDb.OleDbDataReader containing the resultset generated by the command</returns>
        public static System.Data.OleDb.OleDbDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText)
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
        ///  System.Data.OleDb.OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="connection">a valid IDbConnection </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a System.Data.OleDb.OleDbDataReader containing the resultset generated by the command</returns>
        public static System.Data.OleDb.OleDbDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            //pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(connection, (IDbTransaction)null, commandType, commandText, commandParameters, ConnectionOwnership.External);
        }



        /// <summary>
        /// Execute a IDbCommand (that returns a resultset and takes no parameters) against the provided IDbTransaction . 
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  System.Data.OleDb.OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>a System.Data.OleDb.OleDbDataReader containing the resultset generated by the command</returns>
        public static System.Data.OleDb.OleDbDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText)
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
        ///   System.Data.OleDb.OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter ("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">a valid IDbTransaction </param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>a System.Data.OleDb.OleDbDataReader containing the resultset generated by the command</returns>
        public static System.Data.OleDb.OleDbDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
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
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
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
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
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
