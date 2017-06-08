using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace SCA.Model
{
    /// <summary>
    /// 回路信息
    /// </summary>
    public class LoopModel
    {
        private ControllerModel _controller; //Added at 2016-12-06 控制器信息
        private object  _lstDevices;
        private string _iconInTree = @"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.3\SCA.WPF\SCA.WPF\Resources\Icon\Loop.jpg";        
        public LoopModel()
        { 
        
        }
 
        public LoopModel(int level,int id,string loopCode,string name,int controllerID,int deviceNum)
        {
            Level = level;
            ID = id;
            Code = loopCode;
            Name = name;
            ControllerID = controllerID;
            DeviceAmount = deviceNum;
        }
        public string IconInTree
        {
            get
            {
                return _iconInTree;
            }
            set
            {
                _iconInTree = value;
            }
        }
        /// <summary>
        /// 目录级别
        /// 页面显示应用，不需存储
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 自增主键;
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 回路编码(机号路号)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 取得回中程简码（无控制器号）
        /// </summary>
        public string SimpleCode
        {
            get
            {
                if (Controller != null && Code != null)
                {
                    return Code.Substring(Controller.MachineNumber.Length, Controller.LoopAddressLength);
                }
                return null;
            }
        }

        public string Name { get; set; }
        /// <summary>
        /// 控制器ID；外键
        /// </summary>
        public int ControllerID { get; set; }
        /// <summary>
        /// 器件数量
        /// </summary>
        public int DeviceAmount { get; set; }

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

        public void SetDevices<T>(List<T> lstDevices)
        {
            //if (_lstDevices == null)
            //{
            //    _lstDevices = new List<T>();
                _lstDevices = lstDevices;
            //}            
        }
        public void SetDevice<T>(T device)
        {
            //if (_lstDevices == null)
            //{
            //    _lstDevices = new List<T>();
            List<T> lstDevices = GetDevices<T>();
            lstDevices.Add(device);
            _lstDevices = lstDevices;
            //}            
        }

        public List<T> GetDevices<T>()
        {
            if (_lstDevices == null)
            {
                _lstDevices = new List<T>();
            }
            return (List<T>)_lstDevices;
        }
        /// <summary>
        /// 复制回路信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loop"></param>
        //public void CopyLoop<T>(LoopModel loop)
        //{ 
        //    this.IconInTree = loop.IconInTree;
        //    this.Level = loop.Level;
        //    this.ID = loop.ID;
        //    this.Code = loop.Code;
        //    this.Name = loop.Name;
        //    this.ControllerID = loop.ControllerID;
        //    this.DeviceAmount = loop.DeviceAmount;
        //    this.Controller = loop.Controller;
        //    List<T> lstDevices = loop.GetDevices<T>();            
        //    SetDevices<T>(lstDevices);            
        //}
        #region 非业务数据
        //保存状态
        private bool _isLoopDataDirty = true;
        public bool IsLoopDataDirty
        {
            get { return _isLoopDataDirty; }
            set
            {
                _isLoopDataDirty = value;
                if (_isLoopDataDirty)
                {
                    this.Controller.IsDirty = true;
                }
                else
                {
                    this.IsDeviceDataDirty = false;
                }
                
            }
        }

        private bool _isDeviceDataDirty = false;
        public bool IsDeviceDataDirty
        {
            get { return _isDeviceDataDirty; }
            set
            {
                _isDeviceDataDirty = value;
                if (_isDeviceDataDirty)
                {
                    this.Controller.IsDirty = true;
                }
            }
        }
        #endregion


    }
}
