using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.BusinessLib.BusinessLogic;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using System.Reflection;
using SCA.WPF.Utility;
using SCA.BusinessLib.Utility;
using Ookii.Dialogs.Wpf;
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
        #region 属性
        private List<int> _lstDeviceCodeLength;
        private bool _createFromExternalFileFlag = false; //标准组态复选框状态
        private Visibility _standardStyleVisibility = Visibility.Visible; //标准创建方式显示状态
        private Visibility _externalFileStyleVisibility = Visibility.Collapsed; //从外部文件创建方式显示状态 
        private string _filePath;
        public bool CreateFromExternalFileFlag
        {
            get
            {
                return _createFromExternalFileFlag;
            }
            set
            {
                _createFromExternalFileFlag = value;
                ToggleStyle();
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility StandardStyleVisibility
        {
            get
            {
                return _standardStyleVisibility;
            }
            set
            {
                _standardStyleVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ExternalFileStyleVisibility
        {
            get
            {
                return _externalFileStyleVisibility;
            }
            set
            {
                _externalFileStyleVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        #endregion
        #region 方法
        /// <summary>
        /// 去掉ControllerType的None及UNCOMPATIBLE值
        /// </summary>
        /// <returns></returns>
        public List<Model.ControllerType> GetControllerType()
        {
            ControllerConfigNone config = new ControllerConfigNone();
            return config.GetControllerType();            
        }
        public List<string> GetSerialPortNumber()
        {
            ControllerConfigNone config = new ControllerConfigNone();
            return config.GetSerialPortNumber();                        
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
        private string GetExtensionOfFilePath(string filePath)
        { 
            int position=filePath.LastIndexOf('.');
            return filePath.Substring(position + 1);
        }
        private void ToggleStyle()
        {
            if (CreateFromExternalFileFlag)
            {
                ExternalFileStyleVisibility = Visibility.Visible;
                StandardStyleVisibility = Visibility.Collapsed;
            }
            else
            {
                StandardStyleVisibility = Visibility.Visible;
                ExternalFileStyleVisibility = Visibility.Collapsed;
            }
        }
        #endregion 方法
        #region 命令
        public ICommand SelectFilePathCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(SelectFilePathExecute, null); }
        }
        public void SelectFilePathExecute()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "EXCEL2003文件 (*.xls)|*.xls|EXCEL2007文件(*.xlsx)|*.xlsx|MDB文件(*.mdb)|*.mdb";
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                FilePath = dialog.FileName;
            }
        }
        public ICommand ImportDataFromExcelCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ImportDataFromExcelExecute, null); }
        }
        public void ImportDataFromExcelExecute()
        {
            if (FilePath != "")
            {
                string filePathExtension = GetExtensionOfFilePath(FilePath);
                bool blnNotMatchedFlag = false;
                if (filePathExtension != "")
                {
                    switch (filePathExtension.ToUpper())
                    { 
                        case "MDB":
                            break;
                        case "XLS":
                            break;
                        case "XLSX":
                            break;
                        default:
                            blnNotMatchedFlag = true;
                            break;            
                    }
                }

       

            }
        }

        #endregion

    }
}
   