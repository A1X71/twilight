using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using SCA.DatabaseAccess.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/24 10:10:12
* FileName   : OldVersionSoftware8001DBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class OldVersionSoftware8001DBService : OldVersionSoftwareDBServiceBase,IOldVersionSoftwareDBService
    {
        private IDatabaseService _databaseService;
        private int _deviceAddressLength;
        
        public OldVersionSoftware8001DBService(IDatabaseService databaseService)
            : base(databaseService)
        {
            _databaseService = databaseService;
        }
        public int DeviceAddressLength
        {
            get
            {
                return _deviceAddressLength;
            }
        }

    

        public bool  GetDevicesInLoop(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard)
        {
            List<DeviceInfo8001> lstDeviceInfo8001 = new List<DeviceInfo8001>();
            try
            {
                string strDeviceQuerySQL = "select bianhao,leixing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,panhao,jianhao,cleixing,GbZone,didian,louhao,quhao,cenghao,fangjianhao,sdpkey from ";
                if (Version == 6) //8001控制器的版本6中增加了texing字段
                {
                    strDeviceQuerySQL = "select bianhao,leixing,texing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,panhao,jianhao,cleixing,GbZone,didian,louhao,quhao,cenghao,fangjianhao,sdpkey from ";
                }
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
                    //不存储cleixing
                    if (Version > 5) //从第6版开始增加了"特性"列 
                    {
                        device.Feature = dtDevices.Rows[j]["texing"].ToString().ToNullable<Int16>();
                        device.TypeCode = (short)intTypeCode;
                    }
                    else
                    {
                        // 特性字段值 更新依据：
                        // 1>"器件类型编号">36且<66-->特性字段为0
                        // 2>"器件类型编号">=101且<=129-->特性字段为1
                        // 器件编码更新依据：
                        // 1> "器件类型编号">=101且<=129　--> 更新为"器件类型编号"-64
                        // 2> cleixing字段为中文名称，需要去掉“自锁”及“点动”前缀（由于本软件中不存储中文名称，故不需更新）                            
                        if (intTypeCode > 36 && intTypeCode < 66)
                        {
                            device.Feature = 0;
                            device.TypeCode = (short)intTypeCode;
                        }
                        else if (intTypeCode >= 101 && intTypeCode <= 129)
                        {
                            device.TypeCode = Convert.ToInt16(intTypeCode - 64);
                            device.Feature = 1;
                        }
                        else
                        {
                            device.TypeCode = (short)intTypeCode;

                        }
                    }
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
                        if (dictDeviceMappingManualControlBoard.ContainsKey(device.Code))//如果存在网络手控盘的信息定义，则设置MCBCode的值，建立与手动盘的关系
                        {
                            device.MCBCode = dictDeviceMappingManualControlBoard[device.Code];
                        }   
                    }
                    if(lstDeviceInfo8001.Count>0)
                    { 
                        _deviceAddressLength = lstDeviceInfo8001[0].Code.Length;
                    }
                    loop.SetDevice<DeviceInfo8001>(device);
                    lstDeviceInfo8001.Add(device);
                }
            }
            catch
            {
                return false;   
            }
            return true;
            //return (List<T>)lstDeviceInfo8001;
        }

        public List<LinkageConfigStandard> GetStandardLinkageConfig()
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

        public List<LinkageConfigMixed> GetMixedLinkageConfig()
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
                        linkageConfigMixed.DeviceTypeCodeA = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型A"].ToString());
                        
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
                        linkageConfigMixed.DeviceTypeCodeB =ConvertDeviceTypeCodeToCurrentVersion( dt.Rows[i]["类型B"].ToString());
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
                        linkageConfigMixed.DeviceTypeCodeC =ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型C"].ToString());
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

        public List<LinkageConfigGeneral> GetGeneralLinkageConfig()
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
                    linkageConfigGeneral.ActionCoefficient =Convert.ToInt32(dt.Rows[i]["动作常数"]);
                    linkageConfigGeneral.BuildingNoA = dt.Rows[i]["楼号A"].ToString().ToNullable<int>();
                    linkageConfigGeneral.ZoneNoA = dt.Rows[i]["区号A"].ToString().ToNullable<int>();
                    linkageConfigGeneral.LayerNoA1 = dt.Rows[i]["层号A1"].ToString().ToNullable<int>();
                    linkageConfigGeneral.LayerNoA2 = dt.Rows[i]["层号A2"].ToString().ToNullable<int>();                   

                    linkageConfigGeneral.DeviceTypeCodeA = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型A"].ToString());

                    Int16 intTypeCodeC = ConvertDeviceTypeCodeToCurrentVersion(dt.Rows[i]["类型C"].ToString()); 

                    linkageConfigGeneral.TypeC = ConvertLinkageType(dt.Rows[i]["分类C"].ToString());
                    if (linkageConfigGeneral.TypeC == LinkageType.ZoneLayer)
                    {
                        linkageConfigGeneral.BuildingNoC = dt.Rows[i]["楼号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.ZoneNoC = dt.Rows[i]["区号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.LayerNoC = dt.Rows[i]["层号C"].ToString().ToNullable<int>();
                        linkageConfigGeneral.DeviceTypeCodeC = intTypeCodeC;
                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.Address)
                    {
                        linkageConfigGeneral.MachineNoC = dt.Rows[i]["楼号C"].ToString();
                        linkageConfigGeneral.LoopNoC = dt.Rows[i]["区号C"].ToString();
                        linkageConfigGeneral.DeviceCodeC = dt.Rows[i]["层号C"].ToString();

                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.SameLayer)
                    {
                        linkageConfigGeneral.DeviceTypeCodeC = intTypeCodeC;
                    }
                    else if (linkageConfigGeneral.TypeC == LinkageType.AdjacentLayer)
                    {
                        linkageConfigGeneral.DeviceTypeCodeC = intTypeCodeC;
                    }

                    lstGeneral.Add(linkageConfigGeneral);
                }
            }
            catch
            { 
            
            }
            return lstGeneral;                
        }

        public List<ManualControlBoard> GetManualControlBoard()
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

        /// <summary>
        /// 将读入的器件类型转换为当前的器件类型值 
        /// </summary>
        /// <param name="deviceTypeCode"></param>
        /// <returns></returns>
        private Int16 ConvertDeviceTypeCodeToCurrentVersion(string deviceTypeCode)//int? deviceTypeCode)
        {

            Int16? intTypeCode = null;
            if(!string.IsNullOrEmpty(deviceTypeCode))
            {
                if (deviceTypeCode.Length > 3)
                { 
                     intTypeCode = deviceTypeCode.Substring(0,3).ToNullable<Int16>();

                    if (intTypeCode != null)
                    {
                        if (intTypeCode > 36 && intTypeCode < 66)
                        {
                            //linkageConfigGeneral.DeviceTypeCodeA = intTypeCode;
                            //Do Nothing
                        }
                        else if (intTypeCode >= 101 && intTypeCode <= 129)
                        {
                            intTypeCode = Convert.ToInt16((intTypeCode - Convert.ToInt16(64)));
                        }
                        else
                        {
        //                    linkageConfigGeneral.DeviceTypeCodeA = intTypeCode;
                            //Do Nothing
                        }
                    }
                }
            }
            Int16 intTypeResult = Convert.ToInt16(intTypeCode); //增加于2017-05-19 弃用为空类型
            return intTypeResult;
        }
    }
}
