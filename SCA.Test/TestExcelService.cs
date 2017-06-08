using System;
using SCA.Interface;
using NUnit.Framework;
using System.Collections.Generic ;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/7 9:47:55
* FileName   : TestExcelService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestExcelService
    {
        private IExcelService _excelService;
        private IFileService  _fileService;
        private const string _testExcelFilePath=@"E:\Program\4 Project\NT8001SeriesControllerAssistantV0.2\SCA.Test\TestFile\PROJECT.xlsx";
        [OneTimeSetUp]
        public void Initialize()
        {
            Exception excep = null;
            try
            {
                _fileService = new SCA.BusinessLib.Utility.FileService();                
                _excelService = new SCA.BusinessLib.Utility.ExcelService(_testExcelFilePath, _fileService);
            }
            catch (Exception ex)
            {
                excep = ex;
            }
            Assert.IsNull(excep, "Failure");      
        }
        [Test]
        public void SaveFile()
        {

            List<string> lstSheetNames = new List<string>();
            lstSheetNames.Add("设备类型");
            lstSheetNames.Add("回路名称");
            _excelService.CreateExcelSheets(lstSheetNames); //创建工作薄
            _excelService.RowHeight = (short)35.25;
            _excelService.SetCellValue(lstSheetNames[0], 0, 0, lstSheetNames[0], CellStyleType.None);
            _excelService.SetCellValue(lstSheetNames[1], 0, 0, lstSheetNames[1], CellStyleType.None);
            _excelService.SaveToFile();

            bool blnResult = _fileService.IsExistFile(_testExcelFilePath);
            Assert.IsTrue(blnResult);
        }
        [Test]
        public void OpenExcelWithRightSheetName()
        { 
            List<string> lstSheetNames = new List<string>();
            lstSheetNames.Add("设备类型");
            lstSheetNames.Add("回路名称");
            System.Data.DataSet ds=_excelService.OpenExcel(_testExcelFilePath, lstSheetNames);

            Assert.AreEqual(lstSheetNames.Count, ds.Tables.Count);
            StringAssert.AreEqualIgnoringCase(lstSheetNames[0].ToString(), ds.Tables[0].TableName.ToString());
            StringAssert.AreEqualIgnoringCase(lstSheetNames[1].ToString(), ds.Tables[1].TableName.ToString());
            
        }
        [Test]
        public void OpenExcelWithWrongSheetName()
        {
            List<string> lstSheetNames = new List<string>();
            lstSheetNames.Add("设备类型2");
            lstSheetNames.Add("回路名称3");
            System.Data.DataSet ds = _excelService.OpenExcel(_testExcelFilePath, lstSheetNames);
            Assert.AreEqual(lstSheetNames.Count, ds.Tables.Count);
            StringAssert.AreNotEqualIgnoringCase(lstSheetNames[0].ToString(), ds.Tables[0].TableName.ToString());
            StringAssert.AreNotEqualIgnoringCase(lstSheetNames[1].ToString(), ds.Tables[1].TableName.ToString());
        }

    }
}
