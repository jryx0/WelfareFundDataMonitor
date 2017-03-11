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
    public partial class 收集数据统计导入 : Form
    {
        public String DBFileInfo = "";
        public 收集数据统计导入()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.progressBar1.Value = 0;
            // 第一步:初始化
            this.backgroundWorker1.WorkerReportsProgress = true; // 显示进度
            this.backgroundWorker1.WorkerSupportsCancellation = true; // 支持取消

            backgroundWorker1.RunWorkerAsync();
            base.OnLoad(e);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Test.TestComparing tc = new Test.TestComparing();

            tc.compareAims = new List<Models.CompareAim>();
            tc.IsNeedCompare = false;
            tc.log += backgroundWorker1.ReportProgress;
            tc.CompareDirectly = false;

            DBFileInfo = tc.Start();
            if (DBFileInfo.Length == 0)
            {
                this.backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Length != 0)
                tbLogWin.AppendText(e.UserState.ToString() + "\r\n");

            if (e.ProgressPercentage != 0)
                this.progressBar1.Value = e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Value = 100;
            if (e.Cancelled)
            {
                MessageBox.Show("导入数据出错，无法统计数据");
                button1.Visible = true;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
