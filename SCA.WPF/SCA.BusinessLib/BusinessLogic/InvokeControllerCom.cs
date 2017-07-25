using System;
using System.Collections.Concurrent;
using SCA.BusinessLib.Controller;
using SCA.Model;
using SCA.BusinessLib.ComCommunication;
using SCA.BusinessLib.Utility;
using log4net;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/6 13:45:01
* FileName   : InvokeControllerCom
* Description: 控制器通信操作类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{   
    /// <summary>
    /// 控制器的信息全部上传完成
    /// </summary>
    /// <param name="controller"></param>
 
    public delegate void  AllDataUploadedEventHandler(ControllerModel controller);
    
    /// <summary>
    /// 与控制器进行通讯
    /// </summary>
    public class InvokeControllerCom
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        ConcurrentDictionary<ControllerType, ControllerTypeBase> dictControllerCom;
        ProtocolDataBuffer m_BufferManager;
        IProtocolDriver m_protocolDriver;
        SerialComManager m_SerialComManager;
        private static InvokeControllerCom instance;
        private static object syncRoot = new object();
        public  event  AllDataUploadedEventHandler AllDataUploadedEvent;
        
        /// <summary>
        /// i
        /// </summary>
        public ControllerModel TheController { get; set; }
        public  Controller.ControllerTypeBase TheControllerType{get;set;}

        ControllerTypeBase _controllerType;//用于获取控制器类，作为out参数传递，然后赋值给属性TheControllerType

        private InvokeControllerCom()
        {
            m_protocolDriver = new ProtocolDriver(); //指定协议处理类
            m_BufferManager = new ProtocolDataBuffer(m_protocolDriver);
            m_SerialComManager = new SerialComManager();
            dictControllerCom = new ConcurrentDictionary<ControllerType, ControllerTypeBase>();
            dictControllerCom.TryAdd(ControllerType.NONE, new ControllerTypeUnknown(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.NT8036, new ControllerType8036(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.NT8001, new ControllerType8001(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.NT8007, new ControllerType8007(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.FT8003, new ControllerType8003(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.FT8000, new ControllerType8000(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.NT8021, new ControllerType8021(m_SerialComManager, m_protocolDriver));
            dictControllerCom.TryAdd(ControllerType.NT8053, new ControllerType8053(m_SerialComManager, m_protocolDriver));
        }
        /// <summary>
        /// 获取当前实例
        /// </summary>
        public static InvokeControllerCom Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new InvokeControllerCom();
                        }
                    }
                }
                return instance;
            }
        }
        public bool GetPortStatus()
        {
            return m_SerialComManager.GetPortStatus();
        }
        /// <summary>
        /// 通讯开始,处理控制器消息
        /// </summary>
        public void StartCommunication()
        {
            m_SerialComManager.BaudRate = TheController.BaudRate;
            m_SerialComManager.PortName = TheController.PortName;
            m_SerialComManager.OpenPort();
            m_SerialComManager.ReceivedData += DataProcess;
        }
        /// <summary>
        /// 通讯结束
        /// </summary>
        public void StopCommunication()
        {
            m_SerialComManager.ClosePort();
            #region 控制器通信状态初始化
            if (_controllerType!=null)
            { 
                _controllerType.InitializeCommunicateStatus();
            }
            #endregion
        }
        /// <summary>
        /// 取得控制器通信处理类
        /// </summary>
        private  void SetControllerType()
        {
            ControllerType enumCurrentControllerType = ControllerType.NONE;
            if (TheControllerType != null)
            {
                enumCurrentControllerType = TheControllerType.ControllerType;
            }
            switch (enumCurrentControllerType)  //在这可能有问题 ，当时在连接8001时，此处没有8001的代码，但8001仍然通信成功并返回数据了，需要验证此处
            {
                case ControllerType.NONE:

//                    _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NONE, out _controllerType);
                    //_controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;                    

                    break;
                case ControllerType.NT8036:
                  //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NT8036, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.NT8001:
                  //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NT8001, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.NT8007:
                    //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NT8007, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.FT8000:
                    //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.FT8000, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.FT8003:
                    //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.FT8003, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.NT8021:
                    //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NT8021, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                case ControllerType.NT8053:
                    //  _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NT8053, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
                default:
                   // _controllerType = TheControllerType;
                    dictControllerCom.TryGetValue(ControllerType.NONE, out _controllerType);
                    _controllerType.ControllerModel = TheControllerType.ControllerModel;
                    TheControllerType = _controllerType;
                    break;
            }
            //TheControllerType在读取到控制器发来的“类型”信息后，会改变自己的控制器类型状态。[如Unknown会将自己的ContrllerType置]
            //由于经过Switch之后，TheControllerType会改变，
               TheControllerType.ControllerType = enumCurrentControllerType;
            
            
        }
        
        private void DataProcess(byte[] receivedData)
        {
            //思路：数据的接收与发送主体是控制器，应将收到的数据放在控制器类中进行处理
            //1.初始化未知控制器处理类
            //2.将协议集成进控制器处理类中，用控制器处理类中的协议解析收到的数据            
            m_BufferManager.Add(receivedData);
            byte[] package = m_BufferManager.Pop();
            ProtocolDriver driver = new ProtocolDriver();
            log.Info("Poped" + driver.ByteToHex(package));

            if (package == null)
            {
                package = m_BufferManager.PopUploadedMessage();
            }

            if (package == null)
            {
                return;
            }

            ProcessCommand procCMD = new ProcessCommand();
            
            byte[] charCmd = new byte[] { m_protocolDriver.GetCommand(package) };

            SetControllerType();

            ICommand cmd=null;

            switch (m_protocolDriver.ByteToHex(charCmd).Trim())
            {
                case "CA"://巡检命令
                    cmd = new CommandCA(TheControllerType);
                    break;
                case "66"://确认命令
                    cmd = new Command66(TheControllerType);
                    break;
                case "C8"://获取控制器类型
                    TheControllerType.CurrentPackage = package;
                    cmd = new CommandC8(TheControllerType);
                    SetControllerType();
                    break;
                case "C9"://准备上传数据
                    TheControllerType.CurrentPackage = package;
                    cmd = new CommandC9(TheControllerType);
                    break;
                case "CC"://上传器件信息
                    if (TheControllerType.ControllerType != ControllerType.NONE)
                    { 
                        TheControllerType.CurrentPackage = package;
                        TheControllerType.ReceiveDeviceInfo();
                        cmd = new CommandCC(TheControllerType);
                    }
                    break;
                case "CD"://标准组态信息上传
                    if (TheControllerType.ControllerType != ControllerType.NONE)
                    {
                        TheControllerType.CurrentPackage = package;
                        TheControllerType.ReceiveStandardLinkageInfo();
                        cmd = new CommandCD(TheControllerType);
                    }
                    break;
                case "BD"://上传混合组态
                    if (TheControllerType.ControllerType != ControllerType.NONE)
                    {
                        TheControllerType.CurrentPackage = package;
                        TheControllerType.ReceiveMixedLinkageInfo();
                        cmd = new CommandBD(TheControllerType);
                    }
                    break;
                case "BE"://上传通用组态
                    TheControllerType.CurrentPackage = package;
                    TheControllerType.ReceiveGeneralLinkageInfo();
                    cmd = new CommandBE(TheControllerType);
                    break;
                case "BF"://上传网络手动盘信息
                    if (TheControllerType.ControllerType != ControllerType.NONE)
                    {
                        TheControllerType.CurrentPackage = package;
                        TheControllerType.ReceiveManualControlBoardInfo();
                        cmd = new CommandBF(TheControllerType);
                    }
                    break;
                case "CF"://所有信息上传结束
                    if (TheControllerType.ControllerType != ControllerType.NONE)
                    {
                        TheControllerType.CurrentPackage = package;
                        cmd = new CommandCF(TheControllerType);
                        //需要增加各控制器类型的判断
                        ControllerType enumCurrentControllerType = ControllerType.NONE;
                        if (TheControllerType != null)
                        {
                            enumCurrentControllerType = TheControllerType.ControllerType;
                        }
                        switch (enumCurrentControllerType)
                        { 
                            case ControllerType.NT8001: //未测试
                                {
                                    ((ControllerType8001)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                    }
                                }
                                break;
                            case ControllerType.NT8036://未测试
                                {
                                    ((ControllerType8036)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                    }
                                }                                
                                break;
                            case ControllerType.NT8007:
                                { 
                                    ((ControllerType8007)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                        ((ControllerType8007)TheControllerType).UploadedDeviceInfoAccumulatedAmountInCurrentLoop = 0;
                                        ((ControllerType8007)TheControllerType).UploadedDeviceInfoTotalAmountInCurrentLoop = 0;
                                        ((ControllerType8007)TheControllerType).UploadedStandardLinkageConfigAccumulatedAmount = 0;
                                        ((ControllerType8007)TheControllerType).UploadedStandardLinkageConfigTotalAmount = 0;
                                    }
                                }
                                break;
                            case ControllerType.FT8003:
                                { 
                                    ((ControllerType8003)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                        ((ControllerType8003)TheControllerType).UploadedDeviceInfoAccumulatedAmountInCurrentLoop = 0;
                                        ((ControllerType8003)TheControllerType).UploadedDeviceInfoTotalAmountInCurrentLoop = 0;
                                        ((ControllerType8003)TheControllerType).UploadedStandardLinkageConfigAccumulatedAmount = 0;
                                        ((ControllerType8003)TheControllerType).UploadedStandardLinkageConfigTotalAmount = 0;
                                    }
                                }
                                break;
                            case ControllerType.FT8000: 
                                {
                                    ((ControllerType8000)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                    }
                                }
                                break;
                            case ControllerType.NT8021: //未测试
                                {
                                    ((ControllerType8021)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                    }
                                }
                                break;
                            case ControllerType.NT8053:
                                {
                                    ((ControllerType8053)TheControllerType).GetControllerUploadedInfo();
                                    if (AllDataUploadedEvent != null)
                                    {
                                        AllDataUploadedEvent(TheControllerType.ControllerModel);
                                    }
                                }
                                break;
                        }
                        //((ControllerType8007)TheControllerType).GetControllerUploadedInfo();
                        //if (AllDataUploadedEvent != null)
                        //{
                        //    AllDataUploadedEvent(TheControllerType.ControllerModel);
                        //}
                        
                    }
                    break;
                default:
                    cmd = null;
                    break;
            }
            if (cmd != null)
            {
                procCMD.Run(cmd);
            }
        }
    }
}
