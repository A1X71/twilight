using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Specialized;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 14:21:25
* FileName   : DeviceInfo8000ViewModel
* Description: 8000控制器ViewModel
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class EditableDeviceInfo8000 : DeviceInfo8000, IEditableObject,IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8000()
        {

        }
        public EditableDeviceInfo8000(DeviceInfo8000 deviceInfo8000)
        {
            this.Loop = deviceInfo8000.Loop;
            this.LoopID = deviceInfo8000.LoopID;
            this.ID = deviceInfo8000.ID;
            this.Code = deviceInfo8000.Code;
            this.TypeCode = deviceInfo8000.TypeCode;
            this.SensitiveLevel = deviceInfo8000.SensitiveLevel;
            this.Feature = deviceInfo8000.Feature;
            this.Disable = deviceInfo8000.Disable;
            this.LinkageGroup1 = deviceInfo8000.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8000.LinkageGroup2;
            this.LinkageGroup3 = deviceInfo8000.LinkageGroup3;
            this.DelayValue = deviceInfo8000.DelayValue;
            this.sdpKey = deviceInfo8000.sdpKey;            
            this.ZoneNo = deviceInfo8000.ZoneNo;
            this.BroadcastZone = deviceInfo8000.BroadcastZone;            
            this.Location = deviceInfo8000.Location;
        }
        public DeviceInfo8000 ToDeviceInfo8000()
        {
            DeviceInfo8000 deviceInfo8000 = new DeviceInfo8000();
             deviceInfo8000.Loop=this.Loop;
             deviceInfo8000.LoopID = this.LoopID;
             deviceInfo8000.ID = this.ID;
             deviceInfo8000.Code = this.Code;
             deviceInfo8000.TypeCode = this.TypeCode;
             deviceInfo8000.SensitiveLevel = this.SensitiveLevel;
             deviceInfo8000.Feature = this.Feature;
             deviceInfo8000.Disable = this.Disable;
             deviceInfo8000.LinkageGroup1 = this.LinkageGroup1;
             deviceInfo8000.LinkageGroup2 = this.LinkageGroup2;
             deviceInfo8000.LinkageGroup3 = this.LinkageGroup3;
             deviceInfo8000.DelayValue = this.DelayValue;
             deviceInfo8000.sdpKey = this.sdpKey;
             deviceInfo8000.ZoneNo = this.ZoneNo;
             deviceInfo8000.BroadcastZone = this.BroadcastZone;
             deviceInfo8000.Location = this.Location;
             return deviceInfo8000;
        }
        private EditableDeviceInfo8000 backupCopy;
        private bool inEdit;
        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = this.MemberwiseClone() as EditableDeviceInfo8000;
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
            this.Disable = backupCopy.Disable;
            this.Feature = backupCopy.Feature;
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
                    ControllerConfig8000 config = new ControllerConfig8000();
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
                            if (this.DelayValue.ToString() != "")
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
                            rule = dictMessage["SDPKey"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.sdpKey.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                            break;
                        case "ZoneNo":
                            rule = dictMessage["ZoneNo"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ZoneNo.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
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
    public class EditableDeviceInfo8000Collection : ObservableCollection<EditableDeviceInfo8000>
    { 
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8000> _deviceInfo8000Service;
        private LoopModel _loop;
        public EditableDeviceInfo8000Collection(LoopModel loop, List<DeviceInfo8000> lstDeviceInfo8000)
        {
            _loop = loop;
            if (lstDeviceInfo8000 != null)
            {
                foreach (var o in lstDeviceInfo8000)
                {
                    this.Add(new EditableDeviceInfo8000(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8000 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8000 o = (EditableDeviceInfo8000)sender;
            if (o.Code != null)
            {
                _deviceInfo8000Service = new SCA.BusinessLib.BusinessLogic.DeviceService8000();
                _deviceInfo8000Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8000Service.Update(o.ToDeviceInfo8000());
            }
        }
    }
    public class DeviceInfo8000ViewModel : PropertyChangedBase//, IDeviceInfoViewModel<DeviceInfo8001>
    {
 
        private EditableDeviceInfo8000Collection _deviceInfoCollection;
        private List<DeviceInfo8000> _lstDeviceInfo8000;

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


        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8000().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8000 config = new BusinessLib.BusinessLogic.ControllerConfig8000();
                return config.GetDeviceTypeInfo();
            }
        }

        public EditableDeviceInfo8000Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8000Collection(TheLoop, null);

                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                _maxCode = GetMaxCode(value);
                BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8000 = GetMaxID();
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
            int tempCode = _maxCode;
            if (tempCode >= MaxDeviceAmount) //如果已经达到上限，则不添加任何行
            {
                rowsAmount = 0;
            }

            if ((tempCode + rowsAmount) > MaxDeviceAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                rowsAmount = tempCode + rowsAmount - MaxDeviceAmount;
            }
            int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8000;
            for (int i = 0; i < rowsAmount; i++)
            {
                tempCode++;
                deviceID++;
                EditableDeviceInfo8000 deviceInfo = new EditableDeviceInfo8000();
                deviceInfo.Loop = TheLoop;
                deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
                deviceInfo.ID = deviceID;
                DeviceInfoObservableCollection.Add(deviceInfo);
            }
            BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8000 = deviceID;
            _maxCode = tempCode;


        }
        public ICommand DownloadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DownloadExecute, null);
            }
        }
        public List<Model.DeviceInfo8000> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8000 == null)
                {
                    _lstDeviceInfo8000 = new List<DeviceInfo8000>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8000.Add(s.ToDeviceInfo8000());
                    }
                }
                return _lstDeviceInfo8000;
            }
        }
        public void DownloadExecute()
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
                {
                    if (iCC.TheControllerType.ControllerType == ControllerType.FT8000) //如果控制器类型不相符，则不执行操作
                    {
                        List<LoopModel> lstLoopsModel = new List<LoopModel>();
                        lstLoopsModel.Add(TheLoop);
                        ((ControllerType8000)iCC.TheControllerType).Loops = lstLoopsModel;
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
        private int GetMaxCode(EditableDeviceInfo8000Collection deviceInfoCollection)
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
            ControllerOperation8000 controllerOperation = new ControllerOperation8000();
            return controllerOperation.GetMaxDeviceID();
        }

    }
}
