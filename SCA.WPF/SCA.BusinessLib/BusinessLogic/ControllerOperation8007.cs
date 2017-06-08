using System.Linq;
using System.Collections.Generic;
using SCA.Interface;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/7 8:42:41
* FileName   : ControllerOperation8007
* Description: NT8007控制器操作类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperation8007 : ControllerOperationBase, IControllerOperation
    {
        public ControllerOperation8007()
        {

        }
        public ControllerOperation8007(IDatabaseService databaseService)
        { 
        
        }
        public Model.ControllerNodeModel[] GetNodes()
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerNodeType GetControllerNodeType()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LoopModel> GetLoops(int controllerID)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LoopModel> CreateLoops(int amount)
        {
            throw new System.NotImplementedException();
        }

        //public bool DeleteControllerBySpecifiedControllerID(int controllerID)
        //{
        //    throw new System.NotImplementedException();
        //}

        public System.Collections.Generic.List<Model.LoopModel> CreateLoops(int loopsAmount, int deviceAmount, Model.ControllerModel controller)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.IDevice> OrganizeDeviceInfoForSettingController()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.List<Model.LinkageConfigStandard> OrganizeLikageStandardInfoForSettingController()
        {
            throw new System.NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            throw new System.NotImplementedException();
        }

        public bool CreateController(Model.ControllerModel model)
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(Interface.DatabaseAccess.IOldVersionSoftwareDBService oldVersionSoftwareDBService)
        {
            throw new System.NotImplementedException();
        }

        public Model.ControllerType GetControllerType()
        {
            throw new System.NotImplementedException();
        }


        public int GetMaxDeviceID()
        {
            var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8007 select r;
            int maxDeviceID = 0;
            foreach (var c in controllers)
            {
                foreach (var l in c.Loops)
                {
                    List<DeviceInfo8007> lstDeviceInfo = l.GetDevices<DeviceInfo8007>();
                    if (lstDeviceInfo.Count > 0)
                    {
                        int currentLoopMaxDeviceID = lstDeviceInfo.Max(device => device.ID);
                        if (currentLoopMaxDeviceID > maxDeviceID)
                        {
                            maxDeviceID = currentLoopMaxDeviceID;
                        }
                    }
                }
            }
            return maxDeviceID;
        }

        protected override List<short?> GetAllBuildingNoWithController(ControllerModel controller)
        {
            List<short?> lstBuildingNo=new List<short?>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8007>())
                { 
                    if(!lstBuildingNo.Contains(dev.BuildingNo))
                    {
                        lstBuildingNo.Add(dev.BuildingNo);
                    }                    
                }
            }
            return lstBuildingNo;
        }

        protected override List<DeviceType> GetConfiguredDeviceTypeWithController(ControllerModel controller)
        {
            List<DeviceType> lstDeviceType = new List<DeviceType>();

            List<short> lstDeviceCode = new List<short>();
            foreach (var l in controller.Loops)
            {
                foreach (var dev in l.GetDevices<DeviceInfo8007>())
                {
                    if (!lstDeviceCode.Contains(dev.TypeCode))
                    {
                        lstDeviceCode.Add(dev.TypeCode);
                    }
                }
            }
            ControllerConfig8007 config = new ControllerConfig8007();
            List<DeviceType> lstAllTypeInfo = config.GetALLDeviceTypeInfo(null);
            //var result =from c in lstAllTypeInfo where c.Code=

            foreach (var v in lstDeviceCode)
            {
                var result= lstAllTypeInfo.Where((s) => s.Code == v);
                if (result.Count()>0)
                {
                    lstDeviceType.Add(result.FirstOrDefault());
                }
            }
            return lstDeviceType;
        }
  
        public List<DeviceInfoForSimulator> GetSimulatorDevices(ControllerModel controller)
        {

          //  var controllers = from r in SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers where r.Type == ControllerType.NT8007 select r;
            List<DeviceInfo8007> lstDeviceInfo = new List<DeviceInfo8007>();
        //    foreach (var c in controllers)
        //    {
                foreach (var l in controller.Loops)
                {
                    foreach (var d in l.GetDevices<DeviceInfo8007>())
                    {
                        lstDeviceInfo.Add(d);
                    }
               }
         //   }

            List<DeviceInfoForSimulator> lstDeviceSimulator = new List<DeviceInfoForSimulator>();
            int i = 0;
            foreach (var d in lstDeviceInfo)
            {
                DeviceInfoForSimulator simulatorDevice = new DeviceInfoForSimulator();
                simulatorDevice.SequenceNo = i;
                simulatorDevice.Code = d.Code;
                //simulatorDevice.Type =d.TypeCode
                simulatorDevice.TypeCode = d.TypeCode;
                simulatorDevice.ControllerName = controller.Name;
                simulatorDevice.LinkageGroup1 = d.LinkageGroup1;
                simulatorDevice.LinkageGroup2 = d.LinkageGroup2;
                //simulatorDevice.LinkageGroup3 = d.LinkageGroup3;
                simulatorDevice.BuildingNo = d.BuildingNo;
                simulatorDevice.ZoneNo = d.ZoneNo;
                simulatorDevice.FloorNo = d.FloorNo;
                simulatorDevice.Loop = d.Loop;
                i++;
                lstDeviceSimulator.Add(simulatorDevice);
            }
            return lstDeviceSimulator;
        }


        public List<DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode, ControllerModel controller, List<DeviceInfoForSimulator> lstAllDevicesOfOtherMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}
