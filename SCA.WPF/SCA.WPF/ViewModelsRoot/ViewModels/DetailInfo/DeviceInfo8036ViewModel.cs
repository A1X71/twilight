using Caliburn.Micro;
using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.Interface.UIWPF;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:21:03
* FileName   : DeviceInfo8036ViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{

    public class EditableDeviceInfo8036 : DeviceInfo8036,IEditableObject,IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8036()
        {

        }
        public EditableDeviceInfo8036(DeviceInfo8036 deviceInfo8036)
        {
            this.Loop = deviceInfo8036.Loop;
            this.LoopID = deviceInfo8036.LoopID;
            this.ID = deviceInfo8036.ID;
            this.Code = deviceInfo8036.Code;
         //   this.SimpleCode = deviceInfo8036.SimpleCode;
            this.TypeCode = deviceInfo8036.TypeCode;
            this.Disable = deviceInfo8036.Disable;
            this.LinkageGroup1 = deviceInfo8036.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8036.LinkageGroup2;
            this.AlertValue = deviceInfo8036.AlertValue;
            this.ForcastValue = deviceInfo8036.ForcastValue;
            this.DelayValue = deviceInfo8036.DelayValue;
            this.BuildingNo = deviceInfo8036.BuildingNo;
            this.ZoneNo = deviceInfo8036.ZoneNo;
            this.FloorNo = deviceInfo8036.FloorNo;
            this.RoomNo = deviceInfo8036.RoomNo;
            this.Location = deviceInfo8036.Location;
        }
        public DeviceInfo8036 ToDeviceInfo8036()
        {
            DeviceInfo8036 deviceInfo8036 = new DeviceInfo8036();
            deviceInfo8036.Loop = this.Loop;
            deviceInfo8036.LoopID = this.LoopID;
            deviceInfo8036.ID = this.ID;
            deviceInfo8036.Code = this.Code;
            //deviceInfo8036.SimpleCode = this.SimpleCode;
            deviceInfo8036.TypeCode = this.TypeCode;
            deviceInfo8036.Disable = this.Disable;
            deviceInfo8036.LinkageGroup1 = this.LinkageGroup1;
            deviceInfo8036.LinkageGroup2 = this.LinkageGroup2;
            deviceInfo8036.AlertValue = this.AlertValue;
            deviceInfo8036.ForcastValue = this.ForcastValue;
            deviceInfo8036.DelayValue = this.DelayValue;
            deviceInfo8036.BuildingNo = this.BuildingNo;
            deviceInfo8036.ZoneNo = this.ZoneNo;
            deviceInfo8036.FloorNo = this.FloorNo;
            deviceInfo8036.RoomNo = this.RoomNo;
            deviceInfo8036.Location = this.Location;
            return deviceInfo8036;
        }
        public void BeginEdit()
        {
            
        }

        public void CancelEdit()
        {
            
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
                ControllerConfig8036 config = new ControllerConfig8036();
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(this.Loop.Controller.DeviceAddressLength);
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
                    case "AlertValue":
                        if (this.AlertValue != null)
                        { 
                            if (this.AlertValue.ToString() != "")
                            {
                                rule = dictMessage["AlertValue"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.AlertValue.ToString()))
                                {
                                    errorMessage = rule.ErrorMessage;
                                }
                            }
                        }
                        break;
                    case "ForcastValue":
                        if (this.ForcastValue != null)
                        { 
                            if (this.ForcastValue.ToString() != "")
                            {
                                rule = dictMessage["ForcastValue"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.ForcastValue.ToString()))
                                {
                                    errorMessage = rule.ErrorMessage;
                                }
                            }
                        }
                        break;
                    case "DelayValue":
                        if (this.DelayValue != null)
                        { 
                            if (this.DelayValue.ToString() != "")
                            {
                                rule = dictMessage["DelayValue"];
                                exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                                if (!exminator.IsMatch(this.DelayValue.ToString()))
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
    public class EditableDeviceInfo8036Collection : ObservableCollection<EditableDeviceInfo8036>
    { 
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8036> _deviceInfo8036Service;
        private LoopModel _loop;
        public EditableDeviceInfo8036Collection(LoopModel loop, List<DeviceInfo8036> lstDeviceInfo8036)
        {
            _loop=loop;
            if (lstDeviceInfo8036 != null)
            {
                foreach (var o in lstDeviceInfo8036)
               {
                 this.Add(new EditableDeviceInfo8036(o));
               }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8036 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8036 o = (EditableDeviceInfo8036)sender;
            if (o.Code != null)
            { 
                _deviceInfo8036Service = new SCA.BusinessLib.BusinessLogic.DeviceService8036();
                _deviceInfo8036Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8036Service.Update(o.ToDeviceInfo8036());
            }
        }
    }
    #region 计划抽像为共用的，未成型,思路断了，先放弃
    //public abstract class EditableDeviceInfoBase
    //{
    //    public abstract  EditableDeviceInfoBase(SCA.Model.IDevice deviceInfo);
    //}
    //public interface IFactory<T>:IEditableObject
    //{ 
    //    T New(IDevice device);
    //}

    //public class EditableDeviceInfoCollection<T1,T2> : ObservableCollection<T1> where T1:EditableDeviceInfo8036
    //    //where T1 : IFactory<T1>, new()
    //    //where T2 : IDevice
    //{
    //    public event ItemEndEditEventHandler ItemEndEdit;

    //    private SCA.Interface.IDeviceService<T2> _deviceInfo8036Service;
    //    private LoopModel _loop;
    //    public EditableDeviceInfoCollection(LoopModel loop, List<T2> lstDeviceInfo8036)
    //    {
    //        _loop = loop;
    //        if (lstDeviceInfo8036 != null)
    //        {
    //            foreach (var o in lstDeviceInfo8036)
    //            {
    //                //var factory = new T1();
    //                //this.Add(f);
    //            }
    //        }
    //    }
    //    protected override void InsertItem(int index, T1 item)
    //    {
    //        base.InsertItem(index, item);
    //        item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
    //    }
    //    private void ItemEndEditHandler(IEditableObject sender)
    //    {
    //        EditableDeviceInfo8036 o = (EditableDeviceInfo8036)sender;
    //        _deviceInfo8036Service = new SCA.BusinessLib.BusinessLogic.DeviceService8036();
    //        _deviceInfo8036Service.TheLoop = _loop;
    //        o.Loop = _loop;
    //        _deviceInfo8036Service.Update(o.ToDeviceInfo8036());
    //    }
    //}

    #endregion

    //去掉IDeviceInfoViewModel<DeviceInfo8036>,涉及各个控制器的器件类，暂放弃进一步抽象 2017-03-30
    public class DeviceInfo8036ViewModel:PropertyChangedBase,IDisposable//,IDeviceInfoViewModel<DeviceInfo8036>
    {
        private EditableDeviceInfo8036Collection _deviceInfoCollection;
        private List<DeviceInfo8036> _lstDeviceInfo8036;
        private DeviceService8036 _deviceService8036;
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
        private object _detailType = GridDetailType.Device8036;
        private Visibility _addMoreLinesUserControlVisibility = Visibility.Collapsed;
        public string AddIconPath { get { return _appCurrentPath + _addIconPath; } }
        public string DelIconPath { get { return _appCurrentPath + _delIconPath; } }
        public string CopyIconPath { get { return _appCurrentPath + _copyIconPath; } }
        public string PasteIconPath { get { return _appCurrentPath + _pasteIconPath; } }
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        public DeviceInfo8036ViewModel()
        {
            _deviceService8036 = new DeviceService8036();
        }
        public short MaxDeviceAmount
        {
            get
            {
                if (_maxDeviceAmount == 0)
                {
                    _maxDeviceAmount = new ControllerConfig8036().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8036 config = new BusinessLib.BusinessLogic.ControllerConfig8036();
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
#region  作废-->改为IEditable接口集合
        //private ObservableCollection<DeviceInfo8036> _deviceInfoObservableCollection;
        //public ObservableCollection<DeviceInfo8036> DeviceInfoObservableCollection
        //{
        //    get
        //    {
        //        if (_deviceInfoObservableCollection == null)
        //        {
        //            _deviceInfoObservableCollection = new ObservableCollection<DeviceInfo8036>();
        //        }
        //        return _deviceInfoObservableCollection;
        //    }
        //    set
        //    {
        //        _deviceInfoObservableCollection = value;                
        //        NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
        //    }
        //}
#endregion
        public EditableDeviceInfo8036Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8036Collection(TheLoop, null);
                  //  _deviceInfoCollection.CollectionChanged+=new NotifyCollectionChangedEventHandler
                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                //_maxCode = GetMaxCode(value);
                //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8036 = GetMaxID();
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
            //int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8036;
            //for (int i = 0; i < rowsAmount; i++)
            //{
            //    tempCode++;
            //    deviceID++;
            //    EditableDeviceInfo8036 deviceInfo = new EditableDeviceInfo8036();
            //    deviceInfo.Loop = TheLoop;                
            //    deviceInfo.Code = TheLoop.Code+ tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
            //    deviceInfo.ID = deviceID;
            //    DeviceInfoObservableCollection.Add(deviceInfo);
            //}
            //BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8036 = deviceID;
            //_maxCode = tempCode;
            _deviceService8036.TheLoop = this.TheLoop;
            List<DeviceInfo8036> lstDeviceInfo8036 = _deviceService8036.Create(rowsAmount);
            foreach (var device in lstDeviceInfo8036)
            {
                EditableDeviceInfo8036 editDevice8036 = new EditableDeviceInfo8036();
                editDevice8036.Loop = device.Loop;
                editDevice8036.LoopID = device.LoopID;
                editDevice8036.Code = device.Code;
                editDevice8036.ID = device.ID;
                editDevice8036.TypeCode = device.TypeCode;
                DeviceInfoObservableCollection.Add(editDevice8036);
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
        public List<Model.DeviceInfo8036> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8036 == null)
                {
                    _lstDeviceInfo8036 = new List<DeviceInfo8036>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8036.Add(s.ToDeviceInfo8036());
                    }
                }
                return _lstDeviceInfo8036;
            }
        }
        public void DownloadExecute()
        {
            List<DeviceInfo8036> list = TheLoop.GetDevices<DeviceInfo8036>();
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
            //    if(iCC.TheControllerType != null ) //如果已经取得当前的控制器类型
            //    {
            //        if (iCC.TheControllerType.ControllerType == ControllerType.NT8036) //如果控制器类型不相符，则不执行操作
            //        { 
                       
            //            //iCC.TheControllerType.Status = ControllerStatus.DataSending;
            //            List<LoopModel> lstLoopsModel = new List<LoopModel>();
            //            lstLoopsModel.Add(TheLoop);
            //            ((ControllerType8036)iCC.TheControllerType).Loops = lstLoopsModel;
            //            //((ControllerType8036)iCC.TheControllerType).DeviceInfoList = DeviceInfo;                        
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
                _deviceService8036.TheLoop = this.TheLoop;
                _deviceService8036.SaveToDB();
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
        private int GetMaxCode(EditableDeviceInfo8036Collection deviceInfoCollection)
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
            ControllerOperation8036 controllerOperation = new ControllerOperation8036();
            return controllerOperation.GetMaxDeviceID();
        }

        public void Dispose()
        {
            this.DeviceInfoObservableCollection = null;
        }
    }
}
