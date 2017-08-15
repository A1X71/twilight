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
    public class NTPParseResponse
    {
        #region Public Method
        /// <summary>
        /// 设置器件到控制器中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="ntp"></param>
        public static void SetDeviceInfoToControllerResponse(ControllerModel controller, ControllerType type, NTP ntp)
        {
            switch (type)
            {
                case ControllerType.FT8000:
                    DeviceInfo8000 info8000 = GetDeviceInfoFT8000Response(ntp);
                    LoopModel loop8000 = GetLoopFromController(info8000.LoopID, controller);
                    loop8000.Name = loop8000.Code = info8000.Code.Substring(0, 4);
                    info8000.Loop = loop8000;
                    loop8000.SetDevice<DeviceInfo8000>(info8000);
                    break;
                case ControllerType.FT8003:
                    DeviceInfo8003 info8003 = GetDeviceInfoFT8003Response(ntp);
                    LoopModel loop8003 = GetLoopFromController(info8003.LoopID, controller);
                    loop8003.Name = loop8003.Code = info8003.Code.Substring(0, 4);
                    info8003.Loop = loop8003;
                    loop8003.SetDevice<DeviceInfo8003>(info8003);
                    break;
                case ControllerType.NT8001:
                    DeviceInfo8001 info8001 = GetDeviceInfoNT8001Response(ntp);
                    LoopModel loop8001 = GetLoopFromController(info8001.LoopID, controller);
                    if(SerialClient.CurrentControllerVersion == ControllerVersion.NewVersion8)
                    {
                        loop8001.Name = loop8001.Code = info8001.Code.Substring(0, 5);
                    }
                    else
                    {
                        loop8001.Name = loop8001.Code = info8001.Code.Substring(0, 4);
                    }
                    info8001.Loop = loop8001;
                    loop8001.SetDevice<DeviceInfo8001>(info8001);
                    break;
                case ControllerType.NT8007:
                    DeviceInfo8007 info8007 = GetDeviceInfoNT8007Response(ntp);
                    LoopModel loop8007 = GetLoopFromController(info8007.LoopID, controller);
                    loop8007.Name = loop8007.Code = info8007.Code.Substring(0, 5);
                    info8007.Loop = loop8007;
                    loop8007.SetDevice<DeviceInfo8007>(info8007);
                    break;
                case ControllerType.NT8021:
                    DeviceInfo8021 info8021 = GetDeviceInfoNT8021Response(ntp);
                    LoopModel loop8021 = GetLoopFromController(info8021.LoopID, controller);
                    loop8021.Name = loop8021.Code = info8021.Code.Substring(0, 5);
                    info8021.Loop = loop8021;
                    loop8021.SetDevice<DeviceInfo8021>(info8021);
                    break;
                case ControllerType.NT8036:
                    DeviceInfo8036 info8036 = GetDeviceInfoNT8036Response(ntp);
                    LoopModel loop8036 = GetLoopFromController(info8036.LoopID, controller);
                    loop8036.Name = loop8036.Code = info8036.Code.Substring(0, 5);
                    info8036.Loop = loop8036;
                    loop8036.SetDevice<DeviceInfo8036>(info8036);
                    break;
                case ControllerType.NT8053:
                    DeviceInfo8053 info8053 = GetDeviceInfoNT8053Response(ntp);
                    LoopModel loop8053 = GetLoopFromController(info8053.LoopID, controller);
                    loop8053.Name = loop8053.Code = info8053.Code.Substring(0, 5);
                    info8053.Loop = loop8053;
                    loop8053.SetDevice<DeviceInfo8053>(info8053);
                    break;
            }
        }

        /// <summary>
        /// 设置标准组态到控制器中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="ntp"></param>
        public static void SetStandardConfigToControllerResponse(ControllerModel controller, ControllerType type, NTP ntp)
        {
            LinkageConfigStandard config = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    config = GetStandardConfigFT8000Response(ntp);
                    break;
                case ControllerType.FT8003:
                    config = GetStandardConfigFT8003Response(ntp);
                    break;
                case ControllerType.NT8001:
                    config = GetStandardConfigNT8001Response(ntp);
                    break;
                case ControllerType.NT8007:
                    config = GetStandardConfigNT8007Response(ntp);
                    break;
                case ControllerType.NT8021:
                    config = GetStandardConfigNT8021Response(ntp);
                    break;
                case ControllerType.NT8036:
                    config = GetStandardConfigNT8036Response(ntp);
                    break;
                case ControllerType.NT8053:
                    config = GetStandardConfigNT8053Response(ntp);
                    break;
            }
            if(controller.StandardConfig != null && config != null)
            {
                controller.StandardConfig.Add(config);
            }
        }

        /// <summary>
        /// 设置混合组态到控制器中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="ntp"></param>
        public static void SetMixedConfigToControllerResponse(ControllerModel controller, ControllerType type, NTP ntp)
        {
            LinkageConfigMixed config = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    config = GetMixedConfigFT8000Response(ntp);
                    break;
                case ControllerType.FT8003:
                    config = GetMixedConfigFT8003Response(ntp);
                    break;
                case ControllerType.NT8001:
                    config = GetMixedConfigNT8001Response(ntp);
                    break;
                case ControllerType.NT8007:
                    config = GetMixedConfigNT8007Response(ntp);
                    break;
                case ControllerType.NT8021:
                    config = GetMixedConfigNT8021Response(ntp);
                    break;
                case ControllerType.NT8036:
                    config = GetMixedConfigNT8036Response(ntp);
                    break;
                case ControllerType.NT8053:
                    config = GetMixedConfigNT8053Response(ntp);
                    break;
            }
            if (controller.MixedConfig != null && config != null)
            {
                controller.MixedConfig.Add(config);
            }
        }

        /// <summary>
        /// 设置通用组态到控制器中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="ntp"></param>
        public static void SetGeneralConfigToControllerResponse(ControllerModel controller, ControllerType type, NTP ntp)
        {
            LinkageConfigGeneral config = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    config = GetGeneralConfigFT8000Response(ntp);
                    break;
                case ControllerType.FT8003:
                    config = GetGeneralConfigFT8003Response(ntp);
                    break;
                case ControllerType.NT8001:
                    config = GetGeneralConfigNT8001Response(ntp);
                    break;
                case ControllerType.NT8007:
                    config = GetGeneralConfigNT8007Response(ntp);
                    break;
                case ControllerType.NT8021:
                    config = GetGeneralConfigNT8021Response(ntp);
                    break;
                case ControllerType.NT8036:
                    config = GetGeneralConfigNT8036Response(ntp);
                    break;
                case ControllerType.NT8053:
                    config = GetGeneralConfigNT8053Response(ntp);
                    break;
            }
            if (controller.GeneralConfig != null && config != null)
            {
                controller.GeneralConfig.Add(config);
            }
        }

        /// <summary>
        /// 设置手控盘到控制器中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="ntp"></param>
        public static void SetManualBoardToControllerResponse(ControllerModel controller, ControllerType type, NTP ntp)
        {
            ManualControlBoard manual = null;
            switch (type)
            {
                case ControllerType.FT8000:
                    manual = GetManualBoardFT8000Response(ntp);
                    break;
                case ControllerType.FT8003:
                    manual = GetManualBoardFT8003Response(ntp);
                    break;
                case ControllerType.NT8001:
                    manual = GetManualBoardNT8001Response(ntp);
                    break;
                case ControllerType.NT8007:
                    manual = GetManualBoardNT8007Response(ntp);
                    break;
                case ControllerType.NT8021:
                    manual = GetManualBoardNT8021Response(ntp);
                    break;
                case ControllerType.NT8036:
                    manual = GetManualBoardNT8036Response(ntp);
                    break;
                case ControllerType.NT8053:
                    manual = GetManualBoardNT8053Response(ntp);
                    break;
            }
            if (controller.ControlBoard != null && manual != null)
            {
                controller.ControlBoard.Add(manual);
            }
        }

        #endregion Pubclic Method

        #region Private Method

        #region DeviceInfo
        /// <summary>
        /// 解析设备信息协议 FT8000
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8000 GetDeviceInfoFT8000Response(NTP ntp)
        {
            DeviceInfo8000 device = new DeviceInfo8000();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 35) //总共42字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D2}{1:D2}{2:D3}", CID, LID, ID);
                device.SensitiveLevel = (short?)(ntp.Content[4] & 0x03);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.Feature = (short)(ntp.Content[4] >> 3);
                device.TypeCode = ntp.Content[5];
                if(device.TypeCode > 100)
                {
                    device.TypeCode -= 64;
                    device.Feature = 1;
                }
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[6] << 8 + ntp.Content[7]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[8] << 8 + ntp.Content[9]);
                device.LinkageGroup3 = string.Format("{0:D4}", ntp.Content[10] << 8 + ntp.Content[11]);
                device.DelayValue = (short)(ntp.Content[12]);
                device.sdpKey = string.Format("{0:D4}", ntp.Content[13] << 8 + ntp.Content[14]);
                device.ZoneNo = (short)(ntp.Content[15] << 8 + ntp.Content[16]);
                device.BroadcastZone = string.Format("{0:D4}", ntp.Content[17]);
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 18, 17).TrimEnd('\0');    //安装地点 contentBytes[18~34]
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 FT8003
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8003 GetDeviceInfoFT8003Response(NTP ntp)
        {
            DeviceInfo8003 device = new DeviceInfo8003();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 28) //总共35字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D2}{1:D2}{2:D3}", CID, LID, ID);
                device.SensitiveLevel = (short?)(ntp.Content[4] & 0x03);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.Feature = (short)(ntp.Content[4] >> 3);
                device.TypeCode = ntp.Content[5];
                if (device.TypeCode > 100)
                {
                    device.TypeCode -= 64;
                    device.Feature = 1;
                }
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[6]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[7]);
                device.DelayValue = (short)(ntp.Content[8]);
                //防火分区 0x00 0x00
                device.ZoneNo = (short)(ntp.Content[11]);
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 12, 15).TrimEnd('\0');    //安装地点 contentBytes[12~26]
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 NT8001
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8001 GetDeviceInfoNT8001Response(NTP ntp)
        {
            DeviceInfo8001 device = new DeviceInfo8001();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 40) //总共47字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                if (SerialClient.CurrentControllerVersion == ControllerVersion.NewVersion8)
                {
                    device.Code = string.Format("{0:D3}{1:D2}{2:D3}", CID, LID, ID);
                }
                else
                {
                    device.Code = string.Format("{0:D2}{1:D2}{2:D3}", CID, LID, ID);
                }
                
                device.SensitiveLevel = (short)(ntp.Content[4] & 0x03);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.Feature = (short)(ntp.Content[4] >> 3);      
                device.TypeCode = ntp.Content[5];
                if (SerialClient.CurrentControllerVersion == ControllerVersion.OldVersion7 && device.TypeCode > 100)
                {
                    device.TypeCode -= 64;
                    device.Feature = 1;
                }
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[6] << 8 + ntp.Content[7]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[8] << 8 + ntp.Content[9]);
                device.LinkageGroup3 = string.Format("{0:D4}", ntp.Content[10] << 8 + ntp.Content[11]);
                device.DelayValue = (short)(ntp.Content[12]);
                //手控盘号 0x00 0x00
                //防火分区 0x00 0x00
                //广播分区 0x00
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 18, 17).TrimEnd('\0');    //安装地点 contentBytes[19~34]
                //固定值/0 0x00
                device.BuildingNo = (short)(ntp.Content[36]);
                device.ZoneNo = (short)(ntp.Content[37]);
                device.FloorNo = (short)(ntp.Content[38]);
                device.RoomNo = (short)(ntp.Content[39]);
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 NT8007
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8007 GetDeviceInfoNT8007Response(NTP ntp)
        {
            DeviceInfo8007 device = new DeviceInfo8007();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 32) //总共39字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D3}{1:D2}{2:D3}", CID, LID, ID);
                device.SensitiveLevel = (short)(ntp.Content[4] & 0x03);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.Feature = (short)(ntp.Content[4] >> 3);
                device.TypeCode = ntp.Content[5];
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[6] << 8 + ntp.Content[7]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[8] << 8 + ntp.Content[9]);
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 10, 17).TrimEnd('\0');    //安装地点 contentBytes[10~26]
                //固定值/0 0x00
                device.BuildingNo = (short)(ntp.Content[28]);
                device.ZoneNo = (short)(ntp.Content[29]);
                device.FloorNo = (short)(ntp.Content[30]);
                device.RoomNo = (short)(ntp.Content[31]);
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 NT8021
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8021 GetDeviceInfoNT8021Response(NTP ntp)
        {
            DeviceInfo8021 device = new DeviceInfo8021();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 40) //总共47字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D3}{1:D2}{2:D3}", CID, LID, ID);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                //内部类型 0x00
                device.TypeCode = ntp.Content[6];
                device.CurrentThreshold = (float)(ntp.Content[7] << 8 + ntp.Content[8]);
                device.TemperatureThreshold = ntp.Content[9];
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 10, 25).TrimEnd('\0');    //安装地点 contentBytes[10~34]
                //固定值/0 0x00
                device.BuildingNo = (short)(ntp.Content[36]);
                device.ZoneNo = (short)(ntp.Content[37]);
                device.FloorNo = (short)(ntp.Content[38]);
                device.RoomNo = (short)(ntp.Content[39]);
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 NT8036
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8036 GetDeviceInfoNT8036Response(NTP ntp)
        {
            DeviceInfo8036 device = new DeviceInfo8036();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 36) //总共43字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D3}{1:D2}{2:D3}", CID, LID, ID);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.TypeCode = ntp.Content[5];
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[6]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[7]);
                device.DelayValue = (short)(ntp.Content[8]);
                //广播分区 0x00
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 10, 17).TrimEnd('\0');    //安装地点 contentBytes[10~26]
                //固定值/0 0x00
                device.BuildingNo = (short)(ntp.Content[28]);
                device.ZoneNo = (short)(ntp.Content[29]);
                device.FloorNo = (short)(ntp.Content[30]);
                device.RoomNo = (short)(ntp.Content[31]);
                device.AlertValue = (float)(ntp.Content[32] << 8 + ntp.Content[33]) / 10;
                device.ForcastValue = (float)(ntp.Content[34] << 8 + ntp.Content[35]) / 10;
            }
            return device;
        }

        /// <summary>
        /// 解析设备信息协议 NT8053
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static DeviceInfo8053 GetDeviceInfoNT8053Response(NTP ntp)
        {
            DeviceInfo8053 device = new DeviceInfo8053();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 49) //总共56字节 -  NTP头部7字节
            {
                int CID = ntp.Content[1];
                int LID = ntp.Content[2];
                int ID = ntp.Content[3];
                device.Code = string.Format("{0:D3}{1:D2}{2:D3}", CID, LID, ID);
                device.SensitiveLevel = (short)(ntp.Content[4] & 0x03);
                device.Disable = (ntp.Content[4] & 0x04) == 1 ? true : false;
                device.Feature = ntp.Content[5];
                device.TypeCode = ntp.Content[6];
                device.LinkageGroup1 = string.Format("{0:D4}", ntp.Content[7] << 8 + ntp.Content[8]);
                device.LinkageGroup2 = string.Format("{0:D4}", ntp.Content[9] << 8 + ntp.Content[10]);
                device.LinkageGroup3 = string.Format("{0:D4}", ntp.Content[11] << 8 + ntp.Content[12]);
                device.DelayValue = (short)(ntp.Content[13]);
                device.Location = Encoding.GetEncoding("gb18030").GetString(ntp.Content, 19, 25).TrimEnd('\0');    //安装地点 contentBytes[19~43]
                device.BuildingNo = (short)(ntp.Content[45]);
                device.ZoneNo = (short)(ntp.Content[46]);
                device.FloorNo = (short)(ntp.Content[47]);
                device.RoomNo = (short)(ntp.Content[48]);
            }
            return device;
        }
        #endregion DeviceInfo

        #region StandardConfig
        /// <summary>
        /// 解析标准组态信息协议 FT8000
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigFT8000Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 33)            //总共40字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                byte[] tempBytes = new byte[3];
                Array.Copy(ntp.Content, 2, tempBytes, 0, tempBytes.Length);
                config.DeviceNo1 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [2 ~ 4]
                Array.Copy(ntp.Content, 5, tempBytes, 0, tempBytes.Length);
                config.DeviceNo2 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [5 ~ 7]
                Array.Copy(ntp.Content, 8, tempBytes, 0, tempBytes.Length);
                config.DeviceNo3 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件3 [8 ~ 10]
                Array.Copy(ntp.Content, 11, tempBytes, 0, tempBytes.Length);
                config.DeviceNo4 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [11 ~ 13]
                Array.Copy(ntp.Content, 14, tempBytes, 0, tempBytes.Length);
                config.DeviceNo5 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件5 [14 ~ 16]
                Array.Copy(ntp.Content, 17, tempBytes, 0, tempBytes.Length);
                config.DeviceNo6 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件6 [17 ~ 19]
                Array.Copy(ntp.Content, 20, tempBytes, 0, tempBytes.Length);
                config.DeviceNo7 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件7 [20 ~ 22]
                Array.Copy(ntp.Content, 23, tempBytes, 0, tempBytes.Length);
                config.DeviceNo8 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件8 [23 ~ 25]

                config.ActionCoefficient = ntp.Content[26];                                 //动作常数

                config.LinkageNo1 = string.Format("{0:D4}", (ntp.Content[27] << 8) + ntp.Content[28]);          //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", (ntp.Content[29] << 8) + ntp.Content[30]);          //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", (ntp.Content[31] << 8) + ntp.Content[32]);          //联动组3
            }
            return config;
        }

        /// <summary>
        /// 解析标准组态信息协议 FT8003
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigFT8003Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 17)            //总共24字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", ntp.Content[0]);
                byte[] tempBytes = new byte[3];
                Array.Copy(ntp.Content, 1, tempBytes, 0, tempBytes.Length);
                config.DeviceNo1 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [1 ~ 3]
                Array.Copy(ntp.Content, 4, tempBytes, 0, tempBytes.Length);
                config.DeviceNo2 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [4 ~ 6]
                Array.Copy(ntp.Content, 7, tempBytes, 0, tempBytes.Length);
                config.DeviceNo3 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件3 [7 ~ 9]
                Array.Copy(ntp.Content, 10, tempBytes, 0, tempBytes.Length);
                config.DeviceNo4 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [10 ~ 12]

                config.ActionCoefficient = ntp.Content[13];                             //动作常数

                config.LinkageNo1 = string.Format("{0:D4}", ntp.Content[14]);           //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", ntp.Content[15]);           //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", ntp.Content[16]);           //联动组3
            }
            return config;
        }

        /// <summary>
        /// 解析标准组态信息协议 NT8001
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigNT8001Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 33)            //总共40字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                byte[] tempBytes = new byte[3];
                if(SerialClient.CurrentControllerVersion == ControllerVersion.NewVersion8)
                {
                    Array.Copy(ntp.Content, 2, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo1 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [2 ~ 4]
                    Array.Copy(ntp.Content, 5, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo2 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [5 ~ 7]
                    Array.Copy(ntp.Content, 8, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo3 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件3 [8 ~ 10]
                    Array.Copy(ntp.Content, 11, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo4 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [11 ~ 13]
                    Array.Copy(ntp.Content, 14, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo5 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件5 [14 ~ 16]
                    Array.Copy(ntp.Content, 17, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo6 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件6 [17 ~ 19]
                    Array.Copy(ntp.Content, 20, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo7 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件7 [20 ~ 22]
                    Array.Copy(ntp.Content, 23, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo8 = string.Format("{0:D2}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件8 [23 ~ 25]
                }
                else
                {
                    Array.Copy(ntp.Content, 2, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo1 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [2 ~ 4]
                    Array.Copy(ntp.Content, 5, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo2 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [5 ~ 7]
                    Array.Copy(ntp.Content, 8, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo3 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件3 [8 ~ 10]
                    Array.Copy(ntp.Content, 11, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo4 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [11 ~ 13]
                    Array.Copy(ntp.Content, 14, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo5 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件5 [14 ~ 16]
                    Array.Copy(ntp.Content, 17, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo6 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件6 [17 ~ 19]
                    Array.Copy(ntp.Content, 20, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo7 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);      //联动器件7 [20 ~ 22]
                    Array.Copy(ntp.Content, 23, tempBytes, 0, tempBytes.Length);
                    config.DeviceNo8 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件8 [23 ~ 25]
                }
                
                config.ActionCoefficient = ntp.Content[26];                                 //动作常数

                config.LinkageNo1 = string.Format("{0:D4}", (ntp.Content[27] << 8) + ntp.Content[28]);          //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", (ntp.Content[29] << 8) + ntp.Content[30]);          //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", (ntp.Content[31] << 8) + ntp.Content[32]);          //联动组3
            }
            return config;
        }

        /// <summary>
        /// 解析标准组态信息协议 NT8007
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigNT8007Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 17)            //总共24字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", ntp.Content[0]);
                byte[] tempBytes = new byte[3];
                Array.Copy(ntp.Content, 1, tempBytes, 0, tempBytes.Length);
                config.DeviceNo1 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [1 ~ 3]
                Array.Copy(ntp.Content, 4, tempBytes, 0, tempBytes.Length);
                config.DeviceNo2 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [4 ~ 6]
                Array.Copy(ntp.Content, 7, tempBytes, 0, tempBytes.Length);
                config.DeviceNo3 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件3 [7 ~ 9]
                Array.Copy(ntp.Content, 10, tempBytes, 0, tempBytes.Length);
                config.DeviceNo4 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [10 ~ 12]

                config.ActionCoefficient = ntp.Content[13];                             //动作常数

                config.LinkageNo1 = string.Format("{0:D4}", ntp.Content[14]);           //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", ntp.Content[15]);           //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", ntp.Content[16]);           //联动组3
            }
            return config;
        }

        /// <summary>
        /// 解析标准组态信息协议 NT8021
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigNT8021Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析标准组态信息协议 NT8036
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigNT8036Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 17)            //总共24字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", ntp.Content[0]);
                byte[] tempBytes = new byte[3];
                Array.Copy(ntp.Content, 1, tempBytes, 0, tempBytes.Length);
                config.DeviceNo1 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件1 [1 ~ 3]
                Array.Copy(ntp.Content, 4, tempBytes, 0, tempBytes.Length);
                config.DeviceNo2 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件2 [4 ~ 6]
                Array.Copy(ntp.Content, 7, tempBytes, 0, tempBytes.Length);
                config.DeviceNo3 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件3 [7 ~ 9]
                Array.Copy(ntp.Content, 10, tempBytes, 0, tempBytes.Length);
                config.DeviceNo4 = string.Format("{0:D3}{1:D2}{2:D3}", tempBytes[0], tempBytes[1], tempBytes[2]);       //联动器件4 [10 ~ 12]

                config.ActionCoefficient = ntp.Content[13];                             //动作常数

                config.LinkageNo1 = string.Format("{0:D4}", ntp.Content[14]);           //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", ntp.Content[15]);           //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", ntp.Content[16]);           //联动组3
            }
            return config;
        }

        /// <summary>
        /// 解析标准组态信息协议 NT8053
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigStandard GetStandardConfigNT8053Response(NTP ntp)
        {
            LinkageConfigStandard config = new LinkageConfigStandard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 107)            //总共114字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                byte[] tempBytes = new byte[7];
                Array.Copy(ntp.Content, 2, tempBytes, 0, tempBytes.Length);
                config.DeviceNo1 = GetModualString(tempBytes);                              //输入模块1 [2 ~ 8]
                Array.Copy(ntp.Content, 9, tempBytes, 0, tempBytes.Length);
                config.DeviceNo2 = GetModualString(tempBytes);                              //输入模块2 [9 ~ 15]
                Array.Copy(ntp.Content, 16, tempBytes, 0, tempBytes.Length);
                config.DeviceNo3 = GetModualString(tempBytes);                              //输入模块3 [16 ~ 22]
                Array.Copy(ntp.Content, 23, tempBytes, 0, tempBytes.Length);
                config.DeviceNo4 = GetModualString(tempBytes);                              //输入模块4 [23 ~ 29]
                Array.Copy(ntp.Content, 30, tempBytes, 0, tempBytes.Length);
                config.DeviceNo5 = GetModualString(tempBytes);                              //输入模块5 [30 ~ 36]
                Array.Copy(ntp.Content, 37, tempBytes, 0, tempBytes.Length);
                config.DeviceNo6 = GetModualString(tempBytes);                              //输入模块6 [37 ~ 43]
                Array.Copy(ntp.Content, 44, tempBytes, 0, tempBytes.Length);
                config.DeviceNo7 = GetModualString(tempBytes);                              //输入模块7 [44 ~ 50]
                Array.Copy(ntp.Content, 51, tempBytes, 0, tempBytes.Length);
                config.DeviceNo8 = GetModualString(tempBytes);                              //输入模块8 [51 ~ 57]
                Array.Copy(ntp.Content, 58, tempBytes, 0, tempBytes.Length);
                config.DeviceNo9 = GetModualString(tempBytes);                              //输入模块9 [58 ~ 64]
                Array.Copy(ntp.Content, 65, tempBytes, 0, tempBytes.Length);
                config.DeviceNo10 = GetModualString(tempBytes);                             //输入模块10 [65 ~ 71]
                Array.Copy(ntp.Content, 72, tempBytes, 0, tempBytes.Length);
                config.DeviceNo11 = GetModualString(tempBytes);                             //输入模块11 [72 ~ 78]
                Array.Copy(ntp.Content, 79, tempBytes, 0, tempBytes.Length);
                config.DeviceNo12 = GetModualString(tempBytes);                             //输入模块12 [79 ~ 85]
                config.ActionCoefficient = ntp.Content[86];                                 //动作常数
                Array.Copy(ntp.Content, 87, tempBytes, 0, tempBytes.Length);
                config.OutputDevice1 = GetModualString(tempBytes);                          //输出模块1 [87 ~ 93]
                Array.Copy(ntp.Content, 94, tempBytes, 0, tempBytes.Length);
                config.OutputDevice2 = GetModualString(tempBytes);                          //输出模块2 [94 ~ 100]

                config.LinkageNo1 = string.Format("{0:D4}", (ntp.Content[101] << 8) + ntp.Content[102]);          //联动组1
                config.LinkageNo2 = string.Format("{0:D4}", (ntp.Content[103] << 8) + ntp.Content[104]);          //联动组2
                config.LinkageNo3 = string.Format("{0:D4}", (ntp.Content[105] << 8) + ntp.Content[106]);          //联动组3
            }
            return config;
        }
        #endregion StandardConfig

        #region MixedConfig
        /// <summary>
        /// 解析混合组态信息协议 FT8000
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigFT8000Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析混合组态信息协议 FT8003
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigFT8003Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析混合组态信息协议 NT8001
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigNT8001Response(NTP ntp)
        {
            LinkageConfigMixed config = new LinkageConfigMixed();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 23)            //总共30字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                //机号A 0x00
                config.TypeA = ntp.Content[3] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;       //地址类型A
                config.BuildingNoA = ntp.Content[4];                                                    //楼A
                config.ZoneNoA = ntp.Content[5];                                                        //区A
                config.LayerNoA = ntp.Content[6];                                                       //层A
                config.DeviceTypeCodeA = ntp.Content[7];                                                //设备类型A
                //屏蔽 0x00
                config.ActionType = ntp.Content[9] == 1 ? LinkageActionType.AND : LinkageActionType.OR;        //逻辑类型
                //机号B 0x00
                config.TypeB = ntp.Content[11] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;      //地址类型B
                config.BuildingNoB = ntp.Content[12];                                                   //楼B
                config.ZoneNoB = ntp.Content[13];                                                       //区B
                config.LayerNoB = ntp.Content[14];                                                      //层B
                config.DeviceTypeCodeA = ntp.Content[15];                                               //设备类型B
                //机号C 0x00
                config.TypeB = ntp.Content[17] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;      //地址类型C
                config.BuildingNoB = ntp.Content[18];                                                   //楼C
                config.ZoneNoB = ntp.Content[19];                                                       //区C
                config.LayerNoB = ntp.Content[20];                                                      //层C
                config.DeviceTypeCodeA = ntp.Content[21];                                               //设备类型C
                config.ActionCoefficient = ntp.Content[22];                                             //动作常数
            }
            return config;
        }

        /// <summary>
        /// 解析混合组态信息协议 NT8007
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigNT8007Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析混合组态信息协议 NT8021
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigNT8021Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析混合组态信息协议 NT8036
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigNT8036Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析混合组态信息协议 NT8053
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigMixed GetMixedConfigNT8053Response(NTP ntp)
        {
            LinkageConfigMixed config = new LinkageConfigMixed();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 25)            //总共32字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                //机号A 0x00
                config.TypeA = ntp.Content[3] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;       //地址类型A
                config.CategoryA = ntp.Content[4];                                                      //类别A
                config.BuildingNoA = ntp.Content[5];                                                    //楼A
                config.ZoneNoA = ntp.Content[6];                                                        //区A
                config.LayerNoA = ntp.Content[7];                                                       //层A
                config.DeviceTypeCodeA = ntp.Content[8];                                                //设备类型A
                //屏蔽 0x00
                config.ActionType = ntp.Content[10] == 1 ? LinkageActionType.AND : LinkageActionType.OR;        //逻辑类型
                //机号B 0x00
                config.TypeB = ntp.Content[12] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;      //地址类型B
                config.CategoryB = ntp.Content[13];                                                     //类别B
                config.BuildingNoB = ntp.Content[14];                                                   //楼B
                config.ZoneNoB = ntp.Content[15];                                                       //区B
                config.LayerNoB = ntp.Content[16];                                                      //层B
                config.DeviceTypeCodeA = ntp.Content[17];                                               //设备类型B
                //机号C 0x00
                config.TypeB = ntp.Content[19] == 1 ? LinkageType.ZoneLayer : LinkageType.Address;      //地址类型C
                config.BuildingNoB = ntp.Content[20];                                                   //楼C
                config.ZoneNoB = ntp.Content[21];                                                       //区C
                config.LayerNoB = ntp.Content[22];                                                      //层C
                config.DeviceTypeCodeA = ntp.Content[23];                                               //设备类型C
                config.ActionCoefficient = ntp.Content[24];                                             //动作常数
            }
            return config;
        }
        #endregion MixedConfig

        #region GeneralConfig
        /// <summary>
        /// 解析通用组态信息协议 FT8000
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigFT8000Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析通用组态信息协议 FT8003
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigFT8003Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析通用组态信息协议 NT8001
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigNT8001Response(NTP ntp)
        {
            LinkageConfigGeneral config = new LinkageConfigGeneral();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 18)            //总共25字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                //机号A 0x00
                //地址类型A 0x00
                config.BuildingNoA = ntp.Content[4];                                                    //楼A
                config.ZoneNoA = ntp.Content[5];                                                        //区A
                config.LayerNoA1 = ntp.Content[6];                                                      //层A1
                config.LayerNoA2 = ntp.Content[7];                                                      //层A2
                config.DeviceTypeCodeA = ntp.Content[8];                                                //设备类型A
                //屏蔽 0x00
                //逻辑类型 0x00
                if(SerialClient.CurrentControllerVersion == ControllerVersion.NewVersion8)
                {
                    config.MachineNoC = string.Format("{0:D3}", ntp.Content[11]);                           //机号C
                }
                else
                {
                    config.MachineNoC = string.Format("{0:D2}", ntp.Content[11]);                           //机号C
                }
                
                int typeC = ntp.Content[12];                                                            //地址类型C
                switch (typeC)
                {
                    case 3:
                        config.TypeC = LinkageType.AdjacentLayer;
                        break;
                    case 2:
                        config.TypeC = LinkageType.SameLayer;
                        break;
                    case 1:
                        config.TypeC = LinkageType.ZoneLayer;
                        break;
                    default:
                    case 0:
                        config.TypeC = LinkageType.Address;
                        break;
                }
                config.BuildingNoC = ntp.Content[13];                                                   //楼C
                config.ZoneNoC = ntp.Content[14];                                                       //区C
                config.LayerNoC = ntp.Content[15];                                                      //层C
                config.DeviceTypeCodeC = ntp.Content[16];                                               //设备类型C
                config.ActionCoefficient = ntp.Content[17];                                             //动作常数 
            }
            return config;
        }

        /// <summary>
        /// 解析通用组态信息协议 NT8007
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigNT8007Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析通用组态信息协议 NT8021
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigNT8021Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析通用组态信息协议 NT8036
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigNT8036Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析通用组态信息协议 NT8053
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static LinkageConfigGeneral GetGeneralConfigNT8053Response(NTP ntp)
        {
            LinkageConfigGeneral config = new LinkageConfigGeneral();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 19)            //总共26字节 -  NTP头部7字节
            {
                config.Code = string.Format("{0:D4}", (ntp.Content[0] << 8) + ntp.Content[1]);
                config.CategoryA = ntp.Content[4];                                                      //类别A
                config.BuildingNoA = ntp.Content[5];                                                    //楼A
                config.ZoneNoA = ntp.Content[6];                                                        //区A
                config.LayerNoA1 = ntp.Content[7];                                                      //层A1
                config.LayerNoA2 = ntp.Content[8];                                                      //层A2
                config.DeviceTypeCodeA = ntp.Content[9];                                                //设备类型A
                config.MachineNoC = string.Format("{0:D3}", ntp.Content[12]);                           //机号C
                int typeC = ntp.Content[13];                                                            //地址类型C
                switch (typeC)
                {
                    case 3:
                        config.TypeC = LinkageType.AdjacentLayer;
                        break;
                    case 2:
                        config.TypeC = LinkageType.SameLayer;
                        break;
                    case 1:
                        config.TypeC = LinkageType.ZoneLayer;
                        break;
                    default:
                    case 0:
                        config.TypeC = LinkageType.Address;
                        break;
                }
                config.BuildingNoC = ntp.Content[14];                                                   //楼C
                config.ZoneNoC = ntp.Content[15];                                                       //区C
                config.LayerNoC = ntp.Content[16];                                                      //层C
                config.DeviceTypeCodeC = ntp.Content[17];                                               //设备类型C
                config.ActionCoefficient = ntp.Content[18];                                             //动作常数               
            }
            return config;
        }
        #endregion GeneralConfig

        #region ManualBoard
        /// <summary>
        /// 解析手控盘信息协议 FT8000
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardFT8000Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析手控盘信息协议 FT8003
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardFT8003Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析手控盘信息协议 NT8001
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardNT8001Response(NTP ntp)
        {
            ManualControlBoard manual = new ManualControlBoard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 57)            //总共64字节 -  NTP头部7字节
            {
                manual.BoardNo = ntp.Content[0];                                            //板卡号  
                manual.MaxSubBoardNo = ntp.Content[1];                                      //记录当前板卡下的最大“手控盘号”
                manual.SubBoardNo = ntp.Content[2];                                         //盘号 
                manual.KeyNo = ntp.Content[3];                                              //键号 
                switch (SerialClient.CurrentControllerVersion)
                {
                    case ControllerVersion.NewVersion8:
                        manual.DeviceCode = string.Format("{0:D3}{1:D2}{2:D3}", ntp.Content[4], ntp.Content[5], ntp.Content[6]);
                        break;
                    case ControllerVersion.NewVersion7:
                    case ControllerVersion.OldVersion7:
                        manual.DeviceCode = string.Format("{0:D2}{1:D2}{2:D3}", ntp.Content[4], ntp.Content[5], ntp.Content[6]);
                        break;
                }
            }

            return manual;
        }

        /// <summary>
        /// 解析手控盘信息协议 NT8007
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardNT8007Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析手控盘信息协议 NT8021
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardNT8021Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析手控盘信息协议 NT8036
        /// 该设备不支持
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardNT8036Response(NTP ntp)
        {
            return null;
        }

        /// <summary>
        /// 解析手控盘信息协议 NT8053
        /// </summary>
        /// <param name="ntp"></param>
        /// <returns></returns>
        private static ManualControlBoard GetManualBoardNT8053Response(NTP ntp)
        {
            ManualControlBoard manual = new ManualControlBoard();
            if (ntp != null && ntp.Content != null && ntp.Content.Length == 57)            //总共64字节 -  NTP头部7字节
            {
                manual.Code = ntp.Content[0];                                               //手控盘 编号 
                manual.KeyNo = ntp.Content[1];                                              //手控盘 键号
                switch (ntp.Content[2])
                {
                    case 0x00:
                    case 0x01:
                    case 0x02:
                    case 0x03:
                        manual.ControlType = ntp.Content[2];
                        break;
                    case 0x80:
                        manual.ControlType = 4;
                        break;
                }
                byte[] tempBytes = new byte[5];
                Array.Copy(ntp.Content, 3, tempBytes, 0, tempBytes.Length);
                manual.LocalDevice1 = GetDeviceString(tempBytes);                               //本机设备1 [3 ~ 7]
                Array.Copy(ntp.Content, 8, tempBytes, 0, tempBytes.Length);
                manual.LocalDevice2 = GetDeviceString(tempBytes);                               //本机设备2 [8 ~ 12]
                Array.Copy(ntp.Content, 13, tempBytes, 0, tempBytes.Length);
                manual.LocalDevice3 = GetDeviceString(tempBytes);                               //本机设备3 [13 ~ 17]
                Array.Copy(ntp.Content, 18, tempBytes, 0, tempBytes.Length);
                manual.LocalDevice4 = GetDeviceString(tempBytes);                               //本机设备4 [18 ~ 22]
                manual.BuildingNo = ntp.Content[23].ToString();                                 //楼号
                manual.AreaNo = ntp.Content[24].ToString();                                     //区号
                manual.FloorNo = ntp.Content[25].ToString();                                    //层号
                manual.DeviceType = ntp.Content[26];                                            //设备类型
                manual.LinkageGroup = string.Format("{0:D4}", (ntp.Content[27] << 8) + ntp.Content[28]);    //输出组
                Array.Copy(ntp.Content, 29, tempBytes, 0, tempBytes.Length);
                manual.NetDevice1 = GetDeviceString(tempBytes);                                 //网络设备1 [29 ~ 35]
                Array.Copy(ntp.Content, 36, tempBytes, 0, tempBytes.Length);
                manual.NetDevice2 = GetDeviceString(tempBytes);                                 //网络设备2 [36 ~ 42]
                Array.Copy(ntp.Content, 43, tempBytes, 0, tempBytes.Length);
                manual.NetDevice3 = GetDeviceString(tempBytes);                                 //网络设备3 [43 ~ 49]
                Array.Copy(ntp.Content, 50, tempBytes, 0, tempBytes.Length);
                manual.NetDevice4 = GetDeviceString(tempBytes);                                 //网络设备4 [50 ~ 56]
            }

            return manual;
        }
        #endregion ManualBoard

        #region Common
        /// <summary>
        /// 获取回路
        /// </summary>
        /// <param name="loopID"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        private static LoopModel GetLoopFromController(int loopID, ControllerModel controller)
        {
            LoopModel loop = null;
            foreach (var l in controller.Loops)
            {
                if (l.ID == loopID)
                {
                    loop = l;
                    break;
                }
            }
            if (loop == null)
            {
                loop = new LoopModel();
                loop.ID = loopID;
                controller.Loops.Add(loop);
            }
            return loop;
        }

        /// <summary>
        /// 获取输入模块 输出模块 字符串
        /// NT8053 标准组态
        /// </summary>
        /// <returns></returns>
        private static string GetModualString(byte[] bytes)
        {
            string modual = "00000000,00000000";
            if(bytes != null && bytes.Length == 7)
            {
                char sign = ',';
                if(bytes[3] == 0x55)
                {
                    sign = '~';
                }
                modual = string.Format("{0:D3}{1:D2}{2:D3}{3}{4:D3}{5:D2}{6:D3}", bytes[0], bytes[1], bytes[2], sign, bytes[4], bytes[5], bytes[6]);
            }
            return modual;
        }

        /// <summary>
        /// 获取本机设备 网络设备
        /// NT8053 手动盘
        /// </summary>
        /// <returns></returns>
        private static string GetDeviceString(byte[] bytes)
        {
            string modual = "00000,00000";
            if (bytes != null && bytes.Length == 5)
            {
                char sign = ',';
                if (bytes[3] == 0x55)
                {
                    sign = '~';
                }
                modual = string.Format("{0:D2}{1:D3}{2}{3:D2}{4:D3}", bytes[0], bytes[1], sign, bytes[3], bytes[4]);
            }
            return modual;
        }
        #endregion Common
        #endregion Private Method
    }
}
