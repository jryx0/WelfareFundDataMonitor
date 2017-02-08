namespace OnSiteFundComparer.UI
{
    partial class 比对项目设置
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
            this.tbRules = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tbAim = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboxSource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTableName = new System.Windows.Forms.TextBox();
            this.tbSql = new System.Windows.Forms.TextBox();
            this.tbRules3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbRules2 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbRules3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbRules
            // 
            this.tbRules.Location = new System.Drawing.Point(89, 111);
            this.tbRules.Multiline = true;
            this.tbRules.Name = "tbRules";
            this.tbRules.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRules.Size = new System.Drawing.Size(737, 57);
            this.tbRules.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 21);
            this.label4.TabIndex = 1;
            this.label4.Text = "规则说明:";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(28, 524);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(89, 35);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(568, 524);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(708, 524);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 35);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tbAim
            // 
            this.tbAim.Location = new System.Drawing.Point(89, 71);
            this.tbAim.Multiline = true;
            this.tbAim.Name = "tbAim";
            this.tbAim.Size = new System.Drawing.Size(737, 29);
            this.tbAim.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "比对目的:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "源数据:";
            // 
            // comboxSource
            // 
            this.comboxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSource.FormattingEnabled = true;
            this.comboxSource.Location = new System.Drawing.Point(89, 18);
            this.comboxSource.Name = "comboxSource";
            this.comboxSource.Size = new System.Drawing.Size(420, 29);
            this.comboxSource.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(515, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "存储表名:";
            // 
            // tbTableName
            // 
            this.tbTableName.Location = new System.Drawing.Point(599, 18);
            this.tbTableName.Multiline = true;
            this.tbTableName.Name = "tbTableName";
            this.tbTableName.Size = new System.Drawing.Size(227, 29);
            this.tbTableName.TabIndex = 2;
            // 
            // tbSql
            // 
            this.tbSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSql.Location = new System.Drawing.Point(3, 3);
            this.tbSql.Multiline = true;
            this.tbSql.Name = "tbSql";
            this.tbSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbSql.Size = new System.Drawing.Size(723, 265);
            this.tbSql.TabIndex = 2;
            // 
            // tbRules3
            // 
            this.tbRules3.Controls.Add(this.tabPage1);
            this.tbRules3.Controls.Add(this.tabPage2);
            this.tbRules3.Controls.Add(this.tabPage3);
            this.tbRules3.Location = new System.Drawing.Point(89, 196);
            this.tbRules3.Name = "tbRules3";
            this.tbRules3.SelectedIndex = 0;
            this.tbRules3.Size = new System.Drawing.Size(737, 305);
            this.tbRules3.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbSql);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(729, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "比对规则";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbRules2);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(729, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "按人汇总";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbRules2
            // 
            this.tbRules2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRules2.Location = new System.Drawing.Point(3, 3);
            this.tbRules2.Multiline = true;
            this.tbRules2.Name = "tbRules2";
            this.tbRules2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRules2.Size = new System.Drawing.Size(723, 265);
            this.tbRules2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 30);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(729, 271);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "明细";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(723, 265);
            this.textBox1.TabIndex = 1;
            // 
            // 比对项目设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 586);
            this.Controls.Add(this.tbRules3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.tbTableName);
            this.Controls.Add(this.tbAim);
            this.Controls.Add(this.tbRules);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboxSource);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "比对项目设置";
            this.Text = "比对项目设置";
            this.tbRules3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbRules;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox tbAim;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboxSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTableName;
        private System.Windows.Forms.TextBox tbSql;
        private System.Windows.Forms.TabControl tbRules3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbRules2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox1;
    }
}