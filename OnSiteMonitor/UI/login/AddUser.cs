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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Models.User u = new Models.User();

            u.Name = tbUserName.Text;
            u.Password = tbPassword.Text;
            u.isFisrt = 1;

            Service.LoginServices ls = new Service.LoginServices();
            ls.AddUser(u);

            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
