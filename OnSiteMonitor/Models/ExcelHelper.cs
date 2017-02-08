using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Models
{
    public class ExcelHelper
    {

        public static MemoryStream DataTableToExcel(DataTable dtSource, String TitleName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            if (dtSource == null)
                return null;

            dtSource.Columns.RemoveAt(0);

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "省纪委"; //填加xls文件作者信息
                si.ApplicationName = "惠民资金监控程序"; //填加xls文件创建程序信息
                si.LastAuthor = "自动生成程序"; //填加xls文件最后保存者信息
                si.Comments = "惠民资金监控程序"; //填加xls文件作者信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            HSSFCell countCell = null;
            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }

            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;

                    intTemp = intTemp < 20 ? 20 : intTemp;
                    intTemp = intTemp > 55 ? 55 : intTemp;

                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }


            var _border = workbook.CreateCellStyle();
            _border.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            _border.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            _border.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            _border.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            _border.BottomBorderColor = HSSFColor.Black.Index;
            _border.LeftBorderColor = HSSFColor.Black.Index;
            _border.RightBorderColor = HSSFColor.Black.Index;
            _border.TopBorderColor = HSSFColor.Black.Index;

            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 30;
                        headerRow.CreateCell(0).SetCellValue(TitleName);

                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //  headStyle.Alignment = CellHorizontalAlignment.CENTER;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();

                        font.FontHeightInPoints = 16;
                        font.Boldweight = 800;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;


                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));

                        //headerRow.Dispose();
                    }
                    #endregion


                    HSSFRow Row2 = (HSSFRow)sheet.CreateRow(1);

                    countCell = (HSSFCell)Row2.CreateCell(0);
                    // countCell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                    CellRangeAddress region1 = new CellRangeAddress(1, 1, (short)0, (short)dtSource.Columns.Count - 1);
                    sheet.AddMergedRegion(region1);

                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(2);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 12;
                        font.Boldweight = 800;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {

                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        // headerRow.Dispose();
                    }
                    #endregion


                    rowIndex = 3;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);



                int i = 0;
                foreach (DataColumn column in dtSource.Columns)
                {

                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);


                    newCell.CellStyle = _border;

                    newCell.CellStyle.WrapText = true;


                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型

                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            System.DateTime dateV;
                            System.DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");

                            break;
                        default:
                            newCell.SetCellValue("");

                            break;
                    }
                }
                #endregion

                rowIndex++;
            }

            //汇总 
            if (countCell != null)
                countCell.SetCellValue("总条数:" + (rowIndex - 3).ToString());

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }

        public  void SaveToExcelFile(String Dir, String FileName, DataTable dt)
        {
            GlobalEnviroment.MakeSureDirectory(Dir);

            string ResultFile = Dir + FileName + ".xls";
            using (MemoryStream ms = GlobalEnviroment.DataTableToExcel(dt, FileName))
            {
                using (FileStream fs = new FileStream(ResultFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }



    }
}
