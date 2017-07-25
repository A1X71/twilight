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
            this.SDPKey = mcb.SDPKey;
            this.MaxSubBoardNo = mcb.MaxSubBoardNo;
            this.Controller = mcb.Controller;
            this.ControllerID = mcb.ControllerID;
        }
        private string _deviceCode;
        public string DeviceCode 
        {
            get
            {
                return _deviceCode;
            }
            set
            {
                
                if(_deviceCode!=value )
                {
                    _deviceCode = value;
                    RaisePropertyChanged("DeviceCode");
                }
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
            mcb.SDPKey = this.SDPKey;
            mcb.MaxSubBoardNo = this.MaxSubBoardNo;
            mcb.Controller = this.Controller;
            mcb.ControllerID = this.ControllerID;
            return mcb;
        }
        public void BeginEdit()
        {
            string str = "new string";
            
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


        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                ControllerConfig8001 config = new ControllerConfig8001();
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetManualControlBoardRegularExpression(8);
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
                        rule = dictMessage["DeviceCode"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.DeviceCode.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
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
    public class ManualControlBoardViewModel : PropertyChangedBase
    {
        //private ObservableCollection<ManualControlBoard> _manualControlBoardInfoObservableCollection;
        private IManualControlBoardService _manualControlBoardService;
        private int _addedAmount = 1;
        private EditableManualControlBoards _mcbCollection;
        private List<ManualControlBoard> _lstManualControlBoard = null;
        private Visibility _createManualControlBoardVisibility = Visibility.Hidden; //添加手动盘数据视图可见性
        private Visibility _deviceItemSelectorVisibility= Visibility.Hidden; //选择器件视图可见性
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        
        private SCA.WPF.CreateManualControlBoard.CreateManualControlBoardViewModel _createVM; //创建手动盘视图ViewModel
        private SCA.WPF.DeviceItemSelector.DeviceItemSelectorViewModel _deviceItemSelectorVM; //选择器件视图ViewModel

        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

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
        public ManualControlBoardViewModel()
        {
            _manualControlBoardService = new SCA.BusinessLib.BusinessLogic.ManualControlBoardService(TheController);
            EventMediator.Register("AddMoreLines", AddMoreLines);
            EventMediator.Register("RefreshCollection", RefreshCollection);
            
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
                if (_mcbCollection == null)
                {
                    _mcbCollection = new EditableManualControlBoards(TheController, null);
                    _mcbCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(ManualControlBoardCollectionChanged);

                }
                return _mcbCollection;
            }
            set
            {
                _mcbCollection = value;
                _mcbCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(ManualControlBoardCollectionChanged);

                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
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
        public void AddMoreRecordExecute()
        {
         //   SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLines", ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).AddMoreLines);
            //SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLinesRefreshData", RefreshData);


            CreateManualControlBoard.CreateManualControlBoardViewModel  createVM = new CreateManualControlBoard.CreateManualControlBoardViewModel();
            createVM.TheController = TheController;
            CreateVM = createVM;
            this.CreateManualControlBoardVisibility = Visibility.Visible;
            this.DeviceItemSelectorVisibility = Visibility.Collapsed; 
            SCA.WPF.Infrastructure.EventMediator.Register("ManualControlBoardAddMoreLines", AddMoreLines);
        }
        public void AddNewRecordExecute(int rowsAmount)
        {
            //原对象
            // ObservableCollection<ManualControlBoard> ocMCB = ManualControlBoardInfoObservableCollection;
            #region 调业务逻辑
            _manualControlBoardService.TheController = this.TheController;
            List<ManualControlBoard> lstMCB = _manualControlBoardService.Create(rowsAmount);
            #endregion 调业务逻辑


            foreach (var v in lstMCB)
            {
                EditableManualControlBoard mcb = new EditableManualControlBoard();
                mcb.Controller = v.Controller;
                mcb.ControllerID = v.ControllerID;
                mcb.ID = v.ID;
                mcb.Code = v.Code;
                ManualControlBoardInfoObservableCollection.Add(mcb);
            }
        }


        public void DownloadExecute()
        {
            _manualControlBoardService.TheController = this.TheController;
            _manualControlBoardService.DownloadExecute(ManualControlBoard);
        }
        public void SelectDeviceExecute(object args)
        {
            if (args != null)
            {
                EditableManualControlBoard eMCB = (EditableManualControlBoard)args;
                ManualControlBoard mcb = eMCB.ToManualControlBoard();
                CreateManualControlBoardVisibility = Visibility.Collapsed;
                DeviceItemSelectorVisibility = Visibility.Visible;
                DeviceItemSelector.DeviceItemSelectorViewModel deviceItemSelectorVM = new DeviceItemSelector.DeviceItemSelectorViewModel();
                deviceItemSelectorVM.TheController = TheController;
                deviceItemSelectorVM.MCB = mcb;                
                DeviceItemSelectorVM = deviceItemSelectorVM;
            }            
        }
        private void ManualControlBoardCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    string s = "Fired";
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
                SCA.Interface.IControllerConfig config =ControllerConfigManager.GetConfigObject(this.TheController.Type);
                int totalMaxKeyNo=config.GetMaxAmountForKeyNoInManualControlBoardConfig();
                for (int i = Convert.ToInt32(values[1]); i <= Convert.ToInt32(values[2]); i++)
                {
                    int maxKeyNo=1;
                    //获取当前板卡及回路下的最大"手键号"
                    if (ManualControlBoardInfoObservableCollection.Count == 0)
                    {
                        maxKeyNo = 1;
                    }
                    else
                    {
                        var result = ManualControlBoardInfoObservableCollection.Where(mcb => mcb.MaxSubBoardNo == i && mcb.BoardNo == Convert.ToInt32(values[0]));
                        if(result.Count() != 0)
                            maxKeyNo = ManualControlBoardInfoObservableCollection.Where(mcb => mcb.MaxSubBoardNo == i && mcb.BoardNo == Convert.ToInt32(values[0])).Max(mcb => mcb.KeyNo);
                    }
                   if (maxKeyNo < totalMaxKeyNo)
                   {

                       List<ManualControlBoard> lstMCB = _manualControlBoardService.Create(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), maxKeyNo, Convert.ToInt32(values[3]));
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
                }
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
    }
}