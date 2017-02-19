using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OnSiteFundComparer.Models;

namespace OnSiteFundComparer.Business
{
    class DataMgr 
    {
        string MainDB = Application.StartupPath + "\\" +
            OnSiteFundComparer.Properties.Settings.Default.MainDBFile;

        //string MainDB = GlobalEnviroment.MainDBDir;

        int treeDeep = 0;
        public DataMgr()
        {

        }

        public List<Models.DataItem> GetChildDataItemList(int ParentID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            var fundList = diss.GetDisplayDataItems();
            diss.Close();

            return fundList.FindAll(x => x.ParentID == ParentID); ;
        }
        public List<Models.DataItem> GetChildAllList(int ParentID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            var fundList = diss.GetDisplayDataItems();

            List<Models.DataItem> newList = new List<DataItem>();

            CreateDataItem(newList, fundList, ParentID);

            return newList;
        }
        private void CreateDataItem(List<DataItem> allchildlist, List<DataItem> list, int ParentID)
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
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            var fundList = diss.GetDisplayDataItems();
            diss.Close();

            TreeNode tn = new TreeNode();            
            CreateTree(tn, fundList, startID);
            tn.Expand();
            
            return tn;
        }

        public TreeNode BuildAllItemStruct(int startID)
        {
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            var fundList = diss.GetDataItems(); //show hidden items
            diss.Close();

            TreeNode tn = new TreeNode();
            CreateTree(tn, fundList, startID);
            tn.Expand();

            return tn;
        }

        private void CreateTree(TreeNode tn, List<DataItem> list, int rowid)
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
            Service.DataFormatService ffs = new Service.DataFormatService(MainDB);
            ffs.SaveDataFormat(df, di.RowID);
        }

        internal DataSet GetDataFormat(DataItem di)
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainDB);
            return ffs.GetDataFormat(di.RowID);
        }

        
        internal List<Models.DataFormat> GetDataFormatList(DataItem di)
        {
            List<Models.DataFormat> dataformat = new List<DataFormat>();

            DataSet ds = GetDataFormat(di);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                Models.DataFormat df = new DataFormat();

                df.ParentID = di.RowID;
                df.RowID = int.Parse(dr[0].ToString());

                df.colName = dr[2].ToString();
                df.colNumber = int.Parse(dr[3].ToString());
                df.Seq = int.Parse(dr[4].ToString());

                df.colCode = dr[5].ToString();
                df.DisplayName = dr[6].ToString();

                dataformat.Add(df);
            }
            return dataformat;
        }

        internal List<string> GetDataLabelList()
        {
            List<string> labels = new List<string>();
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);

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
            Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);

            diss.UpdateLabels(labels);
        }

        internal DataFormat GetDataRelation(DataItem di)
        {
            DataFormat df = new DataFormat();
            //Service.DataItemStuctServices diss = new Service.DataItemStuctServices(MainDB);
            Service.DataFormatService fs = new Service.DataFormatService(MainDB);

            DataSet ds = fs.GetDataFormat(di.RowID, -1);

            if (ds.Tables[0].Rows.Count == 1)
            {

                df.colName = ds.Tables[0].Rows[0][2].ToString();
                int.TryParse(ds.Tables[0].Rows[0][3].ToString(), out df.colNumber);
                df.Comment = ds.Tables[0].Rows[0][4].ToString();
            }

            return df;
        }
        public List<DataFormat> GetAllDataFormatList()
        {
            Service.DataFormatService ffs = new Service.DataFormatService(MainDB);
            DataSet ds = ffs.GetAllDataFormat();

            List<Models.DataFormat> dataformat = new List<DataFormat>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Models.DataFormat df = new DataFormat();

                df.RowID = int.Parse(dr[0].ToString());
                df.ParentID = int.Parse(dr[1].ToString());               

                df.colName = dr[2].ToString();
                df.colNumber = int.Parse(dr[3].ToString());
                df.Seq = int.Parse(dr[4].ToString());

                dataformat.Add(df);
            }
            return dataformat;
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
    
}
