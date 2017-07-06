using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using SCA.BusinessLib.Utility;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/25 8:04:29
* FileName   : ControllerOperation8021
* Description: NT8021控制器操作类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8021:ControllerOperationBase,IControllerOperation
    {
        public ControllerOperation8021()
        { 
        
        }
        public ControllerOperation8021(IDatabaseService databaseService)
        { 
        
        }
        public Model.ControllerNodeModel[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public Model.ControllerNodeType GetControllerNodeType()
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> GetLoops(int controllerID)
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> CreateLoops(int amount)
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> CreateLoops(int loopsAmount, int deviceAmount, Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }

        public List<Model.IDevice> OrganizeDeviceInfoForSettingController()
        {
            throw new NotImplementedException();
        }

        public List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new NotImplementedException();
        }

        public List<Model.LinkageConfigStandard> OrganizeLikageStandardInfoForSettingController()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            throw new NotImplementedException();
        }

        public bool CreateController(Model.ControllerModel model)
        {
            throw new NotImplementedException();
        }

        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(Interface.DatabaseAccess.IOldVersionSoftwareDBService oldVersionSoftwareDBService)
        {
            throw new NotImplementedException();
        }

        public Model.ControllerType GetControllerType()
        {
            throw new NotImplementedException();
        }


        public int GetMaxDeviceID()
        {
            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8021;
            //var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8021 select r;
            //int maxDeviceID = 0;
            //foreach (var c in controllers)
            //{
            //    foreach (var l in c.Loops)
            //    {
            //        List<DeviceInfo8021> lstDeviceInfo = l.GetDevices<DeviceInfo8021>();
            //        if (lstDeviceInfo.Count > 0)
            //        {
            //            int currentLoopMaxDeviceID = lstDeviceInfo.Max(device => device.ID);
            //            if (currentLoopMaxDeviceID > maxDeviceID)
            //            {
            //                maxDeviceID = currentLoopMaxDeviceID;
            //            }
            //        }
            //    }
            //}
            return maxDeviceID;
        }

        protected override List<short?> GetAllBuildingNoWithController(ControllerModel controller)
        {
            return null;
        }

        protected override List<DeviceType> GetConfiguredDeviceTypeWithController(ControllerModel controller)
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();

            List<short> lstDeviceCode = new List<short>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8021>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8021 config = new ControllerConfig8021();
            List<DeviceType> lstAllTypeInfo = config.GetALLDeviceTypeInfo(null);
            //var result =from c in lstAllTypeInfo where c.Code=

            foreach (var v in lstDeviceCode)
            {
                var result = lstAllTypeInfo.Where((s) => s.Code == v);
                if (result.Count() > 0)
                {
                    lstDeviceType.Add(result.FirstOrDefault());
                }
            }
            return lstDeviceType;
        }
     
        public List<DeviceInfoForSimulator> GetSimulatorDevices(ControllerModel controller)
        {
          //  var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8021 select r;
            List<DeviceInfo8021> lstDeviceInfo = new List<DeviceInfo8021>();
           // foreach (var c in controllers)
          //  {
                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8021>())
                    {
                        lstDeviceInfo.Add(d);
                    }
                }
           // }
            

            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                //simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                //simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                //simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
                simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            return lstDeviceSimulator;
        }


        public List<DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode, ControllerModel controller, List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine)
        {
            throw new NotImplementedException();
        }


        #region 弃用
        //public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo)
        //{
        //    #region 未重构的代码 注释掉
        //    //EXCELVersion version = base.GetExcelVersion(strFilePath);            
        //    //ControllerConfig8021 config = new ControllerConfig8021();
        //    //List<int> lstDeviceCodeLength = config.GetDeviceCodeLength(); //器件编码长度集合
        //    //ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetDeviceColumns(); //取得器件的列定义信息
        //    //Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的配置信息
        //    //int defaultDeviceTypeCode = config.DefaultDeviceTypeCode;//默认器件编码                
        //    //DeviceType defaultDeviceType = config.GetDeviceTypeViaDeviceCode(defaultDeviceTypeCode);//默认器件类型
        //    //int loopMaxLoopAmount = config.GetMaxLoopAmountValue();
        //    //ControllerNodeModel[] nodes = config.GetNodes();
        //    //string strDeviceCodeLength = ""; //器件编码长度字符串                
        //    //string defaultMachineNo = "00";//默认机器号
        //    //int defaultMachineNoLength = defaultMachineNo.Length;  //默认机器号长度
        //    //int defaultLoopCodeLength = 2;//默认回路编码长度
        //    //int defaultDeviceCodeLength = 7;//默认器件编码长度
        //    //string loopSheetNamePrefix = "";//回路页签名称前缀
        //    //string standardLinkageSheetName = "";//标准组态页签名称
        //    //string generalLinkageSheetName = "";//通用组态页签名称
        //    //string mixedLinkageSheetName = "";//混合组态页签名称
        //    //string manualControlBoardSheetName = "";//网络手动盘页签名称
        //    ////此控制器可设置的回路总数量                
        //    //int loopTotalAmount = loopMaxLoopAmount;
        //    //string controllerName = "默认名称NT8021";
        //    //string serialPort = "COM1";
        //    ////每页签内的回路数量
        //    //int loopAmountPerSheet = 8;
        //    ////记录是否需要写
        //    //bool blnStandardLinkageFlag = true;
        //    //bool blnMixedLinkageFlag = true;
        //    //bool blnGeneralLinkageFlag = true;
        //    //bool blnManualControlBoardFlag = true;

        //    //foreach (var r in lstDeviceCodeLength)
        //    //{
        //    //    strDeviceCodeLength += r.ToString() + ",";
        //    //}
        //    //strDeviceCodeLength = strDeviceCodeLength.Substring(0, strDeviceCodeLength.LastIndexOf(","));
        //    //for (int i = 0; i < nodes.Length; i++)
        //    //{
        //    //    switch (nodes[i].Type)
        //    //    {
        //    //        case ControllerNodeType.Loop:
        //    //            loopSheetNamePrefix = nodes[i].Name;
        //    //            break;
        //    //        case ControllerNodeType.Standard:
        //    //            standardLinkageSheetName = nodes[i].Name;
        //    //            break;
        //    //        case ControllerNodeType.General:
        //    //            generalLinkageSheetName = nodes[i].Name;
        //    //            break;
        //    //        case ControllerNodeType.Mixed:
        //    //            mixedLinkageSheetName = nodes[i].Name;
        //    //            break;
        //    //        case ControllerNodeType.Board:
        //    //            manualControlBoardSheetName = nodes[i].Name;
        //    //            break;
        //    //    }
        //    //}
        //    //if (customizedInfo != null)
        //    //{
        //    //    defaultDeviceCodeLength = customizedInfo.SelectedDeviceCodeLength;
        //    //    switch (defaultDeviceCodeLength)
        //    //    {
        //    //        case 8:
        //    //            defaultMachineNoLength = 3;
        //    //            defaultMachineNo = customizedInfo.MachineNumber.ToString().PadLeft(defaultMachineNoLength, '0');
        //    //            break;
        //    //        case 7:
        //    //            defaultMachineNoLength = 2;
        //    //            defaultMachineNo = customizedInfo.MachineNumber.ToString().PadLeft(defaultMachineNoLength, '0');
        //    //            break;
        //    //        default:
        //    //            break;
        //    //    }

        //    //    if (customizedInfo.LoopAmount > loopMaxLoopAmount || customizedInfo.LoopAmount <= 0)//确保回路信息在合理范围内
        //    //    {
        //    //        loopTotalAmount = loopMaxLoopAmount;
        //    //    }
        //    //    else
        //    //    {
        //    //        loopTotalAmount = customizedInfo.LoopAmount;
        //    //    }
        //    //    //分组信息<0或大于最大回路数，或大于设置的回路总数时，所有回路仅分为1组
        //    //    if (customizedInfo.LoopGroupAmount < 0 || customizedInfo.LoopGroupAmount > loopMaxLoopAmount || customizedInfo.LoopGroupAmount > loopTotalAmount)
        //    //    {
        //    //        loopAmountPerSheet = loopTotalAmount;
        //    //    }
        //    //    else
        //    //    {
        //    //        loopAmountPerSheet = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / customizedInfo.LoopGroupAmount));
        //    //    }

        //    //    controllerName = customizedInfo.ControllerName;
        //    //    serialPort = customizedInfo.SerialPortNumber;
        //    //    blnStandardLinkageFlag = customizedInfo.StandardLinkageFlag;
        //    //    blnMixedLinkageFlag = customizedInfo.MixedLinkageFlag;
        //    //    blnGeneralLinkageFlag = customizedInfo.GeneralLinkageFlag;
        //    //    blnManualControlBoardFlag = customizedInfo.ManualControlBoardFlag;
        //    //}
        //    ////回路页签数量
        //    //int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / loopAmountPerSheet));
        //    ////每回路可设置最大器件数量
        //    //int maxDeviceAmount = config.GetMaxDeviceAmountValue();
        //    //string summarySheetName = "摘要信息";
        //    //string deviceTypeSheetName = "设备类型";
        //    ////模板工作薄的Sheet页构成为：
        //    ////摘要信息页签
        //    ////各回路页签
        //    ////标准组态页签
        //    ////通用组态页签
        //    ////混合组态页签
        //    ////网络手动盘页签          
        //    ////  int totalSheetAmount = FIXED_SHEET_AMOUNT + loopSheetAmount;//所有页签数量
        //    //List<string> lstSheetNames = new List<string>();
        //    //lstSheetNames.Add(summarySheetName);
        //    //lstSheetNames.Add(deviceTypeSheetName);
        //    //List<string> lstLoopSheetName;
        //    //if (customizedInfo == null) //默认模板
        //    //{
        //    //    lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, loopAmountPerSheet);
        //    //}
        //    //else  //自定义模板
        //    //{
        //    //    lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, customizedInfo.LoopGroupAmount);
        //    //}

        //    //foreach (var r in lstLoopSheetName)
        //    //{
        //    //    lstSheetNames.Add(r);
        //    //}
        //    ////sheetNames[loopSheetAmount + 1] = "标准组态";
        //    ////sheetNames[loopSheetAmount + 2] = "混合组态";
        //    ////sheetNames[loopSheetAmount + 3] = "通用组态";                
        //    ////sheetNames[loopSheetAmount + 4] = "网络手动盘";
        //    ////注意：在此版本中工作表的顺序不可变
        //    //if (blnStandardLinkageFlag) { lstSheetNames.Add(standardLinkageSheetName); }
        //    //if (blnMixedLinkageFlag) { lstSheetNames.Add(mixedLinkageSheetName); }
        //    //if (blnGeneralLinkageFlag) { lstSheetNames.Add(generalLinkageSheetName); }
        //    //if (blnManualControlBoardFlag) { lstSheetNames.Add(manualControlBoardSheetName); }

        //    //string sheetNamesWithoutLoopName = "";//除“所有回路页签名称”外的其它页签名称
        //    //int sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Standard, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
        //    //if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
        //    //sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Mixed, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
        //    //if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
        //    //sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.General, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
        //    //if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
        //    //sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Board, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
        //    //if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }

        //    //IExcelService excelService = ExcelServiceManager.GetExcelService(version, strFilePath, fileService);
        //    ////Excel2007Service excelService = new Excel2007Service(strFilePath, fileService);                
        //    //excelService.CreateExcelSheets(lstSheetNames);
        //    //#region 设置单元格样式
        //    //CellStyle cellCaptionStyle = GetCaptionCellStyle();
        //    //CellStyle cellSubCaptionStyle = GetSubCaptionCellStyle();
        //    //CellStyle cellDataStyle = GetDataCellStyle("Left", false);
        //    //CellStyle cellTableHeadStyle = GetTableHeadCellStyle();
        //    //excelService.CellCaptionStyle = cellCaptionStyle;
        //    //excelService.CellSubCaptionStyle = cellSubCaptionStyle;
        //    //excelService.CellDataStyle = cellDataStyle;
        //    //excelService.CellTableHeadStyle = cellTableHeadStyle;
        //    //#endregion
        //    //return true;

        //    #endregion
        //    try
        //    {
        //        DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, ControllerType.NT8021);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //        return false;
        //    }
        //}
        #endregion

        protected override bool GenerateExcelTemplateLoopSheet(List<string> sheetNames, IControllerConfig config, Model.BusinessModel.ExcelTemplateCustomizedInfo summaryInfo, ref IExcelService excelService)
        {

            try
            {
                #region 生成所有回路页签模板
                int currentLoopStartIndex = 2;
                int defaultLoopCodeLength = 2;//默认回路编码长度
                string loopSheetNamePrefix = "";//回路页签名称前缀
                short maxDeviceAmount = config.GetMaxDeviceAmountValue();
                int maxLoopAmount = config.GetMaxLoopAmountValue(); //允许最大回路数量
                int defaultDeviceTypeCode = summaryInfo.DefaultDeviceTypeCode;//默认器件编码                
                DeviceType defaultDeviceType = config.GetDeviceTypeViaDeviceCode(defaultDeviceTypeCode);//默认器件类型
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetDeviceColumns(); //取得器件的列定义信息
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                ControllerNodeModel[] nodes = config.GetNodes();
                for (int i = 0; i < nodes.Length; i++)
                {
                    switch (nodes[i].Type)
                    {
                        case ControllerNodeType.Loop:
                            loopSheetNamePrefix = nodes[i].Name;
                            break;
                    }
                }
                for (int i = currentLoopStartIndex; i <= summaryInfo.LoopSheetAmount + 1; i++)
                {

                    for (int j = 0; j < summaryInfo.LoopAmountPerSheet; j++)
                    {
                        if (maxLoopAmount < (j + 1 + (i - currentLoopStartIndex) * summaryInfo.LoopAmountPerSheet))//已经超出最大回路编号，退出循环
                        {
                            break;
                        }
                        //回路号
                        string loopCode = summaryInfo.MachineNumberFormatted + (j + 1 + (i - currentLoopStartIndex) * summaryInfo.LoopAmountPerSheet).ToString().PadLeft(defaultLoopCodeLength, '0');
                        int extraLine = 0;//非数据行数
                        if (j != 0)
                        {
                            extraLine = 2;
                        }
                        //回路标题                      
                        excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + j * extraLine, 0, loopSheetNamePrefix + ":" + loopCode, CellStyleType.SubCaption);
                        for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                        {
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1 + j * extraLine, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                        }
                        //回路标题行合并
                        MergeCellRange mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.FirstColumnIndex = 0;
                        mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length;
                        lstMergeCellRange.Add(mergeCellRange);
                        //回路默认器件信息
                        for (int k = 0; k < maxDeviceAmount; k++)
                        {
                            string deviceCode = (k + 1).ToString().PadLeft(summaryInfo.SelectedDeviceCodeLength - defaultLoopCodeLength - summaryInfo.MachineNumberFormatted.Length, '0');
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 0, loopCode + deviceCode, CellStyleType.Data); //器件编码
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 1, defaultDeviceType.Name, CellStyleType.Data); //器件类型                            
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 2, "0", CellStyleType.Data); //屏蔽                            
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 3, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 4, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 5, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 6, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 7, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 8, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 9, null, CellStyleType.Data);                            
                        }
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 1;
                        mergeCellRange.LastColumnIndex = 1;
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.DeviceType.ToString(), mergeCellRange);                        
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 2;
                        mergeCellRange.LastColumnIndex = 2;
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.Disable.ToString(), mergeCellRange);                        
                    }
                    excelService.SetColumnWidth(sheetNames[i], 1, 15f);
                    excelService.SetMergeCells(sheetNames[i], lstMergeCellRange);//设置"回路页签"合并单元格
                }
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected override bool GenerateExcelTemplateStandardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateMixedSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateGeneralSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateManualControlBoardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }


        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType)
        {
            return base.DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, controllerType);
        }

        protected override bool ExistSameDeviceCode(LoopModel loop)
        {
            DeviceService8021 deviceService = new DeviceService8021();
            deviceService.TheLoop = loop;
            if (deviceService.IsExistSameDeviceCode())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override List<LoopModel> ConvertToLoopModelFromDataTable(System.Data.DataTable dt, ControllerModel controller, out string loopDetailErrorInfo)
        {
            loopDetailErrorInfo = "";//Initialize
            //取得当前控制器最大器件ID
            int maxDeviceID = this.GetMaxDeviceID();
            ControllerConfig8021 config = new ControllerConfig8021();
            //暂停对具体信息进行验证
            //Dictionary<string, RuleAndErrorMessage> dictDeviceDataRule = config.GetDeviceInfoRegularExpression(controller.DeviceAddressLength);
            List<LoopModel> lstLoops = new List<LoopModel>();


            string loopCode = "";
            LoopModel loop = null;
            int[] loopIndexRange = ParseLoopSheetName(dt.TableName);//取得工作表内回路的起始编号
            int loopsAmount = loopIndexRange[1] - loopIndexRange[0] + 1;
            int[] loopIndex = new int[loopsAmount];
            for (int i = 0; i < loopsAmount; i++)
            {
                loopIndex[i] = loopIndexRange[0] + i;
            }
            int loopCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (loopCode != dt.Rows[i]["编码"].ToString().Substring(0, controller.DeviceAddressLength - 3))//器件编码为3位
                {

                    //新建回路
                    loopCode = dt.Rows[i]["编码"].ToString().Substring(0, controller.DeviceAddressLength - 3);
                    loop = new LoopModel();
                    loop.Code = loopCode;
                    loop.Name = loopCode;
                    lstLoops.Add(loop);
                    string machineNumberOfDevice = loop.Code.Substring(0, controller.DeviceAddressLength - 5);
                    bool blnErrorFlag = false;
                    if (controller.MachineNumber != machineNumberOfDevice)  //控制器机号验证
                    {
                        loopDetailErrorInfo += ";" + dt.TableName + "-器件(" + dt.Rows[i]["编码"].ToString() + ")机号(" + machineNumberOfDevice + ")与模板机号(" + controller.MachineNumber + ")不符";
                        blnErrorFlag = true;
                    }
                    if (!blnErrorFlag)
                    {
                        if (loopIndex[loopCount].ToString().PadLeft(controller.LoopAddressLength, '0') != loop.Code.Substring(machineNumberOfDevice.Length)) //回路号验证
                        {
                            loopDetailErrorInfo += ";器件(" + dt.Rows[i]["编码"].ToString() + ")回路号(" + loop.Code.Substring(machineNumberOfDevice.Length) + ")与模板回路号(" + loopIndex[loopCount].ToString().PadLeft(controller.LoopAddressLength, '0') + ")不符";
                            blnErrorFlag = true;
                        }
                    }
                    if (blnErrorFlag)  //机号或回路号不正确，停止处理，返回调用
                    {
                        return null;
                    }
                    loopCount++;
                }
                DeviceInfo8021 device = new DeviceInfo8021();
                maxDeviceID++;
                device.ID = maxDeviceID;
                device.Code = dt.Rows[i]["编码"].ToString();
                device.TypeCode = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["器件类型"].ToString());       
                device.Disable = new Nullable<bool>(Convert.ToBoolean(dt.Rows[i]["屏蔽"].ToString() == "0" ? false : true));
                if (dt.Rows[i]["电流报警值"].ToString() == "")
                {
                    device.CurrentThreshold = null;
                }
                else
                {
                    device.CurrentThreshold = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["电流报警值"].ToString()));
                }
                if (dt.Rows[i]["温度报警值"].ToString() == "")
                {
                    device.TemperatureThreshold = null;
                }
                else
                {
                    device.TemperatureThreshold = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["温度报警值"].ToString()));
                }
                
      
                if (dt.Rows[i]["楼号"].ToString() == "")
                {
                    device.BuildingNo = null;
                }
                else
                {
                    device.BuildingNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["楼号"].ToString()));
                }
                if (dt.Rows[i]["区号"].ToString() == "")
                {
                    device.ZoneNo = null;
                }
                else
                {
                    device.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["区号"].ToString()));
                }
                if (dt.Rows[i]["层号"].ToString() == "")
                {
                    device.FloorNo = null;
                }
                else
                {
                    device.FloorNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["层号"].ToString()));
                }
                if (dt.Rows[i]["房间号"].ToString() == "")
                {
                    device.RoomNo = null;
                }
                else
                {
                    device.RoomNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["房间号"].ToString()));
                }
                device.Location = dt.Rows[i]["安装地点"].ToString();
                if (loop != null)
                {
                    device.Loop = loop;
                    //在合适位置设置
                    //device.LoopID
                    loop.SetDevice<DeviceInfo8021>(device);
                    loop.DeviceAmount++;
                }
            }
            //更新最大器件ID
            ProjectManager.GetInstance.MaxDeviceIDInController8021 = maxDeviceID;
            return lstLoops;          
        }

        protected override List<LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(System.Data.DataTable dtStandard)
        {
            return null;
        }

        protected override List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(System.Data.DataTable dtMixed)
        {
            return null;
        }

        protected override List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(System.Data.DataTable dtGeneral)
        {
            return null;
        }

        protected override List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(System.Data.DataTable dtManualControlBoard)
        {
            return null;
        }
        public void SetStaticProgressBarCancelFlag(bool flag)
        {
            ControllerOperationBase.ProgressBarCancelFlag = flag;
        }
    }
}
