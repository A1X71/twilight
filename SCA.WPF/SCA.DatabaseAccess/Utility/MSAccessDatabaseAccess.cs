using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Interface;
using System.Data.OleDb;
using System.Data;
using System.Collections;
/* ==============================
*
* Author     : William
* Create Date: 2016/10/26 15:40:54
* FileName   : MSAccessDatabaseAccess
* Description:微软Access数据库处理类
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess
{
    public class MSAccessDatabaseAccess:IDatabaseService
    {
        private static Object _locker = new Object(); //并行锁        
        private OleDbConnection _db = null;
        private ILogRecorder _logRecorder;            //日志处理
        private IFileService _fileService;            //文件处理
        private OleDbCommand _dbCommand = null;       //Command命令
        protected string connectionString = "";       //数据库连接串
        public MSAccessDatabaseAccess(string connString, ILogRecorder logRecorder, IFileService fileService)
        {
            connectionString = connString;
            _logRecorder = logRecorder;
            _fileService = fileService;
        }
        /// <summary>
        /// 获取数据为连接对象
        /// </summary>
        /// <returns></returns>
        public OleDbConnection GetInstance()
        {
            if (_db == null)
            {
                //"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=c:\App1\你的数据库名.mdb; User ID=admin; Password="
                return _db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + connectionString + "; User ID=; Password=");//("Driver= {MicrosoftAccessDriver(*.mdb)};DBQ="+_connectionString+";Uid=;Pwd=");                
                
            }
            else
            {
                lock (_locker)
                {
                    return _db;
                }
            }            
        }

        public object GetObjectValue(StringBuilder sql)
        {
            return GetObjectValue(sql, null);            
        }
        public object GetObjectValue(StringBuilder sql, object[] param)
        {
            object result=null;
            OleDbConnection conn  = (OleDbConnection)this.GetInstance();
            try
            {
                //打开连接
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql.ToString(), conn);
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
            OleDbConnection conn = (OleDbConnection)this.GetInstance();
            try
            {
                conn.Open();
                //创建指令
                String strSQL = sql.ToString();
                String[] arraySql = strSQL.Split(';');
                for (int i = 0; i < arraySql.Length; i++)
                { 
                    OleDbCommand cmd = new OleDbCommand(arraySql[i].ToString(), conn);
                    OleDbTransaction DbTrans = conn.BeginTransaction();
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    try
                    {
                        Console.WriteLine("tryExecute");
                        cmd.Transaction = DbTrans;
                        num = cmd.ExecuteNonQuery();
                        DbTrans.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("first:" + e.Message);
                        DbTrans.Rollback();
                        num = -1;
                        this._logRecorder.WriteException(e);
                        Console.Write(_logRecorder.GetType().ToString());

                    }
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
                _logRecorder.WriteException(e);
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

            OleDbConnection conn = (OleDbConnection)this.GetInstance();
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql.ToString();
                cmd.CommandType = CommandType.Text;
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }

                return DbReader.ReaderToDataTable(cmd.ExecuteReader());
            }
            catch (Exception e)
            {
                this._logRecorder.WriteException(e);
                return null;
            }
            finally
            {
                conn.Close();
            }
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
            throw new NotImplementedException();        
        }

        public System.Collections.IList GetDataListBySQL<T>(StringBuilder sql, object[] param)
        {
            throw new NotImplementedException();
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void CreateDBFile()
        {
            throw new NotImplementedException();
        }
    }
}
