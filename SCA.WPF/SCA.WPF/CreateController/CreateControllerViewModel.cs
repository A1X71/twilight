using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.BusinessLib.BusinessLogic;
using Caliburn.Micro;
using System.Reflection;
using SCA.WPF.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/15 10:10:37
* FileName   : CreateControllerViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.CreateController
{
    public class CreateControllerViewModel:PropertyChangedBase
    {
        private List<int> _lstDeviceCodeLength;
        /// <summary>
        /// 去掉ControllerType的None及UNCOMPATIBLE值
        /// </summary>
        /// <returns></returns>
        public List<Model.ControllerType> GetControllerType()
        {
            List<Model.ControllerType> lstControllerType = new List<Model.ControllerType>();
            foreach (Model.ControllerType type in Enum.GetValues(typeof(Model.ControllerType)))
            {
                if (type != Model.ControllerType.NONE && type != Model.ControllerType.UNCOMPATIBLE)
                {
                    lstControllerType.Add(type);
                }
            }            
            return lstControllerType;
        }
        public List<string> GetSerialPortNumber()
        {
            List<string> lstComInfo = new List<string>();            
            for (int i = 1; i < 10; i++)
            {
                lstComInfo.Add("COM" + i.ToString());
            }
            return lstComInfo;
        }
        public List<int> DeviceCodeLength
        {
            get
            {
                return _lstDeviceCodeLength;
            }
            set
            {
                _lstDeviceCodeLength = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public List<int> GetDeviceCodeLength(Model.ControllerType type)
        {
            SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(type);
            DeviceCodeLength = controllerConfig.GetDeviceCodeLength();
            return DeviceCodeLength;
        }
     
    }
}
   