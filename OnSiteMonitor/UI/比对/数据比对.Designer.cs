namespace OnSiteFundComparer.UI
{
    partial class 数据比对
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
            this.tbLogWin = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbLogWin
            // 
            this.tbLogWin.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbLogWin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbLogWin.Location = new System.Drawing.Point(0, 0);
            this.tbLogWin.MaxLength = 327670;
            this.tbLogWin.Multiline = true;
            this.tbLogWin.Name = "tbLogWin";
            this.tbLogWin.ReadOnly = true;
            this.tbLogWin.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLogWin.Size = new System.Drawing.Size(885, 382);
            this.tbLogWin.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Enabled = false;
            this.btnExit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.Location = new System.Drawing.Point(773, 416);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 32);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Enabled = false;
            this.btnOpenDir.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenDir.Location = new System.Drawing.Point(263, 416);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(150, 32);
            this.btnOpenDir.TabIndex = 2;
            this.btnOpenDir.Text = "打开结果目录";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // 数据比对
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 470);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tbLogWin);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "数据比对";
            this.Text = "数据比对";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLogWin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOpenDir;
    }
}