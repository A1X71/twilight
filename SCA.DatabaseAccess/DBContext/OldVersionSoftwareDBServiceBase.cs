using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 14:07:39
* FileName   : OldVersionSoftwareDBServiceBase
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class OldVersionSoftwareDBServiceBase:IDisposable
    {
        private IDatabaseService _databaseService;
        public int Version { get; set; }
        public OldVersionSoftwareDBServiceBase(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public List<LoopModel> GetLoopsInfo()
        {
            DataTable dt = null;
            List<LoopModel> lstLoopInfo = new List<LoopModel>();
            StringBuilder sbQuerySQL = new StringBuilder("select 回路,总数 from 系统设置 order by 回路;");
            dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            int dtRowsCount = dt.Rows.Count;
            for (int i = 0; i < dtRowsCount; i++)//回路信息
            {
                LoopModel loop = new LoopModel();
                loop.Name = dt.Rows[i]["回路"].ToString();
                loop.Code = dt.Rows[i]["回路"].ToString();
                loop.DeviceAmount = Convert.ToInt16(dt.Rows[i]["总数"] == null ? 0 : dt.Rows[i]["总数"]);
                lstLoopInfo.Add(loop);
            }
            return lstLoopInfo;
        }
        /// <summary>
        /// 取得控制器名称及类型 
        /// 返回格式：控制器类型;文件版本
        /// </summary>
        /// <param name="strPath">文件地址</param>
        /// <returns>控制器类型;文件版本</returns>
        public string[] GetFileVersionAndControllerType()
        {
            //1.读"文件配置"表，取得（文件版本，控制器类型）
            //2.根据控制器类型，初始化控制器配置信息，取得相应“文件版本”至目的版本之间需要执行升级的操作内容
            StringBuilder sbQuerySQL = new StringBuilder("select 文件版本,控制器类型 from 文件配置;");
            DataTable dtFile = _databaseService.GetDataTableBySQL(sbQuerySQL);
            string strResult = "";
            if (dtFile != null)
            {
                if (dtFile.Rows.Count > 0)
                {
                    strResult = dtFile.Rows[0]["控制器类型"].ToString() + ";" + dtFile.Rows[0]["文件版本"].ToString();
                }
            }
            return strResult.Split(';');
        }

        public void Dispose()
        {
            _databaseService.Dispose();
        }
        /// <summary>
        /// 将字符型“动作类型”转换为枚举值
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        protected Model.LinkageActionType ConvertLinkageActionType(string actionType)
        {
            switch (actionType)
            {
                case "或":
                    return LinkageActionType.OR;
                case "与":
                    return LinkageActionType.AND;
                default:
                    return LinkageActionType.NONE;
            }
        }
        /// <summary>
        /// 将字符型联动类型转换为枚举值 
        /// </summary>
        /// <param name="linkageType"></param>
        /// <returns></returns>
        protected Model.LinkageType ConvertLinkageType(string linkageType)
        {

            switch (linkageType)
            {
                case "区层":
                    return Model.LinkageType.ZoneLayer;
                case "地址":
                    return Model.LinkageType.Address;
                case "本层":
                    return Model.LinkageType.SameLayer;
                case "邻层":
                    return Model.LinkageType.AdjacentLayer;
                default:
                    return LinkageType.None;
            }
        }
    }
}
