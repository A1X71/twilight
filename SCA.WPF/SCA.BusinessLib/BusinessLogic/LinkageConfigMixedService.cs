using System;
using System.Linq;
using System.Collections.Generic;
using SCA.Model;
using SCA.BusinessLib.Controller;
using SCA.Interface.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.BusinessLogic;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/10 8:40:14
* FileName   : LinkageConfigMixedService
* Description: 混合组态操作
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
   public class LinkageConfigMixedService:ILinkageConfigMixedService
    {
        /// <summary>
        /// 记录当前数据是否已经设置
        /// 关于此属性的应用：用此全局属性控制当前的Code编码是否从Controller中获取
        /// </summary>
       static bool DataRecordAlreadySet { get; set; }

       private ControllerModel _controller;
       private int _maxCode = 0;//当前标准组态最大编号
       private int _maxID = 0;//标准组态当前最大ID
       private short _maxMixedLinkageConfigAmount = 0;       
       public LinkageConfigMixedService(ControllerModel controller)
       {
           DataRecordAlreadySet = true;//初始状态
           _controller = controller;
       }
       public ControllerModel TheController
       {
           get { return _controller; }
           set { _controller = value; }
       }
       public short MaxMixedLinkageConfigAmount
       {
           get
           {
               if (_maxMixedLinkageConfigAmount == 0)//&& TheController!=null 后续在加至条件中
               {
                   _maxMixedLinkageConfigAmount = ControllerConfigManager.GetConfigObject(_controller.Type).GetMaxAmountForMixedLinkageConfig();
               }
               return _maxMixedLinkageConfigAmount;
           }
       }
 

        public System.Collections.Generic.List<Model.LinkageConfigMixed> Create(int amount)
        {
            List<LinkageConfigMixed> lstLinkageConfigMixed = new List<LinkageConfigMixed>();
            //int currentMaxCode = GetMaxCode();
            if (DataRecordAlreadySet)
            {
                _maxCode = GetMaxCode();
                _maxID = GetMaxID();
            }
            int tempCode = _maxCode;
            if (tempCode >= MaxMixedLinkageConfigAmount)
            {
                amount = 0;
            }
            else
            { 
                if ((tempCode + amount) > MaxMixedLinkageConfigAmount) //如果需要添加的行数将达上限，则增加剩余的行数
                {
                    amount = MaxMixedLinkageConfigAmount - tempCode;
                }
                for (int i = 0; i < amount; i++)
                {
                    //currentMaxCode++;
                    tempCode++;
                    _maxID++;
                    LinkageConfigMixed lcm = new LinkageConfigMixed();
                    lcm.Controller = _controller;
                    lcm.ControllerID = _controller.ID;                
                    lcm.ActionType = LinkageActionType.AND;
                    lcm.TypeA = LinkageType.ZoneLayer;
                    lcm.TypeB = LinkageType.ZoneLayer;
                    lcm.TypeC = LinkageType.ZoneLayer;
                    lcm.ID = _maxID;
                    lcm.Code = tempCode.ToString().PadLeft(MaxMixedLinkageConfigAmount.ToString().Length, '0');
                    lcm.IsDirty = true;
                    lstLinkageConfigMixed.Add(lcm);
                }
                _maxCode = tempCode;
                DataRecordAlreadySet = false;
                foreach (var singleItem in lstLinkageConfigMixed)
                {
                    Update(singleItem);
                }
            }
            return lstLinkageConfigMixed;
        }

        public bool Update(Model.LinkageConfigMixed linkageConfigMixed)
        {
            try
            {
                LinkageConfigMixed result = _controller.MixedConfig.Find(
                    delegate(LinkageConfigMixed x)
                    {
                        return x.ID == linkageConfigMixed.ID;
                    }
                    );
                if (result != null)
                {
                    result.Code = linkageConfigMixed.Code;
                    result.Controller = linkageConfigMixed.Controller;
                    result.ControllerID = linkageConfigMixed.ControllerID;                    
                    result.ActionCoefficient = linkageConfigMixed.ActionCoefficient;
                    result.ActionType = linkageConfigMixed.ActionType;
                    //分类A
                    result.TypeA = linkageConfigMixed.TypeA;
                    //回路A
                    //器件编号A [当分类为“地址”时，存储的"回路编号"]
                    result.LoopNoA = linkageConfigMixed.LoopNoA;
                    //器件编号A [当分类为“地址”时，存储的"器件编号"]
                    result.DeviceCodeA = linkageConfigMixed.DeviceCodeA;
                    result.CategoryA = linkageConfigMixed.CategoryA;
                    //楼号A
                    result.BuildingNoA = linkageConfigMixed.BuildingNoA;
                    //区号A
                    result.ZoneNoA = linkageConfigMixed.ZoneNoA;
                    //层号A
                    result.LayerNoA = linkageConfigMixed.LayerNoA;
                    //器件类型A
                    result.DeviceTypeCodeA = linkageConfigMixed.DeviceTypeCodeA;
                    result.TypeB = linkageConfigMixed.TypeB;
                    result.LoopNoB = linkageConfigMixed.LoopNoB;
                    result.DeviceCodeB = linkageConfigMixed.DeviceCodeB;
                    result.CategoryB = linkageConfigMixed.CategoryB;
                    result.BuildingNoB = linkageConfigMixed.BuildingNoB;
                    result.ZoneNoB = linkageConfigMixed.ZoneNoB;
                    result.LayerNoB = linkageConfigMixed.LayerNoB;
                    result.DeviceTypeCodeB = linkageConfigMixed.DeviceTypeCodeB;
                    result.TypeC = linkageConfigMixed.TypeC;
                    result.MachineNoC = linkageConfigMixed.MachineNoC;
                    result.LoopNoC = linkageConfigMixed.LoopNoC;
                    result.DeviceCodeC = linkageConfigMixed.DeviceCodeC;
                    result.BuildingNoC = linkageConfigMixed.BuildingNoC;
                    result.ZoneNoC = linkageConfigMixed.ZoneNoC;
                    result.LayerNoC = linkageConfigMixed.LayerNoC;
                    result.DeviceTypeCodeC = linkageConfigMixed.DeviceTypeCodeC;
                }
                else
                {
                    _controller.MixedConfig.Add(linkageConfigMixed);
                    DataRecordAlreadySet = true;
                    if (linkageConfigMixed.ID > ProjectManager.GetInstance.MaxIDForMixedLinkageConfig)
                    {
                        ProjectManager.GetInstance.MaxIDForMixedLinkageConfig = linkageConfigMixed.ID;
                    }
                    else
                    { 
                        ProjectManager.GetInstance.MaxIDForMixedLinkageConfig++;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新指定混合组态ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            try
            {
                        LinkageConfigMixed result = _controller.MixedConfig.Find(
                              delegate(LinkageConfigMixed x)
                              {
                                  return x.ID == id;
                              }
                              );
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            switch (columnNames[i])
                            { 
                                //case "编号":
                                //    result.Code = data[i];
                                //    break;
                                case "动作常数":
                                    result.ActionCoefficient = Convert.ToInt32(data[i]);
                                    break;
                                case "动作类型":
                                    { 
                                        LinkageActionType lActiontype = result.ActionType;
                                        Enum.TryParse<LinkageActionType>(EnumUtility.GetEnumName(lActiontype.GetType(), data[i]), out lActiontype);
                                        result.ActionType = lActiontype;
                                    }
                                    break;
                                case "A分类":
                                    { 
                                        LinkageType linkageType = result.TypeA;
                                        Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(linkageType.GetType(), data[i]), out linkageType);
                                        result.TypeA = linkageType;
                                    }
                                    break;
                                case "A类别":
                                    { 
                                        switch (data[i])
                                        {
                                            case "本系统":
                                                result.CategoryA = 0;
                                                break;
                                            case "它系统":
                                                result.CategoryA = 1;
                                                break;
                                            case "全系统":
                                                result.CategoryA = 2;
                                                break;
                                        }
                                    }
                                    break;
                                case "A楼号":
                                    result.BuildingNoA =  new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "A区号":
                                    result.ZoneNoA = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "A层号":
                                    result.LayerNoA = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "A路号":
                                    result.LoopNoA = data[i].ToString();
                                    break;
                                case "A编号":
                                    result.DeviceCodeA = data[i].ToString();
                                    break;
                                case "A类型":
                                    result.DeviceTypeCodeA = Convert.ToInt16(data[i]);
                                    break;
                                case "B分类":
                                    {
                                        LinkageType linkageType = result.TypeB;
                                        Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(linkageType.GetType(), data[i]), out linkageType);
                                        result.TypeB = linkageType;
                                    }
                                    break;
                                case "B类别":
                                    {
                                        switch (data[i])
                                        {
                                            case "本系统":
                                                result.CategoryB = 0;
                                                break;
                                            case "它系统":
                                                result.CategoryB = 1;
                                                break;
                                            case "全系统":
                                                result.CategoryB = 2;
                                                break;
                                        }
                                    }
                                    break;
                                case "B楼号":
                                    result.BuildingNoB = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "B区号":
                                    result.ZoneNoB= new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "B层号":
                                    result.LayerNoB = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "B路号":
                                    result.LoopNoB = data[i].ToString();
                                    break;
                                case "B编号":
                                    result.DeviceCodeB = data[i].ToString();
                                    break;
                                case "B类型":
                                    result.DeviceTypeCodeB = Convert.ToInt16(data[i]);
                                    break;
                                case "C分类":
                                    {
                                        LinkageType linkageType = result.TypeC;
                                        Enum.TryParse<LinkageType>(EnumUtility.GetEnumName(linkageType.GetType(), data[i]), out linkageType);
                                        result.TypeC = linkageType;
                                    }
                                    break;
                                case "C楼号":
                                    result.BuildingNoC = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "C区号":
                                    result.ZoneNoC = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "C层号":
                                    result.LayerNoC = new Nullable<int>(Convert.ToInt32(data[i]));
                                    break;
                                case "C机号":
                                    result.MachineNoC = data[i].ToString();
                                    break;
                                case "C路号":
                                    result.LoopNoC = data[i].ToString();
                                    break;
                                case "C编号":
                                    result.DeviceCodeC = data[i].ToString();
                                    break;
                                case "C类型":
                                    result.DeviceTypeCodeC = Convert.ToInt16(data[i]);
                                    break;
                            }
                        }
                
            }
            catch (Exception ex)
            {
                return false;
            }
                return true;
        }
        public bool DeleteBySpecifiedID(int id)
        {
            try
            {
                var result = from lcm in _controller.MixedConfig where lcm.ID == id select lcm;
                LinkageConfigMixed o = result.FirstOrDefault();
                if (o != null)
                {
                    _controller.MixedConfig.Remove(o);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        private bool DeleteFromDB(int id)
        {
            try
            {
                IFileService _fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(TheController.Project.SavePath, logger, _fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(DBFileVersionManager.CurrentDBFileVersion);
                ILinkageConfigMixedDBService mixedDBService = new SCA.DatabaseAccess.DBContext.LinkageConfigMixedDBService(_dbFileVersionService);
                if (mixedDBService.DeleteMixedLinkageConfigInfo(id))
                {
                    if (BusinessLib.ProjectManager.GetInstance.MaxIDForMixedLinkageConfig == id) //如果最大ID等于被删除的ID，则重新赋值
                    {
                        LinkageConfigMixedService mixedService = new LinkageConfigMixedService(TheController);
                        BusinessLib.ProjectManager.GetInstance.MaxIDForMixedLinkageConfig = mixedService.GetMaxID();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private int GetMaxCode()
        {
            int result = 0;
            if (_controller != null)
            {
                var query = from r in _controller.MixedConfig
                            select r.Code;
                if (query != null)
                {

                    foreach (var i in query)
                    {
                        if (Convert.ToInt32(i) > result)
                        {
                            result = Convert.ToInt32(i);
                        }
                    }
                }
            }
            return result;
        }
        private int GetMaxID()
        {
            
            int maxID = ProjectManager.GetInstance.MaxIDForMixedLinkageConfig;
            return maxID;
        }


        public void DownloadExecute(List<LinkageConfigMixed> lstLinkageConfigMixed)
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                switch (iCC.TheControllerType.ControllerType)
                {
                    case ControllerType.NT8001:
                        ((ControllerType8001)iCC.TheControllerType).MixedLinkageConfigList = lstLinkageConfigMixed;
                        iCC.TheControllerType.OperableDataType = OperantDataType.MixedLinkageConfig;
                        iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        break;
                }
            }
        }
        /// <summary>
        /// 在控制器内是否存在相同的组号编码
        /// </summary>
        /// <param name="lstMixedConfig">混合组态集合</param>
        /// <returns></returns>
        public bool IsExistSameCode(List<SCA.Model.LinkageConfigMixed> lstMixedConfig)
        {
            if (lstMixedConfig != null)
            {
                foreach (var config in lstMixedConfig)
                {
                    int configCount = lstMixedConfig.Count((d) => d.Code == config.Code);
                    if (configCount > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool SaveToDB()
        {
            try
            {
                ILogRecorder logger = null;
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(TheController.Project.SavePath, logger, fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(SCA.BusinessLogic.DBFileVersionManager.CurrentDBFileVersion);
                ILinkageConfigMixedDBService dbMixedService = new LinkageConfigMixedDBService(_dbFileVersionService);
                dbMixedService.AddMixedLinkageConfigInfo(TheController.MixedConfig);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
