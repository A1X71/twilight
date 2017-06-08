using System;
using SCA.BusinessLib.Controller;


/* ==============================
*
* Author     : William
* Create Date: 2016/12/5 13:21:34
* FileName   : Command33
* Description:
* Version：V1
* ===============================
*/

namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 检验错误命令,此命令何时发？什么作用？
    /// </summary>
    class Command33 : ICommand
    {
        ControllerTypeBase _controller;
        public Command33(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            //_controller.ActionCA();            
            Console.WriteLine("CRC Error!");
        }
    }
}

