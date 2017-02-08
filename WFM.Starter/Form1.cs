using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFM.Starter
{
    public partial class Form1 : Form
    {
        public FileTranser.MTOM.ClassLibrary.WebServicesHelp theWebService;
        String WFMPath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            var Uploads = theWebService.GetUpLoadInfo();


            if (Uploads == null || Uploads == "请登陆")
            {
                MessageBox.Show("登录错误");
                return;
            }

            if (Uploads.Length == 0)
            {
                MessageBox.Show("服务器中未找到上报数据库");
                return;
            }

            var Tasks = Uploads.Split('^');

            

            var data = Uploads.Split(';');

            var DBInfo = data[1].Substring(0, data[1].LastIndexOf(".bak.gz"));

            if (WFMPath.Length == 0)
            {
                MessageBox.Show("请选择惠民政策监督检查软件(单机版)目录");
                return;
            }

            if (!System.IO.File.Exists(WFMPath+ "\\导入中间数据库\\" + DBInfo))
            {
                MessageBox.Show("未发现本地数据，请检查是否同'惠民政策监督检查软件(单机版).exe'在同一目录？ 如是请联系大数据专班");
                return;
            }

            DAL.MySqlite result = new DAL.MySqlite(WFMPath + "\\Data\\Config");

            String sql = @"INSERT INTO Task  (TaskName,  CreateDate,  TaskComment,  Region,  DBInfo,  Status,   TStatus,  UserName) VALUES
                         ('@TaskName',  '@CreateDate',  '@TaskComment',  '@Region',  '@DBInfo',  @Status,  @TStatus,  '@UserName'); ";
            sql = sql.Replace("@TaskName", data[2]);
            sql = sql.Replace("@CreateDate", data[4]);
            sql = sql.Replace("@TaskComment", data[3]);
            sql = sql.Replace("@Region", data[0].Substring(data[0].LastIndexOf("\\")+1));

            DBInfo = DBInfo.Substring(0, DBInfo.IndexOf(".db"));
            DBInfo = DBInfo.Substring(DBInfo.IndexOf('.') +1);
            sql = sql.Replace("@DBInfo", DBInfo);
            sql = sql.Replace("@Status", "20");
            sql = sql.Replace("@TStatus", "1");
            sql = sql.Replace("@UserName", data[5]);


            result.ExecuteNonQuery(sql);

            MessageBox.Show("OK, 修复完成！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                WFMPath = fbd.SelectedPath ;

                if (!System.IO.Directory.Exists(WFMPath + "\\导入中间数据库"))
                { 
                    MessageBox.Show("没有发现惠民政策监督检查软件(单机版)数据库目录,请重新选择");
                    WFMPath = "";
                }

            }
        }
    }
}