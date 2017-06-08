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
using System.Windows.Shapes;
using Test.WPF.NavigatingViews.ViewModel;
namespace Test.WPF.NavigatingViews.View
{
    /// <summary>
    /// MainWindows.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindows : Window
    {
        public MainWindows()
        {
            InitializeComponent();
            MainViewModel vm = new MainViewModel();
            //vm.NavigateToView(new PortofolioViewModel());
            this.DataContext = vm;
        }
    }
}
