using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/17 16:00:52
* FileName   : ExtendMethod
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtendMethod
    {
        /// <summary>
        /// 扩展String类,空值转为0
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string NullToZero(this string s)
        {
            if (s == null)
                return "0";
            else if (s == "")
                return "0";
            else
                return s;
        }
        /// <summary>
        /// 扩展String类,空值转为9999
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string NullToImpossibleValue(this string s)
        {
            if (s == null)
                return "9999";
            else if (s == "")
                return "9999";
            else
                return s;
        }
        /// <summary>
        /// 判断字符串是否为空或空串
        /// </summary>
        /// <param name="type"></param>
        /// <returns>引用或内容为空时，返回true;否则返回false</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            if (s == null)
                return true;
            else if (s == "")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
    }
}
