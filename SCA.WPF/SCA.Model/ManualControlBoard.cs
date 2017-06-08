using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/24 8:20:09
* FileName   : MannualControlBoard
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualControlBoard
    {

        private ControllerModel _controller; //Added at 2017-01-24 控制器信息

        public ControllerModel Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        public int ID { get; set; }
        public int ControllerID { get; set; }
        //public   int  Code { get; set; }
        public int Code { get; set; }
        public int BoardNo { get; set; }
        public int SubBoardNo { get; set; }
        public int KeyNo { get; set; }
        public string DeviceCode { get; set; }

        public string SDPKey { get; set; }

        /// <summary>
        /// 下位机需要：当前板卡下的最大“手控盘号”
        /// </summary>
        public int MaxSubBoardNo { get; set; }
        #region 非业务数据
        //保存状态
        private bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    this.Controller.IsDirty = true;
                }

            }
        }
        #endregion
    }
}
