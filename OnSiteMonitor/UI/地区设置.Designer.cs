namespace OnSiteFundComparer.UI
{
    partial class 地区设置
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
            this.label2 = new System.Windows.Forms.Label();
            this.comboCity = new System.Windows.Forms.ComboBox();
            this.comboContry = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "城市:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "区县:";
            // 
            // comboCity
            // 
            this.comboCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCity.FormattingEnabled = true;
            this.comboCity.Location = new System.Drawing.Point(83, 26);
            this.comboCity.Name = "comboCity";
            this.comboCity.Size = new System.Drawing.Size(256, 29);
            this.comboCity.TabIndex = 2;
            this.comboCity.SelectedIndexChanged += new System.EventHandler(this.comboCity_SelectedIndexChanged);
            this.comboCity.SelectionChangeCommitted += new System.EventHandler(this.comboCity_SelectionChangeCommitted);
            // 
            // comboContry
            // 
            this.comboContry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboContry.FormattingEnabled = true;
            this.comboContry.Location = new System.Drawing.Point(83, 79);
            this.comboContry.Name = "comboContry";
            this.comboContry.Size = new System.Drawing.Size(256, 29);
            this.comboContry.TabIndex = 3;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(250, 191);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 33);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(104, 191);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 33);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // 地区设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 263);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.comboContry);
            this.Controls.Add(this.comboCity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "地区设置";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地区设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboCity;
        private System.Windows.Forms.ComboBox comboContry;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOK;
    }
}