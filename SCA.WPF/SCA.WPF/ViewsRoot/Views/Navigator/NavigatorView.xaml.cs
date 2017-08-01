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
namespace SCA.WPF.ViewsRoot.Views.Navigator
{
    /// <summary>
    /// NavigatorView.xaml 的交互逻辑
    /// </summary>
    public partial class NavigatorView : UserControl
    {
        public NavigatorView()
        {
            InitializeComponent();
        }
        #region Empty Toolbox event
        public static readonly RoutedEvent NewProjectButtonClickEvent = EventManager.RegisterRoutedEvent("NewProjectButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        public event RoutedEventHandler NewProjectButtonClick
        {
            add { AddHandler(NewProjectButtonClickEvent, value); }
            remove { RemoveHandler(NewProjectButtonClickEvent, value); }
        }

        public static readonly RoutedEvent OpenProjectButtonClickEvent = EventManager.RegisterRoutedEvent("OpenProjectButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        public event RoutedEventHandler OpenProjectButtonClick
        {
            add { AddHandler(OpenProjectButtonClickEvent, value); }
            remove { RemoveHandler(OpenProjectButtonClickEvent, value); }
        }

        public static readonly RoutedEvent CloseProjectButtonClickEvent = EventManager.RegisterRoutedEvent("CloseProjectButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        public event RoutedEventHandler CloseProjectButtonClick
        {
            add { AddHandler(CloseProjectButtonClickEvent, value); }
            remove { RemoveHandler(CloseProjectButtonClickEvent, value); }
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewProjectButtonClickEvent));
        }

        private void btnOpenProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenProjectButtonClickEvent));
        }        
        private void btnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseProjectButtonClickEvent));
        }


        #endregion
        #region Project Toolbox event
        public static readonly RoutedEvent AddControllerButtonClickEvent = EventManager.RegisterRoutedEvent("AddControllerButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        public event RoutedEventHandler AddControllerButtonClick
        {
            add { AddHandler(AddControllerButtonClickEvent, value); }
            remove { RemoveHandler(AddControllerButtonClickEvent, value); }
        }




        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddController_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddControllerButtonClickEvent));
        }

        private void btnDeviceType_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #region Controller Toolbox event
        
        // 删除控制器信息  
        public static readonly RoutedEvent DeleteControllerButtonClickEvent = EventManager.RegisterRoutedEvent("DeleteControllerButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        //启动控制器串口通迅
        public static readonly RoutedEvent StartControllerButtonClickEvent = EventManager.RegisterRoutedEvent("StartControllerButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        //关闭控制器串口通迅
        public static readonly RoutedEvent StopControllerButtonClickEvent = EventManager.RegisterRoutedEvent("StopControllerButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        /// <summary>
        /// 控制器级别合并按钮事件
        /// </summary>
        public static readonly RoutedEvent MergeButtonForControllerClickEvent = EventManager.RegisterRoutedEvent("MergeButtonForControllerClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));        

        public event RoutedEventHandler DeleteControllerButtonClick
        {
            add { AddHandler(DeleteControllerButtonClickEvent, value); }
            remove { RemoveHandler(DeleteControllerButtonClickEvent, value); }
        }
        public event RoutedEventHandler StartControllerButtonClick
        {
            add { AddHandler(StartControllerButtonClickEvent,value); }
            remove { RemoveHandler(StartControllerButtonClickEvent, value); }
        }
        public event RoutedEventHandler StopControllerButtonClick
        {
            add { AddHandler(StopControllerButtonClickEvent, value); }
            remove { RemoveHandler(StopControllerButtonClickEvent, value); }
        }
        public event RoutedEventHandler MergeButtonForControllerClick
        {
            add { AddHandler(MergeButtonForControllerClickEvent, value); }
            remove { RemoveHandler(MergeButtonForControllerClickEvent, value); }
        }

        private void btnCloseProjectInProjectToolStrip_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseProjectButtonClickEvent));
            ToolBoxVisibility(EmptyContentTool);
        }
        private void btnDeleteController_Click(object sender, RoutedEventArgs e)
        {            
            RaiseEvent(new RoutedEventArgs(DeleteControllerButtonClickEvent,this.HierarchyTreeView.SelectedItem));
        }
        private void btnStartController_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(StartControllerButtonClickEvent, this.HierarchyTreeView.SelectedItem));
        }
        private void btnStopController_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(StopControllerButtonClickEvent, this.HierarchyTreeView.SelectedItem));
        }
        private void btnSetMasterController_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        #region Loop Toolbox event
        //添加回路
        public static readonly RoutedEvent AddLoopButtonClickEvent = EventManager.RegisterRoutedEvent("AddLoopButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));        
        public event RoutedEventHandler  AddLoopButtonClick
        {
            add { AddHandler(AddLoopButtonClickEvent, value); }
            remove { RemoveHandler(AddLoopButtonClickEvent, value); }
        }

        private void btnAddLoop_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddLoopButtonClickEvent, ((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)this.HierarchyTreeView.SelectedItem).Parent.DataItem));
        }
        //删除回路
        public static readonly RoutedEvent DeleteLoopButtonClickEvent = EventManager.RegisterRoutedEvent("DeleteLoopButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView));
        public event RoutedEventHandler DeleteLoopButtonClick
        {
            add { AddHandler(DeleteLoopButtonClickEvent, value); }
            remove { RemoveHandler(DeleteLoopButtonClickEvent, value); }
        }        
        private void btnDelLoop_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DeleteLoopButtonClickEvent, ((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)this.HierarchyTreeView.SelectedItem).DataItem));
        }
        #endregion 
        #region TreeNode Event
        public static readonly RoutedEvent ControllerNodeClickEvent = EventManager.RegisterRoutedEvent(
             "ControllerNodeClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView)
             );
        /// <summary>
        /// 总回路节点下的单独回路节点“单击事件”
        /// </summary>
        public static readonly RoutedEvent LoopItemClickEvent = EventManager.RegisterRoutedEvent(
            "LoopItemClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView)
            );

        ///// <summary>
        ///// 控制器下总回路节点“单击事件”
        ///// </summary>
        //public static readonly RoutedEvent MainLoopNodeClickEvent = EventManager.RegisterRoutedEvent(
        //    "MainLoopNodeClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView)
        //    );
        public event RoutedEventHandler ControllerNodeClick
            {
                add    { AddHandler(ControllerNodeClickEvent,value); }
                remove { RemoveHandler(ControllerNodeClickEvent,value); }
            }
        public event RoutedEventHandler LoopItemClick
            {
                add { AddHandler(LoopItemClickEvent, value); }
                remove { RemoveHandler(LoopItemClickEvent, value); }
            }
        //public event RoutedEventHandler MainLoopNodeClick
        //{
        //    add { AddHandler(MainLoopNodeClickEvent, value); }
        //    remove { RemoveHandler(MainLoopNodeClickEvent, value); }
        //}

        public static readonly RoutedEvent ControllerClickEvent = EventManager.RegisterRoutedEvent(
            "ControllerClick",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(NavigatorView)
            );
        public event RoutedEventHandler ControllerClick
        {
            add { AddHandler(ControllerClickEvent, value); }
            remove { RemoveHandler(ControllerClickEvent, value); }
        }
        public static readonly RoutedEvent ProjectClickEvent = EventManager.RegisterRoutedEvent(
            "ProjectClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigatorView)
            );
        public event RoutedEventHandler ProjectClick
        {
            add { AddHandler(ProjectClickEvent, value); }
            remove { RemoveHandler(ProjectClickEvent, value); }
        }
        #endregion
        /// <summary>
        /// TreeView 切换选中项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                if (e.NewValue.GetType() == typeof(SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel))
                { 
                    Object o=((SCA.WPF.ViewModelsRoot.ViewModels.Navigator.NavigatorItemViewModel)e.NewValue).DataItem;
                    if (o.GetType() == typeof(SCA.Model.ControllerNodeModel))
                    {
                        switch(((ControllerNodeModel)o).Type)
                        {
                            case  ControllerNodeType.Loop:
                                ToolBoxVisibility(LoopsTool);
                                break;
                            case ControllerNodeType.Standard:
                                ToolBoxVisibility(StandardLinkageTool);
                                break;
                            case ControllerNodeType.Mixed:
                                ToolBoxVisibility(MixedLinkageTool);
                                break;
                            case ControllerNodeType.General:
                                ToolBoxVisibility(GeneralLinkageTool);
                                break;
                            case ControllerNodeType.Board:
                                ToolBoxVisibility(ManualControlBoardTool);
                                break;   
                        }
                        RaiseEvent(new RoutedEventArgs(ControllerNodeClickEvent, e.NewValue));
                    }            
                    else if (o.GetType() == typeof(SCA.Model.ControllerModel))
                    {
                        ToolBoxVisibility(ControllerTool);
                        RaiseEvent(new RoutedEventArgs(ControllerClickEvent,e.NewValue));
                    }
                    else if (o.GetType() == typeof(SCA.Model.ProjectModel))
                    {
                        ToolBoxVisibility(ProjectTool); 
                        RaiseEvent(new RoutedEventArgs(ProjectClickEvent, e.NewValue));
                    }
                    else if (o.GetType() == typeof(SCA.Model.LoopModel))
                    {
                        ToolBoxVisibility(SingleLoopTool);
                        RaiseEvent(new RoutedEventArgs(LoopItemClickEvent, e.NewValue));
                    }
            }
          }
        }
        /// <summary>
        /// 设置工具栏的可见性
        /// </summary>
        /// <param name="element"></param>
        private void ToolBoxVisibility(UIElement element)
        {
            element.Visibility = Visibility.Visible;
            switch (((Grid)element).Name)
            {
                case "EmptyContentTool":                    
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "ProjectTool":                    
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "ControllerTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "LoopsTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "StandardLinkageTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "MixedLinkageTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;                    
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "GeneralLinkageTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;                    
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "ManualControlBoardTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    SingleLoopTool.Visibility = Visibility.Collapsed;
                    break;
                case "SingleLoopTool":
                    EmptyContentTool.Visibility = Visibility.Collapsed;
                    ProjectTool.Visibility = Visibility.Collapsed;
                    ControllerTool.Visibility = Visibility.Collapsed;
                    LoopsTool.Visibility = Visibility.Collapsed;
                    StandardLinkageTool.Visibility = Visibility.Collapsed;
                    MixedLinkageTool.Visibility = Visibility.Collapsed;
                    GeneralLinkageTool.Visibility = Visibility.Collapsed;
                    ManualControlBoardTool.Visibility = Visibility.Collapsed;
                    break;
            }

        }

        private void btnControllerMerge_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MergeButtonForControllerClickEvent,this.HierarchyTreeView.SelectedItem));            
        }
      
    }
}
