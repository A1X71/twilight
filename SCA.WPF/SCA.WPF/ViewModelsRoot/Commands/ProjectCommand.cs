using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
/* ==============================
*
* Author     : William
* Create Date: 2017/2/28 11:51:28
* FileName   : ProjectCommand
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.ViewModelsRoot.Commands
{
    public class ProjectCommand
    {
        public static RoutedUICommand Add = new RoutedUICommand("Add a project", "Add", typeof(ProjectCommand));
    }
}
