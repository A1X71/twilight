using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/23 11:01:54
* FileName   : CommandCD
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 标准组态信息上传命令
    /// </summary>
    public class CommandCD:ICommand
    {
        ControllerTypeBase _controller;
        public  CommandCD(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            
            Console.WriteLine("Received CD Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "CD";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }
        }
    }
}
