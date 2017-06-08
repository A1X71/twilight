using SCA.Model;
using SCA.BusinessLib.BusinessLogic;
using System.Collections.Generic;
using System.Windows.Input;
using System.Reflection;
using Caliburn.Micro;
using SCA.WPF.Utility;
using SCA.WPF.Infrastructure;
/* ==============================
*
* Author     : William
* Create Date: 2017/5/11 10:49:27
* FileName   : CreateManualControlBoardViewModel
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.CreateManualControlBoard
{
    public class CreateManualControlBoardViewModel : PropertyChangedBase
    {
        public ControllerModel TheController { get;  set; }

        private int _boardNo = 0;
        private int _subBoardStartNo = 1;
        private int _subBoardEndNo = 1;
        private int _keyNoAmount= 32;
        private bool _key64 = true;
        private bool _keyOthers = false;
        public int BoardNo { get { return _boardNo; } set { _boardNo = value; NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName()); } }
        public int SubBoardStartNo { get { return _subBoardStartNo; } set { _subBoardStartNo = value; NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName()); } }
        public int SubBoardEndNo { get { return _subBoardEndNo; } set { _subBoardEndNo = value; NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName()); } }

        public int KeyNoAmount { get { return _keyNoAmount; } set { _keyNoAmount = value; NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName()); } }

        public bool Key64 
        { 
            get { return _key64; }             
            set
            {
                _key64 = value;
                if (_key64)
                {
                    KeyOthers = false;
                }
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            } 
        
        }
        public bool KeyOthers
        {
            get { return _keyOthers; }
            set
            {
                _keyOthers = value;
                if (_keyOthers)
                {
                    Key64 = false;
                }
                NotifyOfPropertyChange(MethodBase.GetCurrentMethod().GetPropertyName());
            }

        }

        private List<int> _boardNumberList;
        private List<int> _subBoardList;
        public List<int> BoardNumberList
        {
            get
            {
                _boardNumberList = new List<int>();
                int maxBoardNumber = 0;
                if (TheController != null)
                {
                    maxBoardNumber = ControllerConfigManager.GetConfigObject(TheController.Type).GetMaxAmountForBoardNoInManualControlBoardConfig();
                }
                for (int i = 0; i < maxBoardNumber; i++)
                {
                    _boardNumberList.Add(i);
                }
                return _boardNumberList;        
            }
            
        }

        public List<int> SubBoardList
        {
            get
            {
                _subBoardList = new List<int>();
                int maxSubBoardNumber = 0;
                if (TheController != null)
                {
                    maxSubBoardNumber = ControllerConfigManager.GetConfigObject(TheController.Type).GetMaxAmountForSubBoardNoInManualControlBoardConfig();   
                }
                for (int i = 1; i < maxSubBoardNumber; i++)
                {
                    _subBoardList.Add(i);
                }
                return _subBoardList;
            }
        }
 //       public static readonly RoutedEvent AddMoreLineClickEvent = EventManager.RegisterRoutedEvent(
 //     "AddMoreLineClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ManualControlBoardView)
 //);
 //       public event RoutedEventHandler AddMoreLineClick
 //       {
 //           add { AddHandler(AddMoreLineClickEvent, value); }
 //           remove { RemoveHandler(AddMoreLineClickEvent, value); }
 //       }
        public ICommand AddMoreLineCommand
        {
            get { return new SCA.WPF.Utility.RelayCommand(AddMoreLineExecute, null); }
        }
        public void AddMoreLineExecute()
        {
            
            object [] numbers = new object[5];
            numbers[0] = BoardNo;
            numbers[1] = SubBoardStartNo;
            numbers[2] = SubBoardEndNo;
            
            //EventMediator.Register("AddMoreLines", AddMoreLines);
            if (Key64)
            {
                numbers[3] = 64;
            }
            else
            {
                numbers[3] = KeyNoAmount;
            }
            EventMediator.NotifyColleagues("ManualControlBoardAddMoreLines", numbers);
            //SCA.WPF.Infrastructure.EventMediator.Register("", RefreshData);
            //List<SCA.Model.ProjectModel> lstProject = new List<SCA.Model.ProjectModel>();
            //lstProject.Add(SCA.BusinessLib.ProjectManager.GetInstance.Project);
          // NavigatingViewModel.UpdateControllerInfo((ControllerModel)((RoutedEventArgs)o).OriginalSource);
           // SetNavigatingViewModel(lstProject);
        }
        
    }
}
