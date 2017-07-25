using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
using SCA.Model.BusinessModel;
/* ==============================
*
* Author     : William
* Create Date: 2016/11/10 17:16:51
* FileName   : IControllerOperation
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Interface
{
    /// <summary>
    /// 关于Controller的操作分为两部分：
    /// 1.与串口通讯无关的操作
    /// 2.与串口通讯有关的操作（接收、处理来自控制器的命令）
    ///  
    /// 此类作为Controller的通用操作部分
    /// </summary>
    public interface IControllerOperation
    {
    //    event Action<int, int> UpdateProgressBarEvent; 
        #region 控制器节点
        /// <summary>
        /// 获取控制器的导航节点
        /// </summary>
        /// <returns></returns>
        ControllerNodeModel[] GetNodes();

        /// <summary>
        ///  获取控制器节点类型
        /// </summary>
        /// <returns>控制器节点枚举值</returns>
        ControllerNodeType GetControllerNodeType();
        #endregion
        #region 控制器信息操作
        /// <summary>
        /// 获取控制器下的所有回路信息
        /// </summary>
        /// <returns></returns>
        List<LoopModel> GetLoops(int controllerID);
        /// <summary>
        /// 创建指定数量的回路信息
        /// </summary>
        /// <param name="amount">回路数量</param>
        /// <returns></returns>
        List<LoopModel> CreateLoops(int amount);

        bool DeleteControllerBySpecifiedControllerID(int  controllerID);

        //bool DeleteControllerBySpecifiedCode(string strLoopCode);

        List<Model.LoopModel> CreateLoops(int loopsAmount, int deviceAmount, ControllerModel controller);
        /// <summary>
        /// 根据协议格式组织器件信息待下传的数据
        /// </summary>
        /// <returns></returns>
        //List<DeviceInfoBase> OrganizeDeviceInfoForSettingController();
        List<IDevice> OrganizeDeviceInfoForSettingController();

        /// <summary>
        /// 取得控制器的器件信息
        /// </summary>
        /// <returns></returns>
        //List<DeviceInfoBase> GetDevicesInfo(int loopID);
        List<IDevice> GetDevicesInfo(int loopID);
        /// <summary>
        /// 根据协议格式组织“标准组态”待下传的数据
        /// </summary>
        /// <returns></returns>
        List<LinkageConfigStandard> OrganizeLikageStandardInfoForSettingController();
        /// <summary>
        /// 获取器件列的配置信息
        /// </summary>
        /// <returns></returns>
        ColumnConfigInfo[] GetDeviceColumns();
        /// <summary>
        /// 取得最大的器件ID
        /// </summary>
        /// <returns></returns>
        int GetMaxDeviceID();
        /// <summary>
        /// 创建控制器
        /// 2017-02-08
        /// </summary>
        /// <returns></returns>
        bool CreateController(Model.ControllerModel model);
        /// <summary>
        /// 在项目中增加控制器
        /// </summary>
        /// <returns></returns>
        bool AddControllerToProject(ControllerModel controller);
        /// <summary>
        /// 获取控制器内的全部楼号信息
        /// </summary>
        /// <param name="controllerID"></param>
        /// <returns></returns>
        List<short?> GetBuildingNoCollection(int controllerID);

        /// <summary>
        /// 获取数据中已经配置的“器件类型”
        /// </summary>
        /// <param name="controllerID"></param>
        /// <returns></returns>
        List<DeviceType> GetConfiguredDeviceTypeCollection(int controllerID);

        /// <summary>
        /// 获取控制器所有器件
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        List<DeviceInfoForSimulator> GetSimulatorDevices(ControllerModel controller);
        List<DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode,ControllerModel controller,List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine);


        #endregion
        #region 低版本数据升级        
        /// <summary>
        /// 通过指定的数据文件操作服务取得控制器信息        
        /// </summary>
        /// <param name="dbFileVersionService"></param>
        /// <returns></returns>
        ControllerModel OrganizeControllerInfoFromSpecifiedDBFileVersion(IDBFileVersionService dbFileVersionService,ControllerModel controller);        
        #endregion        
        #region EXCEL操作
        
        /// <summary>
        /// 更新进度条
        /// </summary>
        event Action<int> UpdateProgressBarEvent;
        /// <summary>
        /// EXCEL读取完毕事件
        /// </summary>
        event Action<ControllerModel, string> ReadingExcelCompletedEvent;
        event Action<ControllerModel, string> ReadingExcelCancelationEvent;
        event Action<string> ReadingExcelErrorEvent;

        bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType);
        /// <summary>
        /// 设置进度条取消标志
        /// </summary>
        /// <param name="flag"></param>
        void SetStaticProgressBarCancelFlag(bool flag);
        #endregion
        ControllerType GetControllerType();
        void ReadEXCELTemplate(string strFilePath, IFileService fileService, ControllerModel targetController);
        /// <summary>
        /// 取得控制器摘要信息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="startLevel"></param>
        /// <returns></returns>
        SummaryInfo GetSummaryNodes(ControllerModel controller, int startLevel);
        /// <summary>
        /// 获取不同的器件类型
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <returns></returns>
        List<DeviceType> GetAllDeviceTypeOfController(ControllerModel controller);
        
        //OrganizeControllerInfoFromOldVersionSoftwareDataFile
    }
}