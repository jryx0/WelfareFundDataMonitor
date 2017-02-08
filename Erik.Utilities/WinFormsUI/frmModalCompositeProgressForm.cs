using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Erik.Utilities.Interfaces;

namespace Erik.Utilities.WinFormsUI
{
    public partial class frmModalCompositeProgressForm : Form
    {
        public frmModalCompositeProgressForm(
            ICompositeProgressiveOperation operation)
        {
            InitializeComponent();

            // Subscribe to operation events
            operation.NewOperation += (sender, e) =>
                {
                    lblCurrentComponent.Text =
                        operation.CurrentOperation.MainTitle;

                    Refresh();
                };

            operation.OperationStart += (sender, e) =>
                {
                    lblTitle.Text = operation.MainTitle;
                    lblSubtitle.Text = operation.SubTitle;

                    Refresh();
                };

            operation.OperationProgress += (sender, e) =>
                {
                    pgbOverallProgress.Value = operation.CurrentProgress;
                    pgbCurrentProgress.Value = operation.CurrentOperation.CurrentProgress;

                    Application.DoEvents();
                };

            operation.OperationEnd += (sender, e) => Close();

            // Subscribe to Shown event of the form
            Shown += (sender, e) => operation.Start();
        }
    }
}
