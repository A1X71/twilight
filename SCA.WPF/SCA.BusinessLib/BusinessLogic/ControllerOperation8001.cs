using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
using SCA.Model.BussinessModel;
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
                //(1)网络手控盘            

                
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
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers  where r.Type==ControllerType.NT8001 select r;
            int maxDeviceID = 0;
            foreach (var c in controllers)
            {
                foreach (var l in c.Loops)
                {
                    List<DeviceInfo8001> lstDeviceInfo8001=l.GetDevices<DeviceInfo8001>();
                    if(lstDeviceInfo8001.Count>0)
                    {
                        int currentLoopMaxDeviceID=lstDeviceInfo8001.Max(device=>device.ID);
                        if(currentLoopMaxDeviceID>maxDeviceID)
                        {
                            maxDeviceID = currentLoopMaxDeviceID;
                        }                    
                    }                        
                }
            }
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
        /// <summary>
        /// 计算节点所在页签的相对位置
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="standardFlag"></param>
        /// <param name="mixedFlag"></param>
        /// <param name="generalFlag"></param>
        /// <param name="mcbFlag"></param>
        /// <returns></returns>
        private int CalculateSheetRelativeIndex(ControllerNodeType nodeType,bool standardFlag, bool mixedFlag, bool generalFlag, bool mcbFlag)
        {
            int result = 1;
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
            return result;
        }
 
        public bool DownloadDefaultEXCELTemplate(string strFilePath,IFileService fileService,ExcelTemplateCustomizedInfo customizedInfo)
        {
            try
            {
                //const int FIXED_SHEET_AMOUNT = 5;//摘要信息,标准组态,通用组态,混合组态,网络手控盘
                ControllerConfig8001 config = new ControllerConfig8001();
                List<int> lstDeviceCodeLength=config.GetDeviceCodeLength(); //器件编码长度集合
                string strDeviceCodeLength = ""; //器件编码长度字符串
                int defaultDeviceTypeCode = config.DefaultDeviceTypeCode;//默认器件编码                
                DeviceType defaultDeviceType = config.GetDeviceTypeViaDeviceCode(defaultDeviceTypeCode);//默认器件类型
                string defaultMachineNo = "00";//默认机器号
                int defaultMachineNoLength=defaultMachineNo.Length;  //默认机器号长度
                int defaultLoopCodeLength = 2;//默认回路编码长度
                int defaultDeviceCodeLength = 7;//默认器件编码长度
                int loopMaxLoopAmount=config.GetMaxLoopAmountValue();
                ControllerNodeModel[] nodes = config.GetNodes();
                string loopSheetNamePrefix="";//回路页签名称前缀
                string standardLinkageSheetName="";//标准组态页签名称
                string generalLinkageSheetName="";//通用组态页签名称
                string mixedLinkageSheetName="";//混合组态页签名称
                string manualControlBoardSheetName = "";//网络手动盘页签名称
                foreach (var r in lstDeviceCodeLength)
                {
                    strDeviceCodeLength += r.ToString()+",";
                }
                strDeviceCodeLength = strDeviceCodeLength.Substring(0, strDeviceCodeLength.LastIndexOf(","));
                for (int i = 0; i < nodes.Length; i++)
                {
                    switch(nodes[i].Type)
                    {
                        case ControllerNodeType.Loop:
                            loopSheetNamePrefix = nodes[i].Name;
                            break;
                        case ControllerNodeType.Standard:
                            standardLinkageSheetName = nodes[i].Name;
                            break;
                        case ControllerNodeType.General:
                            generalLinkageSheetName = nodes[i].Name;
                            break;
                        case ControllerNodeType.Mixed:
                            mixedLinkageSheetName = nodes[i].Name;
                            break;
                        case ControllerNodeType.Board:
                            manualControlBoardSheetName = nodes[i].Name;
                            break;
                    }
                }

                //此控制器可设置的回路总数量                
                int loopTotalAmount = loopMaxLoopAmount;
                
                string controllerName = "默认名称NT8001";
                string serialPort = "COM1";
                //每页签内的回路数量
                int loopAmountPerSheet = 8;
                bool blnStandardLinkageFlag = true;
                bool blnMixedLinkageFlag = true;
                bool blnGeneralLinkageFlag = true;
                bool blnManualControlBoardFlag = true;
                if (customizedInfo != null)
                {
                    defaultDeviceCodeLength = customizedInfo.SelectedDeviceCodeLength;
                    switch(defaultDeviceCodeLength)
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

                    if (customizedInfo.LoopAmount > loopMaxLoopAmount || customizedInfo.LoopAmount <=0 )//确保回路信息在合理范围内
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
                    blnStandardLinkageFlag = customizedInfo.StandardLinkageFlag;
                    blnMixedLinkageFlag = customizedInfo.MixedLinkageFlag;
                    blnGeneralLinkageFlag = customizedInfo.GeneralLinkageFlag;
                    blnManualControlBoardFlag = customizedInfo.ManualControlBoardFlag;
                }


                //回路页签数量
                int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)loopTotalAmount / loopAmountPerSheet));
                //每回路可设置最大器件数量
                int maxDeviceAmount = config.GetMaxDeviceAmountValue();
                string summarySheetName = "摘要信息";
                //模板工作薄的Sheet页构成为：
                //摘要信息页签
                //各回路页签
                //标准组态页签
                //通用组态页签
                //混合组态页签
                //网络手控盘页签          
              //  int totalSheetAmount = FIXED_SHEET_AMOUNT + loopSheetAmount;//所有页签数量
                List<string> lstSheetNames = new List<string>();
                lstSheetNames.Add(summarySheetName);
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
                //sheetNames[loopSheetAmount + 1] = "标准组态";
                //sheetNames[loopSheetAmount + 2] = "通用组态";
                //sheetNames[loopSheetAmount + 3] = "混合组态";
                //sheetNames[loopSheetAmount + 4] = "网络手控盘";
                if (blnStandardLinkageFlag) { lstSheetNames.Add(standardLinkageSheetName); }
                if(blnGeneralLinkageFlag){lstSheetNames.Add(generalLinkageSheetName);}
                if(blnMixedLinkageFlag){lstSheetNames.Add(mixedLinkageSheetName);}
                if (blnManualControlBoardFlag) { lstSheetNames.Add(manualControlBoardSheetName); }

                string sheetNamesWithoutLoopName = "";//除“所有回路页签名称”外的其它页签名称
                int sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Standard, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex]+";"; }
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Mixed, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.General, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
                sheetNamesWithoutLoopNameIndex = CalculateSheetRelativeIndex(ControllerNodeType.Board, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                if (sheetNamesWithoutLoopNameIndex != 0) { sheetNamesWithoutLoopName += lstSheetNames[loopSheetAmount + sheetNamesWithoutLoopNameIndex] + ";"; }
                ExcelService excelService = new ExcelService(strFilePath, fileService);                
                excelService.CreateExcelSheets(lstSheetNames);

                #region 设置单元格样式
                Utility.CellStyle cellCaptionStyle = GetCaptionCellStyle();
                Utility.CellStyle cellSubCaptionStyle = GetSubCaptionCellStyle();
                Utility.CellStyle cellDataStyle = GetDataCellStyle("Left", false);
                Utility.CellStyle cellTableHeadStyle = GetTableHeadCellStyle();
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
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 17;
                mergeCellRange.LastRowIndex = 17;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);

                #endregion
                excelService.RowHeight = 20;//到下一个高度设置前，使用该高度
                excelService.SetCellValue(lstSheetNames[0], 0, 0, "NT8001系列控制器配置数据导入模板", ExcelService.CellStyleType.Caption);
                excelService.RowHeight = 15;
                excelService.CellSubCaptionStyle = GetSubCaptionCellStyle();
                excelService.SetCellValue(lstSheetNames[0], 2, 0, "<适应软件版本" + config.CompatibleSoftwareVersionForExcelTemplate + ">", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 2, 1, null, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 2, 2, null, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 3, 0, "控制器设置", ExcelService.CellStyleType.SubCaption);
                excelService.SetCellValue(lstSheetNames[0], 4, 0, "名称", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 4, 1, "值", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 4, 2, "填写说明", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 5, 0, "控制器名称", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 1, controllerName, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 2, "最多20个字符", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 0, "控制器类型", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 1, "NT8001", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 2, "可填:"+config.CompatibleControllerTypeForExcelTemplate, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 0, "控制器机号", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 1, defaultMachineNo, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 2, "0-" + config.GetMaxMachineAmountValue().ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 0, "器件长度", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 1, defaultDeviceCodeLength.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 2, "可填:" + strDeviceCodeLength, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 0, "串口号", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 1, serialPort, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 2, "可填:COM1-COM10", ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 11, 0, "回路设置", ExcelService.CellStyleType.SubCaption);

                excelService.SetCellValue(lstSheetNames[0], 12, 0, "名称", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 1, "值", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 2, "填写说明", ExcelService.CellStyleType.TableHead);

                excelService.SetCellValue(lstSheetNames[0], 13, 0, "回路数量", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 1, loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 2, "可填：1-" + loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 14, 0, "回路分组", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 1, loopSheetAmount.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 2, "可填:1-" + loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 15, 0, "默认器件类型", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 1, defaultDeviceType.Name, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 2, "可填：有效设备编号", ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 17, 0, " 其它设置", ExcelService.CellStyleType.SubCaption);
                excelService.SetCellValue(lstSheetNames[0], 18, 0, "名称", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 1, "值", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 2, "填写说明", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 19, 0, "工作表名称", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 1, sheetNamesWithoutLoopName, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 2, "除“回路”外的工作表名称，以‘分号’分隔", ExcelService.CellStyleType.Data);


                excelService.SetMergeCells(lstSheetNames[0], lstMergeCellRange);//设置"摘要信息"合并单元格
                //设置列宽               
                excelService.SetColumnWidth(lstSheetNames[0], 0, 15f);
                excelService.SetColumnWidth(lstSheetNames[0], 1, 15f );

                excelService.SetColumnWidth(lstSheetNames[0], 2, 40f);



                //生成所有回路页签模板
                for (int i = 1; i <= loopSheetAmount; i++)
                {
                    lstMergeCellRange.Clear();
                    for (int j = 0; j < loopAmountPerSheet; j++)
                    {
                        //  string deviceCode=defaultMachineNo+ defaultLoopCodeLength
                        string loopCode=defaultMachineNo + (j + 1 + (i - 1) * 4).ToString().PadLeft(defaultLoopCodeLength,'0');
                        //回路标题：回路1                        
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount, 0, "回路:" +loopCode , ExcelService.CellStyleType.SubCaption);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 0, "编码", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 1, "器件类型", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 2, "特性", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 3, "屏蔽", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 4, "灵敏度", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 5, "输出组1", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 6, "输出组2", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 7, "输出组3", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 8, "延时", ExcelService.CellStyleType.TableHead);
                        //板卡号，手盘号，手键号暂定不在器件中处理
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 9, "广播分区", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 10, "楼号", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 11, "区号", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 12, "层号", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 13, "房间号", ExcelService.CellStyleType.TableHead);
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 14, "安装地点", ExcelService.CellStyleType.TableHead);
                        //回路标题行合并
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount;
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount;
                        mergeCellRange.FirstColumnIndex = 0;
                        mergeCellRange.LastColumnIndex = 14;
                        lstMergeCellRange.Add(mergeCellRange);
                        //回路默认器件信息
                        for (int k = 0; k < maxDeviceAmount; k++)
                        {
                            string deviceCode = (k + 1).ToString().PadLeft(defaultDeviceCodeLength - defaultLoopCodeLength - defaultMachineNoLength,'0');
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 0, loopCode+deviceCode, ExcelService.CellStyleType.Data); //器件编码
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 1,  defaultDeviceType.Name, ExcelService.CellStyleType.Data); //器件类型
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 2, null, ExcelService.CellStyleType.Data); 
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 3,  "0", ExcelService.CellStyleType.Data); //屏蔽
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 4,  "2", ExcelService.CellStyleType.Data); //灵敏度
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 5, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 6, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 7, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 8, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 9, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 10, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 11, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 12, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 13, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + 2, 14, null, ExcelService.CellStyleType.Data); 
                        }
                    }
                    excelService.SetMergeCells(lstSheetNames[i], lstMergeCellRange);//设置"回路页签"合并单元格
                }
                #region 标准组态表头
                if (blnStandardLinkageFlag)
                { 
                    lstMergeCellRange.Clear();
                    // 加的1页，为“摘要页”
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Standard, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 0, 0, lstSheetNames[loopSheetAmount + currentIndex], ExcelService.CellStyleType.SubCaption);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 0, "输出组号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 1, "联动模块1", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 2, "联动模块2", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 3, "联动模块3", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "联动模块4", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 5, "联动模块5", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 6, "联动模块6", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 7, "联动模块7", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 8, "联动模块8", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 9, "动作常数", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 10, "联动组1", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 11, "联动组2", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 12, "联动组3", ExcelService.CellStyleType.TableHead);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 12;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(lstSheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"标准组态页签"合并单元格
                }
                #endregion
                #region 混合组态表头
                if (blnMixedLinkageFlag)
                { 
                    lstMergeCellRange.Clear();
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Mixed, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 0, 0, lstSheetNames[loopSheetAmount + currentIndex], ExcelService.CellStyleType.SubCaption);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 1, "动作常数", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 2, "动作类型", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 3, "A分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "A楼", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 5, "A区", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 6, "A层", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 7, "A路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 8, "A编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 9, "A类型", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 10, "B分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 11, "B楼", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 12, "B区", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 13, "B层", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 14, "B路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 15, "B编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 16, "B类型", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 17, "C分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 18, "C楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 19, "C区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 20, "C层号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 21, "C机号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 22, "C回路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 23, "C编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 24, "C类型", ExcelService.CellStyleType.TableHead);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 24;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(lstSheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"混合组态页签"合并单元格
                }
                #endregion
                #region 通用组态表头
                if (blnGeneralLinkageFlag)
                { 
                    lstMergeCellRange.Clear();
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.General, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 0, 0, lstSheetNames[loopSheetAmount + currentIndex], ExcelService.CellStyleType.SubCaption);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 1, "动作常数", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 2, "A楼", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 3, "A区", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "A层1", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 5, "A层2", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 6, "类型A", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 7, "C分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 8, "C楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 9, "C区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 10, "C层号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 11, "C机号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 12, "C回路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 13, "C编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 14, "类型C", ExcelService.CellStyleType.TableHead);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 14;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(lstSheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"通用组态页签"合并单元格
                }
                #endregion
                #region 网络手动盘表头
                if (blnManualControlBoardFlag)
                { 
                    lstMergeCellRange.Clear();
                    int currentIndex = CalculateSheetRelativeIndex(ControllerNodeType.Board, blnStandardLinkageFlag, blnMixedLinkageFlag, blnGeneralLinkageFlag, blnManualControlBoardFlag);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 0, 0, lstSheetNames[loopSheetAmount + currentIndex], ExcelService.CellStyleType.SubCaption);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 0, "编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 1, "板卡号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 2, "手盘号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 3, "手键号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "地编号", ExcelService.CellStyleType.TableHead);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 4;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(lstSheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"网络手动盘页签"合并单元格
                }
                #endregion

                excelService.SaveToFile();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public bool ReadEXCELTemplate(string strFilePath, IFileService fileService)
        {
            ExcelService excelService = new ExcelService(strFilePath, fileService);
            ControllerConfig8001 config = new ControllerConfig8001();
            ControllerNodeModel[] nodes = config.GetNodes();
            
            DataTable dt=  excelService.OpenExcel(strFilePath,config.SummarySheetNameForExcelTemplate);
            string str = "a";
            return true;

        }
        private CellStyle GetCaptionCellStyle()
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
        private CellStyle GetSubCaptionCellStyle()
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
        private CellStyle GetTableHeadCellStyle()
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
        private CellStyle GetDataCellStyle(string pAlignment, bool pBoldFlag)
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

    }
}
