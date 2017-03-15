using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnSiteFundComparer.QuickCompare.Models;
using System.Windows.Forms;
using System.Data.SQLite;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareRuleProxy : CompareServices<Models.CompareRule>
    {
        Services.CompareDataItemServices compareDIService;
        Services.CompareRuleServices compareRuleService;
        Services.CompareRuleAttrServices compareRuleAttrServices;
        

        public CompareRuleProxy() : base()
        {
            compareDIService = new CompareDataItemServices(this.MainSqliteDB);
            compareRuleService = new CompareRuleServices(this.MainSqliteDB);
            compareRuleAttrServices = new CompareRuleAttrServices(this.MainSqliteDB);
        }

        public override List<CompareRule> GetAll()
        {
            List<CompareRule> ruleList = null;
            try
            {
                ruleList = compareRuleService.GetAll();
                var cdiList = compareDIService.GetDisplayDataItems();
                compareRuleService.IncludeParent(ruleList, cdiList);

                var attrList = compareRuleAttrServices.GetAll();
                compareRuleAttrServices.IncludeParent(attrList, ruleList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ruleList;
        }

        

        internal Models.CompareRule GetRuleById(int rowId)
        {
            return compareRuleService.GetRuleById(rowId);
        }

        protected override string SelectSql()
        {
            throw new NotImplementedException();
        }

        protected override CompareRule Mapor(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
