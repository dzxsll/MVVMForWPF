using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.Command
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Predicate<object> _canExcute;

        public event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<object> execute) : this(execute, DefalutCanExecute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExcute)
        {
            if (_execute != null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExcute = canExcute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExcute != null && _canExcute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefalutCanExecute(object parameter)
        {
            return true;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;

        private Predicate<T> _canExcute;

        public RelayCommand(Action<T> execute) : this(execute, DefalutCanExecute)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExcute)
        {
            if (_execute != null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExcute = canExcute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExcute != null && _canExcute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        private static bool DefalutCanExecute(T parameter)
        {
            return true;
        }
    }
}