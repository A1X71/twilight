
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
    public class DeviceManagerDBService
    {
        public static IDeviceDBService<IDevice> GetDeviceDBContext(ControllerType type,IDatabaseService databaseService)
        {
            switch (type)
            {
                case ControllerType.NT8036:
                    return (IDeviceDBService<IDevice>) new Device8036DBService(databaseService);
                case ControllerType.NT8007:
                    return (IDeviceDBService<IDevice>)new Device8007DBService(databaseService);
                //case ControllerType.NT8001:
                //    return new ControllerConfig8001();
                //case ControllerType.FT8000:
                //    return new ControllerConfig8000();
                //case ControllerType.FT8003:
                //    return new ControllerConfig8003();
                //case ControllerType.NT8021:
                //    return new ControllerConfig8021();
            }
            return null;
        }
        public static void  Create8036DBService(IDeviceDBService<DeviceInfo8036> service)
        {
            
        }
        
        //private bool Run(IDatabaseService databaseService)
        //{
        //    Device8036DBService deviceDBService = new Device8036DBService(databaseService);
        //    deviceDBService.GetDevice(2);
        //    return true;

        //}
    }

    public class DeviceManagerDBServiceTest
    {

        public static IDeviceDBServiceTest GetDeviceDBContext(ControllerType type, IDatabaseService databaseService)
        {
            switch (type)
            {
                case ControllerType.NT8036:
                   // return (IDeviceDBService<IDevice>) new Device8036DBService(databaseService);
                    return new Device8036DBServiceTest(databaseService);                    
                case ControllerType.NT8007:
                    return  new Device8007DBServiceTest(databaseService);
                case ControllerType.NT8001:
                    return new Device8001DBService(databaseService);
                case ControllerType.FT8000:
                    return new Device8000DBService(databaseService);
                case ControllerType.FT8003:
                    return new Device8003DBService(databaseService);
                case ControllerType.NT8021:
                    return new Device8021DBService(databaseService);
            }
            return null;
        }
    }
}
