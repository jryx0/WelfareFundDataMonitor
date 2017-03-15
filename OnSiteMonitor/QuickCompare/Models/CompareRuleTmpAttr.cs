namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRuleTmpAttr
    {
        public int RowID { set; get; }
        public CompareRuleTmp RuleTmp { set; get; }


        public string AttrName { set; get; }
        public string TmpSql { set; get; }

        public int Seq { set; get; }
        public int Status { set; get; }

        public static string GetSql()
        {
            return @"
                        SELECT CompRuleTmpAttr.RowID,
                               CompRuleTmpAttr.TmpID,
                               CompRuleTmpAttr.Name,
                               CompRuleTmpAttr.TmpSql,
                               CompRuleTmpAttr.Status,
                               CompRuleTmpAttr.Seq
                          FROM CompRuleTmpAttr
                         WHERE 1 = 1 @para
                         ORDER BY CompRuleTmpAttr.Seq";
        }
    }
}