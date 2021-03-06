﻿/**************************************************************************
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
*  $Revision: 212 $
*  $Author: dennis_zhang $        
*  $Date: 2017-08-01 16:23:47 +0800 (周二, 01 八月 2017) $
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SCA.WPF.Infrastructure;
namespace SCA.WPF.ViewsRoot.Views.DetailInfo
{
    /// <summary>
    /// DeviceInfo8053View.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfo8053View : UserControl
    {
        public DeviceInfo8053View()
        {
            InitializeComponent();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (new WaitCursor())
                {
                    SCA.Model.LoopModel loop = ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8053ViewModel)this.DataContext).TheLoop;
                    var selectedItems = DataGrid_Device.SelectedItems;
                    if (selectedItems != null)
                    {
                        SCA.BusinessLib.BusinessLogic.DeviceService8053 deviceService = new SCA.BusinessLib.BusinessLogic.DeviceService8053();
                        deviceService.TheLoop = loop;

                        foreach (SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8053 r in selectedItems)
                        {
                            if (r != null)
                            {
                                deviceService.DeleteBySpecifiedID(r.ID);
                            }
                        }
                        //刷新界面
                        ((SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.DeviceInfo8053ViewModel)this.DataContext).DeviceInfoObservableCollection = new SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo.EditableDeviceInfo8053Collection(loop, loop.GetDevices<Model.DeviceInfo8053>());
                    }
                }
            }
        }
    }
}
