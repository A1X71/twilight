using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SCA.Model;
using Test.WPF.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/8 15:14:31
* FileName   : LinkageConfigStandardViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class LinkageConfigStandardViewModel:ObservableObject
    {
        private ObservableCollection<LinkageConfigStandard> _standardLinkageConfigInfoObservableCollection;

        public ObservableCollection<LinkageConfigStandard> StandardLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_standardLinkageConfigInfoObservableCollection == null)
                {
                    _standardLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigStandard>();
                }
                return _standardLinkageConfigInfoObservableCollection;
            }
            set
            {
                _standardLinkageConfigInfoObservableCollection = value;
                RaisePropertyChanged("StandardLinkageConfigInfoObservableCollection");

            }
        }
    }
}
