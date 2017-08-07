﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.BusinessLogic;
using SCA.Model;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.Controller;
using SCA.BusinessLogic;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/27 10:18:12
* FileName   : LinkageConfigStandardService
* Description: 标准组态操作
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class LinkageConfigStandardService : ILinkageConfigStandardService    
    {
        /// <summary>
        /// 记录当前数据是否已经设置
        /// 关于此属性的应用：用此全局属性控制当前的Code编码是否从Controller中获取
        /// </summary>
        static bool DataRecordAlreadySet { get; set; }

        private ControllerModel _controller;
        private int _maxCode = 0;//当前标准组态最大编号
        private int _maxID = 0;//标准组态当前最大ID
        private short _maxStandardLinkageConfigAmount = 0;        
        public LinkageConfigStandardService(ControllerModel controller)
        {
            DataRecordAlreadySet = true;//初始状态
            _controller=controller;
        }
        public ControllerModel TheController
        {
            get { return _controller; }
            set { _controller = value; }
        }
        public short MaxStandardLinkageConfigAmount
        {
            get
            {
              //  if (_maxStandardLinkageConfigAmount == 0)//&& TheController!=null 后续在加至条件中
             //   {
                    _maxStandardLinkageConfigAmount = ControllerConfigManager.GetConfigObject(_controller.Type).GetMaxAmountForStandardLinkageConfig();
             //   }
                return _maxStandardLinkageConfigAmount;
            }
        }
        public List<Model.LinkageConfigStandard> Create(int amount)
        {
            List<LinkageConfigStandard> lstLinkageConfigStandard = new List<LinkageConfigStandard>();
            //int currentMaxCode = GetMaxCode();
            //for (int i = 0; i < amount; i++)
            //{
            //    currentMaxCode++;
            //    LinkageConfigStandard lcs = new LinkageConfigStandard();
            //    lcs.Controller = _controller;
            //    lcs.Code = currentMaxCode.ToString().PadLeft(4, '0'); 
            //    lstLinkageConfigStandard.Add(lcs);
            //}
            //return lstLinkageConfigStandard;     
            if (DataRecordAlreadySet) //如果数据已经填写完成，则可获取最大编号
            { 
                _maxCode = GetMaxCode();
                _maxID = GetMaxID();
            }

            int tempCode = _maxCode;
            if (tempCode >= MaxStandardLinkageConfigAmount) //如果已经达到上限，则不添加任何行
            {
                amount = 0;
            }
            else
            { 
                if ((tempCode + amount) > MaxStandardLinkageConfigAmount) //如果需要添加的行数将达上限，则增加剩余的行数
                {
                    amount = MaxStandardLinkageConfigAmount - tempCode;
                }
                for (int i = 0; i < amount; i++)
                {
                    tempCode++;
                    _maxID++;
                    LinkageConfigStandard lcs = new LinkageConfigStandard();
                    lcs.Controller = _controller;
                    lcs.ControllerID = _controller.ID;
                    lcs.ID = _maxID;
                    lcs.Code = tempCode.ToString().PadLeft(MaxStandardLinkageConfigAmount.ToString().Length, '0');
                    lcs.IsDirty = true;
                    lstLinkageConfigStandard.Add(lcs);
                }
                _maxCode = tempCode;
                DataRecordAlreadySet = false;
                foreach (var singleItem in lstLinkageConfigStandard)
                {
                    Update(singleItem);
                }
            }
            return lstLinkageConfigStandard;
        }

        public bool Update(Model.LinkageConfigStandard linkageConfigStandard)
        {
            try
            {
                LinkageConfigStandard result = _controller.StandardConfig.Find(
                    delegate(LinkageConfigStandard x)
                    {
                        return x.ID == linkageConfigStandard.ID;
                    }
                    );
                if (result != null)
                {
                    result.Code = linkageConfigStandard.Code;
                    //result.ID = linkageConfigStandard.ID;
                    result.Controller = linkageConfigStandard.Controller;
                    result.ControllerID = linkageConfigStandard.ControllerID;
                    result.ActionCoefficient = linkageConfigStandard.ActionCoefficient;
                    result.DeviceNo1 = linkageConfigStandard.DeviceNo1;
                    result.DeviceNo2 = linkageConfigStandard.DeviceNo2;
                    result.DeviceNo3 = linkageConfigStandard.DeviceNo3;
                    result.DeviceNo4 = linkageConfigStandard.DeviceNo4;
                    result.DeviceNo5 = linkageConfigStandard.DeviceNo5;
                    result.DeviceNo6 = linkageConfigStandard.DeviceNo6;
                    result.DeviceNo7 = linkageConfigStandard.DeviceNo7;
                    result.DeviceNo8 = linkageConfigStandard.DeviceNo8;
                    result.DeviceNo9 = linkageConfigStandard.DeviceNo9;
                    result.DeviceNo10 = linkageConfigStandard.DeviceNo10;
                    result.DeviceNo11 = linkageConfigStandard.DeviceNo11;
                    result.DeviceNo12 = linkageConfigStandard.DeviceNo12;
                    result.OutputDevice1 = linkageConfigStandard.OutputDevice1;
                    result.OutputDevice2 = linkageConfigStandard.OutputDevice2;
                    result.LinkageNo1 = linkageConfigStandard.LinkageNo1;
                    result.LinkageNo2 = linkageConfigStandard.LinkageNo2;
                    result.LinkageNo3 = linkageConfigStandard.LinkageNo3;
                }
                else
                {
                    _controller.StandardConfig.Add(linkageConfigStandard);
                    DataRecordAlreadySet=true;
                    if (linkageConfigStandard.ID > ProjectManager.GetInstance.MaxIDForStandardLinkageConfig)
                    {
                        ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = linkageConfigStandard.ID;
                    }
                    else
                    {
                        ProjectManager.GetInstance.MaxIDForStandardLinkageConfig++;
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
        /// 更新指定标准组态ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            try
            {
                LinkageConfigStandard result = _controller.StandardConfig.Find(
                      delegate(LinkageConfigStandard x)
                      {
                          return x.ID == id;
                      }
                      );
                for (int i = 0; i < columnNames.Length; i++)
                {
                    switch (columnNames[i])
                    {
                        //case "输出组号":
                        //    result.Code = data[i];
                        //    break;
                        case "联动模块1":
                            result.DeviceNo1 = data[i].ToString();
                            break;
                        case "联动模块2":
                            result.DeviceNo2 = data[i].ToString();
                            break;
                        case "联动模块3":
                            result.DeviceNo3 = data[i].ToString();
                            break;
                        case "联动模块4":
                            result.DeviceNo4 = data[i].ToString();
                            break;
                        case "联动模块5":
                            result.DeviceNo5 = data[i].ToString();
                            break;
                        case "联动模块6":
                            result.DeviceNo6 = data[i].ToString();
                            break;
                        case "联动模块7":
                            result.DeviceNo7 = data[i].ToString();
                            break;
                        case "联动模块8":
                            result.DeviceNo8 = data[i].ToString();
                            break;
                        case "联动模块9":
                            result.DeviceNo9 = data[i].ToString();
                            break;
                        case "联动模块10":
                            result.DeviceNo10 = data[i].ToString();
                            break;
                        case "输入模块1":
                            result.DeviceNo1 = data[i].ToString();
                            break;
                        case "输入模块2":
                            result.DeviceNo2 = data[i].ToString();
                            break;
                        case "输入模块3":
                            result.DeviceNo3 = data[i].ToString();
                            break;
                        case "输入模块4":
                            result.DeviceNo4 = data[i].ToString();
                            break;
                        case "输入模块5":
                            result.DeviceNo5 = data[i].ToString();
                            break;
                        case "输入模块6":
                            result.DeviceNo6 = data[i].ToString();
                            break;
                        case "输入模块7":
                            result.DeviceNo7 = data[i].ToString();
                            break;
                        case "输入模块8":
                            result.DeviceNo8 = data[i].ToString();
                            break;
                        case "输入模块9":
                            result.DeviceNo9 = data[i].ToString();
                            break;
                        case "输入模块10":
                            result.DeviceNo10 = data[i].ToString();
                            break;
                        case "输入模块11":
                            result.DeviceNo11 = data[i].ToString();
                            break;
                        case "输入模块12":
                            result.DeviceNo12 = data[i].ToString();
                            break;
                        case "输出模块1":
                            result.OutputDevice1 = data[i].ToString();
                            break;
                        case "输出模块2":
                            result.OutputDevice2 = data[i].ToString();
                            break;
                        case "动作常数":
                            result.ActionCoefficient = Convert.ToInt32(data[i]);
                            break;
                        case "联动组1":
                            result.LinkageNo1 = data[i].ToString();
                            break;
                        case "联动组2":
                            result.LinkageNo1 = data[i].ToString();
                            break;
                        case "联动组3":
                            result.LinkageNo1 = data[i].ToString();
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
                var result = from lcs in _controller.StandardConfig where lcs.ID == id select lcs;
                LinkageConfigStandard o = result.FirstOrDefault();
                if (o != null)
                {
                    _controller.StandardConfig.Remove(o);
                    DeleteFromDB(id);
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
                ILinkageConfigStandardDBService standardDBService = new SCA.DatabaseAccess.DBContext.LinkageConfigStandardDBService(_dbFileVersionService);
                if (standardDBService.DeleteStandardLinkageConfigInfo(id))
                {
                    if (BusinessLib.ProjectManager.GetInstance.MaxIDForStandardLinkageConfig == id) //如果最大ID等于被删除的ID，则重新赋值
                    {

                        LinkageConfigStandardService standardService = new LinkageConfigStandardService(TheController);
                        BusinessLib.ProjectManager.GetInstance.MaxIDForStandardLinkageConfig = standardService.GetMaxID();
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
                var query = from r in _controller.StandardConfig
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
        public int GetMaxID()
        {   
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers  select r;
            int maxID = 0;
            foreach (var c in controllers)
            {
                if (c.StandardConfig.Count > 0)
                { 
                    int currentLoopMaxID = c.StandardConfig.Max(info => info.ID);
                    if (currentLoopMaxID > maxID)
                    {
                        maxID = currentLoopMaxID;
                    }
                }
            }
            return maxID;
        }
        //private int GetMaxCode(List<LinkageConfigStandard> lcs)
        //{
        //    int result = 0;
        //    if (lcs != null)
        //    {
        //        var query = from r in lcs select r.Code;
        //        if (query != null)
        //        {
        //            foreach (var i in query)
        //            {
        //                if (Convert.ToInt32(i) > result)
        //                {
        //                    result = Convert.ToInt32(i);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}


        public void DownloadExecute(List<LinkageConfigStandard> lstLinkageConfigStandard)
        {

            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {

                if (iCC.TheControllerType.GetType().ToString() != "SCA.BusinessLib.Controller.ControllerTypeUnknown") //操作类仍未准备好，暂不能下传，需要将此逻辑移到按钮的CanExecute中进行处理
                {
                    iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                    switch (iCC.TheControllerType.ControllerType)
                    {
                        case ControllerType.NT8036:
                            //  dictControllerCom.TryGetValue(Model.ControllerModel.ControllerType.NT8036, out controllerTypeBase);                        

                            //ControllerCommunicationTesting.Get8036DevInfo(out lstDevInfo);
                            ((ControllerType8036)iCC.TheControllerType).StandardLinkageConfigList = lstLinkageConfigStandard;
                            //controllerTypeBase.SendDeviceInfo();
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            break;
                        case ControllerType.NT8001:
                            //dictControllerCom.TryGetValue(Model.ControllerModel.ControllerType.NT8001, out controllerTypeBase);
                            //List<DeviceInfo8001> lstDevInfo8001; //#1 需要确认继续抽像？？！！
                            //Get8001DevInfo(out lstDevInfo8001); //#2
                            //((ControllerType8001)iCC.TheControllerType).DeviceInfoList = lstDevInfo8001; //#3
                            ////controllerTypeBase.SendDeviceInfo();
                            //iCC.TheControllerType.OperableDataType = OperantDataType.Device;
                            //iCC.TheControllerType.Status = ControllerStatus.DataSending;                    
                            break;
                        case ControllerType.NT8007:
                            ((ControllerType8007)iCC.TheControllerType).StandardLinkageConfigList = lstLinkageConfigStandard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            break;
                        case ControllerType.FT8003:
                            ((ControllerType8003)iCC.TheControllerType).StandardLinkageConfigList = lstLinkageConfigStandard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            break;
                        case ControllerType.FT8000:
                            ((ControllerType8000)iCC.TheControllerType).StandardLinkageConfigList = lstLinkageConfigStandard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            break;
                        case ControllerType.NT8053:
                            ((ControllerType8000)iCC.TheControllerType).StandardLinkageConfigList = lstLinkageConfigStandard;
                            iCC.TheControllerType.OperableDataType = OperantDataType.StandardLinkageConfig;
                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
                            break;
                        default:
                            iCC.TheControllerType.Status = ControllerStatus.DataSended;// 没有控制器可以处理此信息，将控制器置于数据发送完成状态
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 在控制器内是否存在相同的组号编码
        /// </summary>
        /// <param name="lstStandardConfig">标准组态集合</param>
        /// <returns></returns>
        public bool IsExistSameCode(List<SCA.Model.LinkageConfigStandard> lstStandardConfig)
        {
            if (lstStandardConfig != null)
            {   
                foreach (var config in lstStandardConfig)
                {
                    int configCount=lstStandardConfig.Count((d)=>d.Code==config.Code);                    
                    if (configCount>1)
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
                ILinkageConfigStandardDBService dbMixedService = new LinkageConfigStandardDBService(_dbFileVersionService);
                dbMixedService.AddStandardLinkageConfigInfo(TheController.StandardConfig);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //public void DataRecordSetFlag(bool flag)
        //{
        //    DataRecordAlreadySet = flag;
        //}
    }
}
