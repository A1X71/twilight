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
        bool SaveToDB();
        /// <summary>
        /// 更新指定ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data);
    }
}
