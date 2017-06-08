using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/3 14:09:17
* FileName   : ProcessCommand
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    public class ProcessCommand
    {
        public void Run(ICommand cmd)
        {
           // ControllerComBase controllerCom = new ControllerUnknownCom();
            
            // ICommand cmdCA = new CommandCA(controllerCom);
            Invoker invoker = new Invoker();
            invoker.SetCommand(cmd);
            invoker.Execute();
            
        }
    }
}
