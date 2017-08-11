using SCA.Interface.BusinessLogic;
using SCA.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.Controller;
using SCA.BusinessLogic;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/21 11:08:08
* FileName   : ManualControlBoardService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ManualControlBoardService:IManualControlBoardService
    {
        /// <summary>
        /// 记录当前数据是否已经设置
        /// 关于此属性的应用：用此全局属性控制当前的Code编码是否从Controller中获取
        /// </summary>
        static bool DataRecordAlreadySet { get; set; }

        private ControllerModel _controller;
        private int _maxCode = 0;//当前标准组态最大编号
        private int _maxID = 0;//标准组态当前最大ID
        private short _maxManualControlBoardAmount = 0;    
        public ManualControlBoardService(ControllerModel controller)
        {
            DataRecordAlreadySet = true; //初始状态
            _controller = controller;
        }
        public ControllerModel TheController
        {
            get { return _controller; }
            set { _controller = value; }
        }
        public short MaxManualControlBoardAmount
        {
            get
            {
                if (_maxManualControlBoardAmount == 0)//&& TheController!=null 后续在加至条件中
                {
                    _maxManualControlBoardAmount = ControllerConfigManager.GetConfigObject(_controller.Type).GetMaxAmountForManualControlBoardConfig();
                }
                return _maxManualControlBoardAmount;
            }
        }
        public List<ManualControlBoard> Create(int amount)
        {
            List<ManualControlBoard> lstMCB = new List<ManualControlBoard>();
            if (DataRecordAlreadySet) //如果数据已经填写完成，则可获取最大编号
            {
                _maxCode = GetMaxCode();
                _maxID = GetMaxID();
            }

            int tempCode = _maxCode;
            if (tempCode >= MaxManualControlBoardAmount) //如果已经达到上限，则不添加任何行
            {
                amount = 0;
            }
            else
            { 
                if ((tempCode + amount) > MaxManualControlBoardAmount) //如果需要添加的行数将达上限，则增加剩余的行数
                {
                    amount = MaxManualControlBoardAmount - tempCode;
                }
            
                for (int i = 0; i < amount; i++)
                {
                    tempCode++;
                    _maxID++;
                    ManualControlBoard mcb = new ManualControlBoard();
                    mcb.Controller = _controller;
                    mcb.ControllerID = _controller.ID;
                    mcb.Code = tempCode;//.ToString().PadLeft(MaxManualControlBoardAmount.ToString().Length, '0');
                    mcb.ID = _maxID;
                    mcb.IsDirty = true;
                    lstMCB.Add(mcb);
                }
                _maxCode = tempCode;
                DataRecordAlreadySet = false;

                foreach (var singleItem in lstMCB)
                {
                    Update(singleItem);
                }
            }
            return lstMCB;
        }
        
        public bool Update(ManualControlBoard mcb)
        {
            try
            {           
                ManualControlBoard result=_controller.ControlBoard.Find(
                                            delegate(ManualControlBoard x)
                                            {
                                                return x.ID == mcb.ID;
                                            }
                    );
                if (result!= null)//更新数据
                {                    
                    result.Code = mcb.Code;
                    result.BoardNo = mcb.BoardNo;
                    result.SubBoardNo = mcb.SubBoardNo;
                    result.KeyNo = mcb.KeyNo;
                    result.DeviceCode = mcb.DeviceCode;
                    result.DeviceType = mcb.DeviceType;
                    result.LocalDevice1 = mcb.LocalDevice1;
                    result.LocalDevice2 = mcb.LocalDevice2;
                    result.LocalDevice3 = mcb.LocalDevice3;
                    result.LocalDevice4 = mcb.LocalDevice4;
                    result.NetDevice1 = mcb.NetDevice1;
                    result.NetDevice2 = mcb.NetDevice2;
                    result.NetDevice3 = mcb.NetDevice3;
                    result.NetDevice4 = mcb.NetDevice4;
                    result.BuildingNo = mcb.BuildingNo;
                    result.AreaNo = mcb.AreaNo;
                    result.FloorNo = mcb.FloorNo;
                    result.ControlType = mcb.ControlType;
                    result.LinkageGroup = mcb.LinkageGroup;
                    result.SDPKey = mcb.SDPKey;
                    result.MaxSubBoardNo = mcb.MaxSubBoardNo;
                    result.Controller = mcb.Controller;
                    result.ControllerID = mcb.ControllerID;
                }
                else
                {
                    _controller.ControlBoard.Add(mcb);
                    DataRecordAlreadySet = true;
                    if (mcb.ID > ProjectManager.GetInstance.MaxIDForManualControlBoard)
                    {
                        ProjectManager.GetInstance.MaxIDForManualControlBoard = mcb.ID;
                    }
                    else
                    {
                        ProjectManager.GetInstance.MaxIDForManualControlBoard++;
                    }                    
                }
            }
            catch
            {
                return false;
            }
            return true;

        }


        public bool DeleteBySpecifiedID(int id)
        {
            try
            {
                var result = from mcb in _controller.ControlBoard where mcb.ID == id select mcb;
                ManualControlBoard board = result.FirstOrDefault();
                if (board != null)
                {
                    _controller.ControlBoard.Remove(board);
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
                IManualControlBoardDBService mcbDBService = new SCA.DatabaseAccess.DBContext.ManualControlBoardDBService(_dbFileVersionService);
                if (mcbDBService.DeleteManualControlBoardInfo(id))
                {
                    if (BusinessLib.ProjectManager.GetInstance.MaxIDForManualControlBoard == id) //如果最大ID等于被删除的ID，则重新赋值
                    {

                        ManualControlBoardService mcbService = new ManualControlBoardService(TheController);
                        BusinessLib.ProjectManager.GetInstance.MaxIDForManualControlBoard = mcbService.GetMaxID();
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
                var query = from r in _controller.ControlBoard
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

            int maxID = ProjectManager.GetInstance.MaxIDForManualControlBoard;
            return maxID;
        }


        public void DownloadExecute(List<ManualControlBoard> lstManualControlBoard)
        {
            throw new NotImplementedException();
        }


        public List<ManualControlBoard> Create(int boardNo, int subBoardStartNo, int subBoardEndNo, int amount)
        {
            List<ManualControlBoard> lstMCB = new List<ManualControlBoard>();
            if (DataRecordAlreadySet) //如果数据已经填写完成，则可获取最大编号
            {
                _maxCode = GetMaxCode();
                _maxID = GetMaxID();
            }
            int tempCode = _maxCode;
            if (tempCode >= MaxManualControlBoardAmount) //如果已经达到上限，则不添加任何行
            {
                amount = 0;
            }

            if ((tempCode + amount) > MaxManualControlBoardAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                amount = MaxManualControlBoardAmount - tempCode;
            }
            

            for (int j = subBoardStartNo; j <= subBoardEndNo; j++)
            { 
                SCA.Interface.IControllerConfig config =ControllerConfigManager.GetConfigObject(this.TheController.Type);
                int totalMaxKeyNo=config.GetMaxAmountForKeyNoInManualControlBoardConfig();
              
                int maxKeyNo=0;
                //获取当前板卡及回路下的最大"手键号"
                if (TheController.ControlBoard.Count == 0)
                {
                    maxKeyNo = 0;
                }
                else
                {
                    var result = TheController.ControlBoard.Where(mcb => mcb.SubBoardNo == j && mcb.BoardNo == boardNo);
                    if(result.Count() != 0)
                        maxKeyNo = TheController.ControlBoard.Where(mcb => mcb.SubBoardNo == j && mcb.BoardNo == boardNo).Max(mcb => mcb.KeyNo);
                }
                if (maxKeyNo < totalMaxKeyNo)
                {
                    for (int i = 1; i <= amount; i++)
                    {
                        tempCode++;
                        _maxID++;
                        maxKeyNo++;
                        ManualControlBoard mcb = new ManualControlBoard();
                        mcb.BoardNo = boardNo;
                        mcb.SubBoardNo = j;
                        mcb.KeyNo = maxKeyNo;
                        mcb.Controller = _controller;
                        mcb.ControllerID = _controller.ID;
                        mcb.Code = tempCode;//.ToString().PadLeft(MaxManualControlBoardAmount.ToString().Length, '0');
                        mcb.ID = _maxID;
                        mcb.IsDirty = true;
                        lstMCB.Add(mcb);
                    }
                }   
            }
            _maxCode = tempCode;
            DataRecordAlreadySet = false;
            foreach (var singleItem in lstMCB)
            {
                Update(singleItem);
            }
            return lstMCB;
        }
        /// <summary>
        /// 在控制器内是否存在相同的组号编码
        /// </summary>
        /// <param name="lstMixedConfig">通用组态集合</param>
        /// <returns></returns>
        public bool IsExistSameCode(List<SCA.Model.ManualControlBoard> lstMCB)
        {
            if (lstMCB != null)
            {
                foreach (var config in lstMCB)
                {
                    int configCount = lstMCB.Count((d) => d.Code == config.Code);
                    if (configCount > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 更新指定ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        public bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data)
        {
            try
            {
                ManualControlBoard result = _controller.ControlBoard.Find(
                      delegate(ManualControlBoard x)
                      {
                          return x.ID == id;
                      }
                      );
                for (int i = 0; i < columnNames.Length; i++)
                {
                    switch (columnNames[i])
                    {
                        //case "编号":
                        //    result.Code = Convert.ToInt32(data[i]);
                        //    break;
                        case "板卡号":
                            result.BoardNo = Convert.ToInt32(data[i]);
                            break;
                        case "手盘号":
                            result.SubBoardNo = Convert.ToInt32(data[i]);
                            break;
                        //case "手键号":
                        //    result.KeyNo = Convert.ToInt32(data[i]);
                        //    break;
                        case "器件编号":
                            result.DeviceCode = data[i].ToString();
                            break;
                        case "被控类型":
                            {
                                switch (data[i])
                                {
                                    case "空器件":
                                        result.ControlType = 0;
                                        break;
                                    case "本机设备":
                                        result.ControlType = 1;
                                        break;
                                    case "楼区层":
                                        result.ControlType = 2;
                                        break;
                                    case "输出组":
                                        result.ControlType = 3;
                                        break;
                                    case "网络设备":
                                        result.ControlType = 4;
                                        break;
                                    default:
                                        result.ControlType = 0;
                                        break;
                                }                                
                            }
                            break;
                        case "本机设备1":
                            result.LocalDevice1 = data[i];
                            break;
                        case "本机设备2":
                            result.LocalDevice2 = data[i];
                            break;
                        case "本机设备3":
                            result.LocalDevice3 = data[i];
                            break;
                        case "本机设备4":
                            result.LocalDevice4 = data[i];
                            break;
                        case "楼号":
                            result.BuildingNo = data[i];
                            break;
                        case "区号":
                            result.AreaNo = data[i];
                            break;
                        case "层号":
                            result.FloorNo = data[i];
                            break;
                        case "设备类型":
                            result.DeviceType = data[i]==""?(short)0:Convert.ToInt16(data[i]);
                            break;
                        case "输出组":
                            result.LinkageGroup = data[i];
                            break;
                        case "网络设备1":
                            result.NetDevice1 = data[i];
                            break;
                        case "网络设备2":
                            result.NetDevice2 = data[i];
                            break;
                        case "网络设备3":
                            result.NetDevice3 = data[i];
                            break;
                        case "网络设备4":
                            result.NetDevice4 = data[i];
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
        public bool SaveToDB()
        {
            try
            {
                ILogRecorder logger = null;
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(TheController.Project.SavePath, logger, fileService);
                IDBFileVersionService _dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(SCA.BusinessLogic.DBFileVersionManager.CurrentDBFileVersion);
                IManualControlBoardDBService dbMCBService = new ManualControlBoardDBService(_dbFileVersionService);
                dbMCBService.AddManualControlBoardInfo(TheController.ControlBoard);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
