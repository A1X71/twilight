using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLib;
using SCA.BusinessLib.Utility;
using SCA.Model;
using SCA.BusinessLib.BusinessLogic;
using SCA.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/13 11:17:08
* FileName   : TestProjectOperation
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestProjectOperation
    {
        IProjectManager _projManager;
        IProjectService _projService;

        IFileService _fileService;
        IDatabaseService _databaseService;
        ILogRecorder _logRecorder;

        [OneTimeSetUp]
        public void Initialize()
        {
            _fileService = new FileService();
            _projManager = ProjectManager.GetInstance ;

        }
        [Test]
        public void CreateProject()
        {
            
            ProjectModel pModel = new TestAssistant.ProjectBuilder().Build(); 
            _projManager.CreateProject( pModel);
            Assert.That(_projManager.Project.ID, Is.EqualTo(1));
            Assert.That(_projManager.Project.Name, Is.EqualTo(pModel.Name));
            Assert.That(_projManager.Project.SavePath, Is.EqualTo(pModel.SavePath));
            Assert.That(_projManager.Project.SaveInterval, Is.EqualTo(30));
        }
        [Test]
        public void SaveProject()
        {

            ProjectModel pModel = new TestAssistant.ProjectBuilder().Build();
            _projManager.CreateProject(pModel);
            bool blnFileExists = _fileService.IsExistFile(_projManager.Project.SavePath);
            Assert.That(blnFileExists, Is.EqualTo(false), "文件不应该存在");
            _databaseService = new SQLiteDatabaseAccess(_projManager.Project.SavePath, _logRecorder, _fileService);
            _projService = new ProjectService();
            bool blnResult = _projService.CreateProject(_projManager.Project,null);
            _projService.Dispose();
            blnFileExists = _fileService.IsExistFile(_projManager.Project.SavePath);
            Assert.That(blnFileExists, Is.EqualTo(true), "文件应该存在");
            _fileService.DeleteFile(_projManager.Project.SavePath);
        }
        protected void CreateProjectfile()
        { 
            //ProjectModel pModel = new TestAssistant.ProjectBuilder().Build();
            //_projManager.Project = pModel;
            //bool blnFileExists = _fileService.IsExistFile(_projManager.Project.SavePath);
            
            //_databaseService = new SQLiteDatabaseAccess(_projManager.Project.SavePath, _logRecorder, _fileService);
            //_projService = new ProjectService(_databaseService);
            //bool blnResult = _projService.CreateProject(_projManager.Project);
            //_projService.Dispose();
            //blnFileExists = _fileService.IsExistFile(_projManager.Project.SavePath);
                      
        }
        [Test]
        public void DisplayProjectInfo()
        {
            List<LoopModel> lstLoop = new List<LoopModel>();
            lstLoop.Add(new TestAssistant.LoopBuilder<DeviceInfo8036>().WithControllerID(1).WithDeviceAmount(1)
                           .WithDevice(new TestAssistant.DeviceBuilder8036().WithLoopID(1).Builder()).Build());

            List<ControllerModel> lstController = new List<ControllerModel>();
            lstController.Add(new TestAssistant.ControllerBuilder().WithProjectID(1).WithLoop(lstLoop).Build());
            lstController.Add(new TestAssistant.ControllerBuilder().WithProjectID(1).WithID(2).WithName("Controller2")
                        .WithLoop(lstLoop).Build());
            
            ProjectModel pModel = new TestAssistant.ProjectBuilder()
                    .WithController(lstController)
                    //.WithController(new TestAssistant.ControllerBuilder().WithProjectID(1).WithID(2).WithName("Controller2")
                    //    .WithLoop(new TestAssistant.LoopBuilder().WithControllerID(2)
                    //       .WithDevice(new TestAssistant.DeviceBuilder8036().WithLoopID(1)
                    //       .Builder())
                    //       .WithDevice(new TestAssistant.DeviceBuilder8036().WithLoopID(1).WithID(2).WithSimpleCode("002")
                    //       .Builder())
                    //.Build())
                    //.Build())
                .Build();
            SummaryNodeInfo summaryInfo=_projManager.DisplaySummaryInfo(pModel);
            Assert.That(summaryInfo.ChildNodes.Count(), Is.EqualTo(3), "应为三个子节点");

            var  result = from s in summaryInfo.ChildNodes where s.OrderNumber == 0 select s;
            SummaryNodeInfo projectNodeInfo = result.FirstOrDefault();

            Assert.That(projectNodeInfo.DisplayName, Is.EqualTo("秦皇岛火车站"), "项目名称应为秦皇岛火车站");


            result = from s in summaryInfo.ChildNodes where s.OrderNumber > 0 orderby s.OrderNumber select s ;

            Assert.That(result.Count(), Is.EqualTo(2), "应为两个控制器");

            //控制器级别

            Assert.That(result.ElementAt(0).DisplayName, Is.EqualTo("Controller8036"), "控制器名称应为8036");

            
            var controllerNodes = result.ElementAt(0).ChildNodes;
            Assert.That(controllerNodes.Count, Is.EqualTo(6), "应为6个节点");

            string strNodes="";
            foreach (var node in controllerNodes)
            {
                strNodes += node.DisplayName;
            }
            Assert.That(strNodes.IndexOf("回路"), Is.GreaterThan(-1), "未找到回路");
            
            var controller=result.ElementAt(0);
            Assert.That(controller.NodeAmount["回路"], Is.EqualTo(1), "应为一个回路");

            Assert.That(controller.NodeAmount["器件数量"], Is.EqualTo(1), "应为一个器件");

            //Assert.That(deviceResult.Count, Is.EqualTo(1), "应为一个器件");

            //Assert.That(deviceResult.ElementAt(0).DisplayName, Is.EqualTo(""), "应为一个器件");
            
            Assert.That(result.ElementAt(1).DisplayName, Is.EqualTo("Controller2"), "控制器名称应为Controller2");
            


            //foreach (var controllerNodeInfo in result)
            //{
            //    Assert.That(controllerNodeInfo.DisplayName, Is.EqualTo(""), "文件应该存在");
            //}
            
                   
        }
        [Test]
        public void OpenProject() 
        {

            _databaseService = new SQLiteDatabaseAccess("E:\\myData.db", _logRecorder, _fileService);
            _projService = new ProjectService();
            ProjectModel project=_projService.OpenProject("E:\\myData.db");
            Assert.That(project.Controllers[0].ID, Is.EqualTo(1),"控制器编号为1");

            Assert.That(project.Controllers[0].Name, Is.EqualTo("8036controller"), "8036controller");
            Assert.That(project.Controllers[0].Loops[0].ID, Is.EqualTo(1), "回路1");
            Assert.That(project.Controllers[0].Loops[0].Name, Is.EqualTo("Loop1"), "Loop1");
            Assert.That(project.Controllers[0].Loops[1].ID, Is.EqualTo(4), "回路2 编号4");
            Assert.That(project.Controllers[0].Loops[1].Name, Is.EqualTo("Loop2"), "Loop2");

            Assert.That(project.Controllers[0].StandardConfig[0].Code, Is.EqualTo("1"), "标准级态1");
            Assert.That(project.Controllers[0].StandardConfig[1].Code, Is.EqualTo("2"), "标准级态2");
            Assert.That(project.Controllers[0].StandardConfig[2].Code, Is.EqualTo("3"), "标准级态3");
            Assert.That(project.Controllers[0].StandardConfig[3].Code, Is.EqualTo("4"), "标准级态4");

        }
        public void ExportProject()
        {
            _databaseService = new SQLiteDatabaseAccess("E:\\myData.db", _logRecorder, _fileService);
            _projService = new ProjectService();
            ProjectModel project = _projService.OpenProject("E:\\myData.db");
            

        }
        public void ManipulateProjectData()
        {
            //假定已经成功读取到数据文件中的数据
            //测试组织项目数据的类是否可以正常工作
            List<LoopModel> lstLoop = new List<LoopModel>();
            lstLoop.Add(new TestAssistant.LoopBuilder<DeviceInfo8036>().WithControllerID(2)
                           .WithDevice(new TestAssistant.DeviceBuilder8036().WithLoopID(1).Builder()).Build());
            lstLoop.Add(new TestAssistant.LoopBuilder<DeviceInfo8036>().WithControllerID(2)
                           .WithDevice(new TestAssistant.DeviceBuilder8036().WithLoopID(1).WithID(2).WithSimpleCode("002").Builder()).Build());

            List<ControllerModel> lstController = new List<ControllerModel>();
            lstController.Add(new TestAssistant.ControllerBuilder().WithProjectID(1).Build());
            lstController.Add(new TestAssistant.ControllerBuilder().WithProjectID(1).WithID(2).WithName("Controller2")
                        .WithLoop(lstLoop).Build());


            ProjectModel pModel = new TestAssistant.ProjectBuilder()
                    .WithController(lstController).Build();

            _projManager.CreateProject(pModel);
            //_databaseService = new SQLiteDatabaseAccess(_projManager.Project.SavePath, _logRecorder, _fileService);
            //_projService = new ProjectService(_databaseService);            
            //_projService.Dispose();
            Assert.That(pModel.ID, Is.EqualTo(_projManager.Project.ID), "ID");
            Assert.That(pModel.Name, Is.EqualTo(_projManager.Project.Name), "Name");
            Assert.That(pModel.SavePath, Is.EqualTo(_projManager.Project.SavePath), "SavePath");
            Assert.That(pModel.SaveInterval, Is.EqualTo(_projManager.Project.SaveInterval), "SaveInterval"); 
        }
        [OneTimeTearDown]
        public void CleanUp()
        {
           // _fileService.DeleteFile(_projManager.Project.SavePath);
           // _databaseService.Dispose();            
        }

    }
}
