using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRule
    {
        public int RowID { set; get; }
        public CompareDataItem ParentItem { set; get; }

        public string RuleName { set; get; }
        public string RuleDesc { set; get; }

        public string TableName { set; get; }

        public int Seq { set; get; }
        public bool Status { set; get; }

        public CompareRuleTmp RuleTmp { set; get; }
        public List<CompareRuleAtrr> Attriutes { set; get; }

        internal static string SelectSql()
        {
            return @"SELECT CompRule.RowID,
                           CompRule.SourceID,
                           CompRule.RuleName,
                           CompRule.RuleDesc,
                           CompRule.TableName,
                           CompRule.RuleTmp,
                           CompRule.Status,
                           CompRule.Seq
                      FROM CompRule
                     WHERE 1 = 1 AND 
                           status = 1
                     ORDER BY CompRule.SourceID,
                              CompRule.Seq";
        }
    }
}
