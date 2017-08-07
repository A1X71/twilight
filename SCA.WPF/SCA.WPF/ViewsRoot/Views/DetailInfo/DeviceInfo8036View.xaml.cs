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
using SCA.WPF.Infrastructure;
namespace SCA.WPF.ViewsRoot.Views.DetailInfo
{
    /// <summary>
    /// DeviceInfo8036View.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfo8036View : UserControl
    {
        public DeviceInfo8036View()
        {
            InitializeComponent();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                {
                    SCA.Model.LoopModel loop = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8036ViewModel)this.DataContext).TheLoop;
                    var selectedItems = DataGrid_Device.SelectedItems;
                    if (selectedItems != null)
                    {
                        SCA.BusinessLib.BusinessLogic.DeviceService8036 deviceService = new SCA.BusinessLib.BusinessLogic.DeviceService8036();
                        deviceService.TheLoop = loop;

                        foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8036 r in selectedItems)
                        {
                            if (r != null)
                            {
                                deviceService.DeleteBySpecifiedID(r.ID);
                            }
                        }
                        //刷新界面
                        ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8036ViewModel)this.DataContext).DeviceInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8036Collection(loop, loop.GetDevices<Model.DeviceInfo8036>());
                    }
                }
            }
        }
    }
}
