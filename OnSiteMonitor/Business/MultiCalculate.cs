using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Business
{
    class CompareEnvirment
    {
        //导入文件目录
        public string inputExeclFileDir = Properties.Settings.Default.WorkDir;

        //输出中间数据库名称
        //public string tempDataBaseName = GlobalEnviroment.ResultDir + "\\temp\\";

        //结果数据库名称
        //public string ResultDataBaseName = GlobalEnviroment.ResultDir + "\\data\\";

        //结果文件目录
        //public string ReslutFile = GlobalEnviroment.ResultDir + "\\结果文件\\";

        //配置数据库
        public DAL.MySqlite settingSqliteDB = null;
        //中间数据库
        public DAL.MySqlite tempSqliteDB = null;
        //结果数据库
        public DAL.MySqlite ResultSqliteDB = null;
    }


    class MultiCalculate
    {
        
        public void initEnviroment()
        {

        }

        public void importData()
        {

        }





    }
}
