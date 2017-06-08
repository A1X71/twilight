using System.Windows.Controls;

/* ==============================
*
* Author     : William
* Create Date: 2017/4/15 16:17:24
* FileName   : DeviceBuildNoRule
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Validation
{
    public class SensitiveRule:ValidationRule
    {
        private int min = 0;
        private int max = 3;
        public int Min
        {
            get { return min; }
            set { min = value; }
        }
        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int setValue = 0;

            try
            {
                setValue = int.Parse((string)value,System.Globalization.NumberStyles.Any,cultureInfo);
            }
            catch
            {
                return new ValidationResult(false, "Illegal characters.");
            }
            if ((setValue < Min) || (setValue > Max))
            {
                return new ValidationResult(false, "Not in the range");
            }
            else
            {
                return new ValidationResult(true, null);
            }
            
        }
    }
}
