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
    /// 构建NTP请求
    /// </summary>
    public class NTPBuildRequest
    {
        #region Public Method
        /// <summary>
        /// 获取控制器类型请求
        /// </summary>
        /// <returns></returns>
        public static NTP GetControllerTypeRequest()
        {
            NTP ntp = new NTP();
            ntp.TransType = TransType.TransDown;
            ntp.Command = CommandType.MachineTypeRequest;
            return ntp;
        }

        /// <summary>
        /// 获取确认收到请求
        /// </summary>
        /// <returns></returns>
        public static NTP GetConfirmRequest(bool isConfirm)
        {
            NTP ntp = new NTP();
            if (isConfirm)
            {
                ntp.PState = NTPState.Successed;
            }
            else
            {
                ntp.PState = NTPState.Failed;
            }
            return ntp;
        }

        /// <summary>
        /// 获取发送完成请求
        /// </summary>
        /// <returns></returns>
        public static NTP GetSendOverRequest()
        {
            NTP ntp = new NTP();
            ntp.TransType = TransType.TransDown;
            ntp.Command = CommandType.Over;
            return ntp;
        }

        /// <summary>
        /// 获取器件协议
        /// </summary>
        /// <param name="device">器件</param>
        /// <param name="deviceCount">器件总数</param>
        /// <param name="type">器件类型</param>
        /// <returns></returns>
        public static NTP GetSetDeviceInfoRequest(DeviceInfoBase device, int deviceCount, ControllerType type)
        {
            NTP ntp = null;
            switch(type)
            {
                case ControllerType.FT8000:
                    ntp = GetSetDeviceInfoFT8000(device, deviceCount);
                    break;
                case ControllerType.FT8003:
                    ntp = GetSetDeviceInfoFT8003(device, deviceCount);
                    break;
                case ControllerType.NT8001:
                    ntp = GetSetDeviceInfoNT8001(device, deviceCount);
                    break;
                case ControllerType.NT8007:
                    ntp = GetSetDeviceInfoNT8007(device, deviceCount);
                    break;
                case ControllerType.NT8021:
                    ntp = GetSetDeviceInfoNT8021(device, deviceCount);
                    break;
                case ControllerType.NT8036:
                    ntp = GetSetDeviceInfoNT8036(device, deviceCount);
                    break;
                case ControllerType.NT8053:
                    ntp = GetSetDeviceInfoNT8053(device, deviceCount);
                    break;
            }

            ntp.TransType = TransType.TransDown;
            ntp.Command = CommandType.DeviceDown;
            return ntp;
        }

        /// <summary>
        /// 获取标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NTP GetSetStandardConfigRequest(LinkageConfigStandard config, ControllerType type)
        {
            NTP ntp = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    ntp = GetSetStandardConfigFT8000(config);
                    break;
                case ControllerType.FT8003:
                    ntp = GetSetStandardConfigFT8003(config);
                    break;
                case ControllerType.NT8001:
                    ntp = GetSetStandardConfigNT8001(config);
                    break;
                case ControllerType.NT8007:
                    ntp = GetSetStandardConfigNT8007(config);
                    break;
                case ControllerType.NT8021:
                    ntp = GetSetStandardConfigNT8021(config);
                    break;
                case ControllerType.NT8036:
                    ntp = GetSetStandardConfigNT8036(config);
                    break;
                case ControllerType.NT8053:
                    ntp = GetSetStandardConfigNT8053(config);
                    break;
            }

            if(ntp != null)
            {
                ntp.TransType = TransType.TransDown;
                ntp.Command = CommandType.StandardDown;
            }
            return ntp;
        }

        /// <summary>
        /// 获取混合组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NTP GetSetMixedConfigRequest(LinkageConfigMixed config, ControllerType type)
        {
            NTP ntp = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    ntp = GetSetMixedConfigFT8000(config);
                    break;
                case ControllerType.FT8003:
                    ntp = GetSetMixedConfigFT8003(config);
                    break;
                case ControllerType.NT8001:
                    ntp = GetSetMixedConfigNT8001(config);
                    break;
                case ControllerType.NT8007:
                    ntp = GetSetMixedConfigNT8007(config);
                    break;
                case ControllerType.NT8021:
                    ntp = GetSetMixedConfigNT8021(config);
                    break;
                case ControllerType.NT8036:
                    ntp = GetSetMixedConfigNT8036(config);
                    break;
                case ControllerType.NT8053:
                    ntp = GetSetMixedConfigNT8053(config);
                    break;
            }

            if (ntp != null)
            {
                ntp.TransType = TransType.TransDown;
                ntp.Command = CommandType.MixedDown;
            }
            return ntp;
        }

        /// <summary>
        /// 获取通用组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NTP GetSetGeneralConfigRequest(LinkageConfigGeneral config, ControllerType type)
        {
            NTP ntp = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    ntp = GetSetGeneralConfigFT8000(config);
                    break;
                case ControllerType.FT8003:
                    ntp = GetSetGeneralConfigFT8003(config);
                    break;
                case ControllerType.NT8001:
                    ntp = GetSetGeneralConfigNT8001(config);
                    break;
                case ControllerType.NT8007:
                    ntp = GetSetGeneralConfigNT8007(config);
                    break;
                case ControllerType.NT8021:
                    ntp = GetSetGeneralConfigNT8021(config);
                    break;
                case ControllerType.NT8036:
                    ntp = GetSetGeneralConfigNT8036(config);
                    break;
                case ControllerType.NT8053:
                    ntp = GetSetGeneralConfigNT8053(config);
                    break;
            }

            if (ntp != null)
            {
                ntp.TransType = TransType.TransDown;
                ntp.Command = CommandType.GeneralDown;
            }
            return ntp;
        }

        /// <summary>
        /// 获取手控盘协议
        /// </summary>
        /// <param name="config"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NTP GetSetManualBoardRequest(ManualControlBoard manual, ControllerType type)
        {
            NTP ntp = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    ntp = GetSetManualBoardFT8000(manual);
                    break;
                case ControllerType.FT8003:
                    ntp = GetSetManualBoardFT8003(manual);
                    break;
                case ControllerType.NT8001:
                    ntp = GetSetManualBoardNT8001(manual);
                    break;
                case ControllerType.NT8007:
                    ntp = GetSetManualBoardNT8007(manual);
                    break;
                case ControllerType.NT8021:
                    ntp = GetSetManualBoardNT8021(manual);
                    break;
                case ControllerType.NT8036:
                    ntp = GetSetManualBoardNT8036(manual);
                    break;
                case ControllerType.NT8053:
                    ntp = GetSetManualBoardNT8053(manual);
                    break;
            }

            if (ntp != null)
            {
                ntp.TransType = TransType.TransDown;
                ntp.Command = CommandType.ManualDown;
            }
            return ntp;
        }

        #endregion Public Method


        #region Private Method

        #region DeviceInfo
        /// <summary>
        /// 获取FT8000器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoFT8000(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8000 deviceInfo = device as DeviceInfo8000;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 7)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 2));
                int LID = int.Parse(deviceInfo.Code.Substring(2, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(4, 3));
                byte[] contentBytes = new byte[35];                                 //总共42字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeatureFT8000(deviceInfo.Feature, deviceInfo.TypeCode);
                contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  //特性 使能 灵敏度
                if (deviceInfo.TypeCode > 100)
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode - 64);       //设备类型
                }
                else
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);            //设备类型
                }
                contentBytes[6] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) >> 8); //输出组1 高位
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) & 0xFF); //输出组1 低位
                contentBytes[8] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) >> 8); //输出组2 高位
                contentBytes[9] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) & 0xFF); //输出组2 低位
                contentBytes[10] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) >> 8); //输出组3 高位
                contentBytes[11] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) & 0xFF); //输出组3 低位
                contentBytes[12] = BytesHelper.ShortNullAbleToByte(deviceInfo.DelayValue); //延时
                contentBytes[13] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.sdpKey) >> 8); //手操号 高位
                contentBytes[14] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.sdpKey) & 0xFF); //手操号 低位
                contentBytes[15] = BytesHelper.IntToOneByte(((int)deviceInfo.ZoneNo) >> 8); //防火分区 高位
                contentBytes[16] = BytesHelper.IntToOneByte(((int)deviceInfo.ZoneNo) & 0xFF); //防火分区 低位
                contentBytes[17] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.BroadcastZone)); //广播分区
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[18~34]
                if (location.Length <= 17)
                {
                    Array.Copy(location, 0, contentBytes, 10, location.Length);
                }

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取FT8003器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoFT8003(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8003 deviceInfo = device as DeviceInfo8003;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 7)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 2));
                int LID = int.Parse(deviceInfo.Code.Substring(2, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(4, 3));
                byte[] contentBytes = new byte[27];                                 //总共34字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeatureFT8003(deviceInfo.Feature, deviceInfo.TypeCode);
                contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  //特性 使能 灵敏度
                if (deviceInfo.TypeCode > 100)
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode - 64);       //设备类型
                }
                else
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);            //设备类型
                }
                contentBytes[6] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1)); //输出组1
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2)); //输出组2
                contentBytes[8] = BytesHelper.ShortNullAbleToByte(deviceInfo.DelayValue); //延时
                //防火分区 0x00 0x00
                contentBytes[11] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo); //区号
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[12~26]
                if (location.Length <= 15)
                {
                    Array.Copy(location, 0, contentBytes, 12, location.Length);
                }

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8001器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoNT8001(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8001 deviceInfo = device as DeviceInfo8001;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 7)
                {
                    return null;
                }
                if(deviceInfo.Code.Length == 7)
                {
                    if(SerialClient.CurrentControllerVersion != ControllerVersion.OldVersion7
                        || SerialClient.CurrentControllerVersion != ControllerVersion.NewVersion7)
                    {
                        throw new Exception("设备类型不匹配！");
                    }
                }
                else if(deviceInfo.Code.Length == 8)
                {
                    if (SerialClient.CurrentControllerVersion != ControllerVersion.NewVersion8)
                    {
                        throw new Exception("设备类型不匹配！");
                    }
                }
                else
                {
                    throw new Exception("设备类型不匹配！");
                }
                int CID = 0;
                int LID = 0;
                int ID = 0;
                switch(SerialClient.CurrentControllerVersion)
                {
                    case ControllerVersion.NewVersion8:
                        CID = int.Parse(deviceInfo.Code.Substring(0, 3));
                        LID = int.Parse(deviceInfo.Code.Substring(3, 2));
                        ID = int.Parse(deviceInfo.Code.Substring(5, 3));
                        break;
                    case ControllerVersion.NewVersion7:
                    case ControllerVersion.OldVersion7:
                        CID = int.Parse(deviceInfo.Code.Substring(0, 2));
                        LID = int.Parse(deviceInfo.Code.Substring(2, 2));
                        ID = int.Parse(deviceInfo.Code.Substring(4, 3));
                        break;
                }
                byte[] contentBytes = new byte[40];                                 //总共47字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeature(deviceInfo.Feature, deviceInfo.TypeCode);
                contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  //特性 使能 灵敏度
                if (SerialClient.CurrentControllerVersion == ControllerVersion.OldVersion7 && deviceInfo.TypeCode > 100)
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode - 64);       //设备类型
                    feature = GetFeatureFT8000(deviceInfo.Feature, deviceInfo.TypeCode);
                    contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  //特性 使能 灵敏度
                
                }
                else
                {
                    contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);            //设备类型
                }
                contentBytes[6] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) >> 8); //输出组1 高位
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) & 0xFF); //输出组1 低位
                contentBytes[8] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) >> 8); //输出组2 高位
                contentBytes[9] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) & 0xFF); //输出组2 低位
                contentBytes[10] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) >> 8); //输出组3 高位
                contentBytes[11] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) & 0xFF); //输出组3 低位
                contentBytes[12] = BytesHelper.ShortNullAbleToByte(deviceInfo.DelayValue); //延时
                //手控盘号 0x00 0x00
                //防火分区 0x00 0x00
                //广播分区 0x00
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[18~34]
                if (location.Length <= 17)
                {
                    Array.Copy(location, 0, contentBytes, 18, location.Length);
                }
                //固定值/0 0x00
                contentBytes[36] = BytesHelper.ShortNullAbleToByte(deviceInfo.BuildingNo); //楼号
                contentBytes[37] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo);     //区号
                contentBytes[38] = BytesHelper.ShortNullAbleToByte(deviceInfo.FloorNo);    //层号
                contentBytes[39] = BytesHelper.ShortNullAbleToByte(deviceInfo.RoomNo);     //房间号

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8007器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoNT8007(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8007 deviceInfo = device as DeviceInfo8007;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 8)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 3));
                int LID = int.Parse(deviceInfo.Code.Substring(3, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(5, 3));
                byte[] contentBytes = new byte[32];                                 //总共39字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeature(deviceInfo.Feature, deviceInfo.TypeCode);
                contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  //特性 使能 灵敏度
                contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);            //设备类型
                contentBytes[6] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) >> 8); //输出组1 高位
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) & 0xFF); //输出组1 低位
                contentBytes[8] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) >> 8); //输出组2 高位
                contentBytes[9] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) & 0xFF); //输出组2 低位
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[10~26]
                if (location.Length <= 17)
                {
                    Array.Copy(location, 0, contentBytes, 10, location.Length);
                }
                //固定值/0 0x00
                contentBytes[28] = BytesHelper.ShortNullAbleToByte(deviceInfo.BuildingNo); //楼号
                contentBytes[29] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo);     //区号
                contentBytes[30] = BytesHelper.ShortNullAbleToByte(deviceInfo.FloorNo);    //层号
                contentBytes[31] = BytesHelper.ShortNullAbleToByte(deviceInfo.RoomNo);     //房间号

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8021器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoNT8021(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8021 deviceInfo = device as DeviceInfo8021;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 8)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 3));
                int LID = int.Parse(deviceInfo.Code.Substring(3, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(5, 3));
                byte[] contentBytes = new byte[40];                                 //总共47字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeature(0, deviceInfo.TypeCode);
                if (feature > 31)
                {
                    contentBytes[4] = BytesHelper.IntToOneByte((2 << 3) | ((deviceInfo.Disable == true ? 1 : 0) << 2) | 0x00);
                }
                else
                {
                    contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | 0x00);
                }
                contentBytes[5] = BytesHelper.IntToOneByte(feature);     //内部类型
                contentBytes[6] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);    //设备类型
                contentBytes[7] = BytesHelper.IntToOneByte(BytesHelper.FloatNullAbleToByte(deviceInfo.CurrentThreshold) >> 8); //输出组电流报警值 高位
                contentBytes[8] = BytesHelper.IntToOneByte(BytesHelper.FloatNullAbleToByte(deviceInfo.CurrentThreshold) & 0xFF); //输出组电流报警值 低位
                contentBytes[9] = BytesHelper.FloatNullAbleToByte(deviceInfo.TemperatureThreshold);         //温度报警值 
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[10~34]
                if (location.Length <= 25)
                {
                    Array.Copy(location, 0, contentBytes, 10, location.Length);
                }
                //固定值/0 0x00
                contentBytes[36] = BytesHelper.ShortNullAbleToByte(deviceInfo.BuildingNo); //楼号
                contentBytes[37] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo);     //区号
                contentBytes[38] = BytesHelper.ShortNullAbleToByte(deviceInfo.FloorNo);    //层号
                contentBytes[39] = BytesHelper.ShortNullAbleToByte(deviceInfo.RoomNo);     //房间号

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8036器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoNT8036(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8036 deviceInfo = device as DeviceInfo8036;
            if (deviceInfo != null)
            {
                if (string.IsNullOrEmpty(deviceInfo.Code)
                    || deviceInfo.Code.Length < 8)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 3));
                int LID = int.Parse(deviceInfo.Code.Substring(3, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(5, 3));
                byte[] contentBytes = new byte[36];                                 //总共43字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeature(0, deviceInfo.TypeCode);
                contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | 0x00);  //特性 使能 灵敏度
                contentBytes[5] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);    //设备类型
                contentBytes[6] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1)); //输出组1
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2)); //输出组2
                contentBytes[8] = BytesHelper.ShortNullAbleToByte(deviceInfo.DelayValue); //延时
                //广播分区 0x00
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[10~26]
                if (location.Length <= 17)
                {
                    Array.Copy(location, 0, contentBytes, 10, location.Length);
                }
                //固定值/0 0x00
                contentBytes[28] = BytesHelper.ShortNullAbleToByte(deviceInfo.BuildingNo); //楼号
                contentBytes[29] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo);     //区号
                contentBytes[30] = BytesHelper.ShortNullAbleToByte(deviceInfo.FloorNo);    //层号
                contentBytes[31] = BytesHelper.ShortNullAbleToByte(deviceInfo.RoomNo);     //房间号
                int alert = (int)(deviceInfo.AlertValue * 10);
                contentBytes[32] = BytesHelper.IntToOneByte(alert >> 8);            //报警浓度 高位
                contentBytes[33] = BytesHelper.IntToOneByte(alert & 0xFF);          //报警浓度 低位
                int forcast = (int)(deviceInfo.ForcastValue * 10);
                contentBytes[34] = BytesHelper.IntToOneByte(forcast >> 8);          //预警浓度 高位
                contentBytes[35] = BytesHelper.IntToOneByte(forcast & 0xFF);        //预警浓度 低位

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8053器件协议
        /// </summary>
        /// <param name="devcie"></param>
        /// <returns></returns>
        private static NTP GetSetDeviceInfoNT8053(DeviceInfoBase device, int deviceCount)
        {
            NTP ntp = new NTP();
            DeviceInfo8053 deviceInfo = device as DeviceInfo8053;
            if (deviceInfo != null)
            {
                if(string.IsNullOrEmpty(deviceInfo.Code) 
                    || deviceInfo.Code.Length < 8)
                {
                    return null;
                }
                int CID = int.Parse(deviceInfo.Code.Substring(0, 3));
                int LID = int.Parse(deviceInfo.Code.Substring(3, 2));
                int ID = int.Parse(deviceInfo.Code.Substring(5, 3));
                byte[] contentBytes = new byte[49];                                 //总共56字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(deviceCount);            //器件总数
                contentBytes[1] = BytesHelper.IntToOneByte(CID);                    //控制器号
                contentBytes[2] = BytesHelper.IntToOneByte(LID);                    //路号
                contentBytes[3] = BytesHelper.IntToOneByte(ID);                     //编号
                int feature = GetFeature(deviceInfo.Feature, deviceInfo.TypeCode);
                if (feature > 31)
                {
                    contentBytes[4] = BytesHelper.IntToOneByte((2 << 3) | ((deviceInfo.Disable == true ? 1 : 0) << 2) | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));  
                }
                else
                {
                    contentBytes[4] = BytesHelper.IntToOneByte(feature << 3 | (deviceInfo.Disable == true ? 1 : 0) << 2 | BytesHelper.ShortNullAbleToByte(deviceInfo.SensitiveLevel));
                }
                contentBytes[5] = BytesHelper.IntToOneByte(feature);     //内部类型
                contentBytes[6] = BytesHelper.IntToOneByte(deviceInfo.TypeCode);    //设备类型
                contentBytes[7] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) >> 8); //输出组1 高位
                contentBytes[8] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup1) & 0xFF); //输出组1 低位
                contentBytes[9] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) >> 8); //输出组2 高位
                contentBytes[10] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup2) & 0xFF); //输出组2 低位
                contentBytes[11] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) >> 8); //输出组3 高位
                contentBytes[12] = BytesHelper.IntToOneByte(int.Parse(deviceInfo.LinkageGroup3) & 0xFF); //输出组3 低位
                contentBytes[13] = BytesHelper.ShortNullAbleToByte(deviceInfo.DelayValue); //延时
                //手控盘号 0x00 0x00
                //防火分区 0x00 0x00
                //广播分区 0x00
                byte[] location = Encoding.GetEncoding("gb18030").GetBytes(deviceInfo.Location);      //安装地点 contentBytes[19~43]
                if(location.Length <= 25)
                {
                    Array.Copy(location, 0, contentBytes, 19, location.Length);
                }
                //固定值/0 0x00
                contentBytes[45] = BytesHelper.ShortNullAbleToByte(deviceInfo.BuildingNo); //楼号
                contentBytes[46] = BytesHelper.ShortNullAbleToByte(deviceInfo.ZoneNo);     //区号
                contentBytes[47] = BytesHelper.ShortNullAbleToByte(deviceInfo.FloorNo);    //层号
                contentBytes[48] = BytesHelper.ShortNullAbleToByte(deviceInfo.RoomNo);     //房间号

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        #endregion DeviceInfo

        #region StandardConfig
        /// <summary>
        /// 获取FT8000标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigFT8000(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[33];                                         //总共40字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //标准组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code) & 0xFF);  //标准组态 编号 低位
                byte[] tempBytes = GetLinkage7Bytes(config.DeviceNo1);
                Array.Copy(tempBytes, 0, contentBytes, 2, tempBytes.Length);                //联动器件1 [2 ~ 4]
                tempBytes = GetLinkage7Bytes(config.DeviceNo2);
                Array.Copy(tempBytes, 0, contentBytes, 5, tempBytes.Length);                //联动器件2 [5 ~ 7]
                tempBytes = GetLinkage7Bytes(config.DeviceNo3);
                Array.Copy(tempBytes, 0, contentBytes, 8, tempBytes.Length);                //联动器件3 [8 ~ 10]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 11, tempBytes.Length);               //联动器件4 [11 ~ 13]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 14, tempBytes.Length);               //联动器件5 [14 ~ 16]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 17, tempBytes.Length);               //联动器件6 [17 ~ 19]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 20, tempBytes.Length);               //联动器件7 [20 ~ 22]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 23, tempBytes.Length);               //联动器件8 [23 ~ 25]

                contentBytes[26] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[27] = BytesHelper.IntToOneByte(output1 >> 8);                  //输出组1 高位
                contentBytes[28] = BytesHelper.IntToOneByte(output1 & 0xFF);                //输出组1 低位

                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[29] = BytesHelper.IntToOneByte(output2 >> 8);                  //输出组2 高位
                contentBytes[30] = BytesHelper.IntToOneByte(output2 & 0xFF);                //输出组2 低位

                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[31] = BytesHelper.IntToOneByte(output3 >> 8);                  //输出组3 高位
                contentBytes[32] = BytesHelper.IntToOneByte(output3 & 0xFF);                //输出组3 低位

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取FT8003标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigFT8003(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[17];                                         //总共24字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code));         //标准组态 编号
                byte[] tempBytes = GetLinkage7Bytes(config.DeviceNo1);
                Array.Copy(tempBytes, 0, contentBytes, 1, tempBytes.Length);                //联动器件1 [1 ~ 3]
                tempBytes = GetLinkage7Bytes(config.DeviceNo2);
                Array.Copy(tempBytes, 0, contentBytes, 4, tempBytes.Length);                //联动器件2 [4 ~ 6]
                tempBytes = GetLinkage7Bytes(config.DeviceNo3);
                Array.Copy(tempBytes, 0, contentBytes, 7, tempBytes.Length);                //联动器件3 [7 ~ 9]
                tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 10, tempBytes.Length);               //联动器件4 [10 ~ 12]

                contentBytes[13] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[14] = BytesHelper.IntToOneByte(output1);                       //输出组1
                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[15] = BytesHelper.IntToOneByte(output2);                       //输出组2
                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[16] = BytesHelper.IntToOneByte(output3);                       //输出组3

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8001标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigNT8001(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[33];                                         //总共40字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //标准组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code) & 0xFF);  //标准组态 编号 低位
                byte[] tempBytes = null;
                if(SerialClient.CurrentControllerVersion == ControllerVersion.NewVersion8)
                {
                    tempBytes = GetLinkage8Bytes(config.DeviceNo1);
                    Array.Copy(tempBytes, 0, contentBytes, 1, tempBytes.Length);                //联动器件1 [2 ~ 4]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo2);
                    Array.Copy(tempBytes, 0, contentBytes, 4, tempBytes.Length);                //联动器件2 [5 ~ 7]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo3);
                    Array.Copy(tempBytes, 0, contentBytes, 7, tempBytes.Length);                //联动器件3 [8 ~ 10]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo4);
                    Array.Copy(tempBytes, 0, contentBytes, 10, tempBytes.Length);               //联动器件4 [11 ~ 13]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo5);
                    Array.Copy(tempBytes, 0, contentBytes, 13, tempBytes.Length);               //联动器件5 [14 ~ 16]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo6);
                    Array.Copy(tempBytes, 0, contentBytes, 16, tempBytes.Length);               //联动器件6 [17 ~ 19]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo7);
                    Array.Copy(tempBytes, 0, contentBytes, 19, tempBytes.Length);               //联动器件7 [20 ~ 22]
                    tempBytes = GetLinkage8Bytes(config.DeviceNo8);
                    Array.Copy(tempBytes, 0, contentBytes, 22, tempBytes.Length);               //联动器件8 [23 ~ 25]
                }
                else
                {
                    tempBytes = GetLinkage7Bytes(config.DeviceNo1);
                    Array.Copy(tempBytes, 0, contentBytes, 1, tempBytes.Length);                //联动器件1 [2 ~ 4]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo2);
                    Array.Copy(tempBytes, 0, contentBytes, 4, tempBytes.Length);                //联动器件2 [5 ~ 7]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo3);
                    Array.Copy(tempBytes, 0, contentBytes, 7, tempBytes.Length);                //联动器件3 [8 ~ 10]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo4);
                    Array.Copy(tempBytes, 0, contentBytes, 10, tempBytes.Length);               //联动器件4 [11 ~ 13]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo5);
                    Array.Copy(tempBytes, 0, contentBytes, 13, tempBytes.Length);               //联动器件5 [14 ~ 16]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo6);
                    Array.Copy(tempBytes, 0, contentBytes, 16, tempBytes.Length);               //联动器件6 [17 ~ 19]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo7);
                    Array.Copy(tempBytes, 0, contentBytes, 19, tempBytes.Length);               //联动器件7 [20 ~ 22]
                    tempBytes = GetLinkage7Bytes(config.DeviceNo8);
                    Array.Copy(tempBytes, 0, contentBytes, 22, tempBytes.Length);               //联动器件8 [23 ~ 25]
                }

                contentBytes[26] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[27] = BytesHelper.IntToOneByte(output1 >> 8);                  //输出组1 高位
                contentBytes[28] = BytesHelper.IntToOneByte(output1 & 0xFF);                //输出组1 低位

                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[29] = BytesHelper.IntToOneByte(output2 >> 8);                  //输出组2 高位
                contentBytes[30] = BytesHelper.IntToOneByte(output2 & 0xFF);                //输出组2 低位

                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[31] = BytesHelper.IntToOneByte(output3 >> 8);                  //输出组3 高位
                contentBytes[32] = BytesHelper.IntToOneByte(output3 & 0xFF);                //输出组3 低位

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8007标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigNT8007(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[17];                                         //总共24字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code));         //标准组态 编号
                byte[] tempBytes = GetLinkage8Bytes(config.DeviceNo1);
                Array.Copy(tempBytes, 0, contentBytes, 1, tempBytes.Length);                //联动器件1 [1 ~ 3]
                tempBytes = GetLinkage8Bytes(config.DeviceNo2);
                Array.Copy(tempBytes, 0, contentBytes, 4, tempBytes.Length);                //联动器件2 [4 ~ 6]
                tempBytes = GetLinkage8Bytes(config.DeviceNo3);
                Array.Copy(tempBytes, 0, contentBytes, 7, tempBytes.Length);                //联动器件3 [7 ~ 9]
                tempBytes = GetLinkage8Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 10, tempBytes.Length);               //联动器件4 [10 ~ 12]

                contentBytes[13] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[14] = BytesHelper.IntToOneByte(output1);                       //输出组1
                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[15] = BytesHelper.IntToOneByte(output2);                       //输出组2
                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[16] = BytesHelper.IntToOneByte(output3);                       //输出组3

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8021标准组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigNT8021(LinkageConfigStandard config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8036标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigNT8036(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[17];                                         //总共24字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code));         //标准组态 编号
                byte[] tempBytes = GetLinkage8Bytes(config.DeviceNo1);
                Array.Copy(tempBytes, 0, contentBytes, 1, tempBytes.Length);                //联动器件1 [1 ~ 3]
                tempBytes = GetLinkage8Bytes(config.DeviceNo2);
                Array.Copy(tempBytes, 0, contentBytes, 4, tempBytes.Length);                //联动器件2 [4 ~ 6]
                tempBytes = GetLinkage8Bytes(config.DeviceNo3);
                Array.Copy(tempBytes, 0, contentBytes, 7, tempBytes.Length);                //联动器件3 [7 ~ 9]
                tempBytes = GetLinkage8Bytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 10, tempBytes.Length);               //联动器件4 [10 ~ 12]

                contentBytes[13] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[14] = BytesHelper.IntToOneByte(output1);                       //输出组1
                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[15] = BytesHelper.IntToOneByte(output2);                       //输出组2
                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[16] = BytesHelper.IntToOneByte(output3);                       //输出组3

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8053标准组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetStandardConfigNT8053(LinkageConfigStandard config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[107];                                        //总共114字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //标准组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code) & 0xFF);  //标准组态 编号 低位
                byte[] tempBytes = GetModualBytes(config.DeviceNo1);
                Array.Copy(tempBytes, 0, contentBytes, 2, tempBytes.Length);                //输入模块1 [2 ~ 8]
                tempBytes = GetModualBytes(config.DeviceNo2);
                Array.Copy(tempBytes, 0, contentBytes, 9, tempBytes.Length);                //输入模块2 [9 ~ 15]
                tempBytes = GetModualBytes(config.DeviceNo3);
                Array.Copy(tempBytes, 0, contentBytes, 16, tempBytes.Length);               //输入模块3 [16 ~ 22]
                tempBytes = GetModualBytes(config.DeviceNo4);
                Array.Copy(tempBytes, 0, contentBytes, 23, tempBytes.Length);               //输入模块4 [23 ~ 29]
                tempBytes = GetModualBytes(config.DeviceNo5);
                Array.Copy(tempBytes, 0, contentBytes, 30, tempBytes.Length);               //输入模块5 [30 ~ 36]
                tempBytes = GetModualBytes(config.DeviceNo6);
                Array.Copy(tempBytes, 0, contentBytes, 37, tempBytes.Length);               //输入模块6 [37 ~ 43]
                tempBytes = GetModualBytes(config.DeviceNo7);
                Array.Copy(tempBytes, 0, contentBytes, 44, tempBytes.Length);               //输入模块7 [44 ~ 50]
                tempBytes = GetModualBytes(config.DeviceNo8);
                Array.Copy(tempBytes, 0, contentBytes, 51, tempBytes.Length);               //输入模块8 [51 ~ 57]
                tempBytes = GetModualBytes(config.DeviceNo9);
                Array.Copy(tempBytes, 0, contentBytes, 58, tempBytes.Length);               //输入模块9 [58 ~ 64]
                tempBytes = GetModualBytes(config.DeviceNo10);
                Array.Copy(tempBytes, 0, contentBytes, 65, tempBytes.Length);               //输入模块10 [65 ~ 71]
                tempBytes = GetModualBytes(config.DeviceNo11);
                Array.Copy(tempBytes, 0, contentBytes, 72, tempBytes.Length);               //输入模块11 [72 ~ 78]
                tempBytes = GetModualBytes(config.DeviceNo12);
                Array.Copy(tempBytes, 0, contentBytes, 79, tempBytes.Length);               //输入模块12 [79 ~ 85]

                contentBytes[86] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                tempBytes = GetModualBytes(config.OutputDevice1);
                Array.Copy(tempBytes, 0, contentBytes, 87, tempBytes.Length);               //输出模块1 [87 ~ 93]
                tempBytes = GetModualBytes(config.OutputDevice2);
                Array.Copy(tempBytes, 0, contentBytes, 94, tempBytes.Length);               //输出模块2 [94 ~ 100]

                int output1 = 0;
                int.TryParse(config.LinkageNo1, out output1);
                contentBytes[101] = BytesHelper.IntToOneByte(output1 >> 8);                 //联动组1 高位
                contentBytes[102] = BytesHelper.IntToOneByte(output1 & 0xFF);               //联动组1 低位

                int output2 = 0;
                int.TryParse(config.LinkageNo2, out output2);
                contentBytes[103] = BytesHelper.IntToOneByte(output2 >> 8);                 //联动组2 高位
                contentBytes[104] = BytesHelper.IntToOneByte(output2 & 0xFF);               //联动组2 低位

                int output3 = 0;
                int.TryParse(config.LinkageNo3, out output3);
                contentBytes[105] = BytesHelper.IntToOneByte(output3 >> 8);                 //联动组3 高位
                contentBytes[106] = BytesHelper.IntToOneByte(output3 & 0xFF);               //联动组3 低位

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        #endregion StandardConfig

        #region MixedConfig
        /// <summary>
        /// 获取FT8000混合组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigFT8000(LinkageConfigMixed config)
        {
            return null;
        }

        /// <summary>
        /// 获取FT8003混合组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigFT8003(LinkageConfigMixed config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8001混合组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigNT8001(LinkageConfigMixed config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[23];                                         //总共30字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //混合组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code) & 0xFF);  //混合组态 编号 低位
                contentBytes[2] = 0x00;                                                     //机号A
                int typeA = 0;
                switch (config.TypeA)
                {
                    case LinkageType.ZoneLayer:
                        typeA = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeA = 0;
                        break;
                }
                contentBytes[3] = BytesHelper.IntToOneByte(typeA);                          //地址类型A
                contentBytes[4] = BytesHelper.IntNullAbleToByte(config.BuildingNoA);        //楼A
                contentBytes[5] = BytesHelper.IntNullAbleToByte(config.ZoneNoA);            //区A
                contentBytes[6] = BytesHelper.IntNullAbleToByte(config.LayerNoA);           //层A
                contentBytes[7] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeA);       //设备类型A
                contentBytes[8] = 0x00;                                                     //屏蔽
                int action = 0;
                switch (config.ActionType)
                {
                    case LinkageActionType.AND:
                        action = 1;
                        break;
                    default:
                    case LinkageActionType.OR:
                        action = 0;
                        break;
                }
                contentBytes[9] = BytesHelper.IntToOneByte(action);                         //逻辑类型
                contentBytes[10] = 0x00;                                                    //机号B
                int typeB = 0;
                switch (config.TypeB)
                {
                    case LinkageType.ZoneLayer:
                        typeB = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeB = 0;
                        break;
                }
                contentBytes[11] = BytesHelper.IntToOneByte(typeB);                         //地址类型B
                contentBytes[12] = BytesHelper.IntNullAbleToByte(config.BuildingNoB);       //楼B
                contentBytes[13] = BytesHelper.IntNullAbleToByte(config.ZoneNoB);           //区B
                contentBytes[14] = BytesHelper.IntNullAbleToByte(config.LayerNoB);          //层B
                contentBytes[15] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeB);      //设备类型B
                contentBytes[16] = 0x00;                                                    //机号C
                int typeC = 0;
                switch (config.TypeC)
                {
                    case LinkageType.ZoneLayer:
                        typeC = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeC = 0;
                        break;
                }
                contentBytes[17] = BytesHelper.IntToOneByte(typeC);                         //地址类型C
                contentBytes[18] = BytesHelper.IntNullAbleToByte(config.BuildingNoC);       //楼C
                contentBytes[19] = BytesHelper.IntNullAbleToByte(config.ZoneNoC);           //区C
                contentBytes[20] = BytesHelper.IntNullAbleToByte(config.LayerNoC);          //层C
                contentBytes[21] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeC);      //设备类型C
                contentBytes[22] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8007混合组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigNT8007(LinkageConfigMixed config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8021混合组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigNT8021(LinkageConfigMixed config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8036混合组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigNT8036(LinkageConfigMixed config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8053混合组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetMixedConfigNT8053(LinkageConfigMixed config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[25];                                         //总共32字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //混合组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code) & 0xFF);  //混合组态 编号 低位
                contentBytes[2] = 0x00;                                                     //机号A
                int typeA = 0;
                switch(config.TypeA)
                {
                    case LinkageType.ZoneLayer:
                        typeA = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeA = 0;
                        break;
                }
                contentBytes[3] = BytesHelper.IntToOneByte(typeA);                          //地址类型A
                contentBytes[4] = BytesHelper.IntToOneByte(config.CategoryA);               //类别A
                contentBytes[5] = BytesHelper.IntNullAbleToByte(config.BuildingNoA);        //楼A
                contentBytes[6] = BytesHelper.IntNullAbleToByte(config.ZoneNoA);            //区A
                contentBytes[7] = BytesHelper.IntNullAbleToByte(config.LayerNoA);           //层A
                contentBytes[8] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeA);       //设备类型A
                contentBytes[9] = 0x00;                                                     //屏蔽
                int action = 0;
                switch(config.ActionType)
                {
                    case LinkageActionType.AND:
                        action = 1;
                        break;
                    default:
                    case LinkageActionType.OR:
                        action = 0;
                        break;
                }
                contentBytes[10] = BytesHelper.IntToOneByte(action);                        //逻辑类型
                contentBytes[11] = 0x00;                                                    //机号B
                int typeB = 0;
                switch (config.TypeB)
                {
                    case LinkageType.ZoneLayer:
                        typeB = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeB = 0;
                        break;
                }
                contentBytes[12] = BytesHelper.IntToOneByte(typeB);                         //地址类型B
                contentBytes[13] = BytesHelper.IntToOneByte(config.CategoryB);              //类别B
                contentBytes[14] = BytesHelper.IntNullAbleToByte(config.BuildingNoB);       //楼B
                contentBytes[15] = BytesHelper.IntNullAbleToByte(config.ZoneNoB);           //区B
                contentBytes[16] = BytesHelper.IntNullAbleToByte(config.LayerNoB);          //层B
                contentBytes[17] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeB);      //设备类型B
                contentBytes[18] = 0x00;                                                    //机号C
                int typeC = 0;
                switch (config.TypeC)
                {
                    case LinkageType.ZoneLayer:
                        typeC = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeC = 0;
                        break;
                }
                contentBytes[19] = BytesHelper.IntToOneByte(typeC);                         //地址类型C
                contentBytes[20] = BytesHelper.IntNullAbleToByte(config.BuildingNoC);       //楼C
                contentBytes[21] = BytesHelper.IntNullAbleToByte(config.ZoneNoC);           //区C
                contentBytes[22] = BytesHelper.IntNullAbleToByte(config.LayerNoC);          //层C
                contentBytes[23] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeC);      //设备类型C
                contentBytes[24] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                ntp.Content = contentBytes;
            }
            return ntp;
        }
        #endregion MixedConfig

        #region GeneralConfig
        /// <summary>
        /// 获取FT8000通用组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigFT8000(LinkageConfigGeneral config)
        {
            return null;
        }

        /// <summary>
        /// 获取FT8003通用组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigFT8003(LinkageConfigGeneral config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8001通用组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigNT8001(LinkageConfigGeneral config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[18];                                         //总共25字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //混合组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code));         //混合组态 编号 低位
                contentBytes[2] = 0x00;                                                     //机号A
                contentBytes[3] = 0x00;                                                     //地址类型A
                contentBytes[4] = BytesHelper.IntNullAbleToByte(config.BuildingNoA);        //楼A
                contentBytes[5] = BytesHelper.IntNullAbleToByte(config.ZoneNoA);            //区A
                contentBytes[6] = BytesHelper.IntNullAbleToByte(config.LayerNoA1);          //层A1
                contentBytes[7] = BytesHelper.IntNullAbleToByte(config.LayerNoA2);          //层A2
                contentBytes[8] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeA);       //设备类型A
                contentBytes[9] = 0x00;                                                    //屏蔽
                contentBytes[10] = 0x00;                                                    //逻辑类型
                contentBytes[11] = BytesHelper.IntToOneByte(int.Parse(config.MachineNoC));  //机号C
                int typeC = 0;
                switch (config.TypeC)
                {
                    case LinkageType.AdjacentLayer:
                        typeC = 3;
                        break;
                    case LinkageType.SameLayer:
                        typeC = 2;
                        break;
                    case LinkageType.ZoneLayer:
                        typeC = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeC = 0;
                        break;
                }
                contentBytes[12] = BytesHelper.IntToOneByte(typeC);                         //地址类型C
                contentBytes[13] = BytesHelper.IntNullAbleToByte(config.BuildingNoC);       //楼C
                contentBytes[14] = BytesHelper.IntNullAbleToByte(config.ZoneNoC);           //区C
                contentBytes[15] = BytesHelper.IntNullAbleToByte(config.LayerNoC);          //层C
                contentBytes[16] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeC);      //设备类型C
                contentBytes[17] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8007通用组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigNT8007(LinkageConfigGeneral config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8021通用组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigNT8021(LinkageConfigGeneral config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8036通用组态协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigNT8036(LinkageConfigGeneral config)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8053通用组态协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetGeneralConfigNT8053(LinkageConfigGeneral config)
        {
            NTP ntp = new NTP();
            if (config != null)
            {
                byte[] contentBytes = new byte[19];                                         //总共26字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(int.Parse(config.Code) >> 8);    //混合组态 编号 高位
                contentBytes[1] = BytesHelper.IntToOneByte(int.Parse(config.Code));         //混合组态 编号 低位
                contentBytes[2] = 0x00;                                                     //机号A
                contentBytes[3] = 0x00;                                                     //地址类型A
                contentBytes[4] = BytesHelper.IntToOneByte(config.CategoryA);               //类别A
                contentBytes[5] = BytesHelper.IntNullAbleToByte(config.BuildingNoA);        //楼A
                contentBytes[6] = BytesHelper.IntNullAbleToByte(config.ZoneNoA);            //区A
                contentBytes[7] = BytesHelper.IntNullAbleToByte(config.LayerNoA1);          //层A1
                contentBytes[8] = BytesHelper.IntNullAbleToByte(config.LayerNoA2);          //层A2
                contentBytes[9] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeA);       //设备类型A
                contentBytes[10] = 0x00;                                                    //屏蔽
                contentBytes[11] = 0x00;                                                    //逻辑类型
                contentBytes[12] = BytesHelper.IntToOneByte(int.Parse(config.MachineNoC));  //机号C
                int typeC = 0;
                switch (config.TypeC)
                {
                    case LinkageType.AdjacentLayer:
                        typeC = 3;
                        break;
                    case LinkageType.SameLayer:
                        typeC = 2;
                        break;
                    case LinkageType.ZoneLayer:
                        typeC = 1;
                        break;
                    default:
                    case LinkageType.Address:
                        typeC = 0;
                        break;
                }
                contentBytes[13] = BytesHelper.IntToOneByte(typeC);                         //地址类型C
                contentBytes[14] = BytesHelper.IntNullAbleToByte(config.BuildingNoC);       //楼C
                contentBytes[15] = BytesHelper.IntNullAbleToByte(config.ZoneNoC);           //区C
                contentBytes[16] = BytesHelper.IntNullAbleToByte(config.LayerNoC);          //层C
                contentBytes[17] = BytesHelper.ShortToOneByte(config.DeviceTypeCodeC);      //设备类型C
                contentBytes[18] = BytesHelper.IntToOneByte(config.ActionCoefficient);      //动作常数

                ntp.Content = contentBytes;
            }
            return ntp;
        }
        #endregion GeneralConfig

        #region ManualBoard
        /// <summary>
        /// 获取FT8000手控盘协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardFT8000(ManualControlBoard manual)
        {
            return null;
        }

        /// <summary>
        /// 获取FT8003手控盘协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardFT8003(ManualControlBoard manual)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8001手控盘协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardNT8001(ManualControlBoard manual)
        {
            NTP ntp = new NTP();
            if (manual != null)
            {
                byte[] contentBytes = new byte[7];                                          //总共14字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(manual.BoardNo);                 //板卡号 
                contentBytes[1] = BytesHelper.IntToOneByte(manual.MaxSubBoardNo);           //记录当前板卡下的最大“手控盘号”
                contentBytes[2] = BytesHelper.IntToOneByte(manual.SubBoardNo);              //盘号 
                contentBytes[3] = BytesHelper.IntToOneByte(manual.KeyNo);                   //键号
                int CID = 0;
                int LID = 0;
                int ID = 0;
                if (!string.IsNullOrEmpty(manual.DeviceCode) && manual.DeviceCode.Length >= 7)
                {
                    switch (SerialClient.CurrentControllerVersion)
                    {
                        case ControllerVersion.NewVersion8:
                            CID = int.Parse(manual.DeviceCode.Substring(0, 3));
                            LID = int.Parse(manual.DeviceCode.Substring(3, 2));
                            ID = int.Parse(manual.DeviceCode.Substring(5, 3));
                            break;
                        case ControllerVersion.NewVersion7:
                        case ControllerVersion.OldVersion7:
                            CID = int.Parse(manual.DeviceCode.Substring(0, 2));
                            LID = int.Parse(manual.DeviceCode.Substring(2, 2));
                            ID = int.Parse(manual.DeviceCode.Substring(4, 3));
                            break;
                    }
                }
                contentBytes[4] = BytesHelper.IntToOneByte(CID);                            //控制器号
                contentBytes[5] = BytesHelper.IntToOneByte(LID);                            //路号 
                contentBytes[6] = BytesHelper.IntToOneByte(ID);                             //编号

                ntp.Content = contentBytes;
            }
            return ntp;
        }

        /// <summary>
        /// 获取NT8007手控盘协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardNT8007(ManualControlBoard manual)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8021手控盘协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardNT8021(ManualControlBoard manual)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8036手控盘协议
        /// 该设备不支持
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardNT8036(ManualControlBoard manual)
        {
            return null;
        }

        /// <summary>
        /// 获取NT8053手控盘协议
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static NTP GetSetManualBoardNT8053(ManualControlBoard manual)
        {
            NTP ntp = new NTP();
            if (manual != null)
            {
                byte[] contentBytes = new byte[57];                                         //总共64字节 -  NTP头部7字节
                contentBytes[0] = BytesHelper.IntToOneByte(manual.Code);                    //手控盘 编号 
                contentBytes[1] = BytesHelper.IntToOneByte(manual.KeyNo);                   //手控盘 键号
                bool clear = false;
                switch(manual.ControlType)
                {
                    case 0:
                        clear = true;                                                       //空器件  下面数据全部清零
                        contentBytes[2] = BytesHelper.IntToOneByte(manual.ControlType);     //被控类型
                        break;
                    case 1:
                    case 2:
                    case 3:
                        contentBytes[2] = BytesHelper.IntToOneByte(manual.ControlType);     //被控类型
                        break;
                    case 4:
                        contentBytes[2] = 0x80;                                             //被控类型
                        break;
                }
                if(!clear)
                {
                    byte[] tempBytes = GetDeviceBytes(manual.LocalDevice1);
                    Array.Copy(tempBytes, 0, contentBytes, 3, tempBytes.Length);                //本机设备1 [3 ~ 7]
                    tempBytes = GetDeviceBytes(manual.LocalDevice2);
                    Array.Copy(tempBytes, 0, contentBytes, 8, tempBytes.Length);                //本机设备2 [8 ~ 12]
                    tempBytes = GetDeviceBytes(manual.LocalDevice2);
                    Array.Copy(tempBytes, 0, contentBytes, 13, tempBytes.Length);               //本机设备3 [13 ~ 17]
                    tempBytes = GetDeviceBytes(manual.LocalDevice2);
                    Array.Copy(tempBytes, 0, contentBytes, 18, tempBytes.Length);               //本机设备4 [18 ~ 22]
                    contentBytes[23] = BytesHelper.IntToOneByte(int.Parse(manual.BuildingNo));  //楼号
                    contentBytes[24] = BytesHelper.IntToOneByte(int.Parse(manual.AreaNo));      //区号
                    contentBytes[25] = BytesHelper.IntToOneByte(int.Parse(manual.FloorNo));     //层号
                    contentBytes[26] = BytesHelper.ShortToOneByte(manual.DeviceType);           //设备类型
                    contentBytes[27] = BytesHelper.IntToOneByte(int.Parse(manual.LinkageGroup) >> 8);       //输出组 高位
                    contentBytes[28] = BytesHelper.IntToOneByte(int.Parse(manual.LinkageGroup));            //输出组 低位
                    tempBytes = GetModualBytes(manual.NetDevice1);
                    Array.Copy(tempBytes, 0, contentBytes, 29, tempBytes.Length);               //网络设备1 [29 ~ 35]
                    tempBytes = GetModualBytes(manual.NetDevice2);
                    Array.Copy(tempBytes, 0, contentBytes, 36, tempBytes.Length);               //网络设备2 [36 ~ 42]
                    tempBytes = GetModualBytes(manual.NetDevice3);
                    Array.Copy(tempBytes, 0, contentBytes, 43, tempBytes.Length);               //网络设备3 [43 ~ 49]
                    tempBytes = GetModualBytes(manual.NetDevice4);
                    Array.Copy(tempBytes, 0, contentBytes, 50, tempBytes.Length);               //网络设备4 [50 ~ 56]
                }

                ntp.Content = contentBytes;
            }
            return ntp;
        }
        #endregion ManualBoard
        
        #region Common
        /// <summary>
        /// 获取联动模块 数据
        /// NT8001_7 FT8000 FT8003 标准组态
        /// </summary>
        /// <returns></returns>
        private static byte[] GetLinkage7Bytes(string linkage)
        {
            byte[] bytes = new byte[3];
            int CID = int.Parse(linkage.Substring(0, 2));
            int LID = int.Parse(linkage.Substring(2, 2));
            int ID = int.Parse(linkage.Substring(4, 3));
            bytes[0] = BytesHelper.IntToOneByte(CID);               //控制器号
            bytes[1] = BytesHelper.IntToOneByte(LID);               //路号
            bytes[2] = BytesHelper.IntToOneByte(ID);                //编号
            return bytes;
        }

        /// <summary>
        /// 获取联动器件 数据
        /// NT8036 NT8021 NT8007 NT8001_8 标准组态
        /// </summary>
        /// <returns></returns>
        private static byte[] GetLinkage8Bytes(string linkage)
        {
            byte[] bytes = new byte[3];
            int CID = int.Parse(linkage.Substring(0, 3));
            int LID = int.Parse(linkage.Substring(3, 2));
            int ID = int.Parse(linkage.Substring(5, 3));
            bytes[0] = BytesHelper.IntToOneByte(CID);               //控制器号
            bytes[1] = BytesHelper.IntToOneByte(LID);               //路号
            bytes[2] = BytesHelper.IntToOneByte(ID);                //编号
            return bytes;
        }

        /// <summary>
        /// 获取联动器件 输出模块 数据
        /// NT8053 标准组态
        /// </summary>
        /// <returns></returns>
        private static byte[] GetModualBytes(string modual)
        {
            byte[] bytes = new byte[7];
            char sign = ',';
            bytes[3] = 0xAA;
            if(modual.Contains('~'))
            {
                sign = '~';
                bytes[3] = 0x55;
            }
            string[] array = modual.Split(sign);
            if(array.Length == 2)
            {
                int index = 0;
                for (int i = 0; i < 2; i++)
                {
                    index = (i * 4);
                    int CID = int.Parse(array[i].Substring(0, 3));
                    int LID = int.Parse(array[i].Substring(3, 2));
                    int ID = int.Parse(array[i].Substring(5, 3));
                    bytes[index] = BytesHelper.IntToOneByte(CID);               //控制器号
                    bytes[index + 1] = BytesHelper.IntToOneByte(LID);           //路号
                    bytes[index + 2] = BytesHelper.IntToOneByte(ID);            //编号
                }
            }
            return bytes;
        }

        /// <summary>
        /// 获取本机设备 网络设备
        /// NT8053 手动盘
        /// </summary>
        /// <returns></returns>
        private static byte[] GetDeviceBytes(string device)
        {
            byte[] bytes = new byte[5];
            char sign = ',';
            bytes[2] = 0xAA;
            if (device.Contains('~'))
            {
                sign = '~';
                bytes[2] = 0x55;
            }
            string[] array = device.Split(sign);
            if (array.Length == 2)
            {
                int index = 0;
                for (int i = 0; i < 2; i++)
                {
                    index = (i * 3);
                    int LID = int.Parse(array[i].Substring(0, 2));
                    int ID = int.Parse(array[i].Substring(2, 3));
                    bytes[index] = BytesHelper.IntToOneByte(LID);           //路号
                    bytes[index + 1] = BytesHelper.IntToOneByte(ID);        //编号
                }
            }
            return bytes;
        }


        #region Feature
        /// <summary>
        /// 获取特性
        /// NT8001
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeatureNT8001(short feature, short type)
        {
            int result = 0;
            switch(type)
            {
                case 6:
                    result = 6;
                    break;
                case 7:
                    result = 7;
                    break;
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                    result = 2;
                    break;
                case 65:
                    switch (feature)
                    {
                        case 0:
                            result = 24;
                            break;
                        case 1:
                            result = 23;
                            break;
                        case 2:
                            result = 31;
                            break;
                        case 3:
                            result = 30;
                            break;
                    }
                    break;
                case 77:
                    result = 27;
                    break;
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                    result = 16;            //未定义   以前程序的默认值 16
                    break;
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 129:
                    result = 23;
                    break;
                default:
                    result = GetFeature(feature, type);
                    break;
            }
            if(result < 0)
            {
                result = 16;
            }
            return result;
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeatureFT8003(short? feature, short? type)
        {
            int result = 0;
            if (type != null)
            {
                short t = (short)type;
                result = GetFeatureFT8003(feature, t);
            }
            return result;
        }

        /// <summary>
        /// 获取特性
        /// FT8003
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeatureFT8003(short? feature, short type)
        {
            int result = 0;
            switch (type)
            {
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                    result = 24;
                    break;
                default:
                    result = GetFeatureFT8000(feature, type);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeatureFT8000(short? feature, short? type)
        {
            int result = 0;
            if (type != null)
            {
                short t = (short)type;
                result = GetFeatureFT8000(feature, t);
            }
            return result;
        }

        /// <summary>
        /// 获取特性
        /// FT8000
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeatureFT8000(short? feature, short type)
        {
            int result = 0;
            switch (type)
            {
                case 6:
                    result = 6;
                    break;
                case 7:
                    result = 7;
                    break;
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                    result = 2;
                    break;
                case 24:
                case 25:
                case 26:
                    result = 4;
                    break;
                case 65:
                    switch (feature)
                    {
                        case 0:
                            result = 24;
                            break;
                        case 1:
                            result = 23;
                            break;
                        case 2:
                            result = 31;
                            break;
                        case 3:
                            result = 30;
                            break;
                    }
                    break;
                case 77:
                    result = 27;
                    break;
                case 90:
                case 91:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                    result = 16;            //未定义   以前程序的默认值 16
                    break;
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                case 123:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 129:
                    result = 23;
                    break;
                default:
                    result = GetFeature(feature, type);
                    break;
            }
            if (result < 0)
            {
                result = 16;
            }
            return result;
        }


        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeature(short? feature, short? type)
        {
            int result = 0;
            if (type != null)
            {
                short t = (short)type;
                result = GetFeature(feature, t);
            }
            return result;
        }

        /// <summary>
        /// 获取特性
        /// 器件类型表20170205
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetFeature(short? feature, short type)
        {
            int result = 0;
            switch (type)
            {
                case 0:
                    result = 0;
                    break;
                case 1:
                    result = 1;
                    break;
                case 2:
                    result = 2;
                    break;
                case 3:
                    result = 3;
                    break;
                case 4:
                case 16:
                case 17:
                case 27:
                case 28:
                case 29:
                case 30:
                    result = 4;
                    break;
                case 5:
                    result = 5;
                    break;
                case 25:
                    result = 6;
                    break;
                case 26:
                    result = 7;
                    break;
                case 8:
                    result = 8;
                    break;
                case 9:
                    result = 9;
                    break;
                case 10:
                    result = 10;
                    break;
                case 87:
                case 88:
                case 89:
                    result = 11;
                    break;
                case 6:
                case 7:
                case 11:
                case 12:
                    result = 12;
                    break;
                case 13:
                    result = 13;
                    break;
                case 81:
                    result = 14;
                    break;
                case 15:
                    result = 15;
                    break;
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                    result = 16;
                    break;
                case 14:
                    result = 17;
                    break;
                case 23:
                case 79:
                    result = 18;
                    break;
                case 80:
                    result = 19;
                    break;
                case 67:
                    result = 20;
                    break;
                case 33:
                    result = 21;
                    break;
                case 34:
                    result = 22;
                    break;
                case 31:
                    result = 23;
                    break;
                case 32:
                case 35:
                case 65:
                case 71:
                case 72:
                case 73:
                case 77:
                    result = 24;
                    break;
                case 75:
                    result = 25;
                    break;
                case 76:
                    result = 26;
                    break;
                case 24:
                    result = 27;
                    break;
                case 74:
                case 78:
                    result = 28;
                    break;
                case 36:
                case 66:
                    result = 29;
                    break;
                case 68:
                    result = 30;
                    break;
                case 69:
                case 70:
                    result = 31;
                    break;
                case 19:
                    result = 33;
                    break;
                case 20:
                    result = 34;
                    break;
                case 21:
                    result = 35;
                    break;
                case 22:
                    result = 36;
                    break;
                case 18:
                    result = 37;
                    break;
                case 90:
                    result = 49;
                    break;
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                    if (feature != null)
                    {
                        switch (feature)
                        {
                            case 0:
                                result = 24;
                                break;
                            case 1:
                                result = 23;
                                break;
                            case 2:
                                result = 31;
                                break;
                            case 3:
                                result = 30;
                                break;
                        }
                    }
                    break;
                case 101:
                    if (feature != null)
                    {
                        switch (feature)
                        {
                            case 0:
                            case 1:
                                result = 41;
                                break;
                            case 2:
                            case 3:
                                result = 42;
                                break;
                        }
                    }
                    break;
                case 102:
                    if (feature != null)
                    {
                        switch (feature)
                        {
                            case 0:
                            case 1:
                                result = 43;
                                break;
                            case 2:
                            case 3:
                                result = 44;
                                break;
                        }
                    }
                    break;
                case 103:
                    if (feature != null)
                    {
                        switch (feature)
                        {
                            case 0:
                            case 1:
                                result = 45;
                                break;
                            case 2:
                            case 3:
                                result = 46;
                                break;
                        }
                    }
                    break;
                case 104:
                    if (feature != null)
                    {
                        switch (feature)
                        {
                            case 0:
                            case 1:
                                result = 47;
                                break;
                            case 2:
                            case 3:
                                result = 78;
                                break;
                        }
                    }
                    break;
            }
            return result;
        }
        #endregion Feature

        #endregion Common
        
        #endregion Private Method
    }
}
