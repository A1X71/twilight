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
    /// LinkageConfigStandardView.xaml 的交互逻辑
    /// </summary>
    public partial class LinkageConfigStandardView : UserControl
    {
        public LinkageConfigStandardView()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                {
                    SCA.Model.ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigStandardViewModel)this.DataContext).TheController;
                    var selectedItems = DataGrid_Standard.SelectedItems;
                    if (selectedItems != null)
                    {
                        SCA.Interface.BusinessLogic.ILinkageConfigStandardService lcsService = new SCA.BusinessLib.BusinessLogic.LinkageConfigStandardService(controller);
                        foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigStandard r in selectedItems)
                        {
                            if (r != null)
                            {
                                lcsService.DeleteBySpecifiedID(r.ID);
                            }
                        }
                        //刷新界面
                        ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigStandardViewModel)this.DataContext).StandardLinkageConfigInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigStandards(controller, controller.StandardConfig);
                    }
                }
            }
        }
    }
}
