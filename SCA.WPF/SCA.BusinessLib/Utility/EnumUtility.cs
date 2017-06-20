using System.Reflection;
using System.ComponentModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/6/20 16:54:59
* FileName   : EnumUtility
* Description: 枚举操作
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public class EnumUtility
    {
        /// <summary>
        ///  根据属性描述值,取得枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetEnumName(System.Type value,string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                (typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return fi.Name;
                    }
                }
            }
            return description;
        }
    }
}
