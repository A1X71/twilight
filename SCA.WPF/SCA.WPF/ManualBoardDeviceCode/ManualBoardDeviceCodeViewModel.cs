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
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using SCA.WPF.Utility;
using SCA.Model;
using SCA.Interface;
using SCA.Model.BusinessModel;
using SCA.BusinessLib.BusinessLogic;
using System.Collections.Generic;
using Caliburn.Micro;

namespace SCA.WPF.ManualBoardDeviceCode
{
    public class ManualBoardDeviceCodeViewModel:PropertyChangedBase
    {
        #region 属性
        public ManualControlBoard MCB { get; set; } //需要设置“器件编号”的手控盘对象
        public int ID { get; set; }

        private string startCode = "00000000";
        public string StartCode
        {
            get { return startCode; }
            set
            {
                startCode = value;
                NotifyOfPropertyChange("StartCode");
            }
        }

        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                NotifyOfPropertyChange("SelectedIndex");
            }
        }

        private string endCode = "00000000";
        public string EndCode
        {
            get { return endCode; }
            set
            {
                endCode = value;
                NotifyOfPropertyChange("EndCode");
            }
        }

        public string GetResultCode()
        {
            string result = string.Empty;
            string separator = ",";
            if(SelectedIndex == 1)
            {
                separator = "~";
            }
            result = StartCode + separator + EndCode;
            return result;
        }
        #endregion
        #region 命令
        public ICommand ConfirmCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(ConfirmExecute, null); }
        }
        public void ConfirmExecute()
        {
            SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshDeviceCode", this);
        }
        public ICommand CloseCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(CloseExecute, null); }
        }
        public void CloseExecute()
        {
            ID = -1;
            SCA.WPF.Infrastructure.EventMediator.NotifyColleagues("RefreshDeviceCode", this);
        }
        #endregion
       
    }
}
