using System;
using System.Text;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/3 8:47:54
* FileName   : LinkageConfigStandardDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class LinkageConfigStandardDBService:ILinkageConfigStandardDBService
    {
        private IDatabaseService _databaseService;
        private IDBFileVersionService _dbFileVersionService;
        public LinkageConfigStandardDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public LinkageConfigStandardDBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }
        public Model.LinkageConfigStandard GetStandardLinkageConfigInfo(int id)
        {
            throw new System.NotImplementedException();
        }

        public Model.LinkageConfigStandard GetStandardLinkageConfigInfo(Model.LinkageConfigStandard linkageConfigStandard)
        {
            throw new System.NotImplementedException();
        }

        public bool AddStandardLinkageConfigInfo(Model.LinkageConfigStandard linkageConfigStandard)
        {
            int intEffectiveRows = 0;
            try
            {
                //StringBuilder sbSQL = new StringBuilder("REPLACE INTO LinkageConfigStandard(ID,Code,DeviceNo1,DeviceNo2 , DeviceNo3 , DeviceNo4, DeviceNo5,DeviceNo6,DeviceNo7,DeviceNo8,DeviceNo9,DeviceNo10, ActionCoefficient , LinkageNo1 ,LinkageNo2 ,LinkageNo3,controllerID) ");
                //sbSQL.Append("VALUES(");
                //sbSQL.Append(linkageConfigStandard.ID + ",'");
                //sbSQL.Append(linkageConfigStandard.Code+"','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo1 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo2 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo3 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo4 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo5 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo6 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo7 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo8 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo9 + "','");
                //sbSQL.Append(linkageConfigStandard.DeviceNo10 + "','");
                //sbSQL.Append(linkageConfigStandard.ActionCoefficient + "','");
                //sbSQL.Append(linkageConfigStandard.LinkageNo1 + "','");
                //sbSQL.Append(linkageConfigStandard.LinkageNo2 + "','");
                //sbSQL.Append(linkageConfigStandard.LinkageNo3 + "',");
                //sbSQL.Append(linkageConfigStandard.ControllerID + ");");
                //intEffectiveRows = _databaseService.ExecuteBySql(sbSQL);
                intEffectiveRows = _dbFileVersionService.AddStandardLinkageConfigInfo(linkageConfigStandard);

            }
            catch
            {
                intEffectiveRows = 0;
            }
            if (intEffectiveRows > 0)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

        public bool AddStandardLinkageConfigInfo(List<Model.LinkageConfigStandard> lstLinkageConfigStandard)
        {
            try
            {
                foreach (var linkageConfig in lstLinkageConfigStandard)
                {
                    AddStandardLinkageConfigInfo(linkageConfig);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateStandardLinkageConfigInfo(Model.LinkageConfigStandard lstLinkageConfigStandard)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteStandardLinkageConfigInfo(int id)
        {
            if (_dbFileVersionService.DeleteStandardLinkageConfigInfo(id) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<Model.LinkageConfigStandard> GetStandardLinkageConfigInfoByController(Model.ControllerModel controller)
        {
            
                //List<LinkageConfigStandard> lstData = new List<LinkageConfigStandard>();
                //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,DeviceNo1,DeviceNo2 , DeviceNo3 , DeviceNo4, DeviceNo5,DeviceNo6,DeviceNo7,DeviceNo8,DeviceNo9,DeviceNo10, ActionCoefficient , LinkageNo1 ,LinkageNo2 ,LinkageNo3,controllerID from LinkageConfigStandard where controllerID=" + controller.ID);
                //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    LinkageConfigStandard model = new LinkageConfigStandard();
                //    model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
                //    model.Code = dt.Rows[i]["Code"].ToString();
                //    model.DeviceNo1 = dt.Rows[i]["DeviceNo1"].ToString();
                //    model.DeviceNo2 = dt.Rows[i]["DeviceNo2"].ToString();
                //    model.DeviceNo3 = dt.Rows[i]["DeviceNo3"].ToString();
                //    model.DeviceNo4 = dt.Rows[i]["DeviceNo4"].ToString();
                //    model.DeviceNo5 = dt.Rows[i]["DeviceNo5"].ToString();
                //    model.DeviceNo6 = dt.Rows[i]["DeviceNo6"].ToString();
                //    model.DeviceNo7 = dt.Rows[i]["DeviceNo7"].ToString();
                //    model.DeviceNo8 = dt.Rows[i]["DeviceNo8"].ToString();
                //    model.DeviceNo9 = dt.Rows[i]["DeviceNo9"].ToString();
                //    model.DeviceNo10 = dt.Rows[i]["DeviceNo10"].ToString();
                //    model.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["ActionCoefficient"]);
                //    model.LinkageNo1 = dt.Rows[i]["LinkageNo1"].ToString();
                //    model.LinkageNo2 = dt.Rows[i]["LinkageNo2"].ToString();
                //    model.LinkageNo3 = dt.Rows[i]["LinkageNo3"].ToString();
                //    model.Controller = controller;
                //    model.ControllerID = controller.ID;
                //    lstData.Add(model);
                //}
                //return lstData;
            return _dbFileVersionService.GetStandardLinkageConfig(controller);
            
        }
    }
}
