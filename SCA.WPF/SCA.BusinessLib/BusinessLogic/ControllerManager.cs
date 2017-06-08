using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;
using SCA.Model;
using System.Collections.Concurrent;
/* ==============================
*
* Author     : William
* Create Date: 2016/11/11 13:14:27
* FileName   : ControllerManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    //1.构建表现层
    //2.执行相关操作
    //OPC UA协议
    public class ControllerManager
    {
        private ConcurrentDictionary<ControllerType, IControllerOperation> _controllers;
        private IControllerCommunication _controllerCommunication;
        private IControllerOperation _controllerOperation;
        private IControllerConfig _controllerConfig;
        public ControllerManager()
        {
            _controllers = new ConcurrentDictionary<ControllerType, IControllerOperation>();
        }
        public void InitializeAllControllerOperation(IDatabaseService dbService)
        {
            AddController(ControllerType.NT8036, new ControllerOperation8036(dbService));
            AddController(ControllerType.NT8001, new ControllerOperation8001(dbService));
            AddController(ControllerType.NT8007, new ControllerOperation8007(dbService));
            AddController(ControllerType.FT8000, new ControllerOperation8000(dbService));
            AddController(ControllerType.FT8003, new ControllerOperation8003(dbService));
            AddController(ControllerType.NT8021, new ControllerOperation8021(dbService));
        }
        public bool AddController(ControllerType key, IControllerOperation val)
        {
            return _controllers.TryAdd(key, val);
        }
        public IControllerOperation GetController(ControllerType key)
        {
            IControllerOperation val;
            if (_controllers.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }
        public void GetCurrentStatus()
        { 
            
        }

        public bool ConvertDataFromOldVersionSoftwareToThisVersionSoftware(IDatabaseService databaseService)
        {
            //try
            //{
            //    IControllerOperation controllerOperation = null;
            //    ControllerOperationCommon controllerBase = new ControllerOperationCommon();
            //    string[] strFileInfo = controllerBase.GetFileVersionAndControllerType(databaseService);
            //    if (strFileInfo.Length > 0)
            //    {
            //        switch (strFileInfo[0])
            //        {
            //            case "8036":
            //                controllerOperation = new ControllerOperation8036(databaseService);
            //                break;
            //        }
            //        if (controllerOperation != null)
            //        {
            //            controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(Convert.ToInt16(strFileInfo[0]),7,databaseService);
            //        }
            //        //strFileInfo[1];
            //    }
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}


            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得器件信息
        /// </summary>
        /// <returns></returns>
        //public List<DeviceInfo8036> GetDevicesInfo(int loopID)
        //{
        //    List<DeviceInfoBase> devicesInfo=_controllerOperation.GetDevicesInfo(loopID);

        //    return devicesInfo;
        //}
        public bool CreateController(ControllerModel controller, IDeviceTypeDBService deviceTypeDBService, IControllerDBService controllerDBService)
        {
            try
            {
                #region 控制器配置
                ControllerConfig8036 config = new ControllerConfig8036();
                string strMatchingDevTypeID = config.GetDeviceTypeCodeInfo();
                #region comment
                //StringBuilder sbDeviceTypeSQL = new StringBuilder("Select Code,Name,IsValid,ProjectID, MatchingController from DeviceType where Code in ("+strMatchingDevTypeID+");" );
                //List<Model.DeviceType> lstDeviceType=(List<Model.DeviceType>)_databaseService.GetDataListBySQL<DeviceType>(sbDeviceTypeSQL);
                //foreach (DeviceType devType in lstDeviceType)
                //{
                //    devType.MatchingController = devType.MatchingController == null ? "" : devType.MatchingController;

                //    //如果MatchingController中不包含当前的控制器，则更新字段
                //    if (!devType.MatchingController.Contains(ControllerType.NT8036.ToString()))
                //    {
                //        devType.MatchingController = devType.MatchingController == "" ? devType.MatchingController : devType.MatchingController + ",";
                //        devType.MatchingController = devType.MatchingController  + ControllerType.NT8036;
                //        sbDeviceTypeSQL = new StringBuilder("Update DeviceType set MatchingController='" + devType.MatchingController + "' where Code=" + devType.Code + ";");
                //   //     _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                //    }                    
                //}
                #endregion
                deviceTypeDBService.UpdateMatchingController(ControllerType.NT8036, strMatchingDevTypeID); //refactor 2017-02-23
                #endregion

                #region 增加控制器信息 refactor commented
                //版本号怎么计，是按原版本号累加，还是初始化一个新的版本号
                //当前为初始化一个新的版本号
                //StringBuilder sbControllerSQL = new StringBuilder("Insert into Controller(ID,PrimaryFlag,TypeID,DeviceAddressLength,Name,PortName,BaudRate,MachineNumber,Version,ProjectID) values(");
                //sbControllerSQL.Append(controller.ID  + ",'");
                //sbControllerSQL.Append(controller.PrimaryFlag + "',");//+ "',0);");
                //sbControllerSQL.Append((int)controller.TypeCode + ",");
                //sbControllerSQL.Append(controller.DeviceAddressLength + ",'");
                //sbControllerSQL.Append(controller.Name + "','");
                //sbControllerSQL.Append(controller.PortName + "',");
                //sbControllerSQL.Append(controller.BaudRate + ",'");
                //sbControllerSQL.Append(controller.MachineNumber + "',");
                //sbControllerSQL.Append(controller.Version + ",");
                //sbControllerSQL.Append(controller.Project.ID + ")");
                //_databaseService.ExecuteBySql(sbControllerSQL);

                #endregion
                controllerDBService.AddController(controller); //refactor at 2017-02-23
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
