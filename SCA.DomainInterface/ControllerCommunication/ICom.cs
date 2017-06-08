using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
namespace SCA.Interface.ControllerCommunication
{
    public interface ICom:IDisposable
    {
        #region 属性
        /// <summary>
        /// 是否打开
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 串口号
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 波特率
        /// </summary>
        int Baud { get; }

        /// <summary>
        /// 数据位
        /// </summary>
        int DataBits { get; }

        /// <summary>
        /// 停止位
        /// </summary>
        StopBits StopBits { get; }

        /// <summary>
        /// 检验位
        /// </summary>
        Parity Parity { get; }
        #endregion

        #region Com method
        /// <summary>
        /// 尝试打开串口
        /// </summary>
        void Open();

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int InternalRead(byte[] data);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int InternalRead(byte[] data, int offset, int length);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int InternalWrite(byte[] data);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int InternalWrite(byte[] data, int offset, int length);

        /// <summary>
        /// 清空发送和接收缓冲区
        /// </summary>
        /// <returns></returns>
        void ClearBuffer();

        /// <summary>
        /// 配置串口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baud"></param>
        /// <returns></returns>
        bool Settings(int baud);

        /// <summary>
        /// 配置串口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baud"></param>
        /// <param name="databits"></param>
        /// <param name="stopbits"></param>
        /// <param name="parity"></param>
        /// <returns></returns>
        bool Settings(int baud, int databits, StopBits stopbits, Parity parity);
        
        #endregion

        #region common method
        /// <summary>
        /// 读IO;
        /// </summary>
        /// <returns></returns>
        byte[] Read();

        /// <summary>
        /// 写IO
        /// </summary>
        int Write(byte[] data);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 是否被释放了
        /// </summary>
        bool IsDisposed { get; }
        #endregion

        #region Com 事件
        /// <summary>
        /// 串口打开事件
        /// </summary>
        //event COMOpenHandler COMOpen;

        ///// <summary>
        ///// 串口打开事件
        ///// </summary>
        //event COMCloseHandler COMClose;

        ///// <summary>
        ///// 串口异常
        ///// </summary>
        //event COMErrorHandler COMError;
        #endregion
    }
}
