using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            return result+1;// +1为新增加了一个“设备类型”页签
        }
        /// <summary>
        /// 根据“回路数量”及“回路分组”取得回路名称集合
        /// </summary>
        /// <param name="loopSheetNamePrefix">回路页签命名前缀</param>
        /// <param name="loopAmount">回路数量</param>
        /// <param name="loopGroupAmount">回路分组</param>
        /// <returns></returns>
        private List<string> GetSheetNameForLoop(string loopSheetNamePrefix, int loopAmount, int loopGroupAmount)
        {
            int loopAmountPerSheet = Convert.ToInt32(Math.Ceiling((float)loopAmount / loopGroupAmount));
            //回路页签数量
            int loopSheetAmount = Convert.ToInt32(Math.Ceiling((float)loopAmount / loopAmountPerSheet));
            List<string> lstSheetNames = new List<string>();            
            for (int i = 1; i <= loopSheetAmount; i++)
            {
                string loopNameEndIndex;
                if ((i * loopAmountPerSheet) > loopAmount)
                {
                    loopNameEndIndex = loopAmount.ToString();
                }
                else
                {
                    loopNameEndIndex = (i * loopAmountPerSheet).ToString();
                }
                lstSheetNames.Add(loopSheetNamePrefix + "(" + (((i - 1) * loopAmountPerSheet) + 1).ToString() + "-" + loopNameEndIndex + ")");
            }
            return lstSheetNames;
        }
        public bool DownloadDefaultEXCELTemplate(string strFilePath,IFileService fileService,ExcelTemplateCustomizedInfo customizedInfo)
        {
            try
            {
                //const int FIXED_SHEET_AMOUNT = 5;//摘要信息,标准组态,通用组态,混合组态,网络手动盘
                ControllerConfig8001 config = new ControllerConfig8001();
                List<int> lstDeviceCodeLength=config.GetDeviceCodeLength(); //器件编码长度集合
                ColumnConfigInfo[] deviceColumnDefinitionArray = config.GetDeviceColumns(); //取得器件的列定义信息
                Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的配置信息
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
                string deviceTypeSheetName = "设备类型";
                //模板工作薄的Sheet页构成为：
                //摘要信息页签
                //各回路页签
                //标准组态页签
                //通用组态页签
                //混合组态页签
                //网络手动盘页签          
              //  int totalSheetAmount = FIXED_SHEET_AMOUNT + loopSheetAmount;//所有页签数量
                List<string> lstSheetNames = new List<string>();
                lstSheetNames.Add(summarySheetName);
                lstSheetNames.Add(deviceTypeSheetName);                
                List<string> lstLoopSheetName;
                if (customizedInfo == null) //默认模板
                {
                    lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, loopAmountPerSheet);
                }
                else  //自定义模板
                {
                    lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopTotalAmount, customizedInfo.LoopGroupAmount);
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
                if (blnStandardLinkageFlag) { lstSheetNames.Add(standardLinkageSheetName); }
                if (blnMixedLinkageFlag) { lstSheetNames.Add(mixedLinkageSheetName); }
                if(blnGeneralLinkageFlag){lstSheetNames.Add(generalLinkageSheetName);}                
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
                lstMergeCellRange.Add(mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 17;
                mergeCellRange.LastRowIndex = 17;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 2;
                lstMergeCellRange.Add(mergeCellRange);

                #endregion
                #region 设置摘要页签数据模板
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
                excelService.SetCellValue(lstSheetNames[0], 5, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[5], ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 1, controllerName, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 5, 2, "最多10个字符", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[6], ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 1, "NT8001", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 6, 2, "可填: "+config.CompatibleControllerTypeForExcelTemplate, ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 8, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[8], ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 1, defaultMachineNo, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 8, 2, "可填: 7位编码(00-" + config.GetMaxMachineAmountValue(7).ToString() + ");8位编码(000-" + config.GetMaxMachineAmountValue(8).ToString()+")", ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 7, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[7], ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 1, defaultDeviceCodeLength.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 7, 2, "可填: " + strDeviceCodeLength, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 0, dictNameOfControllerSettingInSummaryInfoOfExcelTemplate[9], ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 1, serialPort, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 9, 2, "可填: COM1-COM9", ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 11, 0, "回路设置", ExcelService.CellStyleType.SubCaption);

                excelService.SetCellValue(lstSheetNames[0], 12, 0, "名称", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 1, "值", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 12, 2, "填写说明", ExcelService.CellStyleType.TableHead);

                excelService.SetCellValue(lstSheetNames[0], 13, 0, "回路数量", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 1, loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 13, 2, "可填: 1-" + loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 14, 0, "回路分组", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 1, loopSheetAmount.ToString(), ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 14, 2, "可填: 1-" + loopTotalAmount.ToString(), ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 15, 0, "默认器件类型", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 1, defaultDeviceType.Name, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 15, 2, "可填: 有效设备编号", ExcelService.CellStyleType.Data);

                excelService.SetCellValue(lstSheetNames[0], 17, 0, " 其它设置", ExcelService.CellStyleType.SubCaption);
                excelService.SetCellValue(lstSheetNames[0], 18, 0, "名称", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 1, "值", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 18, 2, "填写说明", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[0], 19, 0, "工作表名称", ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 1, sheetNamesWithoutLoopName, ExcelService.CellStyleType.Data);
                excelService.SetCellValue(lstSheetNames[0], 19, 2, "可填: 标准组态;混合组态;通用组态;网络手动盘", ExcelService.CellStyleType.Data);
                //除“回路”外的工作表名称，以‘分号’分隔                
                excelService.SetMergeCells(lstSheetNames[0], lstMergeCellRange);//设置"摘要信息"合并单元格
                //设置列宽               
                excelService.SetColumnWidth(lstSheetNames[0], 0, 15f);
                excelService.SetColumnWidth(lstSheetNames[0], 1, 15f);
                excelService.SetColumnWidth(lstSheetNames[0], 2, 40f);                
                mergeCellRange =new MergeCellRange();
                mergeCellRange.FirstRowIndex=6;
                mergeCellRange.LastRowIndex=6;
                mergeCellRange.FirstColumnIndex=1;
                mergeCellRange.LastColumnIndex=1;
                excelService.SetSheetValidationForListConstraint(lstSheetNames[0],RefereceRegionName.ControllerType.ToString(),mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 7;
                mergeCellRange.LastRowIndex = 7;
                mergeCellRange.FirstColumnIndex = 1;
                mergeCellRange.LastColumnIndex = 1;
                excelService.SetSheetValidationForListConstraint(lstSheetNames[0], RefereceRegionName.DeviceCodeLength.ToString(), mergeCellRange);
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 9;
                mergeCellRange.LastRowIndex = 9;
                mergeCellRange.FirstColumnIndex = 1;
                mergeCellRange.LastColumnIndex = 1;
                excelService.SetSheetValidationForListConstraint(lstSheetNames[0], RefereceRegionName.SerialPortNumber.ToString(), mergeCellRange);                
                #endregion
                #region 生成《设备类型》页签模板
                lstMergeCellRange.Clear();
                List<DeviceType> lstDeviceType = config.GetDeviceTypeInfo();
                mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 1;
                lstMergeCellRange.Add(mergeCellRange);
                excelService.RowHeight = 20;//到下一个高度设置前，使用该高度
                excelService.SetCellValue(lstSheetNames[1], 0, 0, "设备类型", ExcelService.CellStyleType.SubCaption);
                excelService.RowHeight = 15;
                excelService.CellSubCaptionStyle = GetSubCaptionCellStyle();
                excelService.SetCellValue(lstSheetNames[1], 1, 0, " 编号", ExcelService.CellStyleType.TableHead);
                excelService.SetCellValue(lstSheetNames[1], 1, 1, " 名称", ExcelService.CellStyleType.TableHead);
                int rowNumber = 1;
                string regionNameStartIndex = "30002";
                foreach (var deviceType in lstDeviceType)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 0, deviceType.Code.ToString(), ExcelService.CellStyleType.Data);
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 1, deviceType.Name, ExcelService.CellStyleType.Data);
                }
                excelService.SetMergeCells(lstSheetNames[1], lstMergeCellRange);//设置"摘要信息"合并单元格
                excelService.SetColumnWidth(lstSheetNames[1], 1, 15f);                                
                excelService.SetRangeName(RefereceRegionName.DeviceType.ToString(), string.Format("'{0}'!$B$3:$B${1}", lstSheetNames[1], (lstDeviceType.Count + 2).ToString()));

                //写入控制器类型数据
                List<ControllerType> lstControllerType = config.GetControllerType();                
                excelService.SetCellValue(lstSheetNames[1], 30000, 3, "控制器类型", ExcelService.CellStyleType.SubCaption);
                rowNumber=30000;
                foreach (var controllerType in lstControllerType)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber , 3, controllerType.ToString() , ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.ControllerType.ToString(), string.Format("'{0}'!$D$" + regionNameStartIndex + ":$D${1}", lstSheetNames[1], (lstControllerType.Count + 30001).ToString()));
                //写入器件长度
                excelService.SetCellValue(lstSheetNames[1], 30000, 5, "器件长度", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                foreach (var codeLength in lstDeviceCodeLength)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 5, codeLength.ToString(), ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.DeviceCodeLength.ToString(), string.Format("'{0}'!$F$" + regionNameStartIndex + ":$F${1}", lstSheetNames[1], (lstDeviceCodeLength.Count + 30001).ToString()));
                //写入串口号
                excelService.SetCellValue(lstSheetNames[1], 30000, 7, "串口号", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<string> lstSerialPort = config.GetSerialPortNumber();
                foreach (var sp in lstSerialPort)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 7, sp, ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.SerialPortNumber.ToString(), string.Format("'{0}'!$H$" + regionNameStartIndex + ":$H${1}", lstSheetNames[1], (lstSerialPort.Count + 30001).ToString()));
                //写入动作常数
                excelService.SetCellValue(lstSheetNames[1], 30000, 9, "动作常数", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<int> lstActionCoefficient = config.GetActionCoefficient();
                foreach (var actionCoefficient in lstActionCoefficient)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 9, actionCoefficient.ToString(), ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.ActionCoefficient.ToString(), string.Format("'{0}'!$J$" + regionNameStartIndex + ":$J${1}", lstSheetNames[1], (lstActionCoefficient.Count + 30001).ToString()));
                //写入动作类型
                excelService.SetCellValue(lstSheetNames[1], 30000, 11, "动作类型", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<LinkageActionType> lstActionType = config.GetLinkageActionType();
                foreach (var actionType in lstActionType)
                {
                    rowNumber++;

                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 11, actionType.GetDescription(), ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.ActionType.ToString(), string.Format("'{0}'!$L$" + regionNameStartIndex + ":$L${1}", lstSheetNames[1], (lstActionType.Count + 30001).ToString()));
                //写入联动分类（全部）
                excelService.SetCellValue(lstSheetNames[1], 30000, 13, "联动分类(全部)", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<LinkageType> lstLinkageType = config.GetLinkageType();
                foreach (var linkageType in lstLinkageType)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 13, linkageType.GetDescription(), ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.LinkageTypeAll.ToString(), string.Format("'{0}'!$N$" + regionNameStartIndex + ":$N${1}", lstSheetNames[1], (lstLinkageType.Count + 30001).ToString()));
                //写入联动分类（精简）
                excelService.SetCellValue(lstSheetNames[1], 30000, 15, "联动分类(精简)", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                lstLinkageType = config.GetLinkageTypeWithCastration();
                foreach (var linkageType in lstLinkageType)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 15, linkageType.GetDescription(), ExcelService.CellStyleType.Data);
                }                
                excelService.SetRangeName(RefereceRegionName.LinkageTypeCastration.ToString(), string.Format("'{0}'!$P$" + regionNameStartIndex + ":$P${1}", lstSheetNames[1], (lstLinkageType.Count + 30001).ToString()));
                //写入特性
                excelService.SetCellValue(lstSheetNames[1], 30000, 17, "特性", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<string> lstFeature = config.GetFeatureList();
                foreach (var feature in lstFeature)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 17, feature, ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.Feature.ToString(), string.Format("'{0}'!$R$" + regionNameStartIndex + ":$R${1}", lstSheetNames[1], (lstFeature.Count + 30001).ToString()));
                //写入屏蔽
                excelService.SetCellValue(lstSheetNames[1], 30000, 19, "屏蔽", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<string> lstDisable = config.GetDisableList();
                foreach (var disable in lstDisable)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 19, disable, ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.Disable.ToString(), string.Format("'{0}'!$T$" + regionNameStartIndex + ":$T${1}", lstSheetNames[1], (lstDisable.Count + 30001).ToString()));
                //写入灵敏度
                excelService.SetCellValue(lstSheetNames[1], 30000, 21, "灵敏度", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<string> lstSensitiveLevel = config.GetSensitiveLevelList();
                foreach (var sensitiveLevel in lstSensitiveLevel)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 21, sensitiveLevel, ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.SensitiveLevel.ToString(), string.Format("'{0}'!$V$" + regionNameStartIndex + ":$V${1}", lstSheetNames[1], (lstSensitiveLevel.Count + 30001).ToString()));

                //写入具有任意火警的器件名称                
                excelService.SetCellValue(lstSheetNames[1], 30000, 23, "器件类型(任意火警)", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<DeviceType> lstDeviceTypeWithAnyAlarm= config.GetDeviceTypeInfoWithAnyAlarm();
                foreach (var deviceType in lstDeviceTypeWithAnyAlarm)
                {
                    rowNumber++;                    
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 23, deviceType.Name, ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.DeviceTypeWithAnyAlarm.ToString(), string.Format("'{0}'!$X$" + regionNameStartIndex + ":$X${1}", lstSheetNames[1], (lstDeviceTypeWithAnyAlarm.Count + 30001).ToString()));

                //写入具有非报警器件的器件名称                
                excelService.SetCellValue(lstSheetNames[1], 30000, 25, "器件类型(非火警器件)", ExcelService.CellStyleType.SubCaption);
                rowNumber = 30000;
                List<DeviceType> lstDeviceTypeWithoutFireAlarm = config.GetDeviceTypeInfoWithoutFireDevice();
                foreach (var deviceType in lstDeviceTypeWithoutFireAlarm)
                {
                    rowNumber++;
                    excelService.SetCellValue(lstSheetNames[1], rowNumber, 25, deviceType.Name, ExcelService.CellStyleType.Data);
                }
                excelService.SetRangeName(RefereceRegionName.DeviceTypeWithoutFireDevice.ToString(), string.Format("'{0}'!$Z$" + regionNameStartIndex + ":$Z${1}", lstSheetNames[1], (lstDeviceTypeWithoutFireAlarm.Count + 30001).ToString()));
                #endregion
                
                #region 生成所有回路页签模板
                int currentLoopStartIndex = 2;
                for (int i = currentLoopStartIndex; i <= loopSheetAmount + 1; i++)
                {
                    lstMergeCellRange.Clear();
                    
                    for (int j = 0; j < loopAmountPerSheet; j++)
                    {
                        //  string deviceCode=defaultMachineNo+ defaultLoopCodeLength
                        string loopCode=defaultMachineNo + (j + 1 + (i - currentLoopStartIndex) * loopAmountPerSheet).ToString().PadLeft(defaultLoopCodeLength,'0');
                        int extraLine = 0;
                        if (j != 0)
                        {
                            extraLine = 2;
                        }
                        //回路标题：回路1                        
                        excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount+ j  * extraLine, 0, loopSheetNamePrefix + ":" +loopCode , ExcelService.CellStyleType.SubCaption);
                        for (int devColumnCount = 0; devColumnCount < deviceColumnDefinitionArray.Length; devColumnCount++)
                        {
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1 + j * extraLine , devColumnCount, deviceColumnDefinitionArray[devColumnCount].ColumnName, ExcelService.CellStyleType.TableHead);
                        }
                        #region Obsolete
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 0, "编码", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 1, "器件类型", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 2, "特性", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 3, "屏蔽", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 4, "灵敏度", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 5, "输出组1", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 6, "输出组2", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 7, "输出组3", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 8, "延时", ExcelService.CellStyleType.TableHead);
                        
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 9, "广播分区", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 10, "楼号", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 11, "区号", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 12, "层号", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 13, "房间号", ExcelService.CellStyleType.TableHead);
                        //excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + 1, 14, "安装地点", ExcelService.CellStyleType.TableHead);
                        #endregion
                        //回路标题行合并
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + j * extraLine;
                        mergeCellRange.FirstColumnIndex = 0;
                        mergeCellRange.LastColumnIndex = 14;
                        lstMergeCellRange.Add(mergeCellRange);
                        //回路默认器件信息
                        for (int k = 0; k < maxDeviceAmount; k++)
                        {
                            string deviceCode = (k + 1).ToString().PadLeft(defaultDeviceCodeLength - defaultLoopCodeLength - defaultMachineNoLength,'0');
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 0, loopCode + deviceCode , ExcelService.CellStyleType.Data); //器件编码
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 1, defaultDeviceType.Name, ExcelService.CellStyleType.Data); //器件类型
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 2, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 3, "0", ExcelService.CellStyleType.Data); //屏蔽
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 4, "2", ExcelService.CellStyleType.Data); //灵敏度
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 5, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 6, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 7, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 8, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 9, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 10, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 11, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 12, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 13, null, ExcelService.CellStyleType.Data);
                            excelService.SetCellValue(lstSheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 14, null, ExcelService.CellStyleType.Data); 
                        }
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount  + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2)+maxDeviceAmount-1;
                        mergeCellRange.FirstColumnIndex = 1;
                        mergeCellRange.LastColumnIndex = 1;
                        excelService.SetSheetValidationForListConstraint(lstSheetNames[i], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 2;
                        mergeCellRange.LastColumnIndex = 2;
                        excelService.SetSheetValidationForListConstraint(lstSheetNames[i], RefereceRegionName.Feature.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 3;
                        mergeCellRange.LastColumnIndex = 3;
                        excelService.SetSheetValidationForListConstraint(lstSheetNames[i], RefereceRegionName.Disable.ToString(), mergeCellRange);
                        mergeCellRange = new MergeCellRange();
                        mergeCellRange.FirstRowIndex = j * maxDeviceAmount + (j * extraLine + 2);
                        mergeCellRange.LastRowIndex = j * maxDeviceAmount + (j * extraLine + 2) + maxDeviceAmount - 1;
                        mergeCellRange.FirstColumnIndex = 4;
                        mergeCellRange.LastColumnIndex = 4;
                        excelService.SetSheetValidationForListConstraint(lstSheetNames[i], RefereceRegionName.SensitiveLevel.ToString(), mergeCellRange);
                    }
                    
                    excelService.SetMergeCells(lstSheetNames[i], lstMergeCellRange);//设置"回路页签"合并单元格
                }
                #endregion
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
                    int maxStandardLinkageAmount = config.GetMaxAmountForStandardLinkageConfig();
                    for (int i = 2; i < maxStandardLinkageAmount+3; i++)
                    {
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 0, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 1, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 2, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 3, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 4, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 5, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 6, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 7, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 8, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 9, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 10, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 11, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 12, null, ExcelService.CellStyleType.Data);  
                    }
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxStandardLinkageAmount+2;
                    mergeCellRange.FirstColumnIndex =  9;
                    mergeCellRange.LastColumnIndex = 9;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                    
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
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "A楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 5, "A区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 6, "A层号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 7, "A路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 8, "A编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 9, "A类型", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 10, "B分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 11, "B楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 12, "B区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 13, "B层号", ExcelService.CellStyleType.TableHead);
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
                    int maxMixedLinkageAmount = config.GetMaxAmountForMixedLinkageConfig();
                    for (int i = 2; i < maxMixedLinkageAmount + 3; i++)
                    {
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 0, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 1, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 2, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 3, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 4, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 5, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 6, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 7, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 8, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 9, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 10, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 11, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 12, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 13, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 14, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 15, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 16, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 17, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 18, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 19, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 20, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 21, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 22, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 23, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 24, null, ExcelService.CellStyleType.Data);
                    }
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 1;
                    mergeCellRange.LastColumnIndex = 1;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 2;
                    mergeCellRange.LastColumnIndex = 2;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionType.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 3;
                    mergeCellRange.LastColumnIndex = 3;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 9;
                    mergeCellRange.LastColumnIndex = 9;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 10;
                    mergeCellRange.LastColumnIndex = 10;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 16;
                    mergeCellRange.LastColumnIndex = 16;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 17;
                    mergeCellRange.LastColumnIndex = 17;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeCastration.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxMixedLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 24;
                    mergeCellRange.LastColumnIndex = 24;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceType.ToString(), mergeCellRange);
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
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 2, "A楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 3, "A区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 4, "A层号1", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 5, "A层号2", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 6, "类型A", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 7, "C分类", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 8, "C楼号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 9, "C区号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 10, "C层号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 11, "C机号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 12, "C回路号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 13, "C编号", ExcelService.CellStyleType.TableHead);
                    excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], 1, 14, "C类型", ExcelService.CellStyleType.TableHead);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 0;
                    mergeCellRange.LastRowIndex = 0;
                    mergeCellRange.FirstColumnIndex = 0;
                    mergeCellRange.LastColumnIndex = 14;
                    lstMergeCellRange.Add(mergeCellRange);
                    excelService.SetMergeCells(lstSheetNames[loopSheetAmount + currentIndex], lstMergeCellRange);//设置"通用组态页签"合并单元格                    
                    int maxGeneralLinkageAmount = config.GetMaxAmountForGeneralLinkageConfig();
                    for (int i = 2; i < maxGeneralLinkageAmount + 3; i++)
                    {
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 0, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 1, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 2, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 3, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 4, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 5, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 6, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 7, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 8, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 9, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 10, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 11, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 12, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 13, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 14, null, ExcelService.CellStyleType.Data);
                    }
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxGeneralLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 1;
                    mergeCellRange.LastColumnIndex = 1;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.ActionCoefficient.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxGeneralLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 6;
                    mergeCellRange.LastColumnIndex = 6;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeWithAnyAlarm.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxGeneralLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 7;
                    mergeCellRange.LastColumnIndex = 7;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.LinkageTypeAll.ToString(), mergeCellRange);
                    mergeCellRange = new MergeCellRange();
                    mergeCellRange.FirstRowIndex = 2;
                    mergeCellRange.LastRowIndex = maxGeneralLinkageAmount + 2;
                    mergeCellRange.FirstColumnIndex = 14;
                    mergeCellRange.LastColumnIndex = 14;
                    excelService.SetSheetValidationForListConstraint(lstSheetNames[loopSheetAmount + currentIndex], RefereceRegionName.DeviceTypeWithoutFireDevice.ToString(), mergeCellRange);
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
                    int maxMCBAmount = config.GetMaxAmountForManualControlBoardConfig();
                    for (int i = 2; i < maxMCBAmount + 3; i++)
                    {
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 0, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 1, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 2, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 3, null, ExcelService.CellStyleType.Data);
                        excelService.SetCellValue(lstSheetNames[loopSheetAmount + currentIndex], i, 4, null, ExcelService.CellStyleType.Data);
                    }
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
        public ControllerModel ReadEXCELTemplate(string strFilePath, IFileService fileService,ControllerModel targetController,out string strErrorMessage)
        {
            ExcelService excelService = new ExcelService(strFilePath, fileService);
            ControllerConfig8001 config = new ControllerConfig8001();
            DeviceService8001 deviceService = new DeviceService8001();
            ControllerNodeModel[] nodes = config.GetNodes();
            ColumnConfigInfo[] deviceColumnDefinitions = config.GetDeviceColumns();
            int maxDeviceAmount = config.GetMaxDeviceAmountValue(); //回路内器件信息最大数量
            //List<string> lstSheetName = new List<string>();//根据“摘要信息”取得所有数据的工作表名称
            Dictionary<int, string> dictNameOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetNameOfControllerSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”控制器设置的名称配置信息
            Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate(targetController.DeviceAddressLength); ////取得“摘要信息页”控制器设置的值的有效性
            Dictionary<int, string> dictNameOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetNameOfLoopSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”回路设置的名称配置信息
            Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”回路设置的值的有效性
            Dictionary<int, string> dictNameOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetNameOfOtherSettingInSummaryInfoOfExcelTemplate();//取得“摘要信息页”其它设置的名称配置信息
            Dictionary<int, RuleAndErrorMessage> dictValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate = config.GetValueVerifyingRuleOfOtherSettingInSummaryInfoOfExcelTemplate(); ////取得“摘要信息页”其它设置的值的有效性


            string strStatusInfo = ""; //状态描述信息
            bool blnIsError=false;     //错误标志
            //读取摘要页的列配置信息

            //读取回路页面的列配置信息

            //读取组态及手动盘的列配置信息

            int controllerSettingStartRow = 4;
            int loopSettingStartRow = controllerSettingStartRow + dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count+3;//3为“控制器表头行+空行+回路标题"
            int otherSettingStartRow = loopSettingStartRow + dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count + 3;
            //定义“摘要信息”页的数据行定义信息
            Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();
            summarySheetRowDefinition.Add(controllerSettingStartRow, 9);//控制器信息起始行定义 4为表头信息,
            summarySheetRowDefinition.Add(loopSettingStartRow, 15);//回路信息起始行定义
            summarySheetRowDefinition.Add(otherSettingStartRow, 19);//其它信息起始行定义
            bool blnSheetExistFlag;
            DataTable dt=  excelService.OpenExcel(strFilePath,config.SummarySheetNameForExcelTemplate,summarySheetRowDefinition,out blnSheetExistFlag);
            ControllerModel controller = new ControllerModel(); //存储"控制器配置"信息
            int loopAmount =1;//回路数量
            int loopGroup=1 ;//回路分组
            
            string loopSheetNamePrefix="";//回路页签名称前缀
            //string standardLinkageSheetName="";//标准组态页签名称
            //string generalLinkageSheetName="";//通用组态页签名称
            //string mixedLinkageSheetName="";//混合组态页签名称
            //string manualControlBoardSheetName = "";//网络手动盘页签名称                
            for (int i = 0; i < nodes.Length; i++)
            {
                switch(nodes[i].Type)
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
                    RuleAndErrorMessage verifingMessage =dictValueVerifyingRuleOfControllerSettingInSummaryInfoOfExcelTemplate[controllerSettingStartRow + 1 + i];
                    //verifingMessage.Rule
                    //verifingMessage.ErrorMessage                     
                    Regex exminator = new Regex(verifingMessage.Rule);
                    if (!exminator.IsMatch(dt.Rows[i][1].ToString()))
                    {
                        strStatusInfo += ";" + verifingMessage.ErrorMessage;
                        blnIsError = true;
                        continue;
                    }
                    else
                    {
                        GetControllerViaExcelTemplate(dt.Rows[i][0].ToString(),dt.Rows[i][1].ToString(),controller);
                    }
                }
            }
            if (!blnIsError)
            {
                if (controller.DeviceAddressLength != targetController.DeviceAddressLength)
                {
                    strStatusInfo += ";模板器件长度("+controller.DeviceAddressLength+")与当前控制器器件长度("+targetController.DeviceAddressLength+")不一致";
                    blnIsError = true;
                }
                if (controller.MachineNumber != targetController.MachineNumber)
                {
                    strStatusInfo += ";模板控制器机号(" + controller.MachineNumber + ")与当前控制器机号(" + targetController.MachineNumber + ")不一致";
                    blnIsError = true;
                }
                if (controller.Type != targetController.Type)
                {
                    strStatusInfo += ";模板控制器类型(" + controller.Type.ToString() + ")与当前控制器类型(" + targetController.Type.ToString() + ")不一致";
                    blnIsError = true;
                }
            }
            if (!blnIsError)//控制器配置信息正确
            {
                //controller
                //验证回路信息
                for (int i = 0 ; i < dictNameOfLoopSettingInSummaryInfoOfExcelTemplate.Count; i++)
                {
                    if (!(dt.Rows[dictNameOfControllerSettingInSummaryInfoOfExcelTemplate.Count + i][0].ToString() == dictNameOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow  +1 + i]))//+1为跨过标题行
                    {
                        strStatusInfo = "摘要信息-->回路设置-->模板名称不正确";
                        blnIsError = true;
                    }
                    else //名称一致，验证值的有效性
                    {
                        RuleAndErrorMessage verifingMessage = dictValueVerifyingRuleOfLoopSettingInSummaryInfoOfExcelTemplate[loopSettingStartRow +1 + i];
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
                string str = "TheErrorMessage";
            }
            if (blnIsError) //存在错误，直接反回
            {
                strErrorMessage = strStatusInfo;
                return controller;
            }

            List<string> lstOtherSheetName = new List<string>();//除“回路”外的其它工作表名称
            //if (lstLoopSheetName != null)
            //{
            //    lstSheetName = lstLoopSheetName;
            //}
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
                        //verifingMessage.Rule
                        //verifingMessage.ErrorMessage                     
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
                strErrorMessage = strStatusInfo;
                return controller;
            }
            //分组信息>回路数量时，重置回路分组信息
            if (loopGroup > loopAmount)
            {
                loopGroup = loopAmount;
            }
            List<string> lstLoopSheetName = GetSheetNameForLoop(loopSheetNamePrefix, loopAmount, loopGroup);//根据“摘要信息”取得所有回路数据的工作表名称

            string[] otherSheetNames = otherSettingValue.Split(';');
            for(int i=0;i<otherSheetNames.Length;i++)
            {
                lstOtherSheetName.Add(otherSheetNames[i]);
            }
            //将EXCEL模板中的回路信息增加至Controller
            foreach (var sheetName in lstLoopSheetName)            
            {
               string loopDetailErrorInfo="";               
               List<LoopModel> lstLoops= GetLoopData(excelService, strFilePath, sheetName, maxDeviceAmount,controller, out blnSheetExistFlag,out loopDetailErrorInfo);
               if (lstLoops != null)
               { 
                   if (blnSheetExistFlag)
                   {
                       foreach (var loop in lstLoops)
                       {
                           deviceService.TheLoop = loop;
                           if (deviceService.IsExistSameDeviceCode())
                           {
                               strStatusInfo += ";" + loop.Code +"回路存在重码";
                           }
                           else
                           { 
                                controller.Loops.Add(loop);
                           }
                       }                       
                   }
                   else
                   {
                       strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                   }
               }
               if (loopDetailErrorInfo != "") //有错误信息，则直接返回调用
               {
                   strStatusInfo += ";" + loopDetailErrorInfo;
                   strErrorMessage = strStatusInfo;
                   return controller;
               }
            }
            foreach (var sheetName in lstOtherSheetName)
            {
                config.GetMaxAmountForStandardLinkageConfig();
                
                switch (sheetName)
                { 
                    case "标准组态":
                         {                             
                             DataTable dtStandard = GetOtherSettingData(excelService, strFilePath, sheetName, config.GetMaxAmountForStandardLinkageConfig(),out blnSheetExistFlag);
                             if (blnSheetExistFlag)
                             {
                                 List<LinkageConfigStandard> lstStandardConfig = ConvertToStandardLinkageModelFromDataTable(dtStandard);
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
                             else
                             {
                                 strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                             }
                         }
                        break;
                    case "混合组态":
                        {
                            DataTable dtMixed = GetOtherSettingData(excelService, strFilePath, sheetName, config.GetMaxAmountForMixedLinkageConfig(),out blnSheetExistFlag);
                            if (blnSheetExistFlag)
                            {
                                List<LinkageConfigMixed> lstMixedConfig = ConvertToMixedLinkageModelFromDataTable(dtMixed);
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
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                    case "通用组态":
                        {
                            DataTable dtGeneral = GetOtherSettingData(excelService, strFilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(),out blnSheetExistFlag);
                            if (blnSheetExistFlag)
                            {

                                List<LinkageConfigGeneral> lstGeneralConfig = ConvertToGeneralLinkageModelFromDataTable(dtGeneral);
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
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                    case "网络手动盘":
                        {
                            DataTable dtMCB = GetOtherSettingData(excelService, strFilePath, sheetName, config.GetMaxAmountForGeneralLinkageConfig(),out blnSheetExistFlag);
                            if (blnSheetExistFlag)
                            {
                                List<ManualControlBoard> lstMCB = ConvertToManualControlBoardModelFromDataTable(dtMCB);
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
                            else
                            {
                                strStatusInfo += ";《" + sheetName + "》工作表名称配置不正确";
                            }
                        }
                        break;
                }
            }
            strErrorMessage = strStatusInfo;
            return controller;            
        }
        /// <summary>
        /// 读取EXEL,根据工作作名称，取得回路及器件信息
        /// </summary>
        /// <param name="excelService"></param>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="maxDevcieAmount"></param>
        /// <param name="deviceCodeLength"></param>
        /// <returns></returns>
        private List<LoopModel> GetLoopData(IExcelService excelService,string filePath, string sheetName,int maxDevcieAmount,ControllerModel controller,out bool sheetExistFlag,out string loopDetailErrorInfo)
        {
            int[] loopRange = ParseLoopSheetName(sheetName);//取得回路编号范围
            Dictionary<int, int> rowDefinition = new Dictionary<int, int>();
            int extraLines=2; //除数据外的其它行数
            for (int i = 0; i < (loopRange[1]-loopRange[0]+1); i++)
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
            dt = excelService.OpenExcel(filePath, sheetName, rowDefinition, out sheetExistFlag);
            return ConvertToLoopModelFromDataTable(dt, controller,out loopDetailErrorInfo);
            //如果未找到指定sheetName的EXCEL数据，需要返回“页签错误”的信息            
        }
        /// <summary>
        /// 将读出的回路及器件信息转换为LoopModel类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deviceCodeLength"></param>
        /// <returns></returns>
        private List<LoopModel> ConvertToLoopModelFromDataTable(DataTable dt,ControllerModel controller,out string loopDetailErrorInfo)
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
            int[] loopIndexRange=ParseLoopSheetName(dt.TableName);
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
                       if (loopIndexRange[loopCount].ToString().PadLeft(controller.LoopAddressLength,'0') == loop.Code.Substring(machineNumberOfDevice.Length)) //回路号验证
                       {
                           loopDetailErrorInfo += ";器件(" + dt.Rows[i]["编码"].ToString() + ")回路号(" + loop.Code.Substring(machineNumberOfDevice.Length) + ")与模板回路号(" + loopIndexRange[loopCount].ToString().PadLeft(controller.LoopAddressLength, '0') + ")不符";
                           blnErrorFlag = true;
                       }
                   }
                   if (blnErrorFlag)  //机号或回路号不正确，停止处理，返回调用
                   {
                       return null;
                   }                   
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
            return lstLoops;            
        }

        
        /// <summary>
        /// 解析回路工作表的名称，取得回路起始号及终止号
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private int[] ParseLoopSheetName(string sheetName)
        {
            int lineCharIndex = sheetName.IndexOf('-');
            int leftParenthesis = sheetName.IndexOf('(');
            int rightParenthesis = sheetName.IndexOf(')');
            int[] loopIndex = new int[2];
            string strLoopStartIndex = sheetName.Substring(leftParenthesis + 1, lineCharIndex-leftParenthesis-1);
            string strLoopEndIndex = sheetName.Substring(lineCharIndex + 1, rightParenthesis- lineCharIndex-1);
            if (strLoopStartIndex != "")
            { 
                loopIndex[0] = Convert.ToInt32(strLoopStartIndex);
            }
            if (strLoopEndIndex != "")
            {
                loopIndex[1] = Convert.ToInt32(strLoopEndIndex);
            }
            return loopIndex;
        }

        private DataTable GetOtherSettingData(IExcelService excelService,string filePath, string sheetName,int maxRowIndex,out bool sheetExistFlag)
        {
            
            Dictionary<int, int> summarySheetRowDefinition = new Dictionary<int, int>();                      
            summarySheetRowDefinition.Add(1,maxRowIndex + 2 - 1); //2为标题+表头,由于从0开始记数，需要减1        
            DataTable dt = new DataTable();
            dt = excelService.OpenExcel(filePath, sheetName, summarySheetRowDefinition,out sheetExistFlag);
            return dt;
        }
        /// <summary>
        /// 将读出的标准组态信息转换为LinkageConfigStandard类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(DataTable dt)
        {
            List<LinkageConfigStandard> lstStandardLinkage = new List<LinkageConfigStandard>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["输出组号"].ToString() == "")//无输出组号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                LinkageConfigStandard lcs = new LinkageConfigStandard();
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
            return lstStandardLinkage;
        }
        /// <summary>
        /// 将读出的混合组态信息转换为LinkageConfigMixed类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8001 config = new ControllerConfig8001();
            List<LinkageConfigMixed> lstMixedLinkage = new List<LinkageConfigMixed>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                LinkageConfigMixed lcm = new LinkageConfigMixed();
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
            return lstMixedLinkage;
        }
        /// <summary>
        /// 将读出的通用组态信息转换为LinkageConfigGeneral类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dt)
        {
            ControllerConfig8001 config = new ControllerConfig8001();
            List<LinkageConfigGeneral> lstGeneralLinkage = new List<LinkageConfigGeneral>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                LinkageConfigGeneral lcg = new LinkageConfigGeneral();
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
            return lstGeneralLinkage;
        }
        /// <summary>
        /// 将读出的网络手动盘信息转换为ManualControlBoard类型存储
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dt)
        {
            //ControllerConfig8001 config = new ControllerConfig8001();
            List<ManualControlBoard> lstManualControlBoard = new List<ManualControlBoard>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["编号"].ToString() == "") //无编号值，认为此工作表已不存在有效数据
                {
                    break;
                }
                ManualControlBoard mcb = new ManualControlBoard();
                mcb.Code = Convert.ToInt32(dt.Rows[i]["编号"].ToString().NullToZero());
                mcb.BoardNo = Convert.ToInt32(dt.Rows[i]["板卡号"].ToString().NullToZero());
                mcb.SubBoardNo = Convert.ToInt32(dt.Rows[i]["手盘号"].ToString().NullToZero());
                mcb.KeyNo = Convert.ToInt32(dt.Rows[i]["手键号"].ToString().NullToZero());
                mcb.DeviceCode = dt.Rows[i]["地编号"].ToString().NullToZero();
                lstManualControlBoard.Add(mcb);
            }
            return lstManualControlBoard;
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
        //private void SetDeviceID

    }
}
