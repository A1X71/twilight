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
*  $Revision: 158 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-25 10:12:59 +0800 (周二, 25 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Model;

namespace SCA.DatabaseAccess.DBContext
{
    public class Device8053DBService:IDeviceDBServiceTest
    {
        private IDBFileVersionService _dbFileVersionService;
        public Device8053DBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }

        public bool CreateTableStructure()
        {
            try
            {
                //List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL("DeviceInfo8001")));
                List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8053");
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8001
                {
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8001(");
                    //sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                    //sbDeviceInfoSQL.Append("Code varchar(8),");
                    //sbDeviceInfoSQL.Append("Disable Boolean,");
                    //sbDeviceInfoSQL.Append("Feature integer,");
                    //sbDeviceInfoSQL.Append("DelayValue integer,");
                    //sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                    //sbDeviceInfoSQL.Append("BoardNo integer,");
                    //sbDeviceInfoSQL.Append("SubBoardNo integer,");
                    //sbDeviceInfoSQL.Append("KeyNo integer,");
                    //sbDeviceInfoSQL.Append("BroadcastZone varchar(8),");
                    //sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                    //sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                    //sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");
                    //sbDeviceInfoSQL.Append("BuildingNo integer,");
                    //sbDeviceInfoSQL.Append("ZoneNo integer,");
                    //sbDeviceInfoSQL.Append("FloorNo integer,");
                    //sbDeviceInfoSQL.Append("RoomNo integer,");
                    //sbDeviceInfoSQL.Append("Location varchar(40),");
                    //sbDeviceInfoSQL.Append("SDPKey varchar(6),");
                    //sbDeviceInfoSQL.Append("MCBID integer references ManualControlBoard(ID) on delete restrict deferrable initially deferred,");
                    //sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                    //sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8053();
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
                List<DeviceInfo8053> lstDevices = loop.GetDevices<DeviceInfo8053>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    //StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8001(");
                    //sbDeviceInfoSQL.Append("ID,");
                    //sbDeviceInfoSQL.Append("Code,");
                    //sbDeviceInfoSQL.Append("Disable,");
                    //sbDeviceInfoSQL.Append("Feature,");
                    //sbDeviceInfoSQL.Append("DelayValue,");
                    //sbDeviceInfoSQL.Append("SensitiveLevel,");
                    //sbDeviceInfoSQL.Append("BoardNo,");
                    //sbDeviceInfoSQL.Append("SubBoardNo,");
                    //sbDeviceInfoSQL.Append("KeyNo,");
                    //sbDeviceInfoSQL.Append("BroadcastZone,");
                    //sbDeviceInfoSQL.Append("LinkageGroup1,");
                    //sbDeviceInfoSQL.Append("LinkageGroup2,");
                    //sbDeviceInfoSQL.Append("LinkageGroup3,");
                    //sbDeviceInfoSQL.Append("BuildingNo,");
                    //sbDeviceInfoSQL.Append("ZoneNo,");
                    //sbDeviceInfoSQL.Append("FloorNo,");
                    //sbDeviceInfoSQL.Append("RoomNo,");
                    //sbDeviceInfoSQL.Append("Location,");
                    //sbDeviceInfoSQL.Append("SDPKey,");
                    //sbDeviceInfoSQL.Append("MCBID,");
                    //sbDeviceInfoSQL.Append("LoopID,");
                    //sbDeviceInfoSQL.Append("TypeCode) ");
                    //sbDeviceInfoSQL.Append(" VALUES(");
                    //sbDeviceInfoSQL.Append(device.ID + ",'");
                    //sbDeviceInfoSQL.Append(device.Code + "','");
                    //sbDeviceInfoSQL.Append(device.Disable + "','");
                    //sbDeviceInfoSQL.Append(device.Feature + "','");
                    //sbDeviceInfoSQL.Append(device.DelayValue + "','");
                    //sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
                    //sbDeviceInfoSQL.Append(device.BoardNo + "','");
                    //sbDeviceInfoSQL.Append(device.SubBoardNo + "','");
                    //sbDeviceInfoSQL.Append(device.KeyNo + "','");
                    //sbDeviceInfoSQL.Append(device.BroadcastZone + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
                    //sbDeviceInfoSQL.Append(device.LinkageGroup3 + "','");
                    //sbDeviceInfoSQL.Append(device.BuildingNo + "','");
                    //sbDeviceInfoSQL.Append(device.ZoneNo + "','");
                    //sbDeviceInfoSQL.Append(device.FloorNo + "','");
                    //sbDeviceInfoSQL.Append(device.RoomNo + "','");
                    //sbDeviceInfoSQL.Append(device.Location + "','");
                    //sbDeviceInfoSQL.Append(device.sdpKey + "','");
                    //sbDeviceInfoSQL.Append(device.MCBID + "','");
                    //sbDeviceInfoSQL.Append(device.LoopID + "','");
                    //sbDeviceInfoSQL.Append(device.TypeCode + "');");
                    //_databaseService.ExecuteBySql(sbDeviceInfoSQL);
                    _dbFileVersionService.AddDeviceForControllerType8053(device);
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
            //StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8001;");

            //return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));       
            return _dbFileVersionService.GetMaxDeviceIDForControllerType8053();
        }


        public LoopModel GetDevicesByLoop(LoopModel loop)
        {
            //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BoardNo,SubBoardNo,KeyNo,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,SDPKey,MCBID,LoopID,TypeCode from DeviceInfo8001 where LoopID=" + loop.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Model.DeviceInfo8001 model = new Model.DeviceInfo8001();
            //    model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
            //    model.Code = dt.Rows[i]["Code"].ToString();
            //    model.Disable = (bool?)dt.Rows[i]["Disable"];
            //    model.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
            //    model.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
            //    model.SensitiveLevel = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
            //    model.BoardNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BoardNo"]));
            //    model.SubBoardNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SubBoardNo"]));
            //    model.KeyNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["KeyNo"]));
            //    model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
            //    model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
            //    model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
            //    model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
            //    model.BuildingNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
            //    model.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
            //    model.FloorNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
            //    model.RoomNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
            //    model.Location = dt.Rows[i]["Location"].ToString();
            //    model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
            //    model.MCBID = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["MCBID"]));
            //    model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
            //    model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
            //    model.Loop = loop;
            //    model.LoopID = loop.ID;
            //    loop.SetDevice<DeviceInfo8053>(model);
            //}
            //return loop;   
            return _dbFileVersionService.GetDevicesByLoopForControllerType8053(loop);
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8001 where  loopid  in (select id from loop where controllerid= " + id + ");");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;

            if (_dbFileVersionService.DeleteAllDevicesByControllerIDForControllerType8053(id) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            if (_dbFileVersionService.DeleteDeviceByIDForControllerType8053(id) > 0)
                return true;
            else
                return false;
        }
    }

    public class Device8053DBServiceTest : IDeviceDBServiceTest
    {
        private IDatabaseService _databaseService;
        public Device8053DBServiceTest(IDatabaseService databaseService)
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
                    sbDeviceInfoSQL.Append("Create table DeviceInfo8053");
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
                    StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8053");                    
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
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8053;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));       
        }


        public LoopModel GetDevicesByLoop(LoopModel loop)
        {           
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code ,Disable,LinkageGroup1,LinkageGroup2,AlertValue ,ForcastValue ,DelayValue ,BuildingNo ,ZoneNo ,FloorNo ,RoomNo ,Location ,LoopID,TypeCode from DeviceInfo8036 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8053 model = new Model.DeviceInfo8053();
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
                loop.SetDevice<DeviceInfo8053>(model);
            }
            return loop;        
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8053 where  loopid  in (select id from loop where controllerid= " + id + ");");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8053 where  id  = " + id + ";");
            if (_databaseService.ExecuteBySql(sbSQL) > 0)
                return true;
            else
                return false;
        }
    }

}
