using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using SCA.WPF.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/15 10:56:08
* FileName   : StatusBarViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels
{
    public class StatusBarViewModel:PropertyChangedBase
    {
        private double _currentProgressValue=0;
        private string _statusOfflinePath = @"Resources/Icon/Style1/status-offline.png";
        //public event Action<bool> CancelEvent;
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string StatusOffinePath { get { return _appCurrentPath + _statusOfflinePath; } }

        public double CurrentProgressValue 
        {
            get
            {
                return _currentProgressValue;
            }             
            set
            {
                _currentProgressValue = value;                
                NotifyOfPropertyChange("CurrentProgressValue");
            }
        }
        private Visibility _isProgressBarVisible=Visibility.Collapsed;
        private Visibility _isDescriptionTextVisible=Visibility.Collapsed;
        private string _descriptionText = string.Empty;

        public Visibility IsProgressBarVisible
        {
            get
            {
                return _isProgressBarVisible;

            }
            set
            {
                _isProgressBarVisible = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility IsDescriptionTextVisible
        {
            get
            {
                return _isDescriptionTextVisible;

            }
            set
            {
                _isDescriptionTextVisible = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string DescriptionText
        { 
            get 
            {
                return _descriptionText;        
            } 
            set 
            {
                _descriptionText = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            } 
        }
        
        public ICommand CancelCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CancelExecute, null); }
        }
        public void CancelExecute()
        {
            //得到需要导入的控制器的信息
            SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("CancelExcelImport", null);                
        }
    }
}
