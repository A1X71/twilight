using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
namespace SCA.BusinessLib
{
    public class ProjectManager:SCA.Interface.IProjectManager<SCA.Model.ProjectModel>
    {

        protected IProjectService _projectService;
        protected IFileService _fileService;
        protected string _dirPath;
        public ProjectManager(IProjectService projectService, IFileService fileService)
        {
            _projectService = projectService;
            _fileService = fileService;
        }
        public  bool ValidateProjectName(string projName)
        {
            if(projName!=null)
            { 
                if (projName.Length > 0 && projName.Length <20)
                { 
                    return true;
                }
            }
            return false;
        }

        public Model.ProjectModel CreateProject(ProjectModel project,string strPath)
        {
            //验证项目名有效性
            //验证创建目录有效性
            //将“存储路径”存入内存对象中
            //创建项目对象
            if (ValidateProjectName(project.Name))
            {
                if(_fileService.IsExistDirectory(strPath))
                {
                    _dirPath = strPath;
                    _projectService.CreateProject(project);                    
                }
            }
            throw new NotImplementedException();
        }

        public Model.ProjectModel OpenProject()
        {

            throw new NotImplementedException();
        }

        public bool CloseProject()
        {
            throw new NotImplementedException();
        }


        public ProjectModel CreateProject(string projectName, string strPath)
        {

            throw new NotImplementedException();
        }
    }
}
