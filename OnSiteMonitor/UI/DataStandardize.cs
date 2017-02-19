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
using System.Diagnostics;

namespace OnSiteFundComparer.UI
{
    public partial class DataStandardize : Form
    {
        int IsShowFile = 0;
        private String SelectFileName;
        String WorkDir = GlobalEnviroment.InputExceltDir;
        public DataStandardize()
        {
            InitializeComponent();

            initTreeView();

            //if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            //{
            //    Properties.Settings.Default.WorkDir = Application.StartupPath;
            //    Properties.Settings.Default.Save();
            //}
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(GlobalEnviroment.LoginedUser.Name != "admin")
            { 
               // this.btnEditFormat.Visible = false;
                this.btnAddItem.Visible = false;
                
            }

            //this.WindowState = FormWindowState.Maximized;
            //this.MaximizeBox = false;
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void initTreeView()
        {
            this.treeView1.Nodes.Clear();
            Business.DataMgr rdm = new Business.DataMgr();

            this.treeView1.ImageList = this.imageList1;

            TreeNode tn = rdm.BuildDisplayItemStruct(1);

            this.treeView1.Nodes.Add(tn);
            this.treeView1.SelectedNode = tn;
            this.treeView1.Select();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //first init
            if (e.Node.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)e.Node.Tag;
            if (di.DataType == Models.FundItemTypes.SourceData || di.DataType == Models.FundItemTypes.ReferenceData)
                SelectData(di);
            else
            {
                SelectItem(di);
            }

            lbInfo.Text = WorkDir + di.Datapath;//Properties.Settings.Default.WorkDir + di.Datapath;
        }
        private void SelectItem(Models.DataItem di)
        {
            if (!Directory.Exists(WorkDir))// Properties.Settings.Default.WorkDir))
            {
                MessageBox.Show("输入目录错误, 请重新设置输入目录!");
                return;
            }

            SelectFileName = "";

            this.dataGridView1.DataSource = null;
            this.dataGridView1.Columns.Clear();

            this.dataGridView2.DataSource = null;
            this.dataGridView2.Columns.Clear();

            this.dataGridView3.DataSource = null;
            this.dataGridView3.Columns.Clear();


            this.dataGridView1.Columns.Add("", "输入目录");
            this.dataGridView1.Columns[0].Width = 600;
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int index = this.dataGridView1.Rows.Add();

            string dir = WorkDir + di.Datapath + "\\";//Properties.Settings.Default.WorkDir + di.Datapath + "\\";
            GlobalEnviroment.MakeSureDirectory(dir); //creat dir if it is not exit.
            this.dataGridView1.Rows[index].Cells[0].Value = dir;
        }
        private void SelectData(DataItem di)
        {
            if (!Directory.Exists(WorkDir))//Properties.Settings.Default.WorkDir))
            {
                MessageBox.Show("输入目录错误, 请重新设置输入目录!");
                return;
            }
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Columns.Clear();

            this.dataGridView2.DataSource = null;
            this.dataGridView2.Columns.Clear();

            this.dataGridView3.DataSource = null;
            this.dataGridView3.Columns.Clear();

            SelectFileName = "";
            string dir = WorkDir + di.Datapath + "\\";//Properties.Settings.Default.WorkDir + di.Datapath + "\\";

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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择源数据或比对数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IsShowFile = 2;

            DataGridViewRow r = this.dataGridView1.SelectedRows[0];
            SelectFileName = WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString(); //Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();

            if (this.tabControl1.SelectedIndex == 1)
                ShowOriginalFile();
            else ShowFormatedFile(di);
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
            if (IsShowFile == 0)
                return;
            IsShowFile--;

            //Fill datagridview1
            Cursor _preCursor = this.Cursor;
            if (File.Exists(SelectFileName))
            {
                this.Cursor = Cursors.WaitCursor;
                this.dataGridView2.DataSource = null;
                this.dataGridView2.Columns.Clear();

                DataTable dt = null;
                if (Path.GetExtension(SelectFileName) == ".xls")
                {
                    //ImportExcelPO iPO = new ImportExcelPO(SelectFileName);
                    //ImportFormatedExcelPO ifPO = new ImportFormatedExcelPO(SelectFileName, formats, 100);
                    //ifPO.MainTitle = "读入Excle文件:" + Path.GetFileNameWithoutExtension(SelectFileName);
                    //Erik.Utilities.Lib.StartProgressiveOperation(ifPO, this);

                    //dt = ifPO.resultDT;

                    dt = GlobalEnviroment.ReadXLSToDataTableWithFormat(SelectFileName, formats, 100);//ReadXLSToDataTableTop(selectedfilename, 20);       
                }
                else
                {
                    dt = GlobalEnviroment.ReadCSVToDataTableWithFormat(SelectFileName, formats, 100);
                }

                if (dt != null)
                {
                    this.dataGridView2.DataSource = dt.DefaultView.ToTable();
                     
                }
            }
            else
                MessageBox.Show("文件不存在:" + SelectFileName);

            this.Cursor = _preCursor;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择源数据或比对数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.tabControl1.SelectedIndex == 1)
            {
                ShowOriginalFile();
            }
            else ShowFormatedFile(di);
        }

        private void ShowOriginalFile()
        {
            if (IsShowFile == 0)
                return;

            IsShowFile--;
            //Fill datagridview1
            Cursor _preCursor = this.Cursor;
            if (File.Exists(SelectFileName))
            {
                this.Cursor = Cursors.WaitCursor;
                this.dataGridView3.DataSource = null;
                this.dataGridView3.Columns.Clear();

                DataTable dt = null;
                if (Path.GetExtension(SelectFileName) == ".xls")
                {
                    //ImportExcelPO ifPO = new ImportExcelPO(SelectFileName, 100);
                    //ifPO.MainTitle = "读入Excle文件:" + Path.GetFileNameWithoutExtension(SelectFileName);
                    //Erik.Utilities.Lib.StartProgressiveOperation(ifPO, this);
                    //dt = ifPO.resultDT;
                    dt = GlobalEnviroment.ReadXLSToDataTableTop(SelectFileName, 100);//ReadXLSToDataTableTop(selectedfilename, 20);        
                }
                else
                    dt = GlobalEnviroment.ReadCSVToDataTableTop(SelectFileName, 100);

                this.dataGridView3.DataSource = dt;// GlobalEnviroment.ReadXLSToDataTableTop(selectedfilename, 15);
                this.Cursor = _preCursor;
            }

        }

        //add file
        private void button2_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择源数据或比对数据项目。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataItem _di = di;
            if (di.DataType == Models.FundItemTypes.File)
                _di = di.parentItem;

            AddFileSimple(_di);
        }

        private void AddFileSimple(Models.DataItem di)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            of.Filter = "Excel文件(*.xls;*.csv)|*.xls;*.csv";
            if (of.ShowDialog() == DialogResult.OK)
            {
                string targetDir = WorkDir + di.Datapath;//Properties.Settings.Default.WorkDir + di.Datapath;

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

                SelectData(di);
                this.Cursor = _preCursor;
            }
        }

        private void btnDelFile_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType != Models.FundItemTypes.SourceData && di.DataType != Models.FundItemTypes.ReferenceData)
            {
                MessageBox.Show("请选择源数据或比对数据项目!");
                return;
            }


            if (this.dataGridView1.SelectedRows.Count <= 0)
                return;


            if (MessageBox.Show("确认删除文件吗?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)

                foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
                {
                    string selectedfilename = WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();//Properties.Settings.Default.WorkDir + di.Datapath + "\\" + r.Cells[0].Value.ToString();
                    if (File.Exists(selectedfilename))
                        try
                        {
                            File.Delete(selectedfilename);
                            SelectData(di);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                }
        }

        private void btnEditFormat_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;           

            if (di.DataType == Models.FundItemTypes.SourceData || di.DataType == Models.FundItemTypes.ReferenceData)
            {

                源数据格式编辑 format = new 源数据格式编辑();
                Business.DataMgr referMgr = new Business.DataMgr();
                format.dataFormats = referMgr.GetDataFormatList(di);
                format.relation = referMgr.GetDataRelation(di);

                format.headtext = di.DataShortName + "格式编辑";

                if (format.ShowDialog() == DialogResult.OK)
                { 
                    referMgr.SaveNewFormat(format.dataFormats, di);
                    dataGridView1_CellClick(null, null);
                }
            }
           
            else MessageBox.Show("请选择源数据或比对数据.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void treeView1_Leave(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                ////让选中项背景色呈现红色
                //treeView1.SelectedNode.BackColor = treeView1.SelectedNode.BackColor == Color.Red ? Color.White : Color.Red;
                ////前景色为白色
                //treeView1.SelectedNode.ForeColor = treeView1.SelectedNode.ForeColor == Color.White ? Color.Black : Color.White;

                //让选中项背景色呈现红色
                treeView1.SelectedNode.BackColor = Color.Red;
                //前景色为白色
                treeView1.SelectedNode.ForeColor = Color.White;
            }
            //else
            //{
            //    treeView1.BackColor = Color.Empty;
            //    treeView1.ForeColor = Color.Black;
            //}
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView1.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView1.SelectedNode.ForeColor = Color.Black;
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            this.treeView1.Select();
            if (treeView1.SelectedNode == null && treeView1.SelectedNode.Tag == null)
            {
                MessageBox.Show("请选择项目！");
                return;
            }

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;

            //if (di.DataType == FundItemTypes.Fund)
            //{
            //    MessageBox.Show("请选择类型！");
            //    return;
            //}

             
            项目管理.项目管理 dlg = new 项目管理.项目管理(di);
            dlg.ShowDialog();

            initTreeView();

        }
        
        //统计数据量
        //必须是经过数据比对后才能看到
        private void button1_Click(object sender, EventArgs e)
        {
            //获取最新比对中间数据库
            var dbFileInfo = GetlastDB();

            收集数据统计向导 dataWz = new 收集数据统计向导();
            if (dataWz.ShowDialog() != DialogResult.OK)
                return;

            if (dataWz.CountType == "Old")
                if (dbFileInfo.Length == 0)
                    if (MessageBox.Show("无法找到比对结果， 需要进行数据导入吗？", "导入数据", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        dataWz.CountType = "New";
                    else return;

            if (dataWz.CountType == "New")
            {
                收集数据统计导入 dataImportor = new 收集数据统计导入();
                if (dataImportor.ShowDialog() == DialogResult.Cancel)
                    return;

                dbFileInfo = dataImportor.DBFileInfo;
            }

            显示收集数据统计结果 dataReport = new 显示收集数据统计结果(dbFileInfo);


            dataReport.ShowDialog();


            //show result


        }

        public string GetlastDB()
        {
            String dbFileInfo = "";
            DirectoryInfo TheFolder = new DirectoryInfo(GlobalEnviroment.InputDBDir);
            if (TheFolder.GetFiles().Count() != 0)
            {
                var filename = TheFolder.GetFiles()
                    .Where(x => ((x.FullName.IndexOf("result") == -1) && (x.FullName.IndexOf("Check") == -1)))
                    .OrderByDescending(n => n.LastWriteTime).First();

                if (filename != null)
                {
                    dbFileInfo = filename.FullName;
                }
            }
            return dbFileInfo;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if(Directory.Exists(lbInfo.Text))
            {
                ProcessStartInfo proc = new ProcessStartInfo("explorer.exe", lbInfo.Text);
                Process.Start(proc);
            }
        }
    }
}



