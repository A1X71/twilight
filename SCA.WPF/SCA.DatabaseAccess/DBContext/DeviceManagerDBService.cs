
using SCA.Interface.DatabaseAccess;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/4 9:09:49
* FileName   : DeviceFactoryDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
     public class DeviceManagerDBServiceTest
    {

        public static IDeviceDBServiceTest GetDeviceDBContext(ControllerType type, IDBFileVersionService dbFileVersionService)
        {
            switch (type)
            {
                case ControllerType.NT8036:
                   // return (IDeviceDBService<IDevice>) new Device8036DBService(databaseService);
                    return new Device8036DBServiceTest(dbFileVersionService);                    
                case ControllerType.NT8007:
                    return new Device8007DBServiceTest(dbFileVersionService);
                case ControllerType.NT8001:
                    return new Device8001DBService(dbFileVersionService);
                case ControllerType.FT8000:
                    return new Device8000DBService(dbFileVersionService);
                case ControllerType.FT8003:
                    return new Device8003DBService(dbFileVersionService);
                case ControllerType.NT8021:
                    return new Device8021DBService(dbFileVersionService);
            }
            return null;
        }
    }
}
