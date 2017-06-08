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
    /// MenuView.xaml 的交互逻辑
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent CreateControllerEvent = EventManager.RegisterRoutedEvent("CreateControllerClick",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(MenuView));
        public static readonly RoutedEvent CreateLoopEvent = EventManager.RegisterRoutedEvent("CreateLoopClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));
        public static readonly RoutedEvent ProjectSettingEvent = EventManager.RegisterRoutedEvent("ProjectSettingClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));        
        public static readonly RoutedEvent EditLoopsEvent = EventManager.RegisterRoutedEvent("EditLoopsClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));

        public static readonly RoutedEvent CreateProjectEvent = EventManager.RegisterRoutedEvent("CreateProjectClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));
        public static readonly RoutedEvent OpenProjectEvent = EventManager.RegisterRoutedEvent("OpenProjectClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));
        public static readonly RoutedEvent SaveProjectEvent = EventManager.RegisterRoutedEvent("SaveProjectClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));
        public static readonly RoutedEvent CloseProjectEvent = EventManager.RegisterRoutedEvent("CloseProjectClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuView));


        public event RoutedEventHandler CreateControllerClick
        {
            add { AddHandler(CreateControllerEvent, value); }
            remove { RemoveHandler(CreateControllerEvent, value); }
        }       

        public event RoutedEventHandler CreateLoopClick
        {
            add { AddHandler(CreateLoopEvent, value); }
            remove { RemoveHandler(CreateLoopEvent, value); }
        }
        public event RoutedEventHandler ProjectSettingClick
        {
            add { AddHandler(ProjectSettingEvent, value); }
            remove { RemoveHandler(ProjectSettingEvent, value); }
        }

        public event RoutedEventHandler CreateProjectClick
        {
            add { AddHandler(CreateProjectEvent, value); }
            remove { RemoveHandler(CreateProjectEvent, value); }
        }
        public event RoutedEventHandler OpenProjectClick
        {
            add { AddHandler(OpenProjectEvent, value); }
            remove { RemoveHandler(OpenProjectEvent, value); }
        }
        public event RoutedEventHandler SaveProjectClick
        {
            add { AddHandler(SaveProjectEvent, value); }
            remove { RemoveHandler(SaveProjectEvent, value); }
        }
        public event RoutedEventHandler CloseProjectClick
        {
            add { AddHandler(CloseProjectEvent, value); }
            remove { RemoveHandler(CloseProjectEvent, value); }
        }


        public event RoutedEventHandler EditLoopsClick
        {
            add { AddHandler(EditLoopsEvent, value); }
            remove { RemoveHandler(EditLoopsEvent, value); }
        }

        public void CreateControllerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CreateControllerEvent));
        }

        private void CreateLoopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //菜单中不指定控制器，默认设置“主控制器”,传递null
            RaiseEvent(new RoutedEventArgs(CreateLoopEvent,null));            
        }

        private void ProjectSettingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ProjectSettingEvent));            
        }
        private void CreateProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CreateProjectEvent));            
        }
        private void EditLoopsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EditLoopsEvent));
        }
        private void menu_OpenProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenProjectEvent));
        }

        private void menu_SaveProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SaveProjectEvent));
        }

        private void menu_CloseProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseProjectEvent));
        }

        private void menu_ImportFromOldVersionFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menu_ImportFromExcelFile_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
