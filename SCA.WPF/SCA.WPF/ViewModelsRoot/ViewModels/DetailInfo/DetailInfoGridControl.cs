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
        /// <summary>
        ///     DependencyProperty for CanUserAddRows.
        /// </summary>
        public static readonly DependencyProperty CanUserPasteToNewRowsProperty =
            DependencyProperty.Register("CanUserPasteToNewRows",
                                        typeof(bool), typeof(DetailInfoGridControl),
                                        new FrameworkPropertyMetadata(true, null, null));
        /// <summary>
        ///     Whether the end-user can add new rows to the ItemsSource.
        /// </summary>
        public bool CanUserPasteToNewRows
        {
            get { return (bool)GetValue(CanUserPasteToNewRowsProperty); }
            set { SetValue(CanUserPasteToNewRowsProperty, value); }
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
            List<string[]> rowData = ClipboardHelper.ParseClipboardData();  
            bool hasAddedNewRow = false;
            if (rowData != null)
            { 
                // call OnPastingCellClipboardContent for each cell
                object item ;
                if(CurrentItem==null)
                {
                    item = this.Items[this.SelectedIndex];//Added by william at 2017-04-20  工具条中的命令找不至CurrentItem
                }
                else
                {
                    item = CurrentItem;
                }
                
                //int minRowIndex = Items.IndexOf(CurrentItem);
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
                int rowDataIndex = 0;
                for (int i = minRowIndex; i <= maxRowIndex && rowDataIndex < rowData.Count; i++, rowDataIndex++)
                {
                    #region 将数据粘贴至新增的行中
                    if (CanUserPasteToNewRows && CanUserAddRows && i == maxRowIndex)
                    {
                        // add a new row to be pasted to
                        ICollectionView cv = CollectionViewSource.GetDefaultView(Items);
                        IEditableCollectionView iecv = cv as IEditableCollectionView;
                        if (iecv != null)
                        {
                            hasAddedNewRow = true;
                            iecv.AddNew();

                            if (rowDataIndex + 1 < rowData.Count)
                            {
                                // still has more items to paste, update the maxRowIndex
                                maxRowIndex = Items.Count - 1;
                            }
                        }
                    }
                    else if (i == maxRowIndex)
                    {
                        continue;
                    }
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
                }
            
                #region 更新选中区域
                // update selection
                if (hasAddedNewRow)
                {
                    UnselectAll();
                    UnselectAllCells();

                    CurrentItem = Items[minRowIndex];

                    if (SelectionUnit == DataGridSelectionUnit.FullRow)
                    {
                        SelectedItem = Items[minRowIndex];
                    }
                    else if (SelectionUnit == DataGridSelectionUnit.CellOrRowHeader ||
                             SelectionUnit == DataGridSelectionUnit.Cell)
                    {
                        SelectedCells.Add(new DataGridCellInfo(Items[minRowIndex], Columns[minColumnDisplayIndex]));

                    }
                }
                #endregion
            }

        }

    }
}
