using System;
using System.Linq;
using System.Collections.Generic;
using SCA.Model;
using SCA.BusinessLib.Controller;
using SCA.Interface.BusinessLogic;

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
            if ((tempCode + amount) > MaxMixedLinkageConfigAmount) //如果需要添加的行数将达上限，则增加剩余的行数
            {
                amount = tempCode + amount - MaxMixedLinkageConfigAmount;
            }
            for (int i = 0; i < amount; i++)
            {
                //currentMaxCode++;
                tempCode++;
                _maxID++;
                LinkageConfigMixed lcm = new LinkageConfigMixed();
                lcm.Controller = _controller;
                lcm.ControllerID = _controller.ID;
                lcm.ID = _maxID;
                lcm.Code = tempCode.ToString().PadLeft(MaxMixedLinkageConfigAmount.ToString().Length, '0');
                lcm.IsDirty = true;
                lstLinkageConfigMixed.Add(lcm);
            }
            _maxCode = tempCode;
            DataRecordAlreadySet = false;
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
                    ProjectManager.GetInstance.MaxIDForMixedLinkageConfig++;
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
    }
}
