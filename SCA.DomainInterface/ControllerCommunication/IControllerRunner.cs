using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Interface.ControllerCommunication
{
    public interface IControllerRunner<T>
    {
        /// <summary>
        /// 保存原始的byte数据
        /// </summary>
        /// <param name="data"></param>
        void SaveBytes(byte[] data, string desc);

        /// <summary>
        /// 保存解析后的数据
        /// </summary>
        void Save();

        /// <summary>
        /// 获得发送数据的命令，如果命令缓存中没有命令，则调用获得实时数据函数
        /// </summary>
        /// <returns></returns>
        byte[] GetSendBytes();
        /// <summary>
        /// 如果当前命令缓存没有命令，则调用该函数,一般返回获得设备的实时数据命令，
        /// </summary>
        /// <returns></returns>
        byte[] GetConstantCommand();
        /// <summary>
        /// 发送IO数据接口
        /// </summary>
        /// <param name="io"></param>
        /// <param name="senddata"></param>
        void Send(ICom io, byte[] senddata);

        /// <summary>
        /// 读取IO数据接口
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        byte[] Receive(ICom io);

        /// <summary>
        /// 同步运行设备（IO）
        /// </summary>
        /// <param name="io">io实例对象</param>
        void Run(ICom io);

        /// <summary>
        /// 同步运行设备（byte[]）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="channel"></param>
        /// <param name="revData">接收到的数据</param>
        void Run(string key, ICom channel, byte[] revData);

        /// <summary>
        /// 如果通讯正常，这个函数负责处理数据
        /// </summary>
        /// <param name="info"></param>
        //void Communicate(IRequestInfo info);

        /// <summary>
        /// 通讯中断，未接收到数据
        /// </summary>
       // void CommunicateInterrupt(IRequestInfo info);

        /// <summary>
        /// 通讯的数据错误或受到干扰
        /// </summary>
      //  void CommunicateError(IRequestInfo info);

        /// <summary>
        /// 通讯未知，默认状态（一般不用）
        /// </summary>
        void CommunicateNone();

        /// <summary>
        /// 当通讯实例为NULL的时候，调用该函数
        /// </summary>
        void UnknownIO();

        /// <summary>
        /// 检测通讯状态
        /// </summary>
        /// <param name="revdata"></param>
        /// <returns></returns>
        CommunicateState CheckCommunicateState(byte[] revdata);
        /// <summary>
        /// 通讯状态改变
        /// </summary>
        /// <param name="comState">改变后的状态</param>
        void CommunicateStateChanged(CommunicateState comState);

        /// <summary>
        /// 通道状态改变
        /// </summary>
        /// <param name="channelState"></param>
        void ChannelStateChanged(ChannelState channelState);
        /// <summary>
        /// 当软件关闭的时间，响应设备退出操作
        /// </summary>
        void Exit();
        /// <summary>
        /// 设备定时器，响应定时任务
        /// </summary>
        void OnRunTimer();
        /// <summary>
        /// 是否开启时钟，标识是否调用OnRunTimer接口函数。
        /// </summary>
        bool IsRunTimer { set; get; }
        /// <summary>
        /// 时钟间隔值，标识定时调用DeviceTimer接口函数的周期
        /// </summary>
        int RunTimerInterval { set; get; }
        /// <summary>
        /// 协议驱动
        /// </summary>
        IProtocolDriver<T> Protocol { get; }

        ControllerType ConbtrollerType { get; set; }
        /// <summary>
        /// 是否释放资源
        /// </summary>
        bool IsDisposed { get; }
    }
    public enum CommunicateState
    {
        //[EnumDescription("未知")]
        None = 0x00,
        //[EnumDescription("通讯中断")]
        Interrupt = 0x01,
        //[EnumDescription("通讯干扰")]
        Error = 0x02,
        //[EnumDescription("通讯正常")]
        Communicate = 0x03
    }
    public enum ChannelState
    {
        //[EnumDescription("未知")]
        None = 0x00,
        //[EnumDescription("打开")]
        Open = 0x01,
        //[EnumDescription("关闭")]
        Close = 0x02,
    }
    public enum ControllerType
    { 
        NT8001=8001,
        NT8036=8036
    }
}
