using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using System.Data;
using SCA.Interface.DatabaseAccess;
namespace SCA.BusinessLib.BusinessLogic
{
    /// <summary>
    /// 项目信息操作
    /// </summary>
    public class ProjectService:IProjectService,IDisposable
    {
        private IExcelService _excelService;
        //private IFileService _fileService;
        public ProjectService()
        {
            
        }        
        public ProjectService(IExcelService excelService)
        {            
            _excelService = excelService;
        }
        public IEnumerable<Model.ProjectModel> GetProjects()
        {
            throw new NotImplementedException();
        }
                
        /// <summary>
        /// 组织控制器信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ControllerModel> OrganizeControllersInfo(DataTable dt)
        {
            List<ControllerModel> lstControllerModel = new List<ControllerModel>();
            
            var rows = from row in dt.AsEnumerable()                      
                       group row["controllerid"]
                       by new { id = row["controllerid"], name = row["controllername"],projectID=row["projectID"] } into g
                       select new
                       {
                           Id = g.Key.id,
                           Name = g.Key.name,
                           ProjectID=g.Key.projectID
                       };

            foreach (var row in rows)
            {
                ControllerModel controllerModel = new ControllerModel();
                controllerModel.ID = Convert.ToInt16(row.Id);
                controllerModel.Name = row.Name.ToString();
                controllerModel.ProjectID = Convert.ToInt16(row.ProjectID);

                OrganizeLoopModel(dt, ref controllerModel);

                OrganizeStandardLinkageConfigModel(dt, ref controllerModel);

                lstControllerModel.Add(controllerModel);
            }            
            return lstControllerModel;
        }
//        private List<LoopModel> OrganizeLoopModel(DataTable dt, ref ControllerModel controller)
        private void  OrganizeLoopModel(DataTable dt,ref ControllerModel controller)
        {
            int intControllerID=controller.ID;
            var rows = from row in dt.AsEnumerable()
                       where Convert.ToInt16(row["controllerId"]) == intControllerID
                       group row["loopid"]
                       by new { id = row["loopid"], name = row["loopname"] } into g
                       select new
                       {
                           Id=g.Key.id,
                           Name=g.Key.name            
                       };            
            foreach (var row in rows)
            {
                if (!(row.Id is System.DBNull))
                { 
                    LoopModel loop = new LoopModel { ID=Convert.ToInt16(row.Id),Name=(string)row.Name,ControllerID=controller.ID };
                    controller.Loops.Add(loop);
                }
            }            
        }
        private void OrganizeStandardLinkageConfigModel(DataTable dt, ref ControllerModel controller)
        {
            int intControllerID = controller.ID;
            var rows = from row in dt.AsEnumerable()
                       where Convert.ToInt16(row["controllerId"]) == intControllerID
                       group row["standardID"]
                       by new { id = row["standardID"], code = row["standardCode"] } into g
                       select new
                       {
                           linkageId = g.Key.id,
                           linkageCode = g.Key.code
                       };
            foreach (var row in rows)
            {
                if (!(row.linkageId is System.DBNull))
                {
                    LinkageConfigStandard loop = new LinkageConfigStandard { ID = Convert.ToInt16(row.linkageId), Code = row.linkageCode.ToString(), ControllerID = controller.ID };
                    controller.StandardConfig.Add(loop);
                }
            }
        }

        /// <summary>
        /// 将项目信息保存至文件中
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool SaveProject(ProjectModel project)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// 将项目文件发布为EXCEL文档
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool ExportProject(ProjectModel project)
        {
            /*
             * Excel导出规则 
             * 1.  第一页为“设备类型”
             * 2.  控制器输出方式
             * 2.1 由于存在多个回路，每个回路输出一页，输出回路中的器件
             * 2.2 输出“标准组态”
             * 2.3 输出“混合组态”
             * 2.4 输出“通用组态”
             * 2.5 输出“网络手动盘”
             * 3.  需要输出所有控制器的信息
             */
            if (project == null)  return false;
            try
            {
                string saveFileName = project.Name;
                //输出设备类型信息
                //_excelService.CreateExcelSheets();
                //控制器信息输出
                List<string> sheetNames = new List<string>();
                sheetNames.Add("设备类型");
                foreach (ControllerModel c in project.Controllers)
                {         
                    if(c.Loops.Count>0)
                    { 
                        foreach (LoopModel l in c.Loops)//增加回路名称
                        {
                            sheetNames.Add(c.Name+l.Name);        
                        }
                    }
                    if (c.StandardConfig.Count > 0)
                    {
                        sheetNames.Add(c.Name + NodeCategory.标准组态.ToString());
                    }
                    if (c.MixedConfig.Count > 0)
                    {
                        sheetNames.Add(c.Name + NodeCategory.混合组态.ToString());
                    }
                    if (c.GeneralConfig.Count > 0)
                    {
                        sheetNames.Add(c.Name + NodeCategory.通用组态.ToString());
                    }
                    if (c.ControlBoard.Count > 0)
                    {
                        sheetNames.Add(c.Name + NodeCategory.网络手动盘.ToString());
                    }       
                }
                _excelService.CreateExcelSheets(sheetNames);
                _excelService.SetCellValue(sheetNames[0].ToString(), 0, 0, sheetNames[0],CellStyleType.Data);
                //第1行
                _excelService.SetCellValue(sheetNames[0].ToString(), 1, 0, "编号", CellStyleType.Data);
                _excelService.SetCellValue(sheetNames[0].ToString(), 1, 1, "名称", CellStyleType.Data);
                //第2行
                
                //读取所有回路信息

                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }

            //_excelService.c
        }

        public void Dispose()
        {
            
            GC.SuppressFinalize(true);
        }


        public Model.ProjectModel GetProject()
        {
            throw new NotImplementedException();
        }


        public ProjectModel GetProject(ProjectModel pModel)
        {
            throw new NotImplementedException();
        }
        private bool ValidateProjectName(string projName)
        {
            if (projName != null)
            {
                if (projName.Length > 0 && projName.Length < 20)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ValidateProjectSavePath(string projectSavePath,IFileService fileService)
        {

            if (fileService.IsExistDirectory(projectSavePath))//如果文件夹存在，则返回True
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CreateProject(ProjectModel project,IFileService fileService)
        {
            //验证项目名有效性
            //验证创建目录有效性
            //将“存储路径”存入内存对象中
            //创建项目对象
            if (ValidateProjectName(project.Name)&& ValidateProjectSavePath(project.SavePath,fileService))
            {          
                return true;
            }
            else
            {
                return false;
            }
        }

        public ProjectModel CreateProject(string name, string savePath)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProject(ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProject(int ProjectId)
        {
            throw new NotImplementedException();
        }

        public ProjectModel OpenProject(string strPath)
        {
            throw new NotImplementedException();
        }
    }
}
