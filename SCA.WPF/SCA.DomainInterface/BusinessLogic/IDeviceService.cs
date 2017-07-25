using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using System.Collections.ObjectModel;
namespace SCA.Interface
{
    public interface IDeviceService<T>
    {
        LoopModel TheLoop { get; set; }
        List<T> InitializeDevices(int deviceAmount);
        List<T> Create(int amount);
        bool Update(T deviceInfo);
        bool DeleteBySpecifiedID(int id);
        bool IsExistSameDeviceCode();

        /// <summary>
        /// 在回路内是否存在相同的器件代码
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        bool IsExistSameDeviceCode(string deviceCode);

    }
}
