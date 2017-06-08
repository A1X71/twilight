using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCA.Interface
{
    public interface IControllerManager
    {
        //Dictionary<string, IControllerCommunication> _controllers;
        //1.从控制器接收数据
        //2.判断控制器当前设置的状态：上传或下载
        //3.得到控制器类型
        //4.加载“具体的控制器处理类”，执行数据处理，及回复６６确认
        void ReceiveDataFromController();

    }
    public interface ISerialController
    { 
        //Command sender
        //Command receiver
        //Polling(asynchronous/synchronous)
        //By corollary you must have a Timeout Function

    }
    public interface ISerialComms
    {
        void SendMessage(string message);
    }
    public class FakeSerialComms : ISerialComms
    {

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }

}
