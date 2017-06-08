using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 15:58:08
* FileName   : ExtensionMethod
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Utility
{
   public static class ExtendMethod
    {
        public static string GetPropertyName(this System.Reflection.MethodBase methodBase)
        {
            return methodBase.Name.Substring(4);
        }
    }
}
