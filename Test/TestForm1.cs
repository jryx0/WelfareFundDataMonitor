using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Test.DAL;

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
        } 
    }
}
