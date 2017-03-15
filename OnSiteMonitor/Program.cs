using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace OnSiteFundComparer
{

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            GlobalEnviroment.InitEnviroment();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoginContext());             
        }
    }

    public class GlobalEnviroment
    {
    
        public static string MainDBFile;  //主数据/
        public static string InputDBDir; //导入数据库目录
        public static string InputExceltDir; //输入excel目录
        public static string ResultOutputDir; //结果目录 Excle

        public static bool LocalVersion = true;
        public static bool isCryt = true;
        public static int MaxThreadNum = Environment.ProcessorCount;


        public static FileTranser.MTOM.ClassLibrary.WebServicesHelp theWebService;
        public static Models.User LoginedUser;

        public static string LoginToken;

        public static string GetInsertSql(string table, string colnames, string values)
        {
            string insertSql = "Insert into :table(:colnames) values(:values)";


            table = table.Trim(new Char[] { ' ', ',' });
            colnames = colnames.Trim(new Char[] { ' ', ',' });
            values = values.Trim(new Char[] { ' ', ',' });

            insertSql = insertSql.Replace(":table", table);
            insertSql = insertSql.Replace(":colnames", colnames);
            insertSql = insertSql.Replace(":values", values);

            return insertSql;
        }
        public static void InitEnviroment()
        {
            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
                Properties.Settings.Default.WorkDir = Application.StartupPath;

            if (!Directory.Exists(Properties.Settings.Default.ResultDir))
                Properties.Settings.Default.ResultDir = Application.StartupPath;
            Properties.Settings.Default.Save();

        
            MainDBFile = Properties.Settings.Default.MainDBFile;

            //Output
            ResultOutputDir = Properties.Settings.Default.ResultDir + "\\Excel结果";

            //input
            InputDBDir = Properties.Settings.Default.WorkDir + "\\导入中间数据库\\";
            InputExceltDir = Properties.Settings.Default.WorkDir;

           // theWebService = new FileTranser.MTOM.ClassLibrary.WebServicesHelp(Properties.Settings.Default.WebServicesUrl + "FileTransfer.asmx");
        }

        #region 日志
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region 公用函数
        public static HSSFWorkbook getHSSFWorkbook(string FileName)
        {
            HSSFWorkbook hssfworkbook = null;

            try
            {
                using (FileStream file = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return hssfworkbook;
        }
        public static System.Collections.IEnumerator getExcelFileRows(string FileName)
        {
            ISheet sheet = null;
            try
            {
                string ext = Path.GetExtension(FileName);
                using (FileStream file = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    if (ext == ".xls")
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                        sheet = hssfworkbook.GetSheetAt(0);
                        
                    }
                    else
                    {
                        XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                        sheet = xssfworkbook.GetSheetAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return null;
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            return rows;
        }
        public static DataTable ReadXLSToDataTableTop(string FileName, int top = int.MaxValue)
        {
            System.Collections.IEnumerator rows = GlobalEnviroment.getExcelFileRows(FileName);
            if (rows == null)
            {
                log.Error("文件" + Path.GetFileName(FileName) + "无法打开");
                return null;
            }

            DataTable dt = new DataTable();
            int readlines = 0;
            int colnumbers = 15;
            string ext = Path.GetExtension(FileName);

            

            while (rows.MoveNext())
            {
                IRow row;
                if (ext == ".xls")
                    row = (HSSFRow)rows.Current;
                else
                    row = (XSSFRow)rows.Current;

                DataRow dr = dt.NewRow();


                if (colnumbers > row.LastCellNum)
                    colnumbers = row.LastCellNum;

                if (readlines == 0)
                {                  
                    for (int j = 0; j < colnumbers; j++)
                        dt.Columns.Add((j + 1).ToString());
                }

                for (int i = 0; i < colnumbers; i++)
                {
                    dr[i] = GlobalEnviroment.GetExcelValue(row.GetCell(i));
                        //GetExcelValue(row.GetCell(i));
                }

                //if (colnumbers < row.LastCellNum)
                //{
                //    for (int j = colnumbers; j < row.LastCellNum; j++)
                //        dt.Columns.Add((j + 1).ToString());
                //    colnumbers = row.LastCellNum;
                //}

                //for (int i = 0; i < row.LastCellNum; i++)
                //{
                //    dr[i] = GetExcelValue(row.GetCell(i));
                //}
                dt.Rows.Add(dr);

                if (readlines++ > top)
                    break;
            }
            return dt;
        }
        public static DataTable ReadXLSToDataTableWithFormat(string FileName, List<Models.DataFormat> dataFormat, int top = int.MaxValue)
        {
            if (dataFormat == null || dataFormat.Count == 0) return null;

            System.Collections.IEnumerator rows = GlobalEnviroment.getExcelFileRows(FileName);
            if (rows == null) return null;

            DataTable dt = new DataTable();
            int readlines = 0, IDError = 0;
            string ext = Path.GetExtension(FileName);
            while (rows.MoveNext())
            {
                IRow row;
                if (ext == ".xls") row = (HSSFRow)rows.Current;
                else row = (XSSFRow)rows.Current;

                if (readlines == 0)
                    for (int j = 0; j < dataFormat.Count - 1; j++)
                        dt.Columns.Add(dataFormat[j + 1].DisplayName);

               // if (readlines++ > top) break;
                if (readlines++ < dataFormat[0].colNumber - 1) continue;
                if (readlines == 95)
                {
                }//for debug 

                DataRow dr = dt.NewRow();
                for (int i = 1; i < dataFormat.Count; i++)//start 1, the first format is line number
                {
                    try
                    {
                        int index = dataFormat[i].colNumber - 1;
                        if (readlines < top)
                        {                            
                            dr[i - 1] = GlobalEnviroment.GetExcelValue(row.GetCell(index));
                            //GetExcelValue(row.GetCell(index));

                            //2017.2.10
                            if (dataFormat[i].colName == "日期")
                           
                            {
                                dr[i - 1] = GlobalEnviroment.tryParingDateTime(dr[i - 1].ToString()).ToString("yyyy-MM-dd");//?
                            }
                        }

                        if (dataFormat[i].colName == "身份证号")
                        {
                            string id = GlobalEnviroment.GetExcelValue(row.GetCell(index));
                            id = GetFullIDEx(id);

                            if (String.IsNullOrEmpty(id) || (id.Length != 18 && id.Length != 15))
                                IDError++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("列名:" + dataFormat[i].colName + ", 列号:" +
                            dataFormat[i].colNumber.ToString() + ",行号:" + readlines.ToString() + ". 错误信息:" +
                            ex.Message);
                    }
                }

                if (readlines < top)
                    dt.Rows.Add(dr);
            }

            if (readlines == 0) return dt;

            //ToDo:显示异常身份证
            //float eRat = (IDError * 1.0f / readlines) * 100;
            //if (eRat > 10)
            //    MessageBox.Show("身份证号异常超过" + eRat + "%, 是否文件格式错误？", "身份证号异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return dt;
        }
        public static DataTable ReadCSVToDataTableWithFormat(string FileName, List<Models.DataFormat> dataFormat, int top = int.MaxValue)
        {
            if (dataFormat == null || dataFormat.Count == 0)
                return null;
            DataTable dt = new DataTable();

            try
            {

                StreamReader mysr = new StreamReader(FileName, System.Text.Encoding.Default);

                String Line = "";
                int colnumbers = 0;
                int readlines = 0;
                int IDError = 0;
                while ((Line = mysr.ReadLine()) != null)
                {
                    DataRow dr = dt.NewRow();
                    string[] cols = Line.Split(',');


                    if (readlines == 0)
                    {
                        for (int j = 0; j < dataFormat.Count - 1; j++)
                            dt.Columns.Add(dataFormat[j + 1].DisplayName);

                        //for (int j = colnumbers; j < dataFormat.Count; j++)
                        //{
                        //    dt.Columns.Add((j + 1).ToString());
                        //    //dt.Columns.Add(dataFormat[j + 1].colName);
                        //}
                        colnumbers = dataFormat.Count;
                    }

                    //if (readlines++ > top)
                    //    break;

                    if (readlines++ < dataFormat[0].colNumber - 1)
                        continue;

                    for (int i = 1; i < dataFormat.Count; i++)
                    {
                        int index = dataFormat[i].colNumber - 1;

                        if (readlines < top)
                        {
                            if (index >= cols.Length)
                                dr[i - 1] = null;
                            else
                                dr[i - 1] = cols[index];
                        }

                        if (dataFormat[i].colName == "身份证号")
                        {
                            string id =  cols[index];
                            id = GetFullIDEx(id);

                            if (String.IsNullOrEmpty(id) || (id.Length != 18 && id.Length != 15))
                                IDError++;
                        }
                    }

                    if (readlines <  top)
                        dt.Rows.Add(dr);
                }

                if (readlines == 0) return dt;

                //ToDo:显示异常身份证
                //float eRat = (IDError * 1.0f / readlines) * 100;
                //if (eRat > 10)
                //    MessageBox.Show("身份证号异常超过" + eRat + "%, 是否文件格式错误？", "身份证号异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

                mysr.Close();
            }
            catch (Exception ex)
            {

            }


           

            return dt;
        }
        public static DataTable ReadCSVToDataTableTop(string FileName, int top = int.MaxValue)
        {
            DataTable dt = new DataTable();

            try
            {
                // FileStream fs = new FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //Encoding encoding = Common.GetType(FileName);
                StreamReader mysr = new StreamReader(FileName, System.Text.Encoding.Default);

                String Line = "";
                int colnumbers = 0;
                int readlines = 0;

                while ((Line = mysr.ReadLine()) != null)
                {
                    DataRow dr = dt.NewRow();
                    string[] cols = Line.Split(',');

                    if (colnumbers < cols.Length)
                        for (int j = colnumbers; j < cols.Length; j++)
                        {
                            dt.Columns.Add((j + 1).ToString());
                            colnumbers = cols.Length;
                        }


                    if (readlines++ > top)
                        break;

                    for (int i = 0; i < cols.Length; i++)
                        dr[i] = cols[i];

                    dt.Rows.Add(dr);
                }
                mysr.Close();
            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        internal static void AddColumn(DataTable dt, string colName, string data)
        {
            if (dt == null)
                return;

            dt.Columns.Add(colName);
            try
            {
                foreach (DataRow row in dt.Rows)
                    row[colName] = data;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream DataTableToExcel(DataTable dtSource, string strHeaderText)
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
                si.Title = strHeaderText; //填加xls文件标题信息
                si.Subject = strHeaderText;//填加文件主题信息
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
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

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
            if(countCell != null)           
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

        public static string GetFullIDEx(String _id)
        {
            if (_id == null) return _id;
            try
            {
                _id = Regex.Replace(_id, @"[^0-9xX]", " ");
                _id = _id.Trim(' ').ToUpper();
                _id = _id.Replace(" ", "");

                if (_id.Length == 15)
                    _id = GetFullID(_id);
            }
            catch (Exception)
            {

            }
            return _id;
        }
        public static string GetFullID(String id15)
        {
            var id = id15;// Regex.Replace(id15, @"[^0-9]", " ");
                          //id = id.Trim(new char[] { ' ', 'X', 'x' });

            if (id.Length != 15)
                return id15;

            string id17 = id.Substring(0, 6) + "19" + id.Substring(6, 9);

            int[] weight = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };    //十七位数字本体码权重
            char[] validate = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };    //mod11,对应校验码字符值    

            int sum = 0;
            int mode = 0;
            try
            {
                for (int i = 0; i < id17.Length; i++)
                {
                    sum = sum + int.Parse(id17[i].ToString()) * weight[i];
                }
                mode = sum % 11;
                id15 = id17 + validate[mode];
            }
            catch (Exception ex)
            {
                //log.Error(id15 + "  :" + ex.Message);
            }

            return id15;
        }
        public static string GetExcelValue(ICell cell)
        {
            string strValue = "";
            if (cell == null)
                strValue = null;
            else if (cell.CellType == CellType.Formula)
                try
                {
                    if (cell.CachedFormulaResultType == CellType.Numeric)
                        strValue = cell.NumericCellValue.ToString();
                    else
                        strValue = cell.StringCellValue;
                }
                catch
                {
                    strValue = cell.ToString();
                }
            else if (cell.CellType == CellType.Numeric)
            {
                short format = cell.CellStyle.DataFormat;
                if (format == 14 || format == 31 || format == 57 || format == 58 || format == 27 || format == 176)
                    try
                    {
                        strValue = cell.DateCellValue.ToShortDateString();
                    }
                    catch (Exception ex)
                    {
                        strValue = cell.ToString();
                    }
                else
                    strValue = cell.NumericCellValue.ToString();
                //strValue = cell.ToString();
            }
            else if (cell.CellType == CellType.Error)
                strValue = cell.ErrorCellValue.ToString();
            else strValue = cell.StringCellValue;

           

            return strValue;
        }
        public static string ToDateTimeString(string _date)
        {
            if (_date.Length < 4)
                return _date;

            DateTime dt = String2Date(_date);

            if (dt == System.DateTime.MinValue)
                return _date;
            else
                return dt.ToString("yyyy-MM-dd");
            //try
            //{
            //    DateTime dt;
            //    _date = Regex.Replace(_date, @"[^0-9]", "");
            //    if (Regex.IsMatch(_date, @"^(20|19)\d{2}\d{2}\d{2}$"))
            //    {// 20150908
            //        dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
            //        _date = dt.ToString("yyyy-MM-dd");
            //    }
            //    else if (Regex.IsMatch(_date, @"^(20|19)\d{2}\d{2}$"))
            //    {//201509

            //        dt = System.DateTime.ParseExact(_date, "yyyyMM", null);
            //        _date = dt.ToString("yyyy-MM-dd");
            //    }
            //    else if (Regex.IsMatch(_date, @"^(20|19)\d{2}"))
            //    {//2015
            //        dt = System.DateTime.ParseExact(_date, "yyyy", null);
            //        _date = dt.ToString("yyyy-MM-dd");
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //return _date;
        }
        public static DateTime tryParingDateTime(string _date)
        {
            var dt = String2Date(_date);


            //DateTime dt = System.DateTime.Now;

            //try
            //{
            //    _date = Regex.Replace(_date, @"[^0-9]", "");
            //    if(Regex.IsMatch(_date, @"^(20|19)\d{2}\d{2}\d{2}$"))
            //    {// 20150908
            //        dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
            //    }
            //    else if (Regex.IsMatch(_date, @"^(20|19)\d{2}\d{2}$"))
            //    {//201509
            //        dt = System.DateTime.ParseExact(_date, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
            //    }

            //    else if(Regex.IsMatch(_date, @"^(20|19)\d{2}$"))
            //    {//2015
            //        dt = System.DateTime.ParseExact(_date, "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            return dt;
        }

        public static DateTime String2Date(string _date)
        {        

            DateTime dt = System.DateTime.MinValue;

            Dictionary<string, string> regStr = new Dictionary<string, string>();

          
            try
            {
                _date = _date.Replace("元", "01");
                _date = _date.Replace(" ", "");

                if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{2}(\-|\/|\.|月|\\)\d{2}(日)*$"))
                {// yyyy-mm-dd
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}\d{2}\d{2}$"))
                {//yyyymmdd
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{1}(\-|\/|\.|月|\\)\d{2}(日)*$"))
                {//yyyy-m-dd
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    _date = _date.Insert(4, "0");
                    dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{1}(\-|\/|\.|月|\\)\d{1}(日)*$"))
                {//yyyy-m-d
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    _date = _date.Insert(4, "0");
                    _date = _date.Insert(6, "0");
                    dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{2}(\-|\/|\.|月|\\)\d{1}(日)*$"))
                {//yyyy-mm-d
                    _date = Regex.Replace(_date, @"[^0-9]", "");                    
                    _date = _date.Insert(6, "0");
                    dt = System.DateTime.ParseExact(_date, "yyyyMMdd", null);
                }
               
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{2}$"))
                {//yyyy-mm
                    _date = Regex.Replace(_date, @"[^0-9]", "");                    
                    dt = System.DateTime.ParseExact(_date, "yyyyMM", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}(\-|\/|\.|年|\\)\d{1}$"))
                {//yyyy-m
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    _date = _date.Insert(4, "0");
                    dt = System.DateTime.ParseExact(_date, "yyyyMM", null);
                }
                else if (Regex.IsMatch(_date, @"^(20|19)\d{2}$"))
                {//yyyy
                    _date = Regex.Replace(_date, @"[^0-9]", "");
                    dt = System.DateTime.ParseExact(_date, "yyyy", null);
                }
            }
            catch(Exception ex)
            {

            }

            return dt;
        }


        public static string MakeSureDirectory(string dir)
        {
            string strRet = "";
            try
            {
                if (!System.IO.Directory.Exists(dir))
                    MakeSureDirectoryPathExists(dir);
            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);
        #endregion
    }

}




