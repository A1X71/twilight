﻿/**************************************************************************
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
*  $Revision: 264 $
*  $Author: william_wang $        
*  $Date: 2017-08-15 11:06:08 +0800 (周二, 15 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Model
{
    public class DeviceInfo8053 : DeviceInfoBase, IDevice
    {
        public LoopModel Loop { get; set; }             //回路
        public int ID { get; set; }                     //标识ID
        public string MachineNo { get; set; }           //机号
        public int LoopID { get; set; }                 //回路ID
        public string Code { get; set; }                //器件编码  (机号+路号+器件编号):七位示例->0102003; 八位示例->00102003
        public Int16 TypeCode { get; set; }             //器件类型
		public Int16? SensitiveLevel { get; set; }      //灵敏性
        //屏蔽
        private bool _disable = false;  //增加默认值  by william at 2017-08-09
        public bool Disable {
            get
            {
                return _disable;
            }
            set
            {
                _disable = value;
            }
        
        } 
        public Int16? DelayValue { get; set; }          //延时
        public Int16? Feature { get; set; }             //特性        器件内部编码
        public Int16? BuildingNo { get; set; }          //楼号
        public Int16? ZoneNo { get; set; }              //区号
        public Int16? FloorNo { get; set; }             //层号
        public Int16? RoomNo { get; set; }              //房间号
        public string Location { get; set; }            //安装地点
        public string LinkageGroup1 { get; set; }       //输出组1
        public string LinkageGroup2 { get; set; }       //输出组2
        public string LinkageGroup3 { get; set; }       //输出组3

        public string SimpleCode                        //无机号及路号的编码
        {
            get
            {
                if (Loop != null && Code != null)
                {
                    return Code.Substring(Loop.Code.Length, 3);
                }
                return string.Empty;
            }
        }          
    }
}
