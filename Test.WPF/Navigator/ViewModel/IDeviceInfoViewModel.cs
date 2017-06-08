using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SCA.Model;
namespace Test.WPF.Navigator.ViewModel
{
    public interface IDeviceInfoViewModel<T>
    {

        ObservableCollection<T> DeviceInfoObservableCollection { get; set; }
    }
}
