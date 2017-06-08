using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/10 13:51:45
* FileName   : ConfirmInvokeCommandAction
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Infrastructure
{
    public class ConfirmInvokeCommandAction:TriggerAction<DependencyObject>
    {

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ConfirmInvokeCommandAction), new PropertyMetadata("Are you sure?"));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var button = this.AssociatedObject as Button;
            if (button != null)
            {
                if (MessageBox.Show(this.Message, "Alert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //checkBox.IsChecked = false;
                    string str = "welcome";
                }
           }
        }
    }
}
