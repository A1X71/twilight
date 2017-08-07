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
* Create Date: 2017/4/7 9:39:17
* FileName   : DeviceService8007
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8007:IDeviceService<DeviceInfo8007>
    {
        private short _maxDeviceAmount = 0;
        public LoopModel TheLoop
        {
            get;
            set;
        }

        public List<DeviceInfo8007> InitializeDevices(int deviceAmount)
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
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8007().GetMaxDeviceAmountValue();
                }
                return _maxDeviceAmount;
            }
        }
        
        public List<DeviceInfo8007> Create(int amount)
        {
            List<DeviceInfo8007> lstDeviceInfo8007 = new List<DeviceInfo8007>();
            int currentMaxCode = GetMaxCode();
            if (currentMaxCode >= MaxDeviceAmount)
            {
                amount = 0;
            }
            if ((currentMaxCode + amount) > MaxDeviceAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                amount = currentMaxCode + amount - MaxDeviceAmount;
            }
            int deviceID = ProjectManager.GetInstance.MaxDeviceIDInController8007;
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                deviceID++;
                DeviceInfo8007 dev = new DeviceInfo8007();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = TheLoop.Code + currentMaxCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
                dev.ID = deviceID;
                lstDeviceInfo8007.Add(dev);
            }
            //更新最大ID值
            BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007 = deviceID;
            foreach (var singleItem in lstDeviceInfo8007)
            {
                Update(singleItem);
            }
            TheLoop.DeviceAmount = TheLoop.GetDevices<DeviceInfo8007>().Count;
            return lstDeviceInfo8007;
        }

        public bool Update(DeviceInfo8007 deviceInfo)
        {
            try
            {
                DeviceInfo8007 result = TheLoop.GetDevices<DeviceInfo8007>().Find(
                    delegate(DeviceInfo8007 x)
                    {
                        return x.Code == deviceInfo.Code;
                    }
                    );
                if (result != null)
                {
                    result.Loop = deviceInfo.Loop;
                    result.LoopID = deviceInfo.LoopID;
                  //  result.ID = deviceInfo.ID;
                    //result.Code = deviceInfo.Code;
                    // result.SimpleCode = deviceInfo.SimpleCode;
                    result.TypeCode = deviceInfo.TypeCode;
                    result.Disable = deviceInfo.Disable;
                    result.LinkageGroup1 = deviceInfo.LinkageGroup1;
                    result.LinkageGroup2 = deviceInfo.LinkageGroup2;
                    result.Feature = deviceInfo.Feature;
                    result.SensitiveLevel = deviceInfo.SensitiveLevel;                    
                    result.BuildingNo = deviceInfo.BuildingNo;
                    result.ZoneNo = deviceInfo.ZoneNo;
                    result.FloorNo = deviceInfo.FloorNo;
                    result.RoomNo = deviceInfo.RoomNo;
                    result.Location = deviceInfo.Location;
                }
                else
                {
                    TheLoop.SetDevice<DeviceInfo8007>(deviceInfo);
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
                var result = from dev in TheLoop.GetDevices<DeviceInfo8007>() where dev.ID == id select dev;
                DeviceInfo8007 o = result.FirstOrDefault();
                if (o != null)
                {
                    TheLoop.GetDevices<DeviceInfo8007>().Remove(o);
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
                    if (BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007 == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        ControllerOperation8007 controllerOperation = new ControllerOperation8007();
                        BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007 = controllerOperation.GetMaxDeviceID();
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
                var query = from r in TheLoop.GetDevices<DeviceInfo8007>() select r.Code;
                if (query != null)
                {
                    foreach (var i in query)
                    {
                        string deviceCode = i.Substring(TheLoop.Code.Length);
                        if (Convert.ToInt32(deviceCode) > result)
                        {
                            result = Convert.ToInt32(deviceCode);
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
                foreach (var device in TheLoop.GetDevices<DeviceInfo8007>())
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
                int deviceCount = TheLoop.GetDevices<DeviceInfo8007>().Count((d) => d.Code == deviceCode);
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


        public bool SaveToDB()
        {
            try
            {
                ILogRecorder logger = null;
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(this.TheLoop.Controller.Project.SavePath, logger, fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(SCA.BusinessLogic.DBFileVersionManager.CurrentDBFileVersion);
                ILoopDBService _loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(_dbFileVersionService);
                _loopDBService.AddLoopInfo(TheLoop);
                IDeviceDBServiceTest dbService = DeviceManagerDBServiceTest.GetDeviceDBContext(ControllerType.NT8007, _dbFileVersionService);
                dbService.AddDevice(TheLoop);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            try
            {
                DeviceInfo8001 result = TheLoop.GetDevices<DeviceInfo8001>().Find(
                      delegate(DeviceInfo8001 x)
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
                        case "屏蔽":
                            //需要将Disable存储为0或1
                            result.Disable = new Nullable<bool>(data[i].ToString().ToUpper() == "TRUE" ? true : false);
                            break;
                        case "灵敏度":
                            result.SensitiveLevel = new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "输出组1":
                            result.LinkageGroup1 = data[i].ToString();
                            break;
                        case "输出组2":
                            result.LinkageGroup2 = data[i].ToString();
                            break;                       
                        case "楼号":
                            result.BuildingNo = new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "区号":
                            result.ZoneNo = new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "层号":
                            result.FloorNo = new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "房间号":
                            result.RoomNo = new Nullable<short>(Convert.ToInt16(data[i]));
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
    }
}
