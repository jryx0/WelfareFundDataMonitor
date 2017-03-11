namespace OnSiteFundComparer.UI
{
    partial class 系统设置
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
            this.tbData = new System.Windows.Forms.TabPage();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tbResultDir = new System.Windows.Forms.TextBox();
            this.tbInputFileDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tpNet = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUploadUrl = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.button1 = new System.Windows.Forms.Button();
            this.tbData.SuspendLayout();
            this.tpNet.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbData
            // 
            this.tbData.Controls.Add(this.button1);
            this.tbData.Controls.Add(this.button9);
            this.tbData.Controls.Add(this.button7);
            this.tbData.Controls.Add(this.button8);
            this.tbData.Controls.Add(this.button6);
            this.tbData.Controls.Add(this.tbResultDir);
            this.tbData.Controls.Add(this.tbInputFileDir);
            this.tbData.Controls.Add(this.label1);
            this.tbData.Controls.Add(this.label8);
            this.tbData.Location = new System.Drawing.Point(4, 30);
            this.tbData.Name = "tbData";
            this.tbData.Padding = new System.Windows.Forms.Padding(3);
            this.tbData.Size = new System.Drawing.Size(592, 302);
            this.tbData.TabIndex = 3;
            this.tbData.Text = "目录设置";
            this.tbData.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(457, 234);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(87, 31);
            this.button9.TabIndex = 2;
            this.button9.Text = "退出";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(296, 234);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(87, 31);
            this.button7.TabIndex = 2;
            this.button7.Text = "确定";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(517, 35);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(58, 28);
            this.button8.TabIndex = 2;
            this.button8.Text = "浏览";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(517, 109);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(58, 28);
            this.button6.TabIndex = 2;
            this.button6.Text = "浏览";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tbResultDir
            // 
            this.tbResultDir.Location = new System.Drawing.Point(128, 36);
            this.tbResultDir.Name = "tbResultDir";
            this.tbResultDir.ReadOnly = true;
            this.tbResultDir.Size = new System.Drawing.Size(375, 29);
            this.tbResultDir.TabIndex = 1;
            // 
            // tbInputFileDir
            // 
            this.tbInputFileDir.Location = new System.Drawing.Point(128, 110);
            this.tbInputFileDir.Name = "tbInputFileDir";
            this.tbInputFileDir.ReadOnly = true;
            this.tbInputFileDir.Size = new System.Drawing.Size(375, 29);
            this.tbInputFileDir.TabIndex = 1;
            this.tbInputFileDir.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "输出结果目录：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 21);
            this.label8.TabIndex = 0;
            this.label8.Text = "文件目录：";
            this.label8.Visible = false;
            // 
            // tpNet
            // 
            this.tpNet.Controls.Add(this.label4);
            this.tpNet.Controls.Add(this.tbUploadUrl);
            this.tpNet.Controls.Add(this.button4);
            this.tpNet.Location = new System.Drawing.Point(4, 30);
            this.tpNet.Name = "tpNet";
            this.tpNet.Size = new System.Drawing.Size(592, 302);
            this.tpNet.TabIndex = 2;
            this.tpNet.Text = "网络设置";
            this.tpNet.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 21);
            this.label4.TabIndex = 13;
            this.label4.Text = "数据上传地址：";
            // 
            // tbUploadUrl
            // 
            this.tbUploadUrl.Location = new System.Drawing.Point(150, 29);
            this.tbUploadUrl.Name = "tbUploadUrl";
            this.tbUploadUrl.ReadOnly = true;
            this.tbUploadUrl.Size = new System.Drawing.Size(422, 29);
            this.tbUploadUrl.TabIndex = 12;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(449, 199);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 31);
            this.button4.TabIndex = 11;
            this.button4.Text = "退出";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbData);
            this.tabControl1.Controls.Add(this.tpNet);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 336);
            this.tabControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(75, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 系统设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 336);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "系统设置";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.tbData.ResumeLayout(false);
            this.tbData.PerformLayout();
            this.tpNet.ResumeLayout(false);
            this.tpNet.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox tbUploadUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tpNet;
        private System.Windows.Forms.Label label8;
     
        private System.Windows.Forms.TextBox tbInputFileDir;
        private System.Windows.Forms.TextBox tbResultDir;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TabPage tbData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}