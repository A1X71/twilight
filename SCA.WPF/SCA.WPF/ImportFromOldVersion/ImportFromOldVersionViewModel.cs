﻿using System;
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
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.WPF.Infrastructure;
using System.Text.RegularExpressions;

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

        private string _errorMessagePromptName;
        private string _errorMessagePromptImportFilePath;
        private string _errorMessagePromptSaveFilePath;
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
        public string ErrorMessagePromptName
        {
            get
            {
                return _errorMessagePromptName;
            }
            set
            {
                _errorMessagePromptName = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessagePromptImportFilePath
        {
            get
            {
                return _errorMessagePromptImportFilePath;
            }
            set
            {
                _errorMessagePromptImportFilePath = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }
        public string ErrorMessagePromptSaveFilePath
        {
            get
            {
                return _errorMessagePromptSaveFilePath;
            }
            set
            {
                _errorMessagePromptSaveFilePath=value;
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
            dialog.Filter = "V2.X文件 (*.mdb)|*.mdb|V4.X文件 (*.nt)|*.nt";            
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
            this.ErrorMessagePromptImportFilePath = "";
            this.ErrorMessagePromptName = "";
            this.ErrorMessagePromptSaveFilePath = "";
            bool verifyFlag = true;
            ProjectConfig projectConfig = new ProjectConfig();
            Dictionary<string,RuleAndErrorMessage> dictRule= projectConfig.GetProjectInfoRegularExpression();
            RuleAndErrorMessage rule = dictRule["Name"];
            Regex exminator = new Regex(rule.Rule);

            if (!string.IsNullOrEmpty(ProjectName))
            {

                if (!exminator.IsMatch(ProjectName))
                {
                    ErrorMessagePromptName = rule.ErrorMessage;
                    verifyFlag = false;
                }
            }
            else
            {
                ErrorMessagePromptName = "请输入工程名称";
                verifyFlag = false;
            }
            if (string.IsNullOrEmpty(ImportedFilePath))
            {
                ErrorMessagePromptImportFilePath = "请选择有效导入文件路径";
                verifyFlag = false;
            }
            if (string.IsNullOrEmpty(SavedFilePath))
            {
                ErrorMessagePromptSaveFilePath = "请选择有效文件存储路径";
                verifyFlag = false;
            }
            if (verifyFlag)
            {
                IFileService _fileService = new FileService();
                ILogRecorder _logRecorder = null;
                string strImportedFilePath = this.ImportedFilePath;
                string strExtentionName = strImportedFilePath.Substring(strImportedFilePath.LastIndexOf(".") + 1);
                DBFileVersionManager dbFileVersionManager = new DBFileVersionManager(this.ImportedFilePath, _logRecorder, _fileService);
                //取得某一系列的数据文件操作服务（4，5，6 系列没有项目，7开始有项目信息，以文件扩展名作为划分依据）
                IDBFileVersionService dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByExtentionName(strExtentionName);
                //取得文件版本
                int fileVersion = dbFileVersionService.GetFileVersion();
                dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(fileVersion);
                //取得项目信息
                ProjectModel project = dbFileVersionService.GetProject(1);
                project.Name = this.ProjectName; //以当前设置的名称作为项目名
                project.SavePath = this.SavedFilePath + "\\" + project.Name + ".nt";  //以当前设置的路径作为项目的存储路径
                IControllerOperation controllerOperation = null;
                //取得项目下所有控制器信息
                List<ControllerModel> lstController = dbFileVersionService.GetControllersByProject(project);
                int dataFileVersion = 0;//数据文件版本
                ControllerModel controller = null;
                foreach (var controllerInfo in lstController)//取得控制器操作服务
                {
                    if (project.FileVersion == -1) //4，5，6版本文件无项目信息
                    {
                        project.FileVersion = controllerInfo.FileVersion;
                    }
                    switch (controllerInfo.Type)
                    {
                        case ControllerType.FT8000:
                            controllerOperation = new ControllerOperation8000();
                            break;
                        case ControllerType.FT8003:
                            controllerOperation = new ControllerOperation8003();
                            break;
                        case ControllerType.NT8001:
                            controllerOperation = new ControllerOperation8001();
                            break;
                        case ControllerType.NT8007:
                            controllerOperation = new ControllerOperation8007();
                            break;
                        case ControllerType.NT8021:
                            controllerOperation = new ControllerOperation8021();
                            break;
                        case ControllerType.NT8036:
                            controllerOperation = new ControllerOperation8036();
                            break;
                    }
                    if (controllerOperation != null)//合法控制器类型 
                    {
                        dataFileVersion = Convert.ToInt32(project.FileVersion);//取得当前项目文件版本号
                        dbFileVersionService = dbFileVersionManager.GetDBFileVersionServiceByVersionID(dataFileVersion); //取得当前文件的数据文件服务
                        //controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                        controller = controllerOperation.OrganizeControllerInfoFromSpecifiedDBFileVersion(dbFileVersionService, controllerInfo);//取得组织完成的控制器信息
                    }
                    if (controller != null)//将组织完成的信息增加至项目中
                    {
                        project.Controllers.Add(controllerInfo);
                    }
                }
                if (dataFileVersion != 0)//将所有数据转换为当前软件应用的数据版本
                {
                    project = dbFileVersionManager.VersionConverter(dataFileVersion, DBFileVersionManager.CurrentDBFileVersion, project);
                }
                SCA.BusinessLib.ProjectManager.GetInstance.CreateProject(project);
                EventMediator.NotifyColleagues("DisplayTheOpenedProject", null);
            }
        }
        public void CancelExecute()
        {
          //  RaiseEvent(new RoutedEventArgs(CloseButtonClickEvent));
            //EventMediator.NotifyColleagues("DisplayTheOpenedProject", null);
        }
        
        #endregion

    }
}
