using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI.项目管理
{
    public partial class 项目管理 : Form
    {
        private bool displayAll = false;
        public 项目管理()
        {
            InitializeComponent();
        }

        public 项目管理(Models.DataItem di)
        {
            InitializeComponent();

            initTreeView();
            Init(di);
        }


        private void Init(Models.DataItem di)
        {
            if (di == null) return;
            

            if (di.DataType == Models.FundItemTypes.ReferenceItems || di.DataType == Models.FundItemTypes.SourceItems)
                return;

            if (di.DataType == Models.FundItemTypes.ReferenceData )
                comboBox1.Text = "比对数据";
            else if(di.DataType == Models.FundItemTypes.SourceData)
                comboBox1.Text = "源数据";

            tbFullName.Text = di.DataFullName;
            tbShortName.Text = di.DataShortName;
            tbPath.Text = di.Datapath;
            tbTable.Text = di.dbTable;
            tbTablePre.Text = di.dbTablePre;
            tbOrder.Text = di.Seq.ToString();
            cbStatuts.Checked = di.Status;
            tbDesc.Text = di.people;
            tbDataPreSql.Text = di.col1;
        }

        private void initTreeView()
        {

            treeView1.Nodes.Clear();

            Business.DataMgr rdm = new Business.DataMgr();

            this.treeView1.ImageList = this.imageList1;
            TreeNode tn = this.displayAll ?  rdm.BuildAllItemStruct(1) : rdm.BuildDisplayItemStruct(1);

            this.treeView1.Nodes.Add(tn);
            this.treeView1.SelectedNode = tn;
            this.treeView1.Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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

        private void button1_Click(object sender, EventArgs e)
        {//删除
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;

            if(di.DataType == Models.FundItemTypes.Fund || di.DataType == Models.FundItemTypes.ReferenceItems ||
                di.DataType == Models.FundItemTypes.SourceItems)
            {
                MessageBox.Show("只能删除项目！");
                return;
            }

            if(treeView1.SelectedNode.FirstNode != null)
            {
                MessageBox.Show("请先删除子项目！");
                return;
            }

            if (MessageBox.Show("确认删除项目：" + di.DataFullName + " 吗？", "删除警告", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                return;

            Service.DataItemStuctServices dss = new Service.DataItemStuctServices(Properties.Settings.Default.MainDBFile);

            var Ret = dss.DelDataItem(di);
            if (String.IsNullOrEmpty(Ret))
                treeView1.SelectedNode.Remove();
            else
                MessageBox.Show(Ret);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        { //显示信息
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            Init(di);
        }

        void clearInfo()
        {
            tbFullName.Text = "";
            tbShortName.Text = "";
            tbPath.Text = "";
            tbTable.Text = "";
            tbOrder.Text = "";
            cbStatuts.Checked = true;
            tbDesc.Text = "";
            tbDataPreSql.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {//新增
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null )
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if (di.DataType == Models.FundItemTypes.Fund)
                return;

            TreeNode tn = treeView1.SelectedNode;
            if (di.DataType == Models.FundItemTypes.ReferenceData || di.DataType == Models.FundItemTypes.SourceData)
                treeView1.SelectedNode = tn.Parent; 

            di = (Models.DataItem)treeView1.SelectedNode.Tag;

            TreeNode newtn = new TreeNode();
            newtn.Text = "测试";

            Models.DataItem newdi = new Models.DataItem();
            newdi.DataFullName = "新建项目全称";
            newdi.DataShortName = "新建项目";
            newdi.parentItem = di;
            newdi.ParentID = di.RowID;
            
            
            if (di.DataType == Models.FundItemTypes.ReferenceItems)
                newdi.DataType = Models.FundItemTypes.ReferenceData;
            if (di.DataType == Models.FundItemTypes.SourceItems)
                newdi.DataType = Models.FundItemTypes.SourceData;

            newtn.ImageIndex = 2;
            newtn.SelectedImageIndex = 2;
            newtn.Tag = newdi;

            treeView1.SelectedNode.Nodes.Insert(0, newtn);
            //add first child
            treeView1.SelectedNode = newtn;
            treeView1.SelectedNode.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            treeView1.Focus();
            clearInfo();

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;
            if(e.Label !=null)
                di.DataFullName = e.Label.Trim();

            Init(di);           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;

            Models.DataItem di = (Models.DataItem)treeView1.SelectedNode.Tag;


            di.DataFullName = tbFullName.Text.Trim();
            di.DataShortName = tbShortName.Text.Trim();


            if (di.DataType == Models.FundItemTypes.ReferenceData)
            {
                if (tbPath.Text.IndexOf("\\02比对数据") == -1)
                    di.Datapath = "\\02比对数据" + tbPath.Text.Trim();
                else di.Datapath = tbPath.Text.Trim();
            }
            else
            {
                if (tbPath.Text.IndexOf("\\01源数据") == -1)
                    di.Datapath = "\\01源数据" + tbPath.Text.Trim();
                else di.Datapath = tbPath.Text.Trim();
            }

            //di.Datapath = tbPath.Text;
            di.dbTable = tbTable.Text.Trim();

            if (tbTablePre.Text.Length == 0)
                di.dbTablePre = di.dbTable + "Pre";
            else di.dbTablePre = tbTablePre.Text;

            
            int seq = 0;
            int.TryParse(tbOrder.Text.Trim(), out seq);
            di.Seq = seq;
            di.Status = cbStatuts.Checked;
            di.people = tbDesc.Text.Trim();
            di.col1 = tbDataPreSql.Text.Trim();


            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(Properties.Settings.Default.MainDBFile);

            var checkdi = diss.GetDataItem(di);

            string strRet = "";
            if (checkdi == null)
                strRet = diss.InsertDataItem(di);
            else strRet = diss.ModifyDataItem(di);

            if (!String.IsNullOrEmpty(strRet))
            {
                MessageBox.Show(strRet);
            }
            else
            { 
                initTreeView();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            displayAll = !displayAll;

            btnDisplay.Text = displayAll ? "隐藏不启用项目" : "显示全部项目";

            initTreeView();
        }

        //private void tbFullName_TextChanged(object sender, EventArgs e)
        //{

        //}
    }
}
