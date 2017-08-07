using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.DatabaseAccess;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using SCA.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/27 9:07:22
* FileName   : LoopService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class LoopService:ILoopService
    {
        private ControllerModel _controller;
        public LoopService(ControllerModel controller)
        {
            _controller=controller;
        }
        public List<Model.LoopModel> CreateLoops<T>(int loopsAmount, int deviceAmount, Model.ControllerModel controller,IDeviceService<T> deviceService)
        {
            string strMachineNumber = controller.MachineNumber;
            ControllerOperationCommon controllerBase = new ControllerOperationCommon();
            int loopID=controllerBase.GetMaxLoopID();

            string strLoopLengthFormat = "#";
            for (int i = 0; i < controller.LoopAddressLength; i++)
            {
                strLoopLengthFormat += "0";
            }
            List<Model.LoopModel> lstLoopModel = new List<LoopModel>();
            for (int i = 0; i < loopsAmount; i++) //创建回路
            {
                LoopModel loop = new LoopModel();
                loop.Controller = controller;
                loop.ID = loopID++;
                loop.Code = strMachineNumber + i.ToString(strLoopLengthFormat);
                loop.Name = strMachineNumber + i.ToString(strLoopLengthFormat);
                loop.DeviceAmount = deviceAmount;    
                loop.SetDevices<T>(deviceService.InitializeDevices(deviceAmount));   
                lstLoopModel.Add(loop);
                loop.IsLoopDataDirty = true;
              //  SetDataDirty();

            }
            return lstLoopModel;
        }
        /// <summary>
        /// 将数据置为需要保存状态
        /// </summary>
        private void SetDataDirty()
        {
            SCA.BusinessLib.ProjectManager.GetInstance.IsDirty = true;
        }
        public bool DeleteLoopBySpecifiedLoopID(string strLoopID)
        {
            throw new NotImplementedException();
        }
        public bool DeleteLoopBySpecifiedLoopCode(string strLoopCode)
        {
            try
            {
                var result = from l in _controller.Loops where l.Code == strLoopCode select l;
                LoopModel loop=result.FirstOrDefault() ;
                if (loop!= null)
                {
                    _controller.Loops.Remove(loop);
                 //   SetDataDirty();
                    DeleteLoopFromDB(loop.ID);
                } 
            }
            catch
            {
                return false;
            }
                return true;    
           //_controller.Loops
        }
        private bool DeleteLoopFromDB(int loopID)
        {
            try
            {
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;                
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(_controller.Project.SavePath, logger, fileService);
                IDBFileVersionService dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(SCA.BusinessLogic.DBFileVersionManager.CurrentDBFileVersion);
                ILoopDBService loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(dbFileVersionService);
                loopDBService.DeleteLoopInfo(loopID);                
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public LoopModel ModifyLoopNameBySpecifiedLoopID(string strLoopID)
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> CopyLoopInfo(Model.LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public Model.ControllerModel PasteLoopInfo()
        {
            throw new NotImplementedException();
        }

        public Model.ControllerModel SwapLoopInfo()
        {
            throw new NotImplementedException();
        }

        public bool SetLoopInfoWithAll()
        {
            throw new NotImplementedException();
        }

        public bool SetLoopInfoWithPartial()
        {
            throw new NotImplementedException();
        }

        public bool CalculateLoop()
        {
            throw new NotImplementedException();
        }

        public bool ValidateLoopInfo()
        {
            throw new NotImplementedException();
        }

        
        public bool AddLoops(LoopModel loop,string machineNumber,int loopsAmount)
        {
            try
            {
                ControllerOperationCommon controllerBase = new ControllerOperationCommon();
                int loopID = controllerBase.GetMaxLoopID();
                //ControllerModel controller = ProjectManager.GetInstance.GetPrimaryController();
                int[] loopsCode = GetAllLoopCode(_controller, null);
                int currentMaxLoopCode = loopsCode.Max();
                int specifiedLoopCode=0; //指定的回路号
                bool loopCodeExistFlag=false; //回路号已经存在标志

                //if (loop.Code != "")
                //{
                //    specifiedLoopCode=Convert.ToInt32(loop.Code);
                //    if (specifiedLoopCode > currentMaxLoopCode)
                //    {
                //        currentMaxLoopCode = specifiedLoopCode;
                //    }
                //}

                for (int i = 0; i < loopsCode.Length; i++)
                { 
                    if(loopsCode[i]==specifiedLoopCode)   
                    {
                        loopCodeExistFlag=true;
                        break;
                    }
                }


                IControllerConfig config=ControllerConfigManager.GetConfigObject(_controller.Type);
                short allowMaxLoopValue=config.GetMaxLoopAmountValue();
                if (loopsAmount == 0)
                {
                    return false;
                }
                if (loopsAmount > allowMaxLoopValue)
                {
                    //超出最大数回数
                    return false;
                }
                if ((allowMaxLoopValue - currentMaxLoopCode) < loopsAmount)
                { 
                    //剩余回路空间无法创建指定回路数量的回路
                    return false;
                }

                string strLoopLengthFormat = "#";

                for (int i = 0; i < _controller.LoopAddressLength; i++)
                {
                    strLoopLengthFormat += "0";
                }

                for (int i = 1; i <= loopsAmount; i++)
                {                    
                    LoopModel l = new LoopModel();
                    l.ID = ++loopID;
                    if (!loopCodeExistFlag)
                    {
                        if (i == 1) //减少复杂性，只对指定的回路号使用一次
                        {
                            l.Code = _controller.MachineNumber + loop.Code.ToString().PadLeft(_controller.LoopAddressLength, '0');
                        }
                        else
                        {                              
                            l.Code = _controller.MachineNumber + (currentMaxLoopCode + i-1).ToString().PadLeft(_controller.LoopAddressLength, '0');                            
                        }
                        
                    }                    
                    else
                    {
                        l.Code = _controller.MachineNumber + (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    }

                    l.Name = loop.Name + "(" + l.Code + ")";// _controller.MachineNumber + (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    //l.Code = (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    //l.Name = loop.Name+i.ToString();
                    l.DeviceAmount = loop.DeviceAmount;                    
                    l.Controller = _controller;
                    l.ControllerID = _controller.ID;
                    InitializeDevicesToLoop(_controller.Type, l);
                    _controller.Loops.Add(l);
//                  SetDataDirty();
                }
       
            }
            catch
            {
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 根据器件数量初始化器件信息
        /// </summary>
        /// <param name="cType">控制器类型</param>
        /// <param name="loop">回路对象</param>        
        private void  InitializeDevicesToLoop(ControllerType cType,LoopModel loop)
        {
                    switch (cType)
                    { 
                        case ControllerType.FT8000:
                            {
                                DeviceService8000 deviceService = new DeviceService8000();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8000> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8000>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.FT8003:
                            {
                                DeviceService8003 deviceService = new DeviceService8003();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8003> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8003>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.NT8001:
                            {
                                DeviceService8001 deviceService = new DeviceService8001();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8001>  lstDeviceInfo=deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8001>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.NT8007:
                            {
                                DeviceService8007 deviceService = new DeviceService8007();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8007> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8007>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.NT8021:
                            {
                                DeviceService8021 deviceService = new DeviceService8021();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8021> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8021>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.NT8036:
                            {
                                DeviceService8036 deviceService = new DeviceService8036();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8036> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8036>(lstDeviceInfo);
                                break;
                            }
                        case ControllerType.NT8053:
                            {
                                DeviceService8053 deviceService = new DeviceService8053();
                                deviceService.TheLoop = loop;
                                List<DeviceInfo8053> lstDeviceInfo = deviceService.Create(loop.DeviceAmount);
                                loop.SetDevices<DeviceInfo8053>(lstDeviceInfo);
                                break;
                            }
                    }
        }
        public int[] GetAllLoopCode(ControllerModel controller,Func<int[],int[]> sort)
        {
            int[] loopCode = null;
            if (controller.Loops.Count == 0)
            {
                loopCode = new int[1];
                loopCode[0] = 0;
                return loopCode;
            }
            else
            { 
                for (int i = 0; i < controller.Loops.Count; i++)
                {
                    loopCode = new int[controller.Loops.Count];
                    loopCode[i] = Convert.ToInt32(controller.Loops[i].SimpleCode);            

                }
                if (sort != null)
                {
                    return sort(loopCode);
                }
                else
                {
                    return loopCode;
                }
                
            }            
        }
    }
}
