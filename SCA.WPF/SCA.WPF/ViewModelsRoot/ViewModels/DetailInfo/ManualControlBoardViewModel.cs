/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 250 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-11 15:33:06 +0800 (周五, 11 八月 2017) $
***************************************************************************/
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Caliburn.Micro;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib.BusinessLogic;
using SCA.Interface.BusinessLogic;
using Neat.Dennis.Common.LoggerManager;
using SCA.BusinessLib;
using SCA.Interface;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:40:39
* FileName   : ManualControlBoardViewModel
* Description: 网络手动盘
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{

    public class EditableManualControlBoard : ManualControlBoard, IEditableObject, IDataErrorInfo, INotifyPropertyChanged
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public event ItemEndEditEventHandler ItemEndEdit;
        private EditableManualControlBoard copy;
        public EditableManualControlBoard()
        {

        }
        public EditableManualControlBoard(ManualControlBoard mcb)
        {
            this.ID = mcb.ID;
            this.Code = mcb.Code;
            this.BoardNo = mcb.BoardNo;
            this.SubBoardNo = mcb.SubBoardNo;
            this.KeyNo = mcb.KeyNo;
            this.DeviceCode = mcb.DeviceCode;
            this.DeviceType = mcb.DeviceType;
            this.LocalDevice1 = mcb.LocalDevice1;
            this.LocalDevice2 = mcb.LocalDevice2;
            this.LocalDevice3 = mcb.LocalDevice3;
            this.LocalDevice4 = mcb.LocalDevice4;
            this.NetDevice1 = mcb.NetDevice1;
            this.NetDevice2 = mcb.NetDevice2;
            this.NetDevice3 = mcb.NetDevice3;
            this.NetDevice4 = mcb.NetDevice4;
            this.BuildingNo = mcb.BuildingNo;
            this.AreaNo = mcb.AreaNo;
            this.FloorNo = mcb.FloorNo;
            this.ControlType = mcb.ControlType;
            this.LinkageGroup = mcb.LinkageGroup;
            this.SDPKey = mcb.SDPKey;
            this.MaxSubBoardNo = mcb.MaxSubBoardNo;
            this.Controller = mcb.Controller;
            this.ControllerID = mcb.ControllerID;
            ToControlTypeString();
        }
        private int _code;
        private int _boardNo;
        private int _subBoardNo;
        private int _keyNo;        
        public new int Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                RaisePropertyChanged("Code");
            }
        }
        public new int BoardNo
        {
            get
            {
                return _boardNo;
            }
            set
            {
                _boardNo = value;
                RaisePropertyChanged("BoardNo");
            }
        }
        public new int SubBoardNo
        {
            get
            {
                return _subBoardNo;
            }
            set
            {
                _subBoardNo = value;
                RaisePropertyChanged("SubBoardNo");
            }
        }
        public new int KeyNo
        {
            get
            {
                return _keyNo;
            }
            set
            {
                _keyNo = value;
                RaisePropertyChanged("KeyNo");
            }
        }
        private string _deviceCode;
        public new string DeviceCode 
        {
            get
            {
                return _deviceCode;
            }
            set
            {        
                    _deviceCode = value;
                    RaisePropertyChanged("DeviceCode");            
            }
        }

        private string _localDevice1;
        public new  string LocalDevice1
        {
            get { return _localDevice1; }
            set
            {
                _localDevice1 = value;
                RaisePropertyChanged("LocalDevice1");
            }
        }
        private string _localDevice2;
        public new string LocalDevice2
        {
            get { return _localDevice2; }
            set
            {
                _localDevice2 = value;
                RaisePropertyChanged("LocalDevice2");
            }
        }
        private string _localDevice3;
        public new string LocalDevice3
        {
            get { return _localDevice3; }
            set
            {
                _localDevice3 = value;
                RaisePropertyChanged("LocalDevice3");
            }
        }
        private string _localDevice4;
        public new string LocalDevice4
        {
            get { return _localDevice4; }
            set
            {
                _localDevice4 = value;
                RaisePropertyChanged("LocalDevice4");
            }
        }

        private string _netDevice1;
        public new string NetDevice1
        {
            get { return _netDevice1; }
            set
            {
                _netDevice1 = value;
                RaisePropertyChanged("NetDevice1");
            }
        }
        private string _netDevice2;
        public new string NetDevice2
        {
            get { return _netDevice2; }
            set
            {
                _netDevice2 = value;
                RaisePropertyChanged("NetDevice2");
            }
        }
        private string _netDevice3;
        public new string NetDevice3
        {
            get { return _netDevice3; }
            set
            {
                _netDevice3 = value;
                RaisePropertyChanged("NetDevice3");
            }
        }
        private string _netDevice4;
        public new  string NetDevice4
        {
            get { return _netDevice4; }
            set
            {
                _netDevice4 = value;
                RaisePropertyChanged("NetDevice4");
            }
        }
        private string _buildingNo;
        public new string BuildingNo
        {
            get
            {
                return _buildingNo;
            }
            set
            {
                _buildingNo=value;
                RaisePropertyChanged("BuildingNo");
            }
        }
        private string _areaNo;
        public new string AreaNo
        {
            get
            {
                return _areaNo;
            }
            set
            {
                _areaNo=value;
                RaisePropertyChanged("AreaNo");
            }
        }
        private string _floorNo;
        public string FloorNo
        {
            get
            {
                return _floorNo;
            }
            set
            {

                _floorNo=value;
                RaisePropertyChanged("FloorNo");
            }
        }
        private short _deviceType;
        public new short DeviceType
        {
            get
            {
                return _deviceType;
            }
            set
            {
                _deviceType=value;
                RaisePropertyChanged("DeviceType");
            }
        }
        private string _linkageGroup;
        public new string LinkageGroup
        {
            get
            {
                return _linkageGroup;                    
            }
            set
            {
                _linkageGroup=value;
                  RaisePropertyChanged("LinkageGroup");
            }
        }
        private string _controlTypeString;
        public string ControlTypeString
        {
            get { return _controlTypeString; }
            set
            {
                _controlTypeString = value;
                RaisePropertyChanged("ControlTypeString");
                switch(_controlTypeString)
                {
                    case "空器件":
                        this.ControlType = 0;
                        break;
                    case "本机设备":
                        this.ControlType = 1;
                        break;
                    case "楼区层":
                        this.ControlType = 2;
                        break;
                    case "输出组":
                        this.ControlType = 3;
                        break;
                    case "网络设备":
                        this.ControlType = 4;
                        break;
                }
            }
        }

        public void ToControlTypeString()
        {
            switch(this.ControlType)
            {
                case 0:
                    ControlTypeString = "空器件";
                    break;
                case 1:
                    ControlTypeString = "本机设备";
                    break;
                case 2:
                    ControlTypeString = "楼区层";
                    break;
                case 3:
                    ControlTypeString = "输出组";
                    break;
                case 4:
                    ControlTypeString = "网络设备";
                    break;
            }
        }

        public ManualControlBoard ToManualControlBoard()
        {
            ManualControlBoard mcb = new ManualControlBoard();
            mcb.ID = this.ID;
            mcb.Code = this.Code;
            mcb.BoardNo = this.BoardNo;
            mcb.SubBoardNo = this.SubBoardNo;
            mcb.KeyNo = this.KeyNo;
            mcb.DeviceCode = this.DeviceCode;
            mcb.ControlType = this.ControlType;
            mcb.LocalDevice1 = this.LocalDevice1;
            mcb.LocalDevice2 = this.LocalDevice2;
            mcb.LocalDevice3 = this.LocalDevice3;
            mcb.LocalDevice4 = this.LocalDevice4;
            mcb.NetDevice1 = this.NetDevice1;
            mcb.NetDevice2 = this.NetDevice2;
            mcb.NetDevice3 = this.NetDevice3;
            mcb.NetDevice4 = this.NetDevice4;
            mcb.BuildingNo = this.BuildingNo;
            mcb.AreaNo = this.AreaNo;
            mcb.FloorNo = this.FloorNo;
            mcb.DeviceType = this.DeviceType;
            mcb.LinkageGroup = this.LinkageGroup;
            mcb.SDPKey = this.SDPKey;
            mcb.MaxSubBoardNo = this.MaxSubBoardNo;
            mcb.Controller = this.Controller;
            mcb.ControllerID = this.ControllerID;
            return mcb;
        }
        public void BeginEdit()
        {
            //throw new System.NotImplementedException();
        }

        public void CancelEdit()
        {
            //  throw new System.NotImplementedException();
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
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = Config.GetManualControlBoardRegularExpression(this.Controller.DeviceAddressLength);                
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
                switch (columnName)
                {
                    case "Code":
                        rule = dictMessage["Code"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.Code.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "BoardNo":
                        rule = dictMessage["BoardNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.BoardNo.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "SubBoardNo":
                        rule = dictMessage["SubBoardNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.SubBoardNo.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "KeyNo":
                        rule = dictMessage["KeyNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.KeyNo.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "DeviceCode":
                        if (!string.IsNullOrEmpty(this.DeviceCode))
                        {
                            rule = dictMessage["DeviceCode"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.DeviceCode.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                }
                return errorMessage;
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
    }
    public class EditableManualControlBoards : ObservableCollection<EditableManualControlBoard>
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        private SCA.Interface.BusinessLogic.IManualControlBoardService _manualControlBoardService;
        private ControllerModel _controller;
        public EditableManualControlBoards(ControllerModel controller, List<ManualControlBoard> lstMCB)
        {
            _controller = controller;
            _manualControlBoardService = new SCA.BusinessLib.BusinessLogic.ManualControlBoardService(_controller);
            if (lstMCB != null)
            {
                foreach (var r in lstMCB)
                {

                    this.Add(new EditableManualControlBoard(r));
                }
            }
        }
        protected override void InsertItem(int index, EditableManualControlBoard item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            //if (ItemEndEdit != null)
            //{
            //    ItemEndEdit(sender);
            //}
            EditableManualControlBoard o = (EditableManualControlBoard)sender;
            o.Controller = _controller;
            o.ControllerID = _controller.ID;
            _manualControlBoardService.Update(o.ToManualControlBoard());
        }
    }
    public class ManualControlBoardViewModel : PropertyChangedBase,IDisposable
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private ObservableCollection<ManualControlBoard> _manualControlBoardInfoObservableCollection;
        private IManualControlBoardService _manualControlBoardService;
        private int _addedAmount = 1;
        private EditableManualControlBoards _mcbCollection;
        private List<ManualControlBoard> _lstManualControlBoard = null;
        private Visibility _createManualControlBoardVisibility = Visibility.Hidden; //添加手动盘数据视图可见性
        private Visibility _deviceItemSelectorVisibility= Visibility.Hidden; //选择器件视图可见性
        private Visibility _manualDeviceCodeVisibility = Visibility.Hidden; //NT8053设备代码
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private object _detailType = GridDetailType.ManualControlBoard;
        private SCA.WPF.CreateManualControlBoard.CreateManualControlBoardViewModel _createVM; //创建手动盘视图ViewModel
        private SCA.WPF.DeviceItemSelector.DeviceItemSelectorViewModel _deviceItemSelectorVM; //选择器件视图ViewModel
        private SCA.WPF.ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel _manualDeviceCodeVM; //NT8053设备代码

        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        private Visibility _isVisualColumnGroup1 = Visibility.Visible;

        public Visibility IsVisualColumnGroup1
        {
            get
            {
                return _isVisualColumnGroup1;
            }
            set
            {
                _isVisualColumnGroup1 = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        private Visibility _isVisualColumnGroup2 = Visibility.Collapsed;

        public Visibility IsVisualColumnGroup2
        {
            get
            {
                return _isVisualColumnGroup2;
            }
            set
            {
                _isVisualColumnGroup2 = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public List<DeviceType> ValidDeviceType
        {
            get
            {
                IControllerConfig config = SCA.BusinessLib.BusinessLogic.ControllerConfigManager.GetConfigObject(TheController.Type);                
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfo();                
                return lstDeviceType;
       
            }
        }

        public Visibility CreateManualControlBoardVisibility
        {
            get
            {
                return _createManualControlBoardVisibility;
            }
            set
            {
                _createManualControlBoardVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        
        public Visibility DeviceItemSelectorVisibility
        {
            get
            {
                return _deviceItemSelectorVisibility;
            }
            set
            {
                _deviceItemSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ManualDeviceCodeVisibility
        {
            get
            {
                return _deviceItemSelectorVisibility;
            }
            set
            {
                _deviceItemSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        

        
        public SCA.WPF.CreateManualControlBoard.CreateManualControlBoardViewModel CreateVM
        {
            get
            {                
                return _createVM;
            }
            set
            {
                _createVM = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public SCA.WPF.DeviceItemSelector.DeviceItemSelectorViewModel DeviceItemSelectorVM
        {
            get
            {
                return _deviceItemSelectorVM;
            }
            set
            {
                _deviceItemSelectorVM = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }        
        public SCA.WPF.ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel ManualDeviceCodeVM
        {
            get { return _manualDeviceCodeVM; }
            set
            {
                _manualDeviceCodeVM = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public ManualControlBoardViewModel()
        {
            _manualControlBoardService = new SCA.BusinessLib.BusinessLogic.ManualControlBoardService(TheController);
            EventMediator.Register("AddMoreLines", AddMoreLines);
            EventMediator.Register("RefreshCollection", RefreshCollection);
            EventMediator.Register("RefreshDeviceCode", RefreshDeviceCode);
            
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
        /// 获取当前控制器的标准组态信息
        /// </summary>
        public List<ManualControlBoard> ManualControlBoard
        {
            get
            {
                if (_lstManualControlBoard == null)
                {

                    _lstManualControlBoard = new List<ManualControlBoard>();
                }
                if (_mcbCollection != null)
                {
                    foreach (var s in _mcbCollection)
                    {
                        _lstManualControlBoard.Add(s.ToManualControlBoard());
                    }
                }
                return _lstManualControlBoard;
            }
        }
        
        /// <summary>
        /// 当前ManualControlBoardViewModel的控制器对象
        /// </summary>
        public ControllerModel TheController { get; set; }
        //public ManualControlBoardViewModel()
        //{
        //    _mcbCollection=new EditableManualControlBoards();
        //}

        public EditableManualControlBoards ManualControlBoardInfoObservableCollection
        {
            get
            {
                if (_mcbCollection == null)                {
                    _mcbCollection = new EditableManualControlBoards(TheController, null);
                    

                }
                return _mcbCollection;
            }
            set
            {
                _mcbCollection = value;       
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
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
        //public ObservableCollection<ManualControlBoard> ManualControlBoardInfoObservableCollection
        //{
        //    get
        //    {
        //        if (_manualControlBoardInfoObservableCollection == null)
        //        {
        //            _manualControlBoardInfoObservableCollection = new ObservableCollection<ManualControlBoard>();
        //        }
        //        return _manualControlBoardInfoObservableCollection;
        //    }
        //    set
        //    {
        //        _manualControlBoardInfoObservableCollection = value;
        //        _maxCode = GetMaxCode(value);
        //        NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
        //    }
        //}
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
        public ICommand AddMoreRecordCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(AddMoreRecordExecute, null);
                
            }
        }
        public ICommand SelectDeviceCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand<object>(SelectDeviceExecute,null);

            }
        }
        public ICommand SelectLocalDevice1Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectLocalDevice1Execute, null); } }
        public ICommand SelectLocalDevice2Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectLocalDevice2Execute, null); } }
        public ICommand SelectLocalDevice3Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectLocalDevice3Execute, null); } }
        public ICommand SelectLocalDevice4Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectLocalDevice4Execute, null); } }

        public ICommand SelectNetDevice1Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectNetDevice1Execute, null); } }
        public ICommand SelectNetDevice2Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectNetDevice2Execute, null); } }
        public ICommand SelectNetDevice3Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectNetDevice3Execute, null); } }
        public ICommand SelectNetDevice4Command { get { return new SCA.WPF.Utility.RelayCommand<object>(SelectNetDevice4Execute, null); } }

        public void AddMoreRecordExecute()
        {
         //   SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLines", ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).AddMoreLines);
            //SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLinesRefreshData", RefreshData);


            CreateManualControlBoard.CreateManualControlBoardViewModel  createVM = new CreateManualControlBoard.CreateManualControlBoardViewModel();
            createVM.TheController = TheController;
            CreateVM = createVM;
            this.CreateManualControlBoardVisibility = Visibility.Visible;
            this.DeviceItemSelectorVisibility = Visibility.Collapsed;
            ManualDeviceCodeVisibility = Visibility.Collapsed;
            SCA.WPF.Infrastructure.EventMediator.Register("ManualControlBoardAddMoreLines", AddMoreLines);
        }
        public void AddNewRecordExecute(int rowsAmount)
        {
            
            #region 调业务逻辑
            _manualControlBoardService.TheController = this.TheController;
            //List<ManualControlBoard> lstMCB = _manualControlBoardService.Create(rowsAmount);
            List<ManualControlBoard> lstMCB = _manualControlBoardService.Create(0,1,1,rowsAmount);
            #endregion 调业务逻辑
            foreach (var v in lstMCB)
            {
                EditableManualControlBoard mcb = new EditableManualControlBoard();
                mcb.Controller = v.Controller;
                mcb.ControllerID = v.ControllerID;
                mcb.ID = v.ID;
                mcb.Code = v.Code;
                mcb.BoardNo = v.BoardNo;
                mcb.SubBoardNo = v.SubBoardNo;
                mcb.KeyNo = v.KeyNo;
                ManualControlBoardInfoObservableCollection.Add(mcb);
            }
        }


        public void DownloadExecute()
        {
            ProjectManager.GetInstance.NTConnection.SetManualBoardSetup(TheController.ControlBoard, this.TheController.Type);
            //if(this.TheController.Type == ControllerType.NT8053)
            //{
            //    ProjectManager.GetInstance.NTConnection.SetManualBoardSetup(TheController.ControlBoard, this.TheController.Type);
            //}
            //else
            //{
            //    _manualControlBoardService.TheController = this.TheController;
            //    _manualControlBoardService.DownloadExecute(ManualControlBoard);
            //}
        }
        public void SaveExecute()
        {
            using (new WaitCursor())
            {
                SCA.Interface.BusinessLogic.IManualControlBoardService _mcbService = new ManualControlBoardService(TheController);
                _mcbService.SaveToDB();
            }
        }
        public void SelectDeviceExecute(object args)
        {
            if (args != null)
            {
                EditableManualControlBoard eMCB = args as EditableManualControlBoard;
                if(eMCB != null)
                {
                    ManualControlBoard mcb = eMCB.ToManualControlBoard();
                    CreateManualControlBoardVisibility = Visibility.Collapsed;
                    DeviceItemSelectorVisibility = Visibility.Visible;
                    ManualDeviceCodeVisibility = Visibility.Collapsed;
                    DeviceItemSelector.DeviceItemSelectorViewModel deviceItemSelectorVM = new DeviceItemSelector.DeviceItemSelectorViewModel();
                    deviceItemSelectorVM.TheController = TheController;
                    deviceItemSelectorVM.MCB = mcb;
                    DeviceItemSelectorVM = deviceItemSelectorVM;
                }
            }            
        }
        public void SelectLocalDevice1Execute(object args)
        {
            SelectDeviceExecute(1, args);
        }
        public void SelectLocalDevice2Execute(object args)
        {
            SelectDeviceExecute(2, args);
        }
        public void SelectLocalDevice3Execute(object args)
        {
            SelectDeviceExecute(3, args);
        }
        public void SelectLocalDevice4Execute(object args)
        {
            SelectDeviceExecute(4, args);
        }
        public void SelectNetDevice1Execute(object args)
        {
            SelectDeviceExecute(5, args);
        }
        public void SelectNetDevice2Execute(object args)
        {
            SelectDeviceExecute(6, args);
        }
        public void SelectNetDevice3Execute(object args)
        {
            SelectDeviceExecute(7, args);
        }
        public void SelectNetDevice4Execute(object args)
        {
            SelectDeviceExecute(8, args);
        }

        public void SelectDeviceExecute(int id, object args)
        {
            if (args != null)
            {
                EditableManualControlBoard eMCB = args as EditableManualControlBoard;
                if (eMCB != null)
                {
                    ManualControlBoard mcb = eMCB.ToManualControlBoard();
                    CreateManualControlBoardVisibility = Visibility.Collapsed;
                    DeviceItemSelectorVisibility = Visibility.Collapsed;
                    ManualDeviceCodeVisibility = Visibility.Visible;
                    ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel deviceCodeVM = new ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel();
                    deviceCodeVM.ID = id;
                    deviceCodeVM.MCB = mcb;
                    ManualDeviceCodeVM = deviceCodeVM;
                }
            }
        }
        public void AddMoreLines(object param)
        {
            if (param != null)
            { 
                // ObservableCollection<ManualControlBoard> ocMCB = ManualControlBoardInfoObservableCollection;
                object[] values = (object[])param;
                #region 调业务逻辑
                _manualControlBoardService.TheController = this.TheController;
                //SCA.Interface.IControllerConfig config =ControllerConfigManager.GetConfigObject(this.TheController.Type);
                //int totalMaxKeyNo=config.GetMaxAmountForKeyNoInManualControlBoardConfig();
                //for (int i = Convert.ToInt32(values[1]); i <= Convert.ToInt32(values[2]); i++)
                //{
                //    int maxKeyNo=1;
                //    //获取当前板卡及回路下的最大"手键号"
                //    if (ManualControlBoardInfoObservableCollection.Count == 0)
                //    {
                //        maxKeyNo = 1;
                //    }
                //    else
                //    {
                //        var result = ManualControlBoardInfoObservableCollection.Where(mcb => mcb.MaxSubBoardNo == Convert.ToInt32(values[1]) && mcb.BoardNo == Convert.ToInt32(values[0]));
                //        if(result.Count() != 0)
                //            maxKeyNo = ManualControlBoardInfoObservableCollection.Where(mcb => mcb.MaxSubBoardNo == Convert.ToInt32(values[1]) && mcb.BoardNo == Convert.ToInt32(values[0])).Max(mcb => mcb.KeyNo);
                //    }
                //   if (maxKeyNo < totalMaxKeyNo)
                //   {

                       List<ManualControlBoard> lstMCB = _manualControlBoardService.Create(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]),  Convert.ToInt32(values[3]));
                       foreach (var v in lstMCB)
                       {
                           EditableManualControlBoard mcb = new EditableManualControlBoard();
                           mcb.Controller = v.Controller;
                           mcb.ControllerID = v.ControllerID;
                           mcb.ID = v.ID;
                           mcb.Code = v.Code;
                           mcb.BoardNo = v.BoardNo;
                           mcb.SubBoardNo = v.SubBoardNo;
                           mcb.KeyNo = v.KeyNo;
                           ManualControlBoardInfoObservableCollection.Add(mcb);                       
                       }   
                //   }                
                //}
                #endregion 调业务逻辑
            }            
            this.CreateManualControlBoardVisibility = Visibility.Collapsed;            
        }
        public void RefreshCollection(object param)
        {
            if (param != null)
            { 
                ManualControlBoard mcb = (ManualControlBoard)param;                
                EditableManualControlBoard eMCB= ManualControlBoardInfoObservableCollection.Where((d) => d.ID == mcb.ID).FirstOrDefault();                
                eMCB.DeviceCode=mcb.DeviceCode;
            }            
            DeviceItemSelectorVisibility = Visibility.Collapsed;                
        }

        public void RefreshDeviceCode(object param)
        {
            try
            {
                SCA.WPF.ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel deviceCodeVM = param as SCA.WPF.ManualBoardDeviceCode.ManualBoardDeviceCodeViewModel;
                EditableManualControlBoard eMCB = ManualControlBoardInfoObservableCollection.Where((d) => d.ID == deviceCodeVM.MCB.ID).FirstOrDefault();
                if(deviceCodeVM != null)
                {
                    switch (deviceCodeVM.ID)
                    {
                        case 1:
                            eMCB.LocalDevice1 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.LocalDevice1 = eMCB.LocalDevice1;
                            break;
                        case 2:
                            eMCB.LocalDevice2 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.LocalDevice2 = eMCB.LocalDevice2;
                            break;
                        case 3:
                            eMCB.LocalDevice3= deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.LocalDevice3 = eMCB.LocalDevice3;
                            break;
                        case 4:
                            eMCB.LocalDevice4 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.LocalDevice4 = eMCB.LocalDevice4;
                            break;
                        case 5:
                            eMCB.NetDevice1 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.NetDevice1 = eMCB.NetDevice1;
                            break;
                        case 6:
                            eMCB.NetDevice2 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.NetDevice2 = eMCB.NetDevice2;
                            break;
                        case 7:
                            eMCB.NetDevice3 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.NetDevice3 = eMCB.NetDevice3;
                            break;
                        case 8:
                            eMCB.NetDevice4 = deviceCodeVM.GetResultCode();
                            deviceCodeVM.MCB.NetDevice4 = eMCB.NetDevice4;
                            break;
                    }
                }
                ManualControlBoardService mcbService = new ManualControlBoardService(TheController);
                mcbService.Update(deviceCodeVM.MCB);
                ManualDeviceCodeVisibility = Visibility.Collapsed;   
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            
        }

        public void Dispose()
        {
            this.ManualControlBoardInfoObservableCollection = null;
        }
    }
}