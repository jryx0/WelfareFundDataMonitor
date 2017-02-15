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
    public partial class 规则模板 : Form
    {
        string currentid = "1";

        public 规则模板()
        {
            InitializeComponent();


            init();
        }

        private void init()
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);

            try
            {
                var ds = configDB.ExecuteDataset(@"SELECT rowid,
                                               tmpName as 模板名称,type as 类型
                                          FROM RulesTmp
                                         WHERE status = 1
                                         ORDER BY seq");
                dgvTmp.DataSource = ds.Tables[0];

                if(dgvTmp.Columns.Count > 0)
                { 
                    dgvTmp.Columns[0].Visible = false;
                    dgvTmp.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    dgvTmp.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                }

                showTmp("1");

            }
            catch(Exception ex)
            {
                
            }
        }

        private void initGridView()
        {
            Service.DataItemStuctServices dis = new Service.DataItemStuctServices(OnSiteFundComparer.GlobalEnviroment.MainDBFile);

            var list = dis.GetDisplayDataItems();
            dgvTmp.DataSource = list.FindAll(x => x.DataType == Models.FundItemTypes.SourceData);

            dgvTmp.Columns[0].Visible = false;
            dgvTmp.Columns[1].Visible = false;


            dgvTmp.Columns[2].Visible = false;
            dgvTmp.Columns[3].Visible = false;
            //dgvTmp.Columns[4].Visible = false;
            dgvTmp.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTmp.Columns[5].Visible = false;
            dgvTmp.Columns[6].Visible = false;
            dgvTmp.Columns[7].Visible = false;
            dgvTmp.Columns[8].Visible = false;
            dgvTmp.Columns[9].Visible = false;
            dgvTmp.Columns[10].Visible = false;


        }

        private void 规则模板_Load(object sender, EventArgs e)
        {
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {           
            规则模板添加 dlg = new 规则模板添加();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            try
            {
                var ds = configDB.ExecuteNonQuery(@"insert into rulestmp(tmpname, type) values ('" + dlg.tmpName + "', " + dlg.type + ")");
                init();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql = "update  RulesTmp set rules =@r1, rule2 = @r2, rule3 = @r3, PreRules = @r4 where rowid = @id";

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            try
            {
                configDB.ExecuteNonQuery(CommandType.Text, sql,
                    new System.Data.SQLite.SQLiteParameter[] {
                    new System.Data.SQLite.SQLiteParameter("@r1", textBox1.Text),
                    new System.Data.SQLite.SQLiteParameter("@r2", textBox2.Text),
                    new System.Data.SQLite.SQLiteParameter("@r3", textBox3.Text),
                    new System.Data.SQLite.SQLiteParameter("@r4", textBox4.Text),
                    new System.Data.SQLite.SQLiteParameter("@id", int.Parse(currentid))
                    });

                //sql = sql.Replace("@r1", textBox1.Text);
                //sql = sql.Replace("@r2", textBox2.Text);
                //sql = sql.Replace("@id", currentid);
                //var ds = configDB.ExecuteNonQuery(sql);
                // init();
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

            }



           // this.DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dgvTmp_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvTmp.CurrentRow != null)
            {
                showTmp(dgvTmp.CurrentRow.Cells[0].Value.ToString());
            }
        }

        private void showTmp(string rowID)
        {

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile);
            try
            {
                var ds = configDB.ExecuteDataset(@"select rules, rule2, rule3
                                                    from RulesTmp
                                                     WHERE status = 1
                                                     and rowid = '" + rowID + "'");

                if(ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    textBox1.Text = ds.Tables[0].Rows[0][0].ToString();
                    textBox2.Text = ds.Tables[0].Rows[0][1].ToString();
                    textBox3.Text = ds.Tables[0].Rows[0][2].ToString();
                    currentid = rowID;
                }
            }

                
            catch (Exception ex)
            {

            }
        }

        private void dgvTmp_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            //if (dgvTmp.CurrentRow != null)
            //{
            //    showTmp(dgvTmp.CurrentRow.Cells[0].Value.ToString());
            //}


             
        }

        private void dgvTmp_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             
            
        }
    }
}
