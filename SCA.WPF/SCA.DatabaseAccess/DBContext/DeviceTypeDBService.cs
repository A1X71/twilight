using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;

using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 15:54:19
* FileName   : DeviceTypeDBContext
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class DeviceTypeDBService:IDeviceTypeDBService
    {
        private IDatabaseService _databaseService;
        private IDBFileVersionService _dbFileVersionService;
        public DeviceTypeDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public DeviceTypeDBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }

        public int AddDeviceType(List<Model.DeviceType> lstDeviceType)
        {
            throw new NotImplementedException();
        }
               
        public bool  UpdateMatchingController(ControllerType controllerType,string matchingType)
        {
            try
            {
                //StringBuilder sbDeviceTypeSQL = new StringBuilder("Select Code,Name,IsValid,ProjectID, MatchingController from DeviceType where Code in (" + matchingType + ");");
                //List<DeviceType> lstDeviceType = (List<DeviceType>)_databaseService.GetDataListBySQL<DeviceType>(sbDeviceTypeSQL);
                //foreach (DeviceType devType in lstDeviceType)
                //{
                //    devType.MatchingController = devType.MatchingController == null ? "" : devType.MatchingController;
                //    //如果MatchingController中不包含当前的控制器，则更新字段
                //    if (!devType.MatchingController.Contains(controllerType.ToString()))
                //    {
                //        devType.MatchingController = devType.MatchingController == "" ? devType.MatchingController : devType.MatchingController + ",";
                //        devType.MatchingController = devType.MatchingController + controllerType.ToString();
                //        sbDeviceTypeSQL = new StringBuilder("Update DeviceType set MatchingController='" + devType.MatchingController + "' where Code=" + devType.Code + ";");
                //        _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                //    }
                //}
                _dbFileVersionService.UpdateMatchingController(controllerType, matchingType);
                
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 初始化器件类型信息
        /// </summary>
        /// <param name="lstDeviceType">控制器类型集合</param>
        /// <returns></returns>
        public bool InitializeDeviceTypeInfo(List<Model.DeviceType> lstDeviceType)
        {
            try
            {
                if (!IsInitializedForDeviceType())
                { 
                    //foreach (SCA.Model.DeviceType devType in lstDeviceType)
                    //{
                    //    StringBuilder sbDeviceTypeSQL = new StringBuilder("Insert into DeviceType(Code,Name,IsValid,ProjectID) values(");
                    //    sbDeviceTypeSQL.Append(devType.Code + ",'");
                    //    sbDeviceTypeSQL.Append(devType.Name + "','");
                    //    sbDeviceTypeSQL.Append(devType.IsValid + "',");
                    //    sbDeviceTypeSQL.Append(devType.ProjectID + ")");
                    //    _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                    //}
                    _dbFileVersionService.AddDeviceTypeInfo(lstDeviceType);                    
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool IsInitializedForDeviceType()
        {
            //StringBuilder sbControllerTypeSQL = new StringBuilder("Select count(*) from DeviceType;");

            //if (Convert.ToInt16(_databaseService.GetObjectValue(sbControllerTypeSQL)) > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //StringBuilder sbControllerTypeSQL = new StringBuilder("Select count(*) from DeviceType;");
            if (_dbFileVersionService.GetAmountOfDeviceType() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
