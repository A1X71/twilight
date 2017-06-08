using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 15:03:04
* FileName   : NavigatorDataTemplateSelector
* Description: 数据模板选择
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Navigator
{
    public class NavigatorDataTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate retval = null;
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is NavigatorItemViewModel)
            {
                NavigatorItemViewModel hierarchyItem = item as NavigatorItemViewModel;
                if (hierarchyItem.DataItem != null)
                {

                    if (hierarchyItem.DataItem.GetType() == typeof(ProjectModel))
                    {
                        retval = element.FindResource("ProjectTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(ControllerModel))
                    {
                        retval = element.FindResource("ControllerTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(ControllerNodeModel))
                    {
                        retval = element.FindResource("ControllerNodeTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(LoopModel))
                    {
                        retval = element.FindResource("LoopTemplate") as DataTemplate;
                    }
                }
            }
            return retval;
        }
    }
}
