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
    /// EditLoopsView.xaml 的交互逻辑
    /// </summary>
    public partial class EditLoopsView : UserControl
    {
        public EditLoopsView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent DeleteButtonClickEvent = EventManager.RegisterRoutedEvent(
                "DeleteButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditLoopsView)
                );
        public static readonly RoutedEvent CloseButtonClickEvent = EventManager.RegisterRoutedEvent(
                 "CloseButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditLoopsView)
                );

        public event RoutedEventHandler DeleteButtonClick
        {
            add { AddHandler(DeleteButtonClickEvent, value); }
            remove { RemoveHandler(DeleteButtonClickEvent, value); }
        }
        public event RoutedEventHandler CloseButtonClick
        {
            add { AddHandler(CloseButtonClickEvent, value); }
            remove { RemoveHandler(CloseButtonClickEvent, value); }
        }
        private void LoopInfoGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var chk = Utility.CustomVisualTreeHelper.FindAncestor<CheckBox>((DependencyObject)e.OriginalSource, "ckLoopsSelector");
            if (chk == null)                
            {
                e.Handled = true;
            }
        }

        private void ckLoopsSelector_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.Media.VisualTreeHelper.
            var chk = (CheckBox)sender;
            var row = Utility.CustomVisualTreeHelper.FindAncestor<DataGridRow>(chk);
            var newValue = !chk.IsChecked.GetValueOrDefault();
            row.IsSelected = newValue;
            chk.IsChecked = newValue;
            //将此事件标记为已处理，使DataGridPreviewMouseDown不处理此事件
            e.Handled = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                SCA.Model.ControllerModel controller = SCA.BusinessLib.ProjectManager.GetInstance.GetPrimaryController();

                SCA.Interface.ILoopService loopService = new SCA.BusinessLib.BusinessLogic.LoopService(controller);

                foreach (SCA.Model.LoopModel loopObject in LoopsInfoGrid.SelectedItems)
                {

                    if (loopObject != null)
                    {
                        loopService.DeleteLoopBySpecifiedLoopCode(loopObject.Code);
                    }
                }

                //刷新界面
                if (controller.Loops.Count != 0)
                {
                    LoopsInfoGrid.ItemsSource = null;
                    LoopsInfoGrid.ItemsSource = controller.Loops;
                }
                else
                {
                    LoopsInfoGrid.ItemsSource = null;
                }

                RaiseEvent(new RoutedEventArgs(DeleteButtonClickEvent));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseButtonClickEvent));
        }


    }
}
