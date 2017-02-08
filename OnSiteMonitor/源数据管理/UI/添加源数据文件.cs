using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 添加源数据文件 : Form
    {
        //public Models.DataItem dataItem;
        public string AddInfo = "";

        public string[] MainFile;
        public string[] AddFile;

        //public List<string> Labels;

        public 添加源数据文件()
        {
            InitializeComponent();

            
        }

        protected override void OnLoad(EventArgs e)
        {
            initComboBox();
            base.OnLoad(e);
        }

        void initComboBox()
        {
            Business.DataMgr dm = new Business.DataMgr();
            var Labels = dm.GetDataLabelList();
            

            this.comboBox1.Items.Clear();
            foreach (string l in Labels)
            {
                this.comboBox1.Items.Add(l);
                this.comboBox1.SelectedIndex = 0;
            }
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainFile= AddFileToDV( dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddFile = AddFileToDV( dataGridView2);
        }

        private string[] AddFileToDV(DataGridView dv)
        {
            string[] fns = null;

            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            of.Filter = "Excel文件(*.xls)|*.xls";
            if (of.ShowDialog() == DialogResult.OK)
            {
               // string targetDir = Properties.Settings.Default.WorkDir + di.Datapath;

                Cursor _preCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                dv.Columns.Clear();

                dv.Columns.Add("FileName", "文件名");
                dv.Columns[0].Width = 380;
                dv.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                foreach (string fn in of.FileNames)
                {
                    int index = dv.Rows.Add();
                    dv.Rows[index].Cells[0].Value = Path.GetFileName(fn);
                }

                fns = of.FileNames;
                this.Cursor = _preCursor;
            }

            return fns;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.comboBox1.Select();
            AddInfo = this.comboBox1.Text;

            this.DialogResult = DialogResult.OK;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Business.DataMgr fileMgr = new Business.DataMgr();

            List<string> labels = fileMgr.GetDataLabelList();

            日期标签管理 dateLabel = new 日期标签管理();
            dateLabel.Labels = labels;
            if (dateLabel.ShowDialog() == DialogResult.OK)
            {
                fileMgr.UpdateLabel(dateLabel.newLabels);

               
                initComboBox();
                //var delLabels = labels.Except(dateLabel.newLabels);
                //UpdateLabel(delLabels);
            }
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
                    //string searchingpath = rootPath + "\\" + di.Datapath + "\\" + l;

                    //if (Directory.Exists(searchingpath))
                    //    Directory.Delete(searchingpath, true);
                }
            }
        }
    }
}
