using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/2/23 11:15:26
* FileName   : DBSchemaDefinition
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    class DBSchemaDefinition
    {

        /// <summary>
        /// 获取SQLite中的
        /// </summary>
        /// <returns></returns>
        public static string GetTablesNameSQL()
        {
            return "select name from sqlite_master where type='table';";
        }
        public static string GetTableNameBySpecificValueSQL( string tableName)
        {
            return "select name from sqlite_master where type='table' and  name= '" + tableName + "';";
        }

    }
}
