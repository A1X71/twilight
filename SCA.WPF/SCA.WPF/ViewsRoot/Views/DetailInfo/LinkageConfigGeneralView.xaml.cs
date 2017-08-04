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
    /// LinkageConfigGeneralView.xaml 的交互逻辑
    /// </summary>
    public partial class LinkageConfigGeneralView : UserControl
    {
        public LinkageConfigGeneralView()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                {
                    SCA.Model.ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigGeneralViewModel)this.DataContext).TheController;
                    var selectedItems = DataGrid_General2.SelectedItems;
                    if (selectedItems != null)
                    {
                        SCA.Interface.BusinessLogic.ILinkageConfigGeneralService lcsService = new SCA.BusinessLib.BusinessLogic.LinkageConfigGeneralService(controller);
                        foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigGeneral r in selectedItems)
                        {
                            if (r != null)
                            {
                                lcsService.DeleteBySpecifiedID(r.ID);
                            }
                        }
                        //刷新界面
                        ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.LinkageConfigGeneralViewModel)this.DataContext).GeneralLinkageConfigInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableLinkageConfigGenerals(controller, controller.GeneralConfig);
                    }
                }
            }
        }
    }
}
