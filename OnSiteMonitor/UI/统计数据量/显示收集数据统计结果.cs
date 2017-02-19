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
    public partial class 显示收集数据统计结果 : Form
    {
        string dbString = "";
        public 显示收集数据统计结果(string DBFileInfo)
        {
            dbString = DBFileInfo;
            InitializeComponent();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (!File.Exists(dbString))
            {
                MessageBox.Show("导入数据库未找到，请重新导入数据!");
                return;
            }

            String CountSql = @"
                Select '@DataName' as 项目名称, Count(*) 数据量 from @tablename
                ";

            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(
                GlobalEnviroment.MainDBFile);
            var diList = diss.GetDisplayDataItems();           

            String _countSql = "";
            foreach (var di in diList.Where(x => (x.ParentID == 2 || x.ParentID == 3) && x.Status && x.dbTable.Length != 0).OrderBy(x => x.ParentID).ThenBy(x =>x.Seq))
            { 
                var sql = CountSql.Replace("@DataName", di.DataFullName);
                sql = sql.Replace("@tablename", di.dbTable);
                _countSql += " union " + sql;
            }
            diss.Close();

            _countSql =  _countSql.Substring(7);
            DAL.MySqlite db = new DAL.MySqlite(dbString);

            try
            {
                var ds = db.ExecuteDataset(_countSql);

                if(ds!= null)
                    this.dataGridView1.DataSource = ds.Tables[0];
            }
            catch
            {

            }
        }
    }
}
