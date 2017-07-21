using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/6 11:42:40
* FileName   : ControllerConfigNone
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfigNone : ControllerConfigBase, IControllerConfig
    {
        public Model.ControllerNodeModel[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetStandardLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetGeneralLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetMixedLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public string GetDeviceTypeCodeInfo()
        {
            throw new NotImplementedException();
        }

        public short GetMaxLoopAmountValue()
        {
            throw new NotImplementedException();
        }

        public short GetMaxMachineAmountValue(int addressLength)
        {
            throw new NotImplementedException();
        }

        public short GetMaxDeviceAmountValue()
        {
            throw new NotImplementedException();
        }

        public List<int> GetDeviceCodeLength()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForStandardLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForMixedLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForGeneralLinkageConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Model.RuleAndErrorMessage> GetStandardLinkageConfigRegularExpression()
        {
            throw new NotImplementedException();
        }


        public override List<Model.DeviceType> GetDeviceTypeInfo()
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, Model.RuleAndErrorMessage> GetDeviceInfoRegularExpression(int addressLength)
        {
            throw new NotImplementedException();
        }


        public short GetMaxAmountForBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForSubBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForKeyNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }


        public List<Model.DeviceType> GetAllowedDeviceTypeInfoForAnyAlarm()
        {
            throw new NotImplementedException();
        }

        public List<Model.DeviceType> GetAllowedDeviceTypeInfoForLinkageGroup8000()
        {
            throw new NotImplementedException();
        }

        public int DefaultDeviceTypeCode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public List<int> GetActionCoefficient()
        {
            throw new NotImplementedException();
        }







        public List<Model.DeviceType> GetDeviceTypeInfoWithAnyAlarm()
        {
            throw new NotImplementedException();
        }

        public List<Model.DeviceType> GetDeviceTypeInfoWithoutFireDevice()
        {
            throw new NotImplementedException();
        }
    }
}
