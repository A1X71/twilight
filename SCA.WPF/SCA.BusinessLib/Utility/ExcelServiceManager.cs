using SCA.Interface;

/* ==============================
*
* Author     : William
* Create Date: 2017/6/22 14:26:11
* FileName   : ExcelServiceManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public class ExcelServiceManager
    {
        public static IExcelService GetExcelService(EXCELVersion excel,string strPath,IFileService fileService)
        {
            switch (excel)
            { 
                case EXCELVersion.EXCEL2003:
                    return new Excel2003Service(strPath, fileService);                    
                case EXCELVersion.EXCEL2007:
                    return new Excel2007Service(strPath, fileService);                    
            }
            return null;
        }
    }
}
