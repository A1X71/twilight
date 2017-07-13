using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SCA.Interface;
using SCA.Model;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.Utility;
using SCA.Interface.DatabaseAccess;
//using Ookii.Dialogs.Wpf;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/17 16:18:25
* FileName   : ControllerOperationBase
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public abstract class ControllerOperationBase
    {


        public event Action<int> UpdateProgressBarEvent;

        public event Action<ControllerModel, string> ReadingExcelCompletedEvent;

        public event Action<ControllerModel, string> ReadingExcelCancelationEvent;

        public event Action<string> ReadingExcelErrorEvent;

        const int DELAY_VALUE = 5000;//线程休息时间5秒(用于读取EXCEL2007文件时，增加可操作的时间窗口)
        protected static volatile bool ProgressBarCancelFlag = false;//进度取消状态
        public ControllerOperationBase()
        { 
        
        }

        protected List<Model.LoopModel> GetLoopInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldDBService)
        {
            return oldDBService.GetLoopsInfo();
            
        }
        /// <summary>
        /// 将字符型联动类型转换为枚举值 
        /// </summary>
        /// <param name="linkageType"></param>
        /// <returns></returns>
        protected Model.LinkageType ConvertLinkageType(string linkageType)
        {

            switch (linkageType)
            {
                case "区层":
                    return Model.LinkageType.ZoneLayer;
                case "地址":
                    return Model.LinkageType.Address;
                case "本层":
                    return Model.LinkageType.SameLayer;
                case "邻层":
                    return Model.LinkageType.AdjacentLayer;
                default:
                    return LinkageType.None;
            }
        }
        /// <summary>
        /// 将字符型“动作类型”转换为枚举值
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        protected Model.LinkageActionType ConvertLinkageActionType(string actionType)
        {
            switch (actionType)
            { 
                case "或":
                    return LinkageActionType.OR;
                case "与":
                    return LinkageActionType.AND;
                default:
                    return LinkageActionType.NONE;
            }
        }
        /// <summary>
        /// 取得当前项目下控制器的最大ID
        /// </summary>
        /// <returns></returns>
        protected int GetMaxControllerID()
        {
            SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;
            int tempID = 0;
            foreach (var c in project.Controllers)
            {
                tempID = c.ID == null ? 0 : c.ID;
                if (c.ID > tempID)
                {
                    tempID = c.ID;
                }
            }
            return tempID;
        }
        public bool AddControllerToProject(ControllerModel controller)
        {
            try
            {
                SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;

                if (project.Controllers.Count == 0)//如果还未设置主控制器，则默认第一个控制器为主控制器
                {
                    controller.PrimaryFlag = true;
                }
                int maxControllerID = GetMaxControllerID();
                controller.ID = maxControllerID + 1;
                controller.ProjectID = project.ID;
                controller.Project = project;
                project.Controllers.Add(controller);
                controller.IsDirty = true;
                //SetDataDirty();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将数据置为需要保存状态
        /// </summary>
        protected void SetDataDirty()
        {
            SCA.BusinessLib.ProjectManager.GetInstance.IsDirty = true;
        }

        public bool DeleteControllerBySpecifiedControllerID(int controllerID)
        {
            try
            {
                ProjectModel project = ProjectManager.GetInstance.Project;
                var result = from c in project.Controllers where c.ID == controllerID select c;
                ControllerModel controller = result.FirstOrDefault();
                if (controller != null)
                {
                    project.Controllers.Remove(controller);                    
                    //SetDataDirty();
                    DeleteControllerFromDB(controller);

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool DeleteControllerFromDB(ControllerModel controller)
        {
            try
            {
                IFileService _fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;
                IDatabaseService _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(controller.Project.SavePath, logger, _fileService);
                IControllerDBService controllerDBService = new SCA.DatabaseAccess.DBContext.ControllerDBService(_databaseService);
                ILoopDBService loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(_databaseService);
                IDeviceDBServiceTest deviceDBService = SCA.DatabaseAccess.DBContext.DeviceManagerDBServiceTest.GetDeviceDBContext(controller.Type, _databaseService);
                deviceDBService.DeleteAllDevicesByControllerID(controller.ID);
                loopDBService.DeleteLoopsByControllerID(controller.ID);
                controllerDBService.DeleteController(controller.ID);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public int GetMaxLoopID()
        {
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers select r;
            int maxLoopID = 0;
            foreach (var c in controllers)
            {
                if (c.Loops.Count > 0)
                { 
                    int currentControllerMaxLoopID = Convert.ToInt32(c.Loops.Max(loop => loop.ID));
                    if (currentControllerMaxLoopID > maxLoopID)
                    {
                        maxLoopID = currentControllerMaxLoopID;
                    }
                }
            }
            return maxLoopID;
        }
        public List<short?> GetBuildingNoCollection(int controllerID)
        {
            var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
            List<ControllerModel> lstControllers = result.ToList<ControllerModel>();

            ControllerModel controller = result.FirstOrDefault();

            List<short?> lstBuildingNo = new List<short?>();
            lstBuildingNo = GetAllBuildingNoWithController(controller);
            return lstBuildingNo;
        }
        protected abstract List<short?>  GetAllBuildingNoWithController(ControllerModel controller);

        
        public List<DeviceType> GetConfiguredDeviceTypeCollection(int controllerID)
        {
            var result = from c in ProjectManager.GetInstance.Controllers where c.ID == controllerID select c;
            List<ControllerModel> lstControllers = result.ToList<ControllerModel>();

            ControllerModel controller = result.FirstOrDefault();

            List<DeviceType> lstDeviceType = new List<DeviceType>();

            lstDeviceType = GetConfiguredDeviceTypeWithController(controller);
            return lstDeviceType;
        }
        protected abstract List<DeviceType> GetConfiguredDeviceTypeWithController(ControllerModel controller);
        //protected List<DeviceInfoForSimulator> GetSimulatorDevicesOfOtherMachine(ControllerType cType)
        //{ 
            
        //}
        //protected abstract List<DeviceInfoForSimulator> GetSimulatorDevices();
        #region EXCEL模板下载
        protected List<string> GetSheetNames(ExcelTemplateCustomizedInfo customizedInfo, ControllerNodeModel[] nodes, int loopTotalAmount,int loopAmountPerSheet,int loopSheetAmount, out string otherSheetNames)
        {
            string loopSheetNamePrefix = "";//回路页签名称前缀
            string standardLinkageSheetName = "";//标准组态页签名称
            string generalLinkageSheetName = "";//通用组态页签名称
            string mixedLinkageSheetName = "";//混合组态页签名称
            string manualControlBoardSheetName = "";//网络手动盘页签名称

            bool blnStandardLinkageFlag = false;
            bool blnMixedLinkageFlag = false;
            bool blnGeneralLinkageFlag = false;
            bool blnManualControlBoardFlag = false;

            string summarySheetName = "摘要信息";
            string deviceTypeSheetName = "设备类型";
            string sheetNamesWithoutLoopName = "";//除“所有回路页签名称”外的其它页签名称
            List<string> lstSheetNames = new List<string>();
            List<string> lstLoopSheetName;
            int sheetNamesWithoutLoopNameIndex;
            
            //模板工作薄的Sheet页构成为：
            //摘要信息页签
            //各回路页签
            //标准组态页签
            //通用组态页签
            //混合组态页签
            //网络手动盘页签          
            //  int totalSheetAmount = FIXED_SHEET_AMOUNT + loopSheetAmount;//所有页签数量
            for (int i = 0; i < nodes.Length; i++)
            {
                switch (nodes[i].Type)
                {
                    case ControllerNodeType.Loop:
                        loopSheetNamePrefix = nodes[i].Name;
                        break;
                    case ControllerNodeType.Standard:
                        standardLinkageSheetName = nodes[i].Name;
                        blnStandardLinkageFlag = true;            
                        break;
                    case ControllerNodeType.General:
                        generalLinkageSheetName = nodes[i].Name;
                        blnGeneralLinkageFlag = true;            
                        break;
                    case ControllerNodeType.Mixed:
                        mixedLinkageSheetName = nodes[i].Name;
                        blnMixedLinkageFlag = true;            
                        break;
                    case ControllerNodeType.Board:
                        manualControlBoardSheetName = nodes[i].Name;
                        blnManualControlBoardFlag = true;
                        break;
                }
            }
            
            lstSheetNames.Add(summarySheetName);
            lstSheetNames.Add(deviceTypeSheetName);

            lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, loopAmountPerSheet);
            if (customizedInfo != null) //默认模板
            {
            //    lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, loopAmountPerSheet);
            //}
            //else  //自定义模板
            //{
                //int loopAmountPerSheet_inner = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / customizedInfo.LoopGroupAmount));
                //int loopAmountPerSheet_inner = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / loopAmountPerSheet));
                
                //lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount,loopAmountPerSheet_inner);
                blnStandardLinkageFlag = customizedInfo.StandardLinkageFlag;
                blnMixedLinkageFlag = customizedInfo.MixedLinkageFlag;
                blnGeneralLinkageFlag = customizedInfo.GeneralLinkageFlag;
                blnManualControlBoardFlag = customizedInfo.ManualControlBoardFlag;
            }

            foreach (var r in lstLoopSheetName)
            {
                lstSheetNames.Add(r);
            }



            //sheetNames[loopSheetAmount + 1] = "标准组态";
            //sheetNames[loopSheetAmount + 2] = "混合组态";
            //sheetNames[loopSheetAmount + 3] = "通用组态";                
            //sheetNames[loopSheetAmount + 4] = "网络手动盘";
            //注意：在此版本中工作表的顺序不可变
            if (blnStandardLinkageFlag)
            {
                lstSheetNames.Add(standardLinkageSheetName);
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Standard, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) 
                {
                    sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; 
                }
            }
            if (blnMixedLinkageFlag) 
            {
                lstSheetNames.Add(mixedLinkageSheetName);
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Mixed, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
            }
            if (blnGeneralLinkageFlag) 
            { 
                lstSheetNames.Add(generalLinkageSheetName);
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.General, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
            }
            if (blnManualControlBoardFlag)
            {
                lstSheetNames.Add(manualControlBoardSheetName);
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Board, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
            }
            otherSheetNames = sheetNamesWithoutLoopName;
            return lstSheetNames;
        }
        protected virtual bool GenerateExcelTemplateSummarySheet(ExcelTemplateCustomizedInfo customizedInfo,ControllerType controllerType,IControllerConfig config,ref IExcelService excelService,out List<string> sheetNames,out ExcelTemplateCustomizedInfo summaryInfo)
        {
            try
            {                
                //const int FIXED_SHEET_AMOUNT = 5;//摘要信息,标准组态,通用组态,混合组态,网络手动盘
                //ControllerConfig8001 config = new ControllerConfig8001();
                List<int> lstDeviceCodeLength = config.GetDeviceCodeLength(); //器件编码长度集合
                
                Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的配置信息                
                int defaultDeviceTypeCode = config.DefaultDeviceTypeCode;//默认器件编码                
                DeviceType defaultDeviceType = config.GetDeviceTypeViaDeviceCode(defaultDeviceTypeCode);//默认器件类型
                int loopMaxLoopAmount = config.GetMaxLoopAmountValue();
                ControllerNodeModel[] nodes = config.GetNodes();
                int defaultDeviceCodeLength = lstDeviceCodeLength[0];//默认器件编码长度

                string strDeviceCodeLength = ""; //器件编码长度字符串                
                string defaultMachineNo = defaultDeviceCodeLength==8?"0".PadLeft(3,'0'):"0".PadLeft(2,'0');//默认机器号
                int defaultMachineNoLength = defaultMachineNo.Length;  //默认机器号长度
                int loopSheetAmount;                
                string sheetNamesWithoutLoopName = "";//除“所有回路页签名称”外的其它页签名称
                List<string> lstSheetNames;

                foreach (var r in lstDeviceCodeLength)
                {
                    strDeviceCodeLength += r.ToString() + ",";
                }
                strDeviceCodeLength = strDeviceCodeLength.Substring(0, strDeviceCodeLength.LastIndexOf(","));


                //此控制器可设置的回路总数量                
                int loopTotalAmount = loopMaxLoopAmount;
                string controllerName = "默认名称" + controllerType.ToString();
                string serialPort = "COM1";
                //每页签内的回路数量
                int loopAmountPerSheet = 8;

                if (customizedInfo != null)
                {
                    defaultDeviceCodeLength = customizedInfo.SelectedDeviceCodeLength;
                    switch (defaultDeviceCodeLength)
                    {
                        case 8:
                            defaultMachineNoLength = 3;
                            defaultMachineNo = customizedInfo.MachineNumber.ToString().PadLeft(defaultMachineNoLength, '0');
                            break;
                        case 7:
                            defaultMachineNoLength = 2;
                            defaultMachineNo = customizedInfo.MachineNumber.ToString().PadLeft(defaultMachineNoLength, '0');
                            break;
                        default:
                            break;
                    }

                    if (customizedInfo.LoopAmount > loopMaxLoopAmount || customizedInfo.LoopAmount <= 0)//确保回路信息在合理范围内
                    {
                        loopTotalAmount = loopMaxLoopAmount;
                    }
                    else
                    {
                        loopTotalAmount = customizedInfo.LoopAmount;
                    }
                    //分组信息<0或大于最大回路数，或大于设置的回路总数时，所有回路仅分为1组
                    if (customizedInfo.LoopGroupAmount < 0 || customizedInfo.LoopGroupAmount > loopMaxLoopAmount || customizedInfo.LoopGroupAmount > loopTotalAmount)
                    {
                        loopAmountPerSheet = loopTotalAmount;                        
                    }
                    else
                    {
                        loopAmountPerSheet = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / customizedInfo.LoopGroupAmount));
                    }

                    controllerName = customizedInfo.ControllerName;
                    serialPort = customizedInfo.SerialPortNumber;
                    defaultDeviceType = config.GetDeviceTypeViaDeviceCode(customizedInfo.DefaultDeviceTypeCode);
                }

                
                //每回路可设置最大器件数量
                int maxDeviceAmount = config.GetMaxDeviceAmountValue();
                //回路页签数量
                loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / loopAmountPerSheet));
                summaryInfo = new ExcelTemplateCustomizedInfo();//返回值 
                summaryInfo.LoopSheetAmount = loopSheetAmount; 
                summaryInfo.LoopAmountPerSheet=loopAmountPerSheet; 
                summaryInfo.MachineNumberFormatted = defaultMachineNo;
                summaryInfo.SelectedDeviceCodeLength = defaultDeviceCodeLength;
                summaryInfo.DefaultDeviceTypeCode = defaultDeviceType.Code;
                
                lstSheetNames=GetSheetNames(customizedInfo, nodes, loopTotalAmount, loopAmountPerSheet,loopSheetAmount, out sheetNamesWithoutLoopName);
                excelService.CreateExcelSheets(lstSheetNames);
                
                #region 设置单元格样式
                CellStyle cellCaptionStyle = GetCaptionCellStyle();
                CellStyle cellSubCaptionStyle = GetSubCaptionCellStyle();
                CellStyle cellDataStyle = GetDataCellStyle("Left", false);
                CellStyle cellTableHeadStyle = GetTableHeadCellStyle();
                excelService.CellCaptionStyle = cellCaptionStyle;
                excelService.CellSubCaptionStyle = cellSubCaptionStyle;
                excelService.CellDataStyle = cellDataStyle;
                excelService.CellTableHeadStyle = cellTableHeadStyle;
                #endregion
                #region  摘要信息页签合并单元格
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 1;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = 2;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 3;
                mergeCellRange.LastRowIndex = 3;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 11;
                mergeCellRange.LastRowIndex = 11;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 17;
                mergeCellRange.LastRowIndex = 17;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);

                #endregion
                #region 设置摘要页签数据模板
                excelService.RowHeight = (short)20;//到下一个高度设置前，使用该高度
                excelService.SetCellValue(lstSheetNames[0], 0, 0, "NT8001系列控制器配置数据导入模板", CellStyleType.Caption);
                excelService.RowHeight = (short)15;
                excelService.CellSubCaptionStyle = GetSubCaptionCellStyle();
                excelService.SetCellValue(lstSheetNames[0], 2, 0, "<适应软件版本" + config.CompatibleSoftwareVersionForExcelTemplate + ">", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 2, 1, null, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 2, 2, null, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 3, 0, "控制器设置", CellStyleType.SubCaption);
                excelService.SetCellValue(lstSheetNames[0], 4, 0, "名称", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 4, 1, "值", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 4, 2, "填写说明", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 5, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[5], CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 1, controllerName, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 2, "最多10个字符", CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[6], CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 1, controllerType.ToString(), CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 2, "可填: " + config.CompatibleControllerTypeForExcelTemplate, CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 8, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[8], CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 1, defaultMachineNo, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 2, "可填: 7位编码(00-" + config.GetMaxMachineAmountValue(7).ToString() + ");8位编码(000-" + config.GetMaxMachineAmountValue(8).ToString() + ")", CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 7, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[7], CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 1, defaultDeviceCodeLength.ToString(), CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 2, "可填: " + strDeviceCodeLength, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[9], CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 1, serialPort, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 2, "可填: COM1-COM9", CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 11, 0, "回路设置", CellStyleType.SubCaption);

                excelService.SetCellValue(lstSheetNames[0], 12, 0, "名称", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 1, "值", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 2, "填写说明", CellStyleType.TableHead);

                excelService.SetCellValue(lstSheetNames[0], 13, 0, "回路数量", CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 1, loopTotalAmount.ToString(), CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 2, "可填: 1-" + loopTotalAmount.ToString(), CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 14, 0, "回路分组", CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 1, loopSheetAmount.ToString(), CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 2, "可填: 1-" + loopTotalAmount.ToString(), CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 15, 0, "默认器件类型", CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 1, defaultDeviceType.Name, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 2, "可填: 有效设备编号", CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 17, 0, " 其它设置", CellStyleType.SubCaption);
                excelService.SetCellValue(lstSheetNames[0], 18, 0, "名称", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 1, "值", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 2, "填写说明", CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 19, 0, "工作表名称", CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 1, sheetNamesWithoutLoopName, CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 2, "可填: 标准组态;混合组态;通用组态;网络手动盘", CellStyleType.Data);
                //除“回路”外的工作表名称，以‘分号’分隔                
                excelService.SetMergeCells(lstSheetNames[0], lstMergeCellRange);//设置"摘要信息"合并单元格
                //设置列宽               
                excelService.SetColumnWidth(lstSheetNames[0], 0, 15f);
                excelService.SetColumnWidth(lstSheetNames[0], 1, 15f);
                excelService.SetColumnWidth(lstSheetNames[0], 2, 40f);
                #endregion
                sheetNames = lstSheetNames;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            
            return true;
        }
        protected virtual bool GenerateExcelTemplateDeviceTypeSheet(List<string> sheetNames, IControllerConfig config,ref IExcelService excelService)
        {
            try
            {
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfo();
                List<int> lstDeviceCodeLength = config.GetDeviceCodeLength(); //器件编码长度集合
                #region 生成《设备类型》页签模板
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 1;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.RowHeight = (short)20;//到下一个高度设置前，使用该高度
                excelService.SetCellValue(sheetNames[1], 0, 0, "设备类型", CellStyleType.SubCaption);
                excelService.RowHeight = (short)15;
                excelService.CellSubCaptionStyle = GetSubCaptionCellStyle();
                excelService.SetCellValue(sheetNames[1], 1, 0, " 编号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[1], 1, 1, " 名称", CellStyleType.TableHead);
                int rowNumber = 1;
                string regionNameStartIndex = "30002";
                foreach (var deviceType in lstDeviceType)
                {
                    rowNumber++;
                    excelService.SetCellValue(sheetNames[1], rowNumber, 0, deviceType.Code.ToString(), CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[1], rowNumber, 1, deviceType.Name, CellStyleType.Data);
                }
                excelService.SetMergeCells(sheetNames[1], lstMergeCellRange);//设置"摘要信息"合并单元格
                excelService.SetColumnWidth(sheetNames[1], 1, 15f);
                excelService.SetRangeName(RefereceRegionName.DeviceType.ToString(), string.Format("'{0}'!$B$3:$B${1}", sheetNames[1], (lstDeviceType.Count + 2).ToString()));
                //写入控制器类型数据
                List<ControllerType> lstControllerType = config.GetControllerType();
                if (lstControllerType != null)
                { 
                    excelService.SetCellValue(sheetNames[1], 30000, 3, "控制器类型", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var controllerTypeInner in lstControllerType)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 3, controllerTypeInner.ToString(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.ControllerType.ToString(), string.Format("'{0}'!$D$" + regionNameStartIndex + ":$D${1}", sheetNames[1], (lstControllerType.Count + 30001).ToString()));
                }
                //写入器件长度
                if (lstDeviceCodeLength != null)
                { 
                    excelService.SetCellValue(sheetNames[1], 30000, 5, "器件长度", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var codeLength in lstDeviceCodeLength)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 5, codeLength.ToString(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.DeviceCodeLength.ToString(), string.Format("'{0}'!$F$" + regionNameStartIndex + ":$F${1}", sheetNames[1], (lstDeviceCodeLength.Count + 30001).ToString()));
                }
                //写入串口号
                List<string> lstSerialPort = config.GetSerialPortNumber();
                if (lstSerialPort != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 7, "串口号", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var sp in lstSerialPort)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 7, sp, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.SerialPortNumber.ToString(), string.Format("'{0}'!$H$" + regionNameStartIndex + ":$H${1}", sheetNames[1], (lstSerialPort.Count + 30001).ToString()));
                }
                //写入动作常数
                
                List<int> lstActionCoefficient = config.GetActionCoefficient();
                if (lstActionCoefficient != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 9, "动作常数", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var actionCoefficient in lstActionCoefficient)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 9, actionCoefficient.ToString(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.ActionCoefficient.ToString(), string.Format("'{0}'!$J$" + regionNameStartIndex + ":$J${1}", sheetNames[1], (lstActionCoefficient.Count + 30001).ToString()));
                }

                //写入动作类型
                List<LinkageActionType> lstActionType = config.GetLinkageActionType();
                if (lstActionType != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 11, "动作类型", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var actionType in lstActionType)
                    {
                        rowNumber++;

                        excelService.SetCellValue(sheetNames[1], rowNumber, 11, actionType.GetDescription(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.ActionType.ToString(), string.Format("'{0}'!$L$" + regionNameStartIndex + ":$L${1}", sheetNames[1], (lstActionType.Count + 30001).ToString()));
                }
                //写入联动分类（全部）
                List<LinkageType> lstLinkageType = config.GetLinkageType();
                if (lstLinkageType != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 13, "联动分类(全部)", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var linkageType in lstLinkageType)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 13, linkageType.GetDescription(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.LinkageTypeAll.ToString(), string.Format("'{0}'!$N$" + regionNameStartIndex + ":$N${1}", sheetNames[1], (lstLinkageType.Count + 30001).ToString()));
                }
                //写入联动分类（精简）
                lstLinkageType = config.GetLinkageTypeWithCastration();
                if (lstLinkageType != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 15, "联动分类(精简)", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var linkageType in lstLinkageType)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 15, linkageType.GetDescription(), CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.LinkageTypeCastration.ToString(), string.Format("'{0}'!$P$" + regionNameStartIndex + ":$P${1}", sheetNames[1], (lstLinkageType.Count + 30001).ToString()));
                }
                //写入特性
                List<string> lstFeature = config.GetFeatureList();
                if (lstFeature != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 17, "特性", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var feature in lstFeature)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 17, feature, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.Feature.ToString(), string.Format("'{0}'!$R$" + regionNameStartIndex + ":$R${1}", sheetNames[1], (lstFeature.Count + 30001).ToString()));
                }
                //写入屏蔽
                List<string> lstDisable = config.GetDisableList();
                if (lstDisable != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 19, "屏蔽", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var disable in lstDisable)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 19, disable, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.Disable.ToString(), string.Format("'{0}'!$T$" + regionNameStartIndex + ":$T${1}", sheetNames[1], (lstDisable.Count + 30001).ToString()));
                }
                //写入灵敏度
                List<string> lstSensitiveLevel = config.GetSensitiveLevelList();
                if (lstSensitiveLevel != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 21, "灵敏度", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var sensitiveLevel in lstSensitiveLevel)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 21, sensitiveLevel, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.SensitiveLevel.ToString(), string.Format("'{0}'!$V$" + regionNameStartIndex + ":$V${1}", sheetNames[1], (lstSensitiveLevel.Count + 30001).ToString()));
                }
                //写入具有任意火警的器件名称                
                List<DeviceType> lstDeviceTypeWithAnyAlarm = config.GetDeviceTypeInfoWithAnyAlarm();
                if (lstDeviceTypeWithAnyAlarm != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 23, "器件类型(任意火警)", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var deviceType in lstDeviceTypeWithAnyAlarm)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 23, deviceType.Name, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.DeviceTypeWithAnyAlarm.ToString(), string.Format("'{0}'!$X$" + regionNameStartIndex + ":$X${1}", sheetNames[1], (lstDeviceTypeWithAnyAlarm.Count + 30001).ToString()));
                }
                //写入具有非报警器件的器件名称                                
                List<DeviceType> lstDeviceTypeWithoutFireAlarm = config.GetDeviceTypeInfoWithoutFireDevice();
                if (lstDeviceTypeWithoutFireAlarm != null)
                {
                    excelService.SetCellValue(sheetNames[1], 30000, 25, "器件类型(非火警器件)", CellStyleType.SubCaption);
                    rowNumber = 30000;
                    foreach (var deviceType in lstDeviceTypeWithoutFireAlarm)
                    {
                        rowNumber++;
                        excelService.SetCellValue(sheetNames[1], rowNumber, 25, deviceType.Name, CellStyleType.Data);
                    }
                    excelService.SetRangeName(RefereceRegionName.DeviceTypeWithoutFireDevice.ToString(), string.Format("'{0}'!$Z$" + regionNameStartIndex + ":$Z${1}", sheetNames[1], (lstDeviceTypeWithoutFireAlarm.Count + 30001).ToString()));
                }                
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置“摘要页”的下拉列表约束
        /// </summary>
        /// <param name="excelService"></param>
        /// <param name="sheetNames"></param>
        private void SetSummarySheetValidationListConstraint(ref IExcelService excelService, List<string> sheetNames)
        {
            #region 设置摘要页的序列验证
            MergeCellRange mergeCellRange = new MergeCellRange();
            mergeCellRange.FirstRowIndex = 6;
            mergeCellRange.LastRowIndex = 6;
            mergeCellRange.FirstColumnIndex = 1;
            mergeCellRange.LastColumnIndex = 1;
            excelService.SetSheetValidationForListConstraint(sheetNames[0], RefereceRegionName.ControllerType.ToString(), mergeCellRange);
            mergeCellRange = new MergeCellRange();
            mergeCellRange.FirstRowIndex = 7;
            mergeCellRange.LastRowIndex = 7;
            mergeCellRange.FirstColumnIndex = 1;
            mergeCellRange.LastColumnIndex = 1;
            excelService.SetSheetValidationForListConstraint(sheetNames[0], RefereceRegionName.DeviceCodeLength.ToString(), mergeCellRange);
            mergeCellRange = new MergeCellRange();
            mergeCellRange.FirstRowIndex = 9;
            mergeCellRange.LastRowIndex = 9;
            mergeCellRange.FirstColumnIndex = 1;
            mergeCellRange.LastColumnIndex = 1;
            excelService.SetSheetValidationForListConstraint(sheetNames[0], RefereceRegionName.SerialPortNumber.ToString(), mergeCellRange);
            #endregion
        }
        protected abstract bool GenerateExcelTemplateLoopSheet(List<string> sheetNames, IControllerConfig config, ExcelTemplateCustomizedInfo summaryInfo,ref IExcelService excelService);
        protected abstract bool GenerateExcelTemplateStandardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService);
        protected abstract bool GenerateExcelTemplateMixedSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService);
        protected abstract bool GenerateExcelTemplateGeneralSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService);
        protected abstract bool GenerateExcelTemplateManualControlBoardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxAmount, ref IExcelService excelService);
        public bool DownLoadDefaultExcelTemplate(string strFilePath, IFileService fileService, ExcelTemplateCustomizedInfo customizedInfo,ControllerType controllerType)
        {
            try
            {
                EXCELVersion version = GetExcelVersion(strFilePath);
                IExcelService excelService = ExcelServiceManager.GetExcelService(version, strFilePath, fileService);
                IControllerConfig config = ControllerConfigManager.GetConfigObject(controllerType);
                
                //int loopSheetAmount;
                //int loopAmountPerSheet;
                List<string> lstSheetNames;
                ExcelTemplateCustomizedInfo excelSummaryInfo;
                GenerateExcelTemplateSummarySheet(customizedInfo, controllerType, config, ref excelService, out lstSheetNames, out excelSummaryInfo);
                GenerateExcelTemplateDeviceTypeSheet(lstSheetNames, config, ref excelService);
                SetSummarySheetValidationListConstraint(ref excelService, lstSheetNames);
                GenerateExcelTemplateLoopSheet(lstSheetNames, config, excelSummaryInfo, ref excelService);
                bool blnStandardLinkageFlag = false;
                bool blnMixedLinkageFlag = false;
                bool blnGeneralLinkageFlag = false;
                bool blnManualControlBoardFlag = false;
                if (customizedInfo != null)
                {
                    blnStandardLinkageFlag = customizedInfo.StandardLinkageFlag;          
                    blnMixedLinkageFlag = customizedInfo.MixedLinkageFlag;           
                    blnGeneralLinkageFlag = customizedInfo.GeneralLinkageFlag;
                    blnManualControlBoardFlag = customizedInfo.ManualControlBoardFlag;
                }
                else
                {
                    ControllerNodeModel[] nodes = config.GetNodes();
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        switch (nodes[i].Type)
                        {                            
                            case ControllerNodeType.Standard:
                                blnStandardLinkageFlag = true;
                                break;
                            case ControllerNodeType.General:
                                blnGeneralLinkageFlag = true;
                                break;
                            case ControllerNodeType.Mixed:
                                blnMixedLinkageFlag = true;
                                break;
                            case ControllerNodeType.Board:
                                blnManualControlBoardFlag = true;
                                break;
                        }
                    }
                }
                if (blnStandardLinkageFlag)
                {
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Standard, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    GenerateExcelTemplateStandardSheet(lstSheetNames, currentIndex, excelSummaryInfo.LoopSheetAmount, config.GetMaxAmountForStandardLinkageConfig(), ref excelService);
                }
                if (blnMixedLinkageFlag)
                {
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Mixed, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    GenerateExcelTemplateMixedSheet(lstSheetNames, currentIndex, excelSummaryInfo.LoopSheetAmount, config.GetMaxAmountForMixedLinkageConfig(), ref excelService);
                }
                if (blnGeneralLinkageFlag)
                {
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.General, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    GenerateExcelTemplateGeneralSheet(lstSheetNames, currentIndex, excelSummaryInfo.LoopSheetAmount, config.GetMaxAmountForGeneralLinkageConfig(), ref excelService);
                }
                if (blnManualControlBoardFlag)
                {
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Board, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    GenerateExcelTemplateManualControlBoardSheet(lstSheetNames, currentIndex, excelSummaryInfo.LoopSheetAmount, config.GetMaxAmountForManualControlBoardConfig(), ref excelService);
                }
                excelService.SaveToFile();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得EXCEL版本
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        protected SCA.Interface.EXCELVersion GetExcelVersion(string strFilePath)
        {
            string extentionName = strFilePath.Substring(strFilePath.LastIndexOf('.') + 1);
            if (extentionName == "xlsx")
            {
                return EXCELVersion.EXCEL2007;
            }
            else
            {
                return EXCELVersion.EXCEL2003;
            }
        }
        /// <summary>
        /// 计算节点所在页签的相对位置
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="standardFlag"></param>
        /// <param name="mixedFlag"></param>
        /// <param name="generalFlag"></param>
        /// <param name="mcbFlag"></param>
        /// <returns></returns>
        protected int CalculateSheetRelativeIndex(ControllerNodeType nodeType, bool standardFlag, bool mixedFlag, bool generalFlag, bool mcbFlag)
        {
            int result = 2;
            switch (nodeType)
            {
                case ControllerNodeType.Standard:
                    if (standardFlag)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case ControllerNodeType.Mixed:
                    if (mixedFlag)
                    {
                        if (standardFlag)
                        {
                            result = 2;
                        }
                        else
                        {
                            result = 1;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case ControllerNodeType.General:
                    if (generalFlag)
                    {
                        if (standardFlag && mixedFlag)
                        {
                            result = 3;
                        }
                        else if ((standardFlag && mixedFlag == false) || (standardFlag == false && mixedFlag))
                        {
                            result = 2;
                        }
                        else
                        {
                            result = 1;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
                case ControllerNodeType.Board:
                    if (mcbFlag)
                    {
                        if (standardFlag && mixedFlag && generalFlag)
                        {
                            result = 4;
                        }
                        else if ((standardFlag && mixedFlag && generalFlag == false) || (standardFlag && mixedFlag == false && generalFlag) || (standardFlag == false && mixedFlag && generalFlag))
                        {
                            result = 3;
                        }
                        else if ((standardFlag && mixedFlag == false && generalFlag == false) || (standardFlag == false && mixedFlag && generalFlag == false) || (standardFlag == false && mixedFlag == false && generalFlag))
                        {
                            result = 2;
                        }
                        else
                        {
                            result = 1;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                    break;
            }
            return result + 1;// +1为新增加了一个“设备类型”页签
        }
        /// <summary>
        /// 根据“回路数量”及“回路分组”取得回路名称集合
        /// </summary>
        /// <param name="loopSheetNamePrefix">回路页签命名前缀</param>
        /// <param name="loopAmount">回路数量</param>
        /// <param name="loopGroupAmount">回路分组</param>
        /// <returns></returns>
        protected List<string> GetSheetNameForLoop(string loopSheetNamePrefix, int loopTotalAmount, int loopAmountPerSheet)
        {
        //    int loopAmountPerSheet = Convert.ToInt32(Math.Ceiling((float)loopAmount / loopGroupAmount));
            //回路页签数量
            int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / loopAmountPerSheet));
            List<string> lstSheetNames = new List<string>();
            for (int i = 1; i <= loopSheetAmount; i++)
            {
                string loopNameEndIndex;
                if ((i * loopAmountPerSheet) > loopTotalAmount)
                {
                    loopNameEndIndex = loopTotalAmount.ToString();
                }
                else
                {
                    loopNameEndIndex = (i * loopAmountPerSheet).ToString();
                }
                lstSheetNames.Add(loopSheetNamePrefix + "(" + (((i - 1) * loopAmountPerSheet) + 1).ToString() + "-" + loopNameEndIndex + ")");
            }
            return lstSheetNames;
        }
        protected CellStyle GetCaptionCellStyle()
        {
            CellStyle style = new CellStyle();
            style.HorizontalAlignment = CellStyle.HorizontalAlignmentValue.Center;
            style.VerticalAlignment = CellStyle.VerticalAlignmentValue.Center;
            style.FontName = CellStyle.FontNameValue.黑体;
            style.FontHeightInPoints = 22;
            style.BorderStyle = CellStyle.BorderStyleValue.None;
            style.BorderColor = CellStyle.BorderColorValue.None;
            return style;
        }
        /// <summary>
        /// 副标题文字样式
        /// </summary>
        /// <param name="p_workbook"></param>
        /// <returns></returns>
        protected CellStyle GetSubCaptionCellStyle()
        {
            CellStyle style = new CellStyle();
            style.HorizontalAlignment = CellStyle.HorizontalAlignmentValue.Center;
            style.VerticalAlignment = CellStyle.VerticalAlignmentValue.Center;
            style.BorderStyle = CellStyle.BorderStyleValue.None;
            style.BorderColor = CellStyle.BorderColorValue.None;
            style.FontName = CellStyle.FontNameValue.宋体;
            style.FontHeightInPoints = 12;
            style.FontBoldFlag = true;
            return style;
        }
        /// <summary>
        /// 表格头文字样式
        /// </summary>
        /// <param name="p_workbook"></param>
        /// <returns></returns>
        protected CellStyle GetTableHeadCellStyle()
        {
            CellStyle style = new CellStyle();
            style.HorizontalAlignment = CellStyle.HorizontalAlignmentValue.Center;
            style.VerticalAlignment = CellStyle.VerticalAlignmentValue.Center;
            style.BorderStyle = CellStyle.BorderStyleValue.Thin;
            style.BorderColor = CellStyle.BorderColorValue.Black;
            style.FontName = CellStyle.FontNameValue.宋体;
            style.FontHeightInPoints = 9;
            style.FontBoldFlag = true;
            return style;
        }
        protected CellStyle GetDataCellStyle(string pAlignment, bool pBoldFlag)
        {
            CellStyle style = new CellStyle();
            if (pAlignment == "Left")
            {
                style.HorizontalAlignment = CellStyle.HorizontalAlignmentValue.Left;
            }
            else if (pAlignment == "Center")
            {
                style.HorizontalAlignment = CellStyle.HorizontalAlignmentValue.Center;
            }
            style.FontBoldFlag = pBoldFlag;
            style.VerticalAlignment = CellStyle.VerticalAlignmentValue.Center;
            style.BorderStyle = CellStyle.BorderStyleValue.Thin;
            style.BorderColor = CellStyle.BorderColorValue.Black;
            style.WrapText = true;
            style.FontName = CellStyle.FontNameValue.宋体;
            style.FontHeightInPoints = 9;
            return style;
        }
        #endregion EXCEL模板下载
        #region EXCEL数据读取
        //protected abstract List<LoopModel> GetLoopData(IExcelService excelService,string filePath,string sheetName, int  maxDeviceAmount,ControllerModel controller, out bool blnSheetExistFlag, out bool loopDetailErrorInfo, out bool elapsedSingleSheetTime);
        protected List<LoopModel> GetLoopData(IExcelService excelService, string filePath, string sheetName, int maxDevcieAmount, ControllerModel controller, out bool sheetExistFlag, out string loopDetailErrorInfo, out int elapsedTime)
        {
            int[] loopRange = ParseLoopSheetName(sheetName);//取得回路编号范围
            Dictionary<int, int> rowDefinition = new Dictionary<int, int>();
            int extraLines = 2; //除数据外的其它行数
            for (int i = 0; i < (loopRange[1] - loopRange[0] + 1); i++)
            {
                if (i == 0)
                {
                    rowDefinition.Add(1, (i + 1) * maxDevcieAmount + (i + 1) * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
                }
                else
                {
                    rowDefinition.Add(i * maxDevcieAmount + (i + 1) * extraLines - 1, (i + 1) * maxDevcieAmount + (i + 1) * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
                }
            }
            DataTable dt = new DataTable();
            int intElapsedTime;
            dt = excelService.OpenExcel(filePath, sheetName, rowDefinition, out sheetExistFlag, out intElapsedTime);
            elapsedTime = intElapsedTime;
            return ConvertToLoopModelFromDataTable(dt, controller, out loopDetailErrorInfo);
        }
        /// <summary>
        /// 解析回路工作表的名称，取得回路号范围 
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        protected int[] ParseLoopSheetName(string sheetName)
        {
            int lineCharIndex = sheetName.IndexOf('-');
            int leftParenthesis = sheetName.IndexOf('(');
            int rightParenthesis = sheetName.IndexOf(')');

            string strLoopStartIndex = sheetName.Substring(leftParenthesis + 1, lineCharIndex - leftParenthesis - 1);
            string strLoopEndIndex = sheetName.Substring(lineCharIndex + 1, rightParenthesis - lineCharIndex - 1);
            int loopStartIndex = 1;
            int loopEndIndex = 1;
            if (strLoopStartIndex != "")
            {
                loopStartIndex = Convert.ToInt32(strLoopStartIndex);
            }
            if (strLoopEndIndex != "")
            {
                loopEndIndex = Convert.ToInt32(strLoopEndIndex);
            }
            //loopStartIndex = loopStartIndex == 0 ? 1 : loopStartIndex;
            //loopEndIndex = loopEndIndex == 0 ? 1 : loopEndIndex;
            int[] loopIndex = new int[2];
            //for(int i=loopStartIndex-1;i<=loopEndIndex-1 ;i++)
            //{
            //    loopIndex[i] = i+1;
            //}         
            loopIndex[0] = loopStartIndex;
            loopIndex[1] = loopEndIndex;
            return loopIndex;
        }

        protected abstract bool ExistSameDeviceCode(LoopModel loop);//检查回路内的器件编码是否有重复
        protected abstract List<LoopModel> ConvertToLoopModelFromDataTable(DataTable dt, ControllerModel controller, out string loopDetailErrorInfo);
        protected abstract List<LinkageConfigStandard>  ConvertToStandardLinkageModelFromDataTable(DataTable dtStandard);
        protected abstract List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dtMixed);
        protected abstract List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dtGeneral);
        protected abstract List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dtManualControlBoard);

        private ControllerModel GetControllerViaExcelTemplate(string name, string value, ControllerModel controller)
        {
            ControllerModel controllerInfo = controller;
            switch (name)
            {
                case "控制器名称":
                    controllerInfo.Name = value;
                    break;
                case "控制器类型":
                    {
                        ControllerType controllerType;
                        Enum.TryParse<ControllerType>(value, out controllerType);
                        controllerInfo.Type = controllerType;
                    }
                    break;
                case "控制器机号":
                    controllerInfo.MachineNumber = value;
                    break;
                case "器件长度":
                    controllerInfo.DeviceAddressLength = Convert.ToInt32(value);
                    break;
                case "串口号":
                    controllerInfo.PortName = value;
                    break;

            }
            return controllerInfo;
        }

        private bool ReadingExcelSummarySheet(ref BackgroundWorker bw, ReadExcelLoopArgumentForIn args,IControllerConfig config,out List<string> loopSheetNames, out List<string> otherSettingSheetNames, out EXCELVersion version, out IExcelService excelService,out ControllerModel controller,out string strStatus ,ref float cumulativeTime)
        {

            List<string> lstLoopSheetName;
            List<string> lstOtherSheetName;
            
            try
            {
                float totalTime = 100;  //总EXCEL读取时间

                int percentageValue = Convert.ToInt32(cumulativeTime / totalTime) * 100; //进度百分比
                //  ReadExcelLoopArgumentForIn args = (ReadExcelLoopArgumentForIn)e.Argument;
                DeviceService8001 deviceService = new DeviceService8001();

                bool blnSheetExistFlag;
                string loopDetailErrorInfo; //回路错误信息
                int elapsedSingleSheetTime; //执行时间
                string strStatusInfo = ""; //状态信息
                ControllerAndStatus result = new ControllerAndStatus();
                cumulativeTime = 0;

                float averageTime = 0; //平均执行时间,避免进度忽大忽小
                version = GetExcelVersion(args.FilePath);
                excelService = ExcelServiceManager.GetExcelService(version, args.FilePath, args.FileService);
                // ControllerConfig8001 config = new ControllerConfig8001();

                ControllerModel innerController = new ControllerModel();
                controller = innerController;

                ControllerNodeModel[] nodes = config.GetNodes();
                ColumnConfigInfo[] deviceColumnDefinitions = config.GetDeviceColumns();


                Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的名称配置信息
                Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate(args.Controller.DeviceAddressLength); ////取得“摘要信息页”控制器设置的值的有效性
                Dictionary<int, string> dictNameOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetNameOfLoopSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”回路设置的名称配置信息
                Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”回路设置的值的有效性
                Dictionary<int, string> dictNameOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetNameOfOtherSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”其它设置的名称配置信息
                Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”其它设置的值的有效性

                bool blnIsError = false;     //错误标志    
                int controllerSettingStartRow = 4;
                int loopSettingStartRow = controllerSettingStartRow + dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + 3;//3为“控制器表头行+空行+回路标题"
                int otherSettingStartRow = loopSettingStartRow + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + 3;
                //定义“摘要信息”页的数据行定义信息
                Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();
                summarySheetRowDefinition.Add(controllerSettingStartRow, 9);//控制器信息起始行定义 4为表头信息,
                summarySheetRowDefinition.Add(loopSettingStartRow, 15);//回路信息起始行定义
                summarySheetRowDefinition.Add(otherSettingStartRow, 19);//其它信息起始行定义               

                bw.ReportProgress(percentageValue); //进度条初始化

                if (version == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
                }
                if (ProgressBarCancelFlag)
                {
                    controller = null;
                    loopSheetNames=null;
                    otherSettingSheetNames=null;                    
                    //result.Controller = null;
                    //result.Status = "用户取消操作";
                    strStatus = strStatusInfo;
                    return false;
                }
                
                DataTable dt = excelService.OpenExcel(args.FilePath, config.SummarySheetNameForExcelTemplate, summarySheetRowDefinition, out blnSheetExistFlag, out elapsedSingleSheetTime);
                #region 计算时间 更新进度百分比
                cumulativeTime += elapsedSingleSheetTime;
                averageTime = cumulativeTime;
                //totalTime = elapsedSingleSheetTime * args.LoopSheetNames.Count;
                totalTime = averageTime * 74;
                percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
                bw.ReportProgress(percentageValue);
                #endregion
                //ControllerModel controller = new ControllerModel(); //存储"控制器配置"信息
                int loopAmount = 1;//回路数量
                int loopGroup = 1;//回路分组
                string loopSheetNamePrefix = "";//回路页签名称前缀                
                for (int i = 0; i < nodes.Length; i++)
                {
                    switch (nodes[i].Type)
                    {
                        case ControllerNodeType.Loop:
                            loopSheetNamePrefix = nodes[i].Name;
                            break;
                    }
                }
                List<string> lstSheetNames = new List<string>();// 除“摘要信息”之外的页签名称
                for (int i = 0; i < dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count; i++)
                {
                    if (!(dt.Rows[i][0].ToString() == dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[controllerSettingStartRow + 1 + i])) //+1为跨过标题行
                    {
                        strStatusInfo = "摘要信息-->控制器设置-->模板名称不正确";
                        blnIsError = true;
                    }
                    else //名称一致，验证值的有效性
                    {
                        RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate[controllerSettingStartRow + 1 + i];
                        Regex exminator = new Regex(verifingMessage.Rule);
                        if (!exminator.IsMatch(dt.Rows[i][1].ToString()))
                        {
                            strStatusInfo += ";" + verifingMessage.ErrorMessage;
                            blnIsError = true;
                            continue;
                        }
                        else
                        {
                            controller = GetControllerViaExcelTemplate(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), innerController);
                        }
                    }
                }
                if (!blnIsError)
                {
                    if (controller.Type != args.Controller.Type)
                    {
                        strStatusInfo += ";模板控制器类型(" + controller.Type.ToString() + ")与当前控制器类型(" + args.Controller.Type.ToString() + ")不一致";
                        blnIsError = true;
                    }
                    if (controller.DeviceAddressLength != args.Controller.DeviceAddressLength)
                    {
                        strStatusInfo += ";模板器件长度(" + controller.DeviceAddressLength + ")与当前控制器器件长度(" + args.Controller.DeviceAddressLength + ")不一致";
                        blnIsError = true;
                    }
                    if (controller.MachineNumber != args.Controller.MachineNumber)
                    {
                        strStatusInfo += ";模板控制器机号(" + controller.MachineNumber + ")与当前控制器机号(" + args.Controller.MachineNumber + ")不一致";
                        blnIsError = true;
                    }                    
                }
                if (!blnIsError)//控制器配置信息正确
                {
                    //controller
                    //验证回路信息
                    for (int i = 0; i < dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count; i++)
                    {
                        if (!(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == dictNameOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow + 1 + i]))//+1为跨过标题行
                        {
                            strStatusInfo = "摘要信息-->回路设置-->模板名称不正确";
                            blnIsError = true;
                        }
                        else //名称一致，验证值的有效性
                        {
                            RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow + 1 + i];
                            //verifingMessage.Rule
                            //verifingMessa、ge.ErrorMessage                     
                            Regex exminator = new Regex(verifingMessage.Rule);
                            if (!exminator.IsMatch(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString()))
                            {
                                strStatusInfo += ";" + verifingMessage.ErrorMessage;
                                blnIsError = true;
                                continue;
                            }
                            else //验证结果正确，存储读入的数据
                            {
                                if (dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == "回路数量")
                                {
                                    loopAmount = Convert.ToInt32(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString());
                                }
                                else if (dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == "回路分组")
                                {
                                    loopGroup = Convert.ToInt32(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Status = strStatusInfo;
                    result.Controller = controller;
                   // ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件                    
                    //controller = null;
                    loopSheetNames = null;
                    otherSettingSheetNames = null;
                    strStatus = strStatusInfo;
                    return false;
                }
                if (blnIsError) //存在错误，直接返回
                {
                    result.Status = strStatusInfo;
                    result.Controller = controller;
                    
                 //   ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
                    loopSheetNames = null;
                    otherSettingSheetNames = null;
                    strStatus = strStatusInfo;
                    return false;
                }
                lstOtherSheetName = new List<string>();//除“回路”外的其它工作表名称
                string otherSettingValue = "";//以“分号”分隔的页签名称
                if (!blnIsError)//取得除回路外的工作表名称
                {
                    for (int i = 0; i < dictNameOfOtherSettingInSummaryInfoOfExcelTemplate.Count; i++)
                    {
                        if (!(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == dictNameOfOtherSettingInSummaryInfoOfExcelTemplate[otherSettingStartRow + 1 + i]))//+1为跨过标题行
                        {
                            strStatusInfo = "摘要信息-->其它设置-->名称不正确";
                            blnIsError = true;
                        }
                        else //名称一致，验证值的有效性
                        {
                            RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate[otherSettingStartRow + 1 + i];
                            Regex exminator = new Regex(verifingMessage.Rule);
                            if (!exminator.IsMatch(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString()))
                            {
                                strStatusInfo += ";摘要信息-->其它设置-->" + verifingMessage.ErrorMessage;
                                blnIsError = true;
                                continue;
                            }
                            else
                            {
                                otherSettingValue = dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString();
                            }
                        }
                    }
                }
                if (blnIsError)
                {
                    result.Status = strStatusInfo;
                    result.Controller = controller;
                    loopSheetNames = null;
                    otherSettingSheetNames = null;  
                //    ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
                    strStatus = strStatusInfo;
                    return false;
                }

                //分组信息>回路数量时，重置回路分组信息
                if (loopGroup > loopAmount)
                {
                    loopGroup = loopAmount;
                }
                int loopAmountPerSheet = Convert.ToInt32(Math.Ceiling((float)loopAmount / loopGroup));
                lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopAmount, loopAmountPerSheet);//根据“摘要信息”取得所有回路数据的工作表名称
                string [] otherSheetNames = otherSettingValue.Split(';');
                for (int i = 0; i < otherSheetNames.Length; i++)
                {
                    lstOtherSheetName.Add(otherSheetNames[i]);
                }
                
            }
            catch(Exception ex)
            {
                loopSheetNames = null;
                otherSettingSheetNames = null;
                version = EXCELVersion.EXCEL2003;
                controller = null;
                excelService = null;
                strStatus = "EXCEPTION:";
                return false;
            }
            loopSheetNames = lstLoopSheetName;
            otherSettingSheetNames = lstOtherSheetName;
            strStatus = "";
            return true;
        }
        public void ReadEXCELTemplate(string strFilePath, IFileService fileService, ControllerModel targetController)
        {
            ReadExcelLoopArgumentForIn inArgs = new ReadExcelLoopArgumentForIn();

            inArgs.FileService = fileService;
            inArgs.FilePath = strFilePath;
            inArgs.Controller = targetController;
            DoWorkEventArgs args = new DoWorkEventArgs(inArgs);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ReadingEXCELDoWork;
            worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(UpdateProgress);
            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(ReadingEXCELCompleteWork);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(inArgs);
        }
        private void ReadingEXCELDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            string strStatusInfo = "";
            ControllerAndStatus result = new ControllerAndStatus();
            ReadExcelLoopArgumentForIn args = (ReadExcelLoopArgumentForIn)e.Argument;
            IControllerConfig config = ControllerConfigManager.GetConfigObject(args.Controller.Type);
            List<string> lstLoopSheetName;
            List<string> lstOtherSheetName;
            EXCELVersion excelVersion;
            IExcelService excelService;
            List<LoopModel> lstLoops = null;//从EXCEL文件中取得数据
            ControllerModel controller;
            int loopCount = 0;
            int maxDeviceAmount = config.GetMaxDeviceAmountValue(); //回路内器件信息最大数量
            float cumulativeTime = 0;  //当前累积时间
            bool blnSheetExistFlag = false;
            string loopDetailErrorInfo;
            int elapsedSingleSheetTime=0;
            float totalTime = 100;  //总EXCEL读取时间
            float averageTime = 0; //平均执行时间,避免进度忽大忽小
            int percentageValue ; //进度百分比
            if (!ReadingExcelSummarySheet(ref bw, args, config, out lstLoopSheetName, out lstOtherSheetName, out excelVersion, out excelService, out controller,out strStatusInfo, ref cumulativeTime))
            {
                //if (ProgressBarCancelFlag)
                //{
                bw.CancelAsync();
                //}
                if (bw.CancellationPending)
                {
                    if (strStatusInfo == "EXCEPTION:")
                    {
                        strStatusInfo += ";EXCEL文件打开失败(文件可能已打开)";
                    }
                    result.Status = strStatusInfo;
                    result.Controller = controller;
                    e.Result = result;
                    e.Cancel = true; //有错误信息，取消执行
                    ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件  
                    return;
                }
            }           
            int totoalSheets = lstLoopSheetName.Count + lstOtherSheetName.Count;//总共需要读取的Sheet页
            foreach (var sheetName in lstLoopSheetName)
            {
                if (excelVersion == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
                }
                if (ProgressBarCancelFlag)
                {
                    bw.CancelAsync();
                }
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    ReadingExcelCancelationEvent(null, null);//触发取消事件
                    return;
                }

                loopCount++;
                Console.WriteLine("开始读取" + sheetName);
                lstLoops = GetLoopData(excelService, args.FilePath, sheetName, maxDeviceAmount, controller, out blnSheetExistFlag, out loopDetailErrorInfo, out elapsedSingleSheetTime);
                
                cumulativeTime += elapsedSingleSheetTime;
                averageTime = cumulativeTime / loopCount;

                //totalTime = elapsedSingleSheetTime * args.LoopSheetNames.Count;
                totalTime = averageTime * totoalSheets;                
                percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
                bw.ReportProgress(percentageValue);
                if (lstLoops != null)
                {
                    if (blnSheetExistFlag)
                    {
                        foreach (var loop in lstLoops)
                        {
                            if (ExistSameDeviceCode(loop))
                            {
                                loopDetailErrorInfo += ";" + loop.Code + "回路存在重码";
                            }
                            else
                            {
                                controller.Loops.Add(loop);
                            }
                            //deviceService.TheLoop = loop;
                            //if (deviceService.IsExistSameDeviceCode())
                            //{
                            //    loopDetailErrorInfo += ";" + loop.Code + "回路存在重码";
                            //}
                            //else
                            //{
                            //    controller.Loops.Add(loop);
                            //}
                        }
                    }
                    else
                    {
                        loopDetailErrorInfo += ";《" + sheetName + "》工作表名称配置不正确";
                    }
                }
                if (loopDetailErrorInfo != "") //有错误信息，则直接返回调用
                {
                    strStatusInfo += ";" + loopDetailErrorInfo;
                    //  strErrorMessage = strStatusInfo;
                    result.Status = strStatusInfo;
                    result.Controller = controller;
                    e.Result = result;
                    e.Cancel = true; //有错误信息，取消执行
                    ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
                    return;
                }
                Console.WriteLine("结束读取" + sheetName);
            }

            foreach (var sheetName in lstOtherSheetName)
            {
                if (excelVersion == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
                {
                    new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
                }
                if (ProgressBarCancelFlag)
                {
                    bw.CancelAsync();
                }
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    ReadingExcelCancelationEvent(null, null);//触发取消事件
                    return;
                }
                loopCount++;
                config.GetMaxAmountForStandardLinkageConfig();
                switch (sheetName)
                {
                    case "标准组态":
                        {
                            DataTable dtStandard = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForStandardLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
                            if (blnSheetExistFlag)
                            {
                                List<LinkageConfigStandard> lstStandardConfig = ConvertToStandardLinkageModelFromDataTable(dtStandard);
                                if (lstStandardConfig != null)
                                {
                                    LinkageConfigStandardService standardConfigService = new LinkageConfigStandardService(controller);
                                    if (standardConfigService.IsExistSameCode(lstStandardConfig))
                                    {
                                        strStatusInfo += ";" + sheetName + "存在重码";
                                    }
                                    else
                                    {
                                        foreach (var r in lstStandardConfig)
                                        {
                                            controller.StandardConfig.Add(r);
                                        }
                                    }
                                }
                                //else
                                //{
                                //    strStatusInfo += ";《" + sheetName + "》为空，未实现";
                                //}
                            }
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                    case "混合组态":
                        {
                            DataTable dtMixed = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForMixedLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
                            if (blnSheetExistFlag)
                            {
                                List<LinkageConfigMixed> lstMixedConfig = ConvertToMixedLinkageModelFromDataTable(dtMixed);
                                if (lstMixedConfig != null)
                                {
                                    LinkageConfigMixedService mixedConfigService = new LinkageConfigMixedService(controller);
                                    if (mixedConfigService.IsExistSameCode(lstMixedConfig))
                                    {
                                        strStatusInfo += ";" + sheetName + "存在重码";
                                    }
                                    else
                                    {
                                        foreach (var r in lstMixedConfig)
                                        {
                                            controller.MixedConfig.Add(r);
                                        }
                                    }
                                }
                                //else
                                //{
                                //    strStatusInfo += ";《" + sheetName + "》为空，未实现";
                                //}                                
                            }
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                    case "通用组态":
                        {
                            DataTable dtGeneral = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
                            if (blnSheetExistFlag)
                            {

                                List<LinkageConfigGeneral> lstGeneralConfig = ConvertToGeneralLinkageModelFromDataTable(dtGeneral);
                                if (lstGeneralConfig != null)
                                {
                                    LinkageConfigGeneralService generalConfigService = new LinkageConfigGeneralService(controller);
                                    if (generalConfigService.IsExistSameCode(lstGeneralConfig))
                                    {
                                        strStatusInfo += ";" + sheetName + "存在重码";
                                    }
                                    else
                                    {
                                        foreach (var r in lstGeneralConfig)
                                        {
                                            controller.GeneralConfig.Add(r);
                                        }
                                    }
                                }
                                //else
                                //{
                                //    strStatusInfo += ";《" + sheetName + "》为空，未实现";
                                //} 
                            }
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                    case "网络手动盘":
                        {
                            DataTable dtMCB = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
                            if (blnSheetExistFlag)
                            {
                                List<ManualControlBoard> lstMCB = ConvertToManualControlBoardModelFromDataTable(dtMCB);
                                if (lstMCB != null)
                                {
                                    ManualControlBoardService mcbService = new ManualControlBoardService(controller);
                                    if (mcbService.IsExistSameCode(lstMCB))
                                    {
                                        strStatusInfo += ";" + sheetName + "存在重码";
                                    }
                                    else
                                    {
                                        foreach (var r in lstMCB)
                                        {
                                            controller.ControlBoard.Add(r);
                                        }
                                    }
                                }
                                //else
                                //{
                                //    strStatusInfo += ";《" + sheetName + "》为空，未实现";
                                //} 
                            }
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                }
                cumulativeTime += elapsedSingleSheetTime;
                averageTime = cumulativeTime / loopCount;
                totalTime = averageTime * totoalSheets;
                percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
                bw.ReportProgress(percentageValue);
            }
            result.Controller = controller;
            result.Status = strStatusInfo;
            e.Result = result;
        }
        private DataTable GetOtherSettingData(IExcelService excelService, string filePath, string sheetName, int maxRowIndex, out bool sheetExistFlag, out int elapsedTime)
        {
            Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();
            summarySheetRowDefinition.Add(1, maxRowIndex + 2 - 1); //2为标题+表头,由于从0开始记数，需要减1        
            DataTable dt = new DataTable();
            dt = excelService.OpenExcel(filePath, sheetName, summarySheetRowDefinition, out sheetExistFlag, out  elapsedTime);
            return dt;
        }
        void UpdateProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            int progress = e.ProgressPercentage;
            UpdateProgressBarEvent(progress);

        }
        void ReadingEXCELCompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {

                
                Console.WriteLine("I have an error!-->" + e.Error.Message.ToString());
                
                ReadingExcelErrorEvent( e.Error.Message.ToString());

            }
            else if (e.Cancelled)
            {
                
             //   ControllerAndStatus result = (ControllerAndStatus)e.Result;
            //    ReadingExcelCancelationEvent(result.Controller, result.Status);

            }
            else
            {
                ControllerAndStatus result = (ControllerAndStatus)e.Result;
                ReadingExcelCompletedEvent(result.Controller, result.Status);
            }
        }

        #endregion
        /// <summary>
        /// 获取控制器内不同器件类型的数量
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public abstract Dictionary<string, int> GetAmountOfDifferentDeviceType(ControllerModel controller);
            
        /// <summary>
        /// 取得控制器摘要信息
        /// </summary>
        /// <param name="controller">控制器信息</param>
        /// <param name="startLevel">控制器信息缩进级别</param>
        /// <returns></returns>
        public SummaryInfo GetSummaryNodes(ControllerModel controller, int startLevel)
        {          
            SummaryInfo summary = new SummaryInfo();            
            summary.Icon = @"Resources\Icon\Style1\Controller.jpg"; 
            summary.Name = "控制器:" + controller.Name + "(" + controller.Type.ToString() + "," + controller.DeviceAddressLength.ToString() + ")";
            summary.Number = 1;
            summary.Level = startLevel;
            //summary.ChildNodes.Add();
            IControllerConfig config = ControllerConfigManager.GetConfigObject(controller.Type);
            ControllerNodeModel[] nodes = config.GetNodes();
            for (int i = 0; i < nodes.Length; i++)
            {
                switch (nodes[i].Type)
                {
                    case ControllerNodeType.Loop:
                        {
                            SummaryInfo node = new SummaryInfo();
                            node.Icon =  @"Resources\Icon\Style1\Loop.jpg";
                            node.Name = ControllerNodeType.Loop.GetDescription();
                            node.Number = controller.Loops.Count;
                            node.Level = startLevel + 1;
                            foreach (var l in controller.Loops)
                            {
                                SummaryInfo subNode = new SummaryInfo();
                                subNode.Icon = "";
                                subNode.Name = l.Name;
                                subNode.Number = l.DeviceAmount;
                                subNode.Level = startLevel + 2;
                                node.ChildNodes.Add(subNode);
                            }
                            summary.ChildNodes.Add(node);
                        }
                        break;
                    case ControllerNodeType.Standard:
                        {
                            SummaryInfo node = new SummaryInfo();
                            node.Icon = @"Resources\Icon\Style1\Linkage.jpg";
                            node.Name = ControllerNodeType.Standard.GetDescription();
                            node.Number = controller.StandardConfig.Count;
                            node.Level = startLevel + 1;
                            summary.ChildNodes.Add(node);
                        }
                        break;

                    case ControllerNodeType.Mixed:
                        {
                            SummaryInfo node = new SummaryInfo();
                            node.Icon = @"Resources\Icon\Style1\Linkage.jpg";
                            node.Name = ControllerNodeType.Mixed.GetDescription();
                            node.Number = controller.MixedConfig.Count;
                            node.Level = startLevel + 1;
                            summary.ChildNodes.Add(node);
                        }
                        break;
                    case ControllerNodeType.General:
                        {
                            SummaryInfo node = new SummaryInfo();
                            node.Icon = @"Resources\Icon\Style1\Linkage.jpg";
                            node.Name = ControllerNodeType.General.GetDescription();
                            node.Number = controller.GeneralConfig.Count;
                            node.Level = startLevel + 1;
                            summary.ChildNodes.Add(node);
                        }
                        break;
                    case ControllerNodeType.Board:
                        {
                            SummaryInfo node = new SummaryInfo();
                            node.Icon = @"Resources\Icon\Style1\Linkage.jpg";
                            node.Name = ControllerNodeType.Board.GetDescription();
                            node.Number = controller.ControlBoard.Count;
                            node.Level = startLevel + 1;
                            summary.ChildNodes.Add(node);
                        }
                        break;
                }
            }
            Dictionary<string, int> dictDeviceTypeCount = GetAmountOfDifferentDeviceType(controller);
            if (dictDeviceTypeCount.Count > 0)
            {
                SummaryInfo node = new SummaryInfo();
                node.Icon = "";
                node.Name = "设备类型";
                node.Level = startLevel + 1;
                foreach (var d in dictDeviceTypeCount)
                {
                    SummaryInfo typeNode = new SummaryInfo();
                    typeNode.Icon = "";
                    typeNode.Name = d.Key;
                    typeNode.Number = d.Value;
                    typeNode.Level = startLevel + 2;
                    node.ChildNodes.Add(typeNode);
                    node.Number = node.Number + d.Value;
                }
                summary.ChildNodes.Add(node);
            }
            return summary;
        }
    }
    public struct ReadExcelLoopArgumentForIn
    {
        //public IExcelService ExcelService;
        public IFileService FileService;
        public string FilePath;
        //public string SheetName;
        //public int MaxDeviceAmount;
        public ControllerModel Controller;
        //public List<string> LoopSheetNames;
        //public List<string> OtherSettingSheetNames;
        //public string statusInfo;
    }
    struct ControllerAndStatus
    {
        public ControllerModel Controller;
        public string Status;
    }
}
