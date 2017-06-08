using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/23 15:26:30
* FileName   : CommandBE
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 通用组态信息上传命令
    /// </summary>
    public class CommandBE:ICommand
    {
        ControllerTypeBase _controller;
        public  CommandBE(ControllerTypeBase controller)
        {
            _controller = controller;
        }

        public void Execute()
        {
            Console.WriteLine("Received BE Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "BE";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }
        }
    }
}
