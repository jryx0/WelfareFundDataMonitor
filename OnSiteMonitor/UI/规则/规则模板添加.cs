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
    public partial class 规则模板添加 : Form
    {
        public string tmpName = "";
        public string type = "2";
        public int tmpType = 0;
        public string rowid = "";
        public string seq = "";

        public 规则模板添加()
        {
            InitializeComponent();
            tbType.Text = "2";

           
        }

        protected override void OnLoad(EventArgs e)
        {
            comboBox1.SelectedIndex = (tmpType > -1 && tmpType < 3) ? tmpType : 0;

            if(rowid.Length !=0)
            {
                DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
                String sql = @"SELECT rowid,
                                    tmpName AS 模板名称,
                                    ruletype AS 形式,
                                      tmptype   AS 模板类型,
                                    seq as 顺序
                                FROM RulesTmp
                                WHERE rowid =  " + rowid + " ORDER BY seq";

                var ds = configDB.ExecuteDataset(sql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.tbName.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.tbType.Text = ds.Tables[0].Rows[0][2].ToString();
                    this.comboBox1.SelectedIndex = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                    this.tbSeq.Text = ds.Tables[0].Rows[0][4].ToString();
                }
            }
            base.OnLoad(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tmpName = tbName.Text;
            type = tbType.Text  ;
            tmpType = comboBox1.SelectedIndex;
            seq = tbSeq.Text;

            if (tmpName.Length != 0)
                this.DialogResult = DialogResult.OK;
            else MessageBox.Show("请输入模板名称!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
