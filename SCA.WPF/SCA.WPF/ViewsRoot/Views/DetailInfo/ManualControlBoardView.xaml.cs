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

namespace SCA.WPF.ViewsRoot.Views.DetailInfo
{
    /// <summary>
    /// ManualControlBoardView.xaml 的交互逻辑
    /// </summary>
    public partial class ManualControlBoardView : UserControl
    {
        public ManualControlBoardView()
        {
            InitializeComponent();            
        }

        public static readonly RoutedEvent AddMoreLineClickEvent = EventManager.RegisterRoutedEvent(
              "AddMoreLineClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ManualControlBoardView)
         );
        public event RoutedEventHandler AddMoreLineClick
        {
            add { AddHandler(AddMoreLineClickEvent, value); }
            remove { RemoveHandler(AddMoreLineClickEvent, value); }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SCA.Model.ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).TheController;
            
            var selectedItems = DataGrid_ManualBoard.SelectedItems;
            if (selectedItems != null)
            {
                SCA.Interface.BusinessLogic.IManualControlBoardService mcbService = new SCA.BusinessLib.BusinessLogic.ManualControlBoardService(controller); 
                foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableManualControlBoard r in selectedItems)
                {
                    if (r != null)
                    {
                        mcbService.DeleteBySpecifiedID(r.ID);
                    }
                }                
                //刷新界面
                ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).ManualControlBoardInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableManualControlBoards(controller, controller.ControlBoard);          
            }
        }
        //网络手控盘，添加多行事件
        private void btnAddMoreLine_Click(object sender, RoutedEventArgs e)
        {
            SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLines", ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).AddMoreLines);
            //SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLinesRefreshData", RefreshData);
            this.CreateManualControlBoard.Visibility = Visibility.Visible;
            SCA.Model.ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).TheController;            
            SCA.WPF.CreateManualControlBoard.CreateManualControlBoardViewModel vm = new CreateManualControlBoard.CreateManualControlBoardViewModel();
            vm.TheController = controller;
            this.CreateManualControlBoard.DataContext = vm;
            SCA.WPF.Infrastructure.EventMediator.Register("ManualControlBoardAddMoreLines", ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).AddMoreLines);
          //  SCA.WPF.Infrastructure.EventMediator.Register("ManualControlBoardAddMoreLinesRefreshData", RefreshData);
            
        }
        private void RefreshData(object o)
        {
            this.CreateManualControlBoard.Visibility = Visibility.Collapsed;
        //    SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLinesRefreshData",RefreshData);
         //   SCA.WPF.Infrastructure.EventMediator.Unregister("ManualControlBoardAddMoreLines", ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.ManualControlBoardViewModel)this.DataContext).AddMoreLines);
        }
        

        //private void btnAddMoreLine_Click(object sender, RoutedEventArgs e)
        //{
        //    RaiseEvent(new RoutedEventArgs(AddMoreLineClickEvent));
        //} 
       
    }
}
