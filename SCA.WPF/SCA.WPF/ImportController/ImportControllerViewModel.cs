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
using SCA.Model.BussinessModel;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.Interface;
using SCA.WPF.ImportContentSelector;
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
        public ImportControllerViewModel()
        { 
            ImportContentSelectorViewModel importContentSelectorVM = new ImportContentSelectorViewModel();
            ImportContentSelectorDataContext = importContentSelectorVM;
        }

        #region 属性
        private bool _excelFormatState = true;
        private bool _mdbFormatState = false;
        private bool _ntFormatState = false;
        private bool _defaultTemplateState=true;
        private bool _customizedTemplateState=false;
        private Visibility _excelFormatContentSettingVisibility=Visibility.Visible;
        private string _controllerName="";
        private int _machineNumber = 0;
        
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
                ControllerOperation8001 operation = new ControllerOperation8001();
                string strErrorMessage;
                SCA.BusinessLib.ProjectManager.GetInstance.TheControllerViaImporting = operation.ReadEXCELTemplate(ExcelFilePath, fileService,TheController,out strErrorMessage);

                #region 显示数据选择页面
                ConfigSection = Visibility.Collapsed;
                ImportContentSelectorViewModel importContentSelectorVM = new ImportContentSelectorViewModel();
                importContentSelectorVM.TheController = TheController;
                importContentSelectorVM.SelfVisibility = Visibility.Visible;
                importContentSelectorVM.ImportDataSelectorVisibility = Visibility.Visible;
                importContentSelectorVM.InitilizeData(strErrorMessage);
                ImportContentSelectorDataContext = importContentSelectorVM;

                //importContentSelectorVM=

                #endregion

            }
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
            dialog.ShowDialog();
            string strFilePath = dialog.SelectedPath;
            FileService fileService=new FileService();
            if (DefaultTemplateState)
            {                
                ControllerOperation8001 operation = new ControllerOperation8001();
                operation.DownloadDefaultEXCELTemplate(strFilePath + "//" + "默认(" + SelectedControllerType.ToString() + ")模板文件.xlsx", fileService, null);
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
                ControllerOperation8001 operation = new ControllerOperation8001(); //暂时写为8001,后续需要改为各控制器通用代码
                string fileName="";
                if(ControllerName.Length>10)
                {
                    fileName = fileName + ControllerName.Substring(0, 10) + "控制器(" + SelectedControllerType.ToString() + ")模板文件.xlsx";
                }
                else
                {
                    fileName = fileName + ControllerName + "控制器(" + SelectedControllerType.ToString() + ")模板文件.xlsx";
                }

                operation.DownloadDefaultEXCELTemplate(strFilePath + "//" +fileName , fileService, customizedInfo);
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
        #endregion
    }
}
