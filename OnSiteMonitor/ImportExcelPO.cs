using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erik.Utilities;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OnSiteFundComparer
{
    public class ImportExcelPO : Erik.Utilities.Bases.BaseProgressiveOperation
    {
        string _filename;
        public DataTable resultDT = new DataTable();
        public int StartNumber = 0;
        public int MaxNumber = 65536;
        public bool WithFormat = false;

        protected int readlines = 0;

        public int StartCol = 0;

        public ImportExcelPO(string xlsFileName,   int top = int.MaxValue)
        {
            //MainTitle = "读取xls文件";
            _filename = xlsFileName;
            _currentStep = 0;

            MaxNumber = top;
        }


        public System.Collections.IEnumerator getExcelFileRows(string FileName)
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

                    _totalSteps = sheet.PhysicalNumberOfRows + 10;

                    _currentStep = 10;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误："+ ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            return rows;
        }

        public override void Start()
        {
            OnOperationStart(EventArgs.Empty);

            System.Collections.IEnumerator rows = getExcelFileRows(_filename);// GlobalEnviroment.getExcelFileRows(_filename);
            if (rows == null)
            {
                OnOperationEnd(EventArgs.Empty);                
                return;
            }
            OnOperationProgress(EventArgs.Empty);


            
            int colnumbers = 15;
            string ext = Path.GetExtension(_filename);

            while (rows.MoveNext())
            {
                _currentStep++;
                OnOperationProgress(EventArgs.Empty);
                
                IRow row;
                if (ext == ".xls")
                    row = (HSSFRow)rows.Current;
                else
                    row = (XSSFRow)rows.Current;

                DataRow dr = resultDT.NewRow();

                if (readlines == 0)
                {
                    if (colnumbers > row.LastCellNum)
                        colnumbers = row.LastCellNum;

                    colnumbers = CreateColums(colnumbers);
                    //for (int j = 0; j < colnumbers; j++)
                    //    resultDT.Columns.Add((j + 1).ToString());
                }

                if (readlines++ < StartNumber)
                    continue;

                //if (readlines >= MaxNumber)
                //    break;

                for (int i = StartCol; i < colnumbers; i++)
                {
                    //var str = GlobalEnviroment.GetExcelValue(row.GetCell(i)); 
                    //dr[i] = Regex.Replace(str, @"[\x00-\x21]|[\x7e]", "");
                    if(readlines < MaxNumber)
                        dr[i - StartCol] = GetValue(row, i);
                }


                resultDT.Rows.Add(dr);
                System.Threading.Thread.Sleep(10);
                

                ExcelEventArgs er = new ExcelEventArgs(dr, _currentStep);
                OnOperationProgress(er);
            }

            OnOperationEnd(EventArgs.Empty);            
        }    
        protected virtual string  GetValue(IRow row, int index)
        {
            var str = GlobalEnviroment.GetExcelValue(row.GetCell(index));
            if (str == null) return "";
            return Regex.Replace(str, @"[\x00-\x21]|[\x7e]", "");
        }

        protected virtual int CreateColums(int totalCols)
        {
            for (int j = 0; j < totalCols; j++)
                resultDT.Columns.Add((j + 1).ToString());

            return totalCols;
        }

    }

    public class ImportFormatedExcelPO : ImportExcelPO
    {
        List<Models.DataFormat> _formats;
        int IDError = 0;
        public ImportFormatedExcelPO(String FileName, List<Models.DataFormat> formats, int top = int.MaxValue) :base (FileName , top)
        {
            MaxNumber = top;
            WithFormat = true;

            _formats = formats;

            StartNumber = formats[0].colNumber;
            StartCol = 1;
        }

        public override void Start()
        {
            base.Start();

            //ToDo：显示异常身份证
            //float eRat = (IDError * 1.0f / _totalSteps) * 100;
            //if (eRat > 10)
            //    MessageBox.Show("身份证号异常超过" + eRat + "%, 是否文件格式错误？", "身份证号异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override int CreateColums(int totalCols)
        {
            for (int j = 0; j < _formats.Count - 1; j++)
                resultDT.Columns.Add(_formats[j + 1].colName);

            return _formats.Count;
        }

        protected override string GetValue(IRow row, int i)
        {
            String strRet = "";

            var index = _formats[i].colNumber - 1;
            try
            {
                if (readlines < MaxNumber)
                {
                    strRet = GlobalEnviroment.GetExcelValue(row.GetCell(index));
                    //GetExcelValue(row.GetCell(index));

                    if (_formats[i].colName == "日期")
                    {
                        strRet = GlobalEnviroment.tryParingDateTime(strRet).ToString("yyyy-MM-dd");//?
                    }
                }
                if (_formats[i].colName == "身份证号")
                {
                    strRet = GlobalEnviroment.GetExcelValue(row.GetCell(index));
                    var id = GlobalEnviroment.GetFullIDEx(strRet);

                    //ToDo:保存异常身份真行数
                    if (!String.IsNullOrEmpty(id) && (id.Length != 18 && id.Length != 15))
                        IDError++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("列名:" + _formats[i].colName + ", 列号:" +
                    _formats[i].colNumber.ToString() + ",行号:" + _currentStep.ToString() + ". 错误信息:" +
                    ex.Message);
            }

            return strRet;
        }


    }


    class ExcelEventArgs : EventArgs//声明事件信息类型，并继承于EventArgs
    {
        public ExcelEventArgs(DataRow dr, int lineNo)
        {
            this._datarow = dr;
            this._lineNumber = lineNo;
        }
        private DataRow _datarow;
        private int _lineNumber;
        public DataRow dataRow
        {
            get { return _datarow; }
        }
        public int LineNumber
        {
            get { return _lineNumber; }
        }
    }

}
