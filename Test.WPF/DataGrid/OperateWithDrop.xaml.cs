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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
namespace Test.WPF.DataGrid
{
    /// <summary>
    /// OperateWithDrop.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class OperateWithDrop : Window
    {
        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;
        public OperateWithDrop()
        {
            InitializeComponent();
            productsDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(productsDataGrid_PreviewMouseLeftButtonDown);
            productsDataGrid.Drop += new DragEventHandler(productsDataGrid_Drop);
        }
        void productsDataGrid_Drop(object sender, DragEventArgs e)
        {
            if (rowIndex < 0)
                return;
            int index = this.GetCurrentRowIndex(e.GetPosition);
            if (index < 0)
                return;
            if (index == rowIndex)
                return;
            if (index == productsDataGrid.Items.Count - 1)
            {
                MessageBox.Show("This row-index cannot be drop");
                return;
            }
            ProductCollection productCollection = Resources["ProductList"] as ProductCollection;
            Product changedProduct = productCollection[rowIndex];
            productCollection.RemoveAt(rowIndex);
            productCollection.Insert(index, changedProduct);
        }

        void productsDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rowIndex = GetCurrentRowIndex(e.GetPosition);
            if (rowIndex < 0)
                return;
            productsDataGrid.SelectedIndex = rowIndex;
            Product selectedEmp = productsDataGrid.Items[rowIndex] as Product;
            if (selectedEmp == null)
                return;
            DragDropEffects dragdropeffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(productsDataGrid, selectedEmp, dragdropeffects)
                                != DragDropEffects.None)
            {
                productsDataGrid.SelectedItem = selectedEmp;
            }
        }

        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private DataGridRow GetRowItem(int index)
        {
            if (productsDataGrid.ItemContainerGenerator.Status
                    != GeneratorStatus.ContainersGenerated)
                return null;
            return productsDataGrid.ItemContainerGenerator.ContainerFromIndex(index)
                                                            as DataGridRow;
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < productsDataGrid.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
    }

    public class ProductCollection : ObservableCollection<Product>
    {
        public ProductCollection()
        {
            Add(new Product() { ProductId = 111, ProductName = "Books", ProductPrice = "500$" });
            Add(new Product() { ProductId = 222, ProductName = "Cameras", ProductPrice = "600$" });
            Add(new Product() { ProductId = 333, ProductName = "Cell Phones", ProductPrice = "700$" });
            Add(new Product() { ProductId = 444, ProductName = "Clothing", ProductPrice = "800$" });
            Add(new Product() { ProductId = 555, ProductName = "Shoes", ProductPrice = "900$" });
            Add(new Product() { ProductId = 666, ProductName = "Gift Cards", ProductPrice = "500$" });
            Add(new Product() { ProductId = 777, ProductName = "Crafts", ProductPrice = "400$" });
            Add(new Product() { ProductId = 888, ProductName = "Computers", ProductPrice = "430$" });
            Add(new Product() { ProductId = 999, ProductName = "Coins", ProductPrice = "460$" });
            Add(new Product() { ProductId = 332, ProductName = "Cars", ProductPrice = "4600$" });
            Add(new Product() { ProductId = 564, ProductName = "Boats", ProductPrice = "3260$" });
            Add(new Product() { ProductId = 346, ProductName = "Dolls", ProductPrice = "120$" });
            Add(new Product() { ProductId = 677, ProductName = "Gift Cards", ProductPrice = "960$" });

        }
    }

}
