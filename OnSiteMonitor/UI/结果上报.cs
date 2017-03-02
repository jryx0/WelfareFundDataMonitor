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
    public partial class 结果上报 : Form
    {
        DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
      

        public 结果上报(DAL.MySqlite result)
        {
            configDB.AttchDatabase(result, "result");
            InitializeComponent();
        }


        public void initTree()
        {
            string sql = @"SELECT distinct dataitem.DataShortName
                              FROM result.report,
                                   dataitem
                             WHERE result.report.table1 = dataitem.RowID
                             and dataitem.status = 1";







        }

    }
}
