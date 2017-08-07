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
    //public class Device8036DBService:IDeviceDBService<DeviceInfo8036>
    //{
    //    private IDatabaseService _databaseService;
    //    private IDBFileVersionService _dbFileVersionService;

    //    public Device8036DBService(IDatabaseService databaseService)
    //    {
    //        _databaseService = databaseService;
    //    }
    //    public Device8036DBService(IDBFileVersionService dbFileVersionService)
    //    {
    //       _dbFileVersionService = dbFileVersionService;
    //    }    
    //    public bool CreateTableStructure()
    //    {
    //        //try
    //        //{
    //        //    if (DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8036").Length == 0)//数据库中不存在表DeviceInfo8036
    //        //    {
    //        //        StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8036(ID integer not null primary key autoincrement,Code varchar(8),Disable Boolean,LinkageGroup1 varchar(4),LinkageGroup2 varchar(4),AlertValue real,ForcastValue real,DelayValue integer, BuildingNo integer,ZoneNo integer,FloorNo integer,RoomNo integer,Location varchar(20),LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
    //        //        _databaseService.ExecuteBySql(sbDeviceInfoSQL);
    //        //    }                
    //        //}
    //        //catch
    //        //{
    //        //    return false;
    //        //}
    //        //return true;
    //        try
    //        {
    //            List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8036");
    //            if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8036
    //            {
    //                _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8036();
    //            }
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public DeviceInfo8036 GetDevice(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public DeviceInfo8036 GetDevice(DeviceInfo8036 device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool AddDevice(DeviceInfo8036 device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool UpdateDevice(DeviceInfo8036 device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteDevice(DeviceInfo8036 device)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public bool AddDevice(LoopModel loop)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public List<T> GetDevice<T>(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<T> GetDevice<T>(T device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool AddDevice<T>(T device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool AddDevice<T>(List<T> Devices)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool UpdateDevice<T>(T device)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool DeleteDevice<T>(T device)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    class Device8036DBServiceTest : IDeviceDBServiceTest
    {
        
        private IDBFileVersionService _dbFileVersionService;

        public Device8036DBServiceTest(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }
        public bool CreateTableStructure()
        {
            //try
            //{
            //    List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8036")));
            //    if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8036
            //    {
            //        //"Create table DeviceInfo8036(ID integer not null primary key autoincrement,Code varchar(8),Disable Boolean,LinkageGroup1 varchar(4),LinkageGroup2 varchar(4),AlertValue real,ForcastValue real,DelayValue integer, BuildingNo integer,ZoneNo integer,FloorNo integer,RoomNo integer,Location varchar(20),LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));"
            //        StringBuilder sbDeviceInfoSQL = new StringBuilder();
            //        sbDeviceInfoSQL.Append("Create table DeviceInfo8036");
            //        sbDeviceInfoSQL.Append("(ID integer not null primary key autoincrement,");
            //        sbDeviceInfoSQL.Append("Code varchar(8),");
            //        sbDeviceInfoSQL.Append("Disable Boolean,");
            //        sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
            //        sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
            //        sbDeviceInfoSQL.Append("AlertValue real,");
            //        sbDeviceInfoSQL.Append("ForcastValue real,");
            //        sbDeviceInfoSQL.Append("DelayValue integer,");
            //        sbDeviceInfoSQL.Append("BuildingNo integer,");
            //        sbDeviceInfoSQL.Append("ZoneNo integer,");
            //        sbDeviceInfoSQL.Append("FloorNo integer,");
            //        sbDeviceInfoSQL.Append("RoomNo integer,");
            //        sbDeviceInfoSQL.Append("Location varchar(20),");
            //        sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
            //        sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,");
            //        sbDeviceInfoSQL.Append("unique(Code,LoopID));");
            //        _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            //    }
            //}
            //catch
            //{
            //    return false;
            //}
            //return true;
            try
            {
                List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8036");
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8036
                {
                    _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8036();
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
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8036");                    
                    //sbDeviceInfoSQL.Append("(ID,");
                    //sbDeviceInfoSQL.Append("Code ,");
                    //sbDeviceInfoSQL.Append("Disable,");
                    //sbDeviceInfoSQL.Append("LinkageGroup1,");
                    //sbDeviceInfoSQL.Append("LinkageGroup2,");
                    //sbDeviceInfoSQL.Append("AlertValue ,");
                    //sbDeviceInfoSQL.Append("ForcastValue ,");
                    //sbDeviceInfoSQL.Append("DelayValue ,");
                    //sbDeviceInfoSQL.Append("BuildingNo ,");
                    //sbDeviceInfoSQL.Append("ZoneNo ,");
                    //sbDeviceInfoSQL.Append("FloorNo ,");
                    //sbDeviceInfoSQL.Append("RoomNo ,");
                    //sbDeviceInfoSQL.Append("Location ,");
                    //sbDeviceInfoSQL.Append("LoopID,");
                    //sbDeviceInfoSQL.Append("TypeCode");
                    //sbDeviceInfoSQL.Append(") VALUES(");
                    //sbDeviceInfoSQL.Append(device.ID + ",'");
                    //sbDeviceInfoSQL.Append(device.Code + "','");
                    //sbDeviceInfoSQL.Append(device.Disable + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    //sbDeviceInfoSQL.Append(device.AlertValue + "','");
                    //sbDeviceInfoSQL.Append(device.ForcastValue + "','");
                    //sbDeviceInfoSQL.Append(device.DelayValue + "','");
                    //sbDeviceInfoSQL.Append(device.BuildingNo + "','");
                    //sbDeviceInfoSQL.Append(device.ZoneNo + "','");
                    //sbDeviceInfoSQL.Append(device.FloorNo + "','");
                    //sbDeviceInfoSQL.Append(device.RoomNo + "','");
                    //sbDeviceInfoSQL.Append(device.Location + "','");
                    //sbDeviceInfoSQL.Append(device.LoopID + "','");
                    //sbDeviceInfoSQL.Append(device.TypeCode + "');");
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.AddDeviceForControllerType8036(device);
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
            //StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8036;");

            //return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));       
            return _dbFileVersionService.GetMaxDeviceIDForControllerType8036();
        }


        public LoopModel GetDevicesByLoop(LoopModel loop)
        {           
     
            return _dbFileVersionService.GetDevicesByLoopForControllerType8036(loop);
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {


            if (_dbFileVersionService.DeleteAllDevicesByControllerIDForControllerType8036(id) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8036 where  id  = " + id + ";");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;

            if (_dbFileVersionService.DeleteDeviceByIDForControllerType8036(id)> 0)
                return true;
            else
                return false;
        }
    }

}
