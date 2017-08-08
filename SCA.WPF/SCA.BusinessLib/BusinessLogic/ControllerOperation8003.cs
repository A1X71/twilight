using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLib.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/24 16:45:22
* FileName   : ControllerOperation8003
* Description: FT8003控制器操作类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8003 : ControllerOperationBase, IControllerOperation
    {
        public ControllerOperation8003()
        { 
        
        }
        public ControllerOperation8003(IDatabaseService databaseService)
        { 
        
        }
        public Model.ControllerNodeModel[] GetNodes()
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerNodeType GetControllerNodeType()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LoopModel> GetLoops(int controllerID)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LoopModel> CreateLoops(int amount)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LoopModel> CreateLoops(int loopsAmount, int deviceAmount, Model.ControllerModel controller)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.IDevice> OrganizeDeviceInfoForSettingController()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LinkageConfigStandard> OrganizeLikageStandardInfoForSettingController()
        {
            throw new System.NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            throw new System.NotImplementedException();
        }

        public bool CreateController(Model.ControllerModel model)
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldVersionSoftwareDBService)
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerType GetControllerType()
        {
            throw new System.NotImplementedException();
        }


        public int GetMaxDeviceID()
        {
            //var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.FT8003 select r;
            //int maxDeviceID = 0;
            //foreach (var c in controllers)
            //{
            //    foreach (var l in c.Loops)
            //    {
            //        List<DeviceInfo8003> lstDeviceInfo = l.GetDevices<DeviceInfo8003>();
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
            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8003;
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
                foreach (var dev in l.GetDevices<DeviceInfo8003>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8003 config = new ControllerConfig8003();
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
          //  var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.FT8003 select r;
            List<DeviceInfo8003> lstDeviceInfo = new List<DeviceInfo8003>();
          //  foreach (var c in controllers)
          //  {
                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8003>())
                    {
                        lstDeviceInfo.Add(d);
                    }
                }
          //  }     
            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.ControllerName = controller.Name;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                //simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
               // simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            return lstDeviceSimulator;
        }


        public List<DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode, ControllerModel controller, List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine)
        {
            throw new System.NotImplementedException();
        }

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
                DeviceType defaultDeviceType = config.GetDeviceTypeViaDeviceCode(defaultDeviceTypeCode);
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
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 4, "2", CellStyleType.Data); //灵敏度
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
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.Feature.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 3;
                        mergeCellRange.LastColumnIndex = 3;
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.Disable.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 4;
                        mergeCellRange.LastColumnIndex = 4;
                        excelService.SetSheetValidationForListConstraint(sheetNames[i], RefereceRegionName.SensitiveLevel.ToString(), mergeCellRange);
                    }
                    excelService.SetColumnWidth(sheetNames[i], 1, 15f);
                    excelService.SetColumnWidth(sheetNames[i], 9, 50f);
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
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();                
                //调整为从配置信息中读取，未测试 2017-07-28
                IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.FT8003);
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetStandardLinkageConfigColumns(); //取得标准组态的列定义信息                
                
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);                
                for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                }

                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "输出组号", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "联动模块1", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "联动模块2", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "联动模块3", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "联动模块4", CellStyleType.TableHead);                
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 5, "动作常数", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 6, "联动组1", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 7, "联动组2", CellStyleType.TableHead);
                //excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 8, "联动组3", CellStyleType.TableHead);

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
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 0, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 1, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 2, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 3, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 4, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 5, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 6, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 7, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 8, null, CellStyleType.Data);
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 5;
                mergeCellRange.LastColumnIndex = 5;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);

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
            throw new System.NotImplementedException();
        }

        protected override bool GenerateExcelTemplateGeneralSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new System.NotImplementedException();
        }

        protected override bool GenerateExcelTemplateManualControlBoardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxAmount, ref IExcelService excelService)
        {
            throw new System.NotImplementedException();
        }


        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType)
        {
            try
            {
                DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, ControllerType.FT8003);
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            } 
        }

        protected override bool ExistSameDeviceCode(LoopModel loop)
        {
            DeviceService8003 deviceService = new DeviceService8003();
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
            ControllerConfig8003 config = new ControllerConfig8003();
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
                DeviceInfo8003 device = new DeviceInfo8003();
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
                device.Disable = new Nullable<bool>(Convert.ToBoolean(dt.Rows[i]["屏蔽"].ToString() == "0" ? false : true));
                if (dt.Rows[i]["灵敏度"].ToString() == "")
                {
                    device.SensitiveLevel = null;
                }
                else
                {
                    device.SensitiveLevel = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["灵敏度"].ToString()));
                }
                device.LinkageGroup1 = dt.Rows[i]["输出组1"].ToString();
                device.LinkageGroup2 = dt.Rows[i]["输出组2"].ToString();                
                if (dt.Rows[i]["延时"].ToString() == "")
                {
                    device.DelayValue = null;
                }
                else
                {
                    device.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["延时"].ToString()));
                }                
                if (dt.Rows[i]["区号"].ToString() == "")
                {
                    device.ZoneNo = null;
                }
                else
                {
                    device.ZoneNo = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["区号"].ToString()));
                }                
                device.Location = dt.Rows[i]["安装地点"].ToString();
                if (loop != null)
                {
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8003>(device);
                    loop.DeviceAmount++;
                }
            }
            //更新最大器件ID
            ProjectManager.GetInstance.MaxDeviceIDInController8003 = maxDeviceID;
            return lstLoops;         
        }

        protected override List<LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(System.Data.DataTable dtStandard)
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
                lcs.DeviceNo1 = dtStandard.Rows[i]["联动模块1"].ToString();
                lcs.DeviceNo2 = dtStandard.Rows[i]["联动模块2"].ToString();
                lcs.DeviceNo3 = dtStandard.Rows[i]["联动模块3"].ToString();
                lcs.DeviceNo4 = dtStandard.Rows[i]["联动模块4"].ToString();                
                lcs.ActionCoefficient = Convert.ToInt32(dtStandard.Rows[i]["动作常数"].ToString().NullToZero());
                lcs.LinkageNo1 = dtStandard.Rows[i]["联动组1"].ToString();
                lcs.LinkageNo2 = dtStandard.Rows[i]["联动组2"].ToString();
                lcs.LinkageNo3 = dtStandard.Rows[i]["联动组3"].ToString();
                lstStandardLinkage.Add(lcs);
            }
            ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = maxID;
            return lstStandardLinkage;
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
        /// <summary>
        /// 获取控制器内不同器件类型的数量
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Dictionary<string, int> GetAmountOfDifferentDeviceType(ControllerModel controller)
        {
            Dictionary<string, int> dictDeviceTypeStatistic = new Dictionary<string, int>();
            ControllerConfig8003 config = new ControllerConfig8003();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8003> lstDistinceInfo = loop.GetDevices<DeviceInfo8003>().Distinct(new CollectionEqualityComparer<DeviceInfo8003>((x, y) => x.TypeCode == y.TypeCode)).ToList();

                        int deviceCountInLoop = loop.GetDevices<DeviceInfo8003>().Count;
                      //  int deviceCountInStatistic = 0;
                        foreach (var device in lstDistinceInfo)
                        {
                            DeviceType dType = config.GetDeviceTypeViaDeviceCode(device.TypeCode);
                            int typeCount = loop.GetDevices<DeviceInfo8003>().Count((d) => d.TypeCode == dType.Code); //记录器件类型的数量
                            if (!dictDeviceTypeStatistic.ContainsKey(dType.Name))
                            {
                                dictDeviceTypeStatistic.Add(dType.Name, typeCount);
                            }
                            else
                            {
                                dictDeviceTypeStatistic[dType.Name] += typeCount;
                            }
                            //deviceCountInStatistic += typeCount;
                            //if (deviceCountInStatistic == deviceCountInLoop)
                            //{
                            //    break;
                            //}
                        }
                    }
                }
            }
            return dictDeviceTypeStatistic;
        }
        public List<DeviceType> GetAllDeviceTypeOfController(ControllerModel controller)
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();
            ControllerConfig8003 config = new ControllerConfig8003();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8003> lstDistinceInfo = loop.GetDevices<DeviceInfo8003>().Distinct(new CollectionEqualityComparer<DeviceInfo8003>((x, y) => x.TypeCode == y.TypeCode)).ToList();
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
        public ControllerModel OrganizeControllerInfoFromSpecifiedDBFileVersion(IDBFileVersionService dbFileVersionService, ControllerModel controller)
        {
            ControllerModel controllerInfo = controller;
            List<LoopModel> lstLoopInfo = dbFileVersionService.GetLoopsByController(controller);
            StringBuilder sbQuerySQL = new StringBuilder();            

            //(1)回路及器件
            foreach (var l in lstLoopInfo)//回路信息
            {
                LoopModel loop = l;
                dbFileVersionService.GetDevicesByLoopForControllerType8003(ref loop);//为loop赋予“器件信息”
                loop.Controller = controllerInfo;
                controllerInfo.Loops.Add(loop);
            }
            //(2)标准组态
            List<LinkageConfigStandard> lstStandard = dbFileVersionService.GetStandardLinkageConfig(controller);
            foreach (var l in lstStandard)
            {
                LinkageConfigStandard standardConfig = l;
                standardConfig.Controller = controllerInfo;
                controllerInfo.StandardConfig.Add(standardConfig);
            }

            return controllerInfo;
        }
        /// <summary>
        /// 将回路数据写入指定的EXCEL工作表
        /// </summary>
        /// <param name="excelService"></param>
        /// <param name="models"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public bool ExportLoopDataToExcel(ref IExcelService excelService, List<LoopModel> models, string sheetName)
        {
            try
            {
                if (models.Count > 0)
                {
                    IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.FT8003);
                    ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetDeviceColumns(); //取得器件的列定义信息
                    List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                    int startRowIndex = 0;
                    int currentRowIndex = 0;
                    foreach (var loop in models)
                    {
                        startRowIndex = currentRowIndex;
                        currentRowIndex++;
                        //回路标题                        
                        excelService.SetCellValue(sheetName, startRowIndex, 0, "回路:" + loop.Name, CellStyleType.SubCaption);
                        for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                        {
                            excelService.SetCellValue(sheetName, currentRowIndex, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                        }
                        //回路标题行合并
                        MergeCellRange mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = startRowIndex;
                        mergeCellRange.LastRowIndex = startRowIndex;
                        mergeCellRange.FirstColumnIndex = 0;
                        mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                        lstMergeCellRange.Add(mergeCellRange);
                        //写入器件信息         
                        foreach (var device in loop.GetDevices<DeviceInfo8003>())
                        {
                            currentRowIndex++;
                            excelService.SetCellValue(sheetName, currentRowIndex, 0, device.Code, CellStyleType.Data); //器件编码
                            excelService.SetCellValue(sheetName, currentRowIndex, 1, config.GetDeviceTypeViaDeviceCode(device.TypeCode).Name, CellStyleType.Data); //器件类型
                            excelService.SetCellValue(sheetName, currentRowIndex, 2, device.Feature, CellStyleType.Data); //特性
                            excelService.SetCellValue(sheetName, currentRowIndex, 3, device.Disable, CellStyleType.Data); //屏蔽
                            excelService.SetCellValue(sheetName, currentRowIndex, 4, device.SensitiveLevel, CellStyleType.Data); //灵敏度
                            excelService.SetCellValue(sheetName, currentRowIndex, 5, device.LinkageGroup1, CellStyleType.Data); //输出组1
                            excelService.SetCellValue(sheetName, currentRowIndex, 6, device.LinkageGroup2, CellStyleType.Data); //输出组2                            
                            excelService.SetCellValue(sheetName, currentRowIndex, 7, device.DelayValue, CellStyleType.Data); //延时
                            excelService.SetCellValue(sheetName, currentRowIndex, 8, device.sdpKey, CellStyleType.Data); //区号                            
                            excelService.SetCellValue(sheetName, currentRowIndex, 9, device.Location, CellStyleType.Data);//安装地点                        }
                        }
                        excelService.SetMergeCells(sheetName, lstMergeCellRange);//设置"回路页签"合并单元格
                        excelService.SetColumnWidth(sheetName, 1, 15f);
                        excelService.SetColumnWidth(sheetName, 9, 50f);


                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ExportStandardLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigStandard> models, string sheetName)
        {
            try
            {
                if (models.Count > 0)
                {
                    IControllerConfig config = ControllerConfigManager.GetConfigObject(ControllerType.FT8003);
                    ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetStandardLinkageConfigColumns(); //取得标准组态的列定义信息
                    List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                    int currentRowIndex = 0;
                    excelService.SetCellValue(sheetName, currentRowIndex, 0, sheetName, CellStyleType.SubCaption);
                    currentRowIndex++;
                    for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                    {
                        excelService.SetCellValue(sheetName, currentRowIndex, devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, CellStyleType.TableHead);
                    }
                    MergeCellRange mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length - 1;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(sheetName, lstMergeCellRange);
                    foreach (var model in models)
                    {
                        currentRowIndex++;
                        excelService.SetCellValue(sheetName, currentRowIndex, 0, model.Code, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 1, model.DeviceNo1, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 2, model.DeviceNo2, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 3, model.DeviceNo3, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 4, model.DeviceNo4, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 5, model.ActionCoefficient.ToString(), CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 6, model.LinkageNo1, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 7, model.LinkageNo2, CellStyleType.Data);
                        excelService.SetCellValue(sheetName, currentRowIndex, 8, model.LinkageNo3, CellStyleType.Data);                        
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool ExportMixedLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigMixed> models, string sheetName)
        {

            throw new NotImplementedException("无混合组态");
                
        }
        public bool ExportGeneralLinkageDataToExcel(ref IExcelService excelService, List<LinkageConfigGeneral> models, string sheetName)
        {

            throw new NotImplementedException("无通用组态");
        }
        public bool ExportManualControlBoardDataToExcel(ref IExcelService excelService, List<ManualControlBoard> models, string sheetName)
        {
            throw new NotImplementedException("无网络手动盘");
        }



    }
}
