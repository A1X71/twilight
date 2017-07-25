using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.Utility;
using SCA.Interface;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 13:43:14
* FileName   : DBFileVersion6Service
* Description:版本6数据文件操作服务
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    /// <summary>
    /// 版本6数据文件操作服务
    /// 与版本5相比,8001控制器增加特性列
    /// </summary>
    public class DBFileVersion6DBService : DBFileVersionBaseService, IDBFileVersionService
    {
        private IDatabaseService _databaseService;   
        public int DBFileVersion
        {
            get { return 6; }
        }

        public string DBFilePath
        {
            get;
            private set;
        }
        public DBFileVersion6DBService(string dataSource, ILogRecorder logger, IFileService fileService)
        {
            _databaseService = new SCA.DatabaseAccess.MSAccessDatabaseAccess(dataSource, logger, fileService);
            DBFilePath = dataSource;
        }
        public bool CreateTableForProject()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForControllerType()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForDeFireSystemCategory()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForDeviceType()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForController()
        {
            throw new NotImplementedException();
        }

        //public bool CreateTableForControllerAttachedInfo()
        //{
        //    throw new NotImplementedException();
        //}

        public bool CreateTableForLinkageConfigStandard()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForLinkageConfigGeneral()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForLinkageconfigMixed()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForManualControlBoard()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForLoop()
        {
            throw new NotImplementedException();
        }

        public List<LoopModel> GetLoopsInfo()
        {
            DataTable dt = null;
            List<LoopModel> lstLoopInfo = new List<LoopModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select 回路,总数 from 系统设置 order by 回路;");
            dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            int dtRowsCount = dt.Rows.Count;
            for (int i = 0; i < dtRowsCount; i++)//回路信息
            {
                LoopModel loop = new LoopModel();
                loop.Name = dt.Rows[i]["回路"].ToString();
                loop.Code = dt.Rows[i]["回路"].ToString();
                loop.DeviceAmount = Convert.ToInt16(dt.Rows[i]["总数"] == null ? 0 : dt.Rows[i]["总数"]);
                lstLoopInfo.Add(loop);
            }
            return lstLoopInfo;
        }

        public string[] GetFileVersionAndControllerType()
        {
            //1.读"文件配置"表，取得（文件版本，控制器类型）
            //2.根据控制器类型，初始化控制器配置信息，取得相应“文件版本”至目的版本之间需要执行升级的操作内容
            StringBuilder sbQuerySQL = new StringBuilder("select 文件版本,控制器类型 from 文件配置;");
            DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            string strResult = "";
            if (dtFile != null)
            {
                if (dtFile.Rows.Count > 0)
                {
                    strResult = dtFile.Rows[0]["控制器类型"].ToString() + ";" + dtFile.Rows[0]["文件版本"].ToString();
                }
            }
            return strResult.Split(';');
        }
        public ControllerAttachedInfo GetAttachedInfo()
        {
            StringBuilder sbQuerySQL = new StringBuilder("select 文件版本,控制器类型,协议版本,位置,器件长度 from 文件配置 left join 位置信息 on 文件配置.文件版本=位置信息.位置;");
            DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            ControllerAttachedInfo controllerAttachedInfo = null;
            if (dtFile != null)
            {
                if (dtFile.Rows.Count > 0)
                {
                    controllerAttachedInfo = new ControllerAttachedInfo();
                    controllerAttachedInfo.FileVersion = dtFile.Rows[0]["文件版本"].ToString();
                    controllerAttachedInfo.ControllerType = dtFile.Rows[0]["控制器类型"].ToString();
                    controllerAttachedInfo.ProtocolVersion = dtFile.Rows[0]["协议版本"].ToString();
                    controllerAttachedInfo.Position = dtFile.Rows[0]["位置"].ToString();
                    controllerAttachedInfo.DeviceAddressLength = dtFile.Rows[0]["器件长度"].ToString();
                }
            }
            return controllerAttachedInfo;
        }


  

 

        public List<ManualControlBoard> GetManualControlBoard(ControllerModel controller)
        {
            List<ManualControlBoard> lstManualControlBoard = new List<ManualControlBoard>();
            try
            {
                StringBuilder sbQuerySQL = new StringBuilder();
                //set sdpkey=xianggh,xianggh= (Round((xianggh/756)+.4999999)-1),panhao=IIF(Round(((xianggh/63))+.4999999)>12,Round(((xianggh/63))+.4999999)-12,Round(((xianggh/63))+.4999999)),jianhao=IIF((xianggh Mod 63)=0,63,xianggh Mod 63)
                //(5)网络手控盘            
                sbQuerySQL = sbQuerySQL.Append("select 编号,板卡号,盘号,键号,地编号,sdpkey from 网络手控盘;");

                DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                int dtRowsCount = dt.Rows.Count;
                for (int i = 0; i < dtRowsCount; i++)
                {
                    ManualControlBoard manualControlBoard = new ManualControlBoard();
                    manualControlBoard.Code = Convert.ToInt32(dt.Rows[i]["编号"]);
                    manualControlBoard.BoardNo = Convert.ToInt32(dt.Rows[i]["板卡号"]);
                    manualControlBoard.SubBoardNo = Convert.ToInt32(dt.Rows[i]["盘号"]);
                    manualControlBoard.KeyNo = Convert.ToInt32(dt.Rows[i]["键号"]);
                    manualControlBoard.DeviceCode = dt.Rows[i]["地编号"].ToString();
                    manualControlBoard.SDPKey = dt.Rows[i]["sdpkey"].ToString();
                    lstManualControlBoard.Add(manualControlBoard);
                }
            }
            catch
            {

            }
            return lstManualControlBoard;
        }

        public bool CreateTableForDeviceInfoOfControllerType8000()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8000(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8000(ref LoopModel loop)
        {
            List<DeviceInfo8000> lstDeviceInfo8000 = new List<DeviceInfo8000>();

            string strDeviceQuerySQL = "select bianhao,leixing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,zjczbh,GbZone,didian from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;

            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8000 device = new DeviceInfo8000();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                device.SensitiveLevel = dtDevices.Rows[j]["lingmd"].ToString().ToNullable<Int16>();
                device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                device.LinkageGroup3 = dtDevices.Rows[j]["shuchu3"].ToString();
                device.DelayValue = dtDevices.Rows[j]["yanshi"].ToString().ToNullable<Int16>();
                device.sdpKey = dtDevices.Rows[j]["xianggh"].ToString();
                device.ZoneNo = dtDevices.Rows[j]["zjczbh"].ToString().ToNullable<Int16>();
                device.BroadcastZone = dtDevices.Rows[j]["GbZone"].ToString();
                device.Location = dtDevices.Rows[j]["didian"].ToString();
                device.Loop = loop;
                loop.SetDevice<DeviceInfo8000>(device);
                lstDeviceInfo8000.Add(device);
            }
            return true;
        }

        public bool DeleteAllDevicesByControllerIDForControllerType8000(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDeviceByIDForControllerType8000(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8000()
        {
            throw new NotImplementedException();
        }

        public bool GetControllerInfo(ref ControllerModel controllerInfo)
        {
            try
            {
                string fileName = base.GetFileName(DBFilePath);
                if (fileName != "")
                {
                    controllerInfo.Name = fileName.Length > 10 ? fileName.Substring(0, 10) : fileName;
                }
                else
                {
                    controllerInfo.Name = "OldVersion";
                }
                controllerInfo.Project = null;                
                controllerInfo.PrimaryFlag = false;
                controllerInfo.LoopAddressLength = 2;
                controllerInfo.DeviceAddressLength = GetDeviceAddressLength();
                return true;
            }
            catch
            {
                return false;
            }   
        }

        public bool CreateTableForDeviceInfoOfControllerType8001()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8001(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8001(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            List<DeviceInfo8001> lstDeviceInfo8001 = new List<DeviceInfo8001>();
            string strDeviceQuerySQL = "select bianhao,leixing,texing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,panhao,jianhao,cleixing,GbZone,didian,louhao,quhao,cenghao,fangjianhao,sdpkey from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;
            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8001 device = new DeviceInfo8001();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                device.SensitiveLevel = dtDevices.Rows[j]["lingmd"].ToString().ToNullable<Int16>();
                device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                device.LinkageGroup3 = dtDevices.Rows[j]["shuchu3"].ToString();
                device.DelayValue = dtDevices.Rows[j]["yanshi"].ToString().ToNullable<Int16>();
                device.BoardNo = dtDevices.Rows[j]["xianggh"].ToString().ToNullable<Int16>();
                device.SubBoardNo = dtDevices.Rows[j]["panhao"].ToString().ToNullable<Int16>();
                device.KeyNo = dtDevices.Rows[j]["jianhao"].ToString().ToNullable<Int16>();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.Feature = dtDevices.Rows[j]["texing"].ToString().ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                //不存储cleixing
                //if (Version > 5) //从第6版开始增加了"特性"列 
                //{
                //    device.Feature = dtDevices.Rows[j]["texing"].ToString().ToNullable<Int16>();
                //    device.TypeCode = (short)intTypeCode;
                //}
                //else
                //{
                //    // 特性字段值 更新依据：
                //    // 1>"器件类型编号">36且<66-->特性字段为0
                //    // 2>"器件类型编号">=101且<=129-->特性字段为1
                //    // 器件编码更新依据：
                //    // 1> "器件类型编号">=101且<=129　--> 更新为"器件类型编号"-64
                //    // 2> cleixing字段为中文名称，需要去掉“自锁”及“点动”前缀（由于本软件中不存储中文名称，故不需更新）                            
                //    if (intTypeCode > 36 && intTypeCode < 66)
                //    {
                //        device.Feature = 0;
                //        device.TypeCode = (short)intTypeCode;
                //    }
                //    else if (intTypeCode >= 101 && intTypeCode <= 129)
                //    {
                //        device.TypeCode = Convert.ToInt16(intTypeCode - 64);
                //        device.Feature = 1;
                //    }
                //    else
                //    {
                //        device.TypeCode = (short)intTypeCode;

                //    }
                //}
                device.BroadcastZone = dtDevices.Rows[j]["GbZone"].ToString();
                device.Location = dtDevices.Rows[j]["didian"].ToString();

                device.BuildingNo = dtDevices.Rows[j]["louhao"].ToString().ToNullable<Int16>();
                device.ZoneNo = dtDevices.Rows[j]["quhao"].ToString().ToNullable<Int16>();
                device.FloorNo = dtDevices.Rows[j]["cenghao"].ToString().ToNullable<Int16>();
                device.RoomNo = dtDevices.Rows[j]["fangjianhao"].ToString().ToNullable<Int16>();
                device.sdpKey = dtDevices.Rows[j]["sdpkey"].ToString();
                device.Loop = loop;
                if (dictDeviceMappingManualControlBoard != null)
                {
                    if (dictDeviceMappingManualControlBoard.ContainsKey(device.Code))//如果存在网络手动盘的信息定义，则设置MCBCode的值，建立与手动盘的关系
                    {
                        device.MCBCode = dictDeviceMappingManualControlBoard[device.Code];
                    }
                }
                //if(lstDeviceInfo8001.Count>0)
                //{ 
                //    _deviceAddressLength = lstDeviceInfo8001[0].Code.Length;
                //}
                loop.SetDevice<DeviceInfo8001>(device);
                lstDeviceInfo8001.Add(device);
            }
            return true;
        }

        public bool DeleteAllDevicesByControllerIDForControllerType8001(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDeviceByIDForControllerType8001(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8001()
        {
            throw new NotImplementedException();
        }

        public bool CreateTableForDeviceInfoOfControllerType8003()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8003(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8003(ref LoopModel loop)
        {
            List<DeviceInfo8003> lstDeviceInfo8003 = new List<DeviceInfo8003>();

            string strDeviceQuerySQL = "select bianhao,leixing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,zjczbh,GbZone,didian from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;

            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8003 device = new DeviceInfo8003();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                device.SensitiveLevel = dtDevices.Rows[j]["lingmd"].ToString().ToNullable<Int16>();
                device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                device.LinkageGroup3 = dtDevices.Rows[j]["shuchu3"].ToString();
                device.DelayValue = dtDevices.Rows[j]["yanshi"].ToString().ToNullable<Int16>();
                device.sdpKey = dtDevices.Rows[j]["xianggh"].ToString();
                device.ZoneNo = dtDevices.Rows[j]["zjczbh"].ToString().ToNullable<Int16>();
                device.BroadcastZone = dtDevices.Rows[j]["GbZone"].ToString();
                device.Location = dtDevices.Rows[j]["didian"].ToString();
                device.Loop = loop;
                loop.SetDevice<DeviceInfo8003>(device);
                lstDeviceInfo8003.Add(device);
            }
            return true;
        }

        public bool DeleteAllDevicesByControllerIDForControllerType8003(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDeviceByIDForControllerType8003(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8003()
        {
            throw new NotImplementedException();
        }

        public bool CreateDeviceInfoOfControllerType8021Table()
        {
            throw new NotImplementedException();
        }

        public bool CreateDeviceInfoOfControllerType8036Table()
        {
            throw new NotImplementedException();
        }

        public bool CreateDeviceInfoOfControllerType8007Table()
        {
            throw new NotImplementedException();
        }
        private int GetDeviceAddressLength()
        {
            //1.读"文件配置"表，取得（文件版本，控制器类型）
            //2.根据控制器类型，初始化控制器配置信息，取得相应“文件版本”至目的版本之间需要执行升级的操作内容
            StringBuilder sbQuerySQL = new StringBuilder("select  器件长度 from 文件配置;");
            DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            string strResult = "";
            if (dtFile != null)
            {
                if (dtFile.Rows.Count > 0)
                {
                    strResult = dtFile.Rows[0]["器件长度"].ToString();
                }
            }
            return strResult == "" ? 0 : Convert.ToInt32(strResult);
        }


        public List<string> GetTablesOfDB()
        {
            throw new NotImplementedException();
        }


        public bool CreateLocalDBFile()
        {
            throw new NotImplementedException();
        }


        public int GetAmountOfControllerType()
        {
            throw new NotImplementedException();
        }

        public bool AddControllerTypeInfo()
        {
            throw new NotImplementedException();
        }
        public int GetFileVersion()
        {
            int fileVersion = 0;
            StringBuilder sbQuerySQL = new StringBuilder("select 文件版本 from 文件配置;");
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            if (dt.Rows.Count > 0)
            {
                fileVersion = Convert.ToInt32(dt.Rows[0]["文件版本"].ToString());
            }
            return fileVersion;
        }

        public ProjectModel GetProject(int id)
        {
            ProjectModel project = new ProjectModel();
            project.FileVersion = 6;
            return project;
        }

        public int AddProject(ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public DataTable OpenProject()
        {
            throw new NotImplementedException();
        }

        public int GetMaxIDFromProject()
        {
            throw new NotImplementedException();
        }

        public List<LinkageConfigStandard> GetStandardLinkageConfig(ControllerModel controller)
        {
            List<LinkageConfigStandard> lstStandard = new List<LinkageConfigStandard>();
            try
            {
                StringBuilder sbQuerySQL = new StringBuilder("select 输出组号,编号1,编号2,编号3,编号4,编号5,编号6,编号7,编号8,动作常数,联动组1,联动组2,联动组3 from 器件组态;");
                DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                int dtRowsCount = dt.Rows.Count;
                for (int i = 0; i < dtRowsCount; i++)
                {
                    LinkageConfigStandard linkageConfigStandard = new LinkageConfigStandard();
                    linkageConfigStandard.Code = dt.Rows[i]["输出组号"].ToString();
                    linkageConfigStandard.DeviceNo1 = dt.Rows[i]["编号1"].ToString();
                    linkageConfigStandard.DeviceNo2 = dt.Rows[i]["编号2"].ToString();
                    linkageConfigStandard.DeviceNo3 = dt.Rows[i]["编号3"].ToString();
                    linkageConfigStandard.DeviceNo4 = dt.Rows[i]["编号4"].ToString();
                    linkageConfigStandard.DeviceNo5 = dt.Rows[i]["编号5"].ToString();
                    linkageConfigStandard.DeviceNo6 = dt.Rows[i]["编号6"].ToString();
                    linkageConfigStandard.DeviceNo7 = dt.Rows[i]["编号7"].ToString();
                    linkageConfigStandard.DeviceNo8 = dt.Rows[i]["编号8"].ToString();
                    linkageConfigStandard.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"].ToString());
                    linkageConfigStandard.LinkageNo1 = dt.Rows[i]["联动组1"].ToString();
                    linkageConfigStandard.LinkageNo2 = dt.Rows[i]["联动组2"].ToString();
                    linkageConfigStandard.LinkageNo3 = dt.Rows[i]["联动组3"].ToString();
                    lstStandard.Add(linkageConfigStandard);
                }
            }
            catch
            {

            }
            return lstStandard;
        }

        public int AddStandardLinkageConfigInfo(LinkageConfigStandard linkageConfigStandard)
        {
            throw new NotImplementedException();
        }

        public List<LinkageConfigMixed> GetMixedLinkageConfig(ControllerModel controller)
        {

            List<LinkageConfigMixed> lstMixedConfig = new List<LinkageConfigMixed>();
            try
            {

                StringBuilder sbQuerySQL = new StringBuilder("select 编号,动作常数,动作类型,分类A,楼号A,区号A,层号A,类型A,分类B,楼号B,区号B,层号B,类型B,分类C,楼号C,区号C,层号C,类型C from 混合组态;");
                DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                int dtRowsCount = dt.Rows.Count;
                for (int i = 0; i < dtRowsCount; i++)
                {
                    LinkageConfigMixed linkageConfigMixed = new LinkageConfigMixed();
                    linkageConfigMixed.Code = dt.Rows[i]["编号"].ToString();
                    linkageConfigMixed.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"]);
                    linkageConfigMixed.ActionType = ConvertLinkageActionType(dt.Rows[i]["动作类型"].ToString());

                    linkageConfigMixed.TypeA = ConvertLinkageType(dt.Rows[i]["分类A"].ToString());
                    if (linkageConfigMixed.TypeA == LinkageType.ZoneLayer)
                    {
                        linkageConfigMixed.BuildingNoA = dt.Rows[i]["楼号A"].ToString().ToNullable<int>();
                        linkageConfigMixed.ZoneNoA = dt.Rows[i]["区号A"].ToString().ToNullable<int>();
                        linkageConfigMixed.LayerNoA = dt.Rows[i]["层号A"].ToString().ToNullable<int>();
                        //linkageConfigMixed.DeviceTypeCodeA = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型A"].ToString());
                        linkageConfigMixed.DeviceTypeNameA = dt.Rows[i]["类型A"].ToString();

                    }
                    else if (linkageConfigMixed.TypeA == LinkageType.Address)
                    {
                        linkageConfigMixed.LoopNoA = dt.Rows[i]["区号A"].ToString();
                        linkageConfigMixed.DeviceCodeA = dt.Rows[i]["层号A"].ToString();
                    }

                    linkageConfigMixed.TypeB = ConvertLinkageType(dt.Rows[i]["分类B"].ToString());
                    if (linkageConfigMixed.TypeB == LinkageType.ZoneLayer)
                    {

                        linkageConfigMixed.BuildingNoB = dt.Rows[i]["楼号B"].ToString().ToNullable<int>();
                        linkageConfigMixed.ZoneNoB = dt.Rows[i]["区号B"].ToString().ToNullable<int>();
                        linkageConfigMixed.LayerNoB = dt.Rows[i]["层号B"].ToString().ToNullable<int>();
                        //linkageConfigMixed.DeviceTypeCodeB = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型B"].ToString());
                        linkageConfigMixed.DeviceTypeNameB = dt.Rows[i]["类型B"].ToString();
                    }
                    else
                    {
                        linkageConfigMixed.LoopNoB = dt.Rows[i]["区号B"].ToString();
                        linkageConfigMixed.DeviceCodeB = dt.Rows[i]["层号B"].ToString();
                    }

                    linkageConfigMixed.TypeC = ConvertLinkageType(dt.Rows[i]["分类C"].ToString());
                    if (linkageConfigMixed.TypeC == LinkageType.ZoneLayer)
                    {
                        linkageConfigMixed.BuildingNoC = dt.Rows[i]["楼号C"].ToString().ToNullable<int>();
                        linkageConfigMixed.ZoneNoC = dt.Rows[i]["区号C"].ToString().ToNullable<int>();
                        linkageConfigMixed.LayerNoC = dt.Rows[i]["层号C"].ToString().ToNullable<int>();
                        //linkageConfigMixed.DeviceTypeCodeC = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型C"].ToString());
                        linkageConfigMixed.DeviceTypeNameC = dt.Rows[i]["类型C"].ToString();
                    }
                    else
                    {
                        linkageConfigMixed.MachineNoC = dt.Rows[i]["楼号C"].ToString();
                        linkageConfigMixed.LoopNoC = dt.Rows[i]["区号C"].ToString();
                        linkageConfigMixed.DeviceCodeC = dt.Rows[i]["层号C"].ToString();
                    }
                    lstMixedConfig.Add(linkageConfigMixed);
                }
            }
            catch
            {

            }
            return lstMixedConfig;
        }

        public int AddMixedLinkageConfigInfo(LinkageConfigMixed linkageConfigMixed)
        {
            throw new NotImplementedException();
        }

        public List<LinkageConfigGeneral> GetGeneralLinkageConfig(ControllerModel controller)
        {
            List<LinkageConfigGeneral> lstGeneral = new List<LinkageConfigGeneral>();
            try
            {
                StringBuilder sbQuerySQL = new StringBuilder("select 编号,动作常数,楼号A,区号A,层号A1,层号A2,类型A,分类C,楼号C,区号C,层号C,类型C from 通用组态;");
                DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                int dtRowsCount = dt.Rows.Count;
                for (int i = 0; i < dtRowsCount; i++)
                {
                    LinkageConfigGeneral linkageConfigGeneral = new LinkageConfigGeneral();
                    linkageConfigGeneral.Code = dt.Rows[i]["编号"].ToString();
                    linkageConfigGeneral.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["动作常数"]);
                    linkageConfigGeneral.BuildingNoA = dt.Rows[i]["楼号A"].ToString().ToNullable<int>();
                    linkageConfigGeneral.ZoneNoA = dt.Rows[i]["区号A"].ToString().ToNullable<int>();
                    linkageConfigGeneral.LayerNoA1 = dt.Rows[i]["层号A1"].ToString().ToNullable<int>();
                    linkageConfigGeneral.LayerNoA2 = dt.Rows[i]["层号A2"].ToString().ToNullable<int>();

                    //linkageConfigGeneral.DeviceTypeCodeA = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型A"].ToString());
                    linkageConfigGeneral.DeviceTypeNameA = dt.Rows[i]["类型A"].ToString();
                    //Int16 intTypeCodeC = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型C"].ToString());
                    string typeNameC = dt.Rows[i]["类型C"].ToString();
                    linkageConfigGeneral.TypeC = ConvertLinkageType(dt.Rows[i]["分类C"].ToString());
                    if (linkageConfigGeneral.TypeC == LinkageType.ZoneLayer)
                    {
                        linkageConfigGeneral.BuildingNoC = dt.Rows[i]["楼号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.ZoneNoC = dt.Rows[i]["区号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.LayerNoC = dt.Rows[i]["层号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.DeviceTypeNameC = typeNameC;
                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.Address)
                    {
                        linkageConfigGeneral.MachineNoC = dt.Rows[i]["楼号C"].ToString();
                        linkageConfigGeneral.LoopNoC = dt.Rows[i]["区号C"].ToString();
                        linkageConfigGeneral.DeviceCodeC = dt.Rows[i]["层号C"].ToString();

                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.SameLayer)
                    {
                        linkageConfigGeneral.DeviceTypeNameC = typeNameC;
                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.AdjacentLayer)
                    {
                        linkageConfigGeneral.DeviceTypeNameC = typeNameC;
                    }
                    lstGeneral.Add(linkageConfigGeneral);
                }
            }
            catch
            {

            }
            return lstGeneral;   
        }

        public int AddGeneralLinkageConfigInfo(LinkageConfigGeneral linkageConfigGeneral)
        {
            throw new NotImplementedException();
        }

        public int AddManualControlBoardInfo(ManualControlBoard manualControlBoard)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8000(DeviceInfo8000 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public LoopModel GetDevicesByLoopForControllerType8000(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteAllDevicesByControllerIDForControllerType8000(int id)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteDeviceByIDForControllerType8000(int id)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8001(DeviceInfo8001 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public LoopModel GetDevicesByLoopForControllerType8001(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteAllDevicesByControllerIDForControllerType8001(int id)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteDeviceByIDForControllerType8001(int id)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8003(DeviceInfo8003 device)
        {
            throw new NotImplementedException();
        }

        public LoopModel GetDevicesByLoopForControllerType8003(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteAllDevicesByControllerIDForControllerType8003(int id)
        {
            throw new NotImplementedException();
        }

        int IDBFileVersionService.DeleteDeviceByIDForControllerType8003(int id)
        {
            throw new NotImplementedException();
        }



        public bool CreateTableForDeviceInfoOfControllerType8007()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8007(LoopModel loop)
        {
            List<DeviceInfo8007> lstDeviceInfo8007 = new List<DeviceInfo8007>();
            string strDeviceQuerySQL = "select bianhao,leixing,texing,geli,lingmd,shuchu1,shuchu2,louhao,quhao,cenghao,fangjianhao,didian from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;
            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8007 device = new DeviceInfo8007();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                //device.Feature = dtDevices.Rows[j]["texing"].ToString();
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                device.SensitiveLevel = dtDevices.Rows[j]["lingmd"].ToString().ToNullable<Int16>();
                device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                device.BuildingNo = dtDevices.Rows[j]["louhao"].ToString().ToNullable<Int16>();
                device.ZoneNo = dtDevices.Rows[j]["quhao"].ToString().ToNullable<Int16>();
                device.FloorNo = dtDevices.Rows[j]["cenghao"].ToString().ToNullable<Int16>();
                device.RoomNo = dtDevices.Rows[j]["fangjianhao"].ToString().ToNullable<Int16>();
                device.Location = dtDevices.Rows[j]["didian"].ToString();
                device.Loop = loop;
                loop.SetDevice<DeviceInfo8007>(device);
                lstDeviceInfo8007.Add(device);
            }
            return true;
        }

        public int AddDeviceForControllerType8007(DeviceInfo8007 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8007(ref LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public LoopModel GetDevicesByLoopForControllerType8007(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int DeleteAllDevicesByControllerIDForControllerType8007(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteDeviceByIDForControllerType8007(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8007()
        {
            throw new NotImplementedException();
        }



        public bool CreateTableForDeviceInfoOfControllerType8021()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8021(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8021(DeviceInfo8021 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8021(ref LoopModel loop)
        {
            List<DeviceInfo8021> lstDeviceInfo8021 = new List<DeviceInfo8021>();
            string strDeviceQuerySQL = "select bianhao,leixing,geli,didian,louhao,quhao,cenghao,fangjianhao,dlbaojing,wdbaojing from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;
            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8021 device = new DeviceInfo8021();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                device.Location = dtDevices.Rows[j]["didian"].ToString();
                device.BuildingNo = dtDevices.Rows[j]["louhao"].ToString().ToNullable<Int16>();
                device.ZoneNo = dtDevices.Rows[j]["quhao"].ToString().ToNullable<Int16>();
                device.FloorNo = dtDevices.Rows[j]["cenghao"].ToString().ToNullable<Int16>();
                device.RoomNo = dtDevices.Rows[j]["fangjianhao"].ToString().ToNullable<Int16>();
                device.CurrentThreshold = dtDevices.Rows[j]["dlbaojing"].ToString().ToNullable<float>();
                device.TemperatureThreshold = dtDevices.Rows[j]["wdbaojing"].ToString().ToNullable<float>();
                device.Loop = loop;
                loop.SetDevice<DeviceInfo8021>(device);
                lstDeviceInfo8021.Add(device);
            }
            return true;
        }

        public LoopModel GetDevicesByLoopForControllerType8021(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int DeleteAllDevicesByControllerIDForControllerType8021(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteDeviceByIDForControllerType8021(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8021()
        {
            throw new NotImplementedException();
        }



        public bool CreateTableForDeviceInfoOfControllerType8036()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8036(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8036(DeviceInfo8036 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8036(ref LoopModel loop)
        {
            List<DeviceInfo8036> lstDeviceInfo8036 = new List<DeviceInfo8036>();
            string strDeviceQuerySQL = "select bianhao,leixing,geli,yanshi,shuchu1,shuchu2,nongdu,yjnongdu,louhao,quhao,cenghao,fangjianhao,didian from ";
            strDeviceQuerySQL += loop.Code;
            DataTable dtDevices = _databaseService.GetDataTableBySQL(new StringBuilder(strDeviceQuerySQL));
            int dtDevicesRowsCount = dtDevices.Rows.Count;
            for (int j = 0; j < dtDevicesRowsCount; j++) //器件信息
            {
                DeviceInfo8036 device = new DeviceInfo8036();
                device.Code = dtDevices.Rows[j]["bianhao"].ToString();
                device.Disable = dtDevices.Rows[j]["geli"].ToString().ToNullable<bool>();
                Int16? intTypeCode = dtDevices.Rows[j]["leixing"].ToString().Substring(0, 3).ToNullable<Int16>();
                device.TypeCode = (short)intTypeCode;
                device.DelayValue = dtDevices.Rows[j]["yanshi"].ToString().ToNullable<Int16>();
                device.LinkageGroup1 = dtDevices.Rows[j]["shuchu1"].ToString();
                device.LinkageGroup2 = dtDevices.Rows[j]["shuchu2"].ToString();
                device.AlertValue = dtDevices.Rows[j]["nongdu"].ToString().ToNullable<float>();
                device.ForcastValue = dtDevices.Rows[j]["yjnongdu"].ToString().ToNullable<float>();
                device.BuildingNo = dtDevices.Rows[j]["louhao"].ToString().ToNullable<Int16>();
                device.ZoneNo = dtDevices.Rows[j]["quhao"].ToString().ToNullable<Int16>();
                device.FloorNo = dtDevices.Rows[j]["cenghao"].ToString().ToNullable<Int16>();
                device.RoomNo = dtDevices.Rows[j]["fangjianhao"].ToString().ToNullable<Int16>();
                device.Location = dtDevices.Rows[j]["didian"].ToString();
                device.Loop = loop;
                loop.SetDevice<DeviceInfo8036>(device);
                lstDeviceInfo8036.Add(device);
            }
            return true;
        }

        public LoopModel GetDevicesByLoopForControllerType8036(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int AddController(ControllerModel controller)
        {
            throw new NotImplementedException();
        }

        public int DeleteController(int controllerID)
        {
            throw new NotImplementedException();
        }

        public int GetMaxIDFromController()
        {
            throw new NotImplementedException();
        }

        public List<ControllerModel> GetControllersByProject(ProjectModel project)
        {
            List<ControllerModel> lstControllers = new List<ControllerModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select 文件版本,控制器类型,协议版本,器件长度,位置,系统设置.回路 from (文件配置 left join 位置信息  on 文件配置.文件版本<>位置信息.位置 ) left join  系统设置 on 文件配置.文件版本 <> 系统设置.回路;");

            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            if (dt.Rows.Count > 0) //在此版本中仅有一个控制器信息
            {
                string loopName = dt.Rows[0]["回路"].ToString();
                Model.ControllerModel model = new Model.ControllerModel();
                model.ID = 0;
                model.PrimaryFlag = true;
                //model.Type = (ControllerType)Enum.ToObject(typeof(ControllerType), Convert.ToInt32(dt.Rows[i]["TYPEID"]));
                //model.DeviceAddressLength = dt.Rows[i]["DEVICEADDRESSLENGTH"] is System.DBNull ? 0 : Convert.ToInt16(dt.Rows[i]["DEVICEADDRESSLENGTH"]);
                model.Name = dt.Rows[0]["控制器类型"].ToString();
                //model.PortName = dt.Rows[i]["PORTNAME"].ToString();
                //model.BaudRate = Convert.ToInt32(dt.Rows[i]["BAUDRATE"]);
                model.FileVersion = Convert.ToInt32(dt.Rows[0]["文件版本"].ToString());
                model.ProtocolVersion = dt.Rows[0]["协议版本"].ToString();
                model.Position = dt.Rows[0]["位置"].ToString();
                model.DeviceAddressLength = Convert.ToInt32(dt.Rows[0]["器件长度"].ToString());
                model.ProjectID = project.ID;
                model.Project = project;
                model.LoopAddressLength = 2;
                model.Type = ControllerTypeConverter(dt.Rows[0]["控制器类型"].ToString());
                if (loopName != "" && loopName.Length > 2)
                {
                    model.MachineNumber = loopName.Substring(0, loopName.Length - 2);
                }
                lstControllers.Add(model);
            }
            return lstControllers; 
        }

        public int DeleteLoopInfo(int loopID)
        {
            throw new NotImplementedException();
        }

        public int DeleteLoopsByControllerID(int controllerID)
        {
            throw new NotImplementedException();
        }

        public int AddLoopInfo(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int GetMaxIDFromLoop()
        {
            throw new NotImplementedException();
        }

        public List<LoopModel> GetLoopsByController(ControllerModel controller)
        {
            DataTable dt = null;
            List<LoopModel> lstLoopInfo = new List<LoopModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select 回路,总数 from 系统设置 order by 回路;");
            dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            int dtRowsCount = dt.Rows.Count;
            for (int i = 0; i < dtRowsCount; i++)//回路信息
            {
                LoopModel loop = new LoopModel();
                loop.Name = dt.Rows[i]["回路"].ToString();
                loop.Code = dt.Rows[i]["回路"].ToString();
                loop.DeviceAmount = Convert.ToInt16(dt.Rows[i]["总数"] == null ? 0 : dt.Rows[i]["总数"]);
                lstLoopInfo.Add(loop);
            }
            return lstLoopInfo;
        }

        public int UpdateMatchingController(ControllerType controllerType, string matchingType)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceTypeInfo(List<DeviceType> lstDeviceType)
        {
            throw new NotImplementedException();
        }

        public int GetAmountOfDeviceType()
        {
            throw new NotImplementedException();
        }

        public List<string> GetTablesOfDB(string tableName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public bool CreateTableForDeviceInfoOfControllerType8053()
        {
            throw new NotImplementedException();
        }

        public bool AddDeviceForControllerType8053(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int AddDeviceForControllerType8053(DeviceInfo8053 deviceInfo)
        {
            throw new NotImplementedException();
        }

        public bool GetDevicesByLoopForControllerType8053(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            throw new NotImplementedException();
        }

        public LoopModel GetDevicesByLoopForControllerType8053(LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public int DeleteAllDevicesByControllerIDForControllerType8053(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteDeviceByIDForControllerType8053(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8053()
        {
            throw new NotImplementedException();
        }

        public int DeleteAllDevicesByControllerIDForControllerType8036(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteDeviceByIDForControllerType8036(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxDeviceIDForControllerType8036()
        {
            throw new NotImplementedException();
        }
    }
}
