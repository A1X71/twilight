using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/8 15:07:09
* FileName   : ControllerBase
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public abstract class ControllerConfigBase
    {   
        private string _compatibleSoftwareVersionForExcelTemplate = "4.0"; //EXCEL导入模板的兼容软件版本
        private string _compatibleControllerTypeForExcelTemplate = "NT8001,NT8036,NT8021,NT8007,FT8000,FT8003,NT8053";//EXCEL导入模板兼容的控制器类型
        private string _summarySheetNameForExcelTemplate = "摘要信息";  //EXCEL导入模板的摘要页签名称
        private  Dictionary<Model.ControllerType, string>  _controllerTablesName ;//{ get; set; }
        
        /// <summary>
        /// 在老版本数据文件中各控制器所拥有的数据表名称
        /// </summary>
        /// <returns></returns>
        public  Dictionary<Model.ControllerType, string> GetControllerTablesName()
        {
            _controllerTablesName = new Dictionary<Model.ControllerType, string>();
            _controllerTablesName.Add(Model.ControllerType.FT8003, "系统设置;文件配置;位置信息;设备种类;器件组态;");
            _controllerTablesName.Add(Model.ControllerType.NT8007, "系统设置;文件配置;位置信息;设备种类;器件组态;");
            _controllerTablesName.Add(Model.ControllerType.NT8021, "系统设置;文件配置;位置信息;设备种类;");
            _controllerTablesName.Add(Model.ControllerType.FT8000, "系统设置;文件配置;位置信息;设备种类;器件组态;");
            _controllerTablesName.Add(Model.ControllerType.NT8001, "系统设置;文件配置;位置信息;设备种类;器件组态;网络手控盘;通用组态;混合组态;");
            _controllerTablesName.Add(Model.ControllerType.NT8036, "系统设置;文件配置;位置信息;设备种类;器件组态;");
            _controllerTablesName.Add(Model.ControllerType.NT8053, "系统设置;文件配置;位置信息;设备种类;器件组态;网络手控盘;通用组态;混合组态;");
            return _controllerTablesName;    
        }
        
        public string CompatibleSoftwareVersionForExcelTemplate
        {
            get
            {
                return _compatibleSoftwareVersionForExcelTemplate;
            }
            set
            {
                _compatibleSoftwareVersionForExcelTemplate = value;
            }
        }
        public string CompatibleControllerTypeForExcelTemplate
        {
            get
            {
                return _compatibleControllerTypeForExcelTemplate;
            }
            set
            {
                _compatibleControllerTypeForExcelTemplate = value;
            }
        }
        public string SummarySheetNameForExcelTemplate
        {
            get
            {
                return _summarySheetNameForExcelTemplate;
            }
            set
            {
                _summarySheetNameForExcelTemplate = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<Model.DeviceType> GetDeviceTypeInfo(int projectID)
        { 
            List<Model.DeviceType> lstDeviceType=new List<Model.DeviceType>();
            //在软件中8001可设置"特性",器件类型”显示0~88
            //在软件中8003不可以设置"特性"，器件类型为0~129,但将12x下传至控制器后，器件类型显示为6x,特性列置为1.

            string strDeviceType="无器件,差定温,未定义,电气测温I,光电感烟,光温复合,点型红外,点型紫外,漏电探测I,可燃气体,红外光束,感温电缆,报警输入,手报,信号输入,急启按钮,未定义,未定义,电气组合,电气测温,漏电探测,组合测温,组合漏电,未定义,家用控制器,家用光电,家用手报,未定义,未定义,未定义,未定义,脉冲输出,电平输出,双动脉冲,双动电平,常规广播,声光警报,消火栓泵,喷淋泵,稳压泵,水幕泵,雨淋泵,泡沫泵,排烟机,送风机,新风机,空压机,防火阀,排烟阀,送风阀,电磁阀,防排烟阀,水幕电磁,雨淋阀,电梯,空调机组,柴油发电,照明配电,动力配电,层号灯,紧急照明,疏导指示,卷帘门中,卷帘门下,防火门,多线盘,声光输出,广播模块,常规脉冲,常规电平,常规广播,未定义,未定义,未定义,喷洒指示,光警报器,声警报器,喷洒模块,气灭盘,防盗探测,显示盘,消火栓,监管输入,压力开关,水流指示,高位水箱,信号碟阀,消防电源,联动电源,电源箱,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义";
//            string []arrDeviceType=strDeviceType.Split(',');
            Int16 intDevTypeIndex=0;
            foreach(var devName in strDeviceType.Split(','))
            {
                Model.DeviceType devType=new Model.DeviceType();
                devType.Code=intDevTypeIndex;
                devType.Name=devName; 
                devType.ProjectID=projectID;
                devType.IsValid=true; //初始全部置成True                
                lstDeviceType.Add(devType);
                intDevTypeIndex++;
            }
            return lstDeviceType;
        }
        /// <summary>
        /// 取得全部器件类型信息
        /// </summary>
        /// <returns></returns>
        public List<Model.DeviceType> GetALLDeviceTypeInfo(int? projectID)
        {
            List<Model.DeviceType> lstDeviceType = new List<Model.DeviceType>();
            //在软件中8001可设置"特性",器件类型”显示0~88
            //在软件中8003不可以设置"特性"，器件类型为0~129,但将12x下传至控制器后，器件类型显示为6x,特性列置为1.

         //string strDeviceType = "无器件,差定温,未定义,未定义,光电感烟,光温复合,点型红外,点型紫外,未定义,可燃气体,红外光束,感温电缆,报警输入,手报,信号输入,急启按钮,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,家用控制器,家用光电,家用手报,未定义,未定义,未定义,未定义,脉冲输出,电平输出,双动脉冲,双动电平,常规广播,声光警报,消火栓泵,喷淋泵,稳压泵,水幕泵,雨淋泵,泡沫泵,排烟机,送风机,新风机,空压机,防火阀,排烟阀,送风阀,电磁阀,防排烟阀,水幕电磁,雨淋阀,电梯,空调机组,柴油发电,照明配电,动力配电,层号灯,紧急照明,疏导指示,卷帘门中,卷帘门下,防火门,多线盘,声光输出,广播模块,常规脉冲,常规电平,常规广播,未定义,未定义,未定义,喷洒指示,光警报器,声警报器,喷洒模块,气灭盘,防盗探测,显示盘,消火栓,监管输入,压力开关,水流指示,高位水箱,信号碟阀,消防电源,联动电源,电源箱,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义";
            string strDeviceType = "无器件,差定温,未定义,电气测温I,光电感烟,光温复合,点型红外,点型紫外,漏电探测I,可燃气体,红外光束,感温电缆,报警输入,手报,信号输入,急启按钮,未定义,未定义,电气组合,电气测温,漏电探测,组合测温,组合漏电,未定义,家用控制器,家用光电,家用手报,未定义,未定义,未定义,未定义,脉冲输出,电平输出,双动脉冲,双动电平,常规广播,声光警报,消火栓泵,喷淋泵,稳压泵,水幕泵,雨淋泵,泡沫泵,排烟机,送风机,新风机,空压机,防火阀,排烟阀,送风阀,电磁阀,防排烟阀,水幕电磁,雨淋阀,电梯,空调机组,柴油发电,照明配电,动力配电,层号灯,紧急照明,疏导指示,卷帘门中,卷帘门下,防火门,多线盘,声光输出,广播模块,常规脉冲,常规电平,常规广播,未定义,未定义,未定义,喷洒指示,光警报器,声警报器,喷洒模块,气灭盘,防盗探测,显示盘,消火栓,监管输入,压力开关,水流指示,高位水箱,信号碟阀,消防电源,联动电源,电源箱,闭门器,门磁开关,电磁门吸,门磁释放器,,未定义,未定义,未定义,未定义,未定义,未定义,双常开模块,单常开模块,双常闭模块,单常闭模块,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义";
            //            string []arrDeviceType=strDeviceType.Split(',');
            Int16 intDevTypeIndex = 0;
            foreach (var devName in strDeviceType.Split(','))
            {
                Model.DeviceType devType = new Model.DeviceType();
                devType.Code = intDevTypeIndex;
                devType.Name =intDevTypeIndex.ToString().PadLeft(3,'0') + devName;                
                devType.IsValid = true; //初始全部置成True                
                if (projectID != null)
                {
                    devType.ProjectID = projectID;
                }
                lstDeviceType.Add(devType);
                intDevTypeIndex++;
            }
            return lstDeviceType;
        }
        /// <summary>
        /// 将“器件类型编码”转换为DeviceType类型
        /// </summary>
        /// <param name="deviceTypeCode">器件类型编码,以'逗号分隔’</param>
        /// <returns></returns>
        protected List<DeviceType> ConvertDeviceTypeCodeToDeviceType(string deviceTypeCode)
        {
            string[] validCode = deviceTypeCode.Split(',');

            List<DeviceType> lstAllTypeInfo = GetALLDeviceTypeInfo(null);

            List<DeviceType> lstResult = new List<DeviceType>();

            for (int i = 0; i < validCode.Length; i++)
            {
                var result = from t in lstAllTypeInfo where t.Code == Convert.ToInt32(validCode[i]) select t;
                lstResult.Add(result.FirstOrDefault());
            }
            return lstResult;
        }
        /// <summary>
        /// 通过器件编码取得器件类型
        /// </summary>
        /// <param name="deviceTypeCode"></param>
        /// <returns></returns>
        public DeviceType GetDeviceTypeViaDeviceCode(int deviceTypeCode)
        {
            List<DeviceType> lstAllTypeInfo = GetALLDeviceTypeInfo(null);
            DeviceType  deviceType = new DeviceType();           
            var result = from t in lstAllTypeInfo where t.Code == deviceTypeCode select t;
            deviceType = result.FirstOrDefault<DeviceType>();
            return deviceType;
        }
        /// <summary>
        /// 通过器件名称取得器件ID
        /// </summary>
        /// <param name="deviceTypeName"></param>
        /// <returns></returns>
        public short GetDeviceCodeViaDeviceTypeName(string deviceTypeName)
        {
            short deviceCode = -1;
            List<DeviceType> lstAllTypeInfo = GetALLDeviceTypeInfo(null);
            DeviceType deviceType = new DeviceType();
            var result = from t in lstAllTypeInfo where t.Name == deviceTypeName select t;
            deviceType = result.FirstOrDefault<DeviceType>();
            if (deviceType != null)
            {
                deviceCode = deviceType.Code;
            }
            return deviceCode;
        }
        /// <summary>
        /// 去掉ControllerType的None及UNCOMPATIBLE值
        /// </summary>
        /// <returns></returns>
        public List<Model.ControllerType> GetControllerType()
        {
            List<Model.ControllerType> lstControllerType = new List<Model.ControllerType>();
            foreach (Model.ControllerType type in Enum.GetValues(typeof(Model.ControllerType)))
            {
                if (type != Model.ControllerType.NONE && type != Model.ControllerType.UNCOMPATIBLE)
                {
                    lstControllerType.Add(type);
                }
            }
            return lstControllerType;
        }
        public List<string> GetSerialPortNumber()
        {
            List<string> lstComInfo = new List<string>();
            for (int i = 1; i < 10; i++)
            {
                lstComInfo.Add("COM" + i.ToString());
            }
            return lstComInfo;
        }

        public virtual List<string> GetFeatureList()
        {
            List<string> lstFeature = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                lstFeature.Add(i.ToString());
            }
            return lstFeature;
        }
        public virtual List<string> GetDisableList()
        {
            List<string> lstDisable = new List<string>();
            for(int i=0;i<2;i++)
            {
                lstDisable.Add(i.ToString());
            }
            return lstDisable;
        }
        public virtual List<string> GetSensitiveLevelList()
        {
            List<string> lstSensitiveLevel = new List<string>();
            for (int i = 1; i < 4; i++)
            {
                lstSensitiveLevel.Add(i.ToString());
            }
            return lstSensitiveLevel;
        }
        public virtual List<int> GetActionCoefficient()
        {
            List<int> lstActionCoefficient = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                lstActionCoefficient.Add(i);
            }
            return lstActionCoefficient;
        }
        /// <summary>
        /// 动作类型
        /// </summary>
        /// <returns></returns>
        public virtual List<LinkageActionType> GetLinkageActionType()
        {
            List<LinkageActionType> lstLinkageType = new List<LinkageActionType>();
            foreach (LinkageActionType type in Enum.GetValues(typeof(LinkageActionType)))
            {
                if (type != LinkageActionType.NONE)
                {
                    lstLinkageType.Add(type);
                }
            }
            return lstLinkageType;
        }
        /// <summary>
        /// 去掉LinkageType的None值,本层,邻层
        /// </summary>
        /// <returns></returns>
        public virtual List<Model.LinkageType> GetLinkageTypeWithCastration()
        {
            List<Model.LinkageType> lstLinkageType = new List<Model.LinkageType>();
            foreach (Model.LinkageType type in Enum.GetValues(typeof(Model.LinkageType)))
            {
                if (type != Model.LinkageType.None && type != Model.LinkageType.SameLayer && type != Model.LinkageType.AdjacentLayer)
                {
                    lstLinkageType.Add(type);
                }
            }
            return lstLinkageType;
        }
        /// <summary>
        /// 去掉LinkageType的None值
        /// </summary>
        /// <returns></returns>
        public virtual List<Model.LinkageType> GetLinkageType()
        {
            List<Model.LinkageType> lstLinkageType = new List<Model.LinkageType>();
            foreach (Model.LinkageType type in Enum.GetValues(typeof(Model.LinkageType)))
            {
                if (type != Model.LinkageType.None)
                {
                    lstLinkageType.Add(type);
                }
            }
            return lstLinkageType;
        }
        /// <summary>
        /// 联动组号为8000时，可启动的器件类型
        /// </summary>
        /// <returns></returns>
        protected string GetAllowedDeviceTypeCodeInfoForLinkageGroup8000()
        {
            string strMatchingDeviceNo = "31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,74,75,76,77,78";
            return strMatchingDeviceNo;
        }
        #region EXCEL 模板
        /// <summary>
        /// EXCEL模板->摘要信息->控制器设置->行数，名称
        /// </summary>
        /// <returns>行号及配置名称组成的字典</returns>
        public Dictionary<int, string> GetNameOfControllerSettingInSummaryInfoOfExcelTemplate()
        {
            Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = new Dictionary<int, string>();
            dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Add(5, "控制器名称");
            dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Add(6, "控制器类型");
            dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Add(7, "器件长度");
            dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Add(8, "控制器机号");
            dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Add(9, "串口号");
            return dictNameOfControllerSettingInSummaryInfoOfExcelTemplate;
        }

        /// <summary>
        ///  EXCEL模板->摘要信息->控制器设置->行数，值验证规则
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RuleAndErrorMessage> GetValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate(int deviceAddressLength)
        {
            Dictionary<int, RuleAndErrorMessage> dictValueRule = new Dictionary<int, RuleAndErrorMessage>();
            dictValueRule.Add(5, new RuleAndErrorMessage("^[A-Za-z0-9_\u4E00-\u9FFF]{0,10}$", "控制器名称为数字、字母、汉字、下划线,最长10个字符"));
            string[] compatibleControllerTypeArray = CompatibleControllerTypeForExcelTemplate.Split(',');
            string compatibleControllerType = "(";
            for (int i = 0; i < compatibleControllerTypeArray.Length; i++)
            {
                compatibleControllerType += "^" + compatibleControllerTypeArray[i] + "$|";
            }
            dictValueRule.Add(6, new RuleAndErrorMessage("" + compatibleControllerType.Substring(0, compatibleControllerType.LastIndexOf('|')) + "){1}", "控制器类型不正确"));//此验证取每一个匹配对象            

            if (deviceAddressLength == 7)
            {
                dictValueRule.Add(7, new RuleAndErrorMessage("^([7])$", "器件长度取值范围为7"));
                dictValueRule.Add(8, new RuleAndErrorMessage("^([0-5][0-9]|6[0-3])$", "机号范围为00~63"));
            }
            else if (deviceAddressLength == 8)
            {
                dictValueRule.Add(7, new RuleAndErrorMessage("^([8])$", "器件长度取值范围为8"));
                dictValueRule.Add(8, new RuleAndErrorMessage("^([0-1][0-9][0-9])$", "机号范围为000~199"));
            }
            else
            {
                dictValueRule.Add(7, new RuleAndErrorMessage("^([78])$", "器件长度取值范围为7或8"));
                dictValueRule.Add(8, new RuleAndErrorMessage("(^([0-5][0-9]|6[0-3])$)|(^([0-1][0-9][0-9])$)", "机号范围为00~63或000~199"));
            }

            dictValueRule.Add(9, new RuleAndErrorMessage("^(COM([1-9]|10))$", "串口号取值范围为COM1-COM10"));
            return dictValueRule;
        }
        /// <summary>
        /// EXCEL模板->摘要信息->回路设置->行数，名称
        /// </summary>
        /// <returns>行号及配置名称组成的字典</returns>
        public Dictionary<int, string> GetNameOfLoopSettingInSummaryInfoOfExcelTemplate()
        {
            Dictionary<int, string> dictNameOfLoopSettingInSummaryInfoOfExcelTemplate = new Dictionary<int, string>();
            dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Add(13, "回路数量");
            dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Add(14, "回路分组");
            dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Add(15, "默认器件类型");
            return dictNameOfLoopSettingInSummaryInfoOfExcelTemplate;
        }
        public abstract List<DeviceType> GetDeviceTypeInfo();
        
        /// <summary>
        ///  EXCEL模板->摘要信息->回路设置->行数，值验证规则
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RuleAndErrorMessage> GetValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate()
        {
            Dictionary<int, RuleAndErrorMessage> dictValueRule = new Dictionary<int, RuleAndErrorMessage>();
            dictValueRule.Add(13, new RuleAndErrorMessage("^[1-9]$|^[1-5][0-9]$|^6[0-4]$", "回路数量取值为1-64"));
            dictValueRule.Add(14, new RuleAndErrorMessage("^[1-9]$|^[1-5][0-9]$|^6[0-4]$", "回路分组取值为1-64"));
            List<DeviceType> lstDeviceType = GetDeviceTypeInfo();
            string devTypeExp = "(";
            foreach (var devType in lstDeviceType)
            {
                devTypeExp += "" + devType.Name + "|";
            }
            dictValueRule.Add(15, new RuleAndErrorMessage("" + devTypeExp.Substring(0, devTypeExp.LastIndexOf('|')) + "){1}", "器件类型不正确"));//此验证取每一个匹配对象

            return dictValueRule;
        }
        /// <summary>
        /// EXCEL模板->摘要信息->其它设置->行数，名称
        /// </summary>
        /// <returns>行号及配置名称组成的字典</returns>
        public Dictionary<int, string> GetNameOfOtherSettingInSummaryInfoOfExcelTemplate()
        {
            Dictionary<int, string> dictNameOfOtherSettingInSummaryInfoOfExcelTemplate = new Dictionary<int, string>();
            dictNameOfOtherSettingInSummaryInfoOfExcelTemplate.Add(19, "工作表名称");
            return dictNameOfOtherSettingInSummaryInfoOfExcelTemplate;
        }
        /// <summary>
        ///  EXCEL模板->摘要信息->其它设置->行数，值验证规则
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, RuleAndErrorMessage> GetValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate()
        {
            Dictionary<int, RuleAndErrorMessage> dictValueRule = new Dictionary<int, RuleAndErrorMessage>();


            // ControllerNodeModel[] nodes = GetNodes();
            //string otherSettingRuleExp = "(";
            //for (int i = 0; i < nodes.Length; i++)
            //{
            //    if (nodes[i].Type != ControllerNodeType.Loop)
            //    { 
            //        otherSettingRuleExp += "[" + nodes[i].Name + "];";
            //    }
            //}
            // dictValueRule.Add(19, new RuleAndErrorMessage("" + otherSettingRuleExp.Substring(0, otherSettingRuleExp.LastIndexOf(';')) + "){1}", "工作表名称不正确"));//此验证取每一个匹配对象
            dictValueRule.Add(19, new RuleAndErrorMessage("^(^(?:^\n){0}$)|标准组态|混合组态|通用组态|网络手动盘|标准组态;混合组态|标准组态;混合组态;通用组态|标准组态;混合组态;通用组态;网络手动盘$", "工作表名称不正确"));//此验证取每一个匹配对象            
            return dictValueRule;
        }
        #endregion
        
    }
    public enum RefereceRegionName
    {         
        DeviceType=0, //器件类型
        ControllerType=1, //控制器类型
        DeviceCodeLength=3, //器件编码长度
        SerialPortNumber=4, //串口号
        ActionCoefficient=5, //动作常数
        ActionType=6,//动作类型
        LinkageTypeAll=7,//联动类型(全部)
        LinkageTypeCastration=8,//联动类型(精简)
        Feature=9,//特性
        Disable=10,//屏蔽
        SensitiveLevel=11, //灵敏度
        DeviceTypeWithAnyAlarm=12,//器件类型(包含任意火警)
        DeviceTypeWithoutFireDevice=13 //器件类型（不包含可报火警的器件）


    }
}
