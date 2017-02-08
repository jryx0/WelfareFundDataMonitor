namespace OnSiteFundComparer
{
    partial class ModifyPassword
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNewPassword1 = new System.Windows.Forms.TextBox();
            this.tbNewPassword2 = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名:";
            // 
            // tbName
            // 
            this.tbName.Enabled = false;
            this.tbName.Location = new System.Drawing.Point(95, 91);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(273, 29);
            this.tbName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "旧密码:";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(95, 141);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(273, 29);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "新密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "确认密码:";
            // 
            // tbNewPassword1
            // 
            this.tbNewPassword1.Location = new System.Drawing.Point(95, 191);
            this.tbNewPassword1.Name = "tbNewPassword1";
            this.tbNewPassword1.Size = new System.Drawing.Size(273, 29);
            this.tbNewPassword1.TabIndex = 4;
            this.tbNewPassword1.UseSystemPasswordChar = true;
            // 
            // tbNewPassword2
            // 
            this.tbNewPassword2.Location = new System.Drawing.Point(95, 241);
            this.tbNewPassword2.Name = "tbNewPassword2";
            this.tbNewPassword2.Size = new System.Drawing.Size(273, 29);
            this.tbNewPassword2.TabIndex = 5;
            this.tbNewPassword2.UseSystemPasswordChar = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(255, 304);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(102, 30);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(50, 304);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(102, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(95, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(273, 29);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "地区:";
            this.label5.Visible = false;
            // 
            // ModifyPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 421);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tbNewPassword2);
            this.Controls.Add(this.tbNewPassword1);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ModifyPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.Load += new System.EventHandler(this.ModifyPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNewPassword1;
        private System.Windows.Forms.TextBox tbNewPassword2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
    }
}