using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Linq.Expressions;
/* ==============================
*
* Author     : William
* Create Date: 2017/3/3 14:32:54
* FileName   : GodClass
* Description:
* Version：V1
* ===============================
*/
namespace Test.WPF.NavigatingViews.ViewModel
{
    public static class PropertySupport
    {
        public static String ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (propertyExpresssion == null)
            {
                throw new ArgumentNullException("propertyExpresssion");
            }

            var memberExpression = propertyExpresssion.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpresssion");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpresssion");
            }

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property.", "propertyExpresssion");
            }

            return memberExpression.Member.Name;
        }
    }
    public interface IViewModelBase : INotifyPropertyChanged
    { 
    
    }
    public interface IPortofolioViewModel : IViewModelBase
    {
        string Name { get; }
        IEnumerable<string> Portfolios { get; }
    }
    public interface IStockQuotesViewModel : IViewModelBase
    {
        string Name { get; }
    }
    public interface IMainViewModel : IViewModelBase
    {
        object CurrentView { get; }
        ICommand NavigateToStocksCommand { get; }
        ICommand NavigateToPortfolioCommand { get; }
        void NavigateToView(object viewToNavigate);

    }
    public class ViewModelBase : IViewModelBase
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
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            this.RaisePropertyChanged(propertyName);
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
                //Debug.Fail("Invalid property name: " + propertyName);
            }
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
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private object _currentView;
        private string _name="MainViewModelFromCodeBehind"; //For testing
        
        IViewModelBase _viewModel = null;
        private Dictionary<string,IViewModelBase> _dictControls;
        
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public MainViewModel()
        { 
            _viewModel=new ViewModel.PortofolioViewModel();

            ViewModel.PortofolioViewModel pVM = new PortofolioViewModel();
            ViewModel.StockQuotesViewModel sVM = new StockQuotesViewModel();

            _dictControls = new Dictionary<string, IViewModelBase>();
            _dictControls.Add("portfolio", pVM);
            _dictControls.Add("stock", sVM);
      //      View.PortfolioView pView = new View.PortfolioView();
            //CurrentView = pView;
            CurrentView =_dictControls["stock"];
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
        public ICommand NavigateToStocksCommand
        {
            get
            {
                //object o=null;
                return new NavigateToViewCommand(_dictControls["stock"]);
                
                    //(Container.Container.GetA<IStockQuotesViewModel>());
            }
        }
        public ICommand NavigateToPortfolioCommand
        {
            get
            {
                return new NavigateToViewCommand(_dictControls["portfolio"]);
                   // (Container.Container.GetA<IPortofolioViewModel>());
            }
        }
        public void NavigateToView(object viewToNavigate)
        {
            CurrentView = viewToNavigate;
        }

    }


}
