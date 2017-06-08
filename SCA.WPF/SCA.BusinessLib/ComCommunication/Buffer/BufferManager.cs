using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/28 10:11:55
* FileName   : BufferManager
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    public class ProtocolDataBuffer
    {
       // private static readonly log4net.ILog // log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int DataLength { get; set; }
        public int CurrentOffset { get; set; }
        public int InitOffset { get; set; }
        public int NextOffset { get { return InitOffset + DataLength; } }
        public byte[] ReceiveBuffer { get; set; }

        private object _SyncLock = new object();

        private byte[] m_PackageStartChar = new byte[] { 0xAA, 0xCA, 0x66 };

        private int m_DevicePackageLength=43; //器件设置一包数据长度 8036为43

        private List<byte[]> m_lstPackage;
        /// <summary>
        /// 存储从控制器上接收到的数据
        /// </summary>
        private List<byte[]> m_lstReceivedDataFromController;

        private IProtocolDriver m_ProtocolDriver;

        public int Capacity { get; set; }
        public ProtocolDataBuffer(IProtocolDriver protocolDriver)
        {
            CurrentOffset = 0;
            InitOffset = 0;
            DataLength = 0;
            Capacity = 1024;
            ReceiveBuffer = new byte[Capacity];
            m_lstPackage = new List<byte[]>();
            m_ProtocolDriver = protocolDriver;

            m_lstReceivedDataFromController = new List<byte[]>();
        }

        public void Reset()
        {
            CurrentOffset = InitOffset;
            DataLength = 0;
        }

        public void Add(byte[] data)
        {
                    
            lock(_SyncLock)
            {
                int remainLength = Capacity - (DataLength + InitOffset);
                if (data.Length > remainLength)
                {
                    Buffer.BlockCopy(data, 0, ReceiveBuffer, NextOffset, remainLength); //可能丢失数据
                    DataLength += remainLength;
                }
                else
                {
                    Buffer.BlockCopy(data, 0, ReceiveBuffer, NextOffset, data.Length);
                    DataLength += data.Length;
                }
             //   log.Info("Add:" + m_ProtocolDriver.ByteToHex(data) + "length:" + DataLength.ToString());
                OrganizeDataPackage();
            }

        }
        public byte[] Pop()
        {
            byte[] package;
            if(m_lstPackage.Count>0)
            { 
                package= m_lstPackage[0];
                m_lstPackage.RemoveRange(0,m_lstPackage.Count);
                return package;
            }
            return null;
        }

        /// <summary>
        /// 将从控制器上传至软件的数据取出
        /// </summary>
        /// <returns></returns>
        public byte[] PopUploadedMessage()
        {
            byte[] package;
            if (m_lstReceivedDataFromController.Count > 0)
            {
                package = m_lstReceivedDataFromController[0];
                //接收到的有效数据都要处理，不需要清除（有效的概念为：校验正确）
                m_lstReceivedDataFromController.RemoveAt (0); 
                return package;
            }
            return null;
        }
        /// <summary>
        /// 清空已接到的数据
        /// </summary>
        public void ClearUploadedMessage()
        {
            m_lstReceivedDataFromController.RemoveRange(0, m_lstReceivedDataFromController.Count);
        }

        private  void OrganizeDataPackage()
        {
            byte[] packageData;
            //foreach (byte b in ReceiveBuffer)
            //{
                
            //    if (b == 0x66)
            //    {
            //        packageData = new byte[] { b };
            //        m_lstPackage.Add(packageData );  
            //    }
            //    else if(b==0x33)
            //    {
            //        packageData = new byte[] { b };
            //        m_lstPackage.Add(packageData );
            //    }
            //    else if (b == 0xAA)
            //    { 
                
            //    }
            //}
            int startIndex=0;
            for (int i = 0; i < DataLength ; i++)
            {
             //   log.Info("DataLength:" + DataLength.ToString());
                byte b = ReceiveBuffer[startIndex];
                if (b == 0x66)
                {

                    packageData = new byte[] { b };
                    m_lstPackage.Add(packageData);
                  //  // log.Info("Orgnization 66:" + m_ProtocolDriver.ByteToHex(packageData) + "length:" + DataLength.ToString());
                    Buffer.BlockCopy(ReceiveBuffer, startIndex + 1, ReceiveBuffer, 0, ReceiveBuffer.Length -1); //更新Buffer中的数据
                    DataLength--;                    
                }
                else if (b == 0x33)
                {
                    packageData = new byte[] { b };
                    m_lstPackage.Add(packageData);
                    // log.Info("Orgnization 33:" + m_ProtocolDriver.ByteToHex(packageData) + "length:" + DataLength.ToString());
                    Buffer.BlockCopy(ReceiveBuffer, startIndex + 1, ReceiveBuffer, 0, ReceiveBuffer.Length - 1); //更新Buffer中的数据
                    DataLength--;
                }
                else if (b == 0xAA && ReceiveBuffer[startIndex+1]==0x55) //有可能出现溢出风险
                {
                    //                  int remainLength = Capacity - (DataLength + InitOffset);

                    //验证数据包长度，取第６位命令字段
                    if (DataLength > 6)
                    {
                        byte cmdByte = ReceiveBuffer[6];
                        byte packageLength = ReceiveBuffer[5];
                        ////将十六进制“10”转换为十进制i
                        //int i = Convert.ToInt32("10", 16);
                        ////将十进制i转换为十六进制s
                        //string type = string.Format("{0:X}", i);
                        

                        int packageLen = Convert.ToInt32(packageLength.ToString(), 10);

                        packageLen = packageLen + 6; //需要加上头部未记入ReceiveBuffer[5]中的6个字节
                        string str = "包0-6：";
                        for (int o = 0; o < 12; o++)
                        {
                            str += ReceiveBuffer[o].ToString("X2")+"   ";
                        }
                        // log.Info( str);
                        // log.Info("Prepare proceed..." + DataLength.ToString()+"命令字节："+cmdByte.ToString("X2"));

                        if (cmdByte == 0xC8)
                        {

                            if (DataLength >= packageLen)//完整的C8命令为10个字节,或许有８个字节
                            {
                                // log.Info("Prepare C8 proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_c8 = 0; i_c8 < packageLen; i_c8++)　//从０开始取此包数据长度的字节数，作为C8命令
                                {
                                    byte temp = ReceiveBuffer[i_c8];
                                    if ((temp == 0xAA) && (i_c8 != 0)) //如果在非第一个元素处为0XAA，则此数据存在问题
                                    {
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_c8, ReceiveBuffer, 0, ReceiveBuffer.Length - i_c8 - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_c8;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_c8] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        m_lstPackage.Add(tempByte);
                                    }
                                    // log.Info("Orgnization C8:" + m_ProtocolDriver.ByteToHex(tempByte) + "length:" + DataLength.ToString());
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - 10;
                                }
                            }
                        }
                        else if (cmdByte == 0xCA)
                        {
                            const int PACKAGELENGTH = 7;
                            // log.Info("Prepare CA proceed...");
                            if (DataLength >= PACKAGELENGTH)//完整的CA命令为6个字节
                            {
                                byte[] tempByte = new byte[PACKAGELENGTH];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_ca = 0; i_ca < PACKAGELENGTH; i_ca++)　//从０开始取7个字节的数据，作为CA命令
                                {
                                    byte temp = ReceiveBuffer[i_ca];
                                    if ((temp == 0xAA) && (i_ca != 0)) //如果在非第一个元素处为0XAA，则此数据存在问题
                                    {
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_ca, ReceiveBuffer, 0, ReceiveBuffer.Length - PACKAGELENGTH - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_ca;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_ca] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        m_lstPackage.Add(tempByte);    //加入待处理命令中
                                    }
                                    // log.Info("Orgnization CA:" + m_ProtocolDriver.ByteToHex(tempByte) + "length:" + DataLength.ToString());
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + PACKAGELENGTH, ReceiveBuffer, 0, ReceiveBuffer.Length - PACKAGELENGTH - 1); //更新Buffer中的数据
                                    DataLength = DataLength - PACKAGELENGTH;
                                }
                            }
                        }
                        else if (cmdByte == 0xC9)
                        {

                            // log.Info("Prepare C9 proceed...");
                            if (DataLength >= packageLen)
                            {
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_c9 = 0; i_c9 < packageLen; i_c9++)
                                {
                                    byte temp = ReceiveBuffer[i_c9];
                                    if ((temp == 0xAA) && (i_c9 != 0)) //如果在非第一个元素处为0XAA，则此数据存在问题
                                    {
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_c9, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_c9;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_c9] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        m_lstPackage.Add(tempByte);    //加入待处理命令中
                                    }
                                    // log.Info("Orgnization C9:" + m_ProtocolDriver.ByteToHex(tempByte) + "length:" + DataLength.ToString());
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
                        else if (cmdByte == 0xCC)
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare CC proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_cc = 0; i_cc < packageLen; i_cc++)　//从０开始取此包数据长度的字节数，作为C8命令
                                {
                                    byte temp = ReceiveBuffer[i_cc];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_cc + 1] == 0x55) && (i_cc != 0)) //如果在非第一个元素处为0XAA，则此数据存在问题
                                    {
                                        // log.Info("Orgnization CC_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer ));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_cc, ReceiveBuffer, 0, ReceiveBuffer.Length - i_cc - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_cc;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_cc] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        IEqualityComparer<byte[]> c = new CCompare();
                                        if (!m_lstReceivedDataFromController.Contains(tempByte,c))
                                        {
                                            m_lstReceivedDataFromController.Add(tempByte);    
                                        }                                        
                                    }                                    
                                    // log.Info("Orgnization CC_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen ;
                                }
                            }
                        }
                        else if (cmdByte == 0xCD)
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare CD proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_cd = 0; i_cd < packageLen; i_cd++)　//从０开始取此包数据长度的字节数，作为CD命令
                                {
                                    byte temp = ReceiveBuffer[i_cd];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_cd + 1] == 0x55) && (i_cd != 0)) //如果在非第一个元素处为0XAA,0x55起始符，则此数据存在问题
                                    {
                                        // log.Info("Orgnization CD_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_cd, ReceiveBuffer, 0, ReceiveBuffer.Length - i_cd - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_cd;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_cd] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        IEqualityComparer<byte[]> c = new CCompare();
                                        if (!m_lstReceivedDataFromController.Contains(tempByte, c))
                                        {
                                            m_lstReceivedDataFromController.Add(tempByte);
                                        }
                                    }
                                    // log.Info("Orgnization CD_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
                        else if (cmdByte == 0xBD)//混合组态
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare BD proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_bd = 0; i_bd < packageLen; i_bd++)　//从０开始取此包数据长度的字节数，作为BD命令
                                {
                                    byte temp = ReceiveBuffer[i_bd];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_bd + 1] == 0x55) && (i_bd != 0)) //如果在非第一个元素处为0XAA,0x55起始符，则此数据存在问题
                                    {
                                        // log.Info("Orgnization BD_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_bd, ReceiveBuffer, 0, ReceiveBuffer.Length - i_bd - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_bd;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_bd] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        //？？考虑是否需要增加去重处理？？

                                        m_lstReceivedDataFromController.Add(tempByte);                                    
                                    }
                                    // log.Info("Orgnization BD_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
                        else if (cmdByte == 0xBE)//通用组态
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare BE proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_be = 0; i_be < packageLen; i_be++)　//从０开始取此包数据长度的字节数，作为BE命令
                                {
                                    byte temp = ReceiveBuffer[i_be];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_be + 1] == 0x55) && (i_be != 0)) //如果在非第一个元素处为0XAA,0x55起始符，则此数据存在问题
                                    {
                                        // log.Info("Orgnization BE_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_be, ReceiveBuffer, 0, ReceiveBuffer.Length - i_be - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_be;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_be] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        //？？考虑是否需要增加去重处理？？

                                        m_lstReceivedDataFromController.Add(tempByte);
                                    }
                                    // log.Info("Orgnization BE_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
       
                        else if (cmdByte == 0xBF) //网络手控盘
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare BF proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_bf = 0; i_bf < packageLen; i_bf++)　//从０开始取此包数据长度的字节数，作为BE命令
                                {
                                    byte temp = ReceiveBuffer[i_bf];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_bf + 1] == 0x55) && (i_bf != 0)) //如果在非第一个元素处为0XAA,0x55起始符，则此数据存在问题
                                    {
                                        // log.Info("Orgnization BF_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_bf, ReceiveBuffer, 0, ReceiveBuffer.Length - i_bf - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_bf;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_bf] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        //？？考虑是否需要增加去重处理？？

                                        m_lstReceivedDataFromController.Add(tempByte);
                                    }
                                    // log.Info("Orgnization BF_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
                        else if (cmdByte == 0xCF)
                        {
                            if (DataLength >= packageLen)
                            {
                                // log.Info("Prepare CF proceed...");
                                byte[] tempByte = new byte[packageLen];
                                bool blnWholeFlag = true;//整包数据标识
                                for (int i_cf = 0; i_cf < packageLen; i_cf++)　//从０开始取此包数据长度的字节数，作为BE命令
                                {
                                    byte temp = ReceiveBuffer[i_cf];
                                    if ((temp == 0xAA) && (ReceiveBuffer[i_cf + 1] == 0x55) && (i_cf != 0)) //如果在非第一个元素处为0XAA,0x55起始符，则此数据存在问题
                                    {
                                        // log.Info("Orgnization CF_ErrorData:" + m_ProtocolDriver.ByteToHex(ReceiveBuffer));
                                        Buffer.BlockCopy(ReceiveBuffer, startIndex + i_cf, ReceiveBuffer, 0, ReceiveBuffer.Length - i_cf - 1); //更新Buffer中的数据
                                        DataLength = DataLength - i_cf;
                                        blnWholeFlag = false;
                                        break; //退出循环
                                    }
                                    tempByte[i_cf] = temp;
                                }

                                if (blnWholeFlag)
                                {
                                    byte[] checkValue = m_ProtocolDriver.CheckValue(tempByte, 6, tempByte.Length); //计算校验数据,6为协议中指定的开始校验的起始位，可考虑将6封装在实现协议中
                                    if ((checkValue[0] == tempByte[3]) && (checkValue[1] == tempByte[4]))  //符合校验数据结果，判定数据合法
                                    {
                                        //？？考虑是否需要增加去重处理？？

                                        m_lstReceivedDataFromController.Add(tempByte);
                                    }
                                    // log.Info("Orgnization CF_new:" + m_ProtocolDriver.ByteToHex(tempByte));
                                    Buffer.BlockCopy(ReceiveBuffer, startIndex + packageLen, ReceiveBuffer, 0, ReceiveBuffer.Length - packageLen - 1); //更新Buffer中的数据
                                    DataLength = DataLength - packageLen;
                                }
                            }
                        }
                        else//非命令字节，删除首位
                        {
                            Buffer.BlockCopy(ReceiveBuffer, startIndex + 1, ReceiveBuffer, 0, ReceiveBuffer.Length - 1); //更新Buffer中的数据
                            DataLength--;                         
                        }
                    }
                }
                else //第一个字节不对，弃掉
                {
                    Buffer.BlockCopy(ReceiveBuffer, startIndex + 1, ReceiveBuffer, 0, ReceiveBuffer.Length - 1); //更新Buffer中的数据
                    DataLength--;    
                }
            }
            
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        /// <returns></returns>
        public byte[] Get()
        {
            if (DataLength <= 0)
            {
                return new byte[] { };
            }

            lock (_SyncLock)
            {
                byte[] data = new byte[DataLength];
                Buffer.BlockCopy(ReceiveBuffer, InitOffset, data, 0, data.Length);
                DataLength = 0;
                CurrentOffset = InitOffset;
                return data;
            }
        }
        
    }
    class CCompare : IEqualityComparer<byte[]>
    {

        public bool Equals(byte[] x, byte[] y)
        {
            if (x.Length == y.Length)
            {
                if (x[6] == y[6] && x[3] == y[3] && x[4] == y[4] )
                {
                    return true ;
                }
                else
                {
                    return false ;
                }
            }
            else
            {
                return false ;
            }
        }

        public int GetHashCode(byte[] obj)
        {
            throw new NotImplementedException();
        }
    }
    class CCCompare:IComparer<byte[]>
    {


        public int Compare(byte[] x, byte[] y)
        {
            if (x.Length == y.Length)
            {
                if (x[6] == y[6] && x[3] == y[3] && x[4] == y[4])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
