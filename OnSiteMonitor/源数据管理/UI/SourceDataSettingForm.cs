using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnSiteFundComparer.Models;

namespace OnSiteFundComparer.UI
{
    public partial class SourceDataSettingForm : Form
    {
        public SourceDataSettingForm()
        {
            InitializeComponent();
            initTreeView();

            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                Properties.Settings.Default.WorkDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 初始化树形结构
        /// </summary>
        private void initTreeView()
        {
            Business.DataMgr rdm = new Business.DataMgr();

            this.treeView1.ImageList = this.imageList1;

            TreeNode tn = rdm.BuildDisplayItemStruct(2);

            this.treeView1.Nodes.Add(tn);
            this.treeView1.SelectedNode = tn;
            this.treeView1.Select();
        }
        /// <summary>
        /// 点击树形控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //first init
            if (e.Node.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)e.Node.Tag;
            if (di.DataType == Models.FundItemTypes.SourceItems)
                SelectItem(di);
            else if (di.DataType == Models.FundItemTypes.SourceData || di.DataType == Models.FundItemTypes.File)
            {
                SelectReferData1(di);
                SelectReferData2(di);
            }

            lbInfo.Text = Properties.Settings.Default.WorkDir + di.Datapath;
        }
        /// <summary>
        /// 点击树,填充datagridview
        /// </summary>
        /// <param name="di"></param>
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
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
                return;

            Business.DataMgr rdm = new Business.DataMgr();
            DataSet ds = rdm.GetDataFormat(di);

            this.dataGridView2.Columns.Clear();
            this.dataGridView2.DataSource = ds.Tables[0];
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.Columns[1].Visible = false;
            this.dataGridView2.Columns[4].Visible = false;
        }
        private void UpdateLabel(IEnumerable<string> delLabels)
        {
            string rootPath = OnSiteFundComparer.Properties.Settings.Default.WorkDir;
            if (!Directory.Exists(rootPath))
                return;

            Business.DataMgr dm = new Business.DataMgr();
            var sourceItem = dm.GetChildDataItemList(2);

            foreach (string l in delLabels)
            {
                foreach (Models.DataItem di in sourceItem)
                {
                    string searchingpath = rootPath + "\\" + di.Datapath + "\\" + l;

                    if(Directory.Exists(searchingpath))
                        Directory.Delete(searchingpath, true);
                }
            }
        }


        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            {
                MessageBox.Show("请选择源数据项目。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }




            DataItem _di = di;
            if (di.DataType == Models.FundItemTypes.File)
                _di = di.parentItem;



            AddFileSimple(_di);

        }
        private void AddFileRelation(Models.DataItem di)
        {
            添加源数据文件 dlg = new 添加源数据文件();

           
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string targetMainDir = Properties.Settings.Default.WorkDir + di.Datapath;
                string targetRelationDir = Properties.Settings.Default.WorkDir + di.Datapath + "\\关联文件";

                Cursor _preCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                if (dlg.MainFile != null)
                    foreach (string fn in dlg.MainFile)
                    {
                        try
                        {
                            string targetFile = targetMainDir + "\\M^" + dlg.AddInfo + "^" + Path.GetFileName(fn);
                            File.Copy(fn, targetFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }

                if (dlg.AddFile != null)
                    foreach (string fn in dlg.AddFile)
                    {
                        try
                        {
                            string targetFile = targetRelationDir + "\\C^" + dlg.AddInfo + "^" + Path.GetFileName(fn);
                            File.Copy(fn, targetFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }

                SelectReferData1(di);
                this.Cursor = _preCursor;
            }
        }
        private void AddFileSimple(Models.DataItem di)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            of.Filter = "Excel文件(*.xls;*.csv)|*.xls;*.csv";
            if (of.ShowDialog() == DialogResult.OK)
            {
                string targetDir = Properties.Settings.Default.WorkDir + di.Datapath;

                Cursor _preCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                foreach (string fn in of.FileNames)
                {
                    try
                    {
                        string targetFile = targetDir + "\\" + Path.GetFileName(fn);
                        File.Copy(fn, targetFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
                SelectReferData1(di);
                this.Cursor = _preCursor;
            }
        }    

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelFile_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            {
                MessageBox.Show("请选择源数据项目!");
                return;
            }


            if (this.dataGridView1.SelectedRows.Count <= 0)
                return;


            if (MessageBox.Show("确认删除文件吗?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)

                foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
                {
                    string selectedfilename = Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();
                    if (File.Exists(selectedfilename))
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

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 格式编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFormat_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            {
                MessageBox.Show("请选择源数据.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

          



            源数据格式编辑 format = new 源数据格式编辑();

            Business.DataMgr referMgr = new Business.DataMgr();
            format.dataFormats = referMgr.GetDataFormatList(di);
            format.relation = referMgr.GetDataRelation(di);

          
            
            if (format.ShowDialog() == DialogResult.OK )
            {
                referMgr.SaveNewFormat(format.dataFormats, di);
                SelectReferData2(di);
            }
        }

        /// <summary>
        /// 查看原始文件-(预览格式化后的文件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
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

                DataTable dt = null;
                if (Path.GetExtension(selectedfilename) == ".xls")
                    dt = GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename,  100);//ReadXLSToDataTableTop(selectedfilename, 20);                  
                else
                    dt = GlobalEnviroment.ReadCSVToDataTableTop(selectedfilename,  100);

                this.dataGridView2.DataSource = dt;// GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename, 15);
                this.Cursor = _preCursor;
            }
            else
                MessageBox.Show("文件不存在:" + selectedfilename);


            // this.treeView1.Select();
            // if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            //     return;

            // Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            // if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            // {
            //     MessageBox.Show("请选择源数据.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     return;
            // }

            //ShowFormatedFile(di);
        }
        private void ShowFormatedFile(DataItem di)
        {
            Business.DataMgr sourceMgr = new Business.DataMgr();
            var formats = sourceMgr.GetDataFormatList(di);
            if (formats == null || formats.Count == 0)
            {
                MessageBox.Show("请先设置" + di.DataFullName + "的文件格式!");
                return;
            }

            

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

                //if (di.parentItem != null && di.parentItem.DataType != Models.FundItemTypes.SourceItems)
                //    GlobalEnviroment.AddColumn(dt, "备注", di.DataShortName);
                this.dataGridView2.DataSource = dt;
                
                this.Cursor = _preCursor;
            }
            else
                MessageBox.Show("文件不存在:" + selectedfilename);
        }

        /// <summary>
        /// gridview1 的事件处理,增加序号/单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            {
                MessageBox.Show("请选择源数据.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowFormatedFile(di);



            //this.treeView1.Select();
            //if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            //    return;

            //Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            //if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
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
            //    this.dataGridView2.DataSource = GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename, 15);
            //    this.Cursor = _preCursor;
            //}
            //else
            //    MessageBox.Show("文件不存在:" + selectedfilename);
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
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)  {

        }
     
        
        /// <summary>
        /// 添加新分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileType_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.File)
            {
                //MessageBox.Show("请选择源数据项目!");
                //return;
            }

             

            TreeNode tn = treeView1.SelectedNode;            

            TreeNode node = new TreeNode();
            node.ImageIndex = tn.ImageIndex + 1;
            node.SelectedImageIndex = tn.SelectedImageIndex + 1;
            node.Text = "请输入分类";
            node.Tag = null;

            tn.Nodes.Add(node);
            tn.ExpandAll();
            treeView1.LabelEdit = true;

            treeView1.SelectedNode = node;
            node.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null)
                isCancelEdit = true;

            if (isCancelEdit)
            {
                //remove add edit
                TreeNode tn = treeView1.SelectedNode;
                treeView1.SelectedNode = tn.Parent;
                treeView1.Nodes.Remove(tn);
                isCancelEdit = false;
            }
            else
            {
                TreeNode tn = treeView1.SelectedNode;

                TreeNode pn = tn.Parent;
                Models.DataItem di = (Models.DataItem)pn.Tag;
                Models.DataItem newid = new DataItem();
                newid.parentItem = di;
                newid.Datapath = di.Datapath + "\\" + e.Label;

                
                if (Directory.Exists(newid.Datapath))
                {
                    MessageBox.Show("项目已经存在,请改名!");

                    treeView1.Nodes.Remove(tn);
                    treeView1.SelectedNode = pn;
                    

                    return;
                }

                GlobalEnviroment.MakeSureDirectory(newid.Datapath + "\\");

                newid.DataFullName = e.Label;
                newid.DataShortName = e.Label;
                newid.DataType = di.DataType;
                newid.ParentID = di.RowID;
                newid.parentItem = di;
                newid.DataLink = 0;
                newid.Status = true;
                newid.Seq = 1;
                newid.dbTable = di.dbTable;

                tn.Tag = newid;
            }
            
            treeView1.LabelEdit = false;
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            TreeNode tn = treeView1.SelectedNode;
            if (tn == null || tn.Parent == null)
                return;

            DataItem di = (DataItem)tn.Tag;
            if (di == null) return;

            if (di.parentItem == null || 
                di.parentItem.parentItem.DataType == FundItemTypes.Fund||
                di.parentItem.parentItem.DataType == FundItemTypes.SourceItems)
                MessageBox.Show("禁止删除!");
            else MessageBox.Show("可以删除!");
            

        }


        private bool isCancelEdit = false;
        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                isCancelEdit = true;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

      
    }
}
