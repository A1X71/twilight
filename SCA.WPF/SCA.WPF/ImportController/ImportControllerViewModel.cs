using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using System.Collections.Generic;
using Caliburn.Micro;
using SCA.WPF.Utility;
using Ookii.Dialogs.Wpf;
using SCA.Model;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.Interface;
using SCA.WPF.Infrastructure;
using SCA.WPF.ImportContentSelector;
using System.ComponentModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/27 13:42:34
* FileName   : ImportControllerViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ImportController
{
    public class ImportControllerViewModel:PropertyChangedBase
    {
        private IControllerOperation _operation;
        public ImportControllerViewModel()
        { 
            ImportContentSelectorViewModel importContentSelectorVM = new ImportContentSelectorViewModel();
            ImportContentSelectorDataContext = importContentSelectorVM;
            EventMediator.Register("CancelExcelImport", CancelExcelImport);
            ToggleButtonStateForReadingExcel(false);
            GetValidDeviceType(SelectedControllerType);
            SetSelectedDeviceTypeCode(SelectedControllerType);
            EventMediator.Register("ImportControllerViewClose", CloseViewExecute);
            
            
        }

        #region 属性
        private bool _excelFormatState = true;
        private bool _mdbFormatState = false;
        private bool _ntFormatState = false;
        private bool _defaultTemplateState=true;
        private bool _customizedTemplateState=false;
        private bool _readingButtonEnableFlag = true; //读取按钮“Enable”标识
        private bool _downloadEXCELTemplateEnableFlag = true; //模板下载按钮"Enable"标识
        private bool _closeButtonEnableFlag = true; //“关闭按钮”Eanble标识
        private bool _excelTemplateRadioPanelFlag = true;//“默认模板"”自定义模板”切换容器
        private bool _importTypePanelFlag = true; //"导入类型"选择容器
        private Visibility _excelFormatContentSettingVisibility=Visibility.Visible;
        private Visibility _excelVersionPromptVisibility = Visibility.Collapsed; //EXCEL版本提示信息可见性
        private string _controllerName="";
        private int _machineNumber = 0;        
        private List<DeviceType> _validDeviceType; //有效设备类型 
        private Int16 _selectedDeviceTypeCode=-1;
        
        private string _serialPortNumber = "COM1";
        private int _loopAmount = 1;//回路数量
        private int _loopGroupAmount = 1;//回路分组数量
        private bool _standardLinkageFlag = true; //标准组态复选框状态
        private bool _generalLinkageFlag = true;  //通用组态复选框状态
        private bool _mixedLinkageFlag = true;    //混合组态复选框状态
        private bool _manualControlBoardFlag = true;//网络手动盘复选框状态
        private string _excelFilePath = ""; //EXCEL导入文件地址
        private Visibility _importControllerVisibilityState = Visibility.Collapsed;//导入控制器数据用户控件的可见性状态
        private Visibility _excelFormatSettingContentCustomizedVisibility = Visibility.Collapsed; //EXCEL导入格式,自定义导入模板
        private ControllerType _controllerType=ControllerType.NT8001;
        private List<int> _lstDeviceCodeLength;//器件地址长度集合
        private int _selectedDeviceCodeLength = 7; //器件地址长度
        private ImportContentSelectorViewModel _importContentSelectorDataContext;//选择面板DataContext
        private Visibility _configSection = Visibility.Visible;   //配置部分的显示属性
        private Visibility _importSection = Visibility.Collapsed; //导入部分的显示属性
        private EXCELVersion _excelVersionForTemplate = EXCELVersion.EXCEL2003; //EXCEL模板版本
        private Visibility _otherSettingsPanelVisibility = Visibility.Visible; //自定义EXCEL模板，"其它设置"容器可见性
        private Visibility _standardLinkageCheckBoxVisibility = Visibility.Visible; //自定义EXCEL模板，标准组态复选框可见性
        private Visibility _mixedLinkageCheckBoxVisibility = Visibility.Visible; //自定义EXCEL模板，混合组态复选框可见性
        private Visibility _generalLinkageCheckBoxVisibility = Visibility.Visible; //自定义EXCEL模板，通用组态复选框可见性
        private Visibility _manualControlBoardCheckBoxVisibility = Visibility.Visible; //自定义EXCEL模板，网络手动盘复选框可见性

        public ControllerModel TheController { get; set; }//当前控制器
        public bool ExcelFormatState
        {
            get
            {
                return _excelFormatState;
            }
            set
            {
                _excelFormatState = value;
                ToggleFileFormatDisplayContent();
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public EXCELVersion ExcelVersionForTemplate
        {
            get
            {
                return _excelVersionForTemplate;
            }
            set
            {
                _excelVersionForTemplate = value;
                if (_excelVersionForTemplate == EXCELVersion.EXCEL2007) //显示提示信息
                {
                    ExcelVersionPromptVisibility = Visibility.Visible;
                }
                else
                {
                    ExcelVersionPromptVisibility = Visibility.Collapsed;
                }
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //if (_excelFormatState)
        //{
        //    MDBFormatState = false;
        //    NTFormatState = false;
        //    ExcelFormatSettingContentVisibility = Visibility.Visible;
        //}
        //else
        //{
        //    MDBFormatState = true;
        //    NTFormatState = true;
        //    ExcelFormatSettingContentVisibility = Visibility.Collapsed;
        //}
        public bool MDBFormatState
        {
            get
            {
                return _mdbFormatState;
            }
            set
            {
                _mdbFormatState = value;
                //if (_mdbFormatState)
                //{
                //    ExcelFormatState = false;
                //    NTFormatState = false;
                //}
                //else
                //{
                //    ExcelFormatState = true;
                //    NTFormatState = true;
                //}
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool NTFormatState
        {
            get
            {
                return _ntFormatState;
            }
            set
            {
                _ntFormatState = value;
                //if (_ntFormatState)
                //{
                //    ExcelFormatState = false;
                //    MDBFormatState = false;
                //}
                //else
                //{
                //    ExcelFormatState = true;
                //    MDBFormatState = true;
                //}
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        /// <summary>
        /// Excel格式设置信息可见性
        /// </summary>
        public Visibility ExcelFormatSettingContentVisibility
        {
            get
            {
                return _excelFormatContentSettingVisibility;
            }
            set
            {
                _excelFormatContentSettingVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool DefaultTemplateState
        {
            get
            {
                return _defaultTemplateState;
            }
            set
            {
                _defaultTemplateState = value;
                ToggleExcelTemplateSettingDisplayContent();
                //if (_defaultTemplateState)
                //{
                //    CustomizedTemplateState = false;
                //}
                //else
                //{
                //    CustomizedTemplateState = true;
                //}
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool CustomizedTemplateState
        {
            get
            {
                return _customizedTemplateState;
            }
            set
            {
                _customizedTemplateState = value;
                //if (_customizedTemplateState)
                //{
                //    DefaultTemplateState = false;
                //}
                //else
                //{
                //    DefaultTemplateState = true;
                //}
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }        
        }
        public string ControllerName
        {
            get
            {
                return _controllerName;
            }
            set
            {
                _controllerName = value;        
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }      
        }
        public int MachineNumber
        {
            get
            {
                return _machineNumber;
            }
            set
            {
                _machineNumber = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }   
        }
        public List<int> DeviceCodeLength
        {
            get
            {
                if (_lstDeviceCodeLength == null)
                {
                    _lstDeviceCodeLength = new List<int>();
                    IControllerConfig config = ControllerConfigManager.GetConfigObject(_controllerType);
                    _lstDeviceCodeLength = config.GetDeviceCodeLength();
                }
                return _lstDeviceCodeLength;
            }
            set
            {
                _lstDeviceCodeLength = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string SerialPortNumber
        {
            get
            {
                return _serialPortNumber;
            }
            set
            {
                _serialPortNumber = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public int LoopAmount
        {
            get
            {
                return _loopAmount;
            }
            set
            {
                _loopAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public int LoopGroupAmount
        {
            get
            {
                return _loopGroupAmount;
            }
            set
            {
                _loopGroupAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool StandardLinkageFlag
        {
            get
            {
                return _standardLinkageFlag;
            }
            set
            {
                _standardLinkageFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool GeneralLinkageFlag
        {
            get
            {
                return _generalLinkageFlag;
            }
            set
            {
                _generalLinkageFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool MixedLinkageFlag
        {
            get
            {
                return _mixedLinkageFlag;
            }
            set
            {
                _mixedLinkageFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ExcelFilePath
        {
            get
            {
                return _excelFilePath;
            }
            set
            {
                _excelFilePath = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ImportControllerVisibilityState
        {
            get
            {               
                return _importControllerVisibilityState;
            }
            set
            {
                _importControllerVisibilityState = value;
                if(_importControllerVisibilityState==Visibility.Visible)
                {
                    ConfigSection = Visibility.Visible;
                }
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ExcelFormatSettingContentCustomizedVisibility
        {
            get
            {
                return _excelFormatSettingContentCustomizedVisibility;
            }
            set
            {
                _excelFormatSettingContentCustomizedVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }        
        public Visibility ExcelVersionPromptVisibility
        {
            get
            {
                return _excelVersionPromptVisibility;
            }
            set
            {
                _excelVersionPromptVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool ManualControlBoardFlag
        {
            get
            {
                return _manualControlBoardFlag;
            }
            set
            {
                _manualControlBoardFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public int SelectedDeviceCodeLength
        {
            get
            {
                return _selectedDeviceCodeLength;
            }
            set
            {
                _selectedDeviceCodeLength = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public ImportContentSelectorViewModel ImportContentSelectorDataContext
        {
            get
            {
                return _importContentSelectorDataContext;
            }
            set
            {
                _importContentSelectorDataContext = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //读取按钮“Enable”标识
        public bool ReadingButtonEnableFlag
        {
            get
            {
                return _readingButtonEnableFlag;
            }
            set
            {
                _readingButtonEnableFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //模板下载按钮"Enable"标识
        public bool DownloadExcelTemplateEnableFlag
        {
            get
            { 
                return _downloadEXCELTemplateEnableFlag;
            }
            set
            { 
                _downloadEXCELTemplateEnableFlag=value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //“关闭按钮”Eanble标识
        public bool CloseButtonEnableFlag
        {
            get
            { 
                return _closeButtonEnableFlag;
            }
            set
            {
                _closeButtonEnableFlag=value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //“默认模板"”自定义模板”切换容器Enable标识        
        public bool ExcelTemplateRadioPanelFlag
        {
            get
            {
                return _excelTemplateRadioPanelFlag;
            }
            set
            {
                _excelTemplateRadioPanelFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //"导入类型"选择容器Enable标识        
        public bool ImportTypePanelFlag
        {
            get
            {
                return _importTypePanelFlag;
            }
            set
            {
                _importTypePanelFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //自定义EXCEL模板，"其它设置"容器可见性
        public Visibility OtherSettingsPanelVisibility
        {
            get 
            {
                return _otherSettingsPanelVisibility;
            }
            set
            {
                _otherSettingsPanelVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }            
        //自定义EXCEL模板，标准组态复选框可见性
        public Visibility StandardLinkageCheckBoxVisibility
        {
            get
            {
                return _standardLinkageCheckBoxVisibility;
            }
            set
            {
                _standardLinkageCheckBoxVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //自定义EXCEL模板，混合组态复选框可见性
        public Visibility MixedLinkageCheckBoxVisibility
        {
            get
            {
                return _mixedLinkageCheckBoxVisibility;
            }
            set
            {
                _mixedLinkageCheckBoxVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //自定义EXCEL模板，通用组态复选框可见性
        public Visibility GeneralLinkageCheckBoxVisibility
        {
            get
            {
                return _generalLinkageCheckBoxVisibility;
            }
            set
            {
                _generalLinkageCheckBoxVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //自定义EXCEL模板，网络手动盘复选框可见性
        public Visibility ManualControlBoardCheckBoxVisibility
        {
            get
            {
                return _manualControlBoardCheckBoxVisibility;
            }
            set
            {
                _manualControlBoardCheckBoxVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public List<DeviceType> ValidDeviceType
        {
            get
            {
                return _validDeviceType;
            }
            set
            {
                _validDeviceType = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                SetSelectedDeviceTypeCode(SelectedControllerType);
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
            }
        }
        
        #endregion
        #region 命令
        /// <summary>
        /// 从EXCEL文件导入数据命令
        /// </summary>
        public ICommand ImportDataFromExcelCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ImportDataFromExcelExecute, null); }
        }
        public void ImportDataFromExcelExecute()
        {
            if (ExcelFilePath != "")
            {              
                FileService fileService = new FileService();                
                //ControllerOperation8001 operation = new ControllerOperation8001();
                ControllerManager controllerManager = new ControllerManager();            
                controllerManager.InitializeAllControllerOperation(null);
                //IControllerOperation controllerOperation = controllerManager.GetController(SelectedControllerType);                
                _operation= controllerManager.GetController(TheController.Type);                
                //ControllerOperationCommon.ProgressBarCancelFlag = false;
                _operation.SetStaticProgressBarCancelFlag(false);
                //_operation
                //_operation = new ControllerOperation8001();
                _operation.UpdateProgressBarEvent += UpdateProgressBarUI;
                _operation.ReadingExcelCompletedEvent += ReadingExcelComplete;
                _operation.ReadingExcelCancelationEvent += CanceledExcelImportHandler;
                _operation.ReadingExcelErrorEvent += ReadingExcelErrorHandler;
                string strErrorMessage;
                ReadExcelLoopArgumentForIn param = new ReadExcelLoopArgumentForIn();
                param.FilePath= ExcelFilePath;
                param.FileService = fileService;
                param.Controller = TheController;


              //  System.Threading.Thread thread = new System.Threading.Thread(Read);                
              //  thread.Start(param);
                Read(param);
                //SCA.BusinessLib.ProjectManager.GetInstance.TheControllerViaImporting = _operation.ReadEXCELTemplate(ExcelFilePath, fileService,TheController,out strErrorMessage);

                //#region 显示数据选择页面
                //ConfigSection = Visibility.Collapsed;
                //ImportContentSelectorViewModel importContentSelectorVM = new ImportContentSelectorViewModel();
                //importContentSelectorVM.TheController = TheController;
                //importContentSelectorVM.SelfVisibility = Visibility.Visible;
                //importContentSelectorVM.ImportDataSelectorVisibility = Visibility.Visible;
                //importContentSelectorVM.InitilizeData(strErrorMessage);
                //ImportContentSelectorDataContext = importContentSelectorVM;
                ////importContentSelectorVM=
                //#endregion
                ToggleButtonStateForReadingExcel(true);

            }
        }
        private void Read(object param)
        { 
            ReadExcelLoopArgumentForIn paramValue=(ReadExcelLoopArgumentForIn)param;
            _operation.ReadEXCELTemplate(paramValue.FilePath, paramValue.FileService, paramValue.Controller);
        }
        /// <summary>
        /// 取消导入操作
        /// </summary>
        /// <param name="args"></param>
        public void CancelExcelImport(object args)
        {

            //ControllerOperationCommon.ProgressBarCancelFlag = true;
            _operation.SetStaticProgressBarCancelFlag(true);
            //_operation.ProgressBarCancelFlag = true;
        }
        public void CanceledExcelImportHandler(ControllerModel controller ,string errorMessage)
        {
            EventMediator.NotifyColleagues("DisappearProgressBar", null);
            ToggleButtonStateForReadingExcel(false);
            if(errorMessage!=""  && errorMessage!=null ) //需要显示错误信息
            {
                DisplayContentSelectorPage(errorMessage);
            }
        }
        public void ReadingExcelErrorHandler(string errorMessage)
        {
            EventMediator.NotifyColleagues("DisappearProgressBar", null);
            ToggleButtonStateForReadingExcel(false);
            if (errorMessage != "" && errorMessage != null) //需要显示错误信息
            {
                DisplayContentSelectorPage(errorMessage);
            }
        }
        public void UpdateProgressBarUI(int currentValue)
        {
            object[] args = new object[2];
            args[0] = currentValue;
            args[1] = 100;            
            EventMediator.NotifyColleagues("UpdateProgressBarStatusForExcelReading", args);//读取EXCEL时的进度条
        }
        void ReadingExcelComplete(ControllerModel controller,string errorMessage)
        {
            EventMediator.NotifyColleagues("DisappearProgressBar", null);
            SCA.BusinessLib.ProjectManager.GetInstance.TheControllerViaImporting = controller;
            ToggleButtonStateForReadingExcel(false);
            DisplayContentSelectorPage(errorMessage);
        }
        private void DisplayContentSelectorPage(string errorMessage)
        {
            #region 显示数据选择页面
            ConfigSection = Visibility.Collapsed;
            ImportContentSelectorViewModel importContentSelectorVM = new ImportContentSelectorViewModel();
            importContentSelectorVM.TheController = TheController;
            importContentSelectorVM.SelfVisibility = Visibility.Visible;
            importContentSelectorVM.ImportDataSelectorVisibility = Visibility.Visible;
            importContentSelectorVM.InitilizeData(errorMessage);
            ImportContentSelectorDataContext = importContentSelectorVM;
            #endregion
        }
        public ICommand SelectExcelPathCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(SelectExcelPathExecute, null); }  
        }
        public void SelectExcelPathExecute()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "EXCEL2003文件 (*.xls)|*.xls|EXCEL2007文件(*.xlsx)|*.xlsx";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                ExcelFilePath = dialog.FileName;
            }
        }
        /// <summary>
        /// 下载EXCEL模板命令
        /// </summary>
        public ICommand DownloadExcelTemplateCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(DownloadExcelTemplateExecute, null); }  
        }
        public void DownloadExcelTemplateExecute()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择存储的文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            bool? blnResult=dialog.ShowDialog();
            if (blnResult!=null)
            {
                if ((bool)blnResult)
                { 
                    string strFilePath = dialog.SelectedPath;
                    FileService fileService=new FileService();
                    string suffixString = "";
                    if (ExcelVersionForTemplate == EXCELVersion.EXCEL2003)
                    {
                        suffixString = ")模板文件.xls";
                    }
                    else
                    {
                        suffixString = ")模板文件.xlsx";
                    }
                    ControllerManager controllerManager = new ControllerManager();
                    controllerManager.InitializeAllControllerOperation(null);
                    IControllerOperation operation = controllerManager.GetController(SelectedControllerType);

                    if (DefaultTemplateState)
                    {                
                
                        //ControllerOperation8001 operation = new ControllerOperation8001();
                    // operation.DownloadDefaultEXCELTemplate(strFilePath + "//" + "默认(" + SelectedControllerType.ToString() + suffixString, fileService, null);
                        operation.DownloadDefaultEXCELTemplate(strFilePath + "//" + "默认(" + SelectedControllerType.ToString() + suffixString, fileService, null, SelectedControllerType);
                    }
                    else if (CustomizedTemplateState)
                    {
                        ExcelTemplateCustomizedInfo customizedInfo = new ExcelTemplateCustomizedInfo();
                        customizedInfo.ControllerType = SelectedControllerType;
                        customizedInfo.ControllerName = ControllerName;//应该限制此控制器名称的长度
                        customizedInfo.MachineNumber = MachineNumber;
                        customizedInfo.SelectedDeviceCodeLength = SelectedDeviceCodeLength;
                        customizedInfo.SerialPortNumber = SerialPortNumber;
                        customizedInfo.LoopAmount = LoopAmount;
                        customizedInfo.LoopGroupAmount = LoopGroupAmount;
                        customizedInfo.StandardLinkageFlag = StandardLinkageFlag;
                        customizedInfo.MixedLinkageFlag = MixedLinkageFlag;
                        customizedInfo.GeneralLinkageFlag = GeneralLinkageFlag;
                        customizedInfo.ManualControlBoardFlag = ManualControlBoardFlag;
                        customizedInfo.DefaultDeviceTypeCode = SelectedDeviceTypeCode;
                       // ControllerOperation8001 operation = new ControllerOperation8001(); //暂时写为8001,后续需要改为各控制器通用代码
                        string fileName="";
                        if(ControllerName.Length>10)
                        {
                            fileName = fileName + ControllerName.Substring(0, 10) + "控制器(" + SelectedControllerType.ToString() + suffixString;
                        }
                        else
                        {
                            fileName = fileName + ControllerName + "控制器(" + SelectedControllerType.ToString() + suffixString;
                        }

                    //    operation.DownloadDefaultEXCELTemplate(strFilePath + "//" +fileName , fileService, customizedInfo);
                        operation.DownloadDefaultEXCELTemplate(strFilePath + "//" + fileName, fileService, customizedInfo, SelectedControllerType);
                    }
                }
            }
        }
        /// <summary>
        /// 关闭当前View命令
        /// </summary>
        public ICommand CloseViewCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CloseViewExecute, null); }  
        }
        public void CloseViewExecute()
        {
            ImportControllerVisibilityState = Visibility.Collapsed;
        }
        private void CloseViewExecute(object o)
        {
            CloseViewExecute();
        }
        /// <summary>
        /// 控制器类型选择后执行命令
        /// </summary>
        public ICommand ControllerTypeChangedCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(ControllerTypeChangedExecute, null); }  
        }
        public void ControllerTypeChangedExecute(object param)
        {
            if (param != null)
            {
                if (param.GetType().ToString() == "SCA.Model.ControllerType")
                {
                    SelectedControllerType = (ControllerType)param;
                    GetDeviceCodeLength(SelectedControllerType);
                    ToggleOtherSettingsVisibility(SelectedControllerType);
                    GetValidDeviceType(SelectedControllerType);
                }
            }
        }
        public ControllerType SelectedControllerType
        {
            get
            {
                return _controllerType;
            }
            set
            {
                _controllerType = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        
        public List<int> GetDeviceCodeLength(Model.ControllerType type)
        {
            SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(type);
            DeviceCodeLength = controllerConfig.GetDeviceCodeLength();
            return DeviceCodeLength;
        }
        /// <summary>
        /// 根据控制器的配置信息决定是否显示其它配置项
        /// </summary>
        /// <param name="type"></param>
        private void ToggleOtherSettingsVisibility(Model.ControllerType type)
        {
            SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(type);
            ControllerNodeModel[]  nodes = controllerConfig.GetNodes();
            bool blnStandardFlag = false;
            bool blnMixedFlag = false;
            bool blnGeneralFlag = false;
            bool blnMCBFlag = false;

            if (nodes.Length > 1)
            {
                OtherSettingsPanelVisibility = Visibility.Visible;
            }
            else
            {
                OtherSettingsPanelVisibility = Visibility.Collapsed;                
            }
            for (int i = 0; i < nodes.Length; i++)
            {
                switch(nodes[i].Type)
                {                    
                    case ControllerNodeType.Standard:
                        blnStandardFlag = true;                        
                        break;
                    case ControllerNodeType.General:
                        blnGeneralFlag = true;                        
                        break;
                    case ControllerNodeType.Mixed:
                        blnMixedFlag = true;                        
                        break;
                    case ControllerNodeType.Board:
                       blnMCBFlag = true;                        
                        break;
                }
             }  
            if (blnStandardFlag)
            {
                StandardLinkageCheckBoxVisibility = Visibility.Visible;
                StandardLinkageFlag = true;
            }
            else
            {
                StandardLinkageCheckBoxVisibility = Visibility.Collapsed;
                StandardLinkageFlag = false;
            }
            if (blnGeneralFlag)
            {
                GeneralLinkageCheckBoxVisibility = Visibility.Visible;
                GeneralLinkageFlag = true;
            }
            else
            {
                GeneralLinkageCheckBoxVisibility = Visibility.Collapsed;
                GeneralLinkageFlag = false;
            }
            if (blnMixedFlag)
            {
                MixedLinkageCheckBoxVisibility = Visibility.Visible;
                MixedLinkageFlag = true;
            }
            else
            {
                MixedLinkageCheckBoxVisibility = Visibility.Collapsed;
                MixedLinkageFlag = false;
            }
            if (blnMCBFlag)
            {
                ManualControlBoardCheckBoxVisibility = Visibility.Visible;
                ManualControlBoardFlag = true;
            }
            else
            {
                ManualControlBoardCheckBoxVisibility = Visibility.Collapsed;
                ManualControlBoardFlag = false;
            }

            return;
        }

        public Visibility ConfigSection
        {
            get
            {
                return _configSection;
            }
            set
            {
                _configSection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ImportSection
        {
            get
            {
                return _importSection;
            }
            set
            {
                _importSection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        //取得EXCEL版本
        public List<EXCELVersion> GetExcelVersion()
        {
            List<EXCELVersion> lstExcelVersion = new List<EXCELVersion>();
            foreach ( EXCELVersion version in Enum.GetValues(typeof(EXCELVersion)))
            {
                lstExcelVersion.Add(version);
            }
            return lstExcelVersion;
        }

        #endregion
        #region 私有方法
        /// <summary>
        /// 切换不同文件格式的显示内容
        /// </summary>
        private void ToggleFileFormatDisplayContent()
        {
            if (ExcelFormatState)
            {
                ExcelFormatSettingContentVisibility = Visibility.Visible;
            }
            else
            {
                ExcelFormatSettingContentVisibility = Visibility.Collapsed;
            }        
        }
        /// <summary>
        /// 切换EXCEL自定义模板显示内容
        /// </summary>
        private void ToggleExcelTemplateSettingDisplayContent()
        {
            if (DefaultTemplateState)
            {
                ExcelFormatSettingContentCustomizedVisibility = Visibility.Collapsed;
            }
            else
            {
                ExcelFormatSettingContentCustomizedVisibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 切换读取EXCEL时的按钮状态
        /// </summary>
        /// <param name="isReading"></param>
        private void ToggleButtonStateForReadingExcel(bool isReading)
        {
            if(isReading)
            {
                ReadingButtonEnableFlag=false;
                DownloadExcelTemplateEnableFlag=false;
                CloseButtonEnableFlag=false;
                ImportTypePanelFlag=false;
                ExcelTemplateRadioPanelFlag = false;
            }
            else
            {
                ReadingButtonEnableFlag=true;
                DownloadExcelTemplateEnableFlag=true;
                CloseButtonEnableFlag=true;
                ImportTypePanelFlag=true;
                ExcelTemplateRadioPanelFlag = true;
            }
        }
        public List<DeviceType> GetValidDeviceType(ControllerType type)
        {
            SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(type);
            ValidDeviceType = controllerConfig.GetDeviceTypeInfo();            
            return ValidDeviceType;
        }
        public void SetSelectedDeviceTypeCode(ControllerType type)
        {
            SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(type);            
            SelectedDeviceTypeCode = Convert.ToInt16(controllerConfig.DefaultDeviceTypeCode);            
        }
        #endregion
    }
}
