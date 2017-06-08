using System.Collections.Generic;
using System.Text;
using System;
using SCA.Model;
using SCA.Interface.DatabaseAccess;


/* ==============================
*
* Author     : William
* Create Date: 2017/5/3 8:49:14
* FileName   : LinkageConfigMixedDBService
* Description:
* Version：V1
* ===============================
*/
namespace SCA.DatabaseAccess.DBContext
{
    public class LinkageConfigMixedDBService:ILinkageConfigMixedDBService
    {
        private IDatabaseService _databaseService;
        public LinkageConfigMixedDBService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public Model.LinkageConfigMixed GetMixedLinkageConfigInfo(int id)
        {
            throw new System.NotImplementedException();
        }

        public Model.LinkageConfigMixed GetMixedLinkageConfigInfo(Model.LinkageConfigMixed linkageConfigMixed)
        {
            throw new System.NotImplementedException();
        }

        public bool AddMixedLinkageConfigInfo(Model.LinkageConfigMixed linkageConfigMixed)
        {
            int intEffectiveRows = 0;
            try
            {
                StringBuilder sbSQL = new StringBuilder("REPLACE INTO  LinkageConfigMixed(ID,Code, ActionCoefficient,ActionType, TypeA, LoopNoA, DeviceCodeA ,BuildingNoA,ZoneNoA , LayerNoA , DeviceTypeCodeA,TypeB,LoopNoB,DeviceCodeB,BuildingNoB ,ZoneNoB , LayerNoB , DeviceTypeCodeB  ,TypeC ,MachineNoC,LoopNoC ,DeviceCodeC ,BuildingNoC ,ZoneNoC , LayerNoC  ,DeviceTypeCodeC  ,controllerID)");                
                sbSQL.Append(" VALUES(");
                sbSQL.Append(linkageConfigMixed.ID + ",'");
                sbSQL.Append(linkageConfigMixed.Code + "','");
                sbSQL.Append(linkageConfigMixed.ActionCoefficient + "','");
                sbSQL.Append((int)linkageConfigMixed.ActionType + "','");
                sbSQL.Append((int)linkageConfigMixed.TypeA + "','");
                sbSQL.Append(linkageConfigMixed.LoopNoA + "','");
                sbSQL.Append(linkageConfigMixed.DeviceTypeCodeA + "','");
                sbSQL.Append(linkageConfigMixed.BuildingNoA + "','");
                sbSQL.Append(linkageConfigMixed.ZoneNoA + "','");
                sbSQL.Append(linkageConfigMixed.LayerNoA + "','");
                sbSQL.Append(linkageConfigMixed.DeviceTypeCodeA + "','");
                sbSQL.Append((int)linkageConfigMixed.TypeB + "','");
                sbSQL.Append(linkageConfigMixed.LoopNoB + "','");
                sbSQL.Append(linkageConfigMixed.DeviceCodeB + "','");
                sbSQL.Append(linkageConfigMixed.BuildingNoB + "','");
                sbSQL.Append(linkageConfigMixed.ZoneNoB + "','");
                sbSQL.Append(linkageConfigMixed.LayerNoB + "','");
                sbSQL.Append(linkageConfigMixed.DeviceTypeCodeB + "','");
                sbSQL.Append((int)linkageConfigMixed.TypeC + "','");
                sbSQL.Append(linkageConfigMixed.MachineNoC + "','");
                sbSQL.Append(linkageConfigMixed.LoopNoC + "','");
                sbSQL.Append(linkageConfigMixed.DeviceCodeC + "','");
                sbSQL.Append(linkageConfigMixed.BuildingNoC + "','");
                sbSQL.Append(linkageConfigMixed.ZoneNoC + "','");
                sbSQL.Append(linkageConfigMixed.LayerNoC + "','");
                sbSQL.Append(linkageConfigMixed.DeviceTypeCodeC + "',");
                sbSQL.Append(linkageConfigMixed.ControllerID + ");");
                intEffectiveRows = _databaseService.ExecuteBySql(sbSQL);
                
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

        public bool AddMixedLinkageConfigInfo(List<Model.LinkageConfigMixed> lstLinkageConfigMixed)
        {
            try
            {
                foreach (var linkageConfig in lstLinkageConfigMixed)
                {
                    AddMixedLinkageConfigInfo(linkageConfig);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int UpdateMixedLinkageConfigInfo(Model.LinkageConfigMixed lstLinkageConfigMixed)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteMixedLinkageConfigInfo(int id)
        {
            throw new System.NotImplementedException();
        }


        public List<LinkageConfigMixed> GetMixedLinkageConfigInfo(Model.ControllerModel controller)
        {

            List<LinkageConfigMixed> lstData = new List<LinkageConfigMixed>();
            StringBuilder sbQuerySQL = new StringBuilder("select ID,Code, ActionCoefficient,ActionType, TypeA, LoopNoA, DeviceCodeA ,BuildingNoA,ZoneNoA , LayerNoA , DeviceTypeCodeA,TypeB,LoopNoB,DeviceCodeB,BuildingNoB ,ZoneNoB , LayerNoB , DeviceTypeCodeB  ,TypeC ,MachineNoC,LoopNoC ,DeviceCodeC ,BuildingNoC ,ZoneNoC , LayerNoC  ,DeviceTypeCodeC  ,controllerID from LinkageConfigMixed where controllerID=" + controller.ID);
            System.Data.DataTable dt = _databaseService.GetDataTableBySQL(sbQuerySQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LinkageConfigMixed model = new LinkageConfigMixed();
                model.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                model.Code = dt.Rows[i]["Code"].ToString();
                model.ActionCoefficient = Convert.ToInt32(dt.Rows[i]["ActionCoefficient"]);
                model.ActionType =  (LinkageActionType)Enum.ToObject(typeof(LinkageActionType),Convert.ToInt16(dt.Rows[i]["ActionType"]));
                model.TypeA = (LinkageType)Enum.ToObject(typeof(LinkageType),Convert.ToInt16(dt.Rows[i]["TypeA"]));
                model.LoopNoA = dt.Rows[i]["LoopNoA"].ToString();
                model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);
                model.BuildingNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoA"]));
                model.ZoneNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoA"]));
                model.LayerNoA = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoA"]));
                model.DeviceTypeCodeA = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeA"]);
                model.TypeB = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeB"]));
                model.LoopNoB = dt.Rows[i]["LoopNoB"].ToString();
                model.DeviceCodeB = dt.Rows[i]["DeviceCodeB"].ToString();
                model.BuildingNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoB"]));
                model.ZoneNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoB"]));
                model.LayerNoB = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoB"]));
                model.DeviceTypeCodeB = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeB"]);
                model.TypeC = (LinkageType)Enum.ToObject(typeof(LinkageType), Convert.ToInt16(dt.Rows[i]["TypeC"]));
                model.MachineNoC = dt.Rows[i]["MachineNoC"].ToString();
                model.LoopNoC = dt.Rows[i]["LoopNoC"].ToString();
                model.DeviceCodeC = dt.Rows[i]["DeviceCodeC"].ToString();
                model.BuildingNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["BuildingNoC"]));
                model.ZoneNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["ZoneNoC"]));
                model.LayerNoC = new Nullable<Int16>(Convert.ToInt16(dt.Rows[i]["LayerNoC"]));
                model.DeviceTypeCodeC = Convert.ToInt16(dt.Rows[i]["DeviceTypeCodeC"]);
                model.Controller = controller;
                model.ControllerID = controller.ID;
                lstData.Add(model);
            }
            return lstData;
        }
    }
}
