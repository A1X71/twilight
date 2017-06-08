using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// WelcomeView.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        public static readonly RoutedEvent NewButtonClickEvent = EventManager.RegisterRoutedEvent("NewButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WelcomeView));
        public static readonly RoutedEvent ImportButtonClickEvent = EventManager.RegisterRoutedEvent("ImportButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WelcomeView));
        public static readonly RoutedEvent OpenButtonClickEvent = EventManager.RegisterRoutedEvent("OpenButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WelcomeView));
        public WelcomeView()
        {
            InitializeComponent();
        }
        
        public event RoutedEventHandler NewButtonClick
        {
            add { AddHandler(NewButtonClickEvent,value); }
            remove { RemoveHandler(NewButtonClickEvent, value); }
        }

        public event RoutedEventHandler ImportButtonClick
        {
            add { AddHandler(ImportButtonClickEvent, value); }
            remove { RemoveHandler(ImportButtonClickEvent, value); }
        }
        public event RoutedEventHandler OpenButtonClick
        {
            add { AddHandler(OpenButtonClickEvent, value); }
            remove { RemoveHandler(OpenButtonClickEvent, value); }
        }

        public void NewButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewButtonClickEvent));
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ImportButtonClickEvent));
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenButtonClickEvent));
        }
        
    }
}
