using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/3 15:06:36
* FileName   : PortofolioViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.NavigatingViews.ViewModel
{
    public class PortofolioViewModel : ViewModelBase,IPortofolioViewModel
    {
       public PortofolioViewModel()
       {
           string str = "";

       }
        public string Name
        {
            get { return "welcome to portfolioViewModel"; }
        }

        public IEnumerable<string> Portfolios
        {
            get { throw new NotImplementedException(); }
        }

        
    }
}
