using System.Collections.Generic;

namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRuleTmp
    {
        public int RowID { set; get; }

        public string TmpName { set; get; }
        public int TmpDesc { set; get; }

        public string Desc { set; get; }

        public bool Status { set; get; }
        public int Seq { set; get; }

        public List<CompareRuleTmpAttr> Attributes {set;get;}
    }
}