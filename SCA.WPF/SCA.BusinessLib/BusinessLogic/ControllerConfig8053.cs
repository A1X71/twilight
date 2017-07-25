/**************************************************************************
*
*  PROPRIETARY and CONFIDENTIAL
*
*  This file is licensed from, and is a trade secret of:
*
*                   Neat, Inc.
*                   No. 66, Xigang North Road
*                   Qinhuangdao City, Hebei Province, China
*                   Telephone: 0335-3660312
*                   WWW: www.neat.com.cn
*
*  Refer to your License Agreement for restrictions on use,
*  duplication, or disclosure.
*
*  Copyright © 2017-2018 Neat® Inc. All Rights Reserved. 
*
*  Unpublished - All rights reserved under the copyright laws of the China.
*  $Revision: 158 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-25 10:12:59 +0800 (周二, 25 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using SCA.Model;

namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerConfig8053 :ControllerConfigBase, IControllerConfig
    {
        private int _defaultDeviceTypeCode = 9;
        public Model.ControllerNodeModel[] GetNodes()
        {
            return new ControllerNodeModel[]
                {
                    new ControllerNodeModel(ControllerNodeType.Loop,"回路",3, @"Resources\Icon\Style1\Loop.jpg"),
                    new ControllerNodeModel(ControllerNodeType.Standard,"标准组态",3, @"Resources\Icon\Style1\Linkage.jpg"),  
                    new ControllerNodeModel(ControllerNodeType.Mixed,"混合组态",3, @"Resources\Icon\Style1\Linkage.jpg"),
                    new ControllerNodeModel(ControllerNodeType.General,"通用组态",3, @"Resources\Icon\Style1\Linkage.jpg"),
                    new ControllerNodeModel(ControllerNodeType.Board,"网络手动盘",3, @"Resources\Icon\Style1\Linkage.jpg"), 
                };
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            ////板卡号，手盘号，手键号暂定不在器件中处理
            ColumnConfigInfo[] columnDefinitionArray = new ColumnConfigInfo[13];
            ColumnConfigInfo code = new ColumnConfigInfo();
            columnDefinitionArray[0] = new ColumnConfigInfo();
            columnDefinitionArray[0].ColumnName = "编码";
            columnDefinitionArray[1] = new ColumnConfigInfo();
            columnDefinitionArray[1].ColumnName = "器件类型";            
            columnDefinitionArray[2] = new ColumnConfigInfo();
            columnDefinitionArray[2].ColumnName = "屏蔽";
            columnDefinitionArray[3] = new ColumnConfigInfo();
            columnDefinitionArray[3].ColumnName = "输出组1";
            columnDefinitionArray[4] = new ColumnConfigInfo();
            columnDefinitionArray[4].ColumnName = "输出组2";            
            columnDefinitionArray[5] = new ColumnConfigInfo();
            columnDefinitionArray[5].ColumnName = "报警浓度";
            columnDefinitionArray[6] = new ColumnConfigInfo();
            columnDefinitionArray[6].ColumnName = "预警浓度";
            columnDefinitionArray[7] = new ColumnConfigInfo();
            columnDefinitionArray[7].ColumnName = "延时";
            columnDefinitionArray[8] = new ColumnConfigInfo();
            columnDefinitionArray[8].ColumnName = "楼号";
            columnDefinitionArray[9] = new ColumnConfigInfo();
            columnDefinitionArray[9].ColumnName = "区号";
            columnDefinitionArray[10] = new ColumnConfigInfo();
            columnDefinitionArray[10].ColumnName = "层号";
            columnDefinitionArray[11] = new ColumnConfigInfo();
            columnDefinitionArray[11].ColumnName = "房间号";
            columnDefinitionArray[12] = new ColumnConfigInfo();
            columnDefinitionArray[12].ColumnName = "安装地点";
            return columnDefinitionArray;
        }

        public Model.ColumnConfigInfo[] GetStandardLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetGeneralLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetMixedLinkageConfigColumns()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 取得8036可以设置的器件类型信息
        /// </summary>
        /// <returns></returns>
        public string GetDeviceTypeCodeInfo()
        {
            string strMatchingDeviceNo = "0,9,31,32,36,89";
            return strMatchingDeviceNo;
        }
        /// <summary>
        /// 获取老版本软件的数据表名称
        /// </summary>
        /// <returns></returns>
        public string GetOldVersionTablesName8036()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;器件组态;";
            return tablesName;
        }

        public string GetOldVersionTablesName8001()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;器件组态;网络手控盘;通用组态;混合组态;";
            return tablesName;
        }
        public string GetOldVersionTablesName8000()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;器件组态;";
            return tablesName;
        }
        public string GetOldVersionTablesName8021()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;";
            return tablesName;
        }
        public string GetOldVersionTablesName8007()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;器件组态;";
            return tablesName;
        }
        public string GetOldVersionTablesName8003()
        {
            string tablesName = "";
            //1.从“系统设置”表中读取“回路”及“总数”
            //2.回路应作为表名
            tablesName = "系统设置;文件配置;位置信息;设备种类;器件组态;";
            return tablesName;
        }



        /// <summary>
        /// 起始为1
        /// </summary>
        /// <returns></returns>
        public short GetMaxLoopAmountValue()
        {
            return 8;
        }
        /// <summary>
        /// 起始为0
        /// </summary>
        /// <returns></returns>
        public short GetMaxMachineAmountValue(int deviceAddress)
        {
            return 95;
        }
        /// <summary>
        /// 起始为1
        /// </summary>
        /// <returns></returns>
        public short GetMaxDeviceAmountValue()
        {
            return 128;
        }


        public List<int> GetDeviceCodeLength()
        {
            List<int> lstDeviceCode = new List<int>();
            lstDeviceCode.Add(8);            
            return lstDeviceCode;
        }


        public short GetMaxAmountForStandardLinkageConfig()
        {
            return 150;
        }

        public Dictionary<string, RuleAndErrorMessage> GetStandardLinkageConfigRegularExpression()
        {
            Dictionary<string, RuleAndErrorMessage> dictExpressionAndInfo = new Dictionary<string, RuleAndErrorMessage>();
            //输出组号
            dictExpressionAndInfo.Add("Code", new RuleAndErrorMessage("^(00[1-9]|0[0-9][0-9]|1[0-4][0-9]|150)$", "输出组取值范围为000~150"));

            //联动模块 考虑是手输还是选择，如果选择就不需要验证

            //动作常数
            dictExpressionAndInfo.Add("ActionCoefficient", new RuleAndErrorMessage("^([1-5])$", "动作常数为1~5"));
            //联动组 与输出组号相同            

            //备注
            return dictExpressionAndInfo;
        }


        public short GetMaxAmountForMixedLinkageConfig()
        {
            return 4000;
        }

        public short GetMaxAmountForGeneralLinkageConfig()
        {
            return 2000;
        }

        public short GetMaxAmountForManualControlBoardConfig()
        {
            return 6804;
        }


        public override List<DeviceType> GetDeviceTypeInfo()
        {

            //string deviceType = GetDeviceTypeCodeInfo();
            //string[] validCode = deviceType.Split(',');

            //List<DeviceType> lstAllTypeInfo = base.GetALLDeviceTypeInfo(null);

            //List<DeviceType> lstResult = new List<DeviceType>();

            //for (int i = 0; i < validCode.Length; i++)
            //{
            //    var result = from t in lstAllTypeInfo where t.Code == Convert.ToInt32(validCode[i]) select t;
            //    lstResult.Add(result.FirstOrDefault());
            //}
            //return lstResult;
            string deviceType = GetDeviceTypeCodeInfo();
            return base.ConvertDeviceTypeCodeToDeviceType(deviceType);
        }


        public Dictionary<string, RuleAndErrorMessage> GetDeviceInfoRegularExpression(int addressLength)
        {
            Dictionary<string, RuleAndErrorMessage> dictDeviceInfoRE = new Dictionary<string, RuleAndErrorMessage>();
            //器件编码 自动生成，不需要验证
            //if (addressLength == 7)
            //{
            //    dictDeviceInfoRE.Add("DeviceCode", new RuleAndErrorMessage("^([06][0-3])$","器件编码为0~63"));
            //}
            //else
            //{
            //    dictDeviceInfoRE.Add("DeviceCode", new RuleAndErrorMessage("^([01][0-9][0-9])$", "器件编码为0~199"));
            //}
            //器件类型为“下拉框”，不需要验证

            //屏蔽
            dictDeviceInfoRE.Add("Disable", new RuleAndErrorMessage("^([01])$", "屏蔽取值范围为0或1"));

            //延时
            dictDeviceInfoRE.Add("DelayValue", new RuleAndErrorMessage("^([1-9]|[1-9][1-9]|1[0-7][0-9]|180)$", "延时取值范围为1~180"));

            //预警浓度
            dictDeviceInfoRE.Add("ForcastValue", new RuleAndErrorMessage("^(([0-9]|[1-9][0-9]|100){1})(\\.[0-9]){0,1}$", "预警浓度取值范围为0~100.9, 1位小数"));

            //报警浓度
            dictDeviceInfoRE.Add("AlertValue", new RuleAndErrorMessage("^(([0-9]|[1-9][0-9]|100){1})(\\.[0-9]){0,1}$", "报警浓度取值范围为0~100.9, 1位小数"));

            //输出组 001~150
            dictDeviceInfoRE.Add("StandardLinkageGroup", new RuleAndErrorMessage("^(00[1-9]|0[0-9][0-9]|1[0-4][0-9]|150)$", "输出组取值范围为000~150"));
            

            //允许楼、区、层、房间号同时为0，如果单独为0时，在其它规则里检查2017-04-20
            //楼 1~63
            dictDeviceInfoRE.Add("BuildingNo", new RuleAndErrorMessage("^([1-9]|[0-5][0-9]|[0-6][0-3]|0)$", "楼号取值范围为1~63"));
            //区1~99
            dictDeviceInfoRE.Add("ZoneNo", new RuleAndErrorMessage("^([1-9]|[0-9][0-9]|0)$", "区号取值范围为1~99"));
            //层 -9~-1 1~63
            dictDeviceInfoRE.Add("FloorNo", new RuleAndErrorMessage("^(-[1-9]|([1-9]|[0-5][0-9]|[0-6][0-3]|0))$", "层号取值范围为-9~63(不包括0)"));
            //房间号  1~255
            dictDeviceInfoRE.Add("RoomNo", new RuleAndErrorMessage("^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]|0)$", "房间号取值范围为1~255"));
            //安装地点 26个字母+数字+中文
            dictDeviceInfoRE.Add("Location", new RuleAndErrorMessage("^[A-Za-z0-9\u4E00-\u9FFF]{0,16}$", "安装地点为字母或汉字或中文,最长16位"));
            return dictDeviceInfoRE;
        }


        public short GetMaxAmountForBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForSubBoardNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }

        public short GetMaxAmountForKeyNoInManualControlBoardConfig()
        {
            throw new NotImplementedException();
        }


        public List<DeviceType> GetAllowedDeviceTypeInfoForAnyAlarm()
        {
            return null;
        }

        public List<DeviceType> GetAllowedDeviceTypeInfoForLinkageGroup8000()
        {
            string deviceType = base.GetAllowedDeviceTypeCodeInfoForLinkageGroup8000();
            return base.ConvertDeviceTypeCodeToDeviceType(deviceType);
        }

        public int DefaultDeviceTypeCode
        {
            get
            {
                return _defaultDeviceTypeCode;
            }
            set
            {
                _defaultDeviceTypeCode = value;
            }
        }


        public override List<string> GetFeatureList()
        {
            return null;
        }
        public override List<string> GetSensitiveLevelList()
        {
            return null;
        }
        //public List<int> GetActionCoefficient()
        //{
        //    throw new NotImplementedException();
        //}

        public override List<LinkageActionType> GetLinkageActionType()
        {
            return null;
        }

        public override List<LinkageType> GetLinkageTypeWithCastration()
        {
            return null;
        }

        public override List<LinkageType> GetLinkageType()
        {
            return null;
        }

        public List<DeviceType> GetDeviceTypeInfoWithAnyAlarm()
        {
            return null;
        }

        public List<DeviceType> GetDeviceTypeInfoWithoutFireDevice()
        {
            return null;
        }
    }
}
