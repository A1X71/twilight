using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using System.Windows;
using System.Windows.Data;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/28 19:20:45
* FileName   : DeviceDataGrid
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.DataGrid
{
    public class DeviceDataGrid:Microsoft.Windows.Controls.DataGrid
    {
        static DeviceDataGrid()
        {
            CommandManager.RegisterClassCommandBinding(
                 typeof(DeviceDataGrid),
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
                                        typeof(bool), typeof(DeviceDataGrid),
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
            ((DeviceDataGrid)target).OnCanExecutePaste(args);
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
            ((DeviceDataGrid)target).OnExecutedPaste(args);
        }
        protected virtual void OnExecutedPaste(ExecutedRoutedEventArgs args)
        {
//            Debug.WriteLine("OnExecutedPaste begin");

            // parse the clipboard data            
            List<string[]> rowData = new List<string[]>();// ClipboardHelper.ParseClipboardData();
            string[] color ={ "red", "black", "yellow" };
            rowData.Add(color);
            bool hasAddedNewRow = false;

            // call OnPastingCellClipboardContent for each cell
            int minRowIndex = Items.IndexOf(CurrentItem);
            int maxRowIndex = Items.Count - 1;
            int minColumnDisplayIndex = (SelectionUnit != DataGridSelectionUnit.FullRow) ? Columns.IndexOf(CurrentColumn) : 0;
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
                for (int j = minColumnDisplayIndex; j < maxColumnDisplayIndex && columnDataIndex < rowData[rowDataIndex].Length; j++, columnDataIndex++)
                {
                    DataGridColumn column = ColumnFromDisplayIndex(j);
                    column.OnPastingCellClipboardContent(Items[i], rowData[rowDataIndex][columnDataIndex]);
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
