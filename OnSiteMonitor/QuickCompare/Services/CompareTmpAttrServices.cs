using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using OnSiteFundComparer.QuickCompare.Models;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareTmpAttrServices : CompareServices<Models.CompareRuleTmpAttr>
    {
        internal void IncludeParent(List<CompareRuleTmp> _ruleList)
        {
            IncludeParent(GetAll(), _ruleList);
        } 

        public void IncludeParent(List<Models.CompareRuleTmpAttr> attrList, List<Models.CompareRuleTmp> tmpList)
        {
            if (attrList == null || tmpList == null)
                return;

            attrList.ForEach(a => a.RuleTmp = tmpList.Find(x => x.RowID == a.RuleTmp.RowID));
        }

        protected override CompareRuleTmpAttr Mapor(SQLiteDataReader reader)
        {
            CompareRuleTmpAttr tmpAttr = new CompareRuleTmpAttr();
            tmpAttr.RuleTmp = new CompareRuleTmp();

            tmpAttr.RowID = Convert.ToInt32(reader.GetValue(0));
            tmpAttr.RuleTmp.RowID = Convert.ToInt32(reader.GetValue(1));
            tmpAttr.AttrName = reader.GetValue(2).ToString();
            tmpAttr.TmpSql = reader.GetValue(3).ToString();
            tmpAttr.Status = Convert.ToInt32(reader.GetValue(4));
            tmpAttr.Seq = Convert.ToInt32(reader.GetValue(5));


            return tmpAttr;
        }

        protected override string SelectSql()
        {
            var Sql = CompareRuleTmpAttr.GetSql();
            Sql = Sql.Replace("@para", " and Status = 1");

            return Sql;
        }

    }
}
