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

namespace Test.WPF.DataGrid
{
    /// <summary>
    /// OperateWithDataGrid1.xaml 的交互逻辑
    /// </summary>
    public partial class OperateWithDataGrid1 : Window
    {
        public OperateWithDataGrid1()
        {
            InitializeComponent();
        }

   
    }
    public class MyDataGrid : System.Windows.Controls.DataGrid
    {

        public MyDataGrid(): base()
        {
            var ctxMenu = new ContextMenu();
            var menu1 = new MenuItem { Header = "Cut", Command = ApplicationCommands.Cut, CommandTarget = this };
            var menu2 = new MenuItem { Header = "Copy", Command = ApplicationCommands.Copy, CommandTarget = this };
            var menu3 = new MenuItem { Header = "Paste", Command = ApplicationCommands.Paste, CommandTarget = this };
            ctxMenu.Items.Add(menu2);
            ctxMenu.Items.Add(menu3);
            ctxMenu.Items.Add(menu1);
            this.ContextMenu = ctxMenu;
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, new ExecutedRoutedEventHandler(MyExecutedRoutedEventHandler), new CanExecuteRoutedEventHandler(CanExecuteRoutedEventHandler)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, new ExecutedRoutedEventHandler(MyExecutedRoutedEventHandler), new CanExecuteRoutedEventHandler(CanExecuteRoutedEventHandler)));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, new ExecutedRoutedEventHandler(MyExecutedRoutedEventHandler), new CanExecuteRoutedEventHandler(CanExecuteRoutedEventHandler)));
        }



        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {

            base.OnMouseRightButtonUp(e);
            if (this.ContextMenu != null)
            {
                this.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }



        private void MyExecutedRoutedEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var i = e.Parameter;
        }



        private void CanExecuteRoutedEventHandler(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = true;

        }

    }



    public class Employee
    {

        public string Name { get; set; }

        public string Gender { get; set; }

    }
}
