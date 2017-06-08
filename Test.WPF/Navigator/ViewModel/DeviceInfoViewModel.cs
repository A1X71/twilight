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
* Create Date: 2017/3/7 17:11:27
* FileName   : DeviceInfo8036ViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class DeviceInfoViewModelBase
    { 
    
    }
    public class DeviceInfoViewModel<T> : IDeviceInfoViewModel<T>
    {
        private ObservableCollection<T> _deviceInfoObservableCollection;   

        public ObservableCollection<T> DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoObservableCollection == null)
                {
                    _deviceInfoObservableCollection = new ObservableCollection<T>();
                }
                return _deviceInfoObservableCollection;
            }
            set
            {
                _deviceInfoObservableCollection = value;
                
            }
        }
    }
    public class DeviceInfo8036ViewModel:ObservableObject,IDeviceInfoViewModel<DeviceInfo8036> //: DeviceInfoViewModel<DeviceInfo8036>
    {
        private ObservableCollection<DeviceInfo8036> _deviceInfoObservableCollection;

        public  ObservableCollection<DeviceInfo8036> DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoObservableCollection == null)
                {
                    _deviceInfoObservableCollection = new ObservableCollection<DeviceInfo8036>();
                }
                return _deviceInfoObservableCollection;
            }
            set
            {
                _deviceInfoObservableCollection = value;
                RaisePropertyChanged("DeviceInfoObservableCollection");

            }
        }
    }

    public class DeviceInfo8001ViewModel :ObservableObject,IDeviceInfoViewModel<DeviceInfo8001> //: DeviceInfoViewModel<DeviceInfo8036>
    {
        private ObservableCollection<DeviceInfo8001> _deviceInfoObservableCollection;

        public ObservableCollection<DeviceInfo8001> DeviceInfoObservableCollection
        {
            get
            {
                if (_deviceInfoObservableCollection == null)
                {
                    _deviceInfoObservableCollection = new ObservableCollection<DeviceInfo8001>();
                }
                return _deviceInfoObservableCollection;
            }
            set
            {
                _deviceInfoObservableCollection = value;
                RaisePropertyChanged("DeviceInfoObservableCollection");

            }
        }
    }
}
