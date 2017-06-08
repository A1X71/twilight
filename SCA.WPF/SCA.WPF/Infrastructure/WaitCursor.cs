using System.Windows.Input;
using System;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/4 15:54:29
* FileName   : WaitCursor
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Infrastructure
{
    public class WaitCursor:IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
