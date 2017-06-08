using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/19 16:57:14
* FileName   : CommandC9
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{

    /// <summary>
    /// 准备上传命令
    /// </summary>
    public class CommandC9 : ICommand
    {
        ControllerTypeBase _controller;
        public  CommandC9(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            
            Console.WriteLine("Received C9 Command");

            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "66--准备接收数据";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }

        }
    }
}
