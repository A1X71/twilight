using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Globalization;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/24 12:00:07
* FileName   : StringToBrushConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Utility
{
    public  class StringToBrushConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            dynamic objValue = value;
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(objValue.Name));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
