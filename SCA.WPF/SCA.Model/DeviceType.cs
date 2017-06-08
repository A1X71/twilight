using System;
using System.ComponentModel;
using System.IO;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/8 14:40:09
* FileName   : DeviceType
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceType
    {
        //public int ID { get; set; }

        public Int16 Code { get; set; }

        public string Name { get; set; }

        public MemoryStream CustomImage { get; set; }

        public MemoryStream StandardImage { get; set; }

        public bool IsValid { get; set; }

        public int? ProjectID { get; set; }

        public string MatchingController { get; set; }
    }
    public enum SpecialValue
    { 
        [Description("-8 任意火警")]
        AnyAlarm=-8
    }
}
