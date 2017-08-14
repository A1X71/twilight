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
        private short _maxDeviceAmount = 0;
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
        
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8001().GetMaxDeviceAmountValue();
                }
                return _maxDeviceAmount;
            }
        }
        public List<DeviceInfo8001> Create(int amount)
        {
            List<DeviceInfo8001> lstDeviceInfo8001 = new List<DeviceInfo8001>();
            int currentMaxCode = GetMaxCode();
            if (currentMaxCode >= MaxDeviceAmount)
            {
                amount = 0;
            }
            else
            {
                if ((currentMaxCode + amount) > MaxDeviceAmount) //如果需要添加的行数将达上限，则增加剩余的行数
                {
                    amount = MaxDeviceAmount - currentMaxCode;
                }
                int deviceID = ProjectManager.GetInstance.MaxDeviceIDInController8001;
                for (int i = 0; i < amount; i++)
                {
                    currentMaxCode++;
                    deviceID++;
                    DeviceInfo8001 dev = new DeviceInfo8001();
                    dev.Loop = TheLoop;
                    //需要根据器件编码指定编码位数
                    dev.Code = TheLoop.Code + currentMaxCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
                    dev.ID = deviceID;
                    lstDeviceInfo8001.Add(dev);
                }
                //更新最大ID值
                BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001 = deviceID;
                foreach (var singleItem in lstDeviceInfo8001)
                {
                    Update(singleItem);
                }
                TheLoop.DeviceAmount = TheLoop.GetDevices<DeviceInfo8001>().Count;
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
                  //  result.ID = deviceInfo.ID;
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
                    TheLoop.DeviceAmount = TheLoop.GetDevices<DeviceInfo8001>().Count;
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
                IDBFileVersionService dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(DBFileVersionManager.CurrentDBFileVersion);
                IDeviceDBServiceTest deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(TheLoop.Controller.Type, dbFileVersionService);
                if (deviceDBService.DeleteDeviceByID(id))
                {
                    if (BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001 == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        ControllerOperation8001 controllerOperation = new ControllerOperation8001();
                        BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001 = controllerOperation.GetMaxDeviceID();
                    }
                }
                ILoopDBService loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(dbFileVersionService); //更新回路中存储的器件数量
                loopDBService.AddLoopInfo(TheLoop);
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
                IDeviceDBServiceTest dbService = DeviceManagerDBServiceTest.GetDeviceDBContext(ControllerType.NT8001, _dbFileVersionService);
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
                        //case "编码":
                        //    result.Code = data[i];
                        //    break;
                        case "器件类型":
                            result.TypeCode = data[i] == "" ? (short)0 : Convert.ToInt16(data[i]);
                            break;
                        case "特性":
                            result.Feature = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "屏蔽":                            
                            result.Disable =  data[i]==""?null:new Nullable<bool>(data[i].ToString().ToUpper() == "TRUE" ? true : false);
                            break;
                        case "灵敏度":
                            result.SensitiveLevel = data[i]==""?null:new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "输出组1":
                            result.LinkageGroup1 = data[i];
                            break;
                        case "输出组2":
                            result.LinkageGroup2 = data[i];
                            break;
                        case "输出组3":
                            result.LinkageGroup3 = data[i];
                            break;
                        case "延时":
                            result.DelayValue = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "板卡号":
                            result.BoardNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "手盘号":
                            result.SubBoardNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "手键号":
                            result.KeyNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "广播分区":
                            result.BroadcastZone = data[i];
                            break;
                        case "楼号":
                            result.BuildingNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "区号":
                            result.ZoneNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "层号":
                            result.FloorNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "房间号":
                            result.RoomNo = data[i] == "" ? null : new Nullable<short>(Convert.ToInt16(data[i]));
                            break;
                        case "安装地点":
                            result.Location = data[i];
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
