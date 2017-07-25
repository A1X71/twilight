using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
namespace SCA.Interface.DatabaseAccess
{
    /// <summary>
    /// Author: William
    /// Created Date: 2016-10-20
    /// Description: 定义数据库操作接口
    /// </summary>
    public interface IDatabaseService:IDisposable
    {
        void CreateDBFile();
        #region 根据 SQL 返回影响行数
        /// <summary>
        /// 根据SQL返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        object GetObjectValue(StringBuilder sql);
        object GetObjectValue(StringBuilder sql, object[] param);        
        #endregion

        #region 根据 SQL 执行
        /// <summary>
        ///  根据SQL执行
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>object</returns>
        int ExecuteBySql(StringBuilder sql);
        int ExecuteBySql(StringBuilder sql, object[] param);  
        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="sqls">sql语句</param>
        /// <param name="m_param">参数化</param>
        /// <returns></returns>
        int BatchExecuteBySql(object[] sqls, object[] param);
        #endregion

        #region 根据 SQL 返回 DataTable 数据集
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTableBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTableBySQL(StringBuilder sql, object[] param);
        #endregion
        
        #region 根据 SQL 返回 DataSet 数据集
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetBySQL(StringBuilder sql, object[] param);
        #endregion        

        #region 根据 SQL 返回 IList
        /// <summary>
        /// 根据 SQL 返回 IList
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 IList,带参数 (比DataSet效率高)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql, object[] param);
        #endregion        

        #region 数据分页 返回 DataTable
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     param：参数化
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        DataTable GetPageList(string sql, object[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        DataTable GetPageList(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        #endregion

        #region 数据分页 返回 IList
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     param：参数化
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        IList GetPageList<T>(string sql, object[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        IList GetPageList<T>(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        #endregion

        #region SqlBulkCopy 批量数据处理
        /// <summary>
        ///大批量数据插入
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        bool BulkInsert(DataTable dt);
        #endregion

    }
}
