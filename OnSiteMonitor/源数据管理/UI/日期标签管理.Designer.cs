namespace OnSiteFundComparer.UI
{
    partial class 日期标签管理
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLabel = new System.Windows.Forms.TextBox();
            this.btnAddLabel = new System.Windows.Forms.Button();
            this.btnDelabel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 73);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(519, 237);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入标签:";
            // 
            // tbLabel
            // 
            this.tbLabel.Location = new System.Drawing.Point(115, 25);
            this.tbLabel.MaxLength = 10;
            this.tbLabel.Name = "tbLabel";
            this.tbLabel.Size = new System.Drawing.Size(267, 29);
            this.tbLabel.TabIndex = 2;
            // 
            // btnAddLabel
            // 
            this.btnAddLabel.Location = new System.Drawing.Point(405, 22);
            this.btnAddLabel.Name = "btnAddLabel";
            this.btnAddLabel.Size = new System.Drawing.Size(115, 33);
            this.btnAddLabel.TabIndex = 3;
            this.btnAddLabel.Text = "添加标签";
            this.btnAddLabel.UseVisualStyleBackColor = true;
            this.btnAddLabel.Click += new System.EventHandler(this.btnAddLabel_Click);
            // 
            // btnDelabel
            // 
            this.btnDelabel.Location = new System.Drawing.Point(47, 333);
            this.btnDelabel.Name = "btnDelabel";
            this.btnDelabel.Size = new System.Drawing.Size(115, 33);
            this.btnDelabel.TabIndex = 3;
            this.btnDelabel.Text = "删除标签";
            this.btnDelabel.UseVisualStyleBackColor = true;
            this.btnDelabel.Click += new System.EventHandler(this.btnDelabel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(296, 333);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 33);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(434, 333);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 33);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // 日期标签管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 378);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDelabel);
            this.Controls.Add(this.btnAddLabel);
            this.Controls.Add(this.tbLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "日期标签管理";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标签管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLabel;
        private System.Windows.Forms.Button btnAddLabel;
        private System.Windows.Forms.Button btnDelabel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

        
    }
}