using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Input;
using SCA.Model;
using SCA.Model.BusinessModel;
using SCA.BusinessLib;
using SCA.BusinessLib.Controller;
using SCA.BusinessLib.BusinessLogic ;
using SCA.WPF.Infrastructure;
using SCA.BusinessLib.Utility;
using SCA.Interface;
using Caliburn.Micro;
using System.Collections.ObjectModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/1 8:35:16
* FileName   : SummaryInfoViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Query
{
    public class SummaryInfoViewModel:PropertyChangedBase
    {

        private List<string> _baudsRate;        
        /// <summary>
        /// 当前选中的串口
        /// </summary>
        private string _selectedComPort;
        /// <summary>
        /// 当前选中的波特率
        /// </summary>
        private string _selectedBaudRate;

        private string _controllerName;


        private string _controllerMachineNumber;
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";

        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }
        
        //存储控制器摘要信息
        private System.Collections.ObjectModel.ObservableCollection<SummaryInfo> _summaryNodes = null;
        public List<string> BaudsRate
        {
            get
            {
                if (_baudsRate == null)
                {
                    _baudsRate = new List<string>();
                }
                if (_baudsRate.Count == 0)
                {
                    _baudsRate.Add("1200");
                    _baudsRate.Add("2400");                    
                    _baudsRate.Add("4800");
                    _baudsRate.Add("9600");
                    _baudsRate.Add("19200");
                    _baudsRate.Add("38400");
                    _baudsRate.Add("115200");
                }
                return _baudsRate;
            }
        }

        public string SelectedBaudRate
        {
            get
            {
                //string strResult = "";
                //if (TheController != null)
                //{
                //    strResult = TheController.BaudRate.ToString();
                //}
                //return strResult;
                return _selectedBaudRate;

            }
            private set
            {
                _selectedBaudRate = value;
                NotifyOfPropertyChange("SelectedBaudRate");
            }
        }
        public IList<SummaryInfo> SummaryNodes { get { return _summaryNodes ?? (_summaryNodes = new System.Collections.ObjectModel.ObservableCollection<SummaryInfo>()); } }
        public string SelectedComPort
        {

            get
            {
                //string  strResult="";
                //if(ComPorts!=null && TheController!=null )
                //{
                //    var  result = from p in ComPorts where p == TheController.PortName select p;
                //    strResult = result.FirstOrDefault().ToString();
                //}   
                //return strResult;
                return _selectedComPort;
            }
            private set
            {
                _selectedComPort = value;
                NotifyOfPropertyChange("SelectedComPort");
            }

        }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName
        {
            get
            {                
                return _controllerName;
            }
            private set
            {
                _controllerName = value;
                NotifyOfPropertyChange("ControllerName");
            }

        }
        /// <summary>
        /// 控制器机号
        /// </summary>
        public string ControllerMachineNumber
        {
            get
            {
                return _controllerMachineNumber;
            }
            private set
            {
                _controllerMachineNumber = value;
                NotifyOfPropertyChange("ControllerMachineNumber");
            }
        }

        public List<string> ComPorts { get; private set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public ControllerModel TheController { get; private set; }
        #region 命令
        public ICommand SaveCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand<object>(SaveExecute, null); }
        }
        /// <summary>
        /// 控制器所有信息下传
        /// </summary>
        public ICommand DownloadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(DownloadExecute, null);
            }
        }
        public ICommand UploadCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(UploadExecute, null);
            }
        }

        //下传当前控制器下的所有信息
        public void DownloadExecute()
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            
            if (iCC.GetPortStatus())
            {

                if (iCC.TheControllerType.GetType().ToString() != "SCA.BusinessLib.Controller.ControllerTypeUnknown")
                {
                    if (iCC.TheControllerType != null) //如果已经取得当前的控制器类型
                    {
                        iCC.TheControllerType.InitializeCommunicateStatus();
                        #region 8036
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8036) //如果控制器类型不相符，则不执行操作
                        {
                            //下传所有回路信息
                      //      foreach (var linkage in TheController.Loops)
                           // {
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                                List<LoopModel> lstLoopsModel = new List<LoopModel>();
                                lstLoopsModel = TheController.Loops.ToList<LoopModel>();
                                ((ControllerType8036)iCC.TheControllerType).Loops = lstLoopsModel;
                                //((ControllerType8036)iCC.TheControllerType).DeviceInfoList = linkage.GetDevices<DeviceInfo8036>().ToList<DeviceInfo8036>();
                            
                                ((ControllerType8036)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig.ToList<LinkageConfigStandard>();

                                iCC.TheControllerType.OperableDataType = OperantDataType.DownloadAll;
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;    
                       //     }
                            ////下传所有标准组态信息
                            //if (iCC.TheControllerType.Status != ControllerStatus.DataSending)
                            //{ 
                            //    iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            
                            //    iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            //    iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            //}
                        }
                        #endregion
                        #region 8001

                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8001) //如果控制器类型不相符，则不执行操作
                        {
                            //下传所有回路信息
                            foreach (var l in TheController.Loops)
                            {
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                                ((ControllerType8001)iCC.TheControllerType).DeviceInfoList = l.GetDevices<DeviceInfo8001>();
                                iCC.TheControllerType.OperableDataType = OperantDataType.Device;
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            }

                            //下传所有标准组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8001)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有混合组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8001)iCC.TheControllerType).MixedLinkageConfigList = TheController.MixedConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.MixedLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有通用组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8001)iCC.TheControllerType).GeneralLinkageConfigList = TheController.GeneralConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.GeneralLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有网络手动盘信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8001)iCC.TheControllerType).ManualControlBoardList = TheController.ControlBoard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.MannualControlBoard;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        }

                        #endregion 
                        #region 8007
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8007) //如果控制器类型不相符，则不执行操作
                        {

                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            List<LoopModel> lstLoopsModel = new List<LoopModel>();
                            lstLoopsModel = TheController.Loops.ToList<LoopModel>();
                            ((ControllerType8007)iCC.TheControllerType).Loops = lstLoopsModel;   
                            ((ControllerType8007)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig.ToList<LinkageConfigStandard>();
                            iCC.TheControllerType.OperableDataType = OperantDataType.DownloadAll;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;

                        }
                        #endregion
                        #region 8003
                        if (iCC.TheControllerType.ControllerType == ControllerType.FT8003) //如果控制器类型不相符，则不执行操作
                        {

                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            List<LoopModel> lstLoopsModel = new List<LoopModel>();
                            lstLoopsModel = TheController.Loops.ToList<LoopModel>();
                            ((ControllerType8003)iCC.TheControllerType).Loops = lstLoopsModel;
                            ((ControllerType8003)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig.ToList<LinkageConfigStandard>();
                            iCC.TheControllerType.OperableDataType = OperantDataType.DownloadAll;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        }
                        #endregion
                        #region 8000
                        if (iCC.TheControllerType.ControllerType == ControllerType.FT8000) //如果控制器类型不相符，则不执行操作
                        {

                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            List<LoopModel> lstLoopsModel = new List<LoopModel>();
                            lstLoopsModel = TheController.Loops.ToList<LoopModel>();
                            ((ControllerType8000)iCC.TheControllerType).Loops = lstLoopsModel;
                            ((ControllerType8000)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig.ToList<LinkageConfigStandard>();
                            iCC.TheControllerType.OperableDataType = OperantDataType.DownloadAll;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        }
                        #endregion
                        #region 8021
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8021) //如果控制器类型不相符，则不执行操作
                        {

                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            List<LoopModel> lstLoopsModel = new List<LoopModel>();
                            lstLoopsModel = TheController.Loops.ToList<LoopModel>();
                            ((ControllerType8021)iCC.TheControllerType).Loops = lstLoopsModel;                            
                            iCC.TheControllerType.OperableDataType = OperantDataType.DownloadAll;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                        #region 8053

                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8053) //如果控制器类型不相符，则不执行操作
                        {
                            //下传所有回路信息
                            foreach (var l in TheController.Loops)
                            {
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                                ((ControllerType8053)iCC.TheControllerType).DeviceInfoList = l.GetDevices<DeviceInfo8053>();
                                iCC.TheControllerType.OperableDataType = OperantDataType.Device;
                                iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            }

                            //下传所有标准组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8053)iCC.TheControllerType).StandardLinkageConfigList = TheController.StandardConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有混合组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8053)iCC.TheControllerType).MixedLinkageConfigList = TheController.MixedConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.MixedLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有通用组态信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8053)iCC.TheControllerType).GeneralLinkageConfigList = TheController.GeneralConfig;
                            iCC.TheControllerType.OperableDataType = OperantDataType.GeneralLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;

                            //下传所有网络手动盘信息
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                            ((ControllerType8053)iCC.TheControllerType).ManualControlBoardList = TheController.ControlBoard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.MannualControlBoard;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        }

                        #endregion 

                    }
                }
            }

        }
        //上传当前控制器下的所有信息
        public void UploadExecute()
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                if (iCC.TheControllerType.GetType().ToString() != "SCA.BusinessLib.Controller.ControllerTypeUnknown")
                {
                    if (iCC.TheControllerType != null && iCC.TheControllerType.ControllerType != ControllerType.NONE) //如果已经取得当前的控制器类型
                    {
                        #region 8036
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8036) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8036> lstDevicesInfo = new List<DeviceInfo8036>();
                            List<LinkageConfigStandard> lstStandardLinkageConfig = new List<LinkageConfigStandard>();
                            ((ControllerType8036)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;
                            ((ControllerType8036)iCC.TheControllerType).StandardLinkageConfigList = lstStandardLinkageConfig;
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                        }
                        #endregion
                        #region 8007
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8007) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8007> lstDevicesInfo = new List<DeviceInfo8007>();
                            List<LinkageConfigStandard> lstStandardLinkageConfig = new List<LinkageConfigStandard>();
                            ((ControllerType8007)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;
                            ((ControllerType8007)iCC.TheControllerType).StandardLinkageConfigList = lstStandardLinkageConfig;
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                        #region 8003
                        if (iCC.TheControllerType.ControllerType == ControllerType.FT8003) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8003> lstDevicesInfo = new List<DeviceInfo8003>();
                            List<LinkageConfigStandard> lstStandardLinkageConfig = new List<LinkageConfigStandard>();
                            ((ControllerType8003)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;
                            ((ControllerType8003)iCC.TheControllerType).StandardLinkageConfigList = lstStandardLinkageConfig;
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                        #region 8000
                        if (iCC.TheControllerType.ControllerType == ControllerType.FT8000) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8000> lstDevicesInfo = new List<DeviceInfo8000>();
                            List<LinkageConfigStandard> lstStandardLinkageConfig = new List<LinkageConfigStandard>();
                            ((ControllerType8000)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;
                            ((ControllerType8000)iCC.TheControllerType).StandardLinkageConfigList = lstStandardLinkageConfig;
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                        #region 8021
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8021) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8021> lstDevicesInfo = new List<DeviceInfo8021>();                            
                            ((ControllerType8021)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;                            
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                        #region 8053
                        if (iCC.TheControllerType.ControllerType == ControllerType.NT8053) //如果控制器类型不相符，则不执行操作
                        {
                            List<DeviceInfo8053> lstDevicesInfo = new List<DeviceInfo8053>();
                            ((ControllerType8053)iCC.TheControllerType).DeviceInfoList = lstDevicesInfo;
                            iCC.TheController = TheController;
                            iCC.TheControllerType.Status = ControllerStatus.DataReceiving;
                            iCC.AllDataUploadedEvent += UploadedFinished;
                            iCC.TheControllerType.UpdateProgressBarEvent += UpdateProcessBarStatus;
                        }
                        #endregion
                    }
                }
            }
        }
        private void UpdateProcessBarStatus(int currentValue, int totalValue,ControllerNodeType nodeType)
        {
            object[] status = new object[3];
            status[0] = currentValue;
            status[1] = totalValue;
            status[2] = nodeType;
            EventMediator.NotifyColleagues("UpdateProgressBarStatusEvent", status);
        }
        private void UploadedFinished(ControllerModel controller)
        {
            ControllerModel c = SCA.BusinessLib.ProjectManager.GetInstance.GetControllerBySpecificID(TheController.ID);
            if (controller.Loops != null)
            { 
                foreach(var loop in controller.Loops)
                {

                  //  loop.Code = loop.ID.ToString().PadLeft(c.LoopAddressLength, '0'); 
                  //  loop.Name = loop.Code;
                    loop.Controller = c;
                    c.Loops.Add(loop);
                }
            }
            if (controller.StandardConfig != null)
            { 
                foreach (var linkage in controller.StandardConfig)
                {                
                    c.StandardConfig.Add(linkage);
                }
            }
            if (controller.MixedConfig != null)
            {
                foreach (var linkage in controller.MixedConfig)
                {
                    c.MixedConfig.Add(linkage);
                }            
            }
            if (controller.GeneralConfig != null)
            {
                foreach (var linkage in controller.GeneralConfig)
                {
                    c.GeneralConfig.Add(linkage);
                }
            }
            if (controller.ControlBoard != null)
            {
                foreach (var board in controller.ControlBoard)
                {
                    c.ControlBoard.Add(board);
                }
            }
            
            
            //p.Controllers.Add(controller);
            //List<SCA.Model.ProjectModel> lstProject = new List<SCA.Model.ProjectModel>();
            //lstProject.Add(p);

            
            EventMediator.NotifyColleagues("UploadedFinished", TheController);
        }

        public void SaveExecute(object o)
        {
            //SCA.WPF.ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel vm = (SCA.WPF.ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel)this.DataContext;
            //ControllerModel controller = vm.TheController;
            //controller.Name = ControllerNameInputTextBox.Text;
            //controller.MachineNumber = MachineNumberInputTextBox.Text;
            //controller.PortName = ComPortComboBox.SelectedItem.ToString();
            //controller.BaudRate = Convert.ToInt32(BaudsRateComboBox.SelectedItem);
            //vm.SaveExecute(controller);
            //controller.IsDirty = true;
        }

        #endregion 
        public SummaryInfoViewModel()
        {
            InitializeComPorts();
            
        }
        /// <summary>
        /// 设置当前页面所属的控制器
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(ControllerModel controller)
        {
            SCA.Model.ControllerModel c;
            if (controller == null)
            {
                c = ProjectManager.GetInstance.GetPrimaryController();
            }
            else
            {
                c = ProjectManager.GetInstance.GetControllerBySpecificID(controller.ID);
            }

            TheController = c;
            ControllerName = TheController.Name;
            ControllerMachineNumber = TheController.MachineNumber;
            SelectedBaudRate = TheController.BaudRate.ToString();
            SelectedComPort = TheController.PortName;
            //string strResult = "";
            //if (ComPorts != null && TheController != null)
            //{
            //    var result = from p in ComPorts where p == TheController.PortName select p;
            //    strResult = result.FirstOrDefault().ToString();
            //}
            //return strResult;
            GenerateSummaryInfo();
        }
        /// <summary>
        /// 取得本机可用的串口号
        /// </summary>
        private void InitializeComPorts()
        {
            List<string> comPorts = new List<string>();
            string [] portsName=SerialPort.GetPortNames();
            if (portsName.Length > 0)
            {
                for (int i = 0; i < portsName.Length; i++)
                {
                    comPorts.Add(portsName[i]);                    
                }
            }
            ComPorts = comPorts;
        }
        /// <summary>
        /// 生成控制器摘要信息
        /// </summary>
        private void GenerateSummaryInfo()
        {
            SummaryNodes.Clear();
            if (TheController != null)
            {
                ControllerManager controllerManager = new ControllerManager();
                controllerManager.InitializeAllControllerOperation(null);
                IControllerOperation controllerOperation = controllerManager.GetController(TheController.Type);    
                //controllerOperation
                SummaryInfo controllerSummary = controllerOperation.GetSummaryNodes(TheController, 1);
                SummaryNodes.Add(controllerSummary);
            }
        }
        
    }
    
}
