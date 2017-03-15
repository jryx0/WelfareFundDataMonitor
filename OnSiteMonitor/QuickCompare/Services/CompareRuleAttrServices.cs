using System;
using System.Data.SQLite;
using OnSiteFundComparer.QuickCompare.Models;
using System.Collections.Generic;

namespace OnSiteFundComparer.QuickCompare.Services
{
    internal class CompareRuleAttrServices : CompareServices<Models.CompareRuleAtrr>
    {
        public CompareRuleAttrServices(DAL.MySqlite _sqlite) : base(_sqlite)
        {
        }
        
        internal void IncludeParent(List<Models.CompareRuleAtrr> attrList, List<Models.CompareRule> ruleList)
        {
            if (attrList == null || ruleList == null)
                return;

            attrList.ForEach(r =>
            {
                var rule = ruleList.Find(x => x.RowID == r.Rule.RowID);
                if (rule != null)
                    r.Rule = rule;
            });
        }
         


        protected override CompareRuleAtrr Mapor(SQLiteDataReader reader)
        {
            CompareRuleAtrr rAttr = new CompareRuleAtrr();
            rAttr.Rule = new CompareRule();

            rAttr.RowID =   Convert.ToInt32(reader.GetValue(0).ToString());
            rAttr.Rule.RowID = Convert.ToInt32(reader.GetValue(1).ToString());
            rAttr.AttrType = (RuleAttrType)(reader.GetValue(2));
            rAttr.AttrName = reader.GetValue(3).ToString();
            rAttr.AttrValue = Convert.ToInt32(reader.GetValue(4).ToString());
            rAttr.AttrDesc = reader.GetValue(5).ToString();
            rAttr.AttrDetail = reader.GetValue(6).ToString();
            rAttr.Seq = Convert.ToInt32(reader.GetValue(7));
            rAttr.Status = Convert.ToInt32(reader.GetValue(8));

            return rAttr;
        }

        protected override string SelectSql()
        {
            var Sql = Models.CompareRuleAtrr.SelectSql();

            Sql = Sql.Replace("@para", " and Status = 1");

            return Sql;
        }
    }
}