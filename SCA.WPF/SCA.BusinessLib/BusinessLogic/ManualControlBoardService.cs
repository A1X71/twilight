using SCA.Interface.BusinessLogic;
using SCA.Model;
using System.Collections.Generic;
using System.Linq;
using System;
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

            if ((tempCode + amount) > MaxManualControlBoardAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                amount = tempCode + amount - MaxManualControlBoardAmount;
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
                    ProjectManager.GetInstance.MaxIDForManualControlBoard++;
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
                }
            }
            catch
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


        public List<ManualControlBoard> Create(int boardNo, int subBoardStartNo, int subBoardEndNo, int startKeyNo, int amount)
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
                amount = tempCode + amount - MaxManualControlBoardAmount;
            }

            for (int j = subBoardStartNo; j <= subBoardEndNo; j++)
            { 
                for (int i = startKeyNo; i < amount; i++)
                {
                    tempCode++;
                    _maxID++;
                    ManualControlBoard mcb = new ManualControlBoard();
                    mcb.BoardNo = boardNo;
                    mcb.SubBoardNo = j;
                    mcb.KeyNo = i;
                    mcb.Controller = _controller;                    
                    mcb.ControllerID = _controller.ID;
                    mcb.Code = tempCode;//.ToString().PadLeft(MaxManualControlBoardAmount.ToString().Length, '0');
                    mcb.ID = _maxID;
                    mcb.IsDirty = true;
                    lstMCB.Add(mcb);
                }
            }
            _maxCode = tempCode;
            DataRecordAlreadySet = false;
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
    }
}
