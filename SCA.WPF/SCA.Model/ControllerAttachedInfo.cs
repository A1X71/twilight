using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/19 15:45:47
* FileName   : ControllerAttachedInfo
* Description: 控制器附属信息，兼容老版数据结构
* Version：V1  
* ===============================
*/
namespace SCA.Model
{  
    public class ControllerAttachedInfo
    {
        
        public string FileVersion { get; set; }
        public string ProtocolVersion { get; set; }
        public string Position { get; set; }                
        public string DeviceAddressLength { get; set; }
        public string ControllerType { get; set; }
    }
}
