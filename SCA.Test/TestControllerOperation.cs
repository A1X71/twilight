using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SCA.Interface;
using SCA.BusinessLib;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.BusinessLogic;
using SCA.DatabaseAccess;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/17 15:28:55
* FileName   : TestControllerOperation
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestControllerOperation
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
            _projManager = ProjectManager.GetInstance;//(_projService, _fileService);
            

        }
        [Test]        
        public void ConvertOldVersionSoftwareDataFileToCurrentModel8036()
        {
            _databaseService = new MSAccessDatabaseAccess(@"C:\Users\Administrator\Desktop\foo\8036.mdb", _logRecorder, _fileService);
            IOldVersionSoftwareDBService oldVersionService = new OldVersionSoftware8036DBService(_databaseService);

            IControllerOperation controllerOperation = null;

            string[] strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            ControllerModel controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8036":
                        controllerOperation = new ControllerOperation8036(_databaseService);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }
            Assert.That(controllerInfo.Type, Is.EqualTo(ControllerType.NT8036), "控制器类型不正确");
            Assert.That(controllerInfo.Loops.Count, Is.EqualTo(2), "回路数量不正确");
            Assert.That(controllerInfo.Loops[0].Code, Is.EqualTo("00101"), "回路名称不正确");
            Assert.That(controllerInfo.Loops[0].DeviceAmount, Is.EqualTo(46), "01回路数量不正确");
            Assert.That(controllerInfo.Loops[0].GetDevices<Model.DeviceInfo8036>()[0].Code, Is.EqualTo("00101001"), "器件编码不正确");
            
            Assert.That(controllerInfo.Loops[1].Code, Is.EqualTo("00102"), "回路名称不正确");

            Assert.That(controllerInfo.StandardConfig.Count, Is.EqualTo(1), "标准组态数量为1");
            Assert.That(controllerInfo.StandardConfig[0].Code, Is.EqualTo("0001"), "标准组态编号为0001");
            Assert.That(controllerInfo.StandardConfig[0].DeviceNo4, Is.EqualTo("00001009"), "编号4的编号为00001009");
            Assert.That(controllerInfo.StandardConfig[0].LinkageNo3, Is.EqualTo("0003"), "联动组3的编号为0003");

        }
        [Test]
        public void ConvertOldVersionSoftwareDataFileToCurrentModel8001()
        {
            _databaseService = new MSAccessDatabaseAccess(@"E:\2016\6 软件优化升级\4 实际工程数据\工程数据111\连城心怡都城_文件版本5.mdb", _logRecorder, _fileService);
            IOldVersionSoftwareDBService oldVersionService = new OldVersionSoftware8001DBService(_databaseService);
            IControllerOperation controllerOperation = null;
            string[] strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            ControllerModel controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8001":
                        controllerOperation = new ControllerOperation8001();
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }
            Assert.That(controllerInfo.Type, Is.EqualTo(ControllerType.NT8001), "控制器类型不正确");
            Assert.That(controllerInfo.DeviceAddressLength, Is.EqualTo(7), "器件长度为7");
            Assert.That(controllerInfo.Loops.Count, Is.EqualTo(10), "回路数量不正确");
            Assert.That(controllerInfo.Loops[0].Code, Is.EqualTo("0001"), "回路名称不正确");
            Assert.That(controllerInfo.Loops[0].DeviceAmount, Is.EqualTo(46), "01回路器件数量不正确");
            Assert.That(controllerInfo.Loops[0].GetDevices<Model.DeviceInfo8001>()[0].Code, Is.EqualTo("0001001"), "器件编码不正确");

            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].Code, Is.EqualTo("0003001"), "器件编码不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].TypeCode, Is.EqualTo(4), "器件类型不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].Disable, Is.EqualTo(0), "隔离不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].SensitiveLevel, Is.EqualTo(2), "灵敏度不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].Location, Is.EqualTo("A2三层306店"), "地点不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].BuildingNo, Is.EqualTo(1), "楼号不正确");
            //区号可以为0
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].ZoneNo, Is.EqualTo(2), "区号不正确");
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].FloorNo, Is.EqualTo(3), "层号不正确");
            //房间可以为0
            Assert.That(controllerInfo.Loops[2].GetDevices<Model.DeviceInfo8001>()[0].RoomNo, Is.EqualTo(null), "房间号不正确");

            Assert.That(controllerInfo.Loops[1].Code, Is.EqualTo("0002"), "回路名称不正确");

            Assert.That(controllerInfo.StandardConfig.Count, Is.EqualTo(20), "标准组态数量为20");
            Assert.That(controllerInfo.StandardConfig[13].Code, Is.EqualTo("0014"), "标准组态编号为0014");
            Assert.That(controllerInfo.StandardConfig[13].DeviceNo4, Is.EqualTo("0007102"), "编号4的编号为0007102");
            Assert.That(controllerInfo.StandardConfig[13].LinkageNo2, Is.EqualTo("0020"), "联动组2的编号为0020");


            Assert.That(controllerInfo.MixedConfig.Count, Is.EqualTo(53), "混合组态数量为53");
            Assert.That(controllerInfo.MixedConfig[13].Code, Is.EqualTo("0014"), "混合组态编号为0014");
            Assert.That(controllerInfo.MixedConfig[13].ActionCoefficient, Is.EqualTo("1"), "编号14的动作常数为1");
            Assert.That(controllerInfo.MixedConfig[13].ActionType, Is.EqualTo(LinkageActionType.AND), "编号14的动作类型为与");
            Assert.That(controllerInfo.MixedConfig[13].DeviceTypeCodeA, Is.EqualTo(13), "编号14的类型A为13");
            Assert.That(controllerInfo.MixedConfig[29].TypeC, Is.EqualTo(LinkageType.ZoneLayer), "编号30的类型c为区层");
            Assert.That(controllerInfo.MixedConfig[29].BuildingNoC, Is.EqualTo(6), "编号30的楼号为6");
            Assert.That(controllerInfo.MixedConfig[29].ZoneNoC, Is.EqualTo(5), "编号30的区号为5");
            Assert.That(controllerInfo.MixedConfig[29].LayerNoC, Is.EqualTo(-1), "编号30的层号为-1");
            Assert.That(controllerInfo.MixedConfig[30].TypeC, Is.EqualTo(LinkageType.Address), "编号31的类型c为地址");
            Assert.That(controllerInfo.MixedConfig[30].MachineNoC, Is.EqualTo("00"), "编号31的机号为00");
            Assert.That(controllerInfo.MixedConfig[30].LoopNoC, Is.EqualTo("07"), "编号31的路号为07");
            Assert.That(controllerInfo.MixedConfig[30].DeviceCodeC, Is.EqualTo("075"), "编号31的地编号为075");

            Assert.That(controllerInfo.GeneralConfig.Count, Is.EqualTo(37), "通用组态数量为37");
            Assert.That(controllerInfo.GeneralConfig[13].Code, Is.EqualTo("0014"), "通用组态编号为0014");
            Assert.That(controllerInfo.GeneralConfig[13].ActionCoefficient, Is.EqualTo("1"), "编号14的动作常数为1");
            Assert.That(controllerInfo.GeneralConfig[13].BuildingNoA, Is.EqualTo(6), "编号14的楼号为6");
            Assert.That(controllerInfo.GeneralConfig[13].ZoneNoA, Is.EqualTo(5), "编号14的区号为5");
            Assert.That(controllerInfo.GeneralConfig[13].LayerNoA1, Is.EqualTo(0), "编号14的层号A1为0");
            Assert.That(controllerInfo.GeneralConfig[13].LayerNoA2, Is.EqualTo(0), "编号14的层号A2为0");
            Assert.That(controllerInfo.GeneralConfig[13].DeviceTypeCodeA, Is.EqualTo(13), "编号14的类型A为13");

            Assert.That(controllerInfo.GeneralConfig[13].TypeC, Is.EqualTo(LinkageType.ZoneLayer), "编号14的分类C为区层");
            Assert.That(controllerInfo.GeneralConfig[13].BuildingNoC, Is.EqualTo(6), "编号14的楼号为6");
            Assert.That(controllerInfo.GeneralConfig[13].ZoneNoC, Is.EqualTo(5), "编号14的区号C为5");
            Assert.That(controllerInfo.GeneralConfig[13].LayerNoC, Is.EqualTo(-1), "编号14的层号C为-1");
            Assert.That(controllerInfo.GeneralConfig[13].DeviceTypeCodeC, Is.EqualTo(36), "编号14的层号C为36");

            Assert.That(controllerInfo.GeneralConfig[14].TypeC, Is.EqualTo(LinkageType.Address), "编号15的分类C为地址");
            Assert.That(controllerInfo.GeneralConfig[14].MachineNoC, Is.EqualTo("00"), "编号15的机号为0");
            Assert.That(controllerInfo.GeneralConfig[14].LoopNoC, Is.EqualTo("01"), "编号15的路号为1");
            Assert.That(controllerInfo.GeneralConfig[14].DeviceCodeC, Is.EqualTo("001"), "编号15的层号C为001");
            Assert.That(controllerInfo.GeneralConfig[14].DeviceTypeCodeC, Is.EqualTo(null), "编号14的层号C为null");

            

        }
        [Test]
        public void StandardLinkageEmulator1()
        {   
            //1.组织控制器对象
            DeviceInfo8001 device8001_1 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(1).WithCode("0001009").WithLinkageGroup1("0001").WithLinkageGroup2("0003").WithLinkageGroup3("0004").Builder();
            DeviceInfo8001 device8001_2 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(2).WithCode("0001017").WithLinkageGroup1("").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_3 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(3).WithCode("0001020").WithLinkageGroup1("0004").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            List<DeviceInfo8001> lstDeviceInfo = new List<DeviceInfo8001>();
            lstDeviceInfo.Add(device8001_1);

            List<DeviceInfo8001> lstDeviceInfoForController = new List<DeviceInfo8001>();//回路中加载的器件信息
            lstDeviceInfoForController.Add(device8001_1);
            lstDeviceInfoForController.Add(device8001_2);
            lstDeviceInfoForController.Add(device8001_3);

            List<LoopModel> lstLoop = new List<LoopModel>();
            lstLoop.Add(new TestAssistant.LoopBuilder<DeviceInfo8001>().WithCode("0001")
                           .WithDevices(lstDeviceInfoForController).Build());


            List<LinkageConfigStandard> lstStandardLinkage = new List<LinkageConfigStandard>();
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithDeviceNo1("0101005").WithDeviceNo2("0101006").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").Build());
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0003").WithDeviceNo1("0001020").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("0020").WithLinkageNo2("").WithLinkageNo3("").Build());
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0004").WithDeviceNo1("0001017").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("").WithLinkageNo2("").WithLinkageNo3("").Build());
            ControllerModel controllerInfo = new TestAssistant.ControllerBuilder().WithMachineNumber("0").WithDeviceAddressLength(7).WithLoop(lstLoop).WithStandardLinkageConfig(lstStandardLinkage).Build();

            
            Dictionary<DeviceInfo8001,LinkageSimulatorDeviceStatus> linkageResult=LinkageEmulatorService.EmulateLinkageStandardInfo(lstDeviceInfo, controllerInfo);


            int assertCount = 0;
            foreach(var s in linkageResult.Keys)
            {
                if(s.Code=="0101005" && linkageResult[s]==LinkageSimulatorDeviceStatus.OtherMachine)
                {
                    assertCount++;
                }
                if (s.Code == "0101006" && linkageResult[s] == LinkageSimulatorDeviceStatus.OtherMachine)
                {
                    assertCount++;
                }
                if (s.Code == "0001020" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }
                if (s.Code == "0001017" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }                
            }
            Assert.That(assertCount, Is.EqualTo(4), "期待4个触发器件");            
    
        }
        [Test]
        public void StandardLinkageEmulator2()
        {
            //1.组织控制器对象
            DeviceInfo8001 device8001_1 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(1).WithCode("0001009").WithLinkageGroup1("").WithLinkageGroup2("0003").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_2 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(2).WithCode("0001017").WithLinkageGroup1("").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_3 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(3).WithCode("0001020").WithLinkageGroup1("0004").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_4 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(4).WithCode("0001021").WithLinkageGroup1("0003").WithLinkageGroup2("0006").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_5 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(5).WithCode("0001022").WithLinkageGroup1("").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            DeviceInfo8001 device8001_6 = new TestAssistant.DeviceBuilder8001().WithLoopID(1).WithID(6).WithCode("0001023").WithLinkageGroup1("").WithLinkageGroup2("").WithLinkageGroup3("").Builder();
            List<DeviceInfo8001> lstDeviceInfo = new List<DeviceInfo8001>();
            lstDeviceInfo.Add(device8001_1);
            lstDeviceInfo.Add(device8001_4);

            List<DeviceInfo8001> lstDeviceInfoForController = new List<DeviceInfo8001>();//回路中加载的器件信息
            lstDeviceInfoForController.Add(device8001_1);
            lstDeviceInfoForController.Add(device8001_2);
            lstDeviceInfoForController.Add(device8001_3);
            lstDeviceInfoForController.Add(device8001_4);
            lstDeviceInfoForController.Add(device8001_5);
            lstDeviceInfoForController.Add(device8001_6);

            List<LoopModel> lstLoop = new List<LoopModel>();
            lstLoop.Add(new TestAssistant.LoopBuilder<DeviceInfo8001>().WithCode("0001")
                           .WithDevices(lstDeviceInfoForController).Build());


            List<LinkageConfigStandard> lstStandardLinkage = new List<LinkageConfigStandard>();
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithDeviceNo1("0101005").WithDeviceNo2("0101006").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").Build());
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0003").WithActionCoefficient(2).WithDeviceNo1("0001020").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("0020").WithLinkageNo2("").WithLinkageNo3("").Build());
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0004").WithDeviceNo1("0001017").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("").WithLinkageNo2("").WithLinkageNo3("").Build());
            
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0005").WithDeviceNo1("0001022").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("").WithLinkageNo2("").WithLinkageNo3("").Build());
            lstStandardLinkage.Add(new TestAssistant.StandardLinkageBuilder().WithCode("0006").WithDeviceNo1("0001023").WithDeviceNo2("").WithDeviceNo3("").WithDeviceNo4("").WithDeviceNo5("").WithDeviceNo6("").WithDeviceNo7("").WithDeviceNo8("").WithLinkageNo1("").WithLinkageNo2("").WithLinkageNo3("").Build());

            ControllerModel controllerInfo = new TestAssistant.ControllerBuilder().WithMachineNumber("0").WithDeviceAddressLength(7).WithLoop(lstLoop).WithStandardLinkageConfig(lstStandardLinkage).Build();

            Dictionary<DeviceInfo8001, LinkageSimulatorDeviceStatus> linkageResult = LinkageEmulatorService.EmulateLinkageStandardInfo(lstDeviceInfo, controllerInfo);


            int assertCount = 0;
            foreach (var s in linkageResult.Keys)
            {
                if (s.Code == "0101005" && linkageResult[s] == LinkageSimulatorDeviceStatus.OtherMachine)
                {
                    assertCount++;
                }
                if (s.Code == "0101006" && linkageResult[s] == LinkageSimulatorDeviceStatus.OtherMachine)
                {
                    assertCount++;
                }
                if (s.Code == "0001020" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }
                if (s.Code == "0001017" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }
                if (s.Code == "0001022" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }
                if (s.Code == "0001023" && linkageResult[s] == LinkageSimulatorDeviceStatus.Actived)
                {
                    assertCount++;
                }

            }
            Assert.That(assertCount, Is.EqualTo(2), "期待2个触发器件");

        }


    }
}
