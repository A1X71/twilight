using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
* Create Date: 2017/3/12 15:35:37
* FileName   : LinkageConfigMixedViewModel
* Description: 混合组态操作逻辑
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableLinkageConfigMixed: LinkageConfigMixed, IEditableObject,IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        private EditableLinkageConfigStandard copy;
        public EditableLinkageConfigMixed()
        {

        }
        public EditableLinkageConfigMixed(LinkageConfigMixed linkageConfigMixed)
        {
            this.Controller=linkageConfigMixed.Controller;
            this.ControllerID = linkageConfigMixed.ControllerID;
            this.ID = linkageConfigMixed.ID;
            this.Code=linkageConfigMixed.Code;
            this.ActionCoefficient=linkageConfigMixed.ActionCoefficient;
            this.ActionType=linkageConfigMixed.ActionType;
            //分类A
            this.TypeA=linkageConfigMixed.TypeA;
            //回路A
            //器件编号A [当分类为“地址”时，存储的"回路编号"]
            this.LoopNoA=linkageConfigMixed.LoopNoA;
            //器件编号A [当分类为“地址”时，存储的"器件编号"]
            this.DeviceCodeA=linkageConfigMixed.DeviceCodeA;
            //楼号A
            this.BuildingNoA=linkageConfigMixed.BuildingNoA;
            //区号A
            this.ZoneNoA=linkageConfigMixed.ZoneNoA;
            //层号A
            this.LayerNoA=linkageConfigMixed.LayerNoA;
            //器件类型A
            this.DeviceTypeCodeA=linkageConfigMixed.DeviceTypeCodeA;
            this.TypeB=linkageConfigMixed.TypeB;
            this.LoopNoB=linkageConfigMixed.LoopNoB;
            this.DeviceCodeB=linkageConfigMixed.DeviceCodeB;
            this.BuildingNoB=linkageConfigMixed.BuildingNoB;
            this.ZoneNoB=linkageConfigMixed.ZoneNoB;        
            this.LayerNoB=linkageConfigMixed.LayerNoB;
            this.DeviceTypeCodeB=linkageConfigMixed.DeviceTypeCodeB;        
            this.TypeC=linkageConfigMixed.TypeC;
            this.MachineNoC=linkageConfigMixed.MachineNoC;
            this.LoopNoC=linkageConfigMixed.LoopNoC;
            this.DeviceCodeC=linkageConfigMixed.DeviceCodeC;
            this.BuildingNoC=linkageConfigMixed.BuildingNoC;
            this.ZoneNoC=linkageConfigMixed.ZoneNoC;
            this.LayerNoC=linkageConfigMixed.LayerNoC;
            this.DeviceTypeCodeC = linkageConfigMixed.DeviceTypeCodeC;
        }
        public LinkageConfigMixed ToLinkageConfigMixed()
        {
            LinkageConfigMixed linkageConfigMixed = new LinkageConfigMixed();

            linkageConfigMixed.Controller=this.Controller;
            linkageConfigMixed.ControllerID = this.ControllerID;
            linkageConfigMixed.ID = this.ID;
            linkageConfigMixed.Code = this.Code;
            linkageConfigMixed.ActionCoefficient = this.ActionCoefficient;
            linkageConfigMixed.ActionType = this.ActionType;
            //分类A
            linkageConfigMixed.TypeA = this.TypeA;
            //回路A
            //器件编号A [当分类为“地址”时，存储的"回路编号"]
            linkageConfigMixed.LoopNoA = this.LoopNoA;
            //器件编号A [当分类为“地址”时，存储的"器件编号"]
            linkageConfigMixed.DeviceCodeA = this.DeviceCodeA;
            //楼号A
            linkageConfigMixed.BuildingNoA = this.BuildingNoA;
            //区号A
            linkageConfigMixed.ZoneNoA = this.ZoneNoA;
            //层号A
            linkageConfigMixed.LayerNoA = this.LayerNoA;
            //器件类型A
            linkageConfigMixed.DeviceTypeCodeA = this.DeviceTypeCodeA;
            linkageConfigMixed.TypeB = this.TypeB;
            linkageConfigMixed.LoopNoB = this.LoopNoB;
            linkageConfigMixed.DeviceCodeB = this.DeviceCodeB;
            linkageConfigMixed.BuildingNoB = this.BuildingNoB;
            linkageConfigMixed.ZoneNoB = this.ZoneNoB;
            linkageConfigMixed.LayerNoB = this.LayerNoB;
            linkageConfigMixed.DeviceTypeCodeB = this.DeviceTypeCodeB;
            linkageConfigMixed.TypeC = this.TypeC;
            linkageConfigMixed.MachineNoC = this.MachineNoC;
            linkageConfigMixed.LoopNoC = this.LoopNoC;
            linkageConfigMixed.DeviceCodeC = this.DeviceCodeC;
            linkageConfigMixed.BuildingNoC = this.BuildingNoC;
            linkageConfigMixed.ZoneNoC = this.ZoneNoC;
            linkageConfigMixed.LayerNoC = this.LayerNoC;
            linkageConfigMixed.DeviceTypeCodeC = this.DeviceTypeCodeC;
            return linkageConfigMixed;
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


        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                ControllerConfig8001 config = new ControllerConfig8001();
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetGeneralLinkageConfigRegularExpression(8);
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
                    case "LoopNoA":
                        if (!string.IsNullOrEmpty(this.LoopNoA))
                        {
                            rule = dictMessage["Loop"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LoopNoA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "DeviceCodeA":
                        if (!string.IsNullOrEmpty(this.DeviceCodeA))
                        {
                            rule = dictMessage["Device"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.DeviceCodeA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
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
                        if(this.ZoneNoA != null)
                        {
                            rule = dictMessage["Zone"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNoA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LayerNoA":
                        if(this.LayerNoA != null)
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LayerNoA.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LoopNoB":
                        if (!string.IsNullOrEmpty(this.LoopNoB))
                        {
                            rule = dictMessage["Loop"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LoopNoB.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "DeviceCodeB":
                        if (!string.IsNullOrEmpty(this.DeviceCodeB))
                        {
                            rule = dictMessage["Device"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.DeviceCodeB.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "BuildingNoB":
                        if (this.BuildingNoB != null)
                        {
                            rule = dictMessage["Building"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.BuildingNoB.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ZoneNoB":
                        if (this.ZoneNoB != null)
                        {
                            rule = dictMessage["Zone"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNoB.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LayerNoB":
                        if (this.LayerNoB != null)
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LayerNoB.ToString()))
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
                        if(this.BuildingNoC != null)
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

    public class EditableLinkageConfigMixeds : ObservableCollection<EditableLinkageConfigMixed>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.BusinessLogic.ILinkageConfigMixedService _linkageConfigMixedService;
        private ControllerModel _controller;
        public EditableLinkageConfigMixeds(ControllerModel controller, List<LinkageConfigMixed> lstLinkageConfigMixed)
        {
            _controller = controller;
            _linkageConfigMixedService = new SCA.BusinessLib.BusinessLogic.LinkageConfigMixedService(_controller);
            if (lstLinkageConfigMixed != null)
            {
                foreach (var o in lstLinkageConfigMixed)
                {
                    this.Add(new EditableLinkageConfigMixed(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableLinkageConfigMixed item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableLinkageConfigMixed o = (EditableLinkageConfigMixed)sender;            
            o.Controller = _controller;
            o.ControllerID = _controller.ID;
            _linkageConfigMixedService.Update(o.ToLinkageConfigMixed());
        }
    }
    public class LinkageConfigMixedViewModel:PropertyChangedBase
    {
        
        private ILinkageConfigMixedService _linkageConfigMixedService;
        private int _addedAmount = 1;//向集合中新增LinkageConfigMixed的数量
        
        private EditableLinkageConfigMixeds _lcmCollection;
        private List<LinkageConfigMixed> _lstLinkageConfigMixed = null;
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        public LinkageConfigMixedViewModel()
        {
            _linkageConfigMixedService=new SCA.BusinessLib.BusinessLogic.LinkageConfigMixedService(TheController);
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
        /// 获取当前控制器的混合组态信息
        /// </summary>
        public List<LinkageConfigMixed> LinkageConfigMixed
        {
            get
            {
                if (_lstLinkageConfigMixed == null)
                {

                    _lstLinkageConfigMixed = new List<LinkageConfigMixed>();
                }
                if (_lcmCollection != null)
                {
                    foreach (var s in _lcmCollection)
                    {
                        _lstLinkageConfigMixed.Add(s.ToLinkageConfigMixed());
                    }
                }
                return _lstLinkageConfigMixed;
            }
        }
        /// <summary>
        /// 混合组态联动类型,仅有“区层”及“地址”
        /// </summary>
        /// <returns></returns>
        public List<Model.LinkageType> GetLinkageType()
        {
            ControllerConfigNone config = new ControllerConfigNone();
            return config.GetLinkageTypeWithCastration();
        }

        /// <summary>
        /// 有效器件类型
        /// </summary>
        public List<DeviceType> ValidDeviceType
        {
            get
            {
                SCA.Interface.IControllerConfig config = SCA.BusinessLib.BusinessLogic.ControllerConfigManager.GetConfigObject(TheController.Type);
                return config.GetDeviceTypeInfo();
            }
        }
        public  List<LinkageActionType> GetLinkageActionType()
        {
            ControllerConfigNone config = new ControllerConfigNone();
            return config.GetLinkageActionType();
        }

        public EditableLinkageConfigMixeds MixedLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_lcmCollection == null)
                {
                    _lcmCollection = new EditableLinkageConfigMixeds(TheController, null);
                    //_lcmCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(StandardLinkageConfigInfoObservableCollectionChanged);

                }
                return _lcmCollection;
            }
            set
            {
                _lcmCollection = value;
               // _lcmCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(StandardLinkageConfigInfoObservableCollectionChanged);
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
        //public ICommand UploadCommand
        //{ 

        //}
        /// <summary>
        /// 添加指定数量的混合组态信息
        /// </summary>
        /// <param name="rowsAmount"></param>
        public void AddNewRecordExecute(int rowsAmount)
        {
            _linkageConfigMixedService.TheController = this.TheController;
            List<LinkageConfigMixed> lstLinkageConfigMixed = _linkageConfigMixedService.Create(rowsAmount);
            foreach (var v in lstLinkageConfigMixed)
            {               
                EditableLinkageConfigMixed eLCM = new EditableLinkageConfigMixed();
                eLCM.Controller =v.Controller;
                eLCM.ControllerID = v.ControllerID;
                eLCM.ID = v.ID;
                eLCM.Code =v.Code;
                MixedLinkageConfigInfoObservableCollection.Add(eLCM);
            }            
        }
        public void DownloadExecute()
        {
            _linkageConfigMixedService.TheController = this.TheController;
            _linkageConfigMixedService.DownloadExecute(LinkageConfigMixed);
        }
        public void UploadExecute()
        {

        }




    }
}
