using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WFM.JW.HB.Models;

namespace WFMRegionCluesReport
{
    public partial class Form1 : Form
    {
        DAL.MyDataBase sqlserver = new DAL.MyDataBase();

        DataSet ds;

        List<WFMUploadTask> tList = new List<WFMUploadTask>();
        List<CluesReport> crList = new List<CluesReport>();


        List<WFMUploadTask> tList1 = new List<WFMUploadTask>();
        List<CluesReport> crList1 = new List<CluesReport>();

        struct data
        {
           public  string regionname;
            public string confirmed;
            public string iscluetrue;
        }

        List<data> dClues = new List<data>();

        public Form1()
        {
            InitializeComponent();

            sqlserver.DBConnectionString = Properties.Settings.Default.ConnStr;//"Provider = SQLNCLI11; Data Source = 192.168.10.1; Initial Catalog = WFM-CM; User ID = sa; Password = 123456";
            //sqlserver.DBConnectionString = "Provider = SQLNCLI11; Data Source = 192.168.10.1; Initial Catalog = WFM-CM; User ID = sa; Password = 123456";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //            ds = sqlserver.ExecuteDataset(@"SELECT     ParentName, RegionName, UserName, FilePath, FileName, Status, CreateTime, TaskName, pseq, seq
            //FROM         (SELECT     HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, WFM_UploadTask.UserName, WFM_UploadTask.FilePath, 
            //                                              WFM_UploadTask.FileName, WFM_UploadTask.Status, WFM_UploadTask.CreateTime, WFM_UploadTask.TaskName, HB_NewRegion_1.Seq AS pseq, 
            //                                              HB_NewRegion.Seq AS seq
            //                       FROM          WFM_UploadTask INNER JOIN
            //                                              WFM_NewRegionUser ON WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
            //                                              HB_NewRegion ON WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
            //                                              HB_NewRegion AS HB_NewRegion_1 ON HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
            //                       WHERE      (WFM_UploadTask.Status = 20) AND (WFM_UploadTask.TStatus = 1)
            //                       UNION
            //                       SELECT     HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, WFM_UploadTask.UserName, WFM_UploadTask.FilePath, 
            //                                             WFM_UploadTask.FileName, WFM_UploadTask.Status, WFM_UploadTask.CreateTime, WFM_UploadTask.TaskName, HB_NewRegion_1.Seq AS pseq, 
            //                                             HB_NewRegion.Seq AS seq
            //                       FROM         WFM_UploadTask INNER JOIN
            //                                             WFM_NewRegionUser ON WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
            //                                             HB_NewRegion ON WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
            //                                             HB_NewRegion AS HB_NewRegion_1 ON HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
            //                       WHERE     (WFM_UploadTask.CreateTime =
            //                                                 (SELECT     MAX(CreateTime) AS Expr1
            //                                                   FROM          WFM_UploadTask AS a
            //                                                   WHERE      (UserName = WFM_UploadTask.UserName))) AND (WFM_UploadTask.Status = 10)) AS a
            //ORDER BY pseq, seq");


            ds = sqlserver.ExecuteDataset(@"
SELECT   ParentName, RegionName, UserName, FilePath, FileName, Status, CreateTime, TaskName, pseq, seq
FROM      (SELECT   HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, WFM_UploadTask.UserName, 
                                 WFM_UploadTask.FilePath, WFM_UploadTask.FileName, WFM_UploadTask.Status, 
                                 WFM_UploadTask.CreateTime, WFM_UploadTask.TaskName, HB_NewRegion_1.Seq AS pseq, 
                                 HB_NewRegion.Seq AS seq
                 FROM      WFM_UploadTask INNER JOIN
                                 WFM_NewRegionUser ON WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
                                 HB_NewRegion ON WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
                                 HB_NewRegion AS HB_NewRegion_1 ON 
                                 HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
                 WHERE   (WFM_UploadTask.Status = 20) AND (WFM_UploadTask.TStatus = 1)
                 UNION
                 SELECT   HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, WFM_UploadTask.UserName, 
                                 WFM_UploadTask.FilePath, WFM_UploadTask.FileName, WFM_UploadTask.Status, 
                                 WFM_UploadTask.CreateTime, WFM_UploadTask.TaskName, HB_NewRegion_1.Seq AS pseq, 
                                 HB_NewRegion.Seq AS seq
                 FROM      WFM_UploadTask INNER JOIN
                                 WFM_NewRegionUser ON WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
                                 HB_NewRegion ON WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
                                 HB_NewRegion AS HB_NewRegion_1 ON 
                                 HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
                 WHERE   (WFM_UploadTask.UserName NOT IN
                                     (SELECT   WFM_UploadTask.UserName
                                      FROM      WFM_UploadTask INNER JOIN
                                                      WFM_NewRegionUser ON 
                                                      WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
                                                      HB_NewRegion ON 
                                                      WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
                                                      HB_NewRegion AS HB_NewRegion_1 ON 
                                                      HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
                                      WHERE   (WFM_UploadTask.Status = 20) AND (WFM_UploadTask.TStatus = 1))) AND 
                                 (WFM_UploadTask.CreateTime =
                                     (SELECT   MAX(CreateTime) AS Expr1
                                      FROM      WFM_UploadTask AS a
                                      WHERE   (UserName = WFM_UploadTask.UserName))) AND (WFM_UploadTask.Status = 10)) AS a
ORDER BY pseq, seq");


            dataGridView1.DataSource = ds.Tables[0];
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return;

            var l = from dr in ds.Tables[0].AsEnumerable()
                    select new WFMUploadTask
                    {
                        ParentName = dr.Field<String>("ParentName"),
                        RegionName = dr.Field<String>("RegionName"),
                        UserName = dr.Field<String>("UserName"),
                        FilePath = dr.Field<String>("FilePath"),
                        FileName = dr.Field<String>("FileName"),
                        CreateTime = dr.Field<DateTime>("CreateTime"),
                        ClientTaskName = dr.Field<String>("TaskName")
                    };

            tList = l.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tList == null || tList.Count() == 0)
            {
                MessageBox.Show("请先获取数据库记录！");
                return;
            }

            ReadUploadFilePO po = new ReadUploadFilePO(tList);
            Erik.Utilities.Lib.StartProgressiveOperation(po, this);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = po.crList;

            crList = po.crList.ToList();


        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (crList == null || crList.Count() == 0)
            {
                MessageBox.Show("请先获取统计结果！");
                return;
            }


            ds = sqlserver.ExecuteDataset(@"SELECT   HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, HB_NewRegion.RegionLevel, 
                                                                    HB_NewRegion.DirectCity, WFM_NewRegionUser.UserName
                                                    FROM      HB_NewRegion INNER JOIN
                                                                    HB_NewRegion AS HB_NewRegion_1 ON 
                                                                    HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID LEFT OUTER JOIN
                                                                    WFM_NewRegionUser ON HB_NewRegion.RegionGUID = WFM_NewRegionUser.RegionGUID
                                                    WHERE   ((HB_NewRegion.RegionLevel = 2) OR
                                                                    (HB_NewRegion.DirectCity = 1)) and len(HB_NewRegion.RegionName)  >0
                                                    ORDER BY HB_NewRegion.DirectCity , HB_NewRegion_1.Seq ,HB_NewRegion.Seq");


            var rList = from dr in ds.Tables[0].AsEnumerable()
                        select new
                        {
                            ParentName = dr.Field<String>("ParentName"),
                            RegionName = dr.Field<String>("RegionName"),
                            UserName = dr.Field<String>("UserName"),
                        };




            var result = from r in rList
                         join cr in crList on r.UserName equals cr.Username into tmpList
                         from tt in tmpList.DefaultIfEmpty()
                         select new
                         {
                             ParentName = r.ParentName,
                             RegionName = r.RegionName,
                             UserName = r.UserName,
                             TotalClues = tt == null ? 0 : tt.TotalClues,
                             InputError = tt == null ? 0 : tt.InputError,
                             Clues = tt == null ? 0 : tt.Clues,
                             CreateDate = tt == null ? System.DateTime.MinValue : tt.UploadDate,
                             Status = tt.Status
                         };

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = result.ToList();
        }

        private void SaveFile(List<string> result, string filename)
        {
            using (FileStream fs = new FileStream(
                      filename, FileMode.Append, FileAccess.Write))
            {
                foreach (var s in result)
                {
                    byte[] arrWriteData = Encoding.Default.GetBytes(s);
                    fs.Write(arrWriteData, 0, arrWriteData.Length);
                }
                fs.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ds = sqlserver.ExecuteDataset(@"SELECT HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, WFM_UploadTask.UserName, WFM_UploadTask.FilePath, 
                                             WFM_UploadTask.FileName, WFM_UploadTask.Status, WFM_UploadTask.CreateTime, WFM_UploadTask.TaskName, HB_NewRegion_1.Seq AS pseq,                                              
                                            HB_NewRegion.Seq AS seq,WFM_UploadTask.Status
                       FROM          WFM_UploadTask INNER JOIN
                                              WFM_NewRegionUser ON WFM_UploadTask.UserName = WFM_NewRegionUser.UserName INNER JOIN
                                              HB_NewRegion ON WFM_NewRegionUser.RegionGUID = HB_NewRegion.RegionGUID INNER JOIN
                                             HB_NewRegion AS HB_NewRegion_1 ON HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID
WHERE      (WFM_UploadTask.Status >= 10 )
order by pseq, seq, createtime 
");


            dataGridView2.DataSource = ds.Tables[0];
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return;


            var l = from dr in ds.Tables[0].AsEnumerable()
                    select new WFMUploadTask
                    {
                        ParentName = dr.Field<String>("ParentName"),
                        RegionName = dr.Field<String>("RegionName"),
                        UserName = dr.Field<String>("UserName"),
                        FilePath = dr.Field<String>("FilePath"),
                        FileName = dr.Field<String>("FileName"),
                        CreateTime = dr.Field<DateTime>("CreateTime"),
                        ClientTaskName = dr.Field<String>("TaskName"),
                        Status = dr.Field<int>("Status")
                    };

            tList1 = l.ToList();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tList1 == null || tList1.Count() == 0)
            {
                MessageBox.Show("请先获取数据库记录！");
                return;
            }

            ReadUploadFilePO po = new ReadUploadFilePO(tList1);
            Erik.Utilities.Lib.StartProgressiveOperation(po, this);

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = po.crList;

            crList1 = po.crList;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (crList1 == null || crList1.Count() == 0)
            {
                MessageBox.Show("请先获取统计结果！");
                return;
            }


            ds = sqlserver.ExecuteDataset(@"SELECT   HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, HB_NewRegion.RegionLevel, 
                                                                    HB_NewRegion.DirectCity, WFM_NewRegionUser.UserName,  HB_NewRegion.RegionCode
                                                    FROM      HB_NewRegion INNER JOIN
                                                                    HB_NewRegion AS HB_NewRegion_1 ON 
                                                                    HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID LEFT OUTER JOIN
                                                                    WFM_NewRegionUser ON HB_NewRegion.RegionGUID = WFM_NewRegionUser.RegionGUID
                                                    WHERE   ((HB_NewRegion.RegionLevel = 2) OR
                                                                    (HB_NewRegion.DirectCity = 1)) and len(HB_NewRegion.RegionName)  >0
                                                    ORDER BY HB_NewRegion.DirectCity , HB_NewRegion_1.Seq ,HB_NewRegion.Seq");


            var rList = from dr in ds.Tables[0].AsEnumerable()
                        select new
                        {
                            ParentName = dr.Field<String>("ParentName"),
                            RegionName = dr.Field<String>("RegionName"),
                            UserName = dr.Field<String>("UserName"),
                            RegionCode = dr.Field<string>("regioncode"),
                            Confirmed = "0",
                            IsTrue = "0"                      
                        };

            var result = from r in rList
                         join cr in crList1 on r.UserName equals cr.Username into tmpList
                         from tt in tmpList.DefaultIfEmpty()
                         select new
                         {
                             ParentName = r.ParentName,
                             RegionName = r.RegionName,
                             UserName = r.UserName,
                             RegionCode = r.RegionCode,
                             TotalClues = tt == null ? 0 : tt.TotalClues,
                             InputError = tt == null ? 0 : tt.InputError,
                             Clues = tt == null ? 0 : tt.Clues,
                             CreateDate = tt == null ? System.DateTime.MinValue : tt.UploadDate,
                             Status = tt == null ? "" : tt.Status == 20 ? "上报数据" : ""
                         };
           
           

           foreach ( var r1 in rList )
           {
                // ds = sqlserver.ExecuteDataset(@"");

                String sql = "select sum(confirmed), sum(isnull(iscluetrue,0)) from ClueData_@regioncode";
                var o = sqlserver.ExecuteDataset(sql.Replace("@regioncode", r1.RegionCode));

                if (o.Tables[0] == null || o.Tables[0].Rows.Count <= 0)
                    continue;


                data d = new data();
                d.regionname = r1.RegionName;
                d.confirmed = o.Tables[0].Rows[0][0].ToString();
                d.iscluetrue = o.Tables[0].Rows[0][1].ToString();

                dClues.Add(d);

                var v = result.Where(x => x.RegionName == r1.RegionName);
            }
           
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = result.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (tbSql.Text.Length == 0)
                return;

            String Sql = tbSql.Text;

            if (tbSql.SelectionLength != 0)
                Sql = tbSql.SelectedText;

            ds = sqlserver.ExecuteDataset(@"SELECT   HB_NewRegion_1.RegionName AS ParentName, HB_NewRegion.RegionName, HB_NewRegion.RegionLevel, 
                                                                    HB_NewRegion.DirectCity, WFM_NewRegionUser.UserName,  HB_NewRegion.RegionCode
                                                    FROM      HB_NewRegion INNER JOIN
                                                                    HB_NewRegion AS HB_NewRegion_1 ON 
                                                                    HB_NewRegion.ParentGUID = HB_NewRegion_1.RegionGUID LEFT OUTER JOIN
                                                                    WFM_NewRegionUser ON HB_NewRegion.RegionGUID = WFM_NewRegionUser.RegionGUID
                                                    WHERE   ((HB_NewRegion.RegionLevel = 2) OR
                                                                    (HB_NewRegion.DirectCity = 1)) and len(HB_NewRegion.RegionName)  >0
                                                    ORDER BY HB_NewRegion.DirectCity , HB_NewRegion_1.Seq ,HB_NewRegion.Seq");

            var rList = from dr in ds.Tables[0].AsEnumerable()
                        select new CluesRegion
                        {
                            ParentName = dr.Field<String>("ParentName"),
                            RegionName = dr.Field<String>("RegionName"),
                            UserName = dr.Field<String>("UserName"),
                            RegionCode = dr.Field<string>("regioncode"),
                            Confirmed = "0",
                            IsTrue = "0"
                        };

            ExecSqlPO sqlPO = new ExecSqlPO(sqlserver, rList, Sql);
            Erik.Utilities.Lib.StartProgressiveOperation(sqlPO, this);
            this.Cursor = Cursors.WaitCursor;
            if (sqlPO.ResultDS != null)
                dataGridView3.DataSource = sqlPO.ResultDS.Tables[0];
            else dataGridView3.DataSource = null;
            this.Cursor = Cursors.Arrow;
        }
    }

   
}
