using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.ComCommunication;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2016/12/7 13:09:31
* FileName   : ControllerTypeUnknown
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{
    public class ControllerTypeUnknown:ControllerTypeBase
    {
        private const string m_ErrorInfo = "未知控制器类型";
        public ControllerTypeUnknown(SerialComManager serialManager,IProtocolDriver protocolDriver):base(serialManager, protocolDriver)
        {
            base.ControllerModel = new Model.ControllerModel(ControllerType.NONE);   
        }
        public override void SendDeviceInfo()
        {
            throw new NotImplementedException();
        }
        public override void SendStandardLinkageConfigInfo()
        {
            throw new NotImplementedException();
        }
        //public byte[] AssemblePackage66()
        //{
        //    return base.AssemblePackage66();
        //}
        //public byte[] AssemblePackageB9()
        //{
        //    return base.AssemblePackageB9();
        //}

        public  List<byte[]> AssemblePackageBB(List<object> deviceInfo)
        {
            throw new NotImplementedException(m_ErrorInfo);
        }



        public override void SendMixedLinkageConfigInfo()
        {
            throw new NotImplementedException();
        }

        public override void SendGeneralLinkageConfigInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveDeviceInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveStandardLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveMixedLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveGeneralLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveManualControlBoardInfo()
        {
            throw new NotImplementedException();
        }

        public override void SendManualControlBoardInfo()
        {
            throw new NotImplementedException();
        }

        protected override void SetDownloadedDeviceInfoTotalAmountInCurrentLoop(LoopModel loopModel)
        {
            throw new NotImplementedException();
        }

        protected override byte[] AssemblePackageBC(LinkageConfigStandard standardConfig)
        {
            throw new NotImplementedException();
        }

        public override ControllerModel GetControllerUploadedInfo()
        {
            throw new NotImplementedException();
        }

        public override LinkageConfigStandard ParsePackageCD(byte[] standardLinkagePackage)
        {
            throw new NotImplementedException();
        }
    }
}
