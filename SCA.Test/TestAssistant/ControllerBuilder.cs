using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/14 8:07:04
* FileName   : ControllerBuilder
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test.TestAssistant
{
    class ControllerBuilder
    {
        int _id = 1;
        string _name = "Controller8036";
        string _portName = "Com1";
        int _deviceAddressLength = 8;
        string _machineNumber = "1";
        List<LinkageConfigStandard> _lstStandardLinkageConfig;
      //  ProjectModel _project;
        int _projectID = 1;
        List<LoopModel> _lstLoopModel;
        public ControllerModel Build()
        {
            ControllerModel controllerModel = new ControllerModel
            {
                ID=_id,
                Name=_name,
                PortName=_portName,
                DeviceAddressLength=_deviceAddressLength,
                MachineNumber=_machineNumber,
                ProjectID=_projectID  
            
            };
            if(_lstLoopModel!=null)
            { 
                foreach (var l in _lstLoopModel)
                {
                    controllerModel.Loops.Add(l);  
                }
            }
            if(_lstStandardLinkageConfig!=null )
            {
                foreach(var l in _lstStandardLinkageConfig)
                {
                    controllerModel.StandardConfig.Add(l);
                }
            }            
            return controllerModel;
        }
        public ControllerBuilder WithProjectID(int projectID)
        {
            _projectID = projectID;
            return this;
        }

        public ControllerBuilder WithLoop(List<LoopModel> lstLoops)
        {
            _lstLoopModel = lstLoops;
            return this;
        }
        public ControllerBuilder WithID(int id)
        {
            _id = id;
            return this;
        }
        public ControllerBuilder WithName(string  name)
        {
            _name = name;
            return this;
        }
        public ControllerBuilder WithStandardLinkageConfig(List<LinkageConfigStandard> lstLinkageConfigStandard)
        {
            _lstStandardLinkageConfig = lstLinkageConfigStandard;
            return this;
        }
        public ControllerBuilder WithDeviceAddressLength(int addressLength)
        {
            _deviceAddressLength = addressLength;
            return this;
        }
        public ControllerBuilder WithMachineNumber(string machineNumber)
        {
            _machineNumber = machineNumber;
            return this;
        }

    }
}
