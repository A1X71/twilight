using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    /// <summary>
    /// 暂放弃此接口的实现，暂无操作，集成至ProjectDBService中
    /// </summary>
    public interface IDeviceTypeDBService
    {
        int AddDeviceType(List<DeviceType> lstDeviceType);

        bool UpdateMatchingController(ControllerType controllerType, string matchingType);

        bool InitializeDeviceTypeInfo(List<Model.DeviceType> lstDeviceType);
    }
}
