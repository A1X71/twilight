﻿#pragma checksum "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CBB25A14441BF16CAABB91D913EF235D"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using SCA.WPF.Infrastructure;
using SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SCA.WPF.ViewsRoot.Views.DetailInfo {
    
    
    /// <summary>
    /// LinkageConfigStandardView
    /// </summary>
    public partial class LinkageConfigStandardView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 46 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddNewLine;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDelete;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCopy;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPaste;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl DataGrid_Standard;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SCA.WPF;component/viewsroot/views/detailinfo/linkageconfigstandardview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnAddNewLine = ((System.Windows.Controls.Button)(target));
            return;
            case 2:
            this.btnDelete = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\..\..\ViewsRoot\Views\DetailInfo\LinkageConfigStandardView.xaml"
            this.btnDelete.Click += new System.Windows.RoutedEventHandler(this.btnDelete_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnCopy = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnPaste = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.DataGrid_Standard = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DetailInfoGridControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

