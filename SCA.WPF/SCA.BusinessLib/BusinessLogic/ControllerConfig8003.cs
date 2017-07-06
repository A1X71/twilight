using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 17:02:49
* FileName   : ControllerConfig8003
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfig8003 : ControllerConfigBase, IControllerConfig
    {
        private int _defaultDeviceTypeCode = 4;
        public ControllerNodeModel[] GetNodes()
        {
            return new ControllerNodeModel[]
                {
                    new ControllerNodeModel(ControllerNodeType.Loop,"回路",3, @"Resources\Icon\Style1\Loop.jpg"),
                    new ControllerNodeModel(ControllerNodeType.Standard,"标准组态",3, @"Resources\Icon\Style1\Linkage.jpg"),                    
                };
        }
        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            ColumnConfigInfo[] columnDefinitionArray = new ColumnConfigInfo[10];
            ColumnConfigInfo code = new ColumnConfigInfo();
            columnDefinitionArray[0] = new ColumnConfigInfo();
            columnDefinitionArray[0].ColumnName = "编码";
            columnDefinitionArray[1] = new ColumnConfigInfo();
            columnDefinitionArray[1].ColumnName = "器件类型";
            columnDefinitionArray[2] = new ColumnConfigInfo();
            columnDefinitionArray[2].ColumnName = "特性";
            columnDefinitionArray[3] = new ColumnConfigInfo();
            columnDefinitionArray[3].ColumnName = "屏蔽";
            columnDefinitionArray[4] = new ColumnConfigInfo();
            columnDefinitionArray[4].ColumnName = "灵敏度";
            columnDefinitionArray[5] = new ColumnConfigInfo();
            columnDefinitionArray[5].ColumnName = "输出组1";
            columnDefinitionArray[6] = new ColumnConfigInfo();
            columnDefinitionArray[6].ColumnName = "输出组2"; 
            columnDefinitionArray[7] = new ColumnConfigInfo();
            columnDefinitionArray[7].ColumnName = "延时";
            columnDefinitionArray[8] = new ColumnConfigInfo();
            columnDefinitionArray[8].ColumnName = "区号";
            columnDefinitionArray[9] = new ColumnConfigInfo();
            columnDefinitionArray[9].ColumnName = "安装地点";
            return columnDefinitionArray;
        }

        public Model.ColumnConfigInfo[] GetStandardLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetGeneralLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetMixedLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }
        public override List<DeviceType> GetDeviceTypeInfo()
        {

            //string deviceType = GetDeviceTypeCodeInfo();
            //string[] validCode = deviceType.Split(',');

            //List<DeviceType> lstAllTypeInfo = base.GetALLDeviceTypeInfo(null);

            //List<DeviceType> lstResult = new List<DeviceType>();

            //for (int i = 0; i < validCode.Length; i++)
            //{
            //    var result = from t in lstAllTypeInfo where t.Code == Convert.ToInt32(validCode[i]) select t;
            //    lstResult.Add(result.FirstOrDefault());
            //}
            //return lstResult;
            string deviceType = GetDeviceTypeCodeInfo();
            return base.ConvertDeviceTypeCodeToDeviceType(deviceType);
        }
        public string GetDeviceTypeCodeInfo()
        {
            string strMatchingDeviceNo = "0,1,4,5,6,7,9,10,11,12,13,14,15,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88";
            return strMatchingDeviceNo;
        }
        public short GetMaxLoopAmountValue()
        {
            return 1;
        }

        public short GetMaxMachineAmountValue(int deviceAddress)
        {
            //最大机号31            
            return 31;
        }

        public short GetMaxDeviceAmountValue()
        {
            return 252;
        }
        /// <summary>
        /// 8000型控制器编码长度为7
        /// </summary>
        /// <returns></returns>
        public List<int> GetDeviceCodeLength()
        {
            List<int> lstDeviceCode = new List<int>();
            lstDeviceCode.Add(7);
            return lstDeviceCode;
        }
        public short GetMaxAmountForStandardLinkageConfig()
        {
            return 2000;
        }
        public virtual Dictionary<string, RuleAndErrorMessage> GetDeviceInfoRegularExpression(int addressLength)
        {
            Dictionary<string, RuleAndErrorMessage> dictDeviceInfoRE = new Dictionary<string, RuleAndErrorMessage>();

            //屏蔽
            dictDeviceInfoRE.Add("Disable", new RuleAndErrorMessage("^([01])$", "屏蔽取值范围为0或1"));

            //灵敏度
            dictDeviceInfoRE.Add("SensitiveLevel", new RuleAndErrorMessage("^([123])$", "灵敏度取值范围为1,2,3"));


            //延时
            dictDeviceInfoRE.Add("DelayValue", new RuleAndErrorMessage("^([1-9]|[1-9][1-9]|1[0-7][0-9]|180)$", "延时取值范围为1~180"));

            //输出组 0001~8000
            dictDeviceInfoRE.Add("StandardLinkageGroup", new RuleAndErrorMessage("^([0-7][0-9][0-9][1-9]|[0-7][0-9][1-9][0-9]|8000)$", "输出组取值范围为0001~8000"));
            //广播分区 说明书的有效范围为1~240【上一版软件的有效范围0~240】。仅“广播模块”可设置此项属性（如“070常规广播”），其余模块一律置0。
            //对于广播模块为0的状态，移至整体验证中
            dictDeviceInfoRE.Add("BroadcastZone", new RuleAndErrorMessage("^([0-9]|[1-9][0-9]|1[0-7][0-9]|180)$", "输出组取值范围为000~180"));
            //sdpKey 1~756
            dictDeviceInfoRE.Add("SDPKey", new RuleAndErrorMessage("^([1-9]|[1-6][0-9][0-9|7[0-4][0-9)|75[0-6]$", "区号取值范围为1~756"));
            //区1~512
            dictDeviceInfoRE.Add("ZoneNo", new RuleAndErrorMessage("^([1-9]|[1-4][0-9][0-9|5[0-1][0-2)$", "区号取值范围为1~512"));
            //安装地点 26个字母+数字+中文
            dictDeviceInfoRE.Add("Location", new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF]{0,16}$", "安装地点为字母或汉字或中文,最长17位"));
            return dictDeviceInfoRE;
        }


        public Dictionary<string, RuleAndErrorMessage> GetStandardLinkageConfigRegularExpression()
        {
            Dictionary<string, RuleAndErrorMessage> dictExpressionAndInfo = new Dictionary<string, RuleAndErrorMessage>();
            //输出组号
            dictExpressionAndInfo.Add("Code", new RuleAndErrorMessage("^([0-7][0-9][0-9][1-9]|[0-7][0-9][1-9][0-9]|8000)$", "输出组取值范围为0001~8000"));

            //联动模块 考虑是手输还是选择，如果选择就不需要验证

            //动作常数
            dictExpressionAndInfo.Add("ActionCoefficient", new RuleAndErrorMessage("^([1-5])$", "动作常数为1~5"));
            //联动组 与输出组号相同            

            //备注
            return dictExpressionAndInfo;
        }


        public short GetMaxAmountForMixedLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForGeneralLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }


        public short GetMaxAmountForBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForSubBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForKeyNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }


        public List<DeviceType> GetAllowedDeviceTypeInfoForAnyAlarm()
        {
            return null;
        }

        public List<DeviceType> GetAllowedDeviceTypeInfoForLinkageGroup8000()
        {
            string deviceType = base.GetAllowedDeviceTypeCodeInfoForLinkageGroup8000();
            return base.ConvertDeviceTypeCodeToDeviceType(deviceType);
        }

        public int DefaultDeviceTypeCode
        {
            get
            {
                return _defaultDeviceTypeCode;
            }
            set
            {
                _defaultDeviceTypeCode = value;
            }
        }




        public override List<LinkageActionType> GetLinkageActionType()
        {
            return null;
        }

        public override List<LinkageType> GetLinkageTypeWithCastration()
        {
            return null;
        }

        public override List<LinkageType> GetLinkageType()
        {
            return null;
        }

        public List<DeviceType> GetDeviceTypeInfoWithAnyAlarm()
        {
            return null;
        }

        public List<DeviceType> GetDeviceTypeInfoWithoutFireDevice()
        {
            return null;
        }
    }
}
