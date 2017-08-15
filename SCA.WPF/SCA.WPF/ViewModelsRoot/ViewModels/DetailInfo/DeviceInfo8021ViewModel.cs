using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/25 8:36:38
* FileName   : DeviceInfo8021ViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableDeviceInfo8021 : DeviceInfo8021, IEditableObject, System.ComponentModel.IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8021()
        {

        }
        public EditableDeviceInfo8021(DeviceInfo8021 deviceInfo8021)
        {
            this.Loop = deviceInfo8021.Loop;
            this.LoopID = deviceInfo8021.LoopID;            
            this.ID = deviceInfo8021.ID;
            this.Code = deviceInfo8021.Code;
       //     this.SimpleCode = deviceInfo8021.SimpleCode;
            this.TypeCode = deviceInfo8021.TypeCode;            
            this.Disable = deviceInfo8021.Disable;
            this.CurrentThreshold = deviceInfo8021.CurrentThreshold;
            this.TemperatureThreshold = deviceInfo8021.TemperatureThreshold;
            this.BuildingNo = deviceInfo8021.BuildingNo;
            this.ZoneNo = deviceInfo8021.ZoneNo;
            this.FloorNo = deviceInfo8021.FloorNo;
            this.RoomNo = deviceInfo8021.RoomNo;
            this.Location = deviceInfo8021.Location;
        }
        public DeviceInfo8021 ToDeviceInfo8021()
        {
            DeviceInfo8021 deviceInfo8021 = new DeviceInfo8021();
            deviceInfo8021.Loop = this.Loop;
            deviceInfo8021.LoopID = this.LoopID;            
            deviceInfo8021.ID = this.ID;
            deviceInfo8021.Code = this.Code;
          //  deviceInfo8021.SimpleCode = this.SimpleCode;
            deviceInfo8021.TypeCode = this.TypeCode;            
            deviceInfo8021.Disable = this.Disable;
            deviceInfo8021.CurrentThreshold = this.CurrentThreshold;
            deviceInfo8021.TemperatureThreshold = this.TemperatureThreshold;
            deviceInfo8021.BuildingNo = this.BuildingNo;
            deviceInfo8021.ZoneNo = this.ZoneNo;
            deviceInfo8021.FloorNo = this.FloorNo;
            deviceInfo8021.RoomNo = this.RoomNo;
            deviceInfo8021.Location = this.Location;            
            return deviceInfo8021;
        }
        private EditableDeviceInfo8021 backupCopy;
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as EditableDeviceInfo8021;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Loop = backupCopy.Loop;
            this.LoopID = backupCopy.LoopID;            
            this.ID = backupCopy.ID;
            this.Code = backupCopy.Code;
            //this.SimpleCode = backupCopy.SimpleCode;
            this.TypeCode = backupCopy.TypeCode;         
            this.Disable = backupCopy.Disable;
            this.CurrentThreshold = backupCopy.CurrentThreshold;
            this.TemperatureThreshold = backupCopy.TemperatureThreshold;
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
            get { return String.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                ControllerConfig8021 config = new ControllerConfig8021();
                //器件编码长度需要动态指定
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
                    case "CurrentThreshold":
                        if (this.CurrentThreshold != null)
                        { 
                            rule = dictMessage["CurrentThreshold"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.CurrentThreshold.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "TemperatureThreshold":
                        if (this.TemperatureThreshold != null)
                        { 
                            rule = dictMessage["TemperatureThreshold"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.TemperatureThreshold.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
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
    public class EditableDeviceInfo8021Collection : ObservableCollection<EditableDeviceInfo8021>
    { 
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8021> _deviceInfo8021Service;
        private LoopModel _loop;
        public EditableDeviceInfo8021Collection(LoopModel loop, List<DeviceInfo8021> lstDeviceInfo8021)
        {
            _loop=loop;
            if (lstDeviceInfo8021 != null)
            {
                foreach (var o in lstDeviceInfo8021)
               {
                 this.Add(new EditableDeviceInfo8021(o));
               }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8021 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8021 o = (EditableDeviceInfo8021)sender;
            if (o.Code != null)
            { 
                _deviceInfo8021Service = new SCA.BusinessLib.BusinessLogic.DeviceService8021();
                _deviceInfo8021Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8021Service.Update(o.ToDeviceInfo8021());
            }
        }
    }

    public class DeviceInfo8021ViewModel : PropertyChangedBase,IDisposable
    {
        private EditableDeviceInfo8021Collection _deviceInfoCollection;
        private List<DeviceInfo8021> _lstDeviceInfo8021;
        private DeviceService8021 _deviceService8021;
        private string _addIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _delIconPath = @"Resources/Icon/Style1/loop-delete.png";
        private string _copyIconPath = @"Resources/Icon/Style1/copy.png";
        private string _pasteIconPath = @"Resources/Icon/Style1/paste.png";
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private object _detailType = GridDetailType.Device8021;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        private int _maxCode = 0;//当前器件最大编号
        private int _addedAmount = 1;//向集合中新增信息的数量
        private short _maxDeviceAmount = 0;
        public DeviceInfo8021ViewModel()
        {
            _deviceService8021 = new DeviceService8021();
        }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8021().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8021 config = new BusinessLib.BusinessLogic.ControllerConfig8021();
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
        public EditableDeviceInfo8021Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8021Collection(TheLoop, null);                    
                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                //_maxCode = GetMaxCode(value);
                //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8021 = GetMaxID();
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
            //int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8021;
            //for (int i = 0; i < rowsAmount; i++)
            //{
            //    tempCode++;
            //    deviceID++;
            //    EditableDeviceInfo8021 deviceInfo = new EditableDeviceInfo8021();
            //    deviceInfo.Loop = TheLoop;
            //    deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
            //    deviceInfo.ID = deviceID;
            //    DeviceInfoObservableCollection.Add(deviceInfo);
            //}
            //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8021 = deviceID;
            //_maxCode = tempCode; 
            _deviceService8021.TheLoop = this.TheLoop;
            List<DeviceInfo8021> lstDeviceInfo8021 = _deviceService8021.Create(rowsAmount);
            foreach (var device in lstDeviceInfo8021)
            {
                EditableDeviceInfo8021 editDevice8021 = new EditableDeviceInfo8021();
                editDevice8021.Loop = device.Loop;
                editDevice8021.LoopID = device.LoopID;
                editDevice8021.Code = device.Code;
                editDevice8021.ID = device.ID;
                editDevice8021.TypeCode = device.TypeCode;
                DeviceInfoObservableCollection.Add(editDevice8021);
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

        public List<Model.DeviceInfo8021> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8021 == null)
                {
                    _lstDeviceInfo8021 = new List<DeviceInfo8021>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8021.Add(s.ToDeviceInfo8021());
                    }
                }
                return _lstDeviceInfo8021;
            }
        }
        public void DownloadExecute()
        {
            List<DeviceInfo8021> list = TheLoop.GetDevices<DeviceInfo8021>();
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
            //        if (iCC.TheControllerType.ControllerType == ControllerType.NT8021) //如果控制器类型不相符，则不执行操作
            //        {
            //            //iCC.TheControllerType.Status = ControllerStatus.DataSending;
            //            List<LoopModel> lstLoopsModel = new List<LoopModel>();
            //            lstLoopsModel.Add(TheLoop);
            //            ((ControllerType8021)iCC.TheControllerType).Loops = lstLoopsModel;                        
            //            iCC.TheControllerType.OperableDataType = OperantDataType.Device;
            //            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
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
                _deviceService8021.TheLoop = this.TheLoop;
                _deviceService8021.SaveToDB();
            }
        }
        #endregion
        private void UpdateProcessBarStatus(int currentValue, int totalValue, ControllerNodeType nodeType)
        {
            object[] status = new object[3];
            status[0] = currentValue;
            status[1] = totalValue;
            status[2] = nodeType;
            EventMediator.NotifyColleagues("UpdateProgressBarStatusEvent", status);
        }
        //private int GetMaxCode(EditableDeviceInfo8021Collection deviceInfoCollection)
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
        //    ControllerOperation8021 controllerOperation = new ControllerOperation8021();
        //    return controllerOperation.GetMaxDeviceID();
        //}

        public void Dispose()
        {
            this.DeviceInfoObservableCollection = null;
        }
    }
}
