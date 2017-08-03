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
        /// <summary>
        /// 更新指定混合组态ID的数据
        /// </summary>
        /// <param name="id">待更新数据的ID</param>
        /// <param name="columnNames">列名</param>
        /// <param name="data">新数据</param>
        /// <returns></returns>
        bool UpdateViaSpecifiedColumnName(int id, string[] columnNames, string[] data);
        bool SaveToDB();

    }
}
