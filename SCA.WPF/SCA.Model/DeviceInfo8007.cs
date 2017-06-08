using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/7 9:12:14
* FileName   : DeviceInfo8007
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceInfo8007 : DeviceInfoBase, IDevice
    {
        private LoopModel _loop; //Added at 2016-12-06 回路信息
        /// <summary>
        /// 回路ID
        /// </summary>
        public Int32 LoopID { get; set; }
        /// <summary>
        /// 机号 ,流程测试，临时应用
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 标识ID
        /// </summary>
        public Int32 ID { get; set; }
        /// <summary>
        /// 器件编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 无机号及路号的编码
        /// </summary>
        public string SimpleCode
        {
            get
            {
                if (Loop != null && Code != null)
                {
                    return Code.Substring(Loop.Code.Length, 3);
                }
                return null;
            }
        }
        /// <summary>
        /// 器件类型
        /// </summary>
        public Int16 TypeCode { get; set; }
        /// <summary>
        /// 特性
        /// </summary>

        public Int16? Feature { get; set; }
        /// <summary>
        ///  灵敏度
        /// </summary>
        public Int16? SensitiveLevel { get; set; }

        private bool? _disable = false;
        /// <summary>
        /// 屏蔽
        /// </summary>
        public bool? Disable
        {
            get
            {
                return _disable;
            }
            set { _disable = value; }
        }
        public LoopModel Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
            }
        }

        public string LinkageGroup1 { get; set; }
        public string LinkageGroup2 { get; set; }
        
        public Int16? BuildingNo { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public Int16? ZoneNo { get; set; }        

        public Int16? FloorNo { get; set; }

        public Int16? RoomNo { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string Location { get; set; }
        #region 非业务字段
        private bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    this.Loop.IsDeviceDataDirty = true;
                }
            }

        }
        #endregion
    }
}
