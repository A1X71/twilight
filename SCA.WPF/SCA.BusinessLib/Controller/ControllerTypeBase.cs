using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.BusinessLib.ComCommunication;
using SCA.BusinessLib.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2016/12/7 10:01:10
* FileName   : ControllerCommonBase
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ControllerTypeBase
    {
        public event Action<int, int,ControllerNodeType> UpdateProgressBarEvent; 
        protected  IProtocolDriver m_ProtocolDriver;
        private bool _allLoopsDownloaded=false;
        private bool _standardLinkageConfigDownloaded = false;
        private bool _generalLinkageConfigDownloaded = false;
        private bool _mixedLinkageConfigDownloaded = false;
        private bool _manualControlBoardInfoDownloaded = false;
        private LoopModel _currentLoopForDownloadedDeviceInfo;
        private List<LoopModel> _loops;
        

        /// <summary>
        /// 控制器当前待处理数据包
        /// </summary>
        public byte[] CurrentPackage { get; set; }

        protected ControllerTypeBase(SerialComManager serialManager,IProtocolDriver protocolDriver)
        {
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ControllerType = ControllerType.NONE;
            SerialManager = serialManager;
            m_ProtocolDriver = protocolDriver;
        }
        public void InitializeCommunicateStatus()
        {
            OperableDataType = OperantDataType.None;
            Status = ControllerStatus.None;
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            SendingCMD = "";
            AllLoopsDownloaded = false;
            GeneralLinkageConfigDownloaded = false;
            ManualControlBoardInfoDownloaded = false;
            MixedLinkageConfigDownloaded = false;
            StandardLinkageConfigDownloaded = false;
            CurrentLoopForDownloadedDeviceInfo = null;
            CurrentPackage = null;
            DeviceInfoSet = null;
            DownloadedDeviceInfoAccumulatedAmountInCurrentLoop = 0;
            DownloadedDeviceInfoTotalAmountInCurrentLoop = 0;
            Loops = null;
        }
        /// <summary>
        /// 需要下传的回路集合
        /// 用于全部下载
        /// </summary>
        public List<LoopModel> Loops 
        {
            get
            {
                return _loops;
            }
            set
            {
                _loops = value;
                if (_loops != null)
                { 
                    SetDownloadedDeviceInfoTotalAmountInCurrentLoop(_loops[0]);
                }
            }
        }

        /// <summary>
        /// 标准组态信息集合
        /// 2017-04-14 由子类调整至基类中
        /// </summary>
        public List<LinkageConfigStandard> StandardLinkageConfigList { get; set; }
        protected abstract void  SetDownloadedDeviceInfoTotalAmountInCurrentLoop(LoopModel loopModel);
        
       /// <summary>
       /// 当前回路内已经完成下传的累积数量
       /// </summary>
        public int DownloadedDeviceInfoAccumulatedAmountInCurrentLoop { get; set; }
        /// <summary>
        /// 当前回路内的器件信息总数量
        /// </summary>
        public int DownloadedDeviceInfoTotalAmountInCurrentLoop { get; set; }

        /// <summary>
        ///  当前回路内已经完成上传的累积数量 Added at 2017-04-28 未测试
        /// </summary>
        public int UploadedDeviceInfoAccumulatedAmountInCurrentLoop { get; set; }
        /// <summary>
        /// 当前回路内的器件信息总数量 Added at 2017-04-28 未测试
        /// </summary>
        public int UploadedDeviceInfoTotalAmountInCurrentLoop { get; set; }

        /// <summary>
        /// 当前控制器已经完成上传标准组态信息的累积数量 Added at 2017-05-02
        /// </summary>
        public int UploadedStandardLinkageConfigAccumulatedAmount { get; set; }
        /// <summary>
        /// 当前控制器标准组态信息的总数量 Added at 2017-05-02
        /// </summary>
        public int UploadedStandardLinkageConfigTotalAmount { get; set; }

        /// <summary>
        /// 执行下传信息操作的当前回路
        /// </summary>
        public LoopModel CurrentLoopForDownloadedDeviceInfo
        {
            get
            {
                return _currentLoopForDownloadedDeviceInfo;
            }
            set
            {
                _currentLoopForDownloadedDeviceInfo = new LoopModel();
                _currentLoopForDownloadedDeviceInfo = value;
            }
        }
        /// <summary>
        /// 指定回路下传完成
        /// </summary>
        public bool AllLoopsDownloaded 
        {
            get
            {
                return _allLoopsDownloaded;
            }
            set
            {
                _allLoopsDownloaded = value;
            }        
        }
        public bool StandardLinkageConfigDownloaded 
        { 
            get
            {
                return _standardLinkageConfigDownloaded;
            }
            set
            {
                _standardLinkageConfigDownloaded = value;
            }
        
        }
        public bool GeneralLinkageConfigDownloaded 
        { 
            get
            {
                return _generalLinkageConfigDownloaded;
            }
            set
            {
                _generalLinkageConfigDownloaded = value;
            }                 
        }
        public bool MixedLinkageConfigDownloaded
        { get { return _mixedLinkageConfigDownloaded; } set { _mixedLinkageConfigDownloaded = value; } }
        public bool ManualControlBoardInfoDownloaded
        { get { return _manualControlBoardInfoDownloaded; } set { _manualControlBoardInfoDownloaded = value; } }
        public List<DeviceInfoBase> DeviceInfoSet { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public  string SendingCMD { get; set; }
        public bool ReceivedB9ConfirmCommand { get; set; }
        /// <summary>
        /// BA命令确认命令
        /// </summary>
        public bool ReceivedBAConfirmCommand { get; set; }
        //收到器件确认命令
        public bool ReceivedBBConfirmCommand { get; set; }

        //收到标准组态确认命令
        public bool ReceivedBCConfirmCommand { get; set; }

        //收到通用组态确认命令
        public bool ReceivedBEConfirmCommand { get; set; }

        //收到混合组态确认命令
        public bool ReceivedBDConfirmCommand { get; set; }
        /// <summary>
        /// 收到“网络手控盘”确认命令
        /// </summary>
        public bool ReceivedBFConfirmCommand { get; set; }

        /// <summary>
        /// 控制器当前的状态
        /// </summary>
        public ControllerStatus Status { get; set; }

        /// <summary>
        /// 当前操作的数据类型
        /// </summary>
        public OperantDataType OperableDataType { get; set; }


        public SerialComManager SerialManager { get; set; }
        /// <summary>
        /// 控制器类型
        /// </summary>
        public ControllerType  ControllerType { get; set; }

        public abstract void  SendDeviceInfo();
        /// <summary>
        /// 下传标准组态信息
        /// 2017-04-14 调整至基类中
        /// </summary>
        public virtual void SendStandardLinkageConfigInfo()
        {
            if (StandardLinkageConfigList != null)
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
                            //    log.Info("Send BC Message && 66:");
                            ReceivedBCConfirmCommand = false;
                            SerialManager.WriteData(sendingData);

                        }
                        else
                        {
                            SetStatusForStandardLinkageConfigDownloaded();
                            SendingCMD = "BA";
                            //需要发送设置完成
                            SerialManager.WriteData(AssemblePackageBA());
                            ReceivedBAConfirmCommand = false;
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
                    StandardLinkageConfigDownloaded = true;
                }
            }
            else
            {
                Status = ControllerStatus.DataSended;
                StandardLinkageConfigDownloaded = true;
            }
        }


        #region 改为virtual 
        //public abstract void SendMixedLinkageConfigInfo();
        //public abstract void SendGeneralLinkageConfigInfo();
        //public abstract void SendManualControlBoardInfo();
        #endregion
        /// <summary>
        /// 删除已经下传的回路
        /// 用于全部下载
        /// </summary>
        private void RemoveDownloadedLoop()
        {
            if (CurrentLoopForDownloadedDeviceInfo != null)
            {
                var result = from l in Loops where l.Code == CurrentLoopForDownloadedDeviceInfo.Code select l;
                LoopModel loop = result.FirstOrDefault();
                if (loop != null)
                {
                    Loops.Remove(loop);
                    CurrentLoopForDownloadedDeviceInfo = null; //当前回路已删除，清空设置
                }
            }
            if (Loops.Count > 0) //设置当前回路中的器件总数
            { 
                SetDownloadedDeviceInfoTotalAmountInCurrentLoop(_loops[0]);
            }
            
        }
        /// <summary>
        /// 判断是否删除回路
        /// </summary>
        /// <returns>true:执行删除回路动作；false:未执行删除回路动作</returns>
        protected bool RemoveLoopWhenDeviceDownloaded()
        {
            if (DownloadedDeviceInfoAccumulatedAmountInCurrentLoop == DownloadedDeviceInfoTotalAmountInCurrentLoop)//当前回路下的所有器件已经下传完成
            {
                RemoveDownloadedLoop();              //移除当前已下传的回路
                DownloadedDeviceInfoAccumulatedAmountInCurrentLoop = 0; //重置为0                 
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 将所有信息下传至控制器
        /// </summary>
        public void SendAllInfo()
        {
           // Status = ControllerStatus.DataSending;
            if (Loops.Count > 0)
            {
                Status = ControllerStatus.DataSending;                
                SendDeviceInfo();
            }
            //if (DownloadedDeviceInfoAccumulatedAmountInCurrentLoop == DownloadedDeviceInfoTotalAmountInCurrentLoop)//当前回路下的所有器件已经下传完成
            //{ 
            //     RemoveDownloadedLoop();              //移除当前已下传的回路
            //     DownloadedDeviceInfoAccumulatedAmountInCurrentLoop = 0; //重置为0                                  
            //}
            RemoveLoopWhenDeviceDownloaded();           
            

            if(Loops.Count==0) //如果回路已经全部下传完成，将回路下传标志置True
            {
                AllLoopsDownloaded=true;
            }

            if (AllLoopsDownloaded && !StandardLinkageConfigDownloaded && ReceivedBAConfirmCommand) 
            {
                
                Status = ControllerStatus.DataSending;
                SendStandardLinkageConfigInfo();
            }

            if (StandardLinkageConfigDownloaded && !MixedLinkageConfigDownloaded && ReceivedBAConfirmCommand)
            {
                Status = ControllerStatus.DataSending;
                SendMixedLinkageConfigInfo();
            }
            if (MixedLinkageConfigDownloaded && !GeneralLinkageConfigDownloaded && ReceivedBAConfirmCommand)
            {
                Status = ControllerStatus.DataSending;
                SendGeneralLinkageConfigInfo(); 
            }
            if (GeneralLinkageConfigDownloaded && !ManualControlBoardInfoDownloaded && ReceivedBAConfirmCommand)
            {
                Status = ControllerStatus.DataSending;
                SendManualControlBoardInfo();
            }
            if (AllLoopsDownloaded && StandardLinkageConfigDownloaded && MixedLinkageConfigDownloaded && GeneralLinkageConfigDownloaded && ManualControlBoardInfoDownloaded)
            {
                InitializeCommunicateStatus();
            }
           // Status = ControllerStatus.DataSended;
        }

        #region 重置状态
        /// <summary>
        /// 标准组态数据下传成功后需要设置的状态
        /// </summary>
        protected void SetStatusForStandardLinkageConfigDownloaded()
        {
            if (OperableDataType == OperantDataType.DownloadAll) //如果为全部下载，当前状态仍置为DataSending
            {
                Status = ControllerStatus.DataSending;
            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
            StandardLinkageConfigDownloaded = true;
            SendingCMD = "";
          //  OperableDataType = OperantDataType.None; //如果重置，会丢掉“全部下传”的状态。
            
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            ReceivedBAConfirmCommand = false;
        }
        /// <summary>
        /// 混合组态数据下传成功后需要设置的状态
        /// </summary>
        protected void SetStatusForMixedLinkageConfigDownLoad()
        {
            if (OperableDataType == OperantDataType.DownloadAll) //如果为全部下载，当前状态仍置为DataSending
            {
                Status = ControllerStatus.DataSending;
            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
            MixedLinkageConfigDownloaded = true;
            SendingCMD = "";           
            
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            ReceivedBAConfirmCommand = false;
        }
        /// <summary>
        ///  通用组态数据下传成功后需要设置的状态
        /// </summary>
        protected void SetStatusForGeneralLinkageConfigDownLoad()
        {
            if (OperableDataType == OperantDataType.DownloadAll) //如果为全部下载，当前状态仍置为DataSending
            {
                Status = ControllerStatus.DataSending;
            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
            GeneralLinkageConfigDownloaded = true;
            SendingCMD = "";
            
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            ReceivedBAConfirmCommand = false;
        }
        /// <summary>
        ///  网络手动盘数据下传成功后需要设置的状态
        /// </summary>
        protected void SetStatusForManualControlBoardInfoDownloaded()
        {            
            ManualControlBoardInfoDownloaded = true;
            SendingCMD = "";
            Status = ControllerStatus.DataSended;//在进行全部下载时，网络手动盘为最后一个下传对象，状态设置为DataSended
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            ReceivedBAConfirmCommand = false;
        }

        /// <summary>
        /// 所有回路信息下传成功后需要设置的状态
        /// </summary>
        protected void SetStatusForAllDevicesDownloaded()
        {
            //OperableDataType = OperantDataType.None;
            if (OperableDataType == OperantDataType.DownloadAll) //如果为全部下载，在一个回路的器件下传完成后，当前状态仍置为DataSending
            {
                Status = ControllerStatus.DataSending;                
            }
            else
            {
                Status = ControllerStatus.DataSended;
            }
            ReceivedB9ConfirmCommand = false;
            ReceivedBBConfirmCommand = false;
            ReceivedBCConfirmCommand = false;
            ReceivedBDConfirmCommand = false;
            ReceivedBEConfirmCommand = false;
            ReceivedBFConfirmCommand = false;
            ReceivedBAConfirmCommand = false;
         //   DownloadedDeviceInfoAccumulatedAmountInCurrentLoop = 0; //在调试8007控制器时添加,调8036时未写此代码
            //CurrentLoopForDownloadedDeviceInfo = null;//在调试8007控制器时添加,调8036时未写此代码
            SendingCMD = "";
             
            
        }

        #endregion

        /// <summary>
        /// 接收控制器上传的器件信息
        /// </summary>
        public abstract void ReceiveDeviceInfo();
        /// <summary>
        /// 接收控制器上传的标准组态信息
        /// Revision: 由abstract改为virtual,除8001外其余控制器的标准组态信息都相同
        /// </summary>
        public virtual void ReceiveStandardLinkageInfo()
        {
            byte[] receivedData = CurrentPackage;
            IEqualityComparer<Model.LinkageConfigStandard> c = new StandardLinkageInfoCompare();
            Model.LinkageConfigStandard linkageInfo = ParsePackageCD(receivedData);
            if (!StandardLinkageConfigList.Contains(linkageInfo, c))
            {
                StandardLinkageConfigList.Add(linkageInfo);
                
                UploadedStandardLinkageConfigAccumulatedAmount++;
                UpdateProcessBarStatusForUploadedStandardLinkageConfig();
            }
        }
        #region 数据比较，暂时用于去重, 有时间需要确认重复数据产生的原因2017-04-12   
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
        /// <summary>
        /// 接收控制器上传的混合组态信息
        /// </summary>
        public abstract void ReceiveMixedLinkageInfo();
        /// <summary>
        /// 接收控制器上传的通用组态信息
        /// </summary>
        public abstract void ReceiveGeneralLinkageInfo();

        /// <summary>
        /// 接收手动盘的信息
        /// </summary>
        public abstract void ReceiveManualControlBoardInfo();

        public Model.ControllerModel ControllerModel { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="controllerModel">需要改变ControllerModel中的器件地址长度</param>
        /// <returns></returns>
        public ControllerType GetControllerType(ref ControllerModel controllerModel)
        {
            byte[] package = CurrentPackage;//将CurrentPackage属性增加至控制器类中

            //byte[] checkValue = CheckValue(package, 6, package.Length);// 移至BufferManager中进行校验
              ControllerType controllerType = ControllerType.NONE;
            //if (checkValue[0] == package[3] && checkValue[1] == package[4])//校验正确  // 移至BufferManager中进行校验
            //{
                if (package[8] == 0x36)//觉得放在这里不妥，因为通用的协议不应该涉及具体的信息如“控制器类型”
                {
                    controllerType = ControllerType.NT8036;
                    controllerModel.DeviceAddressLength = 8;
                }
                else if (package[8] == 0x21)
                {
                    controllerType = ControllerType.NT8021;
                    controllerModel.DeviceAddressLength = 8;
                }
                else if (package[8] == 0x7)
                {
                    controllerType = ControllerType.NT8007;
                    controllerModel.DeviceAddressLength = 8;
                    //暂不知道器件编码长度2016-12-29
                }
                else if (package[8] == 0x6)
                {
                    controllerType = ControllerType.NT8001;
                    controllerModel.DeviceAddressLength = 8;
                }
                else if (package[8] == 0x1)
                {
                    controllerType = ControllerType.NT8001;
                    controllerModel.DeviceAddressLength = 7;
                    if (package.Length == 8) //数据包长度为８，代表老版控制器,由于没有版本信息，写为-1
                    {
                        controllerModel.Version = -1;
                    }
                    else//如果有版本信息，写入Version字段
                    {
                        controllerModel.Version = Convert.ToInt16(package[9].ToString(), 16);
                    }

                }
                else if (package[8] != 0x21 && package[8] != 0x36 && package[8] != 0x6)
                {
                   controllerType= GetControllerType(package[8]);
                   controllerModel.DeviceAddressLength = 7; //8003,8000的器件长度为7
                }
           // }
                controllerModel.Type = controllerType;
            return controllerType;
        }

        private  ControllerType GetControllerType(byte b)
        {
            #region 有些值还不知道是什么意思,如2,4
            //Case &H0, &H3, &H2, &H1, &H4, &H36, &H21, &H6: '未通过查询命令，直接收到控制器类型，则认为控制器处于发送模式ywx
            #endregion
            switch (b)
            {
                case 0x0:
                    return ControllerType.FT8000;
                case 0x3:
                    return ControllerType.FT8003;
                case 0x2: //不兼容此控制器
                    return ControllerType.UNCOMPATIBLE;
                case 0x1:
                    return ControllerType.NT8001;
                case 0x4:
                    return ControllerType.UNCOMPATIBLE;
                case 0x36:
                    return ControllerType.NT8036;
                case 0x21:
                    return ControllerType.NT8021;
                case 0x6:
                    return ControllerType.NT8001;
                default:
                    return ControllerType.NONE;
            }
        }
        /// <summary>
        /// 组装66数据
        /// </summary>
        /// <returns></returns>
        public   byte[] AssemblePackage66()
        {
            byte[] sendData = new byte[1];
            sendData[0] = 0x66;
            return sendData;
        }
        public byte[] AssemblePackageB9()
        {
            byte[] sendData = new byte[7];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0xB9;
            sendData[4] = 0xB9;
            sendData[5] = 0x01;
            sendData[6] = 0xB9;
            return sendData;
        }
        /// <summary>
        /// 发送完毕命令
        /// </summary>
        /// <returns></returns>
        public byte[] AssemblePackageBA()
        {            
            byte[] sendData = new byte[7];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0xBA;
            sendData[4] = 0xBA;
            sendData[5] = 0x01;
            sendData[6] = 0xBA;
            return sendData;
        }

        protected virtual byte[] AssemblePackageBC(Model.LinkageConfigStandard standardConfig)
        {
            byte[] sendData = new byte[24];
            sendData[0] = 0xAA;
            sendData[1] = 0x55;
            sendData[2] = 0xDA;
            sendData[3] = 0x00; //异或值校验
            sendData[4] = 0x00;//累加和校验
            //??
            sendData[5] = 0x12;　//固定值
            sendData[6] = 0xBC;　//发送标准组态命令
            //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数            
            sendData[7] = Convert.ToByte(standardConfig.Code);//输出组编号

            //遍历所有的联动模块信息
            int intDeviceInfoIndex = 0;
            foreach (string deviceInfo in standardConfig.GetDeviceNoList)
            {
                if (deviceInfo.Length >= 7)
                {
                    byte[] byteTempValue = SplitDeviceCode(deviceInfo);
                    sendData[8 + intDeviceInfoIndex * 3] = byteTempValue[0];　//控制器号
                    sendData[9 + intDeviceInfoIndex * 3] = byteTempValue[1];　//控制器号
                    sendData[10 + intDeviceInfoIndex * 3] = byteTempValue[2];　//回路号                    
                }
                else
                {
                    sendData[8 + intDeviceInfoIndex * 3] = 0x0;　//控制器号
                    sendData[9 + intDeviceInfoIndex * 3] = 0x0;　//控制器号
                    sendData[10 + intDeviceInfoIndex * 3] = 0x0;　//回路号 
                }
                intDeviceInfoIndex++;
            }
            //动作常数
            sendData[20] = Convert.ToByte(standardConfig.ActionCoefficient);//;(standardConfig.ActionCoefficient == "" || standardConfig.ActionCoefficient == null) ? "0" : standardConfig.ActionCoefficient);

            //       If liandong = "" Then liandong = "0000"
            //dsent2(34) = CInt(liandong) \ 256
            //dsent2(35) = liandong Mod 256

            //输出组1            
            sendData[21] = Convert.ToByte(standardConfig.LinkageNo1);

            //输出组2            
            sendData[22] = Convert.ToByte(standardConfig.LinkageNo2);

            //输出组3            
            sendData[23] = Convert.ToByte(standardConfig.LinkageNo3);


            byte[] checkValue = m_ProtocolDriver.CheckValue(sendData, 6, 24);
            sendData[3] = checkValue[0];
            sendData[4] = checkValue[1];

            return sendData;
        }

        #region 数据解析
        #region 将此方法写为abstract方法
        /// <summary>
        /// 解析收到的标准组态数据包
        /// </summary>
        /// <param name="standardLinkagePackage"></param>
        /// <returns></returns>
        //private Model.LinkageConfigStandard ParsePackageCD(byte[] standardLinkagePackage)
        //{
        //    //第7字节作为基址

        //    //采用GB18030编码
        //    System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
        //    //器件总数



        //    //机号
        //    string tempMachineNo = Convert.ToInt16(standardLinkagePackage[8]).ToString();
        //    switch (this.ControllerModel.DeviceAddressLength)
        //    {
        //        case 7:
        //            tempMachineNo = tempMachineNo.PadLeft(2, '0');
        //            break;
        //        case 8:
        //            tempMachineNo = tempMachineNo.PadLeft(3, '0');
        //            break;
        //        default://默认7位，但此分支不应该走，此时应该知道编码位数
        //            tempMachineNo = tempMachineNo.PadLeft(2, '0');
        //            break;
        //    }

        //    LinkageConfigStandard standardLinkageInfo = new LinkageConfigStandard();
        //    this.ControllerModel.TypeCode = ControllerType;
        //    standardLinkageInfo.Controller = this.ControllerModel;

        //    //组号
        //    standardLinkageInfo.Code = (standardLinkagePackage[7]).ToString().PadLeft(4, '0');
        //    //器件编码
        //    string strMachineNo, strLoopNo, strDeviceNo; //存储机号，路号，器件编码

        //    List<string> lstDeviceInfo = new List<string>();

        //    for (int i = 0; i < 4; i++)
        //    {
        //        if (this.ControllerModel.DeviceAddressLength == 8)
        //        {
        //            strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(3, '0');
        //        }
        //        else
        //        {
        //            strMachineNo = standardLinkagePackage[8 + i * 3].ToString().PadLeft(2, '0');
        //        }

        //        strLoopNo = standardLinkagePackage[9 + i * 3].ToString().PadLeft(2, '0');
        //        strDeviceNo = standardLinkagePackage[10 + i * 3].ToString().PadLeft(3, '0');


        //        if ((strMachineNo + strLoopNo + strDeviceNo) != "0000000" && (strMachineNo + strLoopNo + strDeviceNo) != "00000000" && (strMachineNo + strLoopNo + strDeviceNo).Length == this.ControllerModel.DeviceAddressLength)
        //        {
        //            lstDeviceInfo.Add(strMachineNo + strLoopNo + strDeviceNo);
        //        }
        //        else
        //        {
        //            lstDeviceInfo.Add("");
        //        }
        //    }

        //    standardLinkageInfo.SetDeviceNoList = lstDeviceInfo;
        //    //动作常数
        //    standardLinkageInfo.ActionCoefficient = Convert.ToInt32(standardLinkagePackage[20].ToString().NullToImpossibleValue());
        //    //输出组1~3
        //    string strLinkageGroupCode; //输出组号

        //    strLinkageGroupCode = (standardLinkagePackage[21]).ToString().PadLeft(4, '0');
        //    standardLinkageInfo.LinkageNo1 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

        //    strLinkageGroupCode = (standardLinkagePackage[22]).ToString().PadLeft(4, '0');
        //    standardLinkageInfo.LinkageNo2 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

        //    strLinkageGroupCode = (standardLinkagePackage[23]).ToString().PadLeft(4, '0');
        //    standardLinkageInfo.LinkageNo3 = strLinkageGroupCode == "0000" ? "" : strLinkageGroupCode;

        //    return standardLinkageInfo;
        //}
        #endregion
        public abstract Model.LinkageConfigStandard ParsePackageCD(byte[] standardLinkagePackage);
        #endregion
        public virtual void SendMixedLinkageConfigInfo()
        {
            //此控制器没有“混合组态”，不需要实现
            SetStatusForMixedLinkageConfigDownLoad();
            ReceivedBAConfirmCommand = true; //为通用性考虑，手动为true
        }

        public virtual void SendGeneralLinkageConfigInfo()
        {
            //此控制器没有“通用组态”，不需要实现            
            SetStatusForGeneralLinkageConfigDownLoad();
            ReceivedBAConfirmCommand = true; //为通用性考虑，手动为true
        }

        public virtual void SendManualControlBoardInfo()
        {
            //此控制器没有“网络手控盘”，不需要实现            
            SetStatusForManualControlBoardInfoDownloaded();
            ReceivedBAConfirmCommand = true; //为通用性考虑，手动为true
        }

        ///// <summary>
        ///// 取得器件类型
        ///// </summary>
        ///// <param name="deviceType"></param>
        ///// <returns></returns>
        //public abstract Int16 GetDevType(Int16 deviceType);
        protected byte[] SplitDeviceCode(string strDeviceCode)
        {
            int deviceCodeLength = strDeviceCode.Length;
            string machineCode = strDeviceCode.Substring(0, deviceCodeLength - 5);//路号+器件编号长度为5
            string loopCode = strDeviceCode.Substring(machineCode.Length, 2);//路号为2位
            string deviceCode = strDeviceCode.Substring(machineCode.Length + loopCode.Length, 3);//路号为2位
            byte[] returnValue = new byte[] { Convert.ToByte(machineCode), Convert.ToByte(loopCode), Convert.ToByte(deviceCode) };
            return returnValue;
        }

        public abstract ControllerModel GetControllerUploadedInfo();
        public void UpdateProcessBarStatusForDownloadedDevice()
        {
            UpdateProgressBarEvent(DownloadedDeviceInfoAccumulatedAmountInCurrentLoop, DownloadedDeviceInfoTotalAmountInCurrentLoop,ControllerNodeType.Loop);
            if (DownloadedDeviceInfoAccumulatedAmountInCurrentLoop != 0 && DownloadedDeviceInfoAccumulatedAmountInCurrentLoop == DownloadedDeviceInfoTotalAmountInCurrentLoop)
            {
                DownloadedDeviceInfoAccumulatedAmountInCurrentLoop = 0;
                DownloadedDeviceInfoTotalAmountInCurrentLoop = 0;
            }
        }

        /// <summary>
        /// 器件信息进度条更新
        /// </summary>
        public void UpdateProcessBarStatusForUploadedDevice()
        {
            UpdateProgressBarEvent(UploadedDeviceInfoAccumulatedAmountInCurrentLoop, UploadedDeviceInfoTotalAmountInCurrentLoop,ControllerNodeType.Loop);
            //if (UploadedDeviceInfoAccumulatedAmountInCurrentLoop != 0 && UploadedDeviceInfoAccumulatedAmountInCurrentLoop == UploadedDeviceInfoTotalAmountInCurrentLoop)
            //{
            //    UploadedDeviceInfoAccumulatedAmountInCurrentLoop = 0;
            //    UploadedDeviceInfoTotalAmountInCurrentLoop = 0;
            //}
        }
        public void UpdateProcessBarStatusForUploadedStandardLinkageConfig()
        {
            UpdateProgressBarEvent(UploadedStandardLinkageConfigAccumulatedAmount, UploadedStandardLinkageConfigTotalAmount,ControllerNodeType.Standard);
            if (UploadedStandardLinkageConfigAccumulatedAmount != 0 && UploadedStandardLinkageConfigTotalAmount == UploadedStandardLinkageConfigTotalAmount)
            {
                UploadedStandardLinkageConfigAccumulatedAmount = 0;
                UploadedStandardLinkageConfigTotalAmount = 0;
            }
        }
    }
    public enum ControllerStatus
    {
        None,
        DataSending,//数据发送中
        DataSended,//数据发送完毕
        DataReceiving,//数据接收中
        DataReceived,//数据接收完毕
        Up,//上行方向 控制器-->软件
        Down//下行方向 软件-->控制器
    }
    /// <summary>
    /// 可操作数据类型
    /// </summary>
    public enum OperantDataType
    {
        None,//无任何操作
        Device,//器件 
        StandardLinkageConfig,//标准组态
        MixedLinkageConfig,   //混合组态
        GeneralLinkageConfig, //通用组态        
        MannualControlBoard,  //网络手动盘
        DownloadAll,          //下传全部
        UploadAll             //上传全部
    }

}
