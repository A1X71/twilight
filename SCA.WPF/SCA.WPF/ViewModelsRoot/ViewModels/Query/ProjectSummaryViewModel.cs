using SCA.Model;
using SCA.WPF.Infrastructure;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SCA.BusinessLib;
using SCA.WPF.Utility;
using SCA.Interface;
using SCA.BusinessLib.BusinessLogic;
using SCA.Model.BusinessModel;

using Caliburn.Micro;
using System;

/* ==============================
*
* Author     : William
* Create Date: 2017/5/3 14:02:16
* FileName   : ProjectSummaryViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Query
{


    public class ProjectSummaryViewModel:PropertyChangedBase
    {
        public ProjectModel TheProject { get; set; }
        private string _projectName;
        private ControllerManager _cManager;
        private bool _standardLinkageSimulatorFlag = true;
        private bool _mixedLinkageSimulatorFlag = false;
        private bool _generalLinkageSimulatorFlag = false;
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _simulatorIconPath = @"Resources/Icon/Style1/simulator.png";
        

        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string SimulatorIconPath { get { return _appCurrentPath + _simulatorIconPath; } }
        //存储摘要信息
        private System.Collections.ObjectModel.ObservableCollection<SummaryInfo> _summaryNodes = null;
        public ProjectSummaryViewModel()
        {
            _cManager = new ControllerManager();
            _cManager.InitializeAllControllerOperation(null);
        }
        public bool StandardLinkageSimulatorFlag
        {
            get
            {
                return _standardLinkageSimulatorFlag;
            }
            set
            {
                _standardLinkageSimulatorFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public bool MixedLinkageSimulatorFlag
        {
            get
            {
                return _mixedLinkageSimulatorFlag;
            }
            set
            {
                _mixedLinkageSimulatorFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public bool GeneralLinkageSimulatorFlag
        {
            get
            {
                return _generalLinkageSimulatorFlag;
            }
            set
            {
                _generalLinkageSimulatorFlag = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
               
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
                NotifyOfPropertyChange("ProjectName");
            }
        }
        private Visibility _simulatorVisibility = Visibility.Collapsed;
        private Visibility _summaryVisibility = Visibility.Visible;
        public Visibility SimulatorVisibility 
        {
            get
            {
                return _simulatorVisibility;
            }
            set
            {
                _simulatorVisibility = value;
                //NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                NotifyOfPropertyChange("SimulatorVisibility");
            }
        }
        public Visibility SummaryVisibility
        {
            get
            {
                return _summaryVisibility;
            }
            set
            {
                _summaryVisibility = value;
//                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
                NotifyOfPropertyChange("SummaryVisibility");
            }        
        }
        public ICommand SaveProjectInfoCommand
        { 
            get { return new SCA.WPF.Utility.RelayCommand(SaveProjectInfoExecute,null);}
        }
        public void SaveProjectInfoExecute()
        {
            using (new WaitCursor())
            { 
                SCA.BusinessLib.ProjectManager.GetInstance.SaveProject();
            }
        }
        public ICommand LinkageSimulatorCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(LinkageSimulatorExecute, null); }
        }
        public ICommand ToggleSimulatorInfoCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ToggleSimulatorInfoExecute, null); }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new SCA.WPF.Utility.RelayCommand(RefreshExecute, null); 
            }
        }
        public IList<SummaryInfo> SummaryNodes { get { return _summaryNodes ?? (_summaryNodes = new System.Collections.ObjectModel.ObservableCollection<SummaryInfo>()); } }
        public void ToggleSimulatorInfoExecute()
        {

            if (SummaryVisibility==Visibility.Visible)
            {
                SummaryVisibility = Visibility.Collapsed;
                SimulatorVisibility = Visibility.Visible;
            }
            else
            {                
                SummaryVisibility = Visibility.Visible;
                SimulatorVisibility = Visibility.Collapsed;
            }
        }

        public void RefreshExecute()
        { 
            if(ControllerID!=-1)
            {
                GetQueryDeviceInfo();
            }
            
        }
        public ICommand QueryGridSelectedDeviceCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(QueryGridSelectedDeviceExecute, null); }
        }
        public void QueryGridSelectedDeviceExecute()
        {
            string str = "welcome";
        }
        #region Simulator界面相关操作
        int _controllerID=-1;
        public List<ControllerModel> ValidController
        {
            get
            {  
                //NT8021没有组态信息
                return ProjectManager.GetInstance.Controllers.Where((d)=>d.Type!=ControllerType.NT8021).ToList<ControllerModel>();
            }            
        }
        public int ControllerID
        {
            get
            {
                return _controllerID;
            }
            set
            {
                _controllerID = value;
                GetValidLoops(_controllerID);
                NotifyOfPropertyChange("ControllerID");
                //MethodBase.GetCurrentMethod().GetPropertyName()
                
                
                
               GetQueryDeviceInfo();
                
            
                //NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        int _loopID;
        List<LoopModel> _lstLoops;
        public List<LoopModel> ValidLoops
        {
            get
            { 
                return _lstLoops;
            }
            set
            {
                _lstLoops = value;
                NotifyOfPropertyChange("ValidLoops");
                GetDeviceTypeOfController(_controllerID);
                //LoopID = _lstLoops.FirstOrDefault().ID;                
                //NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        /// <summary>
        /// 根据选择的控制器ID，获取此控制器下的所有回路
        /// </summary>
        /// <param name="controllerID"></param>
        private void GetValidLoops(int  controllerID)
        { 
            List<ControllerModel> lstControllers;
            
            if (controllerID == -1)
            {
                lstControllers=ProjectManager.GetInstance.Controllers;
            }
            else
            {
               var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
               lstControllers = result.ToList<ControllerModel>();
            }

            List<LoopModel> lstLoops = new List<LoopModel>();
            foreach (var c in lstControllers)
            {
                foreach (var l in c.Loops)
                {
                    lstLoops.Add(l);
                }
            }
            ValidLoops = lstLoops;            
        }

        public int LoopID
        {
            get
            {
                return _loopID;
            }
            set
            {
                _loopID = value;
                
                NotifyOfPropertyChange("LoopID");
                GetQueryDeviceInfo();
                //MethodBase.GetCurrentMethod().GetPropertyName()
                
            }
        }
        private short? _buindingNo;
        public short? BuildingNo
        {
            get
            {
                return _buindingNo;
            }
            set
            {
                _buindingNo = value;
               
                NotifyOfPropertyChange("BuildingNo");
                GetQueryDeviceInfo();
                //MethodBase.GetCurrentMethod().GetPropertyName()
               
                
            }
        }
        Dictionary<short, short?> _lstBuildingNoCollection;
        public Dictionary<short, short?> BuildingNoCollection
        {
            get
            {
                return _lstBuildingNoCollection;
            }
            set
            {
                _lstBuildingNoCollection = value;
                NotifyOfPropertyChange("BuildingNoCollection");
                //MethodBase.GetCurrentMethod().GetPropertyName()
            }
        }

        private void GetValidBuildingNoCollection(int controllerID)
        {
            if (controllerID != -1)
            {
                var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
                ControllerModel controller = result.FirstOrDefault();
                //List<ControllerModel> lstControllers;
                IControllerOperation cOperation = _cManager.GetController(controller.Type);
                Dictionary<short,short?> dictBuildingNo=new Dictionary<short,short?>();
                short i=0;
                foreach(var l in cOperation.GetBuildingNoCollection(controller.ID))
                {
                   dictBuildingNo.Add(i,l);
                   i++;
                }
                BuildingNoCollection = dictBuildingNo;
            }
            //ValidLoops = lstLoops;
        }
        private void GetDeviceTypeOfController(int controllerID)
        {
            if (controllerID != -1)
            {
                var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
                ControllerModel controller = result.FirstOrDefault();

                //List<ControllerModel> lstControllers;
            //    ControllerManager cManager = new ControllerManager();
             //   cManager.InitializeAllControllerOperation(null);
                IControllerOperation cOperation = _cManager.GetController(controller.Type);
                DeviceType = cOperation.GetConfiguredDeviceTypeCollection(controller.ID);
            }
        }

        private List<DeviceType> _lstDeviceType;
        public List<DeviceType> DeviceType
        {
            get
            {
                return _lstDeviceType;
            }
            set
            {
                _lstDeviceType = value;
                NotifyOfPropertyChange("DeviceType");
                GetValidBuildingNoCollection(_controllerID);
                
                //MethodBase.GetCurrentMethod().GetPropertyName()
            }
        }
        private System.Int16 _typeCode;
        public System.Int16 TypeCode
        {
            get
            {
                return _typeCode;
            }
            set
            {
                _typeCode = value;
                
                NotifyOfPropertyChange("TypeCode");
                GetQueryDeviceInfo();
                //MethodBase.GetCurrentMethod().GetPropertyName()
                
                
            }
        }
        
        #endregion

        #region 列表
        private ObservableCollection<DeviceInfoForSimulator> _queryDeviceInfoObservableCollection;

        public ObservableCollection<DeviceInfoForSimulator> QueryDeviceInfoObservableCollection
        {
            get
            {
                if (_queryDeviceInfoObservableCollection == null)
                {
                    _queryDeviceInfoObservableCollection = new ObservableCollection<DeviceInfoForSimulator>();
                }
                return _queryDeviceInfoObservableCollection;
            }
            set
            {
                _queryDeviceInfoObservableCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        /// <summary>
        /// 输入器件
        /// </summary>
        private ObservableCollection<DeviceInfoForSimulator> _inputDeviceInfoObservableCollection;

        public ObservableCollection<DeviceInfoForSimulator> InputDeviceInfoObservableCollection
        {
            get
            {
                if (_inputDeviceInfoObservableCollection == null)
                {
                    _inputDeviceInfoObservableCollection = new ObservableCollection<DeviceInfoForSimulator>();
                }
                return _inputDeviceInfoObservableCollection;
            }
            set
            {
                _inputDeviceInfoObservableCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        
        /// <summary>
        /// 联动器件
        /// </summary>
        private ObservableCollection<DeviceInfoForSimulator> _linkageDeviceInfoObservableCollection;

        public ObservableCollection<DeviceInfoForSimulator> LinkageDeviceInfoObservableCollection
        {
            get
            {
                if (_linkageDeviceInfoObservableCollection == null)
                {
                    _linkageDeviceInfoObservableCollection = new ObservableCollection<DeviceInfoForSimulator>();
                }
                return _linkageDeviceInfoObservableCollection;
            }
            set
            {
                _linkageDeviceInfoObservableCollection = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        
        public void AddDeviceInfoToInputGrid(SCA.Model.DeviceInfoForSimulator device)
        {
            if (!InputDeviceInfoObservableCollection.Contains(device))
            { 
                this.InputDeviceInfoObservableCollection.Add(device);
            }
        }
        public void RemoveDeviceInfoFromInputGrid(SCA.Model.DeviceInfoForSimulator device)
        {
            this.InputDeviceInfoObservableCollection.Remove(device);
        }
        #region 器件查询
        private void GetQueryDeviceInfo()
        {
            DeviceInfoForSimulator deviceInfo = new DeviceInfoForSimulator();
            var result = from c in ProjectManager.GetInstance.Controllers where c.ID == ControllerID select c;
            ControllerModel controller = result.FirstOrDefault<ControllerModel>();
            //返回所有的器件类型
            IControllerOperation cOperation = _cManager.GetController(controller.Type);
            List<DeviceInfoForSimulator> lstSimulatorDevices = cOperation.GetSimulatorDevices(controller);
            lstSimulatorDevices = ProcessQueryCondition(lstSimulatorDevices);
            QueryDeviceInfoObservableCollection.Clear();
            foreach (var d in lstSimulatorDevices)
            {
                QueryDeviceInfoObservableCollection.Add(d);
            }
            
        }
        private List<DeviceInfoForSimulator> ProcessQueryCondition(List<DeviceInfoForSimulator> lstDeviceInfo)
        {
            Dictionary<QueryType, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>> dictMap = new Dictionary<QueryType, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>>();
            dictMap.Add(QueryType.QueryByLoop, QueryByLoop);
            dictMap.Add(QueryType.QueryByBuildingNo, QueryByBuildingNo);
            dictMap.Add(QueryType.QueryByType, QueryByType);
            dictMap.Add(QueryType.QueryByLoopAndBuildingNo, QueryByLoopAndBuildingNo);
            dictMap.Add(QueryType.QueryByLoopAndType, QueryByLoopAndType);
            dictMap.Add(QueryType.QueryByBuildingNoAndType, QueryByBuildingNoAndType);
            dictMap.Add(QueryType.QueryByLoopAndBuildingNoAndType, QueryByLoopAndBuildingNoAndType);
            dictMap.Add(QueryType.QueryByNothing, QueryByNothing);
            System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>> execMethod;
            dictMap.TryGetValue(GetQueryType(),out execMethod);
            return execMethod(lstDeviceInfo);            
        }
        private QueryType GetQueryType()
        {
            QueryType result = QueryType.QueryByLoop;
            if (LoopID != 9999 && BuildingNo!=9999 && TypeCode!=9999)
            {
                result = QueryType.QueryByLoopAndBuildingNoAndType;
                
            }
            else if (LoopID != 9999 && BuildingNo == 9999 && TypeCode == 9999)
            {
                result = QueryType.QueryByLoop;
            }
            else if (LoopID == 9999 && BuildingNo != 9999 && TypeCode == 9999)
            {
                result = QueryType.QueryByBuildingNo;
            }
            else if (LoopID == 9999 && BuildingNo == 9999 && TypeCode != 9999)
            {
                result = QueryType.QueryByType;
            }
            else if (LoopID != 9999 && BuildingNo != 9999 && TypeCode == 9999)
            {
                result = QueryType.QueryByLoopAndBuildingNo;
            }
            else if (LoopID != 9999 && BuildingNo == 9999 && TypeCode != 9999)
            {
                result = QueryType.QueryByLoopAndType;
            }
            else if (LoopID == 9999 && BuildingNo != 9999 && TypeCode != 9999)
            {
                result = QueryType.QueryByLoopAndBuildingNoAndType;
            }
            else if (LoopID == 9999 && BuildingNo == 9999 && TypeCode == 9999)
            {
                result = QueryType.QueryByNothing;
            }
            return result;
        }        
        private List<DeviceInfoForSimulator> QueryByLoop(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == LoopID).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByBuildingNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == BuildingNo).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.TypeCode == TypeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndBuildingNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == LoopID && d.BuildingNo==BuildingNo).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.Loop.ID == LoopID && d.TypeCode == TypeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByBuildingNoAndType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == BuildingNo && d.TypeCode == TypeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByLoopAndBuildingNoAndType(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == BuildingNo && d.TypeCode == TypeCode &&d.Loop.ID==LoopID).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByNothing(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        #endregion

        #endregion

    
        public void LinkageSimulatorExecute()
        {
            var result = ProjectManager.GetInstance.Project.Controllers.Where((c) => c.ID == ControllerID);
            ControllerModel controller = result.FirstOrDefault();
            IControllerOperation cOperation = _cManager.GetController(controller.Type);//取得控制器操作类
            List<string> lstOutputDeviceCode=null; //存储联动输出器件编号
            List<DeviceInfoForSimulator> lstSimulatorDevices=null; //存储此控制器下所有器件信息
            List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine = GetAllDevicesOfOtherMachine();//取得它机器件信息
            LinkageDeviceInfoObservableCollection.Clear();
            #region 标准组态调用
            if (StandardLinkageSimulatorFlag)
            {
                if (lstSimulatorDevices != null)
                {
                    lstSimulatorDevices.Clear();
                }
                if (lstOutputDeviceCode != null)
                {
                    lstOutputDeviceCode.Clear();
                }
                lstOutputDeviceCode = StandardLinkageSimulator(controller);                
                lstSimulatorDevices = cOperation.GetSimulatorDevicesByDeviceCode(lstOutputDeviceCode, controller, lstAllDevicesOfOtherMachine);                
                //LinkageDeviceInfoObservableCollection.Clear();
                foreach (var d in lstSimulatorDevices)
                {
                    LinkageDeviceInfoObservableCollection.Add(d);
                }
            }
            #endregion

            #region 混合组态调用
            if (MixedLinkageSimulatorFlag)
            {                
                if (lstSimulatorDevices != null)
                { 
                    lstSimulatorDevices.Clear();
                }
                if (lstOutputDeviceCode != null)
                {
                    lstOutputDeviceCode.Clear();
                }
                lstOutputDeviceCode = MixedLinkageSimulator(controller);
                lstSimulatorDevices = cOperation.GetSimulatorDevicesByDeviceCode(lstOutputDeviceCode, controller, lstAllDevicesOfOtherMachine);
                foreach (var d in lstSimulatorDevices)
                {
                    LinkageDeviceInfoObservableCollection.Add(d);
                }
            }
            #endregion

            #region 通用组态调用
            if (GeneralLinkageSimulatorFlag)
            { 
                if (lstSimulatorDevices != null)
                {
                    lstSimulatorDevices.Clear();
                }

                if (lstOutputDeviceCode != null)
                {
                    lstOutputDeviceCode.Clear();
                }

             //   LinkageDeviceInfoObservableCollection.Clear();
                lstOutputDeviceCode = GeneralLinkageSimulator(controller);
                lstSimulatorDevices = cOperation.GetSimulatorDevicesByDeviceCode(lstOutputDeviceCode, controller, lstAllDevicesOfOtherMachine);
                foreach (var d in lstSimulatorDevices)
                {
                    LinkageDeviceInfoObservableCollection.Add(d);
                }
            }
            #endregion
        }
        /// <summary>
        /// 取得本项目下其它控制器中的器件信息
        /// </summary>
        /// <returns></returns>
        private List<DeviceInfoForSimulator> GetAllDevicesOfOtherMachine()
        {
            var result = ProjectManager.GetInstance.Project.Controllers.Where((c) => c.ID != ControllerID);
            List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine = new List<DeviceInfoForSimulator>();
            foreach (var c in result.ToList<ControllerModel>())
            {
                IControllerOperation cOperation = _cManager.GetController(c.Type);
                foreach (var d in cOperation.GetSimulatorDevices(c))
                {
                    lstAllDevicesOfOtherMachine.Add(d);
                }
            }
            return lstAllDevicesOfOtherMachine;
        }
        #region 标准组态联动
        /// <summary>
        /// 根据输入器件取得“输出组”，并取得联动器件编码
        /// </summary>
        /// <param name="controller">当前控制器</param>
        /// <returns></returns>
        private List<string> StandardLinkageSimulator(ControllerModel controller)
        {
            //var result = ProjectManager.GetInstance.Project.Controllers.Where((c) => c.ID == ControllerID);
            //ControllerModel controller = result.FirstOrDefault();

            //记录需要输出的器件编号
            List<string> lstOutputDeviceCode = new List<string>();
            //记录标准组态激活次数
            Dictionary<string, int> standardLinkageTriggerCount = new Dictionary<string, int>();

            
            foreach (var r in InputDeviceInfoObservableCollection) //初始化器件信息中所有的“输出组“计数
            {
                if (r.LinkageGroup1 != "")
                {
                    if (!standardLinkageTriggerCount.ContainsKey(r.LinkageGroup1) && r.LinkageGroup1!="0")
                    {
                        standardLinkageTriggerCount.Add(r.LinkageGroup1, 1);
                    }
                    else
                    {
                        standardLinkageTriggerCount[r.LinkageGroup1]++;
                    }
                }
                if (r.LinkageGroup2 != "")
                {
                    if (!standardLinkageTriggerCount.ContainsKey(r.LinkageGroup2) && r.LinkageGroup2 != "0")
                    {
                        standardLinkageTriggerCount.Add(r.LinkageGroup2, 1);
                    }
                    else
                    {
                        standardLinkageTriggerCount[r.LinkageGroup2]++;
                    }
                }
                if (r.LinkageGroup3 != "")
                {
                    if (!standardLinkageTriggerCount.ContainsKey(r.LinkageGroup3) && r.LinkageGroup3 != "0")
                    {
                        standardLinkageTriggerCount.Add(r.LinkageGroup3, 1);
                    }
                    else
                    {
                        standardLinkageTriggerCount[r.LinkageGroup3]++;
                    }
                }
            }
            

            lstOutputDeviceCode=FindAllDevicesForStandardSimulator(standardLinkageTriggerCount,controller.StandardConfig,null);
            return lstOutputDeviceCode;
        }
        /// <summary>
        /// 在“标准组态”中取得可以输出的“器件编吗”
        /// </summary>
        /// <param name="dictTriggerCount">输出组及其计数</param>
        /// <param name="standardConfig">当前控制器的标准组态信息</param>
        /// <param name="triggeredLinkageCode">已经触发的标准组态编码</param>
        /// <returns></returns>
        private List<string> FindAllDevicesForStandardSimulator(Dictionary<string, int> dictTriggerCount,List<LinkageConfigStandard> standardConfig,List<string> triggeredLinkageCode)
        {
            List<string> lstOutputDeviceCode = new List<string>();
      

            //记录已经触发过的标准组态编号
            List<string> lstTriggeredLinkageCode;
            if (triggeredLinkageCode != null)
            {
                lstTriggeredLinkageCode = triggeredLinkageCode;                
            }
            else
            {
                lstTriggeredLinkageCode = new List<string>();
            }


            List<string> lstUnTriggerLinkageCode = new List<string>();

            //记录联动输出组记数
            Dictionary<string, int> dictCascadingLinkageTriggerCount = new Dictionary<string, int>();

            #region 以标组结合为基准,基于效率，弃用
            //foreach (var linkage in standardConfig)
            //{
            //    if (dictTriggerCount.ContainsKey(linkage.Code))
            //    {
            //        if (dictTriggerCount[linkage.Code] >= linkage.ActionCoefficient)
            //        {
            //            lstTriggeredLinkageCode.Add(linkage.Code);//记录至已触发输出组中
            //            //记录输出器件
            //            if (!lstOutputDeviceCode.Contains(linkage.DeviceNo1))
            //            {
            //                lstOutputDeviceCode.Add(linkage.DeviceNo1);
            //            }

            //            //初始化联动组记数
            //            if (!lstTriggeredLinkageCode.Contains(linkage.LinkageNo1))
            //            { 
            //                if (dictTriggerCount.ContainsKey(linkage.LinkageNo1) && !dictCascadingLinkageTriggerCount.ContainsKey(linkage.LinkageNo1))
            //                {
            //                    dictCascadingLinkageTriggerCount.Add(linkage.LinkageNo1, dictTriggerCount[linkage.LinkageNo1]+1);
            //                }
            //                else if (!dictCascadingLinkageTriggerCount.ContainsKey(linkage.LinkageNo1))
            //                {
            //                    dictCascadingLinkageTriggerCount.Add(linkage.LinkageNo1, 0);
            //                }
            //                else 
            //                {
            //                    dictCascadingLinkageTriggerCount[linkage.LinkageNo1]++;
            //                }
            //            }

                        
            //        }
            //    }
            //}
            #endregion
            //联动组态标志
            bool isTriggerCascadeingLinkage = false;
            foreach (var trigger in dictTriggerCount)
            {
                var item =standardConfig.Where((standard) => standard.Code == trigger.Key);
                LinkageConfigStandard lcs=item.FirstOrDefault<LinkageConfigStandard>();
                if (lcs == null)
                {
                    continue;
                }
                if (trigger.Value >= lcs.ActionCoefficient)
                {
                    isTriggerCascadeingLinkage = true;
                    lstTriggeredLinkageCode.Add(trigger.Key);//记录至已触发输出组中
                    //记录输出器件
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo1) && lcs.DeviceNo1!="")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo1);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo2) && lcs.DeviceNo2 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo2);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo3) && lcs.DeviceNo3 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo3);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo4) && lcs.DeviceNo4 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo4);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo5) && lcs.DeviceNo5 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo6);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo6) && lcs.DeviceNo6 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo6);
                    }
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo7) && lcs.DeviceNo7 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo7);
                    } 
                    if (!lstOutputDeviceCode.Contains(lcs.DeviceNo8) && lcs.DeviceNo8 != "")
                    {
                        lstOutputDeviceCode.Add(lcs.DeviceNo8);
                    }


                    //初始化联动组记数
                    if (!lstTriggeredLinkageCode.Contains(lcs.LinkageNo1) && lcs.LinkageNo1 != "0")
                    {
                        if (dictTriggerCount.ContainsKey(lcs.LinkageNo1) && !dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo1))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo1, dictTriggerCount[lcs.LinkageNo1] + 1);
                        }
                        else if (!dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo1))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo1, 1);
                        }
                        else
                        {
                            dictCascadingLinkageTriggerCount[lcs.LinkageNo1]++;
                        }
                    }
                    if (!lstTriggeredLinkageCode.Contains(lcs.LinkageNo2) && lcs.LinkageNo2!="0")
                    {
                        if (dictTriggerCount.ContainsKey(lcs.LinkageNo2) && !dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo2))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo2, dictTriggerCount[lcs.LinkageNo2] + 1);
                        }
                        else if (!dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo2))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo2, 1);
                        }
                        else
                        {
                            dictCascadingLinkageTriggerCount[lcs.LinkageNo2]++;
                        }
                    }
                    if (!lstTriggeredLinkageCode.Contains(lcs.LinkageNo3) && lcs.LinkageNo3 != "0")
                    {
                        if (dictTriggerCount.ContainsKey(lcs.LinkageNo3) && !dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo3))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo3, dictTriggerCount[lcs.LinkageNo3] + 1);
                        }
                        else if (!dictCascadingLinkageTriggerCount.ContainsKey(lcs.LinkageNo3))
                        {
                            dictCascadingLinkageTriggerCount.Add(lcs.LinkageNo3, 1);
                        }
                        else
                        {
                            dictCascadingLinkageTriggerCount[lcs.LinkageNo3]++;
                        }
                    }
                }
                else
                {
                    lstUnTriggerLinkageCode.Add(trigger.Key);
                }
            }
            if (!isTriggerCascadeingLinkage) //已经没有可联动的器件
            {
                return null;
            }
            //删除已经触发的组态
            foreach (var code in lstTriggeredLinkageCode)
            {
                dictTriggerCount.Remove(code);
            }
            ////删除未触发，且在联动组中也不存在的组号
            //foreach (var code in lstUnTriggerLinkageCode)
            //{
            //    if (!dictCascadingLinkageTriggerCount.ContainsKey(code))
            //    {
            //        dictTriggerCount.Remove(code);
            //    }
            //}

            //将未触发的组态信息补全至联动组计数中
            foreach (var trigger in dictTriggerCount)
            {
                if (!dictCascadingLinkageTriggerCount.ContainsKey(trigger.Key))
                {
                    dictCascadingLinkageTriggerCount.Add(trigger.Key, trigger.Value);
                }
            }
            

            List<string> lstTempOutputDeviceCode=null;
            lstTempOutputDeviceCode = FindAllDevicesForStandardSimulator(dictCascadingLinkageTriggerCount, standardConfig, lstTriggeredLinkageCode);

            if (lstTempOutputDeviceCode != null)
            {
                foreach (var code in lstTempOutputDeviceCode)
                {
                    if (!lstOutputDeviceCode.Contains(code))
                    {
                        lstOutputDeviceCode.Add(code);
                    }
                }            
            }
            return lstOutputDeviceCode;
        }

        #endregion

        #region 混合组态联动
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private List<string> MixedLinkageSimulator(ControllerModel controller)
        {
            //var result = ProjectManager.GetInstance.Project.Controllers.Where((c) => c.ID == ControllerID);
            //ControllerModel controller = result.FirstOrDefault();
            //取得本机所有器件
            IControllerOperation cOperation = _cManager.GetController(controller.Type);
            List<DeviceInfoForSimulator> lstSimulatorDevices = cOperation.GetSimulatorDevices(controller);
            //记录需要输出的器件编号
            List<string> lstOutputDeviceCode = new List<string>();
            //记录混合组态激活次数(分类A)
            Dictionary<string, int> mixedTypeATriggerCount = new Dictionary<string, int>();
            //记录混合组态激活次数(分类B)
            Dictionary<string, int> mixedTypeBTriggerCount = new Dictionary<string, int>();
            //记录混合组态中，与输入器件相关的所有组态编号
            List<string> mixedTriggerTotalCount = new List<string>();

            foreach (var config in controller.MixedConfig)
            {
                foreach (var r in InputDeviceInfoObservableCollection) //初始化器件信息中所有的“输出组“计数
                {
                    if(config.TypeA==LinkageType.ZoneLayer)
                    {
                        if (config.DeviceTypeCodeA == r.TypeCode)
                        {
                            if (config.BuildingNoA != null && config.ZoneNoA != null && config.LayerNoA != null)
                            {
                                if (config.BuildingNoA == r.BuildingNo && config.ZoneNoA == r.ZoneNo && config.LayerNoA == r.FloorNo)
                                {
                                    if (!mixedTypeATriggerCount.ContainsKey(config.Code))
                                    {
                                        mixedTypeATriggerCount.Add(config.Code, 1);
                                    }
                                    else
                                    {
                                        mixedTypeATriggerCount[config.Code]++;
                                    }
                                    if (!mixedTriggerTotalCount.Contains(config.Code))
                                    { 
                                        mixedTriggerTotalCount.Add(config.Code);
                                    }
                                }
                            }
                        }
                    }
                    else if (config.TypeA == LinkageType.Address)
                    {

                        if ((config.FormattedLoopNoA + config.FormattedDeviceCodeA) == r.Code.Substring(r.Code.Length - 5))
                        {
                            if (!mixedTypeATriggerCount.ContainsKey(config.Code))
                            {
                                mixedTypeATriggerCount.Add(config.Code, 1);
                            }
                            else
                            {
                                mixedTypeATriggerCount[config.Code]++;
                            }
                            if (!mixedTriggerTotalCount.Contains(config.Code))
                            {
                                mixedTriggerTotalCount.Add(config.Code);
                            }
                        }
                    }

                    if (config.TypeB == LinkageType.ZoneLayer)
                    {
                        if (config.DeviceTypeCodeB == r.TypeCode)
                        {
                            if (config.BuildingNoB != null && config.ZoneNoB != null && config.LayerNoB != null)
                            {
                                if (config.BuildingNoB == r.BuildingNo && config.ZoneNoB == r.ZoneNo && config.LayerNoB == r.FloorNo)
                                {
                                    if (!mixedTypeBTriggerCount.ContainsKey(config.Code))
                                    {
                                        mixedTypeBTriggerCount.Add(config.Code, 1);
                                    }
                                    else
                                    {
                                        mixedTypeBTriggerCount[config.Code]++;
                                    }
                                    if (!mixedTriggerTotalCount.Contains(config.Code))
                                    {
                                        mixedTriggerTotalCount.Add(config.Code);
                                    }
                                }
                            }
                        }
                    }
                    else if (config.TypeB == LinkageType.Address)
                    {

                        if ((config.FormattedLoopNoB + config.FormattedDeviceCodeB) == r.Code.Substring(r.Code.Length - 5))
                        {
                            if (!mixedTypeBTriggerCount.ContainsKey(config.Code))
                            {
                                mixedTypeBTriggerCount.Add(config.Code, 1);
                            }
                            else
                            {
                                mixedTypeBTriggerCount[config.Code]++;
                            }
                            if (!mixedTriggerTotalCount.Contains(config.Code))
                            {
                                mixedTriggerTotalCount.Add(config.Code);
                            }
                        }
                    }
                }
            }

            foreach (var code in mixedTriggerTotalCount)
            {
                LinkageConfigMixed lcm = controller.MixedConfig.Where((l) => l.Code == code).FirstOrDefault<LinkageConfigMixed>();
                int typeACount = 0;
                int typeBCount = 0;
                if (mixedTypeATriggerCount.ContainsKey(lcm.Code))
                {
                    typeACount = mixedTypeATriggerCount[lcm.Code];
                }
                if (mixedTypeBTriggerCount.ContainsKey(lcm.Code))
                {
                    typeBCount = mixedTypeBTriggerCount[lcm.Code];
                }


                if (lcm.ActionType == LinkageActionType.AND)
                {
                
                    if (lcm.ActionCoefficient != 0)
                    {                        
                        if (typeACount>= lcm.ActionCoefficient && typeBCount >=lcm.ActionCoefficient)
                        {
                            OutputMixedTypeC(lcm,ref lstOutputDeviceCode, lstSimulatorDevices);
                        }
                    }
                }
                else if (lcm.ActionType == LinkageActionType.OR)
                { 
                      if ((typeACount + typeBCount) >= lcm.ActionCoefficient)
                      {
                          OutputMixedTypeC(lcm,ref lstOutputDeviceCode, lstSimulatorDevices);
                      }
                }
            }

            return lstOutputDeviceCode;
        }
        /// <summary>
        /// 取得需要输出的器件
        /// </summary>
        /// <param name="lcm">需判断的“混合组态”信息</param>
        /// <param name="lstDevicesParam">需要输出的器件编号</param>
        /// <param name="lstSimulatorDevicesParam">当前控制器的所有器件信息集合</param>
        private void OutputMixedTypeC(LinkageConfigMixed lcm, ref List<string> lstDevicesParam,List<DeviceInfoForSimulator> lstSimulatorDevicesParam)
        {
          
                if (lcm.TypeC == LinkageType.Address)
                {
                    //分类C中的器件编号
                    string typeCDeviceCode = lcm.FormattedMachineNoC + lcm.FormattedLoopNoC + lcm.FormattedDeviceCodeC;
                    lstDevicesParam.Add(typeCDeviceCode);
                }
                else if (lcm.TypeC == LinkageType.ZoneLayer) //楼、区、层，仅可以为本机的器件，不允许为它机器件
                {
                    //在本机器件中查找是否有符合条件的器件
                    if (lcm.DeviceTypeCodeC != (int)SpecialValue.AnyAlarm) //非任意火警，需要匹配设备类型
                    {
                        List<DeviceInfoForSimulator> lstDevices = lstSimulatorDevicesParam.Where((d) => d.BuildingNo == lcm.BuildingNoC && d.ZoneNo == lcm.ZoneNoC && d.FloorNo == lcm.LayerNoC && d.TypeCode == lcm.DeviceTypeCodeC).ToList<DeviceInfoForSimulator>();
                        foreach (var device in lstDevices)
                        {
                            LinkageDeviceInfoObservableCollection.Add(device);
                        }
                    }
                    else
                    {
                        List<DeviceInfoForSimulator> lstDevices = lstSimulatorDevicesParam.Where((d) => d.BuildingNo == lcm.BuildingNoC && d.ZoneNo == lcm.ZoneNoC && d.FloorNo == lcm.LayerNoC).ToList<DeviceInfoForSimulator>();
                        foreach (var device in lstDevices)
                        {
                            LinkageDeviceInfoObservableCollection.Add(device);
                        }
                    }
                }
        }

        #endregion 

        #region 通用组态联动
        private List<string> GeneralLinkageSimulator(ControllerModel controller)
        {
            //取得本机所有器件
            IControllerOperation cOperation = _cManager.GetController(controller.Type);
            List<DeviceInfoForSimulator> lstSimulatorDevices = cOperation.GetSimulatorDevices(controller);

            //记录需要输出的器件编号
            List<string> lstOutputDeviceCode = new List<string>();
            IControllerConfig configInfo = ControllerConfigManager.GetConfigObject(controller.Type);
            List<DeviceType> lstDeviceType = configInfo.GetAllowedDeviceTypeInfoForAnyAlarm();

            //记录通用组态激活次数(任意火警)
            Dictionary<string, int> generalLinkageForAnyAlarmTriggerCount = new Dictionary<string, int>();
            //记录通用组态激活次数(指定器件类型)
            Dictionary<string, int> generalLinkageForSpecificTypeTriggerCount = new Dictionary<string, int>();

            Dictionary<string, List<DeviceInfoForSimulator>> dictTriggerFactorDevice = new Dictionary<string, List<DeviceInfoForSimulator>>();
            
            
            foreach (var config in controller.GeneralConfig)
            {
                foreach (var input in InputDeviceInfoObservableCollection) //初始化器件信息中所有的“输出组“计数
                {
                    if (input.BuildingNo != 0 && input.BuildingNo != null && input.FloorNo != 0 && input.FloorNo != null && input.ZoneNo != 0 && input.ZoneNo != null)
                    {
                        if (config.DeviceTypeCodeA ==(int)SpecialValue.AnyAlarm) //任意火警
                        {
                            if (lstDeviceType.Where((d) => d.Code == input.TypeCode).Count() > 0)//属于可以报警的器件
                            {
                                if ((input.BuildingNo == config.BuildingNoA || config.BuildingNoA == 0) && (input.ZoneNo==config.ZoneNoA || config.ZoneNoA==0) && (((input.FloorNo>=config.LayerNoA1 && input.FloorNo<=config.LayerNoA2) || (input.FloorNo<=config.LayerNoA1 && input.FloorNo>=config.LayerNoA2)) ||config.LayerNoA1==0 || config.LayerNoA2==0))
                                {
                                    if (!generalLinkageForAnyAlarmTriggerCount.ContainsKey(config.Code))
                                    {                                        
                                        generalLinkageForAnyAlarmTriggerCount.Add(config.Code, 1);

                                        //将引发此组态的因子加入集合
                                        List<DeviceInfoForSimulator> lstDeviceInfo=new List<DeviceInfoForSimulator>();
                                        lstDeviceInfo.Add(input);
                                        dictTriggerFactorDevice.Add(config.Code,lstDeviceInfo);
                                    }
                                    else
                                    {
                                        generalLinkageForAnyAlarmTriggerCount[config.Code]++;

                                        //将引发此组态的因子加入集合
                                      //  dictTriggerFactorDevice[config.Code].Add(input);
                                        //((List<DeviceInfoForSimulator>)dictTriggerFactorDevice[config.Code]).Add(input);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (config.DeviceTypeCodeA == input.TypeCode) //匹配器件类型
                            {   
                                if ((input.BuildingNo == config.BuildingNoA || config.BuildingNoA == 0) && (input.ZoneNo == config.ZoneNoA || config.ZoneNoA == 0) &&( ((input.FloorNo >= config.LayerNoA1 && input.FloorNo <= config.LayerNoA2) || (input.FloorNo <= config.LayerNoA1 && input.FloorNo >= config.LayerNoA2)) || config.LayerNoA1 == 0 || config.LayerNoA2 == 0))
                                {
                                    if (!generalLinkageForSpecificTypeTriggerCount.ContainsKey(config.Code))
                                    {
                                        generalLinkageForSpecificTypeTriggerCount.Add(config.Code, 1);

                                        //将引发此组态的因子加入集合
                                        List<DeviceInfoForSimulator> lstDeviceInfo = new List<DeviceInfoForSimulator>();
                                        lstDeviceInfo.Add(input);
                                        dictTriggerFactorDevice.Add(config.Code, lstDeviceInfo);
                                    }
                                    else
                                    {
                                        generalLinkageForSpecificTypeTriggerCount[config.Code]++;

                                        //将引发此组态的因子加入集合
                                     //   dictTriggerFactorDevice[config.Code].Add(input);
                                    }
                                }
                            }                            
                        }
                    }
                }
            }
            foreach (var r in generalLinkageForAnyAlarmTriggerCount)
            {
                LinkageConfigGeneral lcg = controller.GeneralConfig.Where((d) => d.Code == r.Key).FirstOrDefault<LinkageConfigGeneral>();                
                if (r.Value>= lcg.ActionCoefficient)
                {
                  //  if (dictTriggerFactorDevice[r.Key].Count > 0)
                  //  {
                        OutputGeneralTypeC(lcg, ref lstOutputDeviceCode, lstSimulatorDevices, true, dictTriggerFactorDevice[r.Key][0]);
                  //  }
                }
            }
            foreach (var r in generalLinkageForSpecificTypeTriggerCount)
            {
                LinkageConfigGeneral lcg = controller.GeneralConfig.Where((d) => d.Code == r.Key).FirstOrDefault<LinkageConfigGeneral>();
                if (r.Value >= lcg.ActionCoefficient)
                {
                    //if (dictTriggerFactorDevice[r.Key].Count > 0)
                    //{
                        OutputGeneralTypeC(lcg, ref lstOutputDeviceCode, lstSimulatorDevices, true, dictTriggerFactorDevice[r.Key][0]);
                   //}
                    //switch (lcg.TypeC)
                    //{
                    //    case LinkageType.ZoneLayer:
                    //        break;
                    //    case LinkageType.SameLayer:
                    //        break;
                    //    case LinkageType.AdjacentLayer:
                    //        break;
                    //    case LinkageType.Address:
                    //        break;
                    //}
                }
            }
            return lstOutputDeviceCode;
        }
        private void OutputGeneralTypeC(LinkageConfigGeneral lcg, ref List<string> lstDevicesParam, List<DeviceInfoForSimulator> lstSimulatorDevicesParam, bool anyAlarmFlag, List<DeviceInfoForSimulator> lstTriggerFactor)
        {

            List<DeviceInfoForSimulator> lstDevices;

            
            switch (lcg.TypeC)
            {              
                case LinkageType.ZoneLayer: //楼、区、层，仅可以为本机的器件，不允许为它机器件                    
                    lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, lcg.BuildingNoC, lcg.ZoneNoC, lcg.LayerNoC, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }
                    break;
                case LinkageType.SameLayer:                                   
                    {
                        List<DeviceInfoForSimulator> lstValidTriggerFactor = new List<DeviceInfoForSimulator>();
                        foreach (var triggerMinion in lstTriggerFactor)
                        {
                            int validCount = lstValidTriggerFactor.Count((d) => d.FloorNo == triggerMinion.FloorNo && d.BuildingNo == triggerMinion.BuildingNo && d.ZoneNo == triggerMinion.ZoneNo);
                            if (validCount == 0)
                            {
                                int count = lstTriggerFactor.Count((d) => d.FloorNo == triggerMinion.FloorNo && d.BuildingNo == triggerMinion.BuildingNo && d.ZoneNo == triggerMinion.ZoneNo);
                                if (count >= lcg.ActionCoefficient) //激发因子的“同层”数量>=动作常数
                                {
                                    lstValidTriggerFactor.Add(triggerMinion);
                                }
                            }
                        }
                        foreach (var validFactor in lstValidTriggerFactor)
                        {
                            lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, validFactor.FloorNo, lcg.DeviceTypeCodeC, anyAlarmFlag);

                            foreach (var device in lstDevices)
                            {
                                LinkageDeviceInfoObservableCollection.Add(device);
                            }
                        }
                    }
                    break;
                case LinkageType.AdjacentLayer:
                    {
                        List<DeviceInfoForSimulator> lstValidTriggerFactor = new List<DeviceInfoForSimulator>();
                        foreach (var triggerMinion in lstTriggerFactor)
                        {
                            int validCount = lstValidTriggerFactor.Count((d) => d.FloorNo == triggerMinion.FloorNo && d.BuildingNo == triggerMinion.BuildingNo && d.ZoneNo == triggerMinion.ZoneNo);
                            if (validCount == 0)
                            {
                                int count = lstTriggerFactor.Count((d) => d.FloorNo == triggerMinion.FloorNo && d.BuildingNo == triggerMinion.BuildingNo && d.ZoneNo == triggerMinion.ZoneNo);
                                if (count >= lcg.ActionCoefficient) //激发因子的“同层”数量>=动作常数
                                {
                                    lstValidTriggerFactor.Add(triggerMinion);
                                }
                            }
                        }
                        foreach (var validFactor in lstValidTriggerFactor)
                        {
                            lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, validFactor.FloorNo, lcg.DeviceTypeCodeC, anyAlarmFlag);
                            foreach (var device in lstDevices)
                            {
                                LinkageDeviceInfoObservableCollection.Add(device);
                            }
                            lstDevices.Clear();
                            //楼层加1逻辑， 楼层不可为0
                            if (validFactor.FloorNo == -1)
                            {
                                lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, 1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                            }
                            else
                            {
                                lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, validFactor.FloorNo + 1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                            }

                            foreach (var device in lstDevices)
                            {
                                LinkageDeviceInfoObservableCollection.Add(device);
                            }
                            lstDevices.Clear();
                            //楼层减1逻辑， 楼层不可为0
                            if (validFactor.FloorNo == 1)
                            {
                                lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, -1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                            }
                            else
                            {
                                lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, validFactor.BuildingNo, validFactor.ZoneNo, validFactor.FloorNo - 1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                            }
                            foreach (var device in lstDevices)
                            {
                                LinkageDeviceInfoObservableCollection.Add(device);
                            }
                        }            
                    }
                    break;

                case LinkageType.Address:
                    string typeCDeviceCode = lcg.FormattedMachineNoC + lcg.FormattedLoopNoC + lcg.FormattedDeviceCodeC;
                    lstDevicesParam.Add(typeCDeviceCode);

                    break;
            }
        }
        private void OutputGeneralTypeC(LinkageConfigGeneral lcg, ref List<string> lstDevicesParam, List<DeviceInfoForSimulator> lstSimulatorDevicesParam,bool anyAlarmFlag,DeviceInfoForSimulator triggerFactor)
        {

            List<DeviceInfoForSimulator> lstDevices;
            switch (lcg.TypeC)
            {
                case LinkageType.ZoneLayer: //楼、区、层，仅可以为本机的器件，不允许为它机器件                    
                    lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam,lcg.BuildingNoC, lcg.ZoneNoC, lcg.LayerNoC, lcg.DeviceTypeCodeC, anyAlarmFlag);                            
                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }              
                    break;
                case LinkageType.SameLayer:        
            
                    //if(lstTriggerFactor>0 )
                    lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, triggerFactor.FloorNo, lcg.DeviceTypeCodeC, anyAlarmFlag);

                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }              
                    break;


                    //foreach (var s in InputDeviceInfoObservableCollection)
                    //{ 
                        
                    //}


                    //不想再增加用来传递输入器件的函数参数-->用循环来取得器件层位信息
                    //if (lcg.LayerNoA1 <= lcg.LayerNoA2)
                    //{
                    //    for (int? i = lcg.LayerNoA1; i <= lcg.LayerNoA2; i++)
                    //    {                            
                    //        //DeviceInfoForSimulator inputDevice = InputDeviceInfoObservableCollection.Where((d) => d.FloorNo == i && d.BuildingNo == lcg.BuildingNoA && d.ZoneNo == lcg.ZoneNoA).FirstOrDefault<DeviceInfoForSimulator>();
                    //        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, lcg.BuildingNoA, lcg.ZoneNoA, i, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    //        foreach (var device in lstDevices)
                    //        {
                    //            LinkageDeviceInfoObservableCollection.Add(device);
                    //        }  
                    //    }
                    //}
                    //else
                    //{
                    //    for (int? i = lcg.LayerNoA2; i <= lcg.LayerNoA1; i++)
                    //    {
                    //      //  DeviceInfoForSimulator inputDevice = InputDeviceInfoObservableCollection.Where((d) => d.FloorNo == i && d.BuildingNo == lcg.BuildingNoA && d.ZoneNo == lcg.ZoneNoA).FirstOrDefault<DeviceInfoForSimulator>();
                    //        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, lcg.BuildingNoA, lcg.ZoneNoA, i, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    //        foreach (var device in lstDevices)
                    //        {
                    //            LinkageDeviceInfoObservableCollection.Add(device);
                    //        }  
                    //    }
                    //}          
                    
                case LinkageType.AdjacentLayer:
                    lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, triggerFactor.FloorNo, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }
                    lstDevices.Clear();
                    //楼层加1逻辑， 楼层不可为0
                    if (triggerFactor.FloorNo == -1)
                    {
                        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, 1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    }
                    else
                    {
                        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, triggerFactor.FloorNo+1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    }

                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }
                    lstDevices.Clear();
                    //楼层减1逻辑， 楼层不可为0
                    if (triggerFactor.FloorNo == 1)
                    {
                        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, -1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    }
                    else
                    {
                        lstDevices = ProcessGeneralLinkageQueryCondition(lstSimulatorDevicesParam, triggerFactor.BuildingNo, triggerFactor.ZoneNo, triggerFactor.FloorNo - 1, lcg.DeviceTypeCodeC, anyAlarmFlag);
                    }
                    foreach (var device in lstDevices)
                    {
                        LinkageDeviceInfoObservableCollection.Add(device);
                    }
                    break;
                    
                case LinkageType.Address:
                    string typeCDeviceCode = lcg.FormattedMachineNoC + lcg.FormattedLoopNoC + lcg.FormattedDeviceCodeC;
                    lstDevicesParam.Add(typeCDeviceCode);

                    break;
            }
        }

        private List<DeviceInfoForSimulator> ProcessGeneralLinkageQueryCondition(List<DeviceInfoForSimulator> lstDeviceInfo,int? buildingNo, int? zoneNo, int? floorNo,short typeCode,bool anyAlarmFlag)
        {
            Dictionary<QueryAnyFloorLinkageType, System.Func<List<DeviceInfoForSimulator>, int?, int?, int?, short,bool, List<DeviceInfoForSimulator>>> dictMap = new Dictionary<QueryAnyFloorLinkageType, System.Func< List<DeviceInfoForSimulator>, int?, int?, int?, short,bool, List<DeviceInfoForSimulator>>>();
            dictMap.Add(QueryAnyFloorLinkageType.QueryByBuildingNo, QueryByBuildingNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByBuildingAndZoneAndFloor, QueryByBuildingAndZoneAndFloor);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByBuildingNoAndFloorNo, QueryByBuildingNoAndFloorNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByBuildingNoAndZoneNo, QueryByBuildingNoAndZoneNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByFloorNo, QueryByFloorNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByZoneNo, QueryByZoneNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByZoneNoAndFloorNo, QueryByZoneNoAndFloorNo);
            dictMap.Add(QueryAnyFloorLinkageType.QueryByNothing, QueryByNothing);
            System.Func<List<DeviceInfoForSimulator>, int?, int?, int?,short, bool, List<DeviceInfoForSimulator>> execMethod;
            dictMap.TryGetValue(GetQueryAnyFloorLinkageType(buildingNo, zoneNo, floorNo), out execMethod);
            return execMethod(lstDeviceInfo,buildingNo,zoneNo,floorNo,typeCode,anyAlarmFlag);
        }
        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="buildingNo"></param>
        /// <param name="zoneNo"></param>
        /// <param name="floorNo"></param>
        /// <returns></returns>
        private QueryAnyFloorLinkageType GetQueryAnyFloorLinkageType(int? buildingNo, int? zoneNo, int? floorNo)
        {
            QueryAnyFloorLinkageType result = QueryAnyFloorLinkageType.QueryByNothing;
            if (buildingNo!= 0 && zoneNo != 0 && floorNo != 0)
            {
                result = QueryAnyFloorLinkageType.QueryByBuildingAndZoneAndFloor;
            }
            else if (buildingNo != 0 && zoneNo == 0 && floorNo == 0)
            {
                result = QueryAnyFloorLinkageType.QueryByBuildingNo;
            }
            else if (buildingNo !=0 && zoneNo != 0 && floorNo == 0)
            {
                result = QueryAnyFloorLinkageType.QueryByBuildingNoAndZoneNo;
            }
            else if (buildingNo == 0 && zoneNo == 0 && floorNo != 0)
            {
                result = QueryAnyFloorLinkageType.QueryByFloorNo;
            }
            else if (buildingNo == 0 && zoneNo != 0 && floorNo == 0)
            {
                result = QueryAnyFloorLinkageType.QueryByZoneNo;
            }
            else if (buildingNo != 0 && zoneNo == 0 && floorNo != 0)
            {
                result = QueryAnyFloorLinkageType.QueryByBuildingNoAndFloorNo;
            }
            else if (buildingNo == 0 && zoneNo != 0 && floorNo != 0)
            {
                result = QueryAnyFloorLinkageType.QueryByZoneNoAndFloorNo;
            }
            else if (buildingNo == 0 && zoneNo == 0 && floorNo == 0)
            {
                result = QueryAnyFloorLinkageType.QueryByNothing;
            }
            return result;
        }
        private List<DeviceInfoForSimulator> QueryByBuildingAndZoneAndFloor(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator,int? buildingNo,int? zoneNo, int? floorNo,short typeCode,bool anyAlarmFlag)
        {
            if (anyAlarmFlag)
            {
                List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == buildingNo && d.ZoneNo == zoneNo && d.FloorNo == floorNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
                return lstQueryDevice;
            }
            else
            {
                List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == buildingNo && d.ZoneNo == zoneNo && d.FloorNo == floorNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
                return lstQueryDevice;            
            }            
        }
        private List<DeviceInfoForSimulator> QueryByBuildingNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == buildingNo && d.TypeCode==typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByBuildingNoAndFloorNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == buildingNo && d.FloorNo==floorNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByBuildingNoAndZoneNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.BuildingNo == buildingNo && d.ZoneNo == zoneNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByFloorNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.FloorNo == floorNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByZoneNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.ZoneNo== zoneNo && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByZoneNoAndFloorNo(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.ZoneNo == zoneNo && d.FloorNo==floorNo  && d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        private List<DeviceInfoForSimulator> QueryByNothing(List<DeviceInfoForSimulator> lstDeviceInfoForSimulator, int? buildingNo, int? zoneNo, int? floorNo, short typeCode, bool anyAlarmFlag)
        {
            List<DeviceInfoForSimulator> lstQueryDevice = lstDeviceInfoForSimulator.Where((d) => d.TypeCode == typeCode).ToList<DeviceInfoForSimulator>();
            return lstQueryDevice;
        }
        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="buildingNo"></param>
        /// <param name="zoneNo"></param>
        /// <param name="floorNo"></param>
        /// <returns></returns>
        //private QueryAnyFloorLinkageType GetQueryAnyFloorLinkageType(int? buildingNo, int? zoneNo, int? floorNo)
        //{
        //    QueryAnyFloorLinkageType result = QueryAnyFloorLinkageType.QueryByBuildingNo;
        //    if (buildingNo != 0 && zoneNo != 0 && floorNo != 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByBuildingAndZoneAndFloor;

        //    }
        //    else if (buildingNo != 0 && zoneNo == 0 && floorNo == 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByBuildingNo;
        //    }
        //    else if (buildingNo == 0 && zoneNo != 0 && floorNo == 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByZoneNo;
        //    }
        //    else if (buildingNo == 0 && zoneNo == 0 && floorNo != 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByFloorNo;
        //    }
        //    else if (buildingNo != 0 && zoneNo != 0 && floorNo == 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByBuildingNoAndZoneNo;
        //    }
        //    else if (buildingNo != 0 && zoneNo == 0 && floorNo != 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByBuildingNoAndFloorNo;
        //    }
        //    else if (buildingNo == 0 && zoneNo != 0 && floorNo != 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByZoneNoAndFloorNo;
        //    }
        //    else if (buildingNo == 0 && zoneNo == 0 && floorNo == 0)
        //    {
        //        result = QueryAnyFloorLinkageType.QueryByNothing;
        //    }
        //    return result;
        //}    



        #endregion
        public void GenerateSummaryInfo()
        {
            SummaryNodes.Clear();
            if (TheProject != null)
            {
                SummaryInfo summary = new SummaryInfo();
                summary.Icon = @"Resources\Icon\Style1\proj.jpg"; ;
                summary.Name = "工程";// +TheController.Name + "(" + TheController.Type.ToString() + "," + TheController.DeviceAddressLength.ToString() + ")";
                summary.Number = 1;
                summary.Level = 1;
                ControllerManager controllerManager = new ControllerManager();
                controllerManager.InitializeAllControllerOperation(null);                
                foreach (var controller in TheProject.Controllers)
                {
                    IControllerOperation controllerOperation = controllerManager.GetController(controller.Type);                        
                    SummaryInfo controllerSummary=controllerOperation.GetSummaryNodes(controller,2);
                    summary.ChildNodes.Add(controllerSummary);
                }
                SummaryNodes.Add(summary);
            }
            
        }
       
    }
    /// <summary>
    /// 输入器件查询类型
    /// </summary>
    enum QueryType
    { 
        QueryByLoop=0,
        QueryByBuildingNo = 1,
        QueryByType = 2,
        QueryByLoopAndBuildingNo=3,
        QueryByLoopAndType = 4,
        QueryByBuildingNoAndType = 5,        
        QueryByLoopAndBuildingNoAndType = 6,
        QueryByNothing=7,        
    }
    /// <summary>
    /// 任意楼，区，层查询枚举
    /// </summary>
    enum QueryAnyFloorLinkageType
    { 
        QueryByBuildingNo=0,
        QueryByZoneNo=1,
        QueryByFloorNo=2,
        QueryByBuildingNoAndZoneNo=3,
        QueryByBuildingNoAndFloorNo=4,
        QueryByZoneNoAndFloorNo=5,
        QueryByBuildingAndZoneAndFloor=6,
        QueryByNothing=7,
    }
}
