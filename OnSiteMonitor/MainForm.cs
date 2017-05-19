//#define 试点
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OnSiteFundComparer.Properties;
using Telerik.WinControls.UI;
using Telerik.WinControls.Themes;
using Telerik.WinControls;
using System.IO;
using System.Diagnostics;
using OnSiteFundComparer.UI;

namespace OnSiteFundComparer
{
    public partial class MainForm : RadForm
    {


        public const int WM_SIZE = 5;
#if 试点
        public const String headerString = "惠民政策监督检查软件-试点版";  //大标题
#else
        public const String headerString = "精准扶贫政策落实情况监督检查软件(单机版)";  //大标题
#endif
        public FileTranser.MTOM.ClassLibrary.WebServicesHelp theWebService;

        private RadButtonElement backButton;
        private LightVisualElement headerLabel;
        private RadLabelElement regionLabel;
        private RadTitleBarElement titleBar;
        private bool isFormMoving = false;

        public Models.User currentUser = new Models.User();

        #region Initialization
        public MainForm()
        {



            InitializeComponent();

            new TelerikMetroBlueTheme();
            ThemeResolutionService.LoadPackageResource("OnSiteFundComparer.PanoramaDemo.tssp");


            this.radPanorama1.ThemeName = "PanoramaDemo";
            this.radPanorama1.ScrollingBackground = true;

            this.radPanorama1.PanoramaElement.BackgroundImagePrimitive.ImageLayout = ImageLayout.Tile;
            this.radPanorama1.SizeChanged += new EventHandler(radTilePanel1_SizeChanged);
            this.radPanorama1.ScrollBarAlignment = HorizontalScrollAlignment.Bottom;
            this.radPanorama1.ScrollBarThickness = 5;
            this.radPanorama1.PanoramaElement.GradientStyle = GradientStyles.Solid;
            this.radPanorama1.PanoramaElement.DrawFill = true;
            this.radPanorama1.PanoramaElement.BackColor = System.Drawing.Color.FromArgb(238, 238, 242);
            this.FormElement.TitleBar.MaxSize = new Size(0, 1);

            this.PrepareHeader();
            this.PrepareTitleBar();

            //InitGlobalPara();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PrepareUser();
            PrepareRegion();

            if (GlobalEnviroment.LoginedUser.Name.ToLower() == "admin")
                this.radTileNewRules.Visibility = ElementVisibility.Visible;
            else this.radTileNewRules.Visibility = ElementVisibility.Hidden;

            this.Cursor = Cursors.Arrow;
        }

        private void PrepareTitleBar()
        {
            titleBar = new RadTitleBarElement();

            titleBar.FillPrimitive.Visibility = ElementVisibility.Hidden;
            titleBar.MaxSize = new Size(0, 30);
            titleBar.Children[1].Visibility = ElementVisibility.Hidden;

            titleBar.CloseButton.Parent.PositionOffset = new SizeF(0, -10);
            titleBar.CloseButton.MinSize = new Size(50, 50);
            titleBar.CloseButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;


            titleBar.MinimizeButton.MinSize = new Size(50, 50);
            titleBar.MinimizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.MaximizeButton.MinSize = new Size(50, 50);
            titleBar.MaximizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.CloseButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MinimizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MaximizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);

            titleBar.Close += new TitleBarSystemEventHandler(titleBar_Close);
            titleBar.Minimize += new TitleBarSystemEventHandler(titleBar_Minimize);
            titleBar.MaximizeRestore += new TitleBarSystemEventHandler(titleBar_MaximizeRestore);
            this.radPanorama1.PanoramaElement.PanGesture += new PanGestureEventHandler(radTilePanel1_PanGesture);
            this.radPanorama1.PanoramaElement.Children.Add(titleBar);
        }
        private void PrepareHeader()
        {
            StackLayoutElement headerLayout = new StackLayoutElement();
            headerLayout.Orientation = Orientation.Horizontal;
            headerLayout.Margin = new System.Windows.Forms.Padding(0, 35, 0, 0);
            headerLayout.NotifyParentOnMouseInput = true;
            headerLayout.ShouldHandleMouseInput = false;
            headerLayout.StretchHorizontally = false;

            this.backButton = new RadButtonElement() { StretchHorizontally = false };
            this.backButton.Margin = new Padding(40, 0, 28, 0);
            this.backButton.Click += new EventHandler(backButton_Click);
            this.backButton.Visibility = ElementVisibility.Hidden;
            headerLayout.Children.Add(this.backButton);

            this.headerLabel = new LightVisualElement();
            this.headerLabel.Text = headerString;//"Demo Apps Hub";
            this.headerLabel.Font = new Font("微软雅黑", 30, GraphicsUnit.Point);
            this.headerLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.headerLabel.ForeColor = Color.White;//.DimGray;
            this.headerLabel.TextAlignment = ContentAlignment.MiddleLeft;
            this.headerLabel.MaxSize = new Size(850, 110);
            this.headerLabel.NotifyParentOnMouseInput = true;
            this.headerLabel.ShouldHandleMouseInput = false;
            this.headerLabel.StretchHorizontally = false;
            headerLayout.Children.Add(this.headerLabel);

            this.radPanorama1.PanoramaElement.Children.Add(headerLayout);

            this.DoubleBuffered = true;
        }

        private void PrepareRegion()
        {
            StackLayoutElement headerLayout = new StackLayoutElement();
            headerLayout.Orientation = Orientation.Horizontal;
            headerLayout.Margin = new System.Windows.Forms.Padding(810, 60, 0, 0);
            headerLayout.NotifyParentOnMouseInput = true;
            headerLayout.ShouldHandleMouseInput = false;
            headerLayout.StretchHorizontally = false;


            this.regionLabel = new RadLabelElement();
            this.regionLabel.Text = "地区:  " + Properties.Settings.Default.CurentRegionName; //"Demo Apps Hub";
            this.regionLabel.Font = new Font("微软雅黑", 13, GraphicsUnit.Point);
            this.regionLabel.ForeColor = Color.LightGray;
            this.regionLabel.TextAlignment = ContentAlignment.MiddleLeft;
            //this.regionLabel.MaxSize = new Size(630, 110);
            //this.regionLabel.NotifyParentOnMouseInput = true;
            //this.regionLabel.ShouldHandleMouseInput = false;
            //this.regionLabel.StretchHorizontally = false;
            this.regionLabel.Margin = new Padding(-150, 40, 28, 0);

            //this.regionLabel.Click += new EventHandler(region_click);


            headerLayout.Children.Add(regionLabel);

            this.radPanorama1.PanoramaElement.Children.Add(headerLayout);

            this.DoubleBuffered = true;
        }
        private void region_click(object sender, EventArgs e)
        {
            if (new 地区设置().ShowDialog() == DialogResult.OK)
            {
                this.regionLabel.Text = "地区:  " + Properties.Settings.Default.CurentRegionName; //"Demo Apps Hub";
                //headerLabel.Text = headerString + Properties.Settings.Default.CurentRegionName;
                this.regionLabel.ResumeReferenceUpdate();
            }
        }


        private void PrepareUser()
        {
            StackLayoutElement headerLayout = new StackLayoutElement();
            headerLayout.Orientation = Orientation.Horizontal;
            headerLayout.Margin = new System.Windows.Forms.Padding(650, 60, 0, 0);
            headerLayout.NotifyParentOnMouseInput = true;
            headerLayout.ShouldHandleMouseInput = false;
            headerLayout.StretchHorizontally = false;



            RadLabelElement rLabel = new RadLabelElement();
            rLabel.Font = new Font("微软雅黑", 13, GraphicsUnit.Point);
            rLabel.Text = "用户:  " + currentUser.Name;
            rLabel.ForeColor = Color.LightGray;
            rLabel.Margin = new Padding(-150, 40, 28, 0);
            rLabel.Click += new EventHandler(user_click);

            headerLayout.Children.Add(rLabel);

            this.radPanorama1.PanoramaElement.Children.Add(headerLayout);

            this.DoubleBuffered = true;
        }

        private void user_click(object sender, EventArgs e)
        {
            //ModifyPassword mp = new ModifyPassword();

            //mp.UserName = currentUser.Name;
            //if(mp.ShowDialog() == DialogResult.OK)
            //{
            //    currentUser.Name = mp.Name;
            //    currentUser.Password = mp.PassWord;
            //    MessageBox.Show("密码更新成功!");
            //}
        }

        protected override void OnShown(EventArgs e)
        {
            this.radTilePanel1_SizeChanged(this, EventArgs.Empty);
            //foreach (KeyValuePair<string, UserControl> entry in this.exampleControls)
            //{
            //    entry.Value.PerformLayout();
            //    this.Controls.Remove(entry.Value);
            //}

            base.OnShown(e);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SIZE)
            {
                titleBar.CloseButton.SetValue(RadFormElement.FormWindowStateProperty, this.WindowState);
                titleBar.MinimizeButton.SetValue(RadFormElement.FormWindowStateProperty, this.WindowState);
                titleBar.MaximizeButton.SetValue(RadFormElement.FormWindowStateProperty, this.WindowState);
            }

            base.WndProc(ref m);
        }
        protected override FormControlBehavior InitializeFormBehavior()
        {
            return new MyFormBehavior(this, true);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (GlobalEnviroment.LocalVersion == false)
                theWebService.Logout(currentUser.Name, currentUser.Password);

            base.OnFormClosed(e);
        }

        protected override void OnClosed(EventArgs e)
        {

            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        #region Event Handlers

        void radTilePanel1_PanGesture(object sender, PanGestureEventArgs e)
        {
            if (e.IsBegin && this.titleBar.ControlBoundingRectangle.Contains(e.Location))
            {
                isFormMoving = true;
            }

            if (isFormMoving)
            {
                this.Location = new Point(this.Location.X + e.Offset.Width, this.Location.Y + e.Offset.Height);
            }
            else
            {
                e.Handled = false;
            }

            if (e.IsEnd)
            {
                isFormMoving = false;
            }
        }

        //Example, how to use RunProcess
        void rssReaderTile_Click(object sender, System.EventArgs e)
        {
            RunProcess(Application.StartupPath + @"\..\..\RssReader\Bin\RssReader.exe");
        }

        private void RunProcess(string executablePath)
        {
            if (File.Exists(executablePath))
            {
                ProcessStartInfo proc = new ProcessStartInfo(executablePath);
                proc.WorkingDirectory = Path.GetDirectoryName(executablePath);
                Process.Start(proc);
            }
            else
            {
                RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        void radTilePanel1_SizeChanged(object sender, EventArgs e)
        {
            int width = this.radPanorama1.Width + Math.Max((this.radPanorama1.PanoramaElement.ScrollBar.Maximum - this.radPanorama1.Width) / 4, 1);
            this.radPanorama1.PanelImageSize = new Size(width, 768);
            this.radPanorama1.PanoramaElement.UpdateViewOnScroll();
        }

        void backButton_Click(object sender, EventArgs e)
        {

        }

        void titleBar_MaximizeRestore(object sender, EventArgs args)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        void titleBar_Minimize(object sender, EventArgs args)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void titleBar_Close(object sender, EventArgs args)
        {
            Application.Exit();
        }
        #endregion



        #region 处理鼠标移动
        private void radTile_MouseEnter(object sender, EventArgs e)
        {
            setMouseEnter((RadTileElement)sender);
        }
        private void radTile_MouseLeave(object sender, EventArgs e)
        {
            setMouseLeave((RadTileElement)sender);
        }

        private void setMouseEnter(RadTileElement element)
        {
            element.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
        }
        private void setMouseLeave(RadTileElement element)
        {
            element.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
        }

        #endregion

        #endregion

        #region 调用其他功能



        //private void radTileReport_Click(object sender, EventArgs e)
        //{//上报数据

        //}

        //private void radTitledataSetting_Click(object sender, EventArgs e)
        //{//数据设置

        //}

        //private void radTitlesystemSetting_Click(object sender, EventArgs e)
        //{//系统设置
        //    sysSetting set = new sysSetting();

        //    set.ShowDialog();
        //}

        //private void radTitlehelp_Click(object sender, EventArgs e)
        //{//使用帮助

        //}
        #endregion



        /// <summary>
        /// //数据导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileElement1_Click(object sender, EventArgs e)
        {
            new DataStandardize().ShowDialog();
        }
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileElement3_Click(object sender, EventArgs e)
        { 
            if (MessageBox.Show("请确认人口数据文件已导入!", "数据校验", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            数据比对 dlg = new 数据比对(new Models.Task());
            dlg.Text = "比对数据校验...";
            dlg.IsDataCheck = true;
            dlg.ShowDialog();
        }

        /// <summary>
        /// 自动比对
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileElement4_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (GlobalEnviroment.LoginedUser.Name.ToLower() == "admin")
            {//显示比对规则目录

            }




            var dlg = new UI.任务();
            var ret = dlg.ShowDialog();
            if (ret == DialogResult.OK)
            {
                new 数据比对(dlg.task).ShowDialog();
            }
            else if (ret == DialogResult.Yes)
            {
                new QuickCompare.UI.CompareMonitorUI().ShowDialog();
            }
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 核查上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileInputResult_Click(object sender, EventArgs e)
        { 
            new 线索查询().ShowDialog();
        }
        /// <summary>
        /// 数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTitledataSetting_Click(object sender, EventArgs e)
        {
         //  new 数据设置().ShowDialog();
        }

        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTitlesystemSetting_Click(object sender, EventArgs e)
        {
            if (new 系统设置().ShowDialog() == DialogResult.OK)
            {
                this.regionLabel.Text = "地区:  " + Properties.Settings.Default.CurentRegionName; //"Demo Apps Hub";
                //headerLabel.Text = headerString + Properties.Settings.Default.CurentRegionName;
                this.regionLabel.ResumeReferenceUpdate();
            }
        }

        /// <summary>
        /// 规则管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTitlehelp_Click(object sender, EventArgs e)
        {
            new 规则管理().ShowDialog();
        }

        /// <summary>
        /// 结果数据库管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileReport_Click(object sender, EventArgs e)
        {
            new 结果数据库管理().ShowDialog();
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileDataQuery_Click(object sender, EventArgs e)
        {           
            new 数据查询().ShowDialog();
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileNewRules_Click(object sender, EventArgs e)
        {
            if (currentUser.Name.ToLower() == "admin")
            {

                new Test.测试().Show();
            }
           
        }
    }

    public class MyFormBehavior : RadFormBehavior
    {
        public MyFormBehavior(IComponentTreeHandler treeHandler, bool shouldCreateChildren) :
            base(treeHandler, shouldCreateChildren)
        {
        }

        public override Padding BorderWidth
        {
            get
            {
                return new Padding(1);
            }
        }
    }
}
