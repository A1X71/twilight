using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCA.Interface;
using NSubstitute;
using SCA.Model;
using SCA.BusinessLib;
using System.Collections.Concurrent;
using SCA.BusinessLib.Controller;
using SCA.BusinessLib.ComCommunication;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2016/11/11 14:39:23
* FileName   : ControllerCommunicationTesting
* Description: 测试控制器通讯流程
* Version：V1
* ===============================
*/
namespace Test.Console
{
    public class ControllerCommunicationTesting
    {
        

        public void Run()
        {

        }
        
        public static void Get8036DevInfo(out List<DeviceInfo8036> lstDevInfo)
        {
            ControllerModel cModel = new ControllerModel(1, "8036", ControllerType.NT8036,3);
            cModel.MachineNumber = "001";
            LoopModel lModel = new LoopModel();
            lModel.Controller = cModel;
            lModel.Code = "01";
            lstDevInfo = new List<DeviceInfo8036>();
            DeviceInfo8036 dev = new DeviceInfo8036();
            dev.ID = 1;
            dev.Loop = lModel;
            dev.Code = "00101001";
            dev.TypeCode = 36;
            dev.Disable = 0;
            dev.LinkageGroup1 = "1";
            dev.LinkageGroup2 = "2";
            dev.AlertValue = 0;
            dev.ForcastValue = 0;
            dev.DelayValue = 0;
            dev.BuildingNo = 5;
            dev.ZoneNo = 6;
            dev.FloorNo = 7;
            dev.RoomNo = 8;
            dev.Location = "仙鹤哈里发9";
            lstDevInfo.Add(dev);
        }
        public void UseRealArch()
        {

            //controller.
            //1.从控制器接收数据
            //2.判断控制器当前设置的状态：上传或下载
            //3.得到控制器类型
            //4.加载“具体的控制器处理类”，执行数据处理，及回复６６确认
            DeviceInfo8036 deviceInfo = new DeviceInfo8036();

            //1.取得当前操作的控制器数据
            SCA.BusinessLib.ModelOperation.ControllerModelOperation cModelOperation = new SCA.BusinessLib.ModelOperation.ControllerModelOperation();
            ControllerModel controllerModel = cModelOperation.GetControllersBySpecificID(1);

            //2.判断当前控制器的连接状态
            //ICom  com=Substitute.For<ICom>();
            IControllerCommunication controller = Substitute.For<IControllerCommunication>();
            controller.PortName.Returns<string>("Com1");
            controller.BaudRate.Returns<int>(38400);

            controller.OpenPort().Returns<bool>(true);//打开端口

            controller.TestControllerStatus().Returns<bool>(true); //获取控制器状态<收到巡检命令 CA>

            //controller.
            //3.执行下传数据指令

            //controller.Received
            IControllerOperation operation = Substitute.For<IControllerOperation>();
            // operation.GetControllerType().Returns<ControllerType>(ControllerType.NT8036);
            // SCA.BusinessLib.BusinessLogic.ControllerManager controllerManager = new SCA.BusinessLib.BusinessLogic.ControllerManager();

            //  controllerManager.AddController(controllerModel.ID,operation);//一个Controller对应一个Operation
        
        }
    }
}
