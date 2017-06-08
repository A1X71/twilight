using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/9 11:57:32
* FileName   : TestDeviceCreationcs
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    public class TestDeviceCreations
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
            
        }
        [Test]
        public void CreateProject()
        {
            ProjectModel pModel = new ProjectModel(1, "myData", 1);
            pModel.SavePath = "e:\\myData.db";
            _databaseService = new DatabaseAccess.SQLiteDatabaseAccess(pModel.SavePath, _logRecorder, _fileService);
            _projService = new ProjectService();
            _projManager = SCA.BusinessLib.ProjectManager.GetInstance;//(_projService, _fileService);            
            //ControllerModel cModel = new ControllerModel(ControllerType.NT8036);
            //cModel.Project = pModel;
            //cModel.Name = "Name8036";
            //cModel.Level = 3;
            //cModel.DeviceAddressLength = 8;
            //cModel.LoopAddressLength = 2;
            //if (cModel.DeviceAddressLength == 8)
            //{
            //    cModel.MachineNumber = "001";
            //}
            //else
            //{
            //    cModel.MachineNumber = "01";
            //}            
           bool blnResult= _projService.CreateProject(pModel,null);
           _projService.Dispose();
           _fileService.DeleteFile(pModel.SavePath);
           Assert.That(blnResult, Is.EqualTo(true));           
        }
        public void CreateController()
        {
            ProjectModel pModel = new ProjectModel(1, "myData", 1);
            pModel.SavePath = "e:\\myData.db";
            Model.ControllerModel controller = new ControllerModel(ControllerType.NT8036);
            controller.Name = "Name8036";
            controller.PortName = "Com1";
            controller.Project = pModel;            
            controller.DeviceAddressLength = 7;
            if (controller.DeviceAddressLength == 8)
            {
                controller.MachineNumber = "001";
            }
            else
            {
                controller.MachineNumber = "01";
            }
            controller.LoopAddressLength = 2;

        }
    }
}
