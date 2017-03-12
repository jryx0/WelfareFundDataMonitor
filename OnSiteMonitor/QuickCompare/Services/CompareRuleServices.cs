using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnSiteFundComparer.DAL;
using OnSiteFundComparer.QuickCompare.Models;
using System.Data.SQLite;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareRuleServices : CompareServices
    {
        public CompareRuleServices(MySqlite _sqlite) : base(_sqlite)
        {
        }

        public List<Models.CompareRule> GetRuleByDataItem(Models.CompareDataItem cdi)
        {
            return null;
        }

        internal CompareRule GetRuleById(int rowId)
        {
            throw new NotImplementedException();
        }

        internal List<Models.CompareRule> GetAllRules(List<CompareDataItem> cdiList)
        {
            if (cdiList == null)
                return null;

            var ruleList = GetAllRules();

            ruleList.ForEach(r =>
            {
                var cdi = cdiList.Find(x => x.RowID == r.ParentItem.RowID);
                if (cdi != null)
                    r.ParentItem = cdi;
            });


            

            return ruleList;

        }

        internal List<CompareRule> GetAllRules()
        {
            List<Models.CompareRule> cdiList = new List<Models.CompareRule>();

            SQLiteDataReader reader = MainSqliteDB.ExecuteReader(Models.CompareRule.SelectSql());
            if (reader.HasRows)
                while (reader.Read())
                {
                    cdiList.Add(Mapor(reader));
                }

            return cdiList;
        }
        private CompareRule Mapor(SQLiteDataReader reader)
        {
            Models.CompareRule cRule = new Models.CompareRule();
            cRule.ParentItem = new CompareDataItem();
            cRule.RuleTmp = new CompareRuleTmp();

            cRule.RowID = Convert.ToInt32(reader.GetValue(0));
            cRule.ParentItem.RowID = Convert.ToInt32(reader.GetValue(1));
            cRule.RuleName = reader.GetValue(2).ToString();
            cRule.RuleDesc = reader.GetValue(3).ToString();
            cRule.TableName = reader.GetValue(4).ToString();
            cRule.RuleTmp.RowID = Convert.ToInt32(reader.GetValue(5));
            cRule.Status = (bool)reader.GetValue(6);
            cRule.Seq = Convert.ToInt32(reader.GetValue(7));
            

            return cRule;
        }

    
    }
}
