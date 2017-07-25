using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using SCA.WPF.Utility;
using SCA.Model;
using SCA.Interface;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.BusinessLogic;
using System.Collections.Generic;
using Caliburn.Micro;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/25 10:47:19
* FileName   : DeviceInfoSelectorViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.DeviceItemSelector
{
    public class DeviceItemSelectorViewModel:PropertyChangedBase
    {
        #region 属性
        private int _selectedLoopID;
        private Int16 _selectedDeviceTypeCode;
        private ControllerModel _theController;
        private List<LoopModel> _loops;
        //private IObservableCollection<Dev>
        List<DeviceType> _deviceTypes = null;
        private string _deviceCode = "";//器件编码
        private ObservableCollection<DeviceInfoForSimulator> _deviceInfoObservableCollection;
        //private Visibility _selfVisibility; //当前控件可见性
        private DeviceInfoForSimulator _selectedItem;//表格控件选定的内容
        public ManualControlBoard MCB { get; set; } //需要设置“器件编号”的手控盘对象
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
        //public Visibility SelfVisibility
        //{
        //    get
        //    {
        //        return _selfVisibility;
        //    }
        //    set
        //    {
        //        _selfVisibility = value;
        //        NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
        //    }
        //}

        //器件查询集合        
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
        /// <summary>
        /// 当前选择的器件
        /// </summary>
        public DeviceInfoForSimulator SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        #endregion
        #region 命令
        public ICommand ConfirmCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ConfirmExecute, null); }
        }
        public void ConfirmExecute()
        {
            
            //MCB.DeviceCode=
            if (SelectedItem != null)
            {
                ManualControlBoardService mcbService = new ManualControlBoardService(TheController);
                MCB.DeviceCode = SelectedItem.Code;
                mcbService.Update(MCB);
            }            
            //SelfVisibility = Visibility.Collapsed;                 
            SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshCollection", MCB);
            //SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshNavigator", TheController);
        }
        public ICommand CloseCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CloseExecute, null); }
        }
        public void CloseExecute()
        {
            
            SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshCollection", null);
            //EventMediator.NotifyColleagues("ImportControllerViewClose", null);
            // CloseEvent();
        }
        #endregion
        /// <summary>
        /// 取得当前控制器有效的设备类型 
        /// </summary>
        /// <returns></returns>
        private List<DeviceType> GetValidDeviceTypes()
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();
            ControllerManager manager = new ControllerManager();
            manager.InitializeAllControllerOperation(null);
            IControllerOperation controllerOperator = manager.GetController(TheController.Type);
            return controllerOperator.GetAllDeviceTypeOfController(TheController);
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
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID && d.TypeCode == SelectedDeviceTypeCode).ToList<DeviceInfoForSimulator>();
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
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == SelectedLoopID && d.TypeCode == SelectedDeviceTypeCode && d.Code.StartsWith(DeviceCode)).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByNothing(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
    }
}
