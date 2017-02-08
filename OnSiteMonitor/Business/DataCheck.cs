using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Business
{
    public class DataCheck
    {
        private DAL.MySqlite MainSqliteDB = null;
        private DAL.MySqlite ImportSqliteDB = null;

        private int[] checklist = new int[] {
                                                12, //财政供养人员名单
                                                //13, //财政供养人员家属名单
                                                14 ,//领导干部名单
                                                15 ,//领导干部家属名单
                                                16 ,//村干部名单
                                                17 //村干部家属名单
        };

        private string checksql = "Select id, name, region, addr from @table1, @table2 where @table1.id not int( @table2.id)";


        public  void checking()
        {
            //get list
            Business.DataMgr dmgr = new DataMgr();
            var complist = dmgr.GetChildDataItemList(3);

            var di = complist.Find(x => x.RowID == 21);  // 人口信息

            
        }
    }
}
