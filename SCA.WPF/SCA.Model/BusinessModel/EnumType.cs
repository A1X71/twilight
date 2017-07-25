using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/25 11:46:23
* FileName   : EnumType
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model.BusinessModel
{
    /// <summary>
    /// 输入器件查询类型
    /// </summary>
    public enum QueryType
    {
        QueryByLoop = 0,
        QueryByBuildingNo = 1,
        QueryByType = 2,
        QueryByLoopAndBuildingNo = 3,
        QueryByLoopAndType = 4,
        QueryByBuildingNoAndType = 5,
        QueryByLoopAndBuildingNoAndType = 6,
        QueryByNothing = 7,
        QueryByDeviceCode = 8,
        QueryByLoopAndDeviceCode = 9,
        QueryByTypeAndDeviceCode = 10,
        QueryByLoopAndTypeAndDeviceCode = 11,
    }
}
