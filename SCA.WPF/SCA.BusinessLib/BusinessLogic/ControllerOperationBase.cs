using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using System.Data;
using SCA.Interface.DatabaseAccess;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/17 16:18:25
* FileName   : ControllerOperationBase
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public abstract class ControllerOperationBase
    {
      
        public ControllerOperationBase()
        { 
        
        }

        protected List<Model.LoopModel> GetLoopInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldDBService)
        {
            return oldDBService.GetLoopsInfo();
        }
        /// <summary>
        /// 将字符型联动类型转换为枚举值 
        /// </summary>
        /// <param name="linkageType"></param>
        /// <returns></returns>
        protected Model.LinkageType ConvertLinkageType(string linkageType)
        {

            switch (linkageType)
            {
                case "区层":
                    return Model.LinkageType.ZoneLayer;
                case "地址":
                    return Model.LinkageType.Address;
                case "本层":
                    return Model.LinkageType.SameLayer;
                case "邻层":
                    return Model.LinkageType.AdjacentLayer;
                default:
                    return LinkageType.None;
            }
        }
        /// <summary>
        /// 将字符型“动作类型”转换为枚举值
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        protected Model.LinkageActionType ConvertLinkageActionType(string actionType)
        {
            switch (actionType)
            { 
                case "或":
                    return LinkageActionType.OR;
                case "与":
                    return LinkageActionType.AND;
                default:
                    return LinkageActionType.NONE;
            }
        }
        /// <summary>
        /// 取得当前项目下控制器的最大ID
        /// </summary>
        /// <returns></returns>
        protected int GetMaxControllerID()
        {
            SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;
            int tempID = 0;
            foreach (var c in project.Controllers)
            {
                tempID = c.ID == null ? 0 : c.ID;
                if (c.ID > tempID)
                {
                    tempID = c.ID;
                }
            }
            return tempID;
        }
        public bool AddControllerToProject(ControllerModel controller)
        {
            try
            {
                SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;

                if (project.Controllers.Count == 0)//如果还未设置主控制器，则默认第一个控制器为主控制器
                {
                    controller.PrimaryFlag = true;
                }
                int maxControllerID = GetMaxControllerID();
                controller.ID = maxControllerID + 1;
                controller.ProjectID = project.ID;
                controller.Project = project;
                project.Controllers.Add(controller);
                controller.IsDirty = true;
                //SetDataDirty();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将数据置为需要保存状态
        /// </summary>
        protected void SetDataDirty()
        {
            SCA.BusinessLib.ProjectManager.GetInstance.IsDirty = true;
        }

        public bool DeleteControllerBySpecifiedControllerID(int controllerID)
        {
            try
            {
                ProjectModel project = ProjectManager.GetInstance.Project;
                var result = from c in project.Controllers where c.ID == controllerID select c;
                ControllerModel controller = result.FirstOrDefault();
                if (controller != null)
                {
                    project.Controllers.Remove(controller);                    
                    //SetDataDirty();
                    DeleteControllerFromDB(controller);

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool DeleteControllerFromDB(ControllerModel controller)
        {
            try
            {
                IFileService _fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;
                IDatabaseService _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(controller.Project.SavePath, logger, _fileService);
                IControllerDBService controllerDBService = new SCA.DatabaseAccess.DBContext.ControllerDBService(_databaseService);
                ILoopDBService loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(_databaseService);
                IDeviceDBServiceTest deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(controller.Type, _databaseService);
                deviceDBService.DeleteAllDevicesByControllerID(controller.ID);
                loopDBService.DeleteLoopsByControllerID(controller.ID);
                controllerDBService.DeleteController(controller.ID);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public int GetMaxLoopID()
        {
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers select r;
            int maxLoopID = 0;
            foreach (var c in controllers)
            {
                if (c.Loops.Count > 0)
                { 
                    int currentControllerMaxLoopID = Convert.ToInt32(c.Loops.Max(loop => loop.ID));
                    if (currentControllerMaxLoopID > maxLoopID)
                    {
                        maxLoopID = currentControllerMaxLoopID;
                    }
                }
            }
            return maxLoopID;
        }
        public List<short?> GetBuildingNoCollection(int controllerID)
        {
            var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
            List<ControllerModel> lstControllers = result.ToList<ControllerModel>();

            ControllerModel controller = result.FirstOrDefault();

            List<short?> lstBuildingNo = new List<short?>();
            lstBuildingNo = GetAllBuildingNoWithController(controller);
            return lstBuildingNo;
        }
        protected abstract List<short?>  GetAllBuildingNoWithController(ControllerModel controller);

        
        public List<DeviceType> GetConfiguredDeviceTypeCollection(int controllerID)
        {
            var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
            List<ControllerModel> lstControllers = result.ToList<ControllerModel>();

            ControllerModel controller = result.FirstOrDefault();

            List<DeviceType> lstDeviceType = new List<DeviceType>();

            lstDeviceType = GetConfiguredDeviceTypeWithController(controller);
            return lstDeviceType;
        }
        protected abstract List<DeviceType> GetConfiguredDeviceTypeWithController(ControllerModel controller);
        //protected List<DeviceInfoForSimulator> GetSimulatorDevicesOfOtherMachine(ControllerType cType)
        //{ 
            
        //}
        //protected abstract List<DeviceInfoForSimulator> GetSimulatorDevices();
    }
}
