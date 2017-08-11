/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 185 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-28 10:42:19 +0800 (周五, 28 七月 2017) $
***************************************************************************/
using System;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.BusinessLib.Utility;
using SCA.Interface.DatabaseAccess;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.Utility;
using Neat.Dennis.Common.LoggerManager;
using System.Reflection;

namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8053 : ControllerOperationBase, IControllerOperation
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IDatabaseService _databaseService;
        private IDeviceService<DeviceInfo8053> _deviceService;
        public ControllerOperation8053()
        { 
        
        }
        public ControllerOperation8053(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _deviceService = new DeviceService8053();
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

        /// <summary>
        /// 创建回路
        /// </summary>
        /// <param name="loopsAmount">回路数量</param>
        /// <param name="deviceAmount">器件数量</param>
        /// <param name="controller">所属控制器</param>
        /// <returns></returns>
        public List<Model.LoopModel> CreateLoops(int loopsAmount,int deviceAmount, ControllerModel controller)
        {
            ILoopService loopService = new LoopService(controller); //内存操作，返回数据集
            return loopService.CreateLoops<DeviceInfo8053>(loopsAmount, deviceAmount, controller, _deviceService);
        }
        //未测试
        public bool SaveLoops(List<Model.LoopModel> lstLoopModel, ILoopDBService loopDBService, IDeviceDBService<DeviceInfo8053> deviceDBService)
        {
            try
            {
                loopDBService.AddLoopInfo(lstLoopModel);
                deviceDBService.CreateTableStructure();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Model.IDevice> OrganizeDeviceInfoForSettingController()
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

      

        public Model.ControllerType GetControllerType()
        {
            throw new NotImplementedException();
        }


        public List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new NotImplementedException();
        }


        public bool CreateController(ControllerModel controller,IDeviceTypeDBService deviceTypeDBService,IControllerDBService controllerDBService)
        {
            try
            {
                #region 控制器配置
                ControllerConfig8053 config=new ControllerConfig8053();
                string strMatchingDevTypeID=config.GetDeviceTypeCodeInfo();
               
                deviceTypeDBService.UpdateMatchingController(ControllerType.NT8053, strMatchingDevTypeID); 
                #endregion 

                #region 增加控制器信息 refactor commented
                //版本号怎么计，是按原版本号累加，还是初始化一个新的版本号
                //当前为初始化一个新的版本号
                //StringBuilder sbControllerSQL = new StringBuilder("Insert into Controller(ID,PrimaryFlag,TypeID,DeviceAddressLength,Name,PortName,BaudRate,MachineNumber,Version,ProjectID) values(");
                //sbControllerSQL.Append(controller.ID  + ",'");
                //sbControllerSQL.Append(controller.PrimaryFlag + "',");//+ "',0);");
                //sbControllerSQL.Append((int)controller.TypeCode + ",");
                //sbControllerSQL.Append(controller.DeviceAddressLength + ",'");
                //sbControllerSQL.Append(controller.Name + "','");
                //sbControllerSQL.Append(controller.PortName + "',");
                //sbControllerSQL.Append(controller.BaudRate + ",'");
                //sbControllerSQL.Append(controller.MachineNumber + "',");
                //sbControllerSQL.Append(controller.Version + ",");
                //sbControllerSQL.Append(controller.Project.ID + ")");
                //_databaseService.ExecuteBySql(sbControllerSQL);

                #endregion 
                controllerDBService.AddController(controller); 
                return true;
            }
            catch(Exception ex )
            { 
                return false;
            }
        }
        
      
        public List<LoopModel> CreateLoops(int amount)
        {
            throw new NotImplementedException();
        }


        public bool CreateController(ControllerModel model)
        {
            throw new NotImplementedException();
        }
 
        public int GetMaxDeviceID()
        {
            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8053;
            return maxDeviceID;
        }

        protected override List<short?> GetAllBuildingNoWithController(ControllerModel controller)
        {
            List<short?> lstBuildingNo = new List<short?>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8053>())
                {
                    if (!lstBuildingNo.Contains(dev.BuildingNo))
                    {
                        lstBuildingNo.Add(dev.BuildingNo);
                    }
                }
            }
            return lstBuildingNo;
        }

        protected override List<DeviceType> GetConfiguredDeviceTypeWithController(ControllerModel controller)
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();

            List<short> lstDeviceCode = new List<short>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8053>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8053 config = new ControllerConfig8053();
            List<DeviceType> lstAllTypeInfo = config.GetALLDeviceTypeInfo(null);

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

         
            List<DeviceInfo8053> lstDeviceInfo = new List<DeviceInfo8053>();           
            foreach (var l in controller.Loops)
            {
                foreach (var d in l.GetDevices<DeviceInfo8053>())
                {
                    lstDeviceInfo.Add(d);
                }
            }
            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                simulatorDevice.ControllerName = controller.Name;
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
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

        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType)
        {
            try
            {
                DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, ControllerType.NT8053);
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }

        #region 生成EXCEL模板
        protected override bool GenerateExcelTemplateLoopSheet(List<string> sheetNames, IControllerConfig config, ExcelTemplateCustomizedInfo summaryInfo, ref IExcelService excelService)
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
                    lstMergeCellRange.Clear();
                    for (int j = 0; j < summaryInfo.LoopAmountPerSheet; j++)
                    {
                        if (maxLoopAmount < (j + 1 + (i - currentLoopStartIndex) * summaryInfo.LoopAmountPerSheet))//已经超出最大回路编号，退出循环
                        {
                            break;
                        }
                        //  string deviceCode=defaultMachineNo+ defaultLoopCodeLength
                        string loopCode = summaryInfo.MachineNumberFormatted + (j + 1 + (i - currentLoopStartIndex) * summaryInfo.LoopAmountPerSheet).ToString().PadLeft(defaultLoopCodeLength, '0');
                        int extraLine = 0;
                        if (j != 0)
                        {
                            extraLine = 2;
                        }
                        //回路标题：回路1                        
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
                        mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                        lstMergeCellRange.Add(mergeCellRange);
                        //回路默认器件信息
                        for (int k = 0; k < maxDeviceAmount; k++)
                        {

                            string deviceCode = (k + 1).ToString().PadLeft(summaryInfo.SelectedDeviceCodeLength - defaultLoopCodeLength - summaryInfo.MachineNumberFormatted.Length, '0');
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 0, loopCode + deviceCode, CellStyleType.Data); //器件编码
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 1, defaultDeviceType.Name, CellStyleType.Data); //器件类型
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 2, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 3, "0", CellStyleType.Data); //屏蔽
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 4, null, CellStyleType.Data); 
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 5, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 6, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 7, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 8, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 9, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 10, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 11, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 12, null, CellStyleType.Data);
                            
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
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.Feature.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 3;
                        mergeCellRange.LastColumnIndex = 3;
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.Disable.ToString(), mergeCellRange);                        
                    }
                    excelService.SetColumnWidth(sheetNames[i], 1, 15f);
                    excelService.SetColumnWidth(sheetNames[i], deviceColumnDefinitionArray.Length-1, 50f);//安装地点
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
            try
            {
                #region 标准组态表头
                //调整为从配置信息中读取，未测试 2017-07-28
                IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.NT8053);
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetStandardLinkageConfigColumns(); //取得标准组态的列定义信息
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                int currentRowIndex = 0;
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], currentRowIndex, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                currentRowIndex++;
                for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], currentRowIndex, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                }

                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"标准组态页签"合并单元格
                //int maxStandardLinkageAmount = config.GetMaxAmountForStandardLinkageConfig();
                for (int i = 2; i < maxLinkageAmount + 3; i++)
                {
                    for (int j = 0; j < deviceColumnDefinitionArray.Length; j++)
                    {
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, j, null, CellStyleType.Data);
                    }
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 0, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 1, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 2, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 3, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 4, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 5, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 6, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 7, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 8, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 9, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 10, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 11, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 12, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 13, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 14, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 15, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 16, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 17, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 18, null, CellStyleType.Data);
                    //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 19, null, CellStyleType.Data);
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 15;
                mergeCellRange.LastColumnIndex = 15;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                excelService.SetColumnWidth(sheetNames[loopSheetAmount + currentIndex], deviceColumnDefinitionArray.Length - 1, 50f);//备注
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        protected override bool GenerateExcelTemplateMixedSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            try
            {
                #region 混合组态表头
                // 修改为从配置文件中读取，未测试2017-07-28
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.NT8053);
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetMixedLinkageConfigColumns(); //取得混合组态的列定义信息
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                }
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"混合组态页签"合并单元格                
                for (int i = 2; i < maxLinkageAmount + 3; i++)
                {
                    for (int j = 0; j < deviceColumnDefinitionArray.Length; j++)
                    {
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, j, null, CellStyleType.Data);
                    }
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 1;
                mergeCellRange.LastColumnIndex = 1;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 2;
                mergeCellRange.LastColumnIndex = 2;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 3;
                mergeCellRange.LastColumnIndex = 3;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 4;
                mergeCellRange.LastColumnIndex = 4;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageInputPartType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 10;
                mergeCellRange.LastColumnIndex = 10;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 11;
                mergeCellRange.LastColumnIndex = 11;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 12;
                mergeCellRange.LastColumnIndex = 12;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageInputPartType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 18;
                mergeCellRange.LastColumnIndex = 18;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 19;
                mergeCellRange.LastColumnIndex = 19;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 26;
                mergeCellRange.LastColumnIndex = 26;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);

                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        protected override bool GenerateExcelTemplateGeneralSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            try
            {
                #region 通用组态表头
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.NT8053);
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetGeneralLinkageConfigColumns(); //取得混合组态的列定义信息
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                }
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"通用组态页签"合并单元格                    

                for (int i = 2; i < maxLinkageAmount + 3; i++)
                {
                    for (int j = 0; j < deviceColumnDefinitionArray.Length; j++)
                    {
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, j, null, CellStyleType.Data);
                    }                    
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 1;
                mergeCellRange.LastColumnIndex = 1;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 2;
                mergeCellRange.LastColumnIndex = 2;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageInputPartType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 7;
                mergeCellRange.LastColumnIndex = 7;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeInfoForGeneralLinkageInput.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 8;
                mergeCellRange.LastColumnIndex = 8;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeAll.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 15;
                mergeCellRange.LastColumnIndex = 15;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeInfoForGeneralLinkageOutput.ToString(), mergeCellRange);
            }
                #endregion

            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        protected override bool GenerateExcelTemplateManualControlBoardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxAmount, ref IExcelService excelService)
        {
            try
            {
                #region 网络手动盘表头

                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();

                IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.NT8053);
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetManualControlBoardColumns(); 
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                }
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                lstMergeCellRange.Add(mergeCellRange);
                //修改为从配置文件中读取 未测试 2017-07-28
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "板卡号", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "手盘号", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "手键号", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "地编号", CellStyleType.TableHead);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"网络手动盘页签"合并单元格

                for (int i = 2; i < maxAmount + 3; i++)
                {
                    for (int j = 0; j < deviceColumnDefinitionArray.Length; j++)
                    {
                        
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, j, null, CellStyleType.Data);
                    }                               
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex =  maxAmount + 2;
                mergeCellRange.FirstColumnIndex = 3;
                mergeCellRange.LastColumnIndex = 3;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ManualControlBoardControlType.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxAmount + 2;
                mergeCellRange.FirstColumnIndex = 11;
                mergeCellRange.LastColumnIndex = 11;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        #endregion

        protected override bool ExistSameDeviceCode(LoopModel loop)
        {
            DeviceService8053 deviceService = new DeviceService8053();
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

        protected override List<LoopModel> ConvertToLoopModelFromDataTable(DataTable dt, ControllerModel controller, out string loopDetailErrorInfo)
        {
            loopDetailErrorInfo = "";//Initialize
            //取得当前控制器最大器件ID
            int maxDeviceID = this.GetMaxDeviceID();
            ControllerConfig8053 config = new ControllerConfig8053();
            //暂停对具体信息进行验证
            //Dictionary<string, RuleAndErrorMessage> dictDeviceDataRule = config.GetDeviceInfoRegularExpression(controller.DeviceAddressLength);
            List<LoopModel> lstLoops = new List<LoopModel>();


            string loopCode = "";
            LoopModel loop = null;
            int[] loopIndexRange = base.ParseLoopSheetName(dt.TableName);//取得工作表内回路的起始编号
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
                DeviceInfo8053 device = new DeviceInfo8053();
                maxDeviceID++;
                device.ID = maxDeviceID;
                device.Code = dt.Rows[i]["编码"].ToString();
                device.TypeCode = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["器件类型"].ToString());
                if (dt.Rows[i]["特性"].ToString() == "")
                {
                    device.Feature = null;
                }
                else
                {
                    device.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["特性"].ToString()));
                }
                device.Disable = Convert.ToInt32(dt.Rows[i]["屏蔽"].ToString().NullToZero()) == 0 ? false : true;
          
                device.LinkageGroup1 = dt.Rows[i]["输出组1"].ToString();
                device.LinkageGroup2 = dt.Rows[i]["输出组2"].ToString();
                device.LinkageGroup3 = dt.Rows[i]["输出组3"].ToString();
                
                if (dt.Rows[i]["延时"].ToString() == "")
                {
                    device.DelayValue = null;
                }
                else
                {
                    device.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["延时"].ToString()));
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
                    loop.SetDevice<DeviceInfo8053>(device);
                    loop.DeviceAmount++;
                }
            }
            //更新最大器件ID
            ProjectManager.GetInstance.MaxDeviceIDInController8053 = maxDeviceID;
            return lstLoops;           
        }

        protected override List<LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(DataTable dtStandard)
        {
            List<LinkageConfigStandard> lstStandardLinkage = new List<LinkageConfigStandard>();
            int maxID = ProjectManager.GetInstance.MaxIDForStandardLinkageConfig;
            for (int i = 0; i < dtStandard.Rows.Count; i++)
            {
                if (dtStandard.Rows[i]["输出组号"].ToString() == "")//无输出组号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                maxID++;
                LinkageConfigStandard lcs = new LinkageConfigStandard();
                lcs.ID = maxID;
                lcs.Code = dtStandard.Rows[i]["输出组号"].ToString();
                lcs.DeviceNo1 = dtStandard.Rows[i]["输入模块1"].ToString();
                lcs.DeviceNo2 = dtStandard.Rows[i]["输入模块2"].ToString();
                lcs.DeviceNo3 = dtStandard.Rows[i]["输入模块3"].ToString();
                lcs.DeviceNo4 = dtStandard.Rows[i]["输入模块4"].ToString();
                lcs.DeviceNo5 = dtStandard.Rows[i]["输入模块5"].ToString();
                lcs.DeviceNo6 = dtStandard.Rows[i]["输入模块6"].ToString();
                lcs.DeviceNo7 = dtStandard.Rows[i]["输入模块7"].ToString();
                lcs.DeviceNo8 = dtStandard.Rows[i]["输入模块8"].ToString();
                lcs.DeviceNo9 = dtStandard.Rows[i]["输入模块9"].ToString();
                lcs.DeviceNo10 = dtStandard.Rows[i]["输入模块10"].ToString();
                lcs.DeviceNo11 = dtStandard.Rows[i]["输入模块11"].ToString();
                lcs.DeviceNo12 = dtStandard.Rows[i]["输入模块12"].ToString();                
                lcs.ActionCoefficient = Convert.ToInt32(dtStandard.Rows[i]["动作常数"].ToString().NullToZero());
                lcs.LinkageNo1 = dtStandard.Rows[i]["联动组1"].ToString();
                lcs.LinkageNo2 = dtStandard.Rows[i]["联动组2"].ToString();
                lcs.LinkageNo3 = dtStandard.Rows[i]["联动组3"].ToString();
                lcs.Memo = dtStandard.Rows[i]["备注"].ToString();
                lstStandardLinkage.Add(lcs);
            }
            ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = maxID;
            return lstStandardLinkage;
        }

        protected override List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8053 config = new ControllerConfig8053();
            List<LinkageConfigMixed> lstMixedLinkage = new List<LinkageConfigMixed>();
            int maxID = ProjectManager.GetInstance.MaxIDForMixedLinkageConfig;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                maxID++;
                LinkageConfigMixed lcm = new LinkageConfigMixed();
                lcm.ID = maxID;
                lcm.Code = dt.Rows[i]["编号"].ToString();
                lcm.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"].ToString().NullToZero());

                LinkageActionType lActiontype = lcm.ActionType;
                Enum.TryParse<LinkageActionType>(EnumUtility.GetEnumName(lActiontype.GetType(), dt.Rows[i]["动作类型"].ToString()), out lActiontype);
                lcm.ActionType = lActiontype;

                LinkageType lTypeA = lcm.TypeA;
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeA.GetType(), dt.Rows[i]["A分类"].ToString()), out lTypeA);
                lcm.TypeA = lTypeA;

                LinkageInputPartType inputPartTypeA;
                Enum.TryParse<LinkageInputPartType>(EnumUtility.GetEnumName(typeof(LinkageInputPartType), dt.Rows[i]["A类别"].ToString()), out inputPartTypeA);
                lcm.CategoryA = (int)inputPartTypeA; 
            
                if (dt.Rows[i]["A楼号"].ToString() == "")
                {
                    lcm.BuildingNoA = null;
                }
                else
                {
                    lcm.BuildingNoA = Convert.ToInt32(dt.Rows[i]["A楼号"].ToString());
                }
                if (dt.Rows[i]["A区号"].ToString() == "")
                {
                    lcm.ZoneNoA = null;
                }
                else
                {
                    lcm.ZoneNoA = Convert.ToInt32(dt.Rows[i]["A区号"].ToString());
                }
                if (dt.Rows[i]["A层号"].ToString() == "")
                {
                    lcm.LayerNoA = null;
                }
                else
                {
                    lcm.LayerNoA = Convert.ToInt32(dt.Rows[i]["A层号"].ToString());
                }
                lcm.LoopNoA = dt.Rows[i]["A路号"].ToString();
                lcm.DeviceCodeA = dt.Rows[i]["A编号"].ToString();

                lcm.DeviceTypeCodeA = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["A类型"].ToString());


                LinkageType lTypeB = lcm.TypeB;
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeB.GetType(), dt.Rows[i]["B分类"].ToString()), out lTypeB);
                lcm.TypeB = lTypeB;

                LinkageInputPartType inputPartTypeB;
                Enum.TryParse<LinkageInputPartType>(EnumUtility.GetEnumName(typeof(LinkageInputPartType), dt.Rows[i]["B类别"].ToString()), out inputPartTypeB);
                lcm.CategoryB = (int)inputPartTypeB; 
                

                if (dt.Rows[i]["B楼号"].ToString() == "")
                {
                    lcm.BuildingNoB = null;
                }
                else
                {
                    lcm.BuildingNoB = Convert.ToInt32(dt.Rows[i]["B楼号"].ToString());
                }
                if (dt.Rows[i]["B区号"].ToString() == "")
                {
                    lcm.ZoneNoB = null;
                }
                else
                {
                    lcm.ZoneNoB = Convert.ToInt32(dt.Rows[i]["B区号"].ToString());
                }
                if (dt.Rows[i]["B层号"].ToString() == "")
                {
                    lcm.LayerNoB = null;
                }
                else
                {
                    lcm.LayerNoB = Convert.ToInt32(dt.Rows[i]["B层号"].ToString());
                }
                lcm.LoopNoB = dt.Rows[i]["B路号"].ToString();
                lcm.DeviceCodeB = dt.Rows[i]["B编号"].ToString();

                lcm.DeviceTypeCodeB = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["B类型"].ToString());


                LinkageType lTypeC = lcm.TypeC;
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeC.GetType(), dt.Rows[i]["C分类"].ToString()), out lTypeC);
                lcm.TypeC = lTypeC;

                if (dt.Rows[i]["C楼号"].ToString() == "")
                {
                    lcm.BuildingNoC = null;
                }
                else
                {
                    lcm.BuildingNoC = Convert.ToInt32(dt.Rows[i]["C楼号"].ToString());
                }
                if (dt.Rows[i]["C区号"].ToString() == "")
                {
                    lcm.ZoneNoC = null;
                }
                else
                {
                    lcm.ZoneNoC = Convert.ToInt32(dt.Rows[i]["C区号"].ToString());
                }
                if (dt.Rows[i]["C层号"].ToString() == "")
                {
                    lcm.LayerNoC = null;
                }
                else
                {
                    lcm.LayerNoC = Convert.ToInt32(dt.Rows[i]["C层号"].ToString());
                }
                lcm.MachineNoC = dt.Rows[i]["C机号"].ToString();
                lcm.LoopNoC = dt.Rows[i]["C回路号"].ToString();
                lcm.DeviceCodeC = dt.Rows[i]["C编号"].ToString();
                lcm.DeviceTypeCodeC = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["C类型"].ToString());
                lstMixedLinkage.Add(lcm);
            }
            ProjectManager.GetInstance.MaxIDForMixedLinkageConfig = maxID;
            return lstMixedLinkage;
        }

        protected override List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8053 config = new ControllerConfig8053();
            List<LinkageConfigGeneral> lstGeneralLinkage = new List<LinkageConfigGeneral>();
            int maxID = ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                maxID++;
                LinkageConfigGeneral lcg = new LinkageConfigGeneral();
                lcg.ID = maxID;
                lcg.Code = dt.Rows[i]["编号"].ToString();
                lcg.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"].ToString().NullToZero());


                LinkageInputPartType inputPartTypeA;
                Enum.TryParse<LinkageInputPartType>(EnumUtility.GetEnumName(typeof(LinkageInputPartType), dt.Rows[i]["A类别"].ToString()), out inputPartTypeA);
                lcg.CategoryA = (int)inputPartTypeA; 
                

                if (dt.Rows[i]["A楼号"].ToString() == "")
                {
                    lcg.BuildingNoA = null;
                }
                else
                {
                    lcg.BuildingNoA = Convert.ToInt32(dt.Rows[i]["A楼号"].ToString());
                }
                if (dt.Rows[i]["A区号"].ToString() == "")
                {
                    lcg.ZoneNoA = null;
                }
                else
                {
                    lcg.ZoneNoA = Convert.ToInt32(dt.Rows[i]["A区号"].ToString());
                }
                if (dt.Rows[i]["A层号1"].ToString() == "")
                {
                    lcg.LayerNoA1 = null;
                }
                else
                {
                    lcg.LayerNoA1 = Convert.ToInt32(dt.Rows[i]["A层号1"].ToString());
                }
                if (dt.Rows[i]["A层号2"].ToString() == "")
                {
                    lcg.LayerNoA2 = null;
                }
                else
                {
                    lcg.LayerNoA2 = Convert.ToInt32(dt.Rows[i]["A层号2"].ToString());
                }

                lcg.DeviceTypeCodeA = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["A类型"].ToString());
                LinkageType lTypeC = lcg.TypeC;
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeC.GetType(), dt.Rows[i]["C分类"].ToString()), out lTypeC);

                lcg.TypeC = lTypeC;
                if (dt.Rows[i]["C楼号"].ToString() == "")
                {
                    lcg.BuildingNoC = null;
                }
                else
                {
                    lcg.BuildingNoC = Convert.ToInt32(dt.Rows[i]["C楼号"].ToString());
                }
                if (dt.Rows[i]["C区号"].ToString() == "")
                {
                    lcg.ZoneNoC = null;
                }
                else
                {
                    lcg.ZoneNoC = Convert.ToInt32(dt.Rows[i]["C区号"].ToString());
                }
                if (dt.Rows[i]["C层号"].ToString() == "")
                {
                    lcg.LayerNoC = null;
                }
                else
                {
                    lcg.LayerNoC = Convert.ToInt32(dt.Rows[i]["C层号"].ToString());
                }
                lcg.MachineNoC = dt.Rows[i]["C机号"].ToString();
                lcg.LoopNoC = dt.Rows[i]["C回路号"].ToString();
                lcg.DeviceCodeC = dt.Rows[i]["C编号"].ToString();
                lcg.DeviceTypeCodeC = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["C类型"].ToString());
                lstGeneralLinkage.Add(lcg);
            }
            ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig = maxID;
            return lstGeneralLinkage;
        }

        protected override List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dt)
        {
            ControllerConfig8053 config = new ControllerConfig8053();
            List<ManualControlBoard> lstManualControlBoard = new List<ManualControlBoard>();
            int maxID = ProjectManager.GetInstance.MaxIDForManualControlBoard;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                maxID++;
                ManualControlBoard mcb = new ManualControlBoard();
                mcb.ID = maxID;
                mcb.Code = Convert.ToInt32(dt.Rows[i]["编号"].ToString().NullToZero());
                //mcb.BoardNo = Convert.ToInt32(dt.Rows[i][""].ToString().NullToZero());
                mcb.SubBoardNo = Convert.ToInt32(dt.Rows[i]["手盘号"].ToString().NullToZero());
                mcb.KeyNo = Convert.ToInt32(dt.Rows[i]["手键号"].ToString().NullToZero());
                ManualControlBoardControlType mcbControlType;
                Enum.TryParse<ManualControlBoardControlType>(EnumUtility.GetEnumName(typeof(ManualControlBoardControlType), dt.Rows[i]["被控类型"].ToString()), out mcbControlType);
                mcb.ControlType = (int)mcbControlType; 
                mcb.LocalDevice1 = dt.Rows[i]["本机设备1"].ToString();
                mcb.LocalDevice2 = dt.Rows[i]["本机设备2"].ToString();
                mcb.LocalDevice3 = dt.Rows[i]["本机设备3"].ToString();
                mcb.LocalDevice4 = dt.Rows[i]["本机设备4"].ToString();
                mcb.BuildingNo = dt.Rows[i]["楼号"].ToString();
                mcb.AreaNo = dt.Rows[i]["区号"].ToString();
                mcb.FloorNo = dt.Rows[i]["层号"].ToString();
                mcb.DeviceType = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["设备类型"].ToString());
                mcb.LinkageGroup = dt.Rows[i]["输出组"].ToString();
                mcb.NetDevice1 = dt.Rows[i]["网络设备1"].ToString();
                mcb.NetDevice2 = dt.Rows[i]["网络设备2"].ToString();
                mcb.NetDevice3 = dt.Rows[i]["网络设备3"].ToString();
                mcb.NetDevice4 = dt.Rows[i]["网络设备4"].ToString(); 
                lstManualControlBoard.Add(mcb);
            }
            ProjectManager.GetInstance.MaxIDForManualControlBoard = maxID;
            return lstManualControlBoard;
        }
        public void SetStaticProgressBarCancelFlag(bool flag)
        {
            ControllerOperationBase.ProgressBarCancelFlag = flag;
        }

        /// <summary>
        /// 获取控制器内不同器件类型的数量
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Dictionary<string, int> GetAmountOfDifferentDeviceType(ControllerModel controller)
        {
            Dictionary<string, int> dictDeviceTypeStatistic = new Dictionary<string, int>();
            ControllerConfig8053 config = new ControllerConfig8053();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8053> lstDistinceInfo = loop.GetDevices<DeviceInfo8053>().Distinct(new CollectionEqualityComparer<DeviceInfo8053>((x, y) => x.TypeCode == y.TypeCode)).ToList();

                        int deviceCountInLoop = loop.GetDevices<DeviceInfo8053>().Count;
                        int deviceCountInStatistic = 0;
                        foreach (var device in lstDistinceInfo)
                        {
                            DeviceType dType = config.GetDeviceTypeViaDeviceCode(device.TypeCode);
                            int typeCount = loop.GetDevices<DeviceInfo8053>().Count((d) => d.TypeCode == dType.Code);
                            if (!dictDeviceTypeStatistic.ContainsKey(dType.Name))
                            {
                                dictDeviceTypeStatistic.Add(dType.Name, typeCount);
                            }
                            else
                            {
                                dictDeviceTypeStatistic[dType.Name] += typeCount;
                            }
                        }
                    }
                }
            }
            return dictDeviceTypeStatistic;
        }


        public ControllerModel OrganizeControllerInfoFromSpecifiedDBFileVersion(IDBFileVersionService dbFileVersionService, ControllerModel controller)
        {
            throw new NotImplementedException();
        }


        public List<DeviceType> GetAllDeviceTypeOfController(ControllerModel controller)
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();
            ControllerConfig8053 config = new ControllerConfig8053();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8053> lstDistinceInfo = loop.GetDevices<DeviceInfo8053>().Distinct(new CollectionEqualityComparer<DeviceInfo8053>((x, y) => x.TypeCode == y.TypeCode)).ToList();
                        foreach (var device in lstDistinceInfo)
                        {
                            DeviceType dType = config.GetDeviceTypeViaDeviceCode(device.TypeCode);
                            lstDeviceType.Add(dType);
                        }
                    }
                }
            }
            return lstDeviceType;
        }


        public bool ExportLoopDataToExcel(ref IExcelService excelService, List<LoopModel> models, string sheetName)
        {
            throw new NotImplementedException();
        }

        public bool ExportStandardLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigStandard> models, string sheetName)
        {
            throw new NotImplementedException();
        }

        public bool ExportMixedLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigMixed> models, string sheetName)
        {
            throw new NotImplementedException();
        }

        public bool ExportGeneralLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigGeneral> models, string sheetName)
        {
            throw new NotImplementedException();
        }

        public bool ExportManualControlBoardDataToExcel(ref IExcelService excelService, List<ManualControlBoard> models, string sheetName)
        {
            throw new NotImplementedException();
        }
    }
}
