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
*  $Revision: 249 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-09 13:11:06 +0800 (周三, 09 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neat.Dennis.Connection
{
    /// <summary>
    /// 接收到的NTP消息
    /// </summary>
    public class NTPReceiveMsg
    {
        #region Property
        /// <summary>
        /// 存储接收到的NTP
        /// </summary>
        public NTP ntp = new NTP();

        /// <summary>
        /// 是否解析完NTP头
        /// </summary>
        public bool IsReadNTPHeaderFinished = false;

        /// <summary>
        /// 读取完整的头部信息后 读取的数据信息数据
        /// </summary>
        public byte[] AlreadyReadBytes;

        /// <summary>
        /// 是否解析完NTP消息
        /// </summary>
        public bool IsReadNTPFinished = false;

        /// <summary>
        /// 是否可以继续解析  上一条数据解析完成并且有多余数据
        /// </summary>
        public bool IsCanRunNext = true;

        /// <summary>
        /// 接收到的消息临时存储区
        /// </summary>
        public byte[] TempReceiveBytes;

        /// <summary>
        /// 接收到的消息的多余数据存储区
        /// </summary>
        public byte[] OverReceiveBytes;


        #endregion Property
    }
}
