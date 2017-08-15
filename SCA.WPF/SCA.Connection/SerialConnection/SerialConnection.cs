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
using Neat.Dennis.Common.LoggerManager;
using System.Reflection;
using System.Threading;
using SCA.Model;

namespace Neat.Dennis.Connection
{
    /// <summary>
    /// 串口通信协议
    /// </summary>
    public class SerialConnection : INTConnection
    {
        #region Property
        /// <summary>
        /// 日志
        /// </summary>
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 连接状态
        /// </summary>
        private static bool IsConnected = false;

        #endregion Property

        #region Interface Method
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="SerialName">串口名称</param>
        /// <param name="Baudrate">波特率</param>
        /// <returns>是否成功</returns>
        bool INTConnection.Connect(string SerialName, int Baudrate)
        {
            IsConnected = SerialClient.Connect(SerialName, Baudrate);
            return IsConnected;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>是否成功</returns>
        bool INTConnection.Disconnect()
        {
            bool result = SerialClient.Disconnect();
            if (result)
            {
                IsConnected = false;
            }
            return result;
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        bool INTConnection.GetIsConnected()
        {
            return IsConnected;
        }

        /// <summary>
        /// 设置器件信息
        /// </summary>
        /// <param name="list">器件列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetDeviceSetup(List<DeviceInfoBase> list, int deviceCount, ControllerType type)
        {
            bool isSuccess = false;
            try
            {
                if (!IsConnected)
                {
                    return false;
                }
                List<NTP> ntpList = new List<NTP>();
                foreach (var item in list)
                {
                    NTP ntp = NTPBuildRequest.GetSetDeviceInfoRequest(item, deviceCount, type);
                    if (ntp != null)
                    {
                        ntpList.Add(ntp);
                    }
                }
                isSuccess = SerialClient.ExecuteSetTask(ntpList, type);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return isSuccess;
        }

        /// <summary>
        /// 设置标准组态信息
        /// </summary>
        /// <param name="list">标准组态列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetStandardConfigSetup(List<LinkageConfigStandard> list, ControllerType type)
        {
            if (!IsConnected)
            {
                return false;
            }
            List<NTP> ntpList = new List<NTP>();
            foreach (var item in list)
            {
                NTP ntp = NTPBuildRequest.GetSetStandardConfigRequest(item, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }
            return SerialClient.ExecuteSetTask(ntpList, type);
        }

        /// <summary>
        ///  设置混合组态信息
        /// </summary>
        /// <param name="list">混合组态列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetMixedConfigSetup(List<LinkageConfigMixed> list, ControllerType type)
        {
            if (!IsConnected)
            {
                return false;
            }
            List<NTP> ntpList = new List<NTP>();
            foreach (var item in list)
            {
                NTP ntp = NTPBuildRequest.GetSetMixedConfigRequest(item, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }
            return SerialClient.ExecuteSetTask(ntpList, type);
        }

        /// <summary>
        /// 设置通用组态信息
        /// </summary>
        /// <param name="list">通用组态列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetGeneralConfigSetup(List<LinkageConfigGeneral> list, ControllerType type)
        {
            if (!IsConnected)
            {
                return false;
            }
            List<NTP> ntpList = new List<NTP>();
            foreach (var item in list)
            {
                NTP ntp = NTPBuildRequest.GetSetGeneralConfigRequest(item, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }
            return SerialClient.ExecuteSetTask(ntpList, type);
        }

        /// <summary>
        /// 设置手控盘信息
        /// </summary>
        /// <param name="list">手控盘列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetManualBoardSetup(List<ManualControlBoard> list, ControllerType type)
        {
            if (!IsConnected)
            {
                return false;
            }
            List<NTP> ntpList = new List<NTP>();
            foreach (var item in list)
            {
                NTP ntp = NTPBuildRequest.GetSetManualBoardRequest(item, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }
            return SerialClient.ExecuteSetTask(ntpList, type);
        }

        /// <summary>
        /// 设置控制器信息
        /// </summary>
        /// <param name="model">控制器信息</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public bool SetMachineSetup(ControllerModel model, ControllerType type)
        {
            if (!IsConnected)
            {
                return false;
            }
            List<NTP> ntpList = new List<NTP>();
            //器件
            int deviceCount = 0;
            foreach(var loop in model.Loops)
            {
                deviceCount = loop.DeviceAmount;
                List<DeviceInfo8053> list = loop.GetDevices<DeviceInfo8053>();
                foreach (var item in list)
                {
                    NTP ntp = NTPBuildRequest.GetSetDeviceInfoRequest(item, deviceCount, type);
                    if (ntp != null)
                    {
                        ntpList.Add(ntp);
                    }
                }
            }

            //标准组态
            foreach (var standard in model.StandardConfig)
            {
                NTP ntp = NTPBuildRequest.GetSetStandardConfigRequest(standard, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }

            //混合组态
            foreach (var mixed in model.MixedConfig)
            {
                NTP ntp = NTPBuildRequest.GetSetMixedConfigRequest(mixed, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }

            //通用组态
            foreach (var general in model.GeneralConfig)
            {
                NTP ntp = NTPBuildRequest.GetSetGeneralConfigRequest(general, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }

            //手控盘
            foreach (var manual in model.ControlBoard)
            {
                NTP ntp = NTPBuildRequest.GetSetManualBoardRequest(manual, type);
                if (ntp != null)
                {
                    ntpList.Add(ntp);
                }
            }
            
            return SerialClient.ExecuteSetTask(ntpList, type);
        }

        /// <summary>
        /// 获取控制器信息
        /// </summary>
        /// <param name="type">控制器类型</param>
        /// <returns>控制器信息</returns>
        public ControllerModel GetMachineSetup(ControllerType type)
        {
            if (!IsConnected)
            {
                return null;
            }
            ControllerModel model = new ControllerModel();
            List<NTP> ntpList = SerialClient.ExecuteGetTask(type);
            if(ntpList != null)
            {
                foreach(NTP ntp in ntpList)
                {
                    switch(ntp.Command)
                    {
                        case CommandType.DeviceUp:
                            NTPParseResponse.SetDeviceInfoToControllerResponse(model, type, ntp);
                            break;
                        case CommandType.GeneralUp:
                            NTPParseResponse.SetGeneralConfigToControllerResponse(model, type, ntp);
                            break;
                        case CommandType.StandardUp:
                            NTPParseResponse.SetStandardConfigToControllerResponse(model, type, ntp);
                            break;
                        case CommandType.MixedUp:
                            NTPParseResponse.SetMixedConfigToControllerResponse(model, type, ntp);
                            break;
                        case CommandType.ManualUp:
                            NTPParseResponse.SetManualBoardToControllerResponse(model, type, ntp);
                            break;
                    }
                }
            }

            return model;
        }
        
        #endregion Interface Method
    }
}
