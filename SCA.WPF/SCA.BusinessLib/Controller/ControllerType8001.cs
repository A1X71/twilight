using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.BusinessLib.Utility;
using SCA.BusinessLib.ComCommunication;
/* ==============================
*
* Author     : William
* Create Date: 2017/1/6 8:55:18
* FileName   : ControllerType8001
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{
    public class ControllerType8001 : ControllerTypeBase
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int m_manualControlBoardCount=0;// 网络手控盘接收数据记录
        private const int  ANYALARMCODE=-8; //任意火警值 

        private List<DeviceInfo8001> lstDeviceInfo;
        private List<LinkageConfigStandard> lstStandardLinkageInfo;
        private List<LinkageConfigMixed> lstMixedLinkageInfo;
        private List<LinkageConfigGeneral> lstGeneralLinkageInfo;
        private List<ManualControlBoard> lstManualControlBoard;
        public ControllerType8001(SerialComManager serialManager,IProtocolDriver protocolDriver):base(serialManager,protocolDriver )
        {
            base.ControllerModel = new Model.ControllerModel(ControllerType.NT8001);
            m_manualControlBoardCount = 0;
        }

        /// <summary>
        /// 器件信息集合
        /// </summary>
        public List<DeviceInfo8001> DeviceInfoList 
        {
               get
               {

                   if (lstDeviceInfo == null)
                   {
                       lstDeviceInfo = new List<DeviceInfo8001>();
                   }
                   return lstDeviceInfo;
                }
                set
                {
                    if (lstDeviceInfo == null)
                    {
                        lstDeviceInfo = new List<DeviceInfo8001>();
                    }
                    else
                    {
                        lstDeviceInfo = value;
                    }
                }
        }


        //public List<LinkageConfigStandard> StandardLinkageConfigList { get; set; } commented at 2017-01-23
        public List<LinkageConfigStandard> StandardLinkageConfigList
        {
            get
            {

                if (lstStandardLinkageInfo == null)
                {
                    lstStandardLinkageInfo = new List<LinkageConfigStandard>();
                }
                return lstStandardLinkageInfo;
            }
            set
            {
                if (lstStandardLinkageInfo == null)
                {
                    lstStandardLinkageInfo = new List<LinkageConfigStandard>();
                }
                else
                {
                    lstStandardLinkageInfo = value;
                }
            }
        }
        public List<LinkageConfigMixed> MixedLinkageConfigList
        {
            get
            {

                if (lstMixedLinkageInfo == null)
                {
                    lstMixedLinkageInfo = new List<LinkageConfigMixed>();
                }
                return lstMixedLinkageInfo;
            }
            set
            {
                if (lstMixedLinkageInfo == null)
                {
                    lstMixedLinkageInfo = new List<LinkageConfigMixed>();
                }
                else
                {
                    lstMixedLinkageInfo = value;
                }
            }
        }
        public List<LinkageConfigGeneral> GeneralLinkageConfigList 
        {
            get
            {

                if (lstGeneralLinkageInfo == null)
                {
                    lstGeneralLinkageInfo = new List<LinkageConfigGeneral>();
                }
                return lstGeneralLinkageInfo;
            }
            set
            {
                if (lstGeneralLinkageInfo == null)
                {
                    lstGeneralLinkageInfo = new List<LinkageConfigGeneral>();
                }
                else
                {
                    lstGeneralLinkageInfo = value;
                }
            }

        
        }
        public List<ManualControlBoard> ManualControlBoardList 
        {
            get
            {

                if (lstManualControlBoard == null)
                {
                    lstManualControlBoard = new List<ManualControlBoard>();
                }
                return lstManualControlBoard;
            }
            set
            {
                if (lstManualControlBoard == null)
                {
                    lstManualControlBoard = new List<ManualControlBoard>();
                }
                else
                {
                    lstManualControlBoard = value;
                }
            }

        
        }
        
        public override void SendDeviceInfo()
        {
            if (DeviceInfoList != null)
            {
                if (DeviceInfoList.Count > 0)
                {
                    //List<byte[]> lstPackagesByteses = AssemblePackageBB(DeviceInfoList);
                    byte[] sendingData = AssemblePackageBB(DeviceInfoList[0]);
                    //int i = 0;
                    //while (i < lstPackagesByteses.Count)
                    //{
                    if (SendingCMD == "BB" && ReceivedBBConfirmCommand) //如果先前发送的BB命令，已收到66确认命令,则发送下一条数据
                    {
                        //if (lstPackagesByteses.Count > 0)
                        if (DeviceInfoList.Count > 0) //移除已发送的数据
                        {
                            //lstPackagesByteses.RemoveAt(0);
                            DeviceInfoList.RemoveAt(0);
                        }

                        if (DeviceInfoList.Count > 0)  //##edit 
                        {
                            sendingData = AssemblePackageBB(DeviceInfoList[0]);
                            SendingCMD = "BB";
                            //log.Info("Send BB Message && 66:");
                            ReceivedBBConfirmCommand = false;
                            SerialManager.WriteData(sendingData);

                        }
                        else
                        {
                            //需要发送设置完成
                            SendingCMD = "BA";
                            SerialManager.WriteData(base.AssemblePackageBA());
                            ReceivedBAConfirmCommand = false;
                            Status = ControllerStatus.DataSended;
                        }


                    }
                    else　//否则重发本条数据
                    {
                        if (sendingData != null)
                        {
                            SendingCMD = "BB";
                            // log.Info("Send BB Message && Without 66:");
                            SerialManager.WriteData(sendingData);
                        }
                    }
                }
                else
                {
                    Status = ControllerStatus.DataSended;
                }
                // }
                //Status = ControllerStatus.DataSended; //数据发送结束
            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
        }

        public override void SendStandardLinkageConfigInfo()
        {
            if (StandardLinkageConfigList.Count > 0)
            {

                byte[] sendingData = AssemblePackageBC(StandardLinkageConfigList[0]);

                if (SendingCMD == "BC" && ReceivedBCConfirmCommand) //如果先前发送的BC命令，已收到66确认命令,则发送下一条数据
                {
                    //if (lstPackagesByteses.Count > 0)
                    if (StandardLinkageConfigList.Count > 0) //移除已发送的数据
                    {

                        StandardLinkageConfigList.RemoveAt(0);
                    }

                    if (StandardLinkageConfigList.Count > 0)  //##edit 
                    {
                        sendingData = AssemblePackageBC(StandardLinkageConfigList[0]);
                        SendingCMD = "BC";
                       // log.Info("Send BC Message && 66:");
                        ReceivedBCConfirmCommand = false;
                        SerialManager.WriteData(sendingData);

                    }
                    else
                    {
                        SendingCMD = "BA";
                        //需要发送设置完成
                        SerialManager.WriteData(base.AssemblePackageBA());
                        ReceivedBAConfirmCommand = false;
                        Status = ControllerStatus.DataSended;
                    }
                }
                else　//否则重发本条数据
                {
                    if (sendingData != null)
                    {
                        SendingCMD = "BC";
                      //  log.Info("Send BC Message && Without 66:");
                        SerialManager.WriteData(sendingData);
                    }
                }

            }
            else        
            {
                Status = ControllerStatus.DataSended;
            }
            
        }
        /// <summary>
        /// 下传混合组态信息
        /// </summary>
        public override void SendMixedLinkageConfigInfo()
        {
            if (MixedLinkageConfigList.Count > 0)
            {

                byte[] sendingData = AssemblePackageBD(MixedLinkageConfigList[0]);

                if (SendingCMD == "BD" && ReceivedBDConfirmCommand) //如果先前发送的BC命令，已收到66确认命令,则发送下一条数据
                {
                    //if (lstPackagesByteses.Count > 0)
                    if (MixedLinkageConfigList.Count > 0) //移除已发送的数据
                    {
                        MixedLinkageConfigList.RemoveAt(0);
                    }

                    if (MixedLinkageConfigList.Count > 0)  //##edit 
                    {
                        sendingData = AssemblePackageBD(MixedLinkageConfigList[0]);
                        SendingCMD = "BD";
                    //    log.Info("Send BD Message && 66:");
                        ReceivedBDConfirmCommand = false;
                        SerialManager.WriteData(sendingData);

                    }
                    else
                    {
                        SendingCMD = "BA";
                        //需要发送设置完成
                        SerialManager.WriteData(base.AssemblePackageBA());
                        ReceivedBAConfirmCommand = false;
                        Status = ControllerStatus.DataSended;
                    }


                }
                else　//否则重发本条数据
                {
                    if (sendingData != null)
                    {
                        SendingCMD = "BD";
                       // log.Info("Send BD Message && Without 66:");
                        SerialManager.WriteData(sendingData);
                    }
                }
            }
            else        
            {
                Status = ControllerStatus.DataSended;
            }
            
        }
        public override void SendGeneralLinkageConfigInfo()
        {

            if (GeneralLinkageConfigList.Count > 0)
            {

                byte[] sendingData = AssemblePackageBE(GeneralLinkageConfigList[0]);

                if (SendingCMD == "BE" && ReceivedBEConfirmCommand) //如果先前发送的BC命令，已收到66确认命令,则发送下一条数据
                {
                    //if (lstPackagesByteses.Count > 0)
                    if (GeneralLinkageConfigList.Count > 0) //移除已发送的数据
                    {

                        GeneralLinkageConfigList.RemoveAt(0);
                    }

                    if (GeneralLinkageConfigList.Count > 0)  //##edit 
                    {
                        sendingData = AssemblePackageBE(GeneralLinkageConfigList[0]);
                        SendingCMD = "BE";
                     //   log.Info("Send BE Message && 66:");
                        ReceivedBEConfirmCommand = false;
                        SerialManager.WriteData(sendingData);

                    }
                    else
                    {
                        SendingCMD = "BA";
                        //需要发送设置完成
                        SerialManager.WriteData(base.AssemblePackageBA());
                        ReceivedBAConfirmCommand = false;
                        Status = ControllerStatus.DataSended;
                    }


                }
                else　//否则重发本条数据
                {
                    if (sendingData != null)
                    {
                        SendingCMD = "BE";
                      //  log.Info("Send BE Message && Without 66:");
                        SerialManager.WriteData(sendingData);
                    }
                }

            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
        }

        public override void ReceiveDeviceInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.DeviceInfo8001> c = new DeviceCompare();
            Model.DeviceInfo8001 deviceInfo = ParsePackageCC(receivedData);
            if (!DeviceInfoList.Contains(deviceInfo, c))
            {
                DeviceInfoList.Add(deviceInfo);
            }
            

        }
        class DeviceCompare : IEqualityComparer<DeviceInfo8001>
        {
            public bool Equals(DeviceInfo8001 x, DeviceInfo8001 y)
            {
                if (x.Code == y.Code && x.Loop.Code == y.Loop.Code && x.MachineNo == y.MachineNo)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(DeviceInfo8001 obj)
            {
                throw new NotImplementedException();
            }
        }
        class StandardLinkageInfoCompare : IEqualityComparer<LinkageConfigStandard>
        {
            public bool Equals(LinkageConfigStandard x, LinkageConfigStandard y)
            {
                if (x.Code == y.Code )
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(LinkageConfigStandard obj)
            {
                throw new NotImplementedException();
            }
        }
        class MixedLinkageInfoCompare : IEqualityComparer<LinkageConfigMixed>
        {
            public bool Equals(LinkageConfigMixed x, LinkageConfigMixed y)
            {
                if (x.Code == y.Code)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(LinkageConfigMixed obj)
            {
                throw new NotImplementedException();
            }
        }
        class GeneralLinkageInfoCompare : IEqualityComparer<LinkageConfigGeneral>
        {
            public bool Equals(LinkageConfigGeneral x, LinkageConfigGeneral y)
            {
                if (x.Code == y.Code)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(LinkageConfigGeneral obj)
            {
                throw new NotImplementedException();
            }
        }
        class ManualControlBoardInfoCompare : IEqualityComparer<ManualControlBoard>
        {
            public bool Equals(ManualControlBoard x, ManualControlBoard y)
            {
                if (x.BoardNo == y.BoardNo && x.SubBoardNo==y.SubBoardNo && x.KeyNo==y.KeyNo )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(ManualControlBoard obj)
            {
                throw new NotImplementedException();
            }
        }

        private  Int16 GetDevType(Int16 deviceType, Int16 featureValue)
        {
            switch (ConvertSwitchCondition(deviceType))
            {
                case 0: return 0;
                case -1: return deviceType; //1-10
                case -2: return 12; //11-12
                case 13: return 13;
                case 14: return 17;
                case 15: return 15;
                case -3: return 4;  //16 - 17
                case -4: return 2;  //18 - 22
                case 23: return 18;
                case 24: return 27;
                case 25: return 6;
                case 26: return 7;
                case 27: return 4;
                case 28: return 4;
                case 29: return 4;
                case 30: return 4;

                case 31: return 23;
                case 32: return 24;
                case 33: return 21;
                case 34: return 22;
                case 35: return 24;
                case 36: return 29;

                case -6:  //37 - 65
                    if (featureValue == 0)
                    {
                        return 24;
                    }
                    else if (featureValue == 1)
                    {
                        return 23;
                    }
                    else if (featureValue == 2)
                    {
                        return 31;
                    }
                    else if (featureValue == 3)
                    {
                        return 30;
                    }
                    return 0;//此处应加异常日志，正常状态不应为0

                case 66: return 29;
                case 67: return 20;
                case 68: return 30;
                case -7: return 31; //69-70               
                case -8: return 24; //71 - 73
                case 74: return 28;
                case 75: return 25;
                case 76: return 26;
                case 77: return 27;
                case 78: return 28;
                case 79: return 18;
                case 80: return 19;
                case 81: return 14;
                case -9: return 16; //82 - 86
                case -10: return 11; //87 - 89
                case -11: return 23;//101 - 129
                default: return 16;
            }
        }

        private byte[] AssemblePackageBB(Model.DeviceInfo8001 deviceInfo)
        {
            //List<byte[]> lstSendData = new List<byte[]>();
           // foreach (Model.DeviceInfo8001 singleDevInfo in deviceInfo)
          //  {
            Model.DeviceInfo8001 singleDevInfo = deviceInfo;

                byte[] sendData = new byte[54];
                sendData[0] = 0xAA;
                sendData[1] = 0x55;
                sendData[2] = 0xDA;
                sendData[3] = 0x00; //异或值校验
                sendData[4] = 0x00;//累加和校验
                //??
                sendData[5] = 0x30;　//数据长度 ?? 暂定为01 ??
                sendData[6] = 0xBB;　//发送器件命令
                //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
                sendData[7] = Convert.ToByte(0x13);　//器件总数
                sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
                sendData[9] = Convert.ToByte(singleDevInfo.Loop.Code);　//回路号
                sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Code.Length, 3));　//地编号
                //singleDevInfo.Loop.Controller.MachineNumber.Length + 
                sendData[11] = Convert.ToByte(GetDevType(Convert.ToInt16(singleDevInfo.TypeCode), Convert.ToInt16(singleDevInfo.Feature)) * 8 + (singleDevInfo.Disable==true?1:0) * 4 + singleDevInfo.SensitiveLevel-1);//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”

                //GetDevType(CInt(leixing)) * 8 + geli * 4
                sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
                //           dsent(12) = Left(Trim(.Text), 3)    '  设备类型
                if (sendData[12] > 100)
                {
                    sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
                }

                sendData[13] = Convert.ToByte("00"); //输出组1高位
                sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup1); //输出组1低位

                sendData[15] = Convert.ToByte("00"); //输出组2 高位
                sendData[16] = Convert.ToByte(singleDevInfo.LinkageGroup2); //输出组2 低位

                sendData[17] = Convert.ToByte("00"); //输出组2 高位
                sendData[18] = Convert.ToByte(singleDevInfo.LinkageGroup3); //输出组2 低位



                sendData[19] = Convert.ToByte(singleDevInfo.DelayValue); //延时
                //                DipSkpAddr = Trim(.Text)
                //If DipSkpAddr = "" Then DipSkpAddr = 0

                //SpkMun = DipSkpJAdr * 1024 + (DipSkpAdr - 1) * 63 + (DipSkpAddr - 1) + 1
                //If SpkMun >= 0 Then
                //dsent(20) = SpkMun \ 256
                //dsent(21) = SpkMun Mod 256

                sendData[20] = 0x00; //手控盘号 高位
                sendData[21] = 0x00;// 手控盘号 低位

                sendData[22] = 0x00; //防火分区 高位'原软件中直接写为0了
                sendData[23] = 0x00;// 防火分区 低位 '原软件中直接写为0了
                //If vGbzone = "" Then vGbzone = 0
                sendData[24] = 0x00; //广播分区
                //25~49为安装地点
                //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
                //Convert.ToBase64String();     
                int startIndex = 25;
                char[] charArrayLocation = singleDevInfo.Location.ToArray();
                //采用Base64编码传递数据
                System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
                for (int j = 0; j < charArrayLocation.Length; j++)
                {
                    //sendData[startIndex]=Convert.ToByte(Convert.ToBase64String(System.Text.Encoding.GetEncoding(54936).GetBytes(charArrayLocation[j].ToString())));

                    Byte[] encodedBytes = ascii.GetBytes(charArrayLocation[j].ToString());
                    if (encodedBytes.Length == 1)
                    {
                        sendData[startIndex] = encodedBytes[0];
                        startIndex++;
                    }
                    else
                    {
                        sendData[startIndex] = encodedBytes[0];
                        startIndex++;
                        sendData[startIndex] = encodedBytes[1];
                        startIndex++;
                    }


                }
                //补足位数
                for (int j = startIndex; j < 49; j++)
                {
                    sendData[j] = 0x00;
                }


                //楼号
                if (singleDevInfo.BuildingNo == null)
                {
                    sendData[50] = 0x00;
                }
                else
                {
                    sendData[50] = Convert.ToByte(singleDevInfo.BuildingNo);
                }
                //区号
                if (singleDevInfo.ZoneNo == null)
                {
                    sendData[51] = 0x00;
                }
                else
                {
                    sendData[51] = Convert.ToByte(singleDevInfo.ZoneNo);
                }
                //层号
                if (singleDevInfo.FloorNo == null)
                {
                    sendData[52] = 0x00;
                }
                else
                {
                    sendData[52] = Convert.ToByte(singleDevInfo.FloorNo);
                }
                //房间号
                if (singleDevInfo.RoomNo == null)
                {
                    sendData[53] = 0x00;
                }
                else
                {
                    sendData[53] = Convert.ToByte(singleDevInfo.RoomNo);
                }
                byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 54);
                sendData[3] = checkValue[0];
                sendData[4] = checkValue[1];
                //lstSendData.Add(sendData);
           // }
            //return lstSendData;
                return sendData;
        }

        /// <summary>
        /// 器件
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        private   List<byte[]> AssemblePackageBB(List<Model.DeviceInfo8001> deviceInfo)
        {
            List<byte[]> lstSendData = new List<byte[]>();
            foreach (Model.DeviceInfo8001 singleDevInfo in deviceInfo)
            {
                byte[] sendData = new byte[54];
                sendData[0] = 0xAA;
                sendData[1] = 0x55;
                sendData[2] = 0xDA;
                sendData[3] = 0x00; //异或值校验
                sendData[4] = 0x00;//累加和校验
                //??
                sendData[5] = 0x48;　//数据长度 ?? 暂定为01 ??
                sendData[6] = 0xBB;　//发送器件命令
                //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
                sendData[7] = Convert.ToByte(0xFC);　//器件总数
                sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
                sendData[9] = Convert.ToByte(singleDevInfo.Loop.Code);　//回路号
                sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Controller.MachineNumber.Length + singleDevInfo.Loop.Code.Length, 3));　//地编号

                sendData[11] = Convert.ToByte(GetDevType(Convert.ToInt16 (singleDevInfo.TypeCode), Convert.ToInt16(singleDevInfo.Feature)) * 8 + (singleDevInfo.Disable==true?1:0) * 4 + singleDevInfo.SensitiveLevel-1);//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”

                //GetDevType(CInt(leixing)) * 8 + geli * 4
                sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
                //           dsent(12) = Left(Trim(.Text), 3)    '  设备类型
                if (sendData[12] > 100)
                {
                    sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
                }

                //sendData[13] = Convert.ToByte("00"); //输出组1高位
                //sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup1); //输出组1低位

                //sendData[15] = Convert.ToByte("00"); //输出组2 高位
                //sendData[16] = Convert.ToByte(singleDevInfo.LinkageGroup2); //输出组2 低位

                //sendData[17] = Convert.ToByte("00"); //输出组2 高位
                //sendData[18] = Convert.ToByte(singleDevInfo.LinkageGroup3); //输出组2 低位

                sendData[13] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup1.NullToZero()) / 256)); //输出组1高位            
                sendData[14] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup1.NullToZero()) % 256)); //输出组1低位

                sendData[15] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup2.NullToZero()) / 256)); //输出组2 高位
                sendData[16] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup2.NullToZero()) % 256)); //输出组2 低位

                sendData[17] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup3.NullToZero()) / 256)); //输出组3 高位
                sendData[18] = Convert.ToByte((Convert.ToInt16(singleDevInfo.LinkageGroup3.NullToZero()) % 256)); //输出组3 低位




                sendData[19] = Convert.ToByte(singleDevInfo.DelayValue); //延时
                //                DipSkpAddr = Trim(.Text)
                //If DipSkpAddr = "" Then DipSkpAddr = 0

                //SpkMun = DipSkpJAdr * 1024 + (DipSkpAdr - 1) * 63 + (DipSkpAddr - 1) + 1
                //If SpkMun >= 0 Then
                //dsent(20) = SpkMun \ 256
                //dsent(21) = SpkMun Mod 256

                sendData[20] = 0x00; //手控盘号 高位
                sendData[21] = 0x00;// 手控盘号 低位

                sendData[22] = 0x00; //防火分区 高位'原软件中直接写为0了
                sendData[23] = 0x00;// 防火分区 低位 '原软件中直接写为0了
                //If vGbzone = "" Then vGbzone = 0
                sendData[24] = 0x00; //广播分区
                //25~49为安装地点
                //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
                //Convert.ToBase64String();     
                int startIndex = 25;
                char[] charArrayLocation = singleDevInfo.Location.ToArray();
                //采用Base64编码传递数据
                System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
                for (int j = 0; j < charArrayLocation.Length; j++)
                {
                    //sendData[startIndex]=Convert.ToByte(Convert.ToBase64String(System.Text.Encoding.GetEncoding(54936).GetBytes(charArrayLocation[j].ToString())));
                    Byte[] encodedBytes = ascii.GetBytes(charArrayLocation[j].ToString());
                    if (encodedBytes.Length == 1)
                    {
                        sendData[startIndex] = encodedBytes[0];
                        startIndex++;
                    }
                    else
                    {
                        sendData[startIndex] = encodedBytes[0];
                        startIndex++;
                        sendData[startIndex] = encodedBytes[1];
                        startIndex++;
                    }
                }
                //补足位数
                for (int j = startIndex; j < 49; j++)
                {
                    sendData[j] = 0x00;
                }


                //楼号
                if (singleDevInfo.BuildingNo == null)
                {
                    sendData[50] = 0x00;
                }
                else
                {
                    sendData[50] = Convert.ToByte(singleDevInfo.BuildingNo);
                }
                //区号
                if (singleDevInfo.ZoneNo == null)
                {
                    sendData[51] = 0x00;
                }
                else
                {
                    sendData[51] = Convert.ToByte(singleDevInfo.ZoneNo);
                }
                //层号
                if (singleDevInfo.FloorNo == null)
                {
                    sendData[52] = 0x00;
                }
                else
                {
                    sendData[52] = Convert.ToByte(singleDevInfo.FloorNo);
                }
                //房间号
                if (singleDevInfo.RoomNo == null)
                {
                    sendData[53] = 0x00;
                }
                else
                {
                    sendData[53] = Convert.ToByte(singleDevInfo.RoomNo);
                }
                byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 54);
                sendData[3] = checkValue[0];
                sendData[4] = checkValue[1];
                lstSendData.Add(sendData);
            }
            return lstSendData;
        }

        /// <summary>
        /// 标准组态
        /// </summary>
        /// <param name="standardConfig"></param>
        /// <returns></returns>
        protected override byte[] AssemblePackageBC(Model.LinkageConfigStandard standardConfig)
        { 
            


            byte[] sendData = new byte[40];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            //??
            sendData[5] = 0x22;　//固定值
            sendData[6] = 0xBC;　//发送标准组态命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
            sendData[7] = 0x00;//输出组编号(高位）  //!!!!
            sendData[8] = Convert.ToByte(standardConfig.Code);//输出组编号(低位）

            //遍历所有的联动模块信息
            int intDeviceInfoIndex = 0;
            foreach(string deviceInfo in standardConfig.GetDeviceNoList )
            { 
               
                byte[] byteTempValue = SplitDeviceCode(deviceInfo);
                sendData[9 + intDeviceInfoIndex*3] = byteTempValue[0];　//控制器号
                sendData[10 + intDeviceInfoIndex * 3] = byteTempValue[1];　//控制器号
                sendData[11 + intDeviceInfoIndex * 3] = byteTempValue[2];　//回路号
                intDeviceInfoIndex++;
            }
            //动作常数
            sendData[33] = Convert.ToByte(standardConfig.ActionCoefficient);//(standardConfig.ActionCoefficient == "" || standardConfig.ActionCoefficient == null) ? "0" : standardConfig.ActionCoefficient);

            //       If liandong = "" Then liandong = "0000"
            //dsent2(34) = CInt(liandong) \ 256
            //dsent2(35) = liandong Mod 256

            //输出组1
            sendData[34] = 0x00;//高位在前
            sendData[35] = Convert.ToByte(standardConfig.LinkageNo1);
            //输出组2
            sendData[36] = 0x00;//高位在前
            sendData[37] = Convert.ToByte(standardConfig.LinkageNo2);
            //输出组3
            sendData[38] = 0x00;//高位在前
            sendData[39] = Convert.ToByte(standardConfig.LinkageNo3);

           
            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 40);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];
            
            return sendData;

            
        }
        /// <summary>
        /// 通用组态
        /// </summary>
        /// <param name="standardConfig"></param>
        /// <returns></returns>
        private byte[] AssemblePackageBE(Model.LinkageConfigGeneral generalConfig)
        {
            ///注意：此处只实现了“楼区层”的情况；地址的机，路，器件号还需要处理。

            byte[] sendData = new byte[25];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            
            sendData[5] = 0x13;　//固定值
            sendData[6] = 0xBE;　//发送标准组态命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
            sendData[7] = 0x00;//输出组编号(高位）  //!!!!
            sendData[8] = Convert.ToByte(generalConfig.Code);//输出组编号(低位）

            sendData[9] = 0x00;
            sendData[10] = 0x00;

            

            sendData[11] = Convert.ToByte(generalConfig.BuildingNoA == null ? 0 : generalConfig.BuildingNoA);
            sendData[12] = Convert.ToByte(generalConfig.ZoneNoA == null ? 0 : generalConfig.ZoneNoA);
            sendData[13] = Convert.ToByte(generalConfig.LayerNoA1 == null ? 0 : (generalConfig.LayerNoA1<0?generalConfig.LayerNoA1+256:generalConfig.LayerNoA1) );
            sendData[14] = Convert.ToByte(generalConfig.LayerNoA2 == null ? 0 : (generalConfig.LayerNoA2<0?generalConfig.LayerNoA2+256:generalConfig.LayerNoA2));

            //设备类型
            //ANYALARMCODE
            int intTemp = 0;
            intTemp=generalConfig.DeviceTypeCodeA==null?0:Convert.ToInt32(generalConfig.DeviceTypeCodeA);
            if (intTemp == ANYALARMCODE)//如果为任意火警，需要转换代码
            {
                sendData[15] = Convert.ToByte(248);
            }
            else
            {
                sendData[15] = Convert.ToByte(intTemp);   
            }
            //固定值 
            sendData[16] = 0x0;

            //固定值 
            sendData[17] = 0x0;

            sendData[18] = Convert.ToByte(generalConfig.MachineNoC == "" ? "0" : generalConfig.MachineNoC);

            //地址类型
            switch(generalConfig.TypeC)
            {
                case LinkageType.Address:
                    sendData[19]=0x0;
                    break;
                case LinkageType.ZoneLayer:
                    sendData[19]=0x1;
                    break;
                case LinkageType.SameLayer:
                    sendData[19]=0x2;
                    break;
                case LinkageType.AdjacentLayer:
                    sendData[19]=0x4;
                    break;
                default:
                    sendData[19]=0x0;
                    break;
            }
             //楼
            sendData[20] = Convert.ToByte(generalConfig.BuildingNoC == null ? 0 : generalConfig.BuildingNoC);
            //区
            sendData[21] = Convert.ToByte(generalConfig.ZoneNoC == null ? 0 : generalConfig.ZoneNoC);
            //层
            intTemp = generalConfig.LayerNoC == null ? 0 : Convert.ToInt32(generalConfig.LayerNoC);
            if (intTemp < 0)
                sendData[22] = Convert.ToByte(intTemp + 256);
            else                           
                sendData[22] = Convert.ToByte(intTemp);

            //设备类型C
            sendData[23] = Convert.ToByte(generalConfig.DeviceTypeCodeC);
            //动作常数
            //sendData[24] = Convert.ToByte((generalConfig.ActionCoefficient == "" || generalConfig.ActionCoefficient == null) ? "0" : generalConfig.ActionCoefficient);
            sendData[24] = Convert.ToByte(generalConfig.ActionCoefficient);        


            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6,25);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];

            return sendData;


        }
        /// <summary>
        /// 混合组态
        /// </summary>
        /// <param name="linkageConfig"></param>
        /// <returns></returns>
        private byte[] AssemblePackageBD(Model.LinkageConfigMixed  linkageConfig)
        {
            ///注意：此处只实现了“楼区层”的情况；地址的机，路，器件号还需要处理。

            byte[] sendData = new byte[30];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验

            sendData[5] = 0x18;　//固定值
            sendData[6] = 0xBD;　//发送混合组态命令
            
            sendData[7] = 0x00;//输出组编号(高位）  //!!!!
            sendData[8] = Convert.ToByte(linkageConfig.Code);//输出组编号(低位）

            sendData[9] = 0x00;//固定值 

            if (linkageConfig.TypeA == LinkageType.Address)
            {
                sendData[10] = 0x00;//地址
            }
            else
            {
                sendData[10] = 0x01;//区层
            }

            sendData[11] = Convert.ToByte(linkageConfig.BuildingNoA == null ? 0 : linkageConfig.BuildingNoA);
            sendData[12] = Convert.ToByte(linkageConfig.ZoneNoA == null ? 0 : linkageConfig.ZoneNoA);
            int tempValue = Convert.ToInt16(linkageConfig.LayerNoA == null ? 0 : linkageConfig.LayerNoA);

            if (tempValue < 0)
            {
                tempValue = tempValue + 256;
            }

            //层A
            sendData[13] = Convert.ToByte(tempValue.ToString());
            //设备类型A
            sendData[14] = Convert.ToByte(linkageConfig.DeviceTypeCodeA  == null ? 0 : linkageConfig.DeviceTypeCodeA);

            //屏蔽 固定值0x00
            sendData[15] = 0x00;      

            //逻辑类型
            if(linkageConfig.ActionType==LinkageActionType.OR)
            {
                sendData[16] = 0x00;
            }
            else
            {
                sendData[16] = 0x01;
            }

            //机号 固定值0x00
            sendData[17] = 0x00;

            if (linkageConfig.TypeB == LinkageType.Address)
            {
                sendData[18] = 0x00;//地址
            }
            else
            {
                sendData[18] = 0x01;//区层
            }
            //楼B
            sendData[19] = Convert.ToByte(linkageConfig.BuildingNoB == null ? 0 : linkageConfig.BuildingNoB);
            //区B
            sendData[20] = Convert.ToByte(linkageConfig.ZoneNoB == null  ? 0 : linkageConfig.ZoneNoB);
            //层B
            tempValue = Convert.ToInt16(linkageConfig.LayerNoB == null ? 0 : linkageConfig.LayerNoB);

            if (tempValue < 0)
            {
                tempValue = tempValue + 256;
            }
            sendData[21] = Convert.ToByte(tempValue);

            //器件类型
            tempValue  = linkageConfig.DeviceTypeCodeB == null ? 0 : Convert.ToInt32(linkageConfig.DeviceTypeCodeB);
            sendData[22] = Convert.ToByte(tempValue);



            //地址类型C
            if (linkageConfig.TypeC == LinkageType.Address)
            {
                sendData[24] = 0x00;//地址
            }
            else
            {
                sendData[24] = 0x01;//区层
            }
//            sendData[23]
            //楼号C
            tempValue = linkageConfig.BuildingNoC == null ? 0 : Convert.ToInt32(linkageConfig.BuildingNoC);
            sendData[23] = Convert.ToByte(tempValue);
            sendData[25] = Convert.ToByte(tempValue);
            //区号C

            sendData[26] = Convert.ToByte(linkageConfig.ZoneNoC == null ? 0 : linkageConfig.ZoneNoC);

            //层号C          
  
            tempValue = Convert.ToInt16(linkageConfig.LayerNoC == null ? 0 : linkageConfig.LayerNoC);
            if (tempValue < 0)
            {
                tempValue = tempValue + 256;
            }

            sendData[27] = Convert.ToByte(tempValue);

            //器件类型C
            tempValue = linkageConfig.DeviceTypeCodeC == null ? 0 : Convert.ToInt32(linkageConfig.DeviceTypeCodeC);
            sendData[28] = Convert.ToByte(tempValue);


            //动作常数
            tempValue = linkageConfig.ActionCoefficient == null ? 0 : Convert.ToInt32(linkageConfig.ActionCoefficient);
            sendData[29] = Convert.ToByte(tempValue);
           


            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6,30);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];

            return sendData;


        }
        /// <summary>
        /// 网络手控盘数据
        /// </summary>
        /// <param name="manualControlBoard"></param>
        /// <returns></returns>
        private byte[] AssemblePackageBF(Model.ManualControlBoard manualControlBoard)
        {
            ///注意：此处只实现了“楼区层”的情况；地址的机，路，器件号还需要处理。

            byte[] sendData = new byte[14];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验

            sendData[5] = 0x08;　//固定值
            sendData[6] = 0xBF;　//发送网络手控盘命令

            //板卡号
            sendData[7] = Convert.ToByte(manualControlBoard.BoardNo);
            //手控盘总数
            //记录当前板卡下的最大“手控盘号”
            sendData[8]=Convert.ToByte(manualControlBoard.MaxSubBoardNo);
            //盘号
            sendData[9]=Convert.ToByte(manualControlBoard.SubBoardNo==0?0:manualControlBoard.SubBoardNo );
            //键号
            sendData[10]=Convert.ToByte(manualControlBoard.KeyNo==0?0:manualControlBoard.KeyNo );
            
            string deviceCode=manualControlBoard.DeviceCode ;
            if (deviceCode=="")
            {
                for(int i=0;i<this.ControllerModel.DeviceAddressLength;i++)
                {
                    deviceCode+="0";
                }                
            }
      
            //机号
            sendData[11]=Convert.ToByte(deviceCode.Substring(0,ControllerModel.DeviceAddressLength-5));
            //路号
            sendData[12]=Convert.ToByte(deviceCode.Substring((ControllerModel.DeviceAddressLength-5),2));
            //地编号
            sendData[13] = Convert.ToByte(deviceCode.Substring((ControllerModel.DeviceAddressLength - 5) + 2));



            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 14);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];

            return sendData;


        }
        /// <summary>
        /// 解析收到的器件数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        private Model.DeviceInfo8001 ParsePackageCC(byte[] devicePackage)
        {

            ControllerModel cModel = new ControllerModel(2, "8001", ControllerType.NT8001,3);
            cModel.MachineNumber = "00";

            //第7字节作为基址

            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            //器件总数
            int tempDeviceAmount = Convert.ToInt16(devicePackage[7]);
            

            //机号
            string tempMachineNo = Convert.ToInt16(devicePackage[8]).ToString();
            switch (base.ControllerModel.DeviceAddressLength)
            { 
                case 7:
                    tempMachineNo =tempMachineNo.PadLeft(2,'0');
                    break;
                case 8:
                    tempMachineNo = tempMachineNo.PadLeft(3, '0');
                    break;
                default ://默认7位，但此分支不应该走，此时应该知道编码位数
                    tempMachineNo = tempMachineNo.PadLeft(2, '0');
                    break;                
            }
            //#---------start
                //将“器件总数”及"回路号"存入系统设置表中-->机号+路号作为回路号
                //回路号作为"存储器件的表名"： 表名的规则为“路号+编号"
                //
            //#----------end

             
            //回路信息
            Model.LoopModel loop =new LoopModel();
            loop.Code =Convert.ToInt16(devicePackage[9]).ToString();
            loop.DeviceAmount = tempDeviceAmount;
            DeviceInfo8001 deviceInfo = new DeviceInfo8001();
            deviceInfo.Loop = loop;
            //器件编码
            deviceInfo.Code = Convert.ToInt16(devicePackage[10]).ToString();

            deviceInfo.MachineNo = tempMachineNo;
            //.Fields("geli") = CStr((m_arrInputData(nStartPos + 4) Mod 8) \ 4)

            //.Fields("lingmd") = CStr(m_arrInputData(nStartPos + 4) Mod 4 + 1)
            //屏蔽
            deviceInfo.Disable=((devicePackage[11] % 8)/4)==1?true:false;
            //灵敏度
            deviceInfo.SensitiveLevel  = Convert.ToInt16((devicePackage[11] % 4) +1);
            //设备类型
            short tempType = Convert.ToInt16(devicePackage[12]);
            //根据设备类型的取值，决定是否需要特性字段信息
            if (tempType > 36 && tempType < 66)
            {
                int tempSubType = Convert.ToInt16((devicePackage[11] +4)/8 );
                switch( tempSubType)
                {
                    case 23:
                        deviceInfo.Feature = 1;
                        break;
                    case 31:
                        deviceInfo.Feature = 2;
                        break;
                    case 30:
                        deviceInfo.Feature = 3;
                        break;
                    default:
                        deviceInfo.Feature = 0;
                        break;
                }
            }

            deviceInfo.TypeCode = tempType;//.ToString().PadLeft(3,'0');
            
            int tempValue;//临时变量
            tempValue = devicePackage[13] * 256 + devicePackage[14];
            //联动组1
            deviceInfo.LinkageGroup1 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4,'0');

            tempValue = devicePackage[15] * 256 + devicePackage[16];
            //联动组2
            deviceInfo.LinkageGroup2 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            tempValue = devicePackage[17] * 256 + devicePackage[18];
            //联动组3
            deviceInfo.LinkageGroup3 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            //延时
            tempValue = devicePackage[19];
            deviceInfo.DelayValue = Convert.ToInt16(tempValue);//== 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            //手操号
            tempValue = devicePackage[20] * 256 + devicePackage[21];
            //板卡号
            deviceInfo.BoardNo=(Int16)Math.Ceiling((tempValue / 1024)-0.9);
            //手盘号
            //.Fields("xianggh") = nShouCao \ 1024
            //    .Fields("panhao") = (nShouCao - (nShouCao \ 1024) * 1024) \ 63 + 1
            //    .Fields("jianhao") = nShouCao - (nShouCao \ 1024) * 1024 - ((nShouCao - (nShouCao \ 1024) * 1024) \ 63) * 63
            deviceInfo.SubBoardNo = Convert.ToInt16(Math.Ceiling(tempValue - (Convert.ToInt32(deviceInfo.BoardNo)) * 1024 / 63 - 0.9) + 1);
            
            //手键号
            deviceInfo.KeyNo  = Convert.ToInt16(tempValue-(Convert.ToInt32(deviceInfo.BoardNo)*1024- (Math.Ceiling((tempValue-Convert.ToInt32(deviceInfo.BoardNo)*1024)/63-0.9))*63));

            if (deviceInfo.KeyNo == 0)
            {
                deviceInfo.KeyNo = 63;
                deviceInfo.SubBoardNo = Convert.ToInt16(deviceInfo.SubBoardNo - 1);
            }
            //广播分区
            tempValue = devicePackage[22];
            if (tempValue == 0)
            {
                deviceInfo.BroadcastZone = "";
            }
            else
            {
                deviceInfo.BroadcastZone = deviceInfo.BroadcastZone.PadLeft(3,'0');
            }

            byte[] bLocation = new byte[24];
            for (int i = 25;  i < 49; i++)
            {
                bLocation[i - 25] = devicePackage[i];
            }
            deviceInfo.Location  = ascii.GetString(bLocation );

            deviceInfo.BuildingNo = devicePackage[50];
            deviceInfo.ZoneNo  = devicePackage[51];
            deviceInfo.FloorNo = devicePackage[52];
            deviceInfo.RoomNo  = devicePackage[53];

            return deviceInfo;
        }
        /// <summary>
        /// 解析收到的标准组态数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        public override Model.LinkageConfigStandard  ParsePackageCD(byte[] standardLinkagePackage)
        {
            //第7字节作为基址

            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            //器件总数
            


            //机号
            string tempMachineNo = Convert.ToInt16(standardLinkagePackage[8]).ToString();
            switch (base.ControllerModel.DeviceAddressLength)
            {
                case 7:
                    tempMachineNo = tempMachineNo.PadLeft(2,'0');
                    break;
                case 8:
                    tempMachineNo =  tempMachineNo.PadLeft(3,'0');
                    break;
                default://默认7位，但此分支不应该走，此时应该知道编码位数
                    tempMachineNo =  tempMachineNo.PadLeft(2,'0');
                    break;
            }
            
            LinkageConfigStandard standardLinkageInfo = new LinkageConfigStandard();
            standardLinkageInfo.Controller = this.ControllerModel;
            //组号 占两个字节，高位在前
            standardLinkageInfo.Code = (standardLinkagePackage[7] * 256 + standardLinkagePackage[8]).ToString().PadLeft(4,'0');
            //器件编码
            string strMachineNo, strLoopNo, strDeviceNo; //存储机号，路号，器件编码

            List<string> lstDeviceInfo=new List<string>();

            for (int i = 0; i < 8; i++)
            {
                if (base.ControllerModel.DeviceAddressLength == 8)
                {
                    strMachineNo = standardLinkagePackage[9 + i * 3].ToString().PadLeft(3,'0');
                }
                else 
                {
                    strMachineNo = standardLinkagePackage[9 + i * 3].ToString().PadLeft(2,'0');
                }

                strLoopNo = standardLinkagePackage[10 + i * 3].ToString().PadLeft(2,'0');
                strDeviceNo = standardLinkagePackage[11 + i * 3].ToString().PadLeft(3, '0');
            

                if ((strMachineNo + strLoopNo + strDeviceNo) != "0000000" && (strMachineNo + strLoopNo + strDeviceNo) != "00000000" && (strMachineNo + strLoopNo + strDeviceNo).Length == base.ControllerModel.DeviceAddressLength)
                {
                    lstDeviceInfo.Add(strMachineNo + strLoopNo + strDeviceNo);
                }
                else
                {
                    lstDeviceInfo.Add("");
                }
             }

            standardLinkageInfo.SetDeviceNoList=lstDeviceInfo;
            //动作常数
            standardLinkageInfo.ActionCoefficient = Convert.ToInt32(standardLinkagePackage[33].ToString().NullToImpossibleValue());
            //输出组1~3
            string strLinkageGroupCode; //输出组号

            strLinkageGroupCode = (standardLinkagePackage[34] * 256 + standardLinkagePackage[35]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo1 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[36] * 256 + standardLinkagePackage[37]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo2 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[38] * 256 + standardLinkagePackage[39]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo3 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            return standardLinkageInfo;
        }
        /// <summary>
        /// 解析接收到的混合组态数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        private Model.LinkageConfigMixed ParsePackageBD(byte[] mixedLinkagePackage)
        {
            //第7字节作为基址

            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            LinkageConfigMixed linkageConfigMixed = new LinkageConfigMixed();

            linkageConfigMixed.Controller = this.ControllerModel;

            //组号 占两个字节，高位在前
            linkageConfigMixed.Code = (mixedLinkagePackage[7] * 256 + mixedLinkagePackage[8]).ToString().PadLeft(4,'0');

            int tempValue=mixedLinkagePackage[9];
            //分类A
            if (tempValue == 1)
            {
                linkageConfigMixed.TypeA = LinkageType.ZoneLayer;

                tempValue = mixedLinkagePackage[11];
                //楼A
                linkageConfigMixed.BuildingNoA = tempValue;//.ToString();
                tempValue = mixedLinkagePackage[12];
                //区A
                linkageConfigMixed.ZoneNoA = tempValue;//.ToString();
                tempValue = mixedLinkagePackage[13];
                if (tempValue > 245)
                {
                    tempValue = 0 - (256 - tempValue);
                }
                //层A
                linkageConfigMixed.LayerNoA = tempValue;//.ToString();
                
                //设备类型A
                tempValue = mixedLinkagePackage[14];
                linkageConfigMixed.DeviceTypeCodeA = Convert.ToInt16(tempValue.ToString().PadLeft(3, '0').NullToZero());//存储器件代码

            }
            else
            {
                linkageConfigMixed.TypeA = LinkageType.Address;
                
                tempValue = mixedLinkagePackage[12];
                //路号
                linkageConfigMixed.LoopNoA = tempValue.ToString();
                //器件编号
                tempValue = mixedLinkagePackage[13];                                
                linkageConfigMixed.DeviceCodeA = tempValue.ToString();
            }
            //动作类型
            tempValue=mixedLinkagePackage[16];
            if (tempValue == 0)
            {
                linkageConfigMixed.ActionType = LinkageActionType.OR;
            }
            else
            {
                linkageConfigMixed.ActionType = LinkageActionType.AND;
            }
            //分类B
            tempValue = mixedLinkagePackage[18];
            if (tempValue == 1)
            {
                linkageConfigMixed.TypeB= LinkageType.ZoneLayer;

                tempValue = mixedLinkagePackage[19];
                //楼B
                linkageConfigMixed.BuildingNoB = tempValue;//.ToString();

                tempValue = mixedLinkagePackage[20];
                //区B
                linkageConfigMixed.ZoneNoB = tempValue;//.ToString();

                tempValue = mixedLinkagePackage[21];
                if (tempValue > 245)
                {
                    tempValue = 0 - (256 - tempValue);
                }
                //层B
                linkageConfigMixed.LayerNoB = tempValue;//.ToString();
                //设备类型B
                tempValue = mixedLinkagePackage[22];
                linkageConfigMixed.DeviceTypeCodeB = Convert.ToInt16(tempValue.ToString().PadLeft(3, '0').NullToZero());//存储器件代码
            }
            else
            {
                linkageConfigMixed.TypeB = LinkageType.Address;

                tempValue = mixedLinkagePackage[20];
                //路号
                linkageConfigMixed.LoopNoB = tempValue.ToString();

                //器件编号
                tempValue = mixedLinkagePackage[21];
                linkageConfigMixed.DeviceCodeB = tempValue.ToString();
            }
            //C分类
            tempValue = mixedLinkagePackage[24];
            if (tempValue == 1)
            {
                linkageConfigMixed.TypeC = LinkageType.ZoneLayer;

                //楼C
                tempValue = mixedLinkagePackage[25];                
                linkageConfigMixed.BuildingNoC = tempValue;//.ToString();

                //区C
                tempValue = mixedLinkagePackage[26];                
                linkageConfigMixed.ZoneNoC = tempValue;//.ToString();

                //层C
                tempValue = mixedLinkagePackage[27];
                if (tempValue > 245)
                {
                    tempValue = 0 - (256 - tempValue);
                }
                
                linkageConfigMixed.LayerNoC = tempValue;//.ToString();
                //设备类型C
                tempValue = mixedLinkagePackage[28];
                linkageConfigMixed.DeviceTypeCodeC = Convert.ToInt16(tempValue.ToString().PadLeft(3, '0').NullToZero());//存储器件代码
            }
            else
            {
                linkageConfigMixed.TypeC = LinkageType.Address ;
                //机号C
                tempValue = mixedLinkagePackage[23];             
                linkageConfigMixed.MachineNoC  = tempValue.ToString();

                //回路C
                tempValue = mixedLinkagePackage[26];                
                linkageConfigMixed.LoopNoC = tempValue.ToString();

                //器件编号C
                tempValue = mixedLinkagePackage[27];
                linkageConfigMixed.DeviceCodeC = tempValue.ToString();
            }
            tempValue = mixedLinkagePackage[29];
            linkageConfigMixed.ActionCoefficient = Convert.ToInt32(tempValue);
            return linkageConfigMixed;
        }
        /// <summary>
        /// 解析通用组态数据
        /// </summary>
        /// <param name="GerneralLinkagePackage"></param>
        /// <returns></returns>
        private Model.LinkageConfigGeneral ParsePackageBE(byte[] generalLinkagePackage)
        {
            //第7字节作为基址

            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            LinkageConfigGeneral linkageConfigGeneral = new LinkageConfigGeneral();

            linkageConfigGeneral.Controller = this.ControllerModel;

            //组号 占两个字节，高位在前
            linkageConfigGeneral.Code = (generalLinkagePackage[7] * 256 + generalLinkagePackage[8]).ToString().PadLeft(4, '0');

            //楼号A
            int tempValue = generalLinkagePackage[10];
            linkageConfigGeneral.BuildingNoA = tempValue;
            //区号A
            tempValue = generalLinkagePackage[11];
            linkageConfigGeneral.ZoneNoA = tempValue;
            //层A1
            tempValue = generalLinkagePackage[12];
            if( tempValue>245 )
            {
                tempValue=0-(256-tempValue );
            }
            linkageConfigGeneral.LayerNoA1 = tempValue;
            //层A2
            tempValue = generalLinkagePackage[13];
            if (tempValue > 245)
            {
                tempValue = 0 - (256 - tempValue);            
            }
            linkageConfigGeneral.LayerNoA2 = tempValue;
            //设备类型A
            tempValue = generalLinkagePackage[14];
            if (tempValue == 248)
            {
                linkageConfigGeneral.DeviceTypeCodeA = -8;
            }
            else
            {
                linkageConfigGeneral.DeviceTypeCodeA = Convert.ToInt16(tempValue);//.ToString().PadLeft(3, '0');
            }

            //地址类型
            tempValue = generalLinkagePackage[18];
            switch (tempValue)
            { 
                case 0:                    
                    linkageConfigGeneral.TypeC = LinkageType.Address;
                    //机号C
                    tempValue = generalLinkagePackage[17];
                    linkageConfigGeneral.MachineNoC = tempValue.ToString();
                    //路号C
                    tempValue = generalLinkagePackage[20];
                    linkageConfigGeneral.LoopNoC = tempValue.ToString();
                    //编号C
                    tempValue = generalLinkagePackage[21];
                    linkageConfigGeneral.DeviceCodeC = tempValue.ToString();
                    break;
                case 1:
                    linkageConfigGeneral.TypeC = LinkageType.ZoneLayer;
                    //楼号C
                    tempValue = generalLinkagePackage[19];
                    linkageConfigGeneral.BuildingNoC = tempValue;
                    //区号C
                    tempValue = generalLinkagePackage[20];
                    linkageConfigGeneral.ZoneNoC = tempValue;
                    //层号C
                    tempValue = generalLinkagePackage[21];
                    if (tempValue > 245)
                    {
                        tempValue = 0 - (256 - tempValue);
                    }
                    linkageConfigGeneral.LayerNoC = tempValue;
                    break;
                case 2://本层
                    linkageConfigGeneral.TypeC = LinkageType.SameLayer;
                    tempValue = generalLinkagePackage[19];
                    linkageConfigGeneral.BuildingNoC = tempValue;
                    //区号C
                    tempValue = generalLinkagePackage[20];
                    linkageConfigGeneral.ZoneNoC = tempValue;
                    
                    break;
                case 3://邻层
                    linkageConfigGeneral.TypeC = LinkageType.AdjacentLayer ;
                    tempValue = generalLinkagePackage[19];
                    linkageConfigGeneral.BuildingNoC = tempValue;
                    //区号C
                    tempValue = generalLinkagePackage[20];
                    linkageConfigGeneral.ZoneNoC = tempValue;
                    break;
            }
            //设备类型C
            tempValue = generalLinkagePackage[22];
            linkageConfigGeneral.DeviceTypeCodeC = Convert.ToInt16(tempValue);//.ToString().PadLeft(3, '0');
            //动作常数
            tempValue = generalLinkagePackage[23];
            linkageConfigGeneral.ActionCoefficient = Convert.ToInt32(tempValue);



            return linkageConfigGeneral;
        }
        /// <summary>
        /// 解析网络手动盘数据
        /// </summary>
        /// <param name="generalLinkagePackage"></param>
        /// <returns></returns>
        private Model.ManualControlBoard  ParsePackageBF(byte[] manualControlBoardPackage)
        {
            //第7字节作为基址

            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            ManualControlBoard manualControlBoard = new ManualControlBoard();

            manualControlBoard.Controller = this.ControllerModel;

            m_manualControlBoardCount++;
            manualControlBoard.Code = m_manualControlBoardCount;
            //板卡号
            int tempValue=manualControlBoardPackage[7];
            manualControlBoard.BoardNo = tempValue;
            //盘号
            tempValue = manualControlBoardPackage[8];
            manualControlBoard.SubBoardNo = tempValue;
            //键号
            tempValue = manualControlBoardPackage[9];
            manualControlBoard.KeyNo = tempValue;
            string strMachineNo;
            if (this.ControllerModel.DeviceAddressLength == 7)
            {
                //机号
                tempValue = manualControlBoardPackage[10];
                strMachineNo = tempValue.ToString().PadLeft(2, '0');
            }
            else
            {
                //机号
                tempValue = manualControlBoardPackage[10];
                strMachineNo = tempValue.ToString().PadLeft(3, '0');
            }
            //器件编号
            manualControlBoard.DeviceCode = strMachineNo + Convert.ToInt16(manualControlBoardPackage[11]).ToString().PadLeft(2, '0') + Convert.ToInt16(manualControlBoardPackage[12]).ToString().PadLeft(3, '0');
            //SDPKey
            manualControlBoard.SDPKey = (Convert.ToInt16(manualControlBoard.BoardNo) * 1024 + Convert.ToInt16(manualControlBoard.SubBoardNo) * 63 + Convert.ToInt16(manualControlBoard.KeyNo)).ToString();
            
            return manualControlBoard;
        }

        /// <summary>
        /// 将器件编号转换为由“机号”，“路号”，“器件编号”组成的byte数组
        /// </summary>
        /// <returns></returns>
        private byte[] SplitDeviceCode(string strDeviceCode)
        {
            int deviceCodeLength = strDeviceCode.Length;
            string machineCode = strDeviceCode.Substring(0, deviceCodeLength - 5);//路号+器件编号长度为5
            string loopCode = strDeviceCode.Substring(machineCode.Length, 2);//路号为2位
            string deviceCode = strDeviceCode.Substring(machineCode.Length+loopCode.Length ,3);//路号为2位
            byte[] returnValue = new byte[] { Convert.ToByte(machineCode), Convert.ToByte(loopCode), Convert.ToByte(deviceCode) };
            return returnValue;
        }
        private Int16 ConvertSwitchCondition(Int16 originalValue)
        {
            if (originalValue >= 1 && originalValue <= 10)
                return -1;
            else if (originalValue >= 11 && originalValue <= 12)
                return -2;
            else if (originalValue >= 16 && originalValue <= 17)
                return -3;
            else if (originalValue >= 18 && originalValue <= 22)
                return -4;
            else if (originalValue >= 24 && originalValue <= 30)
                return -5;
            else if (originalValue >= 37 && originalValue <= 65)
                return -6;
            else if (originalValue >= 69 && originalValue <= 70)
                return -7;
            else if (originalValue >= 71 && originalValue <= 73)
                return -8;
            else if (originalValue >= 82 && originalValue <= 86)
                return -9;
            else if (originalValue >= 87 && originalValue <= 89)
                return -10;
            else if (originalValue >= 101 && originalValue <= 129)
                return -11;
            else
                return originalValue;
        }
        /// <summary>
        /// 接收标准组态信息
        /// </summary>
        public override void ReceiveStandardLinkageInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.LinkageConfigStandard> c = new StandardLinkageInfoCompare();
            Model.LinkageConfigStandard linkageInfo = ParsePackageCD(receivedData);
            if (!StandardLinkageConfigList.Contains(linkageInfo, c))
            {
                StandardLinkageConfigList.Add(linkageInfo);
            }
            
        }
        /// <summary>
        /// 接收控制器上传的混合组态数据
        /// </summary>
        public override void ReceiveMixedLinkageInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.LinkageConfigMixed> c = new MixedLinkageInfoCompare();
            Model.LinkageConfigMixed linkageInfo = ParsePackageBD(receivedData);
            if (!MixedLinkageConfigList.Contains(linkageInfo, c))
            {
                MixedLinkageConfigList.Add(linkageInfo);
            }
        }

        /// <summary>
        /// 接收通用组态信息
        /// </summary>
        public override void ReceiveGeneralLinkageInfo()
        {
           
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.LinkageConfigGeneral> c = new GeneralLinkageInfoCompare();
            Model.LinkageConfigGeneral linkageInfo = ParsePackageBE(receivedData);
            if (!GeneralLinkageConfigList.Contains(linkageInfo, c))
            {
                GeneralLinkageConfigList.Add(linkageInfo);
            }
            
        }
        /// <summary>
        /// 接收网络手动盘信息
        /// </summary>
        public override void ReceiveManualControlBoardInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.ManualControlBoard > c = new  ManualControlBoardInfoCompare();
            Model.ManualControlBoard manualBoardInfo = ParsePackageBF(receivedData);
            if (!ManualControlBoardList.Contains(manualBoardInfo, c))
            {
                ManualControlBoardList.Add(manualBoardInfo);
            }
        
        }

        public override void SendManualControlBoardInfo()
        {
            if (ManualControlBoardList.Count > 0)
            {
                var query = from r in ManualControlBoardList
                            where r.BoardNo == ManualControlBoardList[0].BoardNo
                            select r.SubBoardNo.ToString().Max();
                ManualControlBoardList[0].MaxSubBoardNo = query.ToList().FirstOrDefault();

                byte[] sendingData = AssemblePackageBF(ManualControlBoardList[0]);

                if (SendingCMD == "BF" && ReceivedBFConfirmCommand) //如果先前发送的BC命令，已收到66确认命令,则发送下一条数据
                {
                    //if (lstPackagesByteses.Count > 0)
                    if (ManualControlBoardList.Count > 0) //移除已发送的数据
                    {

                        ManualControlBoardList.RemoveAt(0);
                    }

                    if (ManualControlBoardList.Count > 0)  //##edit 
                    {
                        sendingData = AssemblePackageBF(ManualControlBoardList[0]);
                        SendingCMD = "BF";
                      //  log.Info("Send BF Message && 66:");
                        ReceivedBFConfirmCommand = false;
                        SerialManager.WriteData(sendingData);
                    }
                    else
                    {
                        SendingCMD = "BA";
                        //需要发送设置完成
                        SerialManager.WriteData(base.AssemblePackageBA());
                        ReceivedBAConfirmCommand = false;
                        Status = ControllerStatus.DataSended;
                    }


                }
                else　//否则重发本条数据
                {
                    if (sendingData != null)
                    {
                        SendingCMD = "BF";
                    //    log.Info("Send BF Message && Without 66:");
                        SerialManager.WriteData(sendingData);
                    }
                }

            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
        }
        /// <summary>
        /// 设置同一板卡下的最大盘号信息
        /// </summary>
        //private void SetManualControlBoardMaxSubBoardValue()
        //{
            
        //    foreach (ManualControlBoard info in ManualControlBoardList)
        //    {
        //        var query  = from r in ManualControlBoardList where r.BoardNo == info.BoardNo 
        //                     select  r.SubBoardNo.Max();

        //    }
        //}

        protected override void SetDownloadedDeviceInfoTotalAmountInCurrentLoop(LoopModel loopModel)
        {
            DownloadedDeviceInfoTotalAmountInCurrentLoop = loopModel.GetDevices<DeviceInfo8001>().Count; //设置当前回路的器件总数2017-04-06
        }

        public override ControllerModel GetControllerUploadedInfo()
        {
            throw new NotImplementedException();
        }
    }
}
