using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using Caliburn.Micro;
using SCA.Model;
using SCA.Interface.BusinessLogic;
using SCA.BusinessLib.BusinessLogic;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
using SCA.Interface;
using SCA.BusinessLib.Controller;
using System.Windows;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:31:56
* FileName   : LinkageConfigGeneralViewModel
* Description: 通用联动组态
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableLinkageConfigGeneral : LinkageConfigGeneral, IEditableObject, IDataErrorInfo, INotifyPropertyChanged
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        private EditableLinkageConfigGeneral copy;
        public EditableLinkageConfigGeneral()
        {

        }
        public EditableLinkageConfigGeneral(LinkageConfigGeneral linkageConfigGeneral)
        {
            this.Controller=linkageConfigGeneral.Controller;
            this.ControllerID = linkageConfigGeneral.ControllerID;
            this.ID = linkageConfigGeneral.ID;
            this.Code=linkageConfigGeneral.Code;
            this.ActionCoefficient= linkageConfigGeneral.ActionCoefficient;
            this.CategoryA = linkageConfigGeneral.CategoryA;
            this.BuildingNoA = linkageConfigGeneral.BuildingNoA;
            this.ZoneNoA = linkageConfigGeneral.ZoneNoA;
            this.LayerNoA1 = linkageConfigGeneral.LayerNoA1;
            this.LayerNoA2 = linkageConfigGeneral.LayerNoA2;
            this.DeviceTypeCodeA = linkageConfigGeneral.DeviceTypeCodeA;
            this.TypeC = linkageConfigGeneral.TypeC;
            this.MachineNoC = linkageConfigGeneral.MachineNoC;
            this.LoopNoC = linkageConfigGeneral.LoopNoC;
            this.DeviceCodeC = linkageConfigGeneral.DeviceCodeC;
            this.BuildingNoC = linkageConfigGeneral.BuildingNoC;
            this.ZoneNoC = linkageConfigGeneral.ZoneNoC;
            this.LayerNoC = linkageConfigGeneral.LayerNoC;
            this.DeviceTypeCodeC = linkageConfigGeneral.DeviceTypeCodeC;
            ToCategoryAString();
        }
        public LinkageConfigGeneral ToLinkageConfigGeneral()
        {
            LinkageConfigGeneral linkageConfigGeneral = new LinkageConfigGeneral();

            linkageConfigGeneral.Controller = this.Controller;
            linkageConfigGeneral.ControllerID = this.ControllerID;
            linkageConfigGeneral.ID = this.ID;
            linkageConfigGeneral.Code =this.Code;
            linkageConfigGeneral.ActionCoefficient = this.ActionCoefficient;
            linkageConfigGeneral.CategoryA = this.CategoryA;
            linkageConfigGeneral.BuildingNoA = this.BuildingNoA;
            linkageConfigGeneral.ZoneNoA = this.ZoneNoA;
            linkageConfigGeneral.LayerNoA1 = this.LayerNoA1;
            linkageConfigGeneral.LayerNoA2 = this.LayerNoA2;
            linkageConfigGeneral.DeviceTypeCodeA = this.DeviceTypeCodeA;
            linkageConfigGeneral.TypeC = this.TypeC;
            linkageConfigGeneral.MachineNoC = this.MachineNoC;
            linkageConfigGeneral.LoopNoC = this.LoopNoC;
            linkageConfigGeneral.DeviceCodeC = this.DeviceCodeC;
            linkageConfigGeneral.BuildingNoC = this.BuildingNoC;
            linkageConfigGeneral.ZoneNoC = this.ZoneNoC;
            linkageConfigGeneral.LayerNoC = this.LayerNoC;
            linkageConfigGeneral.DeviceTypeCodeC = this.DeviceTypeCodeC;
            return linkageConfigGeneral;
        }
        
        private string _code;
        public new string Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                { 
                    _code = value;
                    RaisePropertyChanged("Code");
                }
            }
        }
        private int _actionCoefficient;
        public new int ActionCoefficient
        {
            get
            {
                return _actionCoefficient;
            }
            set
            {
                if (_actionCoefficient != value)
                {
                    _actionCoefficient = value;
                    RaisePropertyChanged("ActionCoefficient");
                }
            }
        }
        private int? _buildingNoA;
        public new int? BuildingNoA
        {
            get
            {
                return _buildingNoA;
            }
            set
            {
                _buildingNoA = value;
                RaisePropertyChanged("BuildingNoA");
            }
        }
        private int? _zoneNoA;
        public new int? ZoneNoA
        {
            get
            {
                return _zoneNoA;
            }
            set
            {
                _zoneNoA = value;
                RaisePropertyChanged("ZoneNoA");
            }
        }
        private int? _layerNoA1;
        public new int? LayerNoA1
        {
            get
            {
                return _layerNoA1;
            }
            set
            {
                _layerNoA1 = value;
                RaisePropertyChanged("LayerNoA1");
            }
        }
        private int? _layerNoA2;
        public new int? LayerNoA2
        {
            get
            {
                return _layerNoA2;
            }
            set
            {
                _layerNoA2 = value;
                RaisePropertyChanged("LayerNoA2");
            }
        }
        private short _deivceTypeCodeA;
        public new short DeviceTypeCodeA
        {
            get
            {
                return _deivceTypeCodeA;
            }
            set
            {
                _deivceTypeCodeA = value;
                RaisePropertyChanged("DeviceTypeCodeA");
            }
        }
        private LinkageType _typeC;
        public new LinkageType TypeC
        {
            get
            {
                return _typeC;
            }
            set
            {
                _typeC = value;
                RaisePropertyChanged("TypeC");
            }    
        }
        private string _machineNoC;
        public new string MachineNoC
        {
            get
            {
                return _machineNoC;
            }
            set
            {
                _machineNoC = value;
                RaisePropertyChanged("MachineNoC");
            }   
        }
        private string _loopNoC;
        public  new string LoopNoC
        {
            get
            {
                return _loopNoC;
            }
            set
            {
                _loopNoC = value;
                RaisePropertyChanged("LoopNoC");
            }             
        }
        private string _deviceCodeC;
        public new string DeviceCodeC
        {
            get
            {
                return _deviceCodeC;
            }
            set
            {
                _deviceCodeC = value;
                RaisePropertyChanged("DeviceCodeC");
            }
        }
        private int? _buildingNoC;
        public new int? BuildingNoC
        {
            get
            {
                return _buildingNoC;
            }
            set
            {
                _buildingNoC = value;
                RaisePropertyChanged("BuldingNoC");
            }
        }
        private int? _zoneNoC;
        public new int? ZoneNoC
        {
            get
            {
                return _zoneNoC;
            }
            set
            {
                _zoneNoC = value;
                RaisePropertyChanged("ZoneNoC");
            }
        }
        private int? _layerNoC;
        public new  int? LayerNoC
        {
            get
            {
                return _layerNoC;
            }
            set
            {
                _layerNoC = value;
                RaisePropertyChanged("LayerNoC");
            }
        }
        private short _deviceTypeCodeC;
        public new short DeviceTypeCodeC
        {
            get
            {
                return _deviceTypeCodeC;
            }
            set
            {
                _deviceTypeCodeC = value;
                RaisePropertyChanged("DeviceTypeCodeC");
            }
        }


        private string _categoryAString;
        public string CategoryAString
        {
            get { return _categoryAString; }
            set
            {
                _categoryAString = value;
                RaisePropertyChanged("CategoryAString");
                switch (_categoryAString)
                {
                    case "本系统":
                        this.CategoryA = 0;
                        break;
                    case "它系统":
                        this.CategoryA = 1;
                        break;
                    case "全系统":
                        this.CategoryA = 2;
                        break;
                }
            }
        }

        public void ToCategoryAString()
        {
            switch (this.CategoryA)
            {
                case 0:
                    CategoryAString = "本系统";
                    break;
                case 1:
                    CategoryAString = "它系统";
                    break;
                case 2:
                    CategoryAString = "全系统";
                    break;
            }
        }

        #region INotifyPropertyChanged Members

       public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

       private void RaisePropertyChanged(string propertyName)
       {
           // take a copy to prevent thread issues
           PropertyChangedEventHandler handler = PropertyChanged;
           if (handler != null)
           {
               handler(this, new PropertyChangedEventArgs(propertyName));
           }
       }
        #endregion

        public void BeginEdit()
        {
            //throw new System.NotImplementedException();
        }

        public void CancelEdit()
        {
            //throw new System.NotImplementedException();
        }

        public void EndEdit()
        {
            if (ItemEndEdit != null)
            {
                ItemEndEdit(this);
            }
        }
        private SCA.Interface.IControllerConfig Config
        {
            get
            {
                return ControllerConfigManager.GetConfigObject(Controller.Type);
            }
        }


        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {                
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = Config.GetGeneralLinkageConfigRegularExpression(this.Controller.DeviceAddressLength);
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
                switch (columnName)
                {
     
                    case "Code":
                        if(!string.IsNullOrEmpty(this.Code))
                        {
                            rule = dictMessage["Code"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.Code.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    //case "ActionCoefficient":
                    //    rule = dictMessage["ActionCoefficient"];
                    //    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                    //    if (!exminator.IsMatch(this.ActionCoefficient.ToString()))
                    //    {
                    //        errorMessage = rule.ErrorMessage;
                    //    }
                    //    break;
                    case "BuildingNoA":
                        if (this.BuildingNoA != null)
                        {
                            rule = dictMessage["Building"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.BuildingNoA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ZoneNoA":
                        if (this.ZoneNoA != null)
                        {
                            rule = dictMessage["Zone"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNoA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LayerNoA1":
                        if (this.LayerNoA1 != null)
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LayerNoA1.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LayerNoA2":
                        if (this.LayerNoA2 != null)
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LayerNoA2.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "MachineNoC":
                        if (!string.IsNullOrEmpty(this.MachineNoC))
                        {
                            rule = dictMessage["MachineNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.MachineNoC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LoopNoC":
                        if (!string.IsNullOrEmpty(this.LoopNoC))
                        {
                            rule = dictMessage["Loop"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LoopNoC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "DeviceCodeC":
                        if (!string.IsNullOrEmpty(this.DeviceCodeC))
                        {
                            rule = dictMessage["Device"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.DeviceCodeC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "BuildingNoC":
                        if (this.BuildingNoC != null)
                        {
                            rule = dictMessage["Building"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.BuildingNoC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ZoneNoC":
                        if (this.ZoneNoC != null)
                        {
                            rule = dictMessage["Zone"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNoC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LayerNoC":
                        if (this.LayerNoC != null)
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LayerNoC.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                }
                return errorMessage;
            }
        }
    }
    public class EditableLinkageConfigGenerals : ObservableCollection<EditableLinkageConfigGeneral>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.BusinessLogic.ILinkageConfigGeneralService _linkageConfigGeneralService;
        private ControllerModel _controller;
        public EditableLinkageConfigGenerals(ControllerModel controller, List<LinkageConfigGeneral> lstLinkageConfigGeneral)
        {
            _controller = controller;
            _linkageConfigGeneralService = new SCA.BusinessLib.BusinessLogic.LinkageConfigGeneralService(_controller);
            if (lstLinkageConfigGeneral != null)
            {
                foreach (var o in lstLinkageConfigGeneral)
                {
                    this.Add(new EditableLinkageConfigGeneral(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableLinkageConfigGeneral item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableLinkageConfigGeneral o = (EditableLinkageConfigGeneral)sender;            
            o.Controller = _controller;
            o.ControllerID = _controller.ID;
            _linkageConfigGeneralService.Update(o.ToLinkageConfigGeneral());
        }

    }
    public class LinkageConfigGeneralViewModel:PropertyChangedBase
    {
        private ILinkageConfigGeneralService _linkageConfigGeneralService;
        private int _addedAmount = 1;//向集合中新增LinkageConfigMixed的数量        
        private EditableLinkageConfigGenerals _lcgCollection;
        private List<LinkageConfigGeneral> _lstLinkageConfigGeneral = null;
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private object _detailType = GridDetailType.General;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        public LinkageConfigGeneralViewModel()
        {
            _linkageConfigGeneralService = new SCA.BusinessLib.BusinessLogic.LinkageConfigGeneralService(TheController);
        }
        /// <summary>
        /// 去掉LinkageType的None值
        /// </summary>
        /// <returns></returns>
        public List<Model.LinkageType> GetLinkageType()
        {
            ControllerConfigNone config = new ControllerConfigNone();
            return config.GetLinkageType();
        }

        private Visibility _isVisualColumnGroup = Visibility.Collapsed;
        public Visibility IsVisualColumnGroup
        {
            get
            {
                return _isVisualColumnGroup;
            }
            set
            {
                _isVisualColumnGroup = value;
                NotifyOfPropertyChange("IsVisualColumnGroup");
            }
        }
        public Visibility AddMoreLinesUserControlVisibility
        {
            get
            {
                return _addMoreLinesUserControlVisibility;
            }
            set
            {
                _addMoreLinesUserControlVisibility = value;
                NotifyOfPropertyChange("AddMoreLinesUserControlVisibility");
            }
        }
        public object DetailType
        {
            get
            {
                return _detailType;
            }
            set
            {
                _detailType = value;
                NotifyOfPropertyChange("DetailType");
            }
        }
        public int AddedAmount
        {
            get
            {
                return _addedAmount;
            }
            set
            {
                _addedAmount = value;
            }
        }
        /// <summary>
        /// 当前LinkageConfigMixedViewModel的控制器对象
        /// </summary>
        public ControllerModel TheController { get; set; }
        /// <summary>
        /// 获取当前控制器的通用组态信息
        /// </summary>
        public List<LinkageConfigGeneral> LinkageConfigGeneral
        {
            get
            {
                if (_lstLinkageConfigGeneral == null)
                {

                    _lstLinkageConfigGeneral = new List<LinkageConfigGeneral>();
                }
                if (_lcgCollection != null)
                {
                    foreach (var s in _lcgCollection)
                    {
                        _lstLinkageConfigGeneral.Add(s.ToLinkageConfigGeneral());
                    }
                }
                return _lstLinkageConfigGeneral;
            }
        }

        public EditableLinkageConfigGenerals GeneralLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_lcgCollection == null)
                {
                    _lcgCollection = new EditableLinkageConfigGenerals(TheController, null);
                    

                }
                return _lcgCollection;
            }
            set
            {
                _lcgCollection = value;                
                
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public ICommand AddNewRecordCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand<int>(AddNewRecordExecute, null);
            }
        }
        public ICommand DownloadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DownloadExecute, null);
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(SaveExecute, null);
            }
        }
        public ICommand AddMoreLinesConfirmCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand<object>(AddNewRecordExecute, null);
            }
        }
        public ICommand AddMoreLinesCloseCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(AddMoreLinesCloseExecute, null);
            }
        }
        public ICommand DisplayMoreLinesViewCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DisplayMoreLinesViewExecute, null);
            }
        }
        public void AddMoreLinesCloseExecute()
        {
            AddMoreLinesUserControlVisibility = Visibility.Collapsed;
        }
        public void DisplayMoreLinesViewExecute()
        {
            AddMoreLinesUserControlVisibility = Visibility.Visible;
        }

        //public ICommand UploadCommand
        //{ 

        //}
        /// <summary>
        /// 添加指定数量的标准组态信息
        /// </summary>
        /// <param name="rowsAmount"></param>
        public void AddNewRecordExecute(int rowsAmount)
        {

            using (new WaitCursor())
            {
                _linkageConfigGeneralService.TheController = this.TheController;
                List<LinkageConfigGeneral> lstLinkageConfigGeneral = _linkageConfigGeneralService.Create(rowsAmount);
                foreach (var v in lstLinkageConfigGeneral)
                {
                    EditableLinkageConfigGeneral eLCG = new EditableLinkageConfigGeneral();
                    eLCG.Controller = v.Controller;
                    eLCG.ControllerID = v.ControllerID;
                    eLCG.ID = v.ID;
                    eLCG.Code = v.Code;
                    GeneralLinkageConfigInfoObservableCollection.Add(eLCG);
                }
            }
        }
        public void AddNewRecordExecute(object rowsAmount)
        {
            if (rowsAmount != null)
            {

                try
                {
                    int amount = Convert.ToInt32(((RoutedEventArgs)rowsAmount).OriginalSource);
                    AddNewRecordExecute(amount);
                }
                catch (Exception ex)
                {
                    //转换出错，不作任何处理
                }
            }
            AddMoreLinesUserControlVisibility = Visibility.Collapsed;
        }
        public void DownloadExecute()
        {
            _linkageConfigGeneralService.TheController = this.TheController;
            _linkageConfigGeneralService.DownloadExecute(LinkageConfigGeneral);       
        }
        public void UploadExecute()
        {

        }

        public List<DeviceType> ValidDeviceType
        {
            get
            {
                //SCA.BusinessLib.BusinessLogic.ControllerConfig8001 config = new BusinessLib.BusinessLogic.ControllerConfig8001();
                IControllerConfig config = SCA.BusinessLib.BusinessLogic.ControllerConfigManager.GetConfigObject(TheController.Type);
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfoForGeneralLinkageInput();                
                return lstDeviceType;
       
            }
        }
        public List<DeviceType> ValidDeviceTypeC
        {
            get
            {
                //SCA.BusinessLib.BusinessLogic.ControllerConfig8001 config = new BusinessLib.BusinessLogic.ControllerConfig8001();
                IControllerConfig config = SCA.BusinessLib.BusinessLogic.ControllerConfigManager.GetConfigObject(TheController.Type);
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfoForGeneralLinkageOutput();                 
                return lstDeviceType;
            }
        }
        public void SaveExecute()
        {
            using (new WaitCursor())
            {
                SCA.Interface.BusinessLogic.ILinkageConfigGeneralService _mixedService = new LinkageConfigGeneralService(TheController);
                _mixedService.SaveToDB();
            }
        }

    }


}
