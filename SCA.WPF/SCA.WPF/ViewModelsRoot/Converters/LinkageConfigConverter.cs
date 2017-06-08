using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Windows.Data;
using System.Collections.Generic;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/21 15:07:59
* FileName   : LinkageConfigConverter
* Description: 未用
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Converters
{
    public class LinkageConfigConverter:IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType().ToString() == "SCA.Model.LinkageType")
                return null;
            List<string> lstReturn = new List<string>();
            if (value != null)
            {
                foreach (var v in (List<LinkageType>)value)
                {
                    LinkageType type = (LinkageType)v;
                    switch (type)
                    {
                        case LinkageType.SameLayer:
                            lstReturn.Add("同层");
                            break;
                        case LinkageType.Address:
                            lstReturn.Add("地址");
                            break;
                        case LinkageType.AdjacentLayer:
                            lstReturn.Add("邻层");
                            break;
                        case LinkageType.ZoneLayer:
                            lstReturn.Add("区层");
                            break;
                    }
                }
            }
            return lstReturn;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string source = (string)value;
                switch (source)
                {
                    case "同层":
                        return LinkageType.SameLayer;
                    case "邻层":
                        return LinkageType.AdjacentLayer;
                    case "区层":
                        return LinkageType.ZoneLayer;
                    case "地址":
                        return LinkageType.Address;

                }
            }
            return LinkageType.None;
        }
    }
}
