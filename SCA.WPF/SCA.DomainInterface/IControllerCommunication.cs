using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface
{
    public delegate bool StartDownLoadData(object sender,EventArgs e);
    /// <summary>
    /// 管理控制器的基本信息
    /// １.接收数据
    /// ２.回复数据
    /// ３.根据数据调用恰当的“控制器通讯”处理类 
    /// </summary>
    public interface IControllerCommunication
    {
        event StartDownLoadData startDownLoadData;
        #region 串口通信
        /// <summary>
        /// 设置串口参数
        /// </summary>
        /// <returns></returns>
        bool SetSerialPortParams(string PortName,int BaudRate);
        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <returns></returns>
        string  PortName{get;set;}
        int  BaudRate {get;set;}
        bool OpenPort();
        bool ClosePort();
        /// <summary>
        /// 测试控制器通讯状态
        /// </summary>
        /// <returns></returns>
        bool TestControllerStatus();
        

        #endregion
        
        #region 数据上传、下传
        /// <summary>
        /// 将数据下传至控制器
        /// </summary>
        /// <returns></returns>
        bool DownloadData();
        /// <summary>
        /// 将控制器的数据上传至软件中
        /// </summary>
        /// <returns></returns>
        bool UploadData();
        /// <summary>
        /// 停止下传数据
        /// </summary>
        /// <returns></returns>
        bool StopDownloadData();
        /// <summary>
        /// 停止上传数据
        /// </summary>
        /// <returns></returns>
        bool StopUploadData();
        #endregion 

    }
}
