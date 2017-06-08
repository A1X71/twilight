using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/28 11:38:39
* FileName   : ContentWithSummaryViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModels
{
          
    

    public class ContentWithSummaryViewModel:TreeView
    {
        //这两个默认的是TreeViewItem
        protected override DependencyObject GetContainerForItemOverride()//创建或标识用于显示指定项的元素。 
        {
            return new ContentWithSummaryItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)//确定指定项是否是（或可作为）其自己的 ItemContainer
        {
            //return item is TreeListViewItem;
            bool _isTreeLVI = item is ContentWithSummaryItem;
            return _isTreeLVI;
        }
    }

                 
    public class ContentWithSummaryItem : TreeViewItem
    {
        /// <summary>
        /// hierarchy 
        /// </summary>
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    ContentWithSummaryItem parent =
                        ItemsControl.ItemsControlFromItemContainer(this) as ContentWithSummaryItem;//返回拥有指定的容器元素中 ItemsControl 。 
                    _level = (parent != null) ? parent.Level + 1 : 0;
                }
                return _level;
            }
        }


        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentWithSummaryItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            //return item is TreeListViewItem;
            bool _isITV = item is ContentWithSummaryItem;
            return _isITV;
        }

        private int _level = -1;
    }
}
