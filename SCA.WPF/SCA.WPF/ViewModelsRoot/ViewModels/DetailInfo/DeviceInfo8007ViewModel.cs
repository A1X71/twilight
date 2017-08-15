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
using SCA.BusinessLib.BusinessLogic;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/7 8:55:34
* FileName   : DeviceInfo8007ViewModel
* Description: 8007控制器ViewModel
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableDeviceInfo8007 : DeviceInfo8007, IEditableObject, IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8007()
        {

        }
        public EditableDeviceInfo8007(DeviceInfo8007 deviceInfo8007)
        {
            this.Loop = deviceInfo8007.Loop;
            this.LoopID = deviceInfo8007.LoopID;
            this.ID = deviceInfo8007.ID;
            this.Code = deviceInfo8007.Code;            
            this.TypeCode = deviceInfo8007.TypeCode;
            this.SensitiveLevel = deviceInfo8007.SensitiveLevel;
            this.Feature = deviceInfo8007.Feature;
            this.Disable = deviceInfo8007.Disable;
            this.LinkageGroup1 = deviceInfo8007.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8007.LinkageGroup2;            
            this.BuildingNo = deviceInfo8007.BuildingNo;
            this.ZoneNo = deviceInfo8007.ZoneNo;
            this.FloorNo = deviceInfo8007.FloorNo;
            this.RoomNo = deviceInfo8007.RoomNo;
            this.Location = deviceInfo8007.Location;
        }
        public DeviceInfo8007 ToDeviceInfo8007()
        {
            DeviceInfo8007 deviceInfo8007 = new DeviceInfo8007();
            deviceInfo8007.Loop = this.Loop;
            deviceInfo8007.LoopID = this.LoopID;
            deviceInfo8007.ID = this.ID;
            deviceInfo8007.Code = this.Code;            
            deviceInfo8007.TypeCode = this.TypeCode;
            deviceInfo8007.Disable = this.Disable;
            deviceInfo8007.LinkageGroup1 = this.LinkageGroup1;
            deviceInfo8007.LinkageGroup2 = this.LinkageGroup2;
            deviceInfo8007.Feature = this.Feature;
            deviceInfo8007.SensitiveLevel = this.SensitiveLevel;            
            deviceInfo8007.BuildingNo = this.BuildingNo;
            deviceInfo8007.ZoneNo = this.ZoneNo;
            deviceInfo8007.FloorNo = this.FloorNo;
            deviceInfo8007.RoomNo = this.RoomNo;
            deviceInfo8007.Location = this.Location;
            return deviceInfo8007;
        }
        private EditableDeviceInfo8007 backupCopy;
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as EditableDeviceInfo8007;
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
            this.SensitiveLevel = backupCopy.SensitiveLevel;
            this.Feature = backupCopy.Feature;
            this.Disable = backupCopy.Disable;
            this.LinkageGroup1 = backupCopy.LinkageGroup1;
            this.LinkageGroup2 = backupCopy.LinkageGroup2;
            this.BuildingNo = backupCopy.BuildingNo;
            this.ZoneNo = backupCopy.ZoneNo;
            this.FloorNo = backupCopy.FloorNo;
            this.RoomNo = backupCopy.RoomNo;
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
            get { return String.Empty;            }
        }

        public string this[string columnName]
        {

            get {                
                    ControllerConfig8007 config = new ControllerConfig8007();
                    Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(this.Loop.Controller.DeviceAddressLength);
                    SCA.Model.RuleAndErrorMessage rule;
                    System.Text.RegularExpressions.Regex exminator;
                    string errorMessage = String.Empty;
                    switch (columnName)
                    {

                        //case"Disable":
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
                        case "BuildingNo":
                            if (this.BuildingNo != null)
                            { 
                                if (this.BuildingNo.ToString() != "")
                                {
                                    rule = dictMessage["BuildingNo"];
                                    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                    if (!exminator.IsMatch(this.BuildingNo.ToString()))
                                    {
                                        errorMessage = rule.ErrorMessage;
                                    }
                                }
                            }
                            break;
                        case "ZoneNo":
                            if (this.ZoneNo != null)
                            { 
                                if (this.ZoneNo.ToString() != "")
                                {
                                    rule = dictMessage["ZoneNo"];
                                    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                    if (!exminator.IsMatch(this.ZoneNo.ToString()))
                                    {
                                        errorMessage = rule.ErrorMessage;
                                    }
                                }
                            }
                            break;
                        case "FloorNo":
                            if (this.FloorNo != null)
                            { 
                                if (this.FloorNo.ToString() != "")
                                {
                                    rule = dictMessage["FloorNo"];
                                    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                    if (!exminator.IsMatch(this.FloorNo.ToString()))
                                    {
                                        errorMessage = rule.ErrorMessage;
                                    }
                                }
                            }
                            break;
                        case "RoomNo":
                            if (this.RoomNo != null)
                            { 
                                if (this.RoomNo.ToString() != "")
                                {
                                    rule = dictMessage["RoomNo"];
                                    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                    if (!exminator.IsMatch(this.RoomNo.ToString()))
                                    {
                                        errorMessage = rule.ErrorMessage;
                                    }
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
    public class EditableDeviceInfo8007Collection : ObservableCollection<EditableDeviceInfo8007>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8007> _deviceInfo8007Service;
        private LoopModel _loop;
        public EditableDeviceInfo8007Collection(LoopModel loop, List<DeviceInfo8007> lstDeviceInfo8007)
        {
            _loop = loop;
            if (lstDeviceInfo8007 != null)
            {
                foreach (var o in lstDeviceInfo8007)
                {
                    this.Add(new EditableDeviceInfo8007(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8007 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8007 o = (EditableDeviceInfo8007)sender;
            if (o.Code != null)
            {
                _deviceInfo8007Service = new SCA.BusinessLib.BusinessLogic.DeviceService8007();
                _deviceInfo8007Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8007Service.Update(o.ToDeviceInfo8007());
            }
        }
    }
    public class DeviceInfo8007ViewModel:PropertyChangedBase,IDisposable
    {
        private EditableDeviceInfo8007Collection _deviceInfoCollection;
        private List<DeviceInfo8007> _lstDeviceInfo8007;
        private DeviceService8007 _deviceService8007;
        private int _maxCode = 0;//当前器件最大编号
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
        private object _detailType = GridDetailType.Device8007;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }
        public DeviceInfo8007ViewModel()
        {
            _deviceService8007 = new DeviceService8007();
        }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8007().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8007 config = new BusinessLib.BusinessLogic.ControllerConfig8007();
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
        public EditableDeviceInfo8007Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8007Collection(TheLoop, null);
                    
                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                //_maxCode = GetMaxCode(value);
                //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007 = GetMaxID();
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
            //int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007;
            //for (int i = 0; i < rowsAmount; i++)
            //{
            //    tempCode++;
            //    deviceID++;
            //    EditableDeviceInfo8007 deviceInfo = new EditableDeviceInfo8007();
            //    deviceInfo.Loop = TheLoop;
            //    deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
            //    deviceInfo.ID = deviceID;
            //    DeviceInfoObservableCollection.Add(deviceInfo);
            //}
            //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8007 = deviceID;
            //_maxCode = tempCode;    
            _deviceService8007.TheLoop = this.TheLoop;
            List<DeviceInfo8007> lstDeviceInfo8007 = _deviceService8007.Create(rowsAmount);
            foreach (var device in lstDeviceInfo8007)
            {
                EditableDeviceInfo8007 editDevice8007 = new EditableDeviceInfo8007();
                editDevice8007.Loop = device.Loop;
                editDevice8007.LoopID = device.LoopID;
                editDevice8007.Code = device.Code;
                editDevice8007.ID = device.ID;
                editDevice8007.TypeCode = device.TypeCode;
                DeviceInfoObservableCollection.Add(editDevice8007);
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
        public List<Model.DeviceInfo8007> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8007 == null)
                {
                    _lstDeviceInfo8007 = new List<DeviceInfo8007>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8007.Add(s.ToDeviceInfo8007());
                    }
                }
                return _lstDeviceInfo8007;
            }
        }
        public void DownloadExecute()
        {
            List<DeviceInfo8007> list = TheLoop.GetDevices<DeviceInfo8007>();
            List<DeviceInfoBase> baseList = new List<DeviceInfoBase>();

            foreach (var item in list)
            {
                DeviceInfoBase baseItem = (DeviceInfoBase)item;
                baseList.Add(baseItem);
            }

            SCA.BusinessLib.ProjectManager.GetInstance.NTConnection.SetDeviceSetup(baseList, TheLoop.DeviceAmount, TheLoop.Controller.Type);


            //InvokeControllerCom iCC = InvokeControllerCom.Instance;
            //if (iCC.GetPortStatus())
            //{
            //    if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
            //    {
            //        if (iCC.TheControllerType.ControllerType == ControllerType.NT8007) //如果控制器类型不相符，则不执行操作
            //        {
            //          //  LoopModel loop = new LoopModel();
            //          //  loop.CopyLoop<DeviceInfo8007>(TheLoop);                        
            //            List<LoopModel> lstLoopsModel = new List<LoopModel>();
            //            lstLoopsModel.Add(TheLoop);
            //            ((ControllerType8007)iCC.TheControllerType).Loops = lstLoopsModel;
            //            //((ControllerType8007)iCC.TheControllerType).DeviceInfoList = DeviceInfo;
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
                _deviceService8007.TheLoop = this.TheLoop;
                _deviceService8007.SaveToDB();
            }
        }
        #endregion
        private void UpdateProcessBarStatus(int currentValue,int totalValue,ControllerNodeType nodeType)
        {
            object [] status = new object[3];
            status[0] = currentValue;
            status[1] = totalValue;
            status[2] = nodeType;
            EventMediator.NotifyColleagues("UpdateProgressBarStatusEvent", status);
        }
        private int GetMaxCode(EditableDeviceInfo8007Collection deviceInfoCollection)
        {
            int result = 0;
            if (deviceInfoCollection != null)
            {
                var query = from r in deviceInfoCollection select r.SimpleCode;
                if (query != null)
                {
                    foreach (var i in query)
                    {
                        if (Convert.ToInt32(i) > result)
                        {
                            result = Convert.ToInt32(i);
                        }
                    }
                }
            }
            return result;
        }
        private int GetMaxID()
        {
            ControllerOperation8007 controllerOperation = new ControllerOperation8007();
            return controllerOperation.GetMaxDeviceID();
        }

        public void Dispose()
        {
            this.DeviceInfoObservableCollection = null;
        }
    }
}
