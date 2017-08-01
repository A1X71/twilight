using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using SCA.Interface;
using SCA.BusinessLib.BusinessLogic;
using SCA.WPF.Infrastructure;
namespace SCA.WPF.ViewsRoot.Views.Query
{
    /// <summary>
    /// SummaryInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class SummaryInfoView : UserControl
    {
        public SummaryInfoView()
        {
            InitializeComponent();
            this.ErrorMessage.Text = "";
            this.ErrorMessage.Visibility = Visibility.Collapsed;
        }
        
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
            "AddButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SummaryInfoView)
            );
        public static readonly RoutedEvent ScanPortsButtonClickEvent = EventManager.RegisterRoutedEvent(
            "ScanPortsButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SummaryInfoView)
            );
        public event RoutedEventHandler AddButtonClick
        {
            add { AddHandler(AddButtonClickEvent, value); }
            remove { RemoveHandler(AddButtonClickEvent, value); }
        }
        public event RoutedEventHandler ScanPortsButtonClick
        {
            add { AddHandler(ScanPortsButtonClickEvent, value); }
            remove { RemoveHandler(ScanPortsButtonClickEvent, value); }
        }

        //public static readonly RoutedEvent AllControllerInfoUploadedEvent = EventManager.RegisterRoutedEvent(
        //    "AllControllerInfoUploaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SummaryInfoView)
        //);


        //public event RoutedEventHandler AllControllerInfoUploaded
        //{
        //    add { AddHandler(AllControllerInfoUploadedEvent, value); }
        //    remove { RemoveHandler(AllControllerInfoUploadedEvent, value); }
        //}


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessage.Text = "";            
           // if (TheController != null)
           // { 
               // ControllerModel controller=new ControllerModel();
               // //controller.ID = TheController.ID;
            
               // ControllerModel controller = ((SCA.WPF.ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel)this.DataContext).TheController;
               // TheController.Name = ControllerNameInputTextBox.Text;
               // TheController.MachineNumber = MachineNumberInputTextBox.Text;
               
               // TheController.BaudRate = Convert.ToInt32(BaudsRateComboBox.SelectedItem);
            
               // TheController.PortName = ComPortComboBox.SelectedItem.ToString();
            SCA.WPF.ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel vm = (SCA.WPF.ViewModelsRoot.ViewModels.Query.SummaryInfoViewModel)this.DataContext;
            ControllerModel controller = vm.TheController;

            IControllerConfig config = ControllerConfigManager.GetConfigObject(controller.Type);
            int maxMachineNumber = config.GetMaxMachineAmountValue(controller.DeviceAddressLength);
            Dictionary<string, RuleAndErrorMessage> dictRule = config.GetControllerInfoRegularExpression(controller.DeviceAddressLength);

            RuleAndErrorMessage rule = dictRule["Name"];

            Regex exminator = new Regex(rule.Rule);
            string errorMessage = "";
            bool verifyFlag = true;
            if (!exminator.IsMatch(ControllerNameInputTextBox.Text))
            {
                errorMessage += "控制器名称:"+rule.ErrorMessage+"; ";
                //errorMessage += rule.ErrorMessage+";";
                verifyFlag = false;
            }
            rule = dictRule["MachineNumber"];
            exminator = new Regex(rule.Rule);
            if (!exminator.IsMatch(MachineNumberInputTextBox.Text))
            {
                //errorMessage += rule.ErrorMessage+";";
                errorMessage += "控制器机号:" + rule.ErrorMessage + "; ";
                verifyFlag = false;
            }

            if (verifyFlag)
            {
                if (Convert.ToInt16(MachineNumberInputTextBox.Text) > maxMachineNumber)
                {
                    errorMessage += "控制器机号:机号超出范围，最大机号为" + maxMachineNumber.ToString()+"; ";
                    //errorMessage += "机号超出范围，最大机号为" + maxMachineNumber.ToString();
                    verifyFlag = false;
                }
            }
            if (verifyFlag)
            {
                controller.Name = ControllerNameInputTextBox.Text;
                controller.MachineNumber = MachineNumberInputTextBox.Text;

                controller.PortName = ComPortComboBox.SelectedItem.ToString();
                controller.BaudRate = Convert.ToInt32(BaudsRateComboBox.SelectedItem);
                vm.SaveExecute(controller);
                controller.IsDirty = true;

                EventMediator.NotifyColleagues("RefreshNavigator", controller);
                RaiseEvent(new RoutedEventArgs(AddButtonClickEvent, controller));
            }
            else
            {
                this.ErrorMessage.Text = errorMessage;
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
           // }
        }
    }
}
