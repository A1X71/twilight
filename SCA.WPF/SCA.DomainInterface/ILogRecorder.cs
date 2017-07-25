using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/10/22 15:31:38
* FileName   : ILogRecorder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Interface
{
    public interface ILogRecorder
    {
        void WriteException(Exception exception);
        void WriteLog(Exception exception);
        void InitialLog(string strPath);

    }
}
