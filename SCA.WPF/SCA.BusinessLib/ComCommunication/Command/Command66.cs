using System;
using SCA.BusinessLib.Controller;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/3 15:38:41
* FileName   : Command66
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 信息确认命令
    /// </summary>
    class Command66:ICommand
    {
      //  private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ControllerTypeBase _controller;
        public  Command66(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        {
            //_controller.Action66();
            switch (_controller.SendingCMD)
            {
                case"B9":
                    Console.WriteLine("Command B9 received 66");
                    // log.Info("66....B9");
                    _controller.ReceivedB9ConfirmCommand = true;
                    break;
                case"BB"://器件设置
                    Console.WriteLine("Command BB received 66");
                  //  log.Info("66....BB::-->" +_controller.ControllerType.ToString());
                    _controller.ReceivedBBConfirmCommand = true;
                    break;
                case "BC"://标准组态
                    Console.WriteLine("Command BC received 66");
                    // log.Info("66....BC::-->" +_controller.ControllerType.ToString());
                    _controller.ReceivedBCConfirmCommand = true;
                    break;
                case "BE"://通用组态
                    Console.WriteLine("Command BE received 66");
                    // log.Info("66....BE::-->" + _controller.ControllerType.ToString());
                    _controller.ReceivedBEConfirmCommand = true;
                    break;
                case "BD"://混合组态
                    Console.WriteLine("Command BD received 66");
                    // log.Info("66....BD::-->" + _controller.ControllerType.ToString());
                    _controller.ReceivedBDConfirmCommand = true;
                    break;
                case "BF"://网络手控盘
                    Console.WriteLine("Command BF received 66");
                    // log.Info("66....BF::-->" + _controller.ControllerType.ToString());
                    _controller.ReceivedBFConfirmCommand = true;
                    break; 
                case "BA":
                    Console.WriteLine("Command BA received 66");
                    // log.Info("66....BA::-->" + _controller.ControllerType.ToString());
                    _controller.ReceivedBAConfirmCommand = true;
                    break; 
                default:
                    Console.WriteLine("Unknown command received 66");
                    break;
            }
        }

    }
}
