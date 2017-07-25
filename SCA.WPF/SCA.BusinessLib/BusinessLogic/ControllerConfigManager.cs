using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/2 11:01:43
* FileName   : ControllerConfigManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfigManager
    {
        public static IControllerConfig GetConfigObject(ControllerType type)
        {
            switch (type)
            { 
                case ControllerType.NT8036:
                    return new ControllerConfig8036();
                case ControllerType.NT8007:
                    return new ControllerConfig8007();
                case ControllerType.NT8001:
                    return new ControllerConfig8001();
                case ControllerType.FT8000:
                    return new ControllerConfig8000();
                case ControllerType.FT8003:
                    return new ControllerConfig8003();
                case ControllerType.NT8021:
                    return new ControllerConfig8021();  
                case ControllerType.NONE:
                    return new ControllerConfigNone();
                case ControllerType.NT8053:
                    return new ControllerConfig8053();
            }
            return null;            
        }

    }
}
