using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI.线索管理
{
    public class ImportCluePO_bak : Erik.Utilities.Bases.BaseProgressiveOperation
    {
        private String _filename;
        Dictionary<String, int> map = new Dictionary<string, int>();
        public DataSet ResultDS;

        public ImportCluePO_bak(String FileName)
        {
            _filename = FileName;
            _totalSteps = 10;

            MainTitle = "批量导入线索核查结果";
            SubTitle = "请稍后...";
            InitItem();
        }

        public string DBFile { get; internal set; }

        public override void Start()
        {
            _currentStep = 0;
            OnOperationStart(EventArgs.Empty);
            int start = 1;
            DAL.MySqlite result = new DAL.MySqlite(DBFile, GlobalEnviroment.isCryt);
            DAL.MySqlite config = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            result.AttchDatabase(config, "Config");
            try
            {
                DataTable dt = GlobalEnviroment.ReadXLSToDataTableTop(_filename);

                
                result.ExecuteNonQuery(InitTable());
                result.ExecuteNonQuery("Delete from report_1"); 

                _totalSteps = dt.Rows.Count * 2;
                foreach (DataRow dr in dt.Rows)
                {
                    if (start++ < 3)
                        continue;

                    _currentStep++;
                    var c = GetClue(dr);
                    var InsertSql = GetInsertSql();

                    if(c.ID.Length != 0)
                        result.ExecuteNonQuery(CommandType.Text, InsertSql, getUpdateParam(c));

                    OnOperationProgress(EventArgs.Empty);
                }

                result.ExecuteNonQuery(InitView());
                _totalSteps += dt.Rows.Count;
                result.ExecuteNonQuery(@"SELECT report.Table1,
                                                           report.id,
                                                           report.name,
                                                           report_1.Region,
                                                           report.addr,
                                                           config.DataItem.DataShortName,
                                                           report.type,
                                                           CASE WHEN report_1.IsClueTrue = 1 THEN '查实' ELSE '查否' END AS 核查情况,
                                                           CASE WHEN report_1.IsCompliance = 1 THEN '不符合' ELSE '符合' END AS 是否符合政策,
                                                           CASE WHEN report_1.IsCP = 1 THEN '党员' ELSE '群众' END AS 是否党员,
                                                           report_1.CheckDate,
                                                           report_1.Fact,
                                                           report_1.IllegalMoney,
                                                           report_1.CheckByName1,
                                                           report_1.CheckByName2
                                                      FROM report,
                                                           report_1,
                                                           config.DataItem
                                                     WHERE report.id = report_1.ID AND 
                                                           report.Table1 = report_1.Table1 AND 
                                                           report.Type = report_1.Type AND 
                                                           config.DataItem.RowID = report.table1");

            }
            catch (Exception ex)
            {
                MessageBox.Show("批量导入出错：" + ex.Message);
            }
            OnOperationEnd(EventArgs.Empty);
        }

        WFM.JW.HB.Models.Clues GetClue(DataRow dr)
        {
            WFM.JW.HB.Models.Clues c = new WFM.JW.HB.Models.Clues();

            c.ID = dr[0].ToString();
            c.Name = dr[1].ToString();
            c.Region = dr[2].ToString();
            c.Table1 = GetValue(dr[3].ToString());
            c.Type = dr[4].ToString();

            c.IsClueTrue = GetValue(dr[5].ToString());
            c.IsCompliance = GetValue(dr[6].ToString());
            c.IsCP = GetValue(dr[7].ToString());

            if (dr[8].ToString() != "")
            {
                System.DateTime dt;
                DateTime.TryParse(dr[8].ToString(), out dt);
                c.CheckDate = dt;
            }
            else c.CheckDate = System.DateTime.Now;

            c.Fact = dr[9].ToString();

            float tmpf = 0.0f;
            float.TryParse(dr[10].ToString(), out tmpf);
            c.IllegalMoney = tmpf;

            c.CheckByName1 = dr[11].ToString();            
            c.CheckByName2 = dr[12].ToString();

            return c;

        }

        private int GetValue(string v)
        {
            int ret = 0;
            try
            {
                ret = map[v];
            }
            catch(Exception ex)
            {
                ret = 1;
            }

            return ret;
        }
        private void InitItem( )
        {
            map["农村低保"] = 36;
            map["城市低保"] = 37;
            map["农村五保"] = 5;
            map["医疗救助"] = 6;
            map["保障房"] = 7;
            map["农村危房"] = 8;
            map["农业补贴"] = 10;
            map["退耕还林"] = 40;
            map["生态公益林"] = 35;
            map["属实"] = 1;
            map["不属实"] = 0;
            map["符合"] = 1;
            map["不符合"] = 0;
            map["是"] = 1;
            map["否"] = 0;
        }

        private string GetInsertSql()
        {
            return @" INSERT INTO report_1 (ID , Name , Region, Table1 , Type 
                                   , IsClueTrue  , IsCompliance ,  IsCP  , Fact , IllegalMoney , CheckDate , CheckByName1 , CheckByName2  )
                                 VALUES (  @ID , @Name , @Region, @Table1     , @Type    
                                           , @IsClueTrue  , @IsCompliance   ,  @IsCP  , @Fact   , @IllegalMoney 
                                           , @CheckDate   , @CheckByName1  , @CheckByName2  )";

        }

        public static System.Data.SQLite.SQLiteParameter[] getUpdateParam(WFM.JW.HB.Models.Clues _clue)
        {
            System.Data.SQLite.SQLiteParameter[] sqlietParams = new System.Data.SQLite.SQLiteParameter[] {
                new System.Data.SQLite.SQLiteParameter("@ID", _clue.ID),
                new System.Data.SQLite.SQLiteParameter("@Name", _clue.Name),
                new System.Data.SQLite.SQLiteParameter("@Region", _clue.Region),
                new System.Data.SQLite.SQLiteParameter("@Type", _clue.Type),
                new System.Data.SQLite.SQLiteParameter("@Table1", _clue.Table1),
                new System.Data.SQLite.SQLiteParameter("@IsClueTrue", _clue.IsClueTrue),
                new System.Data.SQLite.SQLiteParameter("@IsCompliance", _clue.IsCompliance),
                new System.Data.SQLite.SQLiteParameter("@IsCP", _clue.IsCP),
                new System.Data.SQLite.SQLiteParameter("@Fact", _clue.Fact),
                new System.Data.SQLite.SQLiteParameter("@IllegalMoney", _clue.IllegalMoney),
                new System.Data.SQLite.SQLiteParameter("@CheckDate", _clue.CheckDate),
                new System.Data.SQLite.SQLiteParameter("@CheckByName1", _clue.CheckByName1),
                new System.Data.SQLite.SQLiteParameter("@CheckByName2", _clue.CheckByName2),
            };
            return sqlietParams;
        }

        private string InitTable()
        {
            return @"CREATE TABLE  if not exists report_1 (RowID          INTEGER       PRIMARY KEY AUTOINCREMENT,
                                                     ID             VARCHAR (20),
                                                     Name           VARCHAR (20),
                                                     Addr           VARCHAR (100),
                                                     Region         VARCHAR (50),
                                                     Type           VARCHAR (50),
                                                     Amount         VARCHAR (20),
                                                     DateRange      VARCHAR (200),
                                                     Table1         INTERGER,
                                                     Table2         INTERGER,   
                                                     IsConfirmed     INTERGER DEFAULT (0),
                                                     IsClueTrue     INTERGER,
                                                     IsCompliance    INTERGER,
                                                     IsCP           INTERGER,
                                                     Fact           VARCHAR (500),
                                                     IllegalMoney   real,
                                                     CheckDate      DATETIME,
                                                     CheckByName1   VARCHAR (10),
                                                     CheckByName2   VARCHAR (10),
                                                     ReCheckFact    VARCHAR(100),
                                                     ReCheckType    INTERGER,
                                                     ReCheckByName1 VARCHAR (10),
                                                     IsUploaded     INTERGER DEFAULT (0))";
        }

        private string InitView()
        {
            return @"CREATE VIEW IF NOT EXISTS vw_ImportClue AS
                                SELECT report.RowID AS RowID,
                                       report.ID AS ID,
                                       report.Name AS Name,
                                       report.Addr AS addr,
                                       report_1.Region AS Region,
                                       report.Type AS type,
                                       report.Amount AS Amount,
                                       report.DateRange AS DateRange,
                                       report.table1 AS table1,
                                       report.Table2 AS Table2,
                                       report.IsConfirmed AS IsConfirmed,
                                       report_1.IsClueTrue AS IsClueTrue,
                                       report_1.IsCompliance AS IsCompliance,
                                       report_1.IsCP AS IsCP,
                                       report_1.Fact AS Fact,
                                       report_1.IllegalMoney AS IllegalMoney,
                                       report_1.CheckDate AS CheckDate,
                                       report_1.CheckByName1 AS CheckByName1,
                                       report_1.CheckByName2 AS CheckByName2,
                                       report_1.ReCheckByName1 AS ReCheckByName1
                                  FROM report,
                                       report_1
                                 WHERE report.id = report_1.ID AND 
                                       report.Table1 = report_1.Table1 AND 
                                       report.Type = report_1.Type";
        }
    }
}
