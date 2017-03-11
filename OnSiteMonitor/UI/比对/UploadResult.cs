using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class UploadResult : Form
    {
        public Models.Task task;
        public String FilePath;
        private String BakFilePath;
        private String ZipFilePath;

        public FileTranser.MTOM.ClassLibrary.WebServicesHelp ws;
        public UploadResult()
        {
            InitializeComponent();

            //ws = GlobalEnviroment.theWebService;

            lbInfo.Text = "";
            lbProgress.Text = "";
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            System.Threading.Thread.Sleep(1000);

            BakFilePath = FilePath + ".bak";

            lbInfo.Text = "备份结果文件...";
            File.Copy(FilePath, BakFilePath);

            lbInfo.Text = "正在压缩文件...";
            ZipFilePath = CompressResult(BakFilePath);

            lbInfo.Text = "开始上传文件...";
            UploadFile(ZipFilePath);
        }


        private string CompressResult(string FileName)
        {
            string ZipFile = FileName + ".gz";
            FileStream inputStream =
          new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream outputStream =
                new FileStream(ZipFile, FileMode.Create, FileAccess.Write);

            // 决定一次读取数剧的大小，这里是8KB
            int bufferSize = 8192;
            int bytesRead = 0;
            byte[] buffer = new byte[bufferSize];

            GZipStream compressionStream =
                new GZipStream(outputStream, CompressionMode.Compress);
            // bytesRead返回每次读了多少数据，如果等于0就表示已经没有数据
            // 可以读了
            while ((bytesRead = inputStream.Read(buffer, 0, bufferSize)) > 0)
            {
                // 把读到数组中的数据通过GZipStream写入到输出数据流
                compressionStream.Write(buffer, 0, bytesRead);
            }
            compressionStream.Close();

            inputStream.Close();
            outputStream.Close();

            return ZipFile;
        }



        private void UploadFile(string uploadFileName)
        {
            if (!File.Exists(uploadFileName))
                this.DialogResult = DialogResult.Cancel;

             
            var loginToken = ws.Login( GlobalEnviroment.LoginedUser.Name, GlobalEnviroment.LoginedUser.Password);

            if (loginToken.Length != 0)
            {
                GlobalEnviroment.LoginToken = loginToken;
                ws.progressChangeEvent = new ProgressChangedEventHandler(ft_ProgressChanged);
                ws.runWorkerComplateEvent = new RunWorkerCompletedEventHandler(ft_RunWorkerCompleted);

                var uploadtoken = ws.GetUploadToken(Path.GetFileName(uploadFileName));
                ws.UploadFile(uploadtoken, uploadFileName);
            }
        }

        void ft_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EndOperation(e);
        }
        void ft_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged((sender as FileTranser.MTOM.ClassLibrary.FileTransferBase).Guid, e.ProgressPercentage, e.UserState.ToString());
        }


        delegate void EndOperationDelegate(RunWorkerCompletedEventArgs e);
        public void EndOperation(RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EndOperationDelegate(this.EndOperation), new object[] { e });
                return;
            }

            if (e.Error != null)
            {
                // this.label2.Text = e.Error.Message;
                ws.FinishUpload(e.Result.ToString(), -1, task.TaskName + "$" + task.TaskComment, e.Error.Message.Substring(0, 498));
                File.Delete(ZipFilePath);
                File.Delete(BakFilePath);

                this.DialogResult = DialogResult.Cancel;
            }
            else if (e.Cancelled)
            {
                //  this.label2.Text = "取消上传！";
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                // this.label2.Text = "校验成功！上传完成。";
                ws.FinishUpload(e.Result.ToString(), 10, task.TaskName + "$" + task.TaskComment, "校验成功");

                File.Delete(ZipFilePath);
                File.Delete(BakFilePath);

                this.DialogResult = DialogResult.OK;
            }
        }

        delegate void ProgressChangedDelegate(string Guid, int ProgressPercentage, string Message);
        public void ProgressChanged(string Guid, int ProgressPercentage, string Message)
        {
            if (this.InvokeRequired)
            {
                Invoke(new ProgressChangedDelegate(this.ProgressChanged), new object[] { Guid, ProgressPercentage, Message });
                return;
            }

            if (ProgressPercentage > 0 && ProgressPercentage < progressBar1.Maximum)
            {
                progressBar1.Value = ProgressPercentage;

                lbProgress.Text = ProgressPercentage.ToString() + "%";
                lbInfo.Text = Message;
            }
        }


    }
}
