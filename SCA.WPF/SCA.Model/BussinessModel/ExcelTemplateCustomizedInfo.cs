using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ==============================
*
* Author     : William
* Create Date: 2017/6/6 13:56:49
* FileName   : CustomExcelTemplate
* Description:
* Version：V1
* ===============================
*/
namespace SCA.Model.BussinessModel
{
    public class ExcelTemplateCustomizedInfo
    {
        public ControllerType ControllerType { get; set; }
        public string ControllerName { get; set; }
        public int MachineNumber { get; set; }
        public int SelectedDeviceCodeLength { get; set; }
        public string SerialPortNumber { get; set; }
        public int LoopAmount { get; set; }
        public int LoopGroupAmount { get; set; }
        public bool StandardLinkageFlag { get; set; }
        public bool MixedLinkageFlag { get;set; }
        public bool GeneralLinkageFlag{get;set;}
        public bool ManualControlBoardFlag{get;set;}
    }
}
