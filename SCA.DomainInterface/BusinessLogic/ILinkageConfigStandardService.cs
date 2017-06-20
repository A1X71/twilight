using SCA.Model;
using System.Collections.Generic;

namespace SCA.Interface.BusinessLogic
{
/* ==============================
*
* Author     : William
* Create Date: 2017/4/10 10:30:17
* FileName   : ILinkageConfigGeneralService
* Description: 标准组态操作接口
* Version：V1
* ===============================
*/
    public interface ILinkageConfigStandardService
    {
        ControllerModel TheController { get; set; }
        List<LinkageConfigStandard> Create(int amount);
        bool Update(LinkageConfigStandard linkageConfigStandard);
        bool DeleteBySpecifiedID(int id);
        void DownloadExecute(List<LinkageConfigStandard> lstLinkageConfigStandard);

        //void DataRecordSetFlag(bool flag);
        /// <summary>
        /// 在控制器内是否存在相同的组号编码
        /// </summary>
        /// <param name="lstStandardConfig">标准组态集合</param>
        /// <returns></returns>
        bool IsExistSameCode(List<SCA.Model.LinkageConfigStandard> lstStandardConfig);
        
    }
}
