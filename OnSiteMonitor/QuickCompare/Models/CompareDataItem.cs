using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.QuickCompare.Models
{
    public enum FundItemTypes
    {
        /// <summary>
        /// 部分惠民资金
        /// </summary>
        Fund = 0,
        /// <summary>
        /// 源数据
        /// </summary>
        SourceItems = 10,
        /// <summary>
        /// 比对数据
        /// </summary>
        ReferenceItems = 20,

        /// <summary>
        /// 源数据
        /// </summary>
        SourceData = 30,

        /// <summary>
        /// 源数据
        /// </summary>
        ReferenceData = 40,

        /// <summary>
        /// 文件
        /// </summary>
        File = 50
    }

    public class CompareDataItem
    {
        public int RowID { get; set; }
        public int ParentID { get; set; }
        public FundItemTypes DataType { get; set; }
        public string DataShortName { get; set; }
        public string DataFullName { get; set; }
        public int DataLink { get; set; }
        public string Datapath { get; set; } //Data File Path
        public int Status { get; set; }
        public int Seq { get; set; }
        public string dbTable { get; set; }
        public string dbTablePre { get; set; }
        public DateTime CreateDate { get; set; }
        public string people { get; set; }

        public string col1 { get; set; }
        public string col2 { set; get; }

        public int deep = 0;
        public int TotalNumbers = 0;

        public CompareDataItem ParentItem;
        public List<CompareDataFormat> ItemFormat;
         
        public static string CreateTableSql()
        {
            return @"   CREATE TABLE DataItem (
                        RowID         INTEGER       PRIMARY KEY,
                        ParentID      INTEGER,
                        DataType      INTEGER,
                        DataShortName VARCHAR (20)  NOT NULL,
                        DataFullName  VARCHAR (40),
                        DataLink      INTEGER,
                        Datapath  VARCHAR (250),
                        DataTime      VARCHAR (20),
                        dbTable      VARCHAR (20),
                        dbTablePre      VARCHAR (20),
                        Status        BOOLEAN       DEFAULT true,
                        Seq           INTEGER       NOT NULL
                                                    DEFAULT (999999),
                        People        VARCHAR (10),
                        Col1          VarCHAR(500),
                        Col2          VarChar(500),
                        UNIQUE (
                        RowID
                        )
                        )";
        }

        public static string InsertSql()
        {
            return @"
                    INSERT INTO DataItem (ParentID,DataType,DataShortName,DataFullName,
                                           DataLink,Datapath,DataTime,Status,Seq, People,dbTable
,dbTablePre, Col1, Col2)
                                  VALUES (@ParentID,@DataType,@DataShortName,@DataFullName,
                                          @DataLink,@Datapath,@CreateDate,@Status,@Seq, @People,
@dbTable,@dbTablePre, @Col1, @Col2);";
        }

        public static System.Data.SQLite.SQLiteParameter[] getParam(CompareDataItem dataItem)
        {
            System.Data.SQLite.SQLiteParameter[] sqlietParams =
                new System.Data.SQLite.SQLiteParameter[]
            {
                new System.Data.SQLite.SQLiteParameter("@ParentID", dataItem.ParentID),
                new System.Data.SQLite.SQLiteParameter("@DataType", dataItem.DataType),
                new System.Data.SQLite.SQLiteParameter("@DataShortName", dataItem.DataShortName),
                new System.Data.SQLite.SQLiteParameter("@DataFullName", dataItem.DataFullName),
                new System.Data.SQLite.SQLiteParameter("@DataLink", dataItem.DataLink),
                new System.Data.SQLite.SQLiteParameter("@Datapath", dataItem.Datapath),
                new System.Data.SQLite.SQLiteParameter("@CreateDate", dataItem.CreateDate),
                new System.Data.SQLite.SQLiteParameter("@Status", dataItem.Status),
                new System.Data.SQLite.SQLiteParameter("@Seq", dataItem.Seq),
                new System.Data.SQLite.SQLiteParameter("@People", dataItem.people),
                new System.Data.SQLite.SQLiteParameter("@dbTable", dataItem.dbTable),
                new System.Data.SQLite.SQLiteParameter("@dbTablePre", dataItem.dbTablePre),
                new System.Data.SQLite.SQLiteParameter("@Col1", dataItem.col1),
                new System.Data.SQLite.SQLiteParameter("@Col2", dataItem.col2),
                new System.Data.SQLite.SQLiteParameter("@RowID", dataItem.RowID)
            };

            return sqlietParams;

        }

        public static string ModifySql()
        {
            return @"UPDATE dataitem
                       SET  DataShortName = @DataShortName,
                            DataFullName = @DataFullName,
                            Datapath = @Datapath,
                            Status = @Status,
                            Seq = @Seq,
                            People = @People,
                            dbTable = @dbTable,
                            dbTablePre = @dbTablePre,
                            Col1 = @Col1,
                            Col2 = @Col2
                     WHERE rowid = @RowID";
        }

        public static string SelectSql(int Status = -1)
        {
            string sql = @"
                 SELECT   
                        RowID, ParentID, DataType,DataShortName, 
                        DataFullName,  DataLink, Datapath,  
                        Status, Seq, DataTime, dbTable, people,dbTablePre, Col1, Col2
                 FROM  DataItem where 1 = 1 @para Order by Seq";

            switch(Status)
            {
                case 0:
                    sql = sql.Replace("@para", "and Status = 0 ");
                    break;
                case 1:
                    sql = sql.Replace("@para", "and Status = 1 ");
                    break;
                default:
                    sql = sql.Replace("@para", "  ");
                    break;
            }

            return sql;
        }

        public static string Selectby(string sets)
        {            
            String _sql = @"
                 SELECT   
                        RowID, ParentID, DataType,DataShortName, 
                        DataFullName,  DataLink, Datapath,  
                        Status, Seq, DataTime, dbTable, people,dbTablePre, Col1, Col2
                 FROM  DataItem where Status = 1 and rowid in (" + sets + ")  Order by Seq";
            return _sql;

        }
    }

    public class CompareDataFormat
    {
        public int ParentID;
        public int RowID;

        public string colName;
        public int colNumber;
        public string DisplayName;
        public string Comment;
        public string colCode;
        public int Seq;
        //seq = -1 关联列
    }

}
