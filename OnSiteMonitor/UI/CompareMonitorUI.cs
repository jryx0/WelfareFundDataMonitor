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
    public partial class CompareMonitorUI : Form
    {

        public CompareMonitorUI()
        {
            InitializeComponent();
        }

        private void InitControl()
        {
            lbStep.Text = "";
            lbInfo.Text = "";
            lbProgress.Text = "";

            btnExit.Visible = false;


            this.pbStep.Value = 0;
            this.pbProgress.Value = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            InitControl();

            backgroundWorker.RunWorkerAsync();
            base.OnLoad(e);
        }

        private void QCompareInfo(object sender, Business.CompareEventArgs args)
        {
            
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Business.QuickComparer qc = new Business.QuickComparer();
            qc.CompareInfo += backgroundWorker.ReportProgress;

            qc.Comparer(Models.RulesTypes.Compare);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState == null)
                return;

            var cea = (Business.CompareEventArgs)e.UserState;

            switch (cea.ArgType)
            {
                case Business.CompareEventEnum.Info:
                    lbInfo.Text = cea.ProgressMessage;
                    break;
                case Business.CompareEventEnum.Progress:
                    if(cea.Level > 0)
                        lbProgress.Text = cea.ProgressMessage;
                    pbProgress.Value = cea.Progress;
                    break;
                case Business.CompareEventEnum.Step:
                    lbStep.Text = cea.ProgressMessage;
                    pbStep.Value = cea.Progress;
                    break;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
                btnExit.Visible = true;
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
