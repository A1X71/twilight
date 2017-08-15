/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 159 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-25 10:23:50 +0800 (周二, 25 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neat.Dennis.Common.LoggerManager
{
    public enum NeatLogLevel
    {
        ERROR = 0x11170, //70000
        WARN = 0xea60, //60000
        INFO = 0x9c40, //40000
        DEBUG = 0x7530, //30000
    }
}
