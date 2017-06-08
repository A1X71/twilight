using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/5/12 10:39:05
* FileName   : Device8021DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class Device8021DBService:IDeviceDBServiceTest
    {
        private IDatabaseService _databaseService;
        public Device8021DBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        

        public bool CreateTableStructure()
        {
            try
            {
                List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8021")));
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8007
                {
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8021(");
                    sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                    sbDeviceInfoSQL.Append("Code varchar(8),");
                    sbDeviceInfoSQL.Append("Disable Boolean,");
                    sbDeviceInfoSQL.Append("CurrentThreshold real,");
                    sbDeviceInfoSQL.Append("TemperatureThreshold real,");
                    sbDeviceInfoSQL.Append("BuildingNo integer,");
                    sbDeviceInfoSQL.Append("ZoneNo integer,");
                    sbDeviceInfoSQL.Append("FloorNo integer,");
                    sbDeviceInfoSQL.Append("RoomNo integer,");
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

        public bool AddDevice(LoopModel loop)
        {
            try
            {
                List<DeviceInfo8021> lstDevices = loop.GetDevices<DeviceInfo8021>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8021(");
                    sbDeviceInfoSQL.Append("ID,");
                    sbDeviceInfoSQL.Append("Code,");
                    sbDeviceInfoSQL.Append("Disable,");
                    sbDeviceInfoSQL.Append("CurrentThreshold,");
                    sbDeviceInfoSQL.Append("TemperatureThreshold,");
                    sbDeviceInfoSQL.Append("BuildingNo,");
                    sbDeviceInfoSQL.Append("ZoneNo,");
                    sbDeviceInfoSQL.Append("FloorNo,");
                    sbDeviceInfoSQL.Append("RoomNo,");
                    sbDeviceInfoSQL.Append("Location,");
                    sbDeviceInfoSQL.Append("LoopID,");
                    sbDeviceInfoSQL.Append("TypeCode)");
                    sbDeviceInfoSQL.Append(" VALUES(");
                    sbDeviceInfoSQL.Append(device.ID + ",'");
                    sbDeviceInfoSQL.Append(device.Code + "','");
                    sbDeviceInfoSQL.Append(device.Disable + "','");
                    sbDeviceInfoSQL.Append(device.CurrentThreshold + "','");
                    sbDeviceInfoSQL.Append(device.TemperatureThreshold + "','");
                    sbDeviceInfoSQL.Append(device.BuildingNo + "','");
                    sbDeviceInfoSQL.Append(device.ZoneNo + "','");                    
                    sbDeviceInfoSQL.Append(device.FloorNo + "','");
                    sbDeviceInfoSQL.Append(device.RoomNo + "','");
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

        public LoopModel GetDevicesByLoop(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,CurrentThreshold,TemperatureThreshold,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8021 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8021 model = new Model.DeviceInfo8021();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.CurrentThreshold = new Nullable<float>(Convert.ToSingle(dt.Rows[i]["CurrentThreshold"]));
                model.TemperatureThreshold = new Nullable<float>(Convert.ToSingle(dt.Rows[i]["TemperatureThreshold"]));
                model.BuildingNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8021>(model);
            }
            return loop;   
        }

        public bool DeleteAllDevicesByControllerID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8021 where  loopid  in (select id from loop where controllerid= " + id + ");");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }

        public bool DeleteDeviceByID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8021 where  id  = " + id + ";");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }

        public int GetMaxID()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8021;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));        
        }
    }
}
