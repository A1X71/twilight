using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfig8007:ControllerConfigBase,IControllerConfig
    {
        private int _defaultDeviceTypeCode = 1;
        public SCA.Model.ControllerNodeModel[] GetNodes()
        {
            return new ControllerNodeModel[]
                {
                    new ControllerNodeModel(ControllerNodeType.Loop,"回路",3, @"Resources\Icon\Style1\Loop.jpg"),
                    new ControllerNodeModel(ControllerNodeType.Standard,"标准组态",3, @"Resources\Icon\Style1\Linkage.jpg"),                    
                    
                };
        }
        public SCA.Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            ////板卡号，手盘号，手键号暂定不在器件中处理
            ColumnConfigInfo[] columnDefinitionArray = new ColumnConfigInfo[12];
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
            columnDefinitionArray[7].ColumnName = "楼号";
            columnDefinitionArray[8] = new ColumnConfigInfo();
            columnDefinitionArray[8].ColumnName = "区号";
            columnDefinitionArray[9] = new ColumnConfigInfo();
            columnDefinitionArray[9].ColumnName = "层号";
            columnDefinitionArray[10] = new ColumnConfigInfo();
            columnDefinitionArray[10].ColumnName = "房间号";
            columnDefinitionArray[11] = new ColumnConfigInfo();
            columnDefinitionArray[11].ColumnName = "安装地点";
            return columnDefinitionArray;
        }
        public SCA.Model.ColumnConfigInfo[] GetStandardLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }
        public SCA.Model.ColumnConfigInfo[] GetGeneralLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }
        public SCA.Model.ColumnConfigInfo[] GetMixedLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public override List<DeviceType> GetDeviceTypeInfo()
        {

            string deviceType=GetDeviceTypeCodeInfo();
            string []validCode=deviceType.Split(',');

            List<DeviceType> lstAllTypeInfo=base.GetALLDeviceTypeInfo(null);

            List<DeviceType> lstResult=new List<DeviceType>();

            for(int i=0;i<validCode.Length;i++)
            {
                var result=from t in lstAllTypeInfo where t.Code == Convert.ToInt32(validCode[i]) select t ;
                lstResult.Add(result.FirstOrDefault());                
            }
            return lstResult;        
        }

        /// <summary>
        /// 取得8036可以设置的器件类型信息
        /// </summary>
        /// <returns></returns>
        public string GetDeviceTypeCodeInfo()
        {
            string strMatchingDeviceNo = "0,1,4,5,6,7,9,10,11,12,13,25,26,36,66,75,76,80,82";
            return strMatchingDeviceNo;
        }
        
        public short GetMaxLoopAmountValue()
        {
            return 1;
        }

        public short GetMaxMachineAmountValue(int deviceAddress)
        {
            return 200;
        }

        public short GetMaxDeviceAmountValue()
        {
            return 200;
        }

        public short GetMaxAmountForStandardLinkageConfig()
        {
            return 150;
        }

        public List<int> GetDeviceCodeLength()
        {
            List<int> lstDeviceCode = new List<int>();
            lstDeviceCode.Add(8);
            return lstDeviceCode;
        }
        public  Dictionary<string, RuleAndErrorMessage> GetDeviceInfoRegularExpression(int addressLength)
        {
            Dictionary<string, RuleAndErrorMessage> dictDeviceInfoRE = new Dictionary<string, RuleAndErrorMessage>();
            //器件编码 自动生成，不需要验证
            //if (addressLength == 7)
            //{
            //    dictDeviceInfoRE.Add("DeviceCode", new RuleAndErrorMessage("^([06][0-3])$","器件编码为0~63"));
            //}
            //else
            //{
            //    dictDeviceInfoRE.Add("DeviceCode", new RuleAndErrorMessage("^([01][0-9][0-9])$", "器件编码为0~199"));
            //}
            //器件类型为“下拉框”，不需要验证


            //特性(如果为老版控制器，只能取0,1)
            dictDeviceInfoRE.Add("Feature", new RuleAndErrorMessage("^([0-3])$", "特性取值范围为0,1,2,3"));

            //屏蔽
            dictDeviceInfoRE.Add("Disable", new RuleAndErrorMessage("^(true|false)$", "屏蔽取值范围为0或1"));

            //灵敏度
            dictDeviceInfoRE.Add("SensitiveLevel", new RuleAndErrorMessage("^([123])$", "灵敏度取值范围为1,2,3"));


            //延时
            dictDeviceInfoRE.Add("Delay", new RuleAndErrorMessage("^([1-9]|[1-9][1-9]|1[0-7][0-9]|180)$", "延时取值范围为1~180"));

            //输出组 000~150
            dictDeviceInfoRE.Add("StandardLinkageGroup", new RuleAndErrorMessage("^(00[1-9]|0[0-9][0-9]|1[0-4][0-9]|150)$", "输出组取值范围为000~150"));
            //广播分区 说明书的有效范围为1~240【上一版软件的有效范围0~240】。仅“广播模块”可设置此项属性（如“070常规广播”），其余模块一律置0。
            //对于广播模块为0的状态，移至整体验证中
            dictDeviceInfoRE.Add("BroadcastZone", new RuleAndErrorMessage("^([0-9]|[1-9][0-9]|1[0-7][0-9]|180)$", "输出组取值范围为000~180"));

            //允许楼、区、层、房间号同时为0，如果单独为0时，在其它规则里检查2017-04-20
            //楼 1~63
            dictDeviceInfoRE.Add("BuildingNo", new RuleAndErrorMessage("^([1-9]|[0-5][0-9]|[0-6][0-3]|0)$", "楼号取值范围为1~63"));
            //区1~99
            dictDeviceInfoRE.Add("ZoneNo", new RuleAndErrorMessage("^([1-9]|[0-9][0-9]|0)$", "区号取值范围为1~99"));
            //层 -9~-1 1~63
            dictDeviceInfoRE.Add("FloorNo", new RuleAndErrorMessage("^(-[1-9]|([1-9]|[0-5][0-9]|[0-6][0-3]|0))$", "层号取值范围为-9~63(不包括0)"));
            //房间号  1~255
            dictDeviceInfoRE.Add("RoomNo", new RuleAndErrorMessage("^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]|0)$", "房间号取值范围为1~255"));
            //安装地点 26个字母+数字+中文
            dictDeviceInfoRE.Add("Location", new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF]{0,16}$", "安装地点为字母或汉字或中文,最长16位"));
            return dictDeviceInfoRE;
        }
        public  Dictionary<string, RuleAndErrorMessage> GetStandardLinkageConfigRegularExpression()
        {
            Dictionary<string, RuleAndErrorMessage> dictExpressionAndInfo = new Dictionary<string, RuleAndErrorMessage>();
            //输出组号
            dictExpressionAndInfo.Add("Code", new RuleAndErrorMessage("^(00[1-9]|0[0-9][0-9]|1[0-4][0-9]|150)$", "输出组取值范围为000~150"));

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
            throw new NotImplementedException();
        }

        public List<DeviceType> GetAllowedDeviceTypeInfoForLinkageGroup8000()
        {
            string deviceType = base.GetAllowedDeviceTypeCodeInfoForLinkageGroup8000();
            return base.ConvertDeviceTypeCodeToDeviceType(deviceType);
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
    }

}
