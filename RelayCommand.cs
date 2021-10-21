using System;
using System.Windows.Input;

namespace MP3Tagger
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private event Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute is not null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute is not null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }

        }


        public RelayCommand(Action<object> action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
}
