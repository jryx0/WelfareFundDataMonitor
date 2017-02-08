using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Test
{
    public partial class TestDataGenerator : Form
    {
        Random _rand = new Random();

        int totalHJ = 10000000;

        List<string> IDHeaderList = new List<string>();
        List<string> TownList = new List<string>();
        List<string> ContryList = new List<string>();
        List<string> danweiList = new List<string>();
        List<string> RelationList = new List<string>();
        private List<string> xinList = new List<string>();
        private List<string> mingList = new List<string>();

        private List<string> hjresult = new List<string>();



        public TestDataGenerator()
        {
            InitializeComponent();
        }

        private DateTime GetRandDate(string start)
        {
            DateTime dtBegin = DateTime.Parse(start);
            TimeSpan ts = DateTime.Today - dtBegin;

            var day = _rand.Next(ts.Days);

            return dtBegin.AddDays(day);
        }

        private string GetID()
        {
            var index = _rand.Next(0, IDHeaderList.Count - 1);
            var id = IDHeaderList[index];

            //DateTime dtBegin = DateTime.Parse("1930-01-01");
            //TimeSpan ts = DateTime.Today - dtBegin;

            ///var day = _rand.Next(ts.Days);
            //var rear = dtBegin.AddDays(day).ToString("yyyyMMdd") + _rand.Next(1000, 9999);
            var rear = GetRandDate("1930-01-01").ToString("yyyyMMdd") + _rand.Next(1000, 9999);

            id = id + rear;
            return id;
        }
        private string GetName()
        {
            var index = _rand.Next(0, xinList.Count - 1);
            var xin = xinList[index];
            //index = _rand.Next(mingList.Count);
            //var min = mingList[index];

            return xin + (char)((int)'A' + _rand.Next(1, 23)) + (char)((int)'A' + _rand.Next(1, 23)) + (char)((int)'A' + _rand.Next(1, 23));
        }

        private string GetTown()
        {
            var index = _rand.Next(0, TownList.Count - 1);
            var town = TownList[index];

            return town;
        }
        private string GetContry()
        {
            var index = _rand.Next(0, ContryList.Count - 1);
            var Contry = ContryList[index];

            return Contry;
        }

        private string GetDanwei()
        {
            var index = _rand.Next(0, danweiList.Count - 1);
            var danwei = danweiList[index];

            return danwei;
        }

        private string GetRelation(int i = 0)
        {
            if (i != 0)
                return "本人";

            var index = _rand.Next(RelationList.Count - 1);
            var r = RelationList[index];

            return r;
        }

        private string GetPerson(string id, string name = null)
        {
            //id += GetName();
            var town = GetTown();
            //id += GetContry();
            //id += GetDanwei();

            if (name == null)
                name = GetName();
            //return id;
            return id + "," + name + "," + town + "," + town + GetContry() + "," + GetDanwei();
        }

        private string GetCar()
        {
            string[] cars = new string[] { "小型轿车", "大型汽车", "小型汽车" };

            return cars[_rand.Next(0, cars.Length - 1)];
        }

        private string GetMachine()
        {
            string[] ms = new string[] { "轮式拖拉机", "稻麦脱粒机", "微耕机", "旋耕机", "手扶拖拉机", "水稻插秧机", "茶叶杀青机", "稻麦脱粒机", "粮食烘干机", "其他育苗机械设备", "机耕船", "联合收割机" };
            return ms[_rand.Next(0, ms.Length - 1)];
        }
        private string GetTax()
        {
            return _rand.Next(100, 100000).ToString();
        }
        private string GetPos()
        {
            string[] pos = new string[] { "副县长", "局长", "局长", "副局长", "副局长", "副局长", "副局长"
            , "科长", "科长", "科长", "科长", "科长", "副科长", "副科长", "副科长", "副科长", "副科长", "副科长", "副科长", "副科长", "副科长"
            , "主任", "主任" , "主任", "主任" , "副主任", "副主任" , "副主任", "副主任" , "副主任", "副主任" , "副主任" };

            return pos[_rand.Next(0, pos.Length - 1)];
        }

        private string GetGuoqi()
        {
            string[] Guoqi = new string[] { "中国电信", "中国移动", "中国联通", "中国石油", "中国石化", "工商银行", "农业银行", "建设银行", "中国人保", "平安保险", "农商行", "中国邮政" };

            return Guoqi[_rand.Next(0, Guoqi.Length - 1)];
        }

        private void GenerateHJ(int maxNum)
        {
            List<string> leaderresult = new List<string>();
            List<string> leaderjiashu = new List<string>();

            List<string> cun = new List<string>();
            List<string> cunjiashu = new List<string>();
            List<string> cai = new List<string>();
            List<string> guoqi = new List<string>();
            List<string> dead = new List<string>();
            List<string> burn = new List<string>();

            List<string> hj = new List<string>();
            List<string> rk = new List<string>();



            for (int i = 0; i < maxNum; i++)
            {
                var _id = GetID(); //本人
                var _name = GetName();
                var town = GetTown();
                var hjInfo = _id + "," + _name;//GetPerson(_id, _name) + "," + _id + "," + GetRelation(1) + "\r\n";
                string l = "";
                string c = "";

                rk.Add(hjInfo + "\r\n");
                hj.Add(hjInfo + "," + _id + "," + _name + ",本人\r\n");

                int leader = i % 2000;
                if (leader == 0)
                {//add to  1% 领导
                    l = GetDanwei() + "," + hjInfo;
                    cai.Add(l + "\r\n");
                    l = l + "," + GetPos();
                    leaderresult.Add(l + "\r\n");
                }
                else if (leader == 1) //村干部
                {
                    c = town + "," + town + GetContry() + "," + hjInfo + "," + (i / 2 == 0 ? "村长" : "书记");
                    cun.Add(c + "\r\n");
                }
                else if (leader == 10 || leader == 45 || leader == 55) //财政供养
                    cai.Add(GetDanwei() + "," + hjInfo + "\r\n");
                else if (leader > 80 && leader < 85)
                    guoqi.Add(GetGuoqi() + "," + hjInfo + "\r\n");



                hjInfo = _id + "," + _name + "," + town + "," + town + GetContry() + ',' + _id + "," + _name + "," + GetRelation(1) + "\r\n";

                hjresult.Add(hjInfo);

                for (int j = 0; j < _rand.Next(1, 5); j++)
                {
                    var _id1 = GetID();
                    var _name1 = GetName();
                    var relate = GetRelation();
                    hjInfo = _id1 + "," + _name1 + "," + town + "," + town + GetContry() + ',' + _id + "," + _name + "," + relate + "\r\n";

                    if (leader == 0)
                    {//add to  1% 领导家属
                        var nl = _id1 + "," + _name1 + "," + l + "," + relate + "\r\n";
                        leaderjiashu.Add(nl);
                    }
                    else if (leader == 1) //村家属
                    {
                        var nc = _id1 + "," + _name1 + "," + c + "," + relate + "\r\n";
                        cunjiashu.Add(nc);
                    }

                    rk.Add(_id1 + "," + _name1 + "\r\n");
                    hj.Add(hjInfo + "," + _id1 + "," + _name1 + "," + relate + "\r\n");

                    hjresult.Add(hjInfo);
                }                
            }

            SaveFile(rk, "d:\\temp\\人口.csv");
            SaveFile(hj, "d:\\temp\\户籍.csv");
            SaveFile(leaderresult, "d:\\temp\\领导干部.csv");
            SaveFile(leaderjiashu, "d:\\temp\\领导干部家属.csv");
            SaveFile(cun, "d:\\temp\\村干部.csv");
            SaveFile(cunjiashu, "d:\\temp\\村干部家属.csv");
            SaveFile(cai, "d:\\temp\\财政供养人员.csv");
            SaveFile(guoqi, "d:\\temp\\国企职工.csv");

            for (int i = 0; i < hj.Count / 10000; i++)
            {
                int index = _rand.Next(0, hj.Count - 1);
                var info = hj[index];//.Replace("\r\n", ",") +  GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                dead.Add(info);
            }

            for (int i = 0; i < hj.Count / 10000; i++)
            {
                int index = _rand.Next(0, hj.Count - 1);
                var info = hj[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";
                burn.Add(info);
            }

            SaveFile(dead, "d:\\temp\\死亡.csv");
            SaveFile(burn, "d:\\temp\\火化.csv");

            rk.Clear();
            hj.Clear();
            leaderresult.Clear();
            leaderjiashu.Clear();
            cun.Clear();

            cunjiashu.Clear();
            cai.Clear();
            guoqi.Clear();
            dead.Clear();
            burn.Clear();
        }

        void GenerateCar()
        {
            List<string> clist = new List<string>();
            for (int i = 0; i < hjresult.Count / 50; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetCar() + "\r\n";

                clist.Add(info);
            }

            SaveFile(clist, "d:\\temp\\车辆.csv");
        }
        void GenerateMachine()
        {
            List<string> clist = new List<string>();
            for (int i = 0; i < hjresult.Count / 10000; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetMachine() + "\r\n";

                clist.Add(info);
            }

            SaveFile(clist, "d:\\temp\\农机.csv");
        }
        void GenerateHouse()
        {
            List<string> clist = new List<string>();
            for (int i = 0; i < hjresult.Count / 2000; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + ",房产证号00000XXXXXXXXXXXX\r\n";

                clist.Add(info);
            }

            SaveFile(clist, "d:\\temp\\房产.csv");
        }
        void GenerateTax()
        {
            List<string> clist = new List<string>();
            for (int i = 0; i < hjresult.Count / 1000; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetTax() + "," + GetRandDate("2016-01-01").ToString("yyyy-MM-dd") + "\r\n";

                clist.Add(info);
            }

            SaveFile(clist, "d:\\temp\\个人纳税.csv");
        }
        void GenerateCompany()
        {
            List<string> clist = new List<string>();
            for (int i = 0; i < hjresult.Count / 1000; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[0] + "," + data[1] + "," + GetTax() + "," + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + ", XXXX有限公司" + "\r\n";

                clist.Add(info);
            }

            SaveFile(clist, "d:\\temp\\公司纳税.csv");
        }
        void GenerateWelfare(string name, int minMoney, int maxMoney, int num)

        {
            //select 
            List<string> clist1 = new List<string>();
            List<string> clist2 = new List<string>();
            for (int i = 0; i < hjresult.Count / num; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[2] + "," + data[0] + "," + data[1] + "," + data[3];

                clist1.Add(info);

                index = _rand.Next(0, hjresult.Count - 1);
                info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                data = info.Split(',');
                info = data[2] + "," + data[0] + "," + data[1] + "," + data[3];

                clist2.Add(info);
            }



            for (int i = 0; i < 24; i++)
            {
                CreateMoney(clist1, clist2);
                List<string> newslist = new List<string>();
                foreach (var s in clist1)
                {
                    var amount = _rand.Next(minMoney, maxMoney);
                    string news = "";
                    if (i < 12)
                        news = s + "," + amount + String.Format(",2014-{0:00}-01", i + 1) + "\r\n";
                    else news = s + "," + amount + String.Format(",2015-{0:00}-01", i - 12 + 1) + "\r\n";

                    newslist.Add(news);
                }

                string dir = "d:\\temp\\" + name + "\\";
                MakeSureDirectory(dir);

                SaveFile(newslist, dir + name + "资金" + i + ".csv");
            }
        }
        void CreateMoney(List<string> clist1, List<string> clist2)
        {
            var num = _rand.Next(0, clist1.Count / 30);
            for (int i = 0; i < num; i++)
            {
                var index = _rand.Next(0, clist1.Count - 1);
                clist1.RemoveAt(index);


                index = _rand.Next(0, clist2.Count - 1);
                clist1.Add(clist2[index]);
                clist2.RemoveAt(index);
            }
        }
        void GenerateWelfare1(string name, int minMoney, int maxMoney, int num)
        {
            //select 
            List<string> clist1 = new List<string>();
            List<string> clist2 = new List<string>();
            for (int i = 0; i < hjresult.Count / num; i++)
            {
                int index = _rand.Next(0, hjresult.Count - 1);
                var info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                var data = info.Split(',');
                info = data[2] + "," + data[0] + "," + data[1] + "," + data[3];

                clist1.Add(info);

                index = _rand.Next(0, hjresult.Count - 1);
                info = hjresult[index];//.Replace("\r\n", ",") + GetRandDate("2011-01-01").ToString("yyyy-MM-dd") + "\r\n";

                data = info.Split(',');
                info = data[2] + "," + data[0] + "," + data[1] + "," + data[3];

                clist2.Add(info);
            }

            var amount = _rand.Next(minMoney, maxMoney);

            for (int i = 0; i < 24; i++)
            {
                CreateMoney(clist1, clist2);
                List<string> newslist = new List<string>();
                int j = 0;
                foreach (var s in clist1)
                {
                    j++;
                    string news = "";
                    if (i < 12)
                        news = s + "," + amount + String.Format(",2014-{0:00}-01,", i + 1) + (j % 2 == 0 ? "分散供养" : "集中供养") + "\r\n";
                    else news = s + "," + amount + String.Format(",2015-{0:00}-01,", i - 12 + 1) + (j % 2 == 0 ? "分散供养" : "集中供养") + "\r\n";

                    newslist.Add(news);
                }

                string dir = "d:\\temp\\" + name + "\\";
                MakeSureDirectory(dir);

                SaveFile(newslist, dir + name + "资金" + i + ".csv");
            }
        }

        private void SaveFile(List<string> result, string filename)
        {
            using (FileStream fs = new FileStream(
                      filename, FileMode.Append, FileAccess.Write))
            {
                foreach (var s in result)
                {
                    byte[] arrWriteData = Encoding.Default.GetBytes(s);
                    fs.Write(arrWriteData, 0, arrWriteData.Length);
                }
                fs.Close();
            }
        }
        private string ReadFile(String FileName)
        {
            string strRet = "";
            StreamReader mysr;
            try
            {
                mysr = new StreamReader(FileName, System.Text.Encoding.Default);

                String Lines = "";
                while ((Lines = mysr.ReadLine()) != null)
                {
                    strRet += Lines + "/";
                }
            }
            catch (Exception ex) { }
            return strRet;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int.TryParse(tbTotal.Text, out totalHJ);

            for (int i = 0; i < totalHJ / 1000000; i++)
                GenerateHJ(totalHJ / (totalHJ / 1000000));

            GenerateTax();
            //汽车
            GenerateCar();
            //农机
            GenerateMachine();
            //公司
            GenerateCompany();
            //房产
            GenerateHouse();




            GenerateWelfare("农村低保", 600, 800, 200);
            GenerateWelfare("城市低保", 900, 1600, 200);
            GenerateWelfare("医疗救助", 1000, 15000, 350);
            GenerateWelfare("廉租房", 100, 500, 500);
            GenerateWelfare("农村危房", 5000, 7500, 1500);
            GenerateWelfare("农业补贴", 50, 50000, 300);
            GenerateWelfare("退耕还林", 50, 100000, 350);
            GenerateWelfare("生态公益林", 50, 100000, 450);

            GenerateWelfare1("农村五保", 450, 600, 1000);

            MessageBox.Show("OOOOOOOOOOOOOOOOOK, 数据：" + hjresult.Count);
            this.Cursor = Cursors.Arrow;
        }


        private void AddToList(List<string> list, string strValue)
        {
            list.Clear();
            foreach (var s in strValue.Split('/'))
                list.Add(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {//id

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(IDHeaderList, strID);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//乡镇
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(TownList, strID);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//村
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(ContryList, strID);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//关系
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(RelationList, strID);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {//姓
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(xinList, strID);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {//名
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(mingList, strID);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {//单位

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var strID = ReadFile(ofd.FileName);
                AddToList(danweiList, strID);
            }


        }

        public static string MakeSureDirectory(string dir)
        {
            string strRet = "";
            try
            {
                if (!System.IO.Directory.Exists(dir))
                    MakeSureDirectoryPathExists(dir);
            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);

        /*
         string strRet = "";
            Regex reg1 = new Regex(@"[^\u4e00-\u9fa5]+");
            Regex reg2 = new Regex(@"[^a-zA-Z]+");

            StreamReader mysr;
            try
            {
                OpenFileDialog of = new OpenFileDialog();

                if (of.ShowDialog() == DialogResult.OK)
                {

                    mysr = new StreamReader(of.FileName, System.Text.Encoding.Default);
                    FileStream fs = new FileStream(
                        "d:\\日志.csv", FileMode.Append, FileAccess.Write);
                    String Lines = "";
                    
                    while ((Lines = mysr.ReadLine()) != null)
                    {
                        var han = reg1.Replace(Lines, "");
                        var py = reg2.Replace(Lines, "");

                        if (han.Length > 1)
                            continue;


                        byte[] arrWriteData = Encoding.Default.GetBytes(py + "," + han + "," + py.Substring(0, 1) + " \r\n");
                        fs.Write(arrWriteData, 0, arrWriteData.Length);

                    }

                    fs.Close();
                }
            }
            catch (Exception ex) { }
*/
    }
}
