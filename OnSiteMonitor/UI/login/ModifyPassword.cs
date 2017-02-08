using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer
{
    public partial class ModifyPassword : Form
    {
        public string UserName;
        public string PassWord;
        public int RowID;

        public ModifyPassword()
        {
            InitializeComponent();

        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbNewPassword1.Text == tbNewPassword2.Text)
            {
                Service.LoginServices ls = new Service.LoginServices();

                var user = ls.IsAuthorition(tbName.Text, tbPassword.Text);

                if(user != null)
                {
                    user.Password = tbNewPassword1.Text;
                    user.isFisrt = 0;
                    if (ls.UpdateUserByName(user))
                    {
                        PassWord = user.Password;
                        this.DialogResult = DialogResult.OK;
                    }
                }   
                else
                {
                    MessageBox.Show("请输入正确的用户名和旧密码!");
                }             
            }
            else
                MessageBox.Show("密码输入错误,请重输入密码!");
        }

        private void ModifyPassword_Load(object sender, EventArgs e)
        {

            this.tbName.Text = UserName;
        }
    }
}
