using System;
using SCA.BusinessLib.Controller;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2016/12/3 14:03:41
* FileName   : CommandCA
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    /// <summary>
    /// 巡检命令
    /// </summary>
    class CommandCA:ICommand
    {

     //   private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ControllerTypeBase _controller;
        public CommandCA(ControllerTypeBase controller)
        {
            _controller = controller;
        }
        public void Execute()
        { 
            if (_controller.Status == ControllerStatus.DataSending)
            {
                switch (_controller.OperableDataType)
                { 
                    case OperantDataType.Device:
                        _controller.SendDeviceInfo();
                        break;
                    case OperantDataType.StandardLinkageConfig :
                        _controller.SendStandardLinkageConfigInfo();
                        break;
                    case OperantDataType.GeneralLinkageConfig:
                        _controller.SendGeneralLinkageConfigInfo();
                        break;
                    case OperantDataType.MixedLinkageConfig:
                        _controller.SendMixedLinkageConfigInfo();
                        break;
                    case OperantDataType.MannualControlBoard:
                        _controller.SendManualControlBoardInfo();
                        break;
                    case OperantDataType.DownloadAll:
                        _controller.SendAllInfo();
                        break;
                }
                
            }
            else
            {
                //if (((!_controller.ReceivedB9ConfirmCommand) && _controller.SendingCMD == "B9") || _controller.ControllerType==Model.ControllerModel.ControllerType.NONE) //控制器当前发送的命令为B9,且未收到66确认命令，则发送B9命令
                //发送B9后未收到确认命令，则重发B9；在未识别出控制器时，需要发送B9索要控制器类型
                if ((!_controller.ReceivedB9ConfirmCommand) || _controller.ControllerType == ControllerType.NONE)
                {
                    //_controller.ActionCA();        
                    Console.WriteLine("Sending B9...");
                    _controller.SendingCMD = "B9";
                 //   log.Info("Sending B9...");
                    _controller.SerialManager.WriteData(_controller.AssemblePackageB9());
                    // Console.ReadLine();
                }
                else
                {                    
                    Console.WriteLine("-------------收到B9确认命令--------------");
                    _controller.ReceivedB9ConfirmCommand = false; //Added at 2017-04-05
                }
            }
        }


    }
}
