using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;
using System.Data.SQLite;
using System.Data;
using System.Collections;
using Neat.Dennis.Common.LoggerManager;
using System.Reflection;
namespace SCA.DatabaseAccess
{
    /* ==============================
    *
    * Author     : William
    * Create Date: 2016/10/22 15:41:49
    * FileName   : SQLiteDatabaseAccess
    * Description: SQLite数据库处理类
    * Version：V1
    * ===============================
    */
    public class SQLiteDatabaseAccess:IDatabaseService
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Object _locker = new Object(); //并行锁        
        private SQLiteConnection _db = null;
        private ILogRecorder _logRecorder; //日志处理
        private IFileService _fileService; //文件处理
        private SQLiteCommand _dbCommand = null; //Command命令
        protected string _connectionString = "";//数据库连接串

        public int CommandTimeOut = 600;
        public SQLiteDatabaseAccess(string connString, ILogRecorder logRecorder,IFileService fileService)
        {
            _connectionString = connString;
            _logRecorder = logRecorder;
            _fileService = fileService;
        }
        public SQLiteCommand DbCommand 
        {
            get { return this._dbCommand; }
            set { this._dbCommand = value; }
        
        }
        /// <summary>
        /// 将扩展名由NT转换为DB
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        //private string ConvertFileExtensionFromNTToDB(string strPath)
        //{
        //    string strResult = "";
        //    if (strPath.LastIndexOf('.') > 0)//
        //    {
        //        strResult = strPath.Substring(0, strPath.LastIndexOf('.')) + ".db";
        //    }
        //    else
        //    {
        //        strResult = strPath + ".db";
        //    }            
        //    return strResult;
        //}
        /// <summary>
        /// 将扩展名由DB转换为NT
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        //private string ConvertFileExtensionFromDBToNT(string strPath)
        //{
        //    string strResult = "";
        //    if (strPath.LastIndexOf('.') > 0)//
        //    {
        //        strResult = strPath.Substring(0, strPath.LastIndexOf('.')) + ".nt";
        //    }
        //    else
        //    {
        //        strResult = strPath + ".nt";
        //    }
        //    return strResult;
        //}
        /// <summary>
        /// 获取数据为连接对象
        /// </summary>
        /// <returns></returns>
        public System.Data.SQLite.SQLiteConnection GetInstance()
        {
            try
            {
                //在并发时，使用单一对象
                if (_db == null)
                {
                    CreateDBFile();
                    return _db = new SQLiteConnection("Data Source=" + _connectionString);

                }
                else
                {
                    lock (_locker)
                    {

                        return _db;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return null;
            }       
        }
        /// <summary>
        /// 创建数据库文件        /// 
        /// </summary>
        /// <remarks>
        /// 需要测试文件是否创建成功，标记为public
        /// </remarks>
        public void CreateDBFile()
        {
            if (!_fileService.IsExistFile(_connectionString))//如果不存在数据库文件则创建
            {
                SQLiteConnection.CreateFile(_connectionString);
            }
        }

        public object GetObjectValue(StringBuilder sql)
        {
            return GetObjectValue(sql,null);
        }
        public object GetObjectValue(StringBuilder sql, object[] param)
        {
            object result = null;
            //创建连接
            SQLiteConnection conn = (SQLiteConnection)this.GetInstance();
            //创建指令
            
           
            try
            {
                //打开连接
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql.ToString(), conn);
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                _logRecorder.WriteException(e);
            }
            finally
            {
                conn.Close();
                //conn.Dispose();
            }
            return result;
        }
        public int ExecuteBySql(StringBuilder sql)
        {
            return ExecuteBySql(sql, null);
        }
        public int ExecuteBySql(StringBuilder sql, object[] param)
        {
            int num = 0;
            //创建连接
            SQLiteConnection conn = (SQLiteConnection)this.GetInstance();
            try
            {
                conn.Open();
                //创建指令
                SQLiteCommand cmd = new SQLiteCommand(sql.ToString(), conn);
                SQLiteTransaction DbTrans = conn.BeginTransaction();
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }
                try
                {
                    cmd.Transaction = DbTrans;
                    num = cmd.ExecuteNonQuery();
                    DbTrans.Commit();
                }
                catch (Exception e)
                {
                    logger.Error(e.Message, e);
                    DbTrans.Rollback();
                    num = -1;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            finally
            {
                conn.Close();
             //   conn.Dispose();
            }
            return num;
        }

        public int BatchExecuteBySql(object[] sqls, object[] param)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetDataTableBySQL(StringBuilder sql)
        {
            return GetDataTableBySQL(sql, null);
        }

        public System.Data.DataTable GetDataTableBySQL(StringBuilder sql, object[] param)
        {

            DataTable dt = null;
            //创建连接
            SQLiteConnection conn = (SQLiteConnection)this.GetInstance();
            try
            {
                conn.Open();
                //创建指令
                SQLiteCommand cmd = new SQLiteCommand(sql.ToString(), conn);
                
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }
                try
                {
                    
                    System.Data.SQLite.SQLiteDataAdapter dao = new SQLiteDataAdapter(cmd);
                    dt = new DataTable();
                    dao.Fill(dt);                   
                    
                }
                catch (Exception e)
                {
                    this._logRecorder.WriteException(e);
                    Console.Write(_logRecorder.GetType().ToString());
                }
                //finally
                //{
                //    conn.Close();
                //    conn.Dispose();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Second" + e.Message);
              //  _logRecorder.WriteException(e);
            }
            finally
            {
                conn.Close();
                //   conn.Dispose();
            }
            return dt;
        }

        public System.Data.DataSet GetDataSetBySQL(StringBuilder sql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet GetDataSetBySQL(StringBuilder sql, object[] param)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IList GetDataListBySQL<T>(StringBuilder sql)
        {

            return GetDataListBySQL<T>(sql, null);
        }

        public System.Collections.IList GetDataListBySQL<T>(StringBuilder sql, object[] param)
        {
            SQLiteConnection conn = (SQLiteConnection)this.GetInstance();
            try
            {                
                conn.Open();
                _dbCommand = conn.CreateCommand();
                _dbCommand.CommandText = sql.ToString();
                _dbCommand.CommandType = CommandType.Text;
                if (param != null)
                {
                    _dbCommand.Parameters.AddRange(param);
                }
                IList list = new List<T>();
                IDataReader reader = _dbCommand.ExecuteReader();
                //return DbReader.ReaderToList<T>(GetIDataReaderBySql(sql, param));
                return DbReader.ReaderToList<T>(reader);
            }
            catch (Exception ex)
            {
                _logRecorder.WriteException(ex);
                return null;
            }
            finally
            {
                conn.Close();
                //conn.Dispose();
            }
        }

        public System.Data.DataTable GetPageList(string sql, object[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetPageList(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IList GetPageList<T>(string sql, object[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IList GetPageList<T>(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            throw new NotImplementedException();
        }

        public bool BulkInsert(System.Data.DataTable dt)
        {
            throw new NotImplementedException();
        }

        #region 根据 SQL 返回 IDataReader
        /// <summary>
        /// 根据 SQL 返回 IDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>IDataReader</returns>
        public IDataReader GetIDataReaderBySql(StringBuilder sql)
        {
            return GetIDataReaderBySql(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 IDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>IDataReader</returns>
        public IDataReader GetIDataReaderBySql(StringBuilder sql, object[] param)
        {
            try
            {

                using (SQLiteConnection conn = (SQLiteConnection)this.GetInstance())
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sql.ToString(), conn);
                    try
                    {
                        cmd.CommandTimeout = CommandTimeOut;
                        cmd.CommandType = CommandType.Text;
                        if(param!=null)
                        { 
                            cmd.Parameters.AddRange(param);
                        }
                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);                            
                    }
                    catch(Exception e)
                    {
                        _logRecorder.WriteException(e);
                        return null;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                       // conn.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                this._logRecorder.WriteException(e);
                return null;
            }
        }
        #endregion

        public void Dispose()
        {
            if(_db!=null )
            { 
                _db.Close();
                _db.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();  
                Console.WriteLine("Db Disposed");
            }
            if (_dbCommand != null)
            {
                _dbCommand.Dispose();
            }

            
        }
    }
}
