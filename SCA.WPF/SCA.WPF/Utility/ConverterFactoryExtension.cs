using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Diagnostics;
using System.Reflection;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/8 12:23:22
* FileName   : ConverterFactoryExtension
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Utility
{
public class ConverterFactoryExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type type = assembly.GetType(ConverterName);
        if (type != null)
        {
            ConstructorInfo defCons = GetDefaultConstructor(type);
            if (defCons != null)
                return defCons.Invoke(new object[] {});
        }
        return null;
    }

    private ConstructorInfo GetDefaultConstructor(Type type)
    {
        ConstructorInfo[] infos = type.GetConstructors();
        foreach (ConstructorInfo info in infos)
        {
            ParameterInfo[] paramInfos = info.GetParameters();
            if (paramInfos.Length == 0)
                return info;
        }
        return null;
    }

    public string ConverterName { get; set; }
}
}
