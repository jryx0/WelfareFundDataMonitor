using System;
using System.Xml;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using System.Data;

namespace ExcelXmlWriter.Sample
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
            cell.Data.Text = "惠民资金监督检查问题线索汇总表";
            cell.MergeAcross = 4;
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";
            cell = Row0.Cells.Add();
            cell.StyleID = "Default";

            //add header 
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 23;
            foreach (DataColumn dc in dt.Columns)
                Row1.Cells.Add(dc.ColumnName, DataType.String, "sHeader");
             
            foreach(DataRow dr in dt.Rows)
            { 
                WorksheetRow wRow = sheet.Table.Rows.Add();
                wRow.Height = 37;
                foreach(DataColumn c in dr.ItemArray)                 
                    wRow.Cells.Add(c.ToString(), DataType.String, "sTxt");
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
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Simsum";
            Default.Font.Size = 20;

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
            sSum.Font.Size = 14;
            sSum.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            sSum.NumberFormat = "@";

             
            // -----------------------------------------------
            //  表头
            // -----------------------------------------------
            WorksheetStyle sHeader = styles.Add("sHeader");
            sHeader.Font.FontName = "Simsum";
            sHeader.Font.Size = 16;
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
            sBody.Font.Size = 20;
            sBody.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sBody.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sBody.NumberFormat = "@"; //文本

            // -----------------------------------------------
            //  表体
            // -----------------------------------------------
            WorksheetStyle sNum = styles.Add("sNum");
            sNum.Font.FontName = "Simsum";
            sNum.Font.Size = 20;
            sNum.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            sNum.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            sNum.NumberFormat = "0_"; //数字


            


            //表尾

        }

     
    }
}
