using System.Collections.Generic;

namespace OnSiteFundComparer.QuickCompare.Models
{
    public class CompareRuleTmp
    {
        public int RowID { set; get; }

        public string TmpName { set; get; }
        public string TmpDesc { set; get; }

        public CompareRulesTypes TmpType { set; get; }

         
        public int Status { set; get; }
        public int Seq { set; get; }

        public List<CompareRuleTmpAttr> Attributes {set;get;}

        public static string GetSql()
        {
            return @"SELECT CompRuleTmp.RowID,
                           CompRuleTmp.TmpName,
                           CompRuleTmp.TmpType,
                           CompRuleTmp.TmpDesc,
                           CompRuleTmp.Status,
                           CompRuleTmp.Seq
                      FROM CompRuleTmp
                     WHERE 1 = 1 @para
                     ORDER BY CompRuleTmp.Seq";
        }
    }
}