using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using SCA.WPF.Utility;
using SCA.Interface.BusinessLogic;
using SCA.BusinessLib.BusinessLogic;
using SCA.Model;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/12 13:53:58
* FileName   : DetailInfoGridControl
* Description: 详细信息列表控件
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo
{
    public class DetailInfoGridControl:Microsoft.Windows.Controls.DataGrid
    {
        static DetailInfoGridControl()
        {
            CommandManager.RegisterClassCommandBinding(
                    typeof(DetailInfoGridControl),
                     new CommandBinding(ApplicationCommands.Paste,
                     new ExecutedRoutedEventHandler(OnExecutedPaste),
                     new CanExecuteRoutedEventHandler(OnCanExecutePaste)
                     )
                );
        }       
        
        public static readonly DependencyProperty CanUserPasteToNewRowsProperty =
            DependencyProperty.Register("CanUserPasteToNewRows",
                                        typeof(bool), typeof(DetailInfoGridControl),
                                        new FrameworkPropertyMetadata(true, null, null));
        /// <summary>
        /// 列表类型
        /// </summary>
        public static readonly DependencyProperty DetailTypeProperty =
            DependencyProperty.Register("DetailType",
                                        typeof(Object), typeof(DetailInfoGridControl),
                                        new FrameworkPropertyMetadata(true, null, null));
        /// <summary>
        /// 允许用户向集合中添加新行
        /// </summary>
        public bool CanUserPasteToNewRows
        {
            get { return (bool)GetValue(CanUserPasteToNewRowsProperty); }
            set { SetValue(CanUserPasteToNewRowsProperty, value); }
        }
        /// <summary>
        /// 列表装载内容类型
        /// </summary>
        public Object DetailType
        {
            get { return (Object)GetValue(DetailTypeProperty); }
            set { SetValue(DetailTypeProperty, value); }
        }

        private static void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
        {
            ((DetailInfoGridControl)target).OnCanExecutePaste(args);
        }
        /// <summary>
        /// This virtual method is called when ApplicationCommands.Paste command query its state.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCanExecutePaste(CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = CurrentCell != null;
            args.Handled = true;
        }
        private static void OnExecutedPaste(object target, ExecutedRoutedEventArgs args)
        {
            ((DetailInfoGridControl)target).OnExecutedPaste(args);
        }
        protected virtual void OnExecutedPaste(ExecutedRoutedEventArgs args)
        {
            //Debug.WriteLine("OnExecutedPaste begin");
            // parse the clipboard data            
            object obj = this.DetailType;
            List<string[]> rowData = ClipboardHelper.ParseClipboardData();  
            //bool hasAddedNewRow = false;
            if (rowData != null)
            { 
                // call OnPastingCellClipboardContent for each cell
                if (this.SelectedIndex != -1)
                {
                    object item = null;
                    if (CurrentItem == null)
                    {

                        item = this.Items[this.SelectedIndex];//Added by william at 2017-04-20  工具条中的命令找不至CurrentItem
                    }
                    else
                    {
                        item = CurrentItem;
                    }

                    //int minRowIndex = LoopNameCollection.IndexOf(CurrentItem);
                    int minRowIndex = Items.IndexOf(item);
                    int maxRowIndex = Items.Count - 1;

                    int minColumnDisplayIndex;
                    if (CurrentColumn != null)
                    {
                        minColumnDisplayIndex = (SelectionUnit != DataGridSelectionUnit.FullRow) ? Columns.IndexOf(CurrentColumn) : 0;
                    }
                    else
                    {
                        minColumnDisplayIndex = 0;
                    }


                    int maxColumnDisplayIndex = Columns.Count - 1;
                    int rowDataIndex = 1;
                    for (int i = minRowIndex; i <= maxRowIndex && rowDataIndex < rowData.Count; i++, rowDataIndex++)
                    {
                        #region 将数据粘贴至新增的行中
                        //if (CanUserAddRows)
                        //{

                        //    if (CanUserPasteToNewRows && CanUserAddRows && i == maxRowIndex)
                        //    {
                        //        // add a new row to be pasted to
                        //        ICollectionView cv = CollectionViewSource.GetDefaultView(Items);
                        //        IEditableCollectionView iecv = cv as IEditableCollectionView;
                        //        if (iecv != null)
                        //        {
                        //          //  hasAddedNewRow = true;
                        //            iecv.AddNew();

                        //            if (rowDataIndex + 1 < rowData.Count)
                        //            {
                        //                // still has more items to paste, update the maxRowIndex
                        //                maxRowIndex = Items.Count - 1;
                        //            }
                        //        }
                        //    }
                        //    else if (i == maxRowIndex)
                        //    {
                        //        continue;
                        //    }
                        //}
                        #endregion

                        int columnDataIndex = 0;
                        for (int j = minColumnDisplayIndex; j <= maxColumnDisplayIndex && columnDataIndex < rowData[rowDataIndex].Length; j++, columnDataIndex++)
                        {
                            DataGridColumn column = ColumnFromDisplayIndex(j);
                            if (column.Visibility == Visibility.Visible)
                            {
                                
                                column.OnPastingCellClipboardContent(Items[i], rowData[rowDataIndex][columnDataIndex]);
                            }
                            else
                            {
                                columnDataIndex = columnDataIndex - 1;
                            }
                        }
                        UpdateToModel(this.DetailType, Items[i], rowData[0], rowData[rowDataIndex]);
                    }
                }
                else
                {
                    int rowDataIndex = 1;
                    if (this.SelectedCells != null)  //更新列信息
                    {
                        bool singleColumnFlag = true;//仅允许选择一列进行粘贴
                        if (SelectedCells.Count > 2)
                        {
                            
                            int columnIndex = this.SelectedCells[0].Column.DisplayIndex;
                            DataGridColumn column1 = ColumnFromDisplayIndex(columnIndex);
                            columnIndex = this.SelectedCells[1].Column.DisplayIndex;
                            DataGridColumn column2 = ColumnFromDisplayIndex(columnIndex);
                            if (column1.Header != column2.Header)
                            {
                                singleColumnFlag = false;
                            }
                        }
                        if (singleColumnFlag)
                        { 
                            for(int i=0;i<this.SelectedCells.Count;i++)
                            {
                                object item=this.SelectedCells[i].Item;
                                int columnIndex=this.SelectedCells[i].Column.DisplayIndex;
                                DataGridColumn column=ColumnFromDisplayIndex(columnIndex);                            
                                if (column.Visibility == Visibility.Visible)
                                {
                                    if (column.Header.ToString() == rowData[0][0])//粘贴列与复制列为同一列
                                    {
                                        column.OnPastingCellClipboardContent(item, rowData[rowDataIndex][0]);//固定为1列
                                        UpdateToModel(this.DetailType, item, rowData[0], rowData[rowDataIndex]);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                
                                //((EditableLinkageConfigMixed)this.SelectedCells[0].Item).ID
                            }
                        }
                    }
                }
                


            
                #region 更新选中区域                
                //if (hasAddedNewRow)
                //{
                //    UnselectAll();
                //    UnselectAllCells();

                //    CurrentItem = Items[minRowIndex];

                //    if (SelectionUnit == DataGridSelectionUnit.FullRow)
                //    {
                //        SelectedItem = Items[minRowIndex];
                //    }
                //    else if (SelectionUnit == DataGridSelectionUnit.CellOrRowHeader ||
                //             SelectionUnit == DataGridSelectionUnit.Cell)
                //    {
                //        SelectedCells.Add(new DataGridCellInfo(Items[minRowIndex], Columns[minColumnDisplayIndex]));

                //    }
                //}
                #endregion
            }

        }
        private bool UpdateToModel(object type, object item,string[] columnNames, string[] data)
        {
            try
            {
                GridDetailType detailType = (GridDetailType)type;
                switch (detailType)
                {
                    case GridDetailType.Mixed:
                        {

                            int controllerID = ((EditableLinkageConfigMixed)item).ControllerID;
                            int itemID = ((EditableLinkageConfigMixed)item).ID;
                            ControllerModel controller = SCA.BusinessLib.ProjectManager.GetInstance.Project.Controllers.Find(
                                  delegate(ControllerModel x)
                                  {
                                      return x.ID == controllerID;
                                  }
                                );
                            ILinkageConfigMixedService mixedService = new LinkageConfigMixedService(controller);
                            mixedService.UpdateViaSpecifiedColumnName(itemID, columnNames, data);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
    /// <summary>
    /// 表格显示内容类型
    /// </summary>
    enum GridDetailType
    {
        Device8000,
        Device8003,
        Device8001,
        Device8021,
        Device8036,
        Device8007,
        Device8053,
        Standard,
        Mixed,
        General,
        ManualControlBoard,
    }
}
