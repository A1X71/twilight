using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/3 15:06:08
* FileName   : StockQuotesViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.NavigatingViews.ViewModel
{
    public class StockQuotesViewModel:ViewModelBase,IStockQuotesViewModel
    {
        public StockQuotesViewModel()
        {
            string s = "";
        }
        public string Name
        {
            get { return  "welcome to stockViewModel"; }
        }

        
    }
}
