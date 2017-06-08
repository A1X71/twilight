
using System.Collections.ObjectModel;
using System.Reflection;
using Caliburn.Micro;
using SCA.Model;
using SCA.WPF.Utility;
using SCA.BusinessLib;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/17 11:42:20
* FileName   : EditLoopsViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.EditLoop
{
   public class EditLoopsViewModel:PropertyChangedBase
    {
       private ObservableCollection<LoopModel> _loopsObservableCollection;
       /// <summary>
       /// 获取当前控制器的所有回路信息
       /// </summary>
       public ObservableCollection<LoopModel> LoopObservableCollection
       {
           get
           {
               ControllerModel controller = ProjectManager.GetInstance.GetPrimaryController();
               return new ObservableCollection<LoopModel>(controller.Loops);
           }
           set
           {
               _loopsObservableCollection = value;
               NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
           }
       }
    }
}
