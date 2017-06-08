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
* Create Date: 2017/3/10 11:17:40
* FileName   : ManualControlBoard
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class ManualControlBoardViewModel:ObservableObject
    {
        private ObservableCollection<ManualControlBoard> _manualControlBoardInfoObservableCollection;

        public ObservableCollection<ManualControlBoard> ManualControlBoardInfoObservableCollection
        {
            get
            {
                if (_manualControlBoardInfoObservableCollection == null)
                {
                    _manualControlBoardInfoObservableCollection = new ObservableCollection<ManualControlBoard>();
                }
                return _manualControlBoardInfoObservableCollection;
            }
            set
            {
                _manualControlBoardInfoObservableCollection = value;
                RaisePropertyChanged("ManualControlBoardInfoObservableCollection");

            }
        }
    }
}
