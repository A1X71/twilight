using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using SCA.Interface;
/* ==============================
*
* Author     : William
* Create Date: 2016/10/26 14:55:38
* FileName   : TestFileService
* Description: 文件操作测试类
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestFileService
    {
        private IFileService _fileService;
        private const string _filePath = "e:\\TestFile.txt";
        [OneTimeSetUp]
        public void Initialize()
        {            
            _fileService = new SCA.BusinessLib.Utility.FileService();
        }
        [Test]
        public void CreateFile()
        {
            
            Exception excep = null;
            try
            {
                
                _fileService.CreateFile(_filePath);
            }
            catch (Exception ex)
            {
                excep = ex;
            }            
            Assert.IsNull(excep, "Failure");     
        }
        [Test]
        public void DeleteFile()
        {
            
            Exception excep = null;
            try
            {
                _fileService.DeleteFile(_filePath);
            }
            catch (Exception ex)
            {
                excep = ex;
            }
            Assert.IsNull(excep,"Failure");
        }
        [Test]
        public void IsExistFile()
        {

            Exception excep = null;
            bool blnResult=false;
            try
            {
                _fileService.CreateFile(_filePath);
                blnResult=_fileService.IsExistFile(_filePath);
            }
            catch (Exception ex)
            {
                excep = ex;
            }
            Assert.IsNull(excep, "Failure");
            Assert.IsTrue(blnResult);
        }
        [OneTimeTearDown]
        public void Dipsose()
        {
            _fileService = null;
        }
    }
}
