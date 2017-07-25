using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 15:48:26
* FileName   : ControllerDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class ControllerDBService:IControllerDBService
    {
        private IDatabaseService _databaseService;
        private IDBFileVersionService _dbFileVersionService;
        public ControllerDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public ControllerDBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }
        public Model.ControllerModel GetController(int id)
        {
            throw new NotImplementedException();
        }

        public Model.ControllerModel GetController(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }

        public bool  AddController(Model.ControllerModel controller)
        {
            int intEffectiveRows=0;
            try
            {
                //StringBuilder sbControllerSQL = new StringBuilder("Replace into Controller(ID,PrimaryFlag,TypeID,DeviceAddressLength,Name,PortName,BaudRate,MachineNumber,Version,ProjectID) values(");
                //sbControllerSQL.Append(controller.ID + ",'");
                //sbControllerSQL.Append(controller.PrimaryFlag + "',");//+ "',0);");
                //sbControllerSQL.Append((int)controller.Type + ",");
                //sbControllerSQL.Append(controller.DeviceAddressLength + ",'");
                //sbControllerSQL.Append(controller.Name + "','");
                //sbControllerSQL.Append(controller.PortName + "',");
                //sbControllerSQL.Append(controller.BaudRate + ",'");
                //sbControllerSQL.Append(controller.MachineNumber + "',");
                //sbControllerSQL.Append(controller.Version + ",");
                //sbControllerSQL.Append(controller.Project.ID + ")");
                //intEffectiveRows = _databaseService.ExecuteBySql(sbControllerSQL);
                intEffectiveRows = _dbFileVersionService.AddController(controller);
            }
            catch
            {
                intEffectiveRows = 0;
            }

            if (intEffectiveRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UpdateController(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }

        public bool DeleteController(int controllerID)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM Controller where id= "+controllerID+";" );
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;
            try
            {
                _dbFileVersionService.DeleteController(controllerID);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        /// <summary>
        /// 取得当前最大的ID号
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            //StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Controller;");
            //return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
            return _dbFileVersionService.GetMaxIDFromController();
        }
        public void Dispose()
        {
            _dbFileVersionService.Dispose();
        }

        public List<Model.ControllerModel> GetControllersByProject(Model.ProjectModel project)
        {
            //List<ControllerModel> lstControllers=new List<ControllerModel>();
            //StringBuilder sbQuerySQL = new StringBuilder("select id,primaryflag,typeid,deviceaddresslength,name,portname,baudrate,machinenumber,version from controller where projectid=" + project.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Model.ControllerModel model = new Model.ControllerModel();
            //    model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
            //    model.PrimaryFlag = (bool)dt.Rows[i]["PRIMARYFLAG"];
            //    model.Type = (ControllerType)Enum.ToObject(typeof(ControllerType), Convert.ToInt32(dt.Rows[i]["TYPEID"]));
            //    model.DeviceAddressLength = dt.Rows[i]["DEVICEADDRESSLENGTH"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[i]["DEVICEADDRESSLENGTH"]);
            //    model.Name = dt.Rows[i]["Name"].ToString();
            //    model.PortName = dt.Rows[i]["PORTNAME"].ToString();
            //    model.BaudRate = Convert.ToInt32(dt.Rows[i]["BAUDRATE"]);
            //    model.MachineNumber = dt.Rows[i]["MACHINENUMBER"].ToString();
            //    model.Version = Convert.ToInt16(dt.Rows[i]["VERSION"]);
            //    model.ProjectID = project.ID;
            //    model.Project = project;

            //    model.LoopAddressLength = 2;                
            //    lstControllers.Add(model);
            //}            
            //return lstControllers;            
            return _dbFileVersionService.GetControllersByProject(project);
        }
    }
}
