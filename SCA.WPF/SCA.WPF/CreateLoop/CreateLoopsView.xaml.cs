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
using SCA.Model;
using SCA.BusinessLib.BusinessLogic;
namespace SCA.WPF.ViewsRoot.Views
{
    
    /// CreateLoopsView.xaml 的交互逻辑
    /// </summary>
    public partial class CreateLoopsView : UserControl
    {
        public CreateLoopsView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
            "AddButtonClick",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(CreateLoopsView)
            );
        public static readonly RoutedEvent CancelButtonClickEvent = EventManager.RegisterRoutedEvent(
             "CancelButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateLoopsView)
            );
        public event RoutedEventHandler AddButtonClick
        {
            add { AddHandler(AddButtonClickEvent, value); }
            remove { RemoveHandler(AddButtonClickEvent, value); }
        }
        public event RoutedEventHandler CancelButtonClick
        {
            add { AddHandler(CancelButtonClickEvent, value); }
            remove { RemoveHandler(CancelButtonClickEvent, value); }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllErrorMessage();
            bool verifyFlag = true;
            if (this.LoopCodeComboBox.SelectedItem == null)
            {
     
                this.ErrorMessageLoopCode.Content = "请指定回路号";
                verifyFlag = false;
            }

            
            LoopModel loop = new LoopModel();
            string strMachineNumber = this.MachineNumberInputLabel.Content.ToString();
            string strLoopCode = this.LoopCodeComboBox.SelectedItem.ToString();
            string strDeviceAmount = this.DeviceAmountInputTextBox.Text;
            int loopAmount = Convert.ToInt32(this.LoopAmountInputTextBox.Text);
            string strLoopName = this.LoopNameInputTextBox.Text;
            loop.Code = strLoopCode;
            loop.DeviceAmount = Convert.ToInt32(strDeviceAmount);
            loop.Name = strLoopName;
            loop.Controller = ((SCA.WPF.CreateLoop.CreateLoopsViewModel)this.DataContext).TheController;
            loop.ControllerID = loop.Controller.ID;            
            SCA.Interface.ILoopService loopService = new SCA.BusinessLib.BusinessLogic.LoopService(loop.Controller);            
            loopService.AddLoops(loop, strMachineNumber, loopAmount);
            RaiseEvent(new RoutedEventArgs(AddButtonClickEvent));

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CancelButtonClickEvent));
        }
        private void ClearAllErrorMessage()
        {
            this.ErrorMessageDeviceAmount.Content = "";
            this.ErrorMessageLoopAmount.Content = "";
            this.ErrorMessageLoopName.Content = "";
            this.ErrorMessageLoopCode.Content = "";
        }
        

    }
}
