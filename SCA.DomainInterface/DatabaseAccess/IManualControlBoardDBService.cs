using System.Collections.Generic;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface IManualControlBoardDBService
    {
        ManualControlBoard GetManualControlBoardInfo(int id);
        ManualControlBoard GetManualControlBoardInfo(ManualControlBoard manualControlBoard);
        bool AddManualControlBoardInfo(ManualControlBoard manualControlBoard);
        bool AddManualControlBoardInfo(List<ManualControlBoard> lstManualControlBoard);

        int UpdateManualControlBoardInfo(ManualControlBoard lstManualControlBoard);
        bool DeleteManualControlBoardInfo(int id);
    }
}
