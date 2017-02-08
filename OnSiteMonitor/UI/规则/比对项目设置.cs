using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnSiteFundComparer.Models;

namespace OnSiteFundComparer.UI
{
    public partial class 比对项目设置 : Form
    {
        internal CollisionAim collisionAim;
        internal string testDBFile;

        public 比对项目设置()
        {
            InitializeComponent();

            
        }

        protected override void OnLoad(EventArgs e)
        {
            InitCombox();
            if (collisionAim != null)
            {
                this.comboxSource.Enabled = false;
                this.comboxSource.SelectedValue = collisionAim.SourceID;

                this.tbAim.Text = collisionAim.AimName;
                this.tbRules.Text = collisionAim.AimDesc;

                this.tbTableName.Text = collisionAim.TableName;
                this.tbSql.Text = collisionAim.Rules;
                this.tbRules2.Text = collisionAim.Rules2;         
            }

            base.OnLoad(e);
        }

        public void InitCombox()
        {
            Business.DataMgr dm = new Business.DataMgr();

            var sList = dm.GetChildDataItemList(2);
           
            if (sList == null)
                return;
            
           
            this.comboxSource.DataSource = sList;
            this.comboxSource.DisplayMember = "DataFullName";
            this.comboxSource.ValueMember = "RowID";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(collisionAim == null)
                collisionAim = new CollisionAim();

            collisionAim.SourceID = int.Parse(this.comboxSource.SelectedValue.ToString());

            collisionAim.AimName = tbAim.Text;
            collisionAim.AimDesc = tbRules.Text;

            collisionAim.TableName = this.tbTableName.Text ;
            collisionAim.Rules = this.tbSql.Text;
            collisionAim.Rules2 = this.tbRules2.Text;

            Business.DataMgr dm = new Business.DataMgr();
            dm.UpdateCollisionAim(collisionAim);

            this.DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(testDBFile))
            {
                MessageBox.Show("测试数据库不存在");
                return;
            }

            string testSql = tbSql.Text;

            if (tbRules3.SelectedIndex == 0)
                testSql = tbSql.Text;
            else
                testSql = tbRules2.Text;


            DAL.MySqlite sqliteDB = new DAL.MySqlite(testDBFile);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataSet ds = sqliteDB.ExecuteDataset(testSql);
                this.Cursor = Cursors.Arrow;

                显示列表 ret = new 显示列表();
                ret.DataSource = ds.Tables[0];
                ret.TitleName = tbAim.Text;
                ret.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
