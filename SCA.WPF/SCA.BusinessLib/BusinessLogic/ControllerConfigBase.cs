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
    public class ControllerConfigBase
    {   
        private string _compatibleSoftwareVersionForExcelTemplate = "4.0"; //EXCEL导入模板的兼容软件版本
        private string _compatibleControllerTypeForExcelTemplate = "NT8001,NT8036,NT8021,NT8007,FT8000,FT8003";//EXCEL导入模板兼容的控制器类型
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
            string strDeviceType = "无器件,差定温,未定义,电气测温I,光电感烟,光温复合,点型红外,点型紫外,漏电探测I,可燃气体,红外光束,感温电缆,报警输入,手报,信号输入,急启按钮,未定义,未定义,电气组合,电气测温,漏电探测,组合测温,组合漏电,未定义,家用控制器,家用光电,家用手报,未定义,未定义,未定义,未定义,脉冲输出,电平输出,双动脉冲,双动电平,常规广播,声光警报,消火栓泵,喷淋泵,稳压泵,水幕泵,雨淋泵,泡沫泵,排烟机,送风机,新风机,空压机,防火阀,排烟阀,送风阀,电磁阀,防排烟阀,水幕电磁,雨淋阀,电梯,空调机组,柴油发电,照明配电,动力配电,层号灯,紧急照明,疏导指示,卷帘门中,卷帘门下,防火门,多线盘,声光输出,广播模块,常规脉冲,常规电平,常规广播,未定义,未定义,未定义,喷洒指示,光警报器,声警报器,喷洒模块,气灭盘,防盗探测,显示盘,消火栓,监管输入,压力开关,水流指示,高位水箱,信号碟阀,消防电源,联动电源,电源箱,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义,未定义";
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
 
    }
}
