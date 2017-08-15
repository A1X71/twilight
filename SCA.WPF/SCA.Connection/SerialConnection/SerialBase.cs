/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 249 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-09 13:11:06 +0800 (周三, 09 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Neat.Dennis.Common.LoggerManager;
using System.IO.Ports;

namespace Neat.Dennis.Connection
{
    /// <summary>
    /// 串口基类
    /// </summary>
    public class SerialBase
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void ReceiveMessageHandler(NTP ntp);
        public event ReceiveMessageHandler ReceiveMsgEvent;

        private NTPReceiveMsg receiveMessageInfo = new NTPReceiveMsg();
        private SerialPort serialPort = new SerialPort();
        private int BaudRate = 9600;
        private int dataBits = 8;
        private Parity parity = Parity.None;
        private StopBits stopBits = StopBits.One;
        private int bufferSize = 8192;
        private int timeOut = 1000;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serialName"></param>
        /// <param name="baudRate"></param>
        public SerialBase()
        {
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(ReadComplete);
        }

        /// <summary>
        /// 连接串口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baudRate"></param>
        public bool Connect(string serialName, int baudrate)
        {
            bool resutl = false;
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                InitSerialPort();
                serialPort.BaudRate = baudrate;
                serialPort.PortName = serialName;

                serialPort.Open();
                resutl = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return resutl;
        }

        /// <summary>
        /// 初始化串口信息
        /// </summary>
        private void InitSerialPort()
        {
            if (serialPort != null)
            {
                serialPort.BaudRate = BaudRate;
                serialPort.DataBits = dataBits;
                serialPort.Parity = parity;
                serialPort.StopBits = stopBits;
                serialPort.WriteBufferSize = bufferSize;
                serialPort.WriteTimeout = timeOut;
            }
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadComplete(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (sender.GetType() != typeof(System.IO.Ports.SerialPort))
            {
                return;
            }
            System.IO.Ports.SerialPort comPort = (System.IO.Ports.SerialPort)sender;

            try
            {
                comPort.ReceivedBytesThreshold = comPort.ReadBufferSize;
                int bytesRead = comPort.BytesToRead;
                if (bytesRead == 0)
                {
                    comPort.ReceivedBytesThreshold = 1;
                    return;
                }
                else
                {
                    byte[] byteMessage = new byte[bytesRead];
                    comPort.Read(byteMessage, 0, byteMessage.Length);

                    logger.Warn(string.Format("Receive Cmd:{0}", BytesHelper.BytesToHexStr(byteMessage)));
                    if (receiveMessageInfo.OverReceiveBytes != null && receiveMessageInfo.OverReceiveBytes.Length > 0)
                    {
                        receiveMessageInfo.TempReceiveBytes = new byte[receiveMessageInfo.OverReceiveBytes.Length + bytesRead];
                        Array.Copy(receiveMessageInfo.OverReceiveBytes, 0, receiveMessageInfo.TempReceiveBytes, 0, receiveMessageInfo.OverReceiveBytes.Length);
                        Array.Copy(byteMessage, 0, receiveMessageInfo.TempReceiveBytes, receiveMessageInfo.OverReceiveBytes.Length, bytesRead);
                    }
                    else
                    {
                        receiveMessageInfo.TempReceiveBytes = new byte[bytesRead];
                        Array.Copy(byteMessage, receiveMessageInfo.TempReceiveBytes, bytesRead);
                    }

                    receiveMessageInfo.IsCanRunNext = true;
                    while (receiveMessageInfo.IsCanRunNext)
                    {
                        if (receiveMessageInfo.TempReceiveBytes == null)
                        {
                            receiveMessageInfo.TempReceiveBytes = receiveMessageInfo.OverReceiveBytes;
                            receiveMessageInfo.OverReceiveBytes = null;
                        }
                        NTPParseReceiveMsg.ParseResponseMessage(receiveMessageInfo);
                        if (receiveMessageInfo.IsReadNTPFinished)
                        {
                            NTP ntp = receiveMessageInfo.ntp;
                            receiveMessageInfo.ntp = new NTP();
                            receiveMessageInfo.IsReadNTPFinished = false;
                            receiveMessageInfo.IsReadNTPHeaderFinished = false;
                            bool check = ntp.CheckXORSUM();
                            if (check)
                            {
                                if (ReceiveMsgEvent != null)
                                {
                                    byte[] bytes = ntp.ToBytes();
                                    ReceiveMsgEvent(ntp);
                                }
                            }
                            else
                            {
                                logger.Error("Receive message checked error!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            finally
            {
                comPort.ReceivedBytesThreshold = 1;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="datas"></param>
        public void SendMessage(byte[] datas)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(datas, 0, datas.Length);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public bool Disconnect()
        {
            bool result = false;
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return result;
        }




    }
}
