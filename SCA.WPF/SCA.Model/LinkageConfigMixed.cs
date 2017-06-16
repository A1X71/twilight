using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/1/23 11:49:37
* FileName   : LinkageConfigMixed
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    public class LinkageConfigMixed
    {
        private ControllerModel _controller; //Added at 2017-01-23 控制器信息

        public ControllerModel Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        public int ControllerID { get; set; }
        public int ID { get; set; }

        public string Code { get; set; }
        public int ActionCoefficient { get; set; }

        public LinkageActionType ActionType { get; set; }
        //分类A
        public LinkageType TypeA { get; set; }
        //回路A
        //器件编号A [当分类为“地址”时，存储的"回路编号"]
        public string  LoopNoA { get; set; }
        
        //器件编号A [当分类为“地址”时，存储的"器件编号"]
        public string  DeviceCodeA { get; set; }
        //楼号A
        public int? BuildingNoA { get; set; }
        //区号A
        public int? ZoneNoA { get; set; }
        //层号A
        public int? LayerNoA { get; set; }
        //器件类型A
        public Int16 DeviceTypeCodeA { get; set; }
        public LinkageType TypeB { get; set; }
        public string LoopNoB { get; set; }
        public string DeviceCodeB { get; set; }

        public int? BuildingNoB { get; set; }

        public int? ZoneNoB { get; set; }
        
        public int?  LayerNoB { get; set; }

        public Int16 DeviceTypeCodeB { get; set; }
        

        public LinkageType TypeC { get; set; }
        public string MachineNoC { get; set; }
        public string LoopNoC { get; set; }
        public string DeviceCodeC { get; set; }
        public int? BuildingNoC { get; set; }
        public int? ZoneNoC { get; set; }
        public int? LayerNoC { get; set; }
        public Int16 DeviceTypeCodeC { get; set; }
        #region 非业务数据
        //保存状态
        private bool _isDirty = true;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                if (_isDirty)
                {
                    this.Controller.IsDirty = true;
                }

            }
        }

        public string FormattedLoopNoA 
        {
            get
            {
                if (LoopNoA != "")
                {
                    return LoopNoA.PadLeft(Controller.LoopAddressLength, '0');
                }
                return LoopNoA;
            }
        }

        //器件编号A [当分类为“地址”时，存储的"器件编号"]
        public string FormattedDeviceCodeA 
        {
            get
            {
                if (DeviceCodeA != "")
                {
                    return DeviceCodeA.PadLeft(Controller.DeviceAddressLength, '0');
                }
                return DeviceCodeA;
            }            
        }

        public string FormattedLoopNoB 
        {
            get
            {
                if (LoopNoB != "")
                {
                    return LoopNoB.PadLeft(Controller.LoopAddressLength, '0');
                }
                return LoopNoB;
            }        
        }
        public string FormattedDeviceCodeB 
        {

            get
            {
                if (DeviceCodeB != "")
                {
                    return DeviceCodeB.PadLeft(Controller.DeviceAddressLength, '0');
                }
                return DeviceCodeB;
            }  
        }

        public string FormattedMachineNoC 
        {
            get
            {
                if (MachineNoC != "")
                {
                    return MachineNoC.PadLeft(Controller.MachineNumber.Length, '0');
                }
                return MachineNoC;
            }  
        }
        public string FormattedLoopNoC 
        {
            get
            {
                if (LoopNoC != "")
                {
                    return LoopNoC.PadLeft(Controller.LoopAddressLength, '0');
                }
                return LoopNoC;
            }    
        }
        public string FormattedDeviceCodeC
        {
            get
            {
                if (DeviceCodeC != "")
                {
                    return DeviceCodeC.PadLeft(Controller.DeviceAddressLength, '0');
                }
                return DeviceCodeC;
            }  
        
        }
        #endregion
    }
}
