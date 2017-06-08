using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/14 9:04:27
* FileName   : DeviceBuilder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{

    //INSERT INTO "DeviceInfo8036" ("ID", "Code", "Disabled", "LinkageGroup1", "LinkageGroup2", "AlertValue", "ForcastValue", "DelayValue", "BuildingNo", "ZoneNo", "FloorNo", "RoomNo", "Location", "LoopID", "TypeID", "ROWID") VALUES (4, '00101004', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 9, 4);
    class DeviceBuilder8036
    {
        int _id = 1;
        //编号根据“机号和路号”生成
        string _simpleCode = "001";
        
        Int16 _type = 4;
        Int16 _disable = 0;
        int _loopID = 1;
        public DeviceInfo8036 Builder()
        {
            DeviceInfo8036 device = new DeviceInfo8036
            {
                ID=_id,
                //SimpleCode=_simpleCode,//Commented at 2017-04-05 ,后续需要修改单元测试，改为Code
                TypeCode=_type,
                Disable=_disable ,
                LoopID=_loopID
            };
            return device;
        }
        public DeviceBuilder8036 WithID(int id)
        {
            _id = id;
            return this;
        }
        public DeviceBuilder8036 WithSimpleCode(string simpleCode)
        {
            _simpleCode = simpleCode;
            return this;
        }
        public DeviceBuilder8036 WithLoopID(int loopID)
        {
            _loopID = loopID;
            return this;
        }





    }
}
