namespace OnSiteFundComparer.UI
{
    partial class 线索查询
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbItem = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDataStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbReport = new System.Windows.Forms.ComboBox();
            this.lbInfo = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbUploaded = new System.Windows.Forms.ComboBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbCheckStatus = new System.Windows.Forms.ComboBox();
            this.btnPre = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(648, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "身份证号：";
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(733, 58);
            this.tbID.MaxLength = 18;
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(291, 26);
            this.tbID.TabIndex = 1;
            this.tbID.TextChanged += new System.EventHandler(this.tbID_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "项目名称：";
            // 
            // cbItem
            // 
            this.cbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItem.FormattingEnabled = true;
            this.cbItem.Location = new System.Drawing.Point(89, 11);
            this.cbItem.Name = "cbItem";
            this.cbItem.Size = new System.Drawing.Size(199, 28);
            this.cbItem.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(2, 102);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1174, 396);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(1071, 56);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(89, 28);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(298, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "线索状态：";
            // 
            // cbDataStatus
            // 
            this.cbDataStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataStatus.FormattingEnabled = true;
            this.cbDataStatus.Items.AddRange(new object[] {
            "全部",
            "未核查",
            "已核查"});
            this.cbDataStatus.Location = new System.Drawing.Point(372, 11);
            this.cbDataStatus.Name = "cbDataStatus";
            this.cbDataStatus.Size = new System.Drawing.Size(199, 28);
            this.cbDataStatus.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(648, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "上报数据：";
            // 
            // cbReport
            // 
            this.cbReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReport.Enabled = false;
            this.cbReport.FormattingEnabled = true;
            this.cbReport.Items.AddRange(new object[] {
            "全部",
            "未核查",
            "已核查",
            "查实",
            "查否"});
            this.cbReport.Location = new System.Drawing.Point(733, 11);
            this.cbReport.Name = "cbReport";
            this.cbReport.Size = new System.Drawing.Size(427, 28);
            this.cbReport.TabIndex = 3;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(-2, 501);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(50, 20);
            this.lbInfo.TabIndex = 6;
            this.lbInfo.Text = "label4";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(598, 543);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 28);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "批量更新";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "上传状态：";
            // 
            // cbUploaded
            // 
            this.cbUploaded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUploaded.FormattingEnabled = true;
            this.cbUploaded.Items.AddRange(new object[] {
            "全部",
            "未上传",
            "已上传",
            "上传错误"});
            this.cbUploaded.Location = new System.Drawing.Point(89, 58);
            this.cbUploaded.Name = "cbUploaded";
            this.cbUploaded.Size = new System.Drawing.Size(199, 28);
            this.cbUploaded.TabIndex = 3;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(783, 543);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(132, 28);
            this.btnUpload.TabIndex = 5;
            this.btnUpload.Text = "上传核查记录";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1080, 543);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "退出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "核查状态：";
            // 
            // cbCheckStatus
            // 
            this.cbCheckStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheckStatus.FormattingEnabled = true;
            this.cbCheckStatus.Items.AddRange(new object[] {
            "全部",
            "查实",
            "查否"});
            this.cbCheckStatus.Location = new System.Drawing.Point(372, 59);
            this.cbCheckStatus.Name = "cbCheckStatus";
            this.cbCheckStatus.Size = new System.Drawing.Size(199, 28);
            this.cbCheckStatus.TabIndex = 7;
            // 
            // btnPre
            // 
            this.btnPre.Location = new System.Drawing.Point(36, 545);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(103, 28);
            this.btnPre.TabIndex = 8;
            this.btnPre.Text = "上一页";
            this.btnPre.UseVisualStyleBackColor = true;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(185, 545);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(103, 28);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // 线索查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 585);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.cbCheckStatus);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbReport);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbUploaded);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbDataStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "线索查询";
            this.Text = "线索查询";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDataStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbReport;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbUploaded;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbCheckStatus;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.Button btnNext;
    }
}