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
        //public static readonly RoutedEvent ControllerTypeSelectedEvent=EventManager.RegisterRoutedEvent(
        //        "ControllerTypeSelected",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(CreateControllerView)
        //    );

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
            if (SerialPortNumberComboBox.SelectedIndex != -1)
            {
                string str = "Selected";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            ControllerModel controller = new ControllerModel();
            controller.Name = ControllerNameInputTextBox.Text;            
            controller.Type = (ControllerType)ControllerTypeComboBox.SelectedItem;
            controller.DeviceAddressLength = Convert.ToInt32(DeviceCodeLengthComboBox.SelectedItem);
            
            controller.MachineNumber = ControllerMachineNumInputTextBox.Text;
            
            controller.PortName = SerialPortNumberComboBox.SelectedItem.ToString();
            controller.LoopAddressLength = 2;//回路地址长度默认为2
            

            ControllerManager controllerManager = new ControllerManager();            
            controllerManager.InitializeAllControllerOperation(null);

            IControllerOperation controllerOperation= controllerManager.GetController(controller.Type);
            controllerOperation.AddControllerToProject(controller);
            RaiseEvent(new RoutedEventArgs(AddButtonClickEvent));
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
                    //RaiseEvent(new RoutedEventArgs(ControllerTypeSelectedEvent,(ControllerType)e.AddedItems[0]));
                    ((SCA.WPF.CreateController.CreateControllerViewModel)this.DataContext).GetDeviceCodeLength((ControllerType)e.AddedItems[0]);
                    //SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject((ControllerType)e.AddedItems[0]);
                    //    short maxLoopAmount = controllerConfig.GetMaxLoopAmountValue();

                    //    for (int i = 1; i <= maxLoopAmount; i++)
                    //    {
                    //        lstLoopsCode.Add(i.ToString().PadLeft(controller.LoopAddressLength, '0'));
                    //    }
                    //}
                    //LoopsCode = lstLoopsCode;
                    //return lstLoopsCode;

                }               
                
            }
            
           // Object o = ((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)e.type).DataItem;
            //List<string> lstLoopsCode = new List<string>();
            //if (ProjectManager.GetInstance.Project != null)
            //{
            //    SCA.Model.ControllerModel controller = ProjectManager.GetInstance.GetPrimaryController();
            //    SCA.Interface.IControllerConfig controllerConfig = ControllerConfigManager.GetConfigObject(controller.TypeCode);
            //    short maxLoopAmount = controllerConfig.GetMaxLoopAmountValue();

            //    for (int i = 1; i <= maxLoopAmount; i++)
            //    {
            //        lstLoopsCode.Add(i.ToString().PadLeft(controller.LoopAddressLength, '0'));
            //    }
            //}
            //LoopsCode = lstLoopsCode;
            //return lstLoopsCode;
        }


        private void ControllerTypeAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
