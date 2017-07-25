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
    
    /// 注意（仅供参考）：Combox需要指定DisplayMemberPath.如果此属性未设置，Empty item将默认显示为ToString()值
    ///     如在EmptyItem中重写ToString()方法，将无法应用在int,double,bool中。可用DisplayMemberpath作为另一种选项
    ///     为combobox建立一个itemtemplate
    ///     同样需要设置SelectedValuePath属性，由于EmptyItem总是返回"未选择“,此属性必须可为空.如果为primitive或struct时，此值
    ///     的设置不会更新至source.
    ///     以上两种弊端可能过在SelectedValue属性中增加一个“ValueConvert”解决，此Convert有能力将null值转换为默认值。
    /// <summary>
    /// 
    /// </summary>
    public class ComboxEmptyItemConverters:IValueConverter
    {
        /// <summary>
        /// 空条目
        /// </summary>
        private class EmptyItem : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                //设置为“未选择”
                result = "--未选择--";                
                return true;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //value必须为IEnumerable类型
            IEnumerable container = value as IEnumerable;
            if (container != null)
            {                   
                IEnumerable<object> genericContainer = container.OfType<object>();
                //创建“--未选择--”条目
                IEnumerable<object> emptyItem = new object[] { new EmptyItem() };
                // 应用LINQ连接两个IEnumerable
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
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            { 
                if (value.ToString() != "--未选择--")
                {
                    return value;
                }
            }
            return 9999;            
        }
    }
    public class DeviceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {                
                SCA.BusinessLib.BusinessLogic.ControllerConfigNone config = new BusinessLib.BusinessLogic.ControllerConfigNone();
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
