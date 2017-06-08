using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Reflection;
using Test.WPF.Utility;
using System.Windows.Data;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/1 15:27:35
* FileName   : HierarchyItemViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.TreeView.ViewModel
{
    public class HierarchyItemViewModel:PropertyChangedBase
    {
        private CollectionView _children;
        private bool _isExpanded;
        private bool _isSelected;

        public HierarchyItemViewModel(object dataItem)
        {
            DataItem = dataItem;
        }

        public object DataItem { get; private set; }

        public HierarchyItemViewModel Parent { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }
        }

        public System.Windows.Data.CollectionView Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
