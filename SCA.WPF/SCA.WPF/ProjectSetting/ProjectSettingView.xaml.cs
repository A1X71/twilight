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
using SCA.BusinessLib;

namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// ProjectSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectSettingView : UserControl
    {
        public ProjectSettingView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent ConfirmButtonClickEvent = EventManager.RegisterRoutedEvent(
           "ConfirmButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProjectSettingView)
           );
        public static readonly RoutedEvent CancelButtonClickEvent = EventManager.RegisterRoutedEvent(
            "CancelButtonClick",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(ProjectSettingView)
            );
        public event RoutedEventHandler ConfirmButtonClick
        {
            add { AddHandler(ConfirmButtonClickEvent, value); }
            remove { RemoveHandler(ConfirmButtonClickEvent, value); }
        }
        public event RoutedEventHandler CancelButtonClick
        {
            add { AddHandler(CancelButtonClickEvent,value); }
            remove { RemoveHandler(CancelButtonClickEvent, value); }
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectManager.GetInstance.SetPrimaryControllerByID(Convert.ToInt32(this.ControllerNameComboBox.SelectedValue));
            if (this.AutoSaveTimeIntervalInputTextBox.Text != null)
            {
                ProjectManager.GetInstance.SetSaveInterval(Convert.ToInt32(this.AutoSaveTimeIntervalInputTextBox.Text));
            }            
            RaiseEvent(new RoutedEventArgs(ConfirmButtonClickEvent));
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CancelButtonClickEvent));
        }

        

    }
}
