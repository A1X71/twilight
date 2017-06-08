using System.Windows.Controls;
using System.Windows.Data;
using SCA.Model;
using SCA.WPF.ViewModelsRoot.ViewModels.DetailInfo;
using SCA.BusinessLib.BusinessLogic;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/* ==============================
*
* Author     : William
* Create Date: 2017/4/15 16:14:34
* FileName   : DeviceInfoRule
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Validation
{
    public class DeviceInfo8007Rule:ValidationRule
    {


        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            ControllerConfig8007 config = new ControllerConfig8007();
            Dictionary<string, SCA.Model.RuleAndErrorMessage> dictMessage = config.GetDeviceInfoRegularExpression(8);
            ValidationResult vr;
            if ((value as BindingGroup).Items.Count > 0)
            {
                EditableDeviceInfo8007 deviceInfo = (value as BindingGroup).Items[0] as EditableDeviceInfo8007;
                SCA.Model.RuleAndErrorMessage s = dictMessage["BuildingNo"];
                Regex exminator = new Regex(s.Rule);
                if (exminator.IsMatch(deviceInfo.BuildingNo.ToString()))
                {
                    vr= new ValidationResult(true, null);
                }
                else
                {
                    vr= new ValidationResult(false, s.ErrorMessage);
                }
            }
            else
            {
                vr= new  ValidationResult(true,null);
            }
            return vr;
            
            
        }
    }
}
