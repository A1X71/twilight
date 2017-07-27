using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface
{
    public interface IProjectService:IDisposable
    {
        IEnumerable<ProjectModel> GetProjects();
        ProjectModel GetProject();
        Model.ProjectModel GetProject(Model.ProjectModel pModel);
        
        /// <summary>
        /// 创建项目文件及基本表结构信息
        /// 初始化基础数据
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        bool CreateProject(ProjectModel project,IFileService fileService);
        ProjectModel CreateProject(string name, string savePath);
        bool UpdateProject(ProjectModel project);
        bool SaveProjectName(string name);
        bool DeleteProject(int ProjectId);
        /// <summary>
        /// 打开项目文件
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <returns>项目信息结构</returns>
        Model.ProjectModel OpenProject(string strPath);

        /// <summary>
        /// 将项目文件发布为EXCEL文档
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        bool ExportProjectToExcel(ProjectModel project);

        /// <summary>
        /// 将项目信息保存至文件中
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        bool SaveProject(ProjectModel project);
        bool ValidateProjectName(string projName);
    }
}
