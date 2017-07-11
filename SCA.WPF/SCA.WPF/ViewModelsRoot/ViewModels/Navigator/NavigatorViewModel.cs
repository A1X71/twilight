using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SCA.Model;
using SCA.Interface;
using SCA.BusinessLib.BusinessLogic;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 9:19:18
* FileName   : NavigatorViewModel
* Description: 组织导航树内容
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.Navigator
{
    public class NavigatorViewModel
    {
        public CollectionView Projects { get; private set; }
        private NavigatorItemViewModel _selectedItem;
        private string _projectAddIconPath = @"Resources/Icon/Style1/project-add.png";
        private string _projectOpenIconPath = @"Resources/Icon/Style1/project-open.png";
        private string _projectDeleteIconPath = @"Resources/Icon/Style1/project-delete.png";
        private string _exportExcelIconPath = @"Resources/Icon/Style1/export-excel.png";
        private string _projectMergeIconPath = @"Resources/Icon/Style1/project-merge.png";
        private string _controllerAddIconPath = @"Resources/Icon/Style1/controller-add.png";
        private string _deviceTypeIconPath = @"Resources/Icon/Style1/device-type.png";
        private string _controllerDeleteIconPath = @"Resources/Icon/Style1/controller-delete.png";
        private string _controllerStartIconPath = @"Resources/Icon/Style1/controller-start.png";
        private string _controllerStopIconPath = @"Resources/Icon/Style1/controller-stop.png";
        private string _controllerSetMasterIconPath = @"Resources/Icon/Style1/controller-set-master.png";
        private string _loopAddIconPath = @"Resources/Icon/Style1/loop-add.png";
        private string _loopDeleteIconPath = @"Resources/Icon/Style1/loop-delete.png";        
        private string _appCurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public string ProjectAddIconPath { get { return _appCurrentPath + _projectAddIconPath; } set { _projectAddIconPath = value; } }
        public string ProjectOpenIconPath { get { return _appCurrentPath + _projectOpenIconPath; } }
        public string ProjectDeleteIconPath { get { return _appCurrentPath + _projectDeleteIconPath; } }
        public string ExportExcelIconPath { get { return _appCurrentPath + _exportExcelIconPath; } }
        public string ProjectMergeIconPath { get { return _appCurrentPath + _projectMergeIconPath; } }
        public string ControllerAddIconPath { get { return _appCurrentPath + _controllerAddIconPath; } }
        public string DeviceTypeIconPath { get { return _appCurrentPath + _deviceTypeIconPath; } }
        public string ControllerDeleteIconPath { get { return _appCurrentPath + _controllerDeleteIconPath; } }
        public string ControllerStartIconPath { get { return _appCurrentPath + _controllerStartIconPath; } }
        public string ControllerStopIconPath { get { return _appCurrentPath + _controllerStopIconPath; } }
        public string ControllerSetMasterIconPath { get { return _appCurrentPath + _controllerSetMasterIconPath; } }
        public string LoopAddIconPath { get { return _appCurrentPath + _loopAddIconPath; } }
        public string LoopDeleteIconPath { get { return _appCurrentPath + _loopDeleteIconPath; } }

        

        public NavigatorViewModel(List<ProjectModel> projects, object selectedEntity)
        {
            if (projects != null)
            {
                Initialize(projects, projects[0]);
            }
            else
            {
                Projects = null;
            }
        }
        public void Initialize(List<ProjectModel> projects, object selectedEntity)
        {
            // 项目级别信息集合
            var projectHierarchyItemsList = new List<NavigatorItemViewModel>();

            foreach (var p in projects)
            {
                // 为项目信息创建导航节点
                var projectHierarchyItem = new NavigatorItemViewModel(p);
                projectHierarchyItemsList.Add(projectHierarchyItem);

                // 判断当前节点是否为选中节点
                if (selectedEntity != null && selectedEntity.GetType() == typeof(ProjectModel) && (selectedEntity as ProjectModel).Equals(p))
                {
                    _selectedItem = projectHierarchyItem;
                }

                // 判断项目下是否有控制器节点
                if (p.Controllers.Count != 0)
                {
                    // 创建“控制器”导航节点
                    var controllerHierarchyItemsList = new List<NavigatorItemViewModel>();
                    
                    foreach (var c in p.Controllers)
                    {
                        // 为“控制器信息”创建导航节点
                        var controllerHierarchyItem = new NavigatorItemViewModel(c);
                        controllerHierarchyItem.Parent = projectHierarchyItem;
                        controllerHierarchyItemsList.Add(controllerHierarchyItem);

                        // 判断当前节点是否为选中节点
                        if (selectedEntity != null && selectedEntity.GetType() == typeof(ControllerModel) && (selectedEntity as ControllerModel).Equals(c))
                        {
                            _selectedItem = controllerHierarchyItem;
                        }
                        #region 根据控制器类型获取配置的节点
                        IControllerConfig config = ControllerConfigManager.GetConfigObject(c.Type);
                        ControllerNodeModel[] nodeModel = config.GetNodes();
                        #endregion 
                        if (nodeModel.Length != 0)
                        {
                            
                            var controllerNodeHierarchyItemList = new List<NavigatorItemViewModel>();
                            foreach (var cNode in nodeModel)
                            {
                                //为“控制器节点类型”创建导航节点
                                var nodeHierarchyItem = new NavigatorItemViewModel(cNode);
                                nodeHierarchyItem.Parent = controllerHierarchyItem;
                                controllerNodeHierarchyItemList.Add(nodeHierarchyItem);
                                if (selectedEntity != null && selectedEntity.GetType() == typeof(ControllerNodeModel) && (selectedEntity as ControllerNodeModel).Equals(cNode))
                                {
                                    _selectedItem = controllerHierarchyItem;
                                }
                                //如果节点类型为“回路”，需要为其增加“回路号”导航节点
                                if (cNode.Type == ControllerNodeType.Loop) //回路数据应该加载回路信息
                                {
                                    if (c.Loops.Count > 0)  //加载回路数据
                                    {
                                        var loopHierarchyItemList = new List<NavigatorItemViewModel>();
                                        foreach (var l in c.Loops)
                                        {
                                            //为回路信息创建导航节点
                                            var loopHierarchyItem = new NavigatorItemViewModel(l);
                                            loopHierarchyItem.Parent = nodeHierarchyItem;
                                            loopHierarchyItemList.Add(loopHierarchyItem);
                                            if (selectedEntity != null && selectedEntity.GetType() == typeof(LoopModel) && (selectedEntity as LoopModel).Equals(l))
                                            {
                                                _selectedItem = loopHierarchyItem;
                                            }
                                        }                                        
                                        nodeHierarchyItem.Children = new CollectionView(loopHierarchyItemList);
                                    }
                                }
                            }
                            controllerHierarchyItem.Children = new CollectionView(controllerNodeHierarchyItemList);
                        }
                    }                    
                    projectHierarchyItem.Children = new CollectionView(controllerHierarchyItemsList);
                }
            }

            this.Projects = new CollectionView(projectHierarchyItemsList);

            // 设置选中节点状态，并展开当前选中节点
            if (_selectedItem != null)
            {
                _selectedItem.IsSelected = true;
                NavigatorItemViewModel current = _selectedItem.Parent;

                while (current != null)
                {
                    current.IsExpanded = true;
                    current = current.Parent;
                }
            }
        }

        public void UpdateControllerInfo(ControllerModel controller)
        {


            List<ProjectModel> lst = new List<ProjectModel>();
            ProjectModel p = (ProjectModel)((NavigatorItemViewModel)Projects.CurrentItem).DataItem;
            var result = from v in p.Controllers where v.ID == controller.ID select v;
            ControllerModel c = (ControllerModel)result.FirstOrDefault();
            c = controller;
            lst.Add(p);
            foreach (var s in Projects)
            { 
                
            }
            //Initialize(lst, null);
          
        }
        
    }
}
