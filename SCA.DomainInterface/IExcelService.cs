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
       //private CellStyle _subCaptionStyle; //副标题样式
       EXCELVersion Version{get;}//EXCEL版本
       /// <summary>
       /// 设置标题样式
       /// </summary>
       CellStyle CellCaptionStyle { get; set; }
       /// <summary>
       /// 设置副标题样式
       /// </summary>
       CellStyle CellSubCaptionStyle { get; set; }     
       /// <summary>
       /// 设置数据样式
       /// </summary>
       CellStyle CellDataStyle { get; set; }
       /// <summary>
       /// 设置表头样式
       /// </summary>
       CellStyle CellTableHeadStyle { get; set; }
       
           //  get { return CellSubCaptionStyle; } 
           //set 
           //{ 
           //    _subCaptionStyle = value; 
           //    //_xssfSubCaptionCellStyle = PhaseCellStyle(_subCaptionStyle); 
           //    PhaseCellStyle(_subCaptionStyle,Version);
           //} 
        /// <summary>
        /// 工作薄存储路径
        /// </summary>
        string ExcelPath{get;set;}
       /// <summary>
       /// 行高
       /// </summary>
        short RowHeight { get; set; }
        
        
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
        DataTable OpenExcel(string excelPath, string sheetName, Dictionary<int, int> dictRowsDefinition,out bool sheetExistFlag);
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
        
       /// <summary>
       /// 设置区域名称
       /// </summary>
        void SetRangeName(string rangeName,string formula);        
        /// <summary>
        /// 为页签序列验证
        /// </summary>
        /// <param name="sheetName">目标Sheet</param>
        /// <param name="rangeName">值域名称</param>
        /// <param name="cellRange">应用此验证的范围</param>
        void SetSheetValidationForListConstraint(string sheetName, string rangeName, MergeCellRange cellRange);
        //object PhaseCellStyle(CellStyle paramCellStyle);
    }
    /// <summary>
    /// 单元格样式
    /// </summary>
   public enum CellStyleType { Caption, SubCaption, Data, TableHead, None };
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
    
             
    
    public enum EXCELVersion
    { 
        
        EXCEL2003=0,
        EXCEL2007=1
    }
    /// <summary>
    /// 自定义样式类
    /// </summary>
    public class CellStyle
    {
        /// <summary>
        /// 字体
        /// </summary>
        public enum FontNameValue
        {
            黑体,
            宋体,
            //[EnumMember(Value = "Times New Roman")]
            //System.Runtime.Serialization.
            TimesNewRoman
        };

        /// <summary>
        /// 设置字体加粗值
        /// </summary>
        private bool fontBoldFlag = false;
        public bool FontBoldFlag { get { return fontBoldFlag; } set { fontBoldFlag = value; } }
        public enum HorizontalAlignmentValue { Center, Left, Right };
        public enum VerticalAlignmentValue { Center, Top, Bottom };
        public enum BorderStyleValue { Thin, None };
        public enum BorderColorValue { Black, None };
        public FontNameValue FontName { set; get; }
        public short FontHeightInPoints { get; set; }

        public BorderStyleValue BorderStyle { set; get; }
        public HorizontalAlignmentValue HorizontalAlignment { set; get; }
        public VerticalAlignmentValue VerticalAlignment { set; get; }
        public BorderColorValue BorderColor { get; set; }
        public bool WrapText { get; set; }
    }
}