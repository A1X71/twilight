using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/3 14:07:38
* FileName   : ProcessCommand
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    public class Invoker
    {
        private  ICommand _command;//为什么设为Public
        public void SetCommand(ICommand command)
        {
            _command = command;
        }
        public void Execute()
        {
            _command.Execute();
        }
    }
}
