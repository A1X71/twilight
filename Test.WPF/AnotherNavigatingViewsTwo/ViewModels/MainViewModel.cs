using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq.Expressions;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/6 15:57:58
* FileName   : MainViewModel
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.AnotherNavigatingViewsTwo.ViewModels
{
    class MainViewModel:ObservableObject
    {
        private object _currentView;
        Dictionary<string, IVM> _dict;
        private string _name;
        public MainViewModel()
        { 
            _dict=new Dictionary<string,IVM>();
            ExampleVM vm1=new ExampleVM();
            ExampleVM2 vm2=new ExampleVM2();
            _dict.Add("vm1",vm1);
            _dict.Add("vm2",vm2);
            _name = "Initial";
            CurrentView = _dict["vm1"];            
        }
        public ICommand NavigateToStocksCommand
        {
            get { return new Test.WPF.Utility.RelayCommand(StockCommandExecute, null); }
            //get
            //{
            //    _name = "VM1";
            //    //object o=null;
            //    return new NavigateToViewCommand(_dict["vm1"]);

            //    //(Container.Container.GetA<IStockQuotesViewModel>());
            //}
        }
        public void PortfolioCommandExecute()
        {
            Name = "VM2";
            NavigateToView(_dict["vm2"]);
        }
        public void StockCommandExecute()
        {
            Name = "VM1";
            NavigateToView(_dict["vm1"]);
        }
        public ICommand NavigateToPortfolioCommand
        {
            get { return new Test.WPF.Utility.RelayCommand(PortfolioCommandExecute, null); }
            //get
            //{
            //    _name = "VM2";

            //    return new NavigateToViewCommand(_dict["vm2"]);
            //    // (Container.Container.GetA<IPortofolioViewModel>());
            //}
        }
        public object CurrentView
        {
            get { return _currentView; }
            private set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public void NavigateToView(object viewToNavigate)
        {
            CurrentView = viewToNavigate;
        }
    }
    public class NavigateToViewCommand : WpfCommand
    {
        private readonly object _viewToNavigate;
        public NavigateToViewCommand(object viewToNavigate)
            : base("Navigate")
        {
            _viewToNavigate = viewToNavigate;
        }
        protected override void RunCommand(object parameter)
        {
            MainViewModel m = new MainViewModel();
            m.NavigateToView(_viewToNavigate);
            //  Container.Container.GetA<IMainViewModel>().NavigateToView(_viewToNavigate);
        }
        protected override IEnumerable<string> GetPreconditions(object parameter)
        {
            yield break;
        }
    }
    public abstract class WpfCommand : ICommand
    {
        private readonly string _verb;
        protected WpfCommand(string verb)
        {
            _verb = verb;
        }
        public string Verb
        {
            get { return _verb; }
        }
        public void Execute(object parameter)
        {
            
            RunCommand(parameter);
        }
        protected abstract void RunCommand(object parameter);
        protected abstract IEnumerable<string> GetPreconditions(object parameter);
        public bool CanExecute(object parameter)
        {
            return true;
            //   return GetPreconditions(parameter).Count() < 1;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            //var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            //this.RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Warns the developer if this Object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        
        
        public void VerifyPropertyName(String propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this Object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                
            }
        }
    }
}
