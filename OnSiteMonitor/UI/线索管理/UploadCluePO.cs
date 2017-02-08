using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erik.Utilities.Bases;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OnSiteFundComparer.UI.线索管理
{
    public class UploadCluePO : BaseProgressiveOperation
    {
        private List<WFM.JW.HB.Models.Clues> _clueList;
        private int pageSize = 5;

        public List<WFM.JW.HB.Models.Clues> returnList = new List<WFM.JW.HB.Models.Clues>();
        public UploadCluePO(List<WFM.JW.HB.Models.Clues> cList)
        {
            _clueList = cList;

            _totalSteps = cList.Count;

            MainTitle = "上传更新线索核查结果";
            SubTitle = "请稍后...";
        }


        public override void Start()
        {
            _currentStep = 0;
            OnOperationProgress(EventArgs.Empty);

            for(int i = 0; i <= _totalSteps / pageSize; i++)
            {
                var newlist = _clueList.Skip(i * pageSize).Take(pageSize);

                MemoryStream ms = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                //创建序列化的实例            
                formatter.Serialize(ms, newlist.ToList());//序列化对象，写入ms流中  
                ms.Position = 0;
                //byte[] bytes = new byte[ms.Length];//这个有错误
                byte[] bytes = ms.GetBuffer();
                var compressedBuffer = WFM.JW.HB.Models.DataHelper.Compress(bytes);

                var buf = GlobalEnviroment.theWebService.UpdateCheckData(compressedBuffer);

                var decompressedBuffer = WFM.JW.HB.Models.DataHelper.Decompress(buf);
                MemoryStream dms = new MemoryStream(decompressedBuffer);
                dms.Position = 0;
                var rlist = (IEnumerable<WFM.JW.HB.Models.Clues>)formatter.Deserialize(dms);


                returnList.AddRange(rlist);

                _currentStep += (i+1) * pageSize;
            }
            OnOperationEnd(EventArgs.Empty);

        }
    }
}
