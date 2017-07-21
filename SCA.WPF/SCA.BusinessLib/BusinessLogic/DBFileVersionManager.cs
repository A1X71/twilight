using SCA.Interface.DatabaseAccess;
using SCA.Interface.BusinessLogic;
using SCA.Interface;
using SCA.DatabaseAccess.DBContext;
using SCA.BusinessLib.BusinessLogic;
using SCA.Model;
using System.Collections.Generic;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 8:52:22
* FileName   : DBFilerVersionManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLogic
{
    public class DBFileVersionManager
    {
        string _dataSource;
        ILogRecorder _logger;
        IFileService _fileService;
        /// <summary>
        /// 当前数据文件版本
        /// </summary>
        static int _currentDBFileVersion = 7;
        public static int CurrentDBFileVersion //考虑此属性是否在程序初始化时设置？？？？？？？2017-07-17
        {
            get
            {
                return _currentDBFileVersion;
            }
            //private set
            //{
            //    _currentDBFileVersion = value;
            //}
        }
        public DBFileVersionManager(string dataSource, ILogRecorder logger, IFileService fileService)
        {
            _dataSource = dataSource;
            _logger = logger;
            _fileService = fileService;
        }
        public IDBFileVersionService GetDBFileVersionServiceByVersionID(int versionNumber)
        {
            switch (versionNumber)
            { 
                case 4:
                    return new DBFileVersion4DBService(_dataSource, _logger, _fileService);
                case 5:
                    return new DBFileVersion5DBService(_dataSource, _logger, _fileService);
                case 6:
                    return new DBFileVersion6DBService(_dataSource, _logger, _fileService);
                case 7 :
                    return new DBFileVersion7DBService(_dataSource,_logger,_fileService);
                default:
                    throw new System.NotImplementedException();
            }
        }
        public IDBFileVersionService GetDBFileVersionServiceByExtentionName(string strExtentionName)
        {
            if (strExtentionName.ToString().ToUpper() == "MDB")
            {
                return new DBFileVersion4DBService(_dataSource, _logger, _fileService); 
            }
            else
            {
                return new DBFileVersion7DBService(_dataSource, _logger, _fileService);//从版本7开始为新的表结构
            }
        }
        public ProjectModel VersionConverter(int startVersion, int endVersion,ProjectModel projectModel)
        {
            ProjectModel resultProject = null;
            if (startVersion < endVersion)
            {
                //Dictionary<int, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>> dictMap = new Dictionary<QueryType, System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>>>();
                
                Dictionary<int, System.Func<ProjectModel, ProjectModel>> dictMap = new Dictionary<int, System.Func<ProjectModel, ProjectModel>>();
                dictMap.Add(4, Version4ToVersion5);
                dictMap.Add(5, Version5ToVersion6);
                dictMap.Add(6, Version6ToVersion7);
                //System.Func<List<DeviceInfoForSimulator>, List<DeviceInfoForSimulator>> execMethod;
                System.Func<ProjectModel, ProjectModel> execMethod;

                for (int i = startVersion; i <= endVersion; i++)
                {
                    dictMap.TryGetValue(i, out execMethod);
                    resultProject = execMethod(projectModel);
                    if (i + 1 == endVersion)//逐级转换，与目标版本仅相差一级时，实际上已经转换完毕，退出版本循环
                    {
                        break;
                    }
                }
            }
            else
            {
                resultProject = projectModel;
            }
            return resultProject;            
        }
        public ProjectModel Version4ToVersion5(ProjectModel projectModel)
        {
            IDBFileVersionConverter dbFileVersionConverter = new DBFileVersionFromFourToFiveConverter();
            return dbFileVersionConverter.UpgradeToDestinationVersion(projectModel);
        }
        public ProjectModel Version5ToVersion6(ProjectModel projectModel)
        {
            IDBFileVersionConverter dbFileVersionConverter = new DBFileVersionFromFiveToSixConverter();
            return dbFileVersionConverter.UpgradeToDestinationVersion(projectModel);
        }
        public ProjectModel Version6ToVersion7(ProjectModel projectModel)
        {
            IDBFileVersionConverter dbFileVersionConverter = new DBFileVersionFromSixToSevenConverter();
            return dbFileVersionConverter.UpgradeToDestinationVersion(projectModel);
        }
    

    }
}
