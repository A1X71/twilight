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
*  $Revision: 250 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-11 15:33:06 +0800 (周五, 11 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neat.Dennis.Connection
{
    /// <summary>
    /// Neat Protocal
    /// </summary>
    public class NTP
    {
        ///<remark>
        ///The Neat Protocal body has the following format:
        ///  0                   1                   2                   3
        ///  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        /// |    StartCode    0xAA 0x55     |   TransType   |     XOR       |
        /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        /// |      SUM      |    Length     |    Command    | Data Area.... |
        /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        /// </remark>

        #region Property
        /// <summary>
        /// 起始符 2Byte 
        /// </summary>
        public byte[] StartCode { get { return new byte[] { 0xAA, 0x55 }; } }

        /// <summary>
        /// 传输类型 1Byte 
        /// </summary>
        public TransType TransType { get; set; }

        /// <summary>
        /// 异或 1Byte
        /// </summary>
        public byte XOR { get; set; }

        /// <summary>
        /// 累加和 1Byte
        /// </summary>
        public byte SUM { get; set; }

        /// <summary>
        /// 长度 1Byte Content长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 命令 1Byte
        /// </summary>
        public CommandType Command { get; set; }
        /// <summary>
        ///  Data Area
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 协议头长度
        /// </summary>
        public int HeaderLength = 7;

        /// <summary>
        /// 协议状态
        /// </summary>
        public NTPState PState = NTPState.Normal;

        #endregion Property

        /// <summary>
        /// 获取协议数据
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if(PState == NTPState.Successed)
            {
                return new byte[] { 0x66 };
            }
            else if (PState == NTPState.Failed)
            {
                return new byte[] { 0x33 };
            }
            else
            {
                int len = 0;
                if(Content != null)
                {
                    len = Content.Length;
                }
                byte[] message = new byte[HeaderLength + len];
                Array.Copy(StartCode, 0, message, 0, StartCode.Length);
                message[2] = (byte)TransType;
                message[6] = (byte)Command;
                if (Content != null)
                {
                    Array.Copy(Content, 0, message, HeaderLength, Content.Length);
                    message[5] = (byte)(Content.Length + 1);
                }
                else
                {
                    message[5] = 0x01;
                }

                byte[] bytes = GetXORSUM();
                message[3] = bytes[0];
                message[4] = bytes[1];
                return message;
            }
        }

        /// <summary>
        /// 返回数据 异或 累加和
        /// </summary>
        /// <returns></returns>
        private byte[] GetXORSUM()
        {
            byte[] xorsum = new byte[2];
            xorsum[0] = (byte)Command;
            xorsum[1] = (byte)Command;
            if (Content != null)
            {
                foreach(byte b in Content)
                {
                    xorsum[0] ^= b;
                    xorsum[1] += b;
                }
            }
            return xorsum;
        }

        /// <summary>
        /// 返回数据 异或 累加和
        /// </summary>
        /// <returns>是否正确</returns>
        public bool CheckXORSUM()
        {
            bool result = false;
            byte[] xorsum = new byte[2];
            xorsum[0] = (byte)Command;
            xorsum[1] = (byte)Command;
            if (Content != null)
            {
                foreach (byte b in Content)
                {
                    xorsum[0] ^= b;
                    xorsum[1] += b;
                }
            }
            if (xorsum[0] == XOR && xorsum[1] == SUM)
            {
                result = true;
            }
            return result;
        }

    }

    /// <summary>
    /// NTP 协议状态
    /// </summary>
    public enum NTPState
    {
        Normal = 0,             //普通协议
        Successed = 1,          //成功
        Failed = 2              //失败
    }

    /// <summary>
    /// 传输类型
    /// </summary>
    public enum TransType
    {
        TransUp = 0xD1,
        TransDown = 0xDA
    }

    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CommandType
    {
        Patrol = 0xCA,                      //巡检
        Ready = 0xC9,                       //准备发送
        Over = 0xBA,                        //发送完毕
        Over8003 = 0xCF,                    //8003发送完毕
        Clear = 0xC2,                       //清除设置

        MachineTypeRequest = 0xB9,          //请求控制器类型
        MachineTypeResponose = 0xC8,        //回应控制器类型

        DeviceUp = 0xCC,                    //器件设置上传
        DeviceDown = 0xBB,                  //器件设置下传

        StandardUp = 0xCD,                  //标准组态上传
        StandardDown = 0xBC,                //标准组态下传

        MixedUp = 0xBD,                     //混合组态上传
        MixedDown = 0xBD,                   //混合组态下传

        GeneralUp = 0xBE,                   //通用组态上传
        GeneralDown = 0xBE,                 //通用组态下传

        ManualUp = 0xBF,                    //手控盘上传
        ManualDown = 0xBF,                  //手控盘下传
    }
}
