using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface IDeviceDBService<T> where T:IDevice
    {
        bool CreateTableStructure();
        List<T> GetDevice<T>(int id);
        List<T> GetDevice<T>(T device);
        bool AddDevice<T>(T device);

        bool AddDevice<T>(List<T> Devices);
        bool UpdateDevice<T>(T device);
        bool DeleteDevice<T>(T device);
        //List<T> GetDevice<T>(int id);
        //List<T> GetDevice<T>(T device);
        //bool AddDevice<T>(T device);

        //bool AddDevice<T>(List<T> Devices);
        //bool UpdateDevice<T>(T device);
        //bool DeleteDevice<T>(T device);

    }
    public interface IDeviceDBServiceTest
    {
        bool CreateTableStructure();
        bool AddDevice(LoopModel loop);
        /// <summary>
        /// 由于各控制器的器件信息不同，暂以Loop人形式返回
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        LoopModel GetDevicesByLoop(LoopModel loop);

        bool DeleteAllDevicesByControllerID(int id);

        bool DeleteDeviceByID(int id);

        int GetMaxID();
    }
}
