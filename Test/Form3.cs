using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Test
{
    public partial class Form3 : Form
    {
        string FileName;
        Dictionary<string, string> winpy;
        public Form3()
        {
            FileName = "";
            winpy = new Dictionary<string, string>();


            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FileName.Length == 0)
                return;

            StreamReader mysr;
            try
            {
                mysr = new StreamReader(FileName, System.Text.Encoding.Default);

                String Lines = "";
                while ((Lines = mysr.ReadLine()) != null)
                {
                    var word = Regex.Replace(Lines, @"[^\u4e00-\u9fa5]", "");
                    if(word.Length == 1)
                    { 
                        var py = Regex.Replace(Lines, @"[\u4e00-\u9fa5]", "");

                        var w = winpy.Keys.Where(x => x == word);
                        if (w == null || w.Count() == 0)
                            winpy[word] = py;
                        else
                        {
                            var oldpy = winpy[word] + " " + py;

                            var newpy = oldpy.Split(' ').Distinct();
                            winpy[word] = "";
                            foreach (var s in newpy)
                                winpy[word] += s + " "; 
                        }
                    }
                }

                 
                 


                SaveFile(winpy, "d:\\han.txt");

            }
            catch (Exception ex) { }

            MessageBox.Show("OK!");
        }


        private void SaveFile(Dictionary<string, string> result, string filename)
        {
            using (FileStream fs = new FileStream(
                      filename, FileMode.Append, FileAccess.Write))
            {
                foreach (var s in result)
                {
                    byte[] arrWriteData = Encoding.Default.GetBytes(s.Key + ',' + s.Value + "\r\n");
                    fs.Write(arrWriteData, 0, arrWriteData.Length);
                }
                fs.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
                FileName = ofd.FileName;

        }
    }
}
