using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.Interface.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:38:48
* FileName   : LinkageConfigStandardViewModel
* Description: 标准组态操作逻辑
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{

    public class EditableLinkageConfigStandard : LinkageConfigStandard, IEditableObject,IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        private EditableLinkageConfigStandard copy;
        public EditableLinkageConfigStandard()
        { 
        
        }
        public EditableLinkageConfigStandard(LinkageConfigStandard linkageConfigStandard)
        {
            this.ID = linkageConfigStandard.ID;
            this.Controller = linkageConfigStandard.Controller;
            this.ControllerID = linkageConfigStandard.ControllerID;
            this.Code = linkageConfigStandard.Code;
            this.ActionCoefficient = linkageConfigStandard.ActionCoefficient;
            this.DeviceNo1 = linkageConfigStandard.DeviceNo1;
            this.DeviceNo2 = linkageConfigStandard.DeviceNo2;
            this.DeviceNo3 = linkageConfigStandard.DeviceNo3;
            this.DeviceNo4 = linkageConfigStandard.DeviceNo4;
            this.DeviceNo5 = linkageConfigStandard.DeviceNo5;
            this.DeviceNo6 = linkageConfigStandard.DeviceNo6;
            this.DeviceNo7 = linkageConfigStandard.DeviceNo7;
            this.DeviceNo8 = linkageConfigStandard.DeviceNo8;
            this.DeviceNo9 = linkageConfigStandard.DeviceNo9;
            this.DeviceNo10 = linkageConfigStandard.DeviceNo10;
            this.DeviceNo11 = linkageConfigStandard.DeviceNo11;
            this.DeviceNo12 = linkageConfigStandard.DeviceNo12;
            this.OutputDevice1 = linkageConfigStandard.OutputDevice1;
            this.OutputDevice2 = linkageConfigStandard.OutputDevice2;
            this.LinkageNo1 = linkageConfigStandard.LinkageNo1;
            this.LinkageNo2 = linkageConfigStandard.LinkageNo2;
            this.LinkageNo3 = linkageConfigStandard.LinkageNo3;
            this.Memo = linkageConfigStandard.Memo;

        }
        public LinkageConfigStandard ToLinkageConfigStandard()
        {
            LinkageConfigStandard linkageConfigStandard = new LinkageConfigStandard();

            linkageConfigStandard.ID = this.ID;
            linkageConfigStandard.Controller = this.Controller;
            linkageConfigStandard.ControllerID = this.ControllerID;
            linkageConfigStandard.Code = this.Code;
            linkageConfigStandard.ActionCoefficient = this.ActionCoefficient;
            linkageConfigStandard.DeviceNo1 = this.DeviceNo1;
            linkageConfigStandard.DeviceNo2 = this.DeviceNo2;
            linkageConfigStandard.DeviceNo3 = this.DeviceNo3;
            linkageConfigStandard.DeviceNo4 = this.DeviceNo4;
            linkageConfigStandard.DeviceNo5 = this.DeviceNo5;
            linkageConfigStandard.DeviceNo6 = this.DeviceNo6;
            linkageConfigStandard.DeviceNo7 = this.DeviceNo7;
            linkageConfigStandard.DeviceNo8 = this.DeviceNo8;
            linkageConfigStandard.DeviceNo9 = this.DeviceNo9;
            linkageConfigStandard.DeviceNo10 = this.DeviceNo10;
            linkageConfigStandard.DeviceNo11 = this.DeviceNo11;
            linkageConfigStandard.DeviceNo12 = this.DeviceNo12;
            linkageConfigStandard.OutputDevice1 = this.OutputDevice1;
            linkageConfigStandard.OutputDevice2 = this.OutputDevice2;
            linkageConfigStandard.LinkageNo1 = this.LinkageNo1;
            linkageConfigStandard.LinkageNo2 = this.LinkageNo2;
            linkageConfigStandard.LinkageNo3 = this.LinkageNo3;
            linkageConfigStandard.Memo = this.Memo;
            return linkageConfigStandard;
        }

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
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = Config.GetStandardLinkageConfigRegularExpression(this.Controller.DeviceAddressLength);
                ControllerType currentType = this.Controller.Type;
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
        //                public short MaxStandardLinkageConfigAmount
        //{
        //    get
        //    {
        //        if (_maxStandardLinkageConfigAmount == 0)//&& TheController!=null 后续在加至条件中
        //        {                    
        //            _maxStandardLinkageConfigAmount = ControllerConfigManager.GetConfigObject(TheController.TypeCode).GetMaxAmountForStandardLinkageConfig();
        //        }
        //        return _maxStandardLinkageConfigAmount;
        //    }
        //}
                //switch (columnName)
                //{
                //    case "Code":
                //        if(!string.IsNullOrEmpty(this.Code))
                //        {
                //            rule = dictMessage["Code"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.Code.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo1":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo1.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo2":
                //      {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo2.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo3":
                //         {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo3.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo4":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo4.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo5":
                //        {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo5.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo6":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo6.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo7":
                //        {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo7.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo8":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo8.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo9":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo9.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo10":
                //        {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo10.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo11":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo11.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "DeviceNo12":
                //        {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.DeviceNo12.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "OutputDevice1":
                //       {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.OutputDevice1.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "OutputDevice2":
                //        {                   
                //            rule = dictMessage["DeviceCode"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.OutputDevice2.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "LinkageNo1":
                //        if (!string.IsNullOrEmpty(this.LinkageNo1))
                //        { 
                //            rule = dictMessage["Code"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.LinkageNo1.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "LinkageNo2":
                //        if (!string.IsNullOrEmpty(this.LinkageNo2))
                //        {
                //            rule = dictMessage["Code"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.LinkageNo2.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;
                //    case "LinkageNo3":
                //        if (!string.IsNullOrEmpty(this.LinkageNo3))
                //        {
                //            rule = dictMessage["Code"];
                //            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                //            if (!exminator.IsMatch(this.LinkageNo3.ToString()))
                //            {
                //                errorMessage = rule.ErrorMessage;
                //            }
                //        }
                //        break;

                //}
                return errorMessage;
            }
        }
    }

    public class EditableLinkageConfigStandards : ObservableCollection<EditableLinkageConfigStandard>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.BusinessLogic.ILinkageConfigStandardService _linkageConfigStandardService;
        private ControllerModel _controller;
        public EditableLinkageConfigStandards(ControllerModel controller,List<LinkageConfigStandard> lstLinkageConfigStandard)
        {
            _controller = controller;
            _linkageConfigStandardService = new SCA.BusinessLib.BusinessLogic.LinkageConfigStandardService(_controller);
                   
            if (lstLinkageConfigStandard != null)
            {
               foreach(var o in lstLinkageConfigStandard)
               {
                 this.Add(new EditableLinkageConfigStandard(o));
               }
            }
        }
        protected override void InsertItem(int index, EditableLinkageConfigStandard item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableLinkageConfigStandard o = (EditableLinkageConfigStandard)sender;
            
            o.Controller = _controller;
            o.ControllerID = _controller.ID;
            _linkageConfigStandardService.Update(o.ToLinkageConfigStandard());

            
        }

    }
    public class LinkageConfigStandardViewModel:PropertyChangedBase,IDisposable
    {
        //private ObservableCollection<LinkageConfigStandard> _standardLinkageConfigInfoObservableCollection;
        private ILinkageConfigStandardService _linkageConfigStandardService;
        private int _addedAmount = 1;//向集合中新增LinkageConfigStandard的数量
        
        private EditableLinkageConfigStandards _lcsCollection;
        private List<LinkageConfigStandard> _lstLinkageConfigStandard=null;
        private Visibility _isVisualColumnGroup1=Visibility.Visible;
        private Visibility _isVisualColumnGroup2 = Visibility.Visible;
        private Visibility _isVisualColumnGroup3 = Visibility.Collapsed;
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private object _detailType = GridDetailType.Standard;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }


        public LinkageConfigStandardViewModel()
        {
            _linkageConfigStandardService = new SCA.BusinessLib.BusinessLogic.LinkageConfigStandardService(TheController);
        }
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

        public Visibility IsVisualColumnGroup3
        {
            get
            {
                return _isVisualColumnGroup3;
            }
            set
            {
                _isVisualColumnGroup3 = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
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
        /// 当前LinkageConfigStandardViewModel的控制器对象
        /// </summary>
        public ControllerModel TheController { get; set; }
        /// <summary>
        /// 获取当前控制器的标准组态信息
        /// </summary>
        public List<LinkageConfigStandard> LinkageConfigStandard
        { 
            get
            {
                if (_lstLinkageConfigStandard == null)
                {
                    
                    _lstLinkageConfigStandard=new List<LinkageConfigStandard>();
                }
                if (_lcsCollection != null)
                {
                    foreach (var s in _lcsCollection)
                    {
                        _lstLinkageConfigStandard.Add(s.ToLinkageConfigStandard());
                    }
                }
                return _lstLinkageConfigStandard;
            }
        }

        public EditableLinkageConfigStandards StandardLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_lcsCollection == null)
                {
                    _lcsCollection = new EditableLinkageConfigStandards(TheController, null);
                    

                }
                return _lcsCollection;
            }
            set
            {
                _lcsCollection = value;
                
                //_maxCode = GetMaxCode(value);
               
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
            
            _linkageConfigStandardService.TheController = this.TheController;
            List<LinkageConfigStandard> lstLinkageConfigStandard=_linkageConfigStandardService.Create(rowsAmount);
            foreach (var v in lstLinkageConfigStandard)
            { 
                EditableLinkageConfigStandard eLCS = new EditableLinkageConfigStandard();
                eLCS.Controller = v.Controller;
                eLCS.ControllerID = v.ControllerID;
                eLCS.ID = v.ID;
                eLCS.Code = v.Code;
                StandardLinkageConfigInfoObservableCollection.Add(eLCS);
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
            _linkageConfigStandardService.TheController = this.TheController;
            _linkageConfigStandardService.DownloadExecute(LinkageConfigStandard);       
        }
        public void UploadExecute()
        { 
            
        }
        public void SaveExecute()
        {
            using (new WaitCursor())
            {
                SCA.Interface.BusinessLogic.ILinkageConfigStandardService _standardService = new LinkageConfigStandardService(TheController);
                _standardService.SaveToDB();
            }
        }
        #region 作废->改为实现IEditableObject接口的集合
        //public ObservableCollection<LinkageConfigStandard> StandardLinkageConfigInfoObservableCollection
        //{
        //    get
        //    {
        //        if (_standardLinkageConfigInfoObservableCollection == null)
        //        {
        //            _standardLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigStandard>();
        //        }
        //        return _standardLinkageConfigInfoObservableCollection;
        //    }
        //    set
        //    {
        //        _standardLinkageConfigInfoObservableCollection = value;
        //        NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());

        //    }
        //}
        #endregion
        //移至BussinessLogic的Service中
        //private int GetMaxCode(EditableLinkageConfigStandards ocLCS)
        //{
        //    int result = 0;
        //    if (ocLCS != null)
        //    {
        //        var query = from input in ocLCS   select input.Code;
        //        if (query != null)
        //        {
        //            foreach (var i in query)
        //            {
        //                if (Convert.ToInt32(i) > result)
        //                {
        //                    result = Convert.ToInt32(i);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}


        public void Dispose()
        {
            this.StandardLinkageConfigInfoObservableCollection = null;
            //GC.Collect();
           // GC.WaitForPendingFinalizers();
            //GC.SuppressFinalize(true);
        }
    }
}
