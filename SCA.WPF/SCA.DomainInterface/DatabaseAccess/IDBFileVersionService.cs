using System;
using System.Data;
using System.Collections.Generic;
using SCA.Model;

namespace SCA.Interface.DatabaseAccess
{
    /// <summary>
    /// 数据文件版本类
    /// </summary>
    public interface IDBFileVersionService:IDisposable
    {        
        int DBFileVersion { get; }
        string DBFilePath { get; }
        #region 基础数据结构创建
        /// <summary>
        /// 创建数据存储文件
        /// </summary>
        /// <returns></returns>
        bool CreateLocalDBFile();
        bool CreateTableForProject();
        bool CreateTableForControllerType();
        bool CreateTableForDeFireSystemCategory();
        bool CreateTableForDeviceType();
        bool CreateTableForController();
        //bool CreateTableForControllerAttachedInfo();
        bool CreateTableForLinkageConfigStandard();
        bool CreateTableForLinkageConfigGeneral();
        bool CreateTableForLinkageconfigMixed();
        bool CreateTableForManualControlBoard();
        bool CreateTableForLoop();
        #endregion
        #region 基础数据初始化
        /// <summary>
        /// 控制器类型数据记录数量
        /// </summary>
        /// <returns>数量</returns>
        int GetAmountOfControllerType();
        /// <summary>
        /// 增加控制器类型信息
        /// </summary>
        /// <returns>成功，返回True</returns>
        bool AddControllerTypeInfo();
        #endregion
        #region 基础信息读取等操作
        /// <summary>
        /// 根据ID取得项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>项目信息</returns>
        ProjectModel GetProject(int id);
        ///// <summary>
        ///// 取得回路信息
        ///// </summary>
        ///// <param name="controllerID">控制器ID</param>
        ///// <returns></returns>
        //List<LoopModel> GetLoopsInfo(int controllerID);
        /// <summary>
        /// 取得文件版本
        ///</summary>
        int GetFileVersion();
        /// <summary>
        /// 取得附属信息：主要兼容4,5,6版本数据文件
        /// </summary>
        /// <returns></returns>
        //ControllerAttachedInfo GetAttachedInfo();
        /// <summary>
        /// 写入控制器信息
        /// </summary>
        /// <param name="project">控制器信息</param>
        /// <returns>操作影响的行数</returns>
        int AddProject(Model.ProjectModel project);
        
        /// <summary>
        /// 取得所有项目及其所有信息（控制器，回路等信息）
        /// </summary>
        /// <returns></returns>
        DataTable OpenProject();
        /// <summary>
        /// 取得控制器信息表最大ID
        /// </summary>
        /// <returns></returns>
        int GetMaxIDFromProject();
        #endregion
        #region 组态及手动盘读取等操作        
        List<LinkageConfigStandard> GetStandardLinkageConfig(ControllerModel controller);
        int AddStandardLinkageConfigInfo(LinkageConfigStandard linkageConfigStandard);
        
        List<LinkageConfigMixed> GetMixedLinkageConfig(Model.ControllerModel controller);
        int AddMixedLinkageConfigInfo(LinkageConfigMixed linkageConfigMixed);
        
        List<LinkageConfigGeneral> GetGeneralLinkageConfig(Model.ControllerModel controller);
        int AddGeneralLinkageConfigInfo(LinkageConfigGeneral linkageConfigGeneral);
        List<ManualControlBoard> GetManualControlBoard(ControllerModel controller);
        int AddManualControlBoardInfo(ManualControlBoard manualControlBoard);
        #endregion
        //bool GetControllerInfo(ref ControllerModel controllerInfo);
        ////适应新版及老版控制器数据读取
        //List<ControllerModel> GetControllerInfo(ControllerType controllerType);
        #region 8000控制器器件信息
        bool CreateTableForDeviceInfoOfControllerType8000();
        bool AddDeviceForControllerType8000(LoopModel loop);
        int AddDeviceForControllerType8000(DeviceInfo8000 deviceInfo);
        bool GetDevicesByLoopForControllerType8000(ref LoopModel loop);
        LoopModel GetDevicesByLoopForControllerType8000( LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8000(int id);
        int DeleteDeviceByIDForControllerType8000(int id);
        int GetMaxDeviceIDForControllerType8000();
        #endregion
        #region 8001控制器器件信息
        
        bool CreateTableForDeviceInfoOfControllerType8001();
        bool AddDeviceForControllerType8001(LoopModel loop);
        int AddDeviceForControllerType8001(DeviceInfo8001 deviceInfo);
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        bool GetDevicesByLoopForControllerType8001(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard);
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        LoopModel GetDevicesByLoopForControllerType8001(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8001(int id);
        int DeleteDeviceByIDForControllerType8001(int id);
        int GetMaxDeviceIDForControllerType8001();
        #endregion
        #region 8003控制器器件信息
        bool CreateTableForDeviceInfoOfControllerType8003();
        bool AddDeviceForControllerType8003(LoopModel loop);
        int AddDeviceForControllerType8003(DeviceInfo8003 device);
        bool GetDevicesByLoopForControllerType8003(ref LoopModel loop);
        LoopModel GetDevicesByLoopForControllerType8003(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8003(int id);
        int DeleteDeviceByIDForControllerType8003(int id);
        int GetMaxDeviceIDForControllerType8003();
        #endregion        
        #region 8007控制器器件信息
        
        bool CreateTableForDeviceInfoOfControllerType8007();
        bool AddDeviceForControllerType8007(LoopModel loop);
        int AddDeviceForControllerType8007(DeviceInfo8007 deviceInfo);
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        bool GetDevicesByLoopForControllerType8007(ref LoopModel loop);
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        LoopModel GetDevicesByLoopForControllerType8007(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8007(int id);
        int DeleteDeviceByIDForControllerType8007(int id);
        int GetMaxDeviceIDForControllerType8007();
        #endregion
        #region 8021控制器器件信息
        
        bool CreateTableForDeviceInfoOfControllerType8021();
        bool AddDeviceForControllerType8021(LoopModel loop);
        int AddDeviceForControllerType8021(DeviceInfo8021 deviceInfo);
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        bool GetDevicesByLoopForControllerType8021(ref LoopModel loop);
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        LoopModel GetDevicesByLoopForControllerType8021(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8021(int id);
        int DeleteDeviceByIDForControllerType8021(int id);
        int GetMaxDeviceIDForControllerType8021();
        #endregion
        #region 8036控制器器件信息
        
        bool CreateTableForDeviceInfoOfControllerType8036();
        bool AddDeviceForControllerType8036(LoopModel loop);
        int AddDeviceForControllerType8036(DeviceInfo8036 deviceInfo);
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        bool GetDevicesByLoopForControllerType8036(ref LoopModel loop);
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        LoopModel GetDevicesByLoopForControllerType8036(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8036(int id);
        int DeleteDeviceByIDForControllerType8036(int id);
        int GetMaxDeviceIDForControllerType8036();
        #endregion
        #region 8053控制器器件信息

        bool CreateTableForDeviceInfoOfControllerType8053();
        bool AddDeviceForControllerType8053(LoopModel loop);
        int AddDeviceForControllerType8053(DeviceInfo8053 deviceInfo);
        /// <summary>
        /// 读取器件信息时，为器件信息关联手动盘信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <param name="dictDeviceMappingManualControlBoard">手动盘信息</param>
        /// <returns></returns>
        bool GetDevicesByLoopForControllerType8053(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard);
        /// <summary>
        /// 读取器件信息
        /// </summary>
        /// <param name="loop">器件所在的回路</param>
        /// <returns>已添加器件信息的回路</returns>
        LoopModel GetDevicesByLoopForControllerType8053(LoopModel loop);
        int DeleteAllDevicesByControllerIDForControllerType8053(int id);
        int DeleteDeviceByIDForControllerType8053(int id);
        int GetMaxDeviceIDForControllerType8053();
        #endregion


        #region 控制器信息读取等操作
        int AddController(ControllerModel controller);
        int DeleteController(int controllerID);
        int GetMaxIDFromController();
        List<ControllerModel> GetControllersByProject(ProjectModel project);               
        #endregion
        #region 回路信息读取等操作
        int DeleteLoopInfo(int loopID);
        int DeleteLoopsByControllerID(int controllerID);
        int AddLoopInfo(Model.LoopModel loop);
        int GetMaxIDFromLoop();
        List<LoopModel> GetLoopsByController(ControllerModel controller);
        #endregion
        #region 器件类型信息读取等操作
        /// <summary>
        /// 更新“器件类型”表中的“匹配控制器”字段（MatchingController)
        /// </summary>
        /// <param name="controllerType">控制器类型 </param>
        /// <param name="matchingType">控制器可用的器件类型 </param>
        /// <returns>影响的行数</returns>
        int UpdateMatchingController(ControllerType controllerType, string matchingType);
        /// <summary>
        /// 增加器件类型信息
        /// </summary>
        /// <param name="lstDeviceType">需增加的器件类型 </param>
        /// <returns>影响的行数</returns>
        int AddDeviceTypeInfo(List<DeviceType> lstDeviceType);
        /// <summary>
        /// 取得“器件类型”的数量
        /// </summary>
        /// <returns>“器件类型”表记录条数</returns>
        int GetAmountOfDeviceType();
        #endregion  
        /// <summary>
        /// 从数据库结构中取得当前已创建的数据表
        /// </summary>
        /// <returns></returns>
        List<string> GetTablesOfDB();
        /// <summary>
        /// 从数据库结构中取得当前已创建的数据表
        /// </summary>
        /// <param name="tableName">查找指定表名称的数据结构</param>
        /// <returns></returns>
        List<string> GetTablesOfDB(string tableName);

    }
}
