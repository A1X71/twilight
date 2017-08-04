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
    /// LinkageConfigMixedView.xaml 的交互逻辑
    /// </summary>
    public partial class LinkageConfigMixedView : UserControl
    {
        public LinkageConfigMixedView()
        {
            InitializeComponent();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                { 
                    SCA.Model.ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigMixedViewModel)this.DataContext).TheController;
                    var selectedItems = DataGrid_Mixed.SelectedItems;
                    if (selectedItems != null)
                    {
                        SCA.Interface.BusinessLogic.ILinkageConfigMixedService lcsService = new SCA.BusinessLib.BusinessLogic.LinkageConfigMixedService(controller);
                        foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigMixed r in selectedItems)
                        {
                            if (r != null)
                            {
                                lcsService.DeleteBySpecifiedID(r.ID);
                            }
                        }
                        //刷新界面
                        ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigMixedViewModel)this.DataContext).MixedLinkageConfigInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigMixeds(controller, controller.MixedConfig);
                    }
                }
            }
       }
    }
}

