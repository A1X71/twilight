using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SCA.Interface;
using System.IO;
using System.Xml.Serialization;
/* ==============================
*
* Author     : William
* Create Date: 2016/10/24 15:07:02
* FileName   : FileService
* Description:操作文件
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.Utility
{
    public class FileService:IFileService,IDisposable 
    {
        private string _filePath;
        public string FilePath { get; set; }
        public bool IsExistDirectory(string directoryPath)
        {
            
            return Directory.Exists(directoryPath);
        }
        /// <summary>
        /// 是否存在文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>true:存在; false:不存在</returns>
        public bool IsExistFile(string filePath)
        {
            
            _filePath = filePath;
            return File.Exists(_filePath);
        }

        public void CreateDir(string dir)
        {
            if(!IsExistFile(dir))
            { 
                Directory.CreateDirectory(dir);
            }            
        }
        

        public void DeleteDir(string dir)
        {
            throw new NotImplementedException();
        }

        public string GetFileName(string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetExtension(string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetFileNameNoExtension(string filePath)
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(System.IO.MemoryStream ms,string filePath)
        {

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    data = null;
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public void DeleteFile(string filePath)
        {        
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        
        }

        public void CreateFile(string filePath)
        {
            try
            {
                //Console.WriteLine("Create File"+filePath);
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath).Dispose();
                }
            }
            catch (Exception ex)
            {                
                Console.WriteLine("CreateFile:-- > "+ex.Message);
            }
        }

        public void Dispose()
        {
          //  GC.SuppressFinalize(this);
        }


        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }
        public StringCollection ReadStringCollectionFromXML(string filePath)
        {
            StringCollection stringCollection = new StringCollection();
            try
            {
                
                if (File.Exists(filePath))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(StringCollection));
                    using (TextReader reader = new StreamReader(filePath))
                    {
                        stringCollection = (StringCollection)ser.Deserialize(reader);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return stringCollection;
        }
        public void WriteStringCollectionToXML(string filePath, StringCollection stringCollection)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(StringCollection));
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    ser.Serialize(writer, stringCollection);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
