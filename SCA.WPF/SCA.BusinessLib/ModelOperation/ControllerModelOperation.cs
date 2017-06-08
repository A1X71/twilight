using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2016/11/11 14:51:54
* FileName   : ControllerModelOperation
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ModelOperation
{
    public class ControllerModelOperation
    {
        public List<ControllerModel> GetControllers()
        {
            List<ControllerModel> lstControllerModel = new List<ControllerModel>();
            lstControllerModel.Add(new ControllerModel(1, "NT8036", ControllerType.NT8036,3));
            lstControllerModel.Add(new ControllerModel(1, "NT8001", ControllerType.NT8001,3));
            return lstControllerModel;
        }
        public ControllerModel GetControllersBySpecificID(int id)
        {
            List<ControllerModel> lstControllerModel = new List<ControllerModel>();
            lstControllerModel.Add(new ControllerModel(1, "NT8036", ControllerType.NT8036,3));
            lstControllerModel.Add(new ControllerModel(1, "NT8001", ControllerType.NT8001,3));
            var result= from c in lstControllerModel where c.ID == id select c;
            return result.FirstOrDefault();
        }
    }
}
