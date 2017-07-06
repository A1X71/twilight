﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using SCA.Model.BusinessModel;
using System.Data;
using SCA.BusinessLib.Utility;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/17 16:11:41
* FileName   : ControllerOperation8001
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8001:ControllerOperationBase,IControllerOperation
    {

        
        ///// <summary>
        ///// 更新进度条
        ///// </summary>
        //public event Action<int> UpdateProgressBarEvent;        
        ///// <summary>
        ///// EXCEL读取完毕事件
        ///// </summary>
        //public event Action<ControllerModel, string> ReadingExcelCompletedEvent;
        //public event Action<ControllerModel, string> ReadingExcelCancelationEvent;
        public ControllerOperation8001()
        { 
        
        }
        public ControllerOperation8001(IDatabaseService databaseService)
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
        //public bool ProgressBarCancelFlag
        //{
        //    get
        //    {
        //        return _progressBarCancelFlag;
        //    }
        //    set
        //    {
        //        _progressBarCancelFlag = value;
        //    }
        //}
        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldDBService)
        {
            ControllerModel controllerInfo = new ControllerModel();
            try
            {
                //ControllerConfigBase configBase = new ControllerConfigBase();
                
                //8021,8036默认为8
                //8000,8001,8003默认为7
                //根据当前读到的版本需要进行判断                                
                //在版本6时，在每个回路表中增加了“特性”字段，需要更新特性字段的值
       
                //#通用组态更新“器件类型A”，“器件类型C”
                //带“点动”字样的为大数，需要-64
                //带“自动”字样的不需要改变

                //#混合组态更新“器件类型A",“器件类型B",“器件类型C"
                //更新逻辑同上

                //controllerInfo.ID = 
                controllerInfo.Name = "OldVersion";
                controllerInfo.Project = null;                
                controllerInfo.Type = ControllerType.NT8001;
                controllerInfo.PrimaryFlag = false;
                controllerInfo.LoopAddressLength = 2;
                //select deviceCode from deviceInfo8001 a left join controllerboard b 
                //where  a.devicecode =b.devicecode and a.controllerID=b.controllerID
                //a.deviceInfo.cmb=
                bool blnPostponingSetDeviceAddressLength = false;//器件长度延后设置标置 

                if (oldDBService.Version < 5)
                {                     
                    //在版本5之前（not includ），8001器件长度固定为7     
                    controllerInfo.DeviceAddressLength = 7;                    
                }
                else if (oldDBService.Version == 5)
                {
                    //在版本5时，在数据表中增加了“器件长度”字段，可以根据"器件长度”取得当前的器件长度，默认值为7
                    //由于 “器件长度”可通过后续的器件编码来判断，不需重读数据文件，延后设置
                    blnPostponingSetDeviceAddressLength = true;

                }
                else if (oldDBService.Version == 6)
                {
                    blnPostponingSetDeviceAddressLength = true;                    
                }
 
                List<LoopModel> lstLoopInfo = GetLoopInfoFromOldVersionSoftwareDataFile(oldDBService);                
                StringBuilder sbQuerySQL = new StringBuilder();
                //set sdpkey=xianggh,xianggh= (Round((xianggh/756)+.4999999)-1),panhao=IIF(Round(((xianggh/63))+.4999999)>12,Round(((xianggh/63))+.4999999)-12,Round(((xianggh/63))+.4999999)),jianhao=IIF((xianggh Mod 63)=0,63,xianggh Mod 63)
                //(1)网络手动盘            

                
                Dictionary<string, string> dictDeviceMappingManualControlBoard = new Dictionary<string, string>();//存储器件编码与手控盘编号的对应关系，将此关系存储在回路信息中
                List<ManualControlBoard> lstBoard=oldDBService.GetManualControlBoard();
                foreach (var r in lstBoard)
                {
                    dictDeviceMappingManualControlBoard.Add(r.DeviceCode.ToString(), r.Code.ToString());
                    ManualControlBoard board = r;
                    board.Controller = controllerInfo;
                    controllerInfo.ControlBoard.Add(board);
                }
                
                //(2)回路及器件
                foreach  (var l in lstLoopInfo)//回路信息
                {
                    LoopModel loop = l;
                    
                    oldDBService.GetDevicesInLoop(ref loop, dictDeviceMappingManualControlBoard); //为loop赋予“器件信息”
                    loop.Controller = controllerInfo;
                    
                    if (blnPostponingSetDeviceAddressLength)//设置控制器的“地址长度”
                    {
                        controllerInfo.DeviceAddressLength = oldDBService.DeviceAddressLength;
                        blnPostponingSetDeviceAddressLength = false;
                    }
                    controllerInfo.Loops.Add(loop);
                }

                //(3)标准组态
                
                List<LinkageConfigStandard> lstStandard=oldDBService.GetStandardLinkageConfig();
                foreach (var l in lstStandard)
                {
                    LinkageConfigStandard standardConfig = l;
                    standardConfig.Controller = controllerInfo;
                    controllerInfo.StandardConfig.Add(standardConfig);
                }


                //(4)混合组态
                
                List<LinkageConfigMixed> lstMixedConfig = oldDBService.GetMixedLinkageConfig();
                foreach (var l in lstMixedConfig)
                {
                    LinkageConfigMixed mixedConfig = l;
                    mixedConfig.Controller = controllerInfo;
                    controllerInfo.MixedConfig.Add(mixedConfig);
                }

                //(5)通用组态
                
                List<LinkageConfigGeneral> lstGeneral = oldDBService.GetGeneralLinkageConfig();
                foreach (var l in lstGeneral)
                {
                    LinkageConfigGeneral generalConfig = l;
                    generalConfig.Controller = controllerInfo;
                    controllerInfo.GeneralConfig.Add(generalConfig);
                }

            }
            catch
            {


            }
            return controllerInfo;
        }

        public Model.ControllerType GetControllerType()
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceID()
        { 
            //更改于2017-06-23 采用属性值记录
            //var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers  where r.Type==ControllerType.NT8001 select r;
            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8001;
            //foreach (var c in controllers)
            //{
            //    foreach (var l in c.Loops)
            //    {
            //        List<DeviceInfo8001> lstDeviceInfo8001=l.GetDevices<DeviceInfo8001>();
            //        if(lstDeviceInfo8001.Count>0)
            //        {
            //            int currentLoopMaxDeviceID=lstDeviceInfo8001.Max(device=>device.ID);
            //            if(currentLoopMaxDeviceID>maxDeviceID)
            //            {
            //                maxDeviceID = currentLoopMaxDeviceID;
            //            }                    
            //        }                        
            //    }
            //}
            return maxDeviceID;
        }

        //public bool AddControllerToProject(ControllerModel controller)
        //{
        //    try
        //    {
        //        SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;

        //        if (project.Controllers.Count == 0)//如果还未设置主控制器，则默认第一个控制器为主控制器
        //        {
        //            controller.PrimaryFlag = true;
        //        }
        //        int maxControllerID = GetMaxControllerID();
        //        controller.ID = maxControllerID + 1;

        //        project.Controllers.Add(controller);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}


        //public bool DeleteControllerBySpecifiedControllerID(int controllerID)
        //{
        //    try
        //    {
        //        ProjectModel project = ProjectManager.GetInstance.Project;
        //        var result = from c in project.Controllers where c.ID == controllerID select c;
        //        ControllerModel controller = result.FirstOrDefault();
        //        if (controller != null)
        //        {
        //            project.Controllers.Remove(controller);                    
        //        }

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        protected override List<short?> GetAllBuildingNoWithController(ControllerModel controller)
        {
            List<short?> lstBuildingNo = new List<short?>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8001>())
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
                foreach (var dev in l.GetDevices<DeviceInfo8001>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8001 config = new ControllerConfig8001();
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
           // var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8001 select r;
            List<DeviceInfo8001> lstDeviceInfo = new List<DeviceInfo8001>();
           // foreach (var c in controllers)
           // {
                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8001>())
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
                simulatorDevice.ControllerName = controller.Name;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
                simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            return lstDeviceSimulator;
        }



        public List<DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode,ControllerModel controller,List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine)
        {

            var remainControllers = SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers.Where((c) => c.ID != controller.ID);
            List<DeviceInfo8001> lstDeviceInfo = new List<DeviceInfo8001>();
        //    foreach (var c in controllers)
          //  {

                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8001>())
                    {
                        lstDeviceInfo.Add(d);
                    }
                }
          //  }
                List<DeviceInfo8001> lstCurrentMachineDeviceInfo = new List<DeviceInfo8001>(); //本机器件
                List<DeviceInfoForSimulator> lstOtherMachineDeviceInfo = new List<DeviceInfoForSimulator>();//它机器件

                List<string> lstOtherMachineDeviceCode = new List<string>(); //它机器件编码

                List<string> lstUnfoundMachineDeviceCode = new List<string>(); //未匹配至控制器的器件编码

                foreach (var deviceCode in lstDeviceCode)  //遍历本机器件
                {
                    DeviceInfo8001 device=lstDeviceInfo.Where((d) => d.Code == deviceCode).FirstOrDefault();
                    if (device != null)
                    {
                        lstCurrentMachineDeviceInfo.Add(device);                        
                    }
                    else
                    {
                        lstOtherMachineDeviceCode.Add(deviceCode);
                    }
                }
                //---取它机器件---
     
                foreach (var deviceCode in lstOtherMachineDeviceCode)  //用本机未找到的器件，在其它控制器的器件信息中寻找
                {
                    DeviceInfoForSimulator  device = lstAllDevicesOfOtherMachine.Where((d) => d.Code == deviceCode).FirstOrDefault();
                    if (device != null)
                    {
                        lstOtherMachineDeviceInfo.Add(device);
                    }
                    else
                    {
                        lstUnfoundMachineDeviceCode.Add(deviceCode);
                    }
                }


            
            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstCurrentMachineDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                simulatorDevice.Scope = ControllerScope.LocalMachine;
                simulatorDevice.ControllerName = d.Loop.Controller.Name;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
                simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            foreach (var d in lstOtherMachineDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                simulatorDevice.Scope = ControllerScope.OtherMachine;
                simulatorDevice.ControllerName = d.Loop.Controller.Name;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
                simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            foreach (var d in lstUnfoundMachineDeviceCode)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.ControllerName = "未知";
                simulatorDevice.Scope = ControllerScope.Unfound;
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d;                
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            return lstDeviceSimulator;
        }


        protected override bool GenerateExcelTemplateLoopSheet(List<string> sheetNames, IControllerConfig config,ExcelTemplateCustomizedInfo summaryInfo, ref IExcelService excelService)
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
                        #region Obsolete
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 0, "编码", CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 1, "器件类型", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 2, "特性", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 3, "屏蔽", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 4, "灵敏度", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 5, "输出组1", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 6, "输出组2", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 7, "输出组3", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 8, "延时", Excel2007Service.CellStyleType.TableHead);

                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 9, "广播分区", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 10, "楼号", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 11, "区号", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 12, "层号", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 13, "房间号", Excel2007Service.CellStyleType.TableHead);
                        //excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + 1, 14, "安装地点", Excel2007Service.CellStyleType.TableHead);
                        #endregion
                        //回路标题行合并
                        MergeCellRange mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.FirstColumnIndex = 0;
                        mergeCellRange.LastColumnIndex = deviceColumnDefinitionArray.Length-1;
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
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 10, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 11, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 12, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 13, null, CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 14, null, CellStyleType.Data);
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
        protected override bool GenerateExcelTemplateStandardSheet(List<string> sheetNames, int currentIndex,int loopSheetAmount,int maxLinkageAmount, ref IExcelService excelService)
        {
            try
            {
                #region 标准组态表头
                List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                lstMergeCellRange.Clear();
                // 加的1页，为“摘要页”                
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "输出组号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "联动模块1", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "联动模块2", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "联动模块3", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "联动模块4", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 5, "联动模块5", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 6, "联动模块6", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 7, "联动模块7", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 8, "联动模块8", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 9, "动作常数", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 10, "联动组1", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 11, "联动组2", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 12, "联动组3", CellStyleType.TableHead);
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 12;
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
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 9, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 10, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 11, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 12, null, CellStyleType.Data);
                }
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 9;
                mergeCellRange.LastColumnIndex = 9;
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
            try
            { 
            #region 混合组态表头
                    List<MergeCellRange> lstMergeCellRange = new List<MergeCellRange>();
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "动作常数", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "动作类型", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "A分类", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "A楼号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 5, "A区号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 6, "A层号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 7, "A路号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 8, "A编号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 9, "A类型", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 10, "B分类", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 11, "B楼号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 12, "B区号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 13, "B层号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 14, "B路号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 15, "B编号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 16, "B类型", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 17, "C分类", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 18, "C楼号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 19, "C区号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 20, "C层号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 21, "C机号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 22, "C回路号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 23, "C编号", CellStyleType.TableHead);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 24, "C类型", CellStyleType.TableHead);
                    MergeCellRange mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 24;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"混合组态页签"合并单元格
                    //int maxLinkageAmount = config.GetMaxAmountForMixedLinkageConfig();
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
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 9, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 10, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 11, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 12, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 13, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 14, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 15, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 16, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 17, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 18, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 19, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 20, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 21, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 22, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 23, null, CellStyleType.Data);
                        excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 24, null, CellStyleType.Data);
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
                    mergeCellRange.FirstColumnIndex = 9;
                    mergeCellRange.LastColumnIndex = 9;
                    excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 10;
                    mergeCellRange.LastColumnIndex = 10;
                    excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 16;
                    mergeCellRange.LastColumnIndex = 16;
                    excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 17;
                    mergeCellRange.LastColumnIndex = 17;
                    excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 24;
                    mergeCellRange.LastColumnIndex = 24;
                    excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                
                #endregion
             }
            catch(Exception ex )
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
                List<MergeCellRange> lstMergeCellRange=new List<MergeCellRange>();                
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "动作常数", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "A楼号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "A区号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "A层号1", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 5, "A层号2", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 6, "类型A", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 7, "C分类", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 8, "C楼号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 9, "C区号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 10, "C层号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 11, "C机号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 12, "C回路号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 13, "C编号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 14, "C类型", CellStyleType.TableHead);
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 14;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"通用组态页签"合并单元格                    
                
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
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 9, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 10, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 11, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 12, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 13, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 14, null, CellStyleType.Data);
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
                mergeCellRange.FirstColumnIndex = 6;
                mergeCellRange.LastColumnIndex = 6;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeWithAnyAlarm.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 7;
                mergeCellRange.LastColumnIndex = 7;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeAll.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 2;
                mergeCellRange.LastRowIndex = maxLinkageAmount + 2;
                mergeCellRange.FirstColumnIndex = 14;
                mergeCellRange.LastColumnIndex = 14;
                excelService.SetSheetValidationForListConstraint(sheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeWithoutFireDevice.ToString(), mergeCellRange);
            }
            #endregion
            
        catch(Exception ex)
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

                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 0, 0, sheetNames[loopSheetAmount + currentIndex], CellStyleType.SubCaption);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 1, "板卡号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 2, "手盘号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 3, "手键号", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 4, "地编号", CellStyleType.TableHead);
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 4;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.SetMergeCells(sheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"网络手动盘页签"合并单元格

                for (int i = 2; i < maxAmount + 3; i++)
                {
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 0, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 1, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 2, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 3, null, CellStyleType.Data);
                    excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], i, 4, null, CellStyleType.Data);
                }

                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public bool DownloadDefaultEXCELTemplate(string strFilePath,IFileService fileService,ExcelTemplateCustomizedInfo customizedInfo)
        {
            try
            {
                DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, ControllerType.NT8001);                
                return true;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return false;
            }            
        }

        //需注销
        /// <summary>
        /// 通过EXCEL模板的设定值取得Controller对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        private ControllerModel GetControllerViaExcelTemplate(string name,string value,ControllerModel controller)
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
        /// <summary>
        /// 读取EXCEL模板中的数据
        /// </summary>
        /// <param name="strFilePath">打开文件路径</param>
        /// <param name="fileService">文件操作对象</param>
        /// <param name="targetController">目标控制器信息</param>
        /// <param name="strErrorMessage">错误信息</param>
        /// <returns></returns>
        public void ReadEXCELTemplate(string strFilePath, IFileService fileService,ControllerModel targetController)
        {
            //ReadExcelLoopArgumentForIn inArgs = new ReadExcelLoopArgumentForIn();            
            //inArgs.FileService=fileService;
            //inArgs.FilePath=strFilePath;
            //inArgs.Controller=targetController;
            //DoWorkEventArgs args = new DoWorkEventArgs(inArgs);                                    
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += ReadingEXCELDoWork;
            //worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(UpdateProgress);
            //worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(ReadingEXCELCompleteWork); 
            //worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true;
            //worker.RunWorkerAsync(inArgs);    
            base.ReadEXCELTemplate(strFilePath, fileService, targetController);
        }
        
        #region 重构EXCEL读取 注销
        /// <summary>
        /// 读取EXEL,根据工作作名称，取得回路及器件信息
        /// </summary>
        /// <param name="excelService"></param>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="maxDevcieAmount"></param>
        /// <param name="deviceCodeLength"></param>
        /// <returns></returns>
        //private List<LoopModel> GetLoopData(IExcelService excelService,string filePath, string sheetName,int maxDevcieAmount,ControllerModel controller,out bool sheetExistFlag,out string loopDetailErrorInfo,out int elapsedTime)
        //{
        //    int[] loopRange = ParseLoopSheetName(sheetName);//取得回路编号范围
        //    Dictionary<int, int> rowDefinition = new Dictionary<int, int>();
        //    int extraLines=2; //除数据外的其它行数
        //    //for (int i = loopRange[0]; i <= loopRange[loopRange.Length-1]; i++)
        //    //{
        //    //    if (i == 1)
        //    //    {
        //    //        rowDefinition.Add(1, i * maxDevcieAmount + i  * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
        //    //    }
        //    //    else
        //    //    {
        //    //        rowDefinition.Add(i * maxDevcieAmount + (i ) * extraLines - 1, (i ) * maxDevcieAmount + (i) * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
        //    //    }
        //    //}
        //    for (int i = 0; i < (loopRange[1] - loopRange[0] + 1); i++)
        //    {
        //        if (i == 0)
        //        {
        //            rowDefinition.Add(1, (i + 1) * maxDevcieAmount + (i + 1) * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
        //        }
        //        else
        //        {
        //            rowDefinition.Add(i * maxDevcieAmount + (i + 1) * extraLines - 1, (i + 1) * maxDevcieAmount + (i + 1) * extraLines - 1);//控制器信息起始行定义 4为表头信息,                    
        //        }
        //    }
        //    DataTable dt = new DataTable();
        //    int intElapsedTime;
        //    dt = excelService.OpenExcel(filePath, sheetName, rowDefinition, out sheetExistFlag, out intElapsedTime);
        //    elapsedTime = intElapsedTime;
        //    return ConvertToLoopModelFromDataTable(dt, controller,out loopDetailErrorInfo);
        //    //如果未找到指定sheetName的EXCEL数据，需要返回“页签错误”的信息            
        //}
        /// <summary>
        /// 解析回路工作表的名称，取得回路号范围 
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        //private int[] ParseLoopSheetName(string sheetName)
        //{
        //    int lineCharIndex = sheetName.IndexOf('-');
        //    int leftParenthesis = sheetName.IndexOf('(');
        //    int rightParenthesis = sheetName.IndexOf(')');

        //    string strLoopStartIndex = sheetName.Substring(leftParenthesis + 1, lineCharIndex - leftParenthesis - 1);
        //    string strLoopEndIndex = sheetName.Substring(lineCharIndex + 1, rightParenthesis - lineCharIndex - 1);
        //    int loopStartIndex = 1;
        //    int loopEndIndex = 1;
        //    if (strLoopStartIndex != "")
        //    {
        //        loopStartIndex = Convert.ToInt32(strLoopStartIndex);
        //    }
        //    if (strLoopEndIndex != "")
        //    {
        //        loopEndIndex = Convert.ToInt32(strLoopEndIndex);
        //    }
        //    //loopStartIndex = loopStartIndex == 0 ? 1 : loopStartIndex;
        //    //loopEndIndex = loopEndIndex == 0 ? 1 : loopEndIndex;
        //    int[] loopIndex = new int[2];
        //    //for(int i=loopStartIndex-1;i<=loopEndIndex-1 ;i++)
        //    //{
        //    //    loopIndex[i] = i+1;
        //    //}         
        //    loopIndex[0] = loopStartIndex;
        //    loopIndex[1] = loopEndIndex;
        //    return loopIndex;
        //}

        //private DataTable GetOtherSettingData(IExcelService excelService,string filePath, string sheetName,int maxRowIndex,out bool sheetExistFlag,out int elapsedTime)
        //{

        //    Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();                      
        //    summarySheetRowDefinition.Add(1,maxRowIndex + 2 - 1); //2为标题+表头,由于从0开始记数，需要减1        
        //    DataTable dt = new DataTable();
        //    dt = excelService.OpenExcel(filePath, sheetName, summarySheetRowDefinition,out sheetExistFlag,out  elapsedTime);
        //    return dt;
        //}


        //private void ReadingEXCELDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    BackgroundWorker bw = (BackgroundWorker)sender;
        //    List<string> lstLoopSheetName;
        //    List<string> lstOtherSheetName;
        //    EXCELVersion excelVersion;
        //    IExcelService excelService;
        //    int loopCount = 0;
        //    if (!ReadingExcelSummarySheet(ref bw, out lstLoopSheetName, out lstOtherSheetName, out excelVersion, out excelService))
        //    {
        //        //if (ProgressBarCancelFlag)
        //        //{
        //        bw.CancelAsync();
        //        //}
        //        if (bw.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            ReadingExcelCancelationEvent(null, null);//触发取消事件
        //            return;
        //        }
        //    }
        //    int totoalSheets = lstLoopSheetName.Count + lstOtherSheetName.Count;//总共需要读取的Sheet页
        //    foreach (var sheetName in lstLoopSheetName)
        //    {
        //        if (excelVersion == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
        //        {
        //            new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
        //        }
        //        if (ProgressBarCancelFlag)
        //        {
        //            bw.CancelAsync();
        //        }
        //        if (bw.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            ReadingExcelCancelationEvent(null, null);//触发取消事件
        //            return;
        //        }

        //        loopCount++;
        //        Console.WriteLine("开始读取" + sheetName);
        //        lstLoops = GetLoopData(excelService, args.FilePath, sheetName, maxDeviceAmount, controller, out blnSheetExistFlag, out loopDetailErrorInfo, out elapsedSingleSheetTime);
        //        cumulativeTime += elapsedSingleSheetTime;
        //        averageTime = cumulativeTime / loopCount;

        //        //totalTime = elapsedSingleSheetTime * args.LoopSheetNames.Count;
        //        totalTime = averageTime * totoalSheets;
        //        percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
        //        bw.ReportProgress(percentageValue);
        //        if (lstLoops != null)
        //        {
        //            if (blnSheetExistFlag)
        //            {
        //                foreach (var loop in lstLoops)
        //                {
        //                    deviceService.TheLoop = loop;
        //                    if (deviceService.IsExistSameDeviceCode())
        //                    {
        //                        loopDetailErrorInfo += ";" + loop.Code + "回路存在重码";
        //                    }
        //                    else
        //                    {
        //                        controller.Loops.Add(loop);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                loopDetailErrorInfo += ";《" + sheetName + "》工作表名称配置不正确";
        //            }
        //        }
        //        if (loopDetailErrorInfo != "") //有错误信息，则直接返回调用
        //        {
        //            strStatusInfo += ";" + loopDetailErrorInfo;
        //            //  strErrorMessage = strStatusInfo;
        //            result.Status = strStatusInfo;
        //            result.Controller = controller;
        //            e.Result = result;
        //            e.Cancel = true; //有错误信息，取消执行
        //            ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
        //            return;
        //        }
        //        Console.WriteLine("结束读取" + sheetName);
        //    }

        //    foreach (var sheetName in lstOtherSheetName)
        //    {
        //        if (version == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
        //        {
        //            new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
        //        }
        //        if (ProgressBarCancelFlag)
        //        {
        //            bw.CancelAsync();
        //        }
        //        if (bw.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            ReadingExcelCancelationEvent(null, null);//触发取消事件
        //            return;
        //        }
        //        loopCount++;
        //        config.GetMaxAmountForStandardLinkageConfig();
        //        switch (sheetName)
        //        {
        //            case "标准组态":
        //                {
        //                    DataTable dtStandard = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForStandardLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
        //                    if (blnSheetExistFlag)
        //                    {
        //                        List<LinkageConfigStandard> lstStandardConfig = ConvertToStandardLinkageModelFromDataTable(dtStandard);
        //                        LinkageConfigStandardService standardConfigService = new LinkageConfigStandardService(controller);
        //                        if (standardConfigService.IsExistSameCode(lstStandardConfig))
        //                        {
        //                            strStatusInfo += ";" + sheetName + "存在重码";
        //                        }
        //                        else
        //                        {
        //                            foreach (var r in lstStandardConfig)
        //                            {
        //                                controller.StandardConfig.Add(r);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
        //                    }
        //                }
        //                break;
        //            case "混合组态":
        //                {
        //                    DataTable dtMixed = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForMixedLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
        //                    if (blnSheetExistFlag)
        //                    {
        //                        List<LinkageConfigMixed> lstMixedConfig = ConvertToMixedLinkageModelFromDataTable(dtMixed);
        //                        LinkageConfigMixedService mixedConfigService = new LinkageConfigMixedService(controller);
        //                        if (mixedConfigService.IsExistSameCode(lstMixedConfig))
        //                        {
        //                            strStatusInfo += ";" + sheetName + "存在重码";
        //                        }
        //                        else
        //                        {
        //                            foreach (var r in lstMixedConfig)
        //                            {
        //                                controller.MixedConfig.Add(r);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
        //                    }
        //                }
        //                break;
        //            case "通用组态":
        //                {
        //                    DataTable dtGeneral = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
        //                    if (blnSheetExistFlag)
        //                    {

        //                        List<LinkageConfigGeneral> lstGeneralConfig = ConvertToGeneralLinkageModelFromDataTable(dtGeneral);
        //                        LinkageConfigGeneralService generalConfigService = new LinkageConfigGeneralService(controller);
        //                        if (generalConfigService.IsExistSameCode(lstGeneralConfig))
        //                        {
        //                            strStatusInfo += ";" + sheetName + "存在重码";
        //                        }
        //                        else
        //                        {
        //                            foreach (var r in lstGeneralConfig)
        //                            {
        //                                controller.GeneralConfig.Add(r);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
        //                    }
        //                }
        //                break;
        //            case "网络手动盘":
        //                {
        //                    DataTable dtMCB = GetOtherSettingData(excelService, args.FilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(), out blnSheetExistFlag, out elapsedSingleSheetTime);
        //                    if (blnSheetExistFlag)
        //                    {
        //                        List<ManualControlBoard> lstMCB = ConvertToManualControlBoardModelFromDataTable(dtMCB);
        //                        ManualControlBoardService mcbService = new ManualControlBoardService(controller);
        //                        if (mcbService.IsExistSameCode(lstMCB))
        //                        {
        //                            strStatusInfo += ";" + sheetName + "存在重码";
        //                        }
        //                        else
        //                        {
        //                            foreach (var r in lstMCB)
        //                            {
        //                                controller.ControlBoard.Add(r);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
        //                    }
        //                }
        //                break;
        //        }
        //        cumulativeTime += elapsedSingleSheetTime;
        //        averageTime = cumulativeTime / loopCount;
        //        totalTime = averageTime * totoalSheets;
        //        percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
        //        bw.ReportProgress(percentageValue);
        //    }
        //    result.Controller = controller;
        //    result.Status = strStatusInfo;
        //    e.Result = result;

        //}
        //void UpdateProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{

        //    int progress = e.ProgressPercentage;
        //    UpdateProgressBarEvent(progress);

        //}
        //void ReadingEXCELCompleteWork(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {

        //        Console.WriteLine("I have an error!");

        //    }
        //    else if (e.Cancelled)
        //    {



        //    }
        //    else
        //    {
        //        ControllerAndStatus reuslt = (ControllerAndStatus)e.Result;
        //        ReadingExcelCompletedEvent(reuslt.Controller, reuslt.Status);
        //    }
        //}



        //private bool ReadingExcelSummarySheet(ref BackgroundWorker bw, out List<string> loopSheetNames, out List<string> otherSettingSheetNames, out EXCELVersion version, out IExcelService excelService)
        //{
        //    const int DELAY_VALUE = 5000;//线程休息时间5秒
        //    float totalTime = 100;  //总EXCEL读取时间
        //    float cumulativeTime = 0;  //当前累积时间
        //    int percentageValue = Convert.ToInt32(cumulativeTime / totalTime) * 100; //进度百分比
        //    ReadExcelLoopArgumentForIn args = (ReadExcelLoopArgumentForIn)e.Argument;
        //    DeviceService8001 deviceService = new DeviceService8001();
        //    List<LoopModel> lstLoops = null;//从EXCEL文件中取得数据
        //    bool blnSheetExistFlag;
        //    string loopDetailErrorInfo; //回路错误信息
        //    int elapsedSingleSheetTime; //执行时间
        //    string strStatusInfo = ""; //状态信息
        //    ControllerAndStatus result = new ControllerAndStatus();
        //    cumulativeTime = 0;

        //    float averageTime = 0; //平均执行时间,避免进度忽大忽小
        //    version = GetExcelVersion(args.FilePath);
        //    excelService = ExcelServiceManager.GetExcelService(version, args.FilePath, args.FileService);
        //    ControllerConfig8001 config = new ControllerConfig8001();

        //    ControllerNodeModel[] nodes = config.GetNodes();
        //    ColumnConfigInfo[] deviceColumnDefinitions = config.GetDeviceColumns();
        //    int maxDeviceAmount = config.GetMaxDeviceAmountValue(); //回路内器件信息最大数量

        //    Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的名称配置信息
        //    Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate(args.Controller.DeviceAddressLength); ////取得“摘要信息页”控制器设置的值的有效性
        //    Dictionary<int, string> dictNameOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetNameOfLoopSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”回路设置的名称配置信息
        //    Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”回路设置的值的有效性
        //    Dictionary<int, string> dictNameOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetNameOfOtherSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”其它设置的名称配置信息
        //    Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”其它设置的值的有效性

        //    bool blnIsError = false;     //错误标志    
        //    int controllerSettingStartRow = 4;
        //    int loopSettingStartRow = controllerSettingStartRow + dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + 3;//3为“控制器表头行+空行+回路标题"
        //    int otherSettingStartRow = loopSettingStartRow + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + 3;
        //    //定义“摘要信息”页的数据行定义信息
        //    Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();
        //    summarySheetRowDefinition.Add(controllerSettingStartRow, 9);//控制器信息起始行定义 4为表头信息,
        //    summarySheetRowDefinition.Add(loopSettingStartRow, 15);//回路信息起始行定义
        //    summarySheetRowDefinition.Add(otherSettingStartRow, 19);//其它信息起始行定义               

        //    bw.ReportProgress(percentageValue); //进度条初始化

        //    if (version == EXCELVersion.EXCEL2007) //由于2007文件读取很慢，增加空闲时间，以有机会操作UI界面（点击‘取消按钮')
        //    {
        //        new System.Threading.ManualResetEvent(false).WaitOne(DELAY_VALUE);
        //    }
        //    if (ProgressBarCancelFlag)
        //    {
        //        return false;
        //    }

        //    DataTable dt = excelService.OpenExcel(args.FilePath, config.SummarySheetNameForExcelTemplate, summarySheetRowDefinition, out blnSheetExistFlag, out elapsedSingleSheetTime);
        //    #region 计算时间 更新进度百分比
        //    cumulativeTime += elapsedSingleSheetTime;
        //    averageTime = cumulativeTime;
        //    //totalTime = elapsedSingleSheetTime * args.LoopSheetNames.Count;
        //    totalTime = averageTime * 74;
        //    percentageValue = Convert.ToInt32((cumulativeTime == 0 ? 1 : cumulativeTime) / (totalTime == 0 ? 1 : totalTime) * 100);
        //    bw.ReportProgress(percentageValue);
        //    #endregion
        //    ControllerModel controller = new ControllerModel(); //存储"控制器配置"信息
        //    int loopAmount = 1;//回路数量
        //    int loopGroup = 1;//回路分组
        //    string loopSheetNamePrefix = "";//回路页签名称前缀                
        //    for (int i = 0; i < nodes.Length; i++)
        //    {
        //        switch (nodes[i].Type)
        //        {
        //            case ControllerNodeType.Loop:
        //                loopSheetNamePrefix = nodes[i].Name;
        //                break;
        //        }
        //    }
        //    List<string> lstSheetNames = new List<string>();// 除“摘要信息”之外的页签名称
        //    for (int i = 0; i < dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count; i++)
        //    {
        //        if (!(dt.Rows[i][0].ToString() == dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[controllerSettingStartRow + 1 + i])) //+1为跨过标题行
        //        {
        //            strStatusInfo = "摘要信息-->控制器设置-->模板名称不正确";
        //            blnIsError = true;
        //        }
        //        else //名称一致，验证值的有效性
        //        {
        //            RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate[controllerSettingStartRow + 1 + i];
        //            Regex exminator = new Regex(verifingMessage.Rule);
        //            if (!exminator.IsMatch(dt.Rows[i][1].ToString()))
        //            {
        //                strStatusInfo += ";" + verifingMessage.ErrorMessage;
        //                blnIsError = true;
        //                continue;
        //            }
        //            else
        //            {
        //                controller = GetControllerViaExcelTemplate(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), controller);
        //            }
        //        }
        //    }
        //    if (!blnIsError)
        //    {
        //        if (controller.DeviceAddressLength != args.Controller.DeviceAddressLength)
        //        {
        //            strStatusInfo += ";模板器件长度(" + controller.DeviceAddressLength + ")与当前控制器器件长度(" + args.Controller.DeviceAddressLength + ")不一致";
        //            blnIsError = true;
        //        }
        //        if (controller.MachineNumber != args.Controller.MachineNumber)
        //        {
        //            strStatusInfo += ";模板控制器机号(" + controller.MachineNumber + ")与当前控制器机号(" + args.Controller.MachineNumber + ")不一致";
        //            blnIsError = true;
        //        }
        //        if (controller.Type != args.Controller.Type)
        //        {
        //            strStatusInfo += ";模板控制器类型(" + controller.Type.ToString() + ")与当前控制器类型(" + args.Controller.Type.ToString() + ")不一致";
        //            blnIsError = true;
        //        }
        //    }
        //    if (!blnIsError)//控制器配置信息正确
        //    {
        //        //controller
        //        //验证回路信息
        //        for (int i = 0; i < dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count; i++)
        //        {
        //            if (!(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == dictNameOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow + 1 + i]))//+1为跨过标题行
        //            {
        //                strStatusInfo = "摘要信息-->回路设置-->模板名称不正确";
        //                blnIsError = true;
        //            }
        //            else //名称一致，验证值的有效性
        //            {
        //                RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow + 1 + i];
        //                //verifingMessage.Rule
        //                //verifingMessa、ge.ErrorMessage                     
        //                Regex exminator = new Regex(verifingMessage.Rule);
        //                if (!exminator.IsMatch(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString()))
        //                {
        //                    strStatusInfo += ";" + verifingMessage.ErrorMessage;
        //                    blnIsError = true;
        //                    continue;
        //                }
        //                else //验证结果正确，存储读入的数据
        //                {
        //                    if (dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == "回路数量")
        //                    {
        //                        loopAmount = Convert.ToInt32(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString());
        //                    }
        //                    else if (dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == "回路分组")
        //                    {
        //                        loopGroup = Convert.ToInt32(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result.Status = strStatusInfo;
        //        result.Controller = controller;
        //        ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
        //        return false;
        //    }
        //    if (blnIsError) //存在错误，直接返回
        //    {
        //        result.Status = strStatusInfo;
        //        result.Controller = controller;
        //        ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
        //        return false;
        //    }
        //    List<string> lstOtherSheetName = new List<string>();//除“回路”外的其它工作表名称
        //    string otherSettingValue = "";//以“分号”分隔的页签名称
        //    if (!blnIsError)//取得除回路外的工作表名称
        //    {
        //        for (int i = 0; i < dictNameOfOtherSettingInSummaryInfoOfExcelTemplate.Count; i++)
        //        {
        //            if (!(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == dictNameOfOtherSettingInSummaryInfoOfExcelTemplate[otherSettingStartRow + 1 + i]))//+1为跨过标题行
        //            {
        //                strStatusInfo = "摘要信息-->其它设置-->名称不正确";
        //                blnIsError = true;
        //            }
        //            else //名称一致，验证值的有效性
        //            {
        //                RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate[otherSettingStartRow + 1 + i];
        //                Regex exminator = new Regex(verifingMessage.Rule);
        //                if (!exminator.IsMatch(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString()))
        //                {
        //                    strStatusInfo += ";摘要信息-->其它设置-->" + verifingMessage.ErrorMessage;
        //                    blnIsError = true;
        //                    continue;
        //                }
        //                else
        //                {
        //                    otherSettingValue = dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + i][1].ToString();
        //                }
        //            }
        //        }
        //    }
        //    if (blnIsError)
        //    {
        //        result.Status = strStatusInfo;
        //        result.Controller = controller;
        //        ReadingExcelCancelationEvent(controller, strStatusInfo);//触发取消事件
        //        return false;
        //    }

        //    //分组信息>回路数量时，重置回路分组信息
        //    if (loopGroup > loopAmount)
        //    {
        //        loopGroup = loopAmount;
        //    }
        //    List<string> lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopAmount, loopGroup);//根据“摘要信息”取得所有回路数据的工作表名称
        //    string[] otherSheetNames = otherSettingValue.Split(';');
        //    for (int i = 0; i < otherSheetNames.Length; i++)
        //    {
        //        lstOtherSheetName.Add(otherSheetNames[i]);
        //    }
        //    loopSheetNames = lstLoopSheetName;
        //    otherSettingSheetNames = lstOtherSheetName;
        //} 


        #endregion
        /// <summary>
        /// 将读出的回路及器件信息转换为LoopModel类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deviceCodeLength"></param>
        /// <returns></returns>
        protected override List<LoopModel> ConvertToLoopModelFromDataTable(DataTable dt,ControllerModel controller,out string loopDetailErrorInfo)
        {            
            loopDetailErrorInfo = "";//Initialize
            //取得当前控制器最大器件ID
            int maxDeviceID = this.GetMaxDeviceID();
            ControllerConfig8001 config =new ControllerConfig8001();
            //暂停对具体信息进行验证
            //Dictionary<string, RuleAndErrorMessage> dictDeviceDataRule = config.GetDeviceInfoRegularExpression(controller.DeviceAddressLength);
            List<LoopModel> lstLoops = new List<LoopModel>();

            
            string loopCode = "";
            LoopModel loop=null;            
            int[] loopIndexRange=base.ParseLoopSheetName(dt.TableName);//取得工作表内回路的起始编号
            int loopsAmount=loopIndexRange[1] - loopIndexRange[0] + 1;
            int[] loopIndex = new int[loopsAmount];
            for (int i = 0; i < loopsAmount; i++)
            {
                loopIndex[i] = loopIndexRange[0]+i;
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
                   string machineNumberOfDevice=loop.Code.Substring(0,controller.DeviceAddressLength-5);
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
               DeviceInfo8001 device = new DeviceInfo8001();
               maxDeviceID++;
               device.ID = maxDeviceID;
               device.Code = dt.Rows[i]["编码"].ToString();
               device.TypeCode = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["器件类型"].ToString());
               if (dt.Rows[i]["特性"].ToString() == "")
               {
                   device.Feature =  null;
               }
               else
               {
                   device.Feature = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["特性"].ToString()));
               }
               device.Disable = new Nullable<bool>(Convert.ToBoolean(dt.Rows[i]["屏蔽"].ToString()=="0"?false:true));
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
               device.LinkageGroup3 = dt.Rows[i]["输出组3"].ToString();
               if (dt.Rows[i]["延时"].ToString() == "")
               {
                   device.DelayValue = null;
               }
               else
               {
                   device.DelayValue = new Nullable<short>(Convert.ToInt16(dt.Rows[i]["延时"].ToString()));
               }               
               device.BroadcastZone = dt.Rows[i]["广播分区"].ToString();
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
               if (dt.Rows[i]["层号"].ToString()=="")
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
                   loop.SetDevice<DeviceInfo8001>(device);
                   loop.DeviceAmount++;
               }
            }
            //更新最大器件ID
            ProjectManager.GetInstance.MaxDeviceIDInController8001 = maxDeviceID;
            return lstLoops;            
        }


        //检查回路内的器件编码是否有重复
        protected override bool ExistSameDeviceCode(LoopModel loop)
        {
            DeviceService8001 deviceService = new DeviceService8001();
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
        /// <summary>
        /// 将读出的标准组态信息转换为LinkageConfigStandard类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected override List<LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(DataTable dt)
        {
            List<LinkageConfigStandard> lstStandardLinkage = new List<LinkageConfigStandard>();
            int maxID = ProjectManager.GetInstance.MaxIDForStandardLinkageConfig;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["输出组号"].ToString() == "")//无输出组号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                maxID++;
                LinkageConfigStandard lcs = new LinkageConfigStandard();
                lcs.ID = maxID;
                lcs.Code = dt.Rows[i]["输出组号"].ToString();
                lcs.DeviceNo1 = dt.Rows[i]["联动模块1"].ToString();
                lcs.DeviceNo2 = dt.Rows[i]["联动模块2"].ToString();
                lcs.DeviceNo3 = dt.Rows[i]["联动模块3"].ToString();
                lcs.DeviceNo4 = dt.Rows[i]["联动模块4"].ToString();
                lcs.DeviceNo5 = dt.Rows[i]["联动模块5"].ToString();
                lcs.DeviceNo6 = dt.Rows[i]["联动模块6"].ToString();
                lcs.DeviceNo7 = dt.Rows[i]["联动模块7"].ToString();
                lcs.DeviceNo8 = dt.Rows[i]["联动模块8"].ToString();
                lcs.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"].ToString().NullToZero());
                lcs.LinkageNo1 = dt.Rows[i]["联动组1"].ToString();
                lcs.LinkageNo2 = dt.Rows[i]["联动组2"].ToString();
                lcs.LinkageNo3 = dt.Rows[i]["联动组3"].ToString();
                lstStandardLinkage.Add(lcs);
            }
            ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = maxID;
            return lstStandardLinkage;
        }
        /// <summary>
        /// 将读出的混合组态信息转换为LinkageConfigMixed类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected override List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8001 config = new ControllerConfig8001();
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
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeA.GetType(),dt.Rows[i]["A分类"].ToString()), out lTypeA);
                lcm.TypeA = lTypeA;

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
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeB.GetType(),dt.Rows[i]["B分类"].ToString()), out lTypeB);
                lcm.TypeB = lTypeB;

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
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeC.GetType(),dt.Rows[i]["C分类"].ToString()), out lTypeC);
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
        /// <summary>
        /// 将读出的通用组态信息转换为LinkageConfigGeneral类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected override List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8001 config = new ControllerConfig8001();
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

                lcg.DeviceTypeCodeA = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["类型A"].ToString());
                LinkageType lTypeC = lcg.TypeC;
                Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(lTypeC.GetType(),dt.Rows[i]["C分类"].ToString()), out lTypeC);                

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
        /// <summary>
        /// 将读出的网络手动盘信息转换为ManualControlBoard类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected override List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dt)
        {
            //ControllerConfig8001 config = new ControllerConfig8001();
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
                mcb.BoardNo = Convert.ToInt32(dt.Rows[i]["板卡号"].ToString().NullToZero());
                mcb.SubBoardNo = Convert.ToInt32(dt.Rows[i]["手盘号"].ToString().NullToZero());
                mcb.KeyNo = Convert.ToInt32(dt.Rows[i]["手键号"].ToString().NullToZero());
                mcb.DeviceCode = dt.Rows[i]["地编号"].ToString().NullToZero();
                lstManualControlBoard.Add(mcb);
            }
            ProjectManager.GetInstance.MaxIDForManualControlBoard = maxID;
            return lstManualControlBoard;
        }
        //private void SetDeviceID
        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType)
        {
            return base.DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, controllerType);
        }

        public void SetStaticProgressBarCancelFlag(bool flag)
        {
            ControllerOperationBase.ProgressBarCancelFlag = flag;
        }


        //public event Action<int> UpdateProgressBarEvent;

        //public event Action<ControllerModel, string> ReadingExcelCompletedEvent;

        //public event Action<ControllerModel, string> ReadingExcelCancelationEvent;

        //public event Action<string> ReadingExcelErrorEvent;
    }
}
