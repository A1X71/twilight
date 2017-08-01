using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:27:54
* FileName   : DeviceInfo8001ViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableDeviceInfo8001 : DeviceInfo8001, IEditableObject, System.ComponentModel.IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8001()
        {

        }
        public EditableDeviceInfo8001(DeviceInfo8001 deviceInfo8001)
        {
            this.Loop = deviceInfo8001.Loop;
            this.LoopID = deviceInfo8001.LoopID;
            //this.MachineNo = deviceInfo8001.MachineNo;//临时应用
            this.ID = deviceInfo8001.ID;
            this.Code = deviceInfo8001.Code;
            //this.SimpleCode = deviceInfo8001.SimpleCode;
            this.TypeCode = deviceInfo8001.TypeCode;

            this.LinkageGroup1 = deviceInfo8001.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8001.LinkageGroup2;
            this.LinkageGroup3 = deviceInfo8001.LinkageGroup3;
            
            this.BoardNo = deviceInfo8001.BoardNo;
            this.SubBoardNo = deviceInfo8001.SubBoardNo;
            this.KeyNo = deviceInfo8001.KeyNo;
            this.BroadcastZone = deviceInfo8001.BroadcastZone;

            this.DelayValue = deviceInfo8001.DelayValue;
            this.SensitiveLevel = deviceInfo8001.SensitiveLevel;
            this.Feature = deviceInfo8001.Feature;
            this.Disable = deviceInfo8001.Disable;
            

            this.BuildingNo = deviceInfo8001.BuildingNo;
            this.ZoneNo = deviceInfo8001.ZoneNo;
            this.FloorNo = deviceInfo8001.FloorNo;
            this.RoomNo = deviceInfo8001.RoomNo;
            this.Location = deviceInfo8001.Location;
            this.sdpKey = deviceInfo8001.sdpKey;
            this.MCBCode = deviceInfo8001.MCBCode;

        }
        public DeviceInfo8001 ToDeviceInfo8001()
        {
            DeviceInfo8001 deviceInfo8001 = new DeviceInfo8001();
            deviceInfo8001.Loop = this.Loop;
            deviceInfo8001.LoopID=this.LoopID ;
            //this.MachineNo = deviceInfo8001.MachineNo;//临时应用
            deviceInfo8001.ID = this.ID;
            deviceInfo8001.Code = this.Code;
            //deviceInfo8001.SimpleCode = this.SimpleCode;
            deviceInfo8001.TypeCode = this.TypeCode;
            deviceInfo8001.LinkageGroup1 = this.LinkageGroup1;
            deviceInfo8001.LinkageGroup2 = this.LinkageGroup2;
            deviceInfo8001.LinkageGroup3 = this.LinkageGroup3;
            deviceInfo8001.BoardNo = this.BoardNo;
            deviceInfo8001.SubBoardNo = this.SubBoardNo;
            deviceInfo8001.KeyNo = this.KeyNo;
            deviceInfo8001.BroadcastZone = this.BroadcastZone;
            deviceInfo8001.DelayValue = this.DelayValue;
            deviceInfo8001.SensitiveLevel = this.SensitiveLevel;
            deviceInfo8001.Feature = this.Feature;
            deviceInfo8001.Disable = this.Disable;
            deviceInfo8001.BuildingNo = this.BuildingNo;
            deviceInfo8001.ZoneNo = this.ZoneNo;
            deviceInfo8001.FloorNo = this.FloorNo;
            deviceInfo8001.RoomNo = this.RoomNo;
            deviceInfo8001.Location = this.Location;
            deviceInfo8001.sdpKey = this.sdpKey;
            deviceInfo8001.MCBCode = this.MCBCode;
            return deviceInfo8001;
        }
        public new string Code
        {
            get;
            set;
        }
        private EditableDeviceInfo8001 backupCopy;
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as EditableDeviceInfo8001;
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            this.Loop = backupCopy.Loop;
            this.LoopID = backupCopy.LoopID;
            //this.MachineNo = backupCopy.MachineNo;//临时应用
            this.ID = backupCopy.ID;
            this.Code = backupCopy.Code;
            //this.SimpleCode = backupCopy.SimpleCode;
            this.TypeCode = backupCopy.TypeCode;

            this.LinkageGroup1 = backupCopy.LinkageGroup1;
            this.LinkageGroup2 = backupCopy.LinkageGroup2;
            this.LinkageGroup3 = backupCopy.LinkageGroup3;

            this.BoardNo = backupCopy.BoardNo;
            this.SubBoardNo = backupCopy.SubBoardNo;
            this.KeyNo = backupCopy.KeyNo;
            this.BroadcastZone = backupCopy.BroadcastZone;

            this.DelayValue = backupCopy.DelayValue;
            this.SensitiveLevel = backupCopy.SensitiveLevel;
            this.Feature = backupCopy.Feature;
            this.Disable = backupCopy.Disable;


            this.BuildingNo = backupCopy.BuildingNo;
            this.ZoneNo = backupCopy.ZoneNo;
            this.FloorNo = backupCopy.FloorNo;
            this.RoomNo = backupCopy.RoomNo;
            this.Location = backupCopy.Location;
            this.sdpKey = backupCopy.sdpKey;
            this.MCBCode = backupCopy.MCBCode;
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
                ControllerConfig8001 config = new ControllerConfig8001();
                //器件编码长度需要动态指定
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(this.Loop.Controller.DeviceAddressLength);
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
                switch (columnName)
                {
                    case "Feature":
                        rule = dictMessage["Feature"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.Feature.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    //case "Disable":
                    //    rule = dictMessage["Disable"];
                    //    exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                    //    if (!exminator.IsMatch(this.Disable.ToString()))
                    //    {
                    //        errorMessage = rule.ErrorMessage;
                    //    }
                    //    break;
                    case "SensitiveLevel":
                        rule = dictMessage["SensitiveLevel"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.SensitiveLevel.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "LinkageGroup1":
                        if (this.LinkageGroup1.ToString() != "")
                        {
                            rule = dictMessage["StandardLinkageGroup"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LinkageGroup1.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LinkageGroup2":
                        if (this.LinkageGroup2.ToString() != "")
                        {
                            rule = dictMessage["StandardLinkageGroup"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LinkageGroup2.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "LinkageGroup3":
                        if (this.LinkageGroup3.ToString() != "")
                        {
                            rule = dictMessage["StandardLinkageGroup"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LinkageGroup3.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "DelayValue":
                        rule = dictMessage["DelayValue"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.DelayValue.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "BroadcastZone":
                        rule = dictMessage["DelayValue"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.BroadcastZone.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                    case "BuildingNo":
                        if (this.BuildingNo.ToString() != "")
                        {
                            rule = dictMessage["BuildingNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.BuildingNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ZoneNo":
                        if (this.ZoneNo.ToString() != "")
                        {
                            rule = dictMessage["ZoneNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "FloorNo":
                        if (this.FloorNo.ToString() != "")
                        {
                            rule = dictMessage["FloorNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.FloorNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "RoomNo":
                        if (this.RoomNo.ToString() != "")
                        {
                            rule = dictMessage["RoomNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.RoomNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "Location":
                        rule = dictMessage["Location"];
                        exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                        if (!exminator.IsMatch(this.Location.ToString()))
                        {
                            errorMessage = rule.ErrorMessage;
                        }
                        break;
                }
                return errorMessage;
            }
        }
    }
    public class EditableDeviceInfo8001Collection : ObservableCollection<EditableDeviceInfo8001>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8001> _deviceInfo8001Service;
        private LoopModel _loop;
        public EditableDeviceInfo8001Collection(LoopModel loop, List<DeviceInfo8001> lstDeviceInfo8001)
        {
            _loop = loop;
            if (lstDeviceInfo8001 != null)
            {
                //if (lstDeviceInfo8001.Count != 0)
                //{
                    foreach (var o in lstDeviceInfo8001)
                    {
                        this.Add(new EditableDeviceInfo8001(o));
                    }
                //}
                //else
                //{
                //    DeviceInfo8001 o = new DeviceInfo8001();
                //    this.Add(new EditableDeviceInfo8001(o));
                //}
            }
            //else
            //{
            //    DeviceInfo8001 o = new DeviceInfo8001();
            //    this.Add(new EditableDeviceInfo8001(o));
            //}
        }
        protected override void InsertItem(int index, EditableDeviceInfo8001 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8001 o = (EditableDeviceInfo8001)sender;
            if (o.Code != null)
            {
                _deviceInfo8001Service = new SCA.BusinessLib.BusinessLogic.DeviceService8001();
                _deviceInfo8001Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8001Service.Update(o.ToDeviceInfo8001());
            }
        }
    }
    public class DeviceInfo8001ViewModel : PropertyChangedBase//, IDeviceInfoViewModel<DeviceInfo8001>
    {
        //private ObservableCollection<DeviceInfo8001> _deviceInfoObservableCollection;

        //public ObservableCollection<DeviceInfo8001> DeviceInfoObservableCollection
        //{
        //    get
        //    {
        //        if (_deviceInfoObservableCollection == null)
        //        {
        //            _deviceInfoObservableCollection = new ObservableCollection<DeviceInfo8001>();
        //        }
        //        return _deviceInfoObservableCollection;
        //    }
        //    set
        //    {
        //        _deviceInfoObservableCollection = value;
        //        NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
        //    }
        //}
        private EditableDeviceInfo8001Collection _deviceInfoCollection;
        private List<DeviceInfo8001> _lstDeviceInfo8001;
        private DeviceService8001 _deviceService8001;
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
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }
        public DeviceInfo8001ViewModel()
        {
            _deviceService8001 = new DeviceService8001();

        }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8001().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8001 config = new BusinessLib.BusinessLogic.ControllerConfig8001();
                return config.GetDeviceTypeInfo();
            }
        }

        public EditableDeviceInfo8001Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8001Collection(TheLoop, null);

                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
              //  _maxCode = GetMaxCode(value);
               // BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001=GetMaxID();
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
            _deviceService8001.TheLoop = TheLoop;
            List<DeviceInfo8001> lstDeviceInfo8001 = _deviceService8001.Create(rowsAmount);
            //int tempCode = _maxCode;
            //if (tempCode >= MaxDeviceAmount) //如果已经达到上限，则不添加任何行
            //{
            //    rowsAmount = 0;
            //}

            //if ((tempCode + rowsAmount) > MaxDeviceAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            //{
            //    rowsAmount = tempCode + rowsAmount - MaxDeviceAmount;
            //}
            //int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001;
            //for (int i = 0; i < rowsAmount; i++)
            //{
            //    tempCode++;
            //    deviceID++;
            //    EditableDeviceInfo8001 deviceInfo = new EditableDeviceInfo8001();
            //    deviceInfo.Loop = TheLoop;                
            //    deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
            //    deviceInfo.ID = deviceID;
            //    DeviceInfoObservableCollection.Add(deviceInfo);
            //}
            //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8001 = deviceID;
            //_maxCode = tempCode;
            foreach (var device in lstDeviceInfo8001)
            {
                EditableDeviceInfo8001 editDevice8001 = new EditableDeviceInfo8001();
                editDevice8001.Loop = device.Loop;
                editDevice8001.Code = device.Code;
                editDevice8001.ID = device.ID;
                DeviceInfoObservableCollection.Add(editDevice8001);
            }
        }
        public ICommand DownloadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DownloadExecute, null);
            }
        }
        public List<Model.DeviceInfo8001> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8001 == null)
                {
                    _lstDeviceInfo8001 = new List<DeviceInfo8001>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8001.Add(s.ToDeviceInfo8001());
                    }
                }
                return _lstDeviceInfo8001;
            }
        }
        public void DownloadExecute()
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
                {
                    if (iCC.TheControllerType.ControllerType == ControllerType.NT8001) //如果控制器类型不相符，则不执行操作
                    {
                                               
                        List<LoopModel> lstLoopsModel = new List<LoopModel>();
                        lstLoopsModel.Add(TheLoop);
                        ((ControllerType8001)iCC.TheControllerType).Loops = lstLoopsModel;
                        
                        iCC.TheControllerType.OperableDataType = OperantDataType.Device;
                        iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                    }
                }
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
        private int GetMaxCode(EditableDeviceInfo8001Collection deviceInfoCollection)
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
            ControllerOperation8001 controllerOperation = new ControllerOperation8001();
            return controllerOperation.GetMaxDeviceID();            
        }

    }
}
