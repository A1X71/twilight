﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 17:08:37
* FileName   : DeviceService8003
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8003 : IDeviceService<DeviceInfo8003>
    {
        public LoopModel TheLoop
        {
            get;
            set;
        }

        public List<DeviceInfo8003> InitializeDevices(int deviceAmount)
        {

            throw new NotImplementedException();
        }

        //commented by william at 2017-06-23 because of no using
        public List<DeviceInfo8003> Create(int amount)
        {
            List<DeviceInfo8003> lstDeviceInfo8003 = new List<DeviceInfo8003>();
            int currentMaxCode = GetMaxCode();
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                DeviceInfo8003 dev = new DeviceInfo8003();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = currentMaxCode.ToString();
                lstDeviceInfo8003.Add(dev);
            }
            return lstDeviceInfo8003;
        }

        public bool Update(DeviceInfo8003 deviceInfo)
        {
            try
            {
                DeviceInfo8003 result = TheLoop.GetDevices<DeviceInfo8003>().Find(
                    delegate(DeviceInfo8003 x)
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
                    TheLoop.SetDevice<DeviceInfo8003>(deviceInfo);
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
                var result = from dev in TheLoop.GetDevices<DeviceInfo8003>() where dev.ID == id select dev;
                DeviceInfo8003 o = result.FirstOrDefault();
                if (o != null)
                {
                    TheLoop.GetDevices<DeviceInfo8003>().Remove(o);
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
                    if (BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8003 == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        ControllerOperation8003 controllerOperation = new ControllerOperation8003();
                        BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8003 = controllerOperation.GetMaxDeviceID();
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
                var query = from r in TheLoop.GetDevices<DeviceInfo8003>() select r.Code;
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
                foreach (var device in TheLoop.GetDevices<DeviceInfo8003>())
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
                int deviceCount = TheLoop.GetDevices<DeviceInfo8003>().Count((d) => d.Code == deviceCode);
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
    }
}
