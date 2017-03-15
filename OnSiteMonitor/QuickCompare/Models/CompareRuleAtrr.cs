namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRuleAtrr
    {
        public int RowID { set; get; }
        public CompareRule Rule { set; get; }

        public int Status { set; get; }
        public int Seq { set; get; }

        public RuleAttrType AttrType { set; get; }
        public string AttrName { set; get; }
        public int AttrValue { set; get; }

        public string AttrDesc { set; get; }
        public string AttrDetail { set; get; }

        public static string SelectSql()
        {
            return @"SELECT CompRuleAttr.RowID,
                           CompRuleAttr.RulesID,
                           CompRuleAttr.AttrType,
                           CompRuleAttr.AttrName,
                           CompRuleAttr.AttrValue,
                           CompRuleAttr.AttrDesc,
                           CompRuleAttr.AttrDetails,
                           CompRuleAttr.Seq,
                           CompRuleAttr.Status
                      FROM CompRuleAttr
                      WHERE 1=1 @para";
        }

    }

    public enum RuleAttrType
    {
        Table = 0,
        Parameter = 1,
        Rule = 2 
    }
}