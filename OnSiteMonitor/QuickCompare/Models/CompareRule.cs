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
        public int Status { set; get; }

        public CompareRulesTypes RuleType { set; get; }

        public CompareRuleTmp RuleTmp { set; get; }
        public List<CompareRuleAtrr> Attriutes { set; get; }

        internal static string SelectSql()
        {
            return @"SELECT CompRule.RowID,
                           CompRule.SourceID,
                           CompRule.RuleName 规则名称,
                           CompRule.RuleDesc 规则描述,
                           CompRule.TableName 存储表,
                           CompRule.RuleTmp  ,
                           CompRule.Status,
                           CompRule.RuleType 规则类型,
                           CompRule.Seq
                      FROM CompRule
                     WHERE 1 = 1 @para
                     ORDER BY CompRule.SourceID,
                              CompRule.Seq";
        }
    }

    public enum CompareRulesTypes
    {
        Compare = 0,
        Check = 1,
        Preprocess = 2 
    }
}
