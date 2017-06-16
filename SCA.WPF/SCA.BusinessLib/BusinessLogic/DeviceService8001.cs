using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/20 16:34:39
* FileName   : DeviceService8001
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8001:IDeviceService<DeviceInfo8001>
    {
        public LoopModel TheLoop
        {
            get;
            set;
        }

        public List<DeviceInfo8001> InitializeDevices(int deviceAmount)
        {
            //List<DeviceInfo8007> lstDevInfo = new List<DeviceInfo8007>();
            //for (int j = 0; j < deviceAmount; j++) //创建器件
            //{
            //    DeviceInfo8007 devInfo = new DeviceInfo8007();
            //    devInfo.Code = j.ToString("#000");
            //    devInfo.TypeCode = 9; //此处默认值可为各个控制器进行配置。
            //    devInfo.Disable = 0;
            //    lstDevInfo.Add(devInfo);
            //}
            //return lstDevInfo;
            throw new NotImplementedException();
        }

        public List<DeviceInfo8001> Create(int amount)
        {
            List<DeviceInfo8001> lstDeviceInfo8001 = new List<DeviceInfo8001>();
            int currentMaxCode = GetMaxCode();
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                DeviceInfo8001 dev = new DeviceInfo8001();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = currentMaxCode.ToString();
                lstDeviceInfo8001.Add(dev);
            }
            return lstDeviceInfo8001;
        }

        public bool Update(DeviceInfo8001 deviceInfo)
        {
            try
            {
                DeviceInfo8001 result = TheLoop.GetDevices<DeviceInfo8001>().Find(
                    delegate(DeviceInfo8001 x)
                    {
                        return x.Code == deviceInfo.Code;
                    }
                    );
                if (result != null)
                {
                    result.Loop = deviceInfo.Loop;
                    result.LoopID = deviceInfo.LoopID;
                    //deviceInfo.MachineNo = result.MachineNo;//临时应用
                    result.ID = deviceInfo.ID;
                  //  result.Code = deviceInfo.Code;
                    //result.SimpleCode = deviceInfo.SimpleCode;
                    result.TypeCode = deviceInfo.TypeCode;
                    result.LinkageGroup1 = deviceInfo.LinkageGroup1;
                    result.LinkageGroup2 = deviceInfo.LinkageGroup2;
                    result.LinkageGroup3 = deviceInfo.LinkageGroup3;
                    result.BoardNo = deviceInfo.BoardNo;
                    result.SubBoardNo = deviceInfo.SubBoardNo;
                    result.KeyNo = deviceInfo.KeyNo;
                    result.BroadcastZone = deviceInfo.BroadcastZone;
                    result.DelayValue = deviceInfo.DelayValue;
                    result.SensitiveLevel = deviceInfo.SensitiveLevel;
                    result.Feature = deviceInfo.Feature;
                    result.Disable = deviceInfo.Disable;
                    result.BuildingNo = deviceInfo.BuildingNo;
                    result.ZoneNo = deviceInfo.ZoneNo;
                    result.FloorNo = deviceInfo.FloorNo;
                    result.RoomNo = deviceInfo.RoomNo;
                    result.Location = deviceInfo.Location;
                    result.sdpKey = deviceInfo.sdpKey;
                    result.MCBCode = deviceInfo.MCBCode;
                }
                else
                {
                    TheLoop.SetDevice<DeviceInfo8001>(deviceInfo);
                }
                this.TheLoop.IsDeviceDataDirty = true;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteBySpecifiedID(int id)
        {
            try
            {
                var result = from dev in TheLoop.GetDevices<DeviceInfo8001>() where dev.ID == id select dev;
                DeviceInfo8001 o = result.FirstOrDefault();
                if (o != null)
                {
                    TheLoop.GetDevices<DeviceInfo8001>().Remove(o);
                    DeleteDeviceFromDB(id);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool DeleteDeviceFromDB(int id)
        {
            try
            {
                IFileService _fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;
                IDatabaseService _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(TheLoop.Controller.Project.SavePath, logger, _fileService);

                IDeviceDBServiceTest deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(TheLoop.Controller.Type, _databaseService);

                deviceDBService.DeleteDeviceByID(id);


            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        private int GetMaxCode()
        {
            int result = 0;
            if (TheLoop != null)
            {
                var query = from r in TheLoop.GetDevices<DeviceInfo8001>() select r.Code;
                if (query != null)
                {
                    foreach (var i in query)
                    {
                        if (Convert.ToInt32(i) > result)
                        {
                            result = Convert.ToInt32(i);
                        }
                    }
                }
            }
            return result;
        }
        public bool IsExistSameDeviceCode()
        {
            if (TheLoop != null)
            {
                bool existFlag = false;
                foreach (var device in TheLoop.GetDevices<DeviceInfo8001>())
                {
                    existFlag = IsExistSameDeviceCode(device.Code);
                    if (existFlag)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在回路内是否存在相同的器件代码
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public bool IsExistSameDeviceCode(string deviceCode)
        {
            if (TheLoop != null)
            {
                int deviceCount=TheLoop.GetDevices<DeviceInfo8001>().Count((d) => d.Code == deviceCode);
                if (deviceCount > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else //TheLoop不应为空
            {
                return true ;
            }
        }


   


  
    }
}
