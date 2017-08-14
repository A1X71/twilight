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
using SCA.BusinessLib.BusinessLogic;
using SCA.Interface;
namespace SCA.WPF.ViewsRoot.Views
{
    /// <summary>
    /// CreateControllerView.xaml 的交互逻辑
    /// </summary>
    public partial class CreateControllerView : UserControl
    {
        public CreateControllerView()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent AddButtonClickEvent = EventManager.RegisterRoutedEvent(
                "AddButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateControllerView));
        public static readonly RoutedEvent CancelButtonClickEvent = EventManager.RegisterRoutedEvent(
                "CancelButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CreateControllerView)
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
        private void SerialPortNumberAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SerialPortNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllErrorMessage();
            ControllerModel controller = new ControllerModel();
            
            bool verifyFlag = true;
            if (ControllerTypeComboBox.SelectedItem != null)
            {
                   controller.Type = (ControllerType)ControllerTypeComboBox.SelectedItem;
            }
            else
            {
                
                this.ErrorMessageControllerType.Text = "请选择控制器类型";
                verifyFlag = false;
            }
            if (DeviceCodeLengthComboBox.SelectedItem != null)
            {
                controller.DeviceAddressLength = Convert.ToInt32(DeviceCodeLengthComboBox.SelectedItem);
            }
            else
            {
                
                this.ErrorMessageControllerDeviceAddressLength.Text = "请选择器件长度";
                verifyFlag = false;
            }
            if (SerialPortNumberComboBox.SelectedItem != null)
            {
                controller.PortName = SerialPortNumberComboBox.SelectedItem.ToString();
            }
            else
            {
                
                this.ErrorMessageControllerPortName.Text = "请选择端口";
                verifyFlag = false;
            }
            if (this.ControllerMachineNumInputTextBox.Text == "")
            {
                this.ErrorMessageControllerDeviceAddressLength.Text = "请填写器件长度";
                verifyFlag = false;
            }
            if (verifyFlag)
            {
                controller.Name = ControllerNameInputTextBox.Text;
                controller.MachineNumber = ControllerMachineNumInputTextBox.Text;
                controller.LoopAddressLength = 2;//回路地址长度默认为2
                IControllerConfig config = ControllerConfigManager.GetConfigObject(controller.Type);
                int maxMachineNumber = config.GetMaxMachineAmountValue(controller.DeviceAddressLength);
                Dictionary<string, RuleAndErrorMessage> dictRule = config.GetControllerInfoRegularExpression(controller.DeviceAddressLength);

                RuleAndErrorMessage rule = dictRule["Name"];

                Regex exminator = new Regex(rule.Rule);
                if (!string.IsNullOrEmpty(controller.Name))
                {
                    if (!exminator.IsMatch(controller.Name))
                    {
                        this.ErrorMessageControllerName.Text = rule.ErrorMessage;
                        verifyFlag = false;
                    }
                }
                else
                {
                    this.ErrorMessageControllerName.Text = "请填写控制器名称";
                    verifyFlag = false;
                }
                rule = dictRule["MachineNumber"];
                exminator = new Regex(rule.Rule);
                if (!exminator.IsMatch(ControllerMachineNumInputTextBox.Text))
                {
                    
                    this.ErrorMessageControllerMachineNumber.Text = rule.ErrorMessage;
                    verifyFlag = false;
                }
                else
                {
                    controller.MachineNumber = this.ControllerMachineNumInputTextBox.Text;
                }
                if (verifyFlag)
                {
                    if (Convert.ToInt16(controller.MachineNumber) > maxMachineNumber)
                    {
                        this.ErrorMessageControllerMachineNumber.Text = "机号超出范围，最大机号为" + maxMachineNumber.ToString();
                        
                        verifyFlag = false;
                    }
                }
            }
            if (verifyFlag)
            {
                ControllerManager controllerManager = new ControllerManager();
                controllerManager.InitializeAllControllerOperation(null);
                IControllerOperation controllerOperation = controllerManager.GetController(controller.Type);
                controllerOperation.AddControllerToProject(controller);
                RaiseEvent(new RoutedEventArgs(AddButtonClickEvent));
            }
        }
        private void ClearAllErrorMessage()
        {
            this.ErrorMessageControllerType.Text="";
            this.ErrorMessageControllerPortName.Text="";
            this.ErrorMessageControllerName.Text="";
            this.ErrorMessageControllerMachineNumber.Text="";
            this.ErrorMessageControllerDeviceAddressLength.Text="";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CancelButtonClickEvent));
        }

        private void ControllerTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = "welcome";
            if(e.AddedItems!=null )
            {
                if(e.AddedItems.Count>0)
                {
                    //此处需要重构,不应直接访问DataContext 2017-07-31 william
                    ((SCA.WPF.CreateController.CreateControllerViewModel)this.DataContext).GetDeviceCodeLength((ControllerType)e.AddedItems[0]);      

                }               
                
            }
        }


        private void ControllerTypeAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
