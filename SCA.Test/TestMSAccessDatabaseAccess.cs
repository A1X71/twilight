using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SCA.DatabaseAccess;
using SCA.Interface;
using System.Data;
/* ==============================
*
* Author     : William
* Create Date: 2016/10/26 16:16:50
* FileName   : TestMSAccessDatabaseAccess
* Description: Access数据库测试类
* Version：V1
* ===============================
*/
namespace SCA.Test
{
    [TestFixture]
    public class TestMSAccessDatabaseAccess
    {
        private MSAccessDatabaseAccess _dbConn;
        private SCA.Interface.ILogRecorder _logHelper;
        private IFileService _fileService;
        const string strDBFile = "F:\\8001.mdb";
        [SetUp]
        public void Initilize()
        {
            _fileService = new SCA.BusinessLib.Utility.FileService();
            _logHelper = new SCA.BusinessLib.Utility.LogRecorder(_fileService);
        }
        [Test]
        public void ExecuteBySqlTest()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("select count(*) from 文件配置");
            _dbConn = new DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            int returnValue = _dbConn.ExecuteBySql(sbCreateTableSql);
            Assert.AreEqual(0, returnValue);
        }

        [Test]
        public void GetObjectValueTest()
        {
            
            int intControllerAttachedInfoNum = Convert.ToInt16(_dbConn.GetObjectValue(new StringBuilder("select count(*) from 文件配置")));
            Assert.AreEqual(1, intControllerAttachedInfoNum);
            //_dbConn.Dispose();
        }
        [Test]
        public void GetDataTableBySQLTest()
        {
            
            DataTable objDT = new DataTable();
            StringBuilder sbCreateTableSql = new StringBuilder();
            //回路信息
            sbCreateTableSql.Append("select * from 文件配置");

            _dbConn = new DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            //_dbConn.ExecuteBySql(sbCreateTableSql);
            objDT = _dbConn.GetDataTableBySQL(sbCreateTableSql);
            Assert.AreEqual(1, objDT.Rows.Count);

        }
    }
}
