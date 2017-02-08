using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WFM.Starter
{

    public partial class 
        Login : Form
    {
        // the value of permissions on the error at one login procedure 
        private int _logins;
        // the flag of validate
        private bool _ValidForm;

        public static JW.HB.Models.User LoginedUser;

        
        public FileTranser.MTOM.ClassLibrary.WebServicesHelp ws;
        public static bool Logged
        {
            get; set;
        }

        public Login()
        {
            InitializeComponent();
            Login.Logged = false;
            tbUserName.Validating += new CancelEventHandler(ValidateTextBox);
            tbPassword.Validating += new CancelEventHandler(ValidateTextBox);

            
            ws = new FileTranser.MTOM.ClassLibrary.WebServicesHelp();            
            ws.Url = WFM.Starter.Properties.Settings.Default.WebServicesUrl + "WFMRecovery/FileTransfer.asmx";

            lbInfo.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_ValidForm)
            {
                this.Cursor = Cursors.WaitCursor;

                string logintoken = ws.Login( tbUserName.Text.ToLower(), tbPassword.Text);
                this.Cursor = Cursors.Arrow;
                if (logintoken.Length != 0)
                {
                    

                    
                   // Login.LoginedUser = new JW.HB.Models.User { Name = tbUserName.Text.ToLower(), Password = tbPassword.Text };
                    Login.Logged = true;
                    this.Hide();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名密码错误!");
                    
                }
            }
            else
                MessageBox.Show("密码用户名不正确,请重新输入!");
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
    }
}
