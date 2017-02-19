
using log4net.Core;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.IO;

namespace OnSiteFundComparer.Test
{
    public partial class 测试比对 : Form
    {
        private log4net.ILog log;
        private bool logWatching = true;
        private log4net.Appender.MemoryAppender logger;
        private Thread logWatcher;

        public delegate void finished( );
        public finished myDelegate1;
        public finished myDelegate2;
        Models.Task compareTask;

        public bool IsDataCheck = false;

        public String ComparingInfo = "";

        public 测试比对()
        {
            InitializeComponent();

            // this.ControlBox = false;
            compareTask = new Models.Task();


            compareTask.TaskName = OnSiteFundComparer.Properties.Settings.Default.CurrentRegion + "大数据测试比对";
            compareTask.TaskComment = OnSiteFundComparer.Properties.Settings.Default.CurentRegionName +
                   System.DateTime.Now.ToString("yyyy年MM月dd日HH点mm分ss秒") + "，进行惠民政策监督检查大数据分析比对。";

            compareTask.Region = Properties.Settings.Default.CurrentRegion;

            log = GlobalEnviroment.log;
            this.Closing += new CancelEventHandler(Form_Closing);
            logger = new log4net.Appender.MemoryAppender();
            log4net.Config.BasicConfigurator.Configure(logger);
            logWatcher = new Thread(new ThreadStart(LogWatcher));
            logWatcher.Start();

            myDelegate1 = new finished(ShowExitButton);
            myDelegate2 = new finished(ShowDirButton);
        }

        public void ShowExitButton( )
        {
          this.btnExit.Enabled = true;
        }
        public void ShowDirButton()
        {
            this.btnOpenDir.Enabled = true;
        }


        protected override void OnLoad(EventArgs e)
        {
            if (File.Exists(GlobalEnviroment.ResultOutputDir + "\\日志.test.txt"))
                File.Delete(GlobalEnviroment.ResultOutputDir + "\\日志.test.txt");


            ThreadStart starter = delegate { DataComparing(compareTask); };
            //new Thread(new ThreadStart(this.DataComparing)).Start();
            new Thread(starter).Start();

            base.OnLoad(e);
        }


        private void DataComparing(Models.Task _t)
        {
            log.Info("比对开始>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            //CheckComparing comparor = new CheckComparing();
            //comparor.IsDataChecking = IsDataCheck;
            //ComparingInfo = comparor.Start(_t);

            //if (ComparingInfo.Length == 0)
            //{
            //    log.Info("------------------------------------------------------");
            //    log.Info("|            比对错误，请检查文件格式                    |");
            //    log.Info("------------------------------------------------------");
            //    //new Business.FundComparing().Start(_t);
            //}           

            this.btnExit.Invoke(myDelegate1);
            this.btnOpenDir.Invoke(myDelegate2);


        }

        private void FinishComputing()
        {
            this.btnExit.Enabled = true;
            btnOpenDir.Enabled = true;
        }


        #region logger
        void Form_Closing(object sender, CancelEventArgs e)
        {
            logWatching = false;
            logWatcher.Join();

            using (FileStream fs = new FileStream(
                       GlobalEnviroment.ResultOutputDir + "\\日志.txt", FileMode.Append, FileAccess.Write))
            {

                byte[] arrWriteData = Encoding.Default.GetBytes(tbLogWin.Text);
                fs.Write(arrWriteData, 0, arrWriteData.Length);
                fs.Close();
            }
        }

        delegate void delOneStr(string log);
        void AppendLog(string _log)
        {
            if (tbLogWin.InvokeRequired)
            {
                delOneStr dd = new delOneStr(AppendLog);
                tbLogWin.Invoke(dd, new object[] { _log });
            }
            else
            {
                StringBuilder builder;
                if (tbLogWin.Lines.Length > 2000)
                {
                    builder = new StringBuilder(tbLogWin.Text);

                    using (FileStream fs = new FileStream(
                        GlobalEnviroment.ResultOutputDir + "\\日志.txt", FileMode.Append, FileAccess.Write))
                    {
                        int length = tbLogWin.Text.IndexOf('\r', 5000);
                        if (length > tbLogWin.Text.Length - 3) length = tbLogWin.Text.Length - 3;
                        string txt = tbLogWin.Text.Substring(0, length + 2);
                        byte[] arrWriteData = Encoding.Default.GetBytes(txt);
                        fs.Write(arrWriteData, 0, arrWriteData.Length);
                        fs.Close();
                    }

                    builder.Remove(0, tbLogWin.Text.IndexOf('\r', 5000) + 2);
                    builder.Append(_log);
                    tbLogWin.Clear();
                    tbLogWin.AppendText(builder.ToString());
                    
                }
                else
                {
                    tbLogWin.AppendText(_log);
                }
            }
        }


        private void LogWatcher()
        {
            while (logWatching)
            {
                LoggingEvent[] events = logger.GetEvents();
                if (events != null && events.Length > 0)
                {
                    logger.Clear();
                    foreach (LoggingEvent ev in events)
                    {
                        string info = "[" + ev.Level + "]  ";
                        string line = ev.TimeStamp.ToLocalTime() + info + ev.RenderedMessage + "\r\n";
                        AppendLog(line);
                    }
                }
                Thread.Sleep(50);
            }
        }
        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnOpenDir_Click(object sender, EventArgs e)
        {
            ProcessStartInfo proc = new ProcessStartInfo("explorer.exe", OnSiteFundComparer.GlobalEnviroment.ResultOutputDir);
            Process.Start(proc);
            this.DialogResult = DialogResult.OK;
        }
    }
}
