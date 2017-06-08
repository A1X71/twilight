using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 9:29:11
* FileName   : ExtensionMethod
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Utility
{
    public static class ExtensionMethod
    {
        public static string GetPropertyName(this System.Reflection.MethodBase methodBase)
        {
            return methodBase.Name.Substring(4);
        }
    }
}
