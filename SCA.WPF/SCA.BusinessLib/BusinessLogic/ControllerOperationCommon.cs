using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCA.Interface;
using System.Data;
using SCA.Interface.DatabaseAccess;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/7 16:16:31
* FileName   : ControllerOperationCommon
* Description:
* Version：V1
* ===============================
*/
namespace SCA.BusinessLib.BusinessLogic
{
    public class ControllerOperationCommon:ControllerOperationBase,IControllerOperation
    {

             
        public Model.ControllerNodeModel[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public Model.ControllerNodeType GetControllerNodeType()
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> GetLoops(int controllerID)
        {
            throw new NotImplementedException();
        }

        public List<Model.LoopModel> CreateLoops(int amount)
        {
            throw new NotImplementedException();
        }

        public List<Model.IDevice> OrganizeDeviceInfoForSettingController()
        {
            throw new NotImplementedException();
        }

        public List<Model.IDevice> GetDevicesInfo(int loopID)
        {
            throw new NotImplementedException();
        }

        public List<Model.LinkageConfigStandard> OrganizeLikageStandardInfoForSettingController()
        {
            throw new NotImplementedException();
        }

        public Model.ColumnConfigInfo[] GetDeviceColumns()
        {
            throw new NotImplementedException();
        }

        public bool UpdateLowerVersionDataToCurrentVersionData()
        {
            throw new NotImplementedException();
        }

        
        public Model.ControllerType GetControllerType()
        {
            throw new NotImplementedException();
        }


        public bool CreateController(Model.ControllerModel model)
        {
            throw new NotImplementedException();
        }


        public List<Model.LoopModel> CreateLoops(int loopsAmount, int deviceAmount, Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }


        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(int sourceVersion, int destVersion)
        {
            throw new NotImplementedException();
        }


        public Model.ControllerModel OrganizeControllerInfoFromOldVersionSoftwareDataFile(IOldVersionSoftwareDBService oldDBService)
        {
            throw new NotImplementedException();
        }


        public bool AddControllerToProject(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }


        public bool DeleteControllerBySpecifiedControllerID(int controllerID)
        {
            throw new NotImplementedException();
        }


        public int GetMaxDeviceID()
        {
            throw new NotImplementedException();
        }

        protected override List<short?> GetAllBuildingNoWithController(Model.ControllerModel controller)
        {
            return null;
        }

        protected override List<Model.DeviceType> GetConfiguredDeviceTypeWithController(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }





        public List<Model.DeviceInfoForSimulator> GetSimulatorDevices(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }


        public List<Model.DeviceInfoForSimulator> GetSimulatorDevicesByDeviceCode(List<string> lstDeviceCode,Model.ControllerModel controller, List<Model.DeviceInfoForSimulator> lstAllDevicesOfOtherMachine)
        {
            throw new NotImplementedException();
        }


        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo)
        {
            throw new NotImplementedException();
        }


        public bool DownloadDefaultEXCELTemplate(string strFilePath, IFileService fileService, Model.BusinessModel.ExcelTemplateCustomizedInfo customizedInfo, Model.ControllerType controllerType)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateLoopSheet(List<string> sheetNames, IControllerConfig config, Model.BusinessModel.ExcelTemplateCustomizedInfo summaryInfo, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateStandardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateMixedSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateGeneralSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxLinkageAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool GenerateExcelTemplateManualControlBoardSheet(List<string> sheetNames, int currentIndex, int loopSheetAmount, int maxAmount, ref IExcelService excelService)
        {
            throw new NotImplementedException();
        }

        protected override bool ExistSameDeviceCode(Model.LoopModel loop)
        {
            throw new NotImplementedException();
        }

        protected override List<Model.LoopModel> ConvertToLoopModelFromDataTable(DataTable dt, Model.ControllerModel controller, out string loopDetailErrorInfo)
        {
            throw new NotImplementedException();
        }

        protected override List<Model.LinkageConfigStandard> ConvertToStandardLinkageModelFromDataTable(DataTable dtStandard)
        {
            throw new NotImplementedException();
        }

        protected override List<Model.LinkageConfigMixed> ConvertToMixedLinkageModelFromDataTable(DataTable dtMixed)
        {
            throw new NotImplementedException();
        }

        protected override List<Model.LinkageConfigGeneral> ConvertToGeneralLinkageModelFromDataTable(DataTable dtGeneral)
        {
            throw new NotImplementedException();
        }

        protected override List<Model.ManualControlBoard> ConvertToManualControlBoardModelFromDataTable(DataTable dtManualControlBoard)
        {
            throw new NotImplementedException();
        }


        //public event Action<int> UpdateProgressBarEvent;

        //public event Action<Model.ControllerModel, string> ReadingExcelCompletedEvent;

        //public event Action<Model.ControllerModel, string> ReadingExcelCancelationEvent;

        //public event Action<string> ReadingExcelErrorEvent;

        public void SetStaticProgressBarCancelFlag(bool flag)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, int> GetAmountOfDifferentDeviceType(Model.ControllerModel controller)
        {
            throw new NotImplementedException();
        }
    }
}
