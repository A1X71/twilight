/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 158 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-25 10:12:59 +0800 (周二, 25 七月 2017) $
***************************************************************************/
using Caliburn.Micro;
using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using SCA.Interface.UIWPF;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib;

namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{

    public class EditableDeviceInfo8053 : DeviceInfo8053, IEditableObject, IDataErrorInfo
    {
        public event ItemEndEditEventHandler ItemEndEdit;
        public EditableDeviceInfo8053()
        {

        }
        public EditableDeviceInfo8053(DeviceInfo8053 deviceInfo8053)
        {
            this.Loop = deviceInfo8053.Loop;
            this.LoopID = deviceInfo8053.LoopID;
            this.ID = deviceInfo8053.ID;
            this.Code = deviceInfo8053.Code;
            //   this.SimpleCode = deviceInfo8053.SimpleCode;
            this.TypeCode = deviceInfo8053.TypeCode;
            this.Disable = deviceInfo8053.Disable;
            this.LinkageGroup1 = deviceInfo8053.LinkageGroup1;
            this.LinkageGroup2 = deviceInfo8053.LinkageGroup2;
            this.AlertValue = deviceInfo8053.AlertValue;
            this.ForcastValue = deviceInfo8053.ForcastValue;
            this.DelayValue = deviceInfo8053.DelayValue;
            this.BuildingNo = deviceInfo8053.BuildingNo;
            this.ZoneNo = deviceInfo8053.ZoneNo;
            this.FloorNo = deviceInfo8053.FloorNo;
            this.RoomNo = deviceInfo8053.RoomNo;
            this.Location = deviceInfo8053.Location;
        }
        public DeviceInfo8053 ToDeviceInfo8053()
        {
            DeviceInfo8053 deviceInfo8053 = new DeviceInfo8053();
            deviceInfo8053.Loop = this.Loop;
            deviceInfo8053.LoopID = this.LoopID;
            deviceInfo8053.ID = this.ID;
            deviceInfo8053.Code = this.Code;
            //deviceInfo8053.SimpleCode = this.SimpleCode;
            deviceInfo8053.TypeCode = this.TypeCode;
            deviceInfo8053.Disable = this.Disable;
            deviceInfo8053.LinkageGroup1 = this.LinkageGroup1;
            deviceInfo8053.LinkageGroup2 = this.LinkageGroup2;
            deviceInfo8053.AlertValue = this.AlertValue;
            deviceInfo8053.ForcastValue = this.ForcastValue;
            deviceInfo8053.DelayValue = this.DelayValue;
            deviceInfo8053.BuildingNo = this.BuildingNo;
            deviceInfo8053.ZoneNo = this.ZoneNo;
            deviceInfo8053.FloorNo = this.FloorNo;
            deviceInfo8053.RoomNo = this.RoomNo;
            deviceInfo8053.Location = this.Location;
            return deviceInfo8053;
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
                ControllerConfig8053 config = new ControllerConfig8053();
                Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(this.Loop.Controller.DeviceAddressLength);
                SCA.Model.RuleAndErrorMessage rule;
                System.Text.RegularExpressions.Regex exminator;
                string errorMessage = String.Empty;
                switch (columnName)
                {

                    case "Disable":
                        if(this.Disable != null)
                        {
                            rule = dictMessage["Disable"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.Disable.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;                    
                    case "LinkageGroup1":
                        if (!string.IsNullOrEmpty(this.LinkageGroup1))
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
                        if (!string.IsNullOrEmpty(this.LinkageGroup2))
                        {
                            rule = dictMessage["StandardLinkageGroup"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.LinkageGroup2.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "AlertValue":
                        if (this.AlertValue != null)
                        {
                            rule = dictMessage["AlertValue"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.AlertValue.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
                            }
                        }
                        break;
                    case "ForcastValue":
                        if (this.ForcastValue != null)
                        {
                            rule = dictMessage["ForcastValue"];
                            exminator = new System.Text.RegularExpressions.Regex(rule.Rule);
                            if (!exminator.IsMatch(this.ForcastValue.ToString()))
                            {
                                errorMessage = rule.ErrorMessage;
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
                    case "BuildingNo":
                        if (this.BuildingNo != null)
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
                    case "FloorNo":
                        if (this.FloorNo != null)
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
                        if (this.RoomNo != null)
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
                        if(!string.IsNullOrEmpty(this.Location))
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
    public class EditableDeviceInfo8053Collection : ObservableCollection<EditableDeviceInfo8053>
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        private SCA.Interface.IDeviceService<DeviceInfo8053> _deviceInfo8053Service;
        private LoopModel _loop;
        public EditableDeviceInfo8053Collection(LoopModel loop, List<DeviceInfo8053> lstDeviceInfo8053)
        {
            _loop = loop;
            if (lstDeviceInfo8053 != null)
            {
                foreach (var o in lstDeviceInfo8053)
                {
                    
                    this.Add(new EditableDeviceInfo8053(o));
                }
            }
        }
        protected override void InsertItem(int index, EditableDeviceInfo8053 item)
        {
            base.InsertItem(index, item);
            item.ItemEndEdit += new ItemEndEditEventHandler(ItemEndEditHandler);
        }
        private void ItemEndEditHandler(IEditableObject sender)
        {
            EditableDeviceInfo8053 o = (EditableDeviceInfo8053)sender;
            if (o.Code != null)
            {
                _deviceInfo8053Service = new SCA.BusinessLib.BusinessLogic.DeviceService8053();
                _deviceInfo8053Service.TheLoop = _loop;
                o.Loop = _loop;
                _deviceInfo8053Service.Update(o.ToDeviceInfo8053());
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

    //public class EditableDeviceInfoCollection<T1,T2> : ObservableCollection<T1> where T1:EditableDeviceInfo8053
    //    //where T1 : IFactory<T1>, new()
    //    //where T2 : IDevice
    //{
    //    public event ItemEndEditEventHandler ItemEndEdit;

    //    private SCA.Interface.IDeviceService<T2> _deviceInfo8053Service;
    //    private LoopModel _loop;
    //    public EditableDeviceInfoCollection(LoopModel loop, List<T2> lstDeviceInfo8053)
    //    {
    //        _loop = loop;
    //        if (lstDeviceInfo8053 != null)
    //        {
    //            foreach (var o in lstDeviceInfo8053)
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
    //        EditableDeviceInfo8053 o = (EditableDeviceInfo8053)sender;
    //        _deviceInfo8053Service = new SCA.BusinessLib.BusinessLogic.DeviceService8053();
    //        _deviceInfo8053Service.TheLoop = _loop;
    //        o.Loop = _loop;
    //        _deviceInfo8053Service.Update(o.ToDeviceInfo8053());
    //    }
    //}

    #endregion

    //去掉IDeviceInfoViewModel<DeviceInfo8053>,涉及各个控制器的器件类，暂放弃进一步抽象 2017-03-30
    public class DeviceInfo8053ViewModel : PropertyChangedBase//,IDeviceInfoViewModel<DeviceInfo8053>
    {
        private EditableDeviceInfo8053Collection _deviceInfoCollection;
        private List<DeviceInfo8053> _lstDeviceInfo8053;

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
                    _maxDeviceAmount = new ControllerConfig8053().GetMaxDeviceAmountValue();
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
                SCA.BusinessLib.BusinessLogic.ControllerConfig8053 config = new BusinessLib.BusinessLogic.ControllerConfig8053();
                return config.GetDeviceTypeInfo();
            }
        }
        #region  作废-->改为IEditable接口集合
        //private ObservableCollection<DeviceInfo8053> _deviceInfoObservableCollection;
        //public ObservableCollection<DeviceInfo8053> DeviceInfoObservableCollection
        //{
        //    get
        //    {
        //        if (_deviceInfoObservableCollection == null)
        //        {
        //            _deviceInfoObservableCollection = new ObservableCollection<DeviceInfo8053>();
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
        public EditableDeviceInfo8053Collection DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoCollection == null)
                {
                    _deviceInfoCollection = new EditableDeviceInfo8053Collection(TheLoop, null);
                    //  _deviceInfoCollection.CollectionChanged+=new NotifyCollectionChangedEventHandler
                }
                return _deviceInfoCollection;
            }
            set
            {
                _deviceInfoCollection = value;
                _maxCode = GetMaxCode(value);
                BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053 = GetMaxID();
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
            int deviceID = BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053;
            for (int i = 0; i < rowsAmount; i++)
            {
                tempCode++;
                deviceID++;
                EditableDeviceInfo8053 deviceInfo = new EditableDeviceInfo8053();
                deviceInfo.Loop = TheLoop;
                deviceInfo.Code = TheLoop.Code + tempCode.ToString().PadLeft(3, '0');//暂时将器件长度固定为3
                deviceInfo.ID = deviceID;
                DeviceInfoObservableCollection.Add(deviceInfo);
            }
            BusinessLib.ProjectManager.GetInstance.MaxDeviceIDInController8053 = deviceID;
            _maxCode = tempCode;
        }
        public ICommand DownloadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DownloadExecute, null);
            }
        }
        public List<Model.DeviceInfo8053> DeviceInfo
        {
            get
            {
                if (_lstDeviceInfo8053 == null)
                {
                    _lstDeviceInfo8053 = new List<DeviceInfo8053>();
                }
                if (DeviceInfoObservableCollection != null)
                {
                    foreach (var s in DeviceInfoObservableCollection)
                    {
                        _lstDeviceInfo8053.Add(s.ToDeviceInfo8053());
                    }
                }
                return _lstDeviceInfo8053;
            }
        }
        public void DownloadExecute()
        {

            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
                {
                    if (iCC.TheControllerType.ControllerType == ControllerType.NT8053) //如果控制器类型不相符，则不执行操作
                    {

                        //iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        List<LoopModel> lstLoopsModel = new List<LoopModel>();
                        lstLoopsModel.Add(TheLoop);
                        ((ControllerType8053)iCC.TheControllerType).Loops = lstLoopsModel;
                        //((ControllerType8053)iCC.TheControllerType).DeviceInfoList = DeviceInfo;                        
                        iCC.TheControllerType.OperableDataType = OperantDataType.Device;
                        iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                        iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;

                    }
                }
            }
        }
        private void UpdateProcessBarStatus(int currentValue, int totalValue, ControllerNodeType nodeType)
        {
            object[] status = new object[3];
            status[0] = currentValue;
            status[1] = totalValue;
            status[2] = nodeType;
            EventMediator.NotifyColleagues("UpdateProgressBarStatusEvent", status);
        }
        #endregion
        private int GetMaxCode(EditableDeviceInfo8053Collection deviceInfoCollection)
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
            ControllerOperation8053 controllerOperation = new ControllerOperation8053();
            return controllerOperation.GetMaxDeviceID();
        }
    }
}
