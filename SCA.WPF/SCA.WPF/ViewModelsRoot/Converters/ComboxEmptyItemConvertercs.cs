using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Dynamic;
using System.Collections;
using System.Globalization;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/16 11:25:21
* FileName   : ComboxEmptyItemConvertercs
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Converters
{
    ///注意：Combox需要指定DisplayMemberPath.如果此属性未设置，Empty item将默认显示为ToString()值
    ///     如在EmptyItem中重写ToString()方法，将无法应用在int,double,bool中。可用DisplayMemberpath作为另一种选项
    ///     为以为combobox建立一个itemtemplate
    ///     SelectedValuePath同样需要设定，由于EmptyItem总是返回null,此属性必须可为空.如果为primitive或struct时，此值
    ///     的设置不会更新至source.
    ///     以上两种弊端可能过在SelectedValue属性中增加一个“ValueConvert”解决，此Convert有能力将null值转换为默认值。
    /// <summary>
    /// 
    /// </summary>
    public class ComboxEmptyItemConverters:IValueConverter
    {
        /// <summary>
        /// this object is the empty item in the combobox. A dynamic object that
        /// returns null for all property request.
        /// </summary>
        private class EmptyItem : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                // just set the result to null and return true
                result = "--未选择--";
                //result =null;
                return true;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // assume that the value at least inherits from IEnumerable
            // otherwise we cannot use it.
            IEnumerable container = value as IEnumerable;
            if (container != null)
            {
                // everything inherits from object, so we can safely create a generic IEnumerable
                IEnumerable<object> genericContainer = container.OfType<object>();
                // create an array with a single EmptyItem object that serves to show en empty line
                IEnumerable<object> emptyItem = new object[] { new EmptyItem() };
                // use Linq to concatenate the two enumerable
                return emptyItem.Concat(genericContainer);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ComboxEmptyItemForSelectedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable container = value as IEnumerable;
            if (container != null)
            {
                // everything inherits from object, so we can safely create a generic IEnumerable
                IEnumerable<object> genericContainer = container.OfType<object>();
                // create an array with a single EmptyItem object that serves to show en empty line
                
                // use Linq to concatenate the two enumerable
                string s = "welcome";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() != "--未选择--")
            {
                return value;
            }
            return 9999;
            //throw new NotImplementedException();
        }
    }
    public class DeviceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {                
                SCA.BusinessLib.BusinessLogic.ControllerConfigBase config = new BusinessLib.BusinessLogic.ControllerConfigBase();
                SCA.Model.DeviceType dType=config.GetALLDeviceTypeInfo(null).Where((t) => t.Code == System.Convert.ToInt16(value)).FirstOrDefault<SCA.Model.DeviceType>();
                return dType.Name;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
