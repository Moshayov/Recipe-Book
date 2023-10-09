using System;
using System.Windows.Input;

namespace MyRecipeBook
{
    internal class DelegateCommand : ICommand
    {
        private Predicate<object> _canExecuteMetod;
        private Action<object> _executeMetod;
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public DelegateCommand(Action<object> eM, Predicate<object> cEM = null)
        {
            _executeMetod = eM;
            _canExecuteMetod = cEM;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMetod == null || _canExecuteMetod(parameter);
        }

        public void Execute(object parameter)
        {
            _executeMetod(parameter);
        }
    }


}
