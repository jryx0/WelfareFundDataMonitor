using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnSiteFundComparer.Models
{
    [Serializable]
    public class Clues
    {
        public int RowID { get; set; }
        public String ClueGuid { get; set; }
        public String RegionGuid;
        public string ID { get; set; }
        public string Name { get; set; }
        public string Addr { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public float Amount { get; set; }
        public string DateRange { get; set; }
        public int Table1 { get; set; }
        public int Table2 { get; set; }
        public int IsConfired { get; set; }
        public int IsClueTrue { get; set; }
        public int IsCompliance { get; set; }
        public int IsCP { get; set; }
        public string Fact { get; set; }
        public float IllegalMoney { get; set; }
        public DateTime CheckDate { get; set; }
        public string CheckByName1 { get; set; }
        public string CheckByName2 { get; set; }
        public string ReCheckFact { get; set; }
        public int ReCheckType { get; set; }
        public string ReCheckByName1 { get; set; }



        public static System.Data.SQLite.SQLiteParameter[] getUpdateParam(Clues _clue)
        {
            System.Data.SQLite.SQLiteParameter[] sqlietParams =
                new System.Data.SQLite.SQLiteParameter[]
            {
                new System.Data.SQLite.SQLiteParameter("@RowID", _clue.RowID),
                new System.Data.SQLite.SQLiteParameter("@Addr", _clue.Addr),
                new System.Data.SQLite.SQLiteParameter("@Region", _clue.Region),

                new System.Data.SQLite.SQLiteParameter("@IsClueTrue", _clue.IsClueTrue),
                new System.Data.SQLite.SQLiteParameter("@IsCompliance", _clue.IsCompliance),
                new System.Data.SQLite.SQLiteParameter("@IsCP", _clue.IsCP),

                new System.Data.SQLite.SQLiteParameter("@Fact", _clue.Fact),
                new System.Data.SQLite.SQLiteParameter("@IllegalMoney", _clue.IllegalMoney),
                new System.Data.SQLite.SQLiteParameter("@CheckDate", _clue.CheckDate),

                new System.Data.SQLite.SQLiteParameter("@CheckByName1", _clue.CheckByName1),
                new System.Data.SQLite.SQLiteParameter("@CheckByName2", _clue.CheckByName2),
                new System.Data.SQLite.SQLiteParameter("@ReCheckFact", _clue.ReCheckFact),

                new System.Data.SQLite.SQLiteParameter("@ReCheckType", _clue.ReCheckType),
                new System.Data.SQLite.SQLiteParameter("@ReCheckByName1", _clue.ReCheckByName1)
            };
            return sqlietParams;
        }
    }
}
