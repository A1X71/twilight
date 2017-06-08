using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 14:02:45
* FileName   : DeviceInfo8000
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceInfo8000 : DeviceInfoBase, IDevice
    {
        private LoopModel _loop; //Added at 2016-12-06 回路信息
        private bool? _disable = false;
        /// <summary>
        /// 回路ID
        /// </summary>
        public Int32 LoopID { get; set; }
        /// <summary>
        /// 机号 ,流程测试，临时应用
        /// </summary>
        //public string MachineNo { get; set; }
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

        public Int16? SensitiveLevel { get; set; }
        
        public bool? Disable
        {
            get { return _disable; }
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
        public string LinkageGroup3 { get; set; }

        public Int16? Feature { get; set; }
        public Int16? DelayValue { get; set; }
        ///// <summary>
        ///// 手操号
        ///// </summary>
        //public Int16? KeyNo { get; set; }
        /// <summary>
        /// 此值在8001控制器中中为计算得出
        /// 8000控制器中需要在此录入
        /// 应用BoardNo,SubBoardNo,KeyNo计算出来的值
        /// </summary>
        public string sdpKey { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public Int16? ZoneNo { get; set; }
        public string BroadcastZone { get; set; }   
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
