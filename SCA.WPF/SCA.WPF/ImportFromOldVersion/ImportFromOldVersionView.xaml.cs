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
    /// ImportFromOldVersionView.xaml 的交互逻辑
    /// </summary>
    public partial class ImportFromOldVersionView : UserControl
    {
        public ImportFromOldVersionView()
        {
            InitializeComponent();
            SCA.WPF.ImportFromOldVersion.ImportFromOldVersionViewModel vm = new ImportFromOldVersion.ImportFromOldVersionViewModel();
            this.DataContext = vm;
        }
        public static readonly RoutedEvent ConfirmButtonClickEvent = EventManager.RegisterRoutedEvent(
                "ConfirmButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImportFromOldVersionView));

        public static readonly RoutedEvent CloseButtonClickEvent = EventManager.RegisterRoutedEvent(
                "CloseButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImportFromOldVersionView));

        public event RoutedEventHandler ConfirmButtonClick
        {
            add { AddHandler(ConfirmButtonClickEvent, value); }
            remove { RemoveHandler(ConfirmButtonClickEvent, value); }
        }
        public event RoutedEventHandler CloseButtonClick
        {
            add { AddHandler(CloseButtonClickEvent, value); }
            remove { RemoveHandler(CloseButtonClickEvent, value); }
        }

   



        
    }
}
