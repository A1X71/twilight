using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/22 8:14:42
* FileName   : DeviceBuilder8001
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{
    class DeviceBuilder8001
    {
        int _id = 1;
        //编号根据“机号和路号”生成
        string _simpleCode = "001";
        Int16 _type = 4;
        Int16 _disable = 0;
        int _loopID = 1;
        string _linkageGroup1="0001";
        string _linkageGroup2="0002";
        string _linkageGroup3="0003";
        string _code = "0001001";
        public DeviceInfo8001 Builder()
        {
            DeviceInfo8001 device = new DeviceInfo8001
            {
                ID = _id,
                //SimpleCode = _simpleCode,
                TypeCode = _type,
                Disable = _disable,
                LoopID = _loopID,
                LinkageGroup1=_linkageGroup1,
                LinkageGroup2=_linkageGroup2,
                LinkageGroup3=_linkageGroup3,
                Code=_code
            };
            return device;
        }
        public DeviceBuilder8001 WithID(int id)
        {
            _id = id;
            return this;
        }
        public DeviceBuilder8001 WithCode(string code)
        {
            _code = code;
            return this;
        }
        public DeviceBuilder8001 WithSimpleCode(string simpleCode)
        {
            _simpleCode = simpleCode;
            return this;
        }
        public DeviceBuilder8001 WithLoopID(int loopID)
        {
            _loopID = loopID;
            return this;
        }
        public DeviceBuilder8001 WithLinkageGroup1(string linkageGroup)
        {
            _linkageGroup1 = linkageGroup;
            return this;
        }
        public DeviceBuilder8001 WithLinkageGroup2(string linkageGroup)
        {
            _linkageGroup2 = linkageGroup;
            return this;
        }
        public DeviceBuilder8001 WithLinkageGroup3(string linkageGroup)
        {
            _linkageGroup3 = linkageGroup;
            return this;
        }
    }
}
