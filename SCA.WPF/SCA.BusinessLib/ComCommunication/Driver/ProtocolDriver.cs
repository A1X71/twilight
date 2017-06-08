using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2016/12/5 15:45:26
* FileName   : ProtocolDriver
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.ComCommunication
{
    public class ProtocolDriver:IProtocolDriver
    {
        
        public  byte GetCommand(byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                //传进来的数据为合法的包数据。根据协议“命令字节”为第7位或第1位
                return bytes.Length > 5 ? bytes[6] : bytes[0];
            }
            return new byte();
        }
        
        
        /// <summary>
        /// 对数据进行异或值和累加和计算
        /// </summary>        
        /// <param name="byteValues"></param>
        /// <param name="offset">计算的起始位置</param>
        /// <param name="count">从offset开始需计算的字节数量</param>
        /// <returns>计算后的异或值和累加和</returns>
        public   byte[] CheckValue(byte[] byteValues,int offset,int count)
        {
            byte[] result = new byte[2];
            int intSumCheck = 0;
            for (int j = offset; j < count ; j++)
            {
                result[0] = Convert.ToByte(result[0] ^ byteValues[j]);
                intSumCheck = intSumCheck + byteValues[j];
            }
            result[1] = Convert.ToByte(intSumCheck % 256);
            return result;
        }
         
        public   string ByteToHex(byte[] comByte)
        {
            if (comByte == null)
            {
                return "";
            }
            //create a new StringBuilder object
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            //return the converted value
            return builder.ToString().ToUpper();
        }

        #region 注释无用
        //private static Int16 GetDevType(Int16 deviceType)
        //{
        //    switch (ConvertSwitchCondition(deviceType))
        //    {
        //        case 0: return 0;
        //        case -1: return deviceType; //1-10
        //        case -2: return 12; //11-12
        //        case 13: return 13;
        //        case 14: return 17;
        //        case 15: return 15;
        //        case -3: return 4;  //16 - 17
        //        case -4: return 2;  //18 - 22
        //        case 23: return 18;
        //        case -5: return 4; //24 - 30
        //        case 31: return 23;
        //        case 32: return 24;
        //        case 33: return 21;
        //        case 34: return 22;
        //        case 35: return 24;
        //        case 36: return 29;
        //        case -6: return 24; //37 - 65
        //        case 66: return 29;
        //        case 67: return 20;
        //        case 68: return 30;
        //        case -7: return 31; //69-70               
        //        case -8: return 24; //71 - 73
        //        case 74: return 28;
        //        case 75: return 25;
        //        case 76: return 26;
        //        case 77: return 27;
        //        case 78: return 28;
        //        case 79: return 18;
        //        case 80: return 19;
        //        case 81: return 14;
        //        case -9: return 16; //82 - 86
        //        case -10: return 11; //87 - 89
        //        case -11: return 23;//101 - 129
        //        default: return 16;

        //    }
        //}
        //private static Int16 GetDevTypeFor8001(Int16 deviceType, Int16 featureValue)
        //{
        //    switch (ConvertSwitchCondition(deviceType))
        //    {
        //        case 0: return 0;
        //        case -1: return deviceType; //1-10
        //        case -2: return 12; //11-12
        //        case 13: return 13;
        //        case 14: return 17;
        //        case 15: return 15;
        //        case -3: return 4;  //16 - 17
        //        case -4: return 2;  //18 - 22
        //        case 23: return 18;
        //        case 24: return 27;
        //        case 25: return 6;
        //        case 26: return 7;
        //        case 27: return 4;
        //        case 28: return 4;
        //        case 29: return 4;
        //        case 30: return 4;

        //        case 31: return 23;
        //        case 32: return 24;
        //        case 33: return 21;
        //        case 34: return 22;
        //        case 35: return 24;
        //        case 36: return 29;

        //        case -6:  //37 - 65
        //            if (featureValue == 0)
        //            {
        //                return 24;
        //            }
        //            else if (featureValue == 1)
        //            {
        //                return 23;
        //            }
        //            else if (featureValue == 2)
        //            {
        //                return 31;
        //            }
        //            else if (featureValue == 3)
        //            {
        //                return 30;
        //            }
        //            return 0;//此处应加异常日志，正常状态不应为0

        //        case 66: return 29;
        //        case 67: return 20;
        //        case 68: return 30;
        //        case -7: return 31; //69-70               
        //        case -8: return 24; //71 - 73
        //        case 74: return 28;
        //        case 75: return 25;
        //        case 76: return 26;
        //        case 77: return 27;
        //        case 78: return 28;
        //        case 79: return 18;
        //        case 80: return 19;
        //        case 81: return 14;
        //        case -9: return 16; //82 - 86
        //        case -10: return 11; //87 - 89
        //        case -11: return 23;//101 - 129
        //        default: return 16;
        //    }
        //}
        ///// <summary>
        ///// 将一定范围内的值转换为一个值，方便Switch case判断
        ///// </summary>
        ///// <param name="originalValue"></param>
        ///// <returns></returns>
        //private static Int16 ConvertSwitchCondition(Int16 originalValue)
        //{
        //    if (originalValue >= 1 && originalValue <= 10)
        //        return -1;
        //    else if (originalValue >= 11 && originalValue <= 12)
        //        return -2;
        //    else if (originalValue >= 16 && originalValue <= 17)
        //        return -3;
        //    else if (originalValue >= 18 && originalValue <= 22)
        //        return -4;
        //    else if (originalValue >= 24 && originalValue <= 30)
        //        return -5;
        //    else if (originalValue >= 37 && originalValue <= 65)
        //        return -6;
        //    else if (originalValue >= 69 && originalValue <= 70)
        //        return -7;
        //    else if (originalValue >= 71 && originalValue <= 73)
        //        return -8;
        //    else if (originalValue >= 82 && originalValue <= 86)
        //        return -9;
        //    else if (originalValue >= 87 && originalValue <= 89)
        //        return -10;
        //    else if (originalValue >= 101 && originalValue <= 129)
        //        return -11;
        //    else
        //        return originalValue;
        //}
        //public static byte[] GetPackage66()
        //{
        //    byte[] sendData = new byte[1];
        //    sendData[0] = 0x66;
        //    return sendData;
        //}
        //public static byte[] GetPackageB9()
        //{
        //    byte[] sendData = new byte[7];
        //    sendData[0] = 0xAA;
        //    sendData[1] = 0x55;
        //    sendData[2] = 0xDA;
        //    sendData[3] = 0xB9;
        //    sendData[4] = 0xB9;
        //    sendData[5] = 0x01;
        //    sendData[6] = 0xB9;
        //    return sendData;
        //}
        //public static List<byte[]> GetPackageBB(List<Model.DeviceInfo8036> deviceInfo)
        //{
        //    List<byte[]> lstSendData = new List<byte[]>();
        //    foreach (Model.DeviceInfo8036 singleDevInfo in deviceInfo)
        //    {
        //        byte[] sendData = new byte[43];
        //        sendData[0] = 0xAA;
        //        sendData[1] = 0x55;
        //        sendData[2] = 0xDA;
        //        sendData[3] = 0x00; //异或值校验
        //        sendData[4] = 0x00;//累加和校验
        //        //??
        //        sendData[5] = 0x25;　//数据长度 ?? 暂定为01 ??
        //        sendData[6] = 0xBB;　//发送器件命令
        //        //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
        //        sendData[7] = Convert.ToByte(0x37);　//器件总数
        //        sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
        //        sendData[9] = Convert.ToByte(singleDevInfo.Loop.Code);　//回路号

        //        sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Controller.MachineNumber.Length + singleDevInfo.Loop.Code.Length, 3));　//地编号
        //        sendData[11] = Convert.ToByte(GetDevType(singleDevInfo.TypeCode) * 8 + singleDevInfo.Disable * 4);//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”
        //        //GetDevType(CInt(leixing)) * 8 + geli * 4
        //        sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
        //        //           dsent(12) = Left(Trim(.Text), 3)    '  设备类型
        //        if (sendData[12] > 100)
        //        {
        //            sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
        //        }

        //        sendData[13] = Convert.ToByte(singleDevInfo.LinkageGroup1); //输出组1
        //        sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup2); //输出组2
        //        sendData[15] = Convert.ToByte(singleDevInfo.DelayValue); //延时
        //        sendData[16] = 0x00; //防火分区，固定值00

        //        //17~33为安装地点
        //        //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
        //        //Convert.ToBase64String();     
        //        int startIndex = 17;
        //        char[] charArrayLocation = singleDevInfo.Location.ToArray();
        //        //采用Base64编码传递数据
        //        System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
        //        for (int j = 0; j < charArrayLocation.Length; j++)
        //        {
        //            //sendData[startIndex]=Convert.ToByte(Convert.ToBase64String(System.Text.Encoding.GetEncoding(54936).GetBytes(charArrayLocation[j].ToString())));

        //            Byte[] encodedBytes = ascii.GetBytes(charArrayLocation[j].ToString());
        //            if (encodedBytes.Length == 1)
        //            {
        //                sendData[startIndex] = encodedBytes[0];
        //                startIndex++;
        //            }
        //            else
        //            {
        //                sendData[startIndex] = encodedBytes[0];
        //                startIndex++;
        //                sendData[startIndex] = encodedBytes[1];
        //                startIndex++;
        //            }


        //        }
        //        //补足位数
        //        for (int j = startIndex; j < 34; j++)
        //        {
        //            sendData[j] = 0x00;
        //        }
        //        #region　地址转换参考代码
        //        //  sendData[17]=0x;

        //        //       n = 17

        //        //For j = 1 To lenc

        //        //    ascchina = Asc(Mid(sstrchina, j, 1))

        //        //    If ascchina >= 0 And ascchina < 128 Then

        //        //      dsent(n) = ascchina

        //        //    Else

        //        //      getasc (Mid(sstrchina, j, 1))

        //        //      dsent(n) = m4

        //        //      n = n + 1

        //        //      dsent(n) = m1

        //        //  End If

        //        //  n = n + 1

        //        //Next j


        //        //For nn = n To 33

        //        //  dsent(nn) = 0

        //        //Next nn
        //        #endregion
        //        sendData[34] = 0x00;//固定值00
        //        //楼号
        //        if (singleDevInfo.BuildingNo == null)
        //        {
        //            sendData[35] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[35] = Convert.ToByte(singleDevInfo.BuildingNo);
        //        }
        //        //区号
        //        if (singleDevInfo.ZoneNo == null)
        //        {
        //            sendData[36] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[36] = Convert.ToByte(singleDevInfo.ZoneNo);
        //        }
        //        //层号
        //        if (singleDevInfo.FloorNo == null)
        //        {
        //            sendData[37] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[37] = Convert.ToByte(singleDevInfo.FloorNo);
        //        }
        //        //房间号
        //        if (singleDevInfo.RoomNo == null)
        //        {
        //            sendData[38] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[38] = Convert.ToByte(singleDevInfo.RoomNo);
        //        }
        //        float alartValue = singleDevInfo.AlertValue;
        //        float forcastValue = singleDevInfo.ForcastValue;
        //        if (alartValue == null)
        //        {
        //            alartValue = 0;
        //        }
        //        if (forcastValue == null)
        //        {
        //            forcastValue = 0;
        //        }
        //        int sendData39 = (int)(alartValue / 256);
        //        int sendData40 = (int)(alartValue % 256);
        //        int sendData41 = (int)(forcastValue / 256);
        //        int sendData42 = (int)(forcastValue % 256);
        //        sendData[39] = Convert.ToByte(sendData39);
        //        sendData[40] = Convert.ToByte(sendData40);
        //        sendData[41] = Convert.ToByte(sendData41);
        //        sendData[42] = Convert.ToByte(sendData42);
        //        //int intSumCheck=0;
        //        //for (int j = 6; j < 43; j++)
        //        //{
        //        //    sendData[3] = Convert.ToByte(sendData[3] ^ sendData[j]);
        //        //    intSumCheck = intSumCheck + sendData[j];
        //        //}
        //        //sendData[4] = Convert.ToByte(intSumCheck % 256);
        //        byte[] checkValue = CheckValue(sendData, 6, 43);
        //        sendData[3] = checkValue[0];
        //        sendData[4] = checkValue[1];
        //        lstSendData.Add(sendData);
        //    }
        //    return lstSendData;
        //}
        ////下传8001器件数据
        //public static List<byte[]> GetPackageBBFor8001(List<Model.DeviceInfo8001> deviceInfo)
        //{
        //    List<byte[]> lstSendData = new List<byte[]>();
        //    foreach (Model.DeviceInfo8001 singleDevInfo in deviceInfo)
        //    {
        //        byte[] sendData = new byte[54];
        //        sendData[0] = 0xAA;
        //        sendData[1] = 0x55;
        //        sendData[2] = 0xDA;
        //        sendData[3] = 0x00; //异或值校验
        //        sendData[4] = 0x00;//累加和校验
        //        //??
        //        sendData[5] = 0x30;　//数据长度 ?? 暂定为01 ??
        //        sendData[6] = 0xBB;　//发送器件命令
        //        //sendData[7] = Convert.ToByte(deviceInfo.Count);　//器件总数
        //        sendData[7] = Convert.ToByte(0x13);　//器件总数
        //        sendData[8] = Convert.ToByte(singleDevInfo.Loop.Controller.MachineNumber);　//控制器号
        //        sendData[9] = Convert.ToByte(singleDevInfo.Loop.Code);　//回路号
        //        sendData[10] = Convert.ToByte(singleDevInfo.Code.Substring(singleDevInfo.Loop.Controller.MachineNumber.Length + singleDevInfo.Loop.Code.Length, 3));　//地编号

        //        sendData[11] = Convert.ToByte(GetDevTypeFor8001(singleDevInfo.TypeCode, singleDevInfo.Feature) * 8 + singleDevInfo.Disable * 4 + singleDevInfo.SensitiveLevel);//器件状态（灵敏度、屏蔽）;NT8001还有特性;根据这些值转换为“器件内部编码”

        //        //GetDevType(CInt(leixing)) * 8 + geli * 4
        //        sendData[12] = Convert.ToByte(singleDevInfo.TypeCode); //设备类型
        //        //           dsent(12) = Left(Trim(.Text), 3)    '  设备类型
        //        if (sendData[12] > 100)
        //        {
        //            sendData[12] = Convert.ToByte(Convert.ToInt16(sendData[12]) - 64);
        //        }

        //        sendData[13] = Convert.ToByte("00"); //输出组1高位
        //        sendData[14] = Convert.ToByte(singleDevInfo.LinkageGroup1); //输出组1低位

        //        sendData[15] = Convert.ToByte("00"); //输出组2 高位
        //        sendData[16] = Convert.ToByte(singleDevInfo.LinkageGroup2); //输出组2 低位

        //        sendData[17] = Convert.ToByte("00"); //输出组2 高位
        //        sendData[18] = Convert.ToByte(singleDevInfo.LinkageGroup3); //输出组2 低位



        //        sendData[19] = Convert.ToByte(singleDevInfo.DelayValue); //延时
        //        //                DipSkpAddr = Trim(.Text)
        //        //If DipSkpAddr = "" Then DipSkpAddr = 0

        //        //SpkMun = DipSkpJAdr * 1024 + (DipSkpAdr - 1) * 63 + (DipSkpAddr - 1) + 1
        //        //If SpkMun >= 0 Then
        //        //dsent(20) = SpkMun \ 256
        //        //dsent(21) = SpkMun Mod 256

        //        sendData[20] = 0x00; //手控盘号 高位
        //        sendData[21] = 0x00;// 手控盘号 低位

        //        sendData[22] = 0x00; //防火分区 高位'原软件中直接写为0了
        //        sendData[23] = 0x00;// 防火分区 低位 '原软件中直接写为0了
        //        //If vGbzone = "" Then vGbzone = 0
        //        sendData[24] = 0x00; //广播分区
        //        //25~49为安装地点
        //        //将地点信息逐字符取出，将每个字符转换为ANSI代码后，存入sendData数据中；
        //        //Convert.ToBase64String();     
        //        int startIndex = 25;
        //        char[] charArrayLocation = singleDevInfo.Location.ToArray();
        //        //采用Base64编码传递数据
        //        System.Text.Encoding ascii = System.Text.Encoding.GetEncoding(54936);
        //        for (int j = 0; j < charArrayLocation.Length; j++)
        //        {
        //            //sendData[startIndex]=Convert.ToByte(Convert.ToBase64String(System.Text.Encoding.GetEncoding(54936).GetBytes(charArrayLocation[j].ToString())));

        //            Byte[] encodedBytes = ascii.GetBytes(charArrayLocation[j].ToString());
        //            if (encodedBytes.Length == 1)
        //            {
        //                sendData[startIndex] = encodedBytes[0];
        //                startIndex++;
        //            }
        //            else
        //            {
        //                sendData[startIndex] = encodedBytes[0];
        //                startIndex++;
        //                sendData[startIndex] = encodedBytes[1];
        //                startIndex++;
        //            }


        //        }
        //        //补足位数
        //        for (int j = startIndex; j < 49; j++)
        //        {
        //            sendData[j] = 0x00;
        //        }


        //        //楼号
        //        if (singleDevInfo.BuildingNo == null)
        //        {
        //            sendData[50] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[50] = Convert.ToByte(singleDevInfo.BuildingNo);
        //        }
        //        //区号
        //        if (singleDevInfo.ZoneNo == null)
        //        {
        //            sendData[51] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[51] = Convert.ToByte(singleDevInfo.ZoneNo);
        //        }
        //        //层号
        //        if (singleDevInfo.FloorNo == null)
        //        {
        //            sendData[52] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[52] = Convert.ToByte(singleDevInfo.FloorNo);
        //        }
        //        //房间号
        //        if (singleDevInfo.RoomNo == null)
        //        {
        //            sendData[53] = 0x00;
        //        }
        //        else
        //        {
        //            sendData[53] = Convert.ToByte(singleDevInfo.RoomNo);
        //        }
        //        byte[] checkValue = CheckValue(sendData, 6, 54);
        //        sendData[3] = checkValue[0];
        //        sendData[4] = checkValue[1];
        //        lstSendData.Add(sendData);
        //    }
        //    return lstSendData;
        //}
        //public static string AnalysisPackageGetControllerType(string strPackage)
        //{

        //    return strPackage;
        //}
        /// <summary>
        /// 取得控制器类型
        /// </summary>
        /// <param name="package"></param>
        /// <param name="controllerModel"></param>
        /// <returns></returns>
        //public static Model.ControllerModel.ControllerType AnalysisPackageGetControllerType(byte[] package, ref Model.ControllerModel controllerModel)
        //{
        //    byte[] checkValue = CheckValue(package, 6, package.Length);
        //    Model.ControllerModel.ControllerType controllerType = Model.ControllerModel.ControllerType.NONE;
        //    if (checkValue[0] == package[3] && checkValue[1] == package[4])//校验正确
        //    {
        //        if (package[8] == 0x36)
        //        {
        //            controllerType = Model.ControllerModel.ControllerType.NT8036;
        //            controllerModel.DeviceAddressLength = 8;
        //        }
        //        else if (package[8] == 0x21)
        //        {
        //            controllerType = Model.ControllerModel.ControllerType.NT8021;
        //            controllerModel.DeviceAddressLength = 8;
        //        }
        //        else if (package[8] == 0x7)
        //        {
        //            controllerType = Model.ControllerModel.ControllerType.NT8007;
        //            //暂不知道器件编码长度2016-12-29
        //        }
        //        else if (package[8] == 0x6)
        //        {
        //            controllerType = Model.ControllerModel.ControllerType.NT8001;
        //            controllerModel.DeviceAddressLength = 8;
        //        }
        //        else if (package[8] == 0x1)
        //        {
        //            controllerType = Model.ControllerModel.ControllerType.NT8001;
        //            controllerModel.DeviceAddressLength = 7;
        //            if (package.Length == 8) //数据包长度为８，代表老版控制器,由于没有版本信息，写为-1
        //            {
        //                controllerModel.Version = -1;
        //            }
        //            else//如果有版本信息，写入Version字段
        //            {
        //                controllerModel.Version = Convert.ToInt16(package[9].ToString(), 16);
        //            }

        //        }
        //        else if (package[8] != 0x21 && package[8] != 0x36 && package[8] != 0x6)
        //        {
        //            GetControllerType(package[8]);
        //        }
        //    }
        //    return controllerType;
        //}

        //private static Model.ControllerModel.ControllerType GetControllerType(byte b)
        //{
        //    #region 有些值还不知道是什么意思,如2,4
        //    //Case &H0, &H3, &H2, &H1, &H4, &H36, &H21, &H6: '未通过查询命令，直接收到控制器类型，则认为控制器处于发送模式ywx
        //    #endregion
        //    switch (b)
        //    {
        //        case 0x0:
        //            return Model.ControllerModel.ControllerType.FT8000;
        //        case 0x3:
        //            return Model.ControllerModel.ControllerType.FT8003;
        //        case 0x2:
        //            return Model.ControllerModel.ControllerType.NONE;
        //        case 0x1:
        //            return Model.ControllerModel.ControllerType.NT8001;
        //        case 0x4:
        //            return Model.ControllerModel.ControllerType.NONE;
        //        case 0x36:
        //            return Model.ControllerModel.ControllerType.NT8036;
        //        case 0x21:
        //            return Model.ControllerModel.ControllerType.NT8021;
        //        case 0x6:
        //            return Model.ControllerModel.ControllerType.NT8001;
        //        default:
        //            return Model.ControllerModel.ControllerType.NONE;
        //    }
        //}
        #endregion
    }
}
