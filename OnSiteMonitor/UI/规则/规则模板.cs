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
        }

        private void init()
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);

            try
            {
                String sql = @"SELECT rowid,
                            tmpName AS 模板名称,
                            ruletype AS 形式,
                            CASE tmptype WHEN 0 THEN '比对规则' WHEN 1 THEN '校验规则' WHEN 2 THEN '预处理规则' END AS 模板类型,
                            seq as 顺序
                        FROM RulesTmp
                        WHERE status = 1 @para
                        ORDER BY seq";


                if (comboBox1.SelectedIndex < 3 && comboBox1.SelectedIndex > -1)
                    sql = sql.Replace("@para", " and tmptype = " + comboBox1.SelectedIndex);
                else sql = sql.Replace("@para", " ");


                var ds = configDB.ExecuteDataset(sql);
                dgvTmp.DataSource = ds.Tables[0];

                if(dgvTmp.Columns.Count > 0)
                { 
                    dgvTmp.Columns[0].Visible = false;

                    dgvTmp.Columns[1].Width = 180;
                    dgvTmp.Columns[1].ReadOnly = true;
                    dgvTmp.Columns[2].Width = 55;
                    dgvTmp.Columns[3].ReadOnly = true;
                    dgvTmp.Columns[3].Width = 80;
                    dgvTmp.Columns[4].Width = 55;
                }

                if (ds != null && ds.Tables[0].Rows.Count != 0)
                    showTmp(ds.Tables[0].Rows[0][0].ToString());
                else showTmp(null);
            }
            catch(Exception ex)
            {
                
            }
        }  

        private void 规则模板_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {           
            规则模板添加 dlg = new 规则模板添加();
            dlg.tmpType = comboBox1.SelectedIndex;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NewTmp(dlg);
                init();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql = "update  RulesTmp set rules =@r1, rule2 = @r2, rule3 = @r3, Comments = @r4 where rowid = @id";

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
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
                currentid = dgvTmp.CurrentRow.Cells[0].Value.ToString();
                showTmp(currentid);
                 
            }
        }

        private void showTmp(string rowID)
        {
            if(String.IsNullOrEmpty(rowID))
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                currentid = "-1";
            }

            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
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
            if (e.RowIndex < 0)
                return;

            var dlg = new 规则模板添加();            
            dlg.rowid = dgvTmp.Rows[e.RowIndex].Cells[0].Value.ToString();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NewTmp(dlg);
                init();
            }
        }   
        
        private void NewTmp(规则模板添加 dlg)
        {
            DAL.MySqlite configDB = new DAL.MySqlite(OnSiteFundComparer.GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            try
            {
                if (dlg.rowid.Length == 0)
                {
                    string sql = @"insert into rulestmp(Tmpname, Ruletype, TmpType,Seq) values (@Tmpname, @Ruletype, @TmpType, @Seq)";
                    configDB.ExecuteNonQuery(CommandType.Text, sql,
                                            new System.Data.SQLite.SQLiteParameter[] {
                                            new System.Data.SQLite.SQLiteParameter("@Tmpname", dlg.tmpName),
                                            new System.Data.SQLite.SQLiteParameter("@Ruletype", int.Parse(dlg.type)),
                                            new System.Data.SQLite.SQLiteParameter("@TmpType", dlg.tmpType),
                                            new System.Data.SQLite.SQLiteParameter("@Seq", int.Parse(dlg.seq))});
                }
                else
                {
                     string sql = @"update rulestmp set  Tmpname = @Tmpname,  
                                                         Ruletype = @Ruletype, 
                                                         TmpType = @TmpType,
                                                         Seq = @Seq
                                                                  where rowid = @rowid";
                    configDB.ExecuteNonQuery(CommandType.Text, sql,
                                            new System.Data.SQLite.SQLiteParameter[] {
                                            new System.Data.SQLite.SQLiteParameter("@Tmpname", dlg.tmpName),
                                            new System.Data.SQLite.SQLiteParameter("@Ruletype", int.Parse(dlg.type)),
                                            new System.Data.SQLite.SQLiteParameter("@TmpType", dlg.tmpType),
                                            new System.Data.SQLite.SQLiteParameter("@Seq", int.Parse(dlg.seq)),
                                            new System.Data.SQLite.SQLiteParameter("@rowid", int.Parse(dlg.rowid)
                                            )});
                }
                
            }
            catch (Exception ex)
            {

            }
        }  

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            init();
        }
    }
}
