using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.Utility;
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
        /// 将项目信息保存至文件中
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool SaveProject(ProjectModel project)
        {
            throw new NotImplementedException();

        }
        public bool SaveProjectName(string name)
        {
            try
            {
                ProjectManager.GetInstance.Project.Name = name;
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 生成EXCEL文件摘要信息页
        /// </summary>
        /// <param name="excelService"></param>
        /// <param name="sheetNames"></param>
        /// <returns></returns>
        protected  bool GenerateExcelSummarySheet(ProjectModel project,ref IExcelService excelService, out List<string> sheetNames)
        {
            List<string> lstSheetNames = new List<string>();
            try
            {
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);

                const int Loop_Amount_Per_Sheet = 8;
                ControllerManager controllerManager = new ControllerManager();
                controllerManager.InitializeAllControllerOperation(null);
                List<SummaryInfo> lstSummaryInfo = new List<SummaryInfo>();
                lstSheetNames.Add("项目摘要");
                foreach (var controller in project.Controllers)
                {
                    IControllerOperation controllerOperation = controllerManager.GetController(controller.Type);
                    //取控制器摘要信息逻辑
                    SummaryInfo controllerSummary = controllerOperation.GetSummaryNodes(controller, 2);
                    controllerSummary.Name += "-机号:" + controller.MachineNumber;
                    lstSummaryInfo.Add(controllerSummary);
                    int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)controller.Loops.Count / Loop_Amount_Per_Sheet));
                    for (int i = 0; i < loopSheetAmount; i++)
                    {
                        lstSheetNames.Add(controller.Name + "-回路分组" + (i+1).ToString());
                    }
                    if (controller.StandardConfig.Count > 0)
                    {
                        lstSheetNames.Add(controller.Name + "-标准组态");
                    }
                    if (controller.MixedConfig.Count > 0)
                    {
                        lstSheetNames.Add(controller.Name + "-混合组态");
                    }
                    if (controller.GeneralConfig.Count > 0)
                    {
                        lstSheetNames.Add(controller.Name + "-通用组态");
                    }
                    if (controller.ControlBoard.Count > 0)
                    {
                        lstSheetNames.Add(controller.Name + "-网络手动盘");
                    }
                }
                excelService.CreateExcelSheets(lstSheetNames);                

                ControllerOperationCommon controllerOperator = new ControllerOperationCommon();
                controllerOperator.SetDefaultExcelStyle(ref excelService);//取得默认EXCEL样式                

                excelService.RowHeight = (short)20;//到下一个高度设置前，使用该高度
                excelService.SetCellValue(lstSheetNames[0], 0, 0, project.Name, CellStyleType.Caption);
                excelService.RowHeight = (short)15;
                int startRowIndex = 1;//开始行
                int endRowIndex = 0;//结束行
                foreach (var info in lstSummaryInfo)
                {
                    endRowIndex++;
                    //info.name格式：summary.Name = "控制器:控制器名(控制器类型,器件长度)-机号:001";                    
                    startRowIndex = endRowIndex;
                    string strControllerName = "";
                    string strControllerType = "";
                    string strControllerDeviceAddressLength = "";
                    string strControllerMachineNumber = "";
                    int startIndex = info.Name.IndexOf(':')+1;
                    int endIndex = info.Name.IndexOf('[');
                    if (startIndex < endIndex)
                    {
                        strControllerName = info.Name.Substring(startIndex, endIndex - startIndex);
                    }
                    startIndex = info.Name.IndexOf('[')+1;
                    endIndex = info.Name.IndexOf(',');
                    if (startIndex < endIndex)
                    {
                        strControllerType = info.Name.Substring(startIndex, endIndex - startIndex);
                    }

                    startIndex = info.Name.IndexOf(',') + 1;
                    endIndex = info.Name.IndexOf(']');
                    if (startIndex < endIndex)
                    {
                        strControllerDeviceAddressLength = info.Name.Substring(startIndex, endIndex - startIndex);
                    }
                    startIndex = info.Name.LastIndexOf(':')+1;
                    if (startIndex != -1)
                    {
                        strControllerMachineNumber = info.Name.Substring(startIndex);
                    }
                    
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 0, strControllerName, CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 1, "类型", CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 2, strControllerType, CellStyleType.Data);
                    endRowIndex++;
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 0, null, CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 1, "机号", CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 2, strControllerMachineNumber, CellStyleType.Data);
                    endRowIndex++;
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 0, null, CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 1, "器件长度", CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[0], endRowIndex, 2, strControllerDeviceAddressLength + "位", CellStyleType.Data);
                    for (int i = 0; i < info.ChildNodes.Count;i++)
                    {
                        if (info.ChildNodes[i].Name != "设备类型")
                        {
                            endRowIndex++;
                            excelService.SetCellValue(lstSheetNames[0], endRowIndex, 0, null, CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[0], endRowIndex, 1, info.ChildNodes[i].Name+"数量", CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[0], endRowIndex, 2, info.ChildNodes[i].Number.ToString(), CellStyleType.Data);
                        }
                    }
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = startRowIndex;
                    mergeCellRange.LastRowIndex = endRowIndex;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 0;
                    lstMergeCellRange.Add(mergeCellRange);
                }
                excelService.SetColumnWidth(lstSheetNames[0], 1, 15f);
                excelService.SetMergeCells(lstSheetNames[0], lstMergeCellRange);//设置"摘要信息"合并单元格
                sheetNames = lstSheetNames;
            }
            catch (Exception ex)
            {
                sheetNames = lstSheetNames;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将项目文件发布为EXCEL文档
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool ExportProjectToExcel(ProjectModel project,string strFilePath,IFileService fileService)
        {
   
            if (project == null)  return false;
            try
            {
                EXCELVersion version = EXCELVersion.EXCEL2003;
                strFilePath +="\\"+ project.Name + ".xls";
                IExcelService excelService = ExcelServiceManager.GetExcelService(version, strFilePath, fileService);
                List<string> sheetNames = new List<string>();
                const int Loop_Amount_Per_Sheet = 8;
                GenerateExcelSummarySheet(project, ref  excelService, out sheetNames);
                ControllerManager controllerManager = new ControllerManager();
                controllerManager.InitializeAllControllerOperation(null);
                
                //生成“项目摘要”信息
                
                
                foreach (var controller in project.Controllers)
                {
                   IControllerOperation controllerOperation = controllerManager.GetController(controller.Type);
                    
                    int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)controller.Loops.Count / Loop_Amount_Per_Sheet));
                    int loopStartIndex = 0;//记录每个Sheet页内初始索引号                    
                    for (int i = 0; i < loopSheetAmount; i++)
                    {                        
                        string loopName =controller.Name + "-回路分组" + (i+1).ToString();
                        int loopEndIndex = (i+1) * Loop_Amount_Per_Sheet;
                        List<LoopModel> lstLoops=new List<LoopModel>();
                        for (int j = loopStartIndex; j < loopEndIndex; j++)
                        {
                            //loopStartIndex++;
                            if (j >= controller.Loops.Count) //超过最大回路数
                            {
                                break;
                            }
                            lstLoops.Add(controller.Loops[j]);                            
                        }
                        controllerOperation.ExportLoopDataToExcel(ref excelService, lstLoops, loopName);
                        lstLoops.Clear();
                    }
                    if (controller.StandardConfig.Count > 0)
                    {
                        string strSheetName = controller.Name + "-标准组态";
                        controllerOperation.ExportStandardLinkageDataToExcel(ref excelService, controller.StandardConfig,strSheetName);
                    }
                    if (controller.MixedConfig.Count > 0)
                    {   
                        string strSheetName = controller.Name + "-混合组态";
                        controllerOperation.ExportMixedLinkageDataToExcel(ref excelService, controller.MixedConfig, strSheetName);
                    }
                    if (controller.GeneralConfig.Count > 0)
                    {
                        string strSheetName=controller.Name + "-通用组态";
                        controllerOperation.ExportGeneralLinkageDataToExcel(ref excelService, controller.GeneralConfig, strSheetName);
                    }
                    if (controller.ControlBoard.Count > 0)
                    {
                        string strSheetName = controller.Name + "-网络手动盘";
                        controllerOperation.ExportManualControlBoardDataToExcel(ref excelService, controller.ControlBoard, strSheetName);                        
                    }
                    
                }
                excelService.SaveToFile();    

                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }

            
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
        public bool ValidateProjectName(string projName)
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
            if(project.SavePath!=null)
            {
                int lastIndexPosition=project.SavePath.LastIndexOf("\\");                
                string filePath = project.SavePath.Substring(0,lastIndexPosition);
                if (ValidateProjectName(project.Name)&& ValidateProjectSavePath(filePath,fileService))
                {          
                    return true;
                }
                else
                {
                    return false;
                }
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
