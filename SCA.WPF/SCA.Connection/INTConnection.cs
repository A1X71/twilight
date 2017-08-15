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
*  $Revision: 250 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-11 15:33:06 +0800 (周五, 11 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;

namespace Neat.Dennis.Connection
{
    /// <summary>
    /// Neat 通讯协议接口
    /// </summary>
    public interface INTConnection
    {
        /// <summary>
        /// 是否连接
        /// </summary>
        bool GetIsConnected();

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="SerialName">串口名称</param>
        /// <param name="Baudrate">波特率</param>
        /// <returns>是否成功</returns>
        bool Connect(string SerialName, int Baudrate);

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>是否成功</returns>
        bool Disconnect();

        /// <summary>
        /// 设置器件配置
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool SetDeviceSetup(List<DeviceInfoBase> list, int deviceCount, ControllerType type);

        /// <summary>
        /// 设置标准组态配置
        /// </summary>
        /// <param name="bytes"></param>
        bool SetStandardConfigSetup(List<LinkageConfigStandard> list, ControllerType type);

        /// <summary>
        /// 设置混合组态配置
        /// </summary>
        /// <param name="bytes"></param>
        bool SetMixedConfigSetup(List<LinkageConfigMixed> list, ControllerType type);

        /// <summary>
        /// 设置通用组态配置
        /// </summary>
        /// <param name="bytes"></param>
        bool SetGeneralConfigSetup(List<LinkageConfigGeneral> list, ControllerType type);

        /// <summary>
        /// 设置手控盘配置
        /// </summary>
        /// <param name="bytes"></param>
        bool SetManualBoardSetup(List<ManualControlBoard> list, ControllerType type);

        /// <summary>
        /// 设置设备所有配置
        /// </summary>
        /// <param name="bytes"></param>
        bool SetMachineSetup(ControllerModel model, ControllerType type);

        /// <summary>
        /// 获取设备所有信息
        /// </summary>
        /// <returns></returns>
        ControllerModel GetMachineSetup(ControllerType type);

    }
}
