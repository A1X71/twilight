using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/10/27 13:14:03
* FileName   : Dyn
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model.ComCommunication
{
    [Serializable]
    public class Dyn
    {
        private DateTime _CurDT = DateTime.Now;

        public DateTime CurDT
        {
            get { return _CurDT; }
            set { _CurDT = value; }
        }

        private byte[] _ProHead = new byte[] { };
        /// <summary>
        /// 协议头
        /// </summary>
        public byte[] ProHead
        {
            get { return _ProHead; }
            set { _ProHead = value; }
        }

        /// <summary>
        /// 启动符
        /// </summary>
        public byte[] ProStart
        {
            get;
            set;
        }
        /// <summary>
        /// 通讯类型
        /// </summary>
        public byte[] ProComType
        {
            get;
            set;
        }
        /// <summary>
        /// 异或校验位
        /// </summary>
        public byte ProXorBit
        {
            get;
            set;
        }
        /// <summary>
        /// 和校验位
        /// </summary>
        public byte ProSumBit
        {
            get;
            set;
        }
        /// <summary>
        /// 数据长度
        /// </summary>
        public byte ProDataLength
        {
            get;
            set;
        }
        /// <summary>
        /// 固定值
        /// </summary>
        public byte ProFixedValue1
        { get; set; }
        /// <summary>
        /// 输出组编号
        /// </summary>
        public byte ProLinkageStandardCode
        { get; set; }
        /// <summary>
        /// 联动器件
        /// </summary>
        public byte[] ProLinkageDevice
        { get; set; }
        /// <summary>
        /// 动作常数
        /// </summary>
        public byte ProActionCoefficient
        { get; set; }
        public byte ProOutput1
        { get; set; }
        public byte ProOutput2
        { get; set; }
        public byte ProOutput3
        { get; set; }

        private int _DeviceAddr = -1;
        /// <summary>
        /// 解析的地址
        /// </summary>
        public int DeviceAddr
        {
            get { return _DeviceAddr; }
            set { _DeviceAddr = value; }
        }

        private byte[] _Command = new byte[] { };
        /// <summary>
        /// 协议命令
        /// </summary>
        public byte[] Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        private byte[] _ProEnd = new byte[] { };
        /// <summary>
        /// 协议结束
        /// </summary>
        public byte[] ProEnd
        {
            get { return _ProEnd; }
            set { _ProEnd = value; }
        }

        private object _State = null;
        /// <summary>
        /// 状态
        /// </summary>
        public object State
        {
            get { return _State; }
            set { _State = value; }
        }

        private float _Flow = 0.0f;
        /// <summary>
        /// 流量
        /// </summary>
        public float Flow
        {
            get { return _Flow; }
            set { _Flow = value; }
        }
        private float _Signal = 0.0f;
        /// <summary>
        /// 信号
        /// </summary>
        public float Signal
        {
            get { return _Signal; }
            set { _Signal = value; }
        }
    }
}
