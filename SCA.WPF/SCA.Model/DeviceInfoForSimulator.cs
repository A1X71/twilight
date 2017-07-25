using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/16 14:24:48
* FileName   : DeviceInfoForSimulator
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceInfoForSimulator:IDevice
    {        
        /// <summary>
        /// 序号
        /// </summary>
        public Int32 SequenceNo { get; set; }
        /// <summary>
        /// 器件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 器件类型
        /// </summary>
        public DeviceType Type { get; set; }       
        /// <summary>
        /// 器件类型
        /// </summary>
        public Int16 TypeCode { get; set; }
        public string LinkageGroup1 { get; set; }
        public string LinkageGroup2 { get; set; }
        public string LinkageGroup3 { get; set; }     
        public Int16? BuildingNo { get; set; }
        public Int16? ZoneNo { get; set; }
        public Int16? FloorNo { get; set; }

        public LoopModel Loop { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 控制器范围
        /// </summary>
        public ControllerScope Scope { get; set; }
        public bool? Disable
        {
            get;
            set;
        }
        public string Location { get; set; }
        
    }
   /// <summary>
   /// 控制器范围
   /// </summary>
   [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ControllerScope
    {
        [Description("本机")]
        LocalMachine=0,
        [Description("它机")]
        OtherMachine=1,
        [Description("无器件")]
        Unfound=3
    }
}
