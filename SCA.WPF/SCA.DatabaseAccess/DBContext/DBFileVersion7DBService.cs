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
*  $Revision: 262 $
*  $Author: william_wang $        
*  $Date: 2017-08-15 10:53:57 +0800 (周二, 15 八月 2017) $
***************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 8:34:54
* FileName   : DBFileVersion7
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class DBFileVersion7DBService : DBFileVersionBaseService,IDBFileVersionService
    {
        private IDatabaseService _databaseService;
        public string DBFilePath
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource">指定数据源</param>
        /// <param name="logger">日志服务</param>
        /// <param name="fileService">文件操作服务</param>
        public DBFileVersion7DBService(string dataSource, ILogRecorder logger, IFileService fileService)
        {
            _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(dataSource, logger, fileService);
            DBFilePath = dataSource;
        }
        public int DBFileVersion
        {
            get { return 7; }
        }
        #region 基础结构及项目信息
        public bool CreateTableForProject()
        {
            try
            {
                StringBuilder sbProjectSQL = new StringBuilder("Create table project(ID  integer not null primary key autoincrement, Name varchar(255), SaveInterval integer, SavePath varchar(255),FileVersion integer);");
                _databaseService.ExecuteBySql(sbProjectSQL);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CreateTableForControllerType()
        {
            StringBuilder sbControllerTypeSQL = new StringBuilder("Create table ControllerType(ID integer not null primary key,Name varchar(255), DeFireSystemCategoryID integer references DeFireSystemCategory(ID) on delete restrict deferrable initially deferred not null);");
            _databaseService.ExecuteBySql(sbControllerTypeSQL);
            return true;
        }

        public bool CreateTableForDeFireSystemCategory()
        {
            //throw new System.NotImplementedException();
            StringBuilder sbDeFireSystemCategorySQL = new StringBuilder("Create table DeFireSystemCatetory(ID integer not null primary key autoincrement, Name varchar(255), Memo varchar(255));");
            _databaseService.ExecuteBySql(sbDeFireSystemCategorySQL);
            return true;
        }

        public bool CreateTableForDeviceType()
        {
            StringBuilder sbDeviceTypeSQL = new StringBuilder("Create table DeviceType(Code integer not null primary key autoincrement, Name varchar(20),CustomImage blob,StandardImage blob,IsValid Boolean,MatchingController varchar(100), ProjectID integer references Project(ID) on delete restrict deferrable initially deferred not null);");
            _databaseService.ExecuteBySql(sbDeviceTypeSQL);
            return true;
        }

        public bool CreateTableForController()
        {
            StringBuilder sbControllerSQL = new StringBuilder("Create table Controller(ID integer not null primary key autoincrement, PrimaryFlag Boolean,TypeID integer references ControllerType(ID) on delete restrict deferrable initially deferred not null, DeviceAddressLength integer, Name varchar(20), PortName varchar(5),BaudRate integer,MachineNumber varchar(5), Version integer,ProtocolVersion varchar(4),Position varchar(15),ProjectID integer references Project(ID) on delete restrict deferrable initially deferred not null);");
            _databaseService.ExecuteBySql(sbControllerSQL);
            return true;
        }

        //public bool CreateTableForControllerAttachedInfo()
        //{
        //    StringBuilder sbControllerAttachedInfoSQL = new StringBuilder("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,FileVersion varchar(4),ProtocolVersion varchar(4),Position varchar(15),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null);");
        //    _databaseService.ExecuteBySql(sbControllerAttachedInfoSQL);
        //    return true;
        //}

        public bool CreateTableForLinkageConfigStandard()
        {
            StringBuilder sbLinkageConfigStandardSQL = new StringBuilder("Create table LinkageConfigStandard(ID integer not null primary key autoincrement,Code varchar(4) not null, DeviceNo1 varchar(20),DeviceNo2 varchar(20), DeviceNo3 varchar(20), DeviceNo4 varchar(20), DeviceNo5 varchar(20), DeviceNo6 varchar(20),DeviceNo7 varchar(20),DeviceNo8 varchar(20),DeviceNo9 varchar(20),DeviceNo10 varchar(20), DeviceNo11 varchar(20), DeviceNo12 varchar(20), OutputDevice1 varchar(20), OutputDevice2 varchar(20), ActionCoefficient integer, LinkageNo1 varchar(4),LinkageNo2 varchar(4),LinkageNo3 varchar(4),Memo varchar(30),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
            _databaseService.ExecuteBySql(sbLinkageConfigStandardSQL);
            return true;
        }

        public bool CreateTableForLinkageConfigGeneral()
        {
            StringBuilder sbLinkageConfigGeneralSQL = new StringBuilder("Create table LinkageConfigGeneral(ID integer not null primary key autoincrement,Code varchar(4) not null,ActionCoefficient integer, CategoryA integer, BuildingNoA integer,ZoneNoA integer, LayerNoA1 integer, LayerNoA2 integer, DeviceTypeCodeA integer references DeviceType(Code) on delete restrict deferrable initially deferred,TypeC varchar(20),MachineNoC varchar2(10),LoopNoC varchar(10),DeviceCodeC varchar(10),BuildingNoC integer,ZoneNoC integer, LayerNoC integer ,DeviceTypeCodeC integer references DeviceType(Code) on delete restrict deferrable initially deferred,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
            _databaseService.ExecuteBySql(sbLinkageConfigGeneralSQL);
            return true;
        }

        public bool CreateTableForLinkageconfigMixed()
        {
            StringBuilder sbLinkageConfigMixedSQL = new StringBuilder("Create table LinkageConfigMixed(ID integer not null primary key autoincrement,Code varchar(4) not null, ActionCoefficient integer,ActionType varchar(10), TypeA varchar(20), LoopNoA varchar(10), DeviceCodeA varchar(10),CategoryA integer, BuildingNoA integer,ZoneNoA integer, LayerNoA integer, DeviceTypeCodeA integer , TypeB varchar(20), LoopNoB varchar(10), DeviceCodeB varchar(10),CategoryB integer, BuildingNoB integer,ZoneNoB integer, LayerNoB integer, DeviceTypeCodeB integer ,TypeC varchar(20),MachineNoC varchar(10),LoopNoC varchar(10),DeviceCodeC varchar(10),BuildingNoC integer,ZoneNoC integer, LayerNoC integer ,DeviceTypeCodeC integer,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
            _databaseService.ExecuteBySql(sbLinkageConfigMixedSQL);
            return true;
        }

        public bool CreateTableForManualControlBoard()
        {
            StringBuilder sbLinkageConfigMCBSQL = new StringBuilder("Create table ManualControlBoard(ID integer not null primary key autoincrement,Code varchar(4) not null, BoardNo varchar(10),SubBoardNo varchar(10),KeyNo varchar(10), DeviceCode varchar(10), ControlType integer, LocalDevice1 varchar(20), LocalDevice2 varchar(20), LocalDevice3 varchar(20), LocalDevice4 varchar(20), BuildingNo varchar(10), AreaNo varhchar(10), FloorNo varchar(10), DeviceType integer, LinkageGroup varchar(10), NetDevice1 varchar(20), NetDevice2 varchar(20), NetDevice3 varchar(20),  NetDevice4 varchar(20), SDPKey varchar(10),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
            _databaseService.ExecuteBySql(sbLinkageConfigMCBSQL);
            return true;
        }

        public bool CreateTableForLoop()
        {
            StringBuilder sbLoopSQL = new StringBuilder("Create table Loop(ID integer not null primary key autoincrement,Code varchar(6),Name varchar(20),DeviceAmount integer,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID));");
            _databaseService.ExecuteBySql(sbLoopSQL);
            return true;
        }
        #endregion
        #region 8000控制器器件信息
        public bool CreateTableForDeviceInfoOfControllerType8000()
        {
            try
            {
                StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8000(");
                sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                sbDeviceInfoSQL.Append("Code varchar(8),");
                sbDeviceInfoSQL.Append("Disable Boolean,");
                sbDeviceInfoSQL.Append("Feature integer,");
                sbDeviceInfoSQL.Append("DelayValue integer,");
                sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                sbDeviceInfoSQL.Append("BroadcastZone varchar(8),");
                sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");
                sbDeviceInfoSQL.Append("ZoneNo integer,");
                sbDeviceInfoSQL.Append("Location varchar(40),");
                sbDeviceInfoSQL.Append("SDPKey varchar(6),");
                sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
        public bool AddDeviceForControllerType8000(Model.LoopModel loop)
        {
            throw new System.NotImplementedException();
        }
        public int AddDeviceForControllerType8000(DeviceInfo8000 device)
        {
            StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8000(");
            sbDeviceInfoSQL.Append("ID,");
            sbDeviceInfoSQL.Append("Code,");
            sbDeviceInfoSQL.Append("Disable,");
            sbDeviceInfoSQL.Append("Feature,");
            sbDeviceInfoSQL.Append("DelayValue,");
            sbDeviceInfoSQL.Append("SensitiveLevel,");
            sbDeviceInfoSQL.Append("BroadcastZone,");
            sbDeviceInfoSQL.Append("LinkageGroup1,");
            sbDeviceInfoSQL.Append("LinkageGroup2,");
            sbDeviceInfoSQL.Append("LinkageGroup3,");
            sbDeviceInfoSQL.Append("ZoneNo,");
            sbDeviceInfoSQL.Append("Location,");
            sbDeviceInfoSQL.Append("SDPKey,");
            sbDeviceInfoSQL.Append("LoopID,");
            sbDeviceInfoSQL.Append("TypeCode)");
            sbDeviceInfoSQL.Append(" VALUES(");
            sbDeviceInfoSQL.Append(device.ID + ",'");
            sbDeviceInfoSQL.Append(device.Code + "','");
            sbDeviceInfoSQL.Append(device.Disable + "','");
            sbDeviceInfoSQL.Append(device.Feature + "','");
            sbDeviceInfoSQL.Append(device.DelayValue + "','");
            sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
            sbDeviceInfoSQL.Append(device.BroadcastZone + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup3 + "','");
            sbDeviceInfoSQL.Append(device.ZoneNo + "','");
            sbDeviceInfoSQL.Append(device.Location + "','");
            sbDeviceInfoSQL.Append(device.sdpKey + "','");
            sbDeviceInfoSQL.Append(device.LoopID + "','");
            sbDeviceInfoSQL.Append(device.TypeCode + "');");
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
             
        }

        public bool GetDevicesByLoopForControllerType8000(ref LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,ZoneNo,Location,SDPKey,LoopID,TypeCode from DeviceInfo8000 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8000 model = new Model.DeviceInfo8000();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8000>(model);
            }
            //return loop;
            return true;
        }
        public LoopModel GetDevicesByLoopForControllerType8000(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,ZoneNo,Location,SDPKey,LoopID,TypeCode from DeviceInfo8000 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8000 model = new Model.DeviceInfo8000();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8000>(model);
            }
            return loop;
            
        }

        public int DeleteAllDevicesByControllerIDForControllerType8000(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8000 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);                
        }

        public int DeleteDeviceByIDForControllerType8000(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8000 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
      
        }

        public int GetMaxDeviceIDForControllerType8000()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8000;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));      
        }

        #endregion
        #region 8001控制器器件信息
        public bool CreateTableForDeviceInfoOfControllerType8001()
        {
            try
            {
                StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8001(");
                sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                sbDeviceInfoSQL.Append("Code varchar(8),");
                sbDeviceInfoSQL.Append("Disable Boolean,");
                sbDeviceInfoSQL.Append("Feature integer,");
                sbDeviceInfoSQL.Append("DelayValue integer,");
                sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                sbDeviceInfoSQL.Append("BoardNo integer,");
                sbDeviceInfoSQL.Append("SubBoardNo integer,");
                sbDeviceInfoSQL.Append("KeyNo integer,");
                sbDeviceInfoSQL.Append("BroadcastZone varchar(8),");
                sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");
                sbDeviceInfoSQL.Append("BuildingNo integer,");
                sbDeviceInfoSQL.Append("ZoneNo integer,");
                sbDeviceInfoSQL.Append("FloorNo integer,");
                sbDeviceInfoSQL.Append("RoomNo integer,");
                sbDeviceInfoSQL.Append("Location varchar(40),");
                sbDeviceInfoSQL.Append("SDPKey varchar(6),");
                sbDeviceInfoSQL.Append("MCBID integer references ManualControlBoard(ID) on delete restrict deferrable initially deferred,");
                sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool AddDeviceForControllerType8001(Model.LoopModel loop)
        {
            throw new System.NotImplementedException();
        }
        public int AddDeviceForControllerType8001(DeviceInfo8001 device)
        {
            StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8001(");
            sbDeviceInfoSQL.Append("ID,");
            sbDeviceInfoSQL.Append("Code,");
            sbDeviceInfoSQL.Append("Disable,");
            sbDeviceInfoSQL.Append("Feature,");
            sbDeviceInfoSQL.Append("DelayValue,");
            sbDeviceInfoSQL.Append("SensitiveLevel,");
            sbDeviceInfoSQL.Append("BoardNo,");
            sbDeviceInfoSQL.Append("SubBoardNo,");
            sbDeviceInfoSQL.Append("KeyNo,");
            sbDeviceInfoSQL.Append("BroadcastZone,");
            sbDeviceInfoSQL.Append("LinkageGroup1,");
            sbDeviceInfoSQL.Append("LinkageGroup2,");
            sbDeviceInfoSQL.Append("LinkageGroup3,");
            sbDeviceInfoSQL.Append("BuildingNo,");
            sbDeviceInfoSQL.Append("ZoneNo,");
            sbDeviceInfoSQL.Append("FloorNo,");
            sbDeviceInfoSQL.Append("RoomNo,");
            sbDeviceInfoSQL.Append("Location,");
            sbDeviceInfoSQL.Append("SDPKey,");
            sbDeviceInfoSQL.Append("MCBID,");
            sbDeviceInfoSQL.Append("LoopID,");
            sbDeviceInfoSQL.Append("TypeCode) ");
            sbDeviceInfoSQL.Append(" VALUES(");
            sbDeviceInfoSQL.Append(device.ID + ",'");
            sbDeviceInfoSQL.Append(device.Code + "','");
            sbDeviceInfoSQL.Append(device.Disable + "','");
            sbDeviceInfoSQL.Append(device.Feature + "','");
            sbDeviceInfoSQL.Append(device.DelayValue + "','");
            sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
            sbDeviceInfoSQL.Append(device.BoardNo + "','");
            sbDeviceInfoSQL.Append(device.SubBoardNo + "','");
            sbDeviceInfoSQL.Append(device.KeyNo + "','");
            sbDeviceInfoSQL.Append(device.BroadcastZone + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup3 + "','");
            sbDeviceInfoSQL.Append(device.BuildingNo + "','");
            sbDeviceInfoSQL.Append(device.ZoneNo + "','");
            sbDeviceInfoSQL.Append(device.FloorNo + "','");
            sbDeviceInfoSQL.Append(device.RoomNo + "','");
            sbDeviceInfoSQL.Append(device.Location + "','");
            sbDeviceInfoSQL.Append(device.sdpKey + "','");
            sbDeviceInfoSQL.Append(device.MCBID + "','");
            sbDeviceInfoSQL.Append(device.LoopID + "','");
            sbDeviceInfoSQL.Append(device.TypeCode + "');");
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }

        public LoopModel GetDevicesByLoopForControllerType8001(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BoardNo,SubBoardNo,KeyNo,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,SDPKey,MCBID,LoopID,TypeCode from DeviceInfo8001 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8001 model = new Model.DeviceInfo8001();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.BoardNo = dt.Rows[i]["BoardNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BoardNo"]));
                model.SubBoardNo = dt.Rows[i]["SubBoardNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SubBoardNo"]));
                model.KeyNo = dt.Rows[i]["KeyNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["KeyNo"]));
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.MCBID = dt.Rows[i]["MCBID"] == null ? null : new Nullable<int>(Convert.ToInt32(dt.Rows[i]["MCBID"]));
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8001>(model);
            }
            return loop; 
        }
        public bool GetDevicesByLoopForControllerType8001(ref Model.LoopModel loop, System.Collections.Generic.Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,BoardNo,SubBoardNo,KeyNo,BroadcastZone,LinkageGroup1,LinkageGroup2,LinkageGroup3,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,SDPKey,MCBID,LoopID,TypeCode from DeviceInfo8001 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8001 model = new Model.DeviceInfo8001();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.BoardNo = dt.Rows[i]["BoardNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BoardNo"]));
                model.SubBoardNo = dt.Rows[i]["SubBoardNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SubBoardNo"]));
                model.KeyNo = dt.Rows[i]["KeyNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["KeyNo"]));
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.MCBID = dt.Rows[i]["MCBID"] == null ? null : new Nullable<int>(Convert.ToInt32(dt.Rows[i]["MCBID"]));
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                if (dictDeviceMappingManualControlBoard != null)
                {
                    if (dictDeviceMappingManualControlBoard.ContainsKey(model.Code))//如果存在网络手动盘的信息定义，则设置MCBCode的值，建立与手动盘的关系
                    {
                        model.MCBCode = dictDeviceMappingManualControlBoard[model.Code];
                    }
                }
                loop.SetDevice<DeviceInfo8001>(model);
            }
            return true; 
        }

        public int DeleteAllDevicesByControllerIDForControllerType8001(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8001 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int DeleteDeviceByIDForControllerType8001(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8001 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int GetMaxDeviceIDForControllerType8001()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8001;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));      
        }
        #endregion
        #region 8003控制器器件信息

        public bool CreateTableForDeviceInfoOfControllerType8003()
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
        public bool AddDeviceForControllerType8003(Model.LoopModel loop)
        {
            throw new System.NotImplementedException();
        }
        public int AddDeviceForControllerType8003(DeviceInfo8003 device)
        {       
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
                return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }
        public LoopModel GetDevicesByLoopForControllerType8003(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,LinkageGroup1,LinkageGroup2,LinkageGroup3,sdpKey,BroadcastZone,ZoneNo,Location,LoopID,TypeCode from DeviceInfo8003 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8003 model = new Model.DeviceInfo8003();
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"].ToString();
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8003>(model);
            }
            return loop;  
        }
        public bool  GetDevicesByLoopForControllerType8003(ref LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,LinkageGroup1,LinkageGroup2,LinkageGroup3,sdpKey,BroadcastZone,ZoneNo,Location,LoopID,TypeCode from DeviceInfo8003 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8003 model = new Model.DeviceInfo8003();
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.sdpKey = dt.Rows[i]["sdpKey"].ToString();
                model.BroadcastZone = dt.Rows[i]["BroadcastZone"] == null ? null : dt.Rows[i]["BroadcastZone"].ToString();
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8003>(model);
            }
            return true;
        }
        public int DeleteAllDevicesByControllerIDForControllerType8003(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8003 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int DeleteDeviceByIDForControllerType8003(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8003 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        
        }

        public int GetMaxDeviceIDForControllerType8003()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8003;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));    
        }
        #endregion
        #region 8007控制器器件信息
        public bool CreateTableForDeviceInfoOfControllerType8007()
        {
            try
            {
                StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8007(");
                sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                sbDeviceInfoSQL.Append("Code varchar(8),");
                sbDeviceInfoSQL.Append("Disable Boolean,");
                sbDeviceInfoSQL.Append("Feature integer,");
                sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                sbDeviceInfoSQL.Append("BuildingNo integer,");
                sbDeviceInfoSQL.Append("ZoneNo integer,");
                sbDeviceInfoSQL.Append("FloorNo integer,");
                sbDeviceInfoSQL.Append("RoomNo integer,");
                sbDeviceInfoSQL.Append("Location varchar(20),");
                sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool GetControllerInfo(ref ControllerModel controllerInfo)
        {
            try
            {
                string fileName = base.GetFileName(DBFilePath);
                if (fileName != "")
                {
                    controllerInfo.Name = fileName.Length > 10 ? fileName.Substring(0, 10) : fileName;
                }
                else
                {
                    controllerInfo.Name = "OldVersion";
                }
                controllerInfo.Project = null;
                controllerInfo.PrimaryFlag = false;
                controllerInfo.LoopAddressLength = 2;
                controllerInfo.DeviceAddressLength = GetDeviceAddressLength();
                return true;
            }
            catch
            {
                return false;
            }   
        }
        //public List<ControllerModel> GetControllerInfo(ControllerType controllerType)
        //{
        //    List<ControllerModel> lstControllers = null;
        //    try
        //    {

        //        ControllerAttachedInfo controllerAttachedInfo = this.GetAttachedInfo();
        //        ControllerModel controllerInfo = null;
        //        if (controllerAttachedInfo != null)
        //        {
        //            controllerInfo = new ControllerModel();
        //            string fileName = base.GetFileName(DBFilePath);
        //            if (fileName != "")
        //            {
        //                controllerInfo.Name = fileName.Length > 10 ? fileName.Substring(0, 10) : fileName;
        //            }
        //            else
        //            {
        //                controllerInfo.Name = "OldVersion";
        //            }
        //            controllerInfo.Project = null;
        //            controllerInfo.PrimaryFlag = false;
        //            controllerInfo.LoopAddressLength = 2;
        //            controllerInfo.Type = ControllerTypeConverter(controllerAttachedInfo.ControllerType);
        //            controllerInfo.ProtocolVersion = controllerAttachedInfo.ProtocolVersion;
        //            controllerInfo.Position = controllerAttachedInfo.Position;
        //            controllerInfo.FileVersion = controllerAttachedInfo.FileVersion;

        //        }
        //        if (controllerInfo != null)
        //        {
        //            lstControllers = new List<ControllerModel>();
        //            lstControllers.Add(controllerInfo);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return lstControllers;
        //}
        public bool AddDeviceForControllerType8007(LoopModel loop)
        {
            throw new System.NotImplementedException();
        }
        public int AddDeviceForControllerType8007(DeviceInfo8007 device)
        {
            StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8007(");
            sbDeviceInfoSQL.Append("ID,");
            sbDeviceInfoSQL.Append("Code,");
            sbDeviceInfoSQL.Append("Disable,");
            sbDeviceInfoSQL.Append("Feature,");
            sbDeviceInfoSQL.Append("SensitiveLevel,");
            sbDeviceInfoSQL.Append("LinkageGroup1,");
            sbDeviceInfoSQL.Append("LinkageGroup2,");
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
            sbDeviceInfoSQL.Append(device.Feature + "','");
            sbDeviceInfoSQL.Append(device.SensitiveLevel + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup1 + "','");
            sbDeviceInfoSQL.Append(device.LinkageGroup2 + "','");
            sbDeviceInfoSQL.Append(device.BuildingNo + "','");
            sbDeviceInfoSQL.Append(device.ZoneNo + "','");
            sbDeviceInfoSQL.Append(device.FloorNo + "','");
            sbDeviceInfoSQL.Append(device.RoomNo + "','");
            sbDeviceInfoSQL.Append(device.Location + "','");
            sbDeviceInfoSQL.Append(device.LoopID + "','");
            sbDeviceInfoSQL.Append(device.TypeCode + "');");
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        public bool GetDevicesByLoopForControllerType8007(ref LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,SensitiveLevel,LinkageGroup1,LinkageGroup2,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8007 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8007 model = new Model.DeviceInfo8007();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8007>(model);
            }
            return true;   
        }
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        public LoopModel GetDevicesByLoopForControllerType8007(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,SensitiveLevel,LinkageGroup1,LinkageGroup2,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8007 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8007 model = new Model.DeviceInfo8007();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8007>(model);
            }
            return loop; 
        }
        public int DeleteAllDevicesByControllerIDForControllerType8007(int id)
        { 
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8007 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
        }
        public int DeleteDeviceByIDForControllerType8007(int id)
        { 
           StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8007 where  id  = " + id + ";");
           return _databaseService.ExecuteBySql(sbSQL);
        }
        public int GetMaxDeviceIDForControllerType8007()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8007;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));   
        }
        #endregion
        #region 8021控制器器件信息 
        public bool CreateTableForDeviceInfoOfControllerType8021()
        {
            try
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
            catch
            {
                return false;
            }
            return true;        
        }
        public bool AddDeviceForControllerType8021(LoopModel loop)
        {
            throw new System.NotImplementedException();
        }

        public int AddDeviceForControllerType8021(DeviceInfo8021 device)
        {
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
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }

        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        public bool GetDevicesByLoopForControllerType8021(ref LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,CurrentThreshold,TemperatureThreshold,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8021 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8021 model = new Model.DeviceInfo8021();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.CurrentThreshold = dt.Rows[i]["CurrentThreshold"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["CurrentThreshold"]));
                model.TemperatureThreshold = dt.Rows[i]["TemperatureThreshold"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["TemperatureThreshold"]));
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8021>(model);
            }
            return true;  
            
        }
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        public LoopModel GetDevicesByLoopForControllerType8021(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,CurrentThreshold,TemperatureThreshold,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8021 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8021 model = new Model.DeviceInfo8021();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool?)dt.Rows[i]["Disable"];
                model.CurrentThreshold = dt.Rows[i]["CurrentThreshold"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["CurrentThreshold"]));
                model.TemperatureThreshold = dt.Rows[i]["TemperatureThreshold"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["TemperatureThreshold"]));
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8021>(model);
            }
            return loop;
        }
        public int DeleteAllDevicesByControllerIDForControllerType8021(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8021 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
        }
        public int DeleteDeviceByIDForControllerType8021(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8021 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
       
        }
        public int GetMaxDeviceIDForControllerType8021()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8021;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));       
        }
        #endregion
        #region 8036控制器器件信息

        public bool CreateTableForDeviceInfoOfControllerType8036()
        {
            try
            {
                StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8036(ID integer not null primary key autoincrement,Code varchar(8),Disable Boolean,LinkageGroup1 varchar(4),LinkageGroup2 varchar(4),AlertValue real,ForcastValue real,DelayValue integer, BuildingNo integer,ZoneNo integer,FloorNo integer,RoomNo integer,Location varchar(20),LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool AddDeviceForControllerType8036(LoopModel loop)
        {
            throw new System.NotImplementedException();
        }
        public int AddDeviceForControllerType8036(DeviceInfo8036 device)
        {
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
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        public bool GetDevicesByLoopForControllerType8036(ref LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code ,Disable,LinkageGroup1,LinkageGroup2,AlertValue ,ForcastValue ,DelayValue ,BuildingNo ,ZoneNo ,FloorNo ,RoomNo ,Location ,LoopID,TypeCode from DeviceInfo8036 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8036 model = new Model.DeviceInfo8036();
                model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (dt.Rows[i]["Disable"].ToString() == "" || dt.Rows[i]["Disable"].ToString() == "0") ? false : true;
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.AlertValue = dt.Rows[i]["AlertValue"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["AlertValue"]));
                model.ForcastValue = dt.Rows[i]["ForcastValue"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["ForcastValue"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8036>(model);
            }
            return true;    
        }
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        public LoopModel GetDevicesByLoopForControllerType8036(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code ,Disable,LinkageGroup1,LinkageGroup2,AlertValue ,ForcastValue ,DelayValue ,BuildingNo ,ZoneNo ,FloorNo ,RoomNo ,Location ,LoopID,TypeCode from DeviceInfo8036 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8036 model = new Model.DeviceInfo8036();
                model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (dt.Rows[i]["Disable"].ToString() == "" || dt.Rows[i]["Disable"].ToString() == "0") ? false : true;
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.AlertValue = dt.Rows[i]["AlertValue"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["AlertValue"]));
                model.ForcastValue = dt.Rows[i]["ForcastValue"] == null ? null : new Nullable<float>(Convert.ToSingle(dt.Rows[i]["ForcastValue"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8036>(model);
            }
            return loop;    
        }
        public int DeleteAllDevicesByControllerIDForControllerType8036(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8036 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
          
        }
        public int DeleteDeviceByIDForControllerType8036(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8036 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        }
        public int GetMaxDeviceIDForControllerType8036()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8036;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL)); 
        }

        #endregion
        #region 8053控制器器件信息
        public bool CreateTableForDeviceInfoOfControllerType8053()
        {
            try
            {
                StringBuilder sbDeviceInfoSQL = new StringBuilder("Create table DeviceInfo8053(");
                sbDeviceInfoSQL.Append("ID integer not null primary key autoincrement,");
                sbDeviceInfoSQL.Append("Code varchar(8),");
                sbDeviceInfoSQL.Append("Disable Boolean,");
                sbDeviceInfoSQL.Append("Feature integer,");
                sbDeviceInfoSQL.Append("DelayValue integer,");
                sbDeviceInfoSQL.Append("SensitiveLevel integer,");
                sbDeviceInfoSQL.Append("LinkageGroup1 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup2 varchar(4),");
                sbDeviceInfoSQL.Append("LinkageGroup3 varchar(4),");
                sbDeviceInfoSQL.Append("BuildingNo integer,");
                sbDeviceInfoSQL.Append("ZoneNo integer,");
                sbDeviceInfoSQL.Append("FloorNo integer,");
                sbDeviceInfoSQL.Append("RoomNo integer,");
                sbDeviceInfoSQL.Append("Location varchar(40),");
                sbDeviceInfoSQL.Append("LoopID integer references Loop(ID) on delete restrict deferrable initially deferred not null,");
                sbDeviceInfoSQL.Append("TypeCode integer references DeviceType(Code) on delete restrict deferrable initially deferred not null,unique(Code,LoopID));");
                _databaseService.ExecuteBySql(sbDeviceInfoSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool AddDeviceForControllerType8053(Model.LoopModel loop)
        {
            throw new System.NotImplementedException();
        }

        public int AddDeviceForControllerType8053(DeviceInfo8053 device)
        {
            StringBuilder sbDeviceInfoSQL = new StringBuilder("REPLACE INTO DeviceInfo8053(");
            sbDeviceInfoSQL.Append("ID,");
            sbDeviceInfoSQL.Append("Code,");
            sbDeviceInfoSQL.Append("Disable,");
            sbDeviceInfoSQL.Append("Feature,");
            sbDeviceInfoSQL.Append("DelayValue,");
            sbDeviceInfoSQL.Append("SensitiveLevel,");
            sbDeviceInfoSQL.Append("LinkageGroup1,");
            sbDeviceInfoSQL.Append("LinkageGroup2,");
            sbDeviceInfoSQL.Append("LinkageGroup3,");
            sbDeviceInfoSQL.Append("BuildingNo,");
            sbDeviceInfoSQL.Append("ZoneNo,");
            sbDeviceInfoSQL.Append("FloorNo,");
            sbDeviceInfoSQL.Append("RoomNo,");
            sbDeviceInfoSQL.Append("Location,");
            sbDeviceInfoSQL.Append("LoopID,");
            sbDeviceInfoSQL.Append("TypeCode) ");
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
            sbDeviceInfoSQL.Append(device.BuildingNo + "','");
            sbDeviceInfoSQL.Append(device.ZoneNo + "','");
            sbDeviceInfoSQL.Append(device.FloorNo + "','");
            sbDeviceInfoSQL.Append(device.RoomNo + "','");
            sbDeviceInfoSQL.Append(device.Location + "','");
            sbDeviceInfoSQL.Append(device.LoopID + "','");
            sbDeviceInfoSQL.Append(device.TypeCode + "');");
            return _databaseService.ExecuteBySql(sbDeviceInfoSQL);
        }

        public LoopModel GetDevicesByLoopForControllerType8053(LoopModel loop)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,LinkageGroup1,LinkageGroup2,LinkageGroup3,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8053 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8053 model = new Model.DeviceInfo8053();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"]==null?null:new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"].GetType() ==typeof(System.DBNull) ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8053>(model);
            }
            return loop;
        }
        public bool GetDevicesByLoopForControllerType8053(ref Model.LoopModel loop, System.Collections.Generic.Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Disable,Feature,DelayValue,SensitiveLevel,LinkageGroup1,LinkageGroup2,LinkageGroup3,BuildingNo,ZoneNo,FloorNo,RoomNo,Location,LoopID,TypeCode from DeviceInfo8053 where LoopID=" + loop.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.DeviceInfo8053 model = new Model.DeviceInfo8053();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Disable = (bool)dt.Rows[i]["Disable"];
                model.Feature = dt.Rows[i]["Feature"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["Feature"]));
                model.DelayValue = dt.Rows[i]["DelayValue"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["DelayValue"]));
                model.SensitiveLevel = dt.Rows[i]["SensitiveLevel"].GetType() == typeof(System.DBNull) ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["SensitiveLevel"]));
                model.LinkageGroup1 = dt.Rows[i]["LinkageGroup1"].ToString();
                model.LinkageGroup2 = dt.Rows[i]["LinkageGroup2"].ToString();
                model.LinkageGroup3 = dt.Rows[i]["LinkageGroup3"].ToString();
                model.BuildingNo = dt.Rows[i]["BuildingNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["BuildingNo"]));
                model.ZoneNo = dt.Rows[i]["ZoneNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["ZoneNo"]));
                model.FloorNo = dt.Rows[i]["FloorNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["FloorNo"]));
                model.RoomNo = dt.Rows[i]["RoomNo"] == null ? null : new Nullable<short>(Convert.ToInt16(dt.Rows[i]["RoomNo"]));
                model.Location = dt.Rows[i]["Location"].ToString();
                model.LoopID = Convert.ToInt32(dt.Rows[i]["LoopID"]);
                model.TypeCode = Convert.ToInt16(dt.Rows[i]["TypeCode"]);
                model.Loop = loop;
                model.LoopID = loop.ID;
                loop.SetDevice<DeviceInfo8053>(model);
            }
            return true;
        }

        public int DeleteAllDevicesByControllerIDForControllerType8053(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8053 where  loopid  in (select id from loop where controllerid= " + id + ");");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int DeleteDeviceByIDForControllerType8053(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM DeviceInfo8053 where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int GetMaxDeviceIDForControllerType8053()
        {
            StringBuilder sbSQL = new StringBuilder("SELECT MAX(id) FROM DeviceInfo8053;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbSQL));
        }
        #endregion

        public System.Collections.Generic.List<Model.LoopModel> GetLoopsInfo()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetFileVersionAndControllerType()
        {
            throw new System.NotImplementedException();
        }
        public ControllerAttachedInfo GetAttachedInfo()
        {
            //StringBuilder sbQuerySQL = new StringBuilder("select 文件版本,控制器类型,协议版本,位置,器件长度 from 文件配置 left join 位置信息 on 文件配置.文件版本=位置信息.位置;");
            //DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            ControllerAttachedInfo controllerAttachedInfo = null;
            //if (dtFile != null)
            //{
            //    if (dtFile.Rows.Count > 0)
            //    {
            //        controllerAttachedInfo = new ControllerAttachedInfo();
            //        controllerAttachedInfo.FileVersion = dtFile.Rows[0]["文件版本"].ToString();
            //        controllerAttachedInfo.ControllerType = dtFile.Rows[0]["控制器类型"].ToString();
            //        controllerAttachedInfo.ProtocolVersion = dtFile.Rows[0]["协议版本"].ToString();
            //        controllerAttachedInfo.Position = dtFile.Rows[0]["位置"].ToString();
            //        controllerAttachedInfo.DeviceAddressLength = dtFile.Rows[0]["器件长度"].ToString();
            //    }
            //}
            return controllerAttachedInfo;
        }

   
        public System.Collections.Generic.List<ManualControlBoard> GetManualControlBoard(ControllerModel controller)
        {
            List<ManualControlBoard> lstData = new List<ManualControlBoard>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,BoardNo,SubBoardNo,KeyNo,DeviceCode,ControlType,LocalDevice1,LocalDevice2,LocalDevice3,LocalDevice4,BuildingNo,AreaNo,FloorNo,DeviceType,LinkageGroup,NetDevice1,NetDevice2,NetDevice3,NetDevice4,SDPKey from ManualControlBoard where ControllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ManualControlBoard model = new ManualControlBoard();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                if (dt.Rows[i]["Code"].ToString() != "")
                {
                    model.Code = Convert.ToInt32(dt.Rows[i]["Code"]);
                }
                if (dt.Rows[i]["BoardNo"].ToString() != "")
                {
                    model.BoardNo = Convert.ToInt32(dt.Rows[i]["BoardNo"]);
                }
                if (dt.Rows[i]["SubBoardNo"].ToString() != "")
                {
                    model.SubBoardNo = Convert.ToInt32(dt.Rows[i]["SubBoardNo"]);
                }
                if (dt.Rows[i]["KeyNo"].ToString() != "")
                {
                    model.KeyNo = Convert.ToInt32(dt.Rows[i]["KeyNo"]);
                }                  
                model.DeviceCode = dt.Rows[i]["DeviceCode"].ToString();
                model.ControlType = Convert.ToInt32(dt.Rows[i]["ControlType"]);
                model.LocalDevice1 = (string)dt.Rows[i]["LocalDevice1"];
                model.LocalDevice2 = (string)dt.Rows[i]["LocalDevice2"];
                model.LocalDevice3 = (string)dt.Rows[i]["LocalDevice3"];
                model.LocalDevice4 = (string)dt.Rows[i]["LocalDevice4"];
                model.BuildingNo = (string)dt.Rows[i]["BuildingNo"];
                model.AreaNo = (string)dt.Rows[i]["AreaNo"];
                model.FloorNo = (string)dt.Rows[i]["FloorNo"];
                model.DeviceType = Convert.ToInt16(dt.Rows[i]["DeviceType"]);
                model.LinkageGroup = (string)dt.Rows[i]["LinkageGroup"];
                model.NetDevice1 = (string)dt.Rows[i]["NetDevice1"];
                model.NetDevice2 = (string)dt.Rows[i]["NetDevice2"];
                model.NetDevice3 = (string)dt.Rows[i]["NetDevice3"];
                model.NetDevice4 = (string)dt.Rows[i]["NetDevice4"];
                model.SDPKey = dt.Rows[i]["SDPKey"].ToString();
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstData.Add(model);
            }
            return lstData;   
        }

        public List<string> GetTablesOfDB()
        {
            List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTablesNameSQL()));
            return lstTableName;
        }
        public List<string> GetTablesOfDB(string tableName)
        {
            return  (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTableNameBySpecificValueSQL(tableName)));
        }


        public bool CreateLocalDBFile()
        {
            try
            {
                _databaseService.CreateDBFile();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public int GetAmountOfControllerType()
        {
            StringBuilder sbControllerTypeSQL = new StringBuilder("Select count(*) from ControllerType;");
            return Convert.ToInt32(_databaseService.GetObjectValue(sbControllerTypeSQL));            
        }

        public bool AddControllerTypeInfo()
        {
            try
            {       
                    foreach (Model.ControllerType type in Enum.GetValues(typeof(SCA.Model.ControllerType)))
                    {
                        if (type != Model.ControllerType.NONE && type != Model.ControllerType.UNCOMPATIBLE)
                        {
                            StringBuilder sbControllerTypeSQL = new StringBuilder("Insert into ControllerType(ID,Name,DeFireSystemCategoryID) values(");
                            sbControllerTypeSQL.Append(((int)type).ToString() + ",'");
                            sbControllerTypeSQL.Append(type + "',0);");
                            _databaseService.ExecuteBySql(sbControllerTypeSQL);
                        }
                    }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetFileVersion()
        {
            int fileVersion = 0;
            StringBuilder sbQuerySQL = new StringBuilder("select fileversion from project;");
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            if (dt.Rows.Count > 0)
            {
                fileVersion = Convert.ToInt32(dt.Rows[0]["fileversion"].ToString());
            }
            return fileVersion;
        }
        public Model.ProjectModel GetProject(int id)
        {
            StringBuilder sbQueryProjectSQL = new StringBuilder("select id,name,saveinterval,savepath,fileversion from project where id=" + id);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQueryProjectSQL);
            Model.ProjectModel model = new Model.ProjectModel();
            model.ID = Convert.ToInt16(dt.Rows[0]["id"]);
            model.Name = dt.Rows[0]["name"].ToString();
            model.SavePath = dt.Rows[0]["savepath"] is System.DBNull ? null : dt.Rows[0]["savepath"].ToString();
            model.SaveInterval = dt.Rows[0]["saveinterval"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[0]["saveinterval"]);
            model.FileVersion = Convert.ToInt32( dt.Rows[0]["fileversion"].ToString());
            return model;
        }

        public int AddProject(Model.ProjectModel project)
        {
            StringBuilder sbProjectSQL;
            if (project.ID == 0) //在目前软件中，一次仅可管理一个项目信息，后续如有扩展，可将此逻辑拆分至UpdateProject中
            {
                sbProjectSQL = new StringBuilder("Insert into Project(Name,savepath,saveinterval,fileversion) values(\"");
                sbProjectSQL.Append(project.Name + "\",\"");
                sbProjectSQL.Append(project.SavePath + "\",");
                sbProjectSQL.Append(project.SaveInterval + ",");
                sbProjectSQL.Append(project.FileVersion + ");");
            }
            else
            {
                sbProjectSQL = new StringBuilder("Replace into Project(ID,Name,savepath,saveinterval,fileversion) values(\"");
                sbProjectSQL.Append(project.ID + "\",\"");
                sbProjectSQL.Append(project.Name + "\",\"");
                sbProjectSQL.Append(project.SavePath + "\",");
                sbProjectSQL.Append(project.SaveInterval + ",");
                sbProjectSQL.Append(project.FileVersion + ");");
            }
            return _databaseService.ExecuteBySql(sbProjectSQL);
        }

        public System.Data.DataTable OpenProject()
        {
            DataTable dt = null;
            try
            {
                StringBuilder sbQuerySQL = new StringBuilder();
                sbQuerySQL.Append("select project.id as projectID, project.name as projectName, Controller.ID as controllerID,Controller.name as controllername,Loop.ID as loopID,Loop.name as loopname, LinkageConfigStandard.ID as standardID,LinkageConfigStandard.ConfigCode as standardCode from ");
                sbQuerySQL.Append("project left outer join controller ");
                sbQuerySQL.Append("on ");
                sbQuerySQL.Append("project.id=Controller.projectID ");
                sbQuerySQL.Append("left outer join loop ");
                sbQuerySQL.Append("on ");
                sbQuerySQL.Append("Loop.controllerid=controller.id ");
                sbQuerySQL.Append("left OUTER join LinkageConfigStandard ");
                sbQuerySQL.Append("on ");
                sbQuerySQL.Append("LinkageConfigStandard.controllerID=Controller.id ");
                sbQuerySQL.Append("order by controllerID,loopID,standardID");
                dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public int GetMaxIDFromProject()
        {
            StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Project;");

            return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
        }

        public int AddController(Model.ControllerModel controller)
        {
            int intEffectiveRows = 0;           
            StringBuilder sbControllerSQL = new StringBuilder("Replace into Controller(ID,PrimaryFlag,TypeID,DeviceAddressLength,Name,PortName,BaudRate,MachineNumber,Version,ProjectID) values(");
            sbControllerSQL.Append(controller.ID + ",'");
            sbControllerSQL.Append(controller.PrimaryFlag + "',");//+ "',0);");
            sbControllerSQL.Append((int)controller.Type + ",");
            sbControllerSQL.Append(controller.DeviceAddressLength + ",'");
            sbControllerSQL.Append(controller.Name + "','");
            sbControllerSQL.Append(controller.PortName + "',");
            sbControllerSQL.Append(controller.BaudRate + ",'");
            sbControllerSQL.Append(controller.MachineNumber + "',");
            sbControllerSQL.Append(controller.Version + ",");
            sbControllerSQL.Append(controller.Project.ID + ")");
            intEffectiveRows = _databaseService.ExecuteBySql(sbControllerSQL);
            return intEffectiveRows;
        }

        public int DeleteController(int controllerID)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM Controller where id= " + controllerID + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int GetMaxIDFromController()
        {
            StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Controller;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
        }

        public List<Model.ControllerModel> GetControllersByProject(Model.ProjectModel project)
        {
            List<ControllerModel> lstControllers = new List<ControllerModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select id,primaryflag,typeid,deviceaddresslength,name,portname,baudrate,machinenumber,version from controller where projectid=" + project.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.ControllerModel model = new Model.ControllerModel();
                model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
                model.PrimaryFlag = (bool)dt.Rows[i]["PRIMARYFLAG"];
                model.Type = (ControllerType)Enum.ToObject(typeof(ControllerType), Convert.ToInt32(dt.Rows[i]["TYPEID"]));
                model.DeviceAddressLength = dt.Rows[i]["DEVICEADDRESSLENGTH"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[i]["DEVICEADDRESSLENGTH"]);
                model.Name = dt.Rows[i]["Name"].ToString();
                model.PortName = dt.Rows[i]["PORTNAME"].ToString();
                model.BaudRate = Convert.ToInt32(dt.Rows[i]["BAUDRATE"]);
                model.MachineNumber = dt.Rows[i]["MACHINENUMBER"].ToString();
                model.Version = Convert.ToInt16(dt.Rows[i]["VERSION"]);
                model.ProjectID = project.ID;
                model.Project = project;
                model.LoopAddressLength = 2;
                lstControllers.Add(model);
            }
            return lstControllers;            
        }
        public void Dispose()
        {
            _databaseService.Dispose();
        }
        public int DeleteLoopInfo(int loopID)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM Loop where id= " + loopID + ";");
            return _databaseService.ExecuteBySql(sbSQL);
        }

        public int DeleteLoopsByControllerID(int controllerID)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM Loop where controllerid= " + controllerID + ";");
            return _databaseService.ExecuteBySql(sbSQL);                   
        }

        public int AddLoopInfo(LoopModel loop)
        {
            StringBuilder sbLoopSQL;
            //此处未显示加入ID,由SQLite自动生成
            sbLoopSQL = new StringBuilder("Replace into Loop(ID,Code,Name,DeviceAmount,controllerID) values(");
            sbLoopSQL.Append(loop.ID + ",'");
            sbLoopSQL.Append(loop.Code + "','");
            sbLoopSQL.Append(loop.Name + "',");
            sbLoopSQL.Append(loop.DeviceAmount + ",");
            sbLoopSQL.Append(loop.ControllerID + ");");
            return _databaseService.ExecuteBySql(sbLoopSQL);
        }

        public int GetMaxIDFromLoop()
        {
            StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Loop;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
        }

        public List<LoopModel> GetLoopsByController(ControllerModel controller)
        {
            List<LoopModel> lstLoops = new List<LoopModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Name,DeviceAmount from Loop where controllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model.LoopModel model = new Model.LoopModel();
                model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.Name = dt.Rows[i]["Name"].ToString();
                model.DeviceAmount = Convert.ToInt16(dt.Rows[i]["DeviceAmount"]);
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstLoops.Add(model);
            }
            return lstLoops;     
        }
        public int UpdateMatchingController(ControllerType controllerType, string matchingType)
        {     
            int intEffectiveRows = 0;     
            StringBuilder sbDeviceTypeSQL = new StringBuilder("Select Code,Name,IsValid,ProjectID, MatchingController from DeviceType where Code in (" + matchingType + ");");
            List<DeviceType> lstDeviceType = (List<DeviceType>)_databaseService.GetDataListBySQL<DeviceType>(sbDeviceTypeSQL);
            foreach (DeviceType devType in lstDeviceType)
            {
                devType.MatchingController = devType.MatchingController == null ? "" : devType.MatchingController;
                //如果MatchingController中不包含当前的控制器，则更新字段
                if (!devType.MatchingController.Contains(controllerType.ToString()))
                {
                    devType.MatchingController = devType.MatchingController == "" ? devType.MatchingController : devType.MatchingController + ",";
                    devType.MatchingController = devType.MatchingController + controllerType.ToString();
                    sbDeviceTypeSQL = new StringBuilder("Update DeviceType set MatchingController='" + devType.MatchingController + "' where Code=" + devType.Code + ";");
                    intEffectiveRows =  _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                }
            }
            return intEffectiveRows;
        }

        public int AddDeviceTypeInfo(List<DeviceType> lstDeviceType)
        {
            int intEffectiveRows = 0;     
            foreach (SCA.Model.DeviceType devType in lstDeviceType)
            {
                StringBuilder sbDeviceTypeSQL = new StringBuilder("Insert into DeviceType(Code,Name,IsValid,ProjectID) values(");
                sbDeviceTypeSQL.Append(devType.Code + ",'");
                sbDeviceTypeSQL.Append(devType.Name + "','");
                sbDeviceTypeSQL.Append(devType.IsValid + "',");
                sbDeviceTypeSQL.Append(devType.ProjectID + ")");
               intEffectiveRows = _databaseService.ExecuteBySql(sbDeviceTypeSQL);
            }
            return intEffectiveRows;
            
        }

        public int GetAmountOfDeviceType()
        {
            StringBuilder sbControllerTypeSQL = new StringBuilder("Select count(*) from DeviceType;");
            return Convert.ToInt16(_databaseService.GetObjectValue(sbControllerTypeSQL));
        }


        public int AddStandardLinkageConfigInfo(LinkageConfigStandard linkageConfigStandard)
        {
            StringBuilder sbSQL = new StringBuilder("REPLACE INTO LinkageConfigStandard(ID,Code,DeviceNo1,DeviceNo2 , DeviceNo3 , DeviceNo4, DeviceNo5,DeviceNo6,DeviceNo7,DeviceNo8,DeviceNo9,DeviceNo10, DeviceNo11, DeviceNo12, OutputDevice1, OutputDevice2, ActionCoefficient , LinkageNo1 ,LinkageNo2 ,LinkageNo3,Memo,controllerID) ");
            sbSQL.Append("VALUES(");
            sbSQL.Append(linkageConfigStandard.ID + ",'");
            sbSQL.Append(linkageConfigStandard.Code + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo1 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo2 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo3 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo4 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo5 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo6 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo7 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo8 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo9 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo10 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo11 + "','");
            sbSQL.Append(linkageConfigStandard.DeviceNo12 + "','");
            sbSQL.Append(linkageConfigStandard.OutputDevice1 + "','");
            sbSQL.Append(linkageConfigStandard.OutputDevice2 + "','");
            sbSQL.Append(linkageConfigStandard.ActionCoefficient + "','");
            sbSQL.Append(linkageConfigStandard.LinkageNo1 + "','");
            sbSQL.Append(linkageConfigStandard.LinkageNo2 + "','");
            sbSQL.Append(linkageConfigStandard.LinkageNo3 + "','");
            sbSQL.Append(linkageConfigStandard.Memo+ "',");
            sbSQL.Append(linkageConfigStandard.ControllerID + ");");

            return  _databaseService.ExecuteBySql(sbSQL);
        }

        public int AddMixedLinkageConfigInfo(LinkageConfigMixed linkageConfigMixed)
        {
            StringBuilder sbSQL = new StringBuilder("REPLACE INTO  LinkageConfigMixed(ID,Code, ActionCoefficient,ActionType, TypeA, LoopNoA, DeviceCodeA, CategoryA, BuildingNoA,ZoneNoA , LayerNoA , DeviceTypeCodeA,TypeB,LoopNoB,DeviceCodeB,CategoryB,BuildingNoB ,ZoneNoB , LayerNoB , DeviceTypeCodeB  ,TypeC ,MachineNoC,LoopNoC ,DeviceCodeC ,BuildingNoC ,ZoneNoC , LayerNoC  ,DeviceTypeCodeC  ,controllerID)");
            sbSQL.Append(" VALUES(");
            sbSQL.Append(linkageConfigMixed.ID + ",'");
            sbSQL.Append(linkageConfigMixed.Code + "','");
            sbSQL.Append(linkageConfigMixed.ActionCoefficient + "','");
            sbSQL.Append((int)linkageConfigMixed.ActionType + "','");
            sbSQL.Append((int)linkageConfigMixed.TypeA + "','");
            sbSQL.Append(linkageConfigMixed.LoopNoA + "','");
            sbSQL.Append(linkageConfigMixed.DeviceTypeCodeA + "',");
            sbSQL.Append(linkageConfigMixed.CategoryA + ",'");
            sbSQL.Append(linkageConfigMixed.BuildingNoA + "','");
            sbSQL.Append(linkageConfigMixed.ZoneNoA + "','");
            sbSQL.Append(linkageConfigMixed.LayerNoA + "','");
            sbSQL.Append(linkageConfigMixed.DeviceTypeCodeA + "','");
            sbSQL.Append((int)linkageConfigMixed.TypeB + "','");
            sbSQL.Append(linkageConfigMixed.LoopNoB + "','");
            sbSQL.Append(linkageConfigMixed.DeviceCodeB + "',");
            sbSQL.Append(linkageConfigMixed.CategoryB + ",'");
            sbSQL.Append(linkageConfigMixed.BuildingNoB + "','");
            sbSQL.Append(linkageConfigMixed.ZoneNoB + "','");
            sbSQL.Append(linkageConfigMixed.LayerNoB + "','");
            sbSQL.Append(linkageConfigMixed.DeviceTypeCodeB + "','");
            sbSQL.Append((int)linkageConfigMixed.TypeC + "','");
            sbSQL.Append(linkageConfigMixed.MachineNoC + "','");
            sbSQL.Append(linkageConfigMixed.LoopNoC + "','");
            sbSQL.Append(linkageConfigMixed.DeviceCodeC + "','");
            sbSQL.Append(linkageConfigMixed.BuildingNoC + "','");
            sbSQL.Append(linkageConfigMixed.ZoneNoC + "','");
            sbSQL.Append(linkageConfigMixed.LayerNoC + "','");
            sbSQL.Append(linkageConfigMixed.DeviceTypeCodeC + "',");
            sbSQL.Append(linkageConfigMixed.ControllerID + ");");
            return  _databaseService.ExecuteBySql(sbSQL);
        }

        public int AddGeneralLinkageConfigInfo(LinkageConfigGeneral linkageConfigGeneral)
        {
            StringBuilder sbSQL = new StringBuilder("REPLACE INTO  LinkageConfigGeneral(ID,Code,ActionCoefficient,CategoryA,BuildingNoA,ZoneNoA, LayerNoA1 , LayerNoA2 , DeviceTypeCodeA ,TypeC,MachineNoC ,LoopNoC,DeviceCodeC ,BuildingNoC,ZoneNoC, LayerNoC ,DeviceTypeCodeC ,controllerID)");
            sbSQL.Append(" VALUES(");
            sbSQL.Append(linkageConfigGeneral.ID + ",'");
            sbSQL.Append(linkageConfigGeneral.Code + "','");
            sbSQL.Append(linkageConfigGeneral.ActionCoefficient + "',");
            sbSQL.Append(linkageConfigGeneral.CategoryA + ",'");
            sbSQL.Append(linkageConfigGeneral.BuildingNoA + "','");
            sbSQL.Append(linkageConfigGeneral.ZoneNoA + "','");
            sbSQL.Append(linkageConfigGeneral.LayerNoA1 + "','");
            sbSQL.Append(linkageConfigGeneral.LayerNoA2 + "','");
            sbSQL.Append(linkageConfigGeneral.DeviceTypeCodeA + "','");
            sbSQL.Append((int)linkageConfigGeneral.TypeC + "','");
            sbSQL.Append(linkageConfigGeneral.MachineNoC + "','");
            sbSQL.Append(linkageConfigGeneral.LoopNoC + "','");
            sbSQL.Append(linkageConfigGeneral.DeviceCodeC + "','");
            sbSQL.Append(linkageConfigGeneral.BuildingNoC + "','");
            sbSQL.Append(linkageConfigGeneral.ZoneNoC + "','");
            sbSQL.Append(linkageConfigGeneral.LayerNoC + "','");
            sbSQL.Append(linkageConfigGeneral.DeviceTypeCodeC + "',");
            sbSQL.Append(linkageConfigGeneral.ControllerID + ");");
            return  _databaseService.ExecuteBySql(sbSQL);
        }

        public int AddManualControlBoardInfo(ManualControlBoard manualControlBoard)
        {
            StringBuilder sbSQL = new StringBuilder("REPLACE INTO ManualControlBoard(ID,Code, BoardNo ,SubBoardNo ,KeyNo, DeviceCode,ControlType,LocalDevice1,LocalDevice2,LocalDevice3,LocalDevice4,BuildingNo,AreaNo,FloorNo,DeviceType,LinkageGroup,NetDevice1,NetDevice2,NetDevice3,NetDevice4, SDPKey ,controllerID) ");
            sbSQL.Append("VALUES(");
            sbSQL.Append(manualControlBoard.ID + ",");
            sbSQL.Append(manualControlBoard.Code + ",'");
            sbSQL.Append(manualControlBoard.BoardNo + "','");
            sbSQL.Append(manualControlBoard.SubBoardNo + "','");
            sbSQL.Append(manualControlBoard.KeyNo + "','");
            sbSQL.Append(manualControlBoard.DeviceCode + "',");
            sbSQL.Append(manualControlBoard.ControlType + ",'");
            sbSQL.Append(manualControlBoard.LocalDevice1 + "','");
            sbSQL.Append(manualControlBoard.LocalDevice2 + "','");
            sbSQL.Append(manualControlBoard.LocalDevice3 + "','");
            sbSQL.Append(manualControlBoard.LocalDevice4 + "','");
            sbSQL.Append(manualControlBoard.BuildingNo + "','");
            sbSQL.Append(manualControlBoard.AreaNo + "','");
            sbSQL.Append(manualControlBoard.FloorNo + "',");
            sbSQL.Append(manualControlBoard.DeviceType + ",'");
            sbSQL.Append(manualControlBoard.LinkageGroup + "','");
            sbSQL.Append(manualControlBoard.NetDevice1 + "','");
            sbSQL.Append(manualControlBoard.NetDevice2 + "','");
            sbSQL.Append(manualControlBoard.NetDevice3 + "','");
            sbSQL.Append(manualControlBoard.NetDevice4 + "','");
            sbSQL.Append(manualControlBoard.SDPKey + "','");
            sbSQL.Append(manualControlBoard.ControllerID + "');");
            return _databaseService.ExecuteBySql(sbSQL);
        }


        public List<LinkageConfigStandard> GetStandardLinkageConfig(ControllerModel controller)
        {
            List<LinkageConfigStandard> lstData = new List<LinkageConfigStandard>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,DeviceNo1,DeviceNo2 , DeviceNo3 , DeviceNo4, DeviceNo5,DeviceNo6,DeviceNo7,DeviceNo8,DeviceNo9,DeviceNo10,DeviceNo11,DeviceNo12, OutputDevice1, OutputDevice2, ActionCoefficient , LinkageNo1 ,LinkageNo2 ,LinkageNo3,controllerID,Memo from LinkageConfigStandard where controllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LinkageConfigStandard model = new LinkageConfigStandard();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.DeviceNo1 = dt.Rows[i]["DeviceNo1"].ToString();
                model.DeviceNo2 = dt.Rows[i]["DeviceNo2"].ToString();
                model.DeviceNo3 = dt.Rows[i]["DeviceNo3"].ToString();
                model.DeviceNo4 = dt.Rows[i]["DeviceNo4"].ToString();
                model.DeviceNo5 = dt.Rows[i]["DeviceNo5"].ToString();
                model.DeviceNo6 = dt.Rows[i]["DeviceNo6"].ToString();
                model.DeviceNo7 = dt.Rows[i]["DeviceNo7"].ToString();
                model.DeviceNo8 = dt.Rows[i]["DeviceNo8"].ToString();
                model.DeviceNo9 = dt.Rows[i]["DeviceNo9"].ToString();
                model.DeviceNo10 = dt.Rows[i]["DeviceNo10"].ToString();
                model.DeviceNo11 = dt.Rows[i]["DeviceNo11"].ToString();
                model.DeviceNo12 = dt.Rows[i]["DeviceNo12"].ToString();
                model.OutputDevice1 = dt.Rows[i]["OutputDevice1"].ToString();
                model.OutputDevice2 = dt.Rows[i]["OutputDevice2"].ToString();
                model.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["ActionCoefficient"]);
                model.LinkageNo1 = dt.Rows[i]["LinkageNo1"].ToString();
                model.LinkageNo2 = dt.Rows[i]["LinkageNo2"].ToString();
                model.LinkageNo3 = dt.Rows[i]["LinkageNo3"].ToString();
                model.Memo = dt.Rows[i]["Memo"].ToString();
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstData.Add(model);
            }
            return lstData;   
        }

        public List<LinkageConfigMixed> GetMixedLinkageConfig(ControllerModel controller)
        {
            List<LinkageConfigMixed> lstData = new List<LinkageConfigMixed>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code, ActionCoefficient,ActionType, TypeA, LoopNoA, DeviceCodeA,CategoryA, BuildingNoA,ZoneNoA , LayerNoA , DeviceTypeCodeA,TypeB,LoopNoB,DeviceCodeB,CategoryB, BuildingNoB ,ZoneNoB , LayerNoB , DeviceTypeCodeB  ,TypeC ,MachineNoC,LoopNoC ,DeviceCodeC ,BuildingNoC ,ZoneNoC , LayerNoC  ,DeviceTypeCodeC  ,controllerID from LinkageConfigMixed where controllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LinkageConfigMixed model = new LinkageConfigMixed();
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["ActionCoefficient"]);
                model.ActionType = (LinkageActionType)Enum.ToObject(typeof(LinkageActionType), Convert.ToInt16(dt.Rows[i]["ActionType"]));
                model.TypeA = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeA"]));
                model.LoopNoA = dt.Rows[i]["LoopNoA"].ToString();
                model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);
                model.CategoryA = Convert.ToInt32(dt.Rows[i]["CategoryA"]);
                model.BuildingNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoA"]));
                model.ZoneNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoA"]));
                model.LayerNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoA"]));
                model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);
                model.TypeB = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeB"]));
                model.LoopNoB = dt.Rows[i]["LoopNoB"].ToString();
                model.DeviceCodeB = dt.Rows[i]["DeviceCodeB"].ToString();
                model.CategoryB = Convert.ToInt32(dt.Rows[i]["CategoryB"]);
                model.BuildingNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoB"]));
                model.ZoneNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoB"]));
                model.LayerNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoB"]));
                model.DeviceTypeCodeB = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeB"]);
                model.TypeC = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeC"]));
                model.MachineNoC = dt.Rows[i]["MachineNoC"].ToString();
                model.LoopNoC = dt.Rows[i]["LoopNoC"].ToString();
                model.DeviceCodeC = dt.Rows[i]["DeviceCodeC"].ToString();
                model.BuildingNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoC"]));
                model.ZoneNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoC"]));
                model.LayerNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoC"]));
                model.DeviceTypeCodeC = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeC"]);
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstData.Add(model);
            }
            return lstData;
        }

        public List<LinkageConfigGeneral> GetGeneralLinkageConfig(ControllerModel controller)
        {
            List<LinkageConfigGeneral> lstData = new List<LinkageConfigGeneral>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,ActionCoefficient,CategoryA,BuildingNoA,ZoneNoA, LayerNoA1 , LayerNoA2 , DeviceTypeCodeA ,TypeC,MachineNoC ,LoopNoC,DeviceCodeC ,BuildingNoC,ZoneNoC, LayerNoC ,DeviceTypeCodeC ,controllerID from LinkageConfigGeneral where controllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LinkageConfigGeneral model = new LinkageConfigGeneral();
                model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["ActionCoefficient"]);
                model.CategoryA = Convert.ToInt32(dt.Rows[i]["CategoryA"]);
                model.BuildingNoA = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["BuildingNoA"]));
                model.ZoneNoA = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["ZoneNoA"]));
                model.LayerNoA1 = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoA1"]));
                model.LayerNoA2 = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoA2"]));
                model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);
                model.TypeC = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeC"]));
                model.MachineNoC = dt.Rows[i]["MachineNoC"].ToString();
                model.LoopNoC = dt.Rows[i]["LoopNoC"].ToString();
                model.DeviceCodeC = dt.Rows[i]["DeviceCodeC"].ToString();
                model.BuildingNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["BuildingNoC"]));
                model.ZoneNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["ZoneNoC"]));
                model.LayerNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoC"]));
                model.DeviceTypeCodeC = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeC"]);
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstData.Add(model);
            }
            return lstData;   
        }
        private int GetDeviceAddressLength()
        {
            //1.读"文件配置"表，取得（文件版本，控制器类型）
            //2.根据控制器类型，初始化控制器配置信息，取得相应“文件版本”至目的版本之间需要执行升级的操作内容
            StringBuilder sbQuerySQL = new StringBuilder("select  器件长度 from 文件配置;");
            DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            string strResult = "";
            if (dtFile != null)
            {
                if (dtFile.Rows.Count > 0)
                {
                    strResult = dtFile.Rows[0]["器件长度"].ToString();
                }
            }
            return strResult == "" ? 0 : Convert.ToInt32(strResult);
        }



        public int DeleteStandardLinkageConfigInfo(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM LinkageConfigStandard where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);                         
        }

        public int DeleteMixedLinkageConfigInfo(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM LinkageConfigMixed where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);                         
        }

        public int DeleteGeneralLinkageConfigInfo(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM LinkageConfigGeneral where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);                         
        }

        public int DeleteManualControlBoardInfo(int id)
        {
            StringBuilder sbSQL = new StringBuilder("Delete FROM ManualControlBoard where  id  = " + id + ";");
            return _databaseService.ExecuteBySql(sbSQL);                         
        }


        
    }
}
