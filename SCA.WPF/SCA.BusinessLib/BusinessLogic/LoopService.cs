using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.DatabaseAccess;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
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
                IFileService _fileService = new SCA.BusinessLib.Utility.FileService();
                ILogRecorder logger = null;
                IDatabaseService _databaseService = new SCA.DatabaseAccess.SQLiteDatabaseAccess(_controller.Project.SavePath, logger, _fileService);                
                ILoopDBService loopDBService = new SCA.DatabaseAccess.DBContext.LoopDBService(_databaseService);
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
                IControllerConfig config=ControllerConfigManager.GetConfigObject(_controller.Type);
                short allowMaxLoopValue=config.GetMaxLoopAmountValue();
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
                    l.Code = _controller.MachineNumber + (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    l.Name = _controller.MachineNumber + (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    //l.Code = (currentMaxLoopCode + i).ToString().PadLeft(_controller.LoopAddressLength, '0');
                    //l.Name = loop.Name+i.ToString();
                    l.DeviceAmount = loop.DeviceAmount;
                    l.Controller = _controller;
                    _controller.Loops.Add(l);
                 //   SetDataDirty();
                }
       
            }
            catch
            {
                return false;
            }
            return true;
            
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
