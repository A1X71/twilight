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
using System.Windows.Shapes;
using SCA.Model;
using SCA.BusinessLib.BusinessLogic;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;
using SCA.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
using SCA.BusinessLib;
using SCA.BusinessLib.Utility;
namespace Test.WPF.Navigator.View
{
    /// <summary>
    /// NavigatorView.xaml 的交互逻辑
    /// </summary>
    public partial class NavigatorView : Window
    {
        public NavigatorView()
        {
            InitializeComponent();


            ViewModel.HierarchyViewModel vm = new ViewModel.HierarchyViewModel();
            HierarchyTreeView.DataContext = vm;            
        }

    }
}
