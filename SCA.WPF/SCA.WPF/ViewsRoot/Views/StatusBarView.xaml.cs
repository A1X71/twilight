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
    /// StatusBarView.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBarView : UserControl
    {
        public StatusBarView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent UpdateProgressBarStatusEvent = EventManager.RegisterRoutedEvent
        (
            "UpdateProgressBarStatus",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(StatusBarView)
        );
        public event RoutedEventHandler UpdateProgressBarStatus
        {
            add { AddHandler(UpdateProgressBarStatusEvent,value); }
            remove { RemoveHandler(UpdateProgressBarStatusEvent, value); }
        }
    }
}
