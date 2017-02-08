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
    internal partial class frmModalProgressForm : Form
    {
        internal frmModalProgressForm(IProgressiveOperation operation)
        {
            InitializeComponent();

            // Subscribe to the operation events
            operation.OperationStart += (sender, e) =>
                {
                    lblTitle.Text = operation.MainTitle;
                    lblSubtitle.Text = operation.SubTitle;
                    pgb.Value = operation.CurrentProgress;

                    Refresh();
                };

            operation.OperationProgress += (sender, e) =>
                {
                    pgb.Value = operation.CurrentProgress;
                    lblSubtitle.Text = operation.CurrentProgress.ToString() + "%";
                    Application.DoEvents();
                };

            operation.OperationEnd += (sender, e) => Close();

            // Subscribe to Shown event of the Form
            Shown += (sender, e) => operation.Start();
        }
    }
}
