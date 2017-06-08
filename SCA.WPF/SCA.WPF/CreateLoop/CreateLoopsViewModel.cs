using System.Collections.Generic;
using SCA.BusinessLib;
using SCA.Model;
using SCA.Interface;
using SCA.BusinessLib.BusinessLogic;
using SCA.WPF.Utility;
using Caliburn.Micro;
using System.Reflection;

/* ==============================
*
* Author     : William
* Create Date: 2017/3/15 14:21:37
* FileName   : CreateLoopsViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.CreateLoop
{
    public class CreateLoopsViewModel:PropertyChangedBase
    {
        List<string> _loopsCode = null;
        string _controllerMachineNumber;
        public ControllerModel TheController { get; private set; }

        /// <summary>
        /// 可设置的回路编号
        /// </summary>
        public List<string> LoopsCode 
        {
            get
            {
                if (_loopsCode == null)
                {
                    _loopsCode = new List<string>();
                }
                return _loopsCode;
            }
            private  set
            {
                _loopsCode = value;
           //     NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public string ControllerMachineNumber
        {
            get 
            {
                return _controllerMachineNumber;
            }
            private set
            {
                _controllerMachineNumber = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public List<string> GetPrimaryControllerLoopsAmount()
        {
            List<string> lstLoopsCode = new List<string>();            
            if (ProjectManager.GetInstance.Project != null)
            { 
                SCA.Model.ControllerModel controller = ProjectManager.GetInstance.GetPrimaryController();
                SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(controller.Type);
                short maxLoopAmount=controllerConfig.GetMaxLoopAmountValue();
                
                for(int i=1;i<=maxLoopAmount;i++)
                {
                    lstLoopsCode.Add(i.ToString().PadLeft(controller.LoopAddressLength, '0'));
                }
            }
            LoopsCode = lstLoopsCode;
            return lstLoopsCode;
        }
        /// <summary>
        /// 设置当前页面所属的控制器
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(ControllerModel controller)
        {
            SCA.Model.ControllerModel c;
            if (controller == null)
            {
                c = ProjectManager.GetInstance.GetPrimaryController();

            }
            else
            {
                c = ProjectManager.GetInstance.GetControllerBySpecificID(controller.ID);
            }
            TheController = c;
            ControllerMachineNumber = TheController.MachineNumber;
        }
        public  void SetLoopsAmountBySpecifiedController()
        {
            List<string> lstLoopsCode = new List<string>();
            if (ProjectManager.GetInstance.Project != null && TheController!=null)
            {
                SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(TheController.Type);
                short maxLoopAmount = controllerConfig.GetMaxLoopAmountValue();
                for (int i = 1; i <= maxLoopAmount; i++)
                {
                    lstLoopsCode.Add(i.ToString().PadLeft(TheController.LoopAddressLength, '0'));
                }
            }
            LoopsCode = lstLoopsCode;
        }

    }
}
