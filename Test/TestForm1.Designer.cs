namespace Test
{
    partial class TestForm1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbConfigDB = new System.Windows.Forms.TextBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.tbImportDB = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.tbResultDB = new System.Windows.Forms.TextBox();
            this.btnResult = new System.Windows.Forms.Button();
            this.btnRules = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "配置数据库：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "导入数据库：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 103);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "结果数据库：";
            // 
            // tbConfigDB
            // 
            this.tbConfigDB.Location = new System.Drawing.Point(133, 29);
            this.tbConfigDB.Name = "tbConfigDB";
            this.tbConfigDB.ReadOnly = true;
            this.tbConfigDB.Size = new System.Drawing.Size(511, 29);
            this.tbConfigDB.TabIndex = 1;
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(660, 29);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(90, 29);
            this.btnConfig.TabIndex = 2;
            this.btnConfig.Text = "浏览...";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // tbImportDB
            // 
            this.tbImportDB.Location = new System.Drawing.Point(133, 64);
            this.tbImportDB.Name = "tbImportDB";
            this.tbImportDB.ReadOnly = true;
            this.tbImportDB.Size = new System.Drawing.Size(511, 29);
            this.tbImportDB.TabIndex = 1;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(660, 64);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 29);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "浏览...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // tbResultDB
            // 
            this.tbResultDB.Location = new System.Drawing.Point(133, 99);
            this.tbResultDB.Name = "tbResultDB";
            this.tbResultDB.ReadOnly = true;
            this.tbResultDB.Size = new System.Drawing.Size(511, 29);
            this.tbResultDB.TabIndex = 1;
            // 
            // btnResult
            // 
            this.btnResult.Location = new System.Drawing.Point(660, 99);
            this.btnResult.Name = "btnResult";
            this.btnResult.Size = new System.Drawing.Size(90, 29);
            this.btnResult.TabIndex = 2;
            this.btnResult.Text = "浏览...";
            this.btnResult.UseVisualStyleBackColor = true;
            this.btnResult.Click += new System.EventHandler(this.btnResult_Click);
            // 
            // btnRules
            // 
            this.btnRules.Location = new System.Drawing.Point(308, 191);
            this.btnRules.Name = "btnRules";
            this.btnRules.Size = new System.Drawing.Size(158, 32);
            this.btnRules.TabIndex = 5;
            this.btnRules.Text = "确认数据库";
            this.btnRules.UseVisualStyleBackColor = true;
            this.btnRules.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 252);
            this.Controls.Add(this.btnRules);
            this.Controls.Add(this.btnResult);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.tbResultDB);
            this.Controls.Add(this.tbImportDB);
            this.Controls.Add(this.tbConfigDB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TestForm1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbConfigDB;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.TextBox tbImportDB;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox tbResultDB;
        private System.Windows.Forms.Button btnResult;
        private System.Windows.Forms.Button btnRules;
    }
}

