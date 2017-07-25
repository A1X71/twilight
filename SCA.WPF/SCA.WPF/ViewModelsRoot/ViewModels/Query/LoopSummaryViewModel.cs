using SCA.Model;
using SCA.WPF.Infrastructure;
using SCA.WPF.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.Interface;
using SCA.Model.BusinessModel;
using System.Linq;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using Caliburn.Micro;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/8 11:07:38
* FileName   : LoopSummaryViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Query
{
    public class LoopSummaryViewModel:PropertyChangedBase
    {
        //List<LoopModel> _loopsCode = null;
        
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";

        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }
        private int _selectedLoopID;
        private Int16 _selectedDeviceTypeCode;
        private ControllerModel _theController;
        private List<LoopModel> _loops;
        //private IObservableCollection<Dev>
        List<DeviceType> _deviceTypes = null;
        private string _deviceCode="";//器件编码
        

        //器件查询集合
        private ObservableCollection<DeviceInfoForSimulator> _deviceInfoObservableCollection;
        public ObservableCollection<DeviceInfoForSimulator> DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoObservableCollection == null)
                {
                    _deviceInfoObservableCollection = new ObservableCollection<DeviceInfoForSimulator>();
                }
                return _deviceInfoObservableCollection;
            }
            set
            {
                _deviceInfoObservableCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public ControllerModel TheController 
        {
            get
            {
                return _theController;
            }
            set
            {
                _theController = value;                
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                Loops = _theController.Loops;
                DeviceTypes = GetValidDeviceTypes();

                //if (DeviceTypes.Count > 0)
                //{
                //    SelectedDeviceTypeCode = DeviceTypes[0].Code;
                //}
                
            }
        }
        
        public string DeviceCode 
        {
            get
            {
                return _deviceCode;
            }
            set
            {
                _deviceCode = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                QueryDevicesInfo();
            }
        }
        public List<LoopModel> Loops
        {
            get
            {
                return _loops;
            }
            set
            {
                _loops = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());                
            }
        }
        public List<DeviceType> DeviceTypes
        {
            get
            {
                return _deviceTypes;
            }
            set
            {
                _deviceTypes = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());                
            }
        }
        public int SelectedLoopID
        {
            get
            {
                return _selectedLoopID;
            }
            set
            {
                _selectedLoopID = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                QueryDevicesInfo();
            }
        }
        public Int16 SelectedDeviceTypeCode
        {
            get
            {
                return _selectedDeviceTypeCode;
            }
            set
            {
                _selectedDeviceTypeCode = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                QueryDevicesInfo();
            }
        }
        /// <summary>
        /// 取得当前控制器有效的设备类型 
        /// </summary>
        /// <returns></returns>
        private List<DeviceType> GetValidDeviceTypes()
        {   
            List<DeviceType> lstDeviceType = new List<DeviceType>();
            ControllerManager manager = new ControllerManager();
            manager.InitializeAllControllerOperation(null);
            IControllerOperation controllerOperator=manager.GetController(TheController.Type);            
            return   controllerOperator.GetAllDeviceTypeOfController(TheController);      
        }
        /// <summary>
        /// 取得查询结果
        /// </summary>
        private void QueryDevicesInfo()
        {            
            ControllerModel controller = TheController;
            //返回所有的器件类型
            ControllerManager _cManager;
            _cManager = new ControllerManager();
            _cManager.InitializeAllControllerOperation(null);
            IControllerOperation cOperation = _cManager.GetController(controller.Type);
            List<DeviceInfoForSimulator> lstSimulatorDevices = cOperation.GetSimulatorDevices(controller);
            lstSimulatorDevices = ProcessQueryCondition(lstSimulatorDevices);
            DeviceInfoObservableCollection.Clear();
            foreach (var d in lstSimulatorDevices)
            {
                DeviceInfoObservableCollection.Add(d);
            }
        }
        private List<DeviceInfoForSimulator> ProcessQueryCondition(List<DeviceInfoForSimulator> lstDeviceInfo)
        {
            Dictionary<QueryType, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>> dictMap = new Dictionary<QueryType, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>>();
            dictMap.Add(QueryType.QueryByLoop, QueryByLoop);
            dictMap.Add(QueryType.QueryByType, QueryByType);
            dictMap.Add(QueryType.QueryByDeviceCode, QueryByDeviceCode);
            dictMap.Add(QueryType.QueryByLoopAndType, QueryByLoopAndType);
            dictMap.Add(QueryType.QueryByTypeAndDeviceCode, QueryByTypeAndDeviceCode);
            dictMap.Add(QueryType.QueryByLoopAndDeviceCode, QueryByLoopAndDeviceCode);
            dictMap.Add(QueryType.QueryByLoopAndTypeAndDeviceCode, QueryByLoopAndTypeAndDeviceCode);            
            dictMap.Add(QueryType.QueryByNothing, QueryByNothing);            
            System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>> execMethod;
            dictMap.TryGetValue(GetQueryType(), out execMethod);
            return execMethod(lstDeviceInfo);       
        }
        private QueryType GetQueryType()
        {
            QueryType result = QueryType.QueryByLoop;
            if (SelectedLoopID != 9999 && SelectedDeviceTypeCode != 9999 && DeviceCode != "")
            {
                result = QueryType.QueryByLoopAndTypeAndDeviceCode;

            }
            else if (SelectedLoopID != 9999 && SelectedDeviceTypeCode == 9999 && DeviceCode == "")
            {
                result = QueryType.QueryByLoop;
            }
            else if (SelectedLoopID == 9999 && SelectedDeviceTypeCode != 9999 && DeviceCode == "")
            {
                result = QueryType.QueryByType;
            }
            else if (SelectedLoopID == 9999 && SelectedDeviceTypeCode == 9999 && DeviceCode != "")
            {
                result = QueryType.QueryByDeviceCode;
            }
            else if (SelectedLoopID != 9999 && SelectedDeviceTypeCode != 9999 && DeviceCode == "")
            {
                result = QueryType.QueryByLoopAndType;
            }
            else if (SelectedLoopID != 9999 && SelectedDeviceTypeCode == 9999 && DeviceCode != "")
            {
                result = QueryType.QueryByLoopAndDeviceCode;
            }
            else if (SelectedLoopID == 9999 && SelectedDeviceTypeCode != 9999 && DeviceCode != "")
            {
                result = QueryType.QueryByTypeAndDeviceCode;
            }
            else if (SelectedLoopID == 9999 && SelectedDeviceTypeCode == 9999 && DeviceCode == "")
            {
                result = QueryType.QueryByNothing;
            }
            return result;
        }
        private List<DeviceInfoForSimulator> QueryByLoop(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.TypeCode == SelectedDeviceTypeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByDeviceCode(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) =>  d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID && d.TypeCode==SelectedDeviceTypeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByTypeAndDeviceCode(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.TypeCode == SelectedDeviceTypeCode && d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndDeviceCode(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID && d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndTypeAndDeviceCode(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID  && d.TypeCode==SelectedDeviceTypeCode && d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByNothing(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }

    }
}
