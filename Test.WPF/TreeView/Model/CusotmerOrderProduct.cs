using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 15:21:18
* FileName   : CusotmerOrderProduct
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.TreeView.Model
{
    public class Customer
    {
        private List<Order> _orders;
        public string ID { get; set; }
        public string Name { get; set; }

        public List<Order> Orders 
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new List<Order>();
                }
                return _orders;
            }
            set
            {
                _orders = value;
            }
        }
    }
   public  class Order
    {
        private List<Product> _products;
        public string ID { get; set; }
        public string Code { get; set; }
        public List<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    _products = new List<Product>();
                }
                return _products;
            }
            set
            {
                _products = value;
            }
        }
    }
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
