using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data;
using SCA.Interface;
using SCA.Model;
using NSubstitute;
using SCA.DatabaseAccess;
namespace SCA.Test
{
    /* ==============================
*
* Author     : William
* Create Date: 2016/10/26 14:55:38
* FileName   : TestFileService
* Description: 数据库操作测试类
* Version：V1
* ===============================
*/
    //http://codedetective.blogspot.jp/2012/06/unit-testing-databases-using-c-and.html
    [TestFixture]
    public class TestSQLiteDatabaseAccess
    {
        private SQLiteDatabaseAccess _dbConn;
        private SCA.Interface.ILogRecorder _logHelper;
        private IFileService _fileService;
        const string strDBFile = "E:\\test.db";
        [SetUp]
        public void Initilize()
        {
            _fileService = new SCA.BusinessLib.Utility.FileService();
            _logHelper = new SCA.BusinessLib.Utility.LogRecorder(_fileService);            
        }
        [Test]
        public void CreateDatabase()
        {
            _dbConn = new DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            _dbConn.CreateDBFile();
            bool fileExistFlag = _fileService.IsExistFile(strDBFile);
            Assert.AreEqual(true, fileExistFlag);
            //_dbConn.Dispose();
        }
        [Test]
        public void ExecuteBySqlTest()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");
            _dbConn = new DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            int returnValue = _dbConn.ExecuteBySql(sbCreateTableSql);
            Assert.AreEqual(0, returnValue);
         }   
        
        [Test]
        public void GetObjectValueTest()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("Create table ControllerAttachedInfo2(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");                      
            

            _dbConn = new DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            _dbConn.ExecuteBySql(sbCreateTableSql);
            int intControllerAttachedInfoNum = Convert.ToInt16(_dbConn.GetObjectValue(new StringBuilder("select count(*) from ControllerAttachedInfo2")));            
            Assert.AreEqual(0, intControllerAttachedInfoNum);            
            //_dbConn.Dispose();
        }
        
        [Test]
        public void GetDataListBySQLTest()
        {
            IList<LoopModel> lstLoopModel = new List<LoopModel>();

            StringBuilder sbCreateTableSql=new StringBuilder();
            //回路信息
            sbCreateTableSql.Append("Create table Loop(id integer not null primary key autoincrement,name varchar(20),Code varchar(6),controllerID int,DeviceAmount int );");

            _dbConn = new DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            _dbConn.ExecuteBySql(sbCreateTableSql);
            _dbConn.ExecuteBySql(new StringBuilder("insert into Loop(id,name,Code,controllerID,DeviceAmount) values(1,'william','0101',1,55)"));

            lstLoopModel = (List<LoopModel>)_dbConn.GetDataListBySQL<SCA.Model.LoopModel>(new StringBuilder("select * from Loop"));
            Assert.AreEqual(1, lstLoopModel.Count);             
        
        }
        [OneTimeTearDown]
        public void Dipsose()
        {
            Console.WriteLine("TearDown");            
            _dbConn.Dispose();
            _fileService.DeleteFile(strDBFile);            
        }
        [Ignore("应归在控制器测试逻辑中")]
        public void CreateTableOf8001()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");
            //回路表
            sbCreateTableSql.Append("Create table Loop(id integer not null primary key autoincrement,name varchar(20),Code varchar(6),controllerID int,DeviceAmount int );");

          
            _logHelper = Substitute.For<SCA.Interface.ILogRecorder>();
            _dbConn.ExecuteBySql(sbCreateTableSql);
            int intControllerAttachedInfoNum = Convert.ToInt16(_dbConn.GetObjectValue(new StringBuilder("select count(*) from ControllerAttachedInfo")));
            int intLoopNum = Convert.ToInt16(_dbConn.GetObjectValue(new StringBuilder("select count(*) from Loop")));

            Assert.AreEqual(0, intControllerAttachedInfoNum);
            // Assert.AreEqual(0, intLoopNum);//不知为什么查出的结果会是１
            _dbConn.Dispose();
        }

       // [Test]
       //// [SetUp]
       // public void ConnectDatabase()
       // {
       //     List<LoopModel> lstLoopModel = new List<LoopModel>();
       //    // dbConn = Substitute.For<IDatabaseService>();
            

            
            
       //    //dbConn.ExecuteBySql("");
       //     StringBuilder strSqlBuilder = new StringBuilder();
       //     LoopModel loopModel=new LoopModel(3,1,"0101","william",1,55);
       //     strSqlBuilder.Append("select * from loop");            
            
            
       //     //创建表
       //     dbConn.ExecuteBySql(sCreate);          
       //     //增加一条数据
       //     dbConn.ExecuteBySql(new StringBuilder("insert into Loop(id,name,Code,controllerID,DeviceAmount) values(1,'william','0101',1,55)"));            
       //     //取得新增数据对象
       //     //LoopModel loopModel2 = (LoopModel)dbConn.GetDataListBySQL<LoopModel>(strSqlBuilder);
       //     List<LoopModel> lstLoopModel2 = (List<LoopModel>)dbConn.GetDataListBySQL<LoopModel>(strSqlBuilder);
       //     //loopModel2.Level = 3;
       //     foreach (LoopModel l in lstLoopModel2)
       //     {
       //         l.Level = 3;
       //         Assert.AreEqual(loopModel, l);
       //         Assert.AreEqual(loopModel.ID, l.ID);
       //         Assert.AreEqual(loopModel.Level, l.Level);
       //         Assert.AreEqual(loopModel.Code, l.Code);
       //         Assert.AreEqual(loopModel.Name, l.Name);
       //         Assert.AreEqual(loopModel.controllerID, l.controllerID);
       //         Assert.AreEqual(loopModel.DeviceAmount, l.DeviceAmount);
       //     }
            
       //  //   LoopModel loopModel2=(LoopModel)dbConn.GetDataListBySQL<LoopModel>(strSqlBuilder);//.Returns<LoopModel>(loopModel,null);            
       //     //Assert.AreEqual(loopModel, loopModel2);
       // }

    }
}
