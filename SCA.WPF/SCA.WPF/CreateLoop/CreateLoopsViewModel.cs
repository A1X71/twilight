using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SCA.BusinessLib;
using SCA.Model;
using SCA.Interface;
using SCA.BusinessLib.BusinessLogic;
using SCA.WPF.Utility;
using Caliburn.Micro;
using System.Reflection;
using System.Windows.Input;
using SCA.WPF.Infrastructure;
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
        #region 属性
        List<string> _loopsCode = null;
        private string _controllerMachineNumber;
        private string _deviceAmount=null; //节点数
        private string _loopsAmount=null;//回路数
        private string _loopName="回路"; //回路名
        private string _loopCode=null; //回路号
        private string _errorMessageDeviceAmount;//器件数量错误提示信息
        private string _errorMessageLoopAmount; //回路数量错误提示信息
        private string _errorMessageLoopName; //回路名称错误提示信息
        private string _errorMessageLoopCode; //回路编号错误提示信息
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
        public string LoopCode
        {
            get
            {
                return _loopCode;
            }
            set
            {
                _loopCode = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
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
        public string DeviceAmount
        {
            get
            {
                return _deviceAmount;
            }
            set
            {
                _deviceAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string LoopsAmount
        {
            get
            {
                return _loopsAmount;
            }
            set
            {
                _loopsAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string LoopName
        {
            get
            {
                return _loopName;
            }
            set
            {
                _loopName = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessageDeviceAmount
        {
            get
            {
                return _errorMessageDeviceAmount;
            }
            set
            {
                _errorMessageDeviceAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessageLoopAmount //回路数量错误提示信息
        {
            get
            {
                return _errorMessageLoopAmount;
            }
            set
            {
                _errorMessageLoopAmount = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessageLoopName //回路名称错误提示信息
        {
            get
            {
                return _errorMessageLoopName;
            }
            set
            {
                _errorMessageLoopName = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessageLoopCode //回路编号错误提示信息
        {
            get
            {
                return _errorMessageLoopCode;
            }
            set
            {
                _errorMessageLoopCode = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        #endregion
        #region 命令
        public ICommand ConfirmCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ConfirmExecute, null); }
        }
        public ICommand CloseCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CloseExecute, null); }
        }
        public void ConfirmExecute()
        {            
            ClearAllErrorMessage();
            RuleAndErrorMessage rule;
            Regex exminator;
            IControllerConfig config = ControllerConfigManager.GetConfigObject(TheController.Type);
            bool verifyFlag = true;
            if (LoopCode== null)
            {
                ErrorMessageLoopCode = "请指定回路号";
                verifyFlag = false;
            }
            if (DeviceAmount != null)
            { 
                rule =new RuleAndErrorMessage("^[0-9]{1,3}$", "请填写数字");
                 exminator = new Regex(rule.Rule);            
                if (!exminator.IsMatch(DeviceAmount))
                {
                    ErrorMessageDeviceAmount = rule.ErrorMessage;                    
                    verifyFlag = false;
                }
                else
                {
                    int maxValue = config.GetMaxDeviceAmountValue();
                    if (Convert.ToInt32(DeviceAmount) > maxValue)
                    {
                        ErrorMessageDeviceAmount = "控制器最大节点数为:" + maxValue.ToString();
                        verifyFlag = false;
                    }
                }
            }
            if (LoopsAmount != null)
            {                
                rule = new RuleAndErrorMessage("^[0-9]{1,3}$", "请填写数字");
                 exminator = new Regex(rule.Rule);
                if (!exminator.IsMatch(LoopsAmount))
                {
                    ErrorMessageLoopAmount = rule.ErrorMessage;
                    verifyFlag = false;
                }
                else
                {
                    int maxValue = config.GetMaxLoopAmountValue();
                    if (Convert.ToInt32(LoopsAmount) > maxValue)
                    {
                        ErrorMessageLoopAmount = "控制器最大回路数为:" + maxValue.ToString();
                        verifyFlag = false;
                    }
                }
            }
            rule =new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF()（）]{0,8}$", "允许填写”中英文字符、阿拉伯数字、圆括号”,最大长度8个字符");
            exminator = new Regex(rule.Rule);
            if (!exminator.IsMatch(LoopName))
            {
                ErrorMessageLoopName = rule.ErrorMessage;
                verifyFlag = false;
            }
            if(verifyFlag)
            {
                LoopModel loop = new LoopModel();                
                loop.Code = LoopCode;
                loop.DeviceAmount = Convert.ToInt32(DeviceAmount);
                loop.Name = LoopName;
                loop.Controller = TheController;
                loop.ControllerID = loop.Controller.ID;
                SCA.Interface.ILoopService loopService = new SCA.BusinessLib.BusinessLogic.LoopService(loop.Controller);
                bool result=loopService.AddLoops(loop, ControllerMachineNumber, Convert.ToInt32(LoopsAmount));
                SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshNavigator", result);
            }
            
        }
        public void CloseExecute()
        {
            ClearAllErrorMessage();
            EventMediator.NotifyColleagues("ShowDetailPane", null);
        }
        #endregion
        private void ClearAllErrorMessage()
        {
            ErrorMessageDeviceAmount = "";
            ErrorMessageLoopAmount = "";
            ErrorMessageLoopName = "";
            ErrorMessageLoopCode = "";
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
