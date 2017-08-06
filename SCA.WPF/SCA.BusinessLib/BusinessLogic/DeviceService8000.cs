using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
using SCA.Model;
using SCA.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 15:22:48
* FileName   : DeviceService8000
* Description: 8000控制器器件操作类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8000:IDeviceService<DeviceInfo8000>
    {
        public LoopModel TheLoop
        {
            get;
            set;
        }

        public List<DeviceInfo8000> InitializeDevices(int deviceAmount)
        {
      
            throw new NotImplementedException();
        }        
        public List<DeviceInfo8000> Create(int amount)
        {
            List<DeviceInfo8000> lstDeviceInfo8000 = new List<DeviceInfo8000>();
            int currentMaxCode = GetMaxCode();
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                DeviceInfo8000 dev = new DeviceInfo8000();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = currentMaxCode.ToString();

                lstDeviceInfo8000.Add(dev);
            }
            foreach (var singleItem in lstDeviceInfo8000)
            {
                Update(singleItem);
            }
            return lstDeviceInfo8000;
        }

        public bool Update(DeviceInfo8000 deviceInfo)
        {
            try
            {
                DeviceInfo8000 result = TheLoop.GetDevices<DeviceInfo8000>().Find(
                    delegate(DeviceInfo8000 x)
                    {
                        return x.Code == deviceInfo.Code;
                    }
                    );
                if (result != null)
                {
                    result.Loop = deviceInfo.Loop;
                    result.LoopID = deviceInfo.LoopID;
                   // result.ID = deviceInfo.ID;
                   // result.Code = deviceInfo.Code;
                    result.TypeCode = deviceInfo.TypeCode;
                    result.SensitiveLevel = deviceInfo.SensitiveLevel;
                    result.Disable = deviceInfo.Disable;
                    result.LinkageGroup1 = deviceInfo.LinkageGroup1;
                    result.LinkageGroup2 = deviceInfo.LinkageGroup2;
                    result.LinkageGroup3 = deviceInfo.LinkageGroup3;
                    result.DelayValue = deviceInfo.DelayValue;
                    result.sdpKey = deviceInfo.sdpKey;
                    result.ZoneNo = deviceInfo.ZoneNo;
                    result.BroadcastZone = deviceInfo.BroadcastZone;
                    result.Location = deviceInfo.Location;
                }
                else
                {
                    TheLoop.SetDevice<DeviceInfo8000>(deviceInfo);
                    TheLoop.IsDeviceDataDirty = true;
                }
            }
            catch
            {
                return false;
            }
            this.TheLoop.IsDeviceDataDirty = true;
            return true;
        }

        public bool DeleteBySpecifiedID(int id)
        {
            try
            {
                var result = from dev in TheLoop.GetDevices<DeviceInfo8000>() where dev.ID == id select dev;
                DeviceInfo8000 o = result.FirstOrDefault();
                if (o != null)
                {
                    TheLoop.GetDevices<DeviceInfo8000>().Remove(o);
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
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(TheLoop.Controller.Project.SavePath, logger, _fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(DBFileVersionManager.CurrentDBFileVersion);
                IDeviceDBServiceTest deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(TheLoop.Controller.Type, _dbFileVersionService);

                if (deviceDBService.DeleteDeviceByID(id))
                {
                    if (BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8000 == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        ControllerOperation8000 controllerOperation = new ControllerOperation8000();
                        BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8000 = controllerOperation.GetMaxDeviceID();
                    }
                }
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
                var query = from r in TheLoop.GetDevices<DeviceInfo8000>() select r.Code;
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
                foreach (var device in TheLoop.GetDevices<DeviceInfo8000>())
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
                int deviceCount = TheLoop.GetDevices<DeviceInfo8000>().Count((d) => d.Code == deviceCode);
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
                return true;
            }
        }
        /// <summary>
        /// 更新指定ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            try
            {
                DeviceInfo8000 result = TheLoop.GetDevices<DeviceInfo8000>().Find(
                      delegate(DeviceInfo8000 x)
                      {
                          return x.ID == id;
                      }
                      );
                for (int i = 0; i < columnNames.Length; i++)
                {
                    switch (columnNames[i])
                    {
                        case "编码":
                            result.Code = data[i];
                            break;
                        case "器件类型":
                            result.TypeCode = Convert.ToInt16(data[i]);
                            break;
                        case "特性":
                            result.Feature =new Nullable<short>(Convert.ToInt16( data[i]));
                            break;
                        case "屏蔽":
                            //需要将Disable存储为0或1
                            result.Disable =new Nullable<bool>(data[i].ToString().ToUpper()=="TRUE"?true:false);
                            break;
                        case "灵敏度":
                            result.SensitiveLevel =new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "输出组1":
                            result.LinkageGroup1 = data[i].ToString();
                            break;
                        case "输出组2":
                            result.LinkageGroup2 = data[i].ToString();
                            break;
                        case "输出组3":
                            result.LinkageGroup3 = data[i].ToString();
                            break;
                        case "延时":
                            result.DelayValue = new Nullable<short> (Convert.ToInt16(data[i]));
                            break;
                        case "手操号":
                            result.sdpKey = data[i].ToString();
                            break;
                        case "区号":
                            result.ZoneNo = new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "广播分区":
                            result.BroadcastZone = data[i].ToString();
                            break;
                        case "安装地点":
                            result.Location = data[i].ToString();
                            break;                      
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SaveToDB()
        {
            try
            {
                ILogRecorder logger = null;
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(this.TheLoop.Controller.Project.SavePath, logger, fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(SCA.BusinessLogic.DBFileVersionManager.CurrentDBFileVersion);
                IDeviceDBServiceTest dbService = new Device8000DBService(_dbFileVersionService);
                dbService.AddDevice(TheLoop);                
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
