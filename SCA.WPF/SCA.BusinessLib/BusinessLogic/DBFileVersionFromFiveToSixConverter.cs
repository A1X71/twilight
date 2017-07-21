using System;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.Interface.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 11:00:06
* FileName   : DBFileVersionFromFiveToSixConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    // 数据文件版本5->版本6的变化为:
    // 8001控制器的器件信息增加“特性”列
    // 在转换时需要做的工作：
    //      1>需要根据版本4中的器件类型填写“特性”列
    //      2>更新器件类型列：101~129的器件类型-64
    //      3>更新混合组态: 器件类型A，器件类型B,器件类型C的“点动”及“字锁”字样去掉，对于在101及129范围内的器件编码-64
    //      4>更新通用组态: 器件类型A，器件类型C的“点动”及“字锁”字样去掉，对于在101及129范围内的器件编码-64
    public class DBFileVersionFromFiveToSixConverter : IDBFileVersionConverter
    {
        public int DBFileSourceVersion
        {
            get { return 5; }
        }

        public int DBFileDestinationVersion
        {
            get { return 6; }
        }
        public ProjectModel UpgradeToDestinationVersion(ProjectModel project)
        {
            if (project != null)
            {
                //int deviceCodeLength = 0;
                foreach(var c in project.Controllers)
                {

                    //foreach (var l in c.Loops)
                    //{
                    //    if (l.DeviceAmount != 0)
                    //    {
                    //        LoopModel loop = l;
                    //        UpdateDeviceInfo(c.Type,ref loop);
                    //        l = loop;
                    //    }
                    //}

                    for (int i = 0; i < c.Loops.Count; i++)
                    {
                        if (c.Loops[i].DeviceAmount != 0)
                        {
                            LoopModel loop = c.Loops[i];
                            UpdateDeviceInfo(c.Type, ref loop);
                            c.Loops[i] = loop;
                        }
                    }
                    foreach (var config in c.MixedConfig)
                    {
                        UpdateMixedLinkageConfigInfo(config);
                    }
                    foreach (var config in c.GeneralConfig)
                    {
                        UpdateGeneralLinkageConfigInfo(config);
                    }
                }                
            }
            return project;
        }
        /// <summary>
        /// 更新器件信息
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        private bool UpdateDeviceInfo(ControllerType controllerType,ref  LoopModel loop)
        {
            bool result = false;
            switch (controllerType)
            {
                case ControllerType.NT8001:
                    {
                        List<DeviceInfo8001> lstDeviceInfo = loop.GetDevices<DeviceInfo8001>();
                        foreach(var device in lstDeviceInfo)
                        {
                            if (device.TypeCode > 36 && device.TypeCode < 66)
                            {
                                device.Feature = 0;
                            }
                            else if (device.TypeCode >= 101 && device.TypeCode <= 129)
                            {
                                device.Feature = 1;
                                device.TypeCode = System.Convert.ToInt16(device.TypeCode - 64);
                            }
                        }
                    }
                    break;
                case ControllerType.NT8007:                   
                    break;
                case ControllerType.NT8021:       
                    break;
                case ControllerType.NT8036:
                    break;
                case ControllerType.FT8000:
                    break;
                case ControllerType.FT8003:
                    break;
            }
            return result;
        }
        //4>更新通用组态: 器件类型A，器件类型C的“点动”及“字锁”字样去掉，对于在101及129范围内的器件编码-64
        private bool UpdateGeneralLinkageConfigInfo( LinkageConfigGeneral generalConfig)
        {
            bool result = false;
            try
            {
                if (generalConfig.DeviceTypeNameA != null)
                {
                    if (generalConfig.DeviceTypeNameA.Length > 0) //更新器件类型A
                    {
                        Int16 deviceTypeA = Convert.ToInt16(generalConfig.DeviceTypeNameA.Substring(0, 3));
                        if (deviceTypeA >= 101 && deviceTypeA <= 129)
                        {
                            generalConfig.DeviceTypeCodeA = Convert.ToInt16(deviceTypeA - 64);
                        }
                        else
                        {
                            generalConfig.DeviceTypeCodeA = deviceTypeA;
                        }
                    }
                }
                else
                {
                    generalConfig.DeviceTypeCodeA = 0;
                }
                if (generalConfig.DeviceTypeNameC != null)
                {
                    if (generalConfig.DeviceTypeNameC.Length > 0) //更新器件类型C
                    {
                        Int16 deviceTypeC = Convert.ToInt16(generalConfig.DeviceTypeNameC.Substring(0, 3));
                        if (deviceTypeC >= 101 && deviceTypeC <= 129)
                        {
                            generalConfig.DeviceTypeCodeC = Convert.ToInt16(deviceTypeC - 64);
                        }
                        else
                        {
                            generalConfig.DeviceTypeCodeC = deviceTypeC;
                        }
                    }
                }
                else
                {
                    generalConfig.DeviceTypeCodeC = 0;
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        //3>更新混合组态: 器件类型A，器件类型B,器件类型C
        //的“点动”及“字锁”字样去掉，对于在101及129范围内的器件编码-64
        private bool UpdateMixedLinkageConfigInfo(LinkageConfigMixed mixedConfig)
        {
            bool result = false;
            try
            {
                if (mixedConfig.DeviceTypeNameA != null)
                {
                    if (mixedConfig.DeviceTypeNameA.Length > 0) //更新器件类型A
                    {
                        Int16 deviceTypeA = Convert.ToInt16(mixedConfig.DeviceTypeNameA.Substring(0, 3));
                        if (deviceTypeA >= 101 && deviceTypeA <= 129)
                        {
                            mixedConfig.DeviceTypeCodeA = Convert.ToInt16(deviceTypeA - 64);
                        }
                        else
                        {
                            mixedConfig.DeviceTypeCodeA = deviceTypeA;
                        }
                    }
                }
                else
                {
                    mixedConfig.DeviceTypeCodeA = 0;
                }
                if (mixedConfig.DeviceTypeNameB != null)
                {
                    if (mixedConfig.DeviceTypeNameB.Length > 0) //更新器件类型B
                    {
                        Int16 deviceTypeB = Convert.ToInt16(mixedConfig.DeviceTypeNameB.Substring(0, 3));
                        if (deviceTypeB >= 101 && deviceTypeB <= 129)
                        {
                            mixedConfig.DeviceTypeCodeB = Convert.ToInt16(deviceTypeB - 64);
                        }
                        else
                        {
                            mixedConfig.DeviceTypeCodeB = deviceTypeB;
                        }
                    }
                }
                else
                {
                    mixedConfig.DeviceTypeCodeB= 0;
                }
                if (mixedConfig.DeviceTypeNameC != null)
                {
                    if (mixedConfig.DeviceTypeNameC.Length > 0) //更新器件类型C
                    {
                        Int16 deviceTypeC = Convert.ToInt16(mixedConfig.DeviceTypeNameC.Substring(0, 3));
                        if (deviceTypeC >= 101 && deviceTypeC <= 129)
                        {
                            mixedConfig.DeviceTypeCodeC = Convert.ToInt16(deviceTypeC - 64);
                        }
                        else
                        {
                            mixedConfig.DeviceTypeCodeC = deviceTypeC;
                        }
                    }
                }
                else
                {
                    mixedConfig.DeviceTypeCodeC = 0;
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
        
        public void DowngradeToSourceVersion()
        {
            throw new NotImplementedException();
        }
    }
}
