using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using SCA.WPF.Utility;
using SCA.Model;
using System.Windows.Input;
using System.ComponentModel;
using SCA.BusinessLib;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/6/6 16:03:18
* FileName   : ImportContentSelectorViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ImportContentSelector
{
    public class ImportContentSelectorViewModel : PropertyChangedBase
    {
        #region 属性
        const string ERROR_PAGE_TITLE = "错误信息";
        const string SELECTOR_PAGE_TITLE = "选择导入内容";
        private string _title = SELECTOR_PAGE_TITLE;
        private string _errorInfo;        
        private string _promptInfo = ""; //选择区提示信息
        private Visibility _errorContentVisibility = Visibility.Visible;        //错误信息内容可见性
        private Visibility _importDataSelectorVisibility = Visibility.Visible;  //导入数据选择区可见性
        private Visibility _loopInfoInSelectorVisibility = Visibility.Visible;  //选择区的回路信息可见性
        private Visibility _otherInfoInSelectorVisibility = Visibility.Visible; //选择区的其它信息可见性
        private Visibility _promptInfoInSelectorVisibility = Visibility.Visible;//选择区的提示信息可见性
        private Visibility _selfVisibility = Visibility.Collapsed;  //自身控件的可见性
        private CheckItemCollection _loopNameCollection; //可导入的回路名称集合
        private CheckItemCollection _otherSettingsCollection;//可导入的其它类别信息集合
        private CheckItemCollection _errorInfoCollection; //错误信息集合
        private bool _selectedAllFlag = false; //全选标志
        public ControllerModel TheController { get; set; }//当前控制器
        //public System.Action CloseEvent;// 关闭事件
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorInfo
        {
            get
            {
                return _errorInfo;
            }
            set
            {
                _errorInfo = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ErrorContentVisibility
        {
            get
            {
                return _errorContentVisibility;
            }
            set
            {
                _errorContentVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }

        }
        public Visibility ImportDataSelectorVisibility
        {
            get
            {
                return _importDataSelectorVisibility;
            }
            set
            {
                _importDataSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }        
        public Visibility LoopInfoInSelectorVisibility
        {
            get
            {
                return _loopInfoInSelectorVisibility;
            }
            set
            {
                _loopInfoInSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility OtherInfoInSelectorVisibility
        {
            get
            {
                return _otherInfoInSelectorVisibility;
            }
            set
            {
                _otherInfoInSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility PromptInfoInSelectorVisibility
        {
            get
            {
                return _promptInfoInSelectorVisibility;
            }
            set
            {
                _promptInfoInSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility SelfVisibility
        {
            get
            {
                return _selfVisibility;
            }
            set
            {
                _selfVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string PromptInfo
        {
            get
            {
                return _promptInfo;
            }
            set
            {
                _promptInfo = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public CheckItemCollection LoopNameCollection
        {
            get
            {
                return _loopNameCollection;
            }
            set
            {
                _loopNameCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public CheckItemCollection OtherSettingsCollection
        {
            get
            {
                return _otherSettingsCollection;
            }
            set
            {
                _otherSettingsCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public CheckItemCollection ErrorInfoCollection
        {
            get
            {
                return _errorInfoCollection;
            }
            set
            {
                _errorInfoCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool SelectedAll
        {
            get
            {
                return _selectedAllFlag;
            }
            set
            {
                _selectedAllFlag = value;
                setAllLoopCheckStatus(_selectedAllFlag);
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public ICommand ImportCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ImportExecute, null); }
        }
        public void ImportExecute()
        {
            //得到需要导入的控制器的信息
            ControllerModel controller = ProjectManager.GetInstance.TheControllerViaImporting;

            ControllerOperationCommon controllerBase = new ControllerOperationCommon();
            LinkageConfigStandardService standardLinkageService = new LinkageConfigStandardService(TheController);
            LinkageConfigMixedService mixedLinkageService = new LinkageConfigMixedService(TheController);
            LinkageConfigGeneralService gerneralLinkageService = new LinkageConfigGeneralService(TheController);
            int maxLoopID=controllerBase.GetMaxLoopID();
            List<CheckItem> selectedLoops = LoopNameCollection.Where((d)=>d.IsChecked==true).ToList<CheckItem>();

            //比对回路信息是否已经存在：提示，是否覆盖
            foreach (var loop in controller.Loops)
            {
                int selectedLoopCount = selectedLoops.Count((d) => d.Value == loop.Code);
                if (selectedLoopCount <= 0) //未勾选的回路，放弃导入
                {
                    continue;
                }               
                LoopModel existLoop = TheController.Loops.Where((d) => d.Code == loop.Code).FirstOrDefault();                
                int amount = TheController.Loops.Count((d) => d.Code == loop.Code);
                if (amount > 0)
                {
                    string strPromptInfo = "控制器" + TheController.Name + ":已经存在" + loop.Code + ",覆盖吗?";
                    if (MessageBox.Show(strPromptInfo, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        TheController.Loops.RemoveAll((d) => d.Code == loop.Code);                        
                        loop.Controller = TheController;
                        loop.ControllerID = TheController.ID;
                        loop.ID = existLoop.ID;
                        TheController.Loops.Add(loop);
                    }
                }
                else
                {
                    maxLoopID++;                    
                    loop.Controller = TheController;
                    loop.ControllerID = TheController.ID;
                    loop.ID = maxLoopID;
                    TheController.Loops.Add(loop);
                }
            }

            List<CheckItem> lstOtherSetting = OtherSettingsCollection.Where((d) => d.IsChecked == true).ToList<CheckItem>();
            int selectedStandardCount = lstOtherSetting.Count((d) => d.Value == "标准组态");

            int maxStandardLinkageConfigID = standardLinkageService.GetMaxID();
            if (selectedStandardCount > 0) //未勾选，放弃导入
            {
                //比对组态信息，按编号比对
                foreach (var importConfig in controller.StandardConfig)
                {
                    LinkageConfigStandard existConfig = TheController.StandardConfig.Where((d) => d.Code == importConfig.Code).FirstOrDefault();                
                    int amount = TheController.StandardConfig.Count((d) => d.Code == importConfig.Code);
                    if (amount > 0)
                    {
                        string strPromptInfo = "控制器" + TheController.Name + ":已经存在标准组态" + importConfig.Code + ",覆盖吗?";
                        if (MessageBox.Show(strPromptInfo, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            TheController.StandardConfig.RemoveAll((d) => d.Code == importConfig.Code);
                            importConfig.Controller = TheController;
                            importConfig.ControllerID = TheController.ID;
                            importConfig.ID = existConfig.ID;
                            TheController.StandardConfig.Add(importConfig);
                        }
                    }
                    else
                    {
                        maxStandardLinkageConfigID++;
                        importConfig.Controller = TheController;
                        importConfig.ControllerID = TheController.ID;
                        importConfig.ID = maxStandardLinkageConfigID;
                        TheController.StandardConfig.Add(importConfig);
                    }
                }
            }
            int selectedMixedCount = lstOtherSetting.Count((d) => d.Value == "混合组态");
            if (selectedMixedCount > 0) //未勾选，放弃导入
            {
                //比对组态信息，按编号比对
                foreach (var importConfig in controller.MixedConfig)
                {
                    int amount = TheController.MixedConfig.Count((d) => d.Code == importConfig.Code);
                    if (amount > 0)
                    {
                        string strPromptInfo = "控制器" + TheController.Name + ":已经存在混合组态" + importConfig.Code + ",覆盖吗?";
                        if (MessageBox.Show(strPromptInfo, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            TheController.MixedConfig.RemoveAll((d) => d.Code == importConfig.Code);
                            TheController.MixedConfig.Add(importConfig);
                        }
                    }
                    else
                    {
                        TheController.MixedConfig.Add(importConfig);
                    }
                }
            }

            int selectedGeneralCount = lstOtherSetting.Count((d) => d.Value == "通用组态");
            if (selectedGeneralCount > 0) //未勾选，放弃导入
            {
                //比对组态信息，按编号比对
                foreach (var importConfig in controller.GeneralConfig)
                {
                    int amount = TheController.GeneralConfig.Count((d) => d.Code == importConfig.Code);
                    if (amount > 0)
                    {
                        string strPromptInfo = "控制器" + TheController.Name + ":已经存在通用组态" + importConfig.Code + ",覆盖吗?";
                        if (MessageBox.Show(strPromptInfo, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            TheController.GeneralConfig.RemoveAll((d) => d.Code == importConfig.Code);
                            TheController.GeneralConfig.Add(importConfig);
                        }
                    }
                    else
                    {
                        TheController.GeneralConfig.Add(importConfig);
                    }
                }
            }

            int selectedMCBCount = lstOtherSetting.Count((d) => d.Value == "网络手动盘");
            if (selectedMCBCount > 0) //未勾选，放弃导入            {
            {
                foreach (var importConfig in controller.ControlBoard)
                {
                    int amount = TheController.ControlBoard.Count((d) => d.Code == importConfig.Code);
                    if (amount > 0)
                    {
                        string strPromptInfo = "控制器" + TheController.Name + ":已经存在网络手动盘" + importConfig.Code + ",覆盖吗?";
                        if (MessageBox.Show(strPromptInfo, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            TheController.ControlBoard.RemoveAll((d) => d.Code == importConfig.Code);
                            TheController.ControlBoard.Add(importConfig);
                        }
                    }
                    else
                    {
                        TheController.ControlBoard.Add(importConfig);
                    }
                }
            }

            CloseExecute();
            ProjectManager.GetInstance.TheControllerViaImporting = null;
            //导入完成，导入信息清除            
            EventMediator.NotifyColleagues("RefreshNavigator", TheController);
        }
        public ICommand CloseCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CloseExecute, null); }
        }
        public void CloseExecute()
        {
            SelfVisibility = Visibility.Collapsed;
            EventMediator.NotifyColleagues("ImportControllerViewClose", null);
           // CloseEvent();
        }
        #endregion
        /// <summary>
        /// 设置所有回路信息的选择状态
        /// </summary>
        /// <param name="selectedAllFlag"></param>
        private void setAllLoopCheckStatus(bool selectedAllFlag)
        {
            if (selectedAllFlag)
            {
                foreach (var item in LoopNameCollection)
                {
                    item.IsChecked = true;
                }
            }
            else
            {
                foreach (var item in LoopNameCollection)
                {
                    item.IsChecked = false;
                }
            }
        }
        /// <summary>
        /// 初始化页面展示数据
        /// </summary>
        public void InitilizeData(string errorMessage)
        {
            #region 错误信息显示
            if (errorMessage != null && errorMessage != "")
            {
                ImportDataSelectorVisibility = Visibility.Collapsed;
                ErrorContentVisibility = Visibility.Visible;
                Title = ERROR_PAGE_TITLE;
                ErrorInfo = errorMessage;
                SetErrorInfo(errorMessage);
                return;
            }
            else
            {
                ImportDataSelectorVisibility = Visibility.Visible;
                ErrorContentVisibility = Visibility.Collapsed;                
                Title = SELECTOR_PAGE_TITLE;
            }
            #endregion
            ControllerModel controller = ProjectManager.GetInstance.TheControllerViaImporting;
            #region 可见性设置
            if (controller.Loops.Count > 0)
            {
                LoopInfoInSelectorVisibility = Visibility.Visible;
            }
            else
            {
                LoopInfoInSelectorVisibility=Visibility.Collapsed;
                PromptInfoInSelectorVisibility = Visibility.Visible;
                PromptInfo = "EXCEL文件中没有回路配置信息;";
            }
            if (controller.StandardConfig.Count > 0 || controller.MixedConfig.Count > 0 || controller.GeneralConfig.Count > 0 || controller.ControlBoard.Count > 0)
            {
                OtherInfoInSelectorVisibility = Visibility.Visible;
            }
            else
            {
                OtherInfoInSelectorVisibility = Visibility.Collapsed;
                PromptInfo = PromptInfo + "EXCEL文件没有组态或网络手控盘配置信息;";
            }
            //如果没有可导入的信息，此方法直接返回
            if (LoopInfoInSelectorVisibility == Visibility.Collapsed && OtherInfoInSelectorVisibility == Visibility.Collapsed)
            {
                PromptInfoInSelectorVisibility = Visibility.Visible;
                PromptInfo = "EXCEL文件中没有可导入的配置信息";
                return; 
            }
            #endregion
            #region 创建显示内容
            CheckItemCollection loopItems = new CheckItemCollection();
            CheckItemCollection otherItems = new CheckItemCollection();
            int rowIndex = 0;
            int columnIndex = 0;
            const int MAX_COLUMN = 8;
            if (controller != null)
            {
                foreach (var loop in controller.Loops)
                {
                    
                   // List<DeviceInfo8001> lstDevices = loop.GetDevices<DeviceInfo8001>();
                    //foreach (var device in lstDevices)
                    //{
                        CheckItem item = new CheckItem();
                        item.Value = loop.Name;
                        item.RowIndex = rowIndex;
                        item.ColumnIndex = columnIndex;
                        loopItems.Add(item);
                        columnIndex++;
                        if (columnIndex == (MAX_COLUMN))
                        {
                            columnIndex = 0;
                            rowIndex++;
                        }
                    //}
                }
                LoopNameCollection = loopItems;
                if (controller.StandardConfig.Count > 0)
                {
                    CheckItem item = new CheckItem();                    
                    FieldInfo fi = ControllerNodeType.Standard.GetType().GetField(ControllerNodeType.Standard.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        item.Value = ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)) ? attributes[0].Description : ControllerNodeType.Standard.ToString());
                    }
                    otherItems.Add(item);                    
                }
                if (controller.MixedConfig.Count > 0)
                {
                    CheckItem item = new CheckItem();
                    FieldInfo fi = ControllerNodeType.Mixed.GetType().GetField(ControllerNodeType.Mixed.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        item.Value = ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)) ? attributes[0].Description : ControllerNodeType.Mixed.ToString());
                    }
                    otherItems.Add(item);
                }
                if (controller.GeneralConfig.Count > 0)
                {
                    CheckItem item = new CheckItem();
                    FieldInfo fi = ControllerNodeType.General.GetType().GetField(ControllerNodeType.General.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        item.Value = ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)) ? attributes[0].Description : ControllerNodeType.General.ToString());
                    }
                    otherItems.Add(item);
                }
                if (controller.ControlBoard.Count > 0)
                {
                    CheckItem item = new CheckItem();
                    FieldInfo fi = ControllerNodeType.Board.GetType().GetField(ControllerNodeType.Board.ToString());
                    if (fi != null)
                    {
                        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        item.Value = ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)) ? attributes[0].Description : ControllerNodeType.Board.ToString());
                    }
                    otherItems.Add(item);
                }
                OtherSettingsCollection = otherItems;
            }
            #endregion
        }
        private void SetErrorInfo(string errorMessage)
        {
            string[] errorMessages = errorMessage.Split(';');
            CheckItemCollection errorMessageColection=new CheckItemCollection();
            for (int i = 0; i < errorMessages.Length; i++)
            {
                if (errorMessages[i] != "")
                { 
                    CheckItem errorItem = new CheckItem();
                    errorItem.Value = errorMessages[i];
                    errorMessageColection.Add(errorItem);
                }
            }
            ErrorInfoCollection = errorMessageColection;
        }
       // public
            //        VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            //dialog.Filter = "工程文件 (*.nt)|*.nt";
            //dialog.ShowDialog();
            //if (dialog.FileName != "")
            //{ 
            //    ProjectManager.GetInstance.OpenProject(dialog.FileName);
            //    EventMediator.NotifyColleagues("DisplayTheOpenedProject", null);
            //}
    }

    public class CheckItem : PropertyChangedBase
    { 
        //private readonly 
        private string _value;
        private bool _isChecked;
        private int _rowIndex;
        private int _columnIndex;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public int RowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public int ColumnIndex
        {
            get
            {
                return _columnIndex;
            }
            set
            {
                _columnIndex = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
    }
    public class CheckItemCollection : ObservableCollection<CheckItem>
    {
        private ListCollectionView _selected;
        public CheckItemCollection()
        {
            _selected = new ListCollectionView(this);
            _selected.Filter = delegate(object checkObject)
            {
                return ((CheckItem)checkObject).IsChecked;
            };
        }
        public void Add(string item)
        {
            this.Add(new CheckItem() { Value = item });
        }
        public ICollectionView CheckedItems
        {
            get { return _selected; }
        }
    }
}
