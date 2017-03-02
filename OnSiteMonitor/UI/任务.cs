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
    public partial class 任务 : Form
    {
        public Models.Task task = new Models.Task();


        public 任务()
        {
            InitializeComponent();


            task.TaskName = OnSiteFundComparer.Properties.Settings.Default.CurrentRegion + "第" + GetCompareTimes() + "次大数据比对";
            task.TaskComment = OnSiteFundComparer.Properties.Settings.Default.CurentRegionName  +
                   System.DateTime.Now.ToString("yyyy年MM月dd日HH点mm分ss秒") + "，进行精准扶贫政策落实情况检查大数据分析比对。";

            task.Region = Properties.Settings.Default.CurrentRegion;

            tbTask.Text = task.TaskName;
            tbComment.Text = task.TaskComment;
        }


        private int GetCompareTimes()
        {
            int times = 0;
            try
            {
                DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

                var o = configDB.ExecuteScalar("Select count() from Task");
                times = int.Parse(o.ToString());
            }
            catch
            {

            }

            return times + 1;

        }


        protected override void OnClosed(EventArgs e)
        {
            
            base.OnClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            task.CreateDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            this.DialogResult = DialogResult.OK;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            if(GlobalEnviroment.LoginedUser.Name.ToLower() == "admin")
                this.button3.Visible = true;
            base.OnLoad(e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.MySqlite configDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

                configDB.ExecuteScalar("delete from Task");

                MessageBox.Show("清空任务列表成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);             
            }
            catch
            {
                MessageBox.Show("清空任务列表失败！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Business.QuickComparer qc = new Business.QuickComparer();

            DateTime start = DateTime.Now;
            

            qc.Comparer(0);

            TimeSpan ts = DateTime.Now - start;

            MessageBox.Show("总耗时：" + ts.TotalSeconds.ToString());
        }
    }
}
