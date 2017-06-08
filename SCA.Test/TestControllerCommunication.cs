using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using SCA.Interface;
using SCA.Model;
using SCA.BusinessLib;
/* ==============================
*
* Author     : William
* Create Date: 2016/10/28 8:38:20
* FileName   : TestControllerCommunication
* Description: 控制器通讯测试类
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestControllerCommunication
    {
        
        public void TestComCommunication()
        {

        }
        [Test]
        public void TestControllerDownloadData()
        {
            
            //controller.
            //1.从控制器接收数据
            //2.判断控制器当前设置的状态：上传或下载
            //3.得到控制器类型
            //4.加载“具体的控制器处理类”，执行数据处理，及回复６６确认
            IControllerCommunication controller = Substitute.For<IControllerCommunication>();
            controller.PortName.Returns<string>("Com1");
            controller.BaudRate.Returns<int>(38400);
            controller.OpenPort().Returns<bool>(true);
            //controller.Received
            IControllerOperation operation = Substitute.For<IControllerOperation>();
            operation.GetControllerType().Returns<ControllerType>(ControllerType.NT8036);
            SCA.BusinessLib.BusinessLogic.ControllerManager controllerManager = new BusinessLib.BusinessLogic.ControllerManager();

            controllerManager.AddController(1, operation);
        }
    }
}
