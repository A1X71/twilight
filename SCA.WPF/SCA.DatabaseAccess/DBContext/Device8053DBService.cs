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
*  $Revision: 185 $
*  $Author: dennis_zhang $        
*  $Date: 2017-07-28 10:42:19 +0800 (周五, 28 七月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface.DatabaseAccess;
using SCA.Model;
using Neat.Dennis.Common.LoggerManager;
using System.Reflection;

namespace SCA.DatabaseAccess.DBContext
{
    public class Device8053DBService:IDeviceDBServiceTest
    {
        private static NeatLogger logger = new NeatLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IDBFileVersionService _dbFileVersionService;
        public Device8053DBService(IDBFileVersionService dbFileVersionService)
        {
            _dbFileVersionService = dbFileVersionService;
        }

        public bool CreateTableStructure()
        {
            try
            {
                List<String> lstTableName = _dbFileVersionService.GetTablesOfDB("DeviceInfo8053");
                if (lstTableName.Count == 0)//数据库中不存在表DeviceInfo8053
                {
                    _dbFileVersionService.CreateTableForDeviceInfoOfControllerType8053();
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
                return false;
            }
            return true;
        }

        public bool AddDevice(Model.LoopModel loop)
        {
            try
            {
                List<DeviceInfo8053> lstDevices = loop.GetDevices<DeviceInfo8053>();
                foreach (var device in lstDevices)
                {
                    device.Loop.ID = loop.ID;
                    device.LoopID = loop.ID;
                    _dbFileVersionService.AddDeviceForControllerType8053(device);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return false;
            }
            return true;
        }

        public int GetMaxID()
        {
            return _dbFileVersionService.GetMaxDeviceIDForControllerType8053();
        }


        public LoopModel GetDevicesByLoop(LoopModel loop)
        {
            return _dbFileVersionService.GetDevicesByLoopForControllerType8053(loop);
        }


        public bool DeleteAllDevicesByControllerID(int id)
        {
            if (_dbFileVersionService.DeleteAllDevicesByControllerIDForControllerType8053(id) > 0)
                return true;
            else
                return false;
        }


        public bool DeleteDeviceByID(int id)
        {
            if (_dbFileVersionService.DeleteDeviceByIDForControllerType8053(id) > 0)
                return true;
            else
                return false;
        }
    }
}
