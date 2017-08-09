using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/25 8:46:47
* FileName   : ControllerConfig8021
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfig8021 : ControllerConfigBase, IControllerConfig
    {
        private int _defaultDeviceTypeCode = 3;
        public Model.ControllerNodeModel[] GetNodes()
        {
            return new ControllerNodeModel[]
                {
                    new ControllerNodeModel(ControllerNodeType.Loop,"回路",3, @"Resources\Icon\Style1\Loop.jpg"),
                };
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            ////板卡号，手盘号，手键号暂定不在器件中处理
            ColumnConfigInfo[] columnDefinitionArray = new ColumnConfigInfo[10];
            ColumnConfigInfo code = new ColumnConfigInfo();
            columnDefinitionArray[0] = new ColumnConfigInfo();
            columnDefinitionArray[0].ColumnName = "编码";
            columnDefinitionArray[1] = new ColumnConfigInfo();
            columnDefinitionArray[1].ColumnName = "器件类型";
            columnDefinitionArray[2] = new ColumnConfigInfo();
            columnDefinitionArray[2].ColumnName = "屏蔽";
            columnDefinitionArray[3] = new ColumnConfigInfo();
            columnDefinitionArray[3].ColumnName = "电流报警值";
            columnDefinitionArray[4] = new ColumnConfigInfo();
            columnDefinitionArray[4].ColumnName = "温度报警值";
            columnDefinitionArray[5] = new ColumnConfigInfo();
            columnDefinitionArray[5].ColumnName = "楼号";
            columnDefinitionArray[6] = new ColumnConfigInfo();
            columnDefinitionArray[6].ColumnName = "区号";
            columnDefinitionArray[7] = new ColumnConfigInfo();
            columnDefinitionArray[7].ColumnName = "层号";
            columnDefinitionArray[8] = new ColumnConfigInfo();
            columnDefinitionArray[8].ColumnName = "房间号";
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

            string deviceType = GetDeviceTypeCodeInfo();
            string[] validCode = deviceType.Split(',');

            List<DeviceType> lstAllTypeInfo = base.GetALLDeviceTypeInfo(null);

            List<DeviceType> lstResult = new List<DeviceType>();

            for (int i = 0; i < validCode.Length; i++)
            {
                var result = from t in lstAllTypeInfo where t.Code == Convert.ToInt32(validCode[i]) select t;
                lstResult.Add(result.FirstOrDefault());
            }
            return lstResult;
        }
        public string GetDeviceTypeCodeInfo()
        {
            string strMatchingDeviceNo = "0,3,8,18,19,20,21,22";
            return strMatchingDeviceNo;
        }


        public short GetMaxLoopAmountValue()
        {
            return 8;
        }

        public short GetMaxMachineAmountValue(int deviceAddress)
        {
            
            return 63;
        }

        public short GetMaxDeviceAmountValue()
        {
            return 252;
        }


        public List<int> GetDeviceCodeLength()
        {
            List<int> lstDeviceCode = new List<int>();
            lstDeviceCode.Add(8);            
            return lstDeviceCode;
        }

        public short GetMaxAmountForStandardLinkageConfig()
        {
            return 0;
        }

        public virtual Dictionary<string, RuleAndErrorMessage> GetDeviceInfoRegularExpression(int addressLength)
        {
            Dictionary<string, RuleAndErrorMessage> dictDeviceInfoRE = new Dictionary<string, RuleAndErrorMessage>();            
            //器件类型为“下拉框”，不需要验证
            //屏蔽
            dictDeviceInfoRE.Add("Disable", new RuleAndErrorMessage("^([01])$", "屏蔽取值范围为0或1"));
            //电流报警值
            //器件类型020的范围为100~1000;
            //器件类型008的范围为1~20(这个值表达的意思为”值*50mA”)
            //暂时调整为1~1000：后续再考虑验证方式

            dictDeviceInfoRE.Add("CurrentThreshold", new RuleAndErrorMessage("^([0-9]|[1-9][0-9]|[1-9][0-9][0-9]|1000)$", "电流报警值取值范围为100至1000"));
            //温度报警值
            dictDeviceInfoRE.Add("TemperatureThreshold", new RuleAndErrorMessage("^(4[5-9]|[5-9][0-9]|1[0-3][0-9]|140)$", "温度报警取值范围45至140"));            
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

        public Dictionary<string, RuleAndErrorMessage> GetControllerInfoRegularExpression(int deviceAddressLength)
        {
            Dictionary<string, RuleAndErrorMessage> dictControllerInfoRE = new Dictionary<string, RuleAndErrorMessage>();
            //名称
            dictControllerInfoRE.Add("Name", new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF()（）]{0,16}$", "允许填写”中文字符、英文字符、阿拉伯数字、圆括号”,最大长度16个字符"));
            if (deviceAddressLength == 7)
            {
                //机号
                dictControllerInfoRE.Add("MachineNumber", new RuleAndErrorMessage("^[0-9]{2}$", "请填写2位数字"));
            }
            else if (deviceAddressLength == 8)
            {
                //机号
                dictControllerInfoRE.Add("MachineNumber", new RuleAndErrorMessage("^[0-9]{3}$", "请填写3位数字"));
            }

            return dictControllerInfoRE;

        }




  


        public short GetMaxAmountForMixedLinkageConfig()
        {
            return 0;
        }

        public short GetMaxAmountForGeneralLinkageConfig()
        {
            return 0;
        }

        public short GetMaxAmountForManualControlBoardConfig()
        {
            return 0;
        }


        public short GetMaxAmountForBoardNoInManualControlBoardConfig()
        {
            return 0;
        }

        public short GetMaxAmountForSubBoardNoInManualControlBoardConfig()
        {
            return 0;
        }

        public short GetMaxAmountForKeyNoInManualControlBoardConfig()
        {
            return 0;
        }


        public List<DeviceType> GetAllowedDeviceTypeInfoForAnyAlarm()
        {
            return null;
        }

        public List<DeviceType> GetAllowedDeviceTypeInfoForLinkageGroup8000()
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
        public List<int> GetActionCoefficient()
        {
            return null;
        }

        public List<LinkageActionType> GetLinkageActionType()
        {
            return null;
        }

        public List<LinkageType> GetLinkageTypeWithCastration()
        {
            return null;
        }

        public List<LinkageType> GetLinkageType()
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


        public ColumnConfigInfo[] GetManualControlBoardColumns()
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, RuleAndErrorMessage> GetStandardLinkageConfigRegularExpression(int addressLength)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, RuleAndErrorMessage> GetManualControlBoardRegularExpression(int addressLength)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, RuleAndErrorMessage> GetMixedLinkageConfigRegularExpression(int addressLength)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, RuleAndErrorMessage> GetGeneralLinkageConfigRegularExpression(int addressLength)
        {
            throw new NotImplementedException();
        }
    }
}
