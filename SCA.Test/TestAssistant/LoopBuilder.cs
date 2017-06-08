using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/14 8:40:36
* FileName   : LoopBuilder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{
    class LoopBuilder<T>
    {
        //ControllerModel _controller;
        int _id=1;
        string _code="00101";
        string _name="Loop1";
        int _deviceAmount=5;
        T _device;
        int _controllerID=1;
        List<T> _lstDevices;
        
        public LoopModel Build()
        {
            LoopModel loop = new LoopModel
            {
                ID=_id,
                Code=_code,
                Name=_name,
                DeviceAmount=_deviceAmount,
                ControllerID=_controllerID
            };
            foreach (var d in _lstDevices)
            {
                loop.SetDevice<T>(d);
            }
            if (_device != null)
            {
                loop.SetDevice<T>(_device);
            }
            return loop;
        }
        public LoopBuilder<T> WithID(int id)
        {
            _id = id;
            return this;
        }
        public LoopBuilder<T> WithCode(string code)
        {
            _code = code;
            return this;
        }
        public LoopBuilder<T> WithDevices(List<T> lstDevices)
        {
            _lstDevices = lstDevices;
            return this;
        }
        public LoopBuilder<T> WithDevice(T  device)
        {
            _device = device;
            return this;
        }
        public LoopBuilder<T> WithDeviceAmount(int deviceAmount)
        {
            _deviceAmount = deviceAmount;
            return this;
        }
        public LoopBuilder<T> WithControllerID(int controllerID)
        {
            _controllerID = controllerID;
            return this;
        }
    }
}
