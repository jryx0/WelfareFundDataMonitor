using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using OnSiteFundComparer.DAL;

namespace OnSiteFundComparer.QuickCompare.Services
{
    public class CompareDataItemServices : CompareServices
    {
        public CompareDataItemServices(MySqlite _sqlite) : base(_sqlite)
        {
        }

        public List<Models.CompareDataItem> GetAllDataItems()
        {
            List<Models.CompareDataItem> diList = new List<Models.CompareDataItem>();

            SQLiteDataReader reader = MainSqliteDB.ExecuteReader(Models.CompareDataItem.SelectSql());
            if (reader.HasRows)
                while (reader.Read())
                {
                    diList.Add(Mapor(reader));
                }

            return diList;

        }
        public List<Models.CompareDataItem> GetDisplayDataItems()
        {
            var cdiList = GetAllDataItems().Where(x => x.Status).ToList();

            BuildDataItemStruct(cdiList);
            return cdiList;             
        }
        public void BuildDataItemStruct(List<Models.CompareDataItem> list)
        {
            if (list == null) return;

            foreach (Models.CompareDataItem di in list)
            {
                di.ParentItem = list.Find(x => x.RowID == di.ParentID);
            }
        }

        public Models.CompareDataItem GetDataItem(Models.CompareDataItem cdi)
        {
            var list = GetAllDataItems();
            if (list == null)
                return null;

            return list.Find(x => x.RowID == cdi.RowID);
        }

        public String ModifyDataItem(Models.CompareDataItem cdi)
        {
            string strRet = "";
            try
            {
                MainSqliteDB.ExecuteNonQuery(CommandType.Text, Models.CompareDataItem.ModifySql(), Models.CompareDataItem.getParam(cdi));
            }
            catch (Exception ex)
            {
                strRet = "项目" + cdi.DataFullName + "更新失败！" + ex.Message;
            }

            return strRet;
        }
        public String InsertDataItem(Models.CompareDataItem cdi)
        {
            string strRet = "";
            try
            {                
                MainSqliteDB.ExecuteNonQuery(CommandType.Text, Models.CompareDataItem.InsertSql(), Models.CompareDataItem.getParam(cdi));
            }
            catch (Exception ex)
            {
                strRet = "项目" + cdi.DataFullName + "插入失败！" + ex.Message;
            }

            return strRet;
        }
        public String DelDataItem(Models.CompareDataItem cdi)
        {
            string strRet = "项目" + cdi.DataFullName;
            try
            {
                MainSqliteDB.ExecuteNonQuery(@"Delete from dataitem where rowid = " + cdi.RowID.ToString());
                strRet = "";
            }
            catch (Exception ex)
            {
                strRet += "删除失败!(" + ex.Message + ")";
            }

            return strRet;
        }

        private Models.CompareDataItem Mapor(SQLiteDataReader reader)
        {
            Models.CompareDataItem cdi = new Models.CompareDataItem();

            cdi.RowID = Convert.ToInt32(reader.GetValue(0));
            cdi.ParentID = Convert.ToInt32(reader.GetValue(1));


            Models.FundItemTypes ft = new Models.FundItemTypes();
            Enum.TryParse<Models.FundItemTypes>(reader.GetValue(2).ToString(), out ft);
            cdi.DataType = ft;

            cdi.DataShortName = reader.GetValue(3).ToString();
            cdi.DataFullName = reader.GetValue(4).ToString();
            cdi.DataLink = Convert.ToInt32(reader.GetValue(5));
            cdi.Datapath = reader.GetValue(6).ToString();
            cdi.Status = (bool)reader.GetValue(7);
            cdi.Seq = Convert.ToInt32(reader.GetValue(8));
            //fi.CreateDate = reader.GetValue(9).ToString();
            cdi.dbTable = reader.GetValue(10).ToString();
            cdi.people = reader.GetValue(11).ToString();

            cdi.dbTablePre = reader.GetValue(12).ToString();
            cdi.col1 = reader.GetValue(13).ToString();
            cdi.col2 = reader.GetValue(14).ToString();


            return cdi;
        }
    }
}
