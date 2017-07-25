using SCA.Model;
using System.Collections.Generic;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/10 10:30:17
* FileName   : ILinkageConfigGeneralService
* Description: 通用组态操作接口
* Version：V1
* ===============================
*/
namespace SCA.Interface.BusinessLogic
{
    public interface ILinkageConfigGeneralService
    {
        ControllerModel TheController { get; set; }
        List<LinkageConfigGeneral> Create(int amount);
        bool Update(LinkageConfigGeneral linkageConfigGeneral);
        bool DeleteBySpecifiedID(int id);

        void DownloadExecute(List<LinkageConfigGeneral> lstLinkageConfigGeneral);
    }
}
