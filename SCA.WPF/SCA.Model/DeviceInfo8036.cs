using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/11/10 10:40:02
* FileName   : DeviceInfo8036
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class DeviceInfo8036:DeviceInfoBase,IDevice
    {
        private LoopModel _loop; //Added at 2016-12-06 回路信息
        
        #region Private Fields
        private string _code;
        private Int32? _loopID;
        private string _simpleCode;
        private Int16? _type;
        private Int16? _zoneNo;
        private string _location;
        private bool? _disable=false;
        private float? _alertValue;
        private float? _forcastValue;
        private string _linkageGroup1;
        private string _inkageGroup2;
        private Int16? _delayValue;
        private Int16? _buildingNo;
        private Int16? _floorNo;
        private Int16? _roomNo;
        #endregion
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
        /// <存储完整器件编号(机号+路号+器件编号):七位示例->0102003; 八位示例->00102003>
        /// </summary>
        public string Code {
            get {
                //LoopCode应包含机号和路号
                //return _loop.Code + SimpleCode;
                return _code;
            }
            set 
            {
                _code = value;
                //NotifyOfPropertyChange(()=>Code);
            }
        
        }
        /// <summary>
        /// 无机号及路号的编码
        /// </summary>
        public string SimpleCode
        { 
            get
            {
                if (Loop != null && Code!=null )
                {
                    return Code.Substring(Loop.Code.Length, 3);
                }
                return null;
            } 
            //Commented at 2017-04-05, SimpleCode由Code计算得到，不需要设置
            //set 
            //{
            //    _simpleCode = value;
            //    NotifyOfPropertyChange(() => SimpleCode);
            //}
        }
        /// <summary>
        /// 器件类型
        /// </summary>
        public Int16 TypeCode { get; set; }


        public bool? Disable { get { return _disable; } set { _disable = value; } }
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
        //public string BuildingNo { get; set; }
        //public string FloorNo { get; set; }
        //public string RoomNo { get; set; }

        public string LinkageGroup1 { get; set; }
        public string LinkageGroup2 { get; set; }
        /// <summary>
        /// 报警浓度
        /// </summary>
        public float? AlertValue { get { return _alertValue; } set { _alertValue = value; } }
        /// <summary>
        /// 预警浓度
        /// </summary>
        public float? ForcastValue { get { return _forcastValue; } set { _forcastValue = value; } }
        public Int16? DelayValue { get; set; }

        public Int16? BuildingNo 
        {
            get 
            {
                return _buildingNo;
            }
            set
            {
                _buildingNo = value;
                //NotifyOfPropertyChange(() => BuildingNo);
            }
        }
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

        ////灵敏度
        //public Int16 SensitiveLevel { get; set; }
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
