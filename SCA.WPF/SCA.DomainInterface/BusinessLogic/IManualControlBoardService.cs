using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.BusinessLogic
{
    public interface IManualControlBoardService
    {
        ControllerModel TheController { get; set; }
        List<ManualControlBoard> Create(int amount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardNo">板号</param>
        /// <param name="subBoardStartNo">盘号起始值</param>        
        /// /// <param name="subBoardEndNo">盘号终止值</param>        
        /// <param name="startKeyNo">键号起始值</param>
        /// /// <param name="amount">键号数量</param>
        /// <returns></returns>
        List<ManualControlBoard> Create(int boardNo, int subBoardStartNo, int subBoardEndNo,  int amount);
        bool Update(ManualControlBoard mcb);

        bool DeleteBySpecifiedID(int id);

        void DownloadExecute(List<ManualControlBoard> lstManualControlBoard);
        /// <summary>
        /// 更新指定网络手动盘ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data);
        bool SaveToDB();
    }
}
