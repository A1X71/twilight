using System;
using System.Text;
using System.Collections.Generic;
using SCA.Model;
using SCA.Interface.DatabaseAccess;

/* ==============================
*
* Author     : William
* Create Date: 2017/5/3 8:48:53
* FileName   : LinkageConfigGeneralDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class LinkageConfigGeneralDBService:ILinkageConfigGeneralDBService
    {
        private IDatabaseService _databaseService;
        private IDBFileVersionService _dbFileVersionService;
        public LinkageConfigGeneralDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public LinkageConfigGeneralDBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }

        public Model.LinkageConfigGeneral GetGeneralLinkageConfigInfo(int id)
        {
            throw new System.NotImplementedException();
        }

        public Model.LinkageConfigGeneral GetGeneralLinkageConfigInfo(Model.LinkageConfigGeneral linkageConfigGeneral)
        {
            throw new System.NotImplementedException();
        }

        public bool AddGeneralLinkageConfigInfo(Model.LinkageConfigGeneral linkageConfigGeneral)
        {
            int intEffectiveRows = 0;
            try
            {
                //StringBuilder sbSQL = new StringBuilder("REPLACE INTO  LinkageConfigGeneral(ID,Code,ActionCoefficient,BuildingNoA,ZoneNoA, LayerNoA1 , LayerNoA2 , DeviceTypeCodeA ,TypeC,MachineNoC ,LoopNoC,DeviceCodeC ,BuildingNoC,ZoneNoC, LayerNoC ,DeviceTypeCodeC ,controllerID)");                
                //sbSQL.Append(" VALUES(");
                //sbSQL.Append(linkageConfigGeneral.ID + ",'");
                //sbSQL.Append(linkageConfigGeneral.Code + "','");
                //sbSQL.Append(linkageConfigGeneral.ActionCoefficient + "','");
                //sbSQL.Append(linkageConfigGeneral.BuildingNoA + "','");
                //sbSQL.Append(linkageConfigGeneral.ZoneNoA + "','");
                //sbSQL.Append(linkageConfigGeneral.LayerNoA1 + "','");
                //sbSQL.Append(linkageConfigGeneral.LayerNoA2 + "','");
                //sbSQL.Append(linkageConfigGeneral.DeviceTypeCodeA + "','");
                //sbSQL.Append((int)linkageConfigGeneral.TypeC + "','");
                //sbSQL.Append(linkageConfigGeneral.MachineNoC + "','");
                //sbSQL.Append(linkageConfigGeneral.LoopNoC + "','");
                //sbSQL.Append(linkageConfigGeneral.DeviceCodeC + "','");
                //sbSQL.Append(linkageConfigGeneral.BuildingNoC + "','");
                //sbSQL.Append(linkageConfigGeneral.ZoneNoC + "','");
                //sbSQL.Append(linkageConfigGeneral.LayerNoC + "','");
                //sbSQL.Append(linkageConfigGeneral.DeviceTypeCodeC + "',");
                //sbSQL.Append(linkageConfigGeneral.ControllerID + ");");
                //intEffectiveRows = _databaseService.ExecuteBySql(sbSQL);
                intEffectiveRows= _dbFileVersionService.AddGeneralLinkageConfigInfo(linkageConfigGeneral);
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

        public bool AddGeneralLinkageConfigInfo(List<Model.LinkageConfigGeneral> lstLinkageConfigGeneral)
        {
            try
            {
                foreach (var linkageConfig in lstLinkageConfigGeneral)
                {
                    AddGeneralLinkageConfigInfo(linkageConfig);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateGeneralLinkageConfigInfo(Model.LinkageConfigGeneral lstLinkageConfigGeneral)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteGeneralLinkageConfigInfo(int id)
        {
            throw new System.NotImplementedException();
        }


        public List<LinkageConfigGeneral> GetGeneralLinkageConfigInfo(Model.ControllerModel controller)
        {

            return _dbFileVersionService.GetGeneralLinkageConfig(controller);
            //List<LinkageConfigGeneral> lstData = new List<LinkageConfigGeneral>();
            //StringBuilder sbQuerySQL = new StringBuilder("select ID,Code,ActionCoefficient,BuildingNoA,ZoneNoA, LayerNoA1 , LayerNoA2 , DeviceTypeCodeA ,TypeC,MachineNoC ,LoopNoC,DeviceCodeC ,BuildingNoC,ZoneNoC, LayerNoC ,DeviceTypeCodeC ,controllerID from LinkageConfigGeneral where controllerID=" + controller.ID);
            //System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    LinkageConfigGeneral model = new LinkageConfigGeneral();
            //    model.ID = Convert.ToInt32(dt.Rows[i]["id"]);
            //    model.Code = dt.Rows[i]["Code"].ToString();
            //    model.ActionCoefficient =Convert.ToInt32( dt.Rows[i]["ActionCoefficient"]);
            //    model.BuildingNoA = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["BuildingNoA"]));
            //    model.ZoneNoA = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["ZoneNoA"]));
            //    model.LayerNoA1 = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoA1"]));
            //    model.LayerNoA2 = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoA2"]));
            //    model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);                
            //    model.TypeC = (LinkageType)Enum.ToObject(typeof(LinkageType),Convert.ToInt16(dt.Rows[i]["TypeC"]));
            //    model.MachineNoC = dt.Rows[i]["MachineNoC"].ToString();
            //    model.LoopNoC = dt.Rows[i]["LoopNoC"].ToString();
            //    model.DeviceCodeC = dt.Rows[i]["DeviceCodeC"].ToString();
            //    model.BuildingNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["BuildingNoC"]));
            //    model.ZoneNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["ZoneNoC"]));
            //    model.LayerNoC = new Nullable<int>(Convert.ToInt32(dt.Rows[i]["LayerNoC"]));
            //    model.DeviceTypeCodeC = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeC"]); 
            //    model.Controller = controller;
            //    model.ControllerID = controller.ID;
            //    lstData.Add(model);
            //}
            //return lstData;            
        }
    }
}
