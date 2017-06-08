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
using SCA.Model;
namespace SCA.WPF.ViewsRoot.Views.Query
{
    /// <summary>
    /// ProjectSummaryView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectSummaryView : UserControl
    {
        public ProjectSummaryView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row != null && row.IsSelected)
            {
                var viewModel = (SCA.WPF.ViewModelsRoot.ViewModels.Query.ProjectSummaryViewModel)DataContext;
                DeviceInfoForSimulator device = new DeviceInfoForSimulator();
                device = (DeviceInfoForSimulator)((DataGridRow)row).Item;
                viewModel.AddDeviceInfoToInputGrid(device);
            }
        }
        private void InputDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row != null && row.IsSelected)
            {
                var viewModel = (SCA.WPF.ViewModelsRoot.ViewModels.Query.ProjectSummaryViewModel)DataContext;
                DeviceInfoForSimulator device = new DeviceInfoForSimulator();
                device = (DeviceInfoForSimulator)((DataGridRow)row).Item;
                viewModel.RemoveDeviceInfoFromInputGrid(device);
            }
        }
    }
}
