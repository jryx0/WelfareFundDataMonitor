using System;
using System.Xml;
using CarlosAg.ExcelXmlWriter;
using System.Data;

namespace OnSiteFundComparer.Business
{

    public class SaveToExcelXml
    {
        Workbook Book;
        public SaveToExcelXml()
        {
            Book = new Workbook();
            GenerateStyles(Book.Styles);

            WorkbookInit(Book);
        }

        private void WorkbookInit(Workbook _b)
        {
            // -----------------------------------------------
            //  Properties
            // -----------------------------------------------
            _b.Properties.Author = "湖北纪委党风室";
            _b.Properties.Created = new System.DateTime(2016, 4, 12, 10, 57, 32, 0);
            _b.Properties.LastSaved = new System.DateTime(2016, 4, 21, 9, 34, 52, 0);
            _b.ExcelWorkbook.WindowHeight = 8520;
            _b.ExcelWorkbook.WindowWidth = 20400;
            _b.ExcelWorkbook.ActiveSheetIndex = 1;
            _b.ExcelWorkbook.ProtectWindows = false;
            _b.ExcelWorkbook.ProtectStructure = false;
        }

        public void AddItemSheet(String ItemName, String Title, DataRow[] Rows)
        {
            Worksheet sheet = Book.Worksheets.Add(ItemName);
            sheet.Table.DefaultRowHeight = 15.75F;
            sheet.Table.DefaultColumnWidth = 54F;
            sheet.Table.ExpandedColumnCount = 8;
            sheet.Table.ExpandedRowCount = 17;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "Default";

            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 37;
            WorksheetCell cell;
            cell = Row0.Cells.Add();
            cell.StyleID = "sTitle";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Title;
            cell.MergeAcross = 7;
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";

            #region colums
            var column0 = sheet.Table.Columns.Add();
            column0.Index = 1;
            column0.Width = 25;
            column0.StyleID = "Default";

            var column1 = sheet.Table.Columns.Add();
            column1.Index = 2;
            column1.Width = 85;
            column1.StyleID = "Default";

            var column2 = sheet.Table.Columns.Add();
            column2.Index = 3;
            column2.Width = 120;
            column2.StyleID = "Default";

            var column3 = sheet.Table.Columns.Add();
            column3.Index = 4;
            column3.Width = 85;
            column3.StyleID = "Default";

            var column4 = sheet.Table.Columns.Add();
            column4.Index = 5;
            column4.Width = 200;
            column4.StyleID = "Default";

            var column5 = sheet.Table.Columns.Add();
            column5.Index = 6;
            column5.Width = 200;
            column5.StyleID = "Default";

            var column6 = sheet.Table.Columns.Add();
            column6.Index = 7;
            column6.Width = 200;
            column6.StyleID = "Default";

            var column7 = sheet.Table.Columns.Add();
            column7.Index = 8;
            column7.Width = 300;
            column7.StyleID = "Default";



            #endregion

            WorksheetRow sRow = sheet.Table.Rows.Add();
            sRow.Height = 20;

            WorksheetCell scell = sRow.Cells.Add();
            scell.StyleID = "ss";
            scell.Data.Type = DataType.String;
            scell.Data.Text = "总数：";
            scell.MergeAcross = 2;

            WorksheetCell dcell = sRow.Cells.Add();
            dcell.StyleID = "ss1";
            dcell.Data.Type = DataType.String;
            dcell.Data.Text = "日期：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            dcell.MergeAcross = 4;

            WorksheetRow RowHeader = sheet.Table.Rows.Add();
            RowHeader.Cells.Add("序号", DataType.String, "sHeader");
            RowHeader.Cells.Add("乡镇街道", DataType.String, "sHeader");
            RowHeader.Cells.Add("身份证号", DataType.String, "sHeader");
            RowHeader.Cells.Add("姓名", DataType.String, "sHeader");
            RowHeader.Cells.Add("地址", DataType.String, "sHeader");
            RowHeader.Cells.Add("线索类型", DataType.String, "sHeader");
            RowHeader.Cells.Add("领取情况", DataType.String, "sHeader");
            RowHeader.Cells.Add("备注", DataType.String, "sHeader");
           
            int Numbers = 1;
            foreach (DataRow dr in Rows)
            {
                WorksheetRow wRow = sheet.Table.Rows.Add();
                wRow.Height = 18;

                wRow.Cells.Add(Numbers.ToString(), DataType.Number, "sNum");


                foreach (object item in dr.ItemArray)
                {
                    var c = wRow.Cells.Add();
                    if (item.GetType() == typeof(Int32) || item.GetType() == typeof(Int64))
                    {
                        //c.StyleID = "sNum";
                        //c.Data.Text = item.ToString();
                        //c.Data.Type = DataType.Integer;
                        wRow.Cells.Add(item.ToString(), DataType.String, "sTxt");
                    }
                    else
                    {
                        c.StyleID = "sTxt";
                        c.Data.Text = item.ToString();
                        c.Data.Type = DataType.String;

                        if (wRow.Cells.Count >= 6)
                            c.StyleID = "sTxt_Warp";
                        //wRow.Cells.Add(item.ToString(), DataType.String, "sTxt");
                    }
                }

                Numbers++;
            }
            Numbers--;
            scell.Data.Text += Numbers.ToString() + "条";
        }
        public void AddItemSheet(String ItemName, String Title, DataTable dt)
        {
            AddItemSheet(ItemName, Title, dt.Select("1 = 1") );
        }

        public void AddSummarySheet(String Title, DataTable dt)
        {
            Worksheet sheet = Book.Worksheets.Add("汇总");
            sheet.Table.DefaultRowHeight = 15.75F;
            sheet.Table.DefaultColumnWidth = 54F;
            sheet.Table.ExpandedColumnCount = 8;
            sheet.Table.ExpandedRowCount = 17;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "Default";

            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 37;
            WorksheetCell cell;
            cell = Row0.Cells.Add();
            cell.StyleID = "sTitle";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Title;
            cell.MergeAcross = 4;
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
                       

            #region colums
            var column0 = sheet.Table.Columns.Add();
            column0.Index = 1;
            column0.Width = 50;
            column0.StyleID = "Default";

            var column1 = sheet.Table.Columns.Add();
            column1.Index = 2;
            column1.Width = 100;
            column1.StyleID = "Default";

            var column2 = sheet.Table.Columns.Add();
            column2.Index = 3;
            column2.Width = 100;
            column2.StyleID = "Default";

            var column3 = sheet.Table.Columns.Add();
            column3.Index = 4;
            column3.Width = 100;
            column3.StyleID = "Default";

            var column5 = sheet.Table.Columns.Add();
            column5.Index = 5;
            column5.Width = 100;
            column5.StyleID = "Default";
            #endregion


            WorksheetRow sRow = sheet.Table.Rows.Add();
            sRow.Height = 20;
            WorksheetCell dcell = sRow.Cells.Add();
            dcell.StyleID = "Total_ss1";
            dcell.Data.Type = DataType.String;
            dcell.Data.Text = "日期：" + System.DateTime.Now.ToString("yyyy-MM-dd");
            dcell.MergeAcross = 4;
            

            WorksheetRow RowHeader = sheet.Table.Rows.Add();
            RowHeader.Cells.Add("序号", DataType.String, "Total_Header");
            RowHeader.Cells.Add("项目名称", DataType.String, "Total_Header");
            RowHeader.Cells.Add("线索总数", DataType.String, "Total_Header");
            RowHeader.Cells.Add("问题线索", DataType.String, "Total_Header");
            RowHeader.Cells.Add("录入问题", DataType.String, "Total_Header");
            RowHeader.Height = 28;

            int Numbers = 1;
            foreach (DataRow dr in dt.Rows)
            {
                WorksheetRow wRow = sheet.Table.Rows.Add();
                wRow.Height = 25;
                wRow.Cells.Add(Numbers.ToString(), DataType.Number, "Total_Num");
                foreach (object item in dr.ItemArray)
                {
                    if (item.GetType() == typeof(Int32) || item.GetType() == typeof(Int64))
                        wRow.Cells.Add(item.ToString(), DataType.Number, "Total_Num");
                    else
                        wRow.Cells.Add(item.ToString(), DataType.String, "Total_Txt");
                }
                Numbers++;
            }
        }


        public void SaveExcelXml(string filename)
        {
            Book.Save(filename);
        }

        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
        /*    WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Simsum";          

            // -----------------------------------------------
            //  标题
            // -----------------------------------------------
            WorksheetStyle sTitle = styles.Add("sTitle");
            sTitle.Font.Bold = true;
            sTitle.Font.FontName = "simsun";
            sTitle.Font.Size = 18;
            sTitle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            sTitle.NumberFormat = "@";

            // -----------------------------------------------
            //  汇总
            // -----------------------------------------------
            WorksheetStyle sSum = styles.Add("sSum");
            sSum.Font.FontName = "Simsum";
            sSum.Font.Size = 12;
            sSum.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            sSum.NumberFormat = "@";


            // -----------------------------------------------
            //  表头
            // -----------------------------------------------
            WorksheetStyle sHeader = styles.Add("sHeader");
            sHeader.Font.FontName = "Simsum";
            sHeader.Font.Bold = true;
            sHeader.Font.Size = 11;
            sHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            sHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sHeader.NumberFormat = "@";

            // -----------------------------------------------
            //  表体
            // -----------------------------------------------
            WorksheetStyle sBody = styles.Add("sTxt");
            sBody.Font.FontName = "Simsum";
            sBody.Font.Size = 10;
            sBody.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sBody.NumberFormat = "@"; //文本

            WorksheetStyle sTxt_shrink = styles.Add("sTxt_shrink");
            sTxt_shrink.Font.FontName = "Simsum";
            sTxt_shrink.Font.Size = 10;
            sTxt_shrink.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sTxt_shrink.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sTxt_shrink.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sTxt_shrink.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sTxt_shrink.NumberFormat = "@"; //文本
            sTxt_shrink.Alignment.ShrinkToFit = true;

            WorksheetStyle sTxt_Warp = styles.Add("sTxt_Warp");
            sTxt_Warp.Font.FontName = "Simsum";
            sTxt_Warp.Font.Size = 10;
            sTxt_Warp.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sTxt_Warp.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sTxt_Warp.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sTxt_Warp.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sTxt_Warp.NumberFormat = "@"; //文本
            sTxt_Warp.Alignment.WrapText = true;

            // -----------------------------------------------
            //  表体
            // -----------------------------------------------
            WorksheetStyle sNum = styles.Add("sNum");
            sNum.Font.FontName = "Simsum";
            sNum.Font.Size = 10;
            sNum.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sNum.NumberFormat = "0_"; //数字

            //小计
            WorksheetStyle ss = styles.Add("ss");
            ss.Font.FontName = "Simsum";
            ss.Font.Size = 10;          
            ss.NumberFormat = "@"; //文本
            WorksheetStyle ss1 = styles.Add("ss1");
            ss1.Font.FontName = "Simsum";
            ss1.Font.Size = 10;
            ss1.NumberFormat = "@"; //文本
            ss1.Alignment.Horizontal = StyleHorizontalAlignment.Right; 

            // -----------------------------------------------
            //  总表
            // -----------------------------------------------
            WorksheetStyle Total = styles.Add("Total_Header");            
            Total.Font.FontName = "Simsum";
            Total.Font.Size = 16;
            Total.Font.Bold = true;
            Total.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Total.NumberFormat = "@";
            Total.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            Total.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            Total.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            Total.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            WorksheetStyle Total_Txt = styles.Add("Total_Txt");
            Total_Txt.Font.FontName = "Simsum";
            Total_Txt.Font.Size = 16;
            Total_Txt.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Total_Txt.NumberFormat = "@";
            Total_Txt.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            Total_Txt.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            Total_Txt.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            Total_Txt.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            WorksheetStyle Total_Num = styles.Add("Total_Num");
            Total_Num.Font.FontName = "Simsum";
            Total_Num.Font.Size = 16;
            Total_Num.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Total_Num.NumberFormat = "0_";

            Total_Num.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            Total_Num.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            Total_Num.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            Total_Num.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            //小计
            WorksheetStyle Total_ss = styles.Add("Total_ss");
            Total_ss.Font.FontName = "Simsum";
            Total_ss.Font.Size = 14;
            Total_ss.NumberFormat = "@"; //文本

            WorksheetStyle Total_ss1 = styles.Add("Total_ss1");
            Total_ss1.Font.FontName = "Simsum";
            Total_ss1.Font.Size = 14;
            Total_ss1.NumberFormat = "@"; //文本
            Total_ss1.Alignment.Horizontal = StyleHorizontalAlignment.Right;*/
        }


    }





}
