using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Reflection;
using SCA.WPF.Utility;
using System.Windows.Data;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 9:21:10
* FileName   : NavigatorItemViewModel
* Description: 导航树节点信息
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Navigator
{
    public class NavigatorItemViewModel:PropertyChangedBase
    {
         #region TreeItemWrapper
        private CollectionView _children;
        private bool _isExpanded=true;
        private bool _isSelected;
        private object _dataItem;

        public NavigatorItemViewModel(object dataItem)
        {
            DataItem = dataItem;
        }

        public object DataItem 
        {
            get
            {
                return _dataItem;
            }
            private set
            {
                _dataItem = value;
            //    NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public NavigatorItemViewModel Parent { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public System.Windows.Media.Brush ControllerForeColor
        {
            get
            {
                if (DataItem != null)
                {
                    if (DataItem.GetType().ToString() == "SCA.Model.ControllerModel")
                    {
                        if(((SCA.Model.ControllerModel)DataItem).PrimaryFlag)
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                        else
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);

                    }
                }
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            }
        }
        public System.Windows.Data.CollectionView Children
        {
            get { return _children; }
            set { _children = value;
       //     NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        #endregion
    }
    public class PrimaryControllerColorSelector : IValueConverter
    {
        private readonly System.Windows.Media.Color _primaryControllerColor = System.Windows.Media.Colors.Red;
        public object Convert(object value, Type targetType,object parameter,System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return new System.Windows.Media.SolidColorBrush(_primaryControllerColor);
                }
                else
                {
                    return Binding.DoNothing;
                }
            }
            return Binding.DoNothing;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
             
    }
}
