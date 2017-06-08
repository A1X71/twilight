using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/6 15:34:39
* FileName   : BytesUtility
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public static class BytesUtility
    {
        public static string ToHexString(this byte[] data)
        {
            if (data == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder(100);
            foreach (var item in data)
            {
                builder.AppendFormat("{0:X2} ", item);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        public static string GetString(byte[] data)
        {
#if SILVERLIGHT
            return System.Text.Encoding.Unicode.GetString(data, 0, data.Length);
#else
            return System.Text.Encoding.Unicode.GetString(data);
#endif
        }
        /// <summary>
        /// 获取GBK编码字符串
        /// </summary>
        public static string GetGBKString(byte[] data)
        {
#if SILVERLIGHT
            return GBKEncoder.Read(data);
#else
            return System.Text.Encoding.GetEncoding("gbk").GetString(data);
#endif
        }
        /// <summary>
        /// 获取字节序
        /// </summary>
        public static byte[] GetBytes(string value)
        {
            return System.Text.Encoding.Unicode.GetBytes(value);
        }
        /// <summary>
        /// 获取GBK字节序
        /// </summary>
        public static byte[] GetGBKBytes(string value)
        {
#if SILVERLIGHT
            return GBKEncoder.ToBytes(value);
#else
            return System.Text.Encoding.GetEncoding("gbk").GetBytes(value);
#endif
        }
        /// <summary>
        /// 获取时间字节序
        /// </summary>
        public static byte[] GetDateTimeBytes(DateTime datetime)
        {
            var bytes = new byte[6];
            bytes[0] = byte.Parse(datetime.Year.ToString("0000").Substring(2, 2));
            bytes[1] = (byte)datetime.Month;
            bytes[2] = (byte)datetime.Day;
            bytes[3] = (byte)datetime.Hour;
            bytes[4] = (byte)datetime.Minute;
            bytes[5] = (byte)datetime.Second;

            return bytes;
        }
        /// <summary>
        /// 获取时间字节序
        /// </summary>
        public static byte[] GetDateTimeBytes(string datetime)
        {
            var bytes = new byte[6];
            if (!string.IsNullOrWhiteSpace(datetime))
            {
                bytes[0] = byte.Parse(datetime.Substring(0, 2));
                bytes[1] = byte.Parse(datetime.Substring(2, 2));
                bytes[2] = byte.Parse(datetime.Substring(4, 2));
                bytes[3] = byte.Parse(datetime.Substring(6, 2));
                bytes[4] = byte.Parse(datetime.Substring(8, 2));
                bytes[5] = byte.Parse(datetime.Substring(10, 2));
            }

            return bytes;
        }
        /// <summary>
        /// 获取字节序
        /// </summary>
        public static byte[] GetBytes(int value)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(value >> 0x18);
            bytes[1] = (byte)(value >> 0x10);
            bytes[2] = (byte)(value >> 8);
            bytes[3] = (byte)(value);

            return bytes;
        }
        /// <summary>
        /// 获取字节序
        /// </summary>
        public static byte[] GetBytes(ushort value)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)(value);

            return bytes;
        }
        /// <summary>
        /// 获取字节序
        /// </summary>
        public static byte[] GetBytes(UInt32 value)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(value >> 0x18);
            bytes[1] = (byte)(value >> 0x10);
            bytes[2] = (byte)(value >> 8);
            bytes[3] = (byte)(value);

            return bytes;
        }
        /// <summary>
        /// 获取某位是否为1
        /// </summary>
        public static bool GetBitStatus(uint value, int index)
        {
            return ((value >> index) & 0x01) == 0x01;
        }

        /// <summary>
        /// 获取校验码
        /// </summary>
        public static byte XOR(byte[] raw, int lenght)
        {
            byte A = 0;
            for (int i = 0; i < lenght; i++)
            {
                A ^= raw[i];
            }
            return A;
        }
        /// <summary>
        /// 获取校验码
        /// </summary>
        public static byte XOR(byte[] raw, int index, int lenght)
        {
            byte A = 0;
            for (int i = index; i < index + lenght; i++)
            {
                A ^= raw[i];
            }
            return A;
        }

    }
}
