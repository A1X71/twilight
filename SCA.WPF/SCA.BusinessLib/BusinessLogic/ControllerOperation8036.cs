using System;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface;
using SCA.BusinessLib.Utility;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2016/11/11 13:34:06
* FileName   : ControllerOperation8036
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8036 : ControllerOperationBase, IControllerOperation
    {
        private IDatabaseService _databaseService;
        private IDeviceService<DeviceInfo8036> _deviceService;
        public ControllerOperation8036()
        { 
        
        }
        public ControllerOperation8036(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _deviceService = new DeviceService8036();
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
            return loopService.CreateLoops<DeviceInfo8036>(loopsAmount, deviceAmount, controller, _deviceService);
            #region refactor at 2017-02-27
            //string strMachineNumber = controller.MachineNumber;
            
            //string strLoopLengthFormat = "#";
            //for (int i = 0; i < controller.LoopAddressLength; i++)
            //{
            //    strLoopLengthFormat += "0";
            //}
            //List<Model.LoopModel> lstLoopModel = new List<LoopModel>();                        
            //for (int i = 0; i < loopsAmount; i++) //创建回路
            //{
            //    LoopModel loop = new LoopModel();
            //    loop.Controller = controller;
            //    loop.Code = strMachineNumber + i.ToString(strLoopLengthFormat);
            //    loop.Name = strMachineNumber + i.ToString(strLoopLengthFormat);
            //    loop.DeviceAmount = deviceAmount;
            //    List<DeviceInfo8036> lstDevInfo = new List<DeviceInfo8036>();
            //    for (int j = 0; j < deviceAmount; j++) //创建器件
            //    {
            //        DeviceInfo8036 devInfo = new DeviceInfo8036();
            //        devInfo.Code = j.ToString("#000");
            //        devInfo.TypeCode = 9; //此处默认值可为各个控制器进行配置。
            //        devInfo.Disable = 0;
            //        lstDevInfo.Add(devInfo);                    
            //    }
            //    loop.SetDevices<DeviceInfo8036>(lstDevInfo);
            //    lstLoopModel.Add(loop);
            //}
            //return lstLoopModel;
            #endregion refactor at 2017-02-27
        }
        //未测试
        public bool SaveLoops(List<Model.LoopModel> lstLoopModel,ILoopDBService loopDBService,IDeviceDBService<DeviceInfo8036> deviceDBService)
        {
            try
            {
                //INSERT INTO "DeviceInfo8036" ("ID", "Code", "Disabled", "LinkageGroup1", "LinkageGroup2", "AlertValue", "ForcastValue", "DelayValue", "BuildingNo", "ZoneNo", "FloorNo", "RoomNo", "Location", "LoopID", "TypeID", "ROWID") VALUES (4, '00101004', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 9, 4);
                //创建器件
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

        public ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldDBService)
        {
             ControllerModel controllerInfo= new ControllerModel();
            try
            {
                ControllerConfigNone configBase = new ControllerConfigNone();

                controllerInfo.Name = "OldVersion";
                controllerInfo.Project = null;
                controllerInfo.DeviceAddressLength = 8;
                controllerInfo.Type = ControllerType.NT8036;
                controllerInfo.PrimaryFlag = false;
                controllerInfo.LoopAddressLength = 2;
                
                List<LoopModel> lstLoopInfo=null;// = GetLoopInfoFromOldVersionSoftwareDataFile(oldDBService);
                StringBuilder sbQuerySQL = new StringBuilder();
                foreach (var l in lstLoopInfo)//回路信息
                {
                    LoopModel loop = l;
                    #region comment
                    //sbQuerySQL.Clear();
                    //sbQuerySQL.Append("select bianhao,leixing,geli,shuchu1,shuchu2,nongdu,yjnongdu,yanshi,louhao,quhao,cenghao,fangjianhao,didian from " + loop.Code);                             
                    

                    //DataTable dtDevices=_databaseService.GetDataTableBySQL(sbQuerySQL);
                    //int dtDevicesRowsCount=dtDevices.Rows.Count;
                    
                    //for (int j = 0; j< dtDevicesRowsCount; j++) //器件信息
                    //{
                    //    DeviceInfo8036 device = new DeviceInfo8036();
                    //    device.Code    = dtDevices.Rows[j]["bianhao"].ToString();
                    //    device.TypeCode    = Convert.ToInt16(dtDevices.Rows[j]["leixing"].ToString().NullToZero());
                    //    device.Disable = Convert.ToInt16(dtDevices.Rows[j]["geli"].ToString().NullToZero());
                    //    device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                    //    device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                    //    device.AlertValue =float.Parse(dtDevices.Rows[j]["nongdu"].ToString().NullToZero());
                    //    device.ForcastValue =float.Parse(dtDevices.Rows[j]["yjnongdu"].ToString().NullToZero());
                    //    device.DelayValue=Int16.Parse( dtDevices.Rows[j]["yanshi"].ToString().NullToZero());
                    //    device.BuildingNo=Int16.Parse(dtDevices.Rows[j]["louhao"].ToString().NullToZero());                        
                    //    device.ZoneNo=Int16.Parse(dtDevices.Rows[j]["quhao"].ToString().NullToZero());
                    //    device.FloorNo=Int16.Parse(dtDevices.Rows[j]["cenghao"].ToString().NullToZero());
                    //    device.RoomNo=Int16.Parse(dtDevices.Rows[j]["fangjianhao"].ToString().NullToZero());
                    //    device.Location =dtDevices.Rows[j]["didian"].ToString();
                    //    device.Loop = loop;
                    //    loop.SetDevice<DeviceInfo8036>(device);
                    //}
                    #endregion
                    oldDBService.GetDevicesInLoop(ref loop,null);
                    //foreach (var device in lstDevices)
                    //{
                    //    device.Loop = loop;
                    //    loop.SetDevice<DeviceInfo8036>(device);
                    //}
                    loop.Controller = controllerInfo;
                    controllerInfo.Loops.Add(loop);
                }
                #region comment
                //sbQuerySQL.Clear();
                //sbQuerySQL = sbQuerySQL.Append("select 输出组号,编号1,编号2,编号3,编号4,动作常数,联动组1,联动组2,联动组3 from 器件组态;");                
                //DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                //int dtRowsCount=dt.Rows.Count;
                //for(int i=0;i<dtRowsCount;i++)
                //{
                //    LinkageConfigStandard linkageConfigStandard = new LinkageConfigStandard();
                //    linkageConfigStandard.Code=dt.Rows[i]["输出组号"].ToString();
                //    linkageConfigStandard.DeviceNo1=dt.Rows[i]["编号1"].ToString();
                //    linkageConfigStandard.DeviceNo2=dt.Rows[i]["编号2"].ToString();
                //    linkageConfigStandard.DeviceNo3=dt.Rows[i]["编号3"].ToString();
                //    linkageConfigStandard.DeviceNo4=dt.Rows[i]["编号4"].ToString();
                //    linkageConfigStandard.ActionCoefficient=Convert.ToInt32(dt.Rows[i]["动作常数"].ToString().NullToImpossibleValue());
                //    linkageConfigStandard.LinkageNo1=dt.Rows[i]["联动组1"].ToString();
                //    linkageConfigStandard.LinkageNo2=dt.Rows[i]["联动组2"].ToString();
                //    linkageConfigStandard.LinkageNo3 = dt.Rows[i]["联动组3"].ToString();
                //    linkageConfigStandard.Controller = controllerInfo;
                //    controllerInfo.StandardConfig.Add(linkageConfigStandard);
                //}
                #endregion
                List<LinkageConfigStandard> lstStandardLinkageConfig= oldDBService.GetStandardLinkageConfig();
                foreach (var config in lstStandardLinkageConfig)
                {
                    config.Controller = controllerInfo;
                    controllerInfo.StandardConfig.Add(config);
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


        public List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new NotImplementedException();
        }


        public bool CreateController(ControllerModel controller,IDeviceTypeDBService deviceTypeDBService,IControllerDBService controllerDBService)
        {
            try
            {
                #region 控制器配置
                ControllerConfig8036 config=new ControllerConfig8036();
                string strMatchingDevTypeID=config.GetDeviceTypeCodeInfo();
                #region comment 
                //StringBuilder sbDeviceTypeSQL = new StringBuilder("Select Code,Name,IsValid,ProjectID, MatchingController from DeviceType where Code in ("+strMatchingDevTypeID+");" );
                //List<Model.DeviceType> lstDeviceType=(List<Model.DeviceType>)_databaseService.GetDataListBySQL<DeviceType>(sbDeviceTypeSQL);
                //foreach (DeviceType devType in lstDeviceType)
                //{
                //    devType.MatchingController = devType.MatchingController == null ? "" : devType.MatchingController;
                     
                //    //如果MatchingController中不包含当前的控制器，则更新字段
                //    if (!devType.MatchingController.Contains(ControllerType.NT8036.ToString()))
                //    {
                //        devType.MatchingController = devType.MatchingController == "" ? devType.MatchingController : devType.MatchingController + ",";
                //        devType.MatchingController = devType.MatchingController  + ControllerType.NT8036;
                //        sbDeviceTypeSQL = new StringBuilder("Update DeviceType set MatchingController='" + devType.MatchingController + "' where Code=" + devType.Code + ";");
                //   //     _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                //    }                    
                //}
                #endregion
                deviceTypeDBService.UpdateMatchingController(ControllerType.NT8036, strMatchingDevTypeID); //refactor 2017-02-23
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
                controllerDBService.AddController(controller); //refactor at 2017-02-23
                return true;
            }
            catch(Exception ex )
            { 
                return false;
            }
        }
        //此方法的功能逻辑同CreateController,需要删除。测试后如无用，则删掉
        public bool SaveController(ControllerModel controller)
        {
            try
            {
                #region 控制器配置
                ControllerConfig8036 config = new ControllerConfig8036();
                string strMatchingDevTypeID = config.GetDeviceTypeCodeInfo();
                StringBuilder sbDeviceTypeSQL = new StringBuilder("Select Code,Name,IsValid,ProjectID, MatchingController from DeviceType where Code in (" + strMatchingDevTypeID + ");");
                List<Model.DeviceType> lstDeviceType = (List<Model.DeviceType>)_databaseService.GetDataListBySQL<DeviceType>(sbDeviceTypeSQL);
                foreach (DeviceType devType in lstDeviceType)
                {
                    devType.MatchingController = devType.MatchingController == null ? "" : devType.MatchingController;

                    //如果MatchingController中不包含当前的控制器，则更新字段
                    if (!devType.MatchingController.Contains(ControllerType.NT8036.ToString()))
                    {
                        devType.MatchingController = devType.MatchingController == "" ? devType.MatchingController : devType.MatchingController + ",";
                        devType.MatchingController = devType.MatchingController + ControllerType.NT8036;
                        sbDeviceTypeSQL = new StringBuilder("Update DeviceType set MatchingController='" + devType.MatchingController + "' where Code=" + devType.Code + ";");
                        _databaseService.ExecuteBySql(sbDeviceTypeSQL);
                    }

                }
                #endregion
                #region 增加控制器信息
                //版本号怎么计，是按原版本号累加，还是初始化一个新的版本号
                //当前为初始化一个新的版本号
                StringBuilder sbControllerSQL = new StringBuilder("Insert into Controller(ID,PrimaryFlag,TypeID,DeviceAddressLength,Name,PortName,BaudRate,MachineNumber,Version,ProjectID) values(");
                sbControllerSQL.Append(controller.ID + ",'");
                sbControllerSQL.Append(controller.PrimaryFlag + "',");//+ "',0);");
                sbControllerSQL.Append((int)controller.Type + ",");
                sbControllerSQL.Append(controller.DeviceAddressLength + ",'");
                sbControllerSQL.Append(controller.Name + "','");
                sbControllerSQL.Append(controller.PortName + "',");
                sbControllerSQL.Append(controller.BaudRate + ",'");
                sbControllerSQL.Append(controller.MachineNumber + "',");
                sbControllerSQL.Append(controller.Version + ",");
                sbControllerSQL.Append(controller.Project.ID + ")");
                _databaseService.ExecuteBySql(sbControllerSQL);

                #endregion
 
                return true;
            }
            catch (Exception ex)
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

        
        //public bool AddControllerToProject(ControllerModel controller)
        //{
        //    try
        //    {
        //        SCA.Model.ProjectModel project = SCA.BusinessLib.ProjectManager.GetInstance.Project;
                
        //        if (project.Controllers.Count == 0)//如果还未设置主控制器，则默认第一个控制器为主控制器
        //        {
        //            controller.PrimaryFlag = true;
        //        }
        //        int maxControllerID=GetMaxControllerID();
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


 
        public int GetMaxDeviceID()
        {
            //var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8036 select r;
            //int maxDeviceID = 0;
            //foreach (var c in controllers)
            //{
            //    foreach (var l in c.Loops)
            //    {
            //        List<DeviceInfo8036> lstDeviceInfo= l.GetDevices<DeviceInfo8036>();
            //        if (lstDeviceInfo.Count > 0)
            //        {
            //            int currentLoopMaxDeviceID = Convert.ToInt32(lstDeviceInfo.Max(device => device.ID));
            //            if (currentLoopMaxDeviceID > maxDeviceID)
            //            {
            //                maxDeviceID = currentLoopMaxDeviceID;
            //            }
            //        }
            //    }
            //}
            int maxDeviceID = ProjectManager.GetInstance.MaxDeviceIDInController8036;
            return maxDeviceID;
        }

        protected override List<short?> GetAllBuildingNoWithController(ControllerModel controller)
        {
            List<short?> lstBuildingNo = new List<short?>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8036>())
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
                foreach (var dev in l.GetDevices<DeviceInfo8036>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8036 config = new ControllerConfig8036();
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

         //   var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8036 select r;
            List<DeviceInfo8036> lstDeviceInfo = new List<DeviceInfo8036>();
           // foreach (var c in controllers)
           // {
                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8036>())
                    {
                        lstDeviceInfo.Add(d);
                    }
                }
         //   }

            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.ControllerName = controller.Name;
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
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

        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo, ControllerType controllerType)
        {
            try
            {
                DownLoadDefaultExcelTemplate(strFilePath, fileService, customizedInfo, ControllerType.NT8036);
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }   
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
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 2, "0", CellStyleType.Data);
                            excelService.SetCellValue(sheetNames[i], j * maxDeviceAmount + k + (j * extraLine + 2), 3, null, CellStyleType.Data); //屏蔽
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
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 5, "动作常数", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 6, "联动组1", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 7, "联动组2", CellStyleType.TableHead);
                excelService.SetCellValue(sheetNames[loopSheetAmount + currentIndex], 1, 8, "联动组3", CellStyleType.TableHead);
                MergeCellRange mergeCellRange = new MergeCellRange();
                mergeCellRange.FirstRowIndex = 0;
                mergeCellRange.LastRowIndex = 0;
                mergeCellRange.FirstColumnIndex = 0;
                mergeCellRange.LastColumnIndex = 8;
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

        protected override bool ExistSameDeviceCode(LoopModel loop)
        {
            DeviceService8036 deviceService = new DeviceService8036();
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
            ControllerConfig8036 config = new ControllerConfig8036();
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
                DeviceInfo8036 device = new DeviceInfo8036();
                maxDeviceID++;
                device.ID = maxDeviceID;
                device.Code = dt.Rows[i]["编码"].ToString();
                device.TypeCode = config.GetDeviceCodeViaDeviceTypeName(dt.Rows[i]["器件类型"].ToString());   
                device.Disable = new Nullable<bool>(Convert.ToBoolean(dt.Rows[i]["屏蔽"].ToString() == "0" ? false : true));
          
                device.LinkageGroup1 = dt.Rows[i]["输出组1"].ToString();
                device.LinkageGroup2 = dt.Rows[i]["输出组2"].ToString();
                if (dt.Rows[i]["报警浓度"].ToString() == "")
                {
                    device.AlertValue = null;
                }
                else
                {
                    device.AlertValue = new Nullable<float>(Convert.ToSingle(dt.Rows[i]["报警浓度"].ToString()));
                }
                if (dt.Rows[i]["预警浓度"].ToString() == "")
                {
                    device.AlertValue = null;
                }
                else
                {
                    device.AlertValue = new Nullable<float>(Convert.ToSingle(dt.Rows[i]["预警浓度"].ToString()));
                }
                
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
                    //在合适位置设置
                    //device.LoopID
                    loop.SetDevice<DeviceInfo8036>(device);
                    loop.DeviceAmount++;
                }
            }
            //更新最大器件ID
            ProjectManager.GetInstance.MaxDeviceIDInController8036 = maxDeviceID;
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

        protected override List<LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dtMixed)
        {
            return null;
        }

        protected override List<LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dtGeneral)
        {
            return null;
        }

        protected override List<ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dtManualControlBoard)
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
            ControllerConfig8036 config = new ControllerConfig8036();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8036> lstDistinceInfo = loop.GetDevices<DeviceInfo8036>().Distinct(new CollectionEqualityComparer<DeviceInfo8036>((x, y) => x.TypeCode == y.TypeCode)).ToList();

                        int deviceCountInLoop = loop.GetDevices<DeviceInfo8036>().Count;
                        //int deviceCountInStatistic = 0;
                        foreach (var device in lstDistinceInfo)
                        {
                            DeviceType dType = config.GetDeviceTypeViaDeviceCode(device.TypeCode);
                            int typeCount = loop.GetDevices<DeviceInfo8036>().Count((d) => d.TypeCode == dType.Code); //记录器件类型的数量
                            if (!dictDeviceTypeStatistic.ContainsKey(dType.Name))
                            {
                                dictDeviceTypeStatistic.Add(dType.Name, typeCount);
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
            ControllerConfig8036 config = new ControllerConfig8036();
            if (controller != null)
            {
                if (controller.Loops != null)
                {
                    foreach (var loop in controller.Loops)
                    {
                        IEnumerable<DeviceInfo8036> lstDistinceInfo = loop.GetDevices<DeviceInfo8036>().Distinct(new CollectionEqualityComparer<DeviceInfo8036>((x, y) => x.TypeCode == y.TypeCode)).ToList();
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


        public ControllerModel OrganizeControllerInfoFromSpecifiedDBFileVersion(IDBFileVersionService dbFileVersionService)
        {
            throw new NotImplementedException();
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
                dbFileVersionService.GetDevicesByLoopForControllerType8036(ref loop);//为loop赋予“器件信息”
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
    }
}
