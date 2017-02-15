using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Service
{
    //class CollisionAimService
    //{

    //    private DAL.MySqlite _sqliteDB;

    //    public CollisionAimService(string connStr)
    //    {
    //        _sqliteDB = new DAL.MySqlite(connStr);
    //    }


    //    public void DeleteAimbyID(string id)
    //    {
    //        String sql = @"Delete from CollisionAim where rowid = @id";
    //        try
    //        {
    //            _sqliteDB.ExecuteNonQuery(CommandType.Text, sql,
    //                new System.Data.SQLite.SQLiteParameter("@id", id));
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }
    //    public void UpdateAim(Models.CollisionAim ca)
    //    {
    //        String sql = @"Delete from CollisionAim where rowid = @id";
    //        String sqlinsert = @"insert into CollisionAim(sourceid, aimname, aimdesc, status, seq, tablename, rules, rules2) 
    //                             values(@sourceid, @aimname, @aimdesc, 1, @seq, @tablename, @rules, @r2)";

    //        try
    //        {
    //            _sqliteDB.BeginTran();
    //            _sqliteDB.ExecuteNonQuery(CommandType.Text, sql,
    //               new System.Data.SQLite.SQLiteParameter("@id", ca.RowID));
    //            _sqliteDB.ExecuteNonQuery(CommandType.Text, sqlinsert,
    //                new System.Data.SQLite.SQLiteParameter[] {
    //                    new System.Data.SQLite.SQLiteParameter("@sourceid", ca.SourceID),
    //                    new System.Data.SQLite.SQLiteParameter("@aimname", ca.AimName),
    //                    new System.Data.SQLite.SQLiteParameter("@aimdesc", ca.AimDesc),
    //                    new System.Data.SQLite.SQLiteParameter("@seq", ca.seq) ,
    //                    new System.Data.SQLite.SQLiteParameter("@tablename", ca.TableName),
    //                    new System.Data.SQLite.SQLiteParameter("@rules", ca.Rules),
    //                    new System.Data.SQLite.SQLiteParameter("@r2", ca.Rules2)
    //            });
    //            _sqliteDB.Commit();
    //        }
    //        catch (Exception ex)
    //        {
    //            _sqliteDB.RollBack();
    //        }

    //    }

    //    public DataSet GetAimbyID(string id)
    //    {
    //        DataSet ds = new DataSet();
    //        String Sql = @"SELECT  CollisionAim.RowID,
    //                               CollisionAim.SourceID,
    //                               CollisionAim.AimName as 比对目标,
    //                               CollisionAim.aimdesc as 规则说明, 
    //                               DataItem.DataFullName as  源数据,
    //                               CollisionAim.Seq  ,
    //                               CollisionAim.TableName as 表名,
    //                               CollisionAim.Rules,
    //                                CollisionAim.Rules2
    //                          FROM CollisionAim
    //                               JOIN
    //                               DataItem ON CollisionAim.SourceID = DataItem.RowID
    //                         WHERE CollisionAim.Status = 1 and CollisionAim.RowID = @id
    //                         ORDER BY DataItem.seq,
    //                                  CollisionAim.seq";

    //        try
    //        {
    //            ds = _sqliteDB.ExecuteDataset(Sql.Replace("@id", id));
    //        }
    //        catch (Exception ex)
    //        {

    //        }


    //        return ds;
    //    }
    //    public DataSet GetAims()
    //    {
    //        DataSet ds = null;
    //        String Sql = @"SELECT CollisionAim.RowID,
    //                               CollisionAim.SourceID,
    //                               CollisionAim.AimName AS 比对目标,
    //                               CollisionAim.aimdesc AS 规则说明,
    //                               DataItem.DataFullName AS 源数据,
    //                               CollisionAim.Seq,
    //                               CollisionAim.TableName AS 表名,
    //                               CollisionAim.Rules,
    //                               CollisionAim.Rules2
    //                          FROM CollisionAim
    //                               JOIN
    //                               DataItem ON CollisionAim.SourceID = DataItem.RowID
    //                         WHERE CollisionAim.Status = 1 AND 
    //                               DataItem.Status = 1
    //                         ORDER BY DataItem.seq,
    //                                  CollisionAim.seq";

    //        try
    //        {
    //            ds = _sqliteDB.ExecuteDataset(Sql);
    //        }
    //        catch (Exception ex)
    //        {
    //            //MessageBox.Show(ex.Message);
    //        }


    //        return ds;
    //    }
    //}
}
