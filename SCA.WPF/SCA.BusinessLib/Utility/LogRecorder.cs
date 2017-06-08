using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SCA.Interface;
namespace SCA.BusinessLib.Utility
{
    /// <summary>
    /// 日志记录类别
    /// william 
    /// 2016-10-22
    /// </summary>
    public enum LogCategory { 
        Database,
        SerialPort,//="SerialPortLog",
        Common//="CommonLog"
    }
    /// <summary>
    /// 日志处理
    /// william 
    /// 2016-10-22
    /// </summary>
    public class LogRecorder:ILogRecorder
    {        
        private StreamWriter _sw;
        private const string _exceptionFilePath = "E:\\Exception.txt";
        private IFileService _fileService;
        public string FilePath { get; set; }

        public LogRecorder(IFileService fileService)
        {            
            _fileService = fileService;
        }       

        //static void Write 
        public void WriteException(Exception exception)
        {           
            
            DateTime DateNow = new DateTime();
            try
            {                
                _fileService.CreateFile(_exceptionFilePath);
                FileOpen(_exceptionFilePath);
                DateNow = DateTime.Now;
                _sw.WriteLine("***********************************************************************");
                _sw.WriteLine(DateNow.ToString("HH:mm:ss"));
                _sw.WriteLine("输出信息：错误信息");
                if (exception.Message != null)
                {
                    _sw.WriteLine("异常信息：\r\n" + exception.Message + "-->" + exception.Source);
                }
                _sw.Flush();
                _sw.Close();
            }
            catch (System.Exception e)
            {
               Console.WriteLine("WriteException"+e.ToString());
            }
            finally
            {
                FileClose();
            }
        }

        public void WriteLog(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void InitialLog(string strPath)
        {
            throw new NotImplementedException();
        }
        //打开文件准备写入
        private void FileOpen(string filePath)
        {
            try
            {
                _sw = new StreamWriter(filePath, true);
            }
            catch (Exception ex)
            {

                Console.WriteLine("FileOpen-->"+ex.Message);
            }
        }
        //关闭打开的日志文件
        private void FileClose()
        {
            if (_sw != null)
            {         
                _sw.Dispose();
                _sw = null;
            }
        }
    }
}
