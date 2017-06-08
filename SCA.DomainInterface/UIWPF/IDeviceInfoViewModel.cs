using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace SCA.Interface.UIWPF
{
    public interface IDeviceInfoViewModel<T>
    {
        ObservableCollection<T> DeviceInfoObservableCollection { get; set; }
    }
}
