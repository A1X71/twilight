using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/24 9:52:16
* FileName   : Device8036DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class Device8036DBService:IDeviceDBService<DeviceInfo8036>
    {
        private IDatabaseService _databaseService;
        public Device8036DBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool CreateTableStructure()
        {
            try
            {
                if (DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8036").Length == 0)//数据库中不存在表DeviceInfo8036
                {
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8036(ID integer not null primary key autoincrement,Code varchar(8),Disable Boolean,LinkageGroup1 varchar(4),LinkageGroup2 varchar(4),AlertValue real,ForcastValue real,DelayValue integer, BuildingNo integer,ZoneNo integer,FloorNo integer,RoomNo integer,Location varchar(20),LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                    _databaseService.ExecuteBySql(sbDeviceInfoSQL);
                }                
            }
            catch
            {
                return false;
            }
            return true;
        }

        public DeviceInfo8036 GetDevice(int id)
        {
            throw new NotImplementedException();
        }

        public DeviceInfo8036 GetDevice(DeviceInfo8036 device)
        {
            throw new NotImplementedException();
        }

        public bool AddDevice(DeviceInfo8036 device)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDevice(DeviceInfo8036 device)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDevice(DeviceInfo8036 device)
        {
            throw new NotImplementedException();
        }


        public bool AddDevice(LoopModel loop)
        {
            throw new NotImplementedException();
        }


        public List<T> GetDevice<T>(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetDevice<T>(T device)
        {
            throw new NotImplementedException();
        }

        public bool AddDevice<T>(T device)
        {
            throw new NotImplementedException();
        }

        public bool AddDevice<T>(List<T> Devices)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDevice<T>(T device)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDevice<T>(T device)
        {
            throw new NotImplementedException();
        }
    }

    public class Device8036DBServiceTest : IDeviceDBServiceTest
    {
        private IDatabaseService _databaseService;
        public Device8036DBServiceTest(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        bool IDeviceDBServiceTest.CreateTableStructure()
        {
            try
            {
                List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8036")));
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8036
                {
                    //"Create table DeviceInfo8036(ID integer not null primary key autoincrement,Code varchar(8),Disable Boolean,LinkageGroup1 varchar(4),LinkageGroup2 varchar(4),AlertValue real,ForcastValue real,DelayValue integer, BuildingNo integer,ZoneNo integer,FloorNo integer,RoomNo integer,Location varchar(20),LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));"
                    StringBuilder sbDeviceInfoSQL = new StringBuilder();
                    sbDeviceInfoSQL.Append("Create table DeviceInfo8036");
                    sbDeviceInfoSQL.Append("(ID integer not null primary key autoincrement,");
                    sbDeviceInfoSQL.Append("Code varchar(8),");
                    sbDeviceInfoSQL.Append("Disable Boolean,");
                    sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                    sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                    sbDeviceInfoSQL.Append("AlertValue real,");
                    sbDeviceInfoSQL.Append("ForcastValue real,");
                    sbDeviceInfoSQL.Append("DelayValue integer,");
                    sbDeviceInfoSQL.Append("BuildingNo integer,");
                    sbDeviceInfoSQL.Append("ZoneNo integer,");
                    sbDeviceInfoSQL.Append("FloorNo integer,");
                    sbDeviceInfoSQL.Append("RoomNo integer,");
                    sbDeviceInfoSQL.Append("Location varchar(20),");
                    sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                    sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,");
                    sbDeviceInfoSQL.Append("unique(Code,LoopID));");
                    _databaseService.ExecuteBySql(sbDeviceInfoSQL);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        bool IDeviceDBServiceTest.AddDevice(LoopModel loop)
        {
            try
            {
                List<DeviceInfo8036> lstDevices = loop.GetDevices<DeviceInfo8036>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8036");                    
                    sbDeviceInfoSQL.Append("(ID,");
                    sbDeviceInfoSQL.Append("Code ,");
                    sbDeviceInfoSQL.Append("Disable,");
                    sbDeviceInfoSQL.Append("LinkageGroup1,");
                    sbDeviceInfoSQL.Append("LinkageGroup2,");
                    sbDeviceInfoSQL.Append("AlertValue ,");
                    sbDeviceInfoSQL.Append("ForcastValue ,");
                    sbDeviceInfoSQL.Append("DelayValue ,");
                    sbDeviceInfoSQL.Append("BuildingNo ,");
                    sbDeviceInfoSQL.Append("ZoneNo ,");
                    sbDeviceInfoSQL.Append("FloorNo ,");
                    sbDeviceInfoSQL.Append("RoomNo ,");
                    sbDeviceInfoSQL.Append("Location ,");
                    sbDeviceInfoSQL.Append("LoopID,");
                    sbDeviceInfoSQL.Append("TypeCode");
                    sbDeviceInfoSQL.Append(") VALUES(");
                    sbDeviceInfoSQL.Append(device.ID + ",'");
                    sbDeviceInfoSQL.Append(device.Code + "','");
                    sbDeviceInfoSQL.Append(device.Disable + "','");
                    sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    sbDeviceInfoSQL.Append(device.AlertValue + "','");
                    sbDeviceInfoSQL.Append(device.ForcastValue + "','");
                    sbDeviceInfoSQL.Append(device.DelayValue + "','");
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

        int IDeviceDBServiceTest.GetMaxID()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8036;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));       
        }


        public LoopModel GetDevicesByLoop(LoopModel loop)
        {           
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code ,Disable,LinkageGroup1,LinkageGroup2,AlertValue ,ForcastValue ,DelayValue ,BuildingNo ,ZoneNo ,FloorNo ,RoomNo ,Location ,LoopID,TypeCode from DeviceInfo8036 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8036 model = new Model.DeviceInfo8036();
                model.ID = Convert.ToInt16(dt.Rows[i]["id"]);    
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (dt.Rows[i]["Disable"].ToString() == "" || dt.Rows[i]["Disable"].ToString() == "0")?false:true;
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.AlertValue = Convert.ToSingle(dt.Rows[i]["AlertValue"]);
                model.ForcastValue = Convert.ToSingle(dt.Rows[i]["ForcastValue"]);
                model.DelayValue =new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.BuildingNo =new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID= new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LoopID"]));
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8036>(model);
            }
            return loop;        
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8036 where  loopid  in (select id from loop where controllerid= " + id + ");");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8036 where  id  = " + id + ";");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }
    }

}
