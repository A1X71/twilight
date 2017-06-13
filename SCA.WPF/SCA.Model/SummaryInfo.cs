using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/14 10:24:27
* FileName   : SummaryInfo
* Description: 用于组织汇总信息
* Version：V1
* ===============================
*/
namespace SCA.Model
{
    /// <summary>
    /// 用于组织汇总信息
    /// </summary>
    public class SummaryNodeInfo
    {
        public SummaryNodeInfo ParentNode { get; set; }
        private  List<SummaryNodeInfo> _lstChildNodes;
        public string IconPath { get; set; }
        public string DisplayName { get; set; }
        public Int16  NodeLevel { get; set; }
        public int Amount { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNumber { get; set; }
        public List<SummaryNodeInfo> ChildNodes
        {
            get
            {          
                if (_lstChildNodes == null)
                {
                    _lstChildNodes = new List<SummaryNodeInfo>();
                }
                return _lstChildNodes;
            
            }
        }
        public Dictionary<string, int> NodeAmount { get; set; }
    }
    public enum NodeCategory
    {
        回路 = 1,
        标准组态 = 2,
        混合组态 = 3,
        通用组态 = 4,
        网络手动盘 = 5,
        器件数量 = 6
    }  
}
