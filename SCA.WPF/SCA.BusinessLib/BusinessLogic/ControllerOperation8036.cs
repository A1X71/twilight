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
                ControllerConfigBase configBase = new ControllerConfigBase();

                controllerInfo.Name = "OldVersion";
                controllerInfo.Project = null;
                controllerInfo.DeviceAddressLength = 8;
                controllerInfo.Type = ControllerType.NT8036;
                controllerInfo.PrimaryFlag = false;
                controllerInfo.LoopAddressLength = 2;
                
                List<LoopModel> lstLoopInfo = GetLoopInfoFromOldVersionSoftwareDataFile(oldDBService);
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
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8036 select r;
            int maxDeviceID = 0;
            foreach (var c in controllers)
            {
                foreach (var l in c.Loops)
                {
                    List<DeviceInfo8036> lstDeviceInfo= l.GetDevices<DeviceInfo8036>();
                    if (lstDeviceInfo.Count > 0)
                    {
                        int currentLoopMaxDeviceID = Convert.ToInt32(lstDeviceInfo.Max(device => device.ID));
                        if (currentLoopMaxDeviceID > maxDeviceID)
                        {
                            maxDeviceID = currentLoopMaxDeviceID;
                        }
                    }
                }
            }
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
    }
}
