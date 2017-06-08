using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
namespace SCA.BusinessLib
{
    /// <summary>
    /// 项目信息操作
    /// </summary>
    public class ProjectService:IProjectService
    {
        public IEnumerable<Model.ProjectModel> GetProjects()
        {
            throw new NotImplementedException();
        }

        public Model.ProjectModel GetProject()
        {
            throw new NotImplementedException();
        }

        public bool  CreateProject(Model.ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProject(Model.ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProject(int ProjectId)
        {
            throw new NotImplementedException();
        }
    }
}
