using System;
using SCA.Interface;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using SCA.Interface;
using System.IO;
using System.Data;
using System.Collections.Generic;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/6 15:33:05
* FileName   : ExcelService
* Description: EXCEL操作服务类
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public  class ExcelService:IExcelService
    {
        private IFileService _fileService;
        private IWorkbook _workbook =null ;
        private string _strExcelPath;
        /// <summary>
        /// 增加统一的单元格样式属性，避免为每个单元格设置属性，提高效率
        /// </summary>
        private XSSFCellStyle _xssfCaptionCellStyle;//主标题样式
        private XSSFCellStyle _xssfSubCaptionCellStyle;//副标题样式
        private XSSFCellStyle _xssfDataCellStyle;//数据样式
        private XSSFCellStyle _xssfTableHeadStyle; //表格头部样式
        private CellStyle _captionStyle; //标题样式  
        private CellStyle _subCaptionStyle; //副标题样式
        private CellStyle _dataStyle;  //数据样式
        private CellStyle _tableHeadStyle;//表格头部样式

        private short _rowHeight = 0;     //行高

        public string ExcelPath
        {
            set
            {
                _strExcelPath = value;
            }
            get
            {
                return _strExcelPath;
            }
        }
        /// <summary>
        /// 设置行高度
        /// </summary>
        public short RowHeight { get { return _rowHeight; } set { _rowHeight = (short)(value * 20); } }

        public ExcelService(string paramExcelPath,IFileService fileService)
        {
            ExcelPath = paramExcelPath;
            _fileService = fileService;               
        }
        /// <summary>
        /// 设置标题样式
        /// </summary>
        public CellStyle CellCaptionStyle { get { return _captionStyle; } set { _captionStyle = value; _xssfCaptionCellStyle = PhaseCellStyle(_captionStyle); } }
        /// <summary>
        /// 设置副标题样式
        /// </summary>
        public CellStyle CellSubCaptionStyle { get { return CellSubCaptionStyle; } set { _subCaptionStyle = value; _xssfSubCaptionCellStyle = PhaseCellStyle(_subCaptionStyle); } }
        /// <summary>
        /// 设置数据样式
        /// </summary>
        public CellStyle CellDataStyle { get { return _dataStyle; } set { _dataStyle = value; _xssfDataCellStyle = PhaseCellStyle(_dataStyle); } }

        public CellStyle CellTableHeadStyle { get { return _tableHeadStyle; } set { _tableHeadStyle = value; _xssfTableHeadStyle = PhaseCellStyle(_tableHeadStyle); } }
        /// <summary>
        /// 返回工作薄内指定的Sheet页数据
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="lstSheetNames">工作表名称</param>
        /// <returns></returns>
        public System.Data.DataSet OpenExcel(string excelPath, List<string> lstSheetNames)
        {
            DataSet ds = new DataSet();
            foreach (string sheetName in lstSheetNames)
            {
                DataTable dt = OpenExcel(excelPath, sheetName);
                //如果未指定DataTable名称，则赋值表名称
                if(dt.TableName==""){dt.TableName = sheetName;}
                //如果已包含此表的名称，则不处理
                if(!ds.Tables.Contains(dt.TableName)){ds.Tables.Add(dt);}
            }
            return ds;
        }
        /// <summary>
        /// 返回指定Excel文件中指定的Sheet页的内容
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public System.Data.DataTable OpenExcel(string excelPath,string sheetName)
        {
            _workbook = null;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            ISheet sheet = null;
            DataTable data = new DataTable();
            string strStatus="";
            int sheetsAmount;//所有的Sheet数量

            int startRow = 0;

            if (excelPath.IndexOf(".xlsx") > 0) // 2007版本
            {
                _workbook = new XSSFWorkbook(fs);
            }
            else if (excelPath.IndexOf(".xls") > 0) // 2003版本
            { 
                _workbook = new HSSFWorkbook(fs);
            }            
            if(_workbook !=null )
            { 
                  sheetsAmount = _workbook.NumberOfSheets;
                  if (sheetName != null)
                  {
                      sheet = _workbook.GetSheet(sheetName);
                      if (sheet == null) //没有指定名称的Sheet页
                      {
                          //sheet = _workbook.GetSheetAt(0);
                          strStatus = "未找到指定名称的Sheet页";
                      }
                  }
                  else
                  { 
                       sheet = _workbook.GetSheetAt(0);
                  }
                  if (sheet != null)
                  {
                      IRow firstRow = sheet.GetRow(0);
                      int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                      //if (isFirstRowColumn)
                      //{
                      for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                      {
                          ICell cell = firstRow.GetCell(i);
                          if (cell != null)
                          {
                              string cellValue = cell.StringCellValue;
                              if (cellValue != null)
                              {
                                  DataColumn column = new DataColumn(cellValue);
                                  data.Columns.Add(column);
                              }
                          }
                      }
                      startRow = sheet.FirstRowNum + 1;
                      //}
                      //else
                      //{
                      //    startRow = sheet.FirstRowNum;
                      //}

                      //最后一列的标号
                      int rowCount = sheet.LastRowNum;
                      for (int i = startRow; i <= rowCount; ++i)
                      {
                          IRow row = sheet.GetRow(i);
                          if (row == null) continue; //没有数据的行默认是null　　　　　　　

                          DataRow dataRow = data.NewRow();
                          for (int j = row.FirstCellNum; j < cellCount; ++j)
                          {
                              if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                  dataRow[j] = row.GetCell(j).ToString();
                          }
                          data.Rows.Add(dataRow);
                      }
                  }
                  else
                  {
                      DataColumn column = new DataColumn("错误");
                      data.Columns.Add(column);
                      DataRow row = data.NewRow();
                      row[0] = strStatus;
                      data.Rows.Add(row);
                      data.TableName = sheetName+"Error";
                  }
              }
              data.TableName = sheetName;
              return data;
        }
        public System.Data.DataTable OpenExcel(string excelPath, string sheetName, Dictionary<int, int> dictRowsDefinition,out bool sheetExistFlag)
        {
            _workbook = null;
            sheetExistFlag = false;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            ISheet sheet = null;
            DataTable data = new DataTable();
            string strStatus = "";
            int sheetsAmount;//所有的Sheet数量

            int startRow = 0;

            if (excelPath.IndexOf(".xlsx") > 0) // 2007版本
            {
                _workbook = new XSSFWorkbook(fs);
            }
            else if (excelPath.IndexOf(".xls") > 0) // 2003版本
            {
                _workbook = new HSSFWorkbook(fs);
            }
            if (_workbook != null)
            {
                if (sheetName != null)
                {
                    sheet = _workbook.GetSheet(sheetName);
                    if (sheet == null) //没有指定名称的Sheet页
                    {
                        //sheet = _workbook.GetSheetAt(0);
                        strStatus = "未找到指定名称的Sheet页";
                        sheetExistFlag = false;
                    }
                }
                if (sheet != null)
                {
                    foreach (var rowDef in dictRowsDefinition)
                    {
                        #region 定义表头信息
                            IRow rowInfo = sheet.GetRow(rowDef.Key);
                            int cellCount = rowInfo.LastCellNum; //一行最后一个cell的编号 即总的列数
                            for (int j = rowInfo.FirstCellNum; j < cellCount; ++j)//记录当前行各列的信息
                            {
                                ICell cell = rowInfo.GetCell(j);
                                if (cell != null)
                                {
                                    string cellValue = cell.StringCellValue;
                                    if (cellValue != null)
                                    {                                        
                                        if (!data.Columns.Contains(cellValue))
                                        {
                                            DataColumn column = new DataColumn(cellValue);
                                            data.Columns.Add(column);
                                        }

                                        
                                    }
                                }
                            }
                        #endregion

                        #region 获取数据值
                            for (int i = rowDef.Key + 1; i <=rowDef.Value; i++)//遍历起始行至终止行
                            {
                                rowInfo = sheet.GetRow(i);
                                if (rowInfo != null)
                                {
                                    DataRow dataRow = data.NewRow();

                                    for (int j = rowInfo.FirstCellNum; j < cellCount; ++j)//记录当前行各列的信息
                                    {
                                        if (rowInfo.GetCell(j) != null)
                                        {
                                            dataRow[j] = rowInfo.GetCell(j).ToString();
                                        }
                                    }
                                    data.Rows.Add(dataRow);
                                }
                                else //有空行时，剩余数据不必处理，直接返回
                                {
                                    break;
                                }
                            }
                        #endregion
                    }
                    sheetExistFlag = true;
                }
            }
            data.TableName = sheetName;
            return data;
        }
        public System.Data.DataTable OpenExcel()
        {
            throw new NotImplementedException();
        }

        public bool SaveToFile()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                _workbook.Write(ms);
                _fileService.SaveToFile(ms, ExcelPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateExcelSheets(List<string> lstSheetNames)
        {
            _workbook = new XSSFWorkbook();
            foreach (string sheetName in lstSheetNames)
            {
                _workbook.CreateSheet(sheetName);
            }
        }

        public void SetColumnWidth(string strSheetName, int intColumnIndex, float sngColumnWidth)
        {
            ISheet sheet;
            sheet = _workbook.GetSheet(strSheetName);
            //int intNpoiColumnWidth = (int)(sngColumnWidth * 100 / 38)*100 + 100;
            int intNpoiColumnWidth = (int)(sngColumnWidth * 256);
            sheet.SetColumnWidth(intColumnIndex, intNpoiColumnWidth);
        }

        public void SetMergeCells(string strSheetName, System.Collections.Generic.List<MergeCellRange> lstMergeCellRange)
        {
            ISheet sheet = _workbook.GetSheet(strSheetName);
            foreach (MergeCellRange singleValue in lstMergeCellRange)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(singleValue.FirstRowIndex, singleValue.LastRowIndex, singleValue.FirstColumnIndex, singleValue.LastColumnIndex));

            }
        }

        public void SetValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, CellStyleType styleType)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设置单元格数据
        /// </summary>
        /// <param name="strSheetName">需设置的sheet名称</param>
        /// <param name="intRowNumber">需设置的行号</param>
        /// <param name="intCellNumber">需设置的单元格序号</param>
        /// <param name="strCellValue">数据值</param>
        /// <param name="paramCellStyle">单元格样式</param>
        public void SetCellValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, CellStyleType styleType)
        {
            ISheet sheet;
            sheet = _workbook.GetSheet(strSheetName);
            IRow row = sheet.GetRow(intRowNumber);
            if (row == null) //如果未取到，则创建行
            {
                row = sheet.CreateRow(intRowNumber);
                if (RowHeight != 0)
                {
                    row.Height = RowHeight;
                }
            }
            XSSFCell cell;
            if (strCellValue != null) //如果为空，则不设置数据值
            {
                cell = (XSSFCell)row.CreateCell(intCellNumber);
                //cell.SetCellValue(strCellValue);

                if (strCellValue is string)
                {
                    cell.SetCellValue(strCellValue.ToString());
                }
                if (strCellValue is XSSFRichTextString)
                {
                    cell.SetCellValue((XSSFRichTextString)strCellValue);
                }
            }
            else
            {
                cell = (XSSFCell)row.CreateCell(intCellNumber);
            }

            switch (styleType)
            {
                case CellStyleType.Caption:
                    //row.Cells[intCellNumber].CellStyle = xssfCaptionCellStyle;
                    cell.CellStyle = _xssfCaptionCellStyle;
                    break;
                case CellStyleType.SubCaption:
                    //row.Cells[intCellNumber].CellStyle = xssfSubCaptionCellStyle;
                    cell.CellStyle = _xssfSubCaptionCellStyle;
                    break;
                case CellStyleType.Data:
                    //row.Cells[intCellNumber].CellStyle = xssfDataCellStyle;
                    cell.CellStyle = _xssfDataCellStyle;
                    break;
                case CellStyleType.TableHead:
                    cell.CellStyle = _xssfTableHeadStyle;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 解析单元格样式
        /// </summary>
        /// <param name="paramCellStyle">单元格样式</param>
        /// <returns>NPOI单元格样式</returns>
        private XSSFCellStyle PhaseCellStyle(CellStyle paramCellStyle)
        {
            XSSFCellStyle style = (XSSFCellStyle)_workbook.CreateCellStyle();
            XSSFFont font = (XSSFFont)_workbook.CreateFont();
            font.FontName = paramCellStyle.FontName.ToString();
            font.FontHeightInPoints = paramCellStyle.FontHeightInPoints;
            if (paramCellStyle.FontBoldFlag)
            {
                font.Boldweight = 700;
            }
            style.SetFont(font);
            //设置水平位置
            switch (paramCellStyle.HorizontalAlignment.ToString())
            {
                case "Center":
                    style.Alignment = HorizontalAlignment.Center;
                    break;
                case "Left":
                    style.Alignment = HorizontalAlignment.Left;
                    break;
                case "Right":
                    style.Alignment = HorizontalAlignment.Right;
                    break;
                default:
                    style.Alignment = HorizontalAlignment.Center;
                    break;
            }
            //设置垂直位置
            switch (paramCellStyle.VerticalAlignment.ToString())
            {
                case "Center":
                    style.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case "Top":
                    style.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case "Botton":
                    style.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                default:
                    style.VerticalAlignment = VerticalAlignment.Center;
                    break;
            }
            //设置表格线样式宽度
            switch (paramCellStyle.BorderStyle.ToString())
            {
                case "Thin":
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;
                    style.BorderRight = BorderStyle.Thin;
                    style.BorderTop = BorderStyle.Thin;
                    break;
                default:
                    break;
            }
            //设置表格线颜色
            switch (paramCellStyle.BorderStyle.ToString())
            {
                case "Black":
                    style.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    style.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    style.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    style.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    break;
                default:
                    break;
            }
            //设置单元格是否换行
            style.WrapText = paramCellStyle.WrapText;

            return style;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public DataTable OpenExcel(string excelPath)
        {
            throw new NotImplementedException();
        }
        public enum CellStyleType { Caption, SubCaption, Data,TableHead, None };


        public void SetValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, Interface.CellStyleType styleType)
        {
            throw new NotImplementedException();
        }

        public void SetCellValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, Interface.CellStyleType styleType)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 创建区域名称
        /// </summary>
        /// <param name="rangeName">区域名称</param>
        /// <param name="formula">区域公式</param>
        public void SetRangeName(string rangeName, string formula)
        {
            IName range = _workbook.CreateName();
            range.RefersToFormula = formula;
            range.NameName = rangeName;
        }
        /// <summary>
        /// 为页签序列验证
        /// </summary>
        /// <param name="sheetName">目标Sheet</param>
        /// <param name="rangeName">值域名称</param>
        /// <param name="cellRange">应用此验证的范围</param>
        public void SetSheetValidationForListConstraint(string sheetName,string rangeName,MergeCellRange cellRange)
        {
            CellRangeAddressList rangeList =new CellRangeAddressList(cellRange.FirstRowIndex,cellRange.LastRowIndex,cellRange.FirstColumnIndex,cellRange.LastColumnIndex);
            ISheet sheet = _workbook.GetSheet(sheetName);
            IDataValidationHelper dataValidationHelper = sheet.GetDataValidationHelper();
            IDataValidationConstraint constraint = dataValidationHelper.CreateFormulaListConstraint(rangeName);
            XSSFDataValidation validation = (XSSFDataValidation)dataValidationHelper.CreateValidation(constraint, rangeList);
            sheet.AddValidationData(validation);
        }
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
