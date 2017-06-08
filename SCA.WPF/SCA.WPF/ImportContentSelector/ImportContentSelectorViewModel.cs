using System;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using SCA.WPF.Utility;
/* ==============================
*
* Author     : William
* Create Date: 2017/6/6 16:03:18
* FileName   : ImportContentSelectorViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ImportContentSelector
{
    public class ImportContentSelectorViewModel : PropertyChangedBase
    {
        private string _title;
        private string _errorInfo;
        private Visibility _errorContentVisibility = Visibility.Collapsed;
        private Visibility _importDataSelectorVisibility = Visibility.Collapsed;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorInfo
        {
            get
            {
                return _errorInfo;
            }
            set
            {
                _errorInfo = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public Visibility ErrorContentVisibility
        {
            get
            {
                return _errorContentVisibility;
            }
            set
            {
                _errorContentVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }

        }
        public Visibility ImportDataSelectorVisibility
        {
            get
            {
                return _importDataSelectorVisibility;
            }
            set
            {
                _importDataSelectorVisibility = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
       // public
            //        VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            //dialog.Filter = "工程文件 (*.nt)|*.nt";
            //dialog.ShowDialog();
            //if (dialog.FileName != "")
            //{ 
            //    ProjectManager.GetInstance.OpenProject(dialog.FileName);
            //    EventMediator.NotifyColleagues("DisplayTheOpenedProject", null);
            //}
    }
}
