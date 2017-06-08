using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Caliburn.Micro;
using System.Windows.Input;
using Test.WPF.Utility;
using System.Windows.Data;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/4 13:12:07
* FileName   : MainViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.AnotherNavigatingViews.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        public ICommand SwitchViewsCommand { get;  set; }
 
        private Dictionary<string, UserControl> _dictControls;
        public MainViewModel()
        {
            SwitchViewsCommand = new RelayCommand<string>(SetCurrentView);
            Views.UCClients clientUC=new Views.UCClients();
            Views.UCProducts productUC=new Views.UCProducts();
           _dictControls = new Dictionary<string, UserControl>();
           _dictControls.Add("clientUC", clientUC);
           _dictControls.Add("productUC", productUC);
        }
        public void SetCurrentView(string usercontrolName)
        {
            CurrentView = _dictControls[usercontrolName];
        }
        public void SetCurrentView2()
        {
            CurrentView = new Views.UCClients();
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                if (value != _currentView)
                {
                    _currentView = value;
                    NotifyOfPropertyChange("CurrentView");
                }
            }
        }

    }
    public class InstanceEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (parameter as Type).IsInstanceOfType(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Activator.CreateInstance(parameter as Type) : Binding.DoNothing;
        }
    }


}
