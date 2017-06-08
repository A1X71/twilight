using System.Collections.Generic;
using SCA.Model;
namespace SCA.Interface.DatabaseAccess
{
    public interface ILinkageConfigGeneralDBService
    {
        LinkageConfigGeneral GetGeneralLinkageConfigInfo(int id);
        List<LinkageConfigGeneral> GetGeneralLinkageConfigInfo(ControllerModel controller);
        LinkageConfigGeneral GetGeneralLinkageConfigInfo(LinkageConfigGeneral linkageConfigGeneral);
        bool AddGeneralLinkageConfigInfo(LinkageConfigGeneral linkageConfigGeneral);
        bool AddGeneralLinkageConfigInfo(List<LinkageConfigGeneral> lstLinkageConfigGeneral);

        int UpdateGeneralLinkageConfigInfo(LinkageConfigGeneral lstLinkageConfigGeneral);
        bool DeleteGeneralLinkageConfigInfo(int id);
    }
}
