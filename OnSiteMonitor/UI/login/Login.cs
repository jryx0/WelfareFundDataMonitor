using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace OnSiteFundComparer
{

    public partial class Login : Form
    {
        // the value of permissions on the error at one login procedure 
        private int _logins;
        // the flag of validate
        private bool _ValidForm;

        public static Models.User LoginedUser = null;
        public FileTranser.MTOM.ClassLibrary.WebServicesHelp ws;
        public static bool Logged
        {
            get; set;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Text += Properties.Settings.Default.Version;
            base.OnLoad(e);
        }

        public Login()
        {
            InitializeComponent();
            Login.Logged = false;
            tbUserName.Validating += new CancelEventHandler(ValidateTextBox);
            tbPassword.Validating += new CancelEventHandler(ValidateTextBox);

            
            ws = new FileTranser.MTOM.ClassLibrary.WebServicesHelp();
            //ws.Url = Properties.Settings.Default.WebServicesUrl + "WFMUploader/FileTransfer.asmx";
            ws.Url = Properties.Settings.Default.WebServicesUrl + "TMPAServices/FileTransfer.asmx";

            lbInfo.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_ValidForm)
            {
                this.Cursor = Cursors.WaitCursor;

                if (GlobalEnviroment.LocalVersion)
                {
                    DAL.MySqlite _sqlite = new DAL.MySqlite(GlobalEnviroment.MainDBFile);
                    try
                    {
                        var ds = _sqlite.ExecuteScalar(CommandType.Text,
                            @"select ParentName || '\'|| RegionName  as name from user join   vw_region on user.regionid = vw_region.Rowid
where Status = 1 and username=@usernam and password=@password",
                            new SQLiteParameter[] {
                            new System.Data.SQLite.SQLiteParameter("@usernam", tbUserName.Text.ToLower()),
                            new System.Data.SQLite.SQLiteParameter("@password", tbPassword.Text)
                            });

                        if (ds == null)
                        {
                            lbInfo.Text = "用户名密码错误！";
                            lbInfo.ForeColor = Color.Red;

                            //MessageBox.Show("用户名密码错误!");
                            this.Cursor = Cursors.Arrow;
                        }
                        else
                        {
                            var RegionName = ds.ToString().Split('\\');
                            if (RegionName.Length == 3)
                                Properties.Settings.Default.CurentRegionName = RegionName[1] + RegionName[2];
                            else if (RegionName.Length == 2)
                                Properties.Settings.Default.CurentRegionName = RegionName[0] + RegionName[1];
                            else Properties.Settings.Default.CurentRegionName = RegionName[0];

                            if (RegionName.Length != 0)
                                Properties.Settings.Default.CurrentRegion = RegionName[RegionName.Length - 1];

                            Properties.Settings.Default.Save();
                            GlobalEnviroment.LoginToken = "";

                            Login.LoginedUser = new Models.User { Name = tbUserName.Text.ToLower(), Password = tbPassword.Text };
                            Login.Logged = true;

                            this.Hide();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    string logintoken = ws.Login(tbUserName.Text.ToLower(), tbPassword.Text);
                    if (logintoken.Length != 0)
                    {
                        if (PrepareMain(logintoken))
                        {
                            Login.LoginedUser = new Models.User { Name = tbUserName.Text.ToLower(), Password = tbPassword.Text };
                            Login.Logged = true;

                            GlobalEnviroment.theWebService = ws;
                        }
                        this.Hide();
                        this.Close();
                    }
                    else
                    {
                        lbInfo.Text = "用户名密码错误！";
                        lbInfo.ForeColor = Color.Red;
                        //MessageBox.Show("用户名密码错误!");
                        this.Cursor = Cursors.Arrow;
                    }
                }
            }
            else
                MessageBox.Show("密码用户名不正确,请重新输入!");
        }

        private bool TestUser(string username, string password)
        {
            // send a HTTP web request to the login.aspx page, using the querystring to pass in username and password
            string postData = String.Format("?Username={0}&Password={1}", username, password);
            string url = Properties.Settings.Default.WebServicesUrl + "TMPAServices/filetransfer.asmx" + postData;

            //string url = "http://localhost:12792/WFMLogin/webform1.aspx" + postData;
            //theWebservice.Url.Replace("FileTransfer.asmx", "") + "Login.aspx" + postData; // get the path of the login page, assuming it is in the same folder as the web service
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.CookieContainer = new CookieContainer();
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

            return (response.Cookies.Count > 1);
        }

        private bool PrepareMain(string logintoken)
        {
            //Setting the Region
            var RegionPath = ws.GetCurrentUserPath();

            var RegionName = RegionPath.Split('\\');
            if (RegionName.Length == 3)
                Properties.Settings.Default.CurentRegionName = RegionName[1] + RegionName[2];
            else if (RegionName.Length == 2)
                Properties.Settings.Default.CurentRegionName = RegionName[0] + RegionName[1];
            else Properties.Settings.Default.CurentRegionName = RegionName[0];

            if (RegionName.Length != 0)
                Properties.Settings.Default.CurrentRegion = RegionName[RegionName.Length - 1];
           
            Properties.Settings.Default.Save();

            //check update
            string versioninfo = ws.IsUpdate(
                Properties.Settings.Default.Version);

            if (versioninfo.Length != 0)
            {
                MessageBox.Show(versioninfo, "版本升级", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            GlobalEnviroment.LoginToken = logintoken;

            //check data transfer
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Login.Logged = false;
            this.Close();
        }
        #region Validating

        private void ValidateTextBox(object sender, CancelEventArgs e)
        {
            bool NameValid = true, PasswordValid = true;

            if (String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                switch (Convert.ToByte(((TextBox)sender).Tag))
                {
                    case 0:
                        errorProvider1.SetError(tbUserName, "请输入用户名");
                        NameValid = false;
                        break;
                    case 1:
                        errorProvider1.SetError(tbPassword, "请输入密码");
                        PasswordValid = false;
                        break;
                }
            }
            else
            {
                switch (Convert.ToByte(((TextBox)sender).Tag))
                {
                    case 0:
                        errorProvider1.SetError(tbUserName, "");
                        break;
                    case 1:
                        errorProvider1.SetError(tbPassword, "");
                        break;
                }
            }
            _ValidForm = NameValid && PasswordValid;
        }

        #endregion


        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == System.Convert.ToChar(13))
            {
                btnLogin_Click(null, null);
                e.Handled = true;
            }
        }

        private void tbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(13))
            {
                tbPassword.Focus();
                e.Handled = true;
            }
        }
    }
}
