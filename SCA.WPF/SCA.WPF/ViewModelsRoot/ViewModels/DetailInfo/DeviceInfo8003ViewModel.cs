using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Specialized;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 16:58:22
* FileName   : DeviceInfo8003ViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableDeviceInfo8003 : DeviceInfo8003, IEditableObject, IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8003()
        {

        }
        public EditableDeviceInfo8003(DeviceInfo8003 deviceInfo8003)
        {
            this.Loop = deviceInfo8003.Loop;
            this.LoopID = deviceInfo8003.LoopID;
            this.ID = deviceInfo8003.ID;
            this.Code = deviceInfo8003.Code;
            this.TypeCode = deviceInfo8003.TypeCode;
            this.Feature = deviceInfo8003.Feature;
            this.SensitiveLevel = deviceInfo8003.SensitiveLevel;
            this.Disable = deviceInfo8003.Disable;
            this.LinkageGroup1 = deviceInfo8003.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8003.LinkageGroup2;
            this.LinkageGroup3 = deviceInfo8003.LinkageGroup3;
            this.DelayValue = deviceInfo8003.DelayValue;
            this.sdpKey = deviceInfo8003.sdpKey;
            this.ZoneNo = deviceInfo8003.ZoneNo;
            this.BroadcastZone = deviceInfo8003.BroadcastZone;
            this.Location = deviceInfo8003.Location;
        }
        public DeviceInfo8003 ToDeviceInfo8003()
        {
            DeviceInfo8003 deviceInfo8003 = new DeviceInfo8003();
            deviceInfo8003.Loop = this.Loop;
            deviceInfo8003.LoopID = this.LoopID;
            deviceInfo8003.ID = this.ID;
            deviceInfo8003.Code = this.Code;
            deviceInfo8003.TypeCode = this.TypeCode;
            deviceInfo8003.Feature = this.Feature;
            deviceInfo8003.SensitiveLevel = this.SensitiveLevel;
            deviceInfo8003.Disable = this.Disable;
            deviceInfo8003.LinkageGroup1 = this.LinkageGroup1;
            deviceInfo8003.LinkageGroup2 = this.LinkageGroup2;
            deviceInfo8003.LinkageGroup3 = this.LinkageGroup3;
            deviceInfo8003.DelayValue = this.DelayValue;
            deviceInfo8003.sdpKey = this.sdpKey;
            deviceInfo8003.ZoneNo = this.ZoneNo;
            deviceInfo8003.BroadcastZone = this.BroadcastZone;
            deviceInfo8003.Location = this.Location;
            return deviceInfo8003;
        }
        private EditableDeviceInfo8003 backupCopy;
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as EditableDeviceInfo8003;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Loop = backupCopy.Loop;
            this.LoopID = backupCopy.LoopID;
            this.ID = backupCopy.ID;
            this.Code = backupCopy.Code;
            this.TypeCode = backupCopy.TypeCode;
            this.Feature = backupCopy.Feature;
            this.SensitiveLevel = backupCopy.SensitiveLevel;
            this.Disable = backupCopy.Disable;
            this.LinkageGroup1 = backupCopy.LinkageGroup1;
            this.LinkageGroup2 = backupCopy.LinkageGroup2;
            this.LinkageGroup3 = backupCopy.LinkageGroup3;
            this.DelayValue = backupCopy.DelayValue;
            this.sdpKey = backupCopy.sdpKey;
            this.ZoneNo = backupCopy.ZoneNo;
            this.BroadcastZone = backupCopy.BroadcastZone;
            this.Location = backupCopy.Location;
        }

        public void EndEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            backupCopy = null;
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
                ControllerConfig8003 config = new ControllerConfig8003();
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(8);
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
                switch (columnName)
                {

                    //case "Disable":
                    //    rule = dictMessage["Disable"];
                    //    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                    //    if (!exminator.IsMatch(this.Disable.ToString()))
                    //    {
                    //        errorMessage = rule.ErrorMessage;
                    //    }
                    //    break;
                    case "SensitiveLevel":
                        if (this.SensitiveLevel != null)
                        { 
                            rule = dictMessage["SensitiveLevel"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.SensitiveLevel.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LinkageGroup1":
                        if (this.LinkageGroup1 != null)
                        {
                            if (this.LinkageGroup1.ToString() != "")
                            {
                                rule = dictMessage["StandardLinkageGroup"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.LinkageGroup1.ToString()))
                                {
                                    errorMessage = rule.ErrorMessage;
                                }
                            }
                        }
                        break;
                    case "LinkageGroup2":
                        if (this.LinkageGroup2 != null)
                        { 
                            if (this.LinkageGroup2.ToString() != "")
                            {
                                rule = dictMessage["StandardLinkageGroup"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.LinkageGroup2.ToString()))
                                {
                                    errorMessage = rule.ErrorMessage;
                                }
                            }
                        }
                        break;
                    case "LinkageGroup3":
                        if (this.LinkageGroup3 != null)
                        { 
                            if (this.LinkageGroup3.ToString() != "")
                            {
                                rule = dictMessage["StandardLinkageGroup"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.LinkageGroup3.ToString()))
                                {
                                    errorMessage = rule.ErrorMessage;
                                }
                            }
                        }
                        break;
                    case "DelayValue":
                        if (this.DelayValue != null)
                        {
                            rule = dictMessage["DelayValue"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.DelayValue.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "SDPKey":
                        if (!string.IsNullOrEmpty(this.sdpKey))
                        { 
                            rule = dictMessage["SDPKey"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.sdpKey.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ZoneNo":
                        if (this.ZoneNo != null)
                        { 
                            rule = dictMessage["ZoneNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "Location":
                        if (!string.IsNullOrEmpty(this.Location))
                        { 
                            rule = dictMessage["Location"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.Location.ToString()))
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


    public class EditableDeviceInfo8003Collection : ObservableCollection<EditableDeviceInfo8003>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8003> _deviceInfo8003Service;
        private LoopModel _loop;
        public EditableDeviceInfo8003Collection(LoopModel loop, List<DeviceInfo8003> lstDeviceInfo8003)
        {
            _loop = loop;
            if (lstDeviceInfo8003 != null)
            {
                foreach (var o in lstDeviceInfo8003)
                {
                    this.Add(new EditableDeviceInfo8003(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8003 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8003 o = (EditableDeviceInfo8003)sender;
            if (o.Code != null)
            {
                _deviceInfo8003Service = new SCA.BusinessLib.BusinessLogic.DeviceService8003();
                _deviceInfo8003Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8003Service.Update(o.ToDeviceInfo8003());
            }
        }
    }

    public class DeviceInfo8003ViewModel : PropertyChangedBase,IDisposable
    {
        private EditableDeviceInfo8003Collection _deviceInfoCollection;
        private List<DeviceInfo8003> _lstDeviceInfo8003;
        private DeviceService8003 _deviceService8003;
        //private int _maxCode = 0;//当前器件最大编号
        private int _addedAmount = 1;//向集合中新增信息的数量
        private short _maxDeviceAmount = 0;
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private object _detailType = GridDetailType.Device8003;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }
        public DeviceInfo8003ViewModel()
        {
            _deviceService8003 = new DeviceService8003();
        }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8003().GetMaxDeviceAmountValue();
                }
                return _maxDeviceAmount;
            }
        }
        public LoopModel TheLoop { get; set; }
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
        public List<DeviceType> ValidDeviceType
        {
            get
            {
                SCA.BusinessLib.BusinessLogic.ControllerConfig8003 config = new BusinessLib.BusinessLogic.ControllerConfig8003();
                return config.GetDeviceTypeInfo();
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
        public EditableDeviceInfo8003Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8003Collection(TheLoop, null);

                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                //_maxCode = GetMaxCode(value);
                //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8003 = GetMaxID();
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        #region 命令
        public ICommand AddNewRecordCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand<int>(AddNewRecordExecute, null);
            }
        }
        /// <summary>
        /// 添加指定数量的器件信息
        /// </summary>
        /// <param name="rowsAmount"></param>
        public void AddNewRecordExecute(int rowsAmount)
        {
            //int tempCode = _maxCode;
            //if (tempCode >= MaxDeviceAmount) //如果已经达到上限，则不添加任何行
            //{
            //    rowsAmount = 0;
            //}

            //if ((tempCode + rowsAmount) > MaxDeviceAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            //{
            //    rowsAmount = tempCode + rowsAmount - MaxDeviceAmount;
            //}
            //int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8003;
            //for (int i = 0; i < rowsAmount; i++)
            //{
            //    tempCode++;
            //    deviceID++;
            //    EditableDeviceInfo8003 deviceInfo = new EditableDeviceInfo8003();
            //    deviceInfo.Loop = TheLoop;
            //    deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
            //    deviceInfo.ID = deviceID;
            //    DeviceInfoObservableCollection.Add(deviceInfo);
            //}
            //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8003 = deviceID;
            //_maxCode = tempCode;
            _deviceService8003.TheLoop = this.TheLoop;
            List<DeviceInfo8003> lstDeviceInfo8003 = _deviceService8003.Create(rowsAmount);
            foreach (var device in lstDeviceInfo8003)
            {
                EditableDeviceInfo8003 editDevice8003 = new EditableDeviceInfo8003();
                editDevice8003.Loop = device.Loop;
                editDevice8003.LoopID = device.LoopID;
                editDevice8003.Code = device.Code;
                editDevice8003.ID = device.ID;
                editDevice8003.TypeCode = device.TypeCode;
                DeviceInfoObservableCollection.Add(editDevice8003);
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
        public List<Model.DeviceInfo8003> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8003 == null)
                {
                    _lstDeviceInfo8003 = new List<DeviceInfo8003>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8003.Add(s.ToDeviceInfo8003());
                    }
                }
                return _lstDeviceInfo8003;
            }
        }
        public void DownloadExecute()
        {
            List<DeviceInfo8003> list = TheLoop.GetDevices<DeviceInfo8003>();
            List<DeviceInfoBase> baseList = new List<DeviceInfoBase>();

            foreach (var item in list)
            {
                DeviceInfoBase baseItem = (DeviceInfoBase)item;
                baseList.Add(baseItem);
            }

            ProjectManager.GetInstance.NTConnection.SetDeviceSetup(baseList, TheLoop.DeviceAmount, TheLoop.Controller.Type);

            //InvokeControllerCom iCC = InvokeControllerCom.Instance;
            //if (iCC.GetPortStatus())
            //{
            //    if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
            //    {
            //        if (iCC.TheControllerType.ControllerType == ControllerType.FT8003) //如果控制器类型不相符，则不执行操作
            //        {

            //            List<LoopModel> lstLoopsModel = new List<LoopModel>();
            //            lstLoopsModel.Add(TheLoop);
            //            ((ControllerType8003)iCC.TheControllerType).Loops = lstLoopsModel;

            //            iCC.TheControllerType.OperableDataType = OperantDataType.Device;
            //            iCC.TheControllerType.Status = ControllerStatus.DataSending;
            //            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;                        
            //        }
            //    }
            //}
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
        public void SaveExecute()
        {
            using (new WaitCursor())
            {                
                _deviceService8003.TheLoop = this.TheLoop;
                _deviceService8003.SaveToDB();
            }
        }
        #endregion
        private void UpdateProcessBarStatus(int currentValue, int totalValue,ControllerNodeType nodeType)
        {
            object[] status = new object[3];
            status[0] = currentValue;
            status[1] = totalValue;
            status[2] = nodeType;
            EventMediator.NotifyColleagues("UpdateProgressBarStatusEvent", status);
        }
        //private int GetMaxCode(EditableDeviceInfo8003Collection deviceInfoCollection)
        //{
        //    int result = 0;
        //    if (deviceInfoCollection != null)
        //    {
        //        var query = from r in deviceInfoCollection select r.SimpleCode;
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
        //private int GetMaxID()
        //{
        //    ControllerOperation8003 controllerOperation = new ControllerOperation8003();
        //    return controllerOperation.GetMaxDeviceID();
        //}

        public void Dispose()
        {
            this.DeviceInfoObservableCollection = null;
        }
    }
}
