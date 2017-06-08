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
* Create Date: 2017/4/25 14:53:01
* FileName   : ControllerType8021
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{
    public class ControllerType8021 : ControllerTypeBase
    {
        private List<DeviceInfo8021> _lstDeviceInfo8021;
        public ControllerType8021(SerialComManager serialManager,IProtocolDriver protocolDriver):base(serialManager,protocolDriver )
        {
            base.ControllerModel = new Model.ControllerModel(ControllerType.NT8021);
            
        }
        public List<DeviceInfo8021> DeviceInfoList
        {
            get
            {
                return _lstDeviceInfo8021;
            }
            set
            {
                _lstDeviceInfo8021 = value;
                if (_lstDeviceInfo8021 != null)
                {
                    DownloadedDeviceInfoTotalAmountInCurrentLoop = _lstDeviceInfo8021.Count; //设置当前回路的器件总数2017-04-06
                }
            }
        }      

        protected override void SetDownloadedDeviceInfoTotalAmountInCurrentLoop(Model.LoopModel loopModel)
        {
            DownloadedDeviceInfoTotalAmountInCurrentLoop = loopModel.GetDevices<DeviceInfo8021>().Count; //设置当前回路的器件总数2017-04-06
        }

        public override void SendDeviceInfo()
        {
            if (CurrentLoopForDownloadedDeviceInfo == null)
            {
                DeviceInfoList = Loops[0].GetDevices<DeviceInfo8021>().ToList<DeviceInfo8021>();
            }

            if (RemoveLoopWhenDeviceDownloaded())
            {
                if (Loops.Count > 0)
                {
                    DeviceInfoList = Loops[0].GetDevices<DeviceInfo8021>().ToList<DeviceInfo8021>();
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
        public byte[] AssemblePackageBB(Model.DeviceInfo8021 deviceInfo)//各控制器不同
        {
            //List<byte[]> lstSendData = new List<byte[]>();
            //foreach (Model.DeviceInfo8036 singleDevInfo in deviceInfo)
            //{
            Model.DeviceInfo8021 singleDevInfo = deviceInfo;
            byte[] sendData = new byte[47];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            //??
            sendData[5] = 0x29;　//数据长度 ?? 固定29?
            sendData[6] = 0xBB;　//发送器件命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
            //sendData[7] = Convert.ToByte(0x2E);　//器件总数
            sendData[7] = Convert.ToByte(base.DownloadedDeviceInfoTotalAmountInCurrentLoop);　//器件总数
            sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
            sendData[9] = Convert.ToByte(singleDevInfo.Loop.SimpleCode);　//回路号
            //回路的Code已经包含了机器号 2017-04-05
            sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Code.Length, 3));　//地编号  

            sendData[11] = Convert.ToByte(GetDevType(singleDevInfo.TypeCode) * 8 + (singleDevInfo.Disable==true?1:0) * 4);//器件状态（屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”

            if (singleDevInfo.TypeCode <= 17)
            {
                sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //  内部设备类型
            }
            switch (singleDevInfo.TypeCode)
            { 
                case 18 :
                    sendData[12]=Convert.ToByte(37); //设备类型
                    break;
                case 19:
                    sendData[12]=Convert.ToByte(33); //设备类型
                    break;
                case 20:
                    sendData[12]=Convert.ToByte(34); //设备类型
                    break;
                case 21:
                    sendData[12]=Convert.ToByte(35); //设备类型
                    break;
                case 22:
                    sendData[12]=Convert.ToByte(36); //设备类型
                    break;
            }

            sendData[13] = Convert.ToByte(singleDevInfo.TypeCode); //  外部设备类型


            //电流报警值 
            float? tempValue = singleDevInfo.CurrentThreshold == null ? 0 : singleDevInfo.CurrentThreshold;
            sendData[14] = Convert.ToByte(tempValue / 256); 
            sendData[15] = Convert.ToByte(tempValue % 256); 

            //温度报警值
            tempValue = singleDevInfo.TemperatureThreshold == null ? 0 : singleDevInfo.TemperatureThreshold;
            sendData[16] = Convert.ToByte(tempValue); 
           
            //17~33为安装地点
            //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
            int startIndex = 17;
            if (singleDevInfo.Location != null)
            { 
                char[] charArrayLocation = singleDevInfo.Location.ToArray();
                //采用Base64编码传递数据
                System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
                for (int j = 0; j < charArrayLocation.Length; j++)
                {
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
            }
            //补足位数
            for (int j = startIndex; j < 42; j++)
            {
                sendData[j] = 0x00;
            }

            sendData[42] = 0x00;//固定值
            //楼号
            sendData[43] = Convert.ToByte(singleDevInfo.BuildingNo);
            //区号
            sendData[44] = Convert.ToByte(singleDevInfo.ZoneNo);

            //层号
            if (singleDevInfo.FloorNo < 0 && singleDevInfo.FloorNo > -10)
            {
                sendData[45] = Convert.ToByte(singleDevInfo.FloorNo + 256);
            }
            else
            {
                sendData[45] = Convert.ToByte(singleDevInfo.FloorNo);
            }
            

            //房间号
            sendData[46] = Convert.ToByte(singleDevInfo.RoomNo);
            byte[] checkValue = base.m_ProtocolDriver.CheckValue(sendData, 6,47);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];
            return sendData;
        }
        public Int16? GetDevType(Int16? deviceType)//器件类型,//各控制器不同
        {
            switch (deviceType)
            {
                case 3: return 3;
                case 8: return 8;
                case 18: return 2;
                default: return 2;
            }
        }
        #endregion

        public override void ReceiveDeviceInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.DeviceInfo8021> c = new DeviceCompare();
            Model.DeviceInfo8021 deviceInfo = ParsePackageCC(receivedData);
            if (!DeviceInfoList.Contains(deviceInfo, c))
            {
                DeviceInfoList.Add(deviceInfo);
                //更新已上传的器件数量
                base.UploadedDeviceInfoAccumulatedAmountInCurrentLoop++;
                base.UploadedDeviceInfoTotalAmountInCurrentLoop = 4000;
                base.UpdateProcessBarStatusForUploadedDevice();
                
                
            }
        }
        #region 数据比较，暂时用于去重, 有时间需要确认重复数据产生的原因2017-04-12
        class DeviceCompare : IEqualityComparer<DeviceInfo8021>
        {
            public bool Equals(DeviceInfo8021 x, DeviceInfo8021 y)
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

            public int GetHashCode(DeviceInfo8021 obj)
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
        private Model.DeviceInfo8021 ParsePackageCC(byte[] devicePackage)
        {
            ControllerModel cModel = new ControllerModel(2, "8021", ControllerType.NT8021, 3);
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
            loop.Code = Convert.ToInt16(devicePackage[9]).ToString().PadLeft(2,'0');
            loop.DeviceAmount = tempDeviceAmount;
            loop.Controller = cModel;
            DeviceInfo8021 deviceInfo = new DeviceInfo8021();
            deviceInfo.Loop = loop;
            //deviceInfo.Loop.Code =devicePackage[9].ToString();
            //器件编码
            deviceInfo.Code = tempMachineNo + loop.Code +  Convert.ToInt16(devicePackage[10]).ToString().PadLeft(3,'0');

            //deviceInfo.MachineNo = tempMachineNo;
            //.Fields("geli") = CStr((m_arrInputData(nStartPos + 4) Mod 8) \ 4)

            //.Fields("lingmd") = CStr(m_arrInputData(nStartPos + 4) Mod 4 + 1)
            //屏蔽
            deviceInfo.Disable = Convert.ToInt16((devicePackage[11] % 8) / 4)==1?true:false;
            //灵敏度
            //deviceInfo.SensitiveLevel = Convert.ToInt16((devicePackage[11] % 4) + 1);

            //第12位无用???

            //设备类型
            short tempType = Convert.ToInt16(devicePackage[13]);
  
            deviceInfo.TypeCode = tempType;//.ToString().PadLeft(3,'0');

            //电流预警值 .Fields("DLbaojing") = Format(DLbj, "000#")
            float? tempValue;//临时变量
            tempValue = devicePackage[14] * 256 + devicePackage[15];
            deviceInfo.CurrentThreshold = tempValue==0?null:tempValue;

            //温度预警值 
            tempValue = devicePackage[16];
            deviceInfo.TemperatureThreshold = tempValue == 0 ? null : tempValue;
          

            byte[] bLocation = new byte[25];

            for (int i = 17; i < 42; i++)
            {
                bLocation[i - 17] = devicePackage[i];
            }

            deviceInfo.Location = ascii.GetString(bLocation).Trim('\0');
   
            if(devicePackage[43] == 0)
            {
                deviceInfo.BuildingNo = null ;
            }
            else
            {
                deviceInfo.BuildingNo = devicePackage[43];
            }
            if (devicePackage[44] == 0)
            {
                deviceInfo.ZoneNo = null;
            }
            else
            {
                deviceInfo.ZoneNo = devicePackage[44];
            }
            if (devicePackage[45] == 0)
            {
                deviceInfo.FloorNo = null;
            }
            else
            {
                deviceInfo.FloorNo = devicePackage[45];
            }
            if (devicePackage[46] == 0)
            {
                deviceInfo.RoomNo = null;
            }
            else
            {
                deviceInfo.RoomNo = devicePackage[46];
            }

            
            
            
            
            return deviceInfo;
        }
        /// <summary>
        /// 获取控制器上传的全部信息
        /// </summary>
        /// <returns></returns>
        public override ControllerModel GetControllerUploadedInfo()
        {

            LoopModel loop = null; //= new LoopModel();
            int loopAddressLength=2;
            foreach (var device in DeviceInfoList)
            {
                if (loop == null)
                {
                    loop = new LoopModel();                    
                    loop.Controller = device.Loop.Controller;
                    loopAddressLength=loop.Controller.LoopAddressLength;
                    loop.Code = device.Loop.Code.PadLeft(loopAddressLength, '0');
                    loop.Name = loop.Code;
                }
                if (loop.Code == device.Loop.Code.PadLeft(loopAddressLength, '0'))
                {
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8021>(device);
                }
                else
                {
                    loop.Code = loop.Controller.MachineNumber + loop.Code;
                    loop.Name = loop.Code;
                    base.ControllerModel.Loops.Add(loop);
                    loop = new LoopModel();
                    loop.Controller = device.Loop.Controller;
                    loop.Code =  device.Loop.Code.PadLeft(loopAddressLength, '0');
                    loop.Name = loop.Code;
                    device.Loop = loop;
                    loop.SetDevice<DeviceInfo8021>(device);
                }
            }
            //loop.Code = loop.Controller.MachineNumber + loop.Code.PadLeft(loop.Controller.LoopAddressLength, '0');
            //loop.Name = loop.Code;
            if (loop != null) //将最后一个回路增加至控制器的回路中
            {
                loop.Code = loop.Controller.MachineNumber + loop.Code;
                loop.Name = loop.Code;
                base.ControllerModel.Loops.Add(loop);
            }            
            return base.ControllerModel;
        }
        #endregion

        public override LinkageConfigStandard ParsePackageCD(byte[] standardLinkagePackage)
        {
            throw new NotImplementedException();
        }
    }
}
