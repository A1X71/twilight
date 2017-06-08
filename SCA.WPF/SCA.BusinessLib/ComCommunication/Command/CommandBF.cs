using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/24 8:15:57
* FileName   : CommandBF
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 网络手控盘数据上传命令
    /// </summary>
    public class CommandBF:ICommand 
    {
       ControllerTypeBase _controller;
        public  CommandBF(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            Console.WriteLine("Received BF Command");
            if (_controller.Status == ControllerStatus.DataReceiving)
            {
                _controller.SendingCMD = "BF";
                _controller.SerialManager.WriteData(_controller.AssemblePackage66());
            }
        }
    }
}
