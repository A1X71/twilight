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


        public int ControlType { get; set; }                //NT8053 被控类型 0空器件0x00  1本机设备0x01  2楼区层0x02  3输出组0x03  4网络设备0x80

        #region 本机设备
        public string LocalDevice1{ get; set; }             //NT8053 本机设备1
        public string LocalDevice2 { get; set; }            //NT8053 本机设备2
        public string LocalDevice3 { get; set; }            //NT8053 本机设备3
        public string LocalDevice4 { get; set; }            //NT8053 本机设备4
        #endregion 本机设备

        #region 楼区层
        public string BuildingNo { get; set; }          //NT8053 楼区层 楼号
        public string AreaNo { get; set; }              //NT8053 楼区层 区号
        public string FloorNo { get; set; }             //NT8053 楼区层 层号
        public Int16 DeviceType { get; set; }          //NT8053 楼区层 设备类型

        #endregion 楼区层

        #region 输出组
        public string LinkageGroup { get; set; }        //NT8053 输出组

        #endregion 输出组

        #region 网络设备
        public string NetDevice1 { get; set; }          //NT8053 网络设备1
        public string NetDevice2 { get; set; }          //NT8053 网络设备2
        public string NetDevice3 { get; set; }          //NT8053 网络设备3
        public string NetDevice4 { get; set; }          //NT8053 网络设备4
        #endregion 网络设备

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
