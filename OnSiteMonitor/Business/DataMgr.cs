using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 

namespace OnSiteFundComparer.Business
{
    class DataMgr
    {
        //string MainDB = Application.StartupPath + "\\" +
        //    OnSiteFundComparer.Properties.Settings.Default.MainDBFile;

        // string MainDB = GlobalEnviroment.MainDBFile;

        DAL.MySqlite MainSqliteDB;

        int treeDeep = 0;
        public DataMgr()
        {
            MainSqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
        }

        public DataMgr(string connstr)
        {
            MainSqliteDB = new DAL.MySqlite(connstr, GlobalEnviroment.isCryt); ;
        }

        public DataMgr(DAL.MySqlite _sqlite)
        {
            if (MainSqliteDB == null || MainSqliteDB.IsDBClose())
                MainSqliteDB = new DAL.MySqlite(GlobalEnviroment.MainDBFile, GlobalEnviroment.isCryt);
            else
                MainSqliteDB = _sqlite;
        }

        public List<Models.DataItem> GetChildDataItemList(int ParentID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);
            var fundList = diss.GetDisplayDataItems();
            diss.Close();

            return fundList.FindAll(x => x.ParentID == ParentID); ;
        }
        public List<Models.DataItem> GetChildAllList(int ParentID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);
            var fundList = diss.GetDisplayDataItems();

            List<Models.DataItem> newList = new List<Models.DataItem>();

            CreateDataItem(newList, fundList, ParentID);

            return newList;
        }

        private void CreateDataItem(List<Models.DataItem> allchildlist, List<Models.DataItem> list, int ParentID)
        {
            var fi = list.Find(x => x.RowID == ParentID);
            if (fi == null)
                return;

            allchildlist.Add(fi);
            var childlist = list.FindAll(x => x.ParentID == ParentID);
            foreach (var l in childlist)
            {
                l.parentItem = fi;
                CreateDataItem(allchildlist, list, l.RowID);
                //allchildlist.Add(l);
            }

            return;
        }

        public TreeNode BuildDisplayItemStruct(int startID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);
            var fundList = diss.GetDisplayDataItems();
            diss.Close();

            TreeNode tn = new TreeNode();
            CreateTree(tn, fundList, startID);
            tn.Expand();

            return tn;
        }

        public TreeNode BuildAllItemStruct(int startID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);
            var fundList = diss.GetDataItems(); //show hidden items
            diss.Close();

            TreeNode tn = new TreeNode();
            CreateTree(tn, fundList, startID);
            tn.Expand();

            return tn;
        }

        private void CreateTree(TreeNode tn, List<Models.DataItem> list, int rowid)
        {
            var fi = list.Find(x => x.RowID == rowid);
            if (fi == null)
                return;

            tn.ImageIndex = treeDeep;
            tn.SelectedImageIndex = treeDeep;
            treeDeep++;

            tn.Text = fi.DataFullName;
            tn.Tag = fi;

            var newlist = list.FindAll(x => x.ParentID == fi.RowID);
            foreach (var l in newlist)
            {
                TreeNode newTn = new TreeNode();
                CreateTree(newTn, list, l.RowID);
                newTn.Expand();
                tn.Nodes.Add(newTn);
            }

            treeDeep--;
            return;
        }

        public void SaveNewFormat(List<Models.DataFormat> df, Models.DataItem di)
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainSqliteDB);
            ffs.SaveDataFormat(df, di.RowID);
        }

        internal DataSet GetDataFormat(Models.DataItem di)
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainSqliteDB);
            return ffs.GetDataFormat(di.RowID);
        }

        internal DataSet GetDataFormat()
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainSqliteDB);
            return ffs.GetAllDataFormat();
        }

        internal List<Models.DataFormat> GetDataFormatList()
        {
            return GetDataFormatList(null);
        }

        internal List<Models.DataFormat> GetDataFormatList(Models.DataItem di)
        {
            List<Models.DataFormat> dataformat = new List<Models.DataFormat>();

            DataSet ds;
            if (di == null)
                ds = GetDataFormat();
            else
                ds = GetDataFormat(di);

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                //Models.DataFormat df = new Models.DataFormat();

                //df.ParentID = di.RowID;
                //df.RowID = int.Parse(dr[0].ToString());

                //df.colName = dr[2].ToString();
                //df.colNumber = int.Parse(dr[3].ToString());
                //df.Seq = int.Parse(dr[4].ToString());

                //df.colCode = dr[5].ToString();
                //df.DisplayName = dr[6].ToString();

                Models.DataFormat df = Mapor(dr);
               // df.ParentID = di.RowID;

                dataformat.Add(df);
            }
            return dataformat;
        }

        internal Models.DataFormat Mapor(DataRow dr)
        {
            Models.DataFormat df = new Models.DataFormat();

            
            df.RowID = int.Parse(dr[0].ToString());
            df.ParentID = int.Parse(dr[1].ToString());

            df.colName = dr[2].ToString();
            df.colNumber = int.Parse(dr[3].ToString());
            df.Seq = int.Parse(dr[4].ToString());

            df.colCode = dr[5].ToString();
            df.DisplayName = dr[6].ToString();

            return df;
        }


        internal List<string> GetDataLabelList()
        {
            List<string> labels = new List<string>();
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);

            DataSet ds = diss.GetDataLabel();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string label = dr[2].ToString();
                labels.Add(label);
            }
            return labels;

        }
        internal void UpdateLabel(List<string> labels)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainSqliteDB);

            diss.UpdateLabels(labels);
        }

        internal Models.DataFormat GetDataRelation(Models.DataItem di)
        {
            Models.DataFormat df = new Models.DataFormat();
            //Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            Service.DataFormatService fs = new Service.DataFormatService(MainSqliteDB);

            DataSet ds = fs.GetDataFormat(di.RowID, -1);

            if (ds.Tables[0].Rows.Count == 1)
            {

                df.colName = ds.Tables[0].Rows[0][2].ToString();
                int.TryParse(ds.Tables[0].Rows[0][3].ToString(), out df.colNumber);
                df.Comment = ds.Tables[0].Rows[0][4].ToString();
            }

            return df;
        }
        public List<Models.DataFormat> GetAllDataFormatList()
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainSqliteDB);
            DataSet ds = ffs.GetAllDataFormat();

            List<Models.DataFormat> dataformat = new List<Models.DataFormat>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Models.DataFormat df = new Models.DataFormat();

                df.RowID = int.Parse(dr[0].ToString());
                df.ParentID = int.Parse(dr[1].ToString());               

                df.colName = dr[2].ToString();
                df.colNumber = int.Parse(dr[3].ToString());
                df.Seq = int.Parse(dr[4].ToString());

                dataformat.Add(df);
            }
            return dataformat;
        }

        public List<Models.DataItem> GetDataItemByAim(List<Models.CompareAim> aims)
        {
            var AimDi = new List<Models.DataItem>();
            if (aims == null || aims.Count == 0)
                return AimDi;

            using (var dss = new Service.DataItemStuctServices(MainSqliteDB))
            {
                var diList = dss.GetDataItems();

                AimDi = (from di in diList
                         from a in aims
                         where (di.RowID == a.t1 || di.RowID == a.t2 || di.RowID == a.t3) && di.ParentID > 0
                         select di).Distinct().OrderBy(x => x.ParentID).ThenBy(x =>x.Seq).ToList();
            }

            return AimDi;
        }
        


        public List<Models.CompareAim> GetCompareAllAim(Models.RulesTypes tmpType)
        {
            Service.CompareAimService cas = new Service.CompareAimService(MainSqliteDB);
            return cas.GetCompareAllAim().Where(x => x.TmpType == tmpType).ToList();
        }

        //public CollisionAim GetAimbyID(string id)
        //{
        //    CollisionAim ca = null;
        //    Service.CollisionAimService cas = new Service.CollisionAimService(MainDB);


        //    var ds = cas.GetAimbyID(id);
        //    if (ds == null)
        //        return ca;

        //    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count == 1)
        //    {
        //        var dr = ds.Tables[0].Rows[0];

        //        ca = new CollisionAim();

        //        ca.RowID = int.Parse(dr[0].ToString());
        //        ca.SourceID = int.Parse(dr[1].ToString());

        //        ca.AimName = dr[2].ToString();
        //        ca.AimDesc = dr[3].ToString();
        //        ca.seq = int.Parse(dr[5].ToString());
        //        ca.TableName = dr[6].ToString();
        //        ca.Rules = dr[7].ToString();
        //        ca.Rules2 = dr[8].ToString();

        //    }

        //    return ca;
        //}
        //public DataSet GetAllAimDataSet()
        //{
        //    Service.CollisionAimService cas = new Service.CollisionAimService(MainDB);

        //    return cas.GetAims();
        //}
        //public List<CollisionAim> GetAllAim()
        //{
        //    DataSet ds = new Service.CollisionAimService(MainDB).GetAims();

        //    List<CollisionAim> aims = new List<CollisionAim>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        CollisionAim ca = new CollisionAim();

        //        ca.RowID = int.Parse(dr[0].ToString());
        //        ca.SourceID = int.Parse(dr[1].ToString());

        //        ca.AimName = dr[2].ToString();
        //        ca.AimDesc = dr[3].ToString();

        //        ca.seq = int.Parse(dr[5].ToString());
        //        ca.TableName = dr[6].ToString();
        //        ca.Rules = dr[7].ToString();
        //        ca.Rules2 = dr[8].ToString();

        //        aims.Add(ca);
        //    }
        //    return aims;
        //}

        //public void UpdateCollisionAim(CollisionAim ca)
        //{
        //    var cas = new Service.CollisionAimService(MainDB);
        //    cas.UpdateAim(ca);
        //}

        //public void DeleteCollisionAim(CollisionAim ca)
        //{
        //    if (ca == null)
        //        return;

        //    var cas = new Service.CollisionAimService(MainDB);
        //    cas.DeleteAimbyID(ca.RowID.ToString());
        //}
    }
    

    class CompareMgr 
    {
        public List<Models.DataItem> GetDataItemByAim(List<Models.CompareAim> aims)
        {
            var AimDi = new List<Models.DataItem>();
            if (aims == null || aims.Count == 0)
                return AimDi;

            using (var dss = new Service.DataItemStuctServices(GlobalEnviroment.MainDBFile))
            {
                var diList = dss.GetDataItems();

                AimDi = (from di in diList
                         from a in aims
                         where (di.RowID == a.t1 || di.RowID == a.t2 || di.RowID == a.t3)
                         select di).ToList();
            }

            return AimDi;
        }
    }
}
