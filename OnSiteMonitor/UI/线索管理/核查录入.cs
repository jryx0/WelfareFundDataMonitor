using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;


namespace OnSiteFundComparer.UI.线索管理
{
    public partial class 核查录入 : Form
    {
        private string dbFile;
        public 核查录入(string DBFile)
        {
            InitializeComponent();
            dbFile = DBFile;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
             
            GetClue();
        }

        public int RowID { get; internal set; }
        public String ID { get; internal set; }

        public void Init()
        {
            String sql = @"SELECT RowID, ID, Name, Addr, Region, Type,  DateRange, Table1, Comment,
                                   CASE WHEN IsClueTrue IS NULL THEN - 1 ELSE IsClueTrue END AS IsClueTrue,
                                   
                                   CASE WHEN IsCP IS NULL THEN 0 ELSE 1 END AS IsCP,
                                   Fact,
                                   CASE WHEN IllegalMoney IS NULL THEN 0 ELSE IllegalMoney END AS IllegalMoney,
                                   CheckDate,  CheckByName1,  CheckByName2,
                                   ReCheckFact,  
                                   CASE WHEN ReCheckType IS NULL THEN 0 ELSE ReCheckType END AS ReCheckType,
                                    ReCheckByName1
                              FROM Clue_Report Where RowID = @rowid ";

            DAL.MySqlite result = new DAL.MySqlite(dbFile, GlobalEnviroment.isCryt);
            try
            {
                var clue = new WFM.JW.HB.Models.Clues();

                var ds = result.ExecuteDataset(CommandType.Text, sql, new System.Data.SQLite.SQLiteParameter[]
                                                        {                                                            
                                                            new System.Data.SQLite.SQLiteParameter("@rowid", RowID)
                                                        });

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                    return;

                var r = ds.Tables[0].Rows[0];

                clue.RowID = int.Parse(r[0].ToString());
                clue.ID = r[1].ToString();
                clue.Name = r[2].ToString();
                clue.Addr = r[3].ToString();
                clue.Region = r[4].ToString();
                clue.Type = r[5].ToString();
                
                clue.DateRange = r[6].ToString();
                clue.Table1 = int.Parse( r[7].ToString()) ;
                clue.Comment = r[8].ToString();

                clue.IsClueTrue = int.Parse(r[9].ToString());
                clue.IsCompliance = int.Parse(r[10].ToString());
                clue.IsCP = int.Parse(r[11].ToString());
                clue.Fact = r[12].ToString() ;

                clue.IllegalMoney = float.Parse(r[13].ToString());
                if (r[14].ToString() == "")
                    clue.CheckDate = System.DateTime.Now;
                else
                clue.CheckDate = System.DateTime.Parse(r[15].ToString());
                clue.CheckByName1 = r[15].ToString();
                clue.CheckByName2 = r[16].ToString();

                clue.ReCheckFact = r[17].ToString();
                clue.ReCheckType = int.Parse(r[18].ToString());
                clue.ReCheckByName1 = r[19].ToString();

                FillCtrl(clue);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetClue()
        {
            try
            {
                var buf = GlobalEnviroment.theWebService.GetCheckData(RowID, ID);
                if(buf == null)
                {
                    MessageBox.Show("未找到线索(身份证号)："+ID+",RowID： "+RowID+",可能是网络错误，请退出重新登陆！");
                    return;
                }

                var newbuf = Models.DataHelper.Decompress(buf);
                MemoryStream dms = new MemoryStream(newbuf);
                BinaryFormatter formatter = new BinaryFormatter();

                dms.Position = 0;
                var newlist = (IEnumerable<WFM.JW.HB.Models.Clues>)formatter.Deserialize(dms);

                if (newlist == null)
                    return;

                FillCtrl(newlist.ToList().Find(x => x.RowID == RowID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void FillCtrl(WFM.JW.HB.Models.Clues c)
        {
            if (c == null)
                return;
            
            tbID.Text = c.ID;
            tbName.Text = c.Name;
            tbAddr.Text = c.Addr;
            tbTown.Text = c.Region;

            Business.DataMgr dmgr = new Business.DataMgr();
            var sourcelist = dmgr.GetChildDataItemList(2);
           

             
            tbClueType.Text = c.Type;

            if (c.IsConfirmed == 0)
                return;

            cbIsClueTrue.SelectedIndex = c.IsClueTrue;
            if (c.IsClueTrue == -1 )
                cbIsClueTrue.SelectedIndex = 1;

           

            cbIsCP.SelectedIndex = c.IsCP;
            if (c.IsCP == -1)
                cbIsCP.SelectedIndex = 1;
            

            tbFact.Text = c.Fact;
            tbCheckName1.Text = c.CheckByName1;
            tbReCheck.Text = c.ReCheckByName1;

            if(c.CheckDate.Year > 2016)
                dateTimePicker1.Value = c.CheckDate;
        }

        public void SaveClue()
        {
            List<WFM.JW.HB.Models.Clues> cList = new List<WFM.JW.HB.Models.Clues>();
            WFM.JW.HB.Models.Clues clue = new WFM.JW.HB.Models.Clues();
            clue.RowID = RowID;
            clue.ID = tbID.Text;
            clue.Name = tbName.Text;
            clue.Addr = tbAddr.Text;
            clue.Region = tbTown.Text;

            //clue.Table1 = int.Parse(tbItemName.Text);
            //clue.Type = tbClueType.Text;

            clue.IsClueTrue = cbIsClueTrue.SelectedIndex;
           
            clue.IsCP = cbIsCP.SelectedIndex;

            clue.Fact = tbFact.Text;
            clue.CheckByName1 = tbCheckName1.Text;
            clue.ReCheckByName1 = tbReCheck.Text;

            clue.CheckDate = dateTimePicker1.Value;
            cList.Add(clue);




            DAL.MySqlite result = new DAL.MySqlite(dbFile, GlobalEnviroment.isCryt);
            try
            {

                MemoryStream ms = new MemoryStream();
                //创建序列化的实例
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, cList.ToList());//序列化对象，写入ms流中  
                ms.Position = 0;
                //byte[] bytes = new byte[ms.Length];//这个有错误
                byte[] bytes = ms.GetBuffer();
                var compressedBuffer = Models.DataHelper.Compress(bytes);

                byte[] retbuf = GlobalEnviroment.theWebService.UpdateCheckData(compressedBuffer);

                var newbuf = Models.DataHelper.Decompress(retbuf);
                MemoryStream dms = new MemoryStream(newbuf);
                dms.Position = 0;
                var newlist = (IEnumerable<WFM.JW.HB.Models.Clues>)formatter.Deserialize(dms);

                if (newlist == null)
                    return;

                

                String sql = @"UPDATE report  SET 
                                Addr = @Addr,
                                Region = @Region,
                                 
                                IsClueTrue = @IsClueTrue,
                              
                                IsCP = @IsCP,
                                Fact = @Fact,
                                IllegalMoney = @IllegalMoney,
                                CheckDate = @CheckDate,
                                CheckByName1 = @CheckByName1,
                                CheckByName2 = @CheckByName2,
                                ReCheckFact = @ReCheckFact,
                                ReCheckType = @ReCheckType,
                                ReCheckByName1 = @ReCheckByName1
                              Where RowID = @RowID ";

                clue = newlist.First();
                if (clue == null) return;

                System.Data.SQLite.SQLiteParameter[] sqliteParams = new System.Data.SQLite.SQLiteParameter[]
                                           {
                                                new System.Data.SQLite.SQLiteParameter("@RowID", clue.RowID),
                                                new System.Data.SQLite.SQLiteParameter("@Addr", clue.Addr),
                                                new System.Data.SQLite.SQLiteParameter("@Region", clue.Region),

                                                new System.Data.SQLite.SQLiteParameter("@IsClueTrue", clue.IsClueTrue),
                                                new System.Data.SQLite.SQLiteParameter("@IsCP", clue.IsCP),

                                                new System.Data.SQLite.SQLiteParameter("@Fact", clue.Fact),
                                                new System.Data.SQLite.SQLiteParameter("@IllegalMoney", clue.IllegalMoney),
                                                new System.Data.SQLite.SQLiteParameter("@CheckDate", clue.CheckDate),

                                                new System.Data.SQLite.SQLiteParameter("@CheckByName1", clue.CheckByName1),
                                                new System.Data.SQLite.SQLiteParameter("@CheckByName2", clue.CheckByName2),
                                                new System.Data.SQLite.SQLiteParameter("@ReCheckFact", clue.ReCheckFact),

                                                new System.Data.SQLite.SQLiteParameter("@ReCheckType", clue.ReCheckType),
                                                new System.Data.SQLite.SQLiteParameter("@ReCheckByName1", clue.ReCheckByName1)
                                           };

                result.ExecuteNonQuery(CommandType.Text, sql, sqliteParams);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveClue();

            btnClear_Click(null, null);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbID.ReadOnly = false;
            RowID = 0;

            tbID.Text = "";
            tbName.Text = "";
            tbAddr.Text = "";
            tbTown.Text = "";
            tbItemName.Text = "";
            tbClueType.Text = "";
            cbIsClueTrue.SelectedIndex = 1;
            
            cbIsCP.SelectedIndex = 1;
            tbFact.Text = "";
            tbCheckName1.Text = "";
            tbReCheck.Text = "";
        }
    }
}
