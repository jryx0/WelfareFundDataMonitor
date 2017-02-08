using Telerik.WinControls.UI;

namespace OnSiteFundComparer
{
    partial class MainForm 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.FunctionGroup = new Telerik.WinControls.UI.TileGroupElement();
            this.radTileDataQuery = new Telerik.WinControls.UI.RadTileElement();
            this.radTileReport = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement1 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement3 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement4 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileInputResult = new Telerik.WinControls.UI.RadTileElement();
            this.SettingGroup = new Telerik.WinControls.UI.TileGroupElement();
            //this.radTitledataSetting = new Telerik.WinControls.UI.RadTileElement();
            this.radTitlesystemSetting = new Telerik.WinControls.UI.RadTileElement();
            this.radTitlehelp = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement2 = new Telerik.WinControls.UI.RadTileElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanorama1
            // 
            this.radPanorama1.AutoArrangeNewTiles = false;
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanorama1.Groups.AddRange(new Telerik.WinControls.RadItem[] {
            this.FunctionGroup,
            this.SettingGroup});
            this.radPanorama1.Location = new System.Drawing.Point(0, 0);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.PanelImageSize = new System.Drawing.Size(1024, 768);
            this.radPanorama1.RowsCount = 3;
            this.radPanorama1.ShowGroups = true;
            this.radPanorama1.Size = new System.Drawing.Size(1074, 549);
            this.radPanorama1.TabIndex = 0;
            this.radPanorama1.Text = "radTilePanel1";
            ((Telerik.WinControls.UI.RadPanoramaElement)(this.radPanorama1.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadPanoramaElement)(this.radPanorama1.GetChildAt(0))).ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ((Telerik.WinControls.UI.RadPanoramaElement)(this.radPanorama1.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radPanorama1.GetChildAt(0).GetChildAt(1))).Image = null;
            // 
            // FunctionGroup
            // 
            this.FunctionGroup.AccessibleDescription = "功能";
            this.FunctionGroup.AccessibleName = "功能";
            this.FunctionGroup.CellSize = new System.Drawing.Size(165, 165);
            this.FunctionGroup.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FunctionGroup.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FunctionGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radTileDataQuery,
            this.radTileReport,
            this.radTileElement1,
            this.radTileElement3,
            this.radTileElement4,
            this.radTileInputResult});
            this.FunctionGroup.Margin = new System.Windows.Forms.Padding(120, 130, 65, 0);
            this.FunctionGroup.Name = "FunctionGroup";
            this.FunctionGroup.RowsCount = 3;
            this.FunctionGroup.Text = "功能";
            // 
            // radTileDataQuery
            // 
            this.radTileDataQuery.AccessibleDescription = "数据查询";
            this.radTileDataQuery.AccessibleName = "数据查询";
            this.radTileDataQuery.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTileDataQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTileDataQuery.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileDataQuery.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileDataQuery.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileDataQuery.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileDataQuery.BorderGradientStyle = Telerik.WinControls.GradientStyles.Linear;
            this.radTileDataQuery.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileDataQuery.Column = 3;
            this.radTileDataQuery.DrawBorder = true;
            this.radTileDataQuery.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileDataQuery.Image = global::OnSiteFundComparer.Properties.Resources.bug_tracker_icon;
            this.radTileDataQuery.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileDataQuery.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileDataQuery.Name = "radTileDataQuery";
            this.radTileDataQuery.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileDataQuery.Text = "数据查询";
            this.radTileDataQuery.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileDataQuery.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileDataQuery.Click += new System.EventHandler(this.radTileDataQuery_Click);
            this.radTileDataQuery.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileDataQuery.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTileReport
            // 
            this.radTileReport.AccessibleDescription = "上报数据设置";
            this.radTileReport.AccessibleName = "上报数据设置";
            this.radTileReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(161)))), ((int)(((byte)(209)))));
            this.radTileReport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileReport.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileReport.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileReport.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileReport.BorderGradientStyle = Telerik.WinControls.GradientStyles.Linear;
            this.radTileReport.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileReport.ColSpan = 1;
            this.radTileReport.DrawBorder = true;
            this.radTileReport.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileReport.Image = global::OnSiteFundComparer.Properties.Resources.Rss_reader_icon;
            this.radTileReport.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileReport.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileReport.Name = "radTileReport";
            this.radTileReport.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileReport.Row = 1;
            this.radTileReport.Text = "上报数据设置";
            this.radTileReport.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileReport.TextWrap = true;
            this.radTileReport.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileReport.Click += new System.EventHandler(this.radTileReport_Click);
            this.radTileReport.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileReport.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTileElement1
            // 
            this.radTileElement1.AccessibleDescription = "数据导入";
            this.radTileElement1.AccessibleName = "数据导入";
            this.radTileElement1.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTileElement1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTileElement1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement1.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement1.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement1.BorderGradientStyle = Telerik.WinControls.GradientStyles.Linear;
            this.radTileElement1.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileElement1.DrawBorder = true;
            this.radTileElement1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileElement1.Image = global::OnSiteFundComparer.Properties.Resources.ListView;
            this.radTileElement1.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileElement1.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileElement1.Name = "radTileElement1";
            this.radTileElement1.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileElement1.Text = "数据导入";
            this.radTileElement1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileElement1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileElement1.TextWrap = true;
            this.radTileElement1.Click += new System.EventHandler(this.radTileElement1_Click);
            this.radTileElement1.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileElement1.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTileElement3
            // 
            this.radTileElement3.AccessibleDescription = "系统设置";
            this.radTileElement3.AccessibleName = "系统设置";
            this.radTileElement3.BackColor = System.Drawing.Color.Empty;
            this.radTileElement3.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.magnifier_bg;
            this.radTileElement3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.radTileElement3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement3.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement3.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement3.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement3.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileElement3.Column = 1;
            this.radTileElement3.DrawBorder = true;
            this.radTileElement3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileElement3.Image = global::OnSiteFundComparer.Properties.Resources.ListView;
            this.radTileElement3.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileElement3.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileElement3.Name = "radTileElement3";
            this.radTileElement3.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileElement3.Text = "数据校验";
            this.radTileElement3.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileElement3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileElement3.TextWrap = true;
            this.radTileElement3.Click += new System.EventHandler(this.radTileElement3_Click);
            this.radTileElement3.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileElement3.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTileElement4
            // 
            this.radTileElement4.AccessibleDescription = "自动比对";
            this.radTileElement4.AccessibleName = "自动比对";
            this.radTileElement4.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTileElement4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTileElement4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement4.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement4.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement4.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement4.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileElement4.Column = 2;
            this.radTileElement4.DrawBorder = true;
            this.radTileElement4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileElement4.Image = global::OnSiteFundComparer.Properties.Resources.magnifier_icon;
            this.radTileElement4.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileElement4.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileElement4.Margin = new System.Windows.Forms.Padding(0);
            this.radTileElement4.Name = "radTileElement4";
            this.radTileElement4.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileElement4.Text = "自动比对";
            this.radTileElement4.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileElement4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileElement4.Click += new System.EventHandler(this.radTileElement4_Click);
            this.radTileElement4.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileElement4.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);

            // 
            // radTileElement4
            // 
            this.radTileInputResult.AccessibleDescription = "问题线索核查录入";
            this.radTileInputResult.AccessibleName = "问题线索核查录入";
            this.radTileInputResult.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTileInputResult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTileInputResult.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileInputResult.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileInputResult.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileInputResult.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileInputResult.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileInputResult.Column = 1;
            this.radTileInputResult.Row = 1;
            this.radTileInputResult.ColSpan = 2;
            this.radTileInputResult.DrawBorder = true;
            this.radTileInputResult.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileInputResult.Image = global::OnSiteFundComparer.Properties.Resources.Chart;
            this.radTileInputResult.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileInputResult.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileInputResult.Margin = new System.Windows.Forms.Padding(0);
            this.radTileInputResult.Name = "radTileInputResult";
            this.radTileInputResult.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileInputResult.Text = "问题线索核查录入";
            this.radTileInputResult.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileInputResult.TextWrap = true;
            this.radTileInputResult.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileInputResult.Click += new System.EventHandler(this.radTileInputResult_Click);
            this.radTileInputResult.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileInputResult.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // SettingGroup
            // 
            this.SettingGroup.AccessibleDescription = "设置";
            this.SettingGroup.AccessibleName = "设置";
            this.SettingGroup.CellSize = new System.Drawing.Size(165, 165);
            this.SettingGroup.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Italic);
            this.SettingGroup.ForeColor = System.Drawing.Color.White;
            this.SettingGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            //this.radTitledataSetting,
            this.radTitlesystemSetting,
            this.radTitlehelp});
            this.SettingGroup.Margin = new System.Windows.Forms.Padding(0, 130, 65, 0);
            this.SettingGroup.Name = "SettingGroup";
            this.SettingGroup.RowsCount = 2;
            this.SettingGroup.Text = "设置";
            // 
            // radTitledataSetting
            // 
            //this.radTitledataSetting.AccessibleDescription = "数据设置";
            //this.radTitledataSetting.AccessibleName = "数据设置";
            //this.radTitledataSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(161)))), ((int)(((byte)(209)))));
            //this.radTitledataSetting.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            //this.radTitledataSetting.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            //this.radTitledataSetting.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            //this.radTitledataSetting.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            //this.radTitledataSetting.CellPadding = new System.Windows.Forms.Padding(5);
            //this.radTitledataSetting.DrawBorder = true;
            //this.radTitledataSetting.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            //this.radTitledataSetting.Image = global::OnSiteFundComparer.Properties.Resources.GridView;
            //this.radTitledataSetting.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            //this.radTitledataSetting.ImageLayout = System.Windows.Forms.ImageLayout.None;
            //this.radTitledataSetting.Name = "radTitledataSetting";
            //this.radTitledataSetting.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            //this.radTitledataSetting.Text = "数据设置";
            //this.radTitledataSetting.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            //this.radTitledataSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            //this.radTitledataSetting.TextWrap = true;
            //this.radTitledataSetting.Click += new System.EventHandler(this.radTitledataSetting_Click);
            //this.radTitledataSetting.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            //this.radTitledataSetting.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTitlesystemSetting
            // 
            this.radTitlesystemSetting.AccessibleDescription = "系统设置";
            this.radTitlesystemSetting.AccessibleName = "系统设置";
            this.radTitlesystemSetting.BackColor = System.Drawing.Color.Empty;
            this.radTitlesystemSetting.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.magnifier_bg;
            this.radTitlesystemSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.radTitlesystemSetting.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTitlesystemSetting.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTitlesystemSetting.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTitlesystemSetting.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTitlesystemSetting.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTitlesystemSetting.Column = 0;
            this.radTitlesystemSetting.Row = 1;
            this.radTitlesystemSetting.DrawBorder = true;
            this.radTitlesystemSetting.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTitlesystemSetting.Image = global::OnSiteFundComparer.Properties.Resources.magnifier_icon;
            this.radTitlesystemSetting.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTitlesystemSetting.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTitlesystemSetting.Name = "radTitlesystemSetting";
            this.radTitlesystemSetting.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTitlesystemSetting.Text = "系统管理";
            this.radTitlesystemSetting.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTitlesystemSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTitlesystemSetting.Click += new System.EventHandler(this.radTitlesystemSetting_Click);
            this.radTitlesystemSetting.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTitlesystemSetting.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTitlehelp
            // 
            this.radTitlehelp.AccessibleDescription = "使用说明";
            this.radTitlehelp.AccessibleName = "使用说明";
            this.radTitlehelp.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTitlehelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.radTitlehelp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTitlehelp.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTitlehelp.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTitlehelp.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTitlehelp.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTitlehelp.ColSpan = 2;
            this.radTitlehelp.DrawBorder = true;
            this.radTitlehelp.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTitlehelp.Image = global::OnSiteFundComparer.Properties.Resources.vsb_icon;
            this.radTitlehelp.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTitlehelp.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTitlehelp.Margin = new System.Windows.Forms.Padding(0);
            this.radTitlehelp.Name = "radTitlehelp";
            this.radTitlehelp.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTitlehelp.Row = 0;
            this.radTitlehelp.Text = "比对规则管理";
            this.radTitlehelp.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTitlehelp.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTitlehelp.Click += new System.EventHandler(this.radTitlehelp_Click);
            this.radTitlehelp.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTitlehelp.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // radTileElement2
            // 
            this.radTileElement2.AccessibleDescription = "数据查询";
            this.radTileElement2.AccessibleName = "数据查询";
            this.radTileElement2.BackgroundImage = global::OnSiteFundComparer.Properties.Resources.qsf_bg;
            this.radTileElement2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTileElement2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement2.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(64)))), ((int)(((byte)(172)))));
            this.radTileElement2.BorderColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement2.BorderColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(128)))), ((int)(((byte)(197)))));
            this.radTileElement2.BorderGradientStyle = Telerik.WinControls.GradientStyles.Linear;
            this.radTileElement2.CellPadding = new System.Windows.Forms.Padding(5);
            this.radTileElement2.Column = 3;
            this.radTileElement2.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.radTileElement2.DrawBorder = true;
            this.radTileElement2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radTileElement2.Image = global::OnSiteFundComparer.Properties.Resources.Scheduler;
            this.radTileElement2.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.radTileElement2.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radTileElement2.Name = "radTileElement2";
            this.radTileElement2.Padding = new System.Windows.Forms.Padding(15, 15, 15, 10);
            this.radTileElement2.Row = 1;
            this.radTileElement2.Text = "数据查询";
            this.radTileElement2.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.radTileElement2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.radTileElement2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.radTileElement2.MouseEnter += new System.EventHandler(this.radTile_MouseEnter);
            this.radTileElement2.MouseLeave += new System.EventHandler(this.radTile_MouseLeave);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1074, 549);
            this.Controls.Add(this.radPanorama1);
            this.Icon = global::OnSiteFundComparer.Properties.Resources.icon;
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "惠民政策监督检查";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RadPanorama radPanorama1;
        private TileGroupElement FunctionGroup;
        private TileGroupElement SettingGroup;

        private RadTileElement radTileDataQuery;
        private RadTileElement radTileReport;
        //private RadTileElement radTitledataSetting;
        private RadTileElement radTitlesystemSetting;
        private RadTileElement radTitlehelp;
        private RadTileElement radTileElement1;
        private RadTileElement radTileElement3;
        private RadTileElement radTileElement4;
        private RadTileElement radTileElement2;
        private RadTileElement radTileInputResult;
    }
}

