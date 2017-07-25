using System.Collections.Generic;
using SCA.Model;

namespace SCA.Interface.DatabaseAccess
{
    public interface ILinkageConfigMixedDBService
    {
        LinkageConfigMixed GetMixedLinkageConfigInfo(int id);
        List<LinkageConfigMixed> GetMixedLinkageConfigInfo(ControllerModel controller);
        
        LinkageConfigMixed GetMixedLinkageConfigInfo(LinkageConfigMixed linkageConfigMixed);
        bool AddMixedLinkageConfigInfo(LinkageConfigMixed linkageConfigMixed);
        bool AddMixedLinkageConfigInfo(List<LinkageConfigMixed> lstLinkageConfigMixed);

        int UpdateMixedLinkageConfigInfo(LinkageConfigMixed lstLinkageConfigMixed);
        bool DeleteMixedLinkageConfigInfo(int id);
    }
}
