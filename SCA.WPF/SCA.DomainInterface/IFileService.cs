using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SCA.Interface
{
    /// <summary>
    /// Author: William
    /// Created Date: 2016-10-20
    /// Description: 定义文件操作接口
    /// </summary>
    public interface IFileService
    {
        string FilePath { get; set; }
        #region 检测指定文件夹是否存在,如果存在返回true
        bool IsExistDirectory(string directoryPath);
        #endregion
        #region 检测指定文件是否存在,如果存在返回true
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param> 
        /// 存在返回true; 不存在返回false;
        bool IsExistFile(string filePath);
        #endregion
        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void CreateFile(string filePath);
        #endregion
        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void DeleteFile(string filePath);
        #endregion

        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <param name="filePath">目标文件路径</param>
        /// <param name="filePath">是否可覆盖</param>
        void Copy(string sourceFileName, string destFileName, bool overwrite);
        #endregion

        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        void CreateDir(string dir);
        #endregion
        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        void DeleteDir(string dir);     
        #endregion
        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        string GetFileName(string filePath);    
        #endregion
        #region 从文件的绝对路径中获取扩展名
        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        string GetExtension(string filePath);
     
        #endregion
        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        string GetFileNameNoExtension(string filePath);
       
        #endregion
        #region 保存文件
                /// <summary>
                /// 将流保存为文件
                /// </summary>
                /// <param name="ms">字节流</param>
                /// <param name="fileName">保存文件名</param>
                void SaveToFile(MemoryStream ms, string fileName);
        #endregion 

    }
}
