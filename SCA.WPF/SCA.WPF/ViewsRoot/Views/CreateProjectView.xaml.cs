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
using System.Text.RegularExpressions;
using Ookii.Dialogs.Wpf;
using SCA.BusinessLib.BusinessLogic;
using SCA.Model;
namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// CreateProjectView.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectView : UserControl
    {
        public CreateProjectView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
                "AddButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateProjectView));

        public static readonly RoutedEvent CloseButtonClickEvent = EventManager.RegisterRoutedEvent(
                "CloseButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateProjectView));

        public event RoutedEventHandler AddButtonClick
        {
            add { AddHandler(AddButtonClickEvent, value); }
            remove { RemoveHandler(AddButtonClickEvent, value); }
        }
        public event RoutedEventHandler CloseButtonClick
        {
            add { AddHandler(CloseButtonClickEvent, value); }
            remove { RemoveHandler(CloseButtonClickEvent, value); }
        }
        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择存储的文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            dialog.ShowDialog();
            this.FilePathInputTextBox.Text = dialog.SelectedPath;            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessagePromptName.Text = "";
            this.ErrorMessagePromptFilePath.Text = "";
            bool verifyFlag = true;
              
            ProjectConfig projectConfig = new ProjectConfig();
            Dictionary<string, RuleAndErrorMessage> dictRule = projectConfig.GetProjectInfoRegularExpression();
            RuleAndErrorMessage rule = dictRule["Name"];

            Regex exminator = new Regex(rule.Rule);

            if (!exminator.IsMatch(this.ProjectNameInputTextBox.Text))
            {            
                this.ErrorMessagePromptName.Text = rule.ErrorMessage;
                verifyFlag = false;
            }

            if (string.IsNullOrEmpty(this.FilePathInputTextBox.Text))
            {
                this.ErrorMessagePromptFilePath.Text += "请选择有效路径";
                verifyFlag = false;
            }
            if (verifyFlag)
            {
                SCA.Model.ProjectModel project = new Model.ProjectModel();
                project.Name = this.ProjectNameInputTextBox.Text;   
                project.SavePath = this.FilePathInputTextBox.Text + "\\" + project.Name + ".nt";                 
                project.FileVersion = BusinessLogic.DBFileVersionManager.CurrentDBFileVersion;
                SCA.BusinessLib.ProjectManager.GetInstance.CreateProject(project);
                RaiseEvent(new RoutedEventArgs(AddButtonClickEvent));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessagePromptName.Text = "";
            this.ErrorMessagePromptFilePath.Text = "";
            RaiseEvent(new RoutedEventArgs(CloseButtonClickEvent));
        }
    }
}
