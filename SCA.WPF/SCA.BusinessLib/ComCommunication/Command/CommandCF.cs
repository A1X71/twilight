using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/19 17:03:14
* FileName   : CommandCF
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    //控制器回传结束命令
    class CommandCF:ICommand
    {
        ControllerTypeBase _controller;
        public  CommandCF(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {            
            Console.WriteLine("Received CF Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "66--准备接收数据";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());

                _controller.Status = ControllerStatus.DataReceived;
                //至此数据接收完成。
                //在完成后，需要对UI等进行重置操作，待处理........

            }
        }
    }
}
