/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 185 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-28 10:42:19 +0800 (周五, 28 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using SCA.BusinessLogic;
using SCA.DatabaseAccess.DBContext;
using SCA.Interface.DatabaseAccess;
using System.Collections.ObjectModel;

namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8053:IDeviceService<DeviceInfo8053>
    {
        private short _maxDeviceAmount = 0;
        //private LoopModel _loop;
        public List<DeviceInfo8053> InitializeDevices(int deviceAmount)
        {
            List<DeviceInfo8053> lstDevInfo = new List<DeviceInfo8053>();
            for (int j = 0; j < deviceAmount; j++) //创建器件
            {
                DeviceInfo8053 devInfo = new DeviceInfo8053();
                devInfo.Code = j.ToString("#000");
                devInfo.TypeCode = 9; //此处默认值可为各个控制器进行配置。
                devInfo.Disable = false;
                lstDevInfo.Add(devInfo);
            }
            return lstDevInfo;
        } 
        public LoopModel TheLoop { get; set; }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8053().GetMaxDeviceAmountValue();
                }
                return _maxDeviceAmount;
            }
        }
        public List<DeviceInfo8053> Create(int amount)
        {
            //List<DeviceInfo8053> lstDeviceInfo8053 = new List<DeviceInfo8053>();
            //int currentMaxCode = GetMaxCode();
            //for (int i = 0; i < amount; i++)
            //{
            //    currentMaxCode++;
            //    DeviceInfo8053 dev = new DeviceInfo8053();
            //    dev.Loop = TheLoop;
            //    //需要根据器件编码指定编码位数
            //    dev.Code = currentMaxCode.ToString();
            //    lstDeviceInfo8053.Add(dev);
            //}
            //return lstDeviceInfo8053;
            List<DeviceInfo8053> lstDeviceInfo8053 = new List<DeviceInfo8053>();
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
                int deviceID = ProjectManager.GetInstance.MaxDeviceIDInController8053;
                for (int i = 0; i < amount; i++)
                {
                    currentMaxCode++;
                    deviceID++;
                    DeviceInfo8053 dev = new DeviceInfo8053();
                    dev.Loop = TheLoop;
                    //需要根据器件编码指定编码位数
                    //dev.Code = currentMaxCode.ToString();
                    dev.Code = TheLoop.Code + currentMaxCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
                    dev.ID = deviceID;
                    lstDeviceInfo8053.Add(dev);
                }
                //更新最大ID值
                BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053 = deviceID;
                foreach (var singleItem in lstDeviceInfo8053)
                {
                    Update(singleItem);
                }
                TheLoop.DeviceAmount = TheLoop.GetDevices<DeviceInfo8053>().Count;
            }
            return lstDeviceInfo8053;
        }

        public bool Update(DeviceInfo8053 deviceInfo)
        {
            try
            {
                DeviceInfo8053 result = TheLoop.GetDevices<DeviceInfo8053>().Find(
                    delegate(DeviceInfo8053 x)
                    {
                        return x.Code == deviceInfo.Code;
                    }
                    );
                if (result != null)
                {
                    result.Loop = deviceInfo.Loop;
                    result.LoopID = deviceInfo.LoopID;
                    result.MachineNo = deviceInfo.MachineNo;
                  //  result.ID = deviceInfo.ID;
                   // result.Code = deviceInfo.Code;
                    result.TypeCode = deviceInfo.TypeCode;
                    result.Disable = deviceInfo.Disable;
                    result.Feature = deviceInfo.Feature;
                    result.LinkageGroup1 = deviceInfo.LinkageGroup1;
                    result.LinkageGroup2 = deviceInfo.LinkageGroup2;
                    result.LinkageGroup3 = deviceInfo.LinkageGroup3;
                    result.DelayValue = deviceInfo.DelayValue;
                    result.BuildingNo = deviceInfo.BuildingNo;
                    result.ZoneNo = deviceInfo.ZoneNo;
                    result.FloorNo = deviceInfo.FloorNo;
                    result.RoomNo = deviceInfo.RoomNo;
                    result.Location = deviceInfo.Location;
                }
                else
                {
                    TheLoop.SetDevice<DeviceInfo8053>(deviceInfo);
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
                var result = from dev in TheLoop.GetDevices<DeviceInfo8053>() where dev.ID == id select dev;
                DeviceInfo8053 o = result.FirstOrDefault();
                if (o != null)
                {
                    TheLoop.GetDevices<DeviceInfo8053>().Remove(o);
                    TheLoop.DeviceAmount = TheLoop.GetDevices<DeviceInfo8053>().Count;
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
                    if (BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053 == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        ControllerOperation8053 controllerOperation = new ControllerOperation8053();
                        BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053 = controllerOperation.GetMaxDeviceID();
                    }
                }
                ILoopDBService loopDBService = new  SCA.DatabaseAccess.DBContext.LoopDBService(dbFileVersionService);
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
                var query = from r in TheLoop.GetDevices<DeviceInfo8053>() select r.Code;
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
                foreach (var device in TheLoop.GetDevices<DeviceInfo8053>())
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
                int deviceCount = TheLoop.GetDevices<DeviceInfo8053>().Count((d) => d.Code == deviceCode);
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
                IDeviceDBServiceTest dbService = DeviceManagerDBServiceTest.GetDeviceDBContext(ControllerType.NT8053, _dbFileVersionService);
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
                DeviceInfo8053 result = TheLoop.GetDevices<DeviceInfo8053>().Find(
                      delegate(DeviceInfo8053 x)
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
                            //需要将Disable存储为0或1                            
                            result.Disable = data[i] == "" ? false : (data[i].ToString().ToUpper() == "TRUE" ? true : false);
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
