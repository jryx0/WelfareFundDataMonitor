using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Web.UI;

namespace FileTranser.MTOM.ClassLibrary
{
    public class WebServicesHelp
    {
        private FileTranserServiceProxy.FileTransferWebService theWebservice =
            new FileTranserServiceProxy.FileTransferWebService();

        public ProgressChangedEventHandler progressChangeEvent;
        public RunWorkerCompletedEventHandler runWorkerComplateEvent;

        public String LoginToken = "";

        public string LoginUsername;

        public string Url
        {
            get { return theWebservice.Url; }
            set { theWebservice.Url = value; }
        }

        public WebServicesHelp()
        {
            theWebservice.CookieContainer = new CookieContainer();
        }
        public WebServicesHelp(string url)
        {
            theWebservice.Url = url;
        }

        public string IsUpdate(string version)
        {
            return theWebservice.UpdateInfo(version);
        }
        public string Login(string Username, string Password)
        {
            LoginToken = theWebservice.Login(LoginToken, Username, Password);
            return LoginToken;
        }
        public bool Logout(string Username, string Password)
        {
            string postData = String.Format("?Username={0}&Password={1}", Username, Password);
            string url = theWebservice.Url.Replace("FileTransfer.asmx", "") + "Logout.aspx" + postData; // get the path of the login page, assuming it is in the same folder as the web service
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.CookieContainer = theWebservice.CookieContainer;

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            return true;
        }

        public string GetCurrentUserPath()
        {
            return theWebservice.GetCurrentUserPath(LoginToken);
        }

        public string GetUploadToken(string FileName)
        {
            return theWebservice.GetUploadToke(LoginToken, FileName);
        }
        public void UploadFile(string uploadtoken, string FileName)
        {
            string guid = Guid.NewGuid().ToString();
            Triplet t = new Triplet(LoginToken, uploadtoken, 0);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.StartUploadFile), new Triplet(guid, FileName, t));//new Triplet(guid, FileName, 0));
        }
        public void FinishUpload(String UploadToken, int Status, String InfoMsg, String ErrorMsg)
        {
            theWebservice.EndAppendChunk(UploadToken, Status, InfoMsg, ErrorMsg);

        }


        public String GetDataStatus(string dbInfo)
        {
            return theWebservice.GetDataStatus(LoginToken, dbInfo);
        }
        public String SetDefaultData(string dbInfo)
        {
            return theWebservice.SetDefaultData(LoginToken, dbInfo);
        }


        public byte[] GetCheckData(int RowID, String ID)
        {
            return theWebservice.GetClues(LoginToken, RowID, ID);
        }

        public byte[] UpdateCheckData(byte[] buffer)
        {
            return theWebservice.UpdateCheckData(LoginToken, buffer);
        }

        public String GetUpLoadInfo()
        {
            return theWebservice.GetUpLoadInfo(LoginToken);
        }

        public byte[] UpdateReCheckData(byte[] buffer)
        {
            return theWebservice.UpdateReCheckData(LoginToken, buffer);
        }

        /// <summary>
        /// This method is used as a thread start function.
        /// The parameter is a Triplet because the ThreadStart can only take one object parameter
        /// </summary>
        /// <param name="triplet">First=guid; Second=path; Third=offset</param>
        private void StartUploadFile(object para)
        {
            string guid = (para as  Triplet).First.ToString();
            string path = (para as  Triplet).Second.ToString();
            Triplet t = (Triplet)(para as Triplet).Third ;

            string logintoken = t.First.ToString();
            string uploadertoken = t.Second.ToString();
            long offset = Int64.Parse(t.Third.ToString());
           

            FileTransferUpload ftu = new FileTransferUpload();
            ftu.theWebservices = theWebservice;
            ftu.Guid = guid;

            ftu.AutoSetChunkSize = true;

            ftu.LocalFilePath = path;
            ftu.IncludeHashVerification = true;
            ftu.UploadToken = uploadertoken;
            ftu.LoginToken = logintoken;
             
            if(progressChangeEvent != null)
                ftu.ProgressChanged += progressChangeEvent;

            if(runWorkerComplateEvent != null)
                ftu.RunWorkerCompleted += runWorkerComplateEvent;

            ftu.RunWorkerSync(new DoWorkEventArgs(offset));
        }
    }
}
