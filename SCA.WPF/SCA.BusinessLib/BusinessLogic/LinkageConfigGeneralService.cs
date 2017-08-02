using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.BusinessLogic;
using SCA.Model;
using SCA.BusinessLib.Controller;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/10 10:32:46
* FileName   : LinkageConfigGeneralService
* Description: 通用组态操作
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class LinkageConfigGeneralService : ILinkageConfigGeneralService
    {
        /// <summary>
        /// 记录当前数据是否已经设置
        /// 关于此属性的应用：用此全局属性控制当前的Code编码是否从Controller中获取
        /// </summary>
        static bool DataRecordAlreadySet { get; set; }

        private ControllerModel _controller;
        private int _maxCode = 0;//当前标准组态最大编号
        private int _maxID = 0;//标准组态当前最大ID
        private short _maxGeneralLinkageConfigAmount = 0;        

        public LinkageConfigGeneralService(ControllerModel controller)
        {
            DataRecordAlreadySet = true;//初始状态
            _controller=controller;
        }
        public ControllerModel TheController
        {
            get { return _controller; }
            set { _controller = value; }
        }
        public short MaxGeneralLinkageConfigAmount
        {
            get
            {
                if (_maxGeneralLinkageConfigAmount == 0)//&& TheController!=null 后续在加至条件中
                {
                    _maxGeneralLinkageConfigAmount = ControllerConfigManager.GetConfigObject(_controller.Type).GetMaxAmountForGeneralLinkageConfig();
                }
                return _maxGeneralLinkageConfigAmount;
            }
        }
        public List<LinkageConfigGeneral> Create(int amount)
        {
            List<LinkageConfigGeneral> lstLinkageConfigGeneral = new List<LinkageConfigGeneral>();
            if (DataRecordAlreadySet)
            {
                _maxCode = GetMaxCode();
                _maxID = GetMaxID();
            }
            int tempCode = _maxCode;
            if (tempCode >= MaxGeneralLinkageConfigAmount) //如果已经达到上限，则不添加任何行
            {
                amount = 0;
            }
            if ((tempCode + amount) > MaxGeneralLinkageConfigAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                amount = tempCode + amount - MaxGeneralLinkageConfigAmount;
            }
            
            for (int i = 0; i < amount; i++)
            {
                tempCode++;
                _maxID++;
                LinkageConfigGeneral lcg = new LinkageConfigGeneral();
                lcg.Controller = _controller;
                lcg.ControllerID = _controller.ID;
                lcg.ID = _maxID;
                lcg.Code = tempCode.ToString().PadLeft(MaxGeneralLinkageConfigAmount.ToString().Length, '0');
                lcg.IsDirty = true;
                lstLinkageConfigGeneral.Add(lcg);
            }
            _maxCode = tempCode;

            DataRecordAlreadySet = false;
            return lstLinkageConfigGeneral;
        }

        public bool Update(LinkageConfigGeneral linkageConfigGeneral)
        {
            try
            {
                LinkageConfigGeneral result = _controller.GeneralConfig.Find(
                    delegate(LinkageConfigGeneral x)
                    {
                        return x.ID == linkageConfigGeneral.ID;
                    }
                    );
                if (result != null)
                {
                    result.Code = linkageConfigGeneral.Code;
                    result.Controller = linkageConfigGeneral.Controller;
                    result.ControllerID = linkageConfigGeneral.ControllerID;
                    result.ID = linkageConfigGeneral.ID;
                    result.Code = linkageConfigGeneral.Code;
                    result.ActionCoefficient = linkageConfigGeneral.ActionCoefficient;
                    result.BuildingNoA = linkageConfigGeneral.BuildingNoA;
                    result.CategoryA = linkageConfigGeneral.CategoryA;
                    result.ZoneNoA = linkageConfigGeneral.ZoneNoA;
                    result.LayerNoA1 = linkageConfigGeneral.LayerNoA1;
                    result.LayerNoA2 = linkageConfigGeneral.LayerNoA2;
                    result.DeviceTypeCodeA = linkageConfigGeneral.DeviceTypeCodeA;
                    result.TypeC = linkageConfigGeneral.TypeC;
                    result.MachineNoC = linkageConfigGeneral.MachineNoC;
                    result.LoopNoC = linkageConfigGeneral.LoopNoC;
                    result.DeviceCodeC = linkageConfigGeneral.DeviceCodeC;
                    result.BuildingNoC = linkageConfigGeneral.BuildingNoC;
                    result.ZoneNoC = linkageConfigGeneral.ZoneNoC;
                    result.LayerNoC = linkageConfigGeneral.LayerNoC;
                    result.DeviceTypeCodeC = linkageConfigGeneral.DeviceTypeCodeC;
                }
                else
                {
                    _controller.GeneralConfig.Add(linkageConfigGeneral);
                    DataRecordAlreadySet = true;
                    if (linkageConfigGeneral.ID > ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig)
                    {
                        ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig = linkageConfigGeneral.ID;
                    }
                    else
                    {
                        ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig++;
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
                var result = from lcg in _controller.GeneralConfig where lcg.ID == id select lcg;
                LinkageConfigGeneral o = result.FirstOrDefault();
                if (o != null)
                {
                    _controller.GeneralConfig.Remove(o);
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
                var query = from r in _controller.GeneralConfig
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

            int maxID = ProjectManager.GetInstance.MaxIDForGeneralLinkageConfig;
            return maxID;
        }


        public void DownloadExecute(List<LinkageConfigGeneral> lstLinkageConfigGeneral)
        {
            InvokeControllerCom iCC = InvokeControllerCom.Instance;
            if (iCC.GetPortStatus())
            {
                iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
                switch (iCC.TheControllerType.ControllerType)
                {
                    case ControllerType.NT8001:
                        ((ControllerType8001)iCC.TheControllerType).GeneralLinkageConfigList = lstLinkageConfigGeneral;
                        iCC.TheControllerType.OperableDataType = OperantDataType.GeneralLinkageConfig;
                        iCC.TheControllerType.Status = ControllerStatus.DataSending;
                        break;
                }
            }
        }
        /// <summary>
        /// 在控制器内是否存在相同的组号编码
        /// </summary>
        /// <param name="lstMixedConfig">通用组态集合</param>
        /// <returns></returns>
        public bool IsExistSameCode(List<SCA.Model.LinkageConfigGeneral> lstGeneralConfig)
        {
            if (lstGeneralConfig != null)
            {
                foreach (var config in lstGeneralConfig)
                {
                    int configCount = lstGeneralConfig.Count((d) => d.Code == config.Code);
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
