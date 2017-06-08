using SCA.Model;
using SCA.WPF.Infrastructure;
using SCA.WPF.Utility;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using System;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/8 11:07:38
* FileName   : LoopSummaryViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Query
{
    public class LoopSummaryViewModel
    {
        List<string> _loopsCode = null;
        List<DeviceType> _deviceTypes = null;
        private string _saveIconPath = @"Resources/Icon/Style1/save.png";
        private string _downloadIconPath = @"Resources/Icon/Style1/c_download.png";
        private string _uploadIconPath = @"Resources/Icon/Style1/c_upload.png";

        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string SaveIconPath { get { return _appCurrentPath + _saveIconPath; } }
        public string DownloadIconPath { get { return _appCurrentPath + _downloadIconPath; } }
        public string UploadIconPath { get { return _appCurrentPath + _uploadIconPath; } }

        public ControllerModel TheController { get; set; }
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
            //set
            //{
            //    _loopsCode = value;
            // //   NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            //}
        }
        public List<DeviceType> ValidDeviceType
        {
            get
            {
                SCA.Interface.IControllerConfig config = SCA.BusinessLib.BusinessLogic.ControllerConfigManager.GetConfigObject(TheController.Type);
                return config.GetDeviceTypeInfo();
            }
            //set
            //{                
            //    _deviceTypes = value;
            //}
        }
        
        

    }
}
