using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.QuickCompare.UI
{
    public partial class 规则设计 : Form
    {
        bool isInit = false;
        protected Models.CompareRule CurrentRule { set; get; }

        #region 初始化
        public 规则设计(Models.CompareRule cRule)
        {
            CurrentRule = cRule;
            InitializeComponent();
        }

        public 规则设计()
        {
            InitializeComponent();
        }
        #endregion


        protected override void OnLoad(EventArgs e)
        {
            InitUI();
            ShowDetail();
            base.OnLoad(e);
        }

        private void ShowDetail()
        {
            if (CurrentRule != null)
            {

            }
        }

        private void InitUI()
        {
            Services.CompareDataItemServices cdiServices = new Services.CompareDataItemServices();
            Models.CompareDataItem cdi = new Models.CompareDataItem();
            cdi.RowID = 0;
            cdi.ParentID = 2;
            cdi.DataFullName = "请选择...";

            //项目
            var list1 = cdiServices.GetDisplayDataItems();
            list1?.Insert(0, cdi);
            cbTable1.DataSource = list1.Where(x => x.ParentID == 2 || x.ParentID == 3)
                .OrderBy(x => x.ParentItem?.Seq)
                .ThenBy(x => x.Seq)
                .ToList();
            cbTable1.DisplayMember = "DataFullName";
            cbTable1.ValueMember = "RowID";

            var list2 = cdiServices.GetAll();
            list2?.Insert(0, cdi);
            cbTable2.DataSource = list2.Where(x => x.ParentID == 2 || x.ParentID == 3)
                  .OrderBy(x => x.ParentItem?.Seq)
                .ThenBy(x => x.Seq)
                .ToList();
            cbTable2.DisplayMember = "DataFullName";
            cbTable2.ValueMember = "RowID";

            var list3 = cdiServices.GetAll();
            list3?.Insert(0, cdi);
            cbTable3.DataSource = list3.Where(x => x.ParentID == 2 || x.ParentID == 3)
                .OrderBy(x => x.ParentItem?.Seq)
                .ThenBy(x => x.Seq)
                .ToList();
            cbTable3.DisplayMember = "DataFullName";
            cbTable3.ValueMember = "RowID";

            //模板
            Services.CompareTmpServices cTmpServices = new Services.CompareTmpServices();
            var listTmp = cTmpServices.GetAll();

            Models.CompareRuleTmp cTmp = new Models.CompareRuleTmp();
            cTmp.RowID = 0;
            cTmp.TmpName = "请选择...";
            listTmp?.Insert(0, cTmp);

            cbTmp.DataSource = listTmp?.Where(x => x.Status ==1).OrderBy(x => x.Seq).ToList() ;
            cbTmp.DisplayMember = "TmpName";
            cbTmp.ValueMember = "RowID";

            cmbType.SelectedIndex = 0;

            isInit = true;
        }


        private Models.CompareRule GetRule()
        {
            if (CurrentRule == null)
                CurrentRule = new Models.CompareRule();

            CurrentRule.RuleName = tbRuleName.Text;
            CurrentRule.RuleDesc = tbRulesComment.Text;
            CurrentRule.RuleType = (Models.CompareRulesTypes)cmbType.SelectedIndex;
            CurrentRule.Status = 1;

            int Seq = 0;
            int.TryParse(tbSeq.Text, out Seq);
            Seq = GetRuleSeq() | (Seq & 0x000F);
            CurrentRule.Seq = Seq;

            CurrentRule.TableName = "tb" + Seq.ToString("X");
            CurrentRule.RuleType = (Models.CompareRulesTypes)cmbType.SelectedIndex;

            return null;
        }

        private Models.CompareRuleAtrr GetRuleAttr()
        {
            return null;
        }

        #region UI 事件
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion

        private void cbTable1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbTmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private int GetRuleSeq()
        {
            int Seq = 0;
            var cdi1 = (Models.CompareDataItem)cbTable1.SelectedItem;
            if (cdi1 != null && cdi1.ParentItem != null)
                Seq = Seq | (cdi1.ParentItem.Seq & 0x000F) << 24 | (cdi1.ParentItem.Seq) << 16;

            var tmp = (Models.CompareRuleTmp)cbTmp.SelectedItem;
            if (tmp != null)
                Seq = Seq | (tmp.Seq & 0x000F) << 8; 

            return Seq;
        }
    }
}
