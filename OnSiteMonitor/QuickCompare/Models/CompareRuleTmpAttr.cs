namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRuleTmpAttr
    {
        public int RowID { set; get; }
        public CompareRuleTmp RuleTmp { set; get; }


        public string AttrName { set; get; }
        public string TmpSql { set; get; }

        public int Seq { set; get; }
        public bool Status { set; get; }
    }
}