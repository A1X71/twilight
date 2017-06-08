using System;
using System.Windows;
using System.Globalization;
using System.Windows.Interactivity;
using System.Windows.Input;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/13 10:36:47
* FileName   : WelcomeViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.ViewModels
{
    public class WelcomeViewModel:UIElement
    {
  
        public WelcomeViewModel()
        { 
        
        }

      
        //public ICommand NewCommand
        //{
        //    get { return new SCA.WPF.Utility.RelayCommand<object>(NewButton_Click, null); }
        //}
        //private class NewCommand : ICommand
        //{ 

        //}

        /// <summary>
        /// 显示应用程序版本
        /// </summary>
        private void DisplayVersion()
        {
            string strVersionInfo="";
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            strVersionInfo += string.Format(CultureInfo.CurrentCulture,"{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
    }
    public class WelcomeViewBehavior : Behavior<UIElement>
    {

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseUp+=AssociatedObject_MouseUp;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseUp -= AssociatedObject_MouseUp;
        }
        private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
        { 
            
        }
    }
}
