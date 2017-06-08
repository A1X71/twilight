using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SCA.Interface;
using SCA.Interface.DatabaseAccess;
using SCA.DatabaseAccess;
using NSubstitute;
using SCA.Model;
using System.Data;
using SCA.BusinessLib.BusinessLogic;
using SCA.BusinessLib.Controller;
using System.Text.RegularExpressions;
namespace Test.Console
{
    class Program
    {
        static SQLiteDatabaseAccess _dbConn;
        static MSAccessDatabaseAccess _msDBConn;
        static SCA.Interface.ILogRecorder _logHelper;
        static IFileService _fileService;
        static IDatabaseService _databaseService;
        static IProjectManager _projectManager;
        static IProjectService _projectService;
        static IControllerOperation _controllerOperation;
        static IExcelService _excelService;
        static void Main(string[] args)
        {
            //IProjectManager<SCA.Model.ProjectModel> _projManager;
            #region 内存数据操作
            //IProjectService _projService;

            //_projectManager = SCA.BusinessLib.ProjectManager.GetInstance;//(_projectService, _fileService);
            //while (true)
            //{
            //    string cmd=System.Console.ReadLine();
            //    if (cmd == "q")
            //    {
            //        break;
            //    }
            //    else if (cmd == "p")
            //    {
            //        System.Console.WriteLine("1 Character: 项目名称");
            //        string data = System.Console.ReadLine();
            //        string[] strCharacter = data.Split(' ');
            //        SCA.Model.ProjectModel pModel = new ProjectModel();
            //        pModel.ID = 1;
            //        pModel.Name = strCharacter[0];
            //        pModel.SavePath = "e:\\myData.db";
            //        pModel.SaveInterval = 30;
            //        //project不为空时的判断与复盖的数据处理逻辑
            //        _projectManager.CreateProject(pModel);
                    
            //    }
            //    else if (cmd == "c")
            //    {
            //        System.Console.WriteLine("4 Character: 控制器名称，端口名称，器件长度，机号");
            //        string data = System.Console.ReadLine();
            //        string[] strCharacter = data.Split(' ');
            //        ControllerModel controller = new ControllerModel(ControllerType.NT8036);
            //        controller.Name = strCharacter[0];// "Name8036";
            //        controller.PortName = strCharacter[1];// "Com1";
            //        controller.Project = _projectManager.Project;
            //        controller.DeviceAddressLength = Convert.ToInt16(strCharacter[2]);// 7;

            //        if (controller.DeviceAddressLength == 8)
            //        {
            //            controller.MachineNumber = strCharacter[3].PadLeft(3,'0'); 
            //        }
            //        else
            //        {
            //            controller.MachineNumber = strCharacter[3].PadLeft(2, '0'); 
            //        }
            //        ///controller.LoopAddressLength = 2;可以不设置，默认为2
            //        _projectManager.AddControllers(controller);                    
            //    }
            //    else if (cmd == "l")
            //    {
                    

            //        System.Console.WriteLine("控制器名称,回路数量,器件数量");
            //        string data = System.Console.ReadLine();
            //        string[] strCharacter = data.Split(' ');
            //        SCA.Model.ControllerModel controller =_projectManager.GetControllerBySpecificID(strCharacter[0]);

            //        //需要注册所有的ControllerOperation8036, 然后根据得到的"控制器类型"取出相关的处理类[可以考虑用Builder模式]
            //        _controllerOperation = new ControllerOperation8036(_databaseService);
            //        foreach(var l in  _controllerOperation.CreateLoops(Convert.ToInt16(strCharacter[1]), Convert.ToInt16(strCharacter[2]), controller))
            //        {
            //            controller.Loops.Add(l);
            //        }
                    
            //        //if (_projectManager.UpdateControllerBySpecificController(controller))
            //        //{
            //        //    System.Console.WriteLine("创建回路成功");
            //        //}
            //        //else
            //        //{
            //        //    System.Console.WriteLine("创建回路失败");
            //        //}
                    

            //    }
            //    else if (cmd == "d")
            //    {
            //        //System.Console.WriteLine("回路名称,");
            //        //string data = System.Console.ReadLine();
            //        //string[] strCharacter = data.Split(' ');
            //        //SCA.Model.ControllerModel controller = _projectManager.GetControllerBySpecificID(strCharacter[0]);
            //        ////需要注册所有的ControllerOperation8036, 然后根据得到的"控制器类型"取出相关的处理类[可以考虑用Builder模式]
            //        //_controllerOperation = new ControllerOperation8036(_databaseService);
            //        _fileService = new SCA.BusinessLib.Utility.FileService();
            //        string strName = "八一农垦大学";
            //        string strSavePath = "E:\\StarUniversity.db";
            //        ProjectModel pModel = new ProjectModel();
            //        pModel.ID = 1;
            //        pModel.Name = strName;
            //        pModel.SavePath = strSavePath;
            //        pModel.SaveInterval = 30;
            //        _projectManager.CreateProject(pModel);
            //        _databaseService = new SQLiteDatabaseAccess(_projectManager.Project.SavePath, _logHelper, _fileService);
            //        _projService = new ProjectService(_excelService);
            //        SCA.Model.ProjectModel proj = _projService.GetProject(_projectManager.Project);
            //        _projService.Dispose();
                    

            //    }
            //    else if (cmd == "print")
            //    {
            //        ProjectModel p = _projectManager.Project;
            //        System.Console.WriteLine(p.ID + "  " + p.Name + "   " + p.SavePath);
            //        foreach (ControllerModel c in p.Controllers)
            //        {
            //            System.Console.WriteLine(c.Project.Name + "  " + c.Name + "  " + c.PortName + "   " + c.DeviceAddressLength.ToString() + "   " + c.MachineNumber);
            //            foreach (LoopModel l in c.Loops)
            //            {
            //                System.Console.WriteLine(l.Code + "  " + l.Name + "  " + l.DeviceAmount);
            //                foreach (var devInfo in l.GetDevices<DeviceInfo8036>())
            //                {
            //                    System.Console.WriteLine(devInfo.Code + "  " + devInfo.TypeCode + "  " + devInfo.Disable);    
            //                }
            //            } 
            //        }
            //    }
            //}
            #endregion

            //ProjectModel pModel = new ProjectModel(1, "myData", 1);
            //pModel.SavePath = "e:\\myData.db";
            //_fileService = new SCA.BusinessLib.Utility.FileService();
            //_databaseService = new SQLiteDatabaseAccess(pModel.SavePath, _logHelper, _fileService);
            //_projService = new ProjectService(_databaseService);
            
            

            //bool blnResult = _projService.CreateProject(pModel);
            //_projService.Dispose();
            //_fileService.DeleteFile(pModel.SavePath);

            while (true)
            { 
                string chara=System.Console.ReadLine();
                if (chara == "Q")
                {
                    break;
                }
                //器件编码范围
                //string strReg = "^([01][0-9][0-9])$";
                //string strReg = "^(([0-9]|[1-9][0-9]|100))|((([0-9]|[1-9][0-9]|100){1}\\.){1}[0-9])$";
                string strReg = "^(([0-9]|[1-9][0-9]|100){1})(\\.[0-9]){0,1}$";
                //string strReg = "^([0-9]|[1-9][0-9]|100)$";
                //|2[0-4][0-9]|25[0-5]
                //"^([01][0-9][0-9]|2[0-4][0-9]|25[0-5])$"
                Regex reg = new Regex(strReg);
                Match r=reg.Match(chara);                
                System.Console.WriteLine(r.ToString());
            }
            System.Console.ReadLine();

            #region 通讯测试 最新检查的代码
            //InvokeControllerCom iCC = InvokeControllerCom.Instance;
            //iCC.StartCommunication();
            //while (true)
            //{ 
            //  string input = System.Console.ReadLine();
            //            if (input == "Q")
            //            {
            //                break;
            //            }

            //            switch (input)
            //            {
            //                case "BB"://发送器件
            //                    iCC.TheControllerType.Status = ControllerStatus.DataSending;// 将控制器置于数据发送状态
            //                    switch (iCC.TheControllerType.ControllerType)
            //                    {
            //                        case ControllerType.NT8036:
            //                          //  dictControllerCom.TryGetValue(Model.ControllerModel.ControllerType.NT8036, out controllerTypeBase);
            //                            List<DeviceInfo8036> lstDevInfo;
                                        
            //                            ControllerCommunicationTesting.Get8036DevInfo(out lstDevInfo);
            //                            ((ControllerType8036)iCC.TheControllerType).DeviceInfoList = lstDevInfo;
            //                            //controllerTypeBase.SendDeviceInfo();
            //                            iCC.TheControllerType.OperableDataType = OperantDataType.Device;
            //                            iCC.TheControllerType.Status = ControllerStatus.DataSending;
            //                            input = "";
            //                            break;
            //                        case ControllerType.NT8001:
            //                            //dictControllerCom.TryGetValue(Model.ControllerModel.ControllerType.NT8001, out controllerTypeBase);
            //                            //List<DeviceInfo8001> lstDevInfo8001; //#1 需要确认继续抽像？？！！
            //                            //Get8001DevInfo(out lstDevInfo8001); //#2
            //                            //((ControllerType8001)iCC.TheControllerType).DeviceInfoList = lstDevInfo8001; //#3
            //                            ////controllerTypeBase.SendDeviceInfo();
            //                            //iCC.TheControllerType.OperableDataType = OperantDataType.Device;
            //                            //iCC.TheControllerType.Status = ControllerStatus.DataSending;
            //                            input = "";
            //                            break;
            //                    }
            //                    break;
                  //     }
           // }
           // MSAccessExecuteBySql();
            #endregion
            #region 通讯测试
            //SCA.BusinessLib.ComTest.ControllerManager controllerManager = new SCA.BusinessLib.ComTest.ControllerManager();
            //controllerManager.Run();
            //System.Console.Read();
            #endregion

            // var TaskSearchCom = Task.Run(()=>SerialPortHelper.SearchCOM());
           // var TaskStartup = Task.Run(() => Startup());
            //System.IO.Ports.SerialPort type = new System.IO.Ports.SerialPort();
            //ILogRecorder logHelper = Substitute.For<ILogRecorder>();
            //IDatabaseService dbconn = new SCA.SQLiteDatabaseAccess.SQLiteDatabaseAccess("test.db", logHelper);
            //StringBuilder strCreateSql = new StringBuilder("Create table Loop(id integer not null primary key autoincrement)");
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select * from loop");
            
            //dbconn.GetObjectValue(strCreateSql);
            //dbconn.GetObjectValue(new StringBuilder("insert into Loop(id,name) values(i,'william')"));
            //dbconn.GetObjectValue(strSql);

            #region 测试代码
           // IDatabaseService dbConn;
           // SCA.Interface.ILogRecorder logHelper;
           // SCA.Interface.IFileService fileService;
           // List<LoopModel> lstLoopModel = new List<LoopModel>();
           // // dbConn = Substitute.For<IDatabaseService>();
           // logHelper = Substitute.For<SCA.Interface.ILogRecorder>();
           // fileService =Substitute.For<SCA.Interface.IFileService>();
           // dbConn = new SCA.DatabaseAccess.SQLiteDatabaseAccess("test.db", logHelper,fileService);

           // //dbConn.ExecuteBySql("");
           // StringBuilder strSqlBuilder = new StringBuilder();
           // LoopModel loopModel = new LoopModel(3, 1, "0101", "william", 1, 55);
           // strSqlBuilder.Append("select * from loop");

           // StringBuilder sCreate = new StringBuilder("Create table Loop(id integer not null primary key autoincrement,name varchar(20),Code varchar(6),controllerID int,DeviceAmount int );");

           // sCreate.Append("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4))");

           // //创建表
           // dbConn.ExecuteBySql(sCreate);
           //// dbConn = new SCA.SQLiteDatabaseAccess.SQLiteDatabaseAccess("test.db", logHelper);
           // //增加一条数据
           // dbConn.ExecuteBySql(new StringBuilder("insert into Loop(id,name,Code,controllerID,DeviceAmount) values(1,'william','0101',1,5)"));

           // int intControllerAttachedInfoNum = Convert.ToInt16(dbConn.GetObjectValue(new StringBuilder("select count(*) from ControllerAttachedInfo")));
           // int intLoopNum = Convert.ToInt16(dbConn.GetObjectValue(new StringBuilder("select count(*) from Loop")));
           //// dbConn = new SCA.SQLiteDatabaseAccess.SQLiteDatabaseAccess("test.db", logHelper);
           // //取得新增数据对象
           // List<LoopModel> lstLoopModel2 = (List<LoopModel>)dbConn.GetDataListBySQL<LoopModel>(strSqlBuilder);

           // //loopModel2.Level = 3;
           // foreach (LoopModel l in lstLoopModel2)
           // {
           //     System.Console.WriteLine(l.Name);
           // }
           // System.Console.ReadLine();
            #endregion

           // _fileService = new SCA.BusinessLib.Utility.FileService();
           // _logHelper = new SCA.BusinessLib.Utility.LogRecorder(_fileService);
          //  _fileService.DeleteFile("E:\\test.db");
       //     ExecuteBySqlTest();
         //  MSAccessExecuteBySql();
          //  MSAccessGetDataTableBySQLTest();
        }
        static  void TimerProcess(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Console.WriteLine(e.SignalTime);
        }
        public static void MSAccessExecuteRead()
        {
            const string strDBFile = @"E:\ProjectDocuments\9 上位机程序源码\Branch\vb9CrtDraw\bin\x86\Release\database\FtCrtDB.mdb";
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);            
            DataTable objDT = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM DeviceOnMapTb Order by NetId,DeviceId");
            objDT = _msDBConn.GetDataTableBySQL(sbSql);

        }

        public static void MSAccessExecuteBySql()
        {
            const string strDBFile = @"E:\receive\李宏明\Lastest\佰丽酒店备份.mdb";
            
            DataTable objDT = new DataTable();
            StringBuilder sbCreateTableSql = new StringBuilder();

            //文件配置
            //"update  文件配置 set 文件版本=4"
            //sbCreateTableSql.Append("alter table 文件配置 drop column 器件长度");
            List<string> addList = new List<string>();
            //addList.Add("	alter table 0001  	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0002	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0003	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0004	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0005	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0006	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0007	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0008	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0009	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0010	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0011	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0012	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0013	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0014	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0015	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0016	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0017	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0018	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0019	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0020	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0021	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");
            addList.Add("	alter table 0022	add panhao varchar(3),jianhao varchar(3), louhao varchar(3), quhao varchar(3), cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5);	");

            
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);            
            //foreach(string type in addList)
            //{
            //    sbCreateTableSql.Clear();
            //    sbCreateTableSql.Append(type);
            //    _msDBConn.ExecuteBySql(sbCreateTableSql);
            //}
            sbCreateTableSql.Clear();
            ///sbCreateTableSql.Append("select 回路 from  系统设置;");
            //sbCreateTableSql.Append("Create table 网络手控盘(编号 VarChar(4),板卡号 VarChar(4), 盘号 VarChar(4),键号 VarChar(4),地编号 VarChar(7),sdpkey VarChar(5))");
            //sbCreateTableSql.Append("Create table 混合组态(编号 VarChar(4),动作常数 VarChar(4),动作类型 VarChar(4),分类A VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A VarChar(4),类型A VarChar(20),分类B VarChar(4),楼号B VarChar(4),区号B VarChar(4),层号B VarChar(4),类型B VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");
           // sbCreateTableSql.Append("Create table 通用组态(编号 VarChar(4),动作常数 VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A1 VarChar(4),层号A2 VarChar(4),类型A VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");            
           
            //sbCreateTableSql.Clear();
            //sbCreateTableSql.Append("alter table  文件配置 drop 器件长度;");
            //_msDBConn.ExecuteBySql(sbCreateTableSql);
           // sbCreateTableSql.Clear();
            sbCreateTableSql.Append("update 文件配置  set 文件版本=6;");
           //_msDBConn.ExecuteBySql(sbCreateTableSql);
           //sbCreateTableSql.Clear();
           //sbCreateTableSql.Append("alter table  0101 drop texing;");
           _msDBConn.ExecuteBySql(sbCreateTableSql);
           
        }
        #region　传设置数据库损坏后修复
        /// <summary>
        /// 更新器件信息
        /// </summary>
        public static void UpdateDeviceInfo()
        {
            const string strDBFile = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传.mdb";
            const string strDB2File = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传_new.mdb";
            DataTable objDT = new DataTable();
            StringBuilder sbCreateTableSql = new StringBuilder();
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            //foreach(string type in addList)
            //{
            //    sbCreateTableSql.Clear();
            //    sbCreateTableSql.Append(type);
            //    _msDBConn.ExecuteBySql(sbCreateTableSql);
            //}
            sbCreateTableSql.Clear();
            sbCreateTableSql.Append("select 回路 from  系统设置;");
            //sbCreateTableSql.Append("Create table 网络手控盘(编号 VarChar(4),板卡号 VarChar(4), 盘号 VarChar(4),键号 VarChar(4),地编号 VarChar(7),sdpkey VarChar(5))");
            //sbCreateTableSql.Append("Create table 混合组态(编号 VarChar(4),动作常数 VarChar(4),动作类型 VarChar(4),分类A VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A VarChar(4),类型A VarChar(20),分类B VarChar(4),楼号B VarChar(4),区号B VarChar(4),层号B VarChar(4),类型B VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");
            // sbCreateTableSql.Append("Create table 通用组态(编号 VarChar(4),动作常数 VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A1 VarChar(4),层号A2 VarChar(4),类型A VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");            
            System.Text.StringBuilder updateSQL = new StringBuilder();
            DataTable data = _msDBConn.GetDataTableBySQL(sbCreateTableSql);

            List<string> lstLoopName = new List<string>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                lstLoopName.Add(data.Rows[i][0].ToString());
            }
            foreach (string name in lstLoopName)
            {
                sbCreateTableSql.Clear();
                sbCreateTableSql.Append("select * from " + name);
                data = _msDBConn.GetDataTableBySQL(sbCreateTableSql);
                _msDBConn.GetDataTableBySQL(sbCreateTableSql);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    updateSQL.Append("Insert into " + name + "(bianhao,leixing,geli,lingmd,shuchu1,shuchu2,shuchu3,yanshi,xianggh,panhao,jianhao,gbzone,louhao,quhao,cenghao,fangjianhao,didian,cleixing,sdpkey)");
                    updateSQL.Append(" values('" + data.Rows[i]["bianhao"].ToString() + "','" + data.Rows[i]["leixing"].ToString() + "','" + data.Rows[i]["geli"].ToString() + "','" + data.Rows[i]["lingmd"].ToString() + "','");
                    updateSQL.Append(data.Rows[i]["shuchu1"].ToString() + "','" + data.Rows[i]["shuchu2"].ToString() + "','" + data.Rows[i]["shuchu3"].ToString() + "','" + data.Rows[i]["yanshi"].ToString() + "','");
                    updateSQL.Append(data.Rows[i]["xianggh"].ToString() + "','" + data.Rows[i]["panhao"].ToString() + "','" + data.Rows[i]["jianhao"].ToString() + "','" + data.Rows[i]["gbzone"].ToString() + "','");
                    updateSQL.Append(data.Rows[i]["louhao"].ToString() + "','" + data.Rows[i]["quhao"].ToString() + "','" + data.Rows[i]["cenghao"].ToString() + "','" + data.Rows[i]["fangjianhao"].ToString() + "','");
                    updateSQL.Append(data.Rows[i]["didian"].ToString() + "','" + data.Rows[i]["cleixing"].ToString() + "','" + data.Rows[i]["sdpkey"].ToString() + "');");
                }
            }


            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDB2File, _logHelper, _fileService);

            //for (int i = 0; i < lstLoopName.Count; i++)
            //{
            //    updateSQL.Append("Insert into 系统设置(回路,总数) values("+lstLoopName[i]+","+lstNodeNum[i]+");");
            //}
            updateSQL.Remove(updateSQL.Length - 1, 1);
            _msDBConn.ExecuteBySql(updateSQL);
        }
        /// <summary>
        /// 器件组态更新
        /// </summary>
        public static void UpdateGeneralLinkage()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            const string strDBFile = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传.mdb";
            const string strDB2File = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传_new.mdb";
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            //foreach(string type in addList)
            //{
            //    sbCreateTableSql.Clear();
            //    sbCreateTableSql.Append(type);
            //    _msDBConn.ExecuteBySql(sbCreateTableSql);
            //}
            sbCreateTableSql.Clear();
            sbCreateTableSql.Append("select * from  器件组态;");
            //sbCreateTableSql.Append("Create table 网络手控盘(编号 VarChar(4),板卡号 VarChar(4), 盘号 VarChar(4),键号 VarChar(4),地编号 VarChar(7),sdpkey VarChar(5))");
            //sbCreateTableSql.Append("Create table 混合组态(编号 VarChar(4),动作常数 VarChar(4),动作类型 VarChar(4),分类A VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A VarChar(4),类型A VarChar(20),分类B VarChar(4),楼号B VarChar(4),区号B VarChar(4),层号B VarChar(4),类型B VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");
            // sbCreateTableSql.Append("Create table 通用组态(编号 VarChar(4),动作常数 VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A1 VarChar(4),层号A2 VarChar(4),类型A VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");            

            DataTable data = _msDBConn.GetDataTableBySQL(sbCreateTableSql);
            System.Text.StringBuilder updateSQL = new StringBuilder();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                updateSQL.Append("Insert into 器件组态(输出组号,编号1,编号2,编号3,编号4,编号5,编号6,编号7,编号8,编号9,编号10,动作常数,联动组1,联动组2,联动组3) ");
                updateSQL.Append("values('" + data.Rows[i]["输出组号"].ToString() + "','" + data.Rows[i]["编号1"].ToString() + "','" + data.Rows[i]["编号2"].ToString() + "','");
                updateSQL.Append(data.Rows[i]["编号3"].ToString() + "','" + data.Rows[i]["编号4"].ToString() + "','" + data.Rows[i]["编号5"].ToString() + "','" + data.Rows[i]["编号6"].ToString() + "','");
                updateSQL.Append(data.Rows[i]["编号7"].ToString() + "','" + data.Rows[i]["编号8"].ToString() + "','" + data.Rows[i]["编号9"].ToString() + "','" + data.Rows[i]["编号10"].ToString() + "','");
                updateSQL.Append(data.Rows[i]["动作常数"].ToString() + "','" + data.Rows[i]["联动组1"].ToString() + "','" + data.Rows[i]["联动组2"].ToString() + "','" + data.Rows[i]["联动组3"].ToString() + "');");
            }

            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDB2File, _logHelper, _fileService);
            //for (int i = 0; i < lstLoopName.Count; i++)
            //{
            //    updateSQL.Append("Insert into 系统设置(回路,总数) values("+lstLoopName[i]+","+lstNodeNum[i]+");");
            //}
            updateSQL.Remove(updateSQL.Length - 1, 1);
            _msDBConn.ExecuteBySql(updateSQL);
        }
        /// <summary>
        /// 系统设置表
        /// </summary>
        public static void UpdateSystemSetting()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            const string strDBFile = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传.mdb";
            const string strDB2File = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传_new.mdb";
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            //foreach(string type in addList)
            //{
            //    sbCreateTableSql.Clear();
            //    sbCreateTableSql.Append(type);
            //    _msDBConn.ExecuteBySql(sbCreateTableSql);
            //}
            sbCreateTableSql.Clear();
            sbCreateTableSql.Append("select * from 系统设置;");
            //sbCreateTableSql.Append("Create table 网络手控盘(编号 VarChar(4),板卡号 VarChar(4), 盘号 VarChar(4),键号 VarChar(4),地编号 VarChar(7),sdpkey VarChar(5))");
            //sbCreateTableSql.Append("Create table 混合组态(编号 VarChar(4),动作常数 VarChar(4),动作类型 VarChar(4),分类A VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A VarChar(4),类型A VarChar(20),分类B VarChar(4),楼号B VarChar(4),区号B VarChar(4),层号B VarChar(4),类型B VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");
            // sbCreateTableSql.Append("Create table 通用组态(编号 VarChar(4),动作常数 VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A1 VarChar(4),层号A2 VarChar(4),类型A VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");            
            DataTable data = _msDBConn.GetDataTableBySQL(sbCreateTableSql);
            System.Text.StringBuilder updateSQL = new StringBuilder();
            int rows = data.Rows.Count;
            List<String> lstLoopName = new List<string>();
            List<String> lstNodeNum = new List<string>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string strName = data.Rows[i]["回路"].ToString();
                lstLoopName.Add(strName);
                lstNodeNum.Add(data.Rows[i]["总数"].ToString());
            }

            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDB2File, _logHelper, _fileService);
            for (int i = 0; i < lstLoopName.Count; i++)
            {
                updateSQL.Append("Insert into 系统设置(回路,总数) values(" + lstLoopName[i] + "," + lstNodeNum[i] + ");");
            }
            updateSQL.Remove(updateSQL.Length - 1, 1);
            _msDBConn.ExecuteBySql(updateSQL);
        }
        /// <summary>
        /// 根据系统配置信息表，创建回路表
        /// </summary>
        public static void CreateLoopName()
        {
            StringBuilder sbCreateTableSql = new StringBuilder();
            const string strDBFile = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传.mdb";
            const string strDB2File = @"E:\receive\李宏明\图形显示装置和传设置软件异常情况汇总\湖南异常反馈\2016年11月22邵东凤凰城为启动声光回传_new.mdb";
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);
            //foreach(string type in addList)
            //{
            //    sbCreateTableSql.Clear();
            //    sbCreateTableSql.Append(type);
            //    _msDBConn.ExecuteBySql(sbCreateTableSql);
            //}
            sbCreateTableSql.Clear();
            sbCreateTableSql.Append("select 回路 from 系统设置;");
            //sbCreateTableSql.Append("Create table 网络手控盘(编号 VarChar(4),板卡号 VarChar(4), 盘号 VarChar(4),键号 VarChar(4),地编号 VarChar(7),sdpkey VarChar(5))");
            //sbCreateTableSql.Append("Create table 混合组态(编号 VarChar(4),动作常数 VarChar(4),动作类型 VarChar(4),分类A VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A VarChar(4),类型A VarChar(20),分类B VarChar(4),楼号B VarChar(4),区号B VarChar(4),层号B VarChar(4),类型B VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");
            // sbCreateTableSql.Append("Create table 通用组态(编号 VarChar(4),动作常数 VarChar(4),楼号A VarChar(4),区号A VarChar(4),层号A1 VarChar(4),层号A2 VarChar(4),类型A VarChar(20),分类C VarChar(4),楼号C VarChar(4),区号C VarChar(4),层号C VarChar(4),类型C VarChar(20))");            
            DataTable data = _msDBConn.GetDataTableBySQL(sbCreateTableSql);
            System.Text.StringBuilder updateSQL = new StringBuilder();
            int rows = data.Rows.Count;
            List<String> lstLoopName = new List<string>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string strName = data.Rows[i]["回路"].ToString();
                lstLoopName.Add(strName);
            }

            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDB2File, _logHelper, _fileService);
            foreach (string strName in lstLoopName)
            {
                updateSQL.Append("Create table " + strName + "(bianhao varchar(7),leixing varchar(3),geli varchar(1),lingmd varchar(1),shuchu1 varchar(4),shuchu2 varchar(4), shuchu3 varchar(4),yanshi varchar(3),xianggh varchar(5), panhao varchar(3), jianhao varchar(3),cleixing varchar(15), GbZone varchar(3),didian varchar(25),louhao varchar(3),quhao varchar(3),cenghao varchar(3),fangjianhao varchar(3),sdpkey varchar(5));");
            }
            _msDBConn.ExecuteBySql(updateSQL);
        }
        #endregion
        public static void MSAccessGetDataTableBySQLTest()
        {
               
            const string strDBFile = @"C:\Users\Administrator\Desktop\8001.mdb";
            DataTable objDT = new DataTable();
            StringBuilder sbCreateTableSql = new StringBuilder();
            //文件配置
            sbCreateTableSql.Append("select * from 文件配置");
            _msDBConn = new SCA.DatabaseAccess.MSAccessDatabaseAccess(strDBFile, _logHelper, _fileService);            
            objDT = _msDBConn.GetDataTableBySQL(sbCreateTableSql);
            //Assert.AreEqual(1, objDT.Rows.Count);
        }
        public static void ExecuteBySqlTest()
        {
         
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("Create table ControllerAttachedInfo(ID in9teger not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");
            string strDBFile = "E:\\test.db";
            //_logHelper = Substitute.For<SCA.Interface.ILogRecorder>();
            
            _dbConn = new SCA.DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);

            int returnValue = _dbConn.ExecuteBySql(sbCreateTableSql);
            //Assert.AreEqual(0, returnValue);
          //  _dbConn.Dispose();

            //ILogRecorder _logHelper;
            //IFileService _fileService;
            //IDatabaseService _dbConn;// = new SCA.DatabaseAccess.SQLiteDatabaseAccess("test.db", logHelper);
            //StringBuilder sbCreateTableSql = new StringBuilder();
            ////附属信息
            //sbCreateTableSql.Append("Create table ControllerAttachedInfo(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");
            //string strDBFile = "test.db";
            //_logHelper = Substitute.For<SCA.Interface.ILogRecorder>();
            //_fileService = Substitute.For<SCA.Interface.IFileService>();
            //_dbConn = new  SCA.DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            //int returnValue = _dbConn.ExecuteBySql(sbCreateTableSql);
           // Assert.AreEqual(0, returnValue);
            //_dbConn.Dispose();
        }
        public static void GetObjectValueTest()
        {
            
            StringBuilder sbCreateTableSql = new StringBuilder();
            //附属信息
            sbCreateTableSql.Append("Create table ControllerAttachedInfo2(ID integer not null primary key autoincrement,controllerID integer not null,FileVersion varchar(4), ProtocolVersion varchar(4));");
            string strDBFile = "E:\\test.db";
            //_logHelper = Substitute.For<SCA.Interface.ILogRecorder>();
            //_fileService = new SCA.BusinessLib.Utility.FileService();
            _dbConn = new SCA.DatabaseAccess.SQLiteDatabaseAccess(strDBFile, _logHelper, _fileService);
            _dbConn.ExecuteBySql(sbCreateTableSql);
            int intControllerAttachedInfoNum = Convert.ToInt16(_dbConn.GetObjectValue(new StringBuilder("select count(*) from Co9ntrollerAttachedInfo2")));
            
            _dbConn.Dispose();
        }
        //public async Task Startup()
        //{
        //    while (!SerialPortHelper.DeviceConnected)
        //    {
        //        await Task.Delay(250);
        //    }
        //    while (taskState != TaskStates.Terminate)
        //    {

        //        switch (taskState)
        //        {
        //            case TaskStates.AskInfo:
        //                {
        //                    var DeviceID = DeviceCommunication.GetDeviceSerialNumber();
        //                    break;
        //                }
        //        }
        //    }
        //}
    }
}
