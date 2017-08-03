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
    /// AddMoreLinesView.xaml 的交互逻辑
    /// </summary>
    public partial class AddMoreLinesView : UserControl
    {
        public AddMoreLinesView()
        {
            InitializeComponent();
            txtAmount.Text = "";
        }
        public static readonly RoutedEvent ConfirmEvent = EventManager.RegisterRoutedEvent("ConfirmClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddMoreLinesView));
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("CloseClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddMoreLinesView));
        public event RoutedEventHandler ConfirmClick
        {
            add { AddHandler(ConfirmEvent, value); }
            remove { RemoveHandler(ConfirmEvent, value); }
        }
        public event RoutedEventHandler CloseClick
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs(CloseEvent, this.txtAmount.Text));
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ConfirmEvent, this.txtAmount.Text));
        }
    }
}
