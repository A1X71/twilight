using SCA.Model;
using System.Collections.Generic;

namespace SCA.Interface.BusinessLogic
{
/* ==============================
*
* Author     : William
* Create Date: 2017/4/10 10:30:17
* FileName   : ILinkageConfigGeneralService
* Description: 混合组态操作接口
* Version：V1
* ===============================
*/
    public interface ILinkageConfigMixedService
    {
        ControllerModel TheController { get; set; }
        List<LinkageConfigMixed> Create(int amount);
        bool Update(LinkageConfigMixed linkageConfigMixed);
        bool DeleteBySpecifiedID(int id);
        void DownloadExecute(List<LinkageConfigMixed> lstLinkageConfigMixed);
    }
}
