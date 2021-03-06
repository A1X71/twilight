﻿using System;
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
namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// ProjectView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectView : UserControl
    {
        
        public ProjectView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
                 "AddButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProjectView));

        public event RoutedEventHandler AddButtonClick
        {
            add { AddHandler(AddButtonClickEvent, value); }
            remove { RemoveHandler(AddButtonClickEvent, value); }
        }
        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "选择存储的文件夹";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            this.FilePathInputTextBox.Text = dialog.SelectedPath;
            //if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            //    MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            //if ((bool)dialog.ShowDialog(this))
            //    MessageBox.Show(this, "The selected folder was: " + dialog.SelectedPath, "Sample folder browser dialog");    
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {            
            SCA.Model.ProjectModel project = new Model.ProjectModel();
            project.Name = this.ProjectNameInputTextBox.Text;
            project.SavePath = this.FilePathInputTextBox.Text;
            SCA.BusinessLib.ProjectManager.GetInstance.CreateProject(project);

            RaiseEvent(new RoutedEventArgs(AddButtonClickEvent));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
