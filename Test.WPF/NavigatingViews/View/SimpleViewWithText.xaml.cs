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
using System.Windows.Shapes;

namespace Test.WPF.NavigatingViews.View
{
    /// <summary>
    /// SimpleViewWithText.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleViewWithText : Window
    {
        public SimpleViewWithText()
        {
            InitializeComponent();
            ViewModel.SimpleViewWithTextViewModel vm = new ViewModel.SimpleViewWithTextViewModel();
            vm.Name = "Welcome";
            this.DataContext = vm;
        }
    }
}
