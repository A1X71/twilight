using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using System.Data;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 9:18:40
* FileName   : DBContext
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class ProjectDBService:IProjectDBService
    {
        private IDatabaseService _databaseService;
        public ProjectDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        private List<string> GetTablesOfDB()
        {
            List<String> lstTableName = (List<String>)_databaseService.GetDataListBySQL<string>(new StringBuilder(DBSchemaDefinition.GetTablesNameSQL()));
            return lstTableName;
        }
        public bool CreateLocalDBFile()
        {
            try
            {
                _databaseService.CreateDBFile();
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
        public bool CreatFundamentalTableStructure()
        {
            try
            {
                //创建项目信息数据表
                //创建控制器类别数据表
                //创建消防系统分类数据表                            
                //创建器件类别数据表
                List<String> lstTableName = GetTablesOfDB();
                if (lstTableName == null) { lstTableName = new List<string>(); };
                if (!lstTableName.Contains("project"))
                {
                    StringBuilder sbProjectSQL = new StringBuilder("Create table project(ID  integer not null primary key autoincrement, Name varchar(255), SaveInterval integer, SavePath varchar(255));");
                    _databaseService.ExecuteBySql(sbProjectSQL);
                }
                if (!lstTableName.Contains("ControllerType"))
                {
                    StringBuilder sbControllerTypeSQL = new StringBuilder("Create table ControllerType(ID integer not null primary key,Name varchar(255), DeFireSystemCategoryID integer references DeFireSystemCategory(ID) on delete restrict deferrable initially deferred not null);");
                    _databaseService.ExecuteBySql(sbControllerTypeSQL);
                }
                if (!lstTableName.Contains("DeFireSystemCatetory"))
                {
                    StringBuilder sbDeFireSystemCategorySQL = new StringBuilder("Create table DeFireSystemCatetory(ID integer not null primary key autoincrement, Name varchar(255), Memo varchar(255));");
                    _databaseService.ExecuteBySql(sbDeFireSystemCategorySQL);
                }
                if (!lstTableName.Contains("DeviceType"))
                {
                    StringBuilder sbDeviceTypeSQL = new StringBuilder("Create table DeviceType(Code integer not null primary key autoincrement, Name varchar(20),CustomImage blob,StandardImage blob,IsValid Boolean,MatchingController varchar(100), ProjectID integer references Project(ID) on delete restrict deferrable initially deferred not null);");
                    _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                }
                #region 创建控制器相关信息
                //创建控制器信息数据表
                //创建控制器附属信息数据表
                //创建标准组态信息数据表
                //创建回路信息数据表
                if (!lstTableName.Contains("Controller"))
                {
                    StringBuilder sbControllerSQL = new StringBuilder("Create table Controller(ID integer not null primary key autoincrement, PrimaryFlag Boolean,TypeID integer references ControllerType(ID) on delete restrict deferrable initially deferred not null, DeviceAddressLength integer, Name varchar(20), PortName varchar(5),BaudRate integer,MachineNumber varchar(5), Version integer,ProjectID integer references Project(ID) on delete restrict deferrable initially deferred not null);");
                    _databaseService.ExecuteBySql(sbControllerSQL);
                }
                //此处的涉及文件版本，主要用于区分不同版本的Controller相关数据字段特性
                if (!lstTableName.Contains("ControllerAttachedInfo"))
                {
                    StringBuilder sbControllerAttachedInfoSQL = new StringBuilder("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,FileVersion varchar(4),ProtocolVersion varchar(4),Position varchar(15),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null);");
                    _databaseService.ExecuteBySql(sbControllerAttachedInfoSQL);
                }
                if (!lstTableName.Contains("LinkageConfigStandard"))
                {
                    StringBuilder sbLinkageConfigStandardSQL = new StringBuilder("Create table LinkageConfigStandard(ID integer not null primary key autoincrement,Code varchar(4) not null, DeviceNo1 varchar(10),DeviceNo2 varchar(10), DeviceNo3 varchar(10), DeviceNo4 varchar(10), DeviceNo5 varchar(10),DeviceNo6 varchar(10),DeviceNo7 varchar(10),DeviceNo8 varchar(10),DeviceNo9 varchar(10),DeviceNo10 varchar(10), ActionCoefficient integer, LinkageNo1 varchar(4),LinkageNo2 varchar(4),LinkageNo3 varchar(4),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
                    _databaseService.ExecuteBySql(sbLinkageConfigStandardSQL);
                }
                if (!lstTableName.Contains("LinkageConfigGeneral"))
                {
                    StringBuilder sbLinkageConfigStandardSQL = new StringBuilder("Create table LinkageConfigGeneral(ID integer not null primary key autoincrement,Code varchar(4) not null,ActionCoefficient integer, BuildingNoA integer,ZoneNoA integer, LayerNoA1 integer, LayerNoA2 integer, DeviceTypeCodeA integer references DeviceType(Code) on delete restrict deferrable initially deferred,TypeC varchar(20),MachineNoC varchar2(10),LoopNoC varchar(10),DeviceCodeC varchar(10),BuildingNoC integer,ZoneNoC integer, LayerNoC integer ,DeviceTypeCodeC integer references DeviceType(Code) on delete restrict deferrable initially deferred,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
                    _databaseService.ExecuteBySql(sbLinkageConfigStandardSQL);
                }
                if (!lstTableName.Contains("LinkageConfigMixed"))
                {
                    StringBuilder sbLinkageConfigStandardSQL = new StringBuilder("Create table LinkageConfigMixed(ID integer not null primary key autoincrement,Code varchar(4) not null, ActionCoefficient integer,ActionType varchar(10), TypeA varchar(20), LoopNoA varchar(10), DeviceCodeA varchar(10),BuildingNoA integer,ZoneNoA integer, LayerNoA integer, DeviceTypeCodeA integer , TypeB varchar(20), LoopNoB varchar(10), DeviceCodeB varchar(10),BuildingNoB integer,ZoneNoB integer, LayerNoB integer, DeviceTypeCodeB integer ,TypeC varchar(20),MachineNoC varchar(10),LoopNoC varchar(10),DeviceCodeC varchar(10),BuildingNoC integer,ZoneNoC integer, LayerNoC integer ,DeviceTypeCodeC integer,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
                    _databaseService.ExecuteBySql(sbLinkageConfigStandardSQL);
                }
                if (!lstTableName.Contains("ManualControlBoard"))
                {
                    StringBuilder sbLinkageConfigStandardSQL = new StringBuilder("Create table ManualControlBoard(ID integer not null primary key autoincrement,Code varchar(4) not null, BoardNo varchar(10),SubBoardNo varchar(10),KeyNo varchar(10), DeviceCode varchar(10), SDPKey varchar(10),controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID))");
                    _databaseService.ExecuteBySql(sbLinkageConfigStandardSQL);
                }
                if (!lstTableName.Contains("Loop"))
                {
                    StringBuilder sbLoopSQL = new StringBuilder("Create table Loop(ID integer not null primary key autoincrement,Code varchar(6),Name varchar(20),DeviceAmount integer,controllerID integer references Controller(ID) on delete restrict deferrable initially deferred not null,unique(Code,controllerID));");
                    _databaseService.ExecuteBySql(sbLoopSQL);
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InitializeControllerTypeInfo()
        {
            try
            {
                if (!IsInitializedForControllerType())
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
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsInitializedForControllerType()
        {
            StringBuilder sbControllerTypeSQL = new StringBuilder("Select count(*) from ControllerType;");
            
            if(Convert.ToInt16(_databaseService.GetObjectValue(sbControllerTypeSQL))>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Model.ProjectModel GetProject(int id)
        {
            StringBuilder sbQueryProjectSQL = new StringBuilder("select id,name,saveinterval,savepath from project where id=" + id);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQueryProjectSQL);
            Model.ProjectModel model = new Model.ProjectModel();
            model.ID = Convert.ToInt16(dt.Rows[0]["id"]);
            model.Name = dt.Rows[0]["name"].ToString();
            model.SavePath = dt.Rows[0]["savepath"] is System.DBNull ? null : dt.Rows[0]["savepath"].ToString();
            model.SaveInterval = dt.Rows[0]["saveinterval"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[0]["saveinterval"]);
            return model;
        }

        public Model.ProjectModel GetProject(Model.ProjectModel project)
        {
            StringBuilder sbQueryProjectSQL = new StringBuilder("select id,name,saveinterval,savepath from project where id=" + project.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQueryProjectSQL);
            Model.ProjectModel model = new Model.ProjectModel();
            model.ID = Convert.ToInt16(dt.Rows[0]["id"]);
            model.Name = dt.Rows[0]["name"].ToString();
            model.SavePath = dt.Rows[0]["savepath"] is System.DBNull ? null : dt.Rows[0]["savepath"].ToString();
            model.SaveInterval = dt.Rows[0]["saveinterval"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[0]["saveinterval"]);
            return model;
        }

        public int AddProject(Model.ProjectModel project)
        {
            StringBuilder sbProjectSQL;
            if (project.ID == 0) //在目前软件中，一次仅可管理一个项目信息，后续如有扩展，可将此逻辑拆分至UpdateProject中
            {
                sbProjectSQL = new StringBuilder("Insert into Project(Name,savepath,saveinterval) values(\"");
                sbProjectSQL.Append(project.Name + "\",\"");
                sbProjectSQL.Append(project.SavePath + "\",");
                sbProjectSQL.Append(project.SaveInterval + ");");
            }
            else
            {
                sbProjectSQL = new StringBuilder("Replace into Project(ID,Name,savepath,saveinterval) values(\"");
                sbProjectSQL.Append(project.ID + "\",\"");
                sbProjectSQL.Append(project.Name + "\",\"");
                sbProjectSQL.Append(project.SavePath + "\",");
                sbProjectSQL.Append(project.SaveInterval + ");");
            }
            return _databaseService.ExecuteBySql(sbProjectSQL);
        }
        /// <summary>
        /// 取得当前最大的ID号
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Project;");
            
            return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
        }
        public int UpdateProject(Model.ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProject(int ProjectId)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable OpenProject()
        {
            DataTable dt=null;
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
            catch(Exception ex)
            {
            
            }
            return dt;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
