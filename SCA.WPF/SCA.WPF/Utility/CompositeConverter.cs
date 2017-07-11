using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/8 12:19:16
* FileName   : CompositeConverter
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Utility
{
    [ContentProperty("Converters")]
    public class CompositeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (IValueConverter converter in Converters)
            {
                value = converter.Convert(value, targetType, parameter, culture);
                if (value == DependencyProperty.UnsetValue
                    || value == Binding.DoNothing)
                    break;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int index = Converters.Count - 1; index >= 0; index--)
            {
                value = Converters[index].ConvertBack(value, targetType,
                    parameter, culture);
                if (value == DependencyProperty.UnsetValue
                    || value == Binding.DoNothing)
                    break;
            }
            return value;
        }

        public List<IValueConverter> Converters
        {
            get { return mConverters; }
        }

        private List<IValueConverter> mConverters = new List<IValueConverter>();
    }
}
