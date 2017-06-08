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
* Create Date: 2017/4/24 17:11:14
* FileName   : ControllerType8003
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{
    public class ControllerType8003:ControllerTypeBase
    {
        private List<DeviceInfo8003> _lstDeviceInfo8003;
        private int m_CurrentIndex { get; set; }
        public ControllerType8003(SerialComManager serialManager,IProtocolDriver protocolDriver):base(serialManager,protocolDriver )
        {
            base.ControllerModel = new Model.ControllerModel(ControllerType.FT8003);
            
        }
        public List<DeviceInfo8003> DeviceInfoList
        {
            get
            {
                return _lstDeviceInfo8003;
            }
            set
            {
                _lstDeviceInfo8003 = value;

            }
        }        
        protected override void SetDownloadedDeviceInfoTotalAmountInCurrentLoop(Model.LoopModel loopModel)
        {
            DownloadedDeviceInfoTotalAmountInCurrentLoop = loopModel.GetDevices<DeviceInfo8003>().Count; //设置当前回路的器件总数2017-04-06
        }

        public override void SendDeviceInfo()
        {
            if (CurrentLoopForDownloadedDeviceInfo == null)
            {
                DeviceInfoList = Loops[0].GetDevices<DeviceInfo8003>().ToList<DeviceInfo8003>();
            }

            if (RemoveLoopWhenDeviceDownloaded())
            {
                if (Loops.Count > 0)
                {
                    DeviceInfoList = Loops[0].GetDevices<DeviceInfo8003>().ToList<DeviceInfo8003>();
                }
                else
                {
                    DeviceInfoList = null;
                }
            }
            if (DeviceInfoList != null)
            {
                if (DeviceInfoList.Count > 0)
                {
                    //List<byte[]> lstPackagesByteses = AssemblePackageBB(DeviceInfoList); //commented at 2017-04-05
                    byte[] sendingData = AssemblePackageBB(DeviceInfoList[0]);

                    CurrentLoopForDownloadedDeviceInfo = DeviceInfoList[0].Loop;

                    //int i = 0;
                    //while (i < lstPackagesByteses.Count)
                    //{
                    if (SendingCMD == "BB" && ReceivedBBConfirmCommand) //如果先前发送的BB命令，已收到66确认命令,则发送下一条数据
                    {
                        if (DeviceInfoList.Count > 0)
                        {
                            DeviceInfoList.RemoveAt(0);
                            DownloadedDeviceInfoAccumulatedAmountInCurrentLoop++; //已下传的器件记数 2017-04-06
                        }
                        //i++;
                        if (DeviceInfoList.Count > 0)
                        {
                            sendingData = AssemblePackageBB(DeviceInfoList[0]);
                            SendingCMD = "BB";
                            //     log.Info("Send BB Message && 66:");
                            ReceivedBBConfirmCommand = false;
                            SerialManager.WriteData(sendingData);

                        }
                        else
                        {
                            SetStatusForAllDevicesDownloaded();
                            SendingCMD = "BA";
                            SerialManager.WriteData(base.AssemblePackageBA()); //发送设置完成
                            //Status = ControllerStatus.DataSended;
                            ReceivedBAConfirmCommand = false;


                        }
                        //System.Threading.Thread.Sleep (2000);
                    }
                    else　//否则重发本条数据
                    {
                        //if (lstPackagesByteses.Count > 0)
                        if (sendingData != null)
                        {
                            SendingCMD = "BB";
                            // log.Info("Send BB Message && Without 66:");
                            SerialManager.WriteData(sendingData);
                        }
                        //System.Threading.Thread.Sleep(2000);
                    }
                    //  }            
                }
                else
                {
                    Status = ControllerStatus.DataSended;
                    CurrentLoopForDownloadedDeviceInfo = null;//数据发送完成，当前下前回路置空
                }
            }
            else
            {
                Status = ControllerStatus.DataSended;
                CurrentLoopForDownloadedDeviceInfo = null;//数据发送完成，当前下前回路置空
            }
        }



        public override void ReceiveMixedLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveGeneralLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveManualControlBoardInfo()
        {
            throw new NotImplementedException();
        }

 
        #region 非通用Driver操作,可放在具体类中
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="controllerModel">需要改变ControllerModel中的器件地址长度</param>
        /// <returns></returns>
        public ControllerType GetControllerType(byte[] package, ref Model.ControllerModel controllerModel)
        {
            return base.GetControllerType(ref controllerModel);
        }

        /// <summary>
        /// 装配器件信息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        //public   List<byte[]> AssemblePackageBB(List<Model.DeviceInfo8036 > deviceInfo)//各控制器不同 commented at 2017-04-05
        public byte[] AssemblePackageBB(Model.DeviceInfo8003 deviceInfo)//各控制器不同
        {
            //List<byte[]> lstSendData = new List<byte[]>();
            //foreach (Model.DeviceInfo8036 singleDevInfo in deviceInfo)
            //{
            Model.DeviceInfo8003 singleDevInfo = deviceInfo;
            byte[] sendData = new byte[35];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            //??
            sendData[5] = 0x1D;　//数据长度 ?? 暂定为1D
            sendData[6] = 0xBB;　//发送器件命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
            //sendData[7] = Convert.ToByte(0x2E);　//器件总数
            sendData[7] = Convert.ToByte(base.DownloadedDeviceInfoTotalAmountInCurrentLoop);　//器件总数
            sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
            sendData[9] = Convert.ToByte(singleDevInfo.Loop.SimpleCode);　//回路号

            //sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Controller.MachineNumber.Length + singleDevInfo.Loop.Code.Length, 3));　//地编号
            //回路的Code已经包含了机器号 2017-04-05
            sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Code.Length, 3));　//地编号  
            sendData[11] = Convert.ToByte(GetDevType(singleDevInfo.TypeCode) * 8 + (singleDevInfo.Disable==true?1:0) * 4+(singleDevInfo.SensitiveLevel-1));//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”

            sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
            
            if (sendData[12] > 100)
            {
                sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
            }
            //   dsent(13) = shuchu1 Mod 256
            //dsent(14) = shuchu2 Mod 256
            sendData[13] = Convert.ToByte(singleDevInfo.LinkageGroup1.NullToZero()); //输出组1
            sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup2.NullToZero()); //输出组2
            sendData[15] = Convert.ToByte(singleDevInfo.DelayValue); //延时
            sendData[16] = 0x00; //防火分区，固定值00
            sendData[17] = 0x00; //固定值00
            sendData[18] = Convert.ToByte(singleDevInfo.ZoneNo); 
            //17~33为安装地点
            //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
            int startIndex = 19;
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
            for (int j = startIndex; j <= 34; j++)
            {
                sendData[j] = 0x00;
            }            
            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 34);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];            
            return sendData;
        }
        public Int16? GetDevType(Int16? deviceType)//器件类型,//各控制器不同
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
                case -5: return 4; //24 - 30
                case 31: return 23;
                case 32: return 24;
                case 33: return 21;
                case 34: return 22;
                case 35: return 24;
                case 36: return 29;
                case -6: return 24; //37 - 65
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
        /// <summary>
        /// 辅助GetDevType
        /// </summary>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        private Int16? ConvertSwitchCondition(Int16? originalValue)
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
        #endregion
        public override void ReceiveDeviceInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.DeviceInfo8003> c = new DeviceCompare();
            Model.DeviceInfo8003 deviceInfo = ParsePackageCC(receivedData);
            if (!DeviceInfoList.Contains(deviceInfo, c))
            {
                DeviceInfoList.Add(deviceInfo);
                //更新已上传的器件数量
                base.UploadedDeviceInfoAccumulatedAmountInCurrentLoop++;
                base.UpdateProcessBarStatusForUploadedDevice();
            }
        }
        #region 数据比较，暂时用于去重, 有时间需要确认重复数据产生的原因2017-04-12
        class DeviceCompare : IEqualityComparer<DeviceInfo8003>
        {
            public bool Equals(DeviceInfo8003 x, DeviceInfo8003 y)
            {
                if (x.Code == y.Code && x.Loop.Code == y.Loop.Code)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(DeviceInfo8003 obj)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #region 解析数据包

        /// <summary>
        /// 解析收到的器件数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        private Model.DeviceInfo8003 ParsePackageCC(byte[] devicePackage)
        {
            ControllerModel cModel = new ControllerModel(2, "8003", ControllerType.FT8003, 3);
            cModel.MachineNumber = "00";
            cModel.LoopAddressLength = 2;
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
                    tempMachineNo = tempMachineNo.PadLeft(2, '0');
                    break;
                case 8:
                    tempMachineNo = tempMachineNo.PadLeft(3, '0');
                    break;
                default://默认7位，但此分支不应该走，此时应该知道编码位数
                    tempMachineNo = tempMachineNo.PadLeft(2, '0');
                    break;
            }
            cModel.MachineNumber = tempMachineNo;
            //#---------start
            //将“器件总数”及"回路号"存入系统设置表中-->机号+路号作为回路号
            //回路号作为"存储器件的表名"： 表名的规则为“路号+编号"
            //
            //#----------end


            //回路信息
            Model.LoopModel loop = new LoopModel();
            loop.Code = Convert.ToInt16(devicePackage[9]).ToString().PadLeft(cModel.LoopAddressLength,'0');
            loop.DeviceAmount = tempDeviceAmount;
            loop.Controller = cModel;
            DeviceInfo8003 deviceInfo = new DeviceInfo8003();
            deviceInfo.Loop = loop;
            //deviceInfo.LoopID = Convert.ToInt16(devicePackage[9]);
            //器件编码
            //deviceInfo.Code =cModel.MachineNumber + loop.Code+ Convert.ToInt16(devicePackage[10]).ToString().PadLeft(3,'0');
            deviceInfo.Code = tempMachineNo + loop.Code + Convert.ToInt16(devicePackage[10]).ToString().PadLeft(3, '0');

            //deviceInfo.MachineNo = tempMachineNo;
            //.Fields("geli") = CStr((m_arrInputData(nStartPos + 4) Mod 8) \ 4)

            //.Fields("lingmd") = CStr(m_arrInputData(nStartPos + 4) Mod 4 + 1)
            //屏蔽
            deviceInfo.Disable = Convert.ToInt16((devicePackage[11] % 8) / 4)==1?true:false;
            //灵敏度
             deviceInfo.SensitiveLevel = Convert.ToInt16((devicePackage[11] % 4) + 1);
            //设备类型
            short tempType = Convert.ToInt16(devicePackage[12]);
            //根据设备类型的取值，决定是否需要特性字段信息
            if (tempType > 36 && tempType < 66)
            {
                int tempSubType = Convert.ToInt16(devicePackage[11] / 8);
                switch (tempSubType)
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
            
            deviceInfo.TypeCode = tempType;

            int tempValue;//临时变量
            tempValue = devicePackage[13];// *256 + devicePackage[14];
            //联动组1
            deviceInfo.LinkageGroup1 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            tempValue = devicePackage[14]; //* 256 + devicePackage[16];
            //联动组2
            deviceInfo.LinkageGroup2 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            

            //延时  
            tempValue = devicePackage[15];
            if (tempValue == 0)
            {
                deviceInfo.DelayValue = null;
            }
            else
            {
                deviceInfo.DelayValue = Convert.ToInt16(tempValue);                 
            }
            
            //区号
            deviceInfo.ZoneNo =Convert.ToInt16(devicePackage[18]);


            byte[] bLocation = new byte[16];

            for (int i = 19; i < 35; i++)
            {
                bLocation[i - 19] = devicePackage[i];
            }
            deviceInfo.Location = ascii.GetString(bLocation).Trim('\0');            
            return deviceInfo;
        }

        /// <summary>
        /// 解析收到的标准组态数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        public override Model.LinkageConfigStandard ParsePackageCD(byte[] standardLinkagePackage)
        {
            //第7字节作为基址
            //采用GB18030编码
            System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
            
            //机号
            string tempMachineNo = Convert.ToInt16(standardLinkagePackage[8]).ToString();
            switch (base.ControllerModel.DeviceAddressLength)
            {
                case 7:
                    tempMachineNo = tempMachineNo.PadLeft(2, '0');
                    break;
                case 8:
                    tempMachineNo = tempMachineNo.PadLeft(3, '0');
                    break;
                default://默认7位，但此分支不应该走，此时应该知道编码位数
                    tempMachineNo = tempMachineNo.PadLeft(2, '0');
                    break;
            }

            LinkageConfigStandard standardLinkageInfo = new LinkageConfigStandard();
            this.ControllerModel.Type = ControllerType.NT8036;
            standardLinkageInfo.Controller = this.ControllerModel;

            //组号
            standardLinkageInfo.Code = (standardLinkagePackage[7]).ToString().PadLeft(4, '0');
            //器件编码
            string strMachineNo, strLoopNo, strDeviceNo; //存储机号，路号，器件编码

            List<string> lstDeviceInfo = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                if (base.ControllerModel.DeviceAddressLength == 8)
                {
                    strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(3, '0');
                }
                else
                {
                    strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(2, '0');
                }

                strLoopNo = standardLinkagePackage[9 + i * 3].ToString().PadLeft(2, '0');
                strDeviceNo = standardLinkagePackage[10 + i * 3].ToString().PadLeft(3, '0');


                if ((strMachineNo + strLoopNo + strDeviceNo) != "0000000" && (strMachineNo + strLoopNo + strDeviceNo) != "00000000" && (strMachineNo + strLoopNo + strDeviceNo).Length == base.ControllerModel.DeviceAddressLength)
                {
                    lstDeviceInfo.Add(strMachineNo + strLoopNo + strDeviceNo);
                }
                else
                {
                    lstDeviceInfo.Add("");
                }
            }

            standardLinkageInfo.SetDeviceNoList = lstDeviceInfo;
            //动作常数
            standardLinkageInfo.ActionCoefficient = Convert.ToInt32(standardLinkagePackage[20].ToString().NullToImpossibleValue());
            //输出组1~3
            string strLinkageGroupCode; //输出组号

            strLinkageGroupCode = (standardLinkagePackage[21]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo1 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[22]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo2 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[23]).ToString().PadLeft(4, '0');
            standardLinkageInfo.LinkageNo3 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

            return standardLinkageInfo;
        }
        /// <summary>
        /// 获取控制器上传的全部信息
        /// </summary>
        /// <returns></returns>
        public override ControllerModel GetControllerUploadedInfo()
        {

            LoopModel loop = null; //= new LoopModel();
            int loopAddressLength = 2;
            foreach (var device in DeviceInfoList)
            {
                if (loop == null)
                {
                    loop = new LoopModel();
                    //loop.ID = Convert.ToInt32(device.LoopID);
                    loop.Controller = device.Loop.Controller;
                    loopAddressLength = loop.Controller.LoopAddressLength;
                    loop.Code = device.Loop.Code.PadLeft(loopAddressLength, '0');
                    loop.Name = loop.Code;
                }
                //if (loop.ID == device.LoopID)
                if (loop.Code == device.Loop.Code.PadLeft(loopAddressLength, '0'))
                {
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8003>(device);
                }
                else
                {
                    //base.ControllerModel.Loops.Add(loop);
                    //loop = new LoopModel();
                    //loop.ID = Convert.ToInt32(device.LoopID);
                    //loop.SetDevice<DeviceInfo8003>(device);
                    loop.Code = loop.Controller.MachineNumber + loop.Code;
                    loop.Name = loop.Code;
                    base.ControllerModel.Loops.Add(loop);
                    loop = new LoopModel();
                    loop.Controller = device.Loop.Controller;
                    loop.Code = device.Loop.Code.PadLeft(loopAddressLength, '0');
                    loop.Name = loop.Code;
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8003>(device);
                }
            }
            if (loop != null) //将最后一个回路增加至控制器的回路中
            {
                loop.Code = loop.Controller.MachineNumber + loop.Code;
                loop.Name = loop.Code;
                base.ControllerModel.Loops.Add(loop);                
            }
            foreach (var standard in StandardLinkageConfigList)
            {
                base.ControllerModel.StandardConfig.Add(standard);
            }
            return base.ControllerModel;
        }
        #endregion

    }
}
