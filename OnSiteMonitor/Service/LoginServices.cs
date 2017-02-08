using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using OnSiteFundComparer.Models;

namespace OnSiteFundComparer.Service
{
    class LoginServices
    {
        private DAL.MySqlite _sqliteDB;

        public LoginServices(string connStr)
        {
            _sqliteDB = new DAL.MySqlite(connStr);
        }

        public LoginServices()
        {
            _sqliteDB = new DAL.MySqlite(Application.StartupPath + "\\" +
            OnSiteFundComparer.Properties.Settings.Default.MainDBFile);
        }


        public Models.User IsAuthorition(string user, string password)
        {
            Models.User loginuser = null;
            string sql = @"select RowID, userName, Password, isfirst from user where userName =  @user and password = @password and status = 1";
            //string sql = @"select * from dataitem where rowid= 5";

            try
            {

                //_sqliteDB.ExecuteDataset(sql);
                var o = _sqliteDB.ExecuteDataset(CommandType.Text, sql,
                    new SQLiteParameter[] {
                        new SQLiteParameter("@user", user),
                        new SQLiteParameter("@password", password)}
                    );

                if (o != null && o.Tables[0].Rows.Count == 1)
                {
                    DataRow dr = o.Tables[0].Rows[0];
                    loginuser = new Models.User();

                    loginuser.RowID = Convert.ToInt32(dr[0].ToString());
                    loginuser.Name = dr[1].ToString();
                    loginuser.Password = dr[2].ToString();
                    loginuser.isFisrt = Convert.ToInt32(dr[3].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误,请稍后登陆!");
                loginuser = null;
            }

            return loginuser;
        }

        internal void AddUser(User u)
        {
            string sql = @"insert into user(username, password, isfirst) values(@username, @password, @isfirst)";
            try
            {
               
                _sqliteDB.ExecuteNonQuery(CommandType.Text, sql,
                    new SQLiteParameter[] {
                                            new SQLiteParameter("@password", u.Password),
                                            new SQLiteParameter("@username", u.Name),
                                            new SQLiteParameter("@isfirst", u.isFisrt)}
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新密码出错!");
            }

        }

        internal bool UpdateUserByName(User user)
        {
            bool bRet = false;
            string sql = @"Update user set password = @password, isfirst = @isfirst where username=@username";

            try
            {
                user.isFisrt = 0;
                _sqliteDB.ExecuteNonQuery(CommandType.Text, sql, 
                    new SQLiteParameter[] {
                                            new SQLiteParameter("@password", user.Password),
                                            new SQLiteParameter("@username", user.Name),
                                            new SQLiteParameter("@isfirst", user.isFisrt)}
                    );
                bRet = true;
            }
            catch (Exception ex)
            {
            }
            return bRet;
        }
    }
}
