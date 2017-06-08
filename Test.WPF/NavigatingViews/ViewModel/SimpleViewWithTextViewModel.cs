using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/3 17:23:46
* FileName   : SimpleViewWithTextViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.NavigatingViews.ViewModel
{
    public class SimpleViewWithTextViewModel:PropertyChangedBase
    {
        private string _name="Initial";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyOfPropertyChange("Name");
            }
        }
    }
}
