using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 源数据格式编辑 : Form
    {
        string[] colName = new string[] { "起始行号"                                          
                                         ,"身份证号"
                                         ,"姓名"
                                       ,"乡镇街道"
                                         ,"地址"
                                         ,"日期"
                                         ,"金额"
                                         ,"补助类型"                                         
                                         ,"相关人身份证号"
                                         ,"相关人姓名"
                                         ,"关系"
                                         ,"型号"
                                         ,"编号"
                                         ,"面积"
                                         ,"日期1"
                                         ,"编号1"
                                        ,"编号2"

        };

        string[] colCode = new string[] { "RowStart"
                                         ,"InputID"
                                         ,"Name"
                                         ,"Region"
                                         ,"Addr"
                                         ,"DataDate"
                                         ,"Amount"
                                         ,"AmountType"
                                         ,"RelateID"
                                         ,"RelateName"
                                         ,"Relation"
                                         ,"Type"
                                         ,"Number"
                                         ,"Area"
                                        ,"DataDate1"
                                        ,"Serial1"
                                        ,"Serial2"
        };


        /// <summary>
        /*CREATE TABLE IF NOT EXISTS refertable
        (
            RowID INTEGER      PRIMARY KEY AUTOINCREMENT,
            ID VARCHAR(20),
            sRelateID VARCHAR(20),
            sDataDate DATETIME(0),
            InputID VARCHAR(20),
            Name VARCHAR(20),
            Region VARCHAR(20),
            Addr VARCHAR(20),
            DataDate VARCHAR(20),
            Amount DOUBLE(0),
            AmountType VARCHAR(20),
            RelateID VARCHAR(20),
            RelateName VARCHAR(20),
            Relation VARCHAR(20),
            Type VARCHAR(20),
            ItemType VARCHAR(20),
            Number VARCHAR(20),
            Area DOUBLE(0),
            DataDate1 varchar(20),
            Serial1 varchar(30),
            Serial2 varchar(30)
          );*/ 
        /// </summary>

        public int startIndex = 3;
        

        public List<Models.DataFormat> dataFormats;
        public Models.DataFormat relation;
        private bool isHaveRelation;
        public bool isRelationSetting {
            set {
                isHaveRelation = value;
                if (isHaveRelation)
                    startIndex = 1;
                else startIndex = 1;
            }
            get { return isHaveRelation; }
        }

        public string headtext = "格式编辑";

        public 源数据格式编辑()
        {
            InitializeComponent();
            isRelationSetting = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            initGridView();
            initComboBox();    
            
            if(relation != null)
            {
                this.rlatTBName.Text = relation.Comment;
                this.rlatCol.Text = relation.colNumber.ToString();
            }

            this.Text = headtext;      

            base.OnLoad(e);
        }

        public void initGridView()
        {
            this.dataGridView1.Columns.Add("Name", "列名");
            this.dataGridView1.Columns[0].Width = 150;
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[0].ReadOnly = true;

            this.dataGridView1.Columns.Add("Number", "列号");
            this.dataGridView1.Columns[1].Width = 100;
            this.dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[1].ReadOnly = false;

            this.dataGridView1.Columns.Add("Number", "顺序");
            this.dataGridView1.Columns[2].Width = 100;
            this.dataGridView1.Columns[2].Visible = false;
            this.dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[2].ReadOnly = true;

            this.dataGridView1.Columns.Add("ColCode", "表列名");
            this.dataGridView1.Columns[3].Width = 100;
            this.dataGridView1.Columns[3].Visible = false;
            this.dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[3].ReadOnly = true;

            this.dataGridView1.Columns.Add("DisplayName", "显示名");
            this.dataGridView1.Columns[3].Width = 100;
            this.dataGridView1.Columns[3].Visible = true;
            this.dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns[3].ReadOnly = false;

            if (dataFormats == null || dataFormats.Count == 0)
            {
                dataFormats = new List<Models.DataFormat>();
                for (int i = 0; i < startIndex; i++)
                {
                    int index = this.dataGridView1.Rows.Add(1);

                    this.dataGridView1.Rows[index].Cells[0].Value = colName[i];
                    this.dataGridView1.Rows[index].Cells[1].Value = i + 2;
                    this.dataGridView1.Rows[index].Cells[2].Value = i;
                    this.dataGridView1.Rows[index].Cells[3].Value = colCode[i];
                    this.dataGridView1.Rows[index].Cells[4].Value = colName[i];


                    this.dataGridView1.Rows[index].Cells[1].ValueType = typeof(int);
                }

                

            }
            else
            {
                foreach (Models.DataFormat df in dataFormats)
                {
                    int index = this.dataGridView1.Rows.Add(1);

                    this.dataGridView1.Rows[index].Cells[0].Value = df.colName;
                    this.dataGridView1.Rows[index].Cells[1].Value = df.colNumber;
                    this.dataGridView1.Rows[index].Cells[2].Value = df.Seq;
                    this.dataGridView1.Rows[index].Cells[3].Value = df.colCode;
                    this.dataGridView1.Rows[index].Cells[4].Value = df.DisplayName;

                    this.dataGridView1.Rows[index].Cells[1].ValueType = typeof(int);
                }
            }


        }
        public void initComboBox()
        {
            initRelation();

            for (int i = 0; i < colName.Length - startIndex; i++)
            {
                this.comboBox1.Items.Insert(i, colName[i + startIndex]);
            }

            this.comboBox1.SelectedIndex = 0;
        }

        public void initRelation()
        {
            //if (isRelationSetting)
            //{
            //    this.rlatlabel1.Visible = true;
            //    this.rlatlabel2.Visible = true;
            //    this.rlatlabel3.Visible = true;
            //    this.rlatTBName.Visible = true;
            //    this.rlatCol.Visible = true;
            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            dataFormats.Clear();
            foreach(DataGridViewRow r in dataGridView1.Rows)
            {
                Models.DataFormat ff = new Models.DataFormat();

                ff.colName = r.Cells[0].Value.ToString();
                ff.colNumber = Convert.ToInt32(r.Cells[1].Value);
                ff.Seq = Convert.ToInt32(r.Cells[2].Value);

                ff.colCode = r.Cells[3].Value.ToString();
                ff.DisplayName = r.Cells[4].Value.ToString();

                dataFormats.Add(ff);
            }

            if (isRelationSetting)
            {
                //relationgCols = new Models.DataFormat();
                //relationgCols.colName = rlatTBName.Text;
                //relationgCols.colNumber = Convert.ToInt32(rlatCol.Text);
                //relationgCols.Seq = -1;  

                Models.DataFormat ff = new Models.DataFormat();

                ff.colName = "关联列号";
                ff.colCode = "LinkCol";
                ff.Comment =  rlatTBName.Text;
                Int32.TryParse(rlatCol.Text, out ff.colNumber);                
                ff.Seq = -1;

                dataFormats.Add(ff);
            }

            this.DialogResult = DialogResult.OK;
           // this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in this.dataGridView1.SelectedRows)
            {
                int seq = -1;

                Int32.TryParse(r.Cells[2].Value.ToString(), out seq);

                if (seq >= startIndex)
                    this.dataGridView1.Rows.Remove(r);
                else
                    MessageBox.Show(r.Cells[0].Value.ToString() + "是关键字段, 不能删除!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbCol.Text.Length == 0)
            {
                MessageBox.Show("请输入列号!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int inputCol = 0;
            Int32.TryParse(this.tbCol.Text, out inputCol);

            bool isExist = false;
            string errStr = "";
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                int seq = -1;
                Int32.TryParse(r.Cells[2].Value.ToString(), out seq);

                if ((this.comboBox1.SelectedIndex + startIndex) == seq)
                {
                    isExist = true;
                    errStr = this.comboBox1.Text + "已经存在！";
                    break;
                }
                
               
                //if (inputCol.ToString() == r.Cells[1].Value.ToString() || inputCol <= 0)
                //{
                //    isExist = true;
                //    errStr = this.comboBox1.Text + "列号重复 = "+ this.tbCol.Text +" ,  请重新确认!";
                //    break;
                //}
            }

            if(!isExist)
            {
                int index = this.dataGridView1.Rows.Add(1);

                this.dataGridView1.Rows[index].Cells[0].Value = this.comboBox1.Text;
                this.dataGridView1.Rows[index].Cells[1].Value = inputCol;
                this.dataGridView1.Rows[index].Cells[2].Value = this.comboBox1.SelectedIndex + startIndex;
                this.dataGridView1.Rows[index].Cells[3].Value = colCode[this.comboBox1.SelectedIndex + startIndex];
                this.dataGridView1.Rows[index].Cells[4].Value = this.comboBox1.Text;
            }
            else MessageBox.Show(errStr, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            int tmp = 0;
            if (!int.TryParse(e.FormattedValue.ToString(), out tmp))//是否是数字
            {
                if (e.FormattedValue != null && e.FormattedValue.ToString().Length != 0)
                {
                    MessageBox.Show("请输入有效数字！", "提示");
                    e.Cancel = true;
                }



            }
            //else
            //    foreach (DataGridViewRow r in dataGridView1.Rows)
            //    {
            //        if (r.Index != e.RowIndex)
            //            if (r.Cells[1].Value.ToString() == e.FormattedValue.ToString())
            //            {
            //                MessageBox.Show("列号重复！", "提示");
            //                e.Cancel = true;
            //                break;
            //            }
            //    }
        }
    }
}
