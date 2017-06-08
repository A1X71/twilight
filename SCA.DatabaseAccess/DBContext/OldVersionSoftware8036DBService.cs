using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using SCA.DatabaseAccess.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 13:38:07
* FileName   : OldVersionSoftware8036DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class OldVersionSoftware8036DBService : OldVersionSoftwareDBServiceBase,IOldVersionSoftwareDBService 
    {
        
        private IDatabaseService _databaseService;
        private int _deviceAddressLength;
        public OldVersionSoftware8036DBService(IDatabaseService databaseService)
            : base(databaseService)
        {
            _databaseService = databaseService;
        }
        public int DeviceAddressLength
        {
            get
            {
                return _deviceAddressLength;
            }
        }

        public bool GetDevicesInLoop(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            try
            {
                List<DeviceInfo8036> lstDeviceInfo = new List<DeviceInfo8036>();
                StringBuilder sbQuerySQL = new StringBuilder("select bianhao,leixing,geli,shuchu1,shuchu2,nongdu,yjnongdu,yanshi,louhao,quhao,cenghao,fangjianhao,didian from " + loop.Code);
                DataTable dtDevices = _databaseService.GetDataTableBySQL(sbQuerySQL);
                int dtRowsCount = dtDevices.Rows.Count;
                for (int j = 0; j < dtRowsCount; j++)
                {
                    DeviceInfo8036 device = new DeviceInfo8036();
                    device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                    device.TypeCode = Convert.ToInt16(dtDevices.Rows[j]["leixing"].ToString());
                    device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<Int16>();
                    device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                    device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                    device.AlertValue = dtDevices.Rows[j]["nongdu"].ToString().ToNullable<float>();
                    device.ForcastValue = dtDevices.Rows[j]["yjnongdu"].ToString().ToNullable<float>();
                    device.DelayValue = dtDevices.Rows[j]["yanshi"].ToString().ToNullable<Int16>();
                    device.BuildingNo = dtDevices.Rows[j]["louhao"].ToString().ToNullable<Int16>();
                    device.ZoneNo = dtDevices.Rows[j]["quhao"].ToString().ToNullable<Int16>();
                    device.FloorNo = dtDevices.Rows[j]["cenghao"].ToString().ToNullable<Int16>();
                    device.RoomNo = dtDevices.Rows[j]["fangjianhao"].ToString().ToNullable<Int16>();
                    device.Location = dtDevices.Rows[j]["didian"].ToString();
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8036>(device);
                    lstDeviceInfo.Add(device);
                }
                if (lstDeviceInfo.Count > 0)
                {
                    _deviceAddressLength = lstDeviceInfo[0].Code.Length;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<LinkageConfigStandard> GetStandardLinkageConfig()
        {
            List<LinkageConfigStandard> lstLinkageConfigStandard = new List<LinkageConfigStandard>();
            StringBuilder sbQuerySQL = new StringBuilder("select 输出组号,编号1,编号2,编号3,编号4,动作常数,联动组1,联动组2,联动组3 from 器件组态;");
            DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            int dtRowsCount = dt.Rows.Count;
            for (int i = 0; i < dtRowsCount; i++)
            {
                LinkageConfigStandard linkageConfigStandard = new LinkageConfigStandard();
                linkageConfigStandard.Code = dt.Rows[i]["输出组号"].ToString();
                linkageConfigStandard.DeviceNo1 = dt.Rows[i]["编号1"].ToString();
                linkageConfigStandard.DeviceNo2 = dt.Rows[i]["编号2"].ToString();
                linkageConfigStandard.DeviceNo3 = dt.Rows[i]["编号3"].ToString();
                linkageConfigStandard.DeviceNo4 = dt.Rows[i]["编号4"].ToString();
                linkageConfigStandard.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"].ToString());
                linkageConfigStandard.LinkageNo1 = dt.Rows[i]["联动组1"].ToString();
                linkageConfigStandard.LinkageNo2 = dt.Rows[i]["联动组2"].ToString();
                linkageConfigStandard.LinkageNo3 = dt.Rows[i]["联动组3"].ToString();
                lstLinkageConfigStandard.Add(linkageConfigStandard);                
            }
            return lstLinkageConfigStandard;
        }


        public List<T> GetDevicesInLoop<T>(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            throw new NotImplementedException();
        }

        public List<LinkageConfigMixed> GetMixedLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public List<LinkageConfigGeneral> GetGeneralLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public List<ManualControlBoard> GetManualControlBoard()
        {
            throw new NotImplementedException();
        }
    }
}
