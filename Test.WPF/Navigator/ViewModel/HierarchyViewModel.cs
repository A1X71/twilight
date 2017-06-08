using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows.Data;
using SCA.Model;
using System.Windows.Controls;
using System.Windows;
using SCA.BusinessLib.BusinessLogic;
using SCA.Interface;
using Test.WPF.Utility;
using System.Windows.Input;
using SCA.Interface.DatabaseAccess;
using SCA.BusinessLib;
using SCA.BusinessLib.Utility;
using SCA.DatabaseAccess.DBContext;
using SCA.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 16:02:43
* FileName   : HierarchyViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.Navigator.ViewModel
{
    public class HierarchyViewModel 
    {
        public CollectionView Projects { get; private set; }
        private HierarchyItemViewModel _selectedItem;

        public HierarchyViewModel()
        {
            IProjectManager projManager = ProjectManager.GetInstance;
            IFileService _fileService = new FileService();
            IDatabaseService _databaseService = new MSAccessDatabaseAccess(@"C:\Users\Administrator\Desktop\foo\8036.mdb", null, _fileService);
            IOldVersionSoftwareDBService oldVersionService = new OldVersionSoftware8036DBService(_databaseService);
            IControllerOperation controllerOperation = null;

            string[] strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            ControllerModel controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8036":
                        controllerOperation = new ControllerOperation8036(_databaseService);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }


            ProjectModel proj = new ProjectModel() { ID = 1, Name = "尼特智能" };
            proj.Controllers.Add(controllerInfo);



            _databaseService = new MSAccessDatabaseAccess(@"E:\2016\6 软件优化升级\4 实际工程数据\工程数据111\连城心怡都城_文件版本5.mdb", null, _fileService);
            oldVersionService = new OldVersionSoftware8001DBService(_databaseService);

            strFileInfo = oldVersionService.GetFileVersionAndControllerType();
            controllerInfo = null;
            if (strFileInfo.Length > 0)
            {
                switch (strFileInfo[0])
                {
                    case "8001":
                        controllerOperation = new ControllerOperation8001(null);
                        break;
                }
                if (controllerOperation != null)
                {
                    controllerInfo = controllerOperation.OrganizeControllerInfoFromOldVersionSoftwareDataFile(oldVersionService);
                }
                //strFileInfo[1];
            }

            proj.Controllers.Add(controllerInfo);

            projManager.CreateProject(proj);

            List<ProjectModel> lstProjects = new List<ProjectModel>();
            lstProjects.Add(proj);
            Initialize(lstProjects, null);
        }
        public HierarchyViewModel(List<ProjectModel> projects, object selectedEntity)
        {
            Initialize(projects, selectedEntity);
        }
        public void Initialize(List<ProjectModel> projects, object selectedEntity)
        {
            // create the top level collectionview for the customers
            var projectHierarchyItemsList = new List<HierarchyItemViewModel>();

            foreach (var p in projects)
            {
                // create the hierarchy item and add to the list
                var projectHierarchyItem = new HierarchyItemViewModel(p);

                projectHierarchyItemsList.Add(projectHierarchyItem);

                // check if this is the selected item
                if (selectedEntity != null && selectedEntity.GetType() == typeof(ProjectModel) && (selectedEntity as ProjectModel).Equals(p))
                {
                    _selectedItem = projectHierarchyItem;
                }

                // if there are any orders in projectHierarchyItem
                if (p.Controllers.Count != 0)
                {
                    // create a new list of HierarchyItems
                    var controllerHierarchyItemsList = new List<HierarchyItemViewModel>();
                    // loop through the orders and add them
                    foreach (var c in p.Controllers)
                    {
                        // create the hierarchy item and add to the list
                        var controllerHierarchyItem = new HierarchyItemViewModel(c);
                        controllerHierarchyItem.Parent = projectHierarchyItem;
                        controllerHierarchyItemsList.Add(controllerHierarchyItem);

                        // check if this is the selected item
                        if (selectedEntity != null && selectedEntity.GetType() == typeof(ControllerModel) && (selectedEntity as ControllerModel).Equals(c))
                        {
                            _selectedItem = controllerHierarchyItem;
                        }


                        IControllerConfig config = ControllerConfigManager.GetConfigObject(c.Type);
                        ControllerNodeModel[] nodeModel = config.GetNodes();
                        if (nodeModel.Length != 0)
                        {
                            var controllerNodeHierarchyItemList = new List<HierarchyItemViewModel>();
                            foreach (var cNode in nodeModel)
                            {
                                var nodeHierarchyItem = new HierarchyItemViewModel(cNode);
                                nodeHierarchyItem.Parent = controllerHierarchyItem;
                                controllerNodeHierarchyItemList.Add(nodeHierarchyItem);
                                if (selectedEntity != null && selectedEntity.GetType() == typeof(ControllerNodeModel) && (selectedEntity as ControllerNodeModel).Equals(cNode))
                                {
                                    _selectedItem = controllerHierarchyItem;
                                }
                                if (cNode.Type == ControllerNodeType.Loop) //回路数据应该加载回路信息
                                {
                                    if (c.Loops.Count > 0)  //加载回路数据
                                    {
                                        var loopHierarchyItemList = new List<HierarchyItemViewModel>();
                                        foreach (var l in c.Loops)
                                        {
                                            var loopHierarchyItem = new HierarchyItemViewModel(l);
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
                    // create the children of the customer
                    projectHierarchyItem.Children = new CollectionView(controllerHierarchyItemsList);
                }
            }

            this.Projects = new CollectionView(projectHierarchyItemsList);

            // select the selected item and expand it'type parents
            if (_selectedItem != null)
            {
                _selectedItem.IsSelected = true;
                HierarchyItemViewModel current = _selectedItem.Parent;

                while (current != null)
                {
                    current.IsExpanded = true;
                    current = current.Parent;
                }
            }
        }
        private List<HierarchyItemViewModel> BuildHierarchyList<T>(List<T> sourceList, Func<T, T, bool> isSelected, object selectedEntity, Func<T, List<HierarchyItemViewModel>> getChildren)
        {
            List<HierarchyItemViewModel> result = new List<HierarchyItemViewModel>();

            foreach (T item in sourceList)
            {
                // create the hierarchy item and add to the list
                var hierarchyItem = new HierarchyItemViewModel(item);
                result.Add(hierarchyItem);

                // check if this is the selected item
                if (selectedEntity != null && selectedEntity.GetType() == typeof(T) && (isSelected.Invoke(item, (T)selectedEntity)))
                {
                    _selectedItem = hierarchyItem;
                }

                if (getChildren != null)
                {
                    var children = getChildren.Invoke(item);
                    children.ForEach(x => x.Parent = hierarchyItem);
                    hierarchyItem.Children = new CollectionView(children);
                }
            }

            return result;
        }

        #region Command
        public ICommand GetDetails
        {
            get
            {
                return new RelayCommand(GetDetailsExecute, CanGetDetailsExecute);
            }
        }
        public void GetDetailsExecute()
        {
            HierarchyItemViewModel o = this._selectedItem;
           // this.lstMessages.Items.Add(message);
        }
        public bool CanGetDetailsExecute()
        {
            return true;
        }
        #endregion
    }
    public class HierarchyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate retval = null;
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is HierarchyItemViewModel)
            {
                HierarchyItemViewModel hierarchyItem = item as HierarchyItemViewModel;
                if (hierarchyItem.DataItem != null)
                {

                    if (hierarchyItem.DataItem.GetType() == typeof(ProjectModel))
                    {
                        retval = element.FindResource("ProjectTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(ControllerModel))
                    {
                        retval = element.FindResource("ControllerTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(ControllerNodeModel))
                    {
                        retval = element.FindResource("ControllerNodeTemplate") as DataTemplate;
                    }
                    else if (hierarchyItem.DataItem.GetType() == typeof(LoopModel))
                    {
                        retval = element.FindResource("LoopTemplate") as DataTemplate;
                    }
                }
            }

            return retval;
        }
    }
}
