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
* Create Date: 2017/4/7 9:54:08
* FileName   : ControllerType8007
* Description: 8007控制器通讯类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{

    public class ControllerType8007 : ControllerTypeBase
    {
        
        private List<DeviceInfo8007> _lstDeviceInfo8007;
   
        public List<DeviceInfo8007> DeviceInfoList
        {
            get
            {
                return _lstDeviceInfo8007;
            }
            set
            {
                _lstDeviceInfo8007 = value;
                if (_lstDeviceInfo8007 != null)
                { 
                    DownloadedDeviceInfoTotalAmountInCurrentLoop = _lstDeviceInfo8007.Count; //设置当前回路的器件总数2017-04-06
                }
            }
        }
        public ControllerType8007(SerialComManager serialManager,IProtocolDriver protocolDriver):base(serialManager,protocolDriver )
        {
            base.ControllerModel = new Model.ControllerModel(ControllerType.NT8007);
            SCA.BusinessLib.BusinessLogic.ControllerConfig8007 controllerConfig = new BusinessLogic.ControllerConfig8007();
            base.UploadedStandardLinkageConfigTotalAmount=controllerConfig.GetMaxAmountForStandardLinkageConfig();
        }

        public override void SendDeviceInfo()
        {
            if (CurrentLoopForDownloadedDeviceInfo == null)
            {
                DeviceInfoList = Loops[0].GetDevices<DeviceInfo8007>().ToList<DeviceInfo8007>();
            }

            if (RemoveLoopWhenDeviceDownloaded())
            {
                if (Loops.Count > 0)
                {
                    DeviceInfoList = Loops[0].GetDevices<DeviceInfo8007>().ToList<DeviceInfo8007>();
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
                            
                            ///更新进度条状态
                            base.UpdateProcessBarStatusForDownloadedDevice();
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
        
        public override void ReceiveDeviceInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.DeviceInfo8007> c = new DeviceCompare();
            Model.DeviceInfo8007 deviceInfo = ParsePackageCC(receivedData);
            if (!DeviceInfoList.Contains(deviceInfo, c))
            {
                DeviceInfoList.Add(deviceInfo);
                //更新已上传的器件数量
                base.UploadedDeviceInfoAccumulatedAmountInCurrentLoop++;
                base.UpdateProcessBarStatusForUploadedDevice();
            }
        }


        #region 解析数据包

        /// <summary>
        /// 解析收到的器件数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        private Model.DeviceInfo8007 ParsePackageCC(byte[] devicePackage)
        {
            ControllerModel cModel = new ControllerModel(2, "8007", ControllerType.NT8007, 3);
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
            //loop.Code = Convert.ToInt16(devicePackage[9]).ToString();
            loop.Code = Convert.ToInt16(devicePackage[9]).ToString().PadLeft(2, '0');
            loop.DeviceAmount = tempDeviceAmount;

            loop.Controller = cModel;
            
            

            //** 设置回路中器件总数,用于更新进度状态 2017-04-28
            base.UploadedDeviceInfoTotalAmountInCurrentLoop = tempDeviceAmount;

            DeviceInfo8007 deviceInfo = new DeviceInfo8007();
            deviceInfo.Loop = loop;
            //deviceInfo.LoopID = Convert.ToInt16(devicePackage[9]);
            deviceInfo.Code = tempMachineNo + loop.Code + Convert.ToInt16(devicePackage[10]).ToString().PadLeft(3, '0');
            //器件编码
            //deviceInfo.Code = Convert.ToInt16(devicePackage[10]).ToString();


            //屏蔽
            deviceInfo.Disable = ((devicePackage[11] % 8) / 4)==1?true:false;
            //灵敏度
            deviceInfo.SensitiveLevel = Convert.ToInt16((devicePackage[11] % 4) + 1);
            //设备类型
            short tempType = Convert.ToInt16(devicePackage[12]);
            deviceInfo.TypeCode = tempType;

            int tempValue;//临时变量
            tempValue = devicePackage[13];
            //联动组1
            deviceInfo.LinkageGroup1 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(3, '0');
            tempValue = devicePackage[14]; //* 256 + devicePackage[16];
            //联动组2
            deviceInfo.LinkageGroup2 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(3, '0');

            //tempValue = devicePackage[17] * 256 + devicePackage[18];
            ////联动组3
            //deviceInfo.LinkageGroup3 = tempValue == 0 ? "" : tempValue.ToString().PadLeft(4, '0');

            //第15及16字节为空
            tempValue = devicePackage[15];
         //   deviceInfo.DelayValue = Convert.ToInt16(tempValue);//== 0 ? "" : tempValue.ToString().PadLeft(2, '0');


            //好像是16个字节，需确认字符长度是否正确
            byte[] bLocation = new byte[17]; 

            for (int i = 17; i < 34; i++)
            {
                bLocation[i - 17] = devicePackage[i];
            }

            deviceInfo.Location = ascii.GetString(bLocation).Trim('\0');
            //deviceInfo.BuildingNo = devicePackage[35];
            //deviceInfo.ZoneNo = devicePackage[36];
            //deviceInfo.FloorNo = devicePackage[37];
            //deviceInfo.RoomNo = devicePackage[38];
            if (devicePackage[35] == 0)
            {
                deviceInfo.BuildingNo = null;
            }
            else
            {
                deviceInfo.BuildingNo = devicePackage[35];
            }
            if (devicePackage[36] == 0)
            {
                deviceInfo.ZoneNo = null;
            }
            else
            {
                deviceInfo.ZoneNo = devicePackage[36];
            }
            if (devicePackage[37] == 0)
            {
                deviceInfo.FloorNo = null;
            }
            else
            {
                deviceInfo.FloorNo = devicePackage[37];
            }
            if (devicePackage[38] == 0)
            {
                deviceInfo.RoomNo = null;
            }
            else
            {
                deviceInfo.RoomNo = devicePackage[38];
            }
            return deviceInfo;

        }
        #region 将此方法写为abstract方法
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
            switch (this.ControllerModel.DeviceAddressLength)
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
            this.ControllerModel.Type = ControllerType;
            standardLinkageInfo.Controller = this.ControllerModel;

            //组号
            standardLinkageInfo.Code = (standardLinkagePackage[7]).ToString().PadLeft(3, '0');
            //器件编码
            string strMachineNo, strLoopNo, strDeviceNo; //存储机号，路号，器件编码

            List<string> lstDeviceInfo = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                if (this.ControllerModel.DeviceAddressLength == 8)
                {
                    strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(3, '0');
                }
                else
                {
                    strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(2, '0');
                }

                strLoopNo = standardLinkagePackage[9 + i * 3].ToString().PadLeft(2, '0');
                strDeviceNo = standardLinkagePackage[10 + i * 3].ToString().PadLeft(3, '0');


                if ((strMachineNo + strLoopNo + strDeviceNo) != "0000000" && (strMachineNo + strLoopNo + strDeviceNo) != "00000000" && (strMachineNo + strLoopNo + strDeviceNo).Length == this.ControllerModel.DeviceAddressLength)
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

            strLinkageGroupCode = (standardLinkagePackage[21]).ToString().PadLeft(3, '0');
            standardLinkageInfo.LinkageNo1 = strLinkageGroupCode == "000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[22]).ToString().PadLeft(3, '0');
            standardLinkageInfo.LinkageNo2 = strLinkageGroupCode == "000" ? "" : strLinkageGroupCode;

            strLinkageGroupCode = (standardLinkagePackage[23]).ToString().PadLeft(3, '0');
            standardLinkageInfo.LinkageNo3 = strLinkageGroupCode == "000" ? "" : strLinkageGroupCode;

            return standardLinkageInfo;
        }
        #endregion
        #endregion


        #region 数据比较，暂时用于去重, 有时间需要确认重复数据产生的原因2017-04-12
        class DeviceCompare : IEqualityComparer<DeviceInfo8007>
        {
            public bool Equals(DeviceInfo8007 x, DeviceInfo8007 y)
            {
                if (x.Code == y.Code && x.Loop.Code == y.Loop.Code)//&& x.MachineNo == y.MachineNo
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public int GetHashCode(DeviceInfo8007 obj)
            {
                throw new NotImplementedException();
            }
        }
        class StandardLinkageInfoCompare : IEqualityComparer<LinkageConfigStandard>
        {
            public bool Equals(LinkageConfigStandard x, LinkageConfigStandard y)
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

            public int GetHashCode(LinkageConfigStandard obj)
            {
                throw new NotImplementedException();
            }
        }
        #endregion


        public override void ReceiveGeneralLinkageInfo()
        {
            throw new NotImplementedException();
        }

        public override void ReceiveManualControlBoardInfo()
        {
            throw new NotImplementedException();
        }

        protected override void SetDownloadedDeviceInfoTotalAmountInCurrentLoop(LoopModel loopModel)
        {
            DownloadedDeviceInfoTotalAmountInCurrentLoop = loopModel.GetDevices<DeviceInfo8007>().Count; //设置当前回路的器件总数2017-04-06            
        }

        #region 组装信息
        public byte[] AssemblePackageBB(Model.DeviceInfo8007 deviceInfo)//各控制器不同
        {
            //List<byte[]> lstSendData = new List<byte[]>();
            //foreach (Model.DeviceInfo8036 singleDevInfo in deviceInfo)
            //{
            Model.DeviceInfo8007 singleDevInfo = deviceInfo;
            byte[] sendData = new byte[43];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            //??
            sendData[5] = 0x25;　//固定为0x25 否则下位机无法正确识别
            sendData[6] = 0xBB;　//发送器件命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数

            sendData[7] = Convert.ToByte(base.DownloadedDeviceInfoTotalAmountInCurrentLoop);　//器件总数<对于下传的数据没什么用>
            sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
            sendData[9] = Convert.ToByte(singleDevInfo.Loop.SimpleCode);　//回路号

            //sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Controller.MachineNumber.Length + singleDevInfo.Loop.Code.Length, 3));　//地编号
            //回路的Code已经包含了机器号 2017-04-05
            sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Code.Length, 3));　//地编号  
            //注意：灵敏度需要-1
            sendData[11] = Convert.ToByte(GetDevType(singleDevInfo.TypeCode) * 8 + (singleDevInfo.Disable==true?1:0) * 4 +singleDevInfo.SensitiveLevel-1);//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”
            //GetDevType(CInt(leixing)) * 8 + geli * 4
            sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
            //           dsent(12) = Left(Trim(.Text), 3)    '  设备类型
            if (sendData[12] > 100)
            {
                sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
            }

            sendData[13] = Convert.ToByte(singleDevInfo.LinkageGroup1.NullToZero()); //输出组1
            sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup2.NullToZero()); //输出组2
            sendData[15] = 0x00;
            sendData[16] = 0x00; //防火分区，固定值00

            //17~33为安装地点
            //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
            //Convert.ToBase64String();     
            int startIndex = 17;
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
            for (int j = startIndex; j < 34; j++)
            {
                sendData[j] = 0x00;
            }

            sendData[34] = 0x00;//固定值00
            //楼号
            if (singleDevInfo.BuildingNo == null)
            {
                sendData[35] = 0x00;
            }
            else
            {
                sendData[35] = Convert.ToByte(singleDevInfo.BuildingNo);
            }
            //区号
            if (singleDevInfo.ZoneNo == null)
            {
                sendData[36] = 0x00;
            }
            else
            {
                sendData[36] = Convert.ToByte(singleDevInfo.ZoneNo);
            }
            //层号
            if (singleDevInfo.FloorNo == null)
            {
                sendData[37] = 0x00;
            }
            else
            {
                sendData[37] = Convert.ToByte(singleDevInfo.FloorNo);
            }
            //房间号
            if (singleDevInfo.RoomNo == null)
            {
                sendData[38] = 0x00;
            }
            else
            {
                sendData[38] = Convert.ToByte(singleDevInfo.RoomNo);
            
            }
            sendData[39] = 0x00;
            sendData[30] = 0x00;
            sendData[41] = 0x00;
            sendData[42] = 0x00;

            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6, 42);
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

        #region
        /// <summary>
        /// 获取控制器上传的全部信息
        /// </summary>
        /// <returns></returns>
        public  override ControllerModel GetControllerUploadedInfo()
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
                    loop.SetDevice<DeviceInfo8007>(device);
                }
                else
                {
                    loop.Code = loop.Controller.MachineNumber + loop.Code;
                    loop.Name = loop.Code;
                    base.ControllerModel.Loops.Add(loop);
                    loop = new LoopModel();
                    loop.Controller = device.Loop.Controller;
                    loop.Code = device.Loop.Code.PadLeft(loopAddressLength, '0');
                    loop.Name = loop.Code;
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8007>(device);
                    //base.ControllerModel.Loops.Add(loop);
                    //loop = new LoopModel();
                    //loop.ID = Convert.ToInt32(device.LoopID);
                    //loop.SetDevice<DeviceInfo8007>(device);
                }
            }
            if (loop != null) //将最后一个回路增加至控制器的回路中
            {
                loop.Code = loop.Controller.MachineNumber + loop.Code;
                loop.Name = loop.Code;

                ControllerModel.Loops.Add(loop);
            }
            foreach (var standard in StandardLinkageConfigList)
            {
                ControllerModel.StandardConfig.Add(standard);
            }
            return ControllerModel;
        }
        #endregion

        public override void ReceiveMixedLinkageInfo()
        {
            throw new NotImplementedException();
        }
    }
}
