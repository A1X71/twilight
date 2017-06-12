using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace SCA.Interface
{
    /// <summary>
    /// Author: William
    /// Created Date: 2016-10-20
    /// Description: 定义EXCEL操作接口
    /// </summary>
   public  interface IExcelService:IDisposable
    {
        /// <summary>
        /// 工作薄存储路径
        /// </summary>
        string ExcelPath{get;set;}
       /// <summary>
       /// 行高
       /// </summary>
        short RowHeight { get; set; }
        
        /// <summary>
        /// 返回工作薄内指定的Sheet页数据
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="lstSheetNames">工作表名称</param>
        /// <returns></returns>
        DataSet OpenExcel(string excelPath, List<string> lstSheetNames);
        /// <summary>
        /// 打开EXCEL
        /// </summary>
        /// <param name="excelPath">工作薄路径</param>
        /// <returns></returns>
        DataTable OpenExcel(string excelPath);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="excelPath"></param>
       /// <param name="sheetName"></param>
       /// <param name="dictRowsDefinition">读取的数据表的行定义</param>
       /// <returns></returns>
        DataTable OpenExcel(string excelPath, string sheetName, Dictionary<int, int> dictRowsDefinition);
        /// <summary>
        /// 打开EXCEL
        /// </summary>
        /// <returns></returns>
        DataTable OpenExcel();
        /// <summary>
        /// 保存工作薄
        /// </summary>
        /// <returns></returns>
        bool SaveToFile();

        void CreateExcelSheets(List<string> lstSheetNames);
        #region 样式操作        
        /// <summary>
        /// 设置列宽度
        /// </summary>
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="intColumnIndex">列序号</param>
        /// <param name="sngColumnWidth">列宽</param>
         void SetColumnWidth(string strSheetName, int intColumnIndex, Single sngColumnWidth);
        /// <summary>
        /// 设置单元格合并参数
        /// </summary>
        /// <param name="strSheetName">工作表名称</param>
        /// <param name="lstMergeCellRange">单元格合并参数</param>
        void SetMergeCells(string strSheetName, List<MergeCellRange> lstMergeCellRange);
        #endregion
        /// <summary>
        /// 设置单元格数据
        /// </summary>
        /// <param name="strSheetName">需设置的sheet名称</param>
        /// <param name="intRowNumber">需设置的行号</param>
        /// <param name="intCellNumber">需设置的单元格序号</param>
        /// <param name="strCellValue">数据值</param>
        /// <param name="paramCellStyle">单元格样式</param>
        void SetValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, CellStyleType styleType);
        /// <summary>
        /// 设置单元格数据
        /// </summary>
        /// <param name="strSheetName">需设置的sheet名称</param>
        /// <param name="intRowNumber">需设置的行号</param>
        /// <param name="intCellNumber">需设置的单元格序号</param>
        /// <param name="strCellValue">数据值</param>
        /// <param name="paramCellStyle">单元格样式</param>
        void SetCellValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, CellStyleType styleType);
        
    }
    /// <summary>
    /// 单元格样式
    /// </summary>
    public enum CellStyleType { Caption, SubCaption, Data, None };
    /// <summary>
        /// 单元格合并信息
        /// </summary>
    public class MergeCellRange
        {
            /// <summary>
            /// 起始行号
            /// </summary>
            public int FirstRowIndex { get; set; }
            /// <summary>
            /// 终止行号
            /// </summary>
            public int LastRowIndex { get; set; }
            /// <summary>
            /// 起始列号
            /// </summary>
            public int FirstColumnIndex { get; set; }
            /// <summary>
            /// 终始列号
            /// </summary>
            public int LastColumnIndex { get; set; }
        }
}