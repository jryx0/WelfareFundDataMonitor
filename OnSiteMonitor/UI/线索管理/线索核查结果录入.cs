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
    public partial class 线索核查结果录入 : Form
    {
        private string dbFile;
        public 线索核查结果录入(string DBFile)
        {
            InitializeComponent();
            dbFile = DBFile;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init();
           
        }

        public int RowID { get; internal set; }
        public String ID { get; internal set; }

        public void Init()
        {
            String sql = @"SELECT RowID, ID, Name, Addr, Region, Type,   DateRange,Table1, Comment, 
                               IsClueTrue, IsCompliance, IsCP, Fact, IllegalMoney,CheckDate,
                               CheckByName1, CheckByName2, ReCheckFact, ReCheckType, ReCheckByName1, IsConfirmed
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

                int tmpi = 0;
                int.TryParse(r[7].ToString(), out tmpi);
                clue.Table1 = tmpi;

                clue.Comment = r[8].ToString();
                

                tmpi = 0;
                int.TryParse(r[9].ToString(), out tmpi);
                clue.IsClueTrue = tmpi;

                tmpi = 0;
                int.TryParse(r[10].ToString(), out tmpi);
                clue.IsCompliance = tmpi;

                tmpi = 0;
                int.TryParse(r[11].ToString(), out tmpi);
                clue.IsCP = tmpi;                

                clue.Fact = r[12].ToString() ;

                var tmpf = 0.0f;
                float.TryParse(r[13].ToString(), out tmpf);
                clue.IllegalMoney = tmpf;


                if (r[14].ToString() == "")
                    clue.CheckDate = System.DateTime.Now;
                else
                {
                    DateTime dt;
                    System.DateTime.TryParse(r[14].ToString(), out dt);
                    clue.CheckDate = dt;
                }

                clue.CheckByName1 = r[15].ToString();
                clue.CheckByName2 = r[16].ToString();

                clue.ReCheckFact = r[17].ToString();

                tmpi = 0;
                int.TryParse(r[18].ToString(), out tmpi);
                clue.ReCheckType = tmpi;

                clue.ReCheckByName1 = r[19].ToString();

                tmpi = 0;
                int.TryParse(r[20].ToString(), out tmpi);
                clue.IsConfirmed = tmpi;

                FillCtrl(clue);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            if (sourcelist == null) return;
            var item = sourcelist.Find(x => x.RowID == c.Table1);
            if (item == null) return;

            tbItemName.Text = item.DataShortName;
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

            tbIllegal.Text = c.IllegalMoney.ToString();
        }

        public bool SaveClue()
        {
            bool bRet = false;
            WFM.JW.HB.Models.Clues clue = new WFM.JW.HB.Models.Clues();

            clue.RowID = RowID;
            clue.ID = tbID.Text;
            clue.Name = tbName.Text;
            clue.Addr = tbAddr.Text;
            clue.Region = tbTown.Text;

            clue.IsClueTrue = cbIsClueTrue.SelectedIndex;
           
            clue.IsCP = cbIsCP.SelectedIndex;

            clue.Fact = tbFact.Text;
            clue.CheckByName1 = tbCheckName1.Text;
            clue.ReCheckByName1 = tbReCheck.Text;
            clue.CheckDate = dateTimePicker1.Value;

            var cList = new List<WFM.JW.HB.Models.Clues>();
            cList.Add(clue);

            if(线索查询.UpdateLocalClues(dbFile, cList, this) == 1)
                bRet = true;


            //DAL.MySqlite result = new DAL.MySqlite(dbFile);
            //try
            //{
            //    String sql = @"UPDATE Clue_report  SET 
            //                    Addr = @Addr,
            //                    Region = @Region,
            //                    IsConfirmed = 1,
            //                    IsUploaded = 0,
            //                    IsClueTrue = @IsClueTrue,                                
            //                    IsCP = @IsCP,
            //                    Fact = @Fact,
            //                    IllegalMoney = @IllegalMoney,
            //                    CheckDate = @CheckDate,
            //                    CheckByName1 = @CheckByName1,
            //                    CheckByName2 = @CheckByName2,
            //                    ReCheckFact = @ReCheckFact,
            //                    ReCheckType = @ReCheckType,
            //                    ReCheckByName1 = @ReCheckByName1
            //                  Where RowID = @RowID ";

            //    if (tbIllegal.Text == "")
            //        clue.IllegalMoney = 0;
            //    else clue.IllegalMoney = float.Parse(tbIllegal.Text);


            //    System.Data.SQLite.SQLiteParameter[] sqliteParams = new System.Data.SQLite.SQLiteParameter[]
            //                               {
            //                                    new System.Data.SQLite.SQLiteParameter("@RowID", clue.RowID),
            //                                    new System.Data.SQLite.SQLiteParameter("@Addr", clue.Addr),
            //                                    new System.Data.SQLite.SQLiteParameter("@Region", clue.Region),

            //                                    new System.Data.SQLite.SQLiteParameter("@IsClueTrue", clue.IsClueTrue),
                                                
            //                                    new System.Data.SQLite.SQLiteParameter("@IsCP", clue.IsCP),

            //                                    new System.Data.SQLite.SQLiteParameter("@Fact", clue.Fact),
            //                                    new System.Data.SQLite.SQLiteParameter("@IllegalMoney", clue.IllegalMoney),
            //                                    new System.Data.SQLite.SQLiteParameter("@CheckDate", clue.CheckDate),

            //                                    new System.Data.SQLite.SQLiteParameter("@CheckByName1", clue.CheckByName1),
            //                                    new System.Data.SQLite.SQLiteParameter("@CheckByName2", clue.CheckByName2),
            //                                    new System.Data.SQLite.SQLiteParameter("@ReCheckFact", clue.ReCheckFact),

            //                                    new System.Data.SQLite.SQLiteParameter("@ReCheckType", clue.ReCheckType),
            //                                    new System.Data.SQLite.SQLiteParameter("@ReCheckByName1", clue.ReCheckByName1)
            //                               };

            //    result.ExecuteNonQuery(CommandType.Text, sql, sqliteParams);

            //    bRet = true;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("更新线索失败：" + ex.Message); 
            //}

            return bRet;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbCheckName1.Name.Length == 0)
            {
                MessageBox.Show("必须输入检查人姓名！");
                return;
            }

            if (CheckInput())
                if (SaveClue())
                {
                    MessageBox.Show("保存成功！");
                    btnClear_Click(null, null);

                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    
                }
        }

        private bool CheckInput()
        {
            bool bRet = true;
            if(cbIsClueTrue.SelectedIndex < 0)
            {
                MessageBox.Show("请选择是否属实！");
                return false;
            }

            if (tbCheckName1.Text.Length ==  0)
            {
                MessageBox.Show("请输入核查人姓名！");
                return false;
            }


             

            return bRet;
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

            tbIllegal.Text = "";
        }
    }
}
