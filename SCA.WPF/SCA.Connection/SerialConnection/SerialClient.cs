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
using System.Reflection;
using System.Text;
using System.Threading;
using Neat.Dennis.Common.LoggerManager;
using SCA.Model;

namespace Neat.Dennis.Connection
{
    public class SerialClient
    {
        #region Property
        /// <summary>
        /// 日志
        /// </summary>
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 串口
        /// </summary>
        private static SerialBase conn = null;

        /// <summary>
        /// 串口名称
        /// </summary>
        private static string SerialName = string.Empty;

        /// <summary>
        /// 波特率
        /// </summary>
        private static int BaudRate = 9600;

        /// <summary>
        /// 串口锁
        /// </summary>
        private static object SerialLocker = new object();

        /// <summary>
        /// 当前的NTP列表 发送的任务列表
        /// </summary>
        private static List<NTP> CurrentNTPList = null;

        /// <summary>
        /// 当前的控制器类型
        /// </summary>
        private static ControllerType CurrentContrllerType = ControllerType.UNCOMPATIBLE;

        /// <summary>
        /// 当前的设备版本   
        /// </summary>
        public static ControllerVersion CurrentControllerVersion = ControllerVersion.NewVersion8;

        /// <summary>
        /// 当前的执行状态
        /// </summary>
        private static ExecuteState CurrentExecuteState = ExecuteState.Untreated;

        /// <summary>
        /// 当前的完成状态
        /// </summary>
        private static FinishState CurrentFinishState = FinishState.Unfinished;

        /// <summary>
        /// 最大的错误数
        /// </summary>
        private static int ErrorMaxCount = 5;

        #endregion Property

        #region Public Method
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="serialName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        public static bool Connect(string serialName, int baudRate)
        {
            SerialName = serialName;
            BaudRate = baudRate;
            if (conn == null)
            {
                conn = new SerialBase();
                conn.ReceiveMsgEvent += conn_ReceiveMsgEvent;
            }
            return conn.Connect(SerialName, BaudRate);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public static bool Disconnect()
        {
            bool result = true;
            if (conn != null)
            {
                result = conn.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// 执行设置任务
        /// </summary>
        /// <param name="list">NTP协议列表</param>
        /// <param name="type">控制器类型</param>
        /// <returns>是否成功</returns>
        public static bool ExecuteSetTask(List<NTP> list, ControllerType type)
        {
            bool resutl = false;
            try
            {
                logger.Debug("Start task to set!");
                lock (SerialLocker)
                {
                    if (CurrentExecuteState != ExecuteState.Untreated)
                    {
                        return false;
                    }
                    CurrentContrllerType = type;
                    CurrentNTPList = list;
                    CurrentExecuteState = ExecuteState.SetInvalidType;
                }
                while (true)
                {
                    Thread.Sleep(100);
                    lock(SerialLocker)
                    {
                        if (CurrentFinishState == FinishState.Succeessed)
                        {
                            logger.Debug("successed");
                            resutl = true;
                            CurrentFinishState = FinishState.Unfinished;
                            break;
                        }
                        else if (CurrentFinishState == FinishState.Failed)
                        {
                            logger.Error("failed");
                            resutl = false;
                            CurrentFinishState = FinishState.Unfinished;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return resutl;
        }

        /// <summary>
        /// 执行获取任务
        /// </summary>
        /// <param name="type">控制器类型</param>
        /// <returns>NTP协议列表</returns>
        public static List<NTP> ExecuteGetTask(ControllerType type)
        {
            List<NTP> list = new List<NTP>();
            try
            {
                logger.Debug("Start task to get!");
                lock (SerialLocker)
                {
                    if (CurrentExecuteState != ExecuteState.Untreated)
                    {
                        return null;
                    }
                    CurrentContrllerType = type;
                    CurrentNTPList = new List<NTP>();
                    CurrentExecuteState = ExecuteState.GetInvalidType;
                }
                while (true)
                {
                    Thread.Sleep(100);
                    lock (SerialLocker)
                    {
                        if (CurrentFinishState == FinishState.Succeessed)
                        {
                            logger.Debug("Successed");
                            list = CurrentNTPList;
                            CurrentNTPList = null;
                            CurrentFinishState = FinishState.Unfinished;
                            break;
                        }
                        else if (CurrentFinishState == FinishState.Failed)
                        {
                            logger.Error("Failed");
                            list = null;
                            CurrentNTPList = null;
                            CurrentFinishState = FinishState.Unfinished;
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return list;
        }

        #endregion Public Method
        

        #region Private Method
        /// <summary>
        /// 接受到消息回应
        /// </summary>
        /// <param name="ntp"></param>
        private static void conn_ReceiveMsgEvent(NTP ntp)
        {
            try
            {
                lock(SerialLocker)
                {
                    switch(CurrentExecuteState)
                    {
                        case ExecuteState.SetInvalidType:
                        case ExecuteState.SetState:
                        case ExecuteState.SetOver:
                            ExecuteSet(ntp);
                            break;
                        case ExecuteState.GetInvalidType:
                        case ExecuteState.GetState:
                            ExecuteGet(ntp);
                            break;
                        case ExecuteState.Untreated:
                        default:
                            //不处理任何事情
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 执行设置指令
        /// </summary>
        private static void ExecuteSet(NTP ntp)
        {
            switch (ntp.PState)
            {
                case NTPState.Successed:
                    switch(CurrentExecuteState)
                    {
                        case ExecuteState.SetState:                                 //移除已经发送的指令 等待巡检 发送 发送完毕
                            CurrentExecuteState = ExecuteState.SetOver;
                            break;
                        case ExecuteState.SetOver:                                  //等待巡检 发送下一条指令
                            if (CurrentNTPList != null && CurrentNTPList.Count > 0)
                            {
                                CurrentNTPList.RemoveAt(0);
                            }
                            if (CurrentNTPList != null && CurrentNTPList.Count > 0)
                            {
                                CurrentExecuteState = ExecuteState.SetState;  
                            }
                            else
                            {
                                CurrentFinishState = FinishState.Succeessed;
                                CurrentExecuteState = ExecuteState.Untreated;
                            }
                            break;
                    }
                    break;
                case NTPState.Failed:
                    if (ErrorMaxCount <= 0)         //暂未使用
                    {
                        CurrentExecuteState = ExecuteState.Untreated;
                        CurrentFinishState = FinishState.Failed;
                        logger.Error("控制器验证错误数量太多！");
                    }
                    break;
                case NTPState.Normal:
                    switch (ntp.Command)
                    {
                        case CommandType.MachineTypeResponose:      //控制器类型
                            if (CheckIsValidType(ntp))
                            {
                                logger.Debug("machine type matched!");
                                CurrentExecuteState = ExecuteState.SetState;
                                NTP request = NTPBuildRequest.GetConfirmRequest(true);
                                SendCommand(request);
                            }
                            else
                            {
                                CurrentExecuteState = ExecuteState.Untreated;
                                CurrentFinishState = FinishState.Failed;
                                logger.Error("machine type unmatched!");
                            }
                            break;
                        case CommandType.Patrol:                    //巡检
                            ExecuteStep();
                            break;

                    }
                    break;
            }
        }

        /// <summary>
        /// 执行步骤
        /// </summary>
        private static void ExecuteStep()
        {
            NTP request = null;
            switch(CurrentExecuteState)
            {
                case ExecuteState.SetInvalidType:                   //获取控制器类型
                    request = NTPBuildRequest.GetControllerTypeRequest();          
                    SendCommand(request);
                    break;
                case ExecuteState.SetOver:                          //单步完成  发送 发送完毕 指令
                    request = NTPBuildRequest.GetSendOverRequest();                
                    SendCommand(request);
                    break;
                case ExecuteState.SetState:                         //发送请求指令 如果没有请求 结束
                    if (CurrentNTPList != null && CurrentNTPList.Count > 0)
                    {
                        request = CurrentNTPList[0];
                        SendCommand(request);
                    }
                    else
                    {
                        CurrentFinishState = FinishState.Succeessed;
                        CurrentExecuteState = ExecuteState.Untreated;
                    }
                    break;
            }
        }

        /// <summary>
        /// 执行获取指令
        /// </summary>
        private static void ExecuteGet(NTP ntp)
        {
            NTP request = null;
            switch (ntp.PState)
            {
                case NTPState.Successed:
                case NTPState.Failed:
                    break;
                case NTPState.Normal:
                    switch (ntp.Command)
                    {
                        case CommandType.MachineTypeResponose:          //控制器类型
                            if (CheckIsValidType(ntp))
                            {
                                logger.Debug("machine type matched!");
                                CurrentExecuteState = ExecuteState.GetState;
                                request = NTPBuildRequest.GetConfirmRequest(true);
                                SendCommand(request);
                            }
                            else
                            {
                                CurrentExecuteState = ExecuteState.Untreated;
                                CurrentFinishState = FinishState.Failed;
                                logger.Error("machine type unmatched!");
                            }
                            break;
                        case CommandType.Patrol:                    //巡检
                            break;
                        case CommandType.Ready:                     //准备发送
                            logger.Debug("ready!");
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.DeviceUp:                  //器件设置
                            logger.Debug("device up!");
                            if(CurrentNTPList != null)
                            {
                                CurrentNTPList.Add(ntp);
                            }
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.StandardUp:                //标准组态
                            logger.Debug("Standard up!");
                            if (CurrentNTPList != null)
                            {
                                CurrentNTPList.Add(ntp);
                            }
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.MixedUp:                   //混合组态
                            logger.Debug("Mixed up!");
                            if (CurrentNTPList != null)
                            {
                                CurrentNTPList.Add(ntp);
                            }
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.GeneralUp:                 //通用组态
                            logger.Debug("General up!");
                            if (CurrentNTPList != null)
                            {
                                CurrentNTPList.Add(ntp);
                            }
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.ManualUp:                  //手控盘
                            logger.Debug("Manual up!");
                            if (CurrentNTPList != null)
                            {
                                CurrentNTPList.Add(ntp);
                            }
                            CurrentExecuteState = ExecuteState.GetState;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                        case CommandType.Over:                      //发送完毕
                        case CommandType.Over8003:
                            logger.Debug("Get over!");
                            CurrentExecuteState = ExecuteState.Untreated;
                            CurrentFinishState = FinishState.Succeessed;
                            request = NTPBuildRequest.GetConfirmRequest(true);
                            SendCommand(request);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="ntp">NTP协议</param>
        private static void SendCommand(NTP ntp)
        {
            byte[] bytes = ntp.ToBytes();
            logger.Info(string.Format("Send Cmd:{0}", BytesHelper.BytesToHexStr(bytes)));
            conn.SendMessage(bytes);
        }

        /// <summary>
        /// 检查类型是否正确
        /// FT8000 FT8003 都是老版本7位
        /// NT8001 存在老板本7位 与 新版本7位  与 新版本8位
        /// NT8007 NT8021 NT8036 NT8053 都是新版本8位
        /// </summary>
        /// <returns></returns>
        private static bool CheckIsValidType(NTP ntp)
        {
            bool result = false;
            switch(CurrentContrllerType)
            {
                case ControllerType.FT8000:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && ntp.Content[1] == 0x00)
                        {
                            result = true;
                        }
                    }
                    break;
                case ControllerType.FT8003:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && ntp.Content[1] == 0x03)
                        {
                            result = true;
                        }
                    }
                    break;
                case ControllerType.NT8001:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && (ntp.Content[1] == 0x06 || ntp.Content[1] == 0x01))
                        {
                            if (ntp.Content[1] == 0x06)
                            {
                                CurrentControllerVersion = ControllerVersion.NewVersion8;
                            }
                            else if(ntp.Content[1] == 0x01 && ntp.Content.Length > 2)
                            {
                                CurrentControllerVersion = ControllerVersion.NewVersion7;
                            }
                            else
                            {
                                CurrentControllerVersion = ControllerVersion.OldVersion7;             //老版本中不存在 最后的版本号
                            }
                            result = true;
                        }
                    }
                    break;
                case ControllerType.NT8007:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && ntp.Content[1] == 0x07)
                        {
                            result = true;
                        }
                    }
                    break;
                case ControllerType.NT8021:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && ntp.Content[1] == 0x21)
                        {
                            result = true;
                        }
                    }
                    break;
                case ControllerType.NT8036:
                    if (ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if (ntp.Content[0] == 0x80 && ntp.Content[1] == 0x36)
                        {
                            result = true;
                        }
                    }
                    break;
                case ControllerType.NT8053:
                    if(ntp.Content != null && ntp.Content.Length >= 2)
                    {
                        if(ntp.Content[0] == 0x80 && ntp.Content[1] == 0x53)
                        {
                            result = true;
                        }
                    }
                    break;
            }
            return result;
        }

        #endregion Private Method


    }

    /// <summary>
    /// 执行状态
    /// </summary>
    internal enum ExecuteState
    {
        Untreated = 0,              //不处理
        SetState = 1,               //设置
        GetState = 2,               //获取
        SetInvalidType = 3,         //没有验证控制器类型
        GetInvalidType = 4,         //没有验证控制器类型
        SetOver = 5,                //发送步骤完毕状态  需求是发送一条指令发送一条发送完毕 
        GetOver = 6                 //获取完毕
    }

    /// <summary>
    /// 完成状态
    /// </summary>
    internal enum FinishState
    {
        Unfinished = 0,             //未完成
        Succeessed = 1,             //成功
        Failed = 2                  //失败
    }

    /// <summary>
    /// NT8001控制器版本类型
    /// </summary>
    public enum ControllerVersion
    {
        OldVersion7,                //7位老版本
        NewVersion7,                //7位新版本
        NewVersion8                 //8位新版本
    }
}
