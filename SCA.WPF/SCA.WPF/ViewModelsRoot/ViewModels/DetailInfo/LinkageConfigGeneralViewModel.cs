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
using SCA.BusinessLib.Controller;
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
    public class EditableLinkageConfigGeneral : LinkageConfigGeneral, IEditableObject,IDataErrorInfo
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
            this.BuildingNoA=linkageConfigGeneral.BuildingNoA;
            this.ZoneNoA=linkageConfigGeneral.ZoneNoA;
            this.LayerNoA1=linkageConfigGeneral.LayerNoA1;
            this.LayerNoA2=linkageConfigGeneral.LayerNoA2;
            this.DeviceTypeCodeA=linkageConfigGeneral.DeviceTypeCodeA;
	        this.TypeC=linkageConfigGeneral.TypeC;
            this.MachineNoC=linkageConfigGeneral.MachineNoC;
            this.LoopNoC=linkageConfigGeneral.LoopNoC;
            this.DeviceCodeC=linkageConfigGeneral.DeviceCodeC;
            this.BuildingNoC=linkageConfigGeneral.BuildingNoC;
            this.ZoneNoC=linkageConfigGeneral.ZoneNoC;
            this.LayerNoC=linkageConfigGeneral.LayerNoC;
            this.DeviceTypeCodeC = linkageConfigGeneral.DeviceTypeCodeC;

        }
        public LinkageConfigGeneral ToLinkageConfigGeneral()
        {
            LinkageConfigGeneral linkageConfigGeneral = new LinkageConfigGeneral();

            linkageConfigGeneral.Controller = this.Controller;
            linkageConfigGeneral.ControllerID = this.ControllerID;
            linkageConfigGeneral.ID = this.ID;
            linkageConfigGeneral.Code =this.Code;
            linkageConfigGeneral.ActionCoefficient = this.ActionCoefficient;
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
                        rule = dictMessage["Code"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.Code.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
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
                        rule = dictMessage["Building"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.BuildingNoA.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "ZoneNoA":
                        rule = dictMessage["Zone"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.ZoneNoA.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "LayerNoA1":
                        rule = dictMessage["FloorNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.LayerNoA1.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "LayerNoA2":
                        rule = dictMessage["FloorNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.LayerNoA2.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "MachineNoC":
                        rule = dictMessage["MachineNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.MachineNoC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "LoopNoC":
                        rule = dictMessage["Loop"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.LoopNoC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "DeviceCodeC":
                        rule = dictMessage["Device"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.DeviceCodeC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "BuildingNoC":
                        rule = dictMessage["Building"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.BuildingNoC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "ZoneNoC":
                        rule = dictMessage["Zone"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.ZoneNoC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "LayerNoC":
                        rule = dictMessage["FloorNo"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.LayerNoC.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
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
            ControllerConfigBase config = new ControllerConfigBase();
            return config.GetLinkageType();
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
        //public ICommand UploadCommand
        //{ 

        //}
        /// <summary>
        /// 添加指定数量的标准组态信息
        /// </summary>
        /// <param name="rowsAmount"></param>
        public void AddNewRecordExecute(int rowsAmount)
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8001 config = new BusinessLib.BusinessLogic.ControllerConfig8001();
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfoWithAnyAlarm();                
                return lstDeviceType;
       
            }
        }
        public List<DeviceType> ValidDeviceTypeC
        {
            get
            {
                SCA.BusinessLib.BusinessLogic.ControllerConfig8001 config = new BusinessLib.BusinessLogic.ControllerConfig8001();
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfoWithoutFireDevice();                 
                return lstDeviceType;
            }
        }


    }


}
