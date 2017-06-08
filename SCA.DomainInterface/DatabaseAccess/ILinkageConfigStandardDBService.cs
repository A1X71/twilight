using System.Text;
using System.Collections.Generic;
using SCA.Model;

/* ==============================
*
* Author     : William
* Create Date: 2017/5/2 16:57:06
* FileName   : ILinkageConfigStandardDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Interface.DatabaseAccess
{
    public interface ILinkageConfigStandardDBService
    {
  
        LinkageConfigStandard GetStandardLinkageConfigInfo(int id);
        LinkageConfigStandard GetStandardLinkageConfigInfo(LinkageConfigStandard linkageConfigStandard);

        List<Model.LinkageConfigStandard> GetStandardLinkageConfigInfoByController(Model.ControllerModel controller);
        bool AddStandardLinkageConfigInfo(LinkageConfigStandard linkageConfigStandard);
        bool AddStandardLinkageConfigInfo(List<LinkageConfigStandard> lstLinkageConfigStandard);

        int UpdateStandardLinkageConfigInfo(LinkageConfigStandard lstLinkageConfigStandard);
        bool DeleteStandardLinkageConfigInfo(int id);
    }
}
