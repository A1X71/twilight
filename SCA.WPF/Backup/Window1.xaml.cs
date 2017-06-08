using System.Windows;
using System.Windows.Controls;

namespace CheckBoxTest
{
    public partial class Window1
    {
        public Window1()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            ((RadioButton) ((MenuItem) sender).Icon).IsChecked = true;
        }
    }
}