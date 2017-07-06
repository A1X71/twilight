using System;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SCA.Interface.UIWPF;
using SCA.WPF.Utility;
using SCA.WPF.ViewModelsRoot.ViewModels.Navigator;
using SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo;
using SCA.WPF.ImportController;
using SCA.Model;
using Caliburn.Micro;
using SCA.WPF.Infrastructure;
using System.Windows.Threading;
using Ookii.Dialogs.Wpf;
#region 从EXCEL读入数据
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLib;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;

#endregion
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 16:07:32
* FileName   : MainWindowViewModel
* Description: 主页面逻辑
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels
{
    public partial class MainWindowViewModel:PropertyChangedBase
    {
        private object _currentView;
        // Dictionary<ControllerType, IDeviceInfoViewModel<IDevice>> _dict;

        //private IDeviceInfoViewModel<DeviceInfo8036> _deviceInfoViewModel8036; 暂放弃此实现2017-03-30
        //private IDeviceInfoViewModel<DeviceInfo8001> _deviceInfoViewModel8001; 暂放弃此实现2017-03-30

        private DeviceInfo8036ViewModel _deviceInfoViewModel8036;
        private DeviceInfo8001ViewModel _deviceInfoViewModel8001;
        private DeviceInfo8007ViewModel _deviceInfoViewModel8007;
        private DeviceInfo8000ViewModel _deviceInfoViewModel8000;
        private DeviceInfo8003ViewModel _deviceInfoViewModel8003;
        private DeviceInfo8021ViewModel _deviceInfoViewModel8021;

        private LinkageConfigStandardViewModel _linkageConfigStandardViewModel;                
        private LinkageConfigGeneralViewModel _linkageConfigGeneralViewModel;
        private LinkageConfigMixedViewModel _linkageConfigMixedViewModel;
        private ManualControlBoardViewModel _manualControlBoardViewModel;
        //private StatusBarViewModel _statusBarViewModel;
        private DefaultViewModel _defaultViewModel;//窗体右侧默认内容

        private ViewModelsRoot.ViewModels.Query.ProjectSummaryViewModel _projectSummaryViewModel;
        private ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel _summaryViewModel;
        private ViewModelsRoot.ViewModels.Query.LoopSummaryViewModel _loopSummaryViewModel;
        

        private string _name;
        private NavigatorViewModel _navigatingViewModel;
        private bool _blnNavigationStartCommunicationCommandCanExecute=true;//导航控件的"Start"按钮的可用状态 2017-04-20
        private bool _blnNavigationStopCommunicationCommandCanExecute=false;//导航控件的"Stop"按钮的可用状态 
        private DispatcherTimer _autosaveTimer;        //调用自动保存对象
        private string _appIconPath=@"Resources\Icon\Style1\footstone.ico";
        
        private ImportControllerViewModel _importControllerDataContext;////导入控制器数据用户控件的DataContext
        public string AppIconPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + _appIconPath;
            }     
        }
        public NavigatorViewModel NavigatingViewModel
        {
            get
            {
                return _navigatingViewModel;
            }
            
        }
        
        public object CurrentView
        {
            get { return _currentView; }
            private set
            {
                _currentView = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }


        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public ImportControllerViewModel ImportControllerDataContext
        {
            get
            {
                return _importControllerDataContext;
            }
            set
            {
                _importControllerDataContext = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public MainWindowViewModel()
        {            
            _deviceInfoViewModel8036 = new DeviceInfo8036ViewModel();
            _deviceInfoViewModel8001 = new DeviceInfo8001ViewModel();
            _deviceInfoViewModel8007 = new DeviceInfo8007ViewModel();
            _deviceInfoViewModel8000 = new DeviceInfo8000ViewModel();
            _deviceInfoViewModel8003 = new DeviceInfo8003ViewModel();
            _deviceInfoViewModel8021 =new DeviceInfo8021ViewModel();
            _defaultViewModel = new DefaultViewModel();

            _linkageConfigStandardViewModel = new LinkageConfigStandardViewModel();
            _linkageConfigGeneralViewModel = new LinkageConfigGeneralViewModel();
            _linkageConfigMixedViewModel = new LinkageConfigMixedViewModel();
            _manualControlBoardViewModel = new ManualControlBoardViewModel();
            _summaryViewModel = new Query.SummaryInfoViewModel();
            _projectSummaryViewModel = new Query.ProjectSummaryViewModel();
            _loopSummaryViewModel = new Query.LoopSummaryViewModel();
            ImportControllerDataContext = new ImportControllerViewModel();            
            CurrentView = _defaultViewModel;
            AutoSaveExecute();
           //  List<ProjectModel> lstProjects = GetNavigatorInfo();
            //_navigatingViewModel = new NavigatorViewModel(lstProjects,null);            
        }
        public void AutoSaveExecute()
        {
           // _autosaveTimer = new DispatcherTimer(TimeSpan.FromSeconds(SCA.BusinessLib.ProjectManager.GetInstance.AutoSaveInterval()), DispatcherPriority.Background, new EventHandler(SCA.BusinessLib.ProjectManager.GetInstance.DoAutoSave), Application.Current.Dispatcher);
        }
        /// <summary>
        /// 设置Navigator的数据源 2017-03-14
        /// </summary>
        /// <param name="lstProjects"></param>
        public void SetNavigatingViewModel(List<ProjectModel> lstProjects)
        {
           
            _navigatingViewModel = new NavigatorViewModel(lstProjects, null);
        }
        public void CloseNavigatingViewModel()
        {
            _navigatingViewModel = new NavigatorViewModel(null,null);
            CurrentView = _defaultViewModel;
        }
        #region 导航控件--节点导航命令处理
        public ICommand NavigateToDetailInfoCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigateToDetailInfoExecute, null); }
        }

        public ICommand NavigateToDeviceInfoCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigateToDeviceInfoExecute, null); }
        }
        public ICommand NavigateToControllerSummaryCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigateToControllerSummary, null); }
        }
        public ICommand NavigateToProjectSummaryCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigateToProjectSummary, null); }
        }
        //public ICommand NavigateToLoopSummaryCommand
        //{
        //    get { return new SCA.WPF.Utility.RelayCommand<object>(NavigateToLoopSummary, null); }
        //}
        public ICommand DeleteLoopInfoCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(DeleteLoopInfoExecute, null); }
        }
        public ICommand OpenProjectFileCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(OpenProjectFileExecute, null); }
        }
        public void DeleteLoopInfoExecute(object o)
        {
            if (o != null && o.GetType().Name=="LoopModel")
            {
                LoopModel loop = (LoopModel)o;
                SCA.Interface.ILoopService loopService = new SCA.BusinessLib.BusinessLogic.LoopService(loop.Controller);
                loopService.DeleteLoopBySpecifiedLoopCode(loop.Code);                
            }
        }
        public void OpenProjectFileExecute()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "工程文件 (*.nt)|*.nt";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            { 
                ProjectManager.GetInstance.OpenProject(dialog.FileName);
                EventMediator.NotifyColleagues("DisplayTheOpenedProject", null);
            }
        }
        public void NavigateToDetailInfoExecute(object o)
        {
            object item =(SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView)((RoutedEventArgs)o).Source).HierarchyTreeView.SelectedItem;
            switch(((SCA.Model.ControllerNodeModel)((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)item).DataItem).Type)
            {
                case ControllerNodeType.Standard:
                    #region 作废->采用IEditableObject接口集合
                    //_linkageConfigStandardViewModel.StandardLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigStandard>(((SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem).StandardConfig);
                    #endregion
                    
                    _linkageConfigStandardViewModel.TheController = (SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem;                    
                    _linkageConfigStandardViewModel.StandardLinkageConfigInfoObservableCollection = new EditableLinkageConfigStandards(_linkageConfigStandardViewModel.TheController, _linkageConfigStandardViewModel.TheController.StandardConfig);
                    if (_linkageConfigStandardViewModel.TheController.Type != ControllerType.NT8001 && _linkageConfigStandardViewModel.TheController.Type != ControllerType.FT8000)//对于非8001及8000主机的标准组态信息，隐藏部分列
                    {
                        _linkageConfigStandardViewModel.IsVisualColumn = Visibility.Collapsed;
                    }
                    else
                    {
                        _linkageConfigStandardViewModel.IsVisualColumn = Visibility.Visible;
                    }
                    CurrentView = _linkageConfigStandardViewModel;
                    break;
                case ControllerNodeType.General:
                    _linkageConfigGeneralViewModel.TheController = (SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem;
                    _linkageConfigGeneralViewModel.GeneralLinkageConfigInfoObservableCollection = new EditableLinkageConfigGenerals(_linkageConfigGeneralViewModel.TheController, _linkageConfigGeneralViewModel.TheController.GeneralConfig);
                    CurrentView = _linkageConfigGeneralViewModel;
                    break;
                case ControllerNodeType.Mixed:
                    _linkageConfigMixedViewModel.TheController = (SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem;                    
                    _linkageConfigMixedViewModel.MixedLinkageConfigInfoObservableCollection = new EditableLinkageConfigMixeds(_linkageConfigMixedViewModel.TheController, _linkageConfigMixedViewModel.TheController.MixedConfig);
                    CurrentView = _linkageConfigMixedViewModel;
                    break;
                case ControllerNodeType.Board:
                    _manualControlBoardViewModel.TheController=(SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem;                    
                    _manualControlBoardViewModel.ManualControlBoardInfoObservableCollection = new EditableManualControlBoards(_manualControlBoardViewModel.TheController,_manualControlBoardViewModel.TheController.ControlBoard);
                    CurrentView = _manualControlBoardViewModel;
                    break;
                case ControllerNodeType.Loop:
                    _loopSummaryViewModel.TheController=(SCA.Model.ControllerModel)((NavigatorItemViewModel)item).Parent.DataItem;                    
                    CurrentView = _loopSummaryViewModel;
                    break;

            }          
        }

        public void NavigateToDeviceInfoExecute(object o)
        {
            NavigatorItemViewModel item = (SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView)((RoutedEventArgs)o).Source).HierarchyTreeView.SelectedItem;
            LoopModel loop = (SCA.Model.LoopModel)item.DataItem;
            ControllerType controllerType = loop.Controller.Type;           
            switch (controllerType)
            { 
                case ControllerType.NT8036:
                    _deviceInfoViewModel8036.TheLoop = loop;                    
                    _deviceInfoViewModel8036.DeviceInfoObservableCollection = new EditableDeviceInfo8036Collection(loop,loop.GetDevices<DeviceInfo8036>());                  
                    CurrentView = _deviceInfoViewModel8036;                    
                    break;
                case ControllerType.NT8001:
                    _deviceInfoViewModel8001.TheLoop = loop;                    
                    _deviceInfoViewModel8001.DeviceInfoObservableCollection = new EditableDeviceInfo8001Collection(loop,loop.GetDevices<DeviceInfo8001>());                  
                    CurrentView = _deviceInfoViewModel8001;                    
                    break;
                case ControllerType.NT8007:
                    _deviceInfoViewModel8007.TheLoop = loop;                    
                    _deviceInfoViewModel8007.DeviceInfoObservableCollection = new EditableDeviceInfo8007Collection(loop, loop.GetDevices<DeviceInfo8007>());                  
                    CurrentView = _deviceInfoViewModel8007;
                    break;
                case ControllerType.FT8000:
                    _deviceInfoViewModel8000.TheLoop = loop;
                    _deviceInfoViewModel8000.DeviceInfoObservableCollection = new EditableDeviceInfo8000Collection(loop, loop.GetDevices<DeviceInfo8000>());
                    CurrentView = _deviceInfoViewModel8000;
                    break;
                case ControllerType.FT8003:
                    _deviceInfoViewModel8003.TheLoop = loop;
                    _deviceInfoViewModel8003.DeviceInfoObservableCollection = new EditableDeviceInfo8003Collection(loop, loop.GetDevices<DeviceInfo8003>());
                    CurrentView = _deviceInfoViewModel8003;
                    break;
                case ControllerType.NT8021:
                    _deviceInfoViewModel8021.TheLoop = loop;
                    _deviceInfoViewModel8021.DeviceInfoObservableCollection = new EditableDeviceInfo8021Collection(loop, loop.GetDevices<DeviceInfo8021>());
                    CurrentView = _deviceInfoViewModel8021;
                    break;
            }
        }

        public void NavigateToControllerSummary(object o)
        {            
            object item = (SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView)((RoutedEventArgs)o).Source).HierarchyTreeView.SelectedItem;
            _summaryViewModel.SetController((SCA.Model.ControllerModel)((NavigatorItemViewModel)item).DataItem);
            CurrentView = _summaryViewModel;            
        }

        public void NavigateToProjectSummary(object o)
        {
            object item = (SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView)((RoutedEventArgs)o).Source).HierarchyTreeView.SelectedItem;
            _projectSummaryViewModel.ProjectName = ((SCA.Model.ProjectModel)((NavigatorItemViewModel)item).DataItem).Name;
            CurrentView = _projectSummaryViewModel;  
        }
        //public void NavigateToLoopSummary(object o)
        //{
        //    object item = (SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((SCA.WPF.ViewsRoot.Views.Navigator.NavigatorView)((RoutedEventArgs)o).Source).HierarchyTreeView.SelectedItem;
        //    _loopSummaryViewModel.TheController = (SCA.Model.ControllerModel)((NavigatorItemViewModel)item).DataItem;
        //    //.ProjectName = ((SCA.Model.ProjectModel)((NavigatorItemViewModel)item).DataItem).Name;
        //    CurrentView = _loopSummaryViewModel;  
        //}
        public void NavigateToView(object viewToNavigate)
        {
            CurrentView = viewToNavigate;
        }

        #endregion

        #region 导航控件--按钮命令 2017-04-20增加代码，由于没有控制器，未验证

        //原前台绑定代码
        //StartControllerButtonClick="NaviagtorUserControl_StartControllerButtonClick"
        //StopControllerButtonClick="NaviagtorUserControl_StopControllerButtonClick"                                                 


        public ICommand NavigatorUserControl_StartCommunicationCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigatorUserControl_StartCommunicationExecute, Navigation_StartCommunicationCommand_CanExecute); }
        }

        public ICommand NavigatorUserControl_StopCommunicationCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigatorUserControl_StopCommunicationExecute, Navigation_StopCommunicationCommand_CanExecute); }
        }
        public ICommand NavigatorUserControl_MergeForControllerCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(NavigatorUserControl_MergeForControllerExecute, null); }
        }

        private void NavigatorUserControl_StartCommunicationExecute(object o)
        {
            //取得当前的控制器信息
            //ControllerModel controller = (SCA.Model.ControllerModel)((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)o).DataItem; 2017-04-06注释 ，调试8003时报错
            ControllerModel controller = (SCA.Model.ControllerModel)((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((RoutedEventArgs)o).OriginalSource).DataItem;            
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            //为通信组件指定控制器信息
            iCC.TheController = controller;
            iCC.StartCommunication();
            if (iCC.GetPortStatus()) //打开成功时才置可用状态
            {
                _blnNavigationStartCommunicationCommandCanExecute = false;
                _blnNavigationStopCommunicationCommandCanExecute = true;
            }
        }
        private bool Navigation_StartCommunicationCommand_CanExecute(object o)
        {
            if (_blnNavigationStartCommunicationCommandCanExecute)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void NavigatorUserControl_StopCommunicationExecute(object o)
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            iCC.StopCommunication();
            if (!iCC.GetPortStatus()) //打开成功时才置可用状态
            {
                _blnNavigationStartCommunicationCommandCanExecute = true;
                _blnNavigationStopCommunicationCommandCanExecute = false;
            }
        }
        private bool Navigation_StopCommunicationCommand_CanExecute(object o)
        {
            if (_blnNavigationStopCommunicationCommandCanExecute)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //private void Navigation_CommunicationCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    if (_blnNavigationCommunicationCommandCanExecute)
        //    {
        //        e.CanExecute = true;
        //    }
        //    else
        //    {
        //        e.CanExecute = false;
        //    }
        //}
        private void NavigatorUserControl_MergeForControllerExecute(object o )
        {            
            ControllerModel controller = (SCA.Model.ControllerModel)((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)((RoutedEventArgs)o).OriginalSource).DataItem;            

            //ImportControllerViewModel  importControllerVM=new ImportControllerViewModel();
            //importControllerVM.TheController = controller;
            //importControllerVM.ImportControllerVisibilityState = Visibility.Visible;
            //ImportControllerDataContext = importControllerVM;
            ImportControllerDataContext.TheController = controller;
            ImportControllerDataContext.ImportControllerVisibilityState = Visibility.Visible;
        }
        #endregion

        #region 测试Summary命令
        public ICommand SetNavigatorDataCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(SetNavigatorDataExecute, null); }
        }
        public void SetNavigatorDataExecute(object o)
        {
            List<SCA.Model.ProjectModel> lstProject = new List<SCA.Model.ProjectModel>();
            lstProject.Add(SCA.BusinessLib.ProjectManager.GetInstance.Project);
            NavigatingViewModel.UpdateControllerInfo((ControllerModel)((RoutedEventArgs)o).OriginalSource);
            
            SetNavigatingViewModel(lstProject);
        }
        #endregion

        /// <summary>
        /// 打开老版本文件的数据.
        /// </summary>
        /// <returns></returns>
        private List<ProjectModel> GetNavigatorInfo()
        {
            IProjectManager projManager = ProjectManager.GetInstance;
            IFileService _fileService = new FileService();
            IDatabaseService _databaseService = new MSAccessDatabaseAccess(@"C:\Users\Administrator\Desktop\foo\8036.mdb", null, _fileService);
            IOldVersionSoftwareDBService oldVersionService = new OldVersionSoftware8036DBService(_databaseService);
            IControllerOperation controllerOperation = null;

            string[] strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            ControllerModel controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8036":
                        controllerOperation = new ControllerOperation8036(_databaseService);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }


            ProjectModel proj = new ProjectModel() { ID = 1, Name = "尼特智能" };
            proj.Controllers.Add(controllerInfo);



            _databaseService = new MSAccessDatabaseAccess(@"E:\2016\6 软件优化升级\4 实际工程数据\工程数据111\连城心怡都城_文件版本5.mdb", null, _fileService);
            oldVersionService = new OldVersionSoftware8001DBService(_databaseService);

            strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8001":
                        controllerOperation = new ControllerOperation8001(null);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }

            proj.Controllers.Add(controllerInfo);

            projManager.CreateProject(proj);

            List<ProjectModel> lstProjects = new List<ProjectModel>();
            lstProjects.Add(proj);
            return lstProjects;
        }

        public void NewProject()
        { 
        
        }

        /// <summary>
        /// 删除控制器信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool DeleteControllerBySpecifiedControllerID(ControllerModel controller)
        {
            try
            {
                if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                { 
                    ControllerManager controllerManager = new ControllerManager();
                    controllerManager.InitializeAllControllerOperation(null);
                    IControllerOperation controllerOperation = controllerManager.GetController(controller.Type);
                    controllerOperation.DeleteControllerBySpecifiedControllerID(controller.ID);
                }
            }
            catch
            {
                return false;
            }
            return true;
            //controllerOperation.AddControllerToProject(controller);
        }


    }
}
