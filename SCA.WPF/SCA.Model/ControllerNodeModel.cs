using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace SCA.Model
{
    /// <summary>
    /// 控制器节点
    /// 用于树型节点显示
    /// </summary>
    public  class ControllerNodeModel
    {
        private string _iconInTree;
        public string IconInTree
        {
            get
            {
                //Application.StartupPath
                return System.AppDomain.CurrentDomain.BaseDirectory+_iconInTree;
            }
            set
            {
                _iconInTree = value;
            }
        }
        public ControllerNodeModel(ControllerNodeType type,string name,int level,string iconPath)
        {
            Type = type;
            Name = name;
            Level = level;
            _iconInTree = iconPath;
        }
        /// <summary>
        /// 目录级别
        /// 页面显示应用，不需存储
        /// </summary>
        public int Level { get; set; }
        public string Name { get; set; }
        public ControllerNodeType Type { get; set; }
        /// <summary>
        /// 节点下的数据数量
        /// </summary>
        public int Amount { get; set; }

    }
    
    
    /// <summary>
    /// 控制器节点类型
    /// </summary>
   [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ControllerNodeType
    {
       [Description("回路")]
       Loop,
       [Description("标准组态")]
       Standard,
       [Description("混合组态")]
       Mixed,
       [Description("通用组态")]
       General,       
       [Description("网络手动盘")]
       Board
    }
}
