using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ookii.Dialogs.Wpf;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess.DBContext;
using SCA.DatabaseAccess;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Utility;
using SCA.Model;
namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// ImportFromOldVersionView.xaml 的交互逻辑
    /// </summary>
    public partial class ImportFromOldVersionView : UserControl
    {
        public ImportFromOldVersionView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent ConfirmButtonClickEvent = EventManager.RegisterRoutedEvent(
                "ConfirmButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImportFromOldVersionView));

        public static readonly RoutedEvent CloseButtonClickEvent = EventManager.RegisterRoutedEvent(
                "CloseButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImportFromOldVersionView));

        public event RoutedEventHandler ConfirmButtonClick
        {
            add { AddHandler(ConfirmButtonClickEvent, value); }
            remove { RemoveHandler(ConfirmButtonClickEvent, value); }
        }
        public event RoutedEventHandler CloseButtonClick
        {
            add { AddHandler(CloseButtonClickEvent, value); }
            remove { RemoveHandler(CloseButtonClickEvent, value); }
        }

        private void SelectFolderForSourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "V2.X文件 (*.mdb)|*.mdb";
            dialog.ShowDialog();
            ImportFilePathInputTextBox.Text = dialog.FileName;            
            
        }

        private void SelectFolderForSavePathButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            dialog.ShowDialog();
            SaveFilePathInputTextBox.Text=dialog.SelectedPath;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            SCA.Model.ProjectModel project = new Model.ProjectModel();
            project.Name = this.ProjectNameInputTextBox.Text;
            project.SavePath = SaveFilePathInputTextBox.Text;
            string strImportedFilePath=ImportFilePathInputTextBox.Text;
            //string strProjectFileSavePath = SaveFilePathInputTextBox.Text;
            //string strProjectName = ProjectNameInputTextBox.Text;
           // project.SavePath = this.FilePathInputTextBox.Text;            
            

            //~~~~~~~~
            IFileService _fileService =  new FileService();
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
            RaiseEvent(new RoutedEventArgs(ConfirmButtonClickEvent));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseButtonClickEvent));
        }




        
    }
}
