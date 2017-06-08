using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/6 15:38:10
* FileName   : BHexString
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    class BHexString
    {
//        1.请问c#中如何将十进制数的字符串转化成十六进制数的字符串

////十进制转二进制
//Console.WriteLine("十进制166的二进制表示: "+Convert.ToString(166, 2));
////十进制转八进制
//Console.WriteLine("十进制166的八进制表示: "+Convert.ToString(166, 8));
////十进制转十六进制
//Console.WriteLine("十进制166的十六进制表示: "+Convert.ToString(166, 16));
    
////二进制转十进制
//Console.WriteLine("二进制 111101 的十进制表示: "+Convert.ToInt32("111101", 2));
////八进制转十进制
//Console.WriteLine("八进制 44 的十进制表示: "+Convert.ToInt32("44", 8));
////十六进制转十进制
//Console.WriteLine("十六进制 CC的十进制表示: "+Convert.ToInt32("CC", 16));

//2.在串口通讯过程中，经常要用到 16进制与字符串、字节数组之间的转换
//

private string StringToHexString(string s,Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%"+Convert.ToString(b[i], 16);
            }
            return result;
        }
        private string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[]{'%'},StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

 

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
       /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
             hexString = hexString.Replace(" ", "");
           if ((hexString.Length % 2) != 0)
                 hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
         }

 

 

/// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
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

 

 

/// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="type"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
       /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                 s += " ";//空格
                //throw new ArgumentException("type is not valid chinese string!");
             }
             System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                     str += string.Format("{0}", ",");
                 }
             }
            return str.ToLower();
         }

 

 


///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
       /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
           if (hex == null)
                throw new ArgumentNullException("hex");
             hex = hex.Replace(",", "");
             hex = hex.Replace("\n", "");
             hex = hex.Replace("\\", "");
             hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                 hex += "20";//空格
             }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

           for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                     bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                     System.Globalization.NumberStyles.HexNumber);
                 }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                 }
             }
             System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
         }

 

//如果是枚举类。

   public enum AlarmStatus : uint
    {
        FAILURE_DRIVER_TIMEOUT = 0x00000004,
        COLLIDE_WARING = 0x00000008,
        CRITICAL_AREA = 0x00000010,
        MIGRATION_DRIVING_ROUTE = 0x00001000,
              SHAKE_ALARM = 0x00040000,
         TRAILER_ALARM = 0x00080000,
         WRONG_OPEN_DOOR = 0x00100000,
        WRONG_FIRE_UP = 0x00200000,
        PROHIBITED_ENTER_THE_AREA = 0x00400000,
       STOP_CAR_UNUSUAL = 0x01000000,
        HIGH_SPEED_ALARM = 0x02000000,
        HIJACK_ALARM = 0x04000000,
        OIL_NORMAL_ALARM = 0x08000000,
        LOW_SENSOR_TWO = 0x10000000,
          LOW_SENSOR_ONE = 0x20000000,
 
        HIGH_SENSOR_TWO = 0x40000000,

        HIGH_SENSOR_ONE = 0x80000000

 

    }

//  可以通过  枚举的 GetHashCode 方法，把16进制数转成有符号的32位整型

//如： AlarmStatus.HIGH_SENSOR_ONE.GetHashCode()
    }
}
