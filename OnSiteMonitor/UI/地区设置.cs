using OnSiteFundComparer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.UI
{
    public partial class 地区设置 : Form
    {
        
        public 地区设置()
        {
            InitializeComponent();

            init();
        }


        private void init()
        {
            MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                

                var dsCity = configDB.ExecuteDataset(
                    @"SELECT rowid, regioncode, regionname 
                      FROM hb_region
                     WHERE level = 1 and status = 1
                     ORDER BY seq");

                if(dsCity != null && dsCity.Tables[0] != null)
                {
                    comboCity.DataSource = dsCity.Tables[0];
                    comboCity.DisplayMember = "regionname";
                    comboCity.ValueMember = "rowid";

                    comboCity.SelectedIndex = 0;
                }


                var dsContry = configDB.ExecuteDataset(
                    @"SELECT rowid, regioncode, regionname 
                      FROM hb_region
                     WHERE level = 2 and status = 1 and parentid = "+ comboCity.SelectedValue.ToString() + "  ORDER BY seq");

                if (dsContry != null && dsContry.Tables[0] != null)
                {
                    comboContry.DataSource = dsContry.Tables[0];
                    comboContry.DisplayMember = "regionname";
                    comboContry.ValueMember = "rowid";

                    comboContry.SelectedIndex = 0;
                }


            }
            catch(Exception ex)
            {

            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CurrentRegion = comboContry.SelectedValue.ToString();
            Properties.Settings.Default.CurentRegionName = comboCity.Text + comboContry.Text;
            Properties.Settings.Default.Save();


            this.DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCity.SelectedValue == null)
                return;           
        }

        private void comboCity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            var dsContry = configDB.ExecuteDataset(
                   @"SELECT rowid, regioncode, regionname 
                      FROM hb_region
                     WHERE level = 2 and status = 1 and parentid = " + comboCity.SelectedValue.ToString() + "  ORDER BY seq");

            if (dsContry != null && dsContry.Tables[0] != null)
            {
                comboContry.DataSource = dsContry.Tables[0];
                comboContry.DisplayMember = "regionname";
                comboContry.ValueMember = "rowid";

                comboContry.SelectedIndex = 0;
            }
        }
    }
}
