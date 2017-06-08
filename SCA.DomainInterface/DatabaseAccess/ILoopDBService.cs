using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface ILoopDBService:IDisposable
    {
        LoopModel GetLoopInfo(int id);
        LoopModel GetLoopInfo(LoopModel loop);
        List<LoopModel> GetLoopsByController(ControllerModel controller);
        bool AddLoopInfo(LoopModel loop);
        bool AddLoopInfo(List<LoopModel> lstLoop);

        int UpdateLoopInfo(LoopModel loop);
        bool DeleteLoopInfo(int loopID);
        bool DeleteLoopsByControllerID(int controllerID);

        int GetMaxID();

    }
}
