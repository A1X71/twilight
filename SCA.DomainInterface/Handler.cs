using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Interface
{
    public  delegate void COMOpenHandler(ICom com, int port, int baud, bool openSuccess);

    public  delegate void COMCloseHandler(ICom com, int port, int baud, bool closeSuccess);

    public  delegate void COMErrorHandler(ICom com, int port, int baud, string error);
}
