using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class CompareDataSettingForm : Form
    {

        public CompareDataSettingForm()
        {
            InitializeComponent();
            initTreeView();

            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                Properties.Settings.Default.WorkDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }
        }

        private void initTreeView()
        {
            Business.DataMgr rdm = new Business.DataMgr();

            this.treeView1.ImageList = this.imageList1;

            TreeNode tn = rdm.BuildDisplayItemStruct(3);

            this.treeView1.Nodes.Add(tn);
            this.treeView1.SelectedNode = tn;
            this.treeView1.Select();
        }
        private void SelectItem(Models.DataItem di)
        {
            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                MessageBox.Show("输入目录错误, 请重新设置输入目录!");
                return;
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Columns.Clear();

            this.dataGridView2.DataSource = null;
            this.dataGridView2.Columns.Clear();

            this.dataGridView1.Columns.Add("", "输入目录");
            this.dataGridView1.Columns[0].Width = 600;
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int index = this.dataGridView1.Rows.Add();

            string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
            GlobalEnviroment.MakeSureDirectory(dir); //creat dir if it is not exit.
            this.dataGridView1.Rows[index].Cells[0].Value = dir;

        }
        private void SelectReferData1(Models.DataItem di)
        {
            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                MessageBox.Show("输入目录错误, 请重新设置输入目录!");
                return;
            }

            this.dataGridView1.DataSource = null;
            this.dataGridView1.Columns.Clear();


            string dir = Properties.Settings.Default.WorkDir + di.Datapath + "\\";
            GlobalEnviroment.MakeSureDirectory(dir); //creat dir if it is not exit.

            //init gridview read dir files
            this.dataGridView1.Columns.Add("FileName", "文件名");
            this.dataGridView1.Columns[0].Width = 380;
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dataGridView1.Columns.Add("Size", "大小");
            this.dataGridView1.Columns[1].Width = 100;

            this.dataGridView1.Columns.Add("date", "日期");
            this.dataGridView1.Columns[2].Width = 150;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DirectoryInfo folder = new DirectoryInfo(dir);
            int index = 0;
            foreach (FileInfo file in folder.GetFiles("*.xls"))
            {
                index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = file.Name;
                this.dataGridView1.Rows[index].Cells[1].Value = file.Length;
                this.dataGridView1.Rows[index].Cells[2].Value = file.CreationTime;
            }
            foreach (FileInfo file in folder.GetFiles("*.csv"))
            {
                index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = file.Name;
                this.dataGridView1.Rows[index].Cells[1].Value = file.Length;
                this.dataGridView1.Rows[index].Cells[2].Value = file.CreationTime;
            }

        } //gridview1
        private void SelectReferData2(Models.DataItem di)
        {           
            if (di.DataType != Models.FundItemTypes.ReferenceData)
                return;

            Business.DataMgr rdm = new Business.DataMgr();
            DataSet ds = rdm.GetDataFormat(di);

            this.dataGridView2.Columns.Clear();
            this.dataGridView2.DataSource = ds.Tables[0];
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.Columns[1].Visible = false;
            this.dataGridView2.Columns[4].Visible = false;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {//first init
            if (e.Node.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)e.Node.Tag;
            if (di.DataType == Models.FundItemTypes.ReferenceItems)
                SelectItem(di);
            else if (di.DataType == Models.FundItemTypes.ReferenceData)
            { 
                SelectReferData1(di);
                SelectReferData2(di);
            }

            lbInfo.Text = Properties.Settings.Default.WorkDir + di.Datapath;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //this.treeView1.Select();
            //if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            //    return;
            //Models.DataItem di = (Models.DataItem)this.treeView1.SelectedNode.Tag;

            //if (di.DataType != Models.FundItemTypes.Fund)
            //    if (MessageBox.Show("更改数据存放目录吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            //        == DialogResult.No)
            //        return;

            //new 目录设置().ShowDialog();
            //lbInfo.Text = Properties.Settings.Default.WorkDir + di.Datapath;
        }
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择比对项目.","警告",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            of.Filter = "Excel文件(*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv";
            if (of.ShowDialog()== DialogResult.OK)
            {
                string targetDir = Properties.Settings.Default.WorkDir + di.Datapath;

                Cursor _preCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                foreach(string fn in of.FileNames)
                {
                    try
                    {
                        string targetFile = targetDir + "\\" + Path.GetFileName(fn);
                        File.Copy(fn, targetFile);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"错误",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
                SelectReferData1(di);
                this.Cursor = _preCursor;
            }
        }
        private void btnDelFile_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.ReferenceData)
                return;
            

            if (this.dataGridView1.SelectedRows.Count <= 0)
                return;
            DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            if (MessageBox.Show("文件:" + r.Cells[0].Value.ToString() + " 会被删除,确认删除吗?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

                if (File.Exists(selectedfilename))
                {
                    try
                    {
                        File.Delete(selectedfilename);
                        SelectReferData1(di);
                        SelectReferData2(di);
                        //this.dataGridView2.Columns.Clear();
                        //this.dataGridView2.DataSource = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }                        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择对比项目.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Business.DataMgr referMgr = new Business.DataMgr();
            var formats = referMgr.GetDataFormatList(di);
            if (formats == null || formats.Count == 0)
            {
                MessageBox.Show("请先设置" + di.DataFullName + "的文件格式!");
                return;
            }

            if (this.dataGridView1.SelectedRows.Count == 0)
                return;

            DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

            //Fill datagridview1
            Cursor _preCursor = this.Cursor;
            if (File.Exists(selectedfilename))
            {
                this.Cursor = Cursors.WaitCursor;
                this.dataGridView2.DataSource = null;
                this.dataGridView2.Columns.Clear();

                DataTable dt = null;
                if (Path.GetExtension(selectedfilename) == ".xls")
                    dt = GlobalEnviroment.ReadXLSToDataTableWithFormat(selectedfilename, formats, 100);//ReadXLSToDataTableTop(selectedfilename, 20);                  
                else
                    dt = GlobalEnviroment.ReadCSVToDataTableWithFormat(selectedfilename, formats, 100);

                if (di.parentItem != null && di.parentItem.DataType != Models.FundItemTypes.ReferenceItems)
                    GlobalEnviroment.AddColumn(dt, "备注", di.DataShortName);

                this.dataGridView2.DataSource = dt;
                this.Cursor = _preCursor;
            }
            else
                MessageBox.Show("文件不存在:" + selectedfilename);

            #region temp
            //this.treeView1.Select();
            //if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            //    return;

            //Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            //if (di.DataType != Models.FundItemTypes.ReferenceData)
            //    return;

            //if (this.dataGridView1.SelectedRows.Count <= 0)
            //    return;

            //DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            //string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

            ////Fill datagridview1
            //Cursor _preCursor = this.Cursor;
            //if (File.Exists(selectedfilename))
            //{                
            //    this.Cursor = Cursors.WaitCursor;
            //    this.dataGridView2.DataSource = null;
            //    this.dataGridView2.Columns.Clear();

            //    if (Path.GetExtension(selectedfilename) == ".xls")
            //        this.dataGridView2.DataSource = GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename, 20);
            //    else if (Path.GetExtension(selectedfilename) == ".csv")
            //        this.dataGridView2.DataSource = GlobalEnviroment.ReadCSVToDataTableTop(selectedfilename, 20);
            //    this.Cursor = _preCursor;
            //}
            //else
            //    MessageBox.Show("文件不存在:" + selectedfilename);
            #endregion
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        private void btnFormat_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择对比项目.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            比对数据格式编辑 format = new 比对数据格式编辑();

            Business.DataMgr referMgr = new Business.DataMgr();
            format.dataFormats = referMgr.GetDataFormatList(di);
            
            if (format.ShowDialog() == DialogResult.OK)
            {                
                referMgr.SaveNewFormat(format.dataFormats, di);
                SelectReferData2(di);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.ReferenceData)
                return;

            if (this.dataGridView1.SelectedRows.Count <= 0)
                return;

            DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

            //Fill datagridview1
            Cursor _preCursor = this.Cursor;
            if (File.Exists(selectedfilename))
            {
                this.Cursor = Cursors.WaitCursor;
                this.dataGridView2.DataSource = null;
                this.dataGridView2.Columns.Clear();

                if (Path.GetExtension(selectedfilename) == ".xls")
                    this.dataGridView2.DataSource = GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename, 20);
                else if (Path.GetExtension(selectedfilename) == ".csv")
                    this.dataGridView2.DataSource = GlobalEnviroment.ReadCSVToDataTableTop(selectedfilename, 20);
                this.Cursor = _preCursor;
            }
            else
                MessageBox.Show("文件不存在:" + selectedfilename);

            #region 
            //this.treeView1.Select();
            //if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            //    return;

            //Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            //if (di.DataType != Models.FundItemTypes.ReferenceData)
            //{
            //    MessageBox.Show("请选择对比项目.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}


            //Business.DataMgr referMgr = new Business.DataMgr();
            //var formats = referMgr.GetDataFormatList(di);
            //if (formats == null || formats.Count == 0)
            //{                
            //    MessageBox.Show("请先设置" + di.DataFullName + "的文件格式!");
            //    return;
            //}

            //if (this.dataGridView1.SelectedRows.Count == 0)
            //    return;

            //DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            //string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

            ////Fill datagridview1
            //Cursor _preCursor = this.Cursor;
            //if (File.Exists(selectedfilename))
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    this.dataGridView2.DataSource = null;
            //    this.dataGridView2.Columns.Clear();

            //    if(Path.GetExtension(selectedfilename)== ".xls")
            //    this.dataGridView2.DataSource = GlobalEnviroment.ReadXLSToDataTableWithFormat(selectedfilename, formats, 100);//ReadXLSToDataTableTop(selectedfilename, 20);
            //    else
            //        this.dataGridView2.DataSource = GlobalEnviroment.ReadCSVToDataTableWithFormat(selectedfilename, formats, 100);
            //    this.Cursor = _preCursor;
            //}
            //else
            //    MessageBox.Show("文件不存在:" + selectedfilename);
            #endregion
        }

        private void btnDataCheck_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请确认人口数据文件已导入!", "数据校验",  MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            数据比对 dlg = new 数据比对(new Models.Task());
            dlg.Text = "比对数据校验...";
            dlg.IsDataCheck = true;
            dlg.ShowDialog();
        }
    }
}
