using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using OnSiteFundComparer.QuickCompare.Models;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareTmpServices : CompareServices<Models.CompareRuleTmp>
    {
        

        protected override CompareRuleTmp Mapor(SQLiteDataReader reader)
        {
            CompareRuleTmp crTmp = new CompareRuleTmp();

            crTmp.RowID = Convert.ToInt32(reader.GetValue(0));
            crTmp.TmpName = reader.GetValue(1).ToString();
            crTmp.TmpType = (CompareRulesTypes)reader.GetValue(2);
            crTmp.TmpDesc = reader.GetValue(3).ToString();
            crTmp.Status = Convert.ToInt32(reader.GetValue(4) );
            crTmp.Seq = Convert.ToInt32(reader.GetValue(5));

            return crTmp;
        }

        protected override string SelectSql()
        {
            var Sql = Models.CompareRuleTmp.GetSql();
            Sql = Sql.Replace("@para", " and status = 1");

            return Sql;
        }
    }
}
