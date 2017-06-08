using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.WPF.Utility;
using System.Collections.ObjectModel;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/10 10:55:03
* FileName   : LinkageConfigMixedViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class LinkageConfigMixedViewModel : ObservableObject
    {
        private ObservableCollection<LinkageConfigMixed> _mixedLinkageConfigInfoObservableCollection;

        public ObservableCollection<LinkageConfigMixed> MixedLinkageConfigInfoObservableCollection
        {
            get
            {
                if (_mixedLinkageConfigInfoObservableCollection == null)
                {
                    _mixedLinkageConfigInfoObservableCollection = new ObservableCollection<LinkageConfigMixed>();
                }
                return _mixedLinkageConfigInfoObservableCollection;
            }
            set
            {
                _mixedLinkageConfigInfoObservableCollection = value;
                RaisePropertyChanged("MixedLinkageConfigInfoObservableCollection");

            }
        }

    }
}
