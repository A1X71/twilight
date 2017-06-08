using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface IControllerDBService:IDisposable
    {

        List<ControllerModel> GetControllersByProject(ProjectModel project);
        ControllerModel GetController(int id);

        ControllerModel GetController(ControllerModel controller);

        bool AddController(ControllerModel controller);

        int UpdateController(ControllerModel controller);

        bool DeleteController(int ControllerID);
        int GetMaxID();

    }
}
