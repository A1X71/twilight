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
    /// DeviceInfo8007View.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfo8007View : UserControl
    {
        public DeviceInfo8007View()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SCA.Model.LoopModel loop = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8007ViewModel)this.DataContext).TheLoop;
                var selectedItems = DataGrid_Device.SelectedItems;
                if (selectedItems != null)
                {
                    SCA.BusinessLib.BusinessLogic.DeviceService8007 deviceService = new SCA.BusinessLib.BusinessLogic.DeviceService8007();
                    deviceService.TheLoop = loop;

                    foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8007 r in selectedItems)
                    {
                        if (r != null)
                        {
                            deviceService.DeleteBySpecifiedID(r.ID);
                        }
                    }
                    //刷新界面
                    ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8007ViewModel)this.DataContext).DeviceInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8007Collection(loop, loop.GetDevices<Model.DeviceInfo8007>());
                }
            }
        }
    }
}
