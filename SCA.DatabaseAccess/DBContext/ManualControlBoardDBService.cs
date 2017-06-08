using System.Text;
using System.Collections.Generic;
using SCA.Interface.DatabaseAccess;

/* ==============================
*
* Author     : William
* Create Date: 2017/5/3 8:48:23
* FileName   : ManualControlBoardDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class ManualControlBoardDBService:IManualControlBoardDBService
    {
        private IDatabaseService _databaseService;
        public ManualControlBoardDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Model.ManualControlBoard GetManualControlBoardInfo(int id)
        {
            throw new System.NotImplementedException();
        }

        public Model.ManualControlBoard GetManualControlBoardInfo(Model.ManualControlBoard manualControlBoard)
        {
            throw new System.NotImplementedException();
        }

        public bool AddManualControlBoardInfo(Model.ManualControlBoard manualControlBoard)
        {
            int intEffectiveRows = 0;
            try
            {
                StringBuilder sbSQL = new StringBuilder("REPLACE INTO ManualControlBoard(ID,Code, BoardNo ,SubBoardNo ,KeyNo , DeviceCode , SDPKey ,controllerID) ");
                sbSQL.Append("VALUES(");
                sbSQL.Append(manualControlBoard.ID + ",");
                sbSQL.Append(manualControlBoard.Code + ",'");
                sbSQL.Append(manualControlBoard.BoardNo + "','");
                sbSQL.Append(manualControlBoard.SubBoardNo + "','");
                sbSQL.Append(manualControlBoard.KeyNo + "','");
                sbSQL.Append(manualControlBoard.DeviceCode + "','");
                sbSQL.Append(manualControlBoard.SDPKey + "','");
                sbSQL.Append(manualControlBoard.ControllerID + "');");
                intEffectiveRows = _databaseService.ExecuteBySql(sbSQL);
            }
            catch
            {
                intEffectiveRows = 0;
            }
            if (intEffectiveRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddManualControlBoardInfo(List<Model.ManualControlBoard> lstManualControlBoard)
        {
            try
            {
                foreach (var controlBoard in lstManualControlBoard)
                {
                    AddManualControlBoardInfo(controlBoard);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateManualControlBoardInfo(Model.ManualControlBoard lstManualControlBoard)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteManualControlBoardInfo(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
