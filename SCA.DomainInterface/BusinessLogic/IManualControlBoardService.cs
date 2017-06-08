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
        List<ManualControlBoard> Create(int boardNo, int subBoardStartNo, int subBoardEndNo, int startKeyNo, int amount);
        bool Update(ManualControlBoard mcb);

        bool DeleteBySpecifiedID(int id);

        void DownloadExecute(List<ManualControlBoard> lstManualControlBoard);

    }
}
