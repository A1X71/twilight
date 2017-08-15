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
using System.Text;
using Neat.Dennis.Common.LoggerManager;
using System.Reflection;

namespace Neat.Dennis.Connection
{
    public static class NTPParseReceiveMsg
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 解析回应数据
        /// </summary>
        /// <param name="receiveInfo"></param>
        public static void ParseResponseMessage(NTPReceiveMsg receiveInfo)
        {
            try
            {
                receiveInfo.IsCanRunNext = false;
                if (!receiveInfo.IsReadNTPHeaderFinished)
                {
                    // 处理 不符合协议格式的协议
                    if(receiveInfo.TempReceiveBytes != null && receiveInfo.TempReceiveBytes.Length > 0)
                    {
                        if(receiveInfo.TempReceiveBytes[0] == 0x66)
                        {
                            receiveInfo.ntp.PState = NTPState.Successed;
                        }
                        else if(receiveInfo.TempReceiveBytes[0] == 0x33)
                        {
                            receiveInfo.ntp.PState = NTPState.Failed;
                        }

                        if(receiveInfo.ntp.PState != NTPState.Normal)
                        {
                            receiveInfo.IsReadNTPFinished = true;
                            byte[] bytes = new byte[receiveInfo.TempReceiveBytes.Length - 1];
                            Array.Copy(receiveInfo.TempReceiveBytes, 1, bytes, 0, bytes.Length);
                            receiveInfo.OverReceiveBytes = bytes;
                            receiveInfo.TempReceiveBytes = null;
                            return;
                        }
                    }

                    try
                    {
                        if (receiveInfo.TempReceiveBytes == null
                            || receiveInfo.TempReceiveBytes.Length < receiveInfo.ntp.HeaderLength)
                        {
                            receiveInfo.IsReadNTPFinished = false;
                            receiveInfo.IsReadNTPHeaderFinished = false;
                            receiveInfo.OverReceiveBytes = receiveInfo.TempReceiveBytes;
                            receiveInfo.TempReceiveBytes = null;
                            return;
                        }
                        else if (receiveInfo.TempReceiveBytes.Length > receiveInfo.ntp.HeaderLength)
                        {
                            receiveInfo.OverReceiveBytes = new byte[(receiveInfo.TempReceiveBytes.Length - receiveInfo.ntp.HeaderLength)];
                            Array.Copy(receiveInfo.TempReceiveBytes, receiveInfo.ntp.HeaderLength, receiveInfo.OverReceiveBytes, 0, (receiveInfo.TempReceiveBytes.Length - receiveInfo.ntp.HeaderLength));
                        }
                        else
                        {
                            receiveInfo.OverReceiveBytes = null;
                        }
                    
                        if(receiveInfo.TempReceiveBytes[0] == 0xAA && receiveInfo.TempReceiveBytes[1] == 0x55)
                        {
                            receiveInfo.ntp.TransType = (TransType)receiveInfo.TempReceiveBytes[2];
                            receiveInfo.ntp.XOR = receiveInfo.TempReceiveBytes[3];
                            receiveInfo.ntp.SUM = receiveInfo.TempReceiveBytes[4];
                            receiveInfo.ntp.Length = receiveInfo.TempReceiveBytes[5];
                            receiveInfo.ntp.Command = (CommandType)receiveInfo.TempReceiveBytes[6];
                        }
                        else
                        {
                            logger.Error("Receive message error.");
                            receiveInfo.IsReadNTPFinished = false;
                            receiveInfo.IsReadNTPHeaderFinished = false;
                            receiveInfo.OverReceiveBytes = null;
                            receiveInfo.TempReceiveBytes = null;
                            receiveInfo.IsCanRunNext = false;
                            receiveInfo.ntp = new NTP();
                        }
                        
                        receiveInfo.IsReadNTPHeaderFinished = true;
                        receiveInfo.TempReceiveBytes = receiveInfo.OverReceiveBytes;
                        receiveInfo.OverReceiveBytes = null;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                        throw ex;
                    }
                }

                try
                {
                    if (receiveInfo.ntp.Length - 1 > 0)
                    {
                        if (receiveInfo.TempReceiveBytes != null)
                        {
                            byte[] byteMessage = receiveInfo.TempReceiveBytes;
                            int contentLenth = receiveInfo.ntp.Length - 1;

                            int alreadyReadCount = 0;
                            if (receiveInfo.AlreadyReadBytes != null)
                            {
                                alreadyReadCount = receiveInfo.AlreadyReadBytes.Length;
                            }
                            if (receiveInfo.AlreadyReadBytes != null && alreadyReadCount > 0)
                            {
                                if ((byteMessage.Length + alreadyReadCount) >= contentLenth)
                                {
                                    if ((byteMessage.Length + alreadyReadCount) > contentLenth)
                                    {
                                        byte[] overByteArray = new byte[byteMessage.Length - (contentLenth - alreadyReadCount)];
                                        Array.Copy(byteMessage, (contentLenth - alreadyReadCount), overByteArray, 0, byteMessage.Length - (contentLenth - alreadyReadCount));
                                        receiveInfo.OverReceiveBytes = overByteArray;
                                        if (overByteArray.Length >= receiveInfo.ntp.HeaderLength)
                                        {
                                            receiveInfo.IsCanRunNext = true;
                                        }
                                        else if(overByteArray.Length > 0)
                                        {
                                            if(overByteArray[0] == 0x66 || overByteArray[0] == 0x33)
                                            {
                                                receiveInfo.IsCanRunNext = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        receiveInfo.OverReceiveBytes = null;
                                    }
                                    byte[] contentByteArray = new byte[contentLenth];
                                    Array.Copy(receiveInfo.AlreadyReadBytes, contentByteArray, alreadyReadCount);
                                    Array.Copy(byteMessage, 0, contentByteArray, alreadyReadCount, (contentLenth - alreadyReadCount));

                                    receiveInfo.ntp.Content = contentByteArray;
                                    receiveInfo.AlreadyReadBytes = null;
                                    receiveInfo.IsReadNTPFinished = true;
                                    receiveInfo.IsReadNTPHeaderFinished = false;
                                }
                                else
                                {
                                    byte[] readBytes = new byte[byteMessage.Length + alreadyReadCount];
                                    Array.Copy(receiveInfo.AlreadyReadBytes, readBytes, alreadyReadCount);
                                    Array.Copy(byteMessage, 0, readBytes, alreadyReadCount, byteMessage.Length);
                                    receiveInfo.AlreadyReadBytes = readBytes;
                                }
                            }
                            else
                            {
                                if (byteMessage.Length >= contentLenth)
                                {
                                    byte[] contentByteArray = new byte[contentLenth];
                                    Array.Copy(byteMessage, contentByteArray, contentLenth);
                                    receiveInfo.OverReceiveBytes = null;
                                    if (byteMessage.Length > contentLenth)
                                    {
                                        byte[] overByteArray = new byte[byteMessage.Length - contentLenth];
                                        Array.Copy(byteMessage, contentLenth, overByteArray, 0, byteMessage.Length - contentLenth);
                                        receiveInfo.OverReceiveBytes = overByteArray;
                                    }
                                    receiveInfo.ntp.Content = contentByteArray;
                                    receiveInfo.AlreadyReadBytes = null;
                                    receiveInfo.IsReadNTPFinished = true;
                                    receiveInfo.IsReadNTPHeaderFinished = false;
                                    if (receiveInfo.OverReceiveBytes != null && receiveInfo.OverReceiveBytes.Length > receiveInfo.ntp.HeaderLength)
                                    {
                                        receiveInfo.IsCanRunNext = true;
                                    }
                                    else if (receiveInfo.OverReceiveBytes != null && receiveInfo.OverReceiveBytes.Length > 0)
                                    {
                                        if (receiveInfo.OverReceiveBytes[0] == 0x66 || receiveInfo.OverReceiveBytes[0] == 0x33)
                                        {
                                            receiveInfo.IsCanRunNext = true;
                                        }
                                    }
                                }
                                else
                                {
                                    byte[] readBytes = new byte[byteMessage.Length];
                                    Array.Copy(byteMessage, readBytes, byteMessage.Length);
                                    receiveInfo.AlreadyReadBytes = readBytes;
                                }
                            }
                        }
                    }
                    else
                    {
                        receiveInfo.IsReadNTPFinished = true;
                        receiveInfo.OverReceiveBytes = receiveInfo.TempReceiveBytes;
                        if (receiveInfo.OverReceiveBytes != null && receiveInfo.OverReceiveBytes.Length > receiveInfo.ntp.HeaderLength)
                        {
                            receiveInfo.IsCanRunNext = true;
                        }
                        else if (receiveInfo.OverReceiveBytes != null && receiveInfo.OverReceiveBytes.Length > 0)
                        {
                            if (receiveInfo.OverReceiveBytes[0] == 0x66 || receiveInfo.OverReceiveBytes[0] == 0x33)
                            {
                                receiveInfo.IsCanRunNext = true;
                            }
                        }
                    }
                    receiveInfo.TempReceiveBytes = null;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
