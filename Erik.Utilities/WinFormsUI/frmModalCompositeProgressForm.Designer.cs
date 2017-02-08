namespace Erik.Utilities.WinFormsUI
{
    partial class frmModalCompositeProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCurrentComponent = new System.Windows.Forms.Label();
            this.lblOverallProgress = new System.Windows.Forms.Label();
            this.pgbCurrentProgress = new System.Windows.Forms.ProgressBar();
            this.pgbOverallProgress = new System.Windows.Forms.ProgressBar();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCurrentComponent
            // 
            this.lblCurrentComponent.AutoSize = true;
            this.lblCurrentComponent.Location = new System.Drawing.Point(20, 155);
            this.lblCurrentComponent.Name = "lblCurrentComponent";
            this.lblCurrentComponent.Size = new System.Drawing.Size(140, 13);
            this.lblCurrentComponent.TabIndex = 14;
            this.lblCurrentComponent.Text = "Current component progress";
            // 
            // lblOverallProgress
            // 
            this.lblOverallProgress.AutoSize = true;
            this.lblOverallProgress.Location = new System.Drawing.Point(20, 106);
            this.lblOverallProgress.Name = "lblOverallProgress";
            this.lblOverallProgress.Size = new System.Drawing.Size(83, 13);
            this.lblOverallProgress.TabIndex = 13;
            this.lblOverallProgress.Text = "Overall progress";
            // 
            // pgbCurrentProgress
            // 
            this.pgbCurrentProgress.Location = new System.Drawing.Point(16, 171);
            this.pgbCurrentProgress.Name = "pgbCurrentProgress";
            this.pgbCurrentProgress.Size = new System.Drawing.Size(437, 22);
            this.pgbCurrentProgress.TabIndex = 12;
            this.pgbCurrentProgress.UseWaitCursor = true;
            // 
            // pgbOverallProgress
            // 
            this.pgbOverallProgress.Location = new System.Drawing.Point(16, 125);
            this.pgbOverallProgress.Name = "pgbOverallProgress";
            this.pgbOverallProgress.Size = new System.Drawing.Size(437, 22);
            this.pgbOverallProgress.TabIndex = 11;
            this.pgbOverallProgress.UseWaitCursor = true;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Location = new System.Drawing.Point(15, 63);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(438, 32);
            this.lblSubtitle.TabIndex = 10;
            this.lblSubtitle.Text = "Subtitle";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitle.UseWaitCursor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(15, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(438, 32);
            this.lblTitle.TabIndex = 9;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.UseWaitCursor = true;
            // 
            // frmModalCompositeProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 211);
            this.ControlBox = false;
            this.Controls.Add(this.lblCurrentComponent);
            this.Controls.Add(this.lblOverallProgress);
            this.Controls.Add(this.pgbCurrentProgress);
            this.Controls.Add(this.pgbOverallProgress);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmModalCompositeProgressForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCurrentComponent;
        private System.Windows.Forms.Label lblOverallProgress;
        private System.Windows.Forms.ProgressBar pgbCurrentProgress;
        private System.Windows.Forms.ProgressBar pgbOverallProgress;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
    }
}