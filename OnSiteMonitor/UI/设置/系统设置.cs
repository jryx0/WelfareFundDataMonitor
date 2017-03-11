using OnSiteFundComparer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 系统设置 : Form
    {
        public 系统设置()
        {
            InitializeComponent();
            //Region_init();

            Dir_init();


            tbUploadUrl.Text = Properties.Settings.Default.WebServicesUrl;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (GlobalEnviroment.LoginedUser.Name == "admin")
            {
                this.label8.Visible = true;
                this.tbInputFileDir.Visible = true;
                this.button6.Visible = true;

                this.button1.Visible = true;
            }
        }

        //#region 用户

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    Models.User u = new Models.User();

        //    u.Name = tbUserName.Text;
        //    u.Password = tbPassword.Text;
        //    u.isFisrt = 1;

        //    Service.LoginServices ls = new Service.LoginServices();
        //    ls.AddUser(u);

        //    this.DialogResult = DialogResult.OK;
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = DialogResult.Cancel;
        //}
        //#endregion

        //#region 地区
        //private void Region_init()
        //{
        //    MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile);
        //    try
        //    {
        //        string region = Properties.Settings.Default.CurrentRegion;
        //        int cityID = 0, regionID = 0;
        //        if (region == null || region.Length == 0)
        //        {

        //        }
        //        else
        //        {
        //            var ds = configDB.ExecuteDataset(
        //            @"SELECT a.rowid,a.regionname,b.rowid,
        //                   b.regionname
        //              FROM hb_region a,
        //                   hb_region b
        //             WHERE a.rowid = b.parentid AND 
        //                   b.rowid = " + region);

        //            if (ds != null && ds.Tables[0] != null)
        //            {
        //                int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out cityID);
        //                int.TryParse(ds.Tables[0].Rows[0][2].ToString(), out regionID);

        //            }
        //        }



        //        var dsCity = configDB.ExecuteDataset(
        //            @"SELECT rowid, regioncode, regionname 
        //              FROM hb_region
        //             WHERE level = 1 and status = 1
        //             ORDER BY seq");

        //        if (dsCity != null && dsCity.Tables[0] != null)
        //        {
        //            comboCity.DataSource = dsCity.Tables[0];
        //            comboCity.DisplayMember = "regionname";
        //            comboCity.ValueMember = "rowid";

        //            comboCity.SelectedValue = cityID;
        //        }


        //        var dsContry = configDB.ExecuteDataset(
        //            @"SELECT rowid, regioncode, regionname 
        //              FROM hb_region
        //             WHERE level = 2 and status = 1 and parentid = " + comboCity.SelectedValue.ToString() + "  ORDER BY seq");

        //        if (dsContry != null && dsContry.Tables[0] != null)
        //        {
        //            comboContry.DataSource = dsContry.Tables[0];
        //            comboContry.DisplayMember = "regionname";
        //            comboContry.ValueMember = "rowid";

        //            comboContry.SelectedValue = regionID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //}
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    Properties.Settings.Default.CurrentRegion = comboContry.SelectedValue.ToString();
        //    Properties.Settings.Default.CurentRegionName = comboCity.Text + comboContry.Text;

        //    Properties.Settings.Default.Save();
        //    this.DialogResult = DialogResult.OK;
        //}

        //private void comboCity_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile);
        //    var dsContry = configDB.ExecuteDataset(
        //           @"SELECT rowid, regioncode, regionname 
        //              FROM hb_region
        //             WHERE level = 2 and status = 1 and parentid = " + comboCity.SelectedValue.ToString() + "  ORDER BY seq");

        //    if (dsContry != null && dsContry.Tables[0] != null)
        //    {
        //        comboContry.DataSource = dsContry.Tables[0];
        //        comboContry.DisplayMember = "regionname";
        //        comboContry.ValueMember = "rowid";

        //        comboContry.SelectedIndex = 0;
        //    }
        //}
        //private void button5_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = DialogResult.Cancel;
        //}


        //#endregion

        #region 目录设置
        private void Dir_init()
        {
            if (!Directory.Exists(Properties.Settings.Default.ResultDir))
            {
                Properties.Settings.Default.ResultDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }

            this.tbResultDir.Text = Properties.Settings.Default.ResultDir;

            if (!Directory.Exists(Properties.Settings.Default.WorkDir))
            {
                Properties.Settings.Default.WorkDir = Application.StartupPath;
                Properties.Settings.Default.Save();
            }

            this.tbInputFileDir.Text = Properties.Settings.Default.WorkDir;
        }

        private void button7_Click(object sender, EventArgs e)
        {//确定
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void button6_Click(object sender, EventArgs e)
        {//file 浏览
            new UI.输入目录设置().ShowDialog();

            this.tbInputFileDir.Text = Properties.Settings.Default.WorkDir;
        }

        private void button8_Click(object sender, EventArgs e)
        {//结果 浏览
            new UI.输出目录设置().ShowDialog();

            this.tbResultDir.Text = Properties.Settings.Default.ResultDir;
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
          //  Business.SaveToExcelXml reportFile = new Business.SaveToExcelXml();

            //MySqlite config = new MySqlite();
            //config.sqliteConnectionString = GlobalEnviroment.MainDBFile;

            //var ds = config.ExecuteDataset("Select * from dataItem");

            //if(ds != null && ds.Tables[0].Rows.Count != 0)
            //{
            //    reportFile.AddSummarySheet("test", ds.Tables[0]);
            //    reportFile.SaveExcelXml("D:\\temp\\test.xls");
            //}           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DAL.MySqlite config = new MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            config.ExecuteNonQuery("Delete from Task");
            config.ExecuteNonQuery("Update setting set Settingvalue = 1");
        }
    }
}
