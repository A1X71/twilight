using System.Windows;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/28 16:23:44
* FileName   : BindingProxy
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Infrastructure
{
    public class BindingProxy:Freezable
    {
        #region  Overrides of Freezable
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
        #endregion
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        
    }
}
