using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI.login
{    

    public class StartUp : ApplicationContext
    {        
        private MainForm fMain;
        FileTranser.MTOM.ClassLibrary.WebServicesHelp ws;

        public StartUp(String Token, String UserName,  String Password)
        {
            CreateLoginForm(Token, UserName, Password);
        }

        /// <summary>
        /// The Login form
        /// initialization and show
        /// </summary>
        private void CreateLoginForm(String Token, String UserName, String Password)
        {

            ws = new FileTranser.MTOM.ClassLibrary.WebServicesHelp();
            ws.Url = Properties.Settings.Default.WebServicesUrl + "WFMUploader/FileTransfer.asmx";
            ws.LoginToken = Token;

            string logintoken = ws.Login(UserName.ToLower(), Password);
            if (logintoken.Length != 0)
            {
                if (PrepareMain(logintoken))
                {
                    Login.LoginedUser = new Models.User { Name = UserName.ToLower(), Password = Password };
                    Login.Logged = true;

                    GlobalEnviroment.theWebService = ws;
                    fLogin_Closed(null, null);
                }
                
            }
            else
            {
                MessageBox.Show("MainForm Error!");
                ExitThread();            
            }
        }


        /// <summary>
        /// If the login procedure done successfully
        /// we'll see the Main Form
        /// else the application will close 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fLogin_Closed(object sender, EventArgs e)
        {

            if (Login.Logged) //if the user is logged
            {
                
                //if (!PreareMain())
                //    return;
                 
                fMain = new MainForm();
                fMain.Cursor = Cursors.WaitCursor;
                fMain.currentUser = Login.LoginedUser;
                GlobalEnviroment.LoginedUser = Login.LoginedUser;

                fMain.theWebService = ws;

                this.MainForm = fMain; //set the main message loop applicaton in this form
                fMain.Show();
            }
            else
            {
                ExitThread();
            }
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

            // Properties.Settings.Default.CurrentRegion = comboContry.SelectedValue.ToString();
            // Properties.Settings.Default.CurentRegionName = comboCity.Text + comboContry.Text;
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
    }
}
