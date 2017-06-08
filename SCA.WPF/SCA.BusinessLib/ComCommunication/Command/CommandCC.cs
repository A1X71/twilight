using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/20 8:42:41
* FileName   : CommandCC
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 控制器回传器件命令
    /// </summary>
    public class CommandCC:ICommand 
    {
        ControllerTypeBase _controller;
        public  CommandCC(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {            
            Console.WriteLine("Received CC Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "CC";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }
        }
    }
}
