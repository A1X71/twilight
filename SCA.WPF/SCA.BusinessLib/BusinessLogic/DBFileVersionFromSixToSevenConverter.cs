using System;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.Interface.BusinessLogic;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 16:22:16
* FileName   : DBFileVersionFromSixToSeverConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    // 数据文件版本6->版本7的变化为:
    // 版本7为SQLite数据文件，存储时需要为每条数据生成ID
    public class DBFileVersionFromSixToSevenConverter : IDBFileVersionConverter
    {
        public int DBFileSourceVersion
        {
            get { return 6; }
        }

        public int DBFileDestinationVersion
        {
            get { return 7; }
        }

        public ProjectModel UpgradeToDestinationVersion(ProjectModel project)
        {
            int controllerCount = project.Controllers.Count;
            int maxControllerID = ProjectManager.GetInstance.MaxIDForController;
            int maxLoopID = ProjectManager.GetInstance.MaxIDForLoop;
            for (int i = 0; i < controllerCount; i++)
            {
                maxControllerID++;
                ControllerModel controller = project.Controllers[i];
                controller.ID = maxControllerID;
                for (int j = 0; j< controller.Loops.Count; j++)
                {
                    maxLoopID++;
                    if (controller.Loops[j].DeviceAmount != 0)
                    {
                        LoopModel loop = controller.Loops[j];
                       // loop.ID = maxLoopID;
                        GenerateDeviceID(controller.Type, ref loop);
                        UpdateDeviceInfo(controller.Type, ref loop);
                        controller.Loops[j] = loop;
                    }
                }
                for (int k = 0; k < controller.StandardConfig.Count;k++ )
                {
                    int maxStandardLinkageConfigID = ProjectManager.GetInstance.MaxIDForStandardLinkageConfig;                    
                    maxStandardLinkageConfigID++;
                    controller.StandardConfig[k].ID = maxStandardLinkageConfigID;
                    ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = maxStandardLinkageConfigID;

                }
                for (int m = 0; m < controller.MixedConfig.Count; m++)
                {
                    int maxMixedLinkageConfigID = ProjectManager.GetInstance.MaxIDForMixedLinkageConfig;                    
                    maxMixedLinkageConfigID++;
                    controller.MixedConfig[m].ID = maxMixedLinkageConfigID;
                    ProjectManager.GetInstance.MaxIDForMixedLinkageConfig = maxMixedLinkageConfigID;
                }
                for (int n = 0; n < controller.GeneralConfig.Count; n++)
                {

                    int maxGeneralLinkageConfigID = ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig;
                    maxGeneralLinkageConfigID++;
                    controller.GeneralConfig[n].ID = maxGeneralLinkageConfigID;
                    ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig = maxGeneralLinkageConfigID;
                }
                for (int  o= 0; o< controller.MixedConfig.Count; o++)               
                {
                    int maxMCBConfigID = ProjectManager.GetInstance.MaxIDForManualControlBoard;
                    maxMCBConfigID++;
                    controller.ControlBoard[o].ID = maxMCBConfigID;
                    ProjectManager.GetInstance.MaxIDForManualControlBoard = maxMCBConfigID;
                }
                project.Controllers[i] = controller;
            }
            ProjectManager.GetInstance.MaxIDForLoop = maxLoopID;
            ProjectManager.GetInstance.MaxIDForController = maxControllerID;
            return project;
        }

        private bool GenerateDeviceID(ControllerType controllerType,ref LoopModel loop)
        { 
            bool resultFlag = false;
            
            try
            {
                switch (controllerType)
                {
                    case ControllerType.NT8001:
                        {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8001;
                            List<DeviceInfo8001> lstDeviceInfo = loop.GetDevices<DeviceInfo8001>(); 
                            for (int i = 0; i < lstDeviceInfo.Count; i++)
                            { 
                                    maxDeviceID++;
                                    DeviceInfo8001 deviceInfo = lstDeviceInfo[i];
                                    deviceInfo.ID = maxDeviceID;
                                    lstDeviceInfo[i] = deviceInfo;
                            }                      
                            ProjectManager.GetInstance.MaxDeviceIDInController8001 = maxDeviceID;
                        }
                        break;
                    case ControllerType.NT8007:
                        {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8007;
                            List<DeviceInfo8007> lstDeviceInfo = loop.GetDevices<DeviceInfo8007>();
                            foreach (var device in lstDeviceInfo)
                            {
                                maxDeviceID++;                 
                                device.ID = maxDeviceID;
                            }
                            ProjectManager.GetInstance.MaxDeviceIDInController8007 = maxDeviceID;
                        }
                        break;
                    case ControllerType.NT8021:
                       {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8021;
                            List<DeviceInfo8021> lstDeviceInfo = loop.GetDevices<DeviceInfo8021>(); 
                            for (int i = 0; i < lstDeviceInfo.Count; i++)
                            { 
                                    maxDeviceID++;
                                    DeviceInfo8021 deviceInfo = lstDeviceInfo[i];
                                    deviceInfo.ID = maxDeviceID;
                                    lstDeviceInfo[i] = deviceInfo;
                            }                       
                            ProjectManager.GetInstance.MaxDeviceIDInController8021 = maxDeviceID;
                       }
                       break;
                    case ControllerType.NT8036:
                       {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8036;
                            List<DeviceInfo8036> lstDeviceInfo = loop.GetDevices<DeviceInfo8036>(); 
                            for (int i = 0; i < lstDeviceInfo.Count; i++)
                            { 
                                    maxDeviceID++;
                                    DeviceInfo8036 deviceInfo = lstDeviceInfo[i];
                                    deviceInfo.ID = maxDeviceID;
                                    lstDeviceInfo[i] = deviceInfo;
                            }                       
                            ProjectManager.GetInstance.MaxDeviceIDInController8036 = maxDeviceID;
                       }
                       break;                        
                    case ControllerType.FT8000:
                                                {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8000;
                            List<DeviceInfo8000> lstDeviceInfo = loop.GetDevices<DeviceInfo8000>(); 
                            for (int i = 0; i < lstDeviceInfo.Count; i++)
                            { 
                                    maxDeviceID++;
                                    DeviceInfo8000 deviceInfo = lstDeviceInfo[i];
                                    deviceInfo.ID = maxDeviceID;
                                    lstDeviceInfo[i] = deviceInfo;
                            }                              
                            ProjectManager.GetInstance.MaxDeviceIDInController8000 = maxDeviceID;
                        }
                        break;                        
                    case ControllerType.FT8003:
                       {
                            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8003;
                            List<DeviceInfo8003> lstDeviceInfo = loop.GetDevices<DeviceInfo8003>(); 
                            for (int i = 0; i < lstDeviceInfo.Count; i++)
                            { 
                                    maxDeviceID++;
                                    DeviceInfo8003 deviceInfo = lstDeviceInfo[i];
                                    deviceInfo.ID = maxDeviceID;
                                    lstDeviceInfo[i] = deviceInfo;
                            }                       
                            ProjectManager.GetInstance.MaxDeviceIDInController8003 = maxDeviceID;
                       }
                       break;          
                }
                resultFlag = true;
            }
            catch
            { 
                    
            }
            return resultFlag;            
        }

        private bool UpdateDeviceInfo(ControllerType controllerType, ref  LoopModel loop)
        {
            bool result = false;
            switch (controllerType)
            {
                case ControllerType.NT8001:   
                    break;
                case ControllerType.NT8007:
                    break;
                case ControllerType.NT8021:
                    break;
                case ControllerType.NT8036:
                    break;
                case ControllerType.FT8000:
                    {
                        List<DeviceInfo8000> lstDeviceInfo = loop.GetDevices<DeviceInfo8000>();
                        foreach (var device in lstDeviceInfo)
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
                case ControllerType.FT8003:
                    {
                        List<DeviceInfo8003> lstDeviceInfo = loop.GetDevices<DeviceInfo8003>();
                        foreach (var device in lstDeviceInfo)
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
            }
            return result;
        }

        public void DowngradeToSourceVersion()
        {
            throw new NotImplementedException();
        }
    }
}
