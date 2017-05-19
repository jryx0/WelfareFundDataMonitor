using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace OnSiteFundComparer.UI.线索管理
{
    public class ImportExcelCluesPO : ImportExcelPO
    {
        public DAL.MySqlite resultDB;
        public DAL.MySqlite MainDB;
        public DataTable UnSuccessDT;

        //ateTime start;
         

        public ImportExcelCluesPO(String DBFile, String xlsFile) : base(xlsFile)
        {
            //resultDB = new DAL.MySqlite(DBFile);
            

            OperationStart += (sender, e) =>
            {
                InitDB( );
            };

            OperationProgress += (sender, e) =>
             {
                 DoWork(e);
             };

            OperationEnd += (sender, e) =>
             {
                 Finish();
             };

            MainTitle = "正在读取文件:" + Path.GetFileName(xlsFile); 
        }

        private void Finish()
        {
            try
            {
                resultDB.Commit();
                var ds = resultDB.ExecuteDataset(ImportDataSql);
                if (ds != null) //List<clues> 
                    this.resultDT = ds.Tables[0];

                ds = resultDB.ExecuteDataset(UnSuccessSql);

                if (ds != null) //List<clues> 
                    UnSuccessDT = ds.Tables[0];

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                
            }

           // var TimeSpan = DateTime.Now - start;

            //System.Windows.Forms.MessageBox.Show(TimeSpan.TotalSeconds.ToString() );
        }

        void InitDB( )
        {
            //start = DateTime.Now;

            resultDB.ExecuteNonQuery(InitTableSql);
            resultDB.ExecuteNonQuery(DeleteSql);

            resultDB.BeginTran();
            
        }


        void DoWork(EventArgs e)
        {
            if (e == EventArgs.Empty)
                return;

            ExcelEventArgs er = (ExcelEventArgs)e;

            if (er.LineNumber < 3)
                return;
            if (er.dataRow == null)
                return;

            if (er.dataRow.ItemArray.Count() < 11)
                return;

            if (er.dataRow[2].ToString().Length < 15)
                return;

            float tmpf = 0.0f;
            float.TryParse(er.dataRow[9].ToString(), out tmpf);

            SQLiteParameter[] sqlietParams = new SQLiteParameter[] {
                new SQLiteParameter("@Region", er.dataRow[0].ToString()), //乡镇街道
                new SQLiteParameter("@Table1", er.dataRow[1].ToString()),//项目名称                             
                new SQLiteParameter("@Name",  er.dataRow[3].ToString()), //姓名
                new SQLiteParameter("@Addr",  er.dataRow[4].ToString()), //地址
                new SQLiteParameter("@ID", GlobalEnviroment.GetFullIDEx( er.dataRow[2].ToString())), //地址
                new SQLiteParameter("@Fact", er.dataRow[6].ToString()), //简要事实
                new SQLiteParameter("@IsClueTrue", er.dataRow[7].ToString()), //是否属实                
                new SQLiteParameter("@IsCP", er.dataRow[8].ToString()), //是否党员             
                
                new SQLiteParameter("@CheckDate",GlobalEnviroment.String2Date(er.dataRow[9].ToString()).ToString("yyyy-MM-dd")),
              //  new SQLiteParameter("@IllegalMoney", er.dataRow[9].ToString()),
                new SQLiteParameter("@CheckByName1", er.dataRow[10].ToString()) 
           //     new SQLiteParameter("@CheckByName2", er.dataRow[10].ToString()),
            };

            try
            {
                resultDB.ExecuteNonQuery(System.Data.CommandType.Text, InsertSql, sqlietParams);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                resultDB.RollBack();
            }
        }


        internal void Update()
        {
             
        }

        internal void Clear()
        {
            //throw new NotImplementedException();
        }

        #region Sql
        String InitTableSql = @"CREATE TABLE  if not exists report_1 (RowID          INTEGER       PRIMARY KEY AUTOINCREMENT,
                                                     ID             VARCHAR (20),
                                                     Name           VARCHAR (20),
                                                     Addr           VARCHAR (100),
                                                     Region         VARCHAR (50),
                                                     Type           VARCHAR (50),                                                     
                                                     Table1         VARCHAR (10),
                                                     IsClueTrue     VARCHAR (10),
                                                     IsCompliance   VARCHAR (100),
                                                     IsCP           VARCHAR (100),
                                                     Fact           VARCHAR (500),
                                                     IllegalMoney   DOUBLE,
                                                     CheckDate      DateTime,
                                                     CheckByName1   VARCHAR (10),
                                                     CheckByName2   VARCHAR (10))";
        String DeleteSql = @"Delete from report_1";
        String InsertSql = @" INSERT INTO report_1
                     (ID,
                     Name,
                     Addr,
                     Region,
                     Table1,  
                                                         
                     IsClueTrue,                  
                     IsCP,
                     Fact,
                      
                     CheckDate,
                     CheckByName1) VALUES
                     (@ID,
                     @Name,
                     @Addr,
                     @Region,
                     @Table1,
                    
                     @IsClueTrue,                   
                     @IsCP,
                     @Fact,
                     
                     @CheckDate,
                     @CheckByName1)";

        String ImportDataSql = @"SELECT Clue_report.RowID AS RowID,Clue_report.table1 as Table1,Clue_report.IsConfirmed AS IsConfirmed,
                                    report_1.Region AS 乡镇街道,
                                   report_1.Table1 AS 项目类型,
                                   Clue_report.ID AS 身份证号,
                                   report_1.Name AS 姓名,
                                   report_1.Addr AS 详细地址,                                   
                                   Clue_report.Type AS 线索类型,
                                   report_1.IsClueTrue AS 是否属实,
                                   report_1.IsCP AS 是否党员,
                                   report_1.Fact AS 核查说明,                                 
                                   strftime(report_1.CheckDate ) AS 核查日期,
                                   report_1.CheckByName1 AS 核查人1
                              FROM Clue_report,
                                   report_1,
                                   config.DataItem
                             WHERE Clue_report.id = report_1.ID AND 
                                   config.DataItem.RowID = Clue_report.Table1 AND 
                                   config.DataItem.DataShortName = report_1.Table1";

        String UnSuccessSql = @"
                                SELECT '' as 序号, Table1 AS 项目内容,
                                               ID 身份证号,
                                               Name 姓名,
                                               region 乡镇,
                                               addr 地址,
                                               iscluetrue 是否属实,
                                               IsCP 是否党员,
                                               Fact 简要事实,
                                               checkDate 检查时间,
                                               checkbyName1 检查人
                                          FROM report_1
                                         WHERE id NOT IN
                                               (SELECT Clue_report.ID AS 身份证号
                                                  FROM Clue_report,
                                                       report_1,
                                                       config.DataItem
                                                 WHERE Clue_report.id = report_1.ID AND 
                                                       config.DataItem.RowID = Clue_report.Table1 AND 
                                                       config.DataItem.DataShortName = report_1.Table1)";
        //String UnSuccessSql = @"
        //                        SELECT '' as 序号, result.report_1.Table1 AS 项目内容,
        //                                       result.report_1.ID 身份证号,
        //                                       result.report_1.Name 姓名,
        //                                       result.report_1.region 乡镇,
        //                                       result.report_1.addr 地址,
        //                                       result.report_1.iscluetrue 是否属实,
        //                                       result.report_1.IsCP 是否党员,
        //                                       result.report_1.Fact 简要事实,
        //                                       result.report_1.checkDate 检查时间,
        //                                       result.report_1.checkbyName1 检查人
        //                                  FROM result.report_1
        //                                 WHERE result.report_1.id NOT IN
        //                                       (SELECT result.Clue_report.ID AS 身份证号
        //                                          FROM result.Clue_report,
        //                                               result.report_1,
        //                                               DataItem
        //                                         WHERE result.Clue_report.id = result.report_1.ID AND 
        //                                               DataItem.RowID = result.Clue_report.Table1 AND 
        //                                               DataItem.DataShortName = result.report_1.Table1)";

        #endregion
    }
}
