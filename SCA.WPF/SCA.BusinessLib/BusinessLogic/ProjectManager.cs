using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using SCA.BusinessLib.BusinessLogic;
namespace SCA.BusinessLib
{
    public class ProjectManager:SCA.Interface.IProjectManager
    {

        private static ProjectManager _projManager;
        private static object syncRoot = new object();        
        //是否存在未保存的数据
        /// <summary>
        /// true: 有修改过的数据需要保存
        /// false:不需要保存数据
        /// </summary>
        public bool IsDirty { get; set; }
        //protected IProjectService _projectService;
        //protected IFileService _fileService;
        protected string _dirPath;
        public bool IsCreatedFundamentalTableStructure { get; set; }
        //private List<ControllerModel> _lstControllers;
        public ProjectModel Project { get { return _project; } private set { _project = value;  } }
        private ProjectModel _project;
        private IDeviceTypeDBService _deviceTypeDBService;
        private IProjectDBService _projectDBService;
        private IControllerDBService _controllerDBService;
        private ILoopDBService _loopDBService;
        private IDeviceDBServiceTest _deviceDBService;
        private ILinkageConfigStandardDBService _linkageConfigStandardDBService;
        private ILinkageConfigGeneralDBService _linkageConfigGeneralDBService;
        private ILinkageConfigMixedDBService _linkageConfigMixedDBService;
        private IManualControlBoardDBService _manualControlBoardDBService;
        private IDatabaseService _databaseService;
        private IFileService _fileService;
        public static  ProjectManager GetInstance
        {
            get
            {
                //return _projManager ?? (_projManager = new ProjectManager());
                if (_projManager == null)
                {
                    lock (syncRoot)
                    {
                        if(_projManager==null)
                        { 
                            _projManager = new ProjectManager();
                        }
                    }
                    
                }
                return _projManager;
            }
        }
        //本项目中8007控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8007 = 0;
        public int MaxDeviceIDInController8007 { get { return _maxDeviceIDInController8007; } set { _maxDeviceIDInController8007 = value; } }

        //本项目中8001控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8001 = 0;
        public int MaxDeviceIDInController8001 { get { return _maxDeviceIDInController8001; } set { _maxDeviceIDInController8001 = value; } }

        //本项目中8036控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8036 = 0;
        public int MaxDeviceIDInController8036 { get { return _maxDeviceIDInController8036; } set { _maxDeviceIDInController8036 = value; } }

        //本项目中8003控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8003 = 0;
        public int MaxDeviceIDInController8003 { get { return _maxDeviceIDInController8003; } set { _maxDeviceIDInController8003 = value; } }

        //本项目中8021控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8021 = 0;
        public int MaxDeviceIDInController8021 { get { return _maxDeviceIDInController8021; } set { _maxDeviceIDInController8021 = value; } }

        //本项目中8000控制器的器件信息的最大ID编号
        private int _maxDeviceIDInController8000 = 0;
        public int MaxDeviceIDInController8000 { get { return _maxDeviceIDInController8000; } set { _maxDeviceIDInController8000 = value; } }


        //本项目中标准组态信息的最大ID编号
        private int _maxIDForStandardLinkageConfig = 0;
        public int MaxIDForStandardLinkageConfig { get { return _maxIDForStandardLinkageConfig; } set { _maxIDForStandardLinkageConfig = value; } }

        //本项目中混合组态信息的最大ID编号
        private int _maxIDForMixedLinkageConfig = 0;
        public int MaxIDForMixedLinkageConfig { get { return _maxIDForMixedLinkageConfig; } set { _maxIDForMixedLinkageConfig = value; } }

        //本项目中通用组态信息的最大ID编号
        private int _maxIDForGeneralLinkageConfig = 0;
        public int MaxIDForGeneralLinkageConfig { get { return _maxIDForGeneralLinkageConfig; } set { _maxIDForGeneralLinkageConfig = value; } }

        private int _maxIDForManualControlBoard = 0;
        public int MaxIDForManualControlBoard { get { return _maxIDForManualControlBoard; } set { _maxIDForManualControlBoard = value; } }

        #region 控制器操作
        //public List<ControllerModel> Controllers
        //{
        //    get
        //    {
        //        if (_lstControllers == null)
        //        {
        //            _lstControllers = new List<ControllerModel>();
        //        }
        //        return _lstControllers;
        //    }
        //    private set
        //    {
        //        if (_lstControllers == null)
        //        {
        //            _lstControllers = new List<ControllerModel>();
        //        }
        //        else
        //        {
        //            _lstControllers = value;
        //        }
        //    }
        //}
        /// <summary>
        /// 为当前项目添加控制器信息
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool AddControllers(ControllerModel controller)
        {
            try
            {
                Project.Controllers.Add(controller);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public ControllerModel GetControllerBySpecificID(int id)
        {
            if (Project.Controllers != null)
            {
                var result = from c in Project.Controllers where c.ID == id select c;
                return result.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新controller信息  实体类为引用类型，不需要更新
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        //public bool UpdateControllerBySpecificController(ControllerModel controller)
        //{
        //    try
        //    {
        //        for (int i = 0; i < Project.Controllers.Count; i++)
        //        {
        //            if (Project.Controllers[i].ID == controller.ID && Project.Controllers[i].Name == controller.Name)
        //            {
        //                Project.Controllers[i] = controller;
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}
        public bool SetControllerBySpecificName(string name)
        {
            try
            {                
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        
        #endregion 
        /// <summary>
        /// 默认采用SQLite数据操作类
        /// </summary>
        private ProjectManager()
        {
            //_projectService = projectService;     
        }
        private void InitializeDefaultSetting()
        {
            _fileService = new SCA.BusinessLib.Utility.FileService();
            ILogRecorder logger = null;            
            _projectDBService = new SCA.DatabaseAccess.DBContext.ProjectDBService(_databaseService);
            _controllerDBService = new SCA.DatabaseAccess.DBContext.ControllerDBService(_databaseService);
            _loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(_databaseService);
            _linkageConfigStandardDBService = new SCA.DatabaseAccess.DBContext.LinkageConfigStandardDBService(_databaseService);
            _linkageConfigGeneralDBService = new SCA.DatabaseAccess.DBContext.LinkageConfigGeneralDBService(_databaseService);
            _linkageConfigMixedDBService = new SCA.DatabaseAccess.DBContext.LinkageConfigMixedDBService(_databaseService);
            _manualControlBoardDBService = new SCA.DatabaseAccess.DBContext.ManualControlBoardDBService(_databaseService);
            _deviceTypeDBService = new SCA.DatabaseAccess.DBContext.DeviceTypeDBService(_databaseService);
        }
        public IProjectDBService ProjectDBService
        {
            get { return _projectDBService; }
            set { _projectDBService = value; }
        }
        public IControllerDBService ControllerDBService
        {
           get { return _controllerDBService;}
           set { _controllerDBService=value;}
        }
        public ILoopDBService LoopDBService
        {
            get { return _loopDBService; }
            set { _loopDBService = value; }
        }
        public ILinkageConfigStandardDBService LinkageConfigStandardDBService
        {
            get { return _linkageConfigStandardDBService; }
            set { _linkageConfigStandardDBService = value; }
        }
        public ILinkageConfigGeneralDBService LinkageConfigGeneralDBService
        {
            get { return _linkageConfigGeneralDBService; }
            set { _linkageConfigGeneralDBService = value; }
        }
        public ILinkageConfigMixedDBService LinkageConfigMixedDBService
        {
            get { return _linkageConfigMixedDBService; }
            set { _linkageConfigMixedDBService = value; }
        }
        public IManualControlBoardDBService ManualControlBoardDBService
        {
            get { return _manualControlBoardDBService; }
            set { _manualControlBoardDBService = value; }
        }

        public IDeviceTypeDBService DeviceTypeDBService
        {
            get { return _deviceTypeDBService; }
            set { _deviceTypeDBService = value; }
        }

        /// <summary>
        /// 创建Project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Model.ProjectModel CreateProject(ProjectModel project,IProjectService projectService,IFileService fileService)
        {
            if (projectService.CreateProject(project, fileService))
            { 
                Project = project; //将对象信息赋给当前属性
                _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(Project.SaveFilePath, null, fileService); ;
                InitializeDefaultSetting();
            }
          //  IsDirty = true;
            return Project;
        }

        public void OpenProject(string strPath)
        {
            ILogRecorder logger = null;
            _fileService = new SCA.BusinessLib.Utility.FileService();
            _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(strPath, logger, _fileService);
            InitializeDefaultSetting();
            
            IDeviceDBServiceTest deviceDBService;
            ProjectModel project=_projectDBService.GetProject(1);
            List<ControllerModel> lstControllers = _controllerDBService.GetControllersByProject(project);
            foreach (ControllerModel c in lstControllers)
            {   
                List<LoopModel> lstLoops=_loopDBService.GetLoopsByController(c);
                foreach (LoopModel l in lstLoops)
                {
                    deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(c.Type, _databaseService);
                    c.Loops.Add(deviceDBService.GetDevicesByLoop(l));
                
                }
                List<LinkageConfigStandard> lstStandards = _linkageConfigStandardDBService.GetStandardLinkageConfigInfoByController(c);
                foreach(var s in lstStandards)
                {
                    c.StandardConfig.Add(s);
                }
                List<LinkageConfigMixed> lstMixed=_linkageConfigMixedDBService.GetMixedLinkageConfigInfo(c);
                foreach (var s in lstMixed)
                {
                    c.MixedConfig.Add(s);
                }
                List<LinkageConfigGeneral> lstGeneral = _linkageConfigGeneralDBService.GetGeneralLinkageConfigInfo(c);
                foreach (var s in lstGeneral)
                {
                    c.GeneralConfig.Add(s);
                }
                project.Controllers.Add(c);
            }

        
        //private IManualControlBoardDBService _manualControlBoardDBService;


            project.SavePath = strPath;
            this.Project = project;            
        }

        public bool CloseProject()
        {
            try
            {
                if (this.Project != null) //此处待确认
                { 
                    this.Project = null;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public ProjectModel CreateProject(string projectName, string strPath)
        {
            throw new NotImplementedException();
        }





        public List<ControllerModel> Controllers
        {
            get 
            {
                return this.Project.Controllers;
            }
        }


        public SummaryNodeInfo DisplaySummaryInfo(ProjectModel projectModel)
        {
            SummaryNodeInfo rootNode = new SummaryNodeInfo();
            rootNode.NodeLevel = 0;
            rootNode.DisplayName = "不需显示的根结点";
            rootNode.ParentNode = null; 

            SummaryNodeInfo projectNode = new SummaryNodeInfo();
            projectNode.DisplayName = projectModel.Name;
            projectNode.NodeLevel = 1;
            projectNode.ParentNode = rootNode;
         //   projectNode.ChildNode = null;
            projectNode.Amount = 1;
            projectNode.OrderNumber = 0;

            Dictionary<string, int> nodeAmount;
            nodeAmount=InitializeSummaryNode(ref projectNode, null);
            

            if (projectModel.Controllers.Count > 0)
            {
                int i = 0;
                SummaryNodeInfo controllerNode;
                foreach (var c in projectModel.Controllers)
                {   
                    controllerNode = new SummaryNodeInfo();
                    i++;
                    controllerNode.DisplayName = c.Name;
                    controllerNode.NodeLevel = 1;
                    controllerNode.ParentNode = rootNode;
                 //   controllerNode.ChildNode = null;
                    controllerNode.Amount = 1;
                    controllerNode.OrderNumber = i;
                    Dictionary<string, int> cNodeAmount;
                    cNodeAmount=InitializeSummaryNode(ref controllerNode,c);
                    controllerNode.NodeAmount = cNodeAmount;
                    AccumulateNodeAmount(ref nodeAmount, cNodeAmount);
                    rootNode.ChildNodes.Add(controllerNode);
                }            
            }
            projectNode.NodeAmount = nodeAmount;
            rootNode.ChildNodes.Add(projectNode);
            return rootNode;
        }
        /// <summary>
        /// 累加节点数量信息
        /// </summary>
        /// <param name="dictAccumulatedNodeAmount"></param>
        /// <param name="dictNodeAmount"></param>
        private void AccumulateNodeAmount(ref Dictionary<string, int> dictAccumulatedNodeAmount,Dictionary<string,int> dictNodeAmount)
        {
            dictAccumulatedNodeAmount[NodeCategory.回路.ToString()] = dictAccumulatedNodeAmount[NodeCategory.回路.ToString()] + dictNodeAmount[NodeCategory.回路.ToString()];
            dictAccumulatedNodeAmount[NodeCategory.器件数量.ToString()] = dictAccumulatedNodeAmount[NodeCategory.器件数量.ToString()] + dictNodeAmount[NodeCategory.器件数量.ToString()];
            dictAccumulatedNodeAmount[NodeCategory.标准组态.ToString()] = dictAccumulatedNodeAmount[NodeCategory.标准组态.ToString()] + dictNodeAmount[NodeCategory.标准组态.ToString()];
            dictAccumulatedNodeAmount[NodeCategory.混合组态.ToString()] = dictAccumulatedNodeAmount[NodeCategory.混合组态.ToString()] + dictNodeAmount[NodeCategory.混合组态.ToString()];
            dictAccumulatedNodeAmount[NodeCategory.通用组态.ToString()] = dictAccumulatedNodeAmount[NodeCategory.通用组态.ToString()] + dictNodeAmount[NodeCategory.通用组态.ToString()];
            dictAccumulatedNodeAmount[NodeCategory.网络手控盘.ToString()] = dictAccumulatedNodeAmount[NodeCategory.网络手控盘.ToString()] + dictNodeAmount[NodeCategory.网络手控盘.ToString()];
        }
        /// <summary>
        /// 初始化各节点的数量信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, int> InitializeNodeAmount()
        {
            Dictionary<string, int> dictNodeAmount = new Dictionary<string, int>();
            dictNodeAmount.Add(NodeCategory.回路.ToString(), 0);
            dictNodeAmount.Add(NodeCategory.器件数量.ToString(), 0);
            dictNodeAmount.Add(NodeCategory.标准组态.ToString(), 0);
            dictNodeAmount.Add(NodeCategory.混合组态.ToString(), 0);
            dictNodeAmount.Add(NodeCategory.通用组态.ToString(), 0);
            dictNodeAmount.Add(NodeCategory.网络手控盘.ToString(), 0);

            return dictNodeAmount;
        }
        /// <summary>
        /// 初始始化基本节点信息 
        /// </summary>
        /// <param name="parentNode"></param>
        private Dictionary<string,int> InitializeSummaryNode(ref SummaryNodeInfo parentNode,ControllerModel controller)
        {
            Dictionary<string, int> nodeAmount = InitializeNodeAmount();
            int i = 0;
            SummaryNodeInfo loopNode;
            loopNode = new SummaryNodeInfo();
            i++;
            loopNode.DisplayName = NodeCategory.回路.ToString();
            loopNode.NodeLevel = 2;
            loopNode.ParentNode = parentNode;
            //  loopNode.ChildNode = null;
            if(controller !=null )
            { 
                loopNode.Amount = controller.Loops.Count;
                nodeAmount[NodeCategory.回路.ToString()] = controller.Loops.Count;
            }
            loopNode.OrderNumber = i;
            parentNode.ChildNodes.Add(loopNode);
            
            //此处简化业务逻辑，等丰富
            SummaryNodeInfo deviceInfoNode;
            deviceInfoNode = new SummaryNodeInfo();
            i++;
            deviceInfoNode.DisplayName = NodeCategory.器件数量.ToString();
            deviceInfoNode.NodeLevel = 2;
            deviceInfoNode.ParentNode = parentNode;
            deviceInfoNode.Amount = 0;
            if (controller != null)
            {
                foreach (var l in controller.Loops)
                {   //  loopNode.ChildNode = null;
                    deviceInfoNode.Amount += l.DeviceAmount;
                }
            }
            nodeAmount[NodeCategory.器件数量.ToString()] = deviceInfoNode.Amount;
            deviceInfoNode.OrderNumber = i;
            //System.Collections.Generic.Dictionary<int, int> deviceType = new Dictionary<int, int>();
            //deviceType.Add()
            parentNode.ChildNodes.Add(deviceInfoNode);
            SummaryNodeInfo standardLinkageNode;
            standardLinkageNode = new SummaryNodeInfo();
            i++;
            standardLinkageNode.DisplayName =NodeCategory.标准组态.ToString();
            standardLinkageNode.NodeLevel = 2;
            standardLinkageNode.ParentNode = parentNode;            
            standardLinkageNode.Amount = 0;
            nodeAmount[NodeCategory.标准组态.ToString()] = standardLinkageNode.Amount;
            standardLinkageNode.OrderNumber = i;
            parentNode.ChildNodes.Add(standardLinkageNode);


            SummaryNodeInfo mixedLinkageNode;
            mixedLinkageNode = new SummaryNodeInfo();
            i++;
            mixedLinkageNode.DisplayName = NodeCategory.混合组态.ToString();
            mixedLinkageNode.NodeLevel = 2;
            mixedLinkageNode.ParentNode = parentNode;
            //  loopNode.ChildNode = null;
            mixedLinkageNode.Amount = 0;
            nodeAmount[NodeCategory.混合组态.ToString()] = mixedLinkageNode.Amount;
            mixedLinkageNode.OrderNumber = i;
            parentNode.ChildNodes.Add(mixedLinkageNode);

            SummaryNodeInfo generalLinkageNode;
            generalLinkageNode = new SummaryNodeInfo();
            i++;
            generalLinkageNode.DisplayName =NodeCategory.通用组态.ToString();
            generalLinkageNode.NodeLevel = 2;
            generalLinkageNode.ParentNode = parentNode;
            //  loopNode.ChildNode = null;
            generalLinkageNode.Amount = 0;
            nodeAmount[NodeCategory.通用组态.ToString()] = generalLinkageNode.Amount;
            generalLinkageNode.OrderNumber = i;
            parentNode.ChildNodes.Add(generalLinkageNode);

            SummaryNodeInfo mannualBoardNode;
            mannualBoardNode = new SummaryNodeInfo();
            i++;
            mannualBoardNode.DisplayName =NodeCategory.网络手控盘.ToString();
            mannualBoardNode.NodeLevel = 2;
            mannualBoardNode.ParentNode = parentNode;
            //  loopNode.ChildNode = null;
            mannualBoardNode.Amount = 0;
            nodeAmount[NodeCategory.网络手控盘.ToString()] = mannualBoardNode.Amount;
            mannualBoardNode.OrderNumber = i;
            parentNode.ChildNodes.Add(mannualBoardNode);
            return nodeAmount;
        }



        public ProjectModel CreateProject(ProjectModel project)
        {
             IProjectService _projectService=new SCA.BusinessLib.BusinessLogic.ProjectService();
             IFileService _fileService=new SCA.BusinessLib.Utility.FileService();
            return    CreateProject(project, _projectService, _fileService);

        }

        /// <summary>
        /// 取得当前项目中的主控制器信息
        /// </summary>
        /// <returns></returns>
        public ControllerModel GetPrimaryController()
        {                
            var result = from c in Project.Controllers where c.PrimaryFlag == true select c;
            return result.First();
        }


        public void SetPrimaryControllerByID(int controllerID)
        {
            var result = from c in Project.Controllers where c.ID == controllerID select c;
            result.First().PrimaryFlag=true;
            result = from c in Project.Controllers where c.ID != controllerID select c;
            foreach (var s in result)
            {
                s.PrimaryFlag = false;
            }
        }
        public void SetSaveInterval(int value)
        {
            this.Project.SaveInterval = value;
            this.IsDirty = true;
        }
        /// <summary>
        /// 自动保存 2017-05-02添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoAutoSave(object sender, EventArgs e)
        {
            SaveProject();
        }
        public void SaveProject()
        {
            if (this.Project.IsDirty)
            {
                if (!_fileService.IsExistFile(this.Project.SaveFilePath)) //数据文件不存在
                {
                    _projectDBService.CreateLocalDBFile();
                }
                if (!IsCreatedFundamentalTableStructure) //未建立项目基础表结构
                {
                    if (_projectDBService != null)
                    {
                        _projectDBService.CreatFundamentalTableStructure();  //创建基础存储结构
                    }
                }
                //保存工程信息
                _projectDBService.AddProject(this.Project);
                //初始化“控制器类型信息”
                _projectDBService.InitializeControllerTypeInfo();

                //初始化此工程下的“器件类型”
                int projectID=_projectDBService.GetMaxID();
                IControllerConfig controllerConfig =  ControllerConfigManager.GetConfigObject(ControllerType.NONE);               
                
                _deviceTypeDBService.InitializeDeviceTypeInfo(controllerConfig.GetALLDeviceTypeInfo(projectID));
                
                //保存此工程下的回路，器件，组态，手动盘
                foreach (var c in this.Project.Controllers)
                {
                    _deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(c.Type, _databaseService);
                    c.Project.ID = projectID;
                    c.ProjectID = projectID;
                    _controllerDBService.AddController(c);

                    //更新器件类型表的“匹配控制器信息”
                    controllerConfig = ControllerConfigManager.GetConfigObject(c.Type);                    
                    _deviceTypeDBService.UpdateMatchingController(c.Type,controllerConfig.GetDeviceTypeCodeInfo());

                    //取得当前ControllerID
                   // int controllerID = _controllerDBService.GetMaxID();
                    _deviceDBService.CreateTableStructure();
                    foreach (var loop in c.Loops)
                    {
                        loop.Controller.ID = c.ID;
                        loop.ControllerID = c.ID;
                        _loopDBService.AddLoopInfo(loop);
                        //取得当前LoopID
                      //  int loopID = _loopDBService.GetMaxID();
                        loop.ID = loop.ID;                        
                        _deviceDBService.AddDevice(loop);                        
                        //取得当前器件最大ID号
                        //int deviceID=_deviceDBService.GetMaxID();
                    }
                    if (c.StandardConfig.Count != 0)
                    {
                        _linkageConfigStandardDBService.AddStandardLinkageConfigInfo(c.StandardConfig);
                    }
                    if (c.GeneralConfig.Count != 0)
                    {
                        _linkageConfigGeneralDBService.AddGeneralLinkageConfigInfo(c.GeneralConfig);
                    }
                    if (c.MixedConfig.Count!=0)
                    {
                        _linkageConfigMixedDBService.AddMixedLinkageConfigInfo(c.MixedConfig);
                    }
                    if (c.ControlBoard.Count!=0)
                    {
                        _manualControlBoardDBService.AddManualControlBoardInfo(c.ControlBoard);
                    }
                }
            }
            Project.IsDirty = false;
        }
    }
}
