using System.Collections.Generic;
using System.Text;
using System.Data;
using System;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;

/* ==============================
*
* Author     : William
* Create Date: 2017/7/17 14:21:55
* FileName   : DBFileVersionBaseService
* Description: DBFileVersion的基类
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{    
    public class DBFileVersionBaseService//:IDisposable
    {
        //private IDatabaseService _databaseService;
        //public DBFileVersionBaseService()
        //{
 
        //}
        //public DBFileVersionBaseService(string dataSource,ILogRecorder logger,IFileService fileService)
        //{
        //    _databaseService = new SCA.DatabaseAccess.MSAccessDatabaseAccess(dataSource, logger, fileService);            
        //}
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
        //public void Dispose()
        //{
        //    _databaseService.Dispose();
        //}
        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="filePath"></param>
        protected string GetFileName(string filePath)
        {
            if(filePath!=null)
            {
                if(filePath!="")
                {
                    int startPosition = filePath.LastIndexOf('\\') < 0 ? 0 : filePath.LastIndexOf('\\')+1;
                    int endPosition=filePath.LastIndexOf('.');
                    int charLength = endPosition - startPosition;
                    return filePath.Substring(startPosition, charLength < 0 ? filePath.Length : charLength);
                }
            }
            return "";
        }
        protected ControllerType ControllerTypeConverter(string typeInfo)
        {
            switch (typeInfo)
            {
                case "8000":
                    return ControllerType.FT8000;
                case "8003":
                    return ControllerType.FT8003;
                case "8021":
                    return ControllerType.NT8021;
                case "8036":
                    return ControllerType.NT8036;
                case "8007":
                    return ControllerType.NT8007;
                case "8001":
                    return ControllerType.NT8001;
                default:
                    return ControllerType.NONE;
            }
        }
    }
}
