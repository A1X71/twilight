using System;
using SCA.Interface;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Data;
using System.Collections.Generic;

/* ==============================
*
* Author     : William
* Create Date: 2017/6/22 14:18:12
* FileName   : EXCEL2003Service
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public class Excel2003Service:IExcelService
    {
        private IFileService _fileService;
        private string _strExcelPath;
        private IWorkbook _workbook = null;
        /// <summary>
        /// 增加统一的单元格样式属性，避免为每个单元格设置属性，提高效率
        /// </summary>
        private HSSFCellStyle _hssfCaptionCellStyle;//主标题样式
        private HSSFCellStyle _hssfSubCaptionCellStyle;//副标题样式
        private HSSFCellStyle _hssfDataCellStyle;//数据样式
        private HSSFCellStyle _hssfTableHeadStyle; //表格头部样式
        private SCA.Interface.CellStyle _captionStyle; //标题样式  
        private SCA.Interface.CellStyle _subCaptionStyle; //副标题样式
        private SCA.Interface.CellStyle _dataStyle;  //数据样式
        private SCA.Interface.CellStyle _tableHeadStyle;//表格头部样式
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
        public Excel2003Service(string paramExcelPath,IFileService fileService)
        {
            ExcelPath = paramExcelPath;
            _fileService = fileService;               
        }

        public EXCELVersion Version
        {
            get { return EXCELVersion.EXCEL2003; }
        }

        /// <summary>
        /// 设置标题样式
        /// </summary>
        public CellStyle CellCaptionStyle { get { return _captionStyle; } set { _captionStyle = value; _hssfCaptionCellStyle = PhaseCellStyle(_captionStyle); } }
        /// <summary>
        /// 设置副标题样式
        /// </summary>
        public CellStyle CellSubCaptionStyle { get { return CellSubCaptionStyle; } set { _subCaptionStyle = value; _hssfSubCaptionCellStyle = PhaseCellStyle(_subCaptionStyle); } }
        /// <summary>
        /// 设置数据样式
        /// </summary>
        public CellStyle CellDataStyle { get { return _dataStyle; } set { _dataStyle = value; _hssfDataCellStyle = PhaseCellStyle(_dataStyle); } }

        public CellStyle CellTableHeadStyle { get { return _tableHeadStyle; } set { _tableHeadStyle = value; _hssfTableHeadStyle = PhaseCellStyle(_tableHeadStyle); } }

        public short RowHeight
        {
            get
            {
                return  _rowHeight;     //行高
            }
            set
            {
                _rowHeight = (short)(value * 20); 
            }
        }



        public DataTable OpenExcel(string excelPath)
        {
            throw new NotImplementedException();
        }

        public DataTable OpenExcel(string excelPath, string sheetName, Dictionary<int, int> dictRowsDefinition, out bool sheetExistFlag)
        {
            _workbook = null;
            sheetExistFlag = false;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            ISheet sheet = null;
            DataTable data = new DataTable();
            string strStatus = "";
            int sheetsAmount;//所有的Sheet数量
            int startRow = 0;

            _workbook = new HSSFWorkbook(fs);
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
                        for (int i = rowDef.Key + 1; i <= rowDef.Value; i++)//遍历起始行至终止行
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

        public DataTable OpenExcel()
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
            _workbook = new HSSFWorkbook();
            foreach (string sheetName in lstSheetNames)
            {
                _workbook.CreateSheet(sheetName);
            }
        }

        public void SetColumnWidth(string strSheetName, int intColumnIndex, float sngColumnWidth)
        {
            ISheet sheet;
            sheet = _workbook.GetSheet(strSheetName);            
            int intNpoiColumnWidth = (int)(sngColumnWidth * 256);
            sheet.SetColumnWidth(intColumnIndex, intNpoiColumnWidth);
        }

        public void SetMergeCells(string strSheetName, List<MergeCellRange> lstMergeCellRange)
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

        public void SetCellValue(string strSheetName, int intRowNumber, int intCellNumber, object strCellValue, CellStyleType styleType)
        {
            try
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
                HSSFCell cell;
                if (strCellValue != null) //如果为空，则不设置数据值
                {
                    cell = (HSSFCell)row.CreateCell(intCellNumber);
                    //cell.SetCellValue(strCellValue);

                    if (strCellValue is string)
                    {
                        cell.SetCellValue(strCellValue.ToString());
                    }
                    if (strCellValue is HSSFRichTextString)
                    {
                        cell.SetCellValue((HSSFRichTextString)strCellValue);
                    }
                }
                else
                {
                    cell = (HSSFCell)row.CreateCell(intCellNumber);
                }

                switch (styleType)
                {
                    case CellStyleType.Caption:                        
                        cell.CellStyle = _hssfCaptionCellStyle;
                        break;
                    case CellStyleType.SubCaption:                        
                        cell.CellStyle = _hssfSubCaptionCellStyle;
                        break;
                    case CellStyleType.Data:                        
                        cell.CellStyle = _hssfDataCellStyle;
                        break;
                    case CellStyleType.TableHead:                        
                        cell.CellStyle = _hssfTableHeadStyle;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
            }
        }

        public void SetRangeName(string rangeName, string formula)
        {
            IName range = _workbook.CreateName();
            range.RefersToFormula = formula;
            range.NameName = rangeName;
        }

        public void SetSheetValidationForListConstraint(string sheetName, string rangeName, MergeCellRange cellRange)
        {
            CellRangeAddressList rangeList = new CellRangeAddressList(cellRange.FirstRowIndex, cellRange.LastRowIndex, cellRange.FirstColumnIndex, cellRange.LastColumnIndex);
            ISheet sheet = _workbook.GetSheet(sheetName);
            IDataValidationHelper dataValidationHelper = sheet.GetDataValidationHelper();
            IDataValidationConstraint constraint = dataValidationHelper.CreateFormulaListConstraint(rangeName);
            HSSFDataValidation validation = (HSSFDataValidation)dataValidationHelper.CreateValidation(constraint, rangeList);
            sheet.AddValidationData(validation);
        }
        private HSSFCellStyle PhaseCellStyle(CellStyle paramCellStyle)
        {
            HSSFCellStyle style = null;
            //try
            //{
            style = (HSSFCellStyle)_workbook.CreateCellStyle();
            HSSFFont font = (HSSFFont)_workbook.CreateFont();
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
            #region 设置为文本
            style.WrapText = paramCellStyle.WrapText;
            HSSFDataFormat format = (HSSFDataFormat)_workbook.GetCreationHelper().CreateDataFormat();
            short index = format.GetFormat("@");
            style.DataFormat = index;
            #endregion
            //}
            //catch(Exception ex)
            //{
            //    string ss = ex.Message;
            //    return null;
            //}
            return style;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public DataTable OpenExcel(string excelPath, string sheetName, Dictionary<int, int> dictRowsDefinition, out bool sheetExistFlag, out int elapsedTime)
        {
            _workbook = null;
            sheetExistFlag = false;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            ISheet sheet = null;
            DataTable data = new DataTable();
            string strStatus = "";
            int sheetsAmount;//所有的Sheet数量
            int startRow = 0;

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            _workbook = new HSSFWorkbook(fs);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            int intElapsedTime = ts.Hours * 60*60*1000 + ts.Minutes*60*1000+ts.Seconds*1000+ts.Milliseconds; //记录消耗时间
            elapsedTime = intElapsedTime; 
            
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
                        for (int i = rowDef.Key + 1; i <= rowDef.Value; i++)//遍历起始行至终止行
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
    }
}
