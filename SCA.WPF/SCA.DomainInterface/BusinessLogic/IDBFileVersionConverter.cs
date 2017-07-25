using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Model;
namespace SCA.Interface.BusinessLogic
{
    public interface IDBFileVersionConverter
    {
        int DBFileSourceVersion { get; }
        int DBFileDestinationVersion { get; }

        ProjectModel UpgradeToDestinationVersion(ProjectModel project);
        void DowngradeToSourceVersion();
    }
}
