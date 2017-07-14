using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Input;
using System.Collections.Generic;
using Caliburn.Micro;
using Ookii.Dialogs.Wpf;
using SCA.Model;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.WPF.Utility;
using SCA.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/7 13:21:49
* FileName   : ImportFromOldVersionViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ImportFromOldVersion
{
    public class ImportFromOldVersionViewModel:PropertyChangedBase
    {
        #region 属性
        //项目名称
        private string _projectName;
        //导入文件路径
        private string _importedFilePath;
        //文件存储路径
        private string _savedFilePath;
        public String ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ImportedFilePath
        {
            get
            {
                return _importedFilePath;
            }
            set
            {
                _importedFilePath = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string SavedFilePath
        {
            get
            {
                return _savedFilePath;
            }
            set
            {
                _savedFilePath = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        #endregion 
        #region 命令
        /// <summary>
        /// 选择导入文件路径命令
        /// </summary>
        public ICommand SelectedImportedFilePathCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(SelectedImportedFilePathExecute, null); }
        }
        /// <summary>
        /// 选择存储文件路径命令
        /// </summary>
        public ICommand SelectedSavedFilePathCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(SelectedSavedFilePathExecute, null); }
        }        
        /// <summary>
        /// 确定按钮命令
        /// </summary>
        public ICommand ComfirmExecuteCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ComfirmExecute, null); }
        }
        /// <summary>
        /// 取消按钮命令
        /// </summary>
        public ICommand CancelExecuteCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CancelExecute, null); }
        }
        public void SelectedImportedFilePathExecute()
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "V2.X文件 (*.mdb)|*.mdb";
            dialog.ShowDialog();
            ImportedFilePath = dialog.FileName;    
        }
        public void SelectedSavedFilePathExecute()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择文件夹";
            dialog.UseDescriptionForTitle = true; 
            dialog.ShowDialog();
            SavedFilePath = dialog.SelectedPath;
        }
        public void ComfirmExecute()
        {
            SCA.Model.ProjectModel project = new Model.ProjectModel();
            project.Name = this.ProjectName;
            project.SavePath = this.SavedFilePath; // SaveFilePathInputTextBox.Text;
            string strImportedFilePath = this.ImportedFilePath;// ImportFilePathInputTextBox.Text;               


            //~~~~~~~~
            IFileService _fileService = new FileService();
            IDatabaseService _databaseService;
            ILogRecorder _logRecorder;

            _databaseService = new MSAccessDatabaseAccess(strImportedFilePath, null, _fileService);
            IOldVersionSoftwareDBService oldVersionService = new OldVersionSoftware8036DBService(_databaseService);

            IControllerOperation controllerOperation = null;

            string[] strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            ControllerModel controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8036":
                        controllerOperation = new ControllerOperation8036(_databaseService);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }
            //~~~~~~~~~
            project.Controllers.Add(controllerInfo);
            SCA.BusinessLib.ProjectManager.GetInstance.CreateProject(project);
            //RaiseEvent(new RoutedEventArgs(ConfirmButtonClickEvent));
        }
        public void CancelExecute()
        {
            //RaiseEvent(new RoutedEventArgs(CloseButtonClickEvent));
        }
        
        #endregion

    }
}
