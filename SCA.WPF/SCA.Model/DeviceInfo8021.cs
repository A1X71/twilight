using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/25 8:07:45
* FileName   : DeviceInfo8021
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceInfo8021 : DeviceInfoBase, IDevice
    {
        private LoopModel _loop; //Added at 2016-12-06 回路信息
        /// <summary>
        /// 回路ID
        /// </summary>
        public Int32 LoopID { get; set; }
        
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


        private bool? _disable = false;
        public bool? Disable
        {
            get { return _disable; }
            set { _disable = value; }
        }
        /// <summary>
        /// 电流报警值 
        /// </summary>
        public float? CurrentThreshold { get; set; }
        /// <summary>
        /// 温度报警值
        /// </summary>
        public float? TemperatureThreshold { get; set; }
        public Int16? BuildingNo { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public Int16? ZoneNo { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string Location { get; set; }

        public Int16? FloorNo { get; set; }

        public Int16? RoomNo { get; set; }
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
