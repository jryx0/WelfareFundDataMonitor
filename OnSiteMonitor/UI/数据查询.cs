using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnSiteFundComparer.DAL;
using System.Text.RegularExpressions;

namespace OnSiteFundComparer.UI
{
    public partial class 数据查询 : Form
    {
        public 数据查询()
        {
            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {

            init();
            base.OnLoad(e);
        }

        private void init()
        {
            MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile);

            try
            {
              //  结果数据库管理.GetDefaultDB(configDB);

                var ds = configDB.ExecuteDataset(@"SELECT rowid,
                                                               CASE TStatus 
                                                               WHEN 0 THEN taskname || '-' || createdate 
                                                               WHEN 1 THEN '上报库-' || taskname || '-' || createdate END AS name
                                                          FROM task
                                                         ORDER BY TStatus DESC");

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                { 
                    MessageBox.Show("请在网站设置好上报数据！");
                    return;
                }
                cbDB.DataSource = ds.Tables[0];
                cbDB.DisplayMember = "name";
                cbDB.ValueMember = "rowid";

                cbDB.SelectedIndex = 0;

            }
            catch(Exception ex)
            {

            }


        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(tbID.Text.Length < 6)
            {
                MessageBox.Show("请输入15位或18位身份号");
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            MySqlite configDB = new MySqlite(GlobalEnviroment.MainDBFile);
            object ret = null;
            try
            {
                ret = configDB.ExecuteScalar("SELECT dbinfo  FROM task where rowid = " + cbDB.SelectedValue);

                 

                string ImportFile = Properties.Settings.Default.ResultDir + "\\导入中间数据库\\" + ret.ToString() + ".db";
                string ResultFile = Properties.Settings.Default.ResultDir + "\\导入中间数据库\\" + "result." + ret.ToString() + ".db";

                if (System.IO.File.Exists(ImportFile) && System.IO.File.Exists(ResultFile))
                {
                    configDB.ExecuteNonQuery("attach database '" + ImportFile + "' as import");
                    configDB.ExecuteNonQuery("attach database '" + ResultFile + "' as result");
                    this.dataGridView1.DataSource = QueryDate(tbID.Text, configDB);
                    this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    MessageBox.Show("数据库破坏或被删除！");

                    configDB.ExecuteNonQuery("Update task set status = 0 where rowid = " + cbDB.SelectedValue);

                    cbDB.DataSource = null;
                    
                    init();
                }
            }
            catch (Exception ex)
            {
                label3.Text = ex.Message;
                this.Cursor = Cursors.Arrow;
                return;
            }

            this.Cursor = Cursors.Arrow;
        }

        private DataTable QueryDate(string text, MySqlite configDB)
        {            
            var o = configDB.ExecuteScalar(@"
                                SELECT group_concat('select  id as 身份证号, Name as 姓名, ''' || a.DataShortName || ''' as 项目 ,' || CASE a.parentid WHEN 2 THEN '''源数据''' WHEN 3 THEN '''比对数据''' END || '  as 类型 from import.' || b.tbl_name || ' where id like ''%@id%'' ', ' UNION ') AS sql
                                  FROM dataitem a,
                                       import.sqlite_master b
                                 WHERE a.dbtable = b.tbl_name AND
                                       b.type = 'table' AND
                                       (a.parentid = 2 OR
                                        a.parentid = 3) AND
                                       a.status <> 0
                                 ORDER BY a.parentid,
                                          a.seq");

          

            var resultds = configDB.ExecuteDataset(@"
                                        SELECT distinct b.sql,
                                               b.tbl_name
                                          FROM CompareAim a,
                                               result.sqlite_master b,
                                               DataItem c
                                         WHERE a.tablename = b.tbl_name AND 
                                               b.type = 'table' AND 
                                               a.status <> 0 
                                         ORDER BY a.conditions,
                                                  a.seq");

            List<string> query = new List<string>();
            if (resultds != null && resultds.Tables[0] != null)
                foreach (DataRow r in resultds.Tables[0].Rows)
                {
                    var name = r[0].ToString();
                    name = name.Substring(name.IndexOf("类型"));

                    Regex reg = new Regex("[\u4e00-\u9fa5]{2,10}姓名");
                    
                    Match m = reg.Match(name);
                    // 名称
                    query.Add("Select 身份证号, " + m.Value + " as 姓名, '" + r[1].ToString() + "' as 项目, '结果数据' as 类型 from result." + r[1].ToString() + " where 身份证号 like '%" + text + "%'");                         
                }

            string querySql = "";
            foreach(var q in query)
            {
                querySql += q + " UNION ";
            }

            //querySql = querySql.Substring(0, querySql.LastIndexOf("UNION"));

            querySql = querySql + o.ToString().Replace("%@id", text) + " order by 类型 limit 100";
            var ds = configDB.ExecuteDataset(querySql);


            return ds.Tables[0];
        }
    }
}
   
