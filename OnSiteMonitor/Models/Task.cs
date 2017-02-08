using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Models
{
    public class Task
    {
        public int RowID;
        public string TaskName;
        public string CreateDate;
        public string TaskComment;
        public string Region;
        public string DBInfo;

        public string UserName;

        public static System.Data.SQLite.SQLiteParameter[] getParam(Task _task)
        {
            System.Data.SQLite.SQLiteParameter[] sqlietParams =
                new System.Data.SQLite.SQLiteParameter[]
            {
                new System.Data.SQLite.SQLiteParameter("@TaskName", _task.TaskName),
                new System.Data.SQLite.SQLiteParameter("@CreateDate", _task.CreateDate),
                new System.Data.SQLite.SQLiteParameter("@TaskComment", _task.TaskComment),
                new System.Data.SQLite.SQLiteParameter("@Region", _task.Region),
                new System.Data.SQLite.SQLiteParameter("@DBInfo", _task.DBInfo),
                new System.Data.SQLite.SQLiteParameter("@UserName", _task.UserName)
            };
            return sqlietParams;
        }

    }
}
