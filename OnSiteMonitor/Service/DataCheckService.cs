using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Service
{
    public class DataCheckService
    {
        private DAL.MySqlite _sqliteDB;
        public DataCheckService(string connStr)
        {
            _sqliteDB = new DAL.MySqlite(connStr);
        }

        public List<Models.CheckRules> GetCheckAllRules()
        {
            List<Models.CheckRules> crList = new List<Models.CheckRules>();
            try
            {
                var ds = _sqliteDB.ExecuteDataset("select checkname, checksql,Type from DataCheckRules where status = 1 order by seq");

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    Models.CheckRules cr = new Models.CheckRules();

                    cr.CheckName = dr[0].ToString();
                    cr.CheckSql = dr[1].ToString();
                    cr.Type = int.Parse(dr[2].ToString());
                    crList.Add(cr);
                }
            }
            catch(Exception ex)
            {
              //  throw new Exception("public List<Models.CheckRules> GetCheckAllRules()" + ex.Message);
            }

            return crList;
        }

        public List<Models.CheckRules> GetCheckRulesByType(int type)
        {
            var crList = GetCheckAllRules();

            return crList.Where(x => x.Type == type).ToList();
        }
    }
}
