using System;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.Interface.BusinessLogic;
using SCA.DatabaseAccess.DBContext;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 10:30:32
* FileName   : DBFileVersionConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    // 数据文件版本4->版本5的变化为:
    // “文件配置表”中增加“器件长度”列
    // 所有控制器的器件长度需要默认填充至此列
    public class DBFileVersionFromFourToFiveConverter:IDBFileVersionConverter
    {
        
        public int DBFileSourceVersion
        {
            get { return 4; }
        }

        public int DBFileDestinationVersion
        {
            get { return 5; }
        }
        
        
        
        public ProjectModel UpgradeToDestinationVersion(ProjectModel project)
        {
            if (project != null)
            {
                int deviceCodeLength = 0;
                foreach (var c in project.Controllers) 
                {
                    foreach (var l in c.Loops)
                    {
                        if (l.DeviceAmount != 0)
                        {
                            deviceCodeLength = GetDeviceCodeLength(c.Type, l);
                            break;
                        }
                    }
                    //更新器件长度信息
                    if (deviceCodeLength == 0)//无器件信息，无法判定当前控制器长度，采用默认值
                    {
                        IControllerConfig config = ControllerConfigManager.GetConfigObject(c.Type);
                        c.DeviceAddressLength = config.GetDeviceCodeLength()[0];
                    }
                    else
                    {                        
                        c.DeviceAddressLength = deviceCodeLength;
                    }
                }
            }
            return project;
        }


        public void DowngradeToSourceVersion()
        {
            throw new NotImplementedException();
        }
        private int GetDeviceCodeLength(ControllerType controllerType, LoopModel loop)
        {
            int deviceCodeLength = 0;
            switch (controllerType)
            {
                case ControllerType.NT8001:
                    {

                        List<DeviceInfo8001> lstDeviceInfo = loop.GetDevices<DeviceInfo8001>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
                case ControllerType.NT8007:
                    {
                        List<DeviceInfo8007> lstDeviceInfo = loop.GetDevices<DeviceInfo8007>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
                case ControllerType.NT8021:
                    {
                        List<DeviceInfo8021> lstDeviceInfo = loop.GetDevices<DeviceInfo8021>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
                case ControllerType.NT8036:
                    {
                        List<DeviceInfo8036> lstDeviceInfo = loop.GetDevices<DeviceInfo8036>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
                case ControllerType.FT8000:
                    {
                        List<DeviceInfo8000> lstDeviceInfo = loop.GetDevices<DeviceInfo8000>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
                case ControllerType.FT8003:
                    {
                        List<DeviceInfo8003> lstDeviceInfo = loop.GetDevices<DeviceInfo8003>();
                        if (lstDeviceInfo.Count > 0)
                        {
                            deviceCodeLength = lstDeviceInfo[0].Code.Length;
                        }
                    }
                    break;
            }
            return deviceCodeLength;
        }
    }
}
