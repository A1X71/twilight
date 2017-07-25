using System;
using System.Collections.Generic;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/6 10:49:46
* FileName   : ControllerModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    /// <summary>
    /// 控制器信息
    /// </summary>
    public class ControllerModel
    {
        private ProjectModel _project; //Added at 2016-12-06 控制器信息
        private List<LoopModel> _lstLoops;
        private List<LinkageConfigStandard> _lstStandardConfig; //Added at 2017-02-15
        private List<LinkageConfigMixed> _lstMixedConfig; //Added at 2017-02-15
        private List<LinkageConfigGeneral> _lstGeneralConfig; //Added at 2017-02-15
        private List<ManualControlBoard> _lstManualControlBoard; //Added at 2017-02-15
        private string _machineNumber;//机号 2017-02-22
        private string _iconInTree = @"Resources\Icon\Style1\Controller.jpg";
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        private int _baudRate = 38400;
        private bool _primaryFlag = false;
        int _loopAddressLength = 2;
        //private int _deviceCodeLength = 3;//器件编码长度，默认3
        public ControllerModel()
        { 
        
        }
        public ControllerModel(ControllerType type)
        {            
            Type = type;
            LoopAddressLength = 2;
        }
        public string IconInTree
        {
            get
            {
                return _appCurrentPath+_iconInTree;
            }
            set
            {
                _iconInTree = value;
            }
        }
        public ControllerModel(int id, string name,ControllerType type,int level)
        {
            
            ID = id;
            Name = name;
            Type = type;
            Level = level;
            LoopAddressLength = 2;
        }
        /// <summary>
        /// 目录级别
        /// 页面显示应用，不需存储
        /// </summary>
        public int Level { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }

        public bool PrimaryFlag 
        {
            get
            {
                return _primaryFlag;
            }
            set
            {
                _primaryFlag = value;
               // NotifyOfPropertyChange("PrimaryFlag");
            }
        }

        /// <summary>
        /// 器件地址长度
        /// </summary>
        public int DeviceAddressLength { get; set; }
        ///// <summary>
        ///// 回路地址长度，先不考虑，目前按固定2位
        ///// </summary>
        public int LoopAddressLength { get { return _loopAddressLength; } set { _loopAddressLength = value; } }

        //public int DeviceCodeLength
        //{

        //    get
        //    {
        //        return _deviceCodeLength;
        //    }
        //    set
        //    {
        //        _deviceCodeLength = value;
        //    }
        //}

        public ControllerType Type { get; set; }

        public string PortName { get; set; }

        public int BaudRate {
            get
            {
                return _baudRate;
            }
            set
            {
                _baudRate = value;
            }
        }
        //机号,按地址位数格式化好的机号如001,000
        public string MachineNumber {
            get
            {
                if (_machineNumber != null)
                { 
                    if (DeviceAddressLength == 8)
                    {
                        _machineNumber=_machineNumber.PadLeft(3, '0');
                    }
                    else
                    {
                        _machineNumber = _machineNumber.PadLeft(2, '0');
                    }
                }
                return _machineNumber;
            }
            set {
                _machineNumber = value;
            } 
        }
        
        
        public Int16 Version { get; set; }

        /// <summary>
        /// 项目信息 obsolete at 2017-02-14 采用Project ID作为关系联接
        /// </summary>
        public ProjectModel  Project
        {
            get
            {
                return _project ;
            }
            set
            {
                _project  = value;
            }
        }
        /// <summary>
        /// 存储关联的ProjectID
        /// </summary>
        public int ProjectID { get; set; }
        public List<LoopModel> Loops
        {
            get
            {
                if (_lstLoops == null)
                {
                    _lstLoops = new List<LoopModel>();
                }
                return _lstLoops;
            }
            //private set
            //{
            //    if (_lstLoops == null)
            //    {
            //        _lstLoops = new List<LoopModel>();
            //        _lstLoops = value;
            //    }                
            //}
        }

        public List<LinkageConfigStandard> StandardConfig
        {
            get
            {
                if (_lstStandardConfig == null)
                {
                    _lstStandardConfig = new List<LinkageConfigStandard>();
                }
                return _lstStandardConfig;
            }
        }
        public List<LinkageConfigMixed> MixedConfig
        {
            get
            {
                if (_lstMixedConfig == null)
                {
                    _lstMixedConfig = new List<LinkageConfigMixed>();
                }
                return _lstMixedConfig;
            }
        }
        public List<LinkageConfigGeneral> GeneralConfig
        {
            get
            {
                if (_lstGeneralConfig == null)
                {
                    _lstGeneralConfig = new List<LinkageConfigGeneral>();
                }
                return _lstGeneralConfig;
            }
        }
        public List<ManualControlBoard> ControlBoard
        {
            get
            {
                if (_lstManualControlBoard == null)
                {
                    _lstManualControlBoard = new List<ManualControlBoard>();
                }
                return _lstManualControlBoard;
            }
        }
        #region 非业务数据
        //保存状态
        private bool _isDirty = true;
        public bool IsDirty 
        {    get { return _isDirty; } 
            set
            { 
                _isDirty = value;
                if (_isDirty)
                {
                    this.Project.IsDirty = true;
                }
            } 
        }
        /// <summary>
        /// 文件版本，为兼容4，5，6文件版本保留 
        /// </summary>
        public int FileVersion { get; set; }
        #endregion 
        /// <summary>
        /// 协议版本
        /// </summary>
        public string ProtocolVersion { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get; set; }
    }
    public enum ControllerType 
    { 
        NT8001=1,
        NT8007=2,
        NT8036=3,
        NT8021=4,
        FT8000=5,
        FT8003=6,
        UNCOMPATIBLE=7, //非兼容控制器类型
        NT8053 = 8,
        NONE=0
    }


}
