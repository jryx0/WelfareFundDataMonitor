namespace OnSiteFundComparer.UI.项目管理
{
    partial class 项目管理
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(项目管理));
            this.tbShortName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFullName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTable = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbStatuts = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbOrder = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button5 = new System.Windows.Forms.Button();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbTablePre = new System.Windows.Forms.TextBox();
            this.tbDataPreSql = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbShortName
            // 
            this.tbShortName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbShortName.Location = new System.Drawing.Point(523, 150);
            this.tbShortName.Name = "tbShortName";
            this.tbShortName.Size = new System.Drawing.Size(233, 26);
            this.tbShortName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(438, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目类型：";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "源数据",
            "比对数据"});
            this.comboBox1.Location = new System.Drawing.Point(523, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(233, 28);
            this.comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(438, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "项目名称：";
            // 
            // tbFullName
            // 
            this.tbFullName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbFullName.Location = new System.Drawing.Point(523, 48);
            this.tbFullName.Name = "tbFullName";
            this.tbFullName.Size = new System.Drawing.Size(233, 26);
            this.tbFullName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(438, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "项目简称：";
            // 
            // tbTable
            // 
            this.tbTable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbTable.Location = new System.Drawing.Point(523, 82);
            this.tbTable.Name = "tbTable";
            this.tbTable.Size = new System.Drawing.Size(233, 26);
            this.tbTable.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(438, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "存储表名：";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(438, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "存储路径：";
            // 
            // tbPath
            // 
            this.tbPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbPath.Location = new System.Drawing.Point(523, 184);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(233, 26);
            this.tbPath.TabIndex = 1;
            // 
            // tbDesc
            // 
            this.tbDesc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbDesc.Location = new System.Drawing.Point(523, 218);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(233, 26);
            this.tbDesc.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(438, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "项目描述：";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(405, 512);
            this.treeView1.TabIndex = 4;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.Leave += new System.EventHandler(this.treeView1_Leave);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(11, 529);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 29);
            this.button2.TabIndex = 0;
            this.button2.Text = "新增";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(698, 529);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(57, 29);
            this.button3.TabIndex = 0;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(91, 529);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = " 删除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbStatuts
            // 
            this.cbStatuts.AutoSize = true;
            this.cbStatuts.Location = new System.Drawing.Point(523, 286);
            this.cbStatuts.Name = "cbStatuts";
            this.cbStatuts.Size = new System.Drawing.Size(56, 24);
            this.cbStatuts.TabIndex = 5;
            this.cbStatuts.Text = "启用";
            this.cbStatuts.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(438, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "项目顺序：";
            // 
            // tbOrder
            // 
            this.tbOrder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbOrder.Location = new System.Drawing.Point(523, 252);
            this.tbOrder.Name = "tbOrder";
            this.tbOrder.Size = new System.Drawing.Size(233, 26);
            this.tbOrder.TabIndex = 6;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "project.png");
            this.imageList1.Images.SetKeyName(1, "wenjian.png");
            this.imageList1.Images.SetKeyName(2, "Floder.png");
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point(522, 529);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(57, 29);
            this.button5.TabIndex = 0;
            this.button5.Text = "保存";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDisplay.Location = new System.Drawing.Point(220, 529);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(129, 29);
            this.btnDisplay.TabIndex = 7;
            this.btnDisplay.Text = "显示全部";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.button4_Click);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(438, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 20);
            this.label8.TabIndex = 2;
            this.label8.Text = "前处理表：";
            // 
            // tbTablePre
            // 
            this.tbTablePre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbTablePre.Location = new System.Drawing.Point(523, 116);
            this.tbTablePre.Name = "tbTablePre";
            this.tbTablePre.Size = new System.Drawing.Size(233, 26);
            this.tbTablePre.TabIndex = 8;
            // 
            // tbDataPreSql
            // 
            this.tbDataPreSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDataPreSql.Location = new System.Drawing.Point(442, 316);
            this.tbDataPreSql.Multiline = true;
            this.tbDataPreSql.Name = "tbDataPreSql";
            this.tbDataPreSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbDataPreSql.Size = new System.Drawing.Size(314, 196);
            this.tbDataPreSql.TabIndex = 9;
            this.tbDataPreSql.WordWrap = false;
            // 
            // 项目管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 570);
            this.Controls.Add(this.tbDataPreSql);
            this.Controls.Add(this.tbTablePre);
            this.Controls.Add(this.btnDisplay);
            this.Controls.Add(this.tbOrder);
            this.Controls.Add(this.cbStatuts);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTable);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbFullName);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.tbShortName);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "项目管理";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目管理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbShortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFullName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbStatuts;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbOrder;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbTablePre;
        private System.Windows.Forms.TextBox tbDataPreSql;
    }
}