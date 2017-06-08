using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.WPF.Utility;
using System.Windows.Input;
using SCA.Model;
using System.Collections.ObjectModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/7 10:36:12
* FileName   : MainViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class MainViewModel:ObservableObject
    {
        private object _currentView;
       // Dictionary<ControllerType, IDeviceInfoViewModel<IDevice>> _dict;
        private IDeviceInfoViewModel<DeviceInfo8036> _deviceInfoViewModel8036;
        private IDeviceInfoViewModel<DeviceInfo8001> _deviceInfoViewModel8001;
        private LinkageConfigStandardViewModel _linkageConfigStandardViewModel;
        private LinkageConfigGeneralViewModel _linkageConfigGeneralViewModel;
        private LinkageConfigMixedViewModel _linkageConfigMixedViewModel;
        private ManualControlBoardViewModel _manualControlBoardViewModel;
        private string _name;
        
        public object CurrentView
        {
            get { return _currentView; }
            private set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }
        //public IDeviceInfoViewModel<DeviceInfo8036>  DeviceInfoViewModel8036
        //{
        //    get{
        //        return _deviceInfoViewModel8036;

        //    }
        //    set{
        //        this._deviceInfoViewModel8036 = value;
        //        RaisePropertyChanged("DeviceInfoViewModel");
        //    }
        //}

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public MainViewModel()
        {
            //_dict = new Dictionary<ControllerType, IDeviceInfoViewModel<IDevice>>();
            _deviceInfoViewModel8036 = new DeviceInfo8036ViewModel();
            _deviceInfoViewModel8001 = new DeviceInfo8001ViewModel();
            _linkageConfigStandardViewModel = new LinkageConfigStandardViewModel();
            _linkageConfigGeneralViewModel = new LinkageConfigGeneralViewModel();
            _linkageConfigMixedViewModel = new LinkageConfigMixedViewModel();
            _manualControlBoardViewModel = new ManualControlBoardViewModel();
          //  IDeviceInfoViewModel<IDevice> deviceViewModel = new DeviceInfoViewModel<IDevice>();
           // _dict.Add(ControllerType.NT8036, deviceViewModel);
        }
        public ICommand NavigateToDetailInfoCommand
        {
            get { return new Test.WPF.Utility.RelayCommand<object>(NavigateToDetailInfoExecute, null); }
        }

        public ICommand NavigateToDeviceInfoCommand
        {
            get { return new Test.WPF.Utility.RelayCommand<object>(NavigateToDeviceInfoExecute, null); }
        }

        public void NavigateToDetailInfoExecute(object o)
        {
            switch(((SCA.Model.ControllerNodeModel)((HierarchyItemViewModel)o).DataItem).Type)
            {
                case ControllerNodeType.Standard:
                    _linkageConfigStandardViewModel.StandardLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigStandard>(((SCA.Model.ControllerModel)((HierarchyItemViewModel)o).Parent.DataItem).StandardConfig);
                    CurrentView = _linkageConfigStandardViewModel;
                    break;
                case ControllerNodeType.General:
                    _linkageConfigGeneralViewModel.GeneralLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigGeneral>(((SCA.Model.ControllerModel)((HierarchyItemViewModel)o).Parent.DataItem).GeneralConfig);
                    CurrentView = _linkageConfigGeneralViewModel;
                    break;
                case ControllerNodeType.Mixed:
                    _linkageConfigMixedViewModel.MixedLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigMixed>(((SCA.Model.ControllerModel)((HierarchyItemViewModel)o).Parent.DataItem).MixedConfig);
                    CurrentView = _linkageConfigMixedViewModel;
                    break;
                case ControllerNodeType.Board:
                    _manualControlBoardViewModel.ManualControlBoardInfoObservableCollection = new ObservableCollection<ManualControlBoard>(((SCA.Model.ControllerModel)((HierarchyItemViewModel)o).Parent.DataItem).ControlBoard);
                    CurrentView = _manualControlBoardViewModel;
                    break;

            }
            string strTest = "Welcom to main command!";
        }

        public void NavigateToDeviceInfoExecute(object o)
        {
            ControllerType controllerType = ((SCA.Model.LoopModel)o).Controller.Type;
            //实例化DeviceInfo8036的ViewModel
            //将器件信息集合作为参数传至DeviceInfo8036ViewModel, 作为其DataContext
            //在DeviceInfo8036View中，绑定其ViewModel
           // _dict[controllerType].DeviceInfoObservableCollection = new ObservableCollection<IDevice>(((SCA.Model.LoopModel)o).GetDevices<DeviceInfo8036>());
           // CurrentView = _dict[controllerType];
            //_dict[controllerType]
           // ObservableCollection<DeviceInfo8036> lstDevices8036=((SCA.Model.LoopModel)o).GetDevices<DeviceInfo8036>();
            switch (controllerType)
            { 
                case ControllerType.NT8036:
           
                    _deviceInfoViewModel8036.DeviceInfoObservableCollection = new ObservableCollection<DeviceInfo8036>(((SCA.Model.LoopModel)o).GetDevices<DeviceInfo8036>());
                  //  DeviceInfoViewModel8036 = new ObservableCollection<DeviceInfo8036>(((SCA.Model.LoopModel)o).GetDevices<DeviceInfo8036>());
                    CurrentView = _deviceInfoViewModel8036;                    
                    break;
                case ControllerType.NT8001:
                    _deviceInfoViewModel8001.DeviceInfoObservableCollection = new ObservableCollection<DeviceInfo8001>(((SCA.Model.LoopModel)o).GetDevices<DeviceInfo8001>());
                    CurrentView = _deviceInfoViewModel8001;
                    break;
            }
            string strTest = "Welcom to main command!";

        }
        public void NavigateToView(object viewToNavigate)
        {
            CurrentView = viewToNavigate;
        }

    }
}
