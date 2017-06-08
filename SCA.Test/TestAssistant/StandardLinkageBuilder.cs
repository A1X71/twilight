using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/21 13:41:07
* FileName   : StandardLinkageBuilder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{
    class StandardLinkageBuilder
    {
        string _code = "0001";
        int _actionCoefficient = 1;

        string _linkageNo1 = "0002";
        string _linkageNo2 = "0003";
        string _linkageNo3 = "0004";
        string _deviceNo1 = "0001001";
        string _deviceNo2 = "0001002";
        string _deviceNo3 = "0001003";
        string _deviceNo4 = "0001004";
        string _deviceNo5 = "0001005";
        string _deviceNo6 = "0001006";
        string _deviceNo7 = "0001007";
        string _deviceNo8 = "0001008";
        public LinkageConfigStandard Build()
        {
            LinkageConfigStandard standardConfig = new LinkageConfigStandard
            {
                Code=_code,
                ActionCoefficient=_actionCoefficient,
                DeviceNo1 = _deviceNo1,
                DeviceNo2 = _deviceNo2,
                DeviceNo3 = _deviceNo3,
                DeviceNo4 = _deviceNo4,
                DeviceNo5 = _deviceNo5,
                DeviceNo6 = _deviceNo6,
                DeviceNo7 = _deviceNo7,
                DeviceNo8 = _deviceNo8,
                LinkageNo1= _linkageNo1,
                LinkageNo2= _linkageNo2,
                LinkageNo3= _linkageNo3
            };
            return standardConfig;
        }
        public StandardLinkageBuilder WithCode(string strCode)
        {
            _code = strCode;
            return this;
        }
        public StandardLinkageBuilder WithActionCoefficient(int actionCoefficient )
        {
            _actionCoefficient = actionCoefficient;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo1(string deviceNo)
        {
            _deviceNo1 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo2(string deviceNo)
        {
            _deviceNo2 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo3(string deviceNo)
        {
            _deviceNo3 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo4(string deviceNo)
        {
            _deviceNo4 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo5(string deviceNo)
        {
            _deviceNo5 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo6(string deviceNo)
        {
            _deviceNo6 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo7(string deviceNo)
        {
            _deviceNo7 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithDeviceNo8(string deviceNo)
        {
            _deviceNo8 = deviceNo;
            return this;
        }
        public StandardLinkageBuilder WithLinkageNo1(string linkageNo)
        {
            _linkageNo1 = linkageNo;
            return this;
        }
        public StandardLinkageBuilder WithLinkageNo2(string linkageNo)
        {
            _linkageNo2 = linkageNo;
            return this;
        }
        public StandardLinkageBuilder WithLinkageNo3(string linkageNo)
        {
            _linkageNo3 = linkageNo;
            return this;
        }




    }
}
