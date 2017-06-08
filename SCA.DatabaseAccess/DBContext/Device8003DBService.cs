using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/12 8:37:34
* FileName   : Device8003DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    class Device8003DBService : IDeviceDBServiceTest
    {
        private IDatabaseService _databaseService;
        public Device8003DBService (IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public bool CreateTableStructure()
        {
            try
            {
                List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8003")));
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8007
                {
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8003(");
                    sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                    sbDeviceInfoSQL.Append("Code varchar(8),");
                    sbDeviceInfoSQL.Append("Disable Boolean,");
                    sbDeviceInfoSQL.Append("Feature integer,");
                    sbDeviceInfoSQL.Append("DelayValue integer,");
                    sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                    sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                    sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                    sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");
                    sbDeviceInfoSQL.Append("sdpKey integer,");
                    sbDeviceInfoSQL.Append("BroadcastZone varchar(8),");                    
                    sbDeviceInfoSQL.Append("ZoneNo integer,");
                    sbDeviceInfoSQL.Append("Location varchar(20),");
                    sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                    sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                    _databaseService.ExecuteBySql(sbDeviceInfoSQL);

                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool AddDevice(Model.LoopModel loop)
        {
            try
            {
                List<DeviceInfo8003> lstDevices = loop.GetDevices<DeviceInfo8003>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8003(");                    
                    sbDeviceInfoSQL.Append("ID,");
                    sbDeviceInfoSQL.Append("Code,");
                    sbDeviceInfoSQL.Append("Disable,");
                    sbDeviceInfoSQL.Append("Feature,");
                    sbDeviceInfoSQL.Append("DelayValue,");
                    sbDeviceInfoSQL.Append("SensitiveLevel,");
                    sbDeviceInfoSQL.Append("LinkageGroup1,");
                    sbDeviceInfoSQL.Append("LinkageGroup2,");
                    sbDeviceInfoSQL.Append("LinkageGroup3,");
                    sbDeviceInfoSQL.Append("sdpKey,");
                    sbDeviceInfoSQL.Append("BroadcastZone,");
                    sbDeviceInfoSQL.Append("ZoneNo,");
                    sbDeviceInfoSQL.Append("Location,");
                    sbDeviceInfoSQL.Append("LoopID,");                    
                    sbDeviceInfoSQL.Append("TypeCode)");
                    sbDeviceInfoSQL.Append(" VALUES(");
                    sbDeviceInfoSQL.Append(device.ID + ",'");
                    sbDeviceInfoSQL.Append(device.Code + "','");
                    sbDeviceInfoSQL.Append(device.Disable + "','");
                    sbDeviceInfoSQL.Append(device.Feature + "','");
                    sbDeviceInfoSQL.Append(device.DelayValue + "','");
                    sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
                    sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    sbDeviceInfoSQL.Append(device.LinkageGroup3 + "','");
                    sbDeviceInfoSQL.Append(device.sdpKey + "','");
                    sbDeviceInfoSQL.Append(device.BroadcastZone + "','");                    
                    sbDeviceInfoSQL.Append(device.ZoneNo + "','");                    
                    sbDeviceInfoSQL.Append(device.Location + "','");
                    sbDeviceInfoSQL.Append(device.LoopID + "','");
                    sbDeviceInfoSQL.Append(device.TypeCode + "');");
                    _databaseService.ExecuteBySql(sbDeviceInfoSQL);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public Model.LoopModel GetDevicesByLoop(Model.LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,LinkageGroup1,LinkageGroup2,LinkageGroup3,sdpKey,BroadcastZone,ZoneNo,Location,LoopID,TypeCode from DeviceInfo8003 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8003 model = new Model.DeviceInfo8003();
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.sdpKey = Convert.ToInt32(dt.Rows[i]["sdpKey"]);
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();                
                model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));                
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8003>(model);
            }
            return loop;  
        }

        public bool DeleteAllDevicesByControllerID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8003 where  loopid  in (select id from loop where controllerid= " + id + ");");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }

        public bool DeleteDeviceByID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8003 where  id  = " + id + ";");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }

        public int GetMaxID()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8003;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));    
        }
    }
}
