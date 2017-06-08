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
* Create Date: 2017/3/9 8:06:19
* FileName   : LinkageConfigGeneralViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class LinkageConfigGeneralViewModel:ObservableObject
    {
        private ObservableCollection<LinkageConfigGeneral> _generalLinkageConfigInfoObservableCollection;

        public ObservableCollection<LinkageConfigGeneral> GeneralLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_generalLinkageConfigInfoObservableCollection == null)
                {
                    _generalLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigGeneral>();
                }
                return _generalLinkageConfigInfoObservableCollection;
            }
            set
            {
                _generalLinkageConfigInfoObservableCollection = value;
                RaisePropertyChanged("GeneralLinkageConfigInfoObservableCollection");

            }
        }
    }
}
