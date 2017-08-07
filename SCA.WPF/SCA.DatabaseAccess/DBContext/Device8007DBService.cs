using System;
using System.Text;
using System.Collections.Generic;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/2 15:18:30
* FileName   : Device8007DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    //public class Device8007DBService : IDeviceDBService<DeviceInfo8007>
    //{
    //    private IDatabaseService _databaseService;
    //    private IDBFileVersionService _dbFileVersionService;
 
    //    public Device8007DBService(IDatabaseService databaseService)
    //    {
    //        _databaseService = databaseService;
    //    }
    //    public Device8007DBService(IDBFileVersionService dbFileVersionService)
    //    {
    //       _dbFileVersionService = dbFileVersionService;
    //    }   
    //    public bool CreateTableStructure()
    //    {
    //        //try
    //        //{
    //        //    if(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8007").Length == 0)//数据库中不存在表DeviceInfo8007
    //        //    { 
    //        //        StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8007("); 
    //        //        sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
    //        //        sbDeviceInfoSQL.Append("Code varchar(8),");
    //        //        sbDeviceInfoSQL.Append("Disable Boolean,");
    //        //        sbDeviceInfoSQL.Append("Feature integer,");
    //        //        sbDeviceInfoSQL.Append("SensitiveLevel integer,");
    //        //        sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
    //        //        sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
    //        //        sbDeviceInfoSQL.Append("BuildingNo integer,");
    //        //        sbDeviceInfoSQL.Append("ZoneNo integer,");
    //        //        sbDeviceInfoSQL.Append("FloorNo integer,");
    //        //        sbDeviceInfoSQL.Append("RoomNo integer,");
    //        //        sbDeviceInfoSQL.Append("Location varchar(20),");
    //        //        sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
    //        //        sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
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
    //            List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8007");
    //            if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8007
    //            {
    //                _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8007();
    //            }
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public DeviceInfo8007 GetDevice(int id)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public DeviceInfo8007 GetDevice(DeviceInfo8007 device)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool AddDevice(DeviceInfo8007 device)
    //    {
    //        throw new System.NotImplementedException();
    //    }


    //    public bool UpdateDevice(DeviceInfo8007 device)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public bool DeleteDevice(DeviceInfo8007 device)
    //    {
    //        throw new System.NotImplementedException();
    //    }


    //    public bool AddDevice(LoopModel loop)
    //    {
    //        try
    //        {
    //            List<DeviceInfo8007> lstDevices = loop.GetDevices<DeviceInfo8007>();
    //            foreach (var device in lstDevices)
    //            {
    //                device.Loop.ID = loop.ID;
    //                device.LoopID = loop.ID;
    //                //StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8007(");
    //                //sbDeviceInfoSQL.Append("ID,");
    //                //sbDeviceInfoSQL.Append("Code,");
    //                //sbDeviceInfoSQL.Append("Disable,");
    //                //sbDeviceInfoSQL.Append("Feature,");
    //                //sbDeviceInfoSQL.Append("SensitiveLevel,");
    //                //sbDeviceInfoSQL.Append("LinkageGroup1,");
    //                //sbDeviceInfoSQL.Append("LinkageGroup2,");
    //                //sbDeviceInfoSQL.Append("BuildingNo,");
    //                //sbDeviceInfoSQL.Append("ZoneNo,");
    //                //sbDeviceInfoSQL.Append("FloorNo,");
    //                //sbDeviceInfoSQL.Append("RoomNo,");
    //                //sbDeviceInfoSQL.Append("Location,");
    //                //sbDeviceInfoSQL.Append("LoopID,");
    //                //sbDeviceInfoSQL.Append("TypeCode)");
    //                //sbDeviceInfoSQL.Append(" VALUES(");
    //                //sbDeviceInfoSQL.Append(device.ID + ",");
    //                //sbDeviceInfoSQL.Append(device.Code + ",");
    //                //sbDeviceInfoSQL.Append(device.Disable + ",");
    //                //sbDeviceInfoSQL.Append(device.Feature + ",");
    //                //sbDeviceInfoSQL.Append(device.SensitiveLevel + ",");
    //                //sbDeviceInfoSQL.Append(device.LinkageGroup1 + ",");
    //                //sbDeviceInfoSQL.Append(device.LinkageGroup2 + ",");
    //                //sbDeviceInfoSQL.Append(device.BuildingNo + ",");
    //                //sbDeviceInfoSQL.Append(device.ZoneNo + ",");
    //                //sbDeviceInfoSQL.Append(device.FloorNo + ",");
    //                //sbDeviceInfoSQL.Append(device.RoomNo + ",");
    //                //sbDeviceInfoSQL.Append(device.Location + ",");
    //                //sbDeviceInfoSQL.Append(device.LoopID + ",");
    //                //sbDeviceInfoSQL.Append(device.TypeCode + ");");
    //                //sbDeviceInfoSQL.Append(");");
    //                //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
    //                _dbFileVersionService.AddDeviceForControllerType8007(device);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //        return true;

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

    class Device8007DBServiceTest : IDeviceDBServiceTest
    {
        
        private IDBFileVersionService _dbFileVersionService;

        public Device8007DBServiceTest(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }
        
        public bool CreateTableStructure()
        {
          //  try
           // {
            //    List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8007")));
            //    if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8007
            //    {
            //        StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8007(");
            //        sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
            //        sbDeviceInfoSQL.Append("Code varchar(8),");
            //        sbDeviceInfoSQL.Append("Disable Boolean,");
            //        sbDeviceInfoSQL.Append("Feature integer,");
            //        sbDeviceInfoSQL.Append("SensitiveLevel integer,");
            //        sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
            //        sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
            //        sbDeviceInfoSQL.Append("BuildingNo integer,");
            //        sbDeviceInfoSQL.Append("ZoneNo integer,");
            //        sbDeviceInfoSQL.Append("FloorNo integer,");
            //        sbDeviceInfoSQL.Append("RoomNo integer,");
            //        sbDeviceInfoSQL.Append("Location varchar(20),");
            //        sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
            //        sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
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
                List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8007");
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8007
                {
                    _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8007();
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
                List<DeviceInfo8007> lstDevices = loop.GetDevices<DeviceInfo8007>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8007(");
                    //sbDeviceInfoSQL.Append("ID,");
                    //sbDeviceInfoSQL.Append("Code,");
                    //sbDeviceInfoSQL.Append("Disable,");
                    //sbDeviceInfoSQL.Append("Feature,");
                    //sbDeviceInfoSQL.Append("SensitiveLevel,");
                    //sbDeviceInfoSQL.Append("LinkageGroup1,");
                    //sbDeviceInfoSQL.Append("LinkageGroup2,");
                    //sbDeviceInfoSQL.Append("BuildingNo,");
                    //sbDeviceInfoSQL.Append("ZoneNo,");
                    //sbDeviceInfoSQL.Append("FloorNo,");
                    //sbDeviceInfoSQL.Append("RoomNo,");
                    //sbDeviceInfoSQL.Append("Location,");
                    //sbDeviceInfoSQL.Append("LoopID,");
                    //sbDeviceInfoSQL.Append("TypeCode)");
                    //sbDeviceInfoSQL.Append(" VALUES(");
                    //sbDeviceInfoSQL.Append(device.ID + ",'");
                    //sbDeviceInfoSQL.Append(device.Code + "','");
                    //sbDeviceInfoSQL.Append(device.Disable + "','");
                    //sbDeviceInfoSQL.Append(device.Feature + "','");
                    //sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    //sbDeviceInfoSQL.Append(device.BuildingNo + "','");
                    //sbDeviceInfoSQL.Append(device.ZoneNo + "','");
                    //sbDeviceInfoSQL.Append(device.FloorNo + "','");
                    //sbDeviceInfoSQL.Append(device.RoomNo + "','");
                    //sbDeviceInfoSQL.Append(device.Location + "','");
                    //sbDeviceInfoSQL.Append(device.LoopID + "','");
                    //sbDeviceInfoSQL.Append(device.TypeCode + "');");                    
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.AddDeviceForControllerType8007(device);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public int GetMaxID()
        {
          return   _dbFileVersionService.GetMaxDeviceIDForControllerType8007();   
        }



        public LoopModel GetDevicesByLoop(LoopModel loop)
        {
            //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,SensitiveLevel,LinkageGroup1,LinkageGroup2,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8007 where LoopID=" + loop.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Model.DeviceInfo8007 model = new Model.DeviceInfo8007();
            //    model.ID =Convert.ToInt32(dt.Rows[i]["id"]);
            //    model.Code = dt.Rows[i]["Code"].ToString();
            //    model.Disable =(bool?)dt.Rows[i]["Disable"];
            //    model.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
            //    model.SensitiveLevel = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
            //    model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
            //    model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
            //    model.BuildingNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
            //    model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
            //    model.FloorNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
            //    model.RoomNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
            //    model.Location = dt.Rows[i]["Location"].ToString();
            //    model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
            //    model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
            //    model.Loop = loop;
            //    model.LoopID = loop.ID;
            //    loop.SetDevice<DeviceInfo8007>(model);
            //}
            //return loop;   
            return _dbFileVersionService.GetDevicesByLoopForControllerType8007(loop);
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8007 where  loopid  in (select id from loop where controllerid= " + id + ");");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;
            
            if (_dbFileVersionService.DeleteAllDevicesByControllerIDForControllerType8007(id) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8007 where  id  = " + id + ";");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;

            if (_dbFileVersionService.DeleteDeviceByIDForControllerType8007(id) > 0)
                return true;
            else
                return false;
        }
    }
}
