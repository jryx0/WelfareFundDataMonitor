using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnSiteFundComparer.DAL;
using System.IO;
 

namespace Test
{
    public partial class TestForm1 : Form
    {
        private System.Collections.Generic.Dictionary<string, string> tt = new Dictionary<string, string>();
        public string ImportDB { get; private set; }
        public string configDB { get; private set; }
        public string resultDB { get; private set; }

        public TestForm1()
        {
            InitializeComponent();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbConfigDB.Text = ofd.FileName;
            }

        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbImportDB.Text = ofd.FileName;
            }
        }
        private void btnResult_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbResultDB.Text = ofd.FileName;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.button1.Enabled = false;


            lbInfo.Text = "开始校验数据";
            lbInfo.Refresh();

            int nError = 0;

            MySqlite configDB = new MySqlite(tbConfigDB.Text);
            MySqlite importDB = new MySqlite(tbImportDB.Text);
            MySqlite resultDB = new MySqlite(tbResultDB.Text);

            System.Collections.Generic.Dictionary<string, string> testSql = new Dictionary<string, string>();

            string outpurDir = OnSiteFundComparer.GlobalEnviroment.ResultOutputDir + "test\\" + System.DateTime.Now.ToBinary().ToString() + "\\";
            OnSiteFundComparer.GlobalEnviroment.MakeSureDirectory(outpurDir);

            try
            {
                configDB.ExecuteNonQuery("attach database '" + tbImportDB.Text + "' as import");
                configDB.ExecuteNonQuery("attach database '" + tbResultDB.Text + "' as result");


                lbInfo.Text = "获取SQL...";
                lbInfo.Refresh();

                var ds = configDB.ExecuteDataset("select tablename from CollisionAim");
                if (ds != null && ds.Tables[0] != null)
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string rulesname = r[0].ToString();
                        string tables = AnlyzeTable(rulesname);

                        if (tables == null)
                            continue;

                        var ts = tables.Split(',');
                        for (int i = 0; i < ts.Length - 1; i++)
                        {
                            string sql = "select * from result." + rulesname +
                                " where 身份证号 not in (select distinct id from import." + ts[i] + " )";

                            if (rulesname.IndexOf("家属") != -1)
                                sql = "select * from result." + rulesname +
                                " where 身份证号 not in (select distinct sRelateID from import." + ts[i] + " )";




                            testSql.Add(rulesname + "-" + ts[i], sql);
                        }

                        if (rulesname.IndexOf("未查到") != -1)
                        {
                            string sql = "select * from  result." + rulesname +
                                " where 身份证号 in (select distinct id from import.tbComparePersonInfo)";

                            testSql.Add(rulesname + "-tbComparePersonInfo", sql);
                        }

                        if (rulesname.IndexOf("家属") == -1 && rulesname.IndexOf("村干部") != -1)
                        {
                            string sql = "select * from  result." + rulesname +
                                " where 身份证号 not in (select distinct id from import.tbComparecountryInfo)";

                            testSql.Add(rulesname + "-tbComparecountryInfo", sql);
                        }

                        if (rulesname.IndexOf("家属") == -1 && rulesname.IndexOf("领导") != -1)
                        {
                            string sql = "select * from  result." + rulesname +
                                " where 身份证号 not in (select distinct id from import.tbCompareLeaderInfo)";

                            testSql.Add(rulesname + "-tbCompareLeaderInfo", sql);
                        }


                    }

                lbInfo.Text = "获取SQL完成，共" + testSql.Count.ToString() + "项。";
                lbInfo.Refresh();
                int nItem = 0;

                foreach (var s in testSql)
                {
                    nItem++;

                    lbInfo.Text = "开始进行第" + nItem.ToString() + "项/共" + testSql.Count.ToString() + "项。 错误" + nError.ToString() + "个";
                    lbInfo.Refresh();
                    string ResultFile = outpurDir + s.Key + ".xls";

                    var t = s.Key.Split('-');

                    var o = importDB.ExecuteScalar(@"select count(*) from sqlite_master where type = 'table' and name = '" + t[1] + "'");
                    if (o != null)
                    {
                        int i = 0;
                        int.TryParse(o.ToString(), out i);
                        if (i == 0)
                            continue;
                    }

                    o = resultDB.ExecuteScalar(@"select count(*) from sqlite_master where type = 'table' and name = '" + t[0] + "'");
                    if (o != null)
                    {
                        int i = 0;
                        int.TryParse(o.ToString(), out i);
                        if (i == 0)
                            continue;
                    }

                    var rDS = configDB.ExecuteDataset(s.Value);

                    if (rDS != null && rDS.Tables[0] != null)
                    {
                        if (rDS.Tables[0].Rows.Count != 0)
                            using (MemoryStream ms = OnSiteFundComparer.GlobalEnviroment.DataTableToExcel(rDS.Tables[0], s.Key))
                            {
                                nError++;
                                using (FileStream fs = new FileStream(ResultFile, FileMode.Create, FileAccess.Write))
                                {
                                    byte[] data = ms.ToArray();
                                    fs.Write(data, 0, data.Length);
                                    fs.Flush();

                                    //log.Info("成功写入文件: " + FileName + ".xls");
                                }
                            }
                    }
                }

                this.button1.Enabled = true;

            }
            catch (Exception ex)
            {


            }

            this.Cursor = Cursors.Arrow;

            lbInfo.Text = "完成" + testSql.Count.ToString() + "项。共错误" + nError.ToString() + "个";
            lbInfo.Refresh();
        }
        private string AnlyzeTable(string tablename)
        {
            string strRet = null;
            string[] keywords = new string[]
            {
                "五保", "tbSourceFiveGuaranteFamily",
                "医疗", "tbSourceMedicalFinancial",
                "廉租", "tbSourceSafeHouse",
                "危房", "tbSourceRuralBadHouse",

                "粮食", "tbSourceFoodAid",
                "农资", "tbSourceAgriMaterials",
                "财政", "tbCompareCivilInfo",
                
                //"领导",  "tbCompareLeaderInfo",
                "领导干部家属",  "tbCompareLeaderRelateInfo",
               // "村",   "tbComparecountryInfo",
                "村干部家属",   "tbComparecountryRelateInfo",
                "工作",  "tbCompareIncomeTaxInfo",
                "公司",  "tbCompareCompanyInfo",
                //"不存在", "tbComparePersonInfo",
                "不一致", "tbComparePersonInfo",
                "直系",  "tbCompareFamilyInfo",
                "车", "tbCompareCarInfo",
                "死亡",  "tbCompareDeathInfo",
                "死亡","tbCompareBurnInfo",
                "农机",  "tbComparemachineInfo",
                "商品房","tbCompareHouseInfo",
                "农村",     "tbSourceSafeLowLifeContry",
                "城市",  "tbSourceSafeLowLifeCity",
                "退耕",  "tbSourceForestAid"
            };

            for (int i = 0; i < keywords.Length; i = i + 2)
            {
                if (tablename.IndexOf(keywords[i]) != -1)
                {
                    strRet += keywords[i + 1] + ",";
                }
            }

            return strRet;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            configDB = tbConfigDB.Text;
            resultDB = tbResultDB.Text;
            ImportDB = tbImportDB.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            configDB = tbConfigDB.Text;
            resultDB = tbResultDB.Text;
            ImportDB = tbImportDB.Text;


            Sql工具窗口 sql = new Sql工具窗口();

            sql.SetDB(configDB, "配置数据库");
            sql.SetDB(resultDB, "结果数据库");
            sql.SetDB(ImportDB, "导入数据库");

            sql.Show();
        } 
    }
}
