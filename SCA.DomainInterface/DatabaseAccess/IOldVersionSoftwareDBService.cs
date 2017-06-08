using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface IOldVersionSoftwareDBService
    {

        int  Version { get; set; }
        /// <summary>
        /// 取得控制器名称及类型 
        /// 返回格式：控制器类型;文件版本
        /// </summary>
        /// <param name="strPath">文件地址</param>
        /// <returns>控制器类型;文件版本</returns>
        string[] GetFileVersionAndControllerType();
        int DeviceAddressLength { get; }
        /// <summary>
        /// 取得回路信息
        /// </summary>
        /// <returns> </returns>
        List<LoopModel> GetLoopsInfo();
        /// <summary>
        /// 取得相应回路的器件信息
        /// </summary>
        /// <typeparam name="T">器件实体类</typeparam>
        /// <param name="loop">指定的回路对象,并为此回路增加相关器件</param>
        /// <param name="dictDeviceMappingManualControlBoard">器件信息与手动盘的对应字典 Key为string器件编码,Value为int手动盘ID</param>
        /// <returns></returns>
        bool GetDevicesInLoop(ref LoopModel loop, Dictionary<string, string> dictDeviceMappingManualControlBoard);        

        List<LinkageConfigStandard> GetStandardLinkageConfig();

        List<LinkageConfigMixed> GetMixedLinkageConfig();
        List<LinkageConfigGeneral> GetGeneralLinkageConfig();
        List<ManualControlBoard> GetManualControlBoard();

    }
}
