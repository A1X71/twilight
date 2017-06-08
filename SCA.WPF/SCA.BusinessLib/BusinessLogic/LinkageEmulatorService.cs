using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.BusinessLib.Utility;
namespace SCA.BusinessLib
{
    /// <summary>
    /// 联动装置模拟服务
    /// </summary>
    public static class LinkageEmulatorService
    {
        static Dictionary<string, int> dictStandardLinkageCount = new Dictionary<string, int>();//以标准组态的组名为key,存储“标准组态”触发计数
        static List<string> lstDeviceCode = new List<string>();//存储
        static List<LinkageConfigStandard> lstActiveLinkageGroup = new List<LinkageConfigStandard>();//存储处于激活状态的标准组态信息
        //lstStandardLinkageDeviceInfo.Add()
        private static List<string> GetDeviceCode()
        {
            return lstDeviceCode;
        }
       public static Dictionary<DeviceInfo8001,LinkageSimulatorDeviceStatus> EmulateLinkageStandardInfo(List<Model.DeviceInfo8001> lstSourceDevices,ControllerModel controller) 
        {
            dictStandardLinkageCount.Clear();
            Dictionary<DeviceInfo8001, LinkageSimulatorDeviceStatus> linkageResult = new Dictionary<DeviceInfo8001, LinkageSimulatorDeviceStatus>();
            StandardLinkageTriggerForDevice(lstSourceDevices);//初始化输出组信息并计数
            StandardLinkageTriggerForGroup(controller.StandardConfig);//记录处于激活的输出组信息
            List<DeviceInfo8001> lstAllDevices=GetControllerDevices(controller);
            foreach (var code in lstDeviceCode)
            {
                if (code.Substring(0, controller.DeviceAddressLength - 5) != controller.MachineNumber)
                {
                    linkageResult.Add(new DeviceInfo8001 { Code = code }, LinkageSimulatorDeviceStatus.OtherMachine);
                }
                else
                { 
                    DeviceInfo8001 dev = lstAllDevices.Find(x => x.Code == code);
                    if (dev != null)
                    {
                        linkageResult.Add(dev ,LinkageSimulatorDeviceStatus.Actived);   
                    }
                    else
                    {
                        linkageResult.Add(new DeviceInfo8001 { Code = code }, LinkageSimulatorDeviceStatus.NotDefined);
                    }
                }
             
            }
            return linkageResult;

        }

       private static void StandardLinkageTriggerForGroup(List<LinkageConfigStandard> lstLinkageGroup)
        {
            foreach (var standardConfig in lstLinkageGroup)
            {
                if (dictStandardLinkageCount.Keys.Contains(standardConfig.Code))
                {
                    if (dictStandardLinkageCount[standardConfig.Code] >= standardConfig.ActionCoefficient)//大于“动作常数”
                    {
                        if (!lstActiveLinkageGroup.Contains(standardConfig)) //避免加入重复的组号
                        {
                            lstActiveLinkageGroup.Add(standardConfig); //组态信息加入至“已激活的组态信息”中

                            if (!standardConfig.DeviceNo1.IsNullOrEmpty()) { if (!lstDeviceCode.Contains(standardConfig.DeviceNo1)) { lstDeviceCode.Add(standardConfig.DeviceNo1); } }
                            if (!standardConfig.DeviceNo2.IsNullOrEmpty()) { if (!lstDeviceCode.Contains(standardConfig.DeviceNo2)) { lstDeviceCode.Add(standardConfig.DeviceNo2); } }
                            if (!standardConfig.DeviceNo3.IsNullOrEmpty()) { if (!lstDeviceCode.Contains(standardConfig.DeviceNo3)) { lstDeviceCode.Add(standardConfig.DeviceNo3); } }
                            var result = from link in lstLinkageGroup
                                         where link.Code == standardConfig.LinkageNo1 ||
                                             link.Code == standardConfig.LinkageNo2 ||
                                             link.Code == standardConfig.LinkageNo3
                                         select link;

                            List<string> lstLinkageCode = new List<string>();
                            if (standardConfig.LinkageNo1 != "") { lstLinkageCode.Add(standardConfig.LinkageNo1); }
                            if (standardConfig.LinkageNo2 != "") { lstLinkageCode.Add(standardConfig.LinkageNo2); }
                            if (standardConfig.LinkageNo3 != "") { lstLinkageCode.Add(standardConfig.LinkageNo3); }

                            CountStandardLinkageTriggerAmount(lstLinkageCode);

                            StandardLinkageTriggerForGroup(result.ToList());
                            //List<LinkageConfigStandard> lstConfigStandard = new List<LinkageConfigStandard>();

                            //standardConfig.LinkageNo2;
                            //standardConfig.LinkageNo3;
                        }
                    }
                }
            }   
        }
        /// <summary>
        /// 对器件引发的输出组计数
        /// </summary>
        /// <param name="lstSourceDevices"></param>
       private static void StandardLinkageTriggerForDevice(List<Model.DeviceInfo8001> lstSourceDevices)
        {
            //lstSourceDevices.Add()
            //找到已经设置了输出组的“器件信息”
            var deviceOutputLinkageGroup = from devices in lstSourceDevices
                                           where devices.LinkageGroup1 != "" ||
                                               devices.LinkageGroup2 != "" || devices.LinkageGroup3 != ""
                                           select devices;
            List<string> lstLinkageCode = new List<string>();
            foreach (var device in deviceOutputLinkageGroup)
            {
                if (device.LinkageGroup1 != "")
                {
                    lstLinkageCode.Add(device.LinkageGroup1);
                }
                if (device.LinkageGroup2 != "" && device.LinkageGroup2 !=device.LinkageGroup1)
                {
                    lstLinkageCode.Add(device.LinkageGroup2);
                }
                if (device.LinkageGroup3 != "" && device.LinkageGroup3 != device.LinkageGroup1 && device.LinkageGroup3 != device.LinkageGroup2)
                {
                    lstLinkageCode.Add(device.LinkageGroup3);
                }
                // var statndConfigInfo= from standardConfig in   controller.StandardConfig where standardConfig.DeviceNo1                
            }

            CountStandardLinkageTriggerAmount(lstLinkageCode);
        
        }
        /// <summary>
        /// 计算标准组态的触发次数
        /// </summary>
        /// <param name="lstStandardLinkageCode"></param>
       private static void CountStandardLinkageTriggerAmount(List<String> lstStandardLinkageCode)
        {
            foreach (var code in lstStandardLinkageCode)
            { 
                if (dictStandardLinkageCount.ContainsKey(code))
                {
                    dictStandardLinkageCount[code]++;
                }
                else
                {
                    dictStandardLinkageCount.Add(code, 1);
                }
            }
        }
        /// <summary>
        /// 取出控制器中的所有器件信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
       private static List<DeviceInfo8001> GetControllerDevices(ControllerModel controller)
        {
            List<DeviceInfo8001> lstResult = new List<DeviceInfo8001>();
            
            foreach (LoopModel l in controller.Loops)
            {
                foreach (var device in l.GetDevices<DeviceInfo8001>())
                {                    
                    lstResult.Add(device);
                }
            }
            return lstResult;
        }

        static void EmulateLinkageMixedInfo(){}
        static void  EmulateLinkageGeneralInfo(){}
    }
    /// <summary>
    /// 联动组态模拟器，器件状态
    /// </summary>
    public enum LinkageSimulatorDeviceStatus
    { 
        Actived, //已联动
        OtherMachine, //它机器件
        NotDefined   //未定义
    }

}
