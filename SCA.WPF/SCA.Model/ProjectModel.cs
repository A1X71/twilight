using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Model
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectModel
    {
      //  public virtual List<> Reviews{get;set;}
        private List<ControllerModel> _lstControllers;
        private bool _saveStatus=false;
        private string _iconInTree = @"Resources\Icon\Style1\Proj.jpg";        
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        
        public ProjectModel()
        {
            
        }
        public ProjectModel(int id,string name,int level){
            ID = id;
            Name = name;
            Level = level;
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
        public int ID { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 自动保存时间
        /// 单位：分钟
        /// </summary>
        public int SaveInterval { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public int FileVersion { get; set; }
        /// <summary>
        /// 保存文件路径
        /// </summary>
        //public string SaveFilePath { get; set; }
        

        public override string ToString()
        {
            return string.Format("[ID:{0}]--《Name:{1}》",ID,Name);
        }

        public List<ControllerModel> Controllers
        {
            get
            {
                if (_lstControllers == null)
                {
                    _lstControllers = new List<ControllerModel>();
                }
                return _lstControllers;
            }
            //private set
            //{
            //    if (_lstControllers == null)
            //    {
            //        _lstControllers = new List<ControllerModel>();
            //        _lstControllers = value;
            //    }
            //}
        }
        #region 非数据表字段
        /// <summary>
        /// 目录级别
        /// 页面显示应用，不需存储
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 保存状态
        /// true--已保存; false--未保存
        /// </summary>
        public bool SaveStatus 
        {
            get
            {
                return _saveStatus;
            }
            set
            {
                _saveStatus = value;
            }
        }
        //保存状态
        private bool _isDirty=true;
        public bool IsDirty 
        { 
            get { return _isDirty; } 
            set 
            {
                _isDirty = value;
                if (!_isDirty)//project已全部保存
                {
                    foreach (var c in Controllers)
                    {
                        c.IsDirty = false;
                        foreach (var l in c.Loops)
                        {
                            l.IsLoopDataDirty = false;
                        }
                        foreach (var r in c.StandardConfig)
                        {
                            r.IsDirty = false;
                        }
                        foreach (var r in c.MixedConfig)
                        {
                            r.IsDirty = false;
                        }
                        foreach (var r in c.GeneralConfig)
                        {
                            r.IsDirty = false;
                        }
                        foreach (var r in c.ControlBoard)
                        {
                            r.IsDirty = false;
                        }
                    }
                }
            
            }
        }

        #endregion
        //public List<ControllerModel> Controllers
        //{
        //    get { return _controllers; }
        //}
    }
}
