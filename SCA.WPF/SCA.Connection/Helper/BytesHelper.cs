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
    /// byte 转化
    /// </summary>
    public class BytesHelper
    {
        /// <summary>
        /// byte to int
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ByteToInt(byte b)
        {
            int value = (int)((b & 0xFF));
            return value;
        }

        /// <summary>
        /// int to byte
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte IntToOneByte(int i)
        {
            byte value = (byte)((i & 0xFF));
            return value;
        }

        /// <summary>
        /// int? to byte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte IntNullAbleToByte(int? i)
        {
            if (i == null)
            {
                return 0x00;
            }
            byte value = (byte)((i & 0xFF));
            return value;
        }


        /// <summary>
        /// short to byte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte ShortToOneByte(short s)
        {
            byte value = (byte)((s & 0xFF));
            return value;
        }

        /// <summary>
        /// short? to byte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte ShortNullAbleToByte(short? s)
        {
            if(s == null)
            {
                return 0x00;
            }
            byte value = (byte)((s & 0xFF));
            return value;
        }

        /// <summary>
        /// float? to byte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte FloatNullAbleToByte(float? f)
        {
            int i = 0;
            if (f == null)
            {
                return 0x00;
            }
            i = (int)f;
            byte value = (byte)((i & 0xFF));
            return value;
        }

        /// <summary>
        /// hex string to byte
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns
        public static byte[] HexStringToBytes(string hex)
        {
            string value = hex;
            if (hex.Contains("0x"))
            {
                value = hex.Replace("0x", "");
            }
            int len = (value.Length / 2);
            byte[] result = new byte[len];
            char[] achar = value.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                int pos = i * 2;
                result[i] = (byte)(toByte(achar[pos]) << 4 | toByte(achar[pos + 1]));
            }
            return result;
        }

        /// <summary>
        /// toByte
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static int toByte(char c)
        {
            byte b = (byte)"0123456789ABCDEF".IndexOf(c);
            return b;
        }

        /// <summary>
        /// bytes to hex string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }
}
