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
using System.Collections.ObjectModel;

namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8053:IDeviceService<DeviceInfo8053>
    {
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

        public List<DeviceInfo8053> Create(int amount)
        {
            List<DeviceInfo8053> lstDeviceInfo8053 = new List<DeviceInfo8053>();
            int currentMaxCode = GetMaxCode();
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                DeviceInfo8053 dev = new DeviceInfo8053();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = currentMaxCode.ToString();
                lstDeviceInfo8053.Add(dev);
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
                    result.ID = deviceInfo.ID;
                    result.Code = deviceInfo.Code;
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
                }
            }
            catch
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
            throw new NotImplementedException();
        }


        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            throw new NotImplementedException();
        }
    }
}
