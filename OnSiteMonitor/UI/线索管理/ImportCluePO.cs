using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace OnSiteFundComparer.UI.线索管理
{
    public class ImportCluePO : Erik.Utilities.Bases.BaseProgressiveOperation
    {
        private Dictionary<String, int> map = new Dictionary<string, int>();
        public DataTable importDT;
        public string DBFile { get; internal set; }
        private DAL.MySqlite resultDB;

        public ImportCluePO(DAL.MySqlite result, DataTable _dt)
        {
            importDT = _dt;
            InitItem();
            _currentStep = 0;
            if(importDT != null)
                _totalSteps = importDT.Rows.Count;

            if (result != null)
                resultDB = result;

            MainTitle = "导入数据库";
        }

        public override void Start()
        {
            OnOperationStart(EventArgs.Empty);
            

            try
            {

                resultDB.ExecuteNonQuery(InitTableSql);
                resultDB.ExecuteNonQuery(DeleteSql);
                resultDB.BeginTran();
                foreach (DataRow dr in importDT.Rows)
                {
                    if (_currentStep++ < 3)
                        continue;

                    var c = GetClue(dr);
                    if (c.ID.Length != 0)
                        resultDB.ExecuteNonQuery(CommandType.Text, InsertSql, GetUpdateParam(c));

                    OnOperationProgress(EventArgs.Empty);
                }
                resultDB.Commit();
            }
            catch (Exception ex)
            {
                resultDB.RollBack();
            }

            OnOperationEnd(EventArgs.Empty);
        }

        private int GetValue(string v)
        {
            int ret = 0;
            try
            {
                ret = map[v];
            }
            catch (Exception ex)
            {
                ret = 1;
            }

            return ret;
        }
        private void InitItem()
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
        public   SQLiteParameter[] GetUpdateParam(WFM.JW.HB.Models.Clues _clue)
        {
            SQLiteParameter[] sqlietParams = new SQLiteParameter[] {
                new SQLiteParameter("@ID", _clue.ID),
                new SQLiteParameter("@Name", _clue.Name),
                new SQLiteParameter("@Region", _clue.Region),
                new SQLiteParameter("@Type", _clue.Type),
                new SQLiteParameter("@Table1", _clue.Table1),
                new SQLiteParameter("@IsClueTrue", _clue.IsClueTrue),
                new SQLiteParameter("@IsCompliance", _clue.IsCompliance),
                new SQLiteParameter("@IsCP", _clue.IsCP),
                new SQLiteParameter("@Fact", _clue.Fact),
                new SQLiteParameter("@IllegalMoney", _clue.IllegalMoney),
                new SQLiteParameter("@CheckDate", _clue.CheckDate),
                new SQLiteParameter("@CheckByName1", _clue.CheckByName1),
                new SQLiteParameter("@CheckByName2", _clue.CheckByName2),
            };
            return sqlietParams;
        }
        #region Sql
        String DeleteSql = @"Delete from report_1";
        String InsertSql = @" INSERT INTO report_1
                     (ID,
                     Name,
                     Region,
                     Table1,
                     Type,
                     IsClueTrue,
                     IsCompliance,
                     IsCP,
                     Fact,
                     IllegalMoney,
                     CheckDate,
                     CheckByName1,
                     CheckByName2) VALUES
                     (@ID,
                     @Name,
                     @Region,
                     @Table1,
                     @Type,
                     @IsClueTrue,
                     @IsCompliance,
                     @IsCP,
                     @Fact,
                     @IllegalMoney,
                     @CheckDate,
                     @CheckByName1,
                     @CheckByName2)";

        String InitTableSql = @"CREATE TABLE  if not exists report_1 (RowID          INTEGER       PRIMARY KEY AUTOINCREMENT,
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

        String CreateView = @"CREATE VIEW IF NOT EXISTS vw_ImportClue AS
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
        #endregion
    }
}
