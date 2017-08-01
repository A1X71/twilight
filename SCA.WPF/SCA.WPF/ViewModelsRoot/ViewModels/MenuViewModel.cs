using System;
using System.Windows.Input;
using Caliburn.Micro;
using Ookii.Dialogs.Wpf;
using SCA.Interface;
using SCA.BusinessLib;
using SCA.BusinessLib.BusinessLogic;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/31 16:40:59
* FileName   : MenuViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels
{
    public class MenuViewModel:PropertyChangedBase
    {
        public MenuViewModel()
        {
            
        }
        #region 属性

        #endregion
        #region 命令
        public ICommand ExportProjectToExcelCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ExportProjectToExcelExecute, null); }
        }
        public ICommand PublishAsDatabaseFileCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(PublishAsDatabaseFileExecute, null); }
        }
        public void ExportProjectToExcelExecute()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择存储的文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            dialog.ShowDialog();

            string strSelectedFolderPath = dialog.SelectedPath;
            using (new WaitCursor())
            {
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                ProjectService projectService = new ProjectService();
                projectService.ExportProjectToExcel(ProjectManager.GetInstance.Project, strSelectedFolderPath, fileService);
            }
        }
        public void PublishAsDatabaseFileExecute()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择存储的文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            dialog.ShowDialog();
            string strSelectedFolderPath = dialog.SelectedPath;
            using (new WaitCursor())
            {
                IFileService fileService = new SCA.BusinessLib.Utility.FileService();
                ProjectService projectService = new ProjectService();
                projectService.ExportProjectToExcel(ProjectManager.GetInstance.Project, strSelectedFolderPath, fileService);
                ProjectManager.GetInstance.SaveProject();
                string sourcePath=ProjectManager.GetInstance.Project.SavePath;
                string destPath=strSelectedFolderPath+"//"+ProjectManager.GetInstance.Project.Name+ ".nt";
                fileService.Copy(sourcePath, destPath, true);
            }
        }

        #endregion
        #region 方法

        #endregion 

    }
}
