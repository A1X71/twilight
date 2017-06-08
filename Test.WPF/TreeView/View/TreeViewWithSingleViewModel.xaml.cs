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

namespace Test.WPF.TreeView.View
{
    /// <summary>
    /// TreeViewWithSingleViewModel.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewWithSingleViewModel : Window
    {
        public TreeViewWithSingleViewModel()
        {
            InitializeComponent();
            //List<Model.Product> lstProducts = new List<Model.Product>() 
            //{
            //    new Model.Product(){ ID="1",Name="ToothPaste"},
            //    new Model.Product(){ ID="2",Name="Erase"}
            //};

            //List<Model.Order> lstOrders = new List<Model.Order>();
            //Model.Order o = new Model.Order();
            //o.ID = "1";
            //o.Code = "0001";
            //o.Products = lstProducts;
            //lstOrders.Add(o);
            Model.Customer c = new Model.Customer() {ID="1", Name="William", Orders=null};
            List<Model.Customer> lstCustomers = new List<Model.Customer>();
            lstCustomers.Add(c);
            ViewModel.HierarchyViewModel vm = new ViewModel.HierarchyViewModel(lstCustomers, null);
            HierarchyTreeView.DataContext = vm;
            //HierarchyTreeView
        }
    }
}
