using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface
{
    public interface IProjectManager
    {
        ProjectModel Project { get;}
        //是否存在未保存的数据
        bool IsDirty { get; set; }
        /// <summary>
        /// 验证项目名称是否规范
        /// </summary>
        /// <param name="projName"></param>
        /// <returns></returns>
        //bool ValidateProjectName(string projName);
        /// <summary>
        /// 创建一个项目
        /// </summary>
        /// <returns>返回项目模型</returns>
        Model.ProjectModel CreateProject(Model.ProjectModel project);
        Model.ProjectModel CreateProject(string projectName, string strPath);
        /// <summary>
        /// 打开数据文件，构建项目信息
        /// </summary>
        /// <returns>项目信息</returns>
       void OpenProject(string strPath);
        /// <summary>
        /// 关闭一个项目
        /// </summary>
        /// <returns>true为成功，false为失败</returns>
        bool CloseProject();

        #region 控制器操作
        List<ControllerModel> Controllers { get; }
        bool AddControllers(ControllerModel controller);

///        List<ControllerModel> GetControllers();
        
        ControllerModel GetControllerBySpecificID(int id);
        /// <summary>
        /// 更新指定的控制器信息 类为引用类型，不需要更新
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        //bool UpdateControllerBySpecificController(ControllerModel controller);
        /// <summary>
        /// 取得主控制器
        /// </summary>
        /// <returns></returns>
        ControllerModel GetPrimaryController();
        SummaryNodeInfo DisplaySummaryInfo(Model.ProjectModel projectModel);
        void SetPrimaryControllerByID(int controllerID);        
        #endregion



    }
}
