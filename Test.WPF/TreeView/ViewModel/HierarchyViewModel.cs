using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows.Data;
using Test.WPF.TreeView.Model;
using System.Windows.Controls;
using System.Windows;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 16:02:43
* FileName   : HierarchyViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.TreeView.ViewModel
{
    public class HierarchyViewModel 
    {
        public CollectionView Customers { get; private set; }
        private HierarchyItemViewModel _selectedItem;

        public HierarchyViewModel(List<Customer> customers, object selectedEntity)
        {
            // create the top level collectionview for the customers
            var customerHierarchyItemsList = new List<HierarchyItemViewModel>();

            foreach (var c in customers)
            {
                // create the hierarchy item and add to the list
                var customerHierarchyItem = new HierarchyItemViewModel(c);
                customerHierarchyItemsList.Add(customerHierarchyItem);

                // check if this is the selected item
                if (selectedEntity != null && selectedEntity.GetType() == typeof(Customer) && (selectedEntity as Customer).Equals(c))
                {
                    _selectedItem = customerHierarchyItem;
                }

                // if there are any orders in customerHierarchyItem
                if (c.Orders.Count != 0)
                {
                    // create a new list of HierarchyItems
                    var orderHierarchyItemsList = new List<HierarchyItemViewModel>();
                    // loop through the orders and add them
                    foreach (Order o in c.Orders)
                    {
                        // create the hierarchy item and add to the list
                        var orderHierarchyItem = new HierarchyItemViewModel(o);
                        orderHierarchyItem.Parent = customerHierarchyItem;
                        orderHierarchyItemsList.Add(orderHierarchyItem);

                        // check if this is the selected item
                        if (selectedEntity != null && selectedEntity.GetType() == typeof(Order) && (selectedEntity as Order).Equals(o))
                        {
                            _selectedItem = orderHierarchyItem;
                        }

                        // if there are any products in orderHierarchyItem
                        if (o.Products.Count != 0)
                        {
                            // create a new list of HierarchyItems
                            var productHierarchyItemsList = new List<HierarchyItemViewModel>();
                            // loop through the sites and add them
                            foreach (Product p in o.Products)
                            {
                                // create the hierarchy item and add to the list
                                var productHierarchyItem = new HierarchyItemViewModel(p);
                                productHierarchyItem.Parent = orderHierarchyItem;
                                productHierarchyItemsList.Add(productHierarchyItem);

                                // check if this is the selected item
                                if (selectedEntity != null && selectedEntity.GetType() == typeof(Product) && (selectedEntity as Product).Equals(p))
                                {
                                    _selectedItem = productHierarchyItem;
                                }
                            }

                            // create the children of the order
                            orderHierarchyItem.Children = new CollectionView(productHierarchyItemsList);
                        }
                    }
                    // create the children of the customer
                    customerHierarchyItem.Children = new CollectionView(orderHierarchyItemsList);
                }
            }

            this.Customers = new CollectionView(customerHierarchyItemsList);

            // select the selected item and expand it'type parents
            if (_selectedItem != null)
            {
                _selectedItem.IsSelected = true;
                HierarchyItemViewModel current = _selectedItem.Parent;

                while (current != null)
                {
                    current.IsExpanded = true;
                    current = current.Parent;
                }
            }

        }        
    }
    public class HierarchyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate retval = null;
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is HierarchyItemViewModel)
            {
                HierarchyItemViewModel hierarchyItem = item as HierarchyItemViewModel;
                if (hierarchyItem.DataItem != null)
                {

                    if (hierarchyItem.DataItem.GetType() == typeof(Customer))
                    {
                        retval = element.FindResource("CustomerTemplate") as DataTemplate;
                    }

                    else if (hierarchyItem.DataItem.GetType() == typeof(Order))
                    {
                        retval = element.FindResource("OrderTemplate") as DataTemplate;
                    }

                    else if (hierarchyItem.DataItem.GetType() == typeof(Product))
                    {
                        retval = element.FindResource("ProductTemplate") as DataTemplate;
                    }
                }
            }

            return retval;
        }
    }
}
