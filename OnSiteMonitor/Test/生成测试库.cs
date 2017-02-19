using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnSiteFundComparer.Test
{
    public partial class 生成测试库 : Form
    {
        private string ResultExcelDir = GlobalEnviroment.ResultOutputDir; //excel file    
        public string InputDBDir = GlobalEnviroment.InputDBDir;
        private string InputExcelDir = GlobalEnviroment.InputExceltDir;


        public   string ComparingInfo = "";

        public 生成测试库()
        {
            InitializeComponent();            
        }

        protected override void OnLoad(EventArgs e)
        {

            //DirectoryInfo TheFolder = new DirectoryInfo(InputDBDir);

            //if (TheFolder.GetFiles().Count() != 0)
            //{
            //    var filename = TheFolder.GetFiles()
            //        .Where(x => ((x.FullName.IndexOf("result") == -1) && (x.FullName.IndexOf("Check") == -1)))
            //        .OrderByDescending(n => n.LastWriteTime).First();

            //    if (filename != null)
            //    {
            //        ComparingInfo = Path.GetFileNameWithoutExtension(filename.FullName);
            //        InputDBDir += ComparingInfo + ".db";

            //        tbDB.Text = InputDBDir;
            //    }
            //    else
            //    {
            //        cbFile.Checked = true;
            //    }
            //}
            //else
            //{
            //    cbFile.Checked = true;
            //}
            ComparingInfo = GetTestDB();
            if (ComparingInfo.Length != 0)
            {
                InputDBDir += ComparingInfo + ".db";
                tbDB.Text = InputDBDir;
            }
            else
            {
                tbDB.Text = "";
                cbFile.Checked = true;
            }


            base.OnLoad(e);
        }

        public static string GetTestDB()
        {
            String dbFileInfo = "";
            DirectoryInfo TheFolder = new DirectoryInfo(GlobalEnviroment.InputDBDir);            
            if (TheFolder.GetFiles().Count() != 0)
            {
                var filename = TheFolder.GetFiles()
                    .Where(x => ((x.FullName.IndexOf("result") == -1) && (x.FullName.IndexOf("Check") == -1)))
                    .OrderByDescending(n => n.LastWriteTime).First();

                if (filename != null)
                {
                    dbFileInfo = Path.GetFileNameWithoutExtension(filename.FullName);                     
                }
            }
            return dbFileInfo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(cbFile.Checked)
            {
                测试比对 tf = new 测试比对();

                tf.ShowDialog();
                ComparingInfo = tf.ComparingInfo;
                InputDBDir = GlobalEnviroment.InputDBDir +  ComparingInfo + ".db";
                

                tbDB.Text = InputDBDir;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                InputDBDir = ofd.FileName;
                tbDB.Text = ofd.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(GlobalEnviroment.InputDBDir);
            if (TheFolder.GetFiles().Count() != 0)
            {
                foreach (var file in TheFolder.GetFiles())
                {
                    try
                    {
                        File.Delete(file.FullName);
                    }
                    catch
                    { }
                }
            }
        }
    }
}
