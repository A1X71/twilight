using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using System.Collections.ObjectModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/27 9:42:58
* FileName   : DeviceService8036
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class DeviceService8036:IDeviceService<DeviceInfo8036>
    {
        //private LoopModel _loop;
        public List<DeviceInfo8036> InitializeDevices(int deviceAmount)
        {
            List<DeviceInfo8036> lstDevInfo = new List<DeviceInfo8036>();
            for (int j = 0; j < deviceAmount; j++) //创建器件
            {
                DeviceInfo8036 devInfo = new DeviceInfo8036();
                devInfo.Code = j.ToString("#000");
                devInfo.TypeCode = 9; //此处默认值可为各个控制器进行配置。
                devInfo.Disable = 0;
                lstDevInfo.Add(devInfo);
            }
            return lstDevInfo;
        } 
        public LoopModel TheLoop { get; set; }

        public List<DeviceInfo8036> Create(int amount)
        {
            List<DeviceInfo8036> lstDeviceInfo8036 = new List<DeviceInfo8036>();
            int currentMaxCode = GetMaxCode();
            for (int i = 0; i < amount; i++)
            {
                currentMaxCode++;
                DeviceInfo8036 dev = new DeviceInfo8036();
                dev.Loop = TheLoop;
                //需要根据器件编码指定编码位数
                dev.Code = currentMaxCode.ToString();
                lstDeviceInfo8036.Add(dev);
            }
            return lstDeviceInfo8036;
        }

        public bool Update(DeviceInfo8036 deviceInfo)
        {
            try
            {
                DeviceInfo8036 result = TheLoop.GetDevices<DeviceInfo8036>().Find(
                    delegate(DeviceInfo8036 x)
                    {
                        return x.Code == deviceInfo.Code;
                    }
                    );
                if (result != null)
                {
                    result.Loop = deviceInfo.Loop;
                    result.LoopID = deviceInfo.LoopID;
                    result.ID = deviceInfo.ID;
                    //result.Code = deviceInfo.Code;
                   // result.SimpleCode = deviceInfo.SimpleCode;
                    result.TypeCode = deviceInfo.TypeCode;
                    result.Disable = deviceInfo.Disable;
                    result.LinkageGroup1 = deviceInfo.LinkageGroup1;
                    result.LinkageGroup2 = deviceInfo.LinkageGroup2;
                    result.AlertValue = deviceInfo.AlertValue;
                    result.ForcastValue = deviceInfo.ForcastValue;
                    result.DelayValue = deviceInfo.DelayValue;
                    result.BuildingNo = deviceInfo.BuildingNo;
                    result.ZoneNo = deviceInfo.ZoneNo;
                    result.FloorNo = deviceInfo.FloorNo;
                    result.RoomNo = deviceInfo.RoomNo;
                    result.Location = deviceInfo.Location;
                }
                else
                {
                    TheLoop.SetDevice<DeviceInfo8036>(deviceInfo);
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
                var result = from dev in TheLoop.GetDevices<DeviceInfo8036>() where dev.ID == id select dev;
                DeviceInfo8036 o = result.FirstOrDefault();
                if (o != null)
                {                    
                    TheLoop.GetDevices<DeviceInfo8036>().Remove(o);
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
                var query = from r in TheLoop.GetDevices<DeviceInfo8036>()  select r.Code;
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
    }
}
