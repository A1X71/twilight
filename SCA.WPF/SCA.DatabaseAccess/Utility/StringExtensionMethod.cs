using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 14:32:30
* FileName   : StringExtensionMethod
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.Utility
{
    public static class StringExtensionMethod
    {
        public static Nullable<T>  ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                { 
                    TypeConverter conv =TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch
            { }
            return result;
        }

    }
}
