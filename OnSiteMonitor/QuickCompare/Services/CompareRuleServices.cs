using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnSiteFundComparer.DAL;
using OnSiteFundComparer.QuickCompare.Models;
using System.Data.SQLite;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareRuleServices : CompareServices<CompareRule>
    {
        public CompareRuleServices() : base() { }

        public CompareRuleServices(MySqlite _sqlite) : base(_sqlite) { }

        public List<Models.CompareRule> GetRuleByDataItem(Models.CompareDataItem cdi)
        {
            return null;
        }

        internal CompareRule GetRuleById(int rowId)
        {
            return null;
        }

        internal void IncludeParent(List<Models.CompareRule> ruleList, List<CompareDataItem> cdiList)
        {
            if (cdiList == null || ruleList == null)
                return;

            ruleList.ForEach(r => r.ParentItem = cdiList.Find(x => x.RowID == r.ParentItem.RowID));
        }

      
        protected override CompareRule Mapor(SQLiteDataReader reader)
        {
            Models.CompareRule cRule = new Models.CompareRule();
            cRule.ParentItem = new CompareDataItem();
            cRule.RuleTmp = new CompareRuleTmp();

            cRule.RowID = Convert.ToInt32(reader.GetValue(0).ToString());
            cRule.ParentItem.RowID = Convert.ToInt32(reader.GetValue(1).ToString());
            cRule.RuleName = reader.GetValue(2).ToString();
            cRule.RuleDesc = reader.GetValue(3).ToString();
            cRule.TableName = reader.GetValue(4).ToString();
            cRule.RuleTmp.RowID = Convert.ToInt32(reader.GetValue(5).ToString());
            cRule.Status = Convert.ToInt32(reader.GetValue(6));
            cRule.Seq = Convert.ToInt32(reader.GetValue(7));


            return cRule;
        }

        protected override string SelectSql()
        {
            var Sql = Models.CompareRule.SelectSql();
            Sql = Sql.Replace("@para", " and Status = 1");

            return Sql;
        }
    }
}
