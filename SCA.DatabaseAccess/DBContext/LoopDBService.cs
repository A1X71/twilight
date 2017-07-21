using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/24 9:26:05
* FileName   : LoopDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class LoopDBService:ILoopDBService
    {
        private IDatabaseService _databaseService;
        private IDBFileVersionService _dbFileVersionService;
        public LoopDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public LoopDBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }
        public Model.LoopModel GetLoopInfo(int id)
        {
            throw new NotImplementedException();
        }

        public Model.LoopModel GetLoopInfo(Model.LoopModel loop)
        {
            throw new NotImplementedException();
        }

    

        public int UpdateLoopInfo(Model.LoopModel loop)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLoopInfo(int loopID)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM Loop where id= " + loopID + ";");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;            
            if (_dbFileVersionService.DeleteLoopInfo(loopID)> 0)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool AddLoopInfo(Model.LoopModel loop)
        {
            try
            {
                //StringBuilder sbLoopSQL;
                ////此处未显示加入ID,由SQLite自动生成
                //sbLoopSQL = new StringBuilder("Replace into Loop(ID,Code,Name,DeviceAmount,controllerID) values(");
                //sbLoopSQL.Append(loop.ID + ",'");
                //sbLoopSQL.Append(loop.Code + "','");
                //sbLoopSQL.Append(loop.Name + "',");
                //sbLoopSQL.Append(loop.DeviceAmount + ",");
                //sbLoopSQL.Append(loop.ControllerID + ");");
                //_databaseService.ExecuteBySql(sbLoopSQL);
                _dbFileVersionService.AddLoopInfo(loop);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool AddLoopInfo(List<Model.LoopModel> lstLoop)
        {
            try
            {                    
                foreach (var loop in lstLoop)
                {
                    AddLoopInfo(loop);
                }                
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 取得当前最大的ID号
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            //StringBuilder sbProjectSQL = new StringBuilder("SELECT MAX(id) FROM Loop;");
            //return Convert.ToInt16(_databaseService.GetObjectValue(sbProjectSQL));
            return _dbFileVersionService.GetMaxIDFromLoop();
        }


        public List<Model.LoopModel> GetLoopsByController(Model.ControllerModel controller)
        {
            //List<LoopModel> lstLoops = new List<LoopModel>();
            //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,Name,DeviceAmount from Loop where controllerID=" + controller.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Model.LoopModel model = new Model.LoopModel();
            //    model.ID = Convert.ToInt16(dt.Rows[i]["id"]);
            //    model.Code = dt.Rows[i]["Code"].ToString();
            //    model.Name = dt.Rows[i]["Name"].ToString();
            //    model.DeviceAmount = Convert.ToInt16(dt.Rows[i]["DeviceAmount"]);
            //    model.Controller = controller;
            //    model.ControllerID = controller.ID;
            //    lstLoops.Add(model);
            //}
            //return lstLoops;     
            return _dbFileVersionService.GetLoopsByController(controller);
        }


        public bool DeleteLoopsByControllerID(int controllerID)
        {
            //StringBuilder sbSQL = new StringBuilder("Delete FROM Loop where controllerid= " + controllerID + ";");
            //if (_databaseService.ExecuteBySql(sbSQL) > 0)
            //    return true;
            //else
            //    return false;
            
            if (_dbFileVersionService.DeleteLoopsByControllerID(controllerID) > 0)
                return true;
            else
                return false;
        }
    }
}
