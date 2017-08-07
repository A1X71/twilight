using System;
using System.Text;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/12 11:12:25
* FileName   : Device8000DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    class Device8000DBService:IDeviceDBServiceTest
    {
        
        private IDBFileVersionService _dbFileVersionService;
        public Device8000DBService(IDBFileVersionService dbFileVersionService)
        {
           _dbFileVersionService = dbFileVersionService;
        }    

        public bool CreateTableStructure()
        {
            try
            {
                //List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8000")));
                List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8000");
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8000
                {
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8000(");
                    //sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                    //sbDeviceInfoSQL.Append("Code varchar(8),");
                    //sbDeviceInfoSQL.Append("Disable Boolean,");
                    //sbDeviceInfoSQL.Append("Feature integer,");
                    //sbDeviceInfoSQL.Append("DelayValue integer,");
                    //sbDeviceInfoSQL.Append("SensitiveLevel integer,");                    
                    //sbDeviceInfoSQL.Append("BroadcastZone varchar(8),");
                    //sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                    //sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                    //sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");                    
                    //sbDeviceInfoSQL.Append("ZoneNo integer,");
                    //sbDeviceInfoSQL.Append("Location varchar(40),");
                    //sbDeviceInfoSQL.Append("SDPKey varchar(6),");                    
                    //sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                    //sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8000();
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
                List<DeviceInfo8000> lstDevices = loop.GetDevices<DeviceInfo8000>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8000(");
                    //sbDeviceInfoSQL.Append("ID,");
                    //sbDeviceInfoSQL.Append("Code,");
                    //sbDeviceInfoSQL.Append("Disable,");
                    //sbDeviceInfoSQL.Append("Feature,");
                    //sbDeviceInfoSQL.Append("DelayValue,");
                    //sbDeviceInfoSQL.Append("SensitiveLevel,");
                    //sbDeviceInfoSQL.Append("BroadcastZone,");
                    //sbDeviceInfoSQL.Append("LinkageGroup1,");
                    //sbDeviceInfoSQL.Append("LinkageGroup2,");
                    //sbDeviceInfoSQL.Append("LinkageGroup3,");                    
                    //sbDeviceInfoSQL.Append("ZoneNo,");                    
                    //sbDeviceInfoSQL.Append("Location,");
                    //sbDeviceInfoSQL.Append("SDPKey,");
                    //sbDeviceInfoSQL.Append("LoopID,");
                    //sbDeviceInfoSQL.Append("TypeCode)");
                    //sbDeviceInfoSQL.Append(" VALUES(");
                    //sbDeviceInfoSQL.Append(device.ID + ",'");
                    //sbDeviceInfoSQL.Append(device.Code + "','");
                    //sbDeviceInfoSQL.Append(device.Disable + "','");
                    //sbDeviceInfoSQL.Append(device.Feature + "','");
                    //sbDeviceInfoSQL.Append(device.DelayValue + "','");
                    //sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
                    //sbDeviceInfoSQL.Append(device.BroadcastZone + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup3 + "','");                    
                    //sbDeviceInfoSQL.Append(device.ZoneNo + "','");
                    //sbDeviceInfoSQL.Append(device.Location + "','");
                    //sbDeviceInfoSQL.Append(device.sdpKey + "','");
                    //sbDeviceInfoSQL.Append(device.LoopID + "','");
                    //sbDeviceInfoSQL.Append(device.TypeCode + "');");
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.AddDeviceForControllerType8000(device);
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
            //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,ZoneNo,Location,SDPKey,LoopID,TypeCode from DeviceInfo8000 where LoopID=" + loop.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Model.DeviceInfo8000 model = new Model.DeviceInfo8000();
            //    model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
            //    model.Code = dt.Rows[i]["Code"].ToString();
            //    model.Disable = (bool?)dt.Rows[i]["Disable"];
            //    model.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
            //    model.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
            //    model.SensitiveLevel = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
            //    model.BroadcastZone = dt.Rows[i]["SensitiveLevel"].ToString();
            //    model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
            //    model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
            //    model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();                
            //    model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));                
            //    model.Location = dt.Rows[i]["Location"].ToString();
            //    model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
            //    model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
            //    model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
            //    model.Loop = loop;
            //    model.LoopID = loop.ID;
            //    loop.SetDevice<DeviceInfo8000>(model);
            //}
            //return loop; 
            return _dbFileVersionService.GetDevicesByLoopForControllerType8000(loop);
        }

        public bool DeleteAllDevicesByControllerID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8000 where  loopid  in (select id from loop where controllerid= " + id + ");");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;

            if (_dbFileVersionService.DeleteAllDevicesByControllerIDForControllerType8000(id) > 0)
                return true;
            else
                return false;
        }

        public bool DeleteDeviceByID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8000 where  id  = " + id + ";");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;
            if (_dbFileVersionService.DeleteDeviceByIDForControllerType8000(id) > 0)
                return true;
            else
                return false;
        }

        public int GetMaxID()
        {
            //StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8000;");

            //return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));        
            return _dbFileVersionService.GetMaxDeviceIDForControllerType8000();
        }
    }
}
