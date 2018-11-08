using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Input;

namespace NicochViewerUWP.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        protected bool SetProperty<T>(ref T backingStore, T value,
            [System.Runtime.CompilerServices.CallerMemberName]string propertyName = "",
            System.Action onChanged = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged() { CanExecuteChanged?.Invoke(this, new EventArgs()); }

        private Func<object, bool> CanExecuteFunc;
        private Action<object> ExecuteFunc;

        public DelegateCommand(Action<object> executeFunc, Func<object, bool> canExecuteFunc = null)
        {
            ExecuteFunc = executeFunc ?? throw new ArgumentNullException(nameof(executeFunc));
            CanExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            ExecuteFunc?.Invoke(parameter);
        }
    }
}
