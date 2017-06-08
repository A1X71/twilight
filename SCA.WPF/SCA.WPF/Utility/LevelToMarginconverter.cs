using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.WPF.Utility
{
    /// <summary>
    /// 根据设定的级别，控制缩进量
    /// </summary>
    public class LevelToMarginconverter:System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var level = (int)value;
            return new System.Windows.Thickness(8 * level + 10 * (level - 1), 0, 0, 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
