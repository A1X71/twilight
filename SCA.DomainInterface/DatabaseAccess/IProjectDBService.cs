using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using System.Data;
namespace SCA.Interface.DatabaseAccess
{
    public interface IProjectDBService:IDisposable
    {
        /// <summary>
        /// 创建项目时的数据表结构初始化工作
        /// </summary>
        /// <returns></returns>
        bool CreatFundamentalTableStructure();
        bool CreateLocalDBFile();
        /// <summary>
        /// 初始化业务数据
        /// </summary>
        /// <returns></returns>
        bool InitializeControllerTypeInfo();//ProjectModel project
        //bool InitializeDeviceTypeInfo(List<Model.DeviceType> lstDeviceType);
        ProjectModel GetProject(int id);
        ProjectModel GetProject(ProjectModel project);

        int AddProject(ProjectModel project);

        int UpdateProject(ProjectModel project);

        bool DeleteProject(int ProjectId);
        int GetMaxID();
        /// <summary>
        /// 取得项目下的全部信息
        /// 关联表过多,此方法需要进行压力测试
        /// </summary>
        /// <returns></returns>
        DataTable OpenProject();
    }
}
