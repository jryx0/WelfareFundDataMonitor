using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Models
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
    public enum SourceDataTypes
    {
        /// <summary>
        /// 01城市(农村)居民最低生活保障资金
        /// </summary>
        SafeLowLife = 10,
        /// <summary>
        /// 02村五保户资金
        /// </summary>
        FiveGuaranteFamily = 20,
        /// <summary>
        /// 03医疗救助资金
        /// </summary>
        MedicalFinancial = 30,
        /// <summary>
        /// 04中央财政城镇保障性安居工程专项资金
        /// </summary>
        SafeHouse = 40,
        /// <summary>
        ///05农村危房补助资金 
        /// </summary>
        RuralBadHouse = 50,
        /// <summary>
        /// 06农机购置补贴资金
        /// </summary>
        SourceMachineAid = 60,
        /// <summary>
        /// 07粮食直接补贴资金
        /// </summary>
        SourceFoodAid = 70,
        /// <summary>
        /// 08农资综合补贴资金
        /// </summary>
        SourceAgriMaterials = 80
    }
    public enum RefenceDataTypes
    {
        /// <summary>
        /// 01财政供养人员名单
        /// </summary>
        CivilInfo = 1010,
        /// <summary>
        /// 02财政供养人员家属名单
        /// </summary>
        CivilRelateInfo = 1020,
        /// <summary>
        /// 03领导干部名单
        /// </summary>
        LeaderInfo = 1030,
        /// <summary>
        /// 04领导干部家属名单
        /// </summary>
        LeaderRelateInfo = 1040,
        /// <summary>
        /// 05村干部名单
        /// </summary>
        CountryInfo = 1050,
        /// <summary>
        /// 06村干部家属名单
        /// </summary>
        CountryRelateInfo = 1060,
        /// <summary>
        /// 07个人纳税信息
        /// </summary>
        IncomeTaxInfo = 1070,
        /// <summary>
        /// 08工商登记信息
        /// </summary>
        CompareCompanyInfo = 1080,
        /// <summary>
        /// 09贫困人口名单
        /// </summary>
        ComparePoorInfo = 1090,
        /// <summary>
        /// 10人口信息
        /// </summary>
        tbComparePersonInfo = 1100,
        /// <summary>
        /// 11户籍信息
        /// </summary>
        tbCompareFamilyInfo = 1110,
        /// <summary>
        /// 12车辆登记信息
        /// </summary>
        tbCompareCarInfo = 1120,
        /// <summary>
        /// 13死亡人员名单
        /// </summary>
        tbCompareDeathInfo = 1130,
        /// <summary>
        /// 14农机购买户名单
        /// </summary>
        tbComparemachineInfo = 1140,
        /// <summary>
        /// 15火化登记
        /// </summary>
        CremationInfo = 1150,
        /// <summary>
        /// 16房产信息
        /// </summary>
        HouseInfo = 1160
    }
    public enum RulesTypes
    {
        Compare = 0,
        Check = 1,
        Preprocess = 2
    }

    public class DataItem
    {
        public int RowID { get; set; }
        public int ParentID { get; set; }
        public FundItemTypes DataType { get; set; }
        public string DataShortName { get; set; }
        public string DataFullName { get; set; }
        public int DataLink { get; set; }
        public string Datapath { get; set; } //Data File Path
        public bool Status { get; set; }
        public int Seq { get; set; }
        public string dbTable { get; set; }
        public string dbTablePre { get; set; }
        public DateTime CreateDate { get; set; }
        public string people { get; set; }

        public string col1 { get; set; }
        public string col2 { set; get; }

        public int deep = 0;
        public int TotalNumbers = 0;
        
        public DataItem parentItem;

        public DataItem()
        {
           
        }
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
        public static System.Data.SQLite.SQLiteParameter[] getParam(DataItem dataItem)
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


        public static string SelectSql()
        {
            return @"
                 SELECT   
                        RowID, ParentID, DataType,DataShortName, 
                        DataFullName,  DataLink, Datapath,  
                        Status, Seq, DataTime, dbTable, people,dbTablePre, Col1, Col2
                 FROM  DataItem where Status = 1 Order by Seq";

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

        public static string SelectSqlAll()
        {
            return @"
                 SELECT   
                        RowID, ParentID, DataType,DataShortName, 
                        DataFullName,  DataLink, Datapath,  
                        Status, Seq, DataTime, dbTable, people,dbTablePre, Col1, Col2
                 FROM  DataItem  Order by Seq";

        }
    }  
    
    public class DataFormat
    {
            //"起始行号"  0
            //,"身份证号" 1
            //,"姓名"    2
            //,"地址"    3
            //,"日期"    4
            //,"金额"    5
            //,"类型"    6
            //,"相关人身份证号"  7
            //,"相关人姓名"     8
            //,"关系"         9
            //,"编码"         10
            //,"面积"         11
        public int ParentID;        
        public int RowID;
        
        public string colName;
        public int colNumber;
        public string DisplayName;
        public string Comment;
        public string colCode;
        public int Seq;
        //seq = -1 关联列

        public DataFormat()
        {            
        }

       
    }

    //public class CollisionAim
    //{
    //    public int RowID;
    //    public int SourceID;        

    //    public String AimName;
    //    public string AimDesc;

    //    public string TableName;
    //    public string Rules;
    //    public string Rules2;


    //    public int seq;
    //    public int status = 1;

    //}


    public class CompareAim
    {
        public int RowID;
        public int SourceID;

        public String AimName;
        public string AimDesc;

        public string TableName;
        public string Rules;
        public string Rules2;
        public string Rules3;


        public int t1;
        public int t2;
        public int t3;

        public int seq;
        public int status = 1;

        public int RuleType;

        /// <summary>
        /// 0 for compare
        /// 1 for check
        /// 2 for preprocess
        /// </summary>
        public RulesTypes TmpType;  
    }


    public class User
    {
        public int RowID { set; get; }
        public string Name { set; get; }
        public string Password { set; get; }
        public int isFisrt { set; get; }
        public string newPassword { set; get; }

    }

    public class CheckRules
    {
        public string CheckName;
        public string CheckSql;
        public int Type;

        public int t1;
        public int t2;
        public int t3;
    }
}

