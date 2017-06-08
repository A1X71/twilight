using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/23 11:42:59
* FileName   : CommandBD
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 混合组态数据上传命令
    /// </summary>
    class CommandBD:ICommand
    {
        ControllerTypeBase _controller;
        public  CommandBD(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            Console.WriteLine("Received BD Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "BD";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }
        }
    }
}
