using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.BusinessLib.ComCommunication
{
    public interface IProtocolDriver
    {
        byte GetCommand(byte[] bytes);

        #region 非通用Driver操作,可放在具体类中
       // string GetControllerType(); //觉得放在这里不妥，因为通用的协议不应该涉及具体的信息如“控制器类型”
        //byte[] GetPackage66();
        //byte[] GetPackageB9();
        //List<byte[]> GetPackageBB(List<Model.DeviceInfo8036> deviceInfo);//各控制器不同
        //Int16 GetDevType(Int16 deviceType); //器件类型,//各控制器不同
        //Int16 ConvertSwitchCondition(Int16 originalValue);//与GetDevType配套使用
        #endregion

        //计算校验值 
        byte[] CheckValue(byte[] byteValues, int offset, int count);
        //将字节转为16进制值
        string ByteToHex(byte[] comByte);



    }
}
