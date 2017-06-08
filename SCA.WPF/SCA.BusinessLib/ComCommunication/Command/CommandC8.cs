using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/3 15:54:35
* FileName   : CommandC8
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 控制器类型命令
    /// </summary>
    class CommandC8:ICommand
    {
        ControllerTypeBase _controller;
        public  CommandC8(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            Model.ControllerModel tempModel = _controller.ControllerModel;
            //controllerTypeBase.ControllerType =
            //ComTestCP2.ProtocolDriver.AnalysisPackageGetControllerType(package,ref tempModel);
            _controller.ControllerType = _controller.GetControllerType(ref tempModel);
            _controller.ControllerModel = tempModel;
//          _controller.ActionC8();
            //_controller.ControllerType=
            _controller.ReceivedB9ConfirmCommand = false;//收到C8命令，将B9的66命令状态初始化为false
            Console.WriteLine("Received C8 Command");
            //_controller.ActionCA();            
            _controller.SendingCMD = "66";
            Console.WriteLine("Sending...");
            _controller.SerialManager.WriteData(_controller.AssemblePackage66());
        }
    }
}
